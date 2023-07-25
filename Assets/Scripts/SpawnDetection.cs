using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnDetection : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //BossScript.instance.spawnPoints.Clear(); //Clear
        if (BossScript.instance.spawnDetectionScriptOn == true)
        {
            if (collision.CompareTag("Player"))
            {
                foreach (BoxCollider2D col in BossScript.instance.leftMidRight)
                {
                    col.enabled = false;
                }
                BossScript.instance.spawnPoints.Add(transform.GetChild(0).gameObject);
                //play some animation to let player know where the enemy going to spawn

                Debug.Log("shoot");
                BossScript.instance.StartCoroutine("SpawnEnemy");
            }
        }
    }
}
