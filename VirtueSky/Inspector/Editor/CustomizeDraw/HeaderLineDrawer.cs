using UnityEditor;
using UnityEngine;

namespace VirtueSky.Inspector
{
    [CustomPropertyDrawer(typeof(HeaderLineAttribute))]
    public class HeaderLineDrawer : DecoratorDrawer
    {
        private HeaderLineAttribute Target => attribute as HeaderLineAttribute;
        private GUIStyle m_style = new GUIStyle(EditorStyles.boldLabel);
        protected float singleLine = EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect _rect)
        {
            //Draw label
            if (!string.IsNullOrWhiteSpace(Target.text))
            {
                EditorGUI.LabelField(_rect, Target.text.ToUpper(), m_style);

                //Move to new line and set following line height
                _rect.y += singleLine + 1;
                _rect.height = 1;
            }
            else
            {
                _rect.y += singleLine / 2f + 1;
                _rect.height = 1;
            }

            Color c = Color.gray;
            if (EditorGUIUtility.isProSkin)
            {
                c = m_style.normal.textColor;
            }

            //Draw spacer
            UtilityDraw.CreateLineSpacer(EditorGUI.IndentedRect(_rect), c, _rect.height);
        }

        //How tall the GUI is for this decorator
        public override float GetHeight()
        {
            return singleLine * 1.25f;
        }
    }
}