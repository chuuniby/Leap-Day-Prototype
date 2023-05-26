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

    public DistanceJoint2D distanceJoint;

    private void Update()
    {
        Debug.DrawRay(drawPosition.position, new Vector3(player.transform.localScale.x, 6f));

        if(isHolding)
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
                    //Debug.Log(hit.transform.gameObject.name);
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

        if (isGrappling)
        {
            player.GetComponent<MovementScript>().movementSpeed = 0f;
            player.GetComponent<MovementScript>().movementUp = 0f;
            player.GetComponent<MovementScript>().enabled = false;
            canGrapple = false;
            //Vector3 direction = hit.point - new Vector2(transform.position.x, transform.position.y);
            //player.transform.position += direction * Time.deltaTime;

            //Vector3.Lerp(player.transform.position, hit.point, Time.deltaTime * player.GetComponent<MovementScript>().movementSpeed);

            distanceJoint.enabled = true;
            distanceJoint.distance -= Time.deltaTime/2;
        }
        else
        {
            distanceJoint.enabled = false;
            player.GetComponent<MovementScript>().enabled = true;
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
        }
    }
}
