using UnityEditor;
using UnityEngine;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAdjustDrawer
    {
        private static VirtueSky.Tracking.AdjustConfig _config;
        private static UnityEditor.Editor _editor;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null) _editor = null;
            _config = CreateAsset.GetScriptableAsset<VirtueSky.Tracking.AdjustConfig>();
            _editor = UnityEditor.Editor.CreateEditor(_config);
        }


        public static void OnDrawAdjust()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Adjust, "Adjust");
            GUILayout.Space(10);
            CPUtility.DrawButtonInstallPackage("Install Adjust", "Remove Adjust",
                ConstantPackage.PackageNameAdjust, ConstantPackage.MaxVersionAdjust);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Add define symbols");
            GUILayout.Space(10);
#if !VIRTUESKY_ADJUST
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_ADJUST} for Adjust to use",
                MessageType.Info);
#endif
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADJUST);
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            CPUtility.DrawHeader("Adjust Config");
            GUILayout.Space(10);
            if (_config == null)
            {
                if (GUILayout.Button("Create AdjustConfig"))
                {
                    _config =
                        CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Tracking.AdjustConfig>("/AdjustTracking/Resources",
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
                    _editor.OnInspectorGUI();
                }
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Adjust Tracking");
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