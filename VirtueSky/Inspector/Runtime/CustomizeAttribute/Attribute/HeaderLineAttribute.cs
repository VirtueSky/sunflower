using System;
using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.Inspector
{
    /// <summary>
    /// Display a header with an underline
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HeaderLineAttribute : PropertyAttribute
    {
        public readonly string text;
        public readonly bool isToUpper;
        public readonly CustomColor colorText;
        public readonly CustomColor colorLine;

        //Constructor
        public HeaderLineAttribute(string text, bool isToUpper = true, CustomColor colorText = CustomColor.LightGray, CustomColor colorLine = CustomColor.LightGray)
        {
            this.text = text;
            this.isToUpper = isToUpper;
            this.colorText = colorText;
            this.colorLine = colorLine;
        }
    }
}