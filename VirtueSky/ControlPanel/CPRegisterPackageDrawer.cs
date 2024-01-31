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
        private static bool isShowAddPackage = false;
        private static bool isShowRemovePackage = false;

        public static void OnDrawRegisterPackageByManifest(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("REGISTER PACKAGE", EditorStyles.boldLabel);
            GUILayout.Space(10);

            //  GUILayout.Label("Add Some Packages", EditorStyles.boldLabel);
            isShowAddPackage = GUILayout.Toggle(isShowAddPackage, "Add Some Packages");
            if (isShowAddPackage)
            {
                scrollPositionAddPackage =
                    EditorGUILayout.BeginScrollView(scrollPositionAddPackage,
                        GUILayout.Height(150));
                DrawButtonAddPackage();
                EditorGUILayout.EndScrollView();
            }


            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            // GUILayout.Label("Remove Package", EditorStyles.boldLabel);
            isShowRemovePackage = GUILayout.Toggle(isShowRemovePackage, "Remove Some Packages");
            if (isShowRemovePackage)
            {
                scrollPositionRemovePackage =
                    EditorGUILayout.BeginScrollView(scrollPositionRemovePackage,
                        GUILayout.Height(150));
                DrawButtonRemovePackage();
                EditorGUILayout.EndScrollView();
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
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

        static void DrawButtonAddPackage()
        {
            if (GUILayout.Button("Install Firebase App"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameFireBaseApp,
                    ConstantPackage.MaxVersionFireBaseApp);
            }

            if (GUILayout.Button("Install Firebase Remote Config"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameFireBaseRemoveConfig,
                    ConstantPackage.MaxVersionFireBaseRemoveConfig);
            }

            if (GUILayout.Button("Install Firebase Analytics"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameFireBaseAnalytics,
                    ConstantPackage.MaxVersionFireBaseAnalytics);
            }

            if (GUILayout.Button("Install Firebase Crashlytics"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameFireBaseCrashlytics,
                    ConstantPackage.MaxVersionFireBaseCrashlytics);
            }

            if (GUILayout.Button("Install Firebase Database"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameFireBaseDatabase,
                    ConstantPackage.MaxVersionFireBaseDatabase);
            }

            if (GUILayout.Button("Install Google External Dependency Manager"))
            {
                RegistryManager.AddOverrideVersion(
                    ConstantPackage.PackageNameGGExternalDependencyManager,
                    ConstantPackage.MaxVersionGGExternalDependencyManager);
            }

            if (GUILayout.Button("Install Adjust"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameAdjust,
                    ConstantPackage.MaxVersionAdjust);
            }

            if (GUILayout.Button("Install In App Purchasing"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameInAppPurchase,
                    ConstantPackage.MaxVersionInAppPurchase);
            }

            if (GUILayout.Button("Install AppsFlyer"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameAppFlyer,
                    ConstantPackage.MaxVersionAppFlyer);
                RegistryManager.AddOverrideVersion(
                    ConstantPackage.PackageNameAppFlyerRevenueGeneric,
                    ConstantPackage.MaxVersionAppFlyerRevenueGeneric);
            }

            if (GUILayout.Button("Install Google Play Review"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameGGPlayReview,
                    ConstantPackage.MaxVersionGGPlayReview);
            }

            if (GUILayout.Button("Install Google Play Core"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameGGPlayCore,
                    ConstantPackage.MaxVersionGGPlayCore);
            }

            if (GUILayout.Button("Install Google Play Common"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameGGPlayCommon,
                    ConstantPackage.MaxVersionGGPlayCommon);
            }

            if (GUILayout.Button("Install Android App Bundle"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameGGAndroidAppBundle,
                    ConstantPackage.MaxVersionAndroidAppBundle);
            }

            if (GUILayout.Button("Install Newtonsoft.Json"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameNewtonsoftJson,
                    ConstantPackage.MaxVersionNewtonsoftJson);
            }

            if (GUILayout.Button("Install PlayFab"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNamePlayFab,
                    ConstantPackage.MaxVersionPlayFab);
            }

            if (GUILayout.Button("Install Coffee UI Effect"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameCoffeeUIEffect,
                    ConstantPackage.MaxVersionCoffeeUIEffect);
            }

            if (GUILayout.Button("Install Coffee UI Particle"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameCoffeeUIParticle,
                    ConstantPackage.MaxVersionCoffeeUIParticle);
            }

            if (GUILayout.Button("Install iOS 14 Advertising Support"))
            {
                RegistryManager.AddOverrideVersion(
                    ConstantPackage.PackageNameIOS14AdvertisingSupport,
                    ConstantPackage.MaxVersionIOS14AdvertisingSupport);
            }

            if (GUILayout.Button("Install Spine"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameSpineCsharp,
                    ConstantPackage.MaxVersionSpineCsharp);
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameSpineUnity,
                    ConstantPackage.MaxVersionSpineUnity);
            }

            if (GUILayout.Button("Install Apple Sign In"))
            {
                RegistryManager.AddOverrideVersion(ConstantPackage.PackageNameAppleSignIn,
                    ConstantPackage.MaxVersionAppleSignIn);
            }

            if (GUILayout.Button("Install Google Play Game Service"))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/google-play-game.unitypackage"), false);
            }
        }

        static void DrawButtonRemovePackage()
        {
            if (GUILayout.Button("Remove Firebase App"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameFireBaseApp);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Firebase Remote Config"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameFireBaseRemoveConfig);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Firebase Analytics"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameFireBaseAnalytics);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Firebase Crashlytics"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameFireBaseCrashlytics);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Firebase Database"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameFireBaseDatabase);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Google External Dependency Manager"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameGGExternalDependencyManager);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Adjust"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameAdjust);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove In App Purchasing"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameInAppPurchase);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove AppsFlyer"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameAppFlyer);
                RegistryManager.Remove(ConstantPackage.PackageNameAppFlyerRevenueGeneric);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Google Play Review"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameGGPlayReview);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Google Play Core"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameGGPlayCore);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Google Play Common"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameGGPlayCommon);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Android App Bundle"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameGGAndroidAppBundle);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Newtonsoft.Json"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameNewtonsoftJson);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove PlayFab"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNamePlayFab);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Coffee UI Effect"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameCoffeeUIEffect);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Coffee UI Particle"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameCoffeeUIParticle);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove iOS 14 Advertising Support"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameIOS14AdvertisingSupport);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Spine"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameSpineCsharp);
                RegistryManager.Remove(ConstantPackage.PackageNameSpineUnity);
                RegistryManager.Resolve();
            }

            if (GUILayout.Button("Remove Apple Sign In"))
            {
                RegistryManager.Remove(ConstantPackage.PackageNameAppleSignIn);
                RegistryManager.Resolve();
            }
        }
    }
}