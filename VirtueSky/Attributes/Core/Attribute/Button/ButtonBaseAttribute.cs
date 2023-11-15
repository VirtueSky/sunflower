using System;

namespace VirtueSky.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class ButtonBaseAttribute : Attribute
    {
    }
}