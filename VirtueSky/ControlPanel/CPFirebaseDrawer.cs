using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPFirebaseDrawer
    {
        private static bool isShowInstallRemoteConfig;
        private static bool isShowInstallAnalytic;
        private static Vector2 scroll = Vector2.zero;

        public static void OnDrawFirebase(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            scroll = EditorGUILayout.BeginScrollView(scroll);
            DrawRemoteConfig(position);
            DrawAnalytic(position);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
#if !VIRTUESKY_FIREBASE || !VIRTUESKY_FIREBASE_REMOTECONFIG
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: \n {ConstantDefineSymbols.VIRTUESKY_FIREBASE} for Firebase App,\n {ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG} for Firebase Remote Config to use",
                MessageType.Info);
#endif

            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_REMOTECONFIG);
#if !VIRTUESKY_FIREBASE_ANALYTIC
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC} for Firebase Analytic to use",
                MessageType.Info);
#endif
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_FIREBASE_ANALYTIC);
            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        static void DrawRemoteConfig(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.Label("FIREBASE REMOTE CONFIG", EditorStyles.boldLabel);
            GUILayout.Space(10);
            isShowInstallRemoteConfig =
                GUILayout.Toggle(isShowInstallRemoteConfig, "Install Firebase Remote Config And Dependency");
            GUILayout.Space(10);
            if (isShowInstallRemoteConfig)
            {
                CPUtility.DrawButtonInstallPackage("Install Firebase Remote Config", "Remove Firebase Remote Config",
                    ConstantPackage.PackageNameFirebaseRemoveConfig, ConstantPackage.MaxVersionFirebaseRemoveConfig);
                CPUtility.DrawButtonInstallPackage("Install Firebase App", "Remove Firebase App",
                    ConstantPackage.PackageNameFirebaseApp, ConstantPackage.MaxVersionFirebaseApp);
                CPUtility.DrawButtonInstallPackage("Install Google External Dependency Manager",
                    "Remove Google External Dependency Manager",
                    ConstantPackage.PackageNameGGExternalDependencyManager,
                    ConstantPackage.MaxVersionGGExternalDependencyManager);
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
        }

        static void DrawAnalytic(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.Label("FIREBASE ANALYTIC", EditorStyles.boldLabel);
            GUILayout.Space(10);
            isShowInstallAnalytic = GUILayout.Toggle(isShowInstallAnalytic, "Install Firebase Analytic And Dependency");
            GUILayout.Space(10);
            if (isShowInstallAnalytic)
            {
                CPUtility.DrawButtonInstallPackage("Install Firebase Analytics", "Remove Firebase Analytics",
                    ConstantPackage.PackageNameFirebaseAnalytics, ConstantPackage.MaxVersionFirebaseAnalytics);
                CPUtility.DrawButtonInstallPackage("Install Firebase App", "Remove Firebase App",
                    ConstantPackage.PackageNameFirebaseApp, ConstantPackage.MaxVersionFirebaseApp);
                CPUtility.DrawButtonInstallPackage("Install Google External Dependency Manager",
                    "Remove Google External Dependency Manager",
                    ConstantPackage.PackageNameGGExternalDependencyManager,
                    ConstantPackage.MaxVersionGGExternalDependencyManager);
                GUILayout.Space(10);
                CPUtility.GuiLine(2);
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