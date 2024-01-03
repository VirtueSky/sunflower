using UnityEditor;
using UnityEngine;

namespace VirtueSky.ControlPanel
{
    public class ControlPanelWindowEditor : EditorWindow
    {
        private StatePanelControl statePanelControl;

        [MenuItem("Sunflower/Panel %Q", false)]
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
            GUILayout.Space(10);

            GUILayout.Label("SUNFLOWER CONTROL PANEL", EditorStyles.boldLabel);

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
            }
        }

        void OnDrawAdvertising()
        {
            GUILayout.Space(10);
            GUILayout.Label("ADVERTISING", EditorStyles.boldLabel);
        }

        void OnDrawIap()
        {
            GUILayout.Space(10);
            GUILayout.Label("IN APP PURCHASE", EditorStyles.boldLabel);
        }

        void OnDrawAssetUsageDetector()
        {
            GUILayout.Space(10);
            GUILayout.Label("ASSET USAGE DETECTOR", EditorStyles.boldLabel);
        }

        void OnDrawAudio()
        {
            GUILayout.Space(10);
            GUILayout.Label("AUDIO", EditorStyles.boldLabel);
        }

        void OnDrawPools()
        {
            GUILayout.Space(10);
            GUILayout.Label("POOLS", EditorStyles.boldLabel);
        }

        void OnDrawInAppReview()
        {
            GUILayout.Space(10);
            GUILayout.Label("IN APP REVIEW", EditorStyles.boldLabel);
        }

        void OnDrawLevelEditor()
        {
            GUILayout.Space(10);
            GUILayout.Label("LEVEL EDITOR", EditorStyles.boldLabel);
        }

        void OnDrawNotificationChanel()
        {
            GUILayout.Space(10);
            GUILayout.Label("NOTIFICATION CHANEL", EditorStyles.boldLabel);
        }

        void OnDrawSoEvent()
        {
            GUILayout.Space(10);
            GUILayout.Label("SCRIPTABLE OBJECT EVENT", EditorStyles.boldLabel);
        }

        void OnDrawSoVariable()
        {
            GUILayout.Space(10);
            GUILayout.Label("SCRIPTABLE OBJECT VARIABLE", EditorStyles.boldLabel);
        }

        void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
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
        SO_Variable
    }
}