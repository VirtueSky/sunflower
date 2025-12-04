using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    public enum Dependency
    {
        All,
        Direct,
        Indirect
    }

    public enum DepthFilter
    {
        All,
        Equal,
        NotEqual,
        Less,
        LessEqual,
        Greater,
        GreaterEqual
    }

    public enum Sorting
    {
        None,
        Type,
        Path,
        Size
    }

    [Serializable]
    public class AssetFinderInfo
    {
        public string guid;
        public string assetPath;
        public string fileName;
        public string extension;
        public System.Type assetType;
        public int usageCount;
        public int usedByCount;
        public bool isInBuild;
        public long fileSize;
        public bool isFolder;
        public bool isBuiltin;

        public AssetFinderInfo(string guid)
        {
            this.guid = guid;
            RefreshInfo();
        }

        internal void RefreshInfo()
        {
            if (string.IsNullOrEmpty(guid)) return;

            assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath))
            {
                fileName = "Missing";
                extension = "";
                assetType = null;
                fileSize = 0;
                isFolder = false;
                isBuiltin = false;
                return;
            }

            fileName = Path.GetFileName(assetPath);
            extension = Path.GetExtension(assetPath);
            
            // Check if it's a folder
            isFolder = AssetDatabase.IsValidFolder(assetPath);
            
            // Check if it's builtin using the proper constant
            isBuiltin = AssetFinderAsset.BUILT_IN_ASSETS.Contains(guid);

            if (!isFolder)
            {
                UnityObject obj = AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath);
                assetType = obj?.GetType();

                // Get file size
                try
                {
                    if (File.Exists(assetPath))
                    {
                        var fileInfo = new FileInfo(assetPath);
                        fileSize = fileInfo.Length;
                    }
                }
                catch
                {
                    fileSize = 0;
                }
            }
            else
            {
                assetType = typeof(DefaultAsset);
                fileSize = 0;
            }

            // Get usage counts from AssetFinderAsset if available
            if (AssetFinderCache.isReady)
            {
                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset != null)
                {
                    usageCount = asset.UseGUIDsCount;
                    usedByCount = asset.UsedByMap.Count;
                }
            }
        }

        internal void UpdateBuildStatus(HashSet<string> buildGuids)
        {
            isInBuild = buildGuids != null && buildGuids.Contains(guid);
        }
    }

    public static class AssetFinder
    {
        public static bool IsReady => AssetFinderCache.isReady;

        public static void ScanProject()
        {
            AssetFinderCache.DeleteCache();
            AssetFinderCache.CreateCache();
        }

        public static void Refresh()
        {
            if (!AssetFinderCache.hasCache)
            {
                AssetFinderLOG.LogWarning("FR2 cache not found. Use FR2.ScanProject() first.");
                return;
            }

            AssetFinderCache.Api.Check4Changes(true);
        }

        public static List<AssetFinderInfo> GetUses(string[] guids, Dependency dep = Dependency.All, int depth = 0, DepthFilter filter = DepthFilter.All, Sorting sort = Sorting.None)
        {
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return new List<AssetFinderInfo>();
            }

            if (guids == null || guids.Length == 0) return new List<AssetFinderInfo>();

            Dictionary<string, AssetFinderRef> refs = AssetFinderRef.FindUsage(guids);
            return ProcessResults(refs, dep, depth, filter, sort);
        }

        public static List<AssetFinderInfo> GetUsedBy(string[] guids, Dependency dep = Dependency.All, int depth = 0, DepthFilter filter = DepthFilter.All, Sorting sort = Sorting.None)
        {
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return new List<AssetFinderInfo>();
            }

            if (guids == null || guids.Length == 0) return new List<AssetFinderInfo>();

            Dictionary<string, AssetFinderRef> refs = AssetFinderRef.FindUsedBy(guids);
            return ProcessResults(refs, dep, depth, filter, sort);
        }

        public static List<AssetFinderInfo> GetUnused(Sorting sort = Sorting.None)
        {
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return new List<AssetFinderInfo>();
            }

            List<AssetFinderAsset> unusedAssets = AssetFinderCache.Api.ScanUnused(true);
            return ProcessAssetResults(unusedAssets, sort);
        }

        public static List<AssetFinderInfo> GetInBuild(Sorting sort = Sorting.None)
        {
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return new List<AssetFinderInfo>();
            }

            var usedInBuild = new AssetFinderUsedInBuild(null, () => ConvertSorting(sort), () => AssetFinderRefDrawer.Mode.Type);
            usedInBuild.RefreshView();

            if (usedInBuild.refs == null) return new List<AssetFinderInfo>();

            var buildGuids = new HashSet<string>(usedInBuild.refs.Keys);
            var assets = usedInBuild.refs.Values.Select(r => r.asset).ToList();
            var results = ProcessAssetResults(assets, sort);
            
            // Set isInBuild flag for all returned assets
            foreach (var assetInfo in results)
            {
                assetInfo.UpdateBuildStatus(buildGuids);
            }
            
            return results;
        }

        public static Dictionary<string, int> GetUsesCount(string[] guids)
        {
            var result = new Dictionary<string, int>();
            
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return result;
            }

            if (guids == null || guids.Length == 0) return result;

            foreach (string guid in guids)
            {
                if (string.IsNullOrEmpty(guid)) continue;
                
                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                result[guid] = asset?.UseGUIDsCount ?? 0;
            }

            return result;
        }

        public static Dictionary<string, int> GetUsedByCount(string[] guids)
        {
            var result = new Dictionary<string, int>();
            
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return result;
            }

            if (guids == null || guids.Length == 0) return result;

            foreach (string guid in guids)
            {
                if (string.IsNullOrEmpty(guid)) continue;
                
                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                result[guid] = asset?.UsedByMap.Count ?? 0;
            }

            return result;
        }

        public static bool IsUses(string[] guids)
        {
            if (!IsReady || guids == null || guids.Length == 0) return false;

            foreach (string guid in guids)
            {
                if (string.IsNullOrEmpty(guid)) continue;
                
                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset != null && asset.UseGUIDsCount > 0) return true;
            }

            return false;
        }

        public static bool IsUsedBy(string[] guids)
        {
            if (!IsReady || guids == null || guids.Length == 0) return false;

            foreach (string guid in guids)
            {
                if (string.IsNullOrEmpty(guid)) continue;
                
                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset != null && asset.UsedByMap.Count > 0) return true;
            }

            return false;
        }

        public static bool IsInBuild(string[] guids)
        {
            if (!IsReady)
            {
                AssetFinderLOG.LogWarning("FR2 cache not ready. Use FR2.ScanProject() first.");
                return false;
            }

            if (guids == null || guids.Length == 0) return false;

            var usedInBuild = new AssetFinderUsedInBuild(null, () => AssetFinderRefDrawer.Sort.Type, () => AssetFinderRefDrawer.Mode.Type);
            usedInBuild.RefreshView();

            if (usedInBuild.refs == null) return false;

            var buildGuids = new HashSet<string>(usedInBuild.refs.Keys);

            foreach (string guid in guids)
            {
                if (buildGuids.Contains(guid)) return true;
            }

            return false;
        }



        private static List<AssetFinderInfo> ProcessAssetResults(List<AssetFinderAsset> assets, Sorting sorting)
        {
            if (assets == null) return new List<AssetFinderInfo>();

            var results = new List<AssetFinderInfo>();

            foreach (AssetFinderAsset asset in assets)
            {
                if (asset == null) continue;
                
                var assetInfo = new AssetFinderInfo(asset.guid);
                results.Add(assetInfo);
            }

            return ApplySorting(results, sorting);
        }

        private static List<AssetFinderInfo> ProcessResults(Dictionary<string, AssetFinderRef> refs, Dependency dependency, int depth, DepthFilter filter, Sorting sorting)
        {
            if (refs == null) return new List<AssetFinderInfo>();

            var results = new List<AssetFinderInfo>();
            var processedGuids = new HashSet<string>();

            foreach (KeyValuePair<string, AssetFinderRef> kvp in refs)
            {
                AssetFinderRef refItem = kvp.Value;
                
                // Apply dependency filter
                if (dependency == Dependency.Direct && refItem.depth > 1) continue;
                if (dependency == Dependency.Indirect && refItem.depth <= 1) continue;
                
                // Apply depth filter with mathematically correct comparisons
                if (!MatchesDepthFilter(refItem.depth, depth, filter)) continue;

                if (processedGuids.Contains(refItem.asset.guid)) continue;
                processedGuids.Add(refItem.asset.guid);

                var assetInfo = new AssetFinderInfo(refItem.asset.guid);
                results.Add(assetInfo);
            }

            return ApplySorting(results, sorting);
        }



        private static bool MatchesDepthFilter(int itemDepth, int targetDepth, DepthFilter filter)
        {
            switch (filter)
            {
                case DepthFilter.All:
                    return true;
                case DepthFilter.Equal:
                    return itemDepth == targetDepth;
                case DepthFilter.NotEqual:
                    return itemDepth != targetDepth;
                case DepthFilter.Less:
                    return itemDepth < targetDepth;
                case DepthFilter.LessEqual:
                    return itemDepth <= targetDepth;
                case DepthFilter.Greater:
                    return itemDepth > targetDepth;
                case DepthFilter.GreaterEqual:
                    return itemDepth >= targetDepth;
                default:
                    return true;
            }
        }



        private static List<AssetFinderInfo> ApplySorting(List<AssetFinderInfo> assetInfos, Sorting sorting)
        {
            switch (sorting)
            {
                case Sorting.Type:
                    return assetInfos.OrderBy(info => info.assetType?.Name ?? "")
                                    .ThenBy(info => info.assetPath)
                                    .ToList();
                
                case Sorting.Path:
                    return assetInfos.OrderBy(info => info.assetPath)
                                    .ThenBy(info => info.assetType?.Name ?? "")
                                    .ToList();
                
                case Sorting.Size:
                    return assetInfos.OrderByDescending(info => info.fileSize)
                                    .ThenBy(info => info.assetPath)
                                    .ToList();
                
                default:
                    return assetInfos;
            }
        }

        private static AssetFinderRefDrawer.Sort ConvertSorting(Sorting sorting)
        {
            switch (sorting)
            {
                case Sorting.Type: return AssetFinderRefDrawer.Sort.Type;
                case Sorting.Path: return AssetFinderRefDrawer.Sort.Path;
                case Sorting.Size: return AssetFinderRefDrawer.Sort.Size;
                default: return AssetFinderRefDrawer.Sort.Type;
            }
        }
    }
} 