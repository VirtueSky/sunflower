using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderAssetOrganizer : IRefDraw
    {
        // Processing state and dependencies
        private readonly IWindow _window;
        private bool _isProcessing;
        private float _progress;
        private string _currentFolderPath;
        private HashSet<string> _foldersToProcess = new HashSet<string>();
        private Dictionary<string, List<string>> _assetsToMove = new Dictionary<string, List<string>>();
        private List<PlannedMove> _plannedMoves = new List<PlannedMove>();
        
        // Results tracking
        private Dictionary<string, int> _organizedFolders = new Dictionary<string, int>();
        private List<string> _errorAssets = new List<string>();
        private string _reportTitle;
        
        // Settings backup
        private AssetFinderAutoRefreshMode _originalAutoRefreshMode;

        // Asset classification
        private static readonly string[] SpecialFolders = {"/Editor/", "/Resources/", "/StreamingAssets/", "/Gizmos/", "/Plugins/", "/Standard Assets/"};
        private static readonly string[] ScriptExtensions = { ".cs", ".js", ".boo" };
        private HashSet<string> _spriteAtlasFolders = new HashSet<string>();
        private Dictionary<string, SpriteAtlasInfo> _spriteAtlasInfo = new Dictionary<string, SpriteAtlasInfo>();
        private Dictionary<string, SpineAssetInfo> _spineAssetInfo = new Dictionary<string, SpineAssetInfo>();
        private HashSet<string> _spineAssets = new HashSet<string>();

        internal AssetFinderAssetOrganizer(IWindow window, Func<AssetFinderRefDrawer.Sort> getSort, Func<AssetFinderRefDrawer.Mode> getGroup)
        {
            _window = window;
        }

        public IWindow window => _window;

        public int ElementCount()
        {
            return _organizedFolders.Count + _errorAssets.Count;
        }

        public bool Draw(Rect rect)
        {
            GUI.BeginClip(rect);
            GUILayout.BeginArea(new Rect(0, 0, rect.width, rect.height));
            
            bool result = DrawLayout();
            
            GUILayout.EndArea();
            GUI.EndClip();
            return result;
        }

        public bool DrawLayout()
        {
            GUILayout.BeginVertical();
            {
                if (_isProcessing)
                {
                    DrawProgressBar();
                }
                else
                {
                    DrawSelectedFolders();
                    
                    GUILayout.Space(10);

                    bool hasFoldersSelected = _foldersToProcess.Count > 0;
                    GUI.enabled = hasFoldersSelected;
                    if (GUILayout.Button("Organize Selected Folder" + (hasFoldersSelected && _foldersToProcess.Count > 1 ? "s" : ""), AssetFinderTheme.Current.ActionButtonHeight))
                    {
                        StartProcessing();
                    }
                    GUI.enabled = true;

                    if (!hasFoldersSelected)
                    {
                        EditorGUILayout.HelpBox("Please select one or more folders in the Project panel to organize.", MessageType.Info);
                    }

                    if (!string.IsNullOrEmpty(_reportTitle))
                    {
                        EditorGUILayout.HelpBox(_reportTitle, MessageType.Info);
                    }
                    
                    if (_organizedFolders.Count > 0)
                    {
                        GUILayout.Space(10);
                        EditorGUILayout.LabelField("Organized Assets:", EditorStyles.boldLabel);
                        
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        foreach (var folder in _organizedFolders.OrderBy(f => f.Key))
                        {
                            EditorGUILayout.LabelField($"{folder.Key}: {folder.Value} assets", EditorStyles.miniLabel);
                        }
                        GUILayout.EndVertical();
                    }
                    
                    if (_errorAssets.Count > 0)
                    {
                        GUILayout.Space(10);
                        EditorGUILayout.LabelField("Errors:", EditorStyles.boldLabel);
                        
                        GUILayout.BeginVertical(EditorStyles.helpBox);
                        foreach (string error in _errorAssets.Take(10))
                        {
                            EditorGUILayout.LabelField(error, EditorStyles.miniLabel);
                        }
                        
                        if (_errorAssets.Count > 10)
                        {
                            EditorGUILayout.LabelField($"...and {_errorAssets.Count - 10} more", EditorStyles.miniLabel);
                        }
                        
                        GUILayout.EndVertical();
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            return true;
        }

        private void DrawSelectedFolders()
        {
            _foldersToProcess.Clear();
            
            foreach (UnityObject obj in Selection.objects)
            {
                string path = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(path)) continue;
                
                if (AssetDatabase.IsValidFolder(path))
                {
                    _foldersToProcess.Add(path);
                }
            }
            
            CleanupSelectionHierarchy();
            
            if (_foldersToProcess.Count > 0)
            {
                EditorGUILayout.LabelField("Selected Folders:", EditorStyles.boldLabel);
                
                GUILayout.BeginVertical(EditorStyles.helpBox);
                foreach (string folder in _foldersToProcess.OrderBy(f => f))
                {
                    EditorGUILayout.LabelField(folder, EditorStyles.miniLabel);
                }
                GUILayout.EndVertical();
            }
        }
        
        private void CleanupSelectionHierarchy()
        {
            var foldersToRemove = new HashSet<string>();
            
            foreach (string folder in _foldersToProcess)
            {
                foreach (string otherFolder in _foldersToProcess)
                {
                    if (folder != otherFolder && folder.StartsWith(otherFolder + "/"))
                    {
                        foldersToRemove.Add(folder);
                    }
                }
            }
            
            foreach (string folderToRemove in foldersToRemove)
            {
                _foldersToProcess.Remove(folderToRemove);
            }
        }

        private void DrawProgressBar()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Organizing assets...", EditorStyles.boldLabel);
            
            Rect rect = AssetFinderTheme.Current.GetProgressBarRect();
            EditorGUI.ProgressBar(rect, _progress, _currentFolderPath);
            
            EditorGUILayout.Space();
                            if (GUILayout.Button("Cancel", AssetFinderTheme.Current.CancelButtonHeight))
            {
                CancelProcessing();
            }
        }

        private void StartProcessing()
        {
            DisableAutoRefresh();
            _isProcessing = true;
            _progress = 0f;
            _currentFolderPath = string.Empty;
            _organizedFolders.Clear();
            _errorAssets.Clear();
            _assetsToMove.Clear();
            _plannedMoves.Clear();
            _reportTitle = null;
            _spriteAtlasFolders.Clear();
            _spriteAtlasInfo.Clear();
            _spineAssetInfo.Clear();
            _spineAssets.Clear();
            
            AnalyzeSpineAssets();
            AnalyzeSpriteAtlases();
            
            foreach (string folderPath in _foldersToProcess)
            {
                AnalyzeFolder(folderPath);
            }
            
            CleanupEmptyMoveEntries();
            
            var scriptMoves = new Dictionary<string, List<string>>();
            var nonScriptMoves = new Dictionary<string, List<string>>();
            foreach (var kvp in _assetsToMove)
            {
                var scripts = new List<string>();
                var nonScripts = new List<string>();
                foreach (var assetPath in kvp.Value)
                {
                    string ext = Path.GetExtension(assetPath).ToLowerInvariant();
                    if (ScriptExtensions.Contains(ext)) scripts.Add(assetPath);
                    else nonScripts.Add(assetPath);
                }
                if (nonScripts.Count > 0) nonScriptMoves[kvp.Key] = nonScripts;
                if (scripts.Count > 0) scriptMoves[kvp.Key] = scripts;
            }
            _assetsToMove.Clear();
            foreach (var kvp in nonScriptMoves)
            {
                _assetsToMove[kvp.Key] = kvp.Value;
            }

            foreach (var kvp in scriptMoves)
            {
                _assetsToMove[kvp.Key] = kvp.Value;
            }

            PlanAllMoves();
            
            if (_plannedMoves.Count == 0)
            {
                _reportTitle = "No assets needed to be organized.";
                CleanupAfterProcessing();
                return;
            }
            
            EditorApplication.update -= ProcessNextBatch;
            EditorApplication.update += ProcessNextBatch;
        }
        
        private void CancelProcessing()
        {
            _isProcessing = false;
            CleanupAfterProcessing();
        }

        private void CleanupAfterProcessing()
        {
            _isProcessing = false;
            RestoreAutoRefreshMode();
            AssetFinderDeleteEmptyFolder.DeleteAllEmptyFoldersRecursive("Assets");
            AssetDatabase.Refresh();
            _window.Repaint();
        }
        
        private void DisableAutoRefresh()
        {
            _originalAutoRefreshMode = AssetFinderSettingExt.autoRefreshMode;
            AssetFinderSettingExt.autoRefreshMode = AssetFinderAutoRefreshMode.Off;
        }
        
        private void RestoreAutoRefreshMode()
        {
            AssetFinderSettingExt.autoRefreshMode = _originalAutoRefreshMode;
        }
        
        private void AnalyzeSpineAssets()
        {
            string[] spineGUIDs = AssetDatabase.FindAssets("t:SkeletonDataAsset");
            foreach (string guid in spineGUIDs)
            {
                string skeletonPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityObject skeletonAsset = AssetDatabase.LoadAssetAtPath<UnityObject>(skeletonPath);
                if (skeletonAsset != null)
                {
                    string skeletonName = Path.GetFileNameWithoutExtension(skeletonPath);
                    if (skeletonName.EndsWith("_SkeletonData"))
                    {
                        skeletonName = skeletonName.Substring(0, skeletonName.Length - "_SkeletonData".Length);
                    }
                    
                    var spineInfo = new SpineAssetInfo { skeletonPath = skeletonPath, spineName = skeletonName };
                    spineInfo.assets.Add(skeletonPath);
                    _spineAssets.Add(skeletonPath);
                    
                    SerializedObject so = new SerializedObject(skeletonAsset);
                    SerializedProperty iterator = so.GetIterator();
                    while (iterator.NextVisible(true))
                    {
                        if (iterator.propertyType == SerializedPropertyType.ObjectReference && iterator.objectReferenceValue != null)
                        {
                            string refPath = AssetDatabase.GetAssetPath(iterator.objectReferenceValue);
                            if (!string.IsNullOrEmpty(refPath) && refPath != skeletonPath)
                            {
                                spineInfo.assets.Add(refPath);
                                _spineAssets.Add(refPath);
                            }
                        }
                    }
                    
                    _spineAssetInfo[skeletonPath] = spineInfo;
                }
            }
        }
        
        private void AnalyzeSpriteAtlases()
        {
            string[] atlasGUIDs = AssetDatabase.FindAssets("t:SpriteAtlas");
            foreach (string guid in atlasGUIDs)
            {
                string atlasPath = AssetDatabase.GUIDToAssetPath(guid);
                UnityObject atlasAsset = AssetDatabase.LoadAssetAtPath<UnityObject>(atlasPath);
                if (atlasAsset != null)
                {
                    var atlasInfo = new SpriteAtlasInfo { atlasPath = atlasPath };
                    SerializedObject so = new SerializedObject(atlasAsset);
                    SerializedProperty objectsForPacking = so.FindProperty("m_EditorData.packables");
                    if (objectsForPacking != null && objectsForPacking.isArray)
                    {
                        for (int i = 0; i < objectsForPacking.arraySize; i++)
                        {
                            SerializedProperty element = objectsForPacking.GetArrayElementAtIndex(i);
                            UnityObject obj = element.objectReferenceValue;
                            if (obj != null)
                            {
                                string objPath = AssetDatabase.GetAssetPath(obj);
                                if (AssetDatabase.IsValidFolder(objPath))
                                {
                                    _spriteAtlasFolders.Add(objPath);
                                    atlasInfo.folders.Add(objPath);
                                }
                                else
                                {
                                    atlasInfo.sprites.Add(objPath);
                                }
                            }
                        }
                    }
                    _spriteAtlasInfo[atlasPath] = atlasInfo;
                }
            }
        }
        
        private void AnalyzeFolder(string folderPath)
        {
            if (string.IsNullOrEmpty(folderPath)) return;
            if (!AssetDatabase.IsValidFolder(folderPath)) return;

            string[] assetGUIDs = AssetDatabase.FindAssets("*", new[] { folderPath });

            foreach (string guid in assetGUIDs)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                if (AssetDatabase.IsValidFolder(assetPath) || assetPath.EndsWith(".meta"))
                    continue;

                string assetPathLower = assetPath.Replace('\\', '/').ToLowerInvariant();
                bool inSpecial = SpecialFolders.Any(sf => assetPathLower.Contains(sf.ToLowerInvariant()));
                if (inSpecial) continue;
                
                if (IsSpineAsset(assetPath)) continue;
                if (IsInSpriteAtlasFolder(assetPath)) continue;

                string extension = Path.GetExtension(assetPath).ToLowerInvariant();
                string folderType = DetermineFolderType(assetPath, extension);
                
                string targetFolderPath = $"{folderPath}/{folderType}";
                string currentAssetFolder = Path.GetDirectoryName(assetPath);
                
                if (NormalizePath(currentAssetFolder).Equals(NormalizePath(targetFolderPath), StringComparison.OrdinalIgnoreCase))
                    continue;
                
                string key = folderPath + "|" + folderType;
                if (!_assetsToMove.ContainsKey(key)) _assetsToMove[key] = new List<string>();
                _assetsToMove[key].Add(assetPath);
            }
            
            foreach (var spineInfo in _spineAssetInfo.Values)
            {
                bool anyAssetInFolder = spineInfo.assets.Any(asset => asset.StartsWith(folderPath + "/") || asset == folderPath);
                if (anyAssetInFolder)
                {
                    string targetPath = folderPath + "/Spines/" + spineInfo.spineName;
                    string key = targetPath + "|SPINE_GROUP|" + spineInfo.spineName;
                    if (!_assetsToMove.ContainsKey(key)) _assetsToMove[key] = new List<string>();
                    foreach (string asset in spineInfo.assets)
                    {
                        if (asset.StartsWith(folderPath + "/") || asset == folderPath)
                        {
                            string currentAssetFolder = Path.GetDirectoryName(asset);
                            if (!NormalizePath(currentAssetFolder).Equals(NormalizePath(targetPath), StringComparison.OrdinalIgnoreCase))
                            {
                                _assetsToMove[key].Add(asset);
                            }
                        }
                    }
                }
            }
            
            foreach (var atlasInfo in _spriteAtlasInfo.Values)
            {
                if (atlasInfo.atlasPath.StartsWith(folderPath + "/") || atlasInfo.atlasPath == folderPath)
                {
                    string atlasName = Path.GetFileNameWithoutExtension(atlasInfo.atlasPath);
                    string targetPath = folderPath + "/SpriteAtlas";
                    
                    string currentAtlasFolder = Path.GetDirectoryName(atlasInfo.atlasPath);
                    if (!NormalizePath(currentAtlasFolder).Equals(NormalizePath(targetPath), StringComparison.OrdinalIgnoreCase))
                    {
                        string atlasKey = targetPath + "|ATLAS|" + atlasName;
                        if (!_assetsToMove.ContainsKey(atlasKey)) _assetsToMove[atlasKey] = new List<string>();
                        _assetsToMove[atlasKey].Add(atlasInfo.atlasPath);
                    }
                    
                    if (atlasInfo.folders.Count == 1 && atlasInfo.sprites.Count == 0)
                    {
                        string singleFolder = atlasInfo.folders[0];
                        if (singleFolder.StartsWith(folderPath + "/") || singleFolder == folderPath)
                        {
                            string expectedFolderPath = Path.Combine(targetPath, atlasName);
                            if (!NormalizePath(singleFolder).Equals(NormalizePath(expectedFolderPath), StringComparison.OrdinalIgnoreCase))
                            {
                                string folderKey = targetPath + "|ATLAS_FOLDER|" + atlasName;
                                if (!_assetsToMove.ContainsKey(folderKey)) _assetsToMove[folderKey] = new List<string>();
                                _assetsToMove[folderKey].Add(singleFolder);
                            }
                        }
                    }
                }
            }
        }
        
        private bool IsSpineAsset(string assetPath)
        {
            return _spineAssets.Contains(assetPath);
        }
        
        private bool IsInSpriteAtlasFolder(string assetPath)
        {
            return _spriteAtlasFolders.Any(folder => assetPath.StartsWith(folder + "/"));
        }
        
        private string DetermineFolderType(string assetPath, string extension)
        {
            if (extension == ".shadervariants")
            {
                return "Shaders";
            }
            
            if (extension == ".lighting")
            {
                return "Scenes";
            }
            
            if (extension == ".asset")
            {
                Type assetType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (assetType != null)
                {
                    string typeName = assetType.Name;
                    if (typeName == "LightingDataAsset" || typeName == "LightingSettings")
                    {
                        return "Scenes";
                    }
                    if (typeName == "TMPro_FontAsset" || typeName.Contains("FontAsset"))
                    {
                        return "Fonts";
                    }
                    if (typeName == "Mesh")
                    {
                        return "Models";
                    }
                    if (typeName == "AnimationClip")
                    {
                        return "Animations";
                    }
                }
            }
            
            if (IsTextureImportedAsSprite(assetPath))
            {
                return "Sprites";
            }
            
            string folderType = GetAssetFolderType(extension);
            if (folderType == "Others")
            {
                Type mainType = AssetDatabase.GetMainAssetTypeAtPath(assetPath);
                if (mainType != null)
                {
                    string typeName = mainType.Name;
                    string specialType = GetAssetFolderTypeByTypeName(typeName);
                    if (specialType != null) folderType = specialType;
                    else folderType = typeName;
                }
            }
            return folderType;
        }
        
        private bool IsTextureImportedAsSprite(string assetPath)
        {
            AssetImporter importer = AssetImporter.GetAtPath(assetPath);
            if (importer is TextureImporter textureImporter)
            {
                return textureImporter.textureType == TextureImporterType.Sprite;
            }
            return false;
        }

        private string GetAssetFolderTypeByTypeName(string typeName)
        {
            for (int i = 0; i < AssetFinderAssetGroupDrawer.FILTERS.Length - 1; i++)
            {
                AssetFinderAssetGroup filter = AssetFinderAssetGroupDrawer.FILTERS[i];
                if (!string.IsNullOrEmpty(filter.name) && filter.name.Equals(typeName, StringComparison.OrdinalIgnoreCase))
                {
                    return filter.name + "s";
                }
            }
            return null;
        }
        
        private void PlanAllMoves()
        {
            var planned = new List<PlannedMove>();
            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var kvp in _assetsToMove)
            {
                string[] parts = kvp.Key.Split('|');
                string parentFolder = parts[0];
                string folderType = parts[1];
                
                if (parts.Length > 2 && (parts[1] == "SPINE_GROUP" || parts[1] == "ATLAS" || parts[1] == "ATLAS_FOLDER"))
                {
                    string newName = parts[2];
                    string targetFolderPath = parentFolder;
                    string absTargetFolder = AssetPathToAbs(targetFolderPath);
                    if (!Directory.Exists(absTargetFolder)) Directory.CreateDirectory(absTargetFolder);
                    
                    if (parts[1] == "SPINE_GROUP")
                    {
                        foreach (string assetPath in kvp.Value)
                        {
                            string srcAbs = AssetPathToAbs(assetPath);
                            string fileName = Path.GetFileName(assetPath);
                            string dstAbs = Path.Combine(absTargetFolder, fileName);
                            
                            if (NormalizePath(srcAbs).Equals(NormalizePath(dstAbs), StringComparison.OrdinalIgnoreCase))
                                continue;
                            
                            planned.Add(new PlannedMove {
                                srcAsset = srcAbs,
                                srcMeta = srcAbs + ".meta",
                                dstAsset = dstAbs,
                                dstMeta = dstAbs + ".meta",
                                isFolder = false
                            });
                        }
                    }
                    else if (parts[1] == "ATLAS")
                    {
                        foreach (string assetPath in kvp.Value)
                        {
                            string srcAbs = AssetPathToAbs(assetPath);
                            string fileName = Path.GetFileName(assetPath);
                            string dstAbs = Path.Combine(absTargetFolder, fileName);
                            
                            if (NormalizePath(srcAbs).Equals(NormalizePath(dstAbs), StringComparison.OrdinalIgnoreCase))
                                continue;
                            
                            planned.Add(new PlannedMove {
                                srcAsset = srcAbs,
                                srcMeta = srcAbs + ".meta",
                                dstAsset = dstAbs,
                                dstMeta = dstAbs + ".meta",
                                isFolder = false
                            });
                        }
                    }
                    else if (parts[1] == "ATLAS_FOLDER")
                    {
                        foreach (string folderPath in kvp.Value)
                        {
                            string srcAbs = AssetPathToAbs(folderPath);
                            string dstAbs = Path.Combine(absTargetFolder, newName);
                            
                            if (NormalizePath(srcAbs).Equals(NormalizePath(dstAbs), StringComparison.OrdinalIgnoreCase))
                                continue;
                            
                            planned.Add(new PlannedMove {
                                srcAsset = srcAbs,
                                srcMeta = srcAbs + ".meta",
                                dstAsset = dstAbs,
                                dstMeta = dstAbs + ".meta",
                                isFolder = true
                            });
                        }
                    }
                    continue;
                }
                
                string targetFolderPath2 = $"{parentFolder}/{folderType}";
                string absTargetFolder2 = AssetPathToAbs(targetFolderPath2);
                if (!Directory.Exists(absTargetFolder2)) Directory.CreateDirectory(absTargetFolder2);
                
                foreach (string assetPath in kvp.Value)
                {
                    string fileName = Path.GetFileName(assetPath);
                    string baseName = Path.GetFileNameWithoutExtension(fileName);
                    string ext = Path.GetExtension(fileName);
                    string srcAbs = AssetPathToAbs(assetPath);
                    string srcMeta = srcAbs + ".meta";
                    string dstName = fileName;
                    
                    string dstAbs = Path.Combine(absTargetFolder2, dstName);
                    string dstMeta = dstAbs + ".meta";
                    int suffix = 1;
                    
                    while (usedNames.Contains(dstAbs) || (File.Exists(dstAbs) && !NormalizePath(srcAbs).Equals(NormalizePath(dstAbs), StringComparison.OrdinalIgnoreCase)))
                    {
                        dstName = $"{baseName}-{suffix.ToString("D2")}{ext}";
                        dstAbs = Path.Combine(absTargetFolder2, dstName);
                        dstMeta = dstAbs + ".meta";
                        suffix++;
                    }
                    
                    if (NormalizePath(srcAbs).Equals(NormalizePath(dstAbs), StringComparison.OrdinalIgnoreCase))
                        continue;
                    
                    usedNames.Add(dstAbs);
                    planned.Add(new PlannedMove {
                        srcAsset = srcAbs,
                        srcMeta = srcMeta,
                        dstAsset = dstAbs,
                        dstMeta = dstMeta
                    });
                }
            }
            _plannedMoves = planned;
        }

        private static string AssetPathToAbs(string assetPath)
        {
            if (assetPath.StartsWith("Assets/"))
                return Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), assetPath);
            if (assetPath.StartsWith("Assets"))
                return Path.Combine(Application.dataPath.Substring(0, Application.dataPath.Length - 6), assetPath);
            return assetPath;
        }
        
        private void ProcessNextBatch()
        {
            if (!_isProcessing)
            {
                EditorApplication.update -= ProcessNextBatch;
                CleanupAfterProcessing();
                return;
            }
            
            int batchSize = 10;
            int moved = 0;
            while (_plannedMoves.Count > 0 && moved < batchSize)
            {
                var move = _plannedMoves[0];
                _plannedMoves.RemoveAt(0);
                
                if (move.isFolder)
                {
                    if (!NormalizePath(move.srcAsset).Equals(NormalizePath(move.dstAsset), StringComparison.OrdinalIgnoreCase))
                    {
                        if (Directory.Exists(move.dstAsset)) Directory.Delete(move.dstAsset, true);
                        Directory.Move(move.srcAsset, move.dstAsset);
                    }
                    if (File.Exists(move.srcMeta) && !NormalizePath(move.srcMeta).Equals(NormalizePath(move.dstMeta), StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(move.dstMeta)) File.Delete(move.dstMeta);
                        File.Move(move.srcMeta, move.dstMeta);
                    }
                    string folderType = Path.GetFileName(Path.GetDirectoryName(move.dstAsset));
                    if (!_organizedFolders.ContainsKey(folderType)) _organizedFolders[folderType] = 1;
                    else _organizedFolders[folderType]++;
                }
                else
                {
                    if (!NormalizePath(move.srcAsset).Equals(NormalizePath(move.dstAsset), StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(move.dstAsset)) File.Delete(move.dstAsset);
                        if (File.Exists(move.srcAsset))
                        {
                            File.Move(move.srcAsset, move.dstAsset);
                        }
                        else
                        {
                            _errorAssets.Add($"Source file not found: {move.srcAsset}");
                            moved++;
                            continue;
                        }
                    }
                    if (File.Exists(move.srcMeta) && !NormalizePath(move.srcMeta).Equals(NormalizePath(move.dstMeta), StringComparison.OrdinalIgnoreCase))
                    {
                        if (File.Exists(move.dstMeta)) File.Delete(move.dstMeta);
                        File.Move(move.srcMeta, move.dstMeta);
                    }
                    string folderType = Path.GetFileName(Path.GetDirectoryName(move.dstAsset));
                    if (!_organizedFolders.ContainsKey(folderType)) _organizedFolders[folderType] = 1;
                    else _organizedFolders[folderType]++;
                }
                moved++;
            }
            _progress = 1f - (_plannedMoves.Count / (float)(1 + _plannedMoves.Count));
            if (_plannedMoves.Count == 0)
            {
                int totalAssets = _organizedFolders.Values.Sum();
                _reportTitle = totalAssets > 0 
                    ? $"Successfully organized {totalAssets} assets into {_organizedFolders.Count} category folders!" 
                    : "No assets needed to be organized.";
                if (_errorAssets.Count > 0)
                {
                    _reportTitle += $" Encountered {_errorAssets.Count} errors.";
                }
                EditorApplication.update -= ProcessNextBatch;
                AssetDatabase.Refresh();
                CleanupAfterProcessing();
            }
            _window.Repaint();
        }
        
        private void DeleteEmptySubfolders(string parentFolder)
        {
            DeleteEmptyFoldersRecursive(parentFolder);
        }

        private static void DeleteEmptyFoldersRecursive(string root)
        {
            if (!Directory.Exists(root)) return;
            foreach (var dir in Directory.GetDirectories(root))
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) continue;
                DeleteEmptyFoldersRecursive(dir.Replace("\\", "/"));
                if (IsUnityFolderEmptyStatic(dir.Replace("\\", "/")))
                {
                    string relPath = "Assets" + dir.Replace(Application.dataPath, "").Replace("\\", "/");
                    AssetDatabase.DeleteAsset(relPath);
                }
            }
        }
        
        private static bool IsUnityFolderEmptyStatic(string folder)
        {
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (fileName == null) continue;
                if (fileName.StartsWith(".")) return false;
                if (!fileName.EndsWith(".meta", StringComparison.OrdinalIgnoreCase)) return false;
            }
            var dirs = Directory.GetDirectories(folder);
            foreach (var dir in dirs)
            {
                var dirName = Path.GetFileName(dir);
                if (dirName.StartsWith(".")) return false;
                if (!IsUnityFolderEmptyStatic(dir.Replace("\\", "/"))) return false;
            }
            return true;
        }
        
        private string GetAssetFolderType(string extension)
        {
            // Normalize extension to lowercase for case-insensitive comparison
            string normalizedExt = extension?.ToLowerInvariant() ?? "";
            
            for (int i = 0; i < AssetFinderAssetGroupDrawer.FILTERS.Length - 1; i++)
            {
                AssetFinderAssetGroup filter = AssetFinderAssetGroupDrawer.FILTERS[i];
                if (filter.extension.Contains(normalizedExt))
                {
                    return filter.name + "s";
                }
            }
            
            return "Others";
        }

        private class PlannedMove {
            public string srcAsset;
            public string srcMeta;
            public string dstAsset;
            public string dstMeta;
            public bool isFolder;
        }
        
        private class SpriteAtlasInfo {
            public string atlasPath;
            public List<string> folders = new List<string>();
            public List<string> sprites = new List<string>();
        }
        
        private class SpineAssetInfo {
            public string skeletonPath;
            public string spineName;
            public List<string> assets = new List<string>();
        }

        private void CleanupEmptyMoveEntries()
        {
            var keysToRemove = new List<string>();
            foreach (var kvp in _assetsToMove)
            {
                if (kvp.Value.Count == 0)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }
            foreach (string key in keysToRemove)
            {
                _assetsToMove.Remove(key);
            }
        }
        
        private static string NormalizePath(string path)
        {
            return path.Replace('\\', '/').TrimEnd('/');
        }
    }
}