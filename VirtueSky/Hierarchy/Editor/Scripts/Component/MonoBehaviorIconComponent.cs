using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;

namespace VirtueSky.Hierarchy.HComponent
{
    public class MonoBehaviorIconComponent: BaseComponent
    {
        // CONST
        private const float TREE_STEP_WIDTH  = 14.0f;
        private const float TREE_STEP_HEIGHT = 16.0f;

        // PRIVATE
        private Texture2D monoBehaviourIconTexture;   
        private Texture2D monoBehaviourIconObjectTexture; 
        private bool ignoreUnityMonobehaviour;
        private Color iconColor;
        private bool showTreeMap;

        // CONSTRUCTOR 
        public MonoBehaviorIconComponent()
        {
            rect.width  = 14;
            rect.height = 16;
            
            monoBehaviourIconTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyMonoBehaviourIcon);
            monoBehaviourIconObjectTexture  = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyTreeMapObject);

            HierarchySettings.getInstance().addEventListener(HierarchySetting.MonoBehaviourIconIgnoreUnityMonobehaviour , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.MonoBehaviourIconShow                     , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.MonoBehaviourIconShowDuringPlayMode       , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.MonoBehaviourIconColor                    , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TreeMapShow                               , settingsChanged);
            settingsChanged();
        }

        // PRIVATE
        private void settingsChanged()
        {
            ignoreUnityMonobehaviour    = HierarchySettings.getInstance().get<bool>(HierarchySetting.MonoBehaviourIconIgnoreUnityMonobehaviour);
            enabled                     = HierarchySettings.getInstance().get<bool>(HierarchySetting.MonoBehaviourIconShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.MonoBehaviourIconShowDuringPlayMode);
            iconColor                   = HierarchySettings.getInstance().getColor(HierarchySetting.MonoBehaviourIconColor);
            showTreeMap                 = HierarchySettings.getInstance().get<bool>(HierarchySetting.TreeMapShow);
            EditorApplication.RepaintHierarchyWindow();  
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            bool foundCustomComponent = false;   
            if (ignoreUnityMonobehaviour)
            {
                Component[] components = gameObject.GetComponents<MonoBehaviour>();                
                for (int i = components.Length - 1; i >= 0; i--)
                {
                    if (components[i] != null && !components[i].GetType().FullName.Contains("UnityEngine")) 
                    {
                        foundCustomComponent = true;
                        break;
                    }
                }                
            }
            else
            {
                foundCustomComponent = gameObject.GetComponent<MonoBehaviour>() != null;
            }

            if (foundCustomComponent)
            {
                int ident = Mathf.FloorToInt(selectionRect.x / TREE_STEP_WIDTH) - 1;

                rect.x = ident * TREE_STEP_WIDTH;
                rect.y = selectionRect.y;
                rect.width = 16;

                #if UNITY_2018_3_OR_NEWER
                    rect.x += TREE_STEP_WIDTH + 1;
                    rect.width += 1;
                #elif UNITY_5_6_OR_NEWER
                    
                #elif UNITY_5_3_OR_NEWER
                    rect.x += TREE_STEP_WIDTH;
                #endif                

                HierarchyColorUtils.setColor(iconColor);
                GUI.DrawTexture(rect, monoBehaviourIconTexture);
                HierarchyColorUtils.clearColor();

                if (!showTreeMap && gameObject.transform.childCount == 0)
                {
                    rect.width = 14;
                    GUI.DrawTexture(rect, monoBehaviourIconObjectTexture);
                }
            }
        }
    }
}

