using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public float width;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        width = lineRenderer.startWidth;
        lineRenderer.material.SetTextureScale("_MainTex", new Vector2(0.01f / width, 1.0f));
    }
}
