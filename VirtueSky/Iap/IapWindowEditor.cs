#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Iap
{
    public class IapWindowEditor : EditorWindow
    {
        [MenuItem("Sunflower/Iap %W", false)]
        public static void MenuOpenAdSettings()
        {
            var iapSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap");
            Selection.activeObject = iapSetting;
            EditorUtility.FocusProjectWindow();
        }
    }
}
#endif