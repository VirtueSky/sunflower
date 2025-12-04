using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderAsset
    {
        // ----------------------------- CONTENT LOADING ---------------------------------------

        internal void LoadContent()
        {
            if (!fileContentDirty) return;
            m_cachefileWriteTS = m_fileWriteTS;
            m_forceIncludeInBuild = false;

            if (IsMissing || type == AssetType.NON_READABLE) return;
            if (type == AssetType.DLL)
            {
                AssetFinderLOG.LogWarning("Parsing DLL not yet supportted ");
                return;
            }
            
            AssetFinderLOG.Log("LoadContent ... infoHash=" + fileInfoHash + ": assetPath=" + m_assetPath);

            var startTime = DateTime.Now;
            if (shouldWriteImportLog && (fileSize >= MIN_FILE_SIZE_2LOG))
            {
                var logMessage = $"{startTime:yyyy-MM-dd HH:mm:ss} - {assetPath}, Size: {fileSize} bytes";
                File.AppendAllText(logPath, logMessage + Environment.NewLine);
            }
            
            if (!ExistOnDisk())
            {
                state = AssetState.MISSING;
                return;
            }

            ClearUseGUIDs();

            if (IsFolder)
            {
                LoadFolder();
            } else if (IsReferencable)
            {
                LoadYAML2();
            } else if (IsBinaryAsset)
            {
                LoadBinaryAsset();
            }

            if (shouldWriteImportLog && (fileSize >= MIN_FILE_SIZE_2LOG))
            {
                DateTime endTime = DateTime.Now;
                double duration = (endTime - startTime).TotalMilliseconds;
                var logMessage = $", Duration: {duration} ms";
                File.AppendAllText(logPath, logMessage + Environment.NewLine);
            }
        }

        internal void LoadContentFast()
        {
            if (!fileContentDirty) return;
            m_cachefileWriteTS = m_fileWriteTS;
            m_forceIncludeInBuild = false;

            if (IsMissing) return;
            if (type == AssetType.SCRIPT || type == AssetType.DLL || type == AssetType.FOLDER) return;
            // if (assetPath.StartsWith("Packages/")) return;

            DateTime startTime = DateTime.Now;
            if (shouldWriteImportLog && (fileSize >= MIN_FILE_SIZE_2LOG))
            {
                var logMessage = $"{startTime:yyyy-MM-dd HH:mm:ss} - {assetPath}, Size: {fileSize} bytes";
                File.AppendAllText(logPath, logMessage);
            }

            ClearUseGUIDs();

            if (fileSize > 5 * 1024 * 1024 || type == AssetType.NON_READABLE || IsBinaryAsset)
            {
                string[] dependencies = AssetDatabase.GetDependencies(assetPath, false);
                foreach (string dependency in dependencies)
                {
                    string guid = AssetDatabase.AssetPathToGUID(dependency);
                    if (!string.IsNullOrEmpty(guid) && (guid != this.guid))
                    {
                        AddUseGUID(guid);
                    }
                }
            } else if (IsReferencable)
            {
                LoadYAML2();
            }
            
            // CRITICAL FIX: Validate with AssetDatabase to catch missing references
            using (AssetFinderDev.NoLog)
            {
                ValidateWithAssetDatabase();
            }
            

            if (shouldWriteImportLog && (fileSize >= MIN_FILE_SIZE_2LOG))
            {
                DateTime endTime = DateTime.Now;
                double duration = (endTime - startTime).TotalMilliseconds;
                var logMessage = $", Duration: {duration} ms";
                File.AppendAllText(logPath, logMessage + Environment.NewLine);
            }
        }

        internal void ValidateWithAssetDatabase()
        {
            if (IsMissing || IsFolder || type == AssetType.SCRIPT) return;
            
            try
            {
                // Get Unity's known dependencies

                string[] unityDeps = AssetDatabase.GetDependencies(assetPath, false);
                if (unityDeps == null) return;
                var unityGuids = new HashSet<string>();
                
                foreach (string depPath in unityDeps)
                {
                    if (depPath == assetPath) continue; // Skip self-reference
                    string depGuid = AssetDatabase.AssetPathToGUID(depPath);
                    if (!string.IsNullOrEmpty(depGuid))
                        unityGuids.Add(depGuid);
                }
                
                // Compare with FR2's parsed results
                var fr2Guids = new HashSet<string>(UseGUIDs.Keys);
                var missingInFR2 = new List<string>();
                
                // Find guids in Unity but not in FR2
                foreach (string unityGuid in unityGuids)
                {
                    if (!fr2Guids.Contains(unityGuid))
                        missingInFR2.Add(unityGuid);
                }
                
                if (missingInFR2.Count > 0 && extension != ".shadergraph")
                {
                    AssetFinderLOG.LogWarning($"AssetDatabase has {missingInFR2.Count} more dependencies than FR2 for '{assetPath}'. " +
                                   "This may indicate unsaved changes or new asset types not handled by FR2 parser.");
                    
                    // Add missing references to FR2's cache
                    foreach (string missingGuid in missingInFR2)
                    {
                        AddUseGUID(missingGuid);
                        AssetFinderLOG.Log($"Add missingGUID: {missingGuid} --> {AssetDatabase.GUIDToAssetPath(missingGuid)}");
                    }
                }
            }
            catch
            {
                AssetFinderLOG.LogWarning($"Failed to validate dependencies for {assetPath}");
            }
        }

        internal void LoadYAML2()
        {
            if (!m_pathLoaded) LoadPathInfo();

            if (!File.Exists(m_assetPath))
            {
                state = AssetState.MISSING;
                return;
            }

            if (m_assetPath == "ProjectSettings/EditorBuildSettings.asset")
            {
                EditorBuildSettingsScene[] listScenes = EditorBuildSettings.scenes;
                foreach (EditorBuildSettingsScene scene in listScenes)
                {
                    if (!scene.enabled) continue;
                    string path = scene.path;
                    string guid = AssetDatabase.AssetPathToGUID(path);

                    AddUseGUID(guid, 0);
					// AssetFinderLOG.Log("AddScene: " + path);
                }
            }

            if (string.IsNullOrEmpty(extension))
            {
                AssetFinderLOG.LogWarning($"Something wrong? <{m_extension}>");
            }

            if (extension == ".spriteatlas") // check for force include in build
            {
                var atlasAsset = AssetDatabase.LoadAssetAtPath<UnityObject>(m_assetPath);
                if (atlasAsset != null)
                {
                    var so = new SerializedObject(atlasAsset);
                    SerializedProperty prop = so.FindProperty("m_EditorData.bindAsDefault");
                    m_forceIncludeInBuild = prop.boolValue;
                }
            }
            
            AssetFinderParser.ReadContent(m_assetPath, AddUseGUID);
        }

        internal void LoadFolder()
        {
            if (!Directory.Exists(m_assetPath))
            {
                state = AssetState.MISSING;
                return;
            }

            // do not analyse folders outside project
            if (!m_assetPath.StartsWith("Assets/")) return;

            try
            {
                string[] files = Directory.GetFiles(m_assetPath);
                string[] dirs = Directory.GetDirectories(m_assetPath);

                foreach (string f in files)
                {
                    if (f.EndsWith(".meta", StringComparison.Ordinal)) continue;

                    string fguid = AssetDatabase.AssetPathToGUID(f);
                    if (string.IsNullOrEmpty(fguid)) continue;

                    AddUseGUID(fguid);
                }

                foreach (string d in dirs)
                {
                    string fguid = AssetDatabase.AssetPathToGUID(d);
                    if (string.IsNullOrEmpty(fguid)) continue;

                    AddUseGUID(fguid);
                }
            }
            catch (Exception e)
            {
                AssetFinderLOG.LogWarning("LoadFolder() error :: " + e + "\n" + assetPath);
            }
            finally
            {

                state = AssetState.MISSING;
            }
        }

        internal void LoadBinaryAsset()
        {
            ClearUseGUIDs();

            UnityObject assetData = AssetDatabase.LoadAssetAtPath(m_assetPath, typeof(UnityObject));
            if (assetData is GameObject go)
            {
                type = AssetType.MODEL;
                LoadGameObject(go);
                binaryLoaded += 10;
            } else if (assetData is TerrainData terrainData)
            {
                type = AssetType.TERRAIN;
                LoadTerrainData(terrainData);
                binaryLoaded += 20;
            } else if (assetData is LightingDataAsset lightAsset)
            {
                type = AssetType.LIGHTING_DATA;
                LoadLightingData(lightAsset);
                binaryLoaded += 20;
            } else
            {
                LoadSerialized(assetData);
                binaryLoaded++;
            }

			AssetFinderLOG.Log("LoadBinaryAsset :: " + assetData + ":" + type);
            if (binaryLoaded <= 30) return;
            binaryLoaded = 0;
            AssetFinderUnity.UnloadUnusedAssets();
        }

        internal void LoadGameObject(GameObject go)
        {
            Component[] compList = go.GetComponentsInChildren<Component>();
            for (var i = 0; i < compList.Length; i++)
            {
                LoadSerialized(compList[i]);
            }
        }

        internal void LoadSerialized(UnityObject target)
        {
            SerializedProperty[] props = AssetFinderUnity.xGetSerializedProperties(target, true);

            for (var i = 0; i < props.Length; i++)
            {
                if (props[i].propertyType != SerializedPropertyType.ObjectReference) continue;

                UnityObject refObj = props[i].objectReferenceValue;
                if (refObj == null) continue;

                string refGUID = AssetDatabase.AssetPathToGUID(
                    AssetDatabase.GetAssetPath(refObj)
                );

                AddUseGUID(refGUID);
            }
        }

        private void AddTextureGUID(SerializedProperty prop)
        {
            if (prop == null || prop.objectReferenceValue == null) return;
            string path = AssetDatabase.GetAssetPath(prop.objectReferenceValue);
            if (string.IsNullOrEmpty(path)) return;
            AddUseGUID(AssetDatabase.AssetPathToGUID(path));
        }

        internal void LoadLightingData(LightingDataAsset asset)
        {
            foreach (Texture texture in AssetFinderLightmap.Read(asset))
            {
                if (texture == null) continue;
                string path = AssetDatabase.GetAssetPath(texture);
                string assetGUID = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(assetGUID))
                {
                    AddUseGUID(assetGUID);
                }
            }
        }

        internal void LoadTerrainData(TerrainData terrain)
        {
#if UNITY_2018_3_OR_NEWER
            TerrainLayer[] arr0 = terrain.terrainLayers;
            for (var i = 0; i < arr0.Length; i++)
            {
                string aPath = AssetDatabase.GetAssetPath(arr0[i]);
                string refGUID = AssetDatabase.AssetPathToGUID(aPath);
                AddUseGUID(refGUID);
            }
#endif

            DetailPrototype[] arr = terrain.detailPrototypes;

            for (var i = 0; i < arr.Length; i++)
            {
                string aPath = AssetDatabase.GetAssetPath(arr[i].prototypeTexture);
                string refGUID = AssetDatabase.AssetPathToGUID(aPath);
                AddUseGUID(refGUID);
            }

            TreePrototype[] arr2 = terrain.treePrototypes;
            for (var i = 0; i < arr2.Length; i++)
            {
                string aPath = AssetDatabase.GetAssetPath(arr2[i].prefab);
                string refGUID = AssetDatabase.AssetPathToGUID(aPath);
                AddUseGUID(refGUID);
            }

            AssetFinderTerrain.TerrainTextureData[] arr3 = AssetFinderTerrain.GetTerrainTextureDatas(terrain);
            for (var i = 0; i < arr3.Length; i++)
            {
                AssetFinderTerrain.TerrainTextureData texs = arr3[i];
                for (var k = 0; k < texs.textures.Length; k++)
                {
                    Texture2D tex = texs.textures[k];
                    if (tex == null) continue;

                    string aPath = AssetDatabase.GetAssetPath(tex);
                    if (string.IsNullOrEmpty(aPath)) continue;

                    string refGUID = AssetDatabase.AssetPathToGUID(aPath);
                    if (string.IsNullOrEmpty(refGUID)) continue;

                    AddUseGUID(refGUID);
                }
            }
        }

        internal static void ClearLog()
        {
            if (shouldWriteImportLog)
            {
                File.WriteAllText(logPath, string.Empty);
            } else
            {
                if (File.Exists(logPath)) File.Delete(logPath);
            }

            scanStartTime = DateTime.Now;
        }

        internal static void WriteTotalScanTime()
        {
            if (!shouldWriteImportLog) return;
            double totalScanTime = (DateTime.Now - scanStartTime).TotalSeconds;
            File.AppendAllText(logPath, $"\nTotal scan time: {totalScanTime} seconds\n");
        }

        private void ClearUseGUIDs()
        {
		    // AssetFinderLOG.Log("ClearUseGUIDs: " + assetPath);
            UseGUIDs.Clear();
            UseGUIDsList.Clear();
        }
    }
} 