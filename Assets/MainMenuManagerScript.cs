using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagerScript : MonoBehaviour
{
    public void GoFirstLevel()
    {
        SceneManager.LoadScene("Docks");
    }
}
