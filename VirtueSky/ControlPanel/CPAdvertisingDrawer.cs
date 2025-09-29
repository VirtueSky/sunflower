using UnityEditor;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAdvertisingDrawer
    {
        private static Vector2 _scrollPosition;
        private static UnityEditor.Editor _editor;
        private static AdSetting _adSetting;
        private static Vector2 scroll = Vector2.zero;

        public static void OnEnable()
        {
            Init();
        }

        public static void Init()
        {
            if (_editor != null)
            {
                _editor = null;
            }

            _adSetting = CreateAsset.GetScriptableAsset<AdSetting>();
            _editor = UnityEditor.Editor.CreateEditor(_adSetting);
        }

        public static void OnDrawAdvertising()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Advertising, "Advertising");
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            if (_adSetting == null)
            {
                if (GUILayout.Button("Create AdSetting"))
                {
                    _adSetting =
                        CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads/Setting",
                            isPingAsset: false);
                    Init();
                }
            }
            else
            {
                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                        MessageType.Error);
                    return;
                }
                else
                {
                    _editor.OnInspectorGUI();
                }

                DrawDefineSymbols();
                DrawInstallSdk();
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Ping Ads Settings");
            GUILayout.Space(10);
            if (GUILayout.Button("Ping"))
            {
                if (_adSetting == null)
                {
                    Debug.LogError("AdSetting have not been created yet");
                }
                else
                {
                    EditorGUIUtility.PingObject(_adSetting);
                    Selection.activeObject = _adSetting;
                }
            }

            GUILayout.Space(10);
            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }

        static void DrawDefineSymbols()
        {
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Define Symbols");
            GUILayout.Space(10);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            if (_adSetting.UseMax)
            {
#if !VIRTUESKY_ADS || !VIRTUESKY_APPLOVIN
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.VIRTUESKY_APPLOVIN}\" to use Max Ads",
                MessageType.Info);
#endif
                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_APPLOVIN);
            }

            if (_adSetting.UseAdmob)
            {
#if !VIRTUESKY_ADS || !VIRTUESKY_ADMOB
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.VIRTUESKY_ADMOB}\" to use Admob Ads",
                MessageType.Info);
#endif
                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADMOB);
            }

            if (_adSetting.UseIronSource)
            {
#if !VIRTUESKY_ADS || !VIRTUESKY_LEVELPLAY
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.VIRTUESKY_LEVELPLAY}\" to use IronSource Ads",
                MessageType.Info);
#endif

                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_LEVELPLAY);
            }
        }

        static void DrawInstallSdk()
        {
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            CPUtility.DrawHeader("Install Sdk");
            GUILayout.Space(10);
            if (_adSetting.UseMax)
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Install Max Sdk Plugin"))
                {
                    AssetDatabase.ImportPackage(
                        FileExtension.GetPathFileInCurrentEnvironment(
                            "VirtueSky/Utils/Editor/UnityPackage/max-sdk.unitypackage"), false);
                }
            }

            if (_adSetting.UseAdmob)
            {
                GUILayout.Space(10);
                CPUtility.DrawButtonInstallPackage("Install Admob Sdk Plugin", "Remove Admob Sdk Plugin",
                    ConstantPackage.PackageNameAdmob, ConstantPackage.VersionAdmob);
            }

            if (_adSetting.UseIronSource)
            {
                GUILayout.Space(10);
                CPUtility.DrawButtonInstallPackage("Install LevelPlay Sdk Plugin", "Remove LevelPlay Sdk Plugin",
                    ConstantPackage.PackageNameLevelPlay, ConstantPackage.MaxVersionLevelPlay);
            }
        }
    }
}