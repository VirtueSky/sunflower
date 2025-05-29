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

    internal static void DrawEaseAndCycles(SerializedProperty property, ref Rect rect, bool addSpace = true, bool draw = true, bool allowInfiniteCycles = true) {
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
            var cycles = drawCycles(rect, property, draw, allowInfiniteCycles);
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
            property.Next(false);
            bool useFixedUpdateObsolete = property.boolValue;
            var useFixedUpdateProp = property.Copy();

            property.NextVisible(false);
            var current = (_UpdateType)property.enumValueIndex;
            if (useFixedUpdateObsolete && current != _UpdateType.FixedUpdate) {
                property.serializedObject.Update();
                property.enumValueIndex = (int)_UpdateType.FixedUpdate;
                property.serializedObject.ApplyModifiedProperties();
            } else {
                using (var propScope = new PropertyScope(rect, new GUIContent(property.displayName, property.tooltip), property)) {
                    using (var changeCheck = new ChangeCheckScope()) {
                        var newUpdateType = (_UpdateType)EnumPopup(rect, propScope.content, current);
                        if (changeCheck.changed) {
                            property.enumValueIndex = (int)newUpdateType;
                            useFixedUpdateProp.boolValue = newUpdateType == _UpdateType.FixedUpdate;
                        }
                        moveToNextLine(ref rect);
                    }
                }
            }
        }
    }

    internal static int drawCycles(Rect rect, [NotNull] SerializedProperty property, bool draw = true, bool allowInfiniteCycles = true) {
        property.NextVisible(false);
        int val = property.intValue;
        if (val == 0) {
            val = 1;
        } else if (val < 0) {
            val = allowInfiniteCycles ? -1 : 1;
        }
        property.intValue = val;
        if (draw) PropertyField(rect, property);
        return val;
    }

    static void moveToNextLine(ref Rect rect) {
        rect.y += singleLineHeight + standardVerticalSpacing;
    }
}

[CustomPropertyDrawer(typeof(UpdateType))]
class UpdateTypePropDrawer : PropertyDrawer {
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => singleLineHeight;
    public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label) {
        prop.Next(true);
        PropertyField(pos, prop, label);
    }
}
