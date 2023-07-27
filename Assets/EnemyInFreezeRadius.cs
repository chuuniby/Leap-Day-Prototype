using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInFreezeRadius : MonoBehaviour
{
    public List<GameObject> enemyInFreezeRadius;
    public SpriteRenderer freezeRadiusSprite;
    public CircleCollider2D circleCollider;
    private void Awake()
    {
        freezeRadiusSprite = transform.GetComponent<SpriteRenderer>();
        circleCollider = transform.GetComponent<CircleCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyInFreezeRadius.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            enemyInFreezeRadius.Remove(collision.gameObject);
        }
    }

    public void TurnOnFreezeRadiusSprite()
    {
        freezeRadiusSprite.enabled = true;
    }

    public void TurnOffFreezeRadiusSprite()
    {
        freezeRadiusSprite.enabled = false;
    }

    public void TurnOnCollider()
    {
        circleCollider.enabled = true;
    }

    public void TurnOffCollider()
    {
        circleCollider.enabled = false;
    }
}
