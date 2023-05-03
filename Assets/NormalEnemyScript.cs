using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemyScript : MonoBehaviour
{
    public float movementSpeed;
    public float movementUp;
    public Rigidbody2D rb;
    public bool isAtCliff;
    public bool isGrounded;
    public Transform cliffCheck;
    public LayerMask groundLayer;
    public Transform groundCheck;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        movementSpeed = 3f;
        movementUp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(movementSpeed, movementUp);
        isAtCliff = Physics2D.OverlapBox(cliffCheck.position, new Vector2(1.5f, 1.5f), 0f, groundLayer);
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.4f, 0.1f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (!isAtCliff && isGrounded)
        {
            movementSpeed = -movementSpeed;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
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
