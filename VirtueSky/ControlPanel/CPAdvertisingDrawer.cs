using UnityEditor;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAdvertisingDrawer
    {
        private static bool isFieldMax = false;
        private static bool isFielAdmob = false;
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

        public static void OnDrawAdvertising(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ADVERTISING", EditorStyles.boldLabel);
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


                switch (_adSetting.CurrentAdNetwork)
                {
                    case AdNetwork.Max:
                        DrawMaxField(position);
                        break;
                    case AdNetwork.Admob:
                        DrawAdmobField(position);
                        break;
                    case AdNetwork.IronSource_UnityLevelPlay:
                        DrawIronSource(position);
                        break;
                }
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("PING ADS SETTING", EditorStyles.boldLabel);
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


        static void DrawMaxField(Rect position)
        {
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("INSTALL MAX SDK", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Install Max Sdk Plugin"))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/max-sdk.unitypackage"), false);
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("ADD SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
#if !VIRTUESKY_ADS || !ADS_APPLOVIN
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.ADS_APPLOVIN}\" to use Max Ads",
                MessageType.Info);
#endif

            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_APPLOVIN);
        }

        static void DrawAdmobField(Rect position)
        {
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("INSTALL ADMOB SDK", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Install Admob Sdk Plugin"))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/google-mobile-ads.unitypackage"), false);
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("ADD SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
#if !VIRTUESKY_ADS || !ADS_ADMOB
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.ADS_ADMOB}\" to use Admob Ads",
                MessageType.Info);
#endif

            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_ADMOB);
        }

        static void DrawIronSource(Rect position)
        {
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("INSTALL IRON-SOURCE (UNITY LEVEL PLAY) SDK", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Install IronSource Sdk Plugin"))
            {
                AssetDatabase.ImportPackage(
                    FileExtension.GetPathInCurrentEnvironent(
                        "VirtueSky/Utils/Editor/UnityPackage/is-sdk.unitypackage"), false);
            }

            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            GUILayout.Label("ADD SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
#if !VIRTUESKY_ADS || !ADS_IRONSOURCE
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.ADS_IRONSOURCE}\" to use IronSource Ads",
                MessageType.Info);
#endif


            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_IRONSOURCE);
        }
    }
}