using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnDetection : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossScript.instance.spawnPoints.Add(transform.GetChild(0));
            Debug.Log("shoot");
        }
    }
}
