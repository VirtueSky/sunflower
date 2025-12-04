using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class AssetFinderWindowExtensions
    {
        // Panel visibility extensions
        internal static bool IsScenePanelVisible(this AssetFinderWindowAll window)
        {
            if (window.isFocusingAddressable) return false;

            if (window.selection.isSelectingAsset && window.isFocusingUses) // Override
            {
                return false;
            }

            if (!window.selection.isSelectingAsset && window.isFocusingUsedBy)
            {
                return true;
            }

            return window.settings.scene;
        }

        internal static bool IsAssetPanelVisible(this AssetFinderWindowAll window)
        {
            if (window.isFocusingAddressable) return false;

            if (window.selection.isSelectingAsset && window.isFocusingUses) // Override
            {
                return true;
            }

            if (!window.selection.isSelectingAsset && window.isFocusingUsedBy)
            {
                return false;
            }

            return window.settings.asset;
        }

        internal static void RefreshPanelVisibility(this AssetFinderWindowAll window)
        {
            window.sp2.splits[0].visible = window.IsScenePanelVisible();
            window.sp2.splits[1].visible = window.IsAssetPanelVisible();
            window.sp2.splits[2].visible = window.isFocusingAddressable;
            window.sp2.CalculateWeight();
        }

        // Selection management extensions
        internal static UnityObject[] GetCachedSelectionExtension(this AssetFinderWindowAll window)
        {
            int currentFrame = Time.frameCount;
            if (window._cachedSelectionFrame != currentFrame)
            {
                window._cachedSelection = Selection.objects;
                window._cachedSelectionFrame = currentFrame;
            }
            return window._cachedSelection;
        }

        // Asset drawer extension
        internal static AssetFinderRefDrawer GetAssetDrawerExtension(this AssetFinderWindowAll window)
        {
            if (window.isFocusingUses) return window.selection.isSelectingAsset ? window.UsesDrawer : window.SceneToAssetDrawer;
            if (window.isFocusingUsedBy) return window.selection.isSelectingAsset ? window.UsedByDrawer : null;
            if (window.isFocusingAddressable) return window.AddressableDrawer.drawer;
            return null;
        }
        
        internal static void OnCSVClickExtension(this AssetFinderWindowAll window)
        {
            AssetFinderRef[] csvSource = null;
            AssetFinderRefDrawer drawer = window.GetAssetDrawerExtension();

            if (drawer != null) csvSource = drawer.source;

            if (window.isFocusingUnused && (csvSource == null)) csvSource = window.RefUnUse.source;
            if (window.isFocusingUsedInBuild && (csvSource == null)) csvSource = AssetFinderRef.FromDict(window.UsedInBuild.refs);
            if (window.isFocusingDuplicate && (csvSource == null)) csvSource = AssetFinderRef.FromList(window.Duplicated.list);

            AssetFinderExport.ExportCSV(csvSource);
        }
        
        internal static void CacheAllDrawers(this AssetFinderWindowAll window)
        {
            window._allDrawersCache = new AssetFinderRefDrawer[]
            {
                window.UsedByDrawer,
                window.UsesDrawer,
                window.SceneToAssetDrawer,
                window.RefUnUse,
                window.RefInScene,
                window.RefSceneInScene,
                window.SceneUsesDrawer,
                window.UsedInBuild.Drawer
            };
        }

        internal static void RefreshShowUsageType(this AssetFinderWindowAll window)
        {
            if (window._allDrawersCache != null)
            {
                foreach (var drawer in window._allDrawersCache)
                {
                    if (drawer != null && drawer.AssetConfig != null) drawer.AssetConfig.showUsageType = window.settings.showUsageType;
                }
            }
        }

        internal static void RefreshSort(this AssetFinderWindowAll window)
        {
            if (window._allDrawersCache != null)
            {
                foreach (var drawer in window._allDrawersCache)
                {
                    drawer?.RefreshSort();
                }
            }
            window.AddressableDrawer.RefreshSort();
            window.Duplicated.RefreshSort();
            window.UsedInBuild.RefreshSort();
            
            // Ensure tool-specific drawers are also refreshed
            if (window.settings.toolMode)
            {
                window.RefUnUse?.RefreshSort();
            }
        }
    }
} 