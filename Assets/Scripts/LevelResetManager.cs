using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelResetManager : MonoBehaviour
{
    public static LevelResetManager instance;
    public bool reset;
    //public Scene MainGameScene;
    public Vector2 respawnPoint;
    public GameObject player;
    public string currentScene;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        reset = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (reset) //player hp = 0 => reset
        {
            currentScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(currentScene);
            PlayerScript.playerScriptStatic.hp = PlayerScript.playerScriptStatic.hpMax;
            reset = false;
        }
    }
}
