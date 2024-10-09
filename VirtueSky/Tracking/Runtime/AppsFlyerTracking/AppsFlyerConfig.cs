using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Utils;

namespace VirtueSky.Tracking
{
    [EditorIcon("icon_scriptable"), HideMonoScript]
    public class AppsFlyerConfig : ScriptableSettings<AppsFlyerConfig>
    {
        [SerializeField] private string devKey;
        [SerializeField] private string appID;
        [SerializeField] private string uwpAppID;
        [SerializeField] private string macOSAppID;
        [SerializeField] private bool getConversionData;
        [SerializeField] private bool isDebug;
        [SerializeField] private bool isDebugAdRevenue;


        public static string DevKey => Instance.devKey;
        public static string AppID => Instance.appID;
        public static string UWPAppID => Instance.uwpAppID;
        public static string MacOSAppID => Instance.macOSAppID;
        public static bool IsDebug => Instance.isDebug;
        public static bool IsDebugAdRevenue => Instance.isDebugAdRevenue;
        public static bool GetConversionData => Instance.getConversionData;
    }
}