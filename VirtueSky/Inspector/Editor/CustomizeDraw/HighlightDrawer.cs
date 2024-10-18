using System;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using VirtueSky.Utils;

namespace VirtueSky.Inspector
{
    [CustomPropertyDrawer(typeof(HighlightAttribute))]
    public class HighlightDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var highlightAttribute = attribute as HighlightAttribute;

            bool doHighlight = true;

            if (!string.IsNullOrEmpty(highlightAttribute.validateField))
            {
                var t = property.serializedObject.targetObject.GetType();
                var methodInfo = t.GetMethod(highlightAttribute.validateField, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var fieldInfo = t.GetField(highlightAttribute.validateField, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                var propertyInfo = t.GetProperty(highlightAttribute.validateField, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                if (methodInfo != null)
                {
                    doHighlight = (bool)methodInfo.Invoke(property.serializedObject.targetObject, null) == (bool)highlightAttribute.comparationValue;
                    ;
                }

                // else
                // {
                //     Debug.LogError("Invalid Validate function: " + highlightAttribute.ValidateField, property.serializedObject.targetObject);
                // }
                if (fieldInfo != null)
                {
                    SerializedProperty conditionField = property.serializedObject.FindProperty(highlightAttribute.validateField);
                    // We check that exist a Field with the parameter name
                    if (conditionField == null)
                    {
                        //ShowError(position, label, "Error getting the condition Field. Check the name.");
                        return;
                    }

                    switch (conditionField.propertyType)
                    {
                        case SerializedPropertyType.Boolean:
                            try
                            {
                                bool comparationValue = highlightAttribute.comparationValue == null || (bool)highlightAttribute.comparationValue;
                                doHighlight = conditionField.boolValue == comparationValue;
                            }
                            catch
                            {
                                // ShowError(position, label, "Invalid comparation Value Type");
                                return;
                            }

                            break;
                        case SerializedPropertyType.Enum:
                            object paramEnum = highlightAttribute.comparationValue;
                            object[] paramEnumArray = highlightAttribute.comparationValueArray;

                            if (paramEnum == null && paramEnumArray == null)
                            {
                                doHighlight = true;
                                // ShowError(position, label, "The comparation enum value is null");
                                return;
                            }
                            else if (UtilityDraw.IsEnum(paramEnum))
                            {
                                if (!UtilityDraw.CheckSameEnumType(new[] { paramEnum.GetType() }, property.serializedObject.targetObject.GetType(), conditionField.propertyPath))
                                {
                                    doHighlight = true;
                                    //ShowError(position, label, "Enum Types doesn't match");
                                    return;
                                }
                                else
                                {
                                    string enumValue = Enum.GetValues(paramEnum.GetType()).GetValue(conditionField.enumValueIndex).ToString();
                                    if (paramEnum.ToString() != enumValue)
                                        doHighlight = false;
                                    else
                                        doHighlight = true;
                                }
                            }
                            else if (UtilityDraw.IsEnum(paramEnumArray))
                            {
                                if (!UtilityDraw.CheckSameEnumType(paramEnumArray.Select(x => x.GetType()), property.serializedObject.targetObject.GetType(),
                                        conditionField.propertyPath))
                                {
                                    doHighlight = true;
                                    //ShowError(position, label, "Enum Types doesn't match");
                                    return;
                                }
                                else
                                {
                                    string enumValue = Enum.GetValues(paramEnumArray[0].GetType()).GetValue(conditionField.enumValueIndex).ToString();
                                    if (paramEnumArray.All(x => x.ToString() != enumValue))
                                        doHighlight = false;
                                    else
                                        doHighlight = true;
                                }
                            }
                            else
                            {
                                doHighlight = true;
                                //  ShowError(position, label, "The comparation enum value is not an enum");
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
                                stringValue = (string)highlightAttribute.comparationValue;
                            }
                            catch
                            {
                                doHighlight = true;
                                //ShowError(position, label, "Invalid comparation Value Type");
                                return;
                            }

                            if (stringValue.StartsWith("=="))
                            {
                                float? value = UtilityDraw.GetValue(stringValue, "==");
                                if (value == null)
                                    error = true;
                                else
                                    doHighlight = conditionValue == value;
                            }
                            else if (stringValue.StartsWith("!="))
                            {
                                float? value = UtilityDraw.GetValue(stringValue, "!=");
                                if (value == null)
                                    error = true;
                                else
                                    doHighlight = conditionValue != value;
                            }
                            else if (stringValue.StartsWith("<="))
                            {
                                float? value = UtilityDraw.GetValue(stringValue, "<=");
                                if (value == null)
                                    error = true;
                                else
                                    doHighlight = conditionValue <= value;
                            }
                            else if (stringValue.StartsWith(">="))
                            {
                                float? value = UtilityDraw.GetValue(stringValue, ">=");
                                if (value == null)
                                    error = true;
                                else
                                    doHighlight = conditionValue >= value;
                            }
                            else if (stringValue.StartsWith("<"))
                            {
                                float? value = UtilityDraw.GetValue(stringValue, "<");
                                if (value == null)
                                    error = true;
                                else
                                    doHighlight = conditionValue < value;
                            }
                            else if (stringValue.StartsWith(">"))
                            {
                                float? value = UtilityDraw.GetValue(stringValue, ">");
                                if (value == null)
                                    error = true;
                                else
                                    doHighlight = conditionValue > value;
                            }

                            if (error)
                            {
                                doHighlight = true;
                                // ShowError(position, label, "Invalid comparation instruction for Int or float value");
                                return;
                            }

                            break;
                        default:
                            doHighlight = true;
                            //  ShowError(position, label, "This type has not supported.");
                            return;
                    }
                }
                else if (propertyInfo != null)
                {
                    doHighlight = (bool)propertyInfo.GetValue(property.serializedObject.targetObject) == (bool)highlightAttribute.comparationValue;
                }
            }

            if (doHighlight)
            {
                // get the highlight color
                var color = ColorExtensions.ToColor(highlightAttribute.highColor);

                // create a ractangle to draw the highlight to, slightly larger than our property
                var padding = EditorGUIUtility.standardVerticalSpacing;
                var highlightRect = new Rect(position.x - padding, position.y - padding,
                    position.width + (padding * 2), position.height + (padding * 2));

                // draw the highlight first
                EditorGUI.DrawRect(highlightRect, color);

                // make sure the propertys text is dark and easy to read over the bright highlight
                var cc = GUI.contentColor;
                GUI.contentColor = Color.black;

                // draw the property ontop of the highlight
                EditorGUI.PropertyField(position, property, label);

                GUI.contentColor = cc;
            }
            else
            {
                EditorGUI.PropertyField(position, property, label);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}