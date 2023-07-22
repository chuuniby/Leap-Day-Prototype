using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequencer : MonoBehaviour
{
    public static LevelSequencer instance;
    public bool unlockDockLevel;
    public bool unlockTerraceHouseLevel;
    public bool unlockDockLevel2;
    void Awake()
    {
        instance = this;
    }

    public void ChangeLevel()
    {
        if(unlockDockLevel)
        {
            SceneManager.LoadScene("Docks");
        }
        if(unlockTerraceHouseLevel)
        {
            SceneManager.LoadScene("Terrace House");
        }
        if(unlockDockLevel2)
        {
            SceneManager.LoadScene("Docks 2");
        }
    }
}
