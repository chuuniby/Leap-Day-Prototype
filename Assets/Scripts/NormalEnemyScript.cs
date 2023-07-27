using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NormalEnemyScript : MonoBehaviour
{

    //SINCE WE ARE SOMEHOW WALKING ON WALL SO WE NEED TO FIND ANOTHER WAY TO DETECT SURFACE TO MOVE ON LIKE RAYCAST OR SOMETHING
    //PLEASE REMEMBER TO READ THIS
    public float movementSpeed;
    public float normalMovementSpeed;
    public float movementUp;
    public Rigidbody2D rb;
    public BoxCollider2D col;
    public bool isNotAtCliff;
    public bool isGrounded;
    public bool nearWall;
    public bool died;
    public bool freeze;
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
    public int dir;

    public GameObject enemyWall;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dir = 1;
    }
    void Update()
    {
        StartCoroutine(Freeze());

        rb.velocity = new Vector2(movementSpeed, movementUp);
        isNotAtCliff = Physics2D.OverlapBox(cliffCheck.position, new Vector2(2f, 2f), 0f, groundLayer);
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 2f, groundLayer);
        nearWall = Physics2D.Raycast(transform.position, Vector2.right * transform.localScale, 2f, wallLayer);

        if (died)
        {
            col.enabled = false;
            movementSpeed = 0;
            rb.gravityScale = 0;
            animator.enabled = false;
            StartCoroutine(PlayParticleSystem());
            died = false;
        }

        if (!isNotAtCliff && isGrounded)
        {
            dir = -dir;
            movementSpeed = -movementSpeed;
            Vector3 localscale = transform.localScale;
            localscale.x *= -1f;
            transform.localScale = localscale;
        }
        if (!freeze)
        {
            if (nearWall)
            {
                dir = -dir;
                movementSpeed = -movementSpeed;
                Vector3 localscale = transform.localScale;
                localscale.x *= -1f;
                transform.localScale = localscale;
            }
        }
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

    IEnumerator Freeze()
    {
        //if (rb.bodyType == RigidbodyType2D.Dynamic)  //normal movement
        //{
        if (!freeze)
        {
            movementSpeed = normalMovementSpeed * dir;
            rb.velocity = new Vector2(movementSpeed, movementUp);
            animator.enabled = true;
            enemyWall.SetActive(false);
        }

        //if (rb.bodyType == RigidbodyType2D.Static)     //freezing chip
        //{
        if (freeze)
        {
            movementSpeed = 0f;
            animator.enabled = false;
            enemyWall.SetActive(true);
            //}
        }
        yield return null;
    }
}
