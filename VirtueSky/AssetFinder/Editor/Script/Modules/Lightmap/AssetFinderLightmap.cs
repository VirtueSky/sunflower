// CREDITS:
// https://github.com/NewBloodInteractive/com.newblood.lighting-internals/tree/master
//

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        public static IEnumerable<Texture> Read(LightingDataAsset source)
        {
            string json = EditorJsonUtility.ToJson(source);
            var result = new LightingDataAssetRoot();
            EditorJsonUtility.FromJsonOverwrite(json, result);

            foreach (LightmapData item in result.LightingDataAsset.m_Lightmaps)
            {
                if (item.lightmap != null) yield return item.lightmap;
                if (item.dirLightmap != null) yield return item.dirLightmap;
                if (item.shadowMask != null) yield return item.shadowMask;
            }

            foreach (Texture2D item in result.LightingDataAsset.m_AOTextures)
            {
                if (item != null) yield return item;
            }

            foreach (Texture item in result.LightingDataAsset.m_BakedReflectionProbeCubemaps)
            {
                if (item != null) yield return item;
            }
        }
    }
}
