using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public GameObject Sign;
    public bool startFade;
    public bool endFade;
    public float speed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startFade = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            endFade = true;
        }

        if (startFade)
        {
            StartCoroutine(FadeIn());
            startFade = false;
        }
        if (endFade)
        {
            StopAllCoroutines();
            endFade = false;
        }
    }

    IEnumerator FadeIn()
    {
        while (Sign.GetComponent<SpriteRenderer>().color.a > 0)
        {
            // set color with i as alpha
            Color tmpColor = Sign.GetComponent<SpriteRenderer>().color;
            float fadeAmount = tmpColor.a - (speed * Time.deltaTime);
            tmpColor = new Color(tmpColor.r, tmpColor.g, tmpColor.b, fadeAmount);
            Sign.GetComponent<SpriteRenderer>().color = tmpColor;
            yield return null;
        }
        StartCoroutine(FadeOut());

    }

    IEnumerator FadeOut()
    {
        while (Sign.GetComponent<SpriteRenderer>().color.a < 1)
        {
            Color tmpColor = Sign.GetComponent<SpriteRenderer>().color;
            float fadeAmount = tmpColor.a + (speed * Time.deltaTime);
            tmpColor = new Color(tmpColor.r, tmpColor.g, tmpColor.b, fadeAmount);
            Sign.GetComponent<SpriteRenderer>().color = tmpColor;
            yield return null;
            
        }
        StartCoroutine(FadeIn());
    }
}

