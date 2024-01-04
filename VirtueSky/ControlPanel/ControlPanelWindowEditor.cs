using UnityEditor;
using UnityEngine;
using VirtueSky.Ads;
using VirtueSky.AssetFinder.Editor;
using VirtueSky.Audio;
using VirtueSky.Events;
using VirtueSky.Iap;
using VirtueSky.Inspector;
using VirtueSky.LevelEditor;
using VirtueSky.ObjectPooling;
using VirtueSky.Rating;
using VirtueSky.UtilsEditor;
using VirtueSky.Variables;

namespace VirtueSky.ControlPanel
{
    public class ControlPanelWindowEditor : EditorWindow
    {
        private StatePanelControl statePanelControl;

        [MenuItem("Sunflower/Panel &1", false)]
        public static void ShowPanelControlWindow()
        {
            ControlPanelWindowEditor window = GetWindow<ControlPanelWindowEditor>("Sunflower Control Panel");
            if (window == null)
            {
                Debug.LogError("Couldn't open the iap settings window!");
                return;
            }

            window.minSize = new Vector2(500, 300);
            window.Show();
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height), ColorExtensions.ToColor(CustomColor.DarkOlive));
            GUILayout.Space(10);
            GUI.contentColor = ColorExtensions.ToColor(CustomColor.Cyan);
            GUILayout.Label("SUNFLOWER CONTROL PANEL", EditorStyles.boldLabel);
            GUI.backgroundColor = ColorExtensions.ToColor(CustomColor.Orange);
            GuiLine();
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(200));
            DrawButton();
            Handles.color = Color.black;
            Handles.DrawLine(new Vector3(210, 0), new Vector3(210, position.height));
            GUILayout.EndVertical();
            DrawContent();
            GUILayout.EndHorizontal();
        }

        void DrawButton()
        {
            if (GUILayout.Button("Advertising"))
            {
                statePanelControl = StatePanelControl.Advertising;
            }

            if (GUILayout.Button("In App Purchase"))
            {
                statePanelControl = StatePanelControl.InAppPurchase;
            }

            if (GUILayout.Button("Assets Usage Detector"))
            {
                statePanelControl = StatePanelControl.AssetsUsageDetector;
            }

            if (GUILayout.Button("Audio"))
            {
                statePanelControl = StatePanelControl.Audio;
            }

            if (GUILayout.Button("Pools"))
            {
                statePanelControl = StatePanelControl.Pools;
            }

            if (GUILayout.Button("In App Review"))
            {
                statePanelControl = StatePanelControl.InAppReview;
            }

            if (GUILayout.Button("Level Editor"))
            {
                statePanelControl = StatePanelControl.LevelEditor;
            }

            if (GUILayout.Button("Notifications Chanel"))
            {
                statePanelControl = StatePanelControl.NotificationsChanel;
            }

            if (GUILayout.Button("ScriptableObject Event"))
            {
                statePanelControl = StatePanelControl.SO_Event;
            }

            if (GUILayout.Button("ScriptableObject Variable"))
            {
                statePanelControl = StatePanelControl.SO_Variable;
            }

            if (GUILayout.Button("Scripting Define Symbols"))
            {
                statePanelControl = StatePanelControl.ScriptDefineSymbols;
            }
        }

        void DrawContent()
        {
            switch (statePanelControl)
            {
                case StatePanelControl.Advertising:
                    OnDrawAdvertising();
                    break;
                case StatePanelControl.InAppPurchase:
                    OnDrawIap();
                    break;
                case StatePanelControl.AssetsUsageDetector:
                    OnDrawAssetUsageDetector();
                    break;
                case StatePanelControl.Audio:
                    OnDrawAudio();
                    break;
                case StatePanelControl.Pools:
                    OnDrawPools();
                    break;
                case StatePanelControl.InAppReview:
                    OnDrawInAppReview();
                    break;
                case StatePanelControl.LevelEditor:
                    OnDrawLevelEditor();
                    break;
                case StatePanelControl.NotificationsChanel:
                    OnDrawNotificationChanel();
                    break;
                case StatePanelControl.SO_Event:
                    OnDrawSoEvent();
                    break;
                case StatePanelControl.SO_Variable:
                    OnDrawSoVariable();
                    break;
                case StatePanelControl.ScriptDefineSymbols:
                    OnDrawScriptDefineSymbols();
                    break;
            }
        }

        #region Draw Content Details

        void OnDrawAdvertising()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ADVERTISING", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("AdSetting (Alt+4 / Option+4)"))
            {
                AdsWindowEditor.OpenAdSettingsWindows();
            }

            GUILayout.EndVertical();
        }

        void OnDrawIap()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP PURCHASE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("AdSetting (Alt+2 / Option+2)"))
            {
                IapWindowEditor.OpenIapSettingsWindows();
            }

            if (GUILayout.Button("Create Iap Purchase Product Event"))
            {
                IapWindowEditor.CreateIapProductEvent();
            }

            if (GUILayout.Button("Create Iap Purchase Product Event"))
            {
                IapWindowEditor.CreateIsPurchaseProductEvent();
            }

            GUILayout.EndVertical();
        }

        void OnDrawAssetUsageDetector()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("ASSET USAGE DETECTOR", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Active Window"))
            {
                AssetUsageDetectorWindow.OpenActiveWindow();
            }

            if (GUILayout.Button("New Window"))
            {
                AssetUsageDetectorWindow.OpenNewWindow();
            }

            GUILayout.EndVertical();
        }

        void OnDrawAudio()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("AUDIO", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Event Audio Handle"))
            {
                AudioWindowEditor.CreateEventAudioHandle();
            }

            if (GUILayout.Button("Create Sound Data"))
            {
                AudioWindowEditor.CreateSoundData();
            }

            GUILayout.EndVertical();
        }

        void OnDrawPools()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("POOLS", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Pools"))
            {
                PoolWindowEditor.CreatePools();
            }

            GUILayout.EndVertical();
        }

        void OnDrawInAppReview()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("IN APP REVIEW", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create In App Review"))
            {
                RatingWindowEditor.CreateInAppReview();
            }

            GUILayout.EndVertical();
        }

        void OnDrawLevelEditor()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("LEVEL EDITOR", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Open Level Editor (Alt+3 / Option+3)"))
            {
                UtilitiesLevelSystemDrawer.OpenLevelEditor();
            }

            GUILayout.EndVertical();
        }

        void OnDrawNotificationChanel()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("NOTIFICATION CHANEL", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Notification Chanel"))
            {
                NotificationWindowEditor.CreateNotificationChannel();
            }

            GUILayout.EndVertical();
        }

        void OnDrawSoEvent()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("SCRIPTABLE OBJECT EVENT", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Boolean Event"))
            {
                EventWindowEditor.CreateEventBoolean();
            }

            if (GUILayout.Button("Create Dictionary Event"))
            {
                EventWindowEditor.CreateEventDictionary();
            }

            if (GUILayout.Button("Create No Param Event"))
            {
                EventWindowEditor.CreateEventNoParam();
            }

            if (GUILayout.Button("Create Float Event"))
            {
                EventWindowEditor.CreateEventFloat();
            }

            if (GUILayout.Button("Create Int Event"))
            {
                EventWindowEditor.CreateEventInt();
            }

            if (GUILayout.Button("Create Object Event"))
            {
                EventWindowEditor.CreateEventObject();
            }

            if (GUILayout.Button("Create Short Double Event"))
            {
                EventWindowEditor.CreateEventShortDouble();
            }

            if (GUILayout.Button("Create String Event"))
            {
                EventWindowEditor.CreateEventString();
            }

            if (GUILayout.Button("Create Vector3 Event"))
            {
                EventWindowEditor.CreateEventVector3();
            }

            GUILayout.EndVertical();
        }

        void OnDrawSoVariable()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            GUILayout.Label("SCRIPTABLE OBJECT VARIABLE", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (GUILayout.Button("Create Boolean Variable"))
            {
                VariableWindowEditor.CreateVariableBoolean();
            }

            if (GUILayout.Button("Create Float Variable"))
            {
                VariableWindowEditor.CreateVariableFloat();
            }

            if (GUILayout.Button("Create Int Variable"))
            {
                VariableWindowEditor.CreateVariableInt();
            }

            if (GUILayout.Button("Create Object Variable"))
            {
                VariableWindowEditor.CreateVariableObject();
            }

            if (GUILayout.Button("Create Rect Variable"))
            {
                VariableWindowEditor.CreateVariableRect();
            }

            if (GUILayout.Button("Create Short Double Variable"))
            {
                VariableWindowEditor.CreateVariableShortDouble();
            }

            if (GUILayout.Button("Create String Variable"))
            {
                VariableWindowEditor.CreateVariableString();
            }

            if (GUILayout.Button("Create Transform Variable"))
            {
                VariableWindowEditor.CreateVariableTransform();
            }

            if (GUILayout.Button("Create Vector3 Variable"))
            {
                VariableWindowEditor.CreateVariableVector3();
            }

            GUILayout.EndVertical();
        }

        void OnDrawScriptDefineSymbols()
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
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAdsFlag(), TextIsEnable(EditorScriptDefineSymbols.IsAdsFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag applovin

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ADS_APPLOVIN", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.ApplovinConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsApplovinFlag(), TextIsEnable(EditorScriptDefineSymbols.IsApplovinFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag admob

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("ADS_ADMOB", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AdmobConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAdmobFlag(), TextIsEnable(EditorScriptDefineSymbols.IsAdmobFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag adjust

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_ADJUST", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AdjustConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAdjustFlag(), TextIsEnable(EditorScriptDefineSymbols.IsAdjustFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag firebase app

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_FIREBASE", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.FirebaseAppConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsFirebaseAppFlag(), TextIsEnable(EditorScriptDefineSymbols.IsFirebaseAppFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag analytic

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_FIREBASE_ANALYTIC", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.AnalyticConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsAnalyticFlag(), TextIsEnable(EditorScriptDefineSymbols.IsAnalyticFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region Flag Remote Config

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_FIREBASE_REMOTECONFIG", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.RemoteConfigConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsRemoteConfigConfigFlag(), TextIsEnable(EditorScriptDefineSymbols.IsRemoteConfigConfigFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag iap

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_IAP", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.IapConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsIapFlag(), TextIsEnable(EditorScriptDefineSymbols.IsIapFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag ratting

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_RATING", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.RattingConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsRattingFlag(), TextIsEnable(EditorScriptDefineSymbols.IsRattingFlag()));
            GUILayout.EndHorizontal();

            #endregion

            #region flag notifications

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("VIRTUESKY_NOTIFICATION", GUILayout.Width(400)))
            {
                EditorScriptDefineSymbols.NotificationConfigFlag();
            }

            GUILayout.Space(10);
            GUILayout.Toggle(EditorScriptDefineSymbols.IsNotificationFlag(), TextIsEnable(EditorScriptDefineSymbols.IsNotificationFlag()));
            GUILayout.EndHorizontal();

            #endregion

            GUILayout.EndVertical();
        }

        #endregion

        void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }

        string TextIsEnable(bool condition)
        {
            return condition ? "Enable" : "Disable";
        }
    }

    public enum StatePanelControl
    {
        Advertising,
        InAppPurchase,
        AssetsUsageDetector,
        Audio,
        Pools,
        InAppReview,
        LevelEditor,
        NotificationsChanel,
        SO_Event,
        SO_Variable,
        ScriptDefineSymbols,
    }
}