using UnityEditor;
using AddUsageCB = System.Action<string, long>;

namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderParser // LightMap
    {
        // BWCompatible
        internal static void LoadLightingData(this AssetFinderAsset asset, LightingDataAsset data)
        {
            Read_LightMap(data, (guid, fileId) => asset.AddUseGUID(guid, fileId));
        }
        
        private static void Read_LightMap(LightingDataAsset asset, AddUsageCB callback)
        {
            if (asset == null) return;
            foreach (var texture in AssetFinderLightmap.Read(asset))
            {
                AddObjectUsage(texture, callback);
            }
        }
    }
}
