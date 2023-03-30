using UnityEngine;
using UnityEngine.Advertisements;
using Helpers;
using System;

public class AdsHelper : MonoBehaviour
{
    private string GameId = "5224018";

    private void Awake()
    {
        Advertisement.Initialize(GameId);
    }

    public void ShowVideoAd()
    {
        if (AdsVideoHelper.IsReady() == true)
        {
            AdsVideoHelper.Show();
        }
    }

    public void ShowRewardedVideoAd(Action<ShowResult> callback)
    {
        if (AdsRewardedHelper.IsReady() == true)
        {
            AdsRewardedHelper.Show(callback);

            int coins = PlayerPrefs.GetInt("Coins");
            coins += 2;
            PlayerPrefs.SetInt("Coins", coins);
        }
    }

    public bool VideoAdIsReady() => AdsVideoHelper.IsReady();

    public bool RewardedVideoAdIsReady() => AdsRewardedHelper.IsReady();

    public void RewardedVideoAd()
    {
        ShowRewardedVideoAd(callback =>
        {
            switch (callback)
            {
                case ShowResult.Finished:
                    Debug.Log("ShowResult -> Finished");
                    break;
                case ShowResult.Skipped:
                    Debug.Log("ShowResult -> Skipped");
                    break;
                case ShowResult.Failed:
                    Debug.Log("ShowResult -> Failed");
                    break;
            }
        });
    }

}
