using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Relics : MonoBehaviour
{
    private float MRP = 0, TRP = 0, FRP = 0, WRP = 0, URP = 0;
    private float mMRP = 5, mTRP = 5, mFRP = 5, mWRP = 5, mURP = 5;

    public Text MRPText, TRPText, FRPText, WRPText, URPText;
    public Image MRPImage, TRPImage, FRPImage, WRPImage, URPImage;

    // Best score
    public Text bestScoreTimeText;
    private float bestScoreTime;

    public Text bestScoreKillsText;
    private int bestScoreKills;

    private string MeteorBlock,
                TornadoBlock,
                FireBlock,
                WaveBlock,
                UltimateBlock;

    public GameObject MeteorBlocker, 
                    TornadoBlocker, 
                    FireBlocker, 
                    WaveBlocker, 
                    UltimateBlocker;

    private int relicPrice = 50;
    public Text[] relicPriceTexts;

    private void Start()
    {
        MeteorBlocker.gameObject.SetActive(true);
        TornadoBlocker.gameObject.SetActive(true);
        FireBlocker.gameObject.SetActive(true);
        WaveBlocker.gameObject.SetActive(true);
        UltimateBlocker.gameObject.SetActive(true);

        relicPrice = PlayerPrefs.GetInt("RelicPrice");
        if (relicPrice < 50) relicPrice = 50;

        MeteorBlock = PlayerPrefs.GetString("MeteorBlock");
        TornadoBlock = PlayerPrefs.GetString("TornadoBlock");
        FireBlock = PlayerPrefs.GetString("FireBlock");
        WaveBlock = PlayerPrefs.GetString("WaveBlock");
        UltimateBlock = PlayerPrefs.GetString("UltimateBlock");

        if (MeteorBlock == "true") 
            MeteorBlocker.gameObject.SetActive(true);
        else if (MeteorBlock == "false") 
            MeteorBlocker.gameObject.SetActive(false);

        if (TornadoBlock == "true") 
            TornadoBlocker.gameObject.SetActive(true);
        else if (TornadoBlock == "false") 
            TornadoBlocker.gameObject.SetActive(false);

        if (FireBlock == "true") 
            FireBlocker.gameObject.SetActive(true);
        else if (FireBlock == "false") 
            FireBlocker.gameObject.SetActive(false);

        if (WaveBlock == "true") 
            WaveBlocker.gameObject.SetActive(true);
        else if (WaveBlock == "false") 
            WaveBlocker.gameObject.SetActive(false);

        if (UltimateBlock == "true") 
            UltimateBlocker.gameObject.SetActive(true);
        else if (UltimateBlock == "false") 
            UltimateBlocker.gameObject.SetActive(false);

    }

    private void Update()
    {
        MeteorBlock = PlayerPrefs.GetString("MeteorBlock");
        TornadoBlock = PlayerPrefs.GetString("TornadoBlock");
        FireBlock = PlayerPrefs.GetString("FireBlock");
        WaveBlock = PlayerPrefs.GetString("WaveBlock");
        UltimateBlock = PlayerPrefs.GetString("UltimateBlock");

        // Meteor relic progress
        if (MeteorBlock == "false")
        {
            GetRelicsValues();

            MRPText.text = $"Meteor relic: {MRP} / {mMRP}";
            MRPImage.fillAmount = MRP / mMRP;
        }
        else if (MeteorBlock == "true")
        {
            MeteorBlocker.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("Meteor relic progress", 0);
        }

        // Tornado relic progress
        if (TornadoBlock == "false")
        {
            GetRelicsValues();

            TRPText.text = $"Tornado relic: {TRP} / {mTRP}";
            TRPImage.fillAmount = TRP / mTRP;
        }
        else if (TornadoBlock == "true")
        {
            TornadoBlocker.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("Tornado relic progress", 0);
        }

        // Fire relic progress
        if (FireBlock == "false")
        {
            GetRelicsValues();

            FRPText.text = $"Fire relic: {FRP} / {mFRP}";
            FRPImage.fillAmount = FRP / mFRP;
        }
        else if (FireBlock == "true")
        {
            FireBlocker.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("Fire relic progress", 0);
        }

        // Wave relic progress
        if (WaveBlock == "false")
        {
            GetRelicsValues();

            WRPText.text = $"Wave relic: {WRP} / {mWRP}";
            WRPImage.fillAmount = WRP / mWRP;
        }
        else if (WaveBlock == "true")
        {
            WaveBlocker.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("Wave relic progress", 0);
        }

        // Ultimate relic progress
        if (UltimateBlock == "false")
        {
            GetRelicsValues();

            URPText.text = $"Ultimate relic: {URP} / {mURP}";
            URPImage.fillAmount = URP / mURP;
        }
        else if (UltimateBlock == "true")
        {
            UltimateBlocker.gameObject.SetActive(true);
            PlayerPrefs.SetFloat("Ultimate relic progress", 0);
        }

        // Best score value set
        if (bestScoreTimeText != null)
        {
            bestScoreTime = PlayerPrefs.GetFloat("BestScore");
            bestScoreTimeText.text = $"Best score: " + Format(bestScoreTime);
        }
        if (bestScoreKillsText != null)
        {
            bestScoreKills = PlayerPrefs.GetInt("BestScoreKills");
            bestScoreKillsText.text = $"Best score kills: " + bestScoreKills;
        }

        // Relic price set
        for (int i = 0; i < relicPriceTexts.Length; i++)
        {
            relicPriceTexts[i].text = $"Buy relic: {relicPrice} coins";
        }

    }

    private void GetRelicsValues()
    {
        MRP = PlayerPrefs.GetInt("Meteor relic progress");
        TRP = PlayerPrefs.GetInt("Tornado relic progress");
        FRP = PlayerPrefs.GetInt("Fire relic progress");
        WRP = PlayerPrefs.GetInt("Wave relic progress");
        URP = PlayerPrefs.GetInt("Ultimate relic progress");

        if (PlayerPrefs.GetFloat("mMRP") > 0)
        {
            mMRP = PlayerPrefs.GetFloat("mMRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mMRP", 5);
        }
        if (PlayerPrefs.GetFloat("mTRP") > 0)
        {
            mTRP = PlayerPrefs.GetFloat("mTRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mTRP", 5);
        }
        if (PlayerPrefs.GetFloat("mFRP") > 0)
        {
            mFRP = PlayerPrefs.GetFloat("mFRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mFRP", 5);
        }
        if (PlayerPrefs.GetFloat("mWRP") > 0)
        {
            mWRP = PlayerPrefs.GetFloat("mWRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mWRP", 5);
        }
        if (PlayerPrefs.GetFloat("mURP") > 0)
        {
            mURP = PlayerPrefs.GetFloat("mURP");
        }
        else
        {
            PlayerPrefs.SetFloat("mURP", 5);
        }

    }

    private static string Format(float seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);

        if (ts.Hours != 0) return $"{ts.Hours} h, {ts.Minutes} m, {ts.Seconds} s";
        else if (ts.Minutes != 0) return $"{ts.Minutes} m, {ts.Seconds} s";
        else if (ts.Seconds != 0) return $"{ts.Seconds} s";

        return "0 s";
    }

    public void BuyRelic(string relicName)
    {
        int coinsCount = PlayerPrefs.GetInt("Coins");
        if (coinsCount >= relicPrice)
        {
            switch (relicName)
            {
                case "Meteor":
                    coinsCount -= relicPrice;
                    PlayerPrefs.SetInt("Coins", coinsCount);

                    MeteorBlocker.SetActive(false);
                    MeteorBlock = "false";
                    PlayerPrefs.SetString("MeteorBlock", "false");
                    break;
                case "Tornado":
                    coinsCount -= relicPrice;
                    PlayerPrefs.SetInt("Coins", coinsCount);

                    TornadoBlocker.SetActive(false);
                    TornadoBlock = "false";
                    PlayerPrefs.SetString("TornadoBlock", "false");
                    break;
                case "Fire":
                    coinsCount -= relicPrice;
                    PlayerPrefs.SetInt("Coins", coinsCount);

                    FireBlocker.SetActive(false);
                    FireBlock = "false";
                    PlayerPrefs.SetString("FireBlock", "false");
                    break;
                case "Wave":
                    coinsCount -= relicPrice;
                    PlayerPrefs.SetInt("Coins", coinsCount);

                    WaveBlocker.SetActive(false);
                    WaveBlock = "false";
                    PlayerPrefs.SetString("WaveBlock", "false");
                    break;
                case "Ultimate":
                    coinsCount -= relicPrice;
                    PlayerPrefs.SetInt("Coins", coinsCount);

                    UltimateBlocker.SetActive(false);
                    UltimateBlock = "false";
                    PlayerPrefs.SetString("UltimateBlock", "false");
                    break;
            }
            relicPrice *= 5;
            PlayerPrefs.SetInt("RelicPrice", relicPrice);
        }

    }

}