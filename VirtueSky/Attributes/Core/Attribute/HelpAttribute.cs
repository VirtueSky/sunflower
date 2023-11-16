namespace VirtueSky.Attributes
{
    using System;
    using UnityEngine;
#if UNITY_EDITOR
    using UnityEditor;
#endif

    [AttributeUsage(AttributeTargets.Field, Inherited = true)]
    public class HelpAttribute : PropertyAttribute
    {
        public readonly string text;

        // MessageType exists in UnityEditor namespace and can throw an exception when used outside the editor.
        // We spoof MessageType at the bottom of this script to ensure that errors are not thrown when
        // MessageType is unavailable.
        public readonly MessageType type;


        /// <summary>
        /// Adds a HelpBox to the Unity property inspector above this field.
        /// </summary>
        /// <param name="text">The help text to be displayed in the HelpBox.</param>
        /// <param name="type">The icon to be displayed in the HelpBox.</param>
        public HelpAttribute(string text, MessageType type = MessageType.Info)
        {
            this.text = text;
            this.type = type;
        }
    }
}