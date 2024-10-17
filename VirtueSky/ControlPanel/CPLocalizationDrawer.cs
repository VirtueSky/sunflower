using UnityEditor;
using UnityEngine;
using VirtueSky.Localization;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPLocalizationDrawer
    {
        private static LocaleTabType localeTabType = LocaleTabType.Setting;
        private static VirtueSky.Localization.LocaleSettings _config;
        private static UnityEditor.Editor _editor;

        public static void OnDrawLocalization()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.Localization, "Localization");
            GUILayout.Space(10);
            DrawTab();
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
                DrawSetting();
            }

            bool clickPickup = GUILayout.Toggle(localeTabType == LocaleTabType.Explore, "Explore",
                GUI.skin.button, GUILayout.ExpandWidth(true), GUILayout.Height(25));
            if (clickPickup && localeTabType != LocaleTabType.Explore)
            {
                localeTabType = LocaleTabType.Explore;
                DrawExplore();
            }

            EditorGUILayout.EndHorizontal();
        }

        private static void DrawSetting()
        {
            Debug.Log("draw setting");
        }

        private static void DrawExplore()
        {
            Debug.Log("draw explore");
        }
    }

    public enum LocaleTabType
    {
        Setting,
        Explore
    }
}