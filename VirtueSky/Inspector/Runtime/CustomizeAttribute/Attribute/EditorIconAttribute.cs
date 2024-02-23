using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    /// <summary>
    /// Specify a texture name from your assets which you want to be assigned as an icon to the MonoScript.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    [Conditional("UNITY_EDITOR")]
    public class EditorIconAttribute : System.Attribute
    {
        public string Name { get; set; }

        public EditorIconAttribute(string name)
        {
            Name = name;
        }
    }
}