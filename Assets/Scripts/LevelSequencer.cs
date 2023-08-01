using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSequencer : MonoBehaviour
{
    public static LevelSequencer instance;
    public bool unlockDockLevel;
    public bool unlockTerraceHouseLevel;
    public bool unlockTerraceHouseLevel2;
    public bool unlockDockLevel2;
    public bool unlockHDB;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        if (unlockDockLevel2)
        {
            SceneManager.LoadScene("Docks 2");
            unlockDockLevel2 = false;
        }
        if(unlockHDB)
        {
            SceneManager.LoadScene("HDB");
            unlockHDB = false;
        }
    }

    //public void ChangeLevel()
    //{
    //        SceneManager.LoadScene("Docks");
    //    }
    //    if(unlockTerraceHouseLevel2)
    //    {
    //        SceneManager.LoadScene("Terrace House 2");      //This is the wrong sequence it should go to Terrace House then Dock 2 then Terrace House 2
    //    }
    //    if(unlockDockLevel2)
    //    {
    //        SceneManager.LoadScene("Docks 2");
    //    }
    //    if (unlockTerraceHouseLevel)
    //    {
    //        
    //    if(unlockDockLevel)
    //    {SceneManager.LoadScene("Terrace House");
    //    }
    //}
}
