using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderCache
    {
        internal static void DelayCheck4Changes()
        {
            EditorApplication.update -= Check;
            EditorApplication.update += Check;
        }

        private static void Check()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating || AssetFinderSettingExt.disable)
            {
                delayCounter = 100;
                return;
            }

            if (Api == null) return;
            if (delayCounter-- > 0) return;
            
            
            EditorApplication.update -= Check;
            Api.IncrementalRefresh();
        }

        internal void Check4Changes(bool force)
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating || AssetFinderSettingExt.disable)
            {
                DelayCheck4Changes();
                return;
            }

            ready = false;
            ReadFromProject(force);

            // AssetFinderLOG.Log($"After ReadFromProject :: WorkCount: {workCount}, AssetMap: {AssetMap.Count}, AssetList: {AssetList.Count}");
            Check4Work();
        }

        internal void RefreshUsedByOnlyFromCache()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating || AssetFinderSettingExt.disable) return;
            ready = false;
            ReadFromCache();
            workCount = 0;
            if (queueLoadContent != null) queueLoadContent.Clear();
            Check4Usage();
        }

        internal void IncrementalRefresh()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating || AssetFinderSettingExt.disable)
            {
                DelayCheck4Changes();
                return;
            }
            
            ready = false;
            workCount = 0;
            if (queueLoadContent != null) queueLoadContent.Clear();
            if (AssetMap == null)
            {
                Debug.LogWarning("Why should the AssetMap == null? The FR2 cache might be incompatible?");
                return;
            }
            
            // CRITICAL FIX: First check for new assets that were added to the project
            var paths = AssetDatabase.GetAllAssetPaths();
            cacheStamp++;
            
            // Check for new assets
            foreach (string p in paths)
            {
                bool isValid = AssetFinderUnity.StringStartsWith(p, "Assets/", "Packages/", "Library/", "ProjectSettings/");
                if (!isValid) continue;
                
                string guid = AssetDatabase.AssetPathToGUID(p);
                if (!AssetFinderAsset.IsValidGUID(guid)) continue;

                if (!AssetMap.TryGetValue(guid, out AssetFinderAsset asset))
                {
                    // New asset detected - add it
                    AddAsset(guid, false); // Don't force, let auto refresh logic decide
                }
                else
                {
                    // Mark existing asset so it won't be deleted
                    asset.refreshStamp = cacheStamp;
                }
            }
            
            // Remove deleted assets
            for (int i = AssetList.Count - 1; i >= 0; i--)
            {
                if (AssetList[i].refreshStamp != cacheStamp) RemoveAsset(AssetList[i]);
            }
            
            // Only process dirty assets and assets that have never been scanned
            foreach (var asset in AssetList) // only scan in AssetList
            {
                // Skip non-critical assets
                if (!asset.IsCriticalAsset())
                {
                    if (asset.isDirty)
                    {
                        AssetFinderLOG.Log($"[INVALID] non-critical asset is dirty???\n" +
                                $" asset: {asset.assetPath}: isCritical = {asset.IsCriticalAsset()} | isDirty = {asset.isDirty} | assetType: {asset.type}");
                    }
                    continue;
                }
                
                // Skip ignored assets - they shouldn't have their content read
                if (asset.IsExcluded)
                {
                    AssetFinderLOG.Log($"Skipping ignored asset: {asset.assetPath}");
                    continue;
                }
                
                // Only process if asset is dirty or has never been scanned
                if (asset.isDirty || !asset.hasBeenScanned)
                {
                    workCount++;
                    queueLoadContent.Add(asset);
                }
            }
            
            // Clear the HasChanged flag since we're now processing the changes
            HasChanged = false;
            
            AssetFinderLOG.Log($"Incremental refresh: Processing {workCount} dirty/unscanned assets");
            Check4Work();
        }

        internal void Check4Usage()
        {
            currentState = ProcessingState.BuildingUsedBy;
            
            // CRITICAL FIX: Clear UsedByMap for ALL assets in AssetMap, not just AssetList
            // This ensures that non-critical assets (like PNGs) get their stale references cleared
            foreach (var kvp in AssetMap)
            {
                var item = kvp.Value;
                if (item.IsMissing) continue;
                AssetFinderUnity.Clear(ref item.UsedByMap);
            }

            foreach (var item in AssetList)
            {
                if (item.IsMissing) continue;
                AsyncUsedBy(item);
            }
            workCount = 0;
            ready = true;
            currentState = ProcessingState.Idle;
            HasChanged = false; // Clear dirty state when processing is complete
            onReady?.Invoke();
        }

        internal void Check4Work()
        {
            if (workCount == 0)
            {
                Check4Usage();
                return;
            }

            ready = false;
            currentState = ProcessingState.ReadingContent;
            EditorApplication.update -= AsyncProcess;
            EditorApplication.update += AsyncProcess;
            AssetFinderAsset.ClearLog();
        }

        internal void AsyncProcess()
        {
            if (this == null) return;
            if (AssetFinderSettingExt.disable) return;
            if (EditorApplication.isCompiling || EditorApplication.isUpdating) return;
            if (frameSkipped++ < 10 - 2 * priority) return;

            frameSkipped = 0;
            float t = Time.realtimeSinceStartup;

            // AssetFinderLOG.Log("AsyncProcess: time=" + Mathf.Round(t) + " : progress = " + progress*workCount + "/" + workCount + " : isReady =" + isReady + " ::: queueLoadCount = " + queueLoadContent.Count);

            if (!AsyncWork(queueLoadContent, AsyncLoadContent, t)) return;
            AssetFinderAsset.WriteTotalScanTime();
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            EditorApplication.update -= AsyncProcess;
            if (HasPendingChanges())
            {
                AssetFinderLOG.Log("FR2: Detected changes during processing, restarting incremental refresh");
                IncrementalRefresh();
                return;
            }
            
            Check4Usage();
        }

        
        private bool HasPendingChanges()
        {
            return AssetMap.Any(kvp => kvp.Value.isDirty && !queueLoadContent.Contains(kvp.Value));
        }

        internal bool AsyncWork<T>(List<T> arr, Action<int, T> action, float t)
        {
            const float FRAME_DURATION = 1f / 60f; // Cache as const to avoid division
            float endTime = t + FRAME_DURATION; // Calculate end time once

            int c = arr.Count;
            while (c-- > 0)
            {
                T last = arr[c];
                arr.RemoveAt(c);
                action(c, last);

                // Check time less frequently to reduce overhead
                if (Time.realtimeSinceStartup >= endTime) return false;
            }

            if (GC_CountDown-- <= 0) // GC every 5 frames
            {
                GC.Collect(2, GCCollectionMode.Forced, true, true);
                GC_CountDown = 5;
            }

            return c <= 0;
        }

        internal void AsyncLoadContent(int idx, AssetFinderAsset asset)
        {
            // Update the current asset name
            currentAssetName = asset.assetPath;

            if (asset.fileInfoDirty) asset.LoadFileInfo();
            if (asset.fileContentDirty) asset.LoadContentFast();
        }

        internal void AsyncUsedBy(AssetFinderAsset asset)
        {
            if (AssetMap == null) Check4Changes(false);

            if (asset.IsFolder) return;

            // AssetFinderLOG.Log("Async UsedBy: " + asset.assetPath);

            foreach (KeyValuePair<string, HashSet<long>> item in asset.UseGUIDs)
            {
                if (!AssetMap.TryGetValue(item.Key, out AssetFinderAsset tAsset)) continue;
                if (tAsset == null || tAsset.UsedByMap == null) continue;

                if (!tAsset.UsedByMap.ContainsKey(asset.guid)) tAsset.AddUsedBy(asset.guid, asset);
            }
        }
    }
} 