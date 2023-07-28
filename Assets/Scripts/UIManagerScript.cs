using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    public static UIManagerScript instance;
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinText;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        hpText.text = "HP: " + PlayerScript.playerScriptStatic.hp.ToString();
        coinText.text = "Coin: " + PlayerScript.playerScriptStatic.coin.ToString();
    }
}
