using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class getCoinsUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI currencyTxt = null;
    public int playerCoins;
    public void setCurrency(int coins)
    {
        playerCoins = coins;
        currencyTxt.SetText(playerCoins.ToString());

    }
    



}
