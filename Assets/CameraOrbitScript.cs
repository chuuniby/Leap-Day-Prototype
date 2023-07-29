using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbitScript : MonoBehaviour
{
    public Transform target;
    public float speed;

    private void Update()
    {
        transform.LookAt(target);
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
    }
}
