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

    public Text bestScoreTimeText;
    private float bestScoreTime;

    public Text bestScoreKillsText;
    private int bestScoreKills;


    private void Update()
    {
        GetRelicsValues();

        // Meteor relic progress
        MRPText.text = $"Meteor relic: {MRP} / {mMRP}";
        MRPImage.fillAmount = MRP / mMRP;

        // Tornado relic progress
        TRPText.text = $"Tornado relic: {TRP} / {mTRP}";
        TRPImage.fillAmount = TRP / mTRP;

        // Fire relic progress
        FRPText.text = $"Fire relic: {FRP} / {mFRP}";
        FRPImage.fillAmount = FRP / mFRP;

        // Wave relic progress
        WRPText.text = $"Wave relic: {WRP} / {mWRP}";
        WRPImage.fillAmount = WRP / mWRP;

        // Ultimate relic progress
        URPText.text = $"Ultimate relic: {URP} / {mURP}";
        URPImage.fillAmount = URP / mURP;


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

    }

    private void GetRelicsValues()
    {
        MRP = PlayerPrefs.GetInt("Meteor relic progress");
        TRP = PlayerPrefs.GetInt("Tornado relic progress");
        FRP = PlayerPrefs.GetInt("Fire relic progress");
        WRP = PlayerPrefs.GetInt("Wave relic progress");
        URP = PlayerPrefs.GetInt("Ultimate relic progress");

        if (PlayerPrefs.GetFloat("mMRP") > 5)
        {
            mMRP = PlayerPrefs.GetFloat("mMRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mMRP", 5);
        }
        if (PlayerPrefs.GetFloat("mTRP") > 5)
        {
            mTRP = PlayerPrefs.GetFloat("mTRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mTRP", 5);
        }
        if (PlayerPrefs.GetFloat("mFRP") > 5)
        {
            mFRP = PlayerPrefs.GetFloat("mFRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mFRP", 5);
        }
        if (PlayerPrefs.GetFloat("mWRP") > 5)
        {
            mWRP = PlayerPrefs.GetFloat("mWRP");
        }
        else
        {
            PlayerPrefs.SetFloat("mWRP", 5);
        }
        if (PlayerPrefs.GetFloat("mURP") > 5)
        {
            mURP = PlayerPrefs.GetFloat("mURP");
        }
        else
        {
            PlayerPrefs.SetFloat("mURP", 5);
        }

        float mmrp = PlayerPrefs.GetFloat("mMRP");
        if (MRP > mmrp && MRP <= 320)
        { 
            mmrp *= 2;
        }
        else if (MRP > 320)
        {
            mmrp = 1;
        }

        float mtrp = PlayerPrefs.GetFloat("mTRP");
        if (TRP > mtrp && TRP <= 320)
        {
            mtrp *= 2;
        }
        else if (TRP > 320)
        {
            mtrp = 1;
        }

        float mfrp = PlayerPrefs.GetFloat("mFRP");
        if (TRP > mfrp && FRP <= 320)
        {
            mfrp *= 2;
        }
        else if (FRP > 320)
        {
            mfrp = 1;
        }

        float mwrp = PlayerPrefs.GetFloat("mWRP");
        if (TRP > mwrp && WRP <= 320)
        {
            mwrp *= 2;
        }
        else if (WRP > 320)
        {
            mwrp = 1;
        }

        float murp = PlayerPrefs.GetFloat("mURP");
        if (TRP > murp && URP <= 320)
        {
            murp *= 2;
        }
        else if (URP > 320)
        {
            murp = 1;
        }

    }

    private static string Format(float seconds)
    {
        TimeSpan ts = TimeSpan.FromSeconds(seconds);

        if (ts.Hours != 0) return $"{ts.Hours} h, {ts.Minutes} m, {ts.Seconds} s";
        else if (ts.Minutes != 0) return $"0 h, {ts.Minutes} m, {ts.Seconds} s";
        else if (ts.Seconds != 0) return $"0 h, 0 m, {ts.Seconds} s";
        else
        {
            Debug.LogWarning("Err in formating time");
        }

        return "0 h, 0 m, 0 s";
    }

}
