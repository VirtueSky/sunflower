using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using VirtueSky.Localization;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.LocalizationEditor
{
    public static class LocaleEditorUtil
    {
        public static void LocaleDrawLanguageField(Rect position, ref LocaleTreeViewItem localeItem, bool showOnlyBuiltin = false)
        {
            var languages = new List<Language>();
            languages.AddRange(Language.BuiltInLanguages);

            if (!showOnlyBuiltin) languages.AddRange(GetCustomLanguages());

            int currentValueIndex = -1;
            for (int i = 0; i < languages.Count; i++)
            {
                if (languages[i].Code == localeItem.LocaleItem.Language.Code)
                {
                    currentValueIndex = i;
                    break;
                }
            }

            if (currentValueIndex < 0)
            {
                currentValueIndex = languages.FindIndex(x => x.Code == Language.English.Code);
                Debug.Assert(currentValueIndex >= 0);
            }

            int newValueIndex = currentValueIndex;
            if (GUI.Button(position, languages[currentValueIndex].Name, EditorStyles.popup))
            {
                var searchWindow = ExSearchWindow.Create("Choose Language");

                foreach (var lang in languages)
                {
                    var cache = lang;
                    var item = localeItem;
                    searchWindow.AddEntry(lang.Name,
                        () =>
                        {
                            newValueIndex = languages.FindIndex(x => x.Code == cache.Code);

                            if (newValueIndex != currentValueIndex) item.LocaleItem.Language = languages[newValueIndex];
                        });
                }

                searchWindow.Open(position);
            }
        }

        public static void LanguageField(Rect position, SerializedProperty property, GUIContent label, bool showOnlyBuiltin = false)
        {
            var languages = new List<Language>();
            languages.AddRange(Language.BuiltInLanguages);

            if (!showOnlyBuiltin) languages.AddRange(GetCustomLanguages());

            EditorGUI.BeginProperty(position, label, property);

            var languageName = property.FindPropertyRelative("name");
            var languageCode = property.FindPropertyRelative("code");

            int currentValueIndex = languages.FindIndex(x => x.Code == languageCode.stringValue);
            if (currentValueIndex < 0)
            {
                currentValueIndex = languages.FindIndex(x => x == Language.English);
                Debug.Assert(currentValueIndex >= 0);
            }

            if (GUI.Button(position, languages[currentValueIndex].Name, EditorStyles.popup))
            {
                var searchWindow = ExSearchWindow.Create("Choose Language");
                foreach (var lang in languages)
                {
                    var cache = lang;
                    searchWindow.AddEntry(lang.Name,
                        () =>
                        {
                            int newValue = languages.FindIndex(x => x.Code == cache.Code);
                            if (newValue != currentValueIndex)
                            {
                                languageName.stringValue = languages[newValue].Name;
                                languageCode.stringValue = languages[newValue].Code;
                                property.serializedObject.ApplyModifiedProperties();
                            }
                        });
                }

                searchWindow.Open(position);
            }

            EditorGUI.EndProperty();
        }

        private static Language[] GetCustomLanguages()
        {
            if (LocaleSettings.Instance != null)
            {
                var customLanguages = LocaleSettings.AvailableLanguages.Where(x => x.Custom);
                return customLanguages.ToArray();
            }

            return Array.Empty<Language>();
        }

        public static Language GetLanguageValueFromProperty(SerializedProperty languageProperty)
        {
            var nameProperty = languageProperty.FindPropertyRelative("name");
            if (nameProperty == null) throw new ArgumentException("Language.Name property could not be found");

            var codeProperty = languageProperty.FindPropertyRelative("code");
            if (codeProperty == null) throw new ArgumentException("Language.Code property could not be found");

            var customProperty = languageProperty.FindPropertyRelative("custom");
            if (customProperty == null) throw new ArgumentException("Language.Custom property could not be found");

            return new Language(nameProperty.stringValue, codeProperty.stringValue, customProperty.boolValue);
        }

        public static void SetLanguageProperty(SerializedProperty languageProperty, string name, string code, bool custom)
        {
            var nameProperty = languageProperty.FindPropertyRelative("name");
            if (nameProperty == null) throw new ArgumentException("Language.Name property could not be found");

            var codeProperty = languageProperty.FindPropertyRelative("code");
            if (codeProperty == null) throw new ArgumentException("Language.Code property could not be found");

            var customProperty = languageProperty.FindPropertyRelative("custom");
            if (customProperty == null) throw new ArgumentException("Language.Custom property could not be found");

            nameProperty.stringValue = name;
            codeProperty.stringValue = code;
            customProperty.boolValue = custom;
        }

        public static void SetLanguageProperty(SerializedProperty languageProperty, Language language)
        {
            SetLanguageProperty(languageProperty, language.Name, language.Code, language.Custom);
        }

        /// <summary>
        /// Import CSV file
        /// </summary>
        public static void Import()
        {
            string path = EditorUtility.OpenFilePanel("Import CSV file", "", "csv");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                {
                    var serialization = new CsvSerialization();
                    serialization.Deserialize(stream);
                }

                Debug.Log("CSV file has been imported.");
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        /// <summary>
        /// Export CSV file
        /// </summary>
        public static void Export()
        {
            string fileName = Application.productName + "-" + DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            string path = EditorUtility.SaveFilePanel("Export CSV file", "", fileName, "csv");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            try
            {
                using (var stream = File.OpenWrite(path))
                {
                    var serialization = new CsvSerialization();
                    serialization.Serialize(stream);
                }

                Debug.Log("CSV file has been exported to " + path);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}