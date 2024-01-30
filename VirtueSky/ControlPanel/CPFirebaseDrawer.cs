using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPFirebaseDrawer
    {
        private static bool isShowInstallRemoteConfig;
        private static bool isShowInstallAnalytic;

        public static void OnDrawFirebase(Rect position, ref StatePanelControl statePanelControl)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            DrawRemoteConfig(position);
            DrawAnalytic(position, statePanelControl);
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            EditorGUILayout.HelpBox(
                "Add scripting define symbols: \n \"VIRTUESKY_FIREBASE\" for Firebase App, \n \"VIRTUESKY_FIREBASE_REMOTECONFIG\" for Firebase Remote Config, \n \"VIRTUESKY_FIREBASE_ANALYTIC\" for Firebase Analytic \n to use",
                MessageType.Info);
            if (GUILayout.Button("Open Scripting Define Symbols tab to add"))
            {
                statePanelControl = StatePanelControl.ScriptDefineSymbols;
            }

            GUILayout.EndVertical();
        }

        static void DrawRemoteConfig(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.Label("FIREBASE REMOTE CONFIG", EditorStyles.boldLabel);
            GUILayout.Space(10);
            isShowInstallRemoteConfig = GUILayout.Toggle(isShowInstallRemoteConfig, "Install Firebase Remote Config");
            GUILayout.Space(10);
            if (isShowInstallRemoteConfig)
            {
                if (GUILayout.Button("Install Firebase Remote Config and Dependencies"))
                {
                    RegistryManager.Add(ConstantPackage.PackageNameGGExternalDependencyManager,
                        ConstantPackage.MaxVersionGGExternalDependencyManager);
                    RegistryManager.Add(ConstantPackage.PackageNameFireBaseApp,
                        ConstantPackage.MaxVersionFireBaseApp);
                    RegistryManager.Add(ConstantPackage.PackageNameFireBaseRemoveConfig,
                        ConstantPackage.MaxVersionFireBaseRemoveConfig);
                    RegistryManager.Resolve();
                }
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
        }

        static void DrawAnalytic(Rect position, StatePanelControl statePanelControl)
        {
            GUILayout.Space(10);
            GUILayout.Label("FIREBASE ANALYTIC", EditorStyles.boldLabel);
            GUILayout.Space(10);
            isShowInstallAnalytic = GUILayout.Toggle(isShowInstallAnalytic, "Install Firebase Analytic");
            GUILayout.Space(10);
            if (isShowInstallAnalytic)
            {
                if (GUILayout.Button("Install Firebase Analytic and Dependencies"))
                {
                    RegistryManager.Add(ConstantPackage.PackageNameGGExternalDependencyManager,
                        ConstantPackage.MaxVersionGGExternalDependencyManager);
                    RegistryManager.Add(ConstantPackage.PackageNameFireBaseApp,
                        ConstantPackage.MaxVersionFireBaseApp);
                    RegistryManager.Add(ConstantPackage.PackageNameFireBaseAnalytics,
                        ConstantPackage.MaxVersionFireBaseAnalytics);
                    RegistryManager.Resolve();
                }

                GUILayout.Space(10);
                Handles.DrawAAPolyLine(2f, new Vector3(240, GUILayoutUtility.GetLastRect().y + 10),
                    new Vector3(position.width - 30, GUILayoutUtility.GetLastRect().y + 10));
            }


            GUILayout.Space(10);

            if (GUILayout.Button("Create Log Event Firebase No Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseNoParam();
            }

            if (GUILayout.Button("Create Log Event Firebase 1 Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseOneParam();
            }

            if (GUILayout.Button("Create Log Event Firebase 2 Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseTwoParam();
            }

            if (GUILayout.Button("Create Log Event Firebase 3 Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseThreeParam();
            }

            if (GUILayout.Button("Create Log Event Firebase 4 Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseFourParam();
            }

            if (GUILayout.Button("Create Log Event Firebase 5 Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseFiveParam();
            }

            if (GUILayout.Button("Create Log Event Firebase 6 Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseSixParam();
            }
        }
    }
}