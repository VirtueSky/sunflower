using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    public static class AssetFinderInitializer
    {
        [InitializeOnLoadMethod]
        static void Init()
        {
            EditorApplication.update -= DelayInit;
            EditorApplication.update += DelayInit;
            
            AssemblyReloadEvents.afterAssemblyReload  -= Reload;
            AssemblyReloadEvents.afterAssemblyReload  += Reload;
            
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged; 
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    break;
                case PlayModeStateChange.EnteredEditMode:  
                {
                    Reload();
                    if (AssetFinderCache.Api != null && !AssetFinderSettingExt.disable)
                    {
                        AssetFinderCache.Api.IncrementalRefresh();
                    }
                    break;
                }
            }
        }
        
        static void Reload()
        {
            AssetFinderAddressable.Scan();
            AssetFinderCache.Reload();
            
            // Re-init all windows
            var allWindows = Resources.FindObjectsOfTypeAll<AssetFinderWindowAll>();
            for (var i = 0; i < allWindows.Length; i++)
            {
                allWindows[i].Reload(); 
            }
        }
        
        
        static void DelayInit()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                AssetFinderLOG.Log("Keep waiting...");
                return;
            }
            
            EditorApplication.update -= DelayInit;
            
            // Simple type search scoped to Assets/ only
            string[] cache = AssetDatabase.FindAssets("t:AssetFinderCacheAsset", new[] { "Assets" });
            if (cache.Length == 0) 
            {
                return; // No cache found
            }
            
            // Try to load the first valid cache asset
            for (int i = 0; i < cache.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(cache[i]);
                if (string.IsNullOrEmpty(assetPath)) continue;
                
                var cache0 = AssetDatabase.LoadAssetAtPath<AssetFinderCacheAsset>(assetPath);
                if (cache0 != null)
                {
                    AssetFinderCacheAsset.Init(cache0);
                    return;
                }
            }
            
            AssetFinderLOG.LogWarning("FR2: Cache assets found but all failed to load!");
        }
    }
}
