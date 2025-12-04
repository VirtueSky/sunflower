using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderCache
    {
        internal void ReadFromCache()
        {
            if (AssetFinderSettingExt.disable)
            {
                AssetFinderLOG.LogWarning("Something wrong??? FR2 is disabled!");
            }

            if (AssetList == null) AssetList = new List<AssetFinderAsset>();

            AssetFinderUnity.Clear(ref queueLoadContent);
            AssetFinderUnity.Clear(ref AssetMap);

            // Create a new filtered list for critical assets only
            var filteredAssetList = new List<AssetFinderAsset>();

            for (var i = 0; i < AssetList.Count; i++)
            {
                AssetFinderAsset item = AssetList[i];
                item.state = AssetFinderAsset.AssetState.CACHE;

                string path = AssetDatabase.GUIDToAssetPath(item.guid);
                if (string.IsNullOrEmpty(path))
                {
                    item.type = AssetFinderAsset.AssetType.UNKNOWN; // to make sure if GUIDs being reused for a different kind of asset
                    item.state = AssetFinderAsset.AssetState.MISSING;
                    AssetMap.Add(item.guid, item);
                    
                    // Only keep critical assets in AssetList
                    if (item.IsCriticalAsset())
                    {
                        filteredAssetList.Add(item);
                    }
                    continue;
                }

                if (AssetMap.ContainsKey(item.guid))
                {
					AssetFinderLOG.LogWarning("Something wrong, cache found twice <" + item.guid + ">");
                    continue;
                }

                AssetMap.Add(item.guid, item);
                
                // Only keep critical assets in AssetList
                if (item.IsCriticalAsset())
                {
                    filteredAssetList.Add(item);
                }
            }
            
            // Replace AssetList with filtered list containing only critical assets
            AssetList = filteredAssetList;
        }

        internal void ClearCacheCompletely()
        {
            // AssetFinderLOG.Log("=== ClearCacheCompletely START ===");
            // AssetFinderLOG.Log($"Before Clear - AssetList: {AssetList?.Count ?? 0}, AssetMap: {AssetMap?.Count ?? 0}, queueLoadContent: {queueLoadContent?.Count ?? 0}");
            
            // Clear all cache data structures
            if (AssetList != null) AssetList.Clear();
            else AssetList = new List<AssetFinderAsset>();
            
            if (AssetMap != null) AssetMap.Clear();
            else AssetMap = new Dictionary<string, AssetFinderAsset>();
            
            if (queueLoadContent != null) queueLoadContent.Clear();
            else queueLoadContent = new List<AssetFinderAsset>();
            
            // Reset state
            ready = false;
            workCount = 0;
            cacheStamp = 0;
            HasChanged = false;
            currentState = ProcessingState.Idle;
            
            System.GC.Collect();
        }

        internal void ReadFromProject(bool force)
        {
            if (AssetMap == null || AssetMap.Count == 0) ReadFromCache();
            foreach (string b in AssetFinderAsset.BUILT_IN_ASSETS)
            {
                if (AssetMap.ContainsKey(b)) continue;
                var asset = new AssetFinderAsset(b);
                AssetMap.Add(b, asset);
                
                // Only add built-in assets to AssetList if they are critical
                if (asset.IsCriticalAsset())
                {
                    AssetList.Add(asset);
                }
            }

            string[] paths = AssetDatabase.GetAllAssetPaths();
            cacheStamp++;
            workCount = 0;
            if (queueLoadContent != null) queueLoadContent.Clear();

            // Check for new assets
            int validPaths = 0;
            int newAssets = 0;
            int existingAssets = 0;
            foreach (string p in paths)
            {
                bool isValid = AssetFinderUnity.StringStartsWith(p, "Assets/", "Packages/", "Library/", "ProjectSettings/");
                if (!isValid)
                {
                    continue; // Skip invalid paths silently to avoid log spam
                }
                
                validPaths++;
                string guid = AssetDatabase.AssetPathToGUID(p);
                if (!AssetFinderAsset.IsValidGUID(guid)) 
                {
                    continue;
                }

                if (!AssetMap.TryGetValue(guid, out AssetFinderAsset asset))
                {
                    newAssets++;
                    AddAsset(guid, force);
                } else
                {
                    existingAssets++;
                    asset.refreshStamp = cacheStamp; // mark this asset so it won't be deleted
                    if (!asset.IsCriticalAsset()) continue; // not something we can handle
                    if (!asset.isDirty && !force) continue;
                    if (force) asset.MarkAsDirty(true, true);
                    if (!asset.IsExcluded && (force || _cacheJustCreated || AssetFinderSettingExt.isAutoRefreshEnabled))
                    {
                        workCount++;
                        queueLoadContent.Add(asset);    
                    }
                }
            }

            // Check for deleted assets
            for (int i = AssetList.Count - 1; i >= 0; i--)
            {
                if (AssetList[i].refreshStamp != cacheStamp) RemoveAsset(AssetList[i]);
            }
        }

        internal void RefreshAsset(string guid, bool force)
        {
            if (!AssetMap.TryGetValue(guid, out AssetFinderAsset asset)) return;
            RefreshAsset(asset, force);
        }

        internal void RefreshSelection()
        {
            string[] list = AssetFinderUnity.Selection_AssetGUIDs;
            for (var i = 0; i < list.Length; i++)
            {
                RefreshAsset(list[i], true);
            }

            Check4Work();
        }

        internal void RefreshAsset(AssetFinderAsset asset, bool force)
        {
            asset.MarkAsDirty(true, force);
            
            // If we're currently processing and this asset isn't already in the queue, add it
            if (currentState != ProcessingState.Idle && !queueLoadContent.Contains(asset))
            {
                workCount++;
                queueLoadContent.Add(asset);
            }
            
            DelayCheck4Changes();
        }

        internal void AddAsset(string guid, bool force = false)
        {
            if (AssetMap.ContainsKey(guid))
            {
                AssetFinderLOG.LogWarning("guid already exist <" + guid + ">");
                return;
            }

            var asset = new AssetFinderAsset(guid);
            asset.LoadPathInfo();
            asset.refreshStamp = cacheStamp;
            AssetMap.Add(guid, asset);

            // Do not load content for AssetFinderCache asset
            if (guid == CacheGUID) return;

            if (!asset.IsCriticalAsset()) return;

            // Critical assets (even if ignored) should be added to AssetList
            AssetList.Add(asset);
            
            // CRITICAL FIX: Always queue new assets for content loading when force=true
            bool shouldQueue = !asset.IsExcluded && (force || _cacheJustCreated || AssetFinderSettingExt.isAutoRefreshEnabled || currentState != ProcessingState.Idle);
                    // AssetFinderLOG.Log($"AddAsset: {asset.assetPath} - shouldQueue: {shouldQueue} (IsExcluded: {asset.IsExcluded}, force: {force}, _cacheJustCreated: {_cacheJustCreated}, autoRefresh: {AssetFinderSettingExt.isAutoRefreshEnabled}, currentState: {currentState})");
            
            if (shouldQueue)
            {
                workCount++;
                queueLoadContent.Add(asset);
                        // AssetFinderLOG.Log($"QUEUED new asset for content loading: {asset.assetPath}");
            }
            else
            {
                // When content loading is skipped, mark as ready but dirty for future scans
                asset.MarkAsDirty(true, false);
                        // AssetFinderLOG.Log($"SKIPPED new asset: {asset.assetPath} - marked as dirty for future scan");
            }
        }

        internal void RemoveAsset(string guid)
        {
            if (!AssetMap.ContainsKey(guid)) return;

            RemoveAsset(AssetMap[guid]);
        }

        internal void RemoveAsset(AssetFinderAsset asset)
        {
            AssetList.Remove(asset);

            // Deleted Asset : still in the map but not in the AssetList
            asset.state = AssetFinderAsset.AssetState.MISSING;
        }
    }
} 