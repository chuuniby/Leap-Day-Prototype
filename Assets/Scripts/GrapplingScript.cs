using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{
    public GameObject player;
    public GameObject gizmoLine;
    public GameObject _gizmoLine;
    public Transform drawPosition;
    public LayerMask playerLayerMask;
    public bool isHolding;
    public bool canGrapple;
    public bool isGrappling;
    public RaycastHit2D hit;

    public Vector2 grapplingDir;
    public float grapplingForce;
    public bool isGrapplingJump;

    private void Start()
    {
        grapplingForce = 3000f;
    }
    private void FixedUpdate()
    {
        if (isGrappling)    //GrapplingCode
        {
            //player.GetComponent<MovementScript>().movementSpeed = 0f;
            //player.GetComponent<MovementScript>().movementUp = 0f;
            player.GetComponent<MovementScript>().enabled = false; //this doesnt help with why the player does not end up at where the indicator of the grappling is
            canGrapple = false;
            player.GetComponent<Rigidbody2D>().AddForce(grapplingForce * Time.fixedDeltaTime * grapplingDir); //Dont get component in update

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.GetComponent<MovementScript>().enabled = true;
                isGrapplingJump = true;
            }
        }
        else
        {
            player.GetComponent<MovementScript>().enabled = true;
        }

        if (isGrapplingJump)
        {
            isGrappling = false;
            player.GetComponent<MovementScript>().GrapplingJump(); //Dont get component in update
            //Find a way to stop GrapplingJump after hit a wall or landed or afterGraplingJump
        }
    }

    private void Update()
    {
        Debug.DrawRay(drawPosition.position, new Vector3(player.transform.localScale.x, 6f));
        //Debug.DrawRay(drawPosition.position, grapplingDir,Color.red); //Draw Direction of grappling
        if (isHolding)
        {
            hit = Physics2D.Raycast(drawPosition.position, new Vector3(player.transform.localScale.x, 6f), 6f, ~playerLayerMask);
            if (Physics2D.Raycast(drawPosition.position, new Vector3(player.transform.localScale.x, 6f), 6f))
            {
                if (hit.transform == null)
                {

                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
                    canGrapple = false;
                }
                else if (hit.transform.CompareTag("Platform"))
                {
                    grapplingDir = new Vector2(hit.point.x - player.transform.position.x, hit.point.y - player.transform.position.y).normalized;
                    //Debug.Log(hit.transform.gameObject.name);
                    _gizmoLine.GetComponent<LineRenderer>().startColor = Color.green;
                    _gizmoLine.GetComponent<LineRenderer>().endColor = Color.green;
                    canGrapple = true;
                }
                else
                {
                    //Debug.Log(hit.transform.gameObject.name);
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
        Vector3 localscale = player.transform.localScale;
        _gizmoLine.transform.localScale = localscale;
        _gizmoLine.transform.SetParent(player.transform);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Platform"))
        {
            isGrappling = false;
            isGrapplingJump = false;
        }
    }
}
