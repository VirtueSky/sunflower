using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    [Conditional("UNITY_EDITOR")]
    public class ShowInEditModeAttribute : HideInEditModeAttribute
    {
        public ShowInEditModeAttribute()
        {
            Inverse = true;
        }
    }
}