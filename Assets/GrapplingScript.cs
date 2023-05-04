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
    public RaycastHit2D hit;

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
            player.GetComponent<MovementScript>().movementSpeed = 0f;
            player.transform.position += Vector3.MoveTowards(transform.position, hit.point, 100f) * Time.deltaTime* 15f;
            canGrapple = false;
            Destroy(_gizmoLine);
        }
    }
}
