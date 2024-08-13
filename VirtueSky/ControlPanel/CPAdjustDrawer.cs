using UnityEditor;
using UnityEngine;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAdjustDrawer
    {
        private static AdjustSettings _settings;
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

            _settings = CreateAsset.GetScriptableAsset<AdjustSettings>();
            Debug.Log(_settings);
            _editor = UnityEditor.Editor.CreateEditor(_settings);
        }

        public static void OnDrawAdjust()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ADJUST", EditorStyles.boldLabel);
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install Adjust", "Remove Adjust",
                ConstantPackage.PackageNameAdjust, ConstantPackage.MaxVersionAdjust);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
#if !VIRTUESKY_ADJUST
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_ADJUST} for Adjust to use",
                MessageType.Info);
#endif
            GUILayout.Space(10);
            GUILayout.Label("ADD DEFINE SYMBOLS", EditorStyles.boldLabel);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADJUST);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Label("ADJUST SETTINGS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (_settings == null)
            {
                if (GUILayout.Button("Create Adjust Settings"))
                {
                    _settings =
                        CreateAsset.CreateAndGetScriptableAsset<AdjustSettings>(
                            "/AdjustTracking/Settings", isPingAsset: false);
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
                    _editor.OnInspectorGUI();
                }
            }


            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("ADJUST TRACKING", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Scriptable Tracking Adjust"))
            {
                TrackingWindowEditor.CreateTrackingAdjust();
            }

            GUILayout.Space(10);
            GUILayout.EndVertical();
        }
    }
}