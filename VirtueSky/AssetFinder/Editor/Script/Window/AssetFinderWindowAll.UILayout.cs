using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        internal AssetFinderRefDrawer[] _allDrawersCache;

        private void OnCSVClickExtension()
        {
            AssetFinderRef[] csvSource = null;
            AssetFinderRefDrawer drawer = GetAssetDrawer();

            if (drawer != null) csvSource = drawer.source;

            if (isFocusingUnused && (csvSource == null)) csvSource = RefUnUse.source;
            if (isFocusingUsedInBuild && (csvSource == null)) csvSource = AssetFinderRef.FromDict(UsedInBuild.refs);
            if (isFocusingDuplicate && (csvSource == null)) csvSource = AssetFinderRef.FromList(Duplicated.list);

            AssetFinderExport.ExportCSV(csvSource);
        }

        private void RefreshPanelVisible()
        {
            if (sp2 == null) InitIfNeeded();
            if (sp2 == null) return;

            sp2.splits[0].visible = isScenePanelVisible;
            sp2.splits[1].visible = isAssetPanelVisible;
            sp2.splits[2].visible = isFocusingAddressable;
            sp2.CalculateWeight();
        }

        private void RefreshShowFullPath()
        {
            if (_allDrawersCache != null)
            {
                foreach (var drawer in _allDrawersCache)
                {
                    if (drawer != null && drawer.Config != null) drawer.Config.showFullPath = settings.showFullPath;
                }
            }
        }

        private void RefreshShowFileSize()
        {
            if (_allDrawersCache != null)
            {
                foreach (var drawer in _allDrawersCache)
                {
                    if (drawer != null && drawer.AssetConfig != null) drawer.AssetConfig.showFileSize = settings.showFileSize;
                }
            }
        }

        private void RefreshShowFileExtension()
        {
            if (_allDrawersCache == null) return;
            foreach (var drawer in _allDrawersCache)
            {
                if (drawer != null && drawer.AssetConfig != null) drawer.AssetConfig.showExtension = settings.showFileExtension;
            }
        }

        private void MarkDirty()
        {
            if (_allDrawersCache != null)
            {
                foreach (var drawer in _allDrawersCache)
                {
                    drawer?.SetDirty();
                }
            }
            Duplicated.SetDirty();
            UsedInBuild.SetDirty();
            AddressableDrawer.RefreshSort();
            WillRepaint = true;
        }

        private void RefreshSort()
        {
            if (_allDrawersCache != null)
            {
                foreach (var drawer in _allDrawersCache)
                {
                    drawer?.RefreshSort();
                }
            }
            AddressableDrawer.RefreshSort();
            Duplicated.RefreshSort();
            UsedInBuild.RefreshSort();
            
            // Ensure tool-specific drawers are also refreshed
            if (settings.toolMode)
            {
                RefUnUse?.RefreshSort();
            }
        }

        private AssetFinderRefDrawer GetAssetDrawer()
        {
            if (isFocusingUses) return IsSelectingAssets ? UsesDrawer : SceneToAssetDrawer;
            if (isFocusingUsedBy) return IsSelectingAssets ? UsedByDrawer : null;
            if (isFocusingAddressable) return AddressableDrawer.drawer;
            return null;
        }
    }
} 