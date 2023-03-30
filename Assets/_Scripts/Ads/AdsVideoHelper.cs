using UnityEngine.Advertisements;

namespace Helpers
{
    public static class AdsVideoHelper
    {
        public static bool IsReady() => Advertisement.IsReady("Interstitial_Android");

        public static void Show()
        {
            Advertisement.Show("Interstitial_Android");
        }
    }
}