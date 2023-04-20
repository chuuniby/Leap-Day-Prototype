using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowPlatformScript : MonoBehaviour
{
    public float movementSpeed = 2f;
    public Rigidbody2D rb;
    public BoxCollider2D platformCollider;
    public bool isFacingRight;

    Vector3[] verts = new Vector3[4];
    public int nextWayPoint = 1;
    public float distToPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
    }
    private void Update()
    {
        rb.velocity = new Vector2(movementSpeed, 0f);
        //Move();
        
        if(platformCollider != null)
        {
            RaycastHit hit;
            //if(Physics.Raycast(transform.position,transform.TransformDirection()))
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
        verts[0] = mtx.MultiplyPoint3x4(new Vector3(-size.x, size.y));
        verts[1] = mtx.MultiplyPoint3x4(new Vector3(-size.x, -size.y));
        verts[2] = mtx.MultiplyPoint3x4(new Vector3(size.x, -size.y));
        verts[3] = mtx.MultiplyPoint3x4(new Vector3(size.x, size.y));
    }
}
