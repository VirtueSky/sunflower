using System;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;
using VirtueSky.Hierarchy.Data;
using System.Reflection;

namespace VirtueSky.Hierarchy.HComponent
{
    public class GameObjectIconComponent: BaseComponent
    {
        // PRIVATE
        private MethodInfo getIconMethodInfo;
        private object[] getIconMethodParams;

        // CONSTRUCTOR
        public GameObjectIconComponent ()
        {
            rect.width = 14;
            rect.height = 14;

            getIconMethodInfo   = typeof(EditorGUIUtility).GetMethod("GetIconForObject", BindingFlags.NonPublic | BindingFlags.Static );
            getIconMethodParams = new object[1];

            HierarchySettings.getInstance().addEventListener(HierarchySetting.GameObjectIconShow                 , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.GameObjectIconShowDuringPlayMode   , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.GameObjectIconSize                          , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled = HierarchySettings.getInstance().get<bool>(HierarchySetting.GameObjectIconShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.GameObjectIconShowDuringPlayMode);
            HierarchySizeAll size = (HierarchySizeAll)HierarchySettings.getInstance().get<int>(HierarchySetting.GameObjectIconSize);
            rect.width = rect.height = (size == HierarchySizeAll.Normal ? 15 : (size == HierarchySizeAll.Big ? 16 : 13));     
        }

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < rect.width + 2)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= rect.width + 2;
                rect.x = curRect.x;
                rect.y = curRect.y - (rect.height - 16) / 2;
                return LayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {                      
            getIconMethodParams[0] = gameObject;
            Texture2D icon = (Texture2D)getIconMethodInfo.Invoke(null, getIconMethodParams );    
            if (icon != null) 
                GUI.DrawTexture(rect, icon, ScaleMode.ScaleToFit, true);
        }
                
        public override void eventHandler(GameObject gameObject, ObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                currentEvent.Use();

                Type iconSelectorType = Assembly.Load("UnityEditor").GetType("UnityEditor.IconSelector");
                MethodInfo showIconSelectorMethodInfo = iconSelectorType.GetMethod("ShowAtPosition", BindingFlags.Static | BindingFlags.NonPublic);
                showIconSelectorMethodInfo.Invoke(null, new object[] { gameObject, rect, true });
            }
        }
    }
}

