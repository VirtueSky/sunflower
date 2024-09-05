#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Hierarchy
{
    [InitializeOnLoad]
    public class HeaderHierarchyIcon
    {
        #region Static Variables

        // icons
        private static string assetPath = "/VirtueSky/Hierarchy/Icons";
        private static Texture2D icon_HierarchyHighlight;

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Contructors

        static HeaderHierarchyIcon()
        {
            // subscribe to inspector updates
            EditorApplication.hierarchyWindowItemOnGUI += EditorApplication_hierarchyWindowItemOnGUI;
        }

        #endregion

        //----------------------------------------------------------------------------------------------------

        #region Private Functions

        private static void CreateHierarchyIcon_Highlight()
        {
            icon_HierarchyHighlight = FileExtension.FindAssetWithPath<Texture2D>("HierarchyHighlight.png", assetPath);
        }

        private static void EditorApplication_hierarchyWindowItemOnGUI(int instanceID, Rect position)
        {
            // check for valid draw
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject != null)
            {
                HeaderHierarchy component = gameObject.GetComponent<HeaderHierarchy>();
                if (component != null)
                {
                    // cache values
                    int hierarchyPixelHeight = 16;
                    bool isSelected = Selection.instanceIDs.Contains(instanceID);
                    bool isActive = component.isActiveAndEnabled;
                    Color32 defaultContentColor = GUI.contentColor;
                    Color32 textColor;
                    Color32 backgroundColor;

                    if (isActive || isSelected)
                    {
                        // text
                        if (component.customText)
                        {
                            textColor = component.textColor;
                        }
                        else
                        {
                            textColor = InspectorUtility.textNormalColor;
                        }
                    }
                    else
                    {
                        // text
                        if (component.customText)
                        {
                            textColor = (Color)component.textColor * 0.6f;
                        }
                        else
                        {
                            textColor = InspectorUtility.textDisabledColor;
                        }
                    }

                    // draw background
                    if (isSelected)
                    {
                        backgroundColor = InspectorUtility.backgroundActiveColor;
                    }
                    else
                    {
                        backgroundColor = InspectorUtility.backgroundNormalColorLight;
                    }

                    Rect backgroundPosition = new Rect(position.xMin, position.yMin, position.width + hierarchyPixelHeight, position.height);
                    EditorGUI.DrawRect(backgroundPosition, backgroundColor);

                    // check icon exists
                    if (!icon_HierarchyHighlight)
                    {
                        CreateHierarchyIcon_Highlight();
                    }

                    // draw highlight
                    if (component.customHighlight)
                    {
                        GUI.contentColor = component.highlightColor;
                        Rect iconPosition = new Rect(position.xMin, position.yMin, icon_HierarchyHighlight.width, icon_HierarchyHighlight.height);
                        GUIContent iconGUIContent = new GUIContent(icon_HierarchyHighlight);
                        EditorGUI.LabelField(iconPosition, iconGUIContent);
                        GUI.contentColor = defaultContentColor;
                    }

                    // draw text
                    GUIStyle hierarchyText = new GUIStyle() { };
                    hierarchyText.normal = new GUIStyleState() { textColor = textColor };
                    hierarchyText.fontStyle = component.textStyle;
                    int offsetX;
                    if (component.textAlignment == HeaderHierarchy.TextAlignment.Center)
                    {
                        hierarchyText.alignment = TextAnchor.MiddleCenter;
                        offsetX = 0;
                    }
                    else
                    {
                        hierarchyText.alignment = TextAnchor.MiddleLeft;
                        offsetX = hierarchyPixelHeight + 2;
                    }

                    Rect textOffset = new Rect(position.xMin + offsetX, position.yMin, position.width, position.height);
                    EditorGUI.LabelField(textOffset, component.name, hierarchyText);
                }
            }
        }

        #endregion
    } // class end
}
#endif