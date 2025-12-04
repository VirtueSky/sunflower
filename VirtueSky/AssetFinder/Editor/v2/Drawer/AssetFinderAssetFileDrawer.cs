using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    [CustomPropertyDrawer(typeof(AssetFinderAssetFile))]
    public class AssetFinderAssetFileDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            
            // Get serialized properties
            var fr2IdProp = property.FindPropertyRelative("fr2Id");
            var guidProp = property.FindPropertyRelative("guid");
            var fileIdsProp = property.FindPropertyRelative("fileIds");
            
            // Load the asset using the GUID
            string assetPath = AssetDatabase.GUIDToAssetPath(guidProp.stringValue);
            // Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

            // Set fixed widths for AssetFinderID and GUID fields
            float fr2IdWidth = 70f; 
            float guidWidth = 280f;
            float spacing = 5f;

            // Draw Asset Path as clickable label at the top
            Rect assetPathRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            AssetFinderAssetGUI.DrawAsset(assetPathRect, guidProp.stringValue);
            
            // Calculate Rects for AssetFinderID and GUID on the same line below the asset path
            Rect fr2IdRect = new Rect(position.x, assetPathRect.yMax + EditorGUIUtility.standardVerticalSpacing, fr2IdWidth, EditorGUIUtility.singleLineHeight);
            Rect guidRect = new Rect(fr2IdRect.xMax + spacing, fr2IdRect.y, guidWidth, EditorGUIUtility.singleLineHeight);
            
            // Draw GUID button
            var fr2Id = (AssetFinderID)fr2IdProp.FindPropertyRelative("value").intValue;
            AssetFinderAssetGUI.DrawId(fr2IdRect, fr2Id);
            
            // Draw GUID button
            AssetFinderAssetGUI.DrawGuid(guidRect, guidProp.stringValue);
            
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Calculate height based on the contents
            var fileIdsProp = property.FindPropertyRelative("fileIds");
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = EditorGUIUtility.standardVerticalSpacing;

            // Estimate the number of lines required for the fileId buttons
            float totalWidth = EditorGUIUtility.currentViewWidth;
            float currentLineWidth = 0;
            int lineCount = 1; // Start with one line

            for (int i = 0; i < fileIdsProp.arraySize; i++)
            {
                string fileIdText = fileIdsProp.GetArrayElementAtIndex(i).longValue.ToString();
                float buttonWidth = EditorStyles.label.CalcSize(new GUIContent(fileIdText)).x + 8f;

                if (currentLineWidth + buttonWidth > totalWidth)
                {
                    lineCount++;
                    currentLineWidth = buttonWidth;
                }
                else
                {
                    currentLineWidth += buttonWidth + spacing;
                }
            }

            return lineHeight * (4 + lineCount) + spacing * 3 + 8f;
        }
    }
}