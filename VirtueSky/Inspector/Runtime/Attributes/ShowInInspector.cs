using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    [Conditional("UNITY_EDITOR")]
    public class ShowInInspectorAttribute : Attribute
    {
    }
}