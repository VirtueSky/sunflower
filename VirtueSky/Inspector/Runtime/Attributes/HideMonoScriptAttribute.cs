using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage((AttributeTargets.Class | AttributeTargets.Struct))]
    [Conditional("UNITY_EDITOR")]
    public class HideMonoScriptAttribute : Attribute
    {
    }
}