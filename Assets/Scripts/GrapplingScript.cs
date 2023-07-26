using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{
    public MovementScript movementScript;
    public Animator animator;
    public Rigidbody2D rb2D;
    public GameObject gizmoLine;
    public GameObject _gizmoLine;
    public Transform drawPosition;
    public LayerMask playerLayerMask;
    public bool isHolding;
    public bool canGrapple;
    public bool isGrappling;
    public RaycastHit2D hit;
    public float offset;
    public float maxGrapplingSpeed;

    public Vector2 grapplingDir;
    public float grapplingForce;
    public bool isGrapplingJump;

    private void Awake()
    {
        movementScript = transform.GetComponent<MovementScript>();
        animator = transform.GetComponent<Animator>();
        rb2D = transform.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        rb2D.velocity = new Vector2(0f, Mathf.Clamp(rb2D.velocity.y, 0f, maxGrapplingSpeed));
        Debug.Log(rb2D.velocity);
        if (movementScript.isWallSliding)
        {
            canGrapple = false;
        }

        if (isGrappling)    //GrapplingCode
        {
            movementScript.enabled = false;
            animator.enabled = false;
            rb2D.AddForce(grapplingForce * Time.fixedDeltaTime * grapplingDir); //Dont get component in update //5000f for grappling force
            float distance = (hit.point.y + offset)- transform.position.y;
            if(distance <= 0.001f)
            {
                movementScript.enabled = true;
                animator.enabled = true;
                isGrappling = false;
                isGrapplingJump = false;
            }
            //player.transform.position = Vector2.Lerp(player.transform.position, hit.point, grapplingForce * Time.fixedDeltaTime);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                movementScript.enabled = true;
                animator.enabled = true;
                isGrappling = false;
                movementScript.GrapplingJump();
            }
        }
        //else
        //{
        //    player.GetComponent<MovementScript>().enabled = true; //WAS I FUCKING CRAZY???????? now the movementscript cannot be disabled lmao
        //}

        //if (isGrapplingJump)
        //{
        //    isGrappling = false;
        //    player.GetComponent<MovementScript>().GrapplingJump(); //Dont get component in update
        //    //Find a way to stop GrapplingJump after hit a wall or landed or afterGraplingJump
        //}
    }

    private void Update()
    {
        Debug.DrawRay(drawPosition.position, Vector2.up);
        //Debug.DrawRay(drawPosition.position, grapplingDir,Color.red); //Draw Direction of grappling
        if (isHolding)
        {
            hit = Physics2D.Raycast(drawPosition.position, Vector2.up, Mathf.Infinity, ~playerLayerMask);
            if (/*Physics2D.Raycast(drawPosition.position, new Vector3(player.transform.localScale.x, 0f), Mathf.Infinity)*/ hit )
            {
                if (hit.transform == null)
                {
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
                    canGrapple = false;
                }
                else if (hit.transform.CompareTag("LevelSeperation"))
                {
                    grapplingDir = new Vector2(0f, hit.point.y - transform.position.y + offset).normalized;
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.green;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.green;
                    canGrapple = true;
                }
                else
                {
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
                    canGrapple = false;
                }
            }
        }
    }

    public void DrawGizmos()
    {
        _gizmoLine = Instantiate(gizmoLine, drawPosition.position, Quaternion.identity);
        Vector3 localscale = transform.localScale;
        _gizmoLine.transform.localScale = localscale;
        _gizmoLine.transform.SetParent(transform);
    }
    public void IsHoldingButton()
    {
        isHolding = true;
    }

    public void ReleaseButton()
    {
        isHolding = false;
        if (!canGrapple)
        {
            Destroy(_gizmoLine);
        }
        else
        {
            //grapple
            //Debug.Log(hit.point);
            isGrappling = true;

            Destroy(_gizmoLine);
        }
    }
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (isGrappling)
    //    {
    //        if (collision.transform.CompareTag("LevelSeperation"))
    //        {
    //            isGrappling = false;
    //            isGrapplingJump = false;
    //        }
    //    }
    //}
}
