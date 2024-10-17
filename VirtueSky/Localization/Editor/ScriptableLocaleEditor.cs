using System;
using System.Collections.Generic;
using System.Linq;
using VirtueSky.Localization;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace VirtueSky.LocalizationEditor
{
    [CustomEditor(typeof(ScriptableLocaleBase), true)]
    public class ScriptableLocaleEditor : UnityEditor.Editor
    {
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

                _reorderable.drawElementCallback = (rect, index, isActive, isFocused) =>
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
                    var elementHeight = EditorGUIUtility.singleLineHeight;

                    if (assetValueType == typeof(string))
                    {
                        var valueWidth = _currentLayoutRect.width - 100 - 30;
                        elementHeight = TextAreaStyle.CalcHeight(new GUIContent(valueProperty.stringValue), valueWidth);
                    }

                    return Mathf.Max(EditorGUIUtility.singleLineHeight, elementHeight) + 4;
                };
                _reorderable.onAddDropdownCallback = (Rect buttonRect, ReorderableList list) =>
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
            if (_reorderable != null)
            {
                _currentLayoutRect = GUILayoutUtility.GetRect(0, _reorderable.GetHeight(), GUILayout.ExpandWidth(true));
                serializedObject.Update();
                _reorderable.DoList(_currentLayoutRect);
                serializedObject.ApplyModifiedProperties();

                var helpRect = _currentLayoutRect;
                helpRect.y += _reorderable.GetHeight() + 4;
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

            var startIndex = _itemsProperty.arraySize;
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
                var languageCode = languageProperty.FindPropertyRelative("code").stringValue;
                if (languageCode == language.Code)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Adds a locale and the value or updates if specified language is exists.
        /// </summary>
        public static bool AddOrUpdateLocale(ScriptableLocaleBase localizedAsset, Language language, object value)
        {
            var serializedObject = new SerializedObject(localizedAsset);
            serializedObject.Update();

            var elements = serializedObject.FindProperty("items");
            if (elements != null && elements.arraySize > 0)
            {
                var index = Array.FindIndex(localizedAsset.LocaleItems, x => x.Language == language);
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
                var index = Array.IndexOf(localizedAsset.LocaleItems, localeItem);
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