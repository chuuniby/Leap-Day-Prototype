using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class FreezingChipScript : MonoBehaviour
{
    public float minDistance;
    public GameObject minEnemy;
    public GameObject[] enemies;

    private void Start()
    {
        minDistance = Mathf.Infinity;
        minEnemy = null;
    }

    public void FindNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < minDistance)
            {
                minDistance = distance;
                minEnemy = enemy;
            }
        }
    }

    public void FreezeEnemy()
    {
        FindNearestEnemy();
        if (minEnemy != null)
        {
            minEnemy.transform.tag = "Platform";
            minEnemy.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
