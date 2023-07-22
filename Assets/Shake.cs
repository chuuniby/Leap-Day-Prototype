using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class Shake : MonoBehaviour
{
    public float duration = 1.2f;
    public float magnitude;
    public AnimationCurve curve;
    IEnumerator Shaking()
    {
        Vector3 startPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            magnitude = curve.Evaluate(elapsedTime / duration);
            transform.position = startPos + Random.insideUnitSphere * magnitude;
            yield return null;
        }

        transform.position = startPos;
    }
}
