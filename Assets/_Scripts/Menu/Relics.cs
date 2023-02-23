using System.Collections;
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
        MRPText.text = $"Metor relic: {MRP} / {mMRP}";
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
            bestScoreTimeText.text = $"best score: " + bestScoreTime.ToString("0") + " seconds";
        }
        if (bestScoreKillsText != null)
        {
            bestScoreKills = PlayerPrefs.GetInt("BestScoreKills");
            bestScoreKillsText.text = $"kills: " + bestScoreKills.ToString("0");
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

        /*
                PlayerPrefs.SetFloat("mMRP", mMRP);
                PlayerPrefs.SetFloat("mTRP", mTRP);
                PlayerPrefs.SetFloat("mFRP", mFRP);
                PlayerPrefs.SetFloat("mWRP", mWRP);
                PlayerPrefs.SetFloat("mURP", mURP);

                if (MRP > mMRP && MRP <= 100)
                {
                    mMRP *= 2;
                }
                else if (MRP > 100)
                {
                    mMRP = 1;
                }

                if (TRP > mTRP && TRP <= 100)
                {
                    mTRP *= 2;
                }
                else if (TRP > 100)
                {
                    mTRP = 1;
                }

                if (FRP > mFRP && FRP <= 100)
                {
                    mFRP *= 2;
                }
                else if (FRP > 100)
                {
                    mFRP = 1;
                }

                if (WRP > mWRP && WRP <= 100)
                {
                    mWRP *= 2;
                }
                else if (WRP > 100)
                {
                    mWRP = 1;
                }

                if (URP > mURP && URP <= 100)
                {
                    mURP *= 2;
                }
                else if (URP > 100)
                {
                    mURP = 1;
                }
        */
    }

}
