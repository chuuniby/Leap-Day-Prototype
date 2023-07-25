using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemyScript : MonoBehaviour
{

    //SINCE WE ARE SOMEHOW WALKING ON WALL SO WE NEED TO FIND ANOTHER WAY TO DETECT SURFACE TO MOVE ON LIKE RAYCAST OR SOMETHING
    //PLEASE REMEMBER TO READ THIS
    public float movementSpeed;
    public float movementUp;
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public bool isNotAtCliff;
    public bool isGrounded;
    public bool nearWall;
    public bool died;
    public LayerMask wallLayer;
    public Transform cliffCheck;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public Animator animator;
    public ParticleSystem explosionParticleSystem;
    public ParticleSystem nutParticleSystem;
    public ParticleSystem boltParticleSystem;
    public ParticleSystem smokeParticleSystem;

    public int hp;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if(died)
        {
            col.enabled = false;
            movementSpeed = 0;
            rb.gravityScale = 0;
            animator.enabled = false;
            StartCoroutine(PlayParticleSystem());
            died = false;
        }

        //if (rb.bodyType == RigidbodyType2D.Dynamic)
        //{
        rb.velocity = new Vector2(movementSpeed, movementUp);
        isNotAtCliff = Physics2D.OverlapBox(cliffCheck.position, new Vector2(2f, 2f), 0f, groundLayer);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);
        nearWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale, 2f, wallLayer);

        if (!isNotAtCliff && isGrounded)
        {
            movementSpeed = -movementSpeed;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }

        if (nearWall)
        {
            movementSpeed = -movementSpeed;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }

        //}
        //if(rb.bodyType == RigidbodyType2D.Static)     //freezing chip
        //{

        //}
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.transform.CompareTag("Wall"))
    //    {
    //        movementSpeed = -movementSpeed;
    //        Vector3 localscale = transform.localScale;
    //        localscale.x *= -1f;
    //        transform.localScale = localscale;
    //    }
    //}

    IEnumerator PlayParticleSystem()
    {
        explosionParticleSystem.Play();
        nutParticleSystem.Play();
        boltParticleSystem.Play();
        smokeParticleSystem.Play();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
