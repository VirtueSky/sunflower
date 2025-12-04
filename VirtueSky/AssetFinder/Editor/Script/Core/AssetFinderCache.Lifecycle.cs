using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace VirtueSky.AssetFinder.Editor
{


    internal partial class AssetFinderCache
    {
        public static bool CheckSameVersion()
        {
            if (_cache == null) return false;
            return _cache._curCacheVersion == CACHE_VERSION;
        }

        public void MarkChanged()
        {
            HasChanged = true;
        }

        private static void FoundCache()
        {
            _cachePath = AssetDatabase.GetAssetPath(_cache);
            _cache.ReadFromCache();

            _cacheGUID = AssetDatabase.AssetPathToGUID(_cachePath);

            if (AssetFinderSettingExt.isAutoRefreshEnabled || _cacheJustCreated)
            {
                if (_cacheJustCreated) _cache.Check4Changes(true);
                else _cache.RefreshUsedByOnlyFromCache();
            }
            else
            {
                _cache.RefreshUsedByOnlyFromCache();
            }
            
            // Reset flag after use
            _cacheJustCreated = false;
        }

        private static bool RestoreCacheFromPath(string path, bool validateOnly, bool forceLoad)
        {
            if (string.IsNullOrEmpty(path)) return false;
            if (!File.Exists(path)) return false;

            _cache = AssetFinderUnity.LoadAssetAtPath<AssetFinderCache>(path);
            if (_cache == null) return false;
            if (validateOnly && !forceLoad) return true;

            FoundCache();
            return true;
        }

        private static void TryLoadCache()
        {
            _triedToLoadCache = true;

            // Simple type-based search scoped to Assets/ only
            var cacheAssets = AssetDatabase.FindAssets("t:AssetFinderCache", new[] { "Assets" });
            if (cacheAssets.Length > 0)
                foreach (var guid in cacheAssets)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (!string.IsNullOrEmpty(path) && RestoreCacheFromPath(path, true, true))
                        return;
                }
        }

        internal static void DeleteCache()
        {
            if (_cache == null) return;

            try
            {
                _cache.AssetList.Clear();
                _cache.AssetMap.Clear();
                _cache.queueLoadContent.Clear();
                _cache = null;
                if (!string.IsNullOrEmpty(_cachePath)) AssetDatabase.DeleteAsset(_cachePath);
            }
            catch
            {
                // ignored
            }

            AssetDatabase.SaveAssets();
            using (AssetFinderDev.NoLog)
            {
                AssetDatabase.Refresh();
            }
        }

        internal static void CreateCache()
        {
            _cache = CreateInstance<AssetFinderCache>();
            _cache._curCacheVersion = CACHE_VERSION;
            var path = Application.dataPath + DEFAULT_CACHE_PATH
                .Substring(0, DEFAULT_CACHE_PATH.LastIndexOf('/') + 1).Replace("Assets", string.Empty);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);

            AssetDatabase.CreateAsset(_cache, DEFAULT_CACHE_PATH);
            EditorUtility.SetDirty(_cache);

            // Set force refresh flag for initial/recreated cache
            _cacheJustCreated = true;
            
            FoundCache();

            // Delay the scan by one frame so UI can update first
            EditorApplication.delayCall -= DelayCheck4Changes;
            EditorApplication.delayCall += DelayCheck4Changes;
        }

        private void OnEnable()
        {
            if (_cache == null) _cache = this;
        }
    }
}