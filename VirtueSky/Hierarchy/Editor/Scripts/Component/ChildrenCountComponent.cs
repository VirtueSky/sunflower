using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;
using VirtueSky.Hierarchy.Data;

namespace VirtueSky.Hierarchy.HComponent
{
    public class ChildrenCountComponent: BaseComponent 
    {
        // PRIVATE
        private GUIStyle labelStyle;

        // CONSTRUCTOR
        public ChildrenCountComponent ()
        {
            labelStyle = new GUIStyle();
            labelStyle.fontSize = 9;
            labelStyle.clipping = TextClipping.Clip;  
            labelStyle.alignment = TextAnchor.MiddleRight;

            rect.width = 22;
            rect.height = 16;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.ChildrenCountShow              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ChildrenCountShowDuringPlayMode, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ChildrenCountLabelSize         , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ChildrenCountLabelColor        , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled = HierarchySettings.getInstance().get<bool>(HierarchySetting.ChildrenCountShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.ChildrenCountShowDuringPlayMode);
            HierarchySize labelSize = (HierarchySize)HierarchySettings.getInstance().get<int>(HierarchySetting.ChildrenCountLabelSize);
            labelStyle.normal.textColor = HierarchySettings.getInstance().getColor(HierarchySetting.ChildrenCountLabelColor);
            labelStyle.fontSize = labelSize == HierarchySize.Normal ? 8 : 9;
            rect.width = labelSize == HierarchySize.Normal ? 17 : 22;
        }

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y;
                rect.y += (EditorGUIUtility.singleLineHeight - rect.height) * 0.5f;
                rect.height = EditorGUIUtility.singleLineHeight;
                return LayoutStatus.Success;
            }
        }
        
        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {  
            int childrenCount = gameObject.transform.childCount;
            if (childrenCount > 0) GUI.Label(rect, childrenCount.ToString(), labelStyle);
        }
    }
}

