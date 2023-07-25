using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaScript : MonoBehaviour
{
    private const string MethodName = "Shaking";
    public float timer = 1.5f;
    public Camera cam;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                cam.GetComponent<Shake>().StartCoroutine(MethodName);
                BossScript.instance.currentPhase = BossScript.BossPhase.Intro;
                transform.GetComponent<BoxCollider2D>().enabled = false;
                timer = 1.5f;
            }
        }
    }
}
