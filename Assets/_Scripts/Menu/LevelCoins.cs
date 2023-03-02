using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class LevelCoins : MonoBehaviour
{
    private int currentLevel;
    private float currentXP;
    private float XPToNewLevel;

    private int coins;

    public Image ImageXP;
    public Text currentLevelText;
    public Text coinsText;

    private void Start()
    {
        E();
    }

    private void Update()
    {
        E();
    }

    private void E()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        currentXP = PlayerPrefs.GetFloat("Xp");
        coins = PlayerPrefs.GetInt("Coins");

        XPToNewLevelSet();
        coinsText.text = coins.ToString();
    }

    private void XPToNewLevelSet()
    {
        if (currentLevel == 0)
        {
            currentLevel = 10;
            PlayerPrefs.SetInt("Level", currentLevel);
        }

        if (currentLevel <= 30)
        {
            XPToNewLevel = currentLevel * 25 * 1.5f;

            if (currentXP / XPToNewLevel >= 1)
            {
                currentLevel++;
                PlayerPrefs.SetInt("Level", currentLevel);
                currentXP -= XPToNewLevel;
            }

            ImageXP.fillAmount = currentXP / XPToNewLevel;
            currentLevelText.text = currentLevel.ToString();
        }

        /*
        switch (currentLevel)
        {
            case 10:
                XPToNewLevel = 250;
                break;
            case 11:
                XPToNewLevel = 480;
                break;
            case 12:
                XPToNewLevel = 890;
                break;
            case 13:
                XPToNewLevel = 1100;
                break;
            case 14:
                XPToNewLevel = 1290;
                break;
            case 15:
                XPToNewLevel = 1460;
                break;
            case 16:
                XPToNewLevel = 1690;
                break;
        }
        */

    }

}
