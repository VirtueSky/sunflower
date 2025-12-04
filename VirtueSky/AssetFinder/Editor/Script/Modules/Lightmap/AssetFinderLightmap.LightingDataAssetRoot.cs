using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private sealed class LightingDataAssetRoot
        {
            public SerializedData LightingDataAsset;

            [Serializable]
            public struct SerializedData
            {
                public int serializedVersion;
                public SceneAsset m_Scene;
                public LightmapData[] m_Lightmaps;
                public Texture2D[] m_AOTextures;
                public string[] m_LightmapsCacheFiles;
                public LightProbes m_LightProbes;
                public int m_LightmapsMode;
                public SphericalHarmonicsL2 m_BakedAmbientProbeInLinear;
                public RendererData[] m_LightmappedRendererData;
                public SceneObjectIdentifier[] m_LightmappedRendererDataIDs;
                public EnlightenSceneMapping m_EnlightenSceneMapping;
                public SceneObjectIdentifier[] m_EnlightenSceneMappingRendererIDs;
                public SceneObjectIdentifier[] m_Lights;
                public LightBakingOutput[] m_LightBakingOutputs;
                public string[] m_BakedReflectionProbeCubemapCacheFiles;
                public Texture[] m_BakedReflectionProbeCubemaps;
                public SceneObjectIdentifier[] m_BakedReflectionProbes;
                public byte[] m_EnlightenData;
                public int m_EnlightenDataVersion;
            }
        }
    }
}
