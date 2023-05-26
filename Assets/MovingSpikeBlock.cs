using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpikeBlock : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int currentWaypoint;
    public float speed;

    private void Awake()
    {
        speed = 2f;
        currentWaypoint = 0;
    }
    void Update()
    {
        //if (Vector3.Distance(transform.position, patrolPoints[currentWaypoint].position) <= 3f)
        //{
        //    speed = 10f;
        //}
        if (Vector3.Distance(transform.position, patrolPoints[currentWaypoint].position) <= 0.2f)
        {
            currentWaypoint += 1;
            //transform.LookAt(patrolPoints[currentWaypoint].position);
            speed = 2f;
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
