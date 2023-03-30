using System;
using UnityEngine.Advertisements;

namespace Helpers
{
    public static class AdsRewardedHelper
    {
        public static bool IsReady() => Advertisement.IsReady("Rewarded_Android");

        public static void Show(Action<ShowResult> callback)
        {
            ShowOptions showOptions = new ShowOptions
            {
                resultCallback = callback
            };
            Advertisement.Show("Rewarded_Android");

        }

    }
}
