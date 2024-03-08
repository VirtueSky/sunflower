using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;

namespace VirtueSky.Hierarchy.HComponent
{
    public class RendererComponent: BaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private Color specialColor;
        private Texture2D rendererButtonTexture;
        private int targetRendererMode = -1; 

        // CONSTRUCTOR
        public RendererComponent()
        {
            rect.width = 12;

            rendererButtonTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyRendererButton);

            HierarchySettings.getInstance().addEventListener(HierarchySetting.RendererShow              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.RendererShowDuringPlayMode, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalActiveColor     , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalInactiveColor   , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalSpecialColor    , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = HierarchySettings.getInstance().get<bool>(HierarchySetting.RendererShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.RendererShowDuringPlayMode);
            activeColor                 = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalActiveColor);
            inactiveColor               = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalInactiveColor);
            specialColor                = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalSpecialColor);
        }

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 12)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 12;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return LayoutStatus.Success;
            }
        }

        public override void disabledHandler(GameObject gameObject, ObjectList objectList)
        {
            if (objectList != null && objectList.wireframeHiddenObjects.Contains(gameObject))
            {      
                objectList.wireframeHiddenObjects.Remove(gameObject);
                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null) setSelectedRenderState(renderer, false);
            }
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                bool wireframeHiddenObjectsContains = isWireframeHidden(gameObject, objectList);
                if (wireframeHiddenObjectsContains)
                {
                    HierarchyColorUtils.setColor(specialColor);
                    GUI.DrawTexture(rect, rendererButtonTexture);
                    HierarchyColorUtils.clearColor();
                }
                else if (renderer.enabled)
                {
                    HierarchyColorUtils.setColor(activeColor);
                    GUI.DrawTexture(rect, rendererButtonTexture);
                    HierarchyColorUtils.clearColor();
                }
                else
                {
                    HierarchyColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, rendererButtonTexture);
                    HierarchyColorUtils.clearColor();
                }
            }
        }

        public override void eventHandler(GameObject gameObject, ObjectList objectList, Event currentEvent)
        {
            if (currentEvent.isMouse && currentEvent.button == 0 && rect.Contains(currentEvent.mousePosition))
            {
                Renderer renderer = gameObject.GetComponent<Renderer>();
                if (renderer != null)
                {
                    bool wireframeHiddenObjectsContains = isWireframeHidden(gameObject, objectList);
                    bool isEnabled = renderer.enabled;
                    
                    if (currentEvent.type == EventType.MouseDown)
                    {
                        targetRendererMode = ((!isEnabled) == true ? 1 : 0);
                    }
                    else if (currentEvent.type == EventType.MouseDrag && targetRendererMode != -1)
                    {
                        if (targetRendererMode == (isEnabled == true ? 1 : 0)) return;
                    } 
                    else
                    {
                        targetRendererMode = -1;
                        return;
                    }

                    Undo.RecordObject(renderer, "renderer visibility change");                    
                    
                    if (currentEvent.control || currentEvent.command)
                    {
                        if (!wireframeHiddenObjectsContains)
                        {
                            setSelectedRenderState(renderer, true);
                            SceneView.RepaintAll();
                            setWireframeMode(gameObject, objectList, true);
                        }
                    }
                    else
                    {
                        if (wireframeHiddenObjectsContains)
                        {
                            setSelectedRenderState(renderer, false);
                            SceneView.RepaintAll();
                            setWireframeMode(gameObject, objectList, false);
                        }
                        else
                        {
                            Undo.RecordObject(renderer, isEnabled ? "Disable Component" : "Enable Component");
                            renderer.enabled = !isEnabled;
                        }
                    }
                    
                    EditorUtility.SetDirty(gameObject);
                }
                currentEvent.Use();
            }
        }

        // PRIVATE
        public bool isWireframeHidden(GameObject gameObject, ObjectList objectList)
        {
            return objectList == null ? false : objectList.wireframeHiddenObjects.Contains(gameObject);
        }
        
        public void setWireframeMode(GameObject gameObject, ObjectList objectList, bool targetWireframe)
        {
            if (objectList == null && targetWireframe) objectList = HierarchyObjectListManager.getInstance().getObjectList(gameObject, true);
            if (objectList != null)
            {
                Undo.RecordObject(objectList, "Renderer Visibility Change");
                if (targetWireframe) objectList.wireframeHiddenObjects.Add(gameObject);
                else objectList.wireframeHiddenObjects.Remove(gameObject);
                EditorUtility.SetDirty(objectList);
            }
        }

        static public void setSelectedRenderState(Renderer renderer, bool visible)
        {
            #if UNITY_5_5_OR_NEWER
            EditorUtility.SetSelectedRenderState(renderer, visible ? EditorSelectedRenderState.Wireframe : EditorSelectedRenderState.Hidden);
            #else
            EditorUtility.SetSelectedWireframeHidden(renderer, visible);
            #endif
        }
    }
}

