using UnityEditor;
using UnityEngine;
using VirtueSky.Iap;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPIapDrawer
    {
        private static IapSetting _iapSetting;
        private static UnityEditor.Editor _editor;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null)
            {
                _editor = null;
            }

            _iapSetting = CreateAsset.GetScriptableAsset<VirtueSky.Iap.IapSetting>();
            _editor = UnityEditor.Editor.CreateEditor(_iapSetting);
        }

        public static void OnDrawIap(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);


            if (_iapSetting == null)
            {
                if (GUILayout.Button("Create IAP Setting"))
                {
#if VIRTUESKY_IAP
                    _iapSetting =
                        CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap", "", false);
                    Init();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
                }
            }
            else
            {
                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                        MessageType.Error);
                    return;
                }
                else
                {
                    _editor.OnInspectorGUI();
                }
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);

            if (GUILayout.Button("Create Iap Purchase Product Event"))
            {
#if VIRTUESKY_IAP
                IapWindowEditor.CreateIapProductEvent();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
            }

            if (GUILayout.Button("Create Iap Is Purchase Product Event"))
            {
#if VIRTUESKY_IAP
                IapWindowEditor.CreateIsPurchaseProductEvent();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
            }

            if (GUILayout.Button("Create Iap Tracking Revenue Event"))
            {
#if VIRTUESKY_IAP
                IapWindowEditor.CreateIapTrackingRevenueEvent();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("INSTALL PACKAGE IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install In App Purchasing", "Remove In App Purchasing",
                ConstantPackage.PackageNameInAppPurchase, ConstantPackage.MaxVersionInAppPurchase);
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_IAP}\" to use IAP",
                MessageType.Info);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_IAP);


            GUILayout.EndVertical();
        }
    }
}