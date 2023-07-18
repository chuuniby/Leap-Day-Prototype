using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawnProjectileScript : MonoBehaviour
{
    public GameObject player;
    public float timer;
    [Range(0f, 5f)] public float cooldown;
    public GameObject bullet;
    public Transform spawnPoint;
    void Start()
    {
        timer = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        spawnPoint.transform.LookAt(player.transform.position);
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
            timer = cooldown;
        }
    }
}
