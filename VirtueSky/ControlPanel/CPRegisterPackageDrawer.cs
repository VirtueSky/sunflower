using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPRegisterPackageDrawer
    {
        private static Vector2 scrollPositionFileManifest = Vector2.zero;
        private static Vector2 scrollPositionAddPackage = Vector2.zero;
        private static Vector2 scrollPositionRemovePackage = Vector2.zero;
        private static Vector2 scrollPositionAddSomePackage = Vector2.zero;
        private static bool isShowAddPackage = false;
        private static bool isShowRemovePackage = false;

        public static void OnDrawRegisterPackageByManifest(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("REGISTER SOME PACKAGE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            scrollPositionAddSomePackage =
                EditorGUILayout.BeginScrollView(scrollPositionAddSomePackage, GUILayout.Height(150));
            DrawButtonAddSomePackage();
            EditorGUILayout.EndScrollView();
            GUILayout.Space(10);
            CPUtility.DrawLineLastRectY(3, ConstantControlPanel.POSITION_X_START_CONTENT, position.width);
            GUILayout.Space(10);
            GUILayout.Label("Manifest.json", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Resolve Package"))
            {
                RegistryManager.Resolve();
            }

            scrollPositionFileManifest =
                EditorGUILayout.BeginScrollView(scrollPositionFileManifest,
                    GUILayout.Height(250));
            string manifestContent = EditorGUILayout.TextArea(
                System.IO.File.ReadAllText(FileExtension.ManifestPath),
                GUILayout.ExpandHeight(true));
            RegistryManager.WriteAllManifestContent(manifestContent);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        static void DrawButtonAddSomePackage()
        {
            CPUtility.DrawButtonInstallPackage("Install Firebase App", "Remove Firebase App",
                ConstantPackage.PackageNameFirebaseApp, ConstantPackage.MaxVersionFirebaseApp);
            CPUtility.DrawButtonInstallPackage("Install Firebase Support Ios", "Remove Firebase Support Ios",
                ConstantPackage.PackageNameFirebaseSupportIos, ConstantPackage.MaxVersionFirebaseSupportIos);
            CPUtility.DrawButtonInstallPackage("Install Firebase Remote Config", "Remove Firebase Remote Config",
                ConstantPackage.PackageNameFirebaseRemoteConfig, ConstantPackage.MaxVersionFirebaseRemoteConfig);
            CPUtility.DrawButtonInstallPackage("Install Firebase Analytics", "Remove Firebase Analytics",
                ConstantPackage.PackageNameFirebaseAnalytics, ConstantPackage.MaxVersionFirebaseAnalytics);
            CPUtility.DrawButtonInstallPackage("Install Firebase Crashlytics", "Remove Firebase Crashlytics",
                ConstantPackage.PackageNameFirebaseCrashlytics, ConstantPackage.MaxVersionFirebaseCrashlytics);
            CPUtility.DrawButtonInstallPackage("Install Firebase Database", "Remove Firebase Database",
                ConstantPackage.PackageNameFirebaseDatabase, ConstantPackage.MaxVersionFirebaseDatabase);
            CPUtility.DrawButtonInstallPackage("Install Firebase Auth", "Remove Firebase Auth",
                ConstantPackage.PackageNameFirebaseAuth, ConstantPackage.MaxVersionFirebaseAuth);
            CPUtility.DrawButtonInstallPackage("Install Google External Dependency Manager",
                "Remove Google External Dependency Manager",
                ConstantPackage.PackageNameGGExternalDependencyManager,
                ConstantPackage.MaxVersionGGExternalDependencyManager);
            CPUtility.DrawButtonInstallPackage("Install Adjust", "Remove Adjust",
                ConstantPackage.PackageNameAdjust, ConstantPackage.MaxVersionAdjust);
            CPUtility.DrawButtonInstallPackage("Install In App Purchasing", "Remove In App Purchasing",
                ConstantPackage.PackageNameInAppPurchase, ConstantPackage.MaxVersionInAppPurchase);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer", "Remove AppsFlyer",
                ConstantPackage.PackageNameAppFlyer, ConstantPackage.MaxVersionAppFlyer);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer Revenue Generic", "Remove AppsFlyer Revenue Generic",
                ConstantPackage.PackageNameAppFlyerRevenueGeneric, ConstantPackage.MaxVersionAppFlyerRevenueGeneric);
            CPUtility.DrawButtonInstallPackage("Install Google Play Review", "Remove Google Play Review",
                ConstantPackage.PackageNameGGPlayReview, ConstantPackage.MaxVersionGGPlayReview);
            CPUtility.DrawButtonInstallPackage("Install Google Play Core", "Remove Google Play Core",
                ConstantPackage.PackageNameGGPlayCore, ConstantPackage.MaxVersionGGPlayCore);
            CPUtility.DrawButtonInstallPackage("Install Google Play Common", "Remove Google Play Common",
                ConstantPackage.PackageNameGGPlayCommon, ConstantPackage.MaxVersionGGPlayCommon);
            CPUtility.DrawButtonInstallPackage("Install Android App Bundle", "Remove Android App Bundle",
                ConstantPackage.PackageNameGGAndroidAppBundle, ConstantPackage.MaxVersionAndroidAppBundle);
            CPUtility.DrawButtonInstallPackage("Install Newtonsoft.Json", "Remove Newtonsoft.Json",
                ConstantPackage.PackageNameNewtonsoftJson, ConstantPackage.MaxVersionNewtonsoftJson);
            CPUtility.DrawButtonInstallPackage("Install PlayFab", "Remove PlayFab", ConstantPackage.PackageNamePlayFab,
                ConstantPackage.MaxVersionPlayFab);
            CPUtility.DrawButtonInstallPackage("Install Coffee UI Effect", "Remove Coffee UI Effect",
                ConstantPackage.PackageNameCoffeeUIEffect, ConstantPackage.MaxVersionCoffeeUIEffect);
            CPUtility.DrawButtonInstallPackage("Install Coffee UI Particle", "Remove Coffee UI Particle",
                ConstantPackage.PackageNameCoffeeUIParticle, ConstantPackage.MaxVersionCoffeeUIParticle);
            CPUtility.DrawButtonInstallPackage("Install iOS 14 Advertising Support",
                "Remove iOS 14 Advertising Support", ConstantPackage.PackageNameIOS14AdvertisingSupport,
                ConstantPackage.MaxVersionIOS14AdvertisingSupport);
            CPUtility.DrawButtonInstallPackage("Install Spine Csharp", "Remove Spine Csharp",
                ConstantPackage.PackageNameSpineCsharp, ConstantPackage.MaxVersionSpineCsharp);
            CPUtility.DrawButtonInstallPackage("Install Spine Unity", "Remove Spine Unity",
                ConstantPackage.PackageNameSpineUnity, ConstantPackage.MaxVersionSpineUnity);
            CPUtility.DrawButtonInstallPackage("Install Apple Sign In", "Remove Apple Sign In",
                ConstantPackage.PackageNameAppleSignIn, ConstantPackage.MaxVersionAppleSignIn);
            // CPUtility.DrawButtonInstallPackage("Install Animancer", "Remove Animancer",
            //     ConstantPackage.PackageNameAnimancer, ConstantPackage.MaxVersionAnimancer);
            CPUtility.DrawButtonInstallPackage("Install Mobile Notifications", "Remove Mobile Notifications",
                ConstantPackage.PackageNameMobileNotification, ConstantPackage.MaxVersionMobileNotification);
            CPUtility.DrawButtonInstallPackage("Install Addressables", "Remove Addressables",
                ConstantPackage.PackageNameAddressables, ConstantPackage.MaxVersionAddressables);
            if (GUILayout.Button("Install Google Play Game Service", GUILayout.Width(400)))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/google-play-game.unitypackage"), false);
            }
        }
    }
}