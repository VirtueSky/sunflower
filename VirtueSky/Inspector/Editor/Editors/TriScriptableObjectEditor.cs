using UnityEditor;
using UnityEngine;

namespace VirtueSky.Inspector.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(ScriptableObject), editorForChildClasses: true, isFallback = true)]
    internal sealed class TriScriptableObjectEditor : TriEditor
    {
    }
}