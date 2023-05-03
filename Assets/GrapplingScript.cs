using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingScript : MonoBehaviour
{
    public GameObject player;
    public GameObject gizmoLine;
    public GameObject _gizmoLine;
    public Transform drawPosition;

    private void Update()
    {
        Debug.DrawRay(drawPosition.position, new Vector3(player.transform.localScale.x, 6f));
    }

    public void DrawGizmos()
    {
        _gizmoLine = Instantiate(gizmoLine, drawPosition.position, Quaternion.identity);
        Vector3 localscale = player.transform.localScale;
        _gizmoLine.transform.localScale = localscale;
        _gizmoLine.transform.SetParent(player.transform);
    }

    public void DeleteGizmos()
    {
        Destroy(_gizmoLine);
    }

    public void CheckIfGrapplingHitPlatform()
    {
        RaycastHit hit;
        if(Physics.Raycast(drawPosition.position, new Vector3(player.transform.localScale.x, 6f), out hit, 6f))
        {
            Debug.Log(hit.transform.gameObject.name);
            _gizmoLine.GetComponent<LineRenderer>().startColor = Color.green;
            _gizmoLine.GetComponent<LineRenderer>().endColor = Color.green;
        }
        else
        {
            _gizmoLine.GetComponent<LineRenderer>().startColor = Color.red;
            _gizmoLine.GetComponent<LineRenderer>().endColor = Color.red;
        }
    }
}
