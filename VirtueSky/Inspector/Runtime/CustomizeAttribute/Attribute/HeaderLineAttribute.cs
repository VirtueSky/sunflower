using System;
using UnityEngine;

namespace VirtueSky.Inspector
{
    /// <summary>
    /// Display a header with an underline
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HeaderLineAttribute : PropertyAttribute
    {
        public readonly string text;

        //Constructor
        public HeaderLineAttribute(string text)
        {
            this.text = text;
        }
    }
}