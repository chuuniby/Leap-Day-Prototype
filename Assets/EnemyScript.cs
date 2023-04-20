using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float movementSpeed;
    public float movementUp;

    public float tempSpeed;

    public Rigidbody2D rb;

    public bool isMoving;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        movementSpeed = 2f;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(movementSpeed, movementUp);

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            movementSpeed = -movementSpeed;
        }
    }
    public void StopMovement()
    {
        if(isMoving)
        {
            tempSpeed = movementSpeed;
            movementSpeed = 0f;
            isMoving = false;
        }
        else if (!isMoving)
        {
            movementSpeed = tempSpeed;
            isMoving = true;
        }
    }
}
