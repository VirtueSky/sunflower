using System;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private struct EnlightenTerrainChunksInformation
        {
            public int firstSystemId;
            public int numChunksInX;
            public int numChunksInY;
        }
    }
}
