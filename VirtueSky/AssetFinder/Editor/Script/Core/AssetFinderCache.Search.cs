using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderCache
    {
        internal static List<string> FindUsage(string[] listGUIDs)
        {
            if (!isReady) return null;

            List<AssetFinderAsset> refs = Api.FindAssets(listGUIDs, true);

            for (var i = 0; i < refs.Count; i++)
            {
                List<AssetFinderAsset> tmp = AssetFinderAsset.FindUsage(refs[i]);

                for (var j = 0; j < tmp.Count; j++)
                {
                    AssetFinderAsset itm = tmp[j];
                    if (refs.Contains(itm)) continue;

                    refs.Add(itm);
                }
            }

            return refs.Select(item => item.guid).ToList();
        }

        internal AssetFinderAsset Get(string guid, bool autoNew = false)
        {
            if (autoNew && !AssetMap.ContainsKey(guid)) AddAsset(guid);
            return AssetMap.GetValueOrDefault(guid);
        }

        internal List<AssetFinderAsset> FindAssetsOfType(AssetFinderAsset.AssetType type)
        {
            var result = new List<AssetFinderAsset>();
            foreach (KeyValuePair<string, AssetFinderAsset> item in AssetMap)
            {
                if (item.Value.type != type) continue;

                result.Add(item.Value);
            }

            return result;
        }

        internal AssetFinderAsset FindAsset(string guid, string fileId)
        {
            if (AssetMap == null) Check4Changes(false);
            if (!isReady)
            {
			    AssetFinderLOG.LogWarning("Cache not ready !");
                return null;
            }

            if (string.IsNullOrEmpty(guid)) return null;

            //for (var i = 0; i < guids.Length; i++)
            {
                //string guid = guids[i];
                if (!AssetMap.TryGetValue(guid, out AssetFinderAsset asset)) return null;

                if (asset.IsMissing) return null;

                if (asset.IsFolder) return null;
                return asset;
            }
        }

        internal List<AssetFinderAsset> FindAssets(string[] guids, bool scanFolder)
        {
            if (AssetMap == null) Check4Changes(false);

            var result = new List<AssetFinderAsset>();

            if (!isReady)
            {
			    AssetFinderLOG.LogWarning("Cache not ready !");
                return result;
            }

            var folderList = new List<AssetFinderAsset>();

            if (guids.Length == 0) return result;

            for (var i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                AssetFinderAsset asset;
                if (!AssetMap.TryGetValue(guid, out asset)) continue;

                if (asset.IsMissing) continue;

                if (asset.IsFolder)
                {
                    if (!folderList.Contains(asset)) folderList.Add(asset);
                } else
                {
                    result.Add(asset);
                }
            }

            if (!scanFolder || folderList.Count == 0) return result;

            int count = folderList.Count;
            for (var i = 0; i < count; i++)
            {
                AssetFinderAsset item = folderList[i];

                // for (var j = 0; j < item.UseGUIDs.Count; j++)
                // {
                //     AssetFinderAsset a;
                //     if (!AssetMap.TryGetValue(item.UseGUIDs[j], out a)) continue;
                foreach (KeyValuePair<string, HashSet<long>> useM in item.UseGUIDs)
                {
                    AssetFinderAsset a;
                    if (!AssetMap.TryGetValue(useM.Key, out a)) continue;

                    if (a.IsMissing) continue;

                    if (a.IsFolder)
                    {
                        if (!folderList.Contains(a))
                        {
                            folderList.Add(a);
                            count++;
                        }
                    } else
                    {
                        result.Add(a);
                    }
                }
            }

            return result;
        }
    }
} 