using UnityEditor;
using UnityEngine;
using VirtueSky.Iap;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPIapDrawer
    {
        public static void OnDrawIap(Rect position, ref StatePanelControl statePanelControl)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open AdSetting (Alt+2 / Option+2)"))
            {
#if VIRTUESKY_IAP
                IapWindowEditor.OpenIapSettingsWindows();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
            }

            if (GUILayout.Button("Create Iap Purchase Product Event"))
            {
#if VIRTUESKY_IAP
                IapWindowEditor.CreateIapProductEvent();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
            }

            if (GUILayout.Button("Create Iap Purchase Product Event"))
            {
#if VIRTUESKY_IAP
                IapWindowEditor.CreateIsPurchaseProductEvent();

#else
                Debug.LogError("Add scripting define symbols ( VIRTUESKY_IAP ) to use IAP");
#endif
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("INSTALL IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Import In App Purchasing"))
            {
                string packageName = "com.unity.purchasing";
                (bool isInstall, string version) =
                    RegistryManager.IsInstalled(packageName);
                if (isInstall && version == ConstantPackage.MaxVersionInAppPurchase)
                {
                    Debug.Log("In App Purchase Is Installed and Max Version");
                }
                else if (version != ConstantPackage.MaxVersionInAppPurchase)
                {
                    RegistryManager.Remove(packageName);
                    RegistryManager.Add(packageName,
                        ConstantPackage.MaxVersionInAppPurchase);
                    RegistryManager.Resolve();
                }
                else
                {
                    RegistryManager.Add(packageName,
                        ConstantPackage.MaxVersionInAppPurchase);
                    RegistryManager.Resolve();
                }
            }

            if (GUILayout.Button("Remove In App Purchasing"))
            {
                string packageName = "com.unity.purchasing";
                (bool isInstall, string version) =
                    RegistryManager.IsInstalled(packageName);
                if (isInstall)
                {
                    RegistryManager.Remove(packageName);
                    RegistryManager.Resolve();
                }
            }

            GUILayout.Space(10);
            EditorGUILayout.HelpBox(
                "Add scripting define symbols \"VIRTUESKY_IAP\" to use IAP",
                MessageType.Info);
            if (GUILayout.Button("Open Scripting Define Symbols tab to add"))
            {
                statePanelControl = StatePanelControl.ScriptDefineSymbols;
            }

            GUILayout.EndVertical();
        }
    }
}