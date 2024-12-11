using JetBrains.Annotations;
using PrimeTween;
using UnityEditor;
using UnityEngine;
using static UnityEditor.EditorGUI;
using static UnityEditor.EditorGUIUtility;

[CustomPropertyDrawer(typeof(ShakeSettings))]
internal class TweenShakeSettingsPropDrawer : PropertyDrawer { // todo rename to ShakeSettingsPropDrawer
    public override float GetPropertyHeight([NotNull] SerializedProperty property, GUIContent label) {
        if (!property.isExpanded) {
            return singleLineHeight;
        }
        property.NextVisible(true);
        float result = EditorGUI.GetPropertyHeight(property, true); // strength
        var count = 1;
        count++; // frequency
        count++; // duration
        count++; // enableFalloff
        property.NextVisible(false);
        property.NextVisible(false);
        property.NextVisible(false); // enableFalloff
        if (property.boolValue) {
            count++; // falloffEase
            property.NextVisible(false);
            if (property.intValue == -1) {
                count++; // strengthOverTime
            }
        }
        count++; // asymmetry
        count++; // easeBetweenShakes
        count++; // cycles
        count++; // startDelay
        count++; // endDelay
        count++; // useUnscaledTime
        count++; // useFixedUpdate
        result += singleLineHeight * count + standardVerticalSpacing * (count - 1);
        result += standardVerticalSpacing * 2; // extra space
        return result;
    }

    public override void OnGUI(Rect position, [NotNull] SerializedProperty property, GUIContent label) {
        var rect = new Rect(position) { height = singleLineHeight };
        PropertyField(rect, property, label);
        if (!property.isExpanded) {
            return;
        }
        moveToNextLine();
        indentLevel++;
        property.NextVisible(true);
        { // strength
            PropertyField(rect, property);
            rect.y += EditorGUI.GetPropertyHeight(property, true);
        }
        { // duration
            property.NextVisible(false);
            TweenSettingsPropDrawer.DrawDuration(rect, property);
            moveToNextLine();
        }
        { // frequency
            property.NextVisible(false);
            var floatValue = property.floatValue;
            if (floatValue == 0f) {
                property.floatValue = ShakeSettings.defaultFrequency;
            } else if (floatValue < 0.1f) {
                property.floatValue = 0.1f;
            }
            propertyField();
        }
        { // enableFalloff
            property.NextVisible(false);
            propertyField();
            var enableFalloff = property.boolValue;
            property.NextVisible(false);
            if (enableFalloff) {
                // falloffEase
                propertyField();
                // strengthOverTime
                var customFalloffEase = property.intValue == (int)Ease.Custom;
                property.NextVisible(false);
                if (customFalloffEase) {
                    propertyField();
                }
            } else {
                // skipped strengthOverTime
                property.NextVisible(false);
            }
        }
        // extra space
        rect.y += standardVerticalSpacing * 2;
        { // asymmetry
            property.NextVisible(false);
            propertyField();
        }
        { // easeBetweenShakes
            property.NextVisible(false);
            if (property.intValue == (int)Ease.Custom) {
                Debug.LogWarning($"Ease.Custom is not supported for {nameof(ShakeSettings.easeBetweenShakes)}.");
                property.intValue = (int)Ease.Default;
            }
            propertyField();
        }
        TweenSettingsPropDrawer.drawCycles(rect, property);
        moveToNextLine();
        TweenSettingsPropDrawer.drawStartDelayTillEnd(ref rect, property);
        indentLevel--;

        void propertyField() {
            PropertyField(rect, property);
            moveToNextLine();
        }

        void moveToNextLine() {
            rect.y += singleLineHeight + standardVerticalSpacing;
        }
    }
}
