using System;
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;


namespace VirtueSky.ControlPanel.Editor
{
    public class ControlPanelWindowEditor : EditorWindow
    {
        private StatePanelControl statePanelControl;
        private Vector2 scrollButton = Vector2.zero;

        [MenuItem("Sunflower/Magic Panel &1", false, priority = 1)]
        public static void ShowPanelControlWindow()
        {
            ControlPanelWindowEditor window =
                GetWindow<ControlPanelWindowEditor>("Magic Panel");
            if (window == null)
            {
                Debug.LogError("Couldn't open the window!");
                return;
            }

            window.minSize = new Vector2(600, 300);
            window.Show();
        }

        private void OnEnable()
        {
            statePanelControl = StatePanelControl.About;
            CPAdvertisingDrawer.OnEnable();
            CPIapDrawer.OnEnable();
            CPLevelEditorDrawer.OnEnable();
            CPFolderIconDrawer.OnEnable();
            CPAdjustDrawer.OnEnable();
            CPAppsFlyerDrawer.OnEnable();
        }

        private void OnDisable()
        {
            CPLevelEditorDrawer.OnDisable();
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            CPUtility.DrawHeader("Magic Panel", 17);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(4, new Vector3(0, 30), new Vector3(position.width, 30));
            // GuiLine(2, Color.black);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(ConstantControlPanel.WIDTH_CONTENT_BUTTON_STATE_CONTROL_PANEL));
            scrollButton = EditorGUILayout.BeginScrollView(scrollButton);
            DrawButton();
            EditorGUILayout.EndScrollView();
            Handles.DrawAAPolyLine(4, new Vector3(ConstantControlPanel.POSITION_X_START_CONTENT, 30),
                new Vector3(ConstantControlPanel.POSITION_X_START_CONTENT, position.height));

            GUILayout.EndVertical();
            DrawContent();
            GUILayout.EndHorizontal();
        }

        void DrawButton()
        {
            DrawButtonChooseState("Advertising", StatePanelControl.Advertising);
            DrawButtonChooseState("In App Purchase", StatePanelControl.InAppPurchase);
            DrawButtonChooseState("Scriptable Event", StatePanelControl.SO_Event);
            DrawButtonChooseState("Scriptable Variable", StatePanelControl.SO_Variable);
            DrawButtonChooseState("Audio", StatePanelControl.Audio);
            DrawButtonChooseState("Firebase", StatePanelControl.Firebase);
            DrawButtonChooseState("Adjust", StatePanelControl.Adjust);
            DrawButtonChooseState("AppsFlyer", StatePanelControl.AppsFlyer);
            DrawButtonChooseState("Assets Finder", StatePanelControl.AssetsFinder);
            DrawButtonChooseState("In App Review", StatePanelControl.InAppReview);
            DrawButtonChooseState("Level Editor", StatePanelControl.LevelEditor);
            DrawButtonChooseState("Game Service", StatePanelControl.GameService);
            DrawButtonChooseState("Folder Icon", StatePanelControl.FolderIcon);
            DrawButtonChooseState("Hierarchy", StatePanelControl.Hierarchy);
            DrawButtonChooseState("Notifications", StatePanelControl.NotificationsChanel);
            DrawButtonChooseState("Scripting Define Symbols", StatePanelControl.ScriptDefineSymbols);
            DrawButtonChooseState("Register Package", StatePanelControl.RegisterPackage);
            DrawButtonChooseState("Extensions", StatePanelControl.Extensions);
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
                    CPAudioDrawer.OnDrawAudio(position);
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
                case StatePanelControl.FolderIcon:
                    CPFolderIconDrawer.OnDrawFolderIcon();
                    break;
                case StatePanelControl.Hierarchy:
                    CPHierarchyDrawer.OnDrawQHierarchyEvent(position, this);
                    break;
                case StatePanelControl.Firebase:
                    CPFirebaseDrawer.OnDrawFirebase(position);
                    break;
                case StatePanelControl.Adjust:
                    CPAdjustDrawer.OnDrawAdjust();
                    break;
                case StatePanelControl.AppsFlyer:
                    CPAppsFlyerDrawer.OnDrawAppsFlyer();
                    break;
                case StatePanelControl.GameService:
                    CPGameServiceDrawer.OnDrawGameService();
                    break;
                case StatePanelControl.Extensions:
                    CPExtensionsDrawer.OnDrawExtensions(position);
                    break;
                case StatePanelControl.About:
                    CPAboutDrawer.OnDrawAbout(position);
                    break;
            }
        }


        void DrawButtonChooseState(string title, StatePanelControl _statePanelControlTab)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(5);
            GUILayout.Box(CPUtility.GetIcon(_statePanelControlTab), GUIStyle.none, GUILayout.ExpandWidth(true),
                GUILayout.Width(18), GUILayout.Height(18));
            bool clicked = GUILayout.Toggle(_statePanelControlTab == statePanelControl, title, GUI.skin.button,
                GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true), GUILayout.Height(20));
            if (clicked && statePanelControl != _statePanelControlTab)
            {
                statePanelControl = _statePanelControlTab;
            }

            GUILayout.EndHorizontal();
            GUILayout.Space(2);
        }
    }

    public enum StatePanelControl
    {
        Advertising,
        InAppPurchase,
        AssetsFinder,
        Audio,
        InAppReview,
        LevelEditor,
        NotificationsChanel,
        SO_Event,
        SO_Variable,
        Adjust,
        AppsFlyer,
        ScriptDefineSymbols,
        RegisterPackage,
        Hierarchy,
        FolderIcon,
        Firebase,
        GameService,
        Extensions,
        About,
    }
}