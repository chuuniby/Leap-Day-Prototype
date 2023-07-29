using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript playerScriptStatic;
    public MovementScript movementScript;
    public int hp = 3;
    public int coin = 0;
    public int hpMax;
    private void Awake()
    {
        playerScriptStatic = this;

        movementScript = transform.GetComponent<MovementScript>();
    }
    void Start()
    {
        hpMax = GameManagerScript.instance.maxHP;
        coin = GameManagerScript.instance.currentCoin;
        hp = hpMax;
    }
    void Update()
    {
        if (hp <= 0)
        {
            LevelResetManager.instance.reset = true;
        }
        if (hp > hpMax)
        {
            hp = hpMax;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(hp > 0)
        {
            if (collision.transform.CompareTag("Spike"))
            {
                hp -= 1;
                transform.position = LevelResetManager.instance.respawnPoint;
            }
            if (collision.transform.CompareTag("Coin"))
            {
                coin += 1;
                Destroy(collision.gameObject);
            }
            if (collision.transform.CompareTag("EnemyBullet"))
            {
                hp -= 1;
                transform.position = LevelResetManager.instance.respawnPoint;
            }
            if (collision.transform.CompareTag("HealthPack"))
            {
                hp += 1;
                Destroy(collision.gameObject);
                transform.position = LevelResetManager.instance.respawnPoint;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            if (collision.transform.GetComponent<NormalEnemyScript>().freeze)
            {
                return;
            }
            else
            {
                if(hp > 0)
                {
                    if (movementScript.rb2d.velocity.y >= 0)
                    {
                        hp -= 1;
                        transform.position = LevelResetManager.instance.respawnPoint;
                    }
                    else
                    {
                        collision.transform.GetComponent<NormalEnemyScript>().died = true;      //why the fuck would you control the enemy from player script
                    }
                }
            }
        }
    }
}
