using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        public override void AddToCustomMenu(GenericMenu menu)
        {
#if AssetFinderDEV
            menu.AddItem(new GUIContent("Refresh Cache"), false, () => AssetFinderCache.Api.Check4Changes(true));
            menu.AddItem(new GUIContent("Validate References vs Unity"), false, ()=>ValidateReferencesVsUnity());
            menu.AddItem(new GUIContent("Validate References (Export to File)"), false, () => ValidateReferencesVsUnity(true));
            menu.AddItem(new GUIContent("Debug Selected Assets"), false, DebugSelectedAssets);
#endif
        }

#if AssetFinderDEV
        private void ValidateReferencesVsUnity(bool exportToFile = false)
        {
            if (!AssetFinderCache.isReady)
            {
                AssetFinderLOG.LogWarning("[AssetFinderVALIDATION] Cache not ready. Please wait for cache to finish loading.");
                return;
            }

            if (exportToFile)
            {
                AssetFinderLOG.Log("[AssetFinderVALIDATION] Starting validation with file export...");
            }
            else
            {
                AssetFinderLOG.Log("[AssetFinderVALIDATION] Starting comprehensive reference validation against Unity's GetDependencies...");
            }
            
            var validator = new AssetFinderReferenceValidator();
            validator.ValidateAllReferences(exportToFile);
        }

        private void DebugSelectedAssets()
        {
            if (Selection.objects == null || Selection.objects.Length == 0)
            {
                AssetFinderLOG.LogWarning("[AssetFinderDEBUG] No objects selected for debugging");
                return;
            }

            foreach (var obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path)) continue;

                string guid = AssetDatabase.AssetPathToGUID(path);
                Debug.Log($"[AssetFinderDEBUG] === {obj.name} ({guid}) ===");

                if (!AssetFinderCache.isReady)
                {
                    AssetFinderLOG.LogWarning("[AssetFinderDEBUG] Cache not ready!");
                    continue;
                }

                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset == null)
                {
                    AssetFinderLOG.LogWarning("[AssetFinderDEBUG] Asset not found in cache!");
                    continue;
                }

                Debug.Log($"Type: {asset.type} | Critical: {asset.IsCriticalAsset()} | Extension: {asset.extension}");
                AssetFinderLOG.Log($"Uses: {asset.UseGUIDs?.Count ?? 0} | UsedBy: {asset.UsedByMap?.Count ?? 0}");
                Debug.Log($"InAssetList: {AssetFinderCache.Api.AssetList?.Contains(asset) ?? false} | Dirty: {asset.isDirty}");
            }
        }
#endif
    }
}