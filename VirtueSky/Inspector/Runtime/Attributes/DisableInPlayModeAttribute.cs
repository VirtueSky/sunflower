﻿using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    [Conditional("UNITY_EDITOR")]
    public class DisableInPlayModeAttribute : Attribute
    {
        public bool Inverse { get; protected set; }
    }
}