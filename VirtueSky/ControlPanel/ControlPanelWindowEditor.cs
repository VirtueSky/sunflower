using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;


namespace VirtueSky.ControlPanel.Editor
{
    public class ControlPanelWindowEditor : EditorWindow
    {
        private StatePanelControl statePanelControl;
        private bool isFieldMax = false;
        private bool isFielAdmob = false;
        private string inputPackageFullNameAdd = "";
        private string inputPackageFullNameRemove = "";
        private Vector2 scrollButton = Vector2.zero;

        [MenuItem("Sunflower/Control Panel &1", false)]
        public static void ShowPanelControlWindow()
        {
            ControlPanelWindowEditor window =
                GetWindow<ControlPanelWindowEditor>("Sunflower Control Panel");
            if (window == null)
            {
                Debug.LogError("Couldn't open the iap settings window!");
                return;
            }

            window.minSize = new Vector2(600, 300);
            window.Show();
        }

        private void OnEnable()
        {
            CPAdvertisingDrawer.OnEnable();
            CPIapDrawer.OnEnable();
            CPLevelEditorDrawer.OnEnable();
        }

        private void OnDisable()
        {
            CPLevelEditorDrawer.OnDisable();
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height),
                GameDataEditor.ColorBackgroundRectWindowSunflower.ToColor());
            GUILayout.Space(10);
            GUI.contentColor = GameDataEditor.ColorTextContentWindowSunflower.ToColor();
            GUILayout.Label("SUNFLOWER CONTROL PANEL", EditorStyles.boldLabel);
            GUI.backgroundColor = GameDataEditor.ColorContentWindowSunflower.ToColor();
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(4, new Vector3(0, 30), new Vector3(position.width, 30));
            // GuiLine(2, Color.black);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(200));
            scrollButton = EditorGUILayout.BeginScrollView(scrollButton);
            DrawButton();
            EditorGUILayout.EndScrollView();
            Handles.DrawAAPolyLine(4, new Vector3(ConstantControlPanel.POSITION_X_START_CONTENT, 0),
                new Vector3(210, position.height));
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

            if (GUILayout.Button("ScriptableObject Event"))
            {
                statePanelControl = StatePanelControl.SO_Event;
            }

            if (GUILayout.Button("ScriptableObject Variable"))
            {
                statePanelControl = StatePanelControl.SO_Variable;
            }

            if (GUILayout.Button("Audio"))
            {
                statePanelControl = StatePanelControl.Audio;
            }

            if (GUILayout.Button("Pools"))
            {
                statePanelControl = StatePanelControl.Pools;
            }

            if (GUILayout.Button("Firebase"))
            {
                statePanelControl = StatePanelControl.Firebase;
            }

            if (GUILayout.Button("Assets Finder"))
            {
                statePanelControl = StatePanelControl.AssetsUsageDetector;
            }

            if (GUILayout.Button("In App Review"))
            {
                statePanelControl = StatePanelControl.InAppReview;
            }

            if (GUILayout.Button("Level Editor"))
            {
                statePanelControl = StatePanelControl.LevelEditor;
            }

            if (GUILayout.Button("Game Service"))
            {
                statePanelControl = StatePanelControl.GameService;
            }

            if (GUILayout.Button("QHierarchy"))
            {
                statePanelControl = StatePanelControl.QHierarchy;
            }

            if (GUILayout.Button("Notifications Chanel"))
            {
                statePanelControl = StatePanelControl.NotificationsChanel;
            }

            if (GUILayout.Button("Scripting Define Symbols"))
            {
                statePanelControl = StatePanelControl.ScriptDefineSymbols;
            }

            if (GUILayout.Button("Register Package"))
            {
                statePanelControl = StatePanelControl.RegisterPackage;
            }

            if (GUILayout.Button("About"))
            {
                statePanelControl = StatePanelControl.About;
            }
        }

        void DrawContent()
        {
            switch (statePanelControl)
            {
                case StatePanelControl.Advertising:
                    CPAdvertisingDrawer.OnDrawAdvertising(position);
                    break;
                case StatePanelControl.InAppPurchase:
                    CPIapDrawer.OnDrawIap(position);
                    break;
                case StatePanelControl.AssetsUsageDetector:
                    CPAssetFinderDrawer.OnDrawAssetUsageDetector();
                    break;
                case StatePanelControl.Audio:
                    CPAudioDrawer.OnDrawAudio();
                    break;
                case StatePanelControl.Pools:
                    CPPoolDrawer.OnDrawPools();
                    break;
                case StatePanelControl.InAppReview:
                    CPInAppReviewDrawer.OnDrawInAppReview(position);
                    break;
                case StatePanelControl.LevelEditor:
                    CPLevelEditorDrawer.OnDrawLevelEditor(position);
                    break;
                case StatePanelControl.NotificationsChanel:
                    CPNotificationChanelDrawer.OnDrawNotificationChanel(position);
                    break;
                case StatePanelControl.SO_Event:
                    CPSoEventDrawer.OnDrawSoEvent();
                    break;
                case StatePanelControl.SO_Variable:
                    CPSoVariableDrawer.OnDrawSoVariable();
                    break;
                case StatePanelControl.ScriptDefineSymbols:
                    CPScriptingDefineSymbolsDrawer.OnDrawScriptingDefineSymbols();
                    break;
                case StatePanelControl.RegisterPackage:
                    CPRegisterPackageDrawer.OnDrawRegisterPackageByManifest(position);
                    break;
                case StatePanelControl.QHierarchy:
                    CPQHierarchyDrawer.OnDrawQHierarchyEvent(position, this);
                    break;
                case StatePanelControl.Firebase:
                    CPFirebaseDrawer.OnDrawFirebase(position);
                    break;
                case StatePanelControl.GameService:
                    CPGameServiceDrawer.OnDrawGameService();
                    break;
                case StatePanelControl.About:
                    CPAboutDrawer.OnDrawAbout(position, () => { OnSettingColorTheme(); });
                    break;
            }
        }

        void GuiLine(int i_height, Color colorLine)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, colorLine);
        }

        #region Setup theme color

        void OnSettingColorTheme()
        {
            GameDataEditor.ColorContentWindowSunflower =
                (CustomColor)EditorGUILayout.EnumPopup("Color Content:",
                    GameDataEditor.ColorContentWindowSunflower);
            GameDataEditor.ColorTextContentWindowSunflower =
                (CustomColor)EditorGUILayout.EnumPopup("Color Text Content:",
                    GameDataEditor.ColorTextContentWindowSunflower);
            GameDataEditor.ColorBackgroundRectWindowSunflower =
                (CustomColor)EditorGUILayout.EnumPopup("Color Background:",
                    GameDataEditor.ColorBackgroundRectWindowSunflower);
            GUILayout.Space(10);
            if (GUILayout.Button("Theme Default"))
            {
                GameDataEditor.ColorContentWindowSunflower = CustomColor.Bright;
                GameDataEditor.ColorTextContentWindowSunflower = CustomColor.Gold;
                GameDataEditor.ColorBackgroundRectWindowSunflower = CustomColor.DarkSlateGray;
            }
        }

        #endregion
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
        RegisterPackage,
        QHierarchy,
        Firebase,
        GameService,
        About,
    }
}