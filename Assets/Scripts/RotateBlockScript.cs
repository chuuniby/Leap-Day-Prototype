using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlockScript : MonoBehaviour
{
    public float rotateAmount = -90f;
    public float timeWaitTillNextRotate = 3f;
    public float moveSpeed = 50f;
    [SerializeField] private float timer = 3f;
    void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0f)
        StartCoroutine(RotateObject());
    }

    IEnumerator RotateObject()
    {
        Quaternion endingAngle = transform.rotation * Quaternion.Euler(0f,0f,rotateAmount);

        while (Quaternion.Angle(endingAngle, transform.rotation) > 0.1f)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, endingAngle, moveSpeed * Time.deltaTime);
            timer = timeWaitTillNextRotate;
            yield return null;
        }
        transform.rotation = endingAngle;

        if (transform.GetChild(0).transform.CompareTag("Wall") && transform.GetChild(1).transform.CompareTag("Wall"))
        {
            transform.GetChild(0).transform.tag = transform.GetChild(1).transform.tag = "Platform";
        }
        else if (transform.GetChild(0).transform.CompareTag("Platform") && transform.GetChild(1).transform.CompareTag("Platform"))
        {
            transform.GetChild(0).transform.tag = transform.GetChild(1).transform.tag = "Wall";
        }

        if (transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Wall") && transform.GetChild(1).gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            transform.GetChild(0).gameObject.layer = transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Ground");
        }
        else if (transform.GetChild(0).gameObject.layer == LayerMask.NameToLayer("Ground") && transform.GetChild(1).gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            transform.GetChild(0).gameObject.layer = transform.GetChild(1).gameObject.layer = LayerMask.NameToLayer("Wall");
        }
    }
}
