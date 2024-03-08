using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy.Data;
using VirtueSky.Hierarchy.Helper;

namespace VirtueSky.Hierarchy.HComponent
{
    public class PrefabComponent: BaseComponent
    {
        // PRIVATE
        private Color activeColor;
        private Color inactiveColor;
        private Texture2D prefabTexture;
        private bool showPrefabConnectedIcon;

        // CONSTRUCTOR
        public PrefabComponent()
        {
            rect.width = 9;

            prefabTexture = HierarchyResources.getInstance().getTexture(HierarchyTexture.HierarchyPrefabIcon);

            HierarchySettings.getInstance().addEventListener(HierarchySetting.PrefabShowBreakedPrefabsOnly  , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.PrefabShow                    , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalActiveColor         , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.AdditionalInactiveColor       , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            showPrefabConnectedIcon = HierarchySettings.getInstance().get<bool>(HierarchySetting.PrefabShowBreakedPrefabsOnly);
            enabled                 = HierarchySettings.getInstance().get<bool>(HierarchySetting.PrefabShow);
            activeColor             = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalActiveColor);
            inactiveColor           = HierarchySettings.getInstance().getColor(HierarchySetting.AdditionalInactiveColor);
        }

        // DRAW
        public override LayoutStatus layout(GameObject gameObject, ObjectList objectList, Rect selectionRect, ref Rect curRect, float maxWidth)
        {
            if (maxWidth < 9)
            {
                return LayoutStatus.Failed;
            }
            else
            {
                curRect.x -= 9;
                rect.x = curRect.x;
                rect.y = curRect.y;
                return LayoutStatus.Success;
            }
        }
        
        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {
            #if UNITY_2018_3_OR_NEWER
                PrefabInstanceStatus prefabStatus = PrefabUtility.GetPrefabInstanceStatus(gameObject);
                if (prefabStatus == PrefabInstanceStatus.MissingAsset ||
                    prefabStatus == PrefabInstanceStatus.Disconnected) {
                    HierarchyColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, prefabTexture);
                    HierarchyColorUtils.clearColor();
                } else if (!showPrefabConnectedIcon && prefabStatus != PrefabInstanceStatus.NotAPrefab) {
                    HierarchyColorUtils.setColor(activeColor);
                    GUI.DrawTexture(rect, prefabTexture);
                    HierarchyColorUtils.clearColor();
                }
            #else
                PrefabType prefabType = PrefabUtility.GetPrefabType(gameObject);
                if (prefabType == PrefabType.MissingPrefabInstance || 
                    prefabType == PrefabType.DisconnectedPrefabInstance ||
                    prefabType == PrefabType.DisconnectedModelPrefabInstance)
                {
                    QColorUtils.setColor(inactiveColor);
                    GUI.DrawTexture(rect, prefabTexture);
                    QColorUtils.clearColor();
                }
                else if (!showPrefabConnectedIcon && prefabType != PrefabType.None)
                {
                    QColorUtils.setColor(activeColor);
                    GUI.DrawTexture(rect, prefabTexture);
                    QColorUtils.clearColor();
                }
            #endif
        }
    }
}

