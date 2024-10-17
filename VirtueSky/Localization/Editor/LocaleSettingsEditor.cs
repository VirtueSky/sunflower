using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtueSky.Localization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VirtueSky.LocalizationEditor
{
    [CustomEditor(typeof(LocaleSettings), true)]
    public class LocaleSettingsEditor : UnityEditor.Editor
    {
        private ReorderableList _reorderableList;
        private SerializedProperty _avaiableLanguageProperty;
        private SerializedProperty _detectDeviceLanguageProperty;
        private SerializedProperty _importLocationProperty;
        private SerializedProperty _googleCredentialProperty;

        private void Init()
        {
            _avaiableLanguageProperty ??= serializedObject.FindProperty("availableLanguages");
            _detectDeviceLanguageProperty ??= serializedObject.FindProperty("detectDeviceLanguage");
            _importLocationProperty ??= serializedObject.FindProperty("importLocation");
            _googleCredentialProperty ??= serializedObject.FindProperty("googleCredential");

            if (_avaiableLanguageProperty != null)
            {
                _reorderableList = new ReorderableList(serializedObject,
                    _avaiableLanguageProperty,
                    true,
                    true,
                    true,
                    true) { drawHeaderCallback = OnDrawHeaderCallback, drawElementCallback = OnDrawElementCallback };

                _reorderableList.onAddDropdownCallback += OnAddDropdownCallback;
                _reorderableList.onRemoveCallback += OnRemoveCallback;
                _reorderableList.onCanRemoveCallback += OnCanRemoveCallback;
            }
        }

        private bool OnCanRemoveCallback(ReorderableList list)
        {
            return list.count > 1;
        }

        private void OnDrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
        {
            var languageProperty = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var position = new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);

            var isCustom = languageProperty.FindPropertyRelative("custom").boolValue;
            if (isCustom)
            {
                var languageName = languageProperty.FindPropertyRelative("name");
                var languageCode = languageProperty.FindPropertyRelative("code");

                var labelWidth = EditorGUIUtility.labelWidth;

                EditorGUIUtility.labelWidth = 40;
                var r1 = new Rect(position.x, position.y, position.width / 2 - 2, position.height);
                EditorGUI.PropertyField(r1, languageName, new GUIContent(languageName.displayName, "Language name"));

                EditorGUIUtility.labelWidth = 40;
                var r2 = new Rect(position.x + r1.width + 4, position.y, position.width / 2 - 2, position.height);
                EditorGUI.PropertyField(r2, languageCode, new GUIContent(languageCode.displayName, "ISO-639-1 code"));

                EditorGUIUtility.labelWidth = labelWidth;
            }
            else
            {
                LocaleEditorUtil.LanguageField(position, languageProperty, GUIContent.none, true);
            }
        }


        private void OnDrawHeaderCallback(Rect rect)
        {
            EditorGUI.LabelField(rect, "Available Languages");
        }

        private void OnAddDropdownCallback(Rect buttonrect, ReorderableList list)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Language", "Adds built-in language."),
                false,
                () =>
                {
                    ReorderableList.defaultBehaviours.DoAddButton(list);

                    var languageProperty = list.serializedProperty.GetArrayElementAtIndex(list.index);
                    LocaleEditorUtil.SetLanguageProperty(languageProperty, Language.BuiltInLanguages[0]);

                    serializedObject.ApplyModifiedProperties();
                });
            menu.AddItem(new GUIContent("Custom language", "Adds custom language."),
                false,
                () =>
                {
                    ReorderableList.defaultBehaviours.DoAddButton(list);

                    var languageProperty = list.serializedProperty.GetArrayElementAtIndex(list.index);
                    LocaleEditorUtil.SetLanguageProperty(languageProperty, "", "", true);

                    serializedObject.ApplyModifiedProperties();
                });
            menu.AddItem(new GUIContent("Adds languages in-use", "Adds by searching used languages in assets."),
                false,
                () =>
                {
                    AddUsedLocales();
                    serializedObject.ApplyModifiedProperties();
                });
            menu.ShowAsContext();
        }

        private void OnRemoveCallback(ReorderableList list)
        {
            var languageProperty = list.serializedProperty.GetArrayElementAtIndex(list.index);
            var language = LocaleEditorUtil.GetLanguageValueFromProperty(languageProperty);
            if (language.Custom)
            {
                var localizedAssets = Locale.FindAllLocalizedAssets();
                if (localizedAssets.Any(x => x.LocaleItems.Any(y => y.Language == language)))
                {
                    if (!EditorUtility.DisplayDialog("Remove \"" + language + "\" language?",
                            "\"" + language + "\" language is in-use by some localized assets." + " Are you sure to remove?",
                            "Remove",
                            "Cancel"))
                    {
                        return; // Cancelled.
                    }
                }
            }

            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }

        private void AddUsedLocales()
        {
            var languages = FindUsedLanguages();
            _avaiableLanguageProperty.arraySize = languages.Length;
            for (var i = 0; i < _avaiableLanguageProperty.arraySize; i++)
            {
                var languageProperty = _avaiableLanguageProperty.GetArrayElementAtIndex(i);
                LocaleEditorUtil.SetLanguageProperty(languageProperty, languages[i]);
            }
        }

        private Language[] FindUsedLanguages()
        {
            var languages = new HashSet<Language>();
            for (var i = 0; i < _avaiableLanguageProperty.arraySize; i++)
            {
                languages.Add(LocaleEditorUtil.GetLanguageValueFromProperty(_avaiableLanguageProperty.GetArrayElementAtIndex(i)));
            }

            var localizedAssets = Locale.FindAllLocalizedAssets();
            foreach (var localizedAsset in localizedAssets)
            {
                foreach (var locale in localizedAsset.LocaleItems)
                {
                    languages.Add(locale.Language);
                }
            }

            return languages.ToArray();
        }

        public override void OnInspectorGUI()
        {
            Init();

            if (_reorderableList != null)
            {
                serializedObject.Update();

                EditorGUILayout.Separator();

                _reorderableList.DoLayoutList();

                EditorGUILayout.PropertyField(_detectDeviceLanguageProperty, true);
                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(_importLocationProperty.displayName);
                if (GUILayout.Button(_importLocationProperty.stringValue, EditorStyles.objectField))
                {
                    var path = EditorUtility.OpenFolderPanel("Select folder for import location", "Assets/", "");
                    if (Directory.Exists(path))
                    {
                        path = "Assets" + path.Replace(Application.dataPath, "");
                        if (AssetDatabase.IsValidFolder(path))
                        {
                            _importLocationProperty.stringValue = path;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.Separator();

                EditorGUILayout.PropertyField(_googleCredentialProperty);
                if (string.IsNullOrEmpty(_googleCredentialProperty.stringValue))
                {
                    EditorGUILayout.HelpBox("If you want to use Google Translate in editor or in-game, attach the API key file claimed from Google Cloud.",
                        MessageType.Info);
                }
                else
                {
                    EditorGUILayout.HelpBox("Replace with your API key", MessageType.Info);
                }

                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }
    }
}