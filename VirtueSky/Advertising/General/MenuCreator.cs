#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Ads
{
    public class MenuCreator : EditorWindow
    {
        [MenuItem("Sunflower/AdSetting %E", false)]
        public static void MenuOpenAdSettings()
        {
            var adSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads");
            Selection.activeObject = adSetting;
            EditorUtility.FocusProjectWindow();
        }
    }
}
#endif