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

    private void Start()
    {
        GetRelicsValues();
    }

    private void Update()
    {
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
    }

    private void GetRelicsValues()
    {
        MRP = PlayerPrefs.GetInt("Meteor relic progress");
        TRP = PlayerPrefs.GetInt("Tornado relic progress");
        FRP = PlayerPrefs.GetInt("Fire relic progress");
        WRP = PlayerPrefs.GetInt("Wave relic progress");
        URP = PlayerPrefs.GetInt("Ultimate relic progress");

        if (MRP > 5 && MRP <= 10)
        {
            mMRP = 10;
        }
        if (TRP > 5 && TRP <= 10)
        {
            mTRP = 10;
        }
        if (FRP > 5 && FRP <= 10)
        {
            mFRP = 10;
        }
        if (WRP > 5 && WRP <= 10)
        {
            mWRP = 10;
        }
        if (5 < URP && URP <= 10)
        {
            mURP = 10;
        }

    }

}
