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
        private Vector2 scrollPositionFileManifest = Vector2.zero;

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

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height),
                ColorBackgroundRect.ToColor());
            GUILayout.Space(10);
            GUI.contentColor = ColorTextContent.ToColor();
            GUILayout.Label("SUNFLOWER CONTROL PANEL", EditorStyles.boldLabel);
            GUI.backgroundColor = ColorContent.ToColor();
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(4, new Vector3(0, 30), new Vector3(position.width, 30));
            // GuiLine(2, Color.black);
            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(200));
            DrawButton();
            Handles.DrawAAPolyLine(4, new Vector3(210, 0), new Vector3(210, position.height));
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

            if (GUILayout.Button("Assets Usage Detector"))
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
                statePanelControl = StatePanelControl.ImportPackage;
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
                    CPAdvertisingDrawer.OnDrawAdvertising(position, ref statePanelControl);
                    break;
                case StatePanelControl.InAppPurchase:
                    CPIapDrawer.OnDrawIap(position, ref statePanelControl);
                    break;
                case StatePanelControl.AssetsUsageDetector:
                    CPAssetUsageDetectorDrawer.OnDrawAssetUsageDetector();
                    break;
                case StatePanelControl.Audio:
                    CPAudioDrawer.OnDrawAudio();
                    break;
                case StatePanelControl.Pools:
                    CPPoolDrawer.OnDrawPools();
                    break;
                case StatePanelControl.InAppReview:
                    CPInAppReviewDrawer.OnDrawInAppReview();
                    break;
                case StatePanelControl.LevelEditor:
                    CPLevelEditorDrawer.OnDrawLevelEditor();
                    break;
                case StatePanelControl.NotificationsChanel:
                    CPNotificationChanelDrawer.OnDrawNotificationChanel();
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
                case StatePanelControl.ImportPackage:
                    CPRegisterPackageDrawer.OnDrawImportPackageByManifest(position, this);
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
            ColorContent =
                (CustomColor)EditorGUILayout.EnumPopup("Color Content:", ColorContent);
            ColorTextContent =
                (CustomColor)EditorGUILayout.EnumPopup("Color Text Content:",
                    ColorTextContent);
            ColorBackgroundRect =
                (CustomColor)EditorGUILayout.EnumPopup("Color Background:",
                    ColorBackgroundRect);
            GUILayout.Space(10);
            if (GUILayout.Button("Theme Default"))
            {
                ColorContent = CustomColor.LightRed;
                ColorTextContent = CustomColor.Gold;
                ColorBackgroundRect = CustomColor.DarkSlateGray;
            }
        }

        public CustomColor ColorContent
        {
            get => (CustomColor)EditorPrefs.GetInt("ColorContent_ControlPanel",
                (int)CustomColor.LightRed);
            set => EditorPrefs.SetInt("ColorContent_ControlPanel", (int)value);
        }

        public CustomColor ColorTextContent
        {
            get => (CustomColor)EditorPrefs.GetInt("ColorTextContent_ControlPanel",
                (int)CustomColor.Gold);
            set => EditorPrefs.SetInt("ColorTextContent_ControlPanel", (int)value);
        }

        public CustomColor ColorBackgroundRect
        {
            get => (CustomColor)EditorPrefs.GetInt("ColorBackground_ControlPanel",
                (int)CustomColor.DarkSlateGray);
            set => EditorPrefs.SetInt("ColorBackground_ControlPanel", (int)value);
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
        ImportPackage,
        About,
    }
}