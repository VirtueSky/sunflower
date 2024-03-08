using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using VirtueSky.Hierarchy.HComponent.Base;
using VirtueSky.Hierarchy;
using VirtueSky.Hierarchy.Helper;
using VirtueSky.Hierarchy.Data;

namespace VirtueSky.Hierarchy.HComponent
{
    public class LayerIconComponent: BaseComponent
    {
        private List<LayerTexture> layerTextureList;

        // CONSTRUCTOR
        public LayerIconComponent()
        {
            rect.width  = 14;
            rect.height = 14;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.LayerIconShow              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.LayerIconShowDuringPlayMode, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.LayerIconSize              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.LayerIconList              , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled                     = HierarchySettings.getInstance().get<bool>(HierarchySetting.LayerIconShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.LayerIconShowDuringPlayMode);
            HierarchySizeAll size      = (HierarchySizeAll)HierarchySettings.getInstance().get<int>(HierarchySetting.LayerIconSize);
            rect.width = rect.height    = (size == HierarchySizeAll.Normal ? 15 : (size == HierarchySizeAll.Big ? 16 : 13));        
            this.layerTextureList = LayerTexture.loadLayerTextureList();
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
                rect.y = curRect.y - (rect.height - 16) / 2;
                return LayoutStatus.Success;
            }
        }

        public override void draw(GameObject gameObject, ObjectList objectList, Rect selectionRect)
        {                       
            string gameObjectLayerName = LayerMask.LayerToName(gameObject.layer);

            LayerTexture layerTexture = layerTextureList.Find(t => t.layer == gameObjectLayerName);
            if (layerTexture != null && layerTexture.texture != null)
            {
                GUI.DrawTexture(rect, layerTexture.texture, ScaleMode.ScaleToFit, true);
            }
        }
    }
}

