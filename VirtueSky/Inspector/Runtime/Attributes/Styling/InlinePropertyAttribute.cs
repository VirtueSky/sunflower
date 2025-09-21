using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
                    AttributeTargets.Class | AttributeTargets.Struct)]
    [Conditional("UNITY_EDITOR")]
    public class InlinePropertyAttribute : Attribute
    {
        public float LabelWidth { get; set; }
    }
}