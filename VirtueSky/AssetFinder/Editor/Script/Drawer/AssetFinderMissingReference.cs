using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderMissingReference : IRefDraw
    {
        // Processing state and dependencies
        private readonly IWindow _window;
        private bool _isProcessing;
        private float _progress;
        private string _currentAssetPath;
        private HashSet<string> _processedAssets = new HashSet<string>();
        private List<string> _assetsToProcess = new List<string>();
        
        // Results tracking
        private Dictionary<string, int> _cleanedAssets = new Dictionary<string, int>();
        private List<string> _errorAssets = new List<string>();
        private string _reportTitle;
        private StringBuilder _report = new StringBuilder();
        
        // Scene handling
        private List<string> _originalScenePaths = new List<string>();
        private Scene? _currentlyOpenScene = null;
        
        // Settings backup
        private AssetFinderAutoRefreshMode _originalAutoRefreshMode;

        internal AssetFinderMissingReference(IWindow window, Func<AssetFinderRefDrawer.Sort> getSort, Func<AssetFinderRefDrawer.Mode> getGroup)
        {
            _window = window;
            // Note: getSort and getGroup are unused but kept for interface compatibility
        }

        public IWindow window => _window;

        public int ElementCount()
        {
            return _cleanedAssets.Count + _errorAssets.Count;
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
                    if (GUILayout.Button("Scan & Remove Missing Scripts", AssetFinderTheme.Current.ActionButtonHeight))
                    {
                        StartProcessing();
                    }

                    // Display report title if available
                    if (!string.IsNullOrEmpty(_reportTitle)) 
                        EditorGUILayout.HelpBox(_reportTitle, MessageType.Info);
                    
                    // Show full report button if there are results
                    if (_cleanedAssets.Count > 0 || _errorAssets.Count > 0)
                    {
                        if (GUILayout.Button("Full Report"))
                        {
                            AssetFinderLOG.Log(_report.ToString());
                        }
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            return true;
        }

        private void DrawProgressBar()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Processing assets...", EditorStyles.boldLabel);
            
            Rect rect = AssetFinderTheme.Current.GetProgressBarRect();
            EditorGUI.ProgressBar(rect, _progress, 
                $"Processing {_currentAssetPath} ({_processedAssets.Count}/{_assetsToProcess.Count})");
            
            EditorGUILayout.Space();
            if (GUILayout.Button("Cancel", AssetFinderTheme.Current.CancelButtonHeight))
            {
                CancelProcessing();
            }
        }

        private void StartProcessing()
        {
            try
            {
                DisableAutoRefresh();
                SaveCurrentSceneSetup();
                
                _isProcessing = true;
                _progress = 0f;
                _currentAssetPath = string.Empty;
                _processedAssets.Clear();
                _assetsToProcess.Clear();
                _cleanedAssets.Clear();
                _errorAssets.Clear();
                _report.Clear();

                CollectAssetsToProcess();
                SortAssetsByDependencyOrder();

                EditorApplication.update -= ProcessNextAsset;
                EditorApplication.update += ProcessNextAsset;
            }
            catch (Exception ex)
            {
                AssetFinderLOG.LogError($"Error starting processing: {ex.Message}");
                CleanupAfterProcessing();
            }
        }
        
        private void CancelProcessing()
        {
            _isProcessing = false;
            CleanupAfterProcessing();
        }

        private void CleanupAfterProcessing()
        {
            _isProcessing = false;
            RestoreOriginalScenes();
            RestoreAutoRefreshMode();
            _window.Repaint();
        }
        
        private void ProcessNextAsset()
        {
            if (!_isProcessing)
            {
                EditorApplication.update -= ProcessNextAsset;
                CleanupAfterProcessing();
                return;
            }

            if (_processedAssets.Count >= _assetsToProcess.Count)
            {
                EditorApplication.update -= ProcessNextAsset;
                GenerateReport();
                CleanupAfterProcessing();
                return;
            }

            string assetPath = _assetsToProcess[_processedAssets.Count];
            _currentAssetPath = assetPath;
            _progress = _processedAssets.Count / (float)_assetsToProcess.Count;

            try
            {
                if (assetPath.EndsWith(".unity"))
                {
                    ProcessScene(assetPath);
                }
                else if (assetPath.EndsWith(".prefab"))
                {
                    ProcessPrefab(assetPath);
                }
            }
            catch (Exception ex)
            {
                AssetFinderLOG.LogError($"Error processing {assetPath}: {ex.Message}");
                _errorAssets.Add(assetPath);
            }
            
            _processedAssets.Add(assetPath);
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
        
        private void SaveCurrentSceneSetup()
        {
            _originalScenePaths.Clear();
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (!string.IsNullOrEmpty(scene.path))
                {
                    _originalScenePaths.Add(scene.path);
                }
            }
        }

        private void RestoreOriginalScenes()
        {
            try
            {
                _currentlyOpenScene = null;
                
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    return;
                }
                
                EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                
                for (int i = 0; i < _originalScenePaths.Count; i++)
                {
                    string scenePath = _originalScenePaths[i];
                    OpenSceneMode mode = i == 0 ? OpenSceneMode.Single : OpenSceneMode.Additive;
                    EditorSceneManager.OpenScene(scenePath, mode);
                }
            }
            catch (Exception ex)
            {
                AssetFinderLOG.LogError($"Error restoring original scenes: {ex.Message}");
            }
        }
        
        private void CollectAssetsToProcess()
        {
            AddAssetTypeToPipeline("t:Prefab");
            AddAssetTypeToPipeline("t:Scene");
        }
        
        private void AddAssetTypeToPipeline(string assetTypeFilter)
        {
            string[] guids = AssetDatabase.FindAssets(assetTypeFilter, new[] { "Assets" });
            foreach (string guid in guids)
            {
                _assetsToProcess.Add(AssetDatabase.GUIDToAssetPath(guid));
            }
        }

        private void SortAssetsByDependencyOrder()
        {
            Dictionary<string, List<string>> dependencies = BuildDependencyGraph(out Dictionary<string, List<string>> dependents);
            Dictionary<string, int> assetLevels = CalculateAssetLevels(dependencies);
            
            _assetsToProcess = _assetsToProcess
                .OrderBy(path => assetLevels[path])
                .ToList();
        }
        
        private Dictionary<string, List<string>> BuildDependencyGraph(out Dictionary<string, List<string>> dependents)
        {
            Dictionary<string, List<string>> dependencies = new Dictionary<string, List<string>>();
            dependents = new Dictionary<string, List<string>>();
            
            foreach (string assetPath in _assetsToProcess)
            {
                dependencies[assetPath] = new List<string>();
                dependents[assetPath] = new List<string>();
            }
            
            foreach (string assetPath in _assetsToProcess)
            {
                string[] dependencyPaths = AssetDatabase.GetDependencies(assetPath, false);
                foreach (string depPath in dependencyPaths)
                {
                    if (_assetsToProcess.Contains(depPath) && depPath != assetPath)
                    {
                        dependencies[assetPath].Add(depPath);
                        dependents[depPath].Add(assetPath);
                    }
                }
            }
            
            return dependencies;
        }
        
        private Dictionary<string, int> CalculateAssetLevels(Dictionary<string, List<string>> dependencies)
        {
            Dictionary<string, int> assetLevels = new Dictionary<string, int>();
            HashSet<string> processed = new HashSet<string>();
            
            // First pass: assign level 0 to assets with no dependencies
            foreach (string assetPath in _assetsToProcess)
            {
                if (dependencies[assetPath].Count == 0)
                {
                    assetLevels[assetPath] = 0;
                    processed.Add(assetPath);
                }
            }
            
            // Process remaining assets
            bool changesMade = true;
            int currentLevel = 0;
            
            while (changesMade && processed.Count < _assetsToProcess.Count)
            {
                currentLevel++;
                changesMade = false;
                
                foreach (string assetPath in _assetsToProcess)
                {
                    if (processed.Contains(assetPath)) continue;
                    
                    bool allDepsProcessed = true;
                    foreach (string dep in dependencies[assetPath])
                    {
                        if (!processed.Contains(dep))
                        {
                            allDepsProcessed = false;
                            break;
                        }
                    }
                    
                    if (allDepsProcessed)
                    {
                        assetLevels[assetPath] = currentLevel;
                        processed.Add(assetPath);
                        changesMade = true;
                    }
                }
            }
            
            // Handle circular dependencies by assigning them to the next level
            if (processed.Count < _assetsToProcess.Count)
            {
                currentLevel++;
                foreach (string assetPath in _assetsToProcess)
                {
                    if (!processed.Contains(assetPath))
                    {
                        assetLevels[assetPath] = currentLevel;
                    }
                }
            }
            
            return assetLevels;
        }
        
        private void GenerateReport()
        {
            _report.Clear();
            
            int totalRemovedScripts = _cleanedAssets.Values.Sum();
            
            if (_cleanedAssets.Count > 0)
            {
                _reportTitle = $"Removed missing scripts from {_cleanedAssets.Count} assets!";
                _report.AppendLine($"Removed {totalRemovedScripts} missing script(s) from {_cleanedAssets.Count} assets:");
                
                foreach (var asset in _cleanedAssets.OrderBy(x => x.Key))
                {
                    _report.AppendLine($"- {asset.Key}: {asset.Value} script(s) removed");
                }
                _report.AppendLine();
            }
            else
            {
                _reportTitle = "No missing scripts were found to remove.";
                _report.AppendLine("No missing scripts were found to remove.");
                _report.AppendLine();
            }
            
            if (_errorAssets.Count > 0)
            {
                _report.AppendLine($"Errors occurred in {_errorAssets.Count} assets:");
                foreach (string asset in _errorAssets.OrderBy(x => x))
                {
                    _report.AppendLine($"- {asset}");
                }
            }
        }
        
        private void ProcessPrefab(string prefabPath)
        {
            try
            {
                GameObject prefabAsset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
                if (prefabAsset == null) return;

                GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefabAsset) as GameObject;
                if (prefabInstance == null) return;
                
                try
                {
                    int removedCount = RemoveMissingScriptsFromGameObject(prefabInstance);
                    
                    if (removedCount > 0)
                    {
                        PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.AutomatedAction);
                        _cleanedAssets[prefabPath] = removedCount;
                        AssetDatabase.SaveAssets();
                    }
                }
                finally
                {
                    UnityObject.DestroyImmediate(prefabInstance);
                }
            }
            catch (Exception ex)
            {
                AssetFinderLOG.LogError($"Error processing prefab {prefabPath}: {ex.Message}");
                _errorAssets.Add(prefabPath);
            }
        }
        
        private void ProcessScene(string scenePath)
        {
            if (_currentlyOpenScene.HasValue && EditorSceneManager.GetActiveScene().isDirty)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
                _currentlyOpenScene = null;
            }
            
            Scene scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            _currentlyOpenScene = scene;
            
            int removedCount = 0;
            foreach (GameObject rootObj in scene.GetRootGameObjects())
            {
                removedCount += RemoveMissingScriptsFromGameObject(rootObj);
            }
            
            if (removedCount > 0)
            {
                EditorSceneManager.SaveScene(scene);
                _cleanedAssets[scenePath] = removedCount;
            }
        }
        
        private int RemoveMissingScriptsFromGameObject(GameObject gameObject)
        {
            int removedCount = 0;
            
            int missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject);
            if (missingCount > 0)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
                removedCount += missingCount;
            }
            
            foreach (Transform child in gameObject.transform)
            {
                removedCount += RemoveMissingScriptsFromGameObject(child.gameObject);
            }
            
            return removedCount;
        }
    }
} 