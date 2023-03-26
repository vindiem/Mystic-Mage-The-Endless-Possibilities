using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewAd : MonoBehaviour
{
    private string UnitId = "ca-app-pub-3940256099942544/5224354917";
    private RewardedAd rewardedAd;

    private void OnEnable()
    {
        rewardedAd = new RewardedAd(UnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        rewardedAd.LoadAd(adRequest);
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReawrd;
    }

    private void HandleUserEarnedReawrd(object sender, Reward e)
    {
        int coins = PlayerPrefs.GetInt("Coins");
        coins += 2;
        PlayerPrefs.SetInt("Coins", coins);
    }

    public void ShowAd()
    {
        if (rewardedAd.IsLoaded() == true)
        {
            rewardedAd.Show();
        }
    }

}
