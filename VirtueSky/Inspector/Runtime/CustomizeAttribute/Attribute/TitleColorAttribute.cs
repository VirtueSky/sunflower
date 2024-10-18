using UnityEngine;
using System;
using VirtueSky.Utils;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class TitleColorAttribute : PropertyAttribute
    {
        #region Constants

        public const float DefaultLineHeight = 1f;
        public const CustomColor DefaultLineColor = CustomColor.LightGray;
        public const CustomColor DefaultTitleColor = CustomColor.Bright;

        #endregion

        #region Properties

        public string Title { get; private set; }
        public float LineHeight { get; private set; }
        public CustomColor LineColor { get; private set; }
        public CustomColor TitleColor { get; private set; }
        public string LineColorString { get; private set; }
        public string TitleColorString { get; private set; }
        public float Spacing { get; private set; }
        public bool AlignTitleLeft { get; private set; }

        #endregion

        public TitleColorAttribute(string title = "", CustomColor titleColor = DefaultTitleColor,
            CustomColor lineColor = DefaultLineColor, float lineHeight = DefaultLineHeight, float spacing = 14f,
            bool alignTitleLeft = false)
        {
            Title = title;
            TitleColor = titleColor;
            LineColor = lineColor;
            TitleColorString = ColorUtility.ToHtmlStringRGB(TitleColor.ToColor());
            LineColorString = ColorUtility.ToHtmlStringRGB(LineColor.ToColor());
            LineHeight = Mathf.Max(1f, lineHeight);
            Spacing = spacing;
            AlignTitleLeft = alignTitleLeft;
        }
    }
}