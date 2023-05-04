using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class FreezingChipScript : MonoBehaviour
{
    public GameObject[] enemies;
    public GameObject enemy;
    public List<float> distances;
    public float distance;
    public float minDistance;

    void FindNearestEnemy()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            distance = Vector3.Distance(transform.position, enemy.transform.position);
            distances.Add(distance);
            minDistance = distances.AsQueryable().Min();
            //int index = distances.FindIndex(minDistance);
        }
    }
}
