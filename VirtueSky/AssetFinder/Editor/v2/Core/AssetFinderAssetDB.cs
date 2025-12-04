using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    [Serializable] internal class AssetFinderAssetDB
    {
        [SerializeField] internal List<AssetFinderAssetFile> files = new List<AssetFinderAssetFile>();
        [SerializeField] internal List<AssetFinderIDRef> refs = new List<AssetFinderIDRef>();
        
        [NonSerialized] internal readonly Dictionary<string, AssetFinderAssetFile> guidMap = new Dictionary<string, AssetFinderAssetFile>();
        [NonSerialized] internal bool isReady;
        [NonSerialized] internal AssetFinderTimeSlice readContentTS;

        internal AssetFinderAssetFile GetAssetByGUID(string guid)
        {
            return guidMap.GetValueOrDefault(guid);
        }
        private AssetFinderAssetFile AddAsset(string guid)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath)) return null;
            var assetFile = new AssetFinderAssetFile(guid, files.Count);
            guidMap.Add(guid, assetFile);
            files.Add(assetFile);
            return assetFile;
        }

        internal AssetFinderAssetFile GetAsset(AssetFinderID id)
        {
            return files[id.AssetIndex];
        }
        
        internal AssetFinderAssetDB Clear()
        {
            files.Clear();
            refs.Clear();
            guidMap.Clear();
            return this;
        }

        internal void Scan(bool force = false)
        {
            if (force) Clear();
            string[] allAssetPaths = AssetDatabase.GetAllAssetPaths();
            foreach (string path in allAssetPaths)
            {
                if (path.Contains("FindReference2") || path.Contains("AssetFinderCache")) continue;
                
                string guid = AssetDatabase.AssetPathToGUID(path);
                // if (path.StartsWith("Packages/", StringComparison.InvariantCulture))
                // {
                //     AssetFinderLOG.Log($"Skip assets in Packages: {guid} --> {path}");
                //     continue;
                // }
                
                if (AssetDatabase.IsValidFolder(path))
                {
                    AssetFinderLOG.Log($"Skip Folder: {guid} --> {path}");
                    continue;
                }
                if (AssetFinderParser.IsReadable(path)) AddAsset(guid);
            }
            
            ReadContent();
        }
        
        internal void ReadContent()
        {
            int count = files.Count;
            if (readContentTS == null) readContentTS = new AssetFinderTimeSlice(() => count, TS_ReadFileContent, FinishReadContent);
            readContentTS.Start();
        }

        void FinishReadContent()
        {
            // Save refs
            refs.Clear();
            guidMap.Clear();
            
            for (var i =0;i < files.Count; i++)
            {
                AssetFinderAssetFile assetFile = files[i];
                guidMap.Add(assetFile.guid, assetFile);
                refs.AddRange(assetFile.usage);
            }
            isReady = true;
        }

        internal void BuildCache()
        {
            guidMap.Clear();
            for (var i = 0; i < files.Count; i++)
            {
                AssetFinderAssetFile assetFile = files[i];
                if (assetFile == null) continue;
                
                assetFile.fileIdMap.Clear();
                foreach (long fileId in assetFile.fileIds.Distinct())
                {
                    assetFile.fileIdMap.Add(fileId, assetFile.fileIds.IndexOf(fileId));
                }
                
                guidMap.Add(assetFile.guid, assetFile);
                assetFile.usage.Clear();
                assetFile.usedBy.Clear();
            }

            for (var i = 0; i < refs.Count; i++)
            {
                AssetFinderIDRef r = refs[i];
                AssetFinderAssetFile from = GetAsset(r.fromId.WithoutSubAssetIndex());
                AssetFinderAssetFile to = GetAsset(r.toId.WithoutSubAssetIndex());
                
                if (from == null || to == null)
                {
                    Debug.LogWarning($"Invalid reference (asset not found???): {r.fromId} --> {r.toId}");
                    continue;
                }
                
                from.usage.Add(r);
                to.usedBy.Add(r);
            }
            
            isReady = true;
        }
        
        internal void TS_ReadFileContent(int index)
        {
            AssetFinderAssetFile sourceFile = files[index];
            string assetPath = AssetDatabase.GUIDToAssetPath(sourceFile.guid);
            if (string.IsNullOrEmpty(assetPath)) return;

            var usage = new HashSet<AssetFinderID>();
            AssetFinderParser.ReadContent(assetPath, (guid, fileId) =>
            {
                if (guid == sourceFile.guid) return; // Skip self reference
                AssetFinderAssetFile destFile = GetAssetByGUID(guid) ?? AddAsset(guid);
                if (destFile == null) return; // Invalid or missing GUID???

                int subAssetIndex = destFile.Get(fileId);
                if (subAssetIndex == -1) subAssetIndex = destFile.Add(fileId);
                
                AssetFinderID toFR2Id = destFile.fr2Id.WithSubAssetIndex(subAssetIndex);
                if (!usage.Add(toFR2Id)) return; // Already added

                var r = new AssetFinderIDRef() { fromId = sourceFile.fr2Id, toId = toFR2Id };
                sourceFile.usage.Add(r);
                destFile.usedBy.Add(r);
            });
        }
    }
}
