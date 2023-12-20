using UnityEditor;
using UnityEngine;

namespace VirtueSky.Inspector.Editors
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), editorForChildClasses: true, isFallback = true)]
    internal sealed class TriMonoBehaviourEditor : TriEditor
    {
    }
}