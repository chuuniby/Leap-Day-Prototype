using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector3 rawMovement;
    public float horizontalSpeed;
    public float verticalSpeed;
    void Start()
    {
        horizontalSpeed = 8f;
        verticalSpeed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        rawMovement = new Vector3(horizontalSpeed, verticalSpeed);
        transform.position += rawMovement * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
