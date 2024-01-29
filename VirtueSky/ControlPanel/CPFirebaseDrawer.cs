using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPFirebaseDrawer
    {
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

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
        }

        static void DrawAnalytic(Rect position, StatePanelControl statePanelControl)
        {
            GUILayout.Space(10);
            GUILayout.Label("FIREBASE ANALYTIC", EditorStyles.boldLabel);
            GUILayout.Space(10);
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
            GUILayout.Space(10);
            if (GUILayout.Button("Log Event Firebase Analytic"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseAnalytic();
            }

            if (GUILayout.Button("Log Event Firebase Analytic No Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseAnalyticNoParam();
            }

            if (GUILayout.Button("Log Event Firebase Analytic Has Param"))
            {
                FirebaseWindowEditor.CreateLogEventFirebaseAnalyticHasParam();
            }
        }
    }
}