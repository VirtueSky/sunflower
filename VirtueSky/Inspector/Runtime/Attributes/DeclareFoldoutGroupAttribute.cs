using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class DeclareFoldoutGroupAttribute : DeclareGroupBaseAttribute
    {
        public DeclareFoldoutGroupAttribute(string path) : base(path)
        {
            Title = path;
        }

        public string Title { get; set; }
        public bool Expanded { get; set; }
    }
}