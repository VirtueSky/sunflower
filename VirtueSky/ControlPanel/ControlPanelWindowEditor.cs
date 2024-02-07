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
            GUILayout.BeginVertical(GUILayout.Width(ConstantControlPanel.WIDTH_CONTENT_BUTTON_STATE_CONTROL_PANEL));
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
            DrawButtonChooseState("Advertising", StatePanelControl.Advertising);
            DrawButtonChooseState("In App Purchase", StatePanelControl.InAppPurchase);
            DrawButtonChooseState("ScriptableObject Event", StatePanelControl.SO_Event);
            DrawButtonChooseState("ScriptableObject Variable", StatePanelControl.SO_Variable);
            DrawButtonChooseState("Audio", StatePanelControl.Audio);
            DrawButtonChooseState("Pools", StatePanelControl.Pools);
            DrawButtonChooseState("Firebase", StatePanelControl.Firebase);
            DrawButtonChooseState("Assets Finder", StatePanelControl.AssetsFinder);
            DrawButtonChooseState("In App Review", StatePanelControl.InAppReview);
            DrawButtonChooseState("Level Editor", StatePanelControl.LevelEditor);
            DrawButtonChooseState("Game Service", StatePanelControl.GameService);
            DrawButtonChooseState("QHierarchy", StatePanelControl.QHierarchy);
            DrawButtonChooseState("Notifications Chanel", StatePanelControl.NotificationsChanel);
            DrawButtonChooseState("Scripting Define Symbols", StatePanelControl.ScriptDefineSymbols);
            DrawButtonChooseState("Register Package", StatePanelControl.RegisterPackage);
            DrawButtonChooseState("About", StatePanelControl.About);
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
                case StatePanelControl.AssetsFinder:
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

        void DrawButtonChooseState(string title, StatePanelControl _statePanelControlTab)
        {
            if (statePanelControl == _statePanelControlTab)
            {
                if (GUILayout.Button(title))
                {
                }
            }
            else
            {
                if (GUILayout.Button(title,
                        GUILayout.Width(ConstantControlPanel.WIDTH_CONTENT_BUTTON_STATE_CONTROL_PANEL - 20)))
                {
                    statePanelControl = _statePanelControlTab;
                }
            }
        }
    }

    public enum StatePanelControl
    {
        Advertising,
        InAppPurchase,
        AssetsFinder,
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