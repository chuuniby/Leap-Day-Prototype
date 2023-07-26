using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

// IDK WHY BUT I LITERALLY DECIDED TO MAKE MOVEMENTSCRIPT PERMANENTLY ON FROM THE GRAPPLING SCRIPT. PLEASE GO AND FIX IT WHEN HAVE TIME.
public class MovementScript : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public GameObject cam;
    public Animator animator;

    public float movementSpeed;
    public float defaultMovementSpeed;
    public float movementUp;
    public float jumpForce;
    public float fallMultiplier;
    public float wallSlidingSpeed;
    public float wallJumpingDir;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.6f;
    public float offset = 4.5f;

    public Vector2 movementDirection;
    public Vector2 previousPosition;
    public Vector3 rawMovement;
    public Vector2 wallJumpingPower;

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Transform wallCheck;
    public LayerMask wallLayer;

    public bool doubleJump;
    public bool isGrounded;
    public bool touchWall;
    public bool isWallSliding;
    public bool isWallJumping;
    public bool wallStop;
    public bool movingRight;

    public Vector2 gravity;

    public int jumpCount;
    public int wallJumpCount;

    public TmpCollider tmpCollider;
    private void Start()
    {
        defaultMovementSpeed = movementSpeed;
        gravity = new Vector2(0f, -Physics2D.gravity.y);
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponent<Animator>();
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
        //Debug.Log(rb2d.velocity);
        if (TmpCollider.instance.isCollidedThisFrame)
        {
            rb2d.velocity = Vector2.zero;
            TmpCollider.instance.isCollidedThisFrame = false;
        }

        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (!isGrounded)
        {
            isWallSliding = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
            animator.SetBool("AnimationMovingRight", false);
            animator.SetBool("OnGround", false);
        }
        else
        {
            touchWall = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
            isWallSliding = false;
        }

        if(isGrounded && rb2d.velocity.y == 0)
        {
            animator.SetBool("OnGround", true);
        }
        else
        {
            animator.SetBool("OnGround", false);
        }

        if (isGrounded && rb2d.velocity.y <= 0)
        {
            animator.SetBool("AnimationWallJumping", false);
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

        if (jumpCount == 2)
        {
            doubleJump = false;
        }

        if (isGrounded)
        {
            jumpCount = 0;
            wallJumpCount = 0;
            doubleJump = true;
            TmpCollider.instance.tmpCol.enabled = true;
            StopWallJump();
            animator.SetBool("AnimationJumping", false);
            animator.SetBool("AnimationWallJumping",false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                jumpCount++;
                animator.SetBool("AnimationJumping", true);
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            else
            {
                if (doubleJump && !isWallSliding)
                {
                    //animator.SetBool("AnimationJumping", true);
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                    jumpCount++;
                }
            }
        }

        if (rb2d.velocity.y < 0f)
        {
            rb2d.velocity -= fallMultiplier * Time.deltaTime * gravity;
            animator.SetBool("AnimationWallJumping", false);
            animator.SetBool("LandingPhase", true);
        }
        else
        {
            animator.SetBool("LandingPhase", false);
        }

        if ((!isGrounded) && isWallSliding)
        {
            TmpCollider.instance.tmpCol.enabled = false;
            rb2d.velocity = new Vector2(movementSpeed, -wallSlidingSpeed);
        }
        else
        {
            if (movingRight)
            {
                movementSpeed = defaultMovementSpeed;
                animator.SetBool("AnimationMovingRight", true);
            }
            if (!movingRight)
            {
                movementSpeed = -defaultMovementSpeed;
                animator.SetBool("AnimationMovingRight", false);
            }
            TmpCollider.instance.tmpCol.enabled = true;
        }
        WallJump();
    }

    public void WallJump()
    {
        if (isWallSliding)
        {
            animator.SetBool("AnimationWallSliding", true);
            animator.SetBool("AnimationWallJumping", false);
            isWallJumping = false;
            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            wallJumpCount = 0;
            jumpCount = 0;
            doubleJump = false;
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
            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if (transform.localScale.x != wallJumpingDir)
            {
                movingRight = !movingRight;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
        }

        if (isWallJumping && Input.GetKeyDown(KeyCode.Space) && wallJumpCount < 2)
        {
            //animator.SetBool("AnimationJumping", true);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpCount++;
        }
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
    public void GrapplingJump()
    {
        animator.SetBool("AnimationJumping", true);
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.CompareTag("MovingBlock"))
    //    {
    //        transform.parent = collision.transform;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.transform.CompareTag("MovingBlock"))
    //    {
    //        transform.parent = null;
    //    }
    //}
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
        if ((Input.GetMouseButtonDown(0))){
            if (isGrounded)
            {
                jumpCount++;
                animator.SetBool("AnimationJumping", true);
                rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            }
            else
            {
                if (doubleJump && !isWallSliding)
                {
                    //animator.SetBool("AnimationJumping", true);
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                    jumpCount++;
                }
            }
        }
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
                direction = "Tapped";
                

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
        if (TmpCollider.instance.isCollidedThisFrame)
        {
            rb2d.velocity = Vector2.zero;
            TmpCollider.instance.isCollidedThisFrame = false;
        }

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

        if (jumpCount == 2)
        {
            doubleJump = false;
        }

        if (isGrounded)
        {
            jumpCount = 0;
            wallJumpCount = 0;
            doubleJump = true;
            TmpCollider.instance.tmpCol.enabled = true;
            Invoke(nameof(StopWallJump), 0f);
        }

        if (rb2d.velocity.y < 0f)
        {
            rb2d.velocity -= fallMultiplier * Time.deltaTime * gravity;
        }

        if ((!isGrounded) && isWallSliding)
        {
            TmpCollider.instance.tmpCol.enabled = false;
            rb2d.velocity = new Vector2(movementSpeed, -wallSlidingSpeed);
        }
        else
        {
            if (movingRight)
            {
                movementSpeed = defaultMovementSpeed;
                animator.SetBool("AnimationMovingRight", true);
            }
            if (!movingRight)
            {
                movementSpeed = -defaultMovementSpeed;
                animator.SetBool("AnimationMovingRight", false);
            }
            TmpCollider.instance.tmpCol.enabled = true;
        }
        WallJump();

    }
    public void WallJump()
    {
        if (isWallSliding)
        {
            animator.SetBool("AnimationWallSliding", true);
            isWallJumping = false;
            wallJumpingDir = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;
            wallJumpCount = 0;
            jumpCount = 0;
            doubleJump = false;
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
            rb2d.velocity = Vector2.zero;
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if (transform.localScale.x != wallJumpingDir)
            {
                movingRight = !movingRight;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
        }

        if (isWallJumping && Input.GetKeyDown(KeyCode.Space) && wallJumpCount < 2)
        {
            //animator.SetBool("AnimationJumping", true);
            rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpCount++;
        }
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
        rb2d.AddForce(new Vector2(movementSpeed * 5, jumpForce / 10));
    }
}

#endif