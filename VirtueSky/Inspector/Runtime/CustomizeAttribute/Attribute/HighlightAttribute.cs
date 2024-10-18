using System;
using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Field)]
    public class HighlightAttribute : PropertyAttribute
    {
        public CustomColor highColor;
        public readonly string validateField;
        public readonly object comparationValue;
        public readonly object[] comparationValueArray;

        public HighlightAttribute(CustomColor highColor = CustomColor.Yellow, string validateField = null, object comparationValue = null)
        {
            this.highColor = highColor;
            this.validateField = validateField;
            this.comparationValue = comparationValue;
        }
    }
}