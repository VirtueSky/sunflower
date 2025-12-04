#if UNITY_2018_3_OR_NEWER
#define SUPPORT_NESTED_PREFAB
#endif

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_2017_1_OR_NEWER
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
#endif

#if SUPPORT_NESTED_PREFAB
using UnityEditor.Experimental.SceneManagement;
#endif

namespace VirtueSky.AssetFinder.Editor
{
    internal class SceneRefInfo : IEquatable<SceneRefInfo>
    {
        public Component sourceComponent;
        public Object target;
        public string propertyPath;
        public bool isSceneObject;
        
        public bool IsBackwardRef => target != null && sourceComponent != null;
        public Object GetTargetComponent() => target;
        public GameObject GetGameObjectFromTarget()
        {
            if (!target) return null;
            if (target is GameObject go) return go;
            if (target is Component comp && comp != null) return comp.gameObject;
            return null;
        }
        
        public bool Equals(SceneRefInfo other)
        {
            if (other == null) return false;
            return sourceComponent == other.sourceComponent && 
                   target == other.target && 
                   propertyPath == other.propertyPath;
        }
        
        public override bool Equals(object obj)
        {
            return obj is SceneRefInfo other && Equals(other);
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = sourceComponent?.GetHashCode() ?? 0;
                hash = hash * 31 + (target?.GetHashCode() ?? 0);
                hash = hash * 31 + (propertyPath?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }

    internal enum SceneCacheStatus
    {
        None,       // Never initialized (default state)
        Changed,    // Needs refresh
        Scanning,   // Currently scanning
        Ready       // Ready for use
    }

    [Flags]
    internal enum SceneChangeFlags
    {
        None = 0,
        SceneReset = 1,        // Scene unloaded/reloaded or new scene opened
        SceneModify = 2,       // Objects modified (from Undo system)
        SceneAdditive = 4,     // Prefab stage or multi-scene changes
        UserRefresh = 8        // User-requested full refresh
    }

    internal class AssetFinderSceneCache
    {
        private static AssetFinderSceneCache _api;
        public static Action onReady;
        
        private SceneCacheStatus _status = SceneCacheStatus.None;
        private Dictionary<Component, HashSet<HashValue>> _cache = new Dictionary<Component, HashSet<HashValue>>();
        private bool _isDirty;
        private SceneChangeFlags _changeFlags = SceneChangeFlags.None;
        private readonly HashSet<int> _modifiedInstanceIds = new HashSet<int>();
        private bool _autoRefresh;
        private float _lastDirtyTime;
        private const float DIRTY_DEBOUNCE_TIME = 0.1f; // 100ms debounce
        
        public int current;
        private List<GameObject> listGO;

        //public HashSet<string> prefabDependencies = new HashSet<string>();
        public Dictionary<GameObject, HashSet<string>> prefabDependencies =
            new Dictionary<GameObject, HashSet<string>>();
        
        public int total;

        public AssetFinderSceneCache()
        {
            // Register for Unity hierarchy change events
            // Note: Multiple events are registered to catch different types of scene changes
#if UNITY_2018_1_OR_NEWER
            EditorApplication.hierarchyChanged -= OnSceneChanged;
            EditorApplication.hierarchyChanged += OnSceneChanged;
#else
			EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
			EditorApplication.hierarchyWindowChanged += OnSceneChanged;
#endif

#if UNITY_2018_2_OR_NEWER
            EditorSceneManager.activeSceneChangedInEditMode -= OnSceneChanged;
            EditorSceneManager.activeSceneChangedInEditMode += OnSceneChanged;
#endif

#if UNITY_2017_1_OR_NEWER
            SceneManager.activeSceneChanged -= OnSceneChanged;
            SceneManager.activeSceneChanged += OnSceneChanged;

            SceneManager.sceneLoaded -= OnSceneChanged;
            SceneManager.sceneLoaded += OnSceneChanged;

            Undo.postprocessModifications -= OnModify;
            Undo.postprocessModifications += OnModify;
#endif

            // Add play mode detection
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

            // Initialize auto-refresh from settings
            _autoRefresh = AssetFinderSettingExt.isAutoRefreshEnabled;
        }

        public static bool hasCache => _api != null && (_api._status == SceneCacheStatus.Ready || _api._status == SceneCacheStatus.Changed);
        
        public static AssetFinderSceneCache Api
        {
            get
            {
                if (_api != null) return _api;
                _api = new AssetFinderSceneCache();
                return _api;
            }
        }

        public static bool isReady => (_api != null && _api._status == SceneCacheStatus.Ready);
        public static bool hasInit => (_api != null && _api._status != SceneCacheStatus.None);
        
        public SceneCacheStatus Status
        {
            get => _status;
            set => _status = value;
        }

        public Dictionary<Component, HashSet<HashValue>> cache
        {
            get
            {
                if (_cache == null) RefreshCache(false);
                return _cache;
            }
        }

        public bool Dirty
        {
            get => _isDirty;
            set => _isDirty = value;
        }

        public bool AutoRefresh
        {
            get => _autoRefresh;
            set => _autoRefresh = value;
        }

        public void RefreshCache(bool force)
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                AssetFinderLOG.Log($"refreshCache skipped: isCompiling: {EditorApplication.isCompiling} / isUpdating: {EditorApplication.isUpdating}");
                
                SetDirty();
                return;
            }
            
            if (_status == SceneCacheStatus.Scanning) 
            {
                AssetFinderLOG.Log($"refreshCache skipped - already scanning");
                return; // Prevent re-entrance
            }
            
            // User refresh always forces full scan
            if (force || (_changeFlags & SceneChangeFlags.UserRefresh) != 0)
            {
                PerformFullRefresh();
                return;
            }
            
            if (!_autoRefresh)
            {
                #if AssetFinderDEBUG
                Debug.Log($"refreshCache: skipped - autoRefresh == false (but still dirty)");
                #endif
                
                _isDirty = true;
                _status =  SceneCacheStatus.Changed;
                return;
            } 
            
            // if (_status == SceneCacheStatus.None || _status == SceneCacheStatus.Changed)
            // {
            //     Debug.Log($"refreshCache: start scanning");
            // }
            
            // Check if scan is needed
            bool needsScan = _isDirty || _cache.Count == 0 || _changeFlags != SceneChangeFlags.None;
            if (!needsScan)
            {
                AssetFinderLOG.Log($"refreshCache: skipped - do not needScan");
                _status = SceneCacheStatus.Ready;
                return;
            }
            
            if ((_changeFlags & SceneChangeFlags.SceneReset) != 0)
            {
                PerformFullRefresh();
                return;
            }
            
            PerformIncrementalRefresh();
        }

        private void PerformFullRefresh()
        {
            _status = SceneCacheStatus.Scanning;
            _isDirty = false;
            current = 0;
            total = 0;
            
            _cache = new Dictionary<Component, HashSet<HashValue>>();
            prefabDependencies = new Dictionary<GameObject, HashSet<string>>();
            _modifiedInstanceIds.Clear();
            _changeFlags = SceneChangeFlags.None;

            List<GameObject> listRootGO = null;

#if SUPPORT_NESTED_PREFAB
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                GameObject rootPrefab = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;
                if (rootPrefab != null) listRootGO = new List<GameObject> { rootPrefab };
            }
#endif
            if (listRootGO == null)
            {
                listGO = AssetFinderUnity.getAllObjsInCurScene().ToList();
            } else
            {
                listGO = new List<GameObject>();
                foreach (GameObject item in listRootGO)
                {
                    listGO.AddRange(AssetFinderUnity.getAllChild(item, true));
                }
            }

            // Set total as work count (objects to scan) and freeze it during scan
            total = listGO.Count;
            current = 0;
            
            if (total == 0 || total == current)
            {
                Dirty = false;
                _status = SceneCacheStatus.Ready;
                onReady?.Invoke();
                return;
            }
            
            AssetFinderLOG.Log($"Register OnUpdate: {nameof(PerformFullRefresh)}");
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
        }

        private void PerformIncrementalRefresh()
        {
            // AssetFinderLOG.Log("PerformIncrementalRefresh!");
            
            _isDirty = false;
            _status = SceneCacheStatus.Scanning;
            current = 0;
            total = 0;
            
            // Clean up invalid cache entries
            var keysToRemove = new List<Component>();
            foreach (var kvp in _cache)
            {
                if (kvp.Key == null || kvp.Key.gameObject == null)
                {
                    keysToRemove.Add(kvp.Key);
                }
            }
            
            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            // Get objects that need scanning (modified + new ones)
            var currentObjects = new HashSet<int>();
            List<GameObject> allObjects;

#if SUPPORT_NESTED_PREFAB
            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                GameObject rootPrefab = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;
                allObjects = rootPrefab != null ? AssetFinderUnity.getAllChild(rootPrefab, true).ToList() : new List<GameObject>();
            }
            else
#endif
            {
                allObjects = AssetFinderUnity.getAllObjsInCurScene().ToList();
            }

            foreach (var go in allObjects)
            {
                if (go != null) currentObjects.Add(go.GetInstanceID());
            }

            // Find objects to scan (modified or new)
            listGO = new List<GameObject>();
            foreach (var go in allObjects)
            {
                if (go == null) continue;
                
                int instanceId = go.GetInstanceID();
                bool isModified = _modifiedInstanceIds.Contains(instanceId);
                bool isNew = !_cache.Keys.Any(comp => comp != null && comp.gameObject != null && comp.gameObject.GetInstanceID() == instanceId);
                
                if (isModified || isNew)
                {
                    listGO.Add(go);
                }
            }

            // Set total as work count (objects to scan) and freeze it during scan
            total = listGO.Count;
            current = 0;
            _modifiedInstanceIds.Clear();
            _changeFlags = SceneChangeFlags.None;
            
            if (total == 0 || current >= total)
            {
                _status = SceneCacheStatus.Ready;
                if (onReady != null) onReady();
                return;
            }
            
            #if AssetFinderDEBUG
            Debug.Log($"Register OnUpdate: {nameof(PerformIncrementalRefresh)} | {current}/{total}");
            #endif
            
            EditorApplication.update -= OnUpdate;
            EditorApplication.update += OnUpdate;
            Dirty = false;
        }
        
        private void OnUpdate()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating)
            {
                StopScanning(SceneCacheStatus.None);
                return;
            }
            
            for (var i = 0; i < 5 * AssetFinderCache.priority; i++)
            {
                if (listGO == null || listGO.Count <= 0) break;
                var index = listGO.Count - 1;
                var go = listGO[index];
                if (go == null) 
                {
                    listGO.RemoveAt(index);
                    current++;
                    continue;
                }

                // Remove existing cache entries for this GameObject
                var componentsToRemove = _cache.Keys.Where(comp => comp != null && comp.gameObject == go).ToList();
                foreach (var comp in componentsToRemove)
                {
                    _cache.Remove(comp);
                }

                string prefabGUID = AssetFinderUnity.GetPrefabParent(go);
                if (!string.IsNullOrEmpty(prefabGUID))
                {
                    Transform parent = go.transform.parent;
                    while (parent != null)
                    {
                        GameObject g = parent.gameObject;
                        if (!prefabDependencies.ContainsKey(g)) prefabDependencies.Add(g, new HashSet<string>());

                        prefabDependencies[g].Add(prefabGUID);
                        parent = parent.parent;
                    }
                }

                Component[] components = go.GetComponents<Component>();

                foreach (Component com in components)
                {
                    if (com == null) continue;

                    var serialized = new SerializedObject(com);
                    SerializedProperty it = serialized.GetIterator().Copy();
                    while (it.Next(true))
                    {
                        if (it.propertyType != SerializedPropertyType.ObjectReference) continue;

                        if (it.objectReferenceValue == null) continue;
                        bool isSceneObject = it.objectReferenceValue.IsSceneObject();
                        if (!_cache.ContainsKey(com)) _cache.Add(com, new HashSet<HashValue>());

                        _cache[com].Add(new HashValue
                            { target = it.objectReferenceValue, isSceneObject = isSceneObject, propertyPath = it.propertyPath });
                    }
                }

                listGO.RemoveAt(index);
                current++;
            }

            if (listGO != null && listGO.Count > 0) return; // to be continue
            
            Dirty = false;
            StopScanning(SceneCacheStatus.Ready);
            onReady?.Invoke();
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    StopScanning(SceneCacheStatus.None);
                    break;
                    
                case PlayModeStateChange.ExitingPlayMode:
                    SetDirty();
                    break;
                    
                case PlayModeStateChange.EnteredEditMode:
                    if (_autoRefresh)
                    {
                        _changeFlags |= SceneChangeFlags.SceneReset;
                        SetDirty();
                        EditorApplication.delayCall += () => RefreshCache(false);
                    }
                    break;
            }
        }

        private void OnSceneChanged()
        {
            // In play mode, avoid unnecessary dirty marking when auto-refresh is disabled
            if (Application.isPlaying)
            {
                // Only mark dirty if auto-refresh is enabled, otherwise it's pointless
                if (_autoRefresh)
                {
                    #if AssetFinderDEBUG
                    Debug.LogWarning($"Set dirty: {nameof(OnSceneChanged)} - Play Mode with AutoRefresh");
                    #endif
                    SetDirty();
                }
                return;
            }
            
            // Check if we're in prefab mode - don't treat prefab mode changes as scene changes
#if SUPPORT_NESTED_PREFAB
            bool isInPrefabMode = PrefabStageUtility.GetCurrentPrefabStage() != null;
            if (isInPrefabMode)
            {
                #if AssetFinderDEBUG
                Debug.LogWarning($"Set dirty: {nameof(isInPrefabMode)}");
                #endif
                
                // Properly mark as dirty and update status
                SetDirty();
                _changeFlags |= SceneChangeFlags.SceneAdditive;
                
                // Always perform incremental refresh in prefab mode
                // because the scene/prefab hierarchy has changed
                PerformIncrementalRefresh();
                return;
            }
#endif
            
            // Detect if this is a scene unload vs hierarchy change
            List<GameObject> currentObjects = AssetFinderUnity.getAllObjsInCurScene().ToList();
            
            // If no objects in scene, it's likely a scene unload - just clean cache without refresh
            if (currentObjects.Count == 0)
            {
                _cache.Clear();
                prefabDependencies.Clear();
                _modifiedInstanceIds.Clear();
                _changeFlags = SceneChangeFlags.None;
                _status = SceneCacheStatus.Ready;
                _isDirty = false;
                return;
            }
            
            // Check for new GameObjects (cloning/duplication)
            var existingIds = new HashSet<int>();
            foreach (var comp in _cache.Keys)
            {
                if (comp != null && comp.gameObject != null)
                {
                    existingIds.Add(comp.gameObject.GetInstanceID());
                }
            }
            
            bool hasNewObjects = false;
            foreach (var go in currentObjects)
            {
                if (go != null && !existingIds.Contains(go.GetInstanceID()))
                {
                    hasNewObjects = true;
                    _modifiedInstanceIds.Add(go.GetInstanceID());
                }
            }
            
            // Use incremental refresh for new objects, full refresh for major changes
            if (hasNewObjects && _cache.Count > 0)
            {
                _changeFlags |= SceneChangeFlags.SceneModify;
            }
            else
            {
                _changeFlags |= SceneChangeFlags.SceneAdditive;
            }
            
            SetDirty();
            
            // Only auto-refresh if auto refresh is enabled and not currently scanning
            if (_autoRefresh && _status != SceneCacheStatus.Scanning)
            {
                Api.RefreshCache(false);
            }
        }

#if UNITY_2017_1_OR_NEWER
        private UndoPropertyModification[] OnModify(UndoPropertyModification[] modifications)
        {
            bool hasRelevantChange = false;
            
            for (var i = 0; i < modifications.Length; i++)
            {
                var mod = modifications[i];
                
                // Only mark dirty for object reference changes in scene objects
                if (mod.currentValue.objectReference != null || mod.previousValue.objectReference != null)
                {
                    var target = mod.currentValue.target ?? mod.previousValue.target;
                    if (target != null && !EditorUtility.IsPersistent(target))
                    {
                        hasRelevantChange = true;
                        _modifiedInstanceIds.Add(target.GetInstanceID());
                        
                        // Also mark the GameObject if target is a component
                        if (target is Component comp && comp.gameObject != null)
                        {
                            _modifiedInstanceIds.Add(comp.gameObject.GetInstanceID());
                        }
                    }
                }
            }

            if (hasRelevantChange)
            {
                _changeFlags |= SceneChangeFlags.SceneModify;
                SetDirty();
            }

            return modifications;
        }
#endif

        public void SetDirty()
        {
            // Debounce rapid SetDirty calls to prevent unnecessary dirty marking
            float currentTime = Time.realtimeSinceStartup;
            if (currentTime - _lastDirtyTime < DIRTY_DEBOUNCE_TIME && _isDirty)
            {
                return; // Skip if already dirty and within debounce time
            }
            
            _lastDirtyTime = currentTime;
            
            if (_status == SceneCacheStatus.None || _status == SceneCacheStatus.Ready)
            {
                _status = SceneCacheStatus.Changed;
            }
            _isDirty = true;
        }

        public void ForceRefresh()
        {
            _changeFlags |= SceneChangeFlags.UserRefresh;
            SetDirty();
            RefreshCache(true);
        }

        private void StopScanning(SceneCacheStatus updatedStatus)
        {
            // Debug.Log($"StopScanning: {current} / {total} | {_status}");
            EditorApplication.update -= OnUpdate;
            _status = updatedStatus;
            listGO = null;
            current = 0;
            total = 0;
        }
        

        public static Dictionary<string, AssetFinderRef> FindSceneUseSceneObjects(GameObject[] targets)
        {
            var results = new Dictionary<string, AssetFinderRef>();
            
            foreach (var selectedGO in targets)
            {
                if (selectedGO == null || selectedGO.IsAssetObject()) continue;

                var key = selectedGO.GetInstanceID().ToString();
                if (!results.ContainsKey(key)) 
                {
                    results.Add(key, new AssetFinderSceneRef(0, selectedGO));
                }

                ScanForwardReferences(selectedGO, results);
            }

            return results;
        }

        public static Dictionary<string, AssetFinderRef> FindSceneBackwardReferences(GameObject[] targets)
        {
            var results = new Dictionary<string, AssetFinderRef>();
            
            foreach (var selectedGO in targets)
            {
                if (selectedGO.IsAssetObject()) continue;
                
                var key = selectedGO.GetInstanceID().ToString();
                if (!results.ContainsKey(key))
                {
                    results.Add(key, new AssetFinderSceneRef(0, selectedGO));
                }
            }
            
            ScanBackwardReferences(targets.Where(t => t != null).ToHashSet(), results);
            return results;
        }

        public static Dictionary<string, AssetFinderRef> FindSceneInScene(GameObject[] targets)
        {
            var results = new Dictionary<string, AssetFinderRef>();
            
            foreach (var obj in targets)
            {
                if (obj == null || obj.IsAssetObject()) continue;
                var key = obj.GetInstanceID().ToString();
                if (!results.ContainsKey(key)) results.Add(key, new AssetFinderSceneRef(0, obj));
            }

            ScanSceneInScene(targets, results);
            return results;
        }

        private static void ScanForwardReferences(GameObject selectedGO, Dictionary<string, AssetFinderRef> results)
        {
            if (selectedGO == null) return;

            var sceneCache = Api.cache;
            var components = selectedGO.GetComponents<Component>();
            
            for (var i = 0; i < components.Length; i++)
            {
                var com = components[i];
                if (com == null || com is Transform) continue;
                
                if (!sceneCache.TryGetValue(com, out var hashValues)) continue;
                
                foreach (var hashValue in hashValues)
                {
                    if (hashValue.target == null || !hashValue.isSceneObject) continue;
                    
                    var targetGO = GetGameObjectFromTarget(hashValue.target);
                    if (targetGO == null || targetGO == selectedGO) continue;

                    var targetKey = hashValue.target.GetInstanceID().ToString();
                    if (!results.ContainsKey(targetKey)) 
                    {
                        var tRef = new AssetFinderSceneRef(1, hashValue.target)
                        {
                            sourceRefs = new List<SceneRefInfo>()
                        };
                        results.Add(targetKey, tRef);
                    }

                    var targetRef = results[targetKey] as AssetFinderSceneRef;
                    targetRef.sourceRefs.Add(new SceneRefInfo
                    {
                        sourceComponent = com,
                        target = hashValue.target,
                        propertyPath = hashValue.propertyPath,
                        isSceneObject = hashValue.isSceneObject
                    });
                }
            }
        }

        private static void ScanBackwardReferences(HashSet<GameObject> selectedGameObjects, Dictionary<string, AssetFinderRef> results)
        {
            var sceneCache = Api.cache;
            
            foreach (var cacheEntry in sceneCache)
            {
                var sourceComponent = cacheEntry.Key;
                if (sourceComponent == null || sourceComponent.gameObject == null || typeof(Transform).IsAssignableFrom(sourceComponent.GetType())) continue;
                
                var sourceGO = sourceComponent.gameObject;
                if (sourceGO == null || selectedGameObjects.Contains(sourceGO)) continue;
                
                foreach (var hashValue in cacheEntry.Value)
                {
                    if (hashValue.target == null || !hashValue.isSceneObject) continue;
                    
                    var targetGO = GetGameObjectFromTarget(hashValue.target);
                    if (targetGO == null || targetGO == sourceGO || !selectedGameObjects.Contains(targetGO)) continue;

                    var sourceKey = sourceGO.GetInstanceID().ToString();
                    if (!results.ContainsKey(sourceKey))
                    {
                        var sourceRef = new AssetFinderSceneRef(1, sourceGO);
                        sourceRef.backwardRefs = new List<SceneRefInfo>();
                        results.Add(sourceKey, sourceRef);
                    }
                        
                    var backwardRef = results[sourceKey] as AssetFinderSceneRef;
                    backwardRef.backwardRefs.Add(new SceneRefInfo
                    {
                        sourceComponent = sourceComponent,
                        target = hashValue.target,
                        propertyPath = hashValue.propertyPath,
                        isSceneObject = hashValue.isSceneObject
                    });
                }
            }
        }

        private static void ScanSceneInScene(GameObject[] objs, Dictionary<string, AssetFinderRef> results)
        {
            var sceneCache = Api.cache;
            
            foreach (var cacheEntry in sceneCache)
            {
                var sourceComponent = cacheEntry.Key;
                if (sourceComponent == null || sourceComponent.gameObject == null || typeof(Transform).IsAssignableFrom(sourceComponent.GetType())) continue;
                
                foreach (var hashValue in cacheEntry.Value)
                {
                    if (hashValue.target == null) continue;
                    var targetGO = GetGameObjectFromTarget(hashValue.target);
                    if (targetGO == null || sourceComponent.gameObject == targetGO) continue;

                    foreach (var obj in objs)
                    {
                        if (obj == null || targetGO != obj) continue;

                        var key = sourceComponent.GetInstanceID().ToString();
                        if (!results.ContainsKey(key)) 
                        {
                            var sourceRef = new AssetFinderSceneRef(1, sourceComponent)
                            {
                                backwardRefs = new List<SceneRefInfo>()
                            };
                            results.Add(key, sourceRef);
                        }

                        var backwardRef = results[key] as AssetFinderSceneRef;
                        backwardRef.backwardRefs.Add(new SceneRefInfo
                        {
                            sourceComponent = sourceComponent,
                            target = hashValue.target,
                            propertyPath = hashValue.propertyPath,
                            isSceneObject = hashValue.isSceneObject
                        });
                        break;
                    }
                }
            }
        }

        private static GameObject GetGameObjectFromTarget(Object target)
        {
            if (!target) return null;
            if (target is GameObject go) return go;
            if (target is Component comp && comp) return comp.gameObject;
            return null;
        }

        internal class HashValue : IEquatable<HashValue>
        {
            // original fields – DO NOT RENAME
            public bool isSceneObject;
            public Object target;
            public string propertyPath;

            // --------- value equality ---------
            public bool Equals(HashValue other)
            {
                if (ReferenceEquals(other, null)) return false;
                if (ReferenceEquals(this,  other)) return true;

                return isSceneObject == other.isSceneObject &&
                    target        == other.target &&          // Unity overloads ==
                    propertyPath  == other.propertyPath;
            }

            public override bool Equals(object obj) => Equals(obj as HashValue);

            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + isSceneObject.GetHashCode();
                    hash = hash * 23 + (target ? target.GetInstanceID() : 0);
                    hash = hash * 23 + (propertyPath?.GetHashCode() ?? 0);
                    return hash;
                }
            }

            public static bool operator ==(HashValue a, HashValue b) => Equals(a, b);
            public static bool operator !=(HashValue a, HashValue b) => !Equals(a, b);
        }

#if UNITY_2017_1_OR_NEWER
        private void OnSceneChanged(Scene scene, LoadSceneMode mode)
        {
            OnSceneChanged();
        }

        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            OnSceneChanged();
        }
#endif
    }
}
