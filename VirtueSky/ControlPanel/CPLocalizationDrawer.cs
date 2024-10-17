using UnityEditor;
using UnityEngine;
using VirtueSky.Localization;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPLocalizationDrawer
    {
        private static LocaleTabType localeTabType = LocaleTabType.Setting;
        private static VirtueSky.Localization.LocaleSettings _settings;
        private static UnityEditor.Editor _editor;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null) _editor = null;
            _settings = CreateAsset.GetScriptableAsset<LocaleSettings>();
            _editor = UnityEditor.Editor.CreateEditor(_settings);
        }

        public static void OnDrawLocalization()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Localization, "Localization");
            GUILayout.Space(10);
            DrawTab();
            GUILayout.Space(10);
            CPUtility.GuiLine(2);
            GUILayout.Space(10);
            switch (localeTabType)
            {
                case LocaleTabType.Setting:
                    DrawSetting();
                    break;
                case LocaleTabType.Explore:
                    DrawExplore();
                    break;
            }

            GUILayout.EndVertical();
        }

        static void DrawTab()
        {
            EditorGUILayout.BeginHorizontal();
            bool clickSetting = GUILayout.Toggle(localeTabType == LocaleTabType.Setting, "Setting",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickSetting && localeTabType != LocaleTabType.Setting)
            {
                localeTabType = LocaleTabType.Setting;
            }

            bool clickPickup = GUILayout.Toggle(localeTabType == LocaleTabType.Explore, "Explore",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickPickup && localeTabType != LocaleTabType.Explore)
            {
                localeTabType = LocaleTabType.Explore;
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSetting()
        {
            if (_settings == null)
            {
                if (GUILayout.Button("Create LocaleSettings"))
                {
                    _settings = CreateAsset.CreateAndGetScriptableAsset<LocaleSettings>("/Localization/Resources", isPingAsset: false);
                    Init();
                }
            }
            else
            {
                if (_editor == null)
                {
                    EditorGUILayout.HelpBox("Couldn't create the settings editor.",
                        MessageType.Error);
                    return;
                }

                _editor.OnInspectorGUI();
            }
        }

        private static void DrawExplore()
        {
        }
    }

    public enum LocaleTabType
    {
        Setting,
        Explore
    }
}