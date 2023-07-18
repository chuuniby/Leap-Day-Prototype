using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurningOnOffColliderEnemy3DUIScript : MonoBehaviour
{
    public BoxCollider upCollider;
    public BoxCollider downCollider;
    public BoxCollider leftCollider;
    public BoxCollider rightCollider;

    public bool upColliderIsOn;
    public bool downColliderIsOn;
    public bool leftColliderIsOn;
    public bool rightColliderIsOn;
    public void checkUpCollider()
    {
        if (upColliderIsOn)
        {
            upCollider.enabled = false;
            upColliderIsOn = false;
        }
        else if (!upColliderIsOn)
        {
            upCollider.enabled = true;
            upColliderIsOn = true;
        }
    }
    public void checkDownCollider()
    {
        if (downColliderIsOn)
        {
            downCollider.enabled = false;
            downColliderIsOn = false;
        }
        else if (!downColliderIsOn)
        {
            downCollider.enabled = true;
            downColliderIsOn = true;
        }
    }
    public void checkLeftCollider()
    {
        if (leftColliderIsOn)
        {
            leftCollider.enabled = false;
            leftColliderIsOn = false;
        }
        else if (!leftColliderIsOn)
        {
            leftCollider.enabled = true;
            leftColliderIsOn = true;
        }
    }
    public void checkRightCollider()
    {
        if (rightColliderIsOn)
        {
            rightCollider.enabled = false;
            rightColliderIsOn = false;
        }
        else if (!rightColliderIsOn)
        {
            rightCollider.enabled = true;
            rightColliderIsOn = true;
        }
    }


}
