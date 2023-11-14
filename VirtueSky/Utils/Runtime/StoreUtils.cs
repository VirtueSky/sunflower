using UnityEngine;

namespace VirtueSky.Utils
{
    public static class StoreUtils
    {
        public static void OpenStore()
        {
#if UNITY_ANDROID
            Application.OpenURL($"market://details?id={Application.identifier}");
#elif UNITY_IPHONE
            Application.OpenURL("itms-apps://itunes.apple.com/app/1598593737");
#endif
        }
    }
}