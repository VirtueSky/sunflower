using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VirtueSky.Attributes
{
    public class ButtonShowIfDrawer
    {
        private static bool s_ReverseAttributesOrder;
        List<DrawButtonShowIf> listDrawButtons = new List<DrawButtonShowIf>();

        [InitializeOnLoadMethod]
        public static void CheckAttributesOrder()
        {
            var method = typeof(ButtonDrawer).GetMethod("CheckAttributesOrder");
            var attributes = method.GetCustomAttributes(false);
            s_ReverseAttributesOrder = attributes[0].GetType() != typeof(ButtonShowIfAttribute);
        }

        public ButtonShowIfDrawer(object target)
        {
            BindingFlags flags =
                BindingFlags.InvokeMethod |
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Static |
                BindingFlags.Instance;
            var type = target.GetType();
            var methods = type.GetMethods(flags);

            for (int i = 0; i < methods.Length; i++)
            {
                MethodInfo methodInfo = methods[i];
                var attributes = methodInfo.GetCustomAttributes(false);
                int attributeCount = attributes.Length;
                for (int j = 0; j < attributeCount; j++)
                {
                    var attribute = attributes[s_ReverseAttributesOrder ? attributeCount - j - 1 : j];
                    var attributeType = attribute.GetType();
                    if (attributeType == typeof(ButtonShowIfAttribute))
                    {
                        var eButtonAttribute = (ButtonShowIfAttribute)attribute;
                        string text = (eButtonAttribute.text == null) ? methodInfo.Name : eButtonAttribute.text;

                        listDrawButtons.Add(new DrawButtonShowIf(text, methodInfo, target));
                    }
                }
            }
        }

        public void Draw()
        {
            foreach (var drawButton in listDrawButtons)
            {
                drawButton.Execute();
            }
        }
    }

    public class DrawButtonShowIf
    {
        private GUIContent m_GUIContent;
        private MethodInfo m_MethodInfo;
        private object m_Target;
        private bool showField;

        public DrawButtonShowIf(string text, MethodInfo methodInfo, object target)
        {
            m_GUIContent = new GUIContent(text);
            m_MethodInfo = methodInfo;
            m_Target = target;
        }

        public void Execute()
        {
            ConditionShowButton();
            if (showField && GUILayout.Button(m_GUIContent))
            {
                m_MethodInfo.Invoke(m_Target, null);
            }
        }

        void ConditionShowButton()
        {
            ButtonShowIfAttribute attribute = m_MethodInfo.GetCustomAttribute<ButtonShowIfAttribute>();
            SerializedObject serializedObject = new SerializedObject((Object)m_Target);

            FieldInfo fieldInfo = serializedObject.targetObject.GetType()
                .GetField(attribute.conditionFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            PropertyInfo propertyInfo = serializedObject.targetObject.GetType()
                .GetProperty(attribute.conditionFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            MethodInfo methodInfo = serializedObject.targetObject.GetType()
                .GetMethod(attribute.conditionFieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfo != null)
            {
                SerializedProperty conditionField = serializedObject.FindProperty(attribute.conditionFieldName);
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
                            bool comparationValue = attribute.comparationValue == null || (bool)attribute.comparationValue;
                            showField = conditionField.boolValue == comparationValue;
                        }
                        catch
                        {
                            // ShowError(position, label, "Invalid comparation Value Type");
                            return;
                        }

                        break;
                    case SerializedPropertyType.Enum:
                        object paramEnum = attribute.comparationValue;
                        object[] paramEnumArray = attribute.comparationValueArray;

                        if (paramEnum == null && paramEnumArray == null)
                        {
                            showField = true;
                            // ShowError(position, label, "The comparation enum value is null");
                            return;
                        }
                        else if (UtilityDraw.IsEnum(paramEnum))
                        {
                            if (!UtilityDraw.CheckSameEnumType(new[] { paramEnum.GetType() }, serializedObject.targetObject.GetType(), conditionField.propertyPath))
                            {
                                showField = true;
                                //ShowError(position, label, "Enum Types doesn't match");
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
                        else if (UtilityDraw.IsEnum(paramEnumArray))
                        {
                            if (!UtilityDraw.CheckSameEnumType(paramEnumArray.Select(x => x.GetType()), serializedObject.targetObject.GetType(), conditionField.propertyPath))
                            {
                                showField = true;
                                //ShowError(position, label, "Enum Types doesn't match");
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
                            showField = true;
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
                            stringValue = (string)attribute.comparationValue;
                        }
                        catch
                        {
                            showField = true;
                            //ShowError(position, label, "Invalid comparation Value Type");
                            return;
                        }

                        if (stringValue.StartsWith("=="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "==");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue == value;
                        }
                        else if (stringValue.StartsWith("!="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "!=");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue != value;
                        }
                        else if (stringValue.StartsWith("<="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "<=");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue <= value;
                        }
                        else if (stringValue.StartsWith(">="))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, ">=");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue >= value;
                        }
                        else if (stringValue.StartsWith("<"))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, "<");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue < value;
                        }
                        else if (stringValue.StartsWith(">"))
                        {
                            float? value = UtilityDraw.GetValue(stringValue, ">");
                            if (value == null)
                                error = true;
                            else
                                showField = conditionValue > value;
                        }

                        if (error)
                        {
                            showField = true;
                            // ShowError(position, label, "Invalid comparation instruction for Int or float value");
                            return;
                        }

                        break;
                    default:
                        showField = true;
                        //  ShowError(position, label, "This type has not supported.");
                        return;
                }
            }

            else if (methodInfo != null)
            {
                showField = (bool)methodInfo.Invoke(serializedObject.targetObject, null) == (bool)attribute.comparationValue;
            }
            else if (propertyInfo != null)
            {
                showField = (bool)propertyInfo.GetValue(serializedObject.targetObject) == (bool)attribute.comparationValue;
            }
        }
    }
}