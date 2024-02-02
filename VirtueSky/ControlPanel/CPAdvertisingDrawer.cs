using UnityEditor;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPAdvertisingDrawer
    {
        private static bool isFieldMax = false;
        private static bool isFielAdmob = false;
        private static Vector2 _scrollPosition;

        public static void OnDrawAdvertising(Rect position)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            //GUILayout.Label("ADVERTISING", EditorStyles.boldLabel);
            GUILayout.Space(10);
            AdSetting _adSetting = CreateAsset.GetScriptableAsset<AdSetting>();
            if (_adSetting == null)
            {
                if (GUILayout.Button("Create AdSetting"))
                {
                    _adSetting = CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads", "");
                }
            }
            else
            {
                var _editor = UnityEditor.Editor.CreateEditor(_adSetting);

                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                        MessageType.Error);
                    return;
                }

                if (_editor != null)
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
                }
            }


            GUILayout.Space(10);
            GUILayout.EndVertical();
        }


        static void DrawMaxField(Rect position)
        {
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
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
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("ADD SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.ADS_APPLOVIN}\" to use Max Ads",
                MessageType.Info);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_APPLOVIN);
        }

        static void DrawAdmobField(Rect position)
        {
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
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
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            GUILayout.Label("ADD SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            EditorGUILayout.HelpBox(
                $"Add scripting define symbols \"{ConstantDefineSymbols.VIRTUESKY_ADS}\" and \"{ConstantDefineSymbols.ADS_ADMOB}\" to use Admob Ads",
                MessageType.Info);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
            CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_ADMOB);
        }
    }
}