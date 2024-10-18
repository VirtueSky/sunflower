using System.Collections.Generic;
using System.IO;
using System.Linq;

#if VIRTUESKY_BAKINGSHEET
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
#endif

using VirtueSky.Localization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.LocalizationEditor
{
    [CustomEditor(typeof(LocaleSettings), true)]
    public class LocaleSettingsEditor : Editor
    {
        private ReorderableList _reorderableList;
        private SerializedProperty _avaiableLanguageProperty;
        private SerializedProperty _detectDeviceLanguageProperty;
        private SerializedProperty _importLocationProperty;
        private SerializedProperty _googleTranslateApiKeyProperty;
        private SerializedProperty _spreadsheetKeyProperty;
        private SerializedProperty _serviceAccountCredentialProperty;

        private void Init()
        {
            _avaiableLanguageProperty ??= serializedObject.FindProperty("availableLanguages");
            _detectDeviceLanguageProperty ??= serializedObject.FindProperty("detectDeviceLanguage");
            _importLocationProperty ??= serializedObject.FindProperty("importLocation");
            _googleTranslateApiKeyProperty ??= serializedObject.FindProperty("googleTranslateApiKey");
            _spreadsheetKeyProperty ??= serializedObject.FindProperty("spreadsheetKey");
            _serviceAccountCredentialProperty ??= serializedObject.FindProperty("serviceAccountCredential");

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

            bool isCustom = languageProperty.FindPropertyRelative("custom").boolValue;
            if (isCustom)
            {
                var languageName = languageProperty.FindPropertyRelative("name");
                var languageCode = languageProperty.FindPropertyRelative("code");

                float labelWidth = EditorGUIUtility.labelWidth;

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
                            "\"" + language + "\" language is in-use by some localized assets." +
                            " Are you sure to remove?",
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
                languages.Add(
                    LocaleEditorUtil.GetLanguageValueFromProperty(_avaiableLanguageProperty.GetArrayElementAtIndex(i)));
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
                EditorGUILayout.Separator();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(_importLocationProperty.displayName);
                if (GUILayout.Button(_importLocationProperty.stringValue, EditorStyles.objectField))
                {
                    string path = EditorUtility.OpenFolderPanel("Select folder for import location", "Assets/", "");
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
                EditorGUILayout.PropertyField(_googleTranslateApiKeyProperty);
                if (string.IsNullOrEmpty(_googleTranslateApiKeyProperty.stringValue))
                {
                    EditorGUILayout.HelpBox(
                        "If you want to use Google Translate in editor or in-game, attach the API key file claimed from Google Cloud.",
                        MessageType.Info);
                }
                else
                {
                    if (_googleTranslateApiKeyProperty.stringValue.StartsWith("AIzaSyCdaIrr") &&
                        _googleTranslateApiKeyProperty.stringValue.EndsWith("120Dy-mfz6I"))
                        EditorGUILayout.HelpBox("Do not use this key. Replace with your API key", MessageType.Info);
                }

                EditorGUILayout.Separator();
#if VIRTUESKY_BAKINGSHEET
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_spreadsheetKeyProperty);
                if (GUILayout.Button("Open", GUILayout.Width(65)))
                {
                    Application.OpenURL(
                        $"https://docs.google.com/spreadsheets/d/{_spreadsheetKeyProperty.stringValue}");
                }

                GUI.backgroundColor = Uniform.Green;
                GUI.enabled = !EditorApplication.isCompiling && !SessionState.GetBool("spreasheet_importing", false);
                if (GUILayout.Button("Import", GUILayout.Width(65)))
                {
                    if (EditorUtility.DisplayDialog("Import Locale From Spreasheet",
                            "Are you sure you wish import Locale from Spreasheet?\nThis action cannot be reversed.",
                            "Ok",
                            "Cancel"))
                    {
                        ImportFormSpreadsheet();
                    }
                }

                GUI.enabled = true;
                GUI.backgroundColor = Color.white;
                EditorGUILayout.EndHorizontal();
#else
                EditorGUILayout.HelpBox(
                    $"Add scripting define symbols: {ConstantDefineSymbols.VIRTUESKY_BAKINGSHEET} and install sdk to use spread sheet",
                    MessageType.Info);
#endif

                EditorGUILayout.PropertyField(_serviceAccountCredentialProperty, GUILayout.MaxHeight(150));
                serializedObject.ApplyModifiedProperties();
            }
            else
            {
                base.OnInspectorGUI();
            }
        }

        private async void ImportFormSpreadsheet()
        {
            SessionState.SetBool("spreasheet_importing", true);
            string importLocation = LocaleSettings.ImportLocation;
            var localizedTexts = Locale.FindAllLocalizedAssets<LocaleText>();
#if VIRTUESKY_BAKINGSHEET
                    using (var service = new SheetsService(new BaseClientService.Initializer
                   {
                       HttpClientInitializer = GoogleCredential.FromJson(_serviceAccountCredentialProperty.stringValue)
                           .CreateScoped(DriveService.Scope.DriveReadonly)
                   }))
            {
                var sheetReq = service.Spreadsheets.Get(_spreadsheetKeyProperty.stringValue);
                sheetReq.Fields = "properties,sheets(properties,data.rowData.values.formattedValue)";
                var spreadsheet = await sheetReq.ExecuteAsync();

                var languages = new List<Language>();
                var availableLanguages = LocaleSettings.AllLanguages;
                foreach (var s in spreadsheet.Sheets)
                {
                    if (!s.Properties.Title.Equals(Application.productName)) continue;
                    foreach (var g in s.Data)
                    {
                        for (var i = 1; i < g.RowData[0].Values.Count; i++)
                        {
                            string code = g.RowData[0].Values[i].FormattedValue;
                            var language = availableLanguages.FirstOrDefault(x => x.Code == code);
                            if (language == null)
                                Debug.LogWarning("Language code (" + code + ") not exist in localization system.");

                            // Add null language as well to maintain order.
                            languages.Add(language);
                        }

                        for (var i = 1; i < g.RowData.Count; i++)
                        {
                            var row = g.RowData[i].Values;
                            var key = row[0];
                            var localizedText = localizedTexts.FirstOrDefault(x => x.name == key.FormattedValue);
                            if (localizedText == null)
                            {
                                localizedText = CreateInstance<LocaleText>();

                                string assetPath = Path.Combine(importLocation, $"{key}.asset");
                                AssetDatabase.CreateAsset(localizedText, assetPath);
                                AssetDatabase.SaveAssets();
                            }

                            // Read languages by ignoring first column (Key).
                            for (var j = 1; j < row.Count; j++)
                            {
                                ScriptableLocaleEditor.AddOrUpdateLocale(localizedText, languages[j - 1],
                                    row[j].FormattedValue);
                            }

                            EditorUtility.SetDirty(localizedText);
                        }
                    }
                }

                AssetDatabase.Refresh();
            }

            Debug.Log("[Localization] The import process from spreasheet is complete");
            SessionState.SetBool("spreasheet_importing", false);
#endif
        }
    }
}