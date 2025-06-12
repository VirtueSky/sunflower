using System;
using PrimeTween;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ValueContainerStartEnd))]
public class ValueContainerStartEndPropDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty prop, GUIContent label) {
        prop.Next(true);
        var tweenType = (TweenType)prop.enumValueIndex;
        prop.Next(false);
        return GetHeight(prop, label, tweenType);
    }

    internal static float GetHeight(SerializedProperty prop, GUIContent label, TweenType tweenType) {
        var propType = Utils.TweenTypeToTweenData(tweenType).Item1;
        Assert.AreNotEqual(PropType.None, propType);
        bool startFromCurrent = prop.boolValue;
        bool hasStartValue = !startFromCurrent;
        if (hasStartValue) {
            return GetSingleItemHeight(propType, label) * 2f + EditorGUIUtility.standardVerticalSpacing;
        }
        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing + GetSingleItemHeight(propType, label);
    }

    static float GetSingleItemHeight(PropType propType, GUIContent label) {
        return EditorGUI.GetPropertyHeight(ToSerializedPropType(), label);
        SerializedPropertyType ToSerializedPropType() {
            switch (propType) {
                case PropType.Float:
                    return SerializedPropertyType.Float;
                case PropType.Color:
                    return SerializedPropertyType.Color;
                case PropType.Vector2:
                    return SerializedPropertyType.Vector2;
                case PropType.Vector3:
                    return SerializedPropertyType.Vector3;
                case PropType.Vector4:
                case PropType.Quaternion:
                    return SerializedPropertyType.Vector4;
                case PropType.Rect:
                    return SerializedPropertyType.Rect;
                case PropType.Int:
                    return SerializedPropertyType.Integer;
                case PropType.Double: // todo support double
                case PropType.None:
                default:
                    throw new Exception();
            }
        }
    }

    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label) {
        prop.Next(true);
        var tweenType = (TweenType)prop.enumValueIndex;
        prop.Next(false);
        Draw(ref pos, prop, tweenType);
    }

    internal static void Draw(ref Rect pos, SerializedProperty prop, TweenType tweenType) {
        var propType = Utils.TweenTypeToTweenData(tweenType).Item1;
        Assert.AreNotEqual(PropType.None, propType);
        const float toggleWidth = 18f;
        EditorGUIUtility.labelWidth -= toggleWidth;
        var togglePos = new Rect(pos.x + 2, pos.y, toggleWidth - 2, EditorGUIUtility.singleLineHeight);
        var guiContent = EditorGUI.BeginProperty(togglePos, new GUIContent(), prop); // todo is it possible to display tooltip? tooltip is only displayed over the label, but I need to display it over the ToggleLeft
        EditorGUI.BeginChangeCheck();
        bool newStartFromCurrent = !EditorGUI.ToggleLeft(togglePos, guiContent, !prop.boolValue);
        if (EditorGUI.EndChangeCheck()) {
            prop.boolValue = newStartFromCurrent;
        }
        EditorGUI.EndProperty();

        pos.x += toggleWidth;
        pos.width -= toggleWidth;

        prop.Next(false);
        if (newStartFromCurrent) {
            pos.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(pos, new GUIContent(prop.displayName, prop.tooltip));
            prop.Next(false);
        } else {
            DrawValueContainer(ref pos, prop, propType);
        }

        pos.y += pos.height + EditorGUIUtility.standardVerticalSpacing;
        DrawValueContainer(ref pos, prop, propType);
        pos.y += pos.height + EditorGUIUtility.standardVerticalSpacing;

        pos.x -= toggleWidth;
        pos.width += toggleWidth;
    }

    static void DrawValueContainer(ref Rect pos, SerializedProperty prop, PropType propType) {
        var root = prop.Copy();
        prop.Next(true);
        ValueContainer valueContainer = default;
        for (int i = 0; i < 4; i++) {
            valueContainer[i] = prop.floatValue;
            prop.Next(false);
        }
        var guiContent = new GUIContent(root.displayName, root.tooltip);
        pos.height = GetSingleItemHeight(propType, guiContent);
        guiContent = EditorGUI.BeginProperty(pos, guiContent, root);
        EditorGUI.BeginChangeCheck();
        ValueContainer newVal = DrawField(pos);
        ValueContainer DrawField(Rect position) {
            switch (propType) {
                case PropType.Float:
                    return EditorGUI.FloatField(position, guiContent, valueContainer.FloatVal).ToContainer();
                case PropType.Color:
                    return EditorGUI.ColorField(position, guiContent, valueContainer.ColorVal).ToContainer();
                case PropType.Vector2:
                    return EditorGUI.Vector2Field(position, guiContent, valueContainer.Vector2Val).ToContainer();
                case PropType.Vector3:
                    return EditorGUI.Vector3Field(position, guiContent, valueContainer.Vector3Val).ToContainer();
                case PropType.Vector4:
                case PropType.Quaternion: // todo don't draw quaternion
                    return EditorGUI.Vector4Field(position, guiContent, valueContainer.Vector4Val).ToContainer();
                case PropType.Rect:
                    return EditorGUI.RectField(position, guiContent, valueContainer.RectVal).ToContainer();
                case PropType.Int:
                    var newIntVal = EditorGUI.IntField(position, guiContent, Mathf.RoundToInt(valueContainer.FloatVal));
                    return ((float)newIntVal).ToContainer();
                case PropType.Double: // todo support double with EditorGUI.DoubleField()?
                case PropType.None:
                default:
                    throw new Exception();
            }
        }
        if (EditorGUI.EndChangeCheck()) {
            root.Next(true);
            for (int i = 0; i < 4; i++) {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (root.floatValue != newVal[i]) {
                    root.floatValue = newVal[i];
                }
                root.Next(false);
            }
        }
        EditorGUI.EndProperty();
    }
}
