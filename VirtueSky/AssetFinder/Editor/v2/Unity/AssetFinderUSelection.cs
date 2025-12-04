using System.Text;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    public static class AssetFinderUSelection
    {
        private static readonly StringBuilder sb = new StringBuilder();

        internal static void StartDebugReference()
        {
            Selection.selectionChanged -= DebugAssetReference;
            Selection.selectionChanged += DebugAssetReference;
        }

        internal static void StopDebugReference()
        {
            Selection.selectionChanged -= DebugAssetReference;
        }

        private static void DebugAssetReference()
        {
            if (!AssetFinderCacheAsset.isReady) return;
            
            Object activeObject = Selection.activeObject;
            if (activeObject == null) return;
            
            AssetDatabase.TryGetGUIDAndLocalFileIdentifier(activeObject, out string guid, out long fileId);
            var isMainAsset = AssetDatabase.IsMainAsset(activeObject);
            var usageList = AssetFinderCacheAsset.CollectUsage(guid);
            var usedByList = AssetFinderCacheAsset.CollectUsedBy(guid, isMainAsset ? -1 : fileId);
            
            sb.Clear();
            sb.AppendLine($"{guid}:{fileId} : {AssetDatabase.GUIDToAssetPath(guid)}");
            
            sb.AppendLine($"Used: {usageList.Count}\n");
            for (var i = 0; i < usageList.Count; i++)
            {
                AssetFinderIDRef usage = usageList[i];
                var (useGUID, useFileId) = AssetFinderCacheAsset.GetGuidAndFileId(usage.toId);
                sb.AppendLine($"{useGUID}:{useFileId} - {usage} \t\t {AssetDatabase.GUIDToAssetPath(useGUID)}");
            }

            sb.AppendLine($"UsedBy: {usedByList.Count}\n");
            for (var i = 0; i < usedByList.Count; i++)
            {
                AssetFinderIDRef useBy = usedByList[i];
                var (useByGUID, _) = AssetFinderCacheAsset.GetGuidAndFileId(useBy.fromId);
                sb.AppendLine($"{useByGUID} - {useBy} \t\t {AssetDatabase.GUIDToAssetPath(useByGUID)}");
            }
            
            Debug.Log(sb.ToString());
        }
    }
}
