using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserInfo : MonoBehaviour
{
    public static LaserInfo instance;
    public GameObject top;
    public GameObject mid;
    public GameObject bot;
    void Awake()
    {
        instance = this;
    }
}
