using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [EditorIcon("icon_scriptable"), HideMonoScript]
    public class AppsFlyerSetting : ScriptableObject
    {
        [SerializeField] private string devKey;
        [SerializeField] private string appID;
        [SerializeField] private string uwpAppID;
        [SerializeField] private string macOSAppID;
        [SerializeField] private bool getConversionData;
        [SerializeField] private bool isDebug;


        public string DevKey => devKey;
        public string AppID => appID;
        public string UWPAppID => uwpAppID;
        public string MacOSAppID => macOSAppID;
        public bool IsDebug => isDebug;
        public bool GetConversionData => getConversionData;
    }
}