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
    public float offset;

    public Vector3[] verts = new Vector3[4];
    public int nextWayPoint = 1;
    public int index = 1;
    public float distToPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isFacingRight = true;
        enemyDirection = transform.eulerAngles.z;
        offset = 0.25f;
    }
    private void Update()
    {
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, enemyDirection);
        transform.position += transform.right * movementSpeed * Time.deltaTime;
        if (platformCollider != null)
        {
            if (Vector3.Distance(transform.position, verts[index]) < 0.03f)
            {
                enemyDirection -= 90f;
                index++;
            }
            else
            {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.transform.CompareTag("LevelSeperation") || collision.transform.CompareTag("Platform"))
        {
            platformCollider = collision.gameObject.GetComponent<BoxCollider2D>();
            var size = platformCollider.size * 0.5f;
            var mtx = Matrix4x4.TRS(platformCollider.bounds.center, platformCollider.transform.localRotation, platformCollider.transform.localScale);
            verts[0] = mtx.MultiplyPoint3x4(new Vector3(-size.x, size.y)) + new Vector3(-offset, offset); //top left
            verts[1] = mtx.MultiplyPoint3x4(new Vector3(size.x, size.y)) + new Vector3(offset, offset); //top right
            verts[2] = mtx.MultiplyPoint3x4(new Vector3(size.x, -size.y)) + new Vector3(offset, -offset); //bottom right
            verts[3] = mtx.MultiplyPoint3x4(new Vector3(-size.x, -size.y)) + new Vector3(-offset, -offset); //bottom left
        }
    }
}