using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    [Conditional("UNITY_EDITOR")]
    public class LabelWidthAttribute : Attribute
    {
        public float Width { get; }

        public LabelWidthAttribute(float width)
        {
            Width = width;
        }
    }
}