using System;
using UnityEngine;
using Object = UnityEngine.Object;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private struct EnlightenRendererInformation
        {
            public Object renderer;
            public Vector4 dynamicLightmapSTInSystem;
            public int systemId;
            public Hash128 instanceHash;
            public Hash128 geometryHash;
        }
    }
}
