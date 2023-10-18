using System;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace VirtueSky.Variables
{
    [Serializable]
    public class BaseReference : IReference
    {
    }

    [Serializable]
    public class BaseReference<TType, TVariable> : BaseReference, IReference<TType, TVariable>
        where TVariable : BaseVariable<TType>
    {
        [SerializeField] bool useVariable;
        [SerializeField] TType constantValue;
        [SerializeField] TVariable variable;

        public TType Value
        {
            get => useVariable ? variable.Value : constantValue;
            set
            {
                if (useVariable)
                {
                    variable.Value = value;
                }
                else
                {
                    constantValue = value;
                }
            }
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(BaseReference), true)]
    public sealed class BaseReferenceDrawer : PropertyDrawer
    {
        static readonly string[] popupOptions =
        {
            "Use Constant",
            "Use Variable"
        };

        SerializedProperty property;
        SerializedProperty useVariable;
        SerializedProperty constantValue;
        SerializedProperty variable;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            this.property = property;
            useVariable = property.FindPropertyRelative("useVariable");
            constantValue = property.FindPropertyRelative("constantValue");
            variable = property.FindPropertyRelative("variable");

            var oldIndent = ResetIndent();

            var fieldRect = DrawLabel(position, property, label);
            var valueRect = DrawField(position, fieldRect);
            DrawValue(position, valueRect);

            EndIndent(oldIndent);

            property.serializedObject.ApplyModifiedProperties();
        }

        Rect DrawLabel(Rect position, SerializedProperty property, GUIContent label)
        {
            return EditorGUI.PrefixLabel(position, label);
        }

        Rect DrawField(Rect position, Rect fieldRect)
        {
            var buttonRect = GetPopupButtonRect(fieldRect);
            var valueRect = GetValueRect(fieldRect, buttonRect);

            var result = DrawPopupButton(buttonRect, useVariable.boolValue ? 1 : 0);
            useVariable.boolValue = result == 1;

            return valueRect;
        }

        void DrawValue(Rect position, Rect valueRect)
        {
            if (useVariable.boolValue)
            {
                EditorGUI.PropertyField(valueRect, variable, GUIContent.none);
            }
            else
            {
                DrawGenericPropertyField(position, valueRect);
            }
        }

        void DrawGenericPropertyField(Rect position, Rect valueRect)
        {
            EditorGUI.PropertyField(valueRect, constantValue, GUIContent.none);
        }

        int ResetIndent()
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            return indent;
        }

        void EndIndent(int indent)
        {
            EditorGUI.indentLevel = indent;
        }

        int DrawPopupButton(Rect rect, int value)
        {
            return EditorGUI.Popup(rect, value, popupOptions, Styles.PopupStyle);
        }

        Rect GetValueRect(Rect fieldRect, Rect buttonRect)
        {
            var valueRect = new Rect(fieldRect);
            valueRect.x += buttonRect.width;
            valueRect.width -= buttonRect.width;

            return valueRect;
        }

        Rect GetPopupButtonRect(Rect fieldrect)
        {
            var buttonRect = new Rect(fieldrect);
            buttonRect.yMin += Styles.PopupStyle.margin.top;
            buttonRect.width = Styles.PopupStyle.fixedWidth + Styles.PopupStyle.margin.right;
            buttonRect.height = Styles.PopupStyle.fixedHeight + Styles.PopupStyle.margin.top;

            return buttonRect;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        static class Styles
        {
            static Styles()
            {
                PopupStyle = new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
                {
                    imagePosition = ImagePosition.ImageOnly,
                };
            }

            public static GUIStyle PopupStyle { get; set; }
        }
    }
#endif
}