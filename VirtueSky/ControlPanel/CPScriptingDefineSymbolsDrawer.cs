using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public static class CPScriptingDefineSymbolsDrawer
    {
        public static void OnDrawScriptingDefineSymbols()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("SCRIPTING DEFINE SYMBOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);

            #region flag ads

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_ADS", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AdsConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAdsFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsAdsFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag applovin

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ADS_APPLOVIN", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.ApplovinConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsApplovinFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsApplovinFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag admob

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ADS_ADMOB", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AdmobConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAdmobFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsAdmobFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag adjust

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_ADJUST", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AdjustConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAdjustFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsAdjustFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag firebase app

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_FIREBASE", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.FirebaseAppConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsFirebaseAppFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsFirebaseAppFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag analytic

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_FIREBASE_ANALYTIC", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AnalyticConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAnalyticFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsAnalyticFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region Flag Remote Config

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_FIREBASE_REMOTECONFIG", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.RemoteConfigConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsRemoteConfigConfigFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsRemoteConfigConfigFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag iap

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_IAP", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.IapConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsIapFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsIapFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag ratting

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_RATING", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.RattingConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsRattingFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsRattingFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag notifications

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_NOTIFICATION", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.NotificationConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsNotificationFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsNotificationFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag AppsFlyer

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_APPSFLYER", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AppsFlyerConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAppsFlyerFlag(),
                TextIsEnable(EditorScriptDefineSymbols.IsAppsFlyerFlag()));
            GUILayout.EndHorizontal();

            #endregion

            GUILayout.EndVertical();
        }

        static string TextIsEnable(bool condition)
        {
            return condition ? "Enable" : "Disable";
        }
    }
}