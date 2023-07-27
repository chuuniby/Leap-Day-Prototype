using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;

public class FreezingChipScript : MonoBehaviour
{
    public float minDistance;
    public GameObject minEnemy;
    public List<GameObject> enemies;
    //public GameObject minMovingBlock;

    public float enemyFreezingTime = 5f;
    public float enemyFreezeTimer;
    public bool startTimer = false;

    private void Start()
    {
        minDistance = Mathf.Infinity;
        minEnemy = null;
        enemyFreezeTimer = enemyFreezingTime;
    }

    public void Update()
    {
        if(startTimer)
        {
            enemyFreezeTimer -= Time.deltaTime;
            if(enemyFreezeTimer < 0)
            {
                //minEnemy.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;    //unfreeze enemy
                minEnemy.GetComponent<NormalEnemyScript>().freeze = false;
                enemyFreezeTimer = enemyFreezingTime;
                startTimer = false;
            }
        }

    }

    public void FindNearestEnemy()
    {
        enemies = transform.GetComponent<EnemyInFreezeRadius>().enemyInFreezeRadius;   //Only get enemy in player seeing range aka player can see
        //foreach (GameObject enemy in enemies)   //old freezing, which get nearest enemy and pass it to FreezeEnemy();
        //{
        //    float distance = Vector3.Distance(transform.position, enemy.transform.position);
        //    if(distance < minDistance)
        //    {
        //        minDistance = distance;
        //        minEnemy = enemy;
        //    }
        //}
    }
    public void FreezeEnemy()
    {
        FindNearestEnemy();
        //if (minEnemy != null)     //old freezing, which get nearest enemy and freeze
        //{
        //    minEnemy.transform.tag = "Platform";
        //    /*minEnemy.transform.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;*/       //freeze enemy
        //    minEnemy.GetComponent<NormalEnemyScript>().freeze = true;
        //    startTimer = true;
        //}

        foreach(GameObject enemy in enemies)
        {
            enemy.transform.tag = "Platform";
            enemy.GetComponent<NormalEnemyScript>().freeze = true;
        }
        startTimer = true;
    }
}
