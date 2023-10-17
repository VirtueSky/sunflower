#if UNITY_EDITOR
using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Iap
{
    public class IapWindowEditor : EditorWindow
    {
#if VIRTUESKY_IAP
        [MenuItem("Sunflower/Iap/IapSetting %W", false)]
        public static void MenuOpenAdSettings()
        {
            var iapSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap");
            Selection.activeObject = iapSetting;
            EditorUtility.FocusProjectWindow();
        }

        [MenuItem("Sunflower/Iap/Iap Data Variable")]
        public static void CreateVariableTransform()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IapDataVariable>("/Iap/Products");
        }

        [MenuItem("Sunflower/Iap/Iap Purchase Product Event")]
        public static void CreateIapProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIapProduct>("/Iap", "iap_purchase_product");
        }
#endif
    }
}
#endif