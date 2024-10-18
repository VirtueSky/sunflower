using UnityEditor;
using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.Inspector
{
    [CustomPropertyDrawer(typeof(TitleColorAttribute))]
    public class TitleColorAttributeDrawer : DecoratorDrawer
    {
        private float _nestedMinimumXPosition = 18f;
        private float _paddingRightLine = 10f;

        public override float GetHeight()
        {
            TitleColorAttribute titleColorAttribute = (TitleColorAttribute)attribute;
            return titleColorAttribute.Spacing + titleColorAttribute.LineHeight + titleColorAttribute.Spacing;
        }

        public override void OnGUI(Rect position)
        {
            Rect rect = EditorGUI.IndentedRect(position);
            rect.y += EditorGUIUtility.singleLineHeight / 2f;
            TitleColorAttribute titleColorAttribute = (TitleColorAttribute)attribute;
            Color lineColor = titleColorAttribute.LineColor.ToColor();

            if (string.IsNullOrEmpty(titleColorAttribute.Title))
            {
                rect.height = titleColorAttribute.LineHeight;
                EditorGUI.DrawRect(rect, lineColor);
                return;
            }

            // Label style
            GUIStyle style = new GUIStyle(EditorStyles.label) { richText = true };
            style.stretchWidth = true;
            style.clipping = TextClipping.Overflow;
            GUIContent label = new GUIContent($"<color=#{titleColorAttribute.TitleColorString}><b>{titleColorAttribute.Title}</b></color>");
            Vector2 textSize = style.CalcSize(label);

            float linesRectWidth = (position.width - textSize.x) / 2f;
            float labelPaddingSize = 5f;

            if (titleColorAttribute.AlignTitleLeft)
            {
                var rigthLinePositionX = position.xMin + textSize.x + labelPaddingSize + _paddingRightLine;

                Rect labelLeftRect = new Rect(position.xMin, position.yMin - (titleColorAttribute.Spacing * 0.01f), textSize.x, position.height);

                if (rect.xMin > _nestedMinimumXPosition)
                {
                    rigthLinePositionX = labelLeftRect.xMax + labelPaddingSize * 2 + (rect.xMin / 2f);
                }

                Rect RightRect = new Rect(rigthLinePositionX, position.yMin + titleColorAttribute.Spacing, position.width - textSize.x - 10f, titleColorAttribute.LineHeight);

                EditorGUI.LabelField(labelLeftRect, label, style);
                EditorGUI.DrawRect(RightRect, lineColor);
                return;
            }

            Rect leftLineRect = new Rect(position.xMin, position.yMin + titleColorAttribute.Spacing, linesRectWidth, titleColorAttribute.LineHeight);

            var labelPositionX = leftLineRect.xMax + labelPaddingSize;
            var rightLineRectWidth = linesRectWidth - _paddingRightLine;

            // If the rect is nested inside a list or an object's property
            if (rect.xMin > _nestedMinimumXPosition)
            {
                labelPositionX = leftLineRect.xMax - (rect.xMin / 2f);
                rightLineRectWidth = linesRectWidth;
            }

            Rect labelRect = new Rect(labelPositionX, position.yMin - (titleColorAttribute.Spacing * 0.01f), textSize.x, position.height);

            var rightLinePositionX = rect.xMin > _nestedMinimumXPosition ? labelRect.xMax + labelPaddingSize * 2 + (rect.xMin / 2f) : labelRect.xMax + labelPaddingSize;

            Rect rightLineRect = new Rect(rightLinePositionX, position.yMin + titleColorAttribute.Spacing, rightLineRectWidth, titleColorAttribute.LineHeight);

            EditorGUI.DrawRect(leftLineRect, lineColor);
            EditorGUI.LabelField(labelRect, label, style);
            EditorGUI.DrawRect(rightLineRect, lineColor);
        }
    }
}