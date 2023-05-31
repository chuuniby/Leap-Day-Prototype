using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class MovementScript : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public GameObject cam;
    public Animator animator;

    public float movementSpeed;
    public float movementUp;
    public float jumpForce;
    public float fallMultiplier;
    public float wallSlidingSpeed;
    public float wallJumpingDir;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;
    public float offset = 4.5f;

    public Vector2 movementDirection;
    public Vector2 previousPosition;
    public Vector3 rawMovement;
    public Vector2 wallJumpingPower;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Transform wallCheck;
    public LayerMask wallLayer;

    public bool movingRight;
    public bool doubleJump;
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;
    public bool isWallJumpDoubleJump;
    public bool touchWall;

    public Vector2 gravity;

    public int jumpCount;
    public int wallJumpCount;
    private void Start()
    {
        gravity = new Vector2(0f, -Physics2D.gravity.y);
        movementSpeed = 7f;
        movementUp = 0f;
        jumpForce = 1400f;
        fallMultiplier = 2.5f;
        movingRight = true;
        wallSlidingSpeed = 2f;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
        wallJumpingPower = new Vector2(0f, 25f);
    }


    private void FixedUpdate()
    {
        if (!isWallSliding)
        {

        rawMovement = new Vector3(movementSpeed, 0f);
        transform.position += rawMovement * Time.deltaTime;
        }

        cam.transform.position = Vector3.Lerp(new Vector3(cam.transform.position.x, cam.transform.position.y, cam.transform.position.z),
            new Vector3(cam.transform.position.x, transform.position.y + offset, transform.position.z - 18f), 2f * Time.deltaTime);
    }

#if UNITY_STANDALONE_WIN
    private void Update()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (!isGrounded)
        {
            isWallSliding = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
        }
        else
        {
            touchWall = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
            isWallSliding = false;
        }

        if (touchWall)
        {
            movementSpeed = -movementSpeed;
            movingRight = !movingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
            touchWall = false;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                animator.SetBool("AnimationJumping", true);
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                rb2d.AddForce(Vector2.up * jumpForce);
                jumpCount++;
            }
            //else
            //{
            //    if (doubleJump && !isWallSliding)
            //    {
            //        //animator.SetBool("AnimationJumping", true);
            //        rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            //        rb2d.AddForce(Vector2.up * jumpForce);
            //        jumpCount++;
            //    }
            //}
        }

        //if(isGrounded && isWallSliding)
        //{
        //    movingRight = !movingRight;
        //    isWallSliding = false;
        //}

        if (isGrounded)
        {
            jumpCount = 0;
            wallJumpCount = 0;
            doubleJump = true;
        }

        //if (jumpCount == 2)
        //{
        //    doubleJump = false;
        //}

        if (rb2d.velocity.y < 0f)
        {
            rb2d.velocity -= fallMultiplier * Time.deltaTime * gravity;
        }

        if (!isGrounded && isWallSliding)
        {
            movementSpeed = 0f;
            rb2d.velocity = new Vector2(movementSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            if (movingRight)
            {
                movementSpeed = 7f;
                animator.SetBool("AnimationMovingRight", true);
            }
            if (!movingRight)
            {
                movementSpeed = -7f;
                animator.SetBool("AnimationMovingRight", false);
            }
        }

        WallJump();

    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (isGrounded && collision.transform.CompareTag("Wall"))
    //    {
    //        movementSpeed = -movementSpeed;
    //        movingRight = !movingRight;
    //        Vector3 localscale = transform.localScale;
    //        localscale.x *= -1f;
    //        transform.localScale = localscale;
    //    }
    //}

    public void WallJump()
    {
        if (isWallSliding)
        {
            animator.SetBool("AnimationWallSliding", true);
            wallJumpingDir = -transform.localScale.x;
            isWallJumping = false;
            wallJumpingCounter = wallJumpingTime;
            wallJumpCount = 0;
            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            animator.SetBool("AnimationWallSliding", false);
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            animator.SetBool("AnimationWallJumping", true);
            isWallJumping = true;
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDir)
            {
                movingRight = !movingRight;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
            Invoke(nameof(StopWallJump), wallJumpingDuration);
        }

        //if (isWallJumping && Input.GetKeyDown(KeyCode.Space))
        //{
        //    //animator.SetBool("AnimationJumping", true);
        //    rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        //    rb2d.AddForce(Vector2.up * jumpForce);
        //    wallJumpCount++;
        //}
    }

    public void StopWallJump()
    {
        isWallJumping = false;
        animator.SetBool("AnimationWallJumping", false);
    }

    public void StopJumpingAnimation()
    {
        animator.SetBool("AnimationJumping", false);
    }

    public void LandingAnimation()
    {
        animator.SetBool("AnimationLanding", true);
    }

    public void GrapplingJump()
    {
        animator.SetBool("AnimationJumping", true);
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        rb2d.AddForce(new Vector2(movementSpeed*5,jumpForce/10));
    }
}

#endif

#if UNITY_ANDROID
    public Touch theTouch;
    public float timeTouchEnded;
    public float displayTime = 0.5f;
    public Vector2 touchStartPosition;
    public Vector2 touchEndPosition;
    public string direction;
    public float x;
    public float y;
    public TextMeshProUGUI text;
    private void Update()
    {
        //text.text = direction;
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            }
            else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;
            }
            if (theTouch.phase == TouchPhase.Ended)
            {
                timeTouchEnded = Time.time;
            }
            x = touchEndPosition.x -= touchStartPosition.x;
            y = touchEndPosition.y - touchStartPosition.y;

            if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
            {
                direction = "tapped";
            }
            else if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x > 0)
                {
                    direction = "Right";
                }
                else
                {
                    direction = "Left";
                }
            }
            else
            {
                if (y > 0)
                {
                    direction = "Up";
                }
                else
                {
                    direction = "Down";
                }
            }
        }
        else if (Time.time - timeTouchEnded > displayTime)
        {
            //do smth
        }
        else
        {
            direction = "nothing happened";
        }

        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        isWallSliding = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
        if (Input.touchCount > 0)
        {
            if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
            {
                if (isGrounded)
                {
                    rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                    rb2d.AddForce(Vector2.up * jumpForce);
                    jumpCount++;
                }
                else
                {
                    if (doubleJump)
                    {
                        doubleJump = false;
                        rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                        rb2d.AddForce(Vector2.up * jumpForce);
                    }
                }
            }

            if (isGrounded)
            {
                jumpCount = 0;
                wallJumpCount = 0;
                doubleJump = true;
            }

            if (jumpCount == 2)
            {
                doubleJump = false;
            }

            if (rb2d.velocity.y < 0f)
            {
                rb2d.velocity -= fallMultiplier * Time.deltaTime * gravity;
            }

            if (!isGrounded && isWallSliding)
            {
                movementSpeed = 0f;
                rb2d.velocity = new Vector2(movementSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }
            else
            {
                if (movingRight)
                {
                    movementSpeed = 7f;
                }
                if (!movingRight)
                {
                    movementSpeed = -7f;
                }
            }

            wallJump();

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            movementSpeed = -movementSpeed;
            movingRight = !movingRight;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }

    public void wallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJump));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if ((Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDir)
            {
                movingRight = !movingRight;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
            Invoke(nameof(StopWallJump), wallJumpingDuration);
        }

        if (isWallJumping && (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0))
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.AddForce(Vector2.up * jumpForce);
            wallJumpCount++;
        }
    }

    public void StopWallJump()
    {
        isWallJumping = false;
    }
}

#endif