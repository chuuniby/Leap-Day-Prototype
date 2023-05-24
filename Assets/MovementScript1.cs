using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;
using Mono.Cecil.Cil;

public class MovementScript1 : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public GameObject cam;

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
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Transform wallCheck;
    public LayerMask wallLayer;

    public bool movingRight;
    public bool isJumping;
    public bool doubleJump;
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;
    public bool isWallJumpDoubleJump;

    public Vector2 gravity;

    public int jumpCount;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
    private void Start()
    {
        gravity = new Vector2(0f, -Physics2D.gravity.y);
        movementSpeed = 7f;
        movementUp = 0f;
        jumpForce = 1250f;
        fallMultiplier = 2.5f;
        movingRight = true;
        wallSlidingSpeed = 2f;
    }
#if UNITY_STANDALONE_WIN
    private void Update()
    {
        MovementCalculation();
        GroundCheck();
        WallCheck();
        CameraLerp();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                IsJumping();
            }
            else
            {
                if(doubleJump)
                {
                    IsJumping();
                }
                if(isWallSliding)
                {
                    WallJump();
                }
            }
        }

        if (isGrounded)                                                 //Whenever the player is on the ground
        {
            jumpCount = 0;                                              //reset double jump
            doubleJump = true;
            if (movingRight)                                            //if player is moving right
            {
                movementSpeed = 7f;                                     //set speed to moving right
            }
            if (!movingRight)                                           //if player is movinng left
            {
                movementSpeed = -7f;                                    //set speed to moving left
            }
        }

        if (jumpCount == 2)                                             //if player already jump twice
        {
            doubleJump = false;                                         //disable double jump
        }

        if (rb2d.velocity.y < 0f)                                       //if player is falling
        {
            CustomGravity();
        }

        if (!isGrounded && isWallSliding)                               //if you are not on the ground and you are wall sliding
        {
            IsWallSliding();
        }                                                               //wall slide
        WallJump();                                                     //Wall Jump method

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))                     //if player hit the wall
        {
            movementSpeed = -movementSpeed;                             //move in the reverse direction
            movingRight = !movingRight;                                 //tell the system that you are moving in the reverse direction
            Vector3 localscale = transform.localScale;                  
            localscale.x *= -1f;                                        
            transform.localScale = localscale;                          //change the sprite to face the opposite direction
        }
    }

    public void WallJump()
    {
        if (isWallSliding)                                              //if player is wall sliding
        {
            isWallJumping = false;                                      //player is not wall jumping
            wallJumpingDir = -transform.localScale.x;                   //jump in the opposite direction of where the sprite is facing
            wallJumpingCounter = wallJumpingTime;                       //???
            jumpCount = 0;
            CancelInvoke(nameof(StopWallJump));                         //Let player wall jump
        }
        else                                                            //if player is not wall sliding
        {
            wallJumpingCounter -= Time.deltaTime;                       //???
        }

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f) //if player press space and ???
        {
            isWallJumping = true;                                       //start wall jumping
            rb2d.velocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);   //wall jump by a fixed amount
            wallJumpingCounter = 0f;                                    //???

            if (transform.localScale.x != wallJumpingDir)               //if player is not facing the same direction as wall jumping direction
            {
                movingRight = !movingRight;                             //move the same way as jumping direction
                Vector3 localscale = transform.localScale;              
                localscale.x *= -1f;
                transform.localScale = localscale;                      //face the same direction as wall jumping direction
            }
            Invoke(nameof(StopWallJump), wallJumpingDuration);          //stop wall jump
        }

        if (isWallJumping && Input.GetKeyDown(KeyCode.Space))           //if you are wall jumping and you press space
        {
            movementUp = 0f;                                            //set vertical velocity to 0
            rb2d.AddForce(Vector2.up * jumpForce);                      //jump
            jumpCount++;
        }
    }

    public void StopWallJump()
    {
        isWallJumping = false;                                          //player is not wall jumping
    }

    public void MovementCalculation()
    {
        rawMovement = new Vector3(movementSpeed, movementUp);
        transform.position += rawMovement * Time.deltaTime;
    }

    public void GroundCheck()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    public void WallCheck()
    {
        isWallSliding = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
    }

    public void CameraLerp()
    {
        cam.transform.position = Vector3.Lerp(new Vector3(cam.transform.position.x, cam.transform.position.y, -18f),
            new Vector3(cam.transform.position.x, transform.position.y + offset, -18f), 2f * Time.deltaTime);
    }

    public void IsJumping()
    {
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
        rb2d.AddForce(Vector2.up * jumpForce);
        jumpCount++;
    }

    public void IsWallSliding()
    {
        movementSpeed = 0f;
        rb2d.velocity = new Vector2(movementSpeed, Mathf.Clamp(rb2d.velocity.y, -wallSlidingSpeed, float.MaxValue));
    }

    public void CustomGravity()
    {
        rb2d.velocity -= fallMultiplier * Time.deltaTime * gravity;
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