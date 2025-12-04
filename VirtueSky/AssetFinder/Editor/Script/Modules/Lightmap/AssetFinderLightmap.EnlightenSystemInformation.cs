using System;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private struct EnlightenSystemInformation
        {
            public int rendererIndex;
            public int rendererSize;
            public int atlasIndex;
            public int atlasOffsetX;
            public int atlasOffsetY;
            public Hash128 inputSystemHash;
            public Hash128 radiositySystemHash;
        }
    }
}
