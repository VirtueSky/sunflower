#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.EditorUtils;

namespace VirtueSky.Ads
{
    public class MenuCreator : EditorWindow
    {
        [MenuItem("Sunflower/AdSetting %E", false)]
        public static void MenuOpenAdSettings()
        {
            var adSetting = ScriptableSetting.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads");
            Selection.activeObject = adSetting;
            EditorUtility.FocusProjectWindow();
        }
    }
}
#endif