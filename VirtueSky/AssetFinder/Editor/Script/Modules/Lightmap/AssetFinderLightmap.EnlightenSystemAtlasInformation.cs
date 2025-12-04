using System;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private struct EnlightenSystemAtlasInformation
        {
            public int atlasSize;
            public Hash128 atlasHash;
            public int firstSystemId;
        }
    }
}
