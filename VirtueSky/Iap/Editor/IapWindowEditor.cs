#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Iap
{
    public class IapWindowEditor : EditorWindow
    {
#if VIRTUESKY_IAP
        // private IapSetting _iapSetting;
        // private Vector2 _scrollPosition;
        // private Editor _editor;
        //
        // private bool isSetupTheme = false;

        // [MenuItem("Sunflower/Iap/IapSetting &2", false)]
        // public static void OpenIapSettingsWindows()
        // {
        //     var iapSetting =
        //         CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap");
        //     IapWindowEditor window = GetWindow<IapWindowEditor>("Iap Settings");
        //     window._iapSetting = iapSetting;
        //     if (window == null)
        //     {
        //         Debug.LogError("Couldn't open the iap settings window!");
        //         return;
        //     }
        //
        //     window.minSize = new Vector2(300, 0);
        //     window.Show();
        //     EditorGUIUtility.PingObject(iapSetting);
        //     // Selection.activeObject = iapSetting;
        //     // EditorUtility.FocusProjectWindow();
        // }

        // private void OnGUI()
        // {
        //     EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height),
        //         GameDataEditor.ColorBackgroundRectWindowSunflower.ToColor());
        //     GUI.contentColor = GameDataEditor.ColorTextContentWindowSunflower.ToColor();
        //     GUI.backgroundColor = GameDataEditor.ColorContentWindowSunflower.ToColor();
        //     if (_editor == null)
        //     {
        //         _editor = Editor.CreateEditor(_iapSetting);
        //     }
        //
        //     if (_editor == null)
        //     {
        //         EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
        //             MessageType.Error);
        //         return;
        //     }
        //
        //     //  _editor.DrawHeader();
        //     _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
        //     EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(6, 3, 3, 3) });
        //     _editor.OnInspectorGUI();
        //     GUILayout.Space(10);
        //     Handles.color = Color.black;
        //     Handles.DrawAAPolyLine(3, new Vector3(0, GUILayoutUtility.GetLastRect().y + 10),
        //         new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
        //     GUILayout.Space(10);
        //
        //
        //     EditorGUILayout.EndVertical();
        //     EditorGUILayout.EndScrollView();
        // }

        // [MenuItem("Sunflower/Iap/Iap Data Variable")]
        // public static void CreateVariableTransform()
        // {
        //     CreateAsset.CreateScriptableAssetsOnlyName<IapDataVariable>("/Iap/Products");
        // }

        // [MenuItem("Sunflower/Iap/Iap Purchase Product Event")]
        public static void CreateIapProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventPurchaseProduct>("/Iap", "iap_purchase_product_event");
        }

        //  [MenuItem("Sunflower/Iap/Iap Is Purchase Product Event")]
        public static void CreateIsPurchaseProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIsPurchaseProduct>("/Iap",
                "iap_is_purchase_product_event");
        }

        // [MenuItem("Sunflower/Iap/Iap Tracking Revenue Event")]
        public static void CreateIapTrackingPurchaseProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIapTrackingPurchaseProduct>("/Iap",
                "iap_tracking_purchase_product_event");
        }

        public static void CreateIapLocalizedPriceProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventLocalizedPriceProduct>("/Iap",
                "iap_localized_price_product_event");
        }
#endif
    }
}
#endif