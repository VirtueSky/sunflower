using UnityEditor;
using UnityEngine;

namespace VirtueSky.Inspector
{
    [CustomPropertyDrawer(typeof(TagAttribute))]
    public class TagAttributeDraw : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                //Debug.LogWarning("Tag attribute must be used with 'string' property type");
                //base.OnGUI(position, property, label);
                EditorGUI.LabelField(position, "Tag attribute must be used with 'string' property type", new GUIStyle { normal = new GUIStyleState { textColor = Color.yellow } });
                return;
            }

            if (property.stringValue == "")
            {
                property.stringValue = UnityEditorInternal.InternalEditorUtility.tags[0];
            }

            property.stringValue = EditorGUI.TagField(position, label, property.stringValue);
        }
    }
}