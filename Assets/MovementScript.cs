using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public GameObject leftWall;
    public GameObject rightWall;
    public Camera cam;

    public float movementSpeed;
    public float movementUp;
    public float jumpForce;
    public float fallMultiplier;
    public float wallSlidingSpeed;
    public float wallJumpingDir;
    public float wallJumpingTime = 0.2f;
    public float wallJumpingCounter;
    public float wallJumpingDuration = 0.4f;


    public Vector2 movementDirection;
    public Vector2 previousPosition;
    public Vector3 rawMovement;
    public Vector2 wallJumpingPower = new Vector2(8f, 16f);

    public Transform groundCheck;
    public LayerMask groundLayer;

    public Transform wallCheck;
    public LayerMask wallLayer;

    public bool movingRight;
    public bool doubleJump;
    public bool isGrounded;
    public bool isWallSliding;
    public bool isWallJumping;

    public Vector2 gravity;

    public int jumpCount;

    private void Start()
    {
        gravity = new Vector2(0f, -Physics2D.gravity.y);
        movementSpeed = 4f;
        movementUp = 0f;
        jumpForce = 500f;
        fallMultiplier = 2.5f;
        movingRight = true;
        wallSlidingSpeed = 0.5f;
    }

    private void FixedUpdate()
    {
        rawMovement = new Vector3(movementSpeed, 0f);
        transform.position += rawMovement * Time.deltaTime;

        cam.gameObject.transform.position = Vector3.Lerp(new Vector3(cam.transform.position.x, cam.transform.position.y, -1f),
            new Vector3(cam.transform.position.x, transform.position.y, -1f), 2f);
    }
    private void Update()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        isWallSliding = Physics2D.OverlapCapsule(wallCheck.position, new Vector2(0.2f, 0.1f), CapsuleDirection2D.Vertical, 0, wallLayer);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
                jumpCount++;
                rb2d.AddForce(Vector2.up * jumpForce);
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
                movementSpeed = 4f;
            }
            if (!movingRight)
            {
                movementSpeed = -4f;
            }
        }

        wallJump();

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

        if (Input.GetKeyDown(KeyCode.Space) && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb2d.velocity = new Vector2 (wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
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
    }

    public void StopWallJump()
    {
        isWallJumping = false;
    }
}

