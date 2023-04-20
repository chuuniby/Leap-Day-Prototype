using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int hp;
    public int coin;
    void Start()
    {
        hp = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            hp = 0;
            //Game Over
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
        }

    }
}
