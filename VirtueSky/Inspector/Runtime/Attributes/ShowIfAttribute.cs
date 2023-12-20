using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class ShowIfAttribute : HideIfAttribute
    {
        public ShowIfAttribute(string condition) : this(condition, true)
        {
        }

        public ShowIfAttribute(string condition, object value) : base(condition, value)
        {
            Inverse = true;
        }
    }
}