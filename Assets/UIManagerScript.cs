using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManagerScript : MonoBehaviour
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI coinText;
    public PlayerScript playerScript;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP: " + playerScript.hp.ToString();
        coinText.text = "Coin: " + playerScript.coin.ToString();
    }
}
