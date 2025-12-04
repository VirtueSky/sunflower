using System;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private struct LightBakingOutput
        {
            public int serializedVersion;
            public int probeOcclusionLightIndex;
            public int occlusionMaskChannel;
            public LightmapBakeMode lightmapBakeMode;
            public bool isBaked;

            [Serializable]
            public struct LightmapBakeMode
            {
                public LightmapBakeType lightmapBakeType;
                public MixedLightingMode mixedLightingMode;
            }
        }
    }
}
