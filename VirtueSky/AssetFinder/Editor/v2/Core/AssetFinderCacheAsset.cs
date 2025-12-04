using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
namespace VirtueSky.AssetFinder.Editor
{
    [CreateAssetMenu]
    internal class AssetFinderCacheAsset : ScriptableObject
    {
        // static APIs
        private static AssetFinderCacheAsset _api;
        public static bool isReady => _api != null && _api.db.isReady;

        internal static void MarkAsDirty()
        {
            if (_api == null) return;
            EditorUtility.SetDirty(_api);
        }
        
        public static AssetFinderAssetFile GetFile(string guid) => _api.db.GetAssetByGUID(guid);
        
        public static List<AssetFinderIDRef> CollectUsage(string guid, List<AssetFinderIDRef> result = null)
        {
            if (result == null) result = new List<AssetFinderIDRef>();
            if (!isReady)
            {
                AssetFinderLOG.Log($"CacheAsset is not ready!");
                return result;
            }

            AssetFinderAssetFile assetFile = GetFile(guid);
            if (assetFile == null)
            {
                Debug.Log($"Asset not found in cache: {guid} : {AssetDatabase.GUIDToAssetPath(guid)}");
                return result;
            }
            
            result.AddRange(assetFile.usage);
            return result;
        }
        public static List<AssetFinderIDRef> CollectUsedBy(string guid, long fileId = -1, List<AssetFinderIDRef> result = null) // -1 = all
        {
            if (result == null) result = new List<AssetFinderIDRef>();
            if (!isReady)
            {
                AssetFinderLOG.Log($"CacheAsset is not ready!");
                return result;
            }
            
            AssetFinderAssetFile assetFile = _api.db.GetAssetByGUID(guid);
            if (assetFile == null)
            {
                Debug.Log($"Asset not found in cache: {guid} : {AssetDatabase.GUIDToAssetPath(guid)}");
                return result;
            }
            
            int subAssetIndex = fileId < 0 ? -1 : assetFile.Get(fileId);
            // Debug.Log($"CollectUsedBy: {guid}:{fileId} ({subAssetIndex} --> {AssetDatabase.GUIDToAssetPath(guid)} | Count = {assetFile.usedBy.Count}");
            if (fileId <= 0 || subAssetIndex <= 0)
            {
                result.AddRange(assetFile.usedBy);
            } else
            {
                result.AddRange(assetFile.usedBy.Where(a=> a.toId.SubAssetIndex == subAssetIndex));
            }
            
            return result;
        }
        
        public static (string guid, long fileId) GetGuidAndFileId(AssetFinderID fr2ID)
        {
            if (!isReady) return (null, -1);
            
            AssetFinderAssetFile assetFile = _api.db.GetAsset(fr2ID);
            if (assetFile == null)
            {
                AssetFinderLOG.Log($"Asset not found in cache: {fr2ID}");
                return (null, -1);
            }

            return (assetFile.guid, assetFile.fileIds[fr2ID.SubAssetIndex]);
        }
        
        
        
        
        // Serializable
        [FormerlySerializedAs("sampleAsset")] [SerializeField] private AssetFinderIDRef sampleID = new AssetFinderIDRef();
        [SerializeField] internal AssetFinderAssetDB db = new AssetFinderAssetDB();
        [SerializeField] internal List<string> dirtyAssets = new List<string>();
        
        internal static void Init(AssetFinderCacheAsset cache)
        {
            _api = cache;
            _api.db.isReady = false;

            if (_api.dirtyAssets.Count > 0)
            {
                // Do check for changes
            }
            
            _api.db.BuildCache();
        }
        
        // Scan
        [ContextMenu("Scan")]
        internal void Scan()
        {
            db.isReady = false;
            db.Scan(true);
            EditorUtility.SetDirty(this);
        }
        
        [ContextMenu("BuildCache")]
        internal void BuildCache()
        {
            db.isReady = false;
            db.BuildCache();
            EditorUtility.SetDirty(this);
        }

        [ContextMenu("Start Debug")]
        internal void StartDebug()
        {
            AssetFinderUSelection.StartDebugReference();
        }
    }
}
