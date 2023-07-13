using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TmpCollider : MonoBehaviour
{
    public static TmpCollider instance;

    public BoxCollider2D tmpCol;
    public bool isCollidedThisFrame;
    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Wall"))
        {
            TmpCollider.instance.isCollidedThisFrame = true;
        }
    }
}
