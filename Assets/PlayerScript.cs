using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript playerScriptStatic;
    public int hp;
    public int coin;
    public int hpMax;
    private void Awake()
    {
        playerScriptStatic = this;
    }
    void Start()
    {
        hpMax = 3;
        hp = hpMax;
        coin = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            hp = 0;
            //respawn or smth
        }
        if (hp > hpMax)
        {
            hp = hpMax;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Spike"))
        {
            hp -= 1;
        }
        if (collision.transform.CompareTag("Coin"))
        {
            coin += 1;
            Destroy(collision.gameObject);
        }
        if (collision.transform.CompareTag("EnemyBullet"))
        {
            hp -= 1;
        }
        if (collision.transform.CompareTag("HealthPack"))
        {
            hp += 1;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            hp -= 1;
        }
    }
}
