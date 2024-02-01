using UnityEditor;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPAdvertisingDrawer
    {
        private static bool isFieldMax = false;
        private static bool isFielAdmob = false;

        public static void OnDrawAdvertising(Rect position, ref StatePanelControl statePanelControl)
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ADVERTISING", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open AdSetting (Alt+4 / Option+4)"))
            {
                AdsWindowEditor.OpenAdSettingsWindows();
            }

            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            DrawMaxField(position);
            GUILayout.Space(10);
            Handles.DrawAAPolyLine(3, new Vector3(210, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);
            DrawAdmobField(position);
            GUILayout.EndVertical();
        }

        static void DrawMaxField(Rect position)
        {
            isFieldMax = GUILayout.Toggle(isFieldMax, "Max");
            if (isFieldMax)
            {
                if (GUILayout.Button("Create Max Client"))
                {
                    AdsWindowEditor.CreateMaxClient();
                }

                if (GUILayout.Button("Create Max Banner"))
                {
                    AdsWindowEditor.CreateMaxBanner();
                }

                if (GUILayout.Button("Create Max Inter"))
                {
                    AdsWindowEditor.CreateMaxInter();
                }

                if (GUILayout.Button("Create Max Reward"))
                {
                    AdsWindowEditor.CreateMaxReward();
                }

                if (GUILayout.Button("Create Max Reward Inter"))
                {
                    AdsWindowEditor.CreateMaxRewardInter();
                }

                if (GUILayout.Button("Create Max App Open"))
                {
                    AdsWindowEditor.CreateMaxAppOpen();
                }

                GUILayout.Space(10);
                Handles.DrawAAPolyLine(2f, new Vector3(225, GUILayoutUtility.GetLastRect().y + 10),
                    new Vector3(position.width - 20, GUILayoutUtility.GetLastRect().y + 10));
                GUILayout.Space(10);
                GUILayout.Space(10);
                if (GUILayout.Button("Install Max Sdk Plugin"))
                {
                    AssetDatabase.ImportPackage(
                        FileExtension.GetPathInCurrentEnvironent(
                            "VirtueSky/Utils/Editor/UnityPackage/max-sdk.unitypackage"), false);
                }

                GUILayout.Space(10);
                Handles.DrawAAPolyLine(2f, new Vector3(225, GUILayoutUtility.GetLastRect().y + 10),
                    new Vector3(position.width - 20, GUILayoutUtility.GetLastRect().y + 10));
                GUILayout.Space(10);
                EditorGUILayout.HelpBox(
                    "Add scripting define symbols \"VIRTUESKY_ADS\" and \"ADS_APPLOVIN\" to use Max Ads",
                    MessageType.Info);
                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_APPLOVIN);
            }
        }

        static void DrawAdmobField(Rect position)
        {
            isFielAdmob = GUILayout.Toggle(isFielAdmob, "Admob");
            if (isFielAdmob)
            {
                if (GUILayout.Button("Create Admob Client"))
                {
                    AdsWindowEditor.CreateAdmobClient();
                }

                if (GUILayout.Button("Create Admob Banner"))
                {
                    AdsWindowEditor.CreateAdmobBanner();
                }

                if (GUILayout.Button("Create Admob Inter"))
                {
                    AdsWindowEditor.CreateAdmobInter();
                }

                if (GUILayout.Button("Create Admob Reward"))
                {
                    AdsWindowEditor.CreateAdmobReward();
                }

                if (GUILayout.Button("Create Admob Reward Inter"))
                {
                    AdsWindowEditor.CreateAdmobRewardInter();
                }

                if (GUILayout.Button("Create Admob App Open"))
                {
                    AdsWindowEditor.CreateAdmobAppOpen();
                }

                GUILayout.Space(10);
                Handles.DrawAAPolyLine(2f, new Vector3(225, GUILayoutUtility.GetLastRect().y + 10),
                    new Vector3(position.width - 20, GUILayoutUtility.GetLastRect().y + 10));
                GUILayout.Space(10);
                if (GUILayout.Button("Install Admob Sdk Plugin"))
                {
                    AssetDatabase.ImportPackage(
                        FileExtension.GetPathInCurrentEnvironent(
                            "VirtueSky/Utils/Editor/UnityPackage/google-mobile-ads.unitypackage"), false);
                }

                GUILayout.Space(10);
                Handles.DrawAAPolyLine(2f, new Vector3(225, GUILayoutUtility.GetLastRect().y + 10),
                    new Vector3(position.width - 20, GUILayoutUtility.GetLastRect().y + 10));
                GUILayout.Space(10);
                EditorGUILayout.HelpBox(
                    "Add scripting define symbols \"VIRTUESKY_ADS\" and \"ADS_ADMOB\" to use Admob Ads",
                    MessageType.Info);
                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.VIRTUESKY_ADS);
                CPUtility.DrawButtonAddDefineSymbols(ConstantDefineSymbols.ADS_ADMOB);
            }
        }
    }
}