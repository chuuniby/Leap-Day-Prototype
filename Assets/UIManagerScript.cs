using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinText;
    void Update()
    {
        hpText.text = "HP: " + PlayerScript.playerScriptStatic.hp.ToString();
        coinText.text = "Coin: " + PlayerScript.playerScriptStatic.coin.ToString();
    }
}
