using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using VirtueSky.Hierarchy.HComponent;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;
using System.Reflection;

namespace VirtueSky.Hierarchy.phierarchy
{
    public class VHierarchy
    {
        // PRIVATE
        private HashSet<int> errorHandled = new HashSet<int>();      
        private Dictionary<int, BaseComponent> componentDictionary;          
        private List<BaseComponent> preComponents;
        private List<BaseComponent> orderedComponents;
        private bool hideIconsIfThereIsNoFreeSpace;
        private int indentation;
        private Texture2D trimIcon;
        private Color backgroundColor;
        private Color inactiveColor;

        // CONSTRUCTOR
        public VHierarchy ()
        {           
            componentDictionary = new Dictionary<int, BaseComponent>();
            componentDictionary.Add((int)HierarchyComponentEnum.LockComponent             , new LockComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.VisibilityComponent       , new VisibilityComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.StaticComponent           , new StaticComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.RendererComponent         , new RendererComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.TagAndLayerComponent      , new TagLayerComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.GameObjectIconComponent   , new GameObjectIconComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.ErrorComponent            , new ErrorComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.TagIconComponent          , new TagIconComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.LayerIconComponent        , new LayerIconComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.ColorComponent            , new ColorComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.ComponentsComponent       , new ComponentsComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.ChildrenCountComponent    , new ChildrenCountComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.PrefabComponent           , new PrefabComponent());
            componentDictionary.Add((int)HierarchyComponentEnum.VerticesAndTrianglesCount , new VerticesAndTrianglesCountComponent());

            preComponents = new List<BaseComponent>();
            preComponents.Add(new MonoBehaviorIconComponent());
            preComponents.Add(new TreeMapComponent());
            preComponents.Add(new SeparatorComponent());

            orderedComponents = new List<BaseComponent>();

            trimIcon = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTrimIcon);

            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalIdentation             , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.ComponentsOrder                  , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalHideIconsIfNotFit      , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalBackgroundColor        , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalInactiveColor          , settingsChanged);
            settingsChanged();
        }
         
        // PRIVATE
        private void settingsChanged()
        {
            string componentOrder = HierarchySettings.getInstance().get<string>(HierarchySetting.ComponentsOrder);
            string[] componentIds = componentOrder.Split(';');
            if (componentIds.Length != HierarchySettings.DEFAULT_ORDER_COUNT) 
            {
                HierarchySettings.getInstance().set(HierarchySetting.ComponentsOrder, HierarchySettings.DEFAULT_ORDER, false);
                componentIds = HierarchySettings.DEFAULT_ORDER.Split(';');
            }

            orderedComponents.Clear(); 
            for (int i = 0; i < componentIds.Length; i++)                
                orderedComponents.Add(componentDictionary[int.Parse(componentIds[i])]);
            orderedComponents.Add(componentDictionary[(int)HierarchyComponentEnum.ComponentsComponent]);

            indentation                     = HierarchySettings.getInstance().get<int>(HierarchySetting.AdditionalIdentation);
            hideIconsIfThereIsNoFreeSpace   = HierarchySettings.getInstance().get<bool>(HierarchySetting.AdditionalHideIconsIfNotFit);
            backgroundColor                 = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalBackgroundColor);
            inactiveColor                   = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalInactiveColor);
        } 

        public void hierarchyWindowItemOnGUIHandler(int instanceId, Rect selectionRect)
        {
            try
            {
                HierarchyColorUtils.setDefaultColor(GUI.color);

                GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceId);
                if (gameObject == null) return;

                Rect curRect = new Rect(selectionRect);
                curRect.width = 16;
                curRect.x += selectionRect.width - indentation;

                float gameObjectNameWidth = hideIconsIfThereIsNoFreeSpace ? GUI.skin.label.CalcSize(new GUIContent(gameObject.name)).x : 0;

                ObjectList objectList = HierarchyObjectListManager.getInstance().getObjectList(gameObject, false);

                drawComponents(orderedComponents, selectionRect, ref curRect, gameObject, objectList, true, hideIconsIfThereIsNoFreeSpace ? selectionRect.x + gameObjectNameWidth + 7 : 0);    

                errorHandled.Remove(instanceId);
            }
            catch (Exception exception)
            {
                if (errorHandled.Add(instanceId))
                {
                    Debug.LogError(exception.ToString());
                }
            }
        }

        private void drawComponents(List<BaseComponent> components, Rect selectionRect, ref Rect rect, GameObject gameObject, ObjectList objectList, bool drawBackground = false, float minX = 50)
        {
            if (Event.current.type == EventType.Repaint)
            {
                int toComponent = components.Count;
                LayoutStatus layoutStatus = LayoutStatus.Success;
                for (int i = 0, n = toComponent; i < n; i++)
                {
                    BaseComponent component = components[i];
                    if (component.isEnabled())
                    {
                        layoutStatus = component.layout(gameObject, objectList, selectionRect, ref rect, rect.x - minX);
                        if (layoutStatus != LayoutStatus.Success)
                        {
                            toComponent = layoutStatus == LayoutStatus.Failed ? i : i + 1;
                            rect.x -= 7;

                            break;
                        }
                    }
                    else
                    {
                        component.disabledHandler(gameObject, objectList);
                    }
                } 

                if (drawBackground)
                {
                    if (backgroundColor.a != 0)
                    {
                        rect.width = selectionRect.x + selectionRect.width - rect.x /*- indentation*/;
                        EditorGUI.DrawRect(rect, backgroundColor);
                    }
                    drawComponents(preComponents    , selectionRect, ref rect, gameObject, objectList);
                }

                for (int i = 0, n = toComponent; i < n; i++)
                {
                    BaseComponent component = components[i];
                    if (component.isEnabled())
                    {
                        component.draw(gameObject, objectList, selectionRect);
                    }
                }

                if (layoutStatus != LayoutStatus.Success)
                {
                    rect.width = 7;
                    HierarchyColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, trimIcon);
                    HierarchyColorUtils.clearColor();
                }
            }
            else if (Event.current.isMouse)
            {
                for (int i = 0, n = components.Count; i < n; i++)
                {
                    BaseComponent component = components[i];
                    if (component.isEnabled())
                    {
                        if (component.layout(gameObject, objectList, selectionRect, ref rect, rect.x - minX) != LayoutStatus.Failed)
                        {
                            component.eventHandler(gameObject, objectList, Event.current);
                        }
                    }
                }
            }
        }
    }
}

