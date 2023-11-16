namespace VirtueSky.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        private bool showField = true;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            ShowIfAttribute attribute = (ShowIfAttribute)this.attribute;

            FieldInfo fieldInfo = property.serializedObject.targetObject.GetType()
                .GetField(attribute.conditionFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            PropertyInfo propertyInfo = property.serializedObject.targetObject.GetType()
                .GetProperty(attribute.conditionFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo methodInfo = property.serializedObject.targetObject.GetType()
                .GetMethod(attribute.conditionFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfo != null)
            {
                SerializedProperty conditionField = property.serializedObject.FindProperty(attribute.conditionFieldName);
                // We check that exist a Field with the parameter name
                if (conditionField == null)
                {
                    ShowError(position, label, "Error getting the condition Field. Check the name.");
                    return;
                }

                switch (conditionField.propertyType)
                {
                    case SerializedPropertyType.Boolean:
                        try
                        {
                            bool comparationValue = attribute.comparationValue == null || (bool)attribute.comparationValue;
                            showField = conditionField.boolValue == comparationValue;
                        }
                        catch
                        {
                            ShowError(position, label, "Invalid comparation Value Type");
                            return;
                        }

                        break;
                    case SerializedPropertyType.Enum:
                        object paramEnum = attribute.comparationValue;
                        object[] paramEnumArray = attribute.comparationValueArray;

                        if (paramEnum == null && paramEnumArray == null)
                        {
                            ShowError(position, label, "The comparation enum value is null");
                            return;
                        }
                        else if (IsEnum(paramEnum))
                        {
                            if (!CheckSameEnumType(new[] { paramEnum.GetType() }, property.serializedObject.targetObject.GetType(), conditionField.propertyPath))
                            {
                                ShowError(position, label, "Enum Types doesn't match");
                                return;
                            }
                            else
                            {
                                string enumValue = Enum.GetValues(paramEnum.GetType()).GetValue(conditionField.enumValueIndex).ToString();
                                if (paramEnum.ToString() != enumValue)
                                    showField = false;
                                else
                                    showField = true;
                            }
                        }
                        else if (IsEnum(paramEnumArray))
                        {
                            if (!CheckSameEnumType(paramEnumArray.Select(x => x.GetType()), property.serializedObject.targetObject.GetType(), conditionField.propertyPath))
                            {
                                ShowError(position, label, "Enum Types doesn't match");
                                return;
                            }
                            else
                            {
                                string enumValue = Enum.GetValues(paramEnumArray[0].GetType()).GetValue(conditionField.enumValueIndex).ToString();
                                if (paramEnumArray.All(x => x.ToString() != enumValue))
                                    showField = false;
                                else
                                    showField = true;
                            }
                        }
                        else
                        {
                            ShowError(position, label, "The comparation enum value is not an enum");
                            return;
                        }

                        break;
                    case SerializedPropertyType.Integer:
                    case SerializedPropertyType.Float:
                        string stringValue;
                        bool error = false;

                        float conditionValue = 0;
                        if (conditionField.propertyType == SerializedPropertyType.Integer)
                            conditionValue = conditionField.intValue;
                        else if (conditionField.propertyType == SerializedPropertyType.Float)
                            conditionValue = conditionField.floatValue;

                        try
                        {
                            stringValue = (string)attribute.comparationValue;
                        }
                        catch
                        {
                            ShowError(position, label, "Invalid comparation Value Type");
                            return;
                        }

                        if (stringValue.StartsWith("=="))
                        {
                            float? value = GetValue(stringValue, "==");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue == value;
                        }
                        else if (stringValue.StartsWith("!="))
                        {
                            float? value = GetValue(stringValue, "!=");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue != value;
                        }
                        else if (stringValue.StartsWith("<="))
                        {
                            float? value = GetValue(stringValue, "<=");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue <= value;
                        }
                        else if (stringValue.StartsWith(">="))
                        {
                            float? value = GetValue(stringValue, ">=");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue >= value;
                        }
                        else if (stringValue.StartsWith("<"))
                        {
                            float? value = GetValue(stringValue, "<");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue < value;
                        }
                        else if (stringValue.StartsWith(">"))
                        {
                            float? value = GetValue(stringValue, ">");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue > value;
                        }

                        if (error)
                        {
                            ShowError(position, label, "Invalid comparation instruction for Int or float value");
                            return;
                        }

                        break;
                    default:
                        ShowError(position, label, "This type has not supported.");
                        return;
                }
            }

            else if (methodInfo != null)
            {
                showField = (bool)methodInfo.Invoke(property.serializedObject.targetObject, null) == (bool)attribute.comparationValue;
            }
            else if (propertyInfo != null)
            {
                showField = (bool)propertyInfo.GetValue(property.serializedObject.targetObject) == (bool)attribute.comparationValue;
            }

            if (showField)
            {
                EditorGUI.PropertyField(position, property, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (showField)
                return EditorGUI.GetPropertyHeight(property);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }

        /// <summary>
        /// Return if the object is enum and not null
        /// </summary>
        private static bool IsEnum(object obj)
        {
            return obj != null && obj.GetType().IsEnum;
        }

        /// <summary>
        /// Return if all the objects are enums and not null
        /// </summary>
        private static bool IsEnum(object[] obj)
        {
            return obj != null && obj.All(o => o.GetType().IsEnum);
        }

        /// <summary>
        /// Check if the field with name "fieldName" has the same class as the "checkTypes" classes through reflection
        /// </summary>
        private static bool CheckSameEnumType(IEnumerable<Type> checkTypes, Type classType, string fieldName)
        {
            string[] fieldNames = fieldName.Split('.');
            Type currentType = classType;

            foreach (var name in fieldNames)
            {
                FieldInfo field = currentType.GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                PropertyInfo property = currentType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (field != null)
                {
                    currentType = field.FieldType;
                }
                else if (property != null)
                {
                    currentType = property.PropertyType;
                }
                else
                {
                    return false;
                }
            }

            return checkTypes.All(x => x == currentType);
        }

        private void ShowError(Rect position, GUIContent label, string errorText)
        {
            EditorGUI.LabelField(position, label, new GUIContent(errorText));
            showField = true;
        }

        /// <summary>
        /// Return the float value in the content string removing the remove string
        /// </summary>
        private static float? GetValue(string content, string remove)
        {
            string removed = content.Replace(remove, "");
            try
            {
                return float.Parse(removed);
            }
            catch
            {
                return null;
            }
        }
    }
}