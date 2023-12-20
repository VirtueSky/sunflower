using UnityEditor;
using UnityEngine;
using VirtueSky.Utils;

namespace VirtueSky.Inspector
{
    public class GUIDAttribute : PropertyAttribute
    {
        public string prefix;

        public GUIDAttribute()
        {
            this.prefix = string.Empty;
        }

        public GUIDAttribute(string prefix)
        {
            this.prefix = prefix;
        }
    }

#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(GUIDAttribute))]
    public class GuidAttributeDrawer : PropertyDrawer
    {
        string Prefix => (attribute as GUIDAttribute).prefix;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            if (string.IsNullOrEmpty(property.stringValue))
            {
                property.stringValue = Prefix + SimpleMath.NewGuid();
            }

            var w = position.width * 0.3f;

            position.width = position.width * 0.7f;
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, GUIContent.none);
            GUI.enabled = true;

            position.position += new Vector2(position.width, 0);
            position.width = w;
            if (GUI.Button(position, new GUIContent("Change")))
            {
                if (!property.serializedObject.isEditingMultipleObjects)
                    property.stringValue = Prefix + SimpleMath.NewGuid();
            }

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }
    }
#endif
}