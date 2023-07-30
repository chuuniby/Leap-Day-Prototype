using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInFreezeRadius : MonoBehaviour
{
    public List<GameObject> enemyInFreezeRadius;
    public SpriteRenderer freezeRadiusSprite;
    private void Awake()
    {
        freezeRadiusSprite = transform.GetComponent<SpriteRenderer>();
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

    private void FixedUpdate()
    {
        transform.position = new Vector2(5f, GameObject.FindGameObjectWithTag("Player").transform.position.y + 5f);
    }
}
