using System;
using System.Collections.Generic;
using System.Linq;
using VirtueSky.Localization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VirtueSky.Misc;
using VirtueSky.Utils;

namespace VirtueSky.LocalizationEditor
{
    [CustomEditor(typeof(ScriptableLocaleBase), true)]
    [CanEditMultipleObjects]
    public class ScriptableLocaleEditor : UnityEditor.Editor
    {
        private static GoogleTranslator Translator => new(LocaleSettings.GoogleTranslateApiKey);
        private ReorderableList _reorderable;
        private SerializedProperty _itemsProperty;
        private Rect _currentLayoutRect;
        private GUIStyle _textAreaStyle;

        private GUIStyle TextAreaStyle
        {
            get { return _textAreaStyle ??= new GUIStyle(EditorStyles.textArea) { wordWrap = true }; }
        }

        private void OnEnable()
        {
            if (target == null) return;
            var assetValueType = ((ScriptableLocaleBase)target).GetGenericType;
            _itemsProperty = serializedObject.FindProperty("items");
            if (_itemsProperty != null)
            {
                _reorderable = new ReorderableList(serializedObject: serializedObject,
                    elements: _itemsProperty,
                    draggable: true,
                    displayHeader: true,
                    displayAddButton: true,
                    displayRemoveButton: true)
                {
                    drawHeaderCallback = rect => { EditorGUI.LabelField(rect, ObjectNames.NicifyVariableName(target.GetType().Name) + "s"); }
                };

                _reorderable.drawElementCallback = (rect, index, _, _) =>
                {
                    var element = _reorderable.serializedProperty.GetArrayElementAtIndex(index);

                    // Language field.
                    var languageRect = new Rect(rect.x, rect.y + 2, 100, rect.height - 4);
                    var languageProperty = element.FindPropertyRelative("language");
                    EditorGUI.PropertyField(languageRect, languageProperty, GUIContent.none);

                    // Value field.
                    var valueRect = new Rect(languageRect.x + languageRect.width + 4, languageRect.y, rect.width - languageRect.width - 4, rect.height - 4);
                    var valueProperty = element.FindPropertyRelative("value");
                    if (assetValueType == typeof(string))
                    {
                        valueProperty.stringValue = EditorGUI.TextArea(valueRect, valueProperty.stringValue, TextAreaStyle);
                    }
                    else
                    {
                        EditorGUI.PropertyField(valueRect, valueProperty, GUIContent.none);
                    }
                };
                _reorderable.onCanRemoveCallback = list => list.count > 1;
                _reorderable.elementHeightCallback = index =>
                {
                    var element = _reorderable.serializedProperty.GetArrayElementAtIndex(index);
                    var valueProperty = element.FindPropertyRelative("value");
                    float elementHeight = EditorGUIUtility.singleLineHeight;

                    if (assetValueType == typeof(string))
                    {
                        float valueWidth = _currentLayoutRect.width - 100 - 30;
                        elementHeight = TextAreaStyle.CalcHeight(new GUIContent(valueProperty.stringValue), valueWidth);
                    }

                    return Mathf.Max(EditorGUIUtility.singleLineHeight, elementHeight) + 4;
                };
                _reorderable.onAddDropdownCallback = (_, list) =>
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Language", "Adds a language."),
                        false,
                        () =>
                        {
                            ReorderableList.defaultBehaviours.DoAddButton(list);
                            serializedObject.ApplyModifiedProperties();
                        });
                    menu.AddItem(new GUIContent("Add languages from settings", "Adds by searching used languages in assets."),
                        false,
                        () =>
                        {
                            AddLanguagesFromSettings();
                            serializedObject.ApplyModifiedProperties();
                        });
                    menu.AddItem(new GUIContent("Add all languages", "Adds all languages."),
                        false,
                        () =>
                        {
                            AddAllLanguages();
                            serializedObject.ApplyModifiedProperties();
                        });
                    menu.ShowAsContext();
                };
            }
        }

        public override void OnInspectorGUI()
        {
            if (Selection.objects.Length > 1)
            {
                EditorGUILayout.HelpBox("Currently does not support editing multiple Locales at the same time. Just choose one", MessageType.Warning);
                return;
            }

            if (_reorderable != null)
            {
                _currentLayoutRect = GUILayoutUtility.GetRect(0, _reorderable.GetHeight(), GUILayout.ExpandWidth(true));
                serializedObject.Update();
                _reorderable.DoList(_currentLayoutRect);
                serializedObject.ApplyModifiedProperties();

                Rect helpRect;
                if (target is LocaleText localeText)
                {
                    if (GUILayout.Button("Translate"))
                    {
                        Debug.Log("[Localization] Starting Translate LocaleText: ".SetColor(Color.cyan) + localeText.name);
                        var firstLocale = localeText.TypedLocaleItems.First();
                        foreach (var locale in localeText.TypedLocaleItems)
                        {
                            if (!string.IsNullOrEmpty(locale.Value)) continue;

                            var localeItem = locale;
                            Translator.Translate(new GoogleTranslateRequest(firstLocale.Language, locale.Language, firstLocale.Value),
                                e =>
                                {
                                    var response = e.Responses.FirstOrDefault();
                                    if (response != null)
                                    {
                                        localeItem.Value = response.translatedText;
                                        Debug.Log("[Localization] Translate Successfull: ".SetColor(CustomColor.Green) + localeText.name);
                                    }

                                    EditorUtility.SetDirty(localeText);
                                },
                                e => { Debug.LogError("Response (" + e.ResponseCode + "): " + e.Message); });
                        }
                    }

                    if (GUILayout.Button("Fill Language Same Avaiable Language"))
                    {
                        Debug.Log("[Localization] Starting fill language same with AvaiableLanguage for LocaleText!".SetColor(Color.cyan));

                        foreach (var item in localeText.LocaleItems.ToList())
                        {
                            var result = localeText.LocaleItems.Select(itemBase => new LocaleItem<string>(itemBase.Language, itemBase.ObjectValue.ToString())).ToList();

                            // remove duplicate
                            for (var i = 0; i < result.ToList().Count - 1; i++)
                            {
                                var cache = result[i];
                                for (var j = 0; j < result.ToList().Count; j++)
                                {
                                    if (j == i) continue;
                                    if (result[j].Language == cache.Language) result.RemoveAt(j);
                                }
                            }

                            int index = Array.FindIndex(localeText.LocaleItems, x => x.Language == item.Language);
                            if (!LocaleSettings.AvailableLanguages.Contains(localeText.LocaleItems[index].Language)) result.RemoveAt(index);

                            localeText.SetLocaleItems(result);
                        }

                        if (localeText.LocaleItems.Length < LocaleSettings.AvailableLanguages.Count)
                        {
                            foreach (var lang in LocaleSettings.AvailableLanguages)
                            {
                                int index = Array.FindIndex(localeText.LocaleItems, x => x.Language == lang);
                                if (index >= 0) continue;

                                AddLocale(localeText);
                                index = localeText.LocaleItems.Length - 1;
                                var localeItem = localeText.LocaleItems[index];
                                localeItem.Language = lang;
                                localeItem.ObjectValue = "";
                            }
                        }

                        Debug.Log("[Localization] Fill language same with AvaiableLanguage Successfull: ".SetColor(CustomColor.Green) + localeText.name);
                    }

                    helpRect = _currentLayoutRect;
                    helpRect.y += _reorderable.GetHeight() + 50;
                }
                else
                {
                    helpRect = _currentLayoutRect;
                    helpRect.y += _reorderable.GetHeight() + 4;
                }

                helpRect.height = EditorGUIUtility.singleLineHeight * 1.5f;
                EditorGUI.HelpBox(helpRect, "First locale item is used as fallback if needed.", MessageType.Info);
            }
            else
            {
                base.OnInspectorGUI();

                EditorGUILayout.HelpBox("Make sure that locale items variable name is declared as \"m_LocaleItems\" and it is serializable.", MessageType.Error);
            }
        }

        private void AddAllLanguages()
        {
            AddLanguages(LocaleSettings.AllLanguages);
        }

        private void AddLanguagesFromSettings()
        {
            AddLanguages(LocaleSettings.AvailableLanguages);
        }

        private void AddLanguages(List<Language> languages)
        {
            var filteredLanguages = languages.Where(x => !IsLanguageExist(_itemsProperty, x)).ToArray();

            int startIndex = _itemsProperty.arraySize;
            _itemsProperty.arraySize += filteredLanguages.Length;

            for (var i = 0; i < filteredLanguages.Length; i++)
            {
                var localeItem = _itemsProperty.GetArrayElementAtIndex(startIndex + i);

                var localeItemValue = localeItem.FindPropertyRelative("value");
                switch (localeItemValue.propertyType)
                {
                    case SerializedPropertyType.String:
                        localeItemValue.stringValue = "";
                        break;
                    default:
                        localeItemValue.objectReferenceValue = null;
                        break;
                }

                var localeItemLanguage = localeItem.FindPropertyRelative("language");
                LocaleEditorUtil.SetLanguageProperty(localeItemLanguage, filteredLanguages[i]);
            }
        }

        private static bool IsLanguageExist(SerializedProperty localeItemsProperty, Language language)
        {
            for (var i = 0; i < localeItemsProperty.arraySize; i++)
            {
                var element = localeItemsProperty.GetArrayElementAtIndex(i);
                var languageProperty = element.FindPropertyRelative("language");
                string languageCode = languageProperty.FindPropertyRelative("code").stringValue;
                if (languageCode == language.Code)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a locale and the value or updates if specified language is existing.
        /// </summary>
        public static bool AddOrUpdateLocale(ScriptableLocaleBase localizedAsset, Language language, object value)
        {
            var serializedObject = new SerializedObject(localizedAsset);
            serializedObject.Update();

            var elements = serializedObject.FindProperty("items");
            if (elements != null && elements.arraySize > 0)
            {
                int index = Array.FindIndex(localizedAsset.LocaleItems, x => x.Language == language);
                if (index < 0)
                {
                    AddLocale(localizedAsset);
                    index = localizedAsset.LocaleItems.Length - 1;
                }

                var localeItem = localizedAsset.LocaleItems[index];
                localeItem.Language = language;
                localeItem.ObjectValue = value;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Adds a locale end of the list by copying last one.
        /// </summary>
        public static bool AddLocale(ScriptableLocaleBase localizedAsset)
        {
            var serializedObject = new SerializedObject(localizedAsset);
            serializedObject.Update();

            var elements = serializedObject.FindProperty("items");
            if (elements != null)
            {
                elements.arraySize += 1;
                serializedObject.ApplyModifiedProperties();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes specified locale item from the list.
        /// </summary>
        public static bool RemoveLocale(ScriptableLocaleBase localizedAsset, LocaleItemBase localeItem)
        {
            var serializedObject = new SerializedObject(localizedAsset);
            serializedObject.Update();

            var elements = serializedObject.FindProperty("items");
            if (elements != null && elements.arraySize > 1)
            {
                int index = Array.IndexOf(localizedAsset.LocaleItems, localeItem);
                if (index >= 0)
                {
                    elements.DeleteArrayElementAtIndex(index);
                    serializedObject.ApplyModifiedProperties();
                    return true;
                }
            }

            return false;
        }
    }
}