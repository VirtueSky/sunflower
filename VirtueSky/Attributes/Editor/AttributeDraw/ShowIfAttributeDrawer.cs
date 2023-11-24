namespace VirtueSky.Attributes
{
    using System;
    using System.Linq;
    using System.Reflection;
    using UnityEngine;
    using UnityEditor;

    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfAttributeDrawer : PropertyDrawer
    {
        private bool isFieldShow = true;

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
                            isFieldShow = conditionField.boolValue == comparationValue;
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
                        else if (UtilityDraw.IsEnum(paramEnum))
                        {
                            if (!UtilityDraw.CheckSameEnumType(new[] { paramEnum.GetType() }, property.serializedObject.targetObject.GetType(), conditionField.propertyPath))
                            {
                                ShowError(position, label, "Enum Types doesn't match");
                                return;
                            }
                            else
                            {
                                string enumValue = Enum.GetValues(paramEnum.GetType()).GetValue(conditionField.enumValueIndex).ToString();
                                if (paramEnum.ToString() != enumValue)
                                    isFieldShow = false;
                                else
                                    isFieldShow = true;
                            }
                        }
                        else if (UtilityDraw.IsEnum(paramEnumArray))
                        {
                            if (!UtilityDraw.CheckSameEnumType(paramEnumArray.Select(x => x.GetType()), property.serializedObject.targetObject.GetType(),
                                    conditionField.propertyPath))
                            {
                                ShowError(position, label, "Enum Types doesn't match");
                                return;
                            }
                            else
                            {
                                string enumValue = Enum.GetValues(paramEnumArray[0].GetType()).GetValue(conditionField.enumValueIndex).ToString();
                                if (paramEnumArray.All(x => x.ToString() != enumValue))
                                    isFieldShow = false;
                                else
                                    isFieldShow = true;
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
                            float? value = UtilityDraw.GetValue(stringValue, "==");
                            if (value == null)
                                error = true;
                            else
                                isFieldShow = conditionValue == value;
                        }
                        else if (stringValue.StartsWith("!="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "!=");
                            if (value == null)
                                error = true;
                            else
                                isFieldShow = conditionValue != value;
                        }
                        else if (stringValue.StartsWith("<="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "<=");
                            if (value == null)
                                error = true;
                            else
                                isFieldShow = conditionValue <= value;
                        }
                        else if (stringValue.StartsWith(">="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, ">=");
                            if (value == null)
                                error = true;
                            else
                                isFieldShow = conditionValue >= value;
                        }
                        else if (stringValue.StartsWith("<"))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "<");
                            if (value == null)
                                error = true;
                            else
                                isFieldShow = conditionValue < value;
                        }
                        else if (stringValue.StartsWith(">"))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, ">");
                            if (value == null)
                                error = true;
                            else
                                isFieldShow = conditionValue > value;
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
                bool comparationValue = attribute.comparationValue == null || (bool)attribute.comparationValue;
                isFieldShow = (bool)methodInfo.Invoke(property.serializedObject.targetObject, null) == comparationValue;
            }
            else if (propertyInfo != null)
            {
                bool comparationValue = attribute.comparationValue == null || (bool)attribute.comparationValue;
                isFieldShow = (bool)propertyInfo.GetValue(property.serializedObject.targetObject) == comparationValue;
            }

            if (isFieldShow)
            {
                EditorGUI.PropertyField(position, property, true);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (isFieldShow)
                return EditorGUI.GetPropertyHeight(property);
            else
                return -EditorGUIUtility.standardVerticalSpacing;
        }

        private void ShowError(Rect position, GUIContent label, string errorText)
        {
            EditorGUI.LabelField(position, label, new GUIContent(errorText));
            isFieldShow = true;
        }
    }
}