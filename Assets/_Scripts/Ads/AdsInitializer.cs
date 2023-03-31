using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameID = "5224018";
    [SerializeField] private string iOSGameID = "5224019";
    [SerializeField] private bool testMode = true;
    private string GameID;

    private void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        GameID = (Application.platform == RuntimePlatform.IPhonePlayer) ? iOSGameID : androidGameID;
        Advertisement.Initialize(GameID, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.LogWarning("Ads initialization complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogWarning($"Ads initialization failed error: {error}, message: {message}");
    }
}
