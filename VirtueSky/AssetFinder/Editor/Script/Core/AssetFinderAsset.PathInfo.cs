using System;
using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderAsset
    {
        // ----------------------- PATH INFO ------------------------

        [NonSerialized] private string m_assetFolder;
        [NonSerialized] private string m_assetName;
        [NonSerialized] private string m_assetPath;
        [NonSerialized] private string m_extension;
        [NonSerialized] private bool m_inEditor;
        [NonSerialized] private bool m_inPackage;
        [NonSerialized] private bool m_inPlugins;
        [NonSerialized] private bool m_inResources;
        [NonSerialized] private bool m_inStreamingAsset;
        [NonSerialized] private bool m_pathLoaded;

        public string assetName => LoadPathInfo().m_assetName;
        public string assetPath
        {
            get
            {
                if (!string.IsNullOrEmpty(m_assetPath)) return m_assetPath;
                m_assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (string.IsNullOrEmpty(m_assetPath)) state = AssetState.MISSING;
                return m_assetPath;
            }
        }

        public string parentFolderPath => LoadPathInfo().m_assetFolder;
        public string assetFolder => LoadPathInfo().m_assetFolder;
        public string extension => LoadPathInfo().m_extension;
        public bool inEditor => LoadPathInfo().m_inEditor;
        public bool inPlugins => LoadPathInfo().m_inPlugins;
        public bool inPackages => LoadPathInfo().m_inPackage;
        public bool inResources => LoadPathInfo().m_inResources;
        public bool inStreamingAsset => LoadPathInfo().m_inStreamingAsset;

        internal bool IsExcluded
        {
            get
            {
                if (excludeTS >= ignoreTS) return _isExcluded;

                excludeTS = ignoreTS;
                _isExcluded = false;

                var h = AssetFinderSetting.IgnoreAsset;
                foreach (string item in h)
                {
                    if (!m_assetPath.StartsWith(item, false, CultureInfo.InvariantCulture)) continue;
                    _isExcluded = true;
                    return true;
                }

                return false;
            }
        }

        public AssetFinderAsset LoadPathInfo()
        {
            if (m_pathLoaded) return this;
            m_pathLoaded = true;

            m_assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath))
            {
                state = AssetState.MISSING;
                return this;
            }

// #if AssetFinderDEBUG
// 			AssetFinderLOG.Log("LoadPathInfo ... " + fileInfoHash + ":" + AssetDatabase.GUIDToAssetPath(guid));
// #endif
            AssetFinderUnity.SplitPath(m_assetPath, out m_assetName, out m_extension, out m_assetFolder);

            if (m_assetFolder.StartsWith("Assets/"))
            {
                m_assetFolder = m_assetFolder.Substring(7);
            } else if (!AssetFinderUnity.StringStartsWith(m_assetPath,"Project Settings/", "Library/")) m_assetFolder = "built-in/";

            m_inEditor = m_assetPath.Contains("/Editor/") || m_assetPath.Contains("/Editor Default Resources/");
            m_inResources = m_assetPath.Contains("/Resources/");
            m_inStreamingAsset = m_assetPath.Contains("/StreamingAssets/");
            m_inPlugins = m_assetPath.Contains("/Plugins/");
            m_inPackage = m_assetPath.StartsWith("Packages/");
            return this;
        }
    }
} 