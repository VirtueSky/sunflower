using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetRefUI : FileUI
    {
        private static Dictionary<SubAssetType, Texture2D> SubAssetIcons;
        private Texture2D GetIcon(SubAssetType subType)
        {
            if (subType == SubAssetType.Unknown) return null;
            if (SubAssetIcons == null) CacheIcon();
            return SubAssetIcons.GetValueOrDefault(subType);
        }
        
        /// <summary>
        /// Caches icons for different SubAssetTypes using Unity’s internal icon system.
        /// </summary>
        public static void CacheIcon()
        {
            SubAssetIcons = new Dictionary<SubAssetType, Texture2D>
            {
                [SubAssetType.Sprite] = LoadIcon("d_Texture Icon"),
                [SubAssetType.Mesh] = LoadIcon("d_Mesh Icon"),
                [SubAssetType.AnimationClip] = LoadIcon("d_AnimationClip Icon"),
                [SubAssetType.Avatar] = LoadIcon("d_Avatar Icon"),
                [SubAssetType.Material] = LoadIcon("d_Material Icon"),
                [SubAssetType.AudioMixerSnapshot] = LoadIcon("d_AudioMixerController Icon"),
                [SubAssetType.LightingDataAsset] = LoadIcon("LightingDataAsset Icon", "LightProbeGroup Icon"),
                [SubAssetType.ScriptableObject] = LoadIcon("d_ScriptableObject Icon"),
                [SubAssetType.ShaderVariantCollection] = LoadIcon("d_Shader Icon"),
                [SubAssetType.Texture2DArray] = LoadIcon("d_Texture Icon"),
                [SubAssetType.Cubemap] = LoadIcon("d_Cubemap Icon", "PreTexCube"),
                [SubAssetType.SpineAnimation] = LoadIcon("d_Animation Icon"),
                [SubAssetType.SpineSkeletonData] = LoadIcon("d_Animation Icon"),
                [SubAssetType.PhysicsMaterial2D] = LoadIcon("d_PhysicMaterial Icon", "d_Texture Icon")
            };
        }
        
        /// <summary>
        /// Loads a Unity Editor icon with optional fallback support.
        /// </summary>
        private static Texture2D LoadIcon(string iconName, string fallbackIcon = null)
        {
            Texture2D icon = EditorGUIUtility.IconContent(iconName).image as Texture2D;
            if (icon == null && !string.IsNullOrEmpty(fallbackIcon))
            {
                icon = EditorGUIUtility.IconContent(fallbackIcon).image as Texture2D;
            }
            return icon ?? EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D;
        }
        
        
        private readonly List<SubAssetDetail> details = new List<SubAssetDetail>();
        
        public AssetRefUI(string guid, string path, List<long> localIds):base(guid, path)
        {
            var file = AssetFinderCacheAsset.GetFile(guid);

            for (var i = 0; i < localIds.Count; i++)
            {
                long localId = localIds[i];
                var detail = file.GetSubDetail(localId);
                details.Add(detail);
            }
        }
        
        public void DrawSubDetails(Rect rect, SubAssetDetail detail)
        {
            if (detail == null) return;
            rect.xMin += 16f;
            rect.height = AssetFinderTheme.Current.TreeItemHeight;
            
            Rect iconRect = new Rect(rect.x, rect.y, 16f, 16f);
            var icon = GetIcon(detail.type);

            if (icon != null)
            {
                GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }
            
            // Draw label underneath
            var labelRect = new Rect(rect.x + 16f, rect.y, rect.width, 16);
            GUI.Label(labelRect, detail.name);
        }
        
        public void Draw(ref Rect rect)
        {
            // Draw main Asset
            Rect r1 = rect;
            DrawAsset(ref r1, true, false);
            rect.y += 18f;
            
            // Draw references: should exclude main asset?
            for (var i = 0; i < details.Count; i++)
            {
                var detail = details[i];
                if (detail == null) continue;
                
                DrawSubDetails(rect, details[i]);
                rect.y += 18f;
            }
        }
    }
}
