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
    public class TagIconComponent: BaseComponent
    {
        private List<TagTexture> tagTextureList;

        // CONSTRUCTOR
        public TagIconComponent()
        {
            rect.width  = 14;
            rect.height = 14;

            HierarchySettings.getInstance().addEventListener(HierarchySetting.TagIconShow              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TagIconShowDuringPlayMode, settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TagIconSize              , settingsChanged);
            HierarchySettings.getInstance().addEventListener(HierarchySetting.TagIconList              , settingsChanged);
            settingsChanged();
        }
        
        // PRIVATE
        private void settingsChanged()
        {
            enabled = HierarchySettings.getInstance().get<bool>(HierarchySetting.TagIconShow);
            showComponentDuringPlayMode = HierarchySettings.getInstance().get<bool>(HierarchySetting.TagIconShowDuringPlayMode);
            HierarchySizeAll size = (HierarchySizeAll)HierarchySettings.getInstance().get<int>(HierarchySetting.TagIconSize);
            rect.width = rect.height = (size == HierarchySizeAll.Normal ? 15 : (size == HierarchySizeAll.Big ? 16 : 13));        
            this.tagTextureList = TagTexture.loadTagTextureList();
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
            string gameObjectTag = "";
            try { gameObjectTag = gameObject.tag; }
            catch {}

            TagTexture tagTexture = tagTextureList.Find(t => t.tag == gameObjectTag);
            if (tagTexture != null && tagTexture.texture != null)
            {
                GUI.DrawTexture(rect, tagTexture.texture, ScaleMode.ScaleToFit, true);
            }
        }
    }
}

