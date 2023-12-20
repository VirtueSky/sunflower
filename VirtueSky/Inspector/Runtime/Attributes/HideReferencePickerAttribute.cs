using System;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class HideReferencePickerAttribute : Attribute
    {
    }
}