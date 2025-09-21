using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    [Conditional("UNITY_EDITOR")]
    public class PropertyOrderAttribute : Attribute
    {
        public int Order { get; }

        public PropertyOrderAttribute(int order)
        {
            Order = order;
        }
    }
}