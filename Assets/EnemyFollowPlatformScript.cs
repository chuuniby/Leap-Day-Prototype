using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlatformScript : MonoBehaviour
{
    public float movementSpeed = 1f;
    public Rigidbody2D rb;
    public BoxCollider2D platformCollider;
    public bool isFacingRight;
    public float enemyDirection;

    Vector3[] verts = new Vector3[4];
    public int nextWayPoint = 1;
    public int index = 1;
    public float distToPoint;
    public float offset;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        enemyDirection = transform.eulerAngles.z;
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x,transform.rotation.y,enemyDirection);
        transform.position += transform.right * movementSpeed * Time.deltaTime;

        //Move();

        if (platformCollider != null)
        {
            //RaycastHit hit;
            //if(Physics.Raycast(transform.position,transform.TransformDirection()))
            Debug.Log(index);
            Debug.Log(Vector3.Distance(transform.position, verts[index]));

            if (Vector3.Distance(transform.position, verts[index]) < 0.3f)
            {
                enemyDirection -= 90f;
                index++;
                //movementSpeed = 0f;

            }
            else
            {
                //rb.AddForce(transform.right * movementSpeed);
                Vector2.MoveTowards(transform.position, verts[index], movementSpeed * Time.deltaTime);
            }
            if (index >= verts.Length)
            {
                index = 0;
            }
            if (enemyDirection <= -360f)
            {
                enemyDirection = 0f;
            }
        }
    }

    //public void Move()
    //{
    //    distToPoint = Vector2.Distance(transform.position, verts[nextWayPoint]);
    //    transform.position = Vector2.MoveTowards(transform.position, verts[nextWayPoint], movementSpeed*Time.deltaTime);

    //    if(distToPoint < 0.2f)
    //    {
    //        TakeTurn();
    //    }
    //}

    //public void TakeTurn()
    //{
    //    Vector3 currRot = transform.eulerAngles;
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        platformCollider = collision.gameObject.GetComponent<BoxCollider2D>();
        var size = platformCollider.size * 0.5f;
        var mtx = Matrix4x4.TRS(platformCollider.bounds.center, platformCollider.transform.localRotation, platformCollider.transform.localScale);
        verts[0] = mtx.MultiplyPoint3x4(new Vector3(-size.x, size.y)); //top left
        verts[1] = mtx.MultiplyPoint3x4(new Vector3(size.x, size.y)); //top right
        verts[2] = mtx.MultiplyPoint3x4(new Vector3(size.x, -size.y)); //bottom right
        verts[3] = mtx.MultiplyPoint3x4(new Vector3(-size.x, -size.y)); //bottom left
        Debug.Log("top left: " + verts[0]);
        Debug.Log("top right: " + verts[1]);
        Debug.Log("bottom right: " + verts[2]);
        Debug.Log("bottom left: " + verts[3]);
    }
}