using System;
using System.Diagnostics;

namespace VirtueSky.Inspector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
    [Conditional("UNITY_EDITOR")]
    public class DeclareHorizontalGroupAttribute : DeclareGroupBaseAttribute
    {
        public DeclareHorizontalGroupAttribute(string path) : base(path)
        {
        }

        public float[] Sizes { get; set; }
    }
}