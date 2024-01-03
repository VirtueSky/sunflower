#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Iap
{
    public class IapWindowEditor : EditorWindow
    {
#if VIRTUESKY_IAP
        private IapSetting _iapSetting;
        private Vector2 _scrollPosition;
        private Editor _editor;

        [MenuItem("Sunflower/Iap/IapSetting %#W", false)]
        public static void MenuOpenAdSettings()
        {
            var iapSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap");
            IapWindowEditor window = GetWindow<IapWindowEditor>("Iap Settings");
            window._iapSetting = iapSetting;
            if (window == null)
            {
                Debug.LogError("Couldn't open the iap settings window!");
                return;
            }

            window.minSize = new Vector2(275, 0);
            window.Show();
            // Selection.activeObject = iapSetting;
            // EditorUtility.FocusProjectWindow();
        }

        private void OnGUI()
        {
            if (_editor == null)
            {
                _editor = Editor.CreateEditor(_iapSetting);
            }

            if (_editor == null)
            {
                EditorGUILayout.HelpBox("Couldn't create the settings resources editor.", MessageType.Error);
                return;
            }

            _editor.DrawHeader();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(6, 3, 3, 3) });
            _editor.OnInspectorGUI();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
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

        [MenuItem("Sunflower/Iap/Iap Is Purchase Product Event")]
        public static void CreateIsPurchaseProductEvent()
        {
            CreateAsset.CreateScriptableAssets<EventIsPurchaseProduct>("/Iap", "iap_is_purchase_product");
        }
#endif
    }
}
#endif