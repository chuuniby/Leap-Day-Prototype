using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemyScript : MonoBehaviour
{

    //SINCE WE ARE SOMEHOW WALKING ON WALL ALSO SO WE NEED TO FIND ANOTHER WAY TO DETECT SURFACE TO MOVE ON LIKE RAYCAST OR SOMETHING
    //PLEASE REMEMBER TO READ THIS

    public float movementSpeed;
    public float movementUp;
    public Rigidbody2D rb;
    public bool isNotAtCliff;
    public bool isGrounded;
    public Transform cliffCheck;
    public LayerMask groundLayer;
    public Transform groundCheck;

    public int hp;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.velocity = new Vector2(movementSpeed, movementUp);
            isNotAtCliff = Physics2D.OverlapBox(cliffCheck.position, new Vector2(0.5f, 0.5f), 0f, groundLayer);
            isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

            if (!isNotAtCliff && isGrounded)
            {
                movementSpeed = -movementSpeed;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
        }
        //if(rb.bodyType == RigidbodyType2D.Static)     //freezing chip
        //{

        //}
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            movementSpeed = -movementSpeed;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
    }
}
