using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderUsedInBuild : IRefDraw
    {
        private readonly AssetFinderRefDrawer drawer;
        private readonly AssetFinderTreeUI2.GroupDrawer groupDrawer;

        private bool dirty;
        internal Dictionary<string, AssetFinderRef> refs;

        public AssetFinderUsedInBuild(IWindow window, Func<AssetFinderRefDrawer.Sort> getSortMode, Func<AssetFinderRefDrawer.Mode> getGroupMode)
        {
            this.window = window;
            drawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = window,
                getSortMode = getSortMode,
                getGroupMode = getGroupMode,
                showFullPath = false,
                showFileSize = true,
                showExtension = true,
                showUsageType = false,
                showAssetBundleName = false,
                showAtlasName = false
            })
            {
                messageNoRefs = "No scene enabled in Build Settings!"
            };

            dirty = true;
            drawer.SetDirty();
        }

        public IWindow window { get; set; }
        
        // Expose internal drawer for display property access
        public AssetFinderRefDrawer Drawer => drawer;


        public int ElementCount()
        {
            return refs?.Count ?? 0;
        }

        public bool Draw(Rect rect)
        {
            if (dirty) RefreshView();
            return drawer.Draw(rect);
        }

        public bool DrawLayout()
        {
            if (dirty) RefreshView();
            return drawer.DrawLayout();
        }

        public void SetDirty()
        {
            dirty = true;
            drawer.SetDirty();
        }

        public void RefreshView()
        {
            var scenes = new HashSet<string>();

            foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
            {
                if (scene == null) continue;
                if (scene.enabled == false) continue;
                string sce = AssetDatabase.AssetPathToGUID(scene.path);
                if (scenes.Contains(sce)) continue;
                scenes.Add(sce);
            }

            refs = new Dictionary<string, AssetFinderRef>();
            Dictionary<string, AssetFinderRef> directRefs = AssetFinderRef.FindUsage(scenes.ToArray());
            foreach (string scene in scenes)
            {
                if (!directRefs.TryGetValue(scene, out AssetFinderRef asset)) continue;
                asset.depth = 1;
            }

            List<AssetFinderAsset> list = AssetFinderCache.Api.AssetList;
            int count = list.Count;

            // Collect assets in Resources / Streaming Assets
            for (var i = 0; i < count; i++)
            {
                AssetFinderAsset item = list[i];
                if (item.inEditor) continue;
                if (item.IsExcluded) continue;
                if (item.IsFolder) continue;
                if (!item.assetPath.StartsWith("Assets/", StringComparison.Ordinal)) continue;

                if (item.inResources || item.inStreamingAsset || item.inPlugins || item.forcedIncludedInBuild
                    || !string.IsNullOrEmpty(item.AssetBundleName)
                    || !string.IsNullOrEmpty(item.AtlasName))
                {
                    if (refs.ContainsKey(item.guid)) continue;
                    refs.Add(item.guid, new AssetFinderRef(0, 1, item, null));
                }
            }

            // Collect direct references
            foreach (KeyValuePair<string, AssetFinderRef> kvp in directRefs)
            {
                AssetFinderAsset item = kvp.Value.asset;
                if (item.inEditor) continue;
                if (item.IsExcluded) continue;
                if (!item.assetPath.StartsWith("Assets/", StringComparison.Ordinal)) continue;
                if (refs.ContainsKey(item.guid)) continue;
                refs.Add(item.guid, new AssetFinderRef(0, 1, item, null));
            }

            drawer.SetRefs(refs);
            dirty = false;
        }

        internal void RefreshSort()
        {
            drawer.RefreshSort();
        }
    }
}
