using UnityEditor;
using UnityEngine;

#if VIRTUESKY_IAP
using VirtueSky.Iap;
#endif

using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPIapDrawer
    {
#if VIRTUESKY_IAP
        private static IapSetting _iapSetting;
#endif
        private static UnityEditor.Editor _editor;
        private static Vector2 scroll = Vector2.zero;

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
#if VIRTUESKY_IAP
            _iapSetting = CreateAsset.GetScriptableAsset<VirtueSky.Iap.IapSetting>();
            _editor = UnityEditor.Editor.CreateEditor(_iapSetting);
#endif
        }

        public static void OnDrawIap(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
#if VIRTUESKY_IAP
            if (_iapSetting == null)
            {
                if (GUILayout.Button("Create IAP Setting"))
                {
                    _iapSetting =
                        CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Iap.IapSetting>("/Iap", "", false);
                    Init();
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
#else
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_IAP}\" to use IAP",
                MessageType.Warning);
#endif
            GUILayout.Space(10);
            // Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
            //     new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            CPUtility.GuiLine(2);
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
            // Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
            //     new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("INSTALL PACKAGE IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install In App Purchasing", "Remove In App Purchasing",
                ConstantPackage.PackageNameInAppPurchase, ConstantPackage.MaxVersionInAppPurchase);
            GUILayout.Space(10);
            // Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
            //     new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);

#if !VIRTUESKY_IAP
            EditorGUILayout.HelpBox(
                    $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_IAP}\" to use IAP",
                    MessageType.Info);
#endif

            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_IAP);
#if VIRTUESKY_IAP
            GUILayout.Space(10);
            // Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
            //     new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("PING IAP SETTING", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Ping"))
            {
                if (_iapSetting == null)
                {
                    Debug.LogError("IapSetting have not been created yet");
                }
                else
                {
                    EditorGUIUtility.PingObject(_iapSetting);
                    Selection.activeObject = _iapSetting;
                }
            }
#endif
            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}