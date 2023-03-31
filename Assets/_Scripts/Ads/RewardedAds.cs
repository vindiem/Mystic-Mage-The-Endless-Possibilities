using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

public class RewardedAds : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] private Button showAdButton;

    [SerializeField] private string androidAdID = "Rewarded_Android";
    [SerializeField] private string iOSAdID = "Rewarded_iOS";
    private string AdID;

    private void Awake()
    {
        AdID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSAdID : androidAdID;
        showAdButton.interactable = false;
    }

    private void Start()
    {
        LoadAd();
    }

    public void LoadAd()
    {
        Debug.Log("Ad is loading... : " + AdID);
        Advertisement.Load(AdID, this);
        showAdButton.interactable = true;
    }

    public void ShowAd()
    {
        Debug.Log("Ad is showing... : " + AdID);
        //showAdButton.interactable = true;
        Advertisement.Show(AdID, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log("Ad is loading... : " + placementId);

        if (placementId.Equals(AdID) == true)
        {
            showAdButton.onClick.AddListener(ShowAd);
            showAdButton.interactable = true;
        }

    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        // Player reward after ad showing
        int coins = PlayerPrefs.GetInt("Coins");
        coins += 2;
        PlayerPrefs.SetInt("Coins", coins);

        Debug.Log("Ad has been watched, coins += 2");
    }

    private void OnDestroy()
    {
        showAdButton.onClick.RemoveAllListeners();
    }

}
