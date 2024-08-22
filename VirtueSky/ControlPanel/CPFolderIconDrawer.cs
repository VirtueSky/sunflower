using UnityEditor;
using UnityEngine;
using Virtuesky.FolderIcon.Editor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.ControlPanel.Editor
{
    public class CPFolderIconDrawer
    {
        private static UnityEditor.Editor _editor;
        private static FolderIconSettings _settings;
        private static Vector2 scroll = Vector2.zero;

        public static void OnEnable()
        {
            Init();
        }

        private static void Init()
        {
            if (_editor != null)
            {
                _editor = null;
            }

            _settings = CreateAsset.GetScriptableAsset<FolderIconSettings>();
            _editor = UnityEditor.Editor.CreateEditor(_settings);
        }

        public static void OnDrawFolderIcon()
        {
            GUILayout.Space(10);
            GUILayout.BeginVertical();
            CPUtility.DrawHeaderIcon(StatePanelControl.FolderIcon, "Folder Icon");
            GUILayout.Space(10);
            scroll = EditorGUILayout.BeginScrollView(scroll);
            if (_settings == null)
            {
                if (GUILayout.Button("Create FolderIconSetting"))
                {
                    _settings = CreateAsset.CreateAndGetScriptableAsset<FolderIconSettings>(
                        "Assets/_Sunflower/Editor/FolderIconSettings", useDefaultPath: false);
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
                else
                {
                    _editor.OnInspectorGUI();
                    GUILayout.Space(10);

                    if (_settings.enableFolderIcons)
                    {
                        if (GUILayout.Button("Clear cache"))
                        {
                            _settings.ClearCache();
                        }

                        GUILayout.Space(10);
                        if (GUILayout.Button("Import texture icon folder"))
                        {
                            AssetDatabase.ImportPackage(
                                FileExtension.GetPathFileInCurrentEnvironment(
                                    "VirtueSky/FolderIcon/Editor/PackageIcon/icon_folder.unitypackage"), false);
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
            GUILayout.EndVertical();
        }
    }
}