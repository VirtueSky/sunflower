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
            GUILayout.Label("FOLDER ICON", EditorStyles.boldLabel);
            GUILayout.Space(10);
            if (_settings == null)
            {
                if (GUILayout.Button("Create FolderIconSetting"))
                {
                    _settings = CreateAsset.CreateAndGetScriptableAsset<FolderIconSettings>(
                        "Assets/_Sunflower/Editor/FolderIcon");
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
                    if (_settings.customIcon)
                    {
                        if (GUILayout.Button("Import texture icon folder"))
                        {
                            AssetDatabase.ImportPackage(
                                FileExtension.GetPathInCurrentEnvironent(
                                    "VirtueSky/FolderIcon/Editor/PackageIcon/icon_folder.unitypackage"), false);
                        }
                    }
                }
            }


            GUILayout.EndVertical();
        }
    }
}