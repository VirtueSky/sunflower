using UnityEngine;
using UnityEditor;
using System;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;
using System.Collections.Generic;
using System.Collections;

namespace VirtueSky.Hierarchy.HComponent
{
    public class TreeMapComponent: BaseComponent
    {
        // CONST
        private const float TREE_STEP_WIDTH  = 14.0f;
        
        // PRIVATE
        private Texture2D treeMapLevelTexture;       
        private Texture2D treeMapLevel4Texture;       
        private Texture2D treeMapCurrentTexture;   
        private Texture2D treeMapLastTexture;
        private Texture2D treeMapObjectTexture;    
        private bool enhanced;
        private bool transparentBackground;
        private Color backgroundColor;
        private Color treeMapColor;
        
        // CONSTRUCTOR
        public TreeMapComponent()
        { 

            treeMapLevelTexture   = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTreeMapLevel);
            treeMapLevel4Texture  = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTreeMapLevel4);
            treeMapCurrentTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTreeMapCurrent);
            #if UNITY_2018_3_OR_NEWER
                treeMapObjectTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTreeMapLine);
            #else
                treeMapObjectTexture  = QResources.getInstance().getTexture(QTexture.QTreeMapObject);
            #endif
            treeMapLastTexture    = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTreeMapLast);
            
            rect.width  = 14;
            rect.height = 16;
            
            showComponentDuringPlayMode = true;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalBackgroundColor, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TreeMapShow           , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TreeMapColor          , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TreeMapEnhanced       , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TreeMapTransparentBackground, settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged() {
            backgroundColor     = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalBackgroundColor);
            enabled             = HierarchySettings.getInstance().get<bool>(HierarchySetting.TreeMapShow);
            treeMapColor        = HierarchySettings.getInstance().getColor(HierarchySetting.TreeMapColor);
            enhanced            = HierarchySettings.getInstance().get<bool>(HierarchySetting.TreeMapEnhanced);
            transparentBackground = HierarchySettings.getInstance().get<bool>(HierarchySetting.TreeMapTransparentBackground);
        }
        
        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            rect.y = selectionRect.y;
            
            if (!transparentBackground) 
            {
                rect.x = 0;
                
                rect.width = selectionRect.x - 14;
                EditorGUI.DrawRect(rect, backgroundColor);
                rect.width = 14;
            }

            return LayoutStatus.Success;
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            int childCount = gameObject.transform.childCount;
            int level = Mathf.RoundToInt(selectionRect.x / 14.0f);

            if (enhanced)
            {
                Transform gameObjectTransform = gameObject.transform;
                Transform parentTransform = null;

                for (int i = 0, j = level - 1; j >= 0; i++, j--)
                {
                    rect.x = 14 * j;
                    if (i == 0)
                    {
                        if (childCount == 0) {
                            #if UNITY_2018_3_OR_NEWER
                                HierarchyColorUtils.setColor(treeMapColor);
                            #endif
                            GUI.DrawTexture(rect, treeMapObjectTexture);
                        }
                        gameObjectTransform = gameObject.transform;
                    }
                    else if (i == 1)
                    {
                        HierarchyColorUtils.setColor(treeMapColor);
                        if (parentTransform == null) {
                            if (gameObjectTransform.GetSiblingIndex() == gameObject.scene.rootCount - 1) {
                                GUI.DrawTexture(rect, treeMapLastTexture);
                            } else {
                                GUI.DrawTexture(rect, treeMapCurrentTexture);
                            }
                        } else if (gameObjectTransform.GetSiblingIndex() == parentTransform.childCount - 1) {
                            GUI.DrawTexture(rect, treeMapLastTexture);
                        } else {
                            GUI.DrawTexture(rect, treeMapCurrentTexture);
                        }
                        gameObjectTransform = parentTransform;
                    }
                    else
                    {
                        if (parentTransform == null) {
                            if (gameObjectTransform.GetSiblingIndex() != gameObject.scene.rootCount - 1)
                                GUI.DrawTexture(rect, treeMapLevelTexture);
                        } else if (gameObjectTransform.GetSiblingIndex() != parentTransform.childCount - 1)
                            GUI.DrawTexture(rect, treeMapLevelTexture);

                        gameObjectTransform = parentTransform;                       
                    }
                    if (gameObjectTransform != null) 
						parentTransform = gameObjectTransform.parent;
					else
                        break;
                }
                HierarchyColorUtils.clearColor();
            }
            else
            {
                for (int i = 0, j = level - 1; j >= 0; i++, j--)
                {
                    rect.x = 14 * j;
                    if (i == 0)
                    {
                        if (childCount > 0)
                            continue;
                        else {
                            #if UNITY_2018_3_OR_NEWER
                                HierarchyColorUtils.setColor(treeMapColor);
                            #endif
                            GUI.DrawTexture(rect, treeMapObjectTexture);
                        }
                    }
                    else if (i == 1)
                    {
                        HierarchyColorUtils.setColor(treeMapColor);
                        GUI.DrawTexture(rect, treeMapCurrentTexture);
                    }
                    else
                    {
                        rect.width = 14 * 4;
                        rect.x -= 14 * 3;
                        j -= 3;
                        GUI.DrawTexture(rect, treeMapLevel4Texture);
                        rect.width = 14;
                    }
                }
                HierarchyColorUtils.clearColor();
            }
        }
    }
}

