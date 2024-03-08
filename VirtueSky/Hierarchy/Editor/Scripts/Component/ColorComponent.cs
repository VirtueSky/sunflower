using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;

namespace VirtueSky.Hierarchy.HComponent
{
    public class ColorComponent: BaseComponent
    {
        // PRIVATE
        private Color inactiveColor;
        private Texture2D colorTexture;
        private Rect colorRect = new Rect();

        // CONSTRUCTOR
        public ColorComponent()
        {
            colorTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyColorButton);

            rect.width = 8;
            rect.height = 16;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.ColorShow              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ColorShowDuringPlayMode, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalInactiveColor, settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = HierarchySettings.getInstance().get<bool>(HierarchySetting.ColorShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.ColorShowDuringPlayMode);
            inactiveColor               = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalInactiveColor);
        }

        // LAYOUT
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 8)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 8;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return LayoutStatus.Success;
            }
        }

        // DRAW
        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            if (objectList != null)
            {
                Color newColor;
                if (objectList.gameObjectColor.TryGetValue(gameObject, out newColor))
                {
                    colorRect.Set(rect.x + 1, rect.y + 1, 5, rect.height - 1);
                    EditorGUI.DrawRect(colorRect, newColor);
                    return;
                }
            }

            HierarchyColorUtils.setColor(inactiveColor);
            GUI.DrawTexture(rect, colorTexture, ScaleMode.StretchToFill, true, 1);
            HierarchyColorUtils.clearColor();
        }

        // EVENTS
        public override void eventHandler(GameObject gameObject, ObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.type == EventType.MouseDown && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                if (currentEvent.type == EventType.MouseDown)
                {
                    try {
                        PopupWindow.Show(rect, new HierarchyColorPickerWindow(Selection.Contains(gameObject) ? Selection.gameObjects : new GameObject[] { gameObject }, colorSelectedHandler, colorRemovedHandler));
                    } 
                    catch {}
                }
                currentEvent.Use();
            }
        }

        // PRIVATE
        private void colorSelectedHandler(GameObject[] gameObjects, Color color)
        {
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                ObjectList objectList = HierarchyObjectListManager.getInstance().getObjectList(gameObjects[i], true);
                Undo.RecordObject(objectList, "Color Changed");
                if (objectList.gameObjectColor.ContainsKey(gameObject))
                {
                    objectList.gameObjectColor[gameObject] = color;
                }
                else
                {
                    objectList.gameObjectColor.Add(gameObject, color);
                }                
            }
            EditorApplication.RepaintHierarchyWindow();
        }

        private void colorRemovedHandler(GameObject[] gameObjects)
        {
            for (int i = gameObjects.Length - 1; i >= 0; i--)
            {
                GameObject gameObject = gameObjects[i];
                ObjectList objectList = HierarchyObjectListManager.getInstance().getObjectList(gameObjects[i], true);
                if (objectList.gameObjectColor.ContainsKey(gameObject))                
                {
                    Undo.RecordObject(objectList, "Color Changed");
                    objectList.gameObjectColor.Remove(gameObject);                          
                }
            }
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}

