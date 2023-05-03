using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawnerScript : MonoBehaviour
{
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
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Instantiate(bullet, spawnPoint.position, Quaternion.identity);
            timer = cooldown;
        }
    }
}
