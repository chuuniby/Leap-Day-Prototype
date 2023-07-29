using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikeBlock : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int currentWaypoint;
    public float speed = 2f;

    public GameObject wayPointGroup;
    private void Awake()
    {
        wayPointGroup = transform.GetChild(0).gameObject;
        wayPointGroup.transform.parent = null;
    }
    void Update()
    {
        Debug.Log(Vector3.Distance(transform.position, patrolPoints[currentWaypoint].position));
        if (Vector3.Distance(transform.position, patrolPoints[currentWaypoint].position) <= 0.2f)
        {
            currentWaypoint += 1;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, patrolPoints[currentWaypoint].position, speed * Time.deltaTime);
        }

        if(currentWaypoint >= patrolPoints.Length)
        {
            currentWaypoint = 0;
        }
    }
}
