using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCoins : MonoBehaviour
{
    private int currentLevel;
    private float currentXP;
    private float TargetXp;

    private int coins;

    public Image ImageXP;
    public Text currentLevelText;
    public Text coinsText;

    private void Start()
    {
        ImageXP.fillAmount = 0;
        E();
    }

    private void Update()
    {
        E();
    }

    public void E()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        currentXP = PlayerPrefs.GetFloat("Xp");
        coins = PlayerPrefs.GetInt("Coins");
        TargetXp = PlayerPrefs.GetFloat("TargetXp");

        XPToNewLevelSet();
        coinsText.text = coins.ToString();

        //Debug.Log($"Level: {currentLevel}, xp {currentXP}, target xp {TargetXp}");
    }

    private void XPToNewLevelSet()
    {
        if (currentLevel == 0)
        {
            currentLevel = 10;
            PlayerPrefs.SetInt("Level", currentLevel);
        }

        if (currentLevel < 30)
        {
            if (currentXP / TargetXp >= 1)
            {
                currentLevel = PlayerPrefs.GetInt("Level");
                currentLevel++;
                PlayerPrefs.SetInt("Level", currentLevel);
                currentXP -= TargetXp;
                PlayerPrefs.SetFloat("Xp", currentXP);
            }

            ImageXP.fillAmount = currentXP / TargetXp;
            currentLevelText.text = currentLevel.ToString();
            TargetXp = currentLevel * 25 * 2.5f;
            PlayerPrefs.SetFloat("TargetXp", TargetXp);
        }
        else
        {
            ImageXP.fillAmount = currentXP / TargetXp;
            currentLevelText.text = currentLevel.ToString();
        }

    }

}
