using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractObjectScript : MonoBehaviour
{
    public GameObject redObject;
    public GameObject blueObject;
    public bool red = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!red)
            {
                if(blueObject != null)
                {
                    redObject.SetActive(true);
                    blueObject.SetActive(false);
                    transform.GetComponent<SpriteRenderer>().color = Color.red;
                    red = true;
                }
                else
                {
                    redObject.SetActive(true);
                    red = true;
                }
            }
            else
            {
                if (blueObject != null)
                {
                    redObject.SetActive(false);
                    blueObject.SetActive(true);
                    transform.GetComponent<SpriteRenderer>().color = Color.blue;
                    red = false;
                }
                else
                {
                    redObject.SetActive(false);
                    red = false;
                }
            }
        }
    }
}
