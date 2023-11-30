using UnityEngine;

namespace VirtueSky.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field)]
    public class ExtendEnumAttribute : PropertyAttribute
    {
        public readonly bool display = true;

        public ExtendEnumAttribute(bool displayValues = true)
        {
            display = displayValues;
        }
    }
}