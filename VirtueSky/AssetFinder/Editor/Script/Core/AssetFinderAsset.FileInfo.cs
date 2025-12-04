using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderAsset
    {
        // ----------------------- FILE INFO ------------------------

        public bool fileInfoDirty => type == AssetType.UNKNOWN || m_fileInfoReadTS <= m_assetChangeTS;
        public bool fileContentDirty => (m_fileWriteTS != m_cachefileWriteTS) && !isBuiltIn;
        public bool isDirty => (fileInfoDirty || fileContentDirty) && !isBuiltIn;
        public bool isBuiltIn => type == AssetType.BUILT_IN;
        public bool hasBeenScanned => m_cachefileWriteTS > 0 || isBuiltIn;

        internal string fileInfoHash => LoadFileInfo().m_fileInfoHash;
        internal long fileSize => LoadFileInfo().m_fileSize;
        public string AtlasName => LoadFileInfo().m_atlas;
        public string AssetBundleName => LoadFileInfo().m_assetbundle;
        public string AddressableName => LoadFileInfo().m_addressable;

        private bool ExistOnDisk()
        {
            if (isBuiltIn) return true;
            if (IsMissing) return false; // asset not exist - no need to check FileSystem!
            if (type == AssetType.FOLDER || type == AssetType.UNKNOWN)
            {
                if (Directory.Exists(m_assetPath))
                {
                    if (type == AssetType.UNKNOWN) type = AssetType.FOLDER;
                    return true;
                }

                if (type == AssetType.FOLDER) return false;
            }

            // must be file here
            if (!File.Exists(m_assetPath)) return false;

            if (type == AssetType.UNKNOWN) GuessAssetType();
            return true;
        }

        internal AssetFinderAsset LoadFileInfo()
        {
            if (!fileInfoDirty) return this;
            if (string.IsNullOrEmpty(m_assetPath)) LoadPathInfo(); // always reload Path Info

            m_fileInfoReadTS = AssetFinderUnity.Epoch(DateTime.Now);
            if (isBuiltIn) return this;
            if (IsMissing)
            {
                return this;
            }

            if (!ExistOnDisk())
            {
                state = AssetState.MISSING;
                return this;
            }

            if (type == AssetType.FOLDER) return this; // nothing to read

            Type assetType = AssetDatabase.GetMainAssetTypeAtPath(m_assetPath);
            if (assetType == typeof(AssetFinderCache)) return this;

            var info = new FileInfo(m_assetPath);
            m_fileSize = info.Length;
            m_fileInfoHash = info.Length + info.Extension;
            m_addressable = AssetFinderUnity.GetAddressable(guid);

            m_assetbundle = AssetDatabase.GetImplicitAssetBundleName(m_assetPath);

            if (assetType == typeof(Texture2D))
            {
                AssetImporter importer = AssetImporter.GetAtPath(m_assetPath);
                if (importer is TextureImporter tImporter)
                {
                    #pragma warning disable CS0618
                    if (tImporter.qualifiesForSpritePacking) m_atlas = tImporter.spritePackingTag;
                    #pragma warning restore CS0618
                }
            }

            // check if file content changed
            var metaInfo = new FileInfo(m_assetPath + ".meta");
            int assetTime = AssetFinderUnity.Epoch(info.LastWriteTime);
            int metaTime = AssetFinderUnity.Epoch(metaInfo.LastWriteTime);

            // update fileChangeTimeStamp
            m_fileWriteTS = Mathf.Max(metaTime, assetTime);
            return this;
        }

        internal void GuessAssetType()
        {
            var ext = extension.ToLowerInvariant();
            if (SCRIPT_EXTENSIONS.Contains(ext))
            {
                type = AssetType.SCRIPT;
            } else if (REFERENCABLE_EXTENSIONS.Contains(ext))
            {
                bool isUnity = ext == ".unity";
                type = isUnity ? AssetType.SCENE : AssetType.REFERENCABLE;

                if (ext == ".asset" || isUnity || ext == ".spriteatlas")
                {
                    var buffer = new byte[5];
                    FileStream stream = null;

                    try
                    {
                        stream = File.OpenRead(m_assetPath);
                        stream.Read(buffer, 0, 5);
                        stream.Close();
                    }
#if AssetFinderDEBUG
                    catch (Exception e)
                    {
                        AssetFinderLOG.LogWarning("Guess Asset Type error :: " + e + "\n" + m_assetPath);
#else
                    catch
                    {
#endif
                        if (stream != null) stream.Close();
                        state = AssetState.MISSING;
                        return;
                    } finally
                    {
                        if (stream != null) stream.Close();
                    }

                    var str = string.Empty;
                    foreach (byte t in buffer)
                    {
                        str += (char)t;
                    }

                    if (str != "%YAML") type = AssetType.BINARY_ASSET;
                }
            } else if (REFERENCABLE_JSON.Contains(ext) || UI_TOOLKIT.Contains(ext))
            {
                type = AssetType.REFERENCABLE;
            } else if (REFERENCABLE_META.Contains(ext))
            {
                type = AssetType.REFERENCABLE;
            } else if (ext == ".fbx")
            {
                type = AssetType.MODEL;
            } else if (ext == ".dll")
            {
                type = AssetType.DLL;
            } else
            {
                type = AssetType.NON_READABLE;
            }
        }

        internal void MarkAsDirty(bool isMoved = true, bool force = false)
        {
            if (isMoved)
            {
                string newPath = AssetDatabase.GUIDToAssetPath(guid);
                if (newPath != m_assetPath)
                {
                    m_pathLoaded = false;
                    m_assetPath = newPath;
                }
            }

            state = AssetState.CACHE;
            m_assetChangeTS = AssetFinderUnity.Epoch(DateTime.Now); // re-read FileInfo
            if (force) m_cachefileWriteTS = 0;
        }
    }
} 