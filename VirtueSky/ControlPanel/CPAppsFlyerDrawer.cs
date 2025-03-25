using UnityEditor;
using UnityEngine;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAppsFlyerDrawer
    {
        private static VirtueSky.Tracking.AppsFlyerConfig _config;
        private static UnityEditor.Editor _editor;
        private static Vector2 scroll = Vector2.zero;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null) _editor = null;
            _config = CreateAsset.GetScriptableAsset<VirtueSky.Tracking.AppsFlyerConfig>();
            _editor = UnityEditor.Editor.CreateEditor(_config);
        }

        public static void OnDrawAppsFlyer()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.AppsFlyer, "AppsFlyer");
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            CPUtility.DrawButtonInstallPackage("Install AppsFlyer", "Remove AppsFlyer",
                ConstantPackage.PackageNameAppFlyer, ConstantPackage.MaxVersionAppFlyer);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
#if !VIRTUESKY_APPSFLYER
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_APPSFLYER} for AppsFlyer to use",
                MessageType.Info);
#endif
            GUILayout.Space(10);
            CPUtility.DrawHeader("Define Symbols");
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPSFLYER);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            CPUtility.DrawHeader("AppsFlyer Config");
            GUILayout.Space(10);
            if (_config == null)
            {
                if (GUILayout.Button("Create AppsFlyerConfig"))
                {
                    _config =
                        CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Tracking.AppsFlyerConfig>(
                            "/AppsFlyerTracking/Resources",
                            isPingAsset: false);
                    Init();
                }
            }
            else
            {
                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings editor.",
                        MessageType.Error);
                    return;
                }
                else
                {
                    EditorGUILayout.HelpBox(
                        "Set your devKey and appID to init the AppsFlyer SDK and start tracking. You must modify these fields and provide:\ndevKey - Your application devKey provided by AppsFlyer.\nappId - For iOS only. Your iTunes Application ID.\nUWP app id - For UWP only. Your application app id \nMac OS app id - For MacOS app only.",
                        MessageType.Info);
                    _editor.OnInspectorGUI();
                }
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            CPUtility.DrawHeader("AppsFlyer Tracking");
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
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Ping AppsflyerConfig");
            GUILayout.Space(10);
            if (GUILayout.Button("Ping"))
            {
                if (_config == null)
                {
                    Debug.LogError("AppsflyerConfig have not been created yet");
                }
                else
                {
                    EditorGUIUtility.PingObject(_config);
                    Selection.activeObject = _config;
                }
            }

            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}