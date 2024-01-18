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
        private IapSetting _iapSetting;
        private Vector2 _scrollPosition;
        private Editor _editor;

        private bool isSetupTheme = false;

        [MenuItem("Sunflower/Iap/IapSetting &2", false)]
        public static void OpenIapSettingsWindows()
        {
            var iapSetting =
                CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap");
            IapWindowEditor window = GetWindow<IapWindowEditor>("Iap Settings");
            window._iapSetting = iapSetting;
            if (window == null)
            {
                Debug.LogError("Couldn't open the iap settings window!");
                return;
            }

            window.minSize = new Vector2(300, 0);
            window.Show();
            EditorGUIUtility.PingObject(iapSetting);
            // Selection.activeObject = iapSetting;
            // EditorUtility.FocusProjectWindow();
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height),
                GameDataEditor.ColorBackgroundRectWindowSunflower.ToColor());
            GUI.contentColor = GameDataEditor.ColorTextContentWindowSunflower.ToColor();
            GUI.backgroundColor = GameDataEditor.ColorContentWindowSunflower.ToColor();
            if (_editor == null)
            {
                _editor = Editor.CreateEditor(_iapSetting);
            }

            if (_editor == null)
            {
                EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                    MessageType.Error);
                return;
            }

            _editor.DrawHeader();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(6, 3, 3, 3) });
            _editor.OnInspectorGUI();
            GUILayout.Space(10);
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(3, new Vector3(0, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            // isSetupTheme = GUILayout.Toggle(isSetupTheme, "Setup Theme");
            // if (isSetupTheme)
            // {
            //     ColorContent =
            //         (CustomColor)EditorGUILayout.EnumPopup("Color Content:", ColorContent);
            //     ColorTextContent =
            //         (CustomColor)EditorGUILayout.EnumPopup("Color Text Content:", ColorTextContent);
            //     ColorBackgroundRect =
            //         (CustomColor)EditorGUILayout.EnumPopup("Color Background:",
            //             ColorBackgroundRect);
            //     GUILayout.Space(10);
            //     if (GUILayout.Button("Theme Default"))
            //     {
            //         ColorContent = CustomColor.Bright;
            //         ColorTextContent = CustomColor.Gold;
            //         ColorBackgroundRect = CustomColor.DarkSlateGray;
            //     }
            // }

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }


        // [MenuItem("Sunflower/Iap/Iap Data Variable")]
        // public static void CreateVariableTransform()
        // {
        //     CreateAsset.CreateScriptableAssetsOnlyName<IapDataVariable>("/Iap/Products");
        // }

        [MenuItem("Sunflower/Iap/Iap Purchase Product Event")]
        public static void CreateIapProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIapProduct>("/Iap", "iap_purchase_product");
        }

        [MenuItem("Sunflower/Iap/Iap Is Purchase Product Event")]
        public static void CreateIsPurchaseProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIsPurchaseProduct>("/Iap",
                "iap_is_purchase_product");
        }

        [MenuItem("Sunflower/Iap/Iap Tracking Revenue Event")]
        public static void CreateIapTrackingRevenueEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIapTrackingRevenue>("/Iap",
                "iap_tracking_revenue_event");
        }

        // private CustomColor ColorContent
        // {
        //     get => (CustomColor)EditorPrefs.GetInt("ColorContent_Iap", (int)CustomColor.Bright);
        //     set => EditorPrefs.SetInt("ColorContent_Iap", (int)value);
        // }
        //
        // private CustomColor ColorTextContent
        // {
        //     get => (CustomColor)EditorPrefs.GetInt("ColorTextContent_Iap", (int)CustomColor.Gold);
        //     set => EditorPrefs.SetInt("ColorTextContent_Iap", (int)value);
        // }
        //
        // private CustomColor ColorBackgroundRect
        // {
        //     get => (CustomColor)EditorPrefs.GetInt("ColorBackground_Iap",
        //         (int)CustomColor.DarkSlateGray);
        //     set => EditorPrefs.SetInt("ColorBackground_Iap", (int)value);
        // }
#endif
    }
}
#endif