using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAppsFlyerDrawer
    {
        public static void OnDrawAppsFlyer()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("APPSFLYER", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer", "Remove AppsFlyer",
                ConstantPackage.PackageNameAppFlyer, ConstantPackage.MaxVersionAppFlyer);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer Revenue Generic", "Remove AppsFlyer Revenue Generic",
                ConstantPackage.PackageNameAppFlyerRevenueGeneric, ConstantPackage.MaxVersionAppFlyerRevenueGeneric);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
#if !VIRTUESKY_APPSFLYER
                EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_APPSFLYER} for AppsFlyer to use",
                MessageType.Info);
#endif
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPSFLYER);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("APPSFLYER TRACKING", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer No Param"))
            {
                TrackingWindowEditor.CreateTrackingAfNoParam();
            }

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer 1 Param"))
            {
                TrackingWindowEditor.CreateTrackingAf1Param();
            }

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer 2 Param"))
            {
                TrackingWindowEditor.CreateTrackingAf2Param();
            }

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer 3 Param"))
            {
                TrackingWindowEditor.CreateTrackingAf3Param();
            }

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer 4 Param"))
            {
                TrackingWindowEditor.CreateTrackingAf4Param();
            }

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer 5 Param"))
            {
                TrackingWindowEditor.CreateTrackingAf5Param();
            }

            if (GUILayout.Button("Create Scriptable Tracking AppsFlyer Has Param"))
            {
                TrackingWindowEditor.CreateTrackingAfHasParam();
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }
    }
}