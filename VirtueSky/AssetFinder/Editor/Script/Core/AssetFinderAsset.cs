#if AssetFinderADDRESSABLE
using UnityEditor.AddressableAssets;
using UnityEngine.AddressableAssets;
#endif
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{

    [Serializable]
    internal partial class AssetFinderAsset
    {

        // Constants moved to AssetFinderAsset.Constants.cs

        // ----------------------------- DRAW  ---------------------------------------

        [SerializeField] public string guid;

        // Need to read FileInfo: soft-cache (always re-read when needed)
        [FormerlySerializedAs("type2")] [SerializeField] public AssetType type;
        [SerializeField] private string m_fileInfoHash;
        [SerializeField] private string m_assetbundle;
        [SerializeField] private string m_addressable;

        [SerializeField] private string m_atlas;
        [SerializeField] private long m_fileSize;

        [SerializeField] private int m_assetChangeTS; // Realtime when asset changed (trigger by import asset operation)
        [SerializeField] private int m_fileInfoReadTS; // Realtime when asset being read

        [SerializeField] private int m_fileWriteTS; // file's lastModification (file content + meta)
        [SerializeField] private int m_cachefileWriteTS; // file's lastModification at the time the content being read
        [SerializeField] private bool m_forceIncludeInBuild;

        [SerializeField] internal int refreshStamp; // use to check if asset has been deleted (refreshStamp not updated)
        [SerializeField] internal List<Classes> UseGUIDsList = new List<Classes>();

        private bool _isExcluded;
        private Dictionary<string, HashSet<long>> _UseGUIDs;
        private float excludeTS;


        // ----------------------------- DRAW  ---------------------------------------
        [NonSerialized] private GUIContent fileSizeText;
        internal HashSet<long> HashUsedByClassesIds = new HashSet<long>();
        // Path info moved to AssetFinderAsset.PathInfo.cs


        // Do not cache
        [NonSerialized] internal AssetState state;
        internal Dictionary<string, AssetFinderAsset> UsedByMap = new Dictionary<string, AssetFinderAsset>();

        public AssetFinderAsset(string guid)
        {
            this.guid = guid;
            type = BUILT_IN_ASSETS.Contains(guid) ? AssetType.BUILT_IN : AssetType.UNKNOWN;
        }

        public bool forcedIncludedInBuild => m_forceIncludeInBuild;

        // ----------------------- TYPE INFO ------------------------

        internal bool IsFolder => type == AssetType.FOLDER;
        internal bool IsScript => type == AssetType.SCRIPT;
        internal bool IsMissing => (state == AssetState.MISSING) && !isBuiltIn;

        internal bool IsReferencable => type == AssetType.REFERENCABLE ||
            type == AssetType.SCENE;

        internal bool IsBinaryAsset => type == AssetType.BINARY_ASSET ||
            type == AssetType.MODEL ||
            type == AssetType.TERRAIN ||
            type == AssetType.LIGHTING_DATA;
        // ------------------------------- GETTERS -----------------------------

        internal bool IsCriticalAsset()
        {
            // Packages assets are always non-critical
            if (string.IsNullOrEmpty(assetPath)) //  && assetPath.StartsWith("Packages/")
            {
                return false;
            }

            if (AssetDatabase.IsValidFolder(assetPath))
            {
                return false;
            }

            if (type == AssetType.UNKNOWN) GuessAssetType();
            
            // Fast checks on asset type
            switch (type)
            {
                case AssetType.REFERENCABLE:
                case AssetType.SCENE:
                case AssetType.BINARY_ASSET:
                case AssetType.MODEL:
                case AssetType.TERRAIN:
                case AssetType.LIGHTING_DATA:
                return true;

                case AssetType.FOLDER:
                case AssetType.SCRIPT:
                case AssetType.DLL:
                case AssetType.NON_READABLE:
                {
                    // if (assetPath.Contains(".png")) AssetFinderLOG.LogWarning($"Wrong assetType? {type}");
                    return false;
                }
            }

            if (string.IsNullOrEmpty(extension))
            {
                // if (assetPath.Contains(".png")) AssetFinderLOG.LogWarning($"Wrong extensions? {extension}");
                return false;
            }

            var result = !NON_REFERENCE_EXTENSIONS.Contains(extension);
            // if (assetPath.Contains(".png")) AssetFinderLOG.LogWarning($"Result = {result}");
            return result;
        }

        // GUID management methods moved to AssetFinderAsset.GuidManager.cs

        public override string ToString()
        {
            return $"AssetFinderAsset[{m_assetName}]";
        }

        // AddUseGUID methods moved to AssetFinderAsset.GuidManager.cs

        // ----------------------------- STATIC  ---------------------------------------

        internal static int SortByExtension(AssetFinderAsset a1, AssetFinderAsset a2)
        {
            if (a1 == null) return -1;
            if (a2 == null) return 1;

            int result = string.Compare(a1.m_extension, a2.m_extension, StringComparison.Ordinal);
            return result == 0 ? string.Compare(a1.m_assetName, a2.m_assetName, StringComparison.Ordinal) : result;
        }

        internal static List<AssetFinderAsset> FindUsage(AssetFinderAsset asset)
        {
            if (asset == null) return null;

            List<AssetFinderAsset> refs = AssetFinderCache.Api.FindAssets(asset.UseGUIDs.Keys.ToArray(), true);


            return refs;
        }

        internal static List<AssetFinderAsset> FindUsedBy(AssetFinderAsset asset)
        {
            return asset.UsedByMap.Values.ToList();
        }
        
        // ----------------------------- REPLACE GUIDS ---------------------------------------

        internal bool ReplaceReference(string fromGUID, string toGUID, TerrainData terrain = null)
        {
            if (IsMissing) return false;

            if (IsReferencable)
            {
                if (!File.Exists(m_assetPath))
                {
                    state = AssetState.MISSING;
                    return false;
                }

                try
                {
                    string text = File.ReadAllText(m_assetPath).Replace("\r", "\n");
                    File.WriteAllText(m_assetPath, text.Replace(fromGUID, toGUID));
                    return true;
                } catch (Exception e)
                {
                    state = AssetState.MISSING;
                    AssetFinderLOG.LogWarning("Replace Reference error :: " + e + "\n" + m_assetPath);
                }

                return false;
            }

            if (type == AssetType.TERRAIN)
            {
                var fromObj = AssetFinderUnity.LoadAssetWithGUID<UnityObject>(fromGUID);
                var toObj = AssetFinderUnity.LoadAssetWithGUID<UnityObject>(toGUID);
                var found = 0;

                if (fromObj is Texture2D tex)
                {
                    DetailPrototype[] arr = terrain.detailPrototypes;
                    for (var i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].prototypeTexture != tex) continue;
                        found++;
                        arr[i].prototypeTexture = (Texture2D)toObj;
                    }

                    terrain.detailPrototypes = arr;
                    AssetFinderTerrain.ReplaceTerrainTextureDatas(terrain, tex, (Texture2D)toObj);
                }

                if (fromObj is GameObject go)
                {
                    TreePrototype[] arr2 = terrain.treePrototypes;
                    for (var i = 0; i < arr2.Length; i++)
                    {
                        if (arr2[i].prefab != go) continue;
                        found++;
                        arr2[i].prefab = (GameObject)toObj;
                    }

                    terrain.treePrototypes = arr2;
                }

                return found > 0;
            }

            AssetFinderLOG.LogWarning("Something wrong, should never be here - Ignored <" + m_assetPath + "> : not a readable type, can not replace ! " + type);
            return false;
        }

        internal string ReplaceFileIdIfNeeded(string line, long toFileId)
        {
            const string FileID = "fileID: ";
            int index = line.IndexOf(FileID, StringComparison.Ordinal);
            if (index < 0 || toFileId <= 0) return line;
            int startIndex = index + FileID.Length;
            int endIndex = line.IndexOf(',', startIndex);
            if (endIndex > startIndex)
            {
                string fromFileId = line.Substring(startIndex, endIndex - startIndex);
                if (long.TryParse(fromFileId, out long fileType) &&
                    fileType.ToString().StartsWith(toFileId.ToString().Substring(0, 3)))
                {
                    AssetFinderLOG.Log($"ReplaceReference: fromFileId {fromFileId} to File Id {toFileId}");
                    return line.Replace(fromFileId, toFileId.ToString());
                }
                AssetFinderLOG.LogWarning($"[Skip] Difference file type: {fromFileId} -> {toFileId}");
            } else
            {
                AssetFinderLOG.LogWarning("Cannot parse fileID in the line.");
            }
            return line;
        }

        internal bool ReplaceReference(string fromGUID, string toGUID, long toFileId, TerrainData terrain = null)
        {
            if (IsMissing)
            {
                return false;
            }

            if (IsReferencable)
            {
                if (!File.Exists(m_assetPath))
                {
                    state = AssetState.MISSING;
                    return false;
                }

                try
                {
                    var sb = new StringBuilder();
                    string text = File.ReadAllText(assetPath);
                    var currentIndex = 0;

                    while (currentIndex < text.Length)
                    {
                        int lineEndIndex = text.IndexOfAny(new[] { '\r', '\n' }, currentIndex);
                        if (lineEndIndex == -1)
                        {
                            lineEndIndex = text.Length;
                        }

                        string line = text.Substring(currentIndex, lineEndIndex - currentIndex);

                        // Check if the line contains the GUID and possibly the fileID
                        if (line.Contains(fromGUID))
                        {
                            line = ReplaceFileIdIfNeeded(line, toFileId);
                            line = line.Replace(fromGUID, toGUID);
                        }

                        sb.Append(line);

                        // Skip through any EOL characters
                        while (lineEndIndex < text.Length)
                        {
                            char c = text[lineEndIndex];
                            if (c == '\r' || c == '\n')
                            {
                                sb.Append(c);
                                lineEndIndex++;
                            }
                            break;
                        }

                        currentIndex = lineEndIndex;
                    }

                    File.WriteAllText(assetPath, sb.ToString());
                    //AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.Default);
                    return true;
                } catch (Exception e)
                {
                    state = AssetState.MISSING;
                    AssetFinderLOG.LogWarning("Replace Reference error :: " + e + "\n" + m_assetPath);
                }

                return false;
            }

            if (type == AssetType.TERRAIN)
            {
                var fromObj = AssetFinderUnity.LoadAssetWithGUID<UnityObject>(fromGUID);
                var toObj = AssetFinderUnity.LoadAssetWithGUID<UnityObject>(toGUID);
                var found = 0;

                // var terrain = AssetDatabase.LoadAssetAtPath(assetPath, typeof(Object)) as TerrainData;

                if (fromObj is Texture2D)
                {
                    DetailPrototype[] arr = terrain.detailPrototypes;
                    for (var i = 0; i < arr.Length; i++)
                    {
                        if (arr[i].prototypeTexture == (Texture2D)fromObj)
                        {
                            found++;
                            arr[i].prototypeTexture = (Texture2D)toObj;
                        }
                    }

                    terrain.detailPrototypes = arr;
                    AssetFinderTerrain.ReplaceTerrainTextureDatas(terrain, (Texture2D)fromObj, (Texture2D)toObj);
                }

                if (fromObj is GameObject go)
                {
                    TreePrototype[] arr2 = terrain.treePrototypes;
                    for (var i = 0; i < arr2.Length; i++)
                    {
                        if (arr2[i].prefab != go) continue;
                        found++;
                        arr2[i].prefab = (GameObject)toObj;
                    }

                    terrain.treePrototypes = arr2;
                }

                // EditorUtility.SetDirty(terrain);
                // AssetDatabase.SaveAssets();
                // AssetFinderUnity.UnloadUnusedAssets();
                return found > 0;
            }

            AssetFinderLOG.LogWarning("Something wrong, should never be here - Ignored <" + m_assetPath +
                "> : not a readable type, can not replace ! " + type);
            return false;
        }

    }
}
