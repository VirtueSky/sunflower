using System;
using JetBrains.Annotations;
using PrimeTween;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUIUtility;

/// todo clear the custom ease curve when ease != Ease.Custom
[CustomPropertyDrawer(typeof(TweenSettings))]
internal class TweenSettingsPropDrawer : PropertyDrawer {
    public override float GetPropertyHeight([NotNull] SerializedProperty property, GUIContent label) {
        if (!property.isExpanded) {
            return singleLineHeight;
        }
        return getPropHeight(property);
    }

    internal static float getPropHeight([NotNull] SerializedProperty property) {
        var count = 1;
        count++; // duration
        count++; // ease
        var easeIndex = property.FindPropertyRelative(nameof(TweenSettings.ease)).intValue;
        if (easeIndex == (int)Ease.Custom) {
            count++; // customEase
        }
        count++; // cycles
        var cycles = property.FindPropertyRelative(nameof(TweenSettings.cycles)).intValue;
        if (cycles != 0 && cycles != 1) {
            count++; // cycleMode
        }
        count++; // startDelay
        count++; // endDelay
        count++; // useUnscaledTime
        count++; // useFixedUpdate
        var result = singleLineHeight * count + standardVerticalSpacing * (count - 1);
        result += standardVerticalSpacing * 2; // extra spacing
        return result;
    }

    public override void OnGUI(Rect position, [NotNull] SerializedProperty property, GUIContent label) {
        var rect = new Rect(position) { height = singleLineHeight };
        PropertyField(rect, property, label);
        if (!property.isExpanded) {
            return;
        }
        moveToNextLine(ref rect);
        indentLevel++;
        { // duration
            property.NextVisible(true);
            DrawDuration(rect, property);
            moveToNextLine(ref rect);
        }
        drawEaseTillEnd(property, ref rect);
        indentLevel--;
    }

    internal static void DrawDuration(Rect rect, [NotNull] SerializedProperty property) {
        if (GUI.enabled) {
            ClampProperty(property, 1f);
        }
        PropertyField(rect, property);
    }

    internal static void ClampProperty(SerializedProperty prop, float defaultValue, float min = 0.01f, float max = float.MaxValue) {
        prop.floatValue = prop.floatValue == 0f ? defaultValue : Mathf.Clamp(prop.floatValue, min, max);
    }

    internal static void drawEaseTillEnd([NotNull] SerializedProperty property, ref Rect rect) {
        DrawEaseAndCycles(property, ref rect);
        drawStartDelayTillEnd(ref rect, property);
    }

    internal static void DrawEaseAndCycles(SerializedProperty property, ref Rect rect, bool addSpace = true, bool draw = true) {
        { // ease
            property.NextVisible(true);
            if (draw) PropertyField(rect, property);
            moveToNextLine(ref rect);
            // customEase
            bool isCustom = property.intValue == (int) Ease.Custom;
            property.NextVisible(true);
            if (isCustom) {
                if (draw) PropertyField(rect, property);
                moveToNextLine(ref rect);
            }
        }
        if (addSpace) {
            rect.y += standardVerticalSpacing * 2;
        }
        { // cycles
            var cycles = drawCycles(rect, property, draw);
            moveToNextLine(ref rect);
            {
                // cycleMode
                property.NextVisible(true);
                if (cycles != 0 && cycles != 1) {
                    if (draw) PropertyField(rect, property);
                    moveToNextLine(ref rect);
                }
            }
        }
    }

    internal static void drawStartDelayTillEnd(ref Rect rect, [NotNull] SerializedProperty property) {
        { // startDelay, endDelay
            for (int _ = 0; _ < 2; _++) {
                property.NextVisible(true);
                if (property.floatValue < 0f) {
                    property.floatValue = 0f;
                }
                PropertyField(rect, property);
                moveToNextLine(ref rect);
            }
        }
        { // useUnscaledTime
            property.NextVisible(true);
            PropertyField(rect, property);
            moveToNextLine(ref rect);
        }
        { // useFixedUpdate
            property.NextVisible(true);
            PropertyField(rect, property);
            moveToNextLine(ref rect);
        }
    }

    internal static int drawCycles(Rect rect, [NotNull] SerializedProperty property, bool draw = true) {
        property.NextVisible(false);
        if (property.intValue == 0) {
            property.intValue = 1;
        } else if (property.intValue < -1) {
            property.intValue = -1;
        }
        if (draw) PropertyField(rect, property);
        return property.intValue;
    }

    static void moveToNextLine(ref Rect rect) {
        rect.y += singleLineHeight + standardVerticalSpacing;
    }
}
