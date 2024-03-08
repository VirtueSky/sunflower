using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;

namespace VirtueSky.Hierarchy.HComponent
{
    public class SeparatorComponent: BaseComponent
    {
        // PRIVATE
        private Color separatorColor;
        private Color evenShadingColor;
        private Color oddShadingColor;
        private bool showRowShading;

        // CONSTRUCTOR
        public SeparatorComponent ()
        {
            showComponentDuringPlayMode = true;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.SeparatorShowRowShading   , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.SeparatorShow             , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.SeparatorColor                , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.SeparatorEvenRowShadingColor  , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.SeparatorOddRowShadingColor , settingsChanged);

            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            showRowShading   = HierarchySettings.getInstance().get<bool>(HierarchySetting.SeparatorShowRowShading);
            enabled          = HierarchySettings.getInstance().get<bool>(HierarchySetting.SeparatorShow);
            evenShadingColor = HierarchySettings.getInstance().getColor(HierarchySetting.SeparatorEvenRowShadingColor);
            oddShadingColor  = HierarchySettings.getInstance().getColor(HierarchySetting.SeparatorOddRowShadingColor);
            separatorColor   = HierarchySettings.getInstance().getColor(HierarchySetting.SeparatorColor);
        }

        // DRAW
        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            rect.y = selectionRect.y;
            rect.width = selectionRect.width + selectionRect.x;
            rect.height = 1;
            rect.x = 0;

            EditorGUI.DrawRect(rect, separatorColor);

            if (showRowShading)
            {
                selectionRect.width += selectionRect.x;
                selectionRect.x = 0;
                selectionRect.height -=1;
                selectionRect.y += 1;
                EditorGUI.DrawRect(selectionRect, ((Mathf.FloorToInt(((selectionRect.y - 4) / 16) % 2) == 0)) ? evenShadingColor : oddShadingColor);
            }
        }
    }
}

