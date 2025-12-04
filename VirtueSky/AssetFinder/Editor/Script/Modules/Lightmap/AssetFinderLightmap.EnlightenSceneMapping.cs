using System;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderLightmap
    {
        [Serializable]
        private sealed class EnlightenSceneMapping
        {
            [SerializeField] private EnlightenRendererInformation[] m_Renderers;

            [SerializeField] private EnlightenSystemInformation[] m_Systems;

            [SerializeField] private Hash128[] m_Probesets;

            [SerializeField] private EnlightenSystemAtlasInformation[] m_SystemAtlases;

            [SerializeField] private EnlightenTerrainChunksInformation[] m_TerrainChunks;

            public EnlightenRendererInformation[] renderers
            {
                get => m_Renderers;
                set => m_Renderers = value;
            }

            public EnlightenSystemInformation[] systems
            {
                get => m_Systems;
                set => m_Systems = value;
            }

            public Hash128[] probesets
            {
                get => m_Probesets;
                set => m_Probesets = value;
            }

            public EnlightenSystemAtlasInformation[] systemAtlases
            {
                get => m_SystemAtlases;
                set => m_SystemAtlases = value;
            }

            public EnlightenTerrainChunksInformation[] terrainChunks
            {
                get => m_TerrainChunks;
                set => m_TerrainChunks = value;
            }
        }
    }
}
