using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class InterAd : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    
    private string UnitId = "";

    private void OnEnable()
    {
        interstitialAd = new InterstitialAd(UnitId);
        AdRequest adRequest = new AdRequest.Builder().Build();
        interstitialAd.LoadAd(adRequest);
    }

    public void ShowAd()
    {
        if (interstitialAd.IsLoaded() == true)
        {
            interstitialAd.Show();
        }
    }

}
