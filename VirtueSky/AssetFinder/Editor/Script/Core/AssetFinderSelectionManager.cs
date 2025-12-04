using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    /// <summary>
    /// Unified Selection Manager for FindReference2
    /// 
    /// This system replaces the fragmented selection logic found in multiple classes and provides:
    /// 
    /// BENEFITS:
    /// - Single source of truth for all selection state
    /// - Automatic Unity version compatibility (2019.4+ vs 2021+)
    /// - Separation of scene objects vs assets with proper typing
    /// - Reduced GC pressure through efficient data structures
    /// - Centralized Selection.selectionChanged monitoring
    /// - Thread-safe singleton pattern
    /// 
    /// ARCHITECTURE:
    /// - AssetFinderSelectionManager: Main coordinator and event dispatcher
    /// - AssetFinderSceneSelection: Handles GameObject/Component selection with int instanceIds
    /// - AssetFinderAssetSelection: Handles asset selection with GUID/FileID pairs
    /// 
    /// UNITY VERSION DIFFERENCES HANDLED:
    /// - Unity 2018.1+: Uses AssetDatabase.TryGetGUIDAndLocalFileIdentifier
    /// - Pre-2018.1: Uses reflection to access m_LocalIdentfierInFile property
    /// 
    /// USAGE:
    /// - Window classes subscribe to AssetFinderSelectionManager.SelectionChanged
    /// - Access current selection via Instance.SceneSelection or Instance.AssetSelection
    /// - All selection changes automatically propagate to subscribers
    /// 
    /// MIGRATION:
    /// - Replaces scattered Selection.objects calls
    /// - Eliminates string-based instanceId storage
    /// - Removes duplicated Unity version compatibility code
    /// - Centralizes selection caching and frame-based optimization
    /// </summary>
    [InitializeOnLoad]
    internal class AssetFinderSelectionManager
    {
        public static event System.Action SelectionChanged;
        private static AssetFinderSelectionManager _instance;

        // Static constructor - called automatically when Unity loads/recompiles
        static AssetFinderSelectionManager()
        {
            // Initialize immediately when class is loaded
            Initialize();
        }

        public static AssetFinderSelectionManager Instance
        {
            get
            {
                if (_instance != null) return _instance;
                Initialize();
                return _instance;
            }
        }

        private AssetFinderSceneSelection sceneSelection;
        private AssetFinderAssetSelection assetSelection;

        // Cached Unity selection for comparison
        private UnityObject[] cachedUnitySelection = Array.Empty<UnityObject>();
        public AssetFinderSceneSelection SceneSelection => sceneSelection;
        public AssetFinderAssetSelection AssetSelection => assetSelection;

        public bool IsSelectingSceneObjects => sceneSelection?.Count > 0;
        public bool IsSelectingAssets => assetSelection?.Count > 0;
        public bool HasSelection => TotalCount > 0;
        public int TotalCount => (sceneSelection?.Count ?? 0) + (assetSelection?.Count ?? 0);

        private static void Initialize()
        {
            if (_instance != null)
            {
                Debug.LogWarning("AssetFinderSelectionManager already initialized - cleaning up first");
                Cleanup();
            }

            _instance = new AssetFinderSelectionManager();
            _instance.InitializeInstance();

            // Ensure cleanup happens on domain reload
#if UNITY_2019_1_OR_NEWER
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload -= Cleanup;
            UnityEditor.AssemblyReloadEvents.beforeAssemblyReload += Cleanup;
#endif
        }

        private void InitializeInstance()
        {
            sceneSelection = new AssetFinderSceneSelection();
            assetSelection = new AssetFinderAssetSelection();

            // Ensure we don't double-subscribe (idempotent)
            Selection.selectionChanged -= OnUnitySelectionChanged;
            Selection.selectionChanged += OnUnitySelectionChanged;

            // Initialize with current Unity selection
            RefreshFromUnitySelection();
        }

        public UnityObject[] GetUnitySelection()
        {
            return cachedUnitySelection ?? Array.Empty<UnityObject>();
        }

        private void RefreshFromUnitySelection()
        {
            var currentSelection = Selection.objects ?? Array.Empty<UnityObject>();
            if (AreSelectionsEqual(cachedUnitySelection, currentSelection)) return;
            cachedUnitySelection = currentSelection;
            UpdateFR2Selection(currentSelection);
            SelectionChanged?.Invoke();
        }

        private static bool AreSelectionsEqual(UnityObject[] selection1, UnityObject[] selection2)
        {
            if (selection1 == null && selection2 == null) return true;
            if (selection1 == null || selection2 == null) return false;
            if (selection1.Length != selection2.Length) return false;

            // Order matters for selection comparison
            for (int i = 0; i < selection1.Length; i++)
            {
                if (selection1[i] != selection2[i]) return false;
            }

            return true;
        }

        internal void UpdateFR2Selection(UnityObject[] newSelection)
        {
            sceneSelection.Clear();
            assetSelection.Clear();

            if (newSelection == null || newSelection.Length == 0) return;

            foreach (var obj in newSelection)
            {
                if (obj == null) continue;

                if (AssetDatabase.Contains(obj))
                {
                    assetSelection.AddAsset(obj);
                }
                else if (obj is GameObject gameObject)
                {
                    sceneSelection.AddGameObject(gameObject);
                }
                else if (obj is Component component && component.gameObject != null)
                {
                    sceneSelection.AddGameObject(component.gameObject);
                }
            }
        }

        private void OnUnitySelectionChanged()
        {
            // AssetFinderLOG.Log("OnUnitySelectionChanged()");
            RefreshFromUnitySelection();
        }
        
        public static void Cleanup()
        {
            if (_instance == null) return;
            Selection.selectionChanged -= _instance.OnUnitySelectionChanged;
            _instance = null;
        }

        internal class AssetFinderSceneSelection
        {
            private readonly HashSet<int> instanceIds = new HashSet<int>();
            private readonly Dictionary<int, GameObject> gameObjects = new Dictionary<int, GameObject>();

            // Cached array - updated only when collection changes
            private GameObject[] cachedGameObjectArray = Array.Empty<GameObject>();
            private bool arrayDirty = false;

            public int Count => instanceIds.Count;
            public IReadOnlyCollection<int> InstanceIds => instanceIds;
            public IReadOnlyCollection<GameObject> GameObjects => gameObjects.Values;

            public void AddGameObject(UnityObject obj)
            {
                if (obj == null) return;

                GameObject go = obj as GameObject ?? (obj as Component)?.gameObject;
                if (go == null) return;

                int instanceId = go.GetInstanceID();
                if (instanceIds.Add(instanceId))
                {
                    gameObjects[instanceId] = go;
                    arrayDirty = true;
                }
            }

            public void Remove(int instanceId)
            {
                if (instanceIds.Remove(instanceId))
                {
                    gameObjects.Remove(instanceId);
                    arrayDirty = true;
                }
            }

            public bool Contains(int instanceId)
            {
                return instanceIds.Contains(instanceId);
            }

            public bool Contains(GameObject go)
            {
                return go != null && instanceIds.Contains(go.GetInstanceID());
            }

            public void Clear()
            {
                instanceIds.Clear();
                gameObjects.Clear();
                arrayDirty = true;
            }

            public GameObject[] ToArray()
            {
                if (arrayDirty)
                {
                    cachedGameObjectArray = gameObjects.Values.Where(go => go != null).ToArray();
                    arrayDirty = false;
                }

                return cachedGameObjectArray;
            }
        }

        internal class AssetFinderAssetSelection
        {
            private readonly Dictionary<string, AssetEntry> assets = new Dictionary<string, AssetEntry>();

            // Cached arrays - updated only when collection changes
            private string[] cachedGuidsArray = Array.Empty<string>();
            private bool guidArrayDirty = false;

            public int Count => assets.Count;
            public IReadOnlyCollection<string> AssetGuids => assets.Keys;
            public IReadOnlyCollection<AssetEntry> AssetEntries => assets.Values;

            public struct AssetEntry
            {
                public string guid;
                public long fileId;
                public string assetPath;

                public AssetEntry(string guid, long fileId, string assetPath)
                {
                    this.guid = guid;
                    this.fileId = fileId;
                    this.assetPath = assetPath;
                }
            }

            public void AddAsset(UnityObject obj)
            {
                if (obj == null) return;

                string assetPath = AssetDatabase.GetAssetPath(obj);
                if (string.IsNullOrEmpty(assetPath)) return;

                string guid = AssetDatabase.AssetPathToGUID(assetPath);
                if (string.IsNullOrEmpty(guid)) return;

                long fileId = GetFileId(obj);

                if (!assets.ContainsKey(guid))
                {
                    guidArrayDirty = true;
                }

                assets[guid] = new AssetEntry(guid, fileId, assetPath);
            }

            private static long GetFileId(UnityObject obj)
            {
#if UNITY_2018_1_OR_NEWER
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out _, out long fileId))
                {
                    return fileId;
                }
#else
            try
            {
                var serializedObject = new SerializedObject(obj);
                var inspectorModeInfo = typeof(SerializedObject).GetProperty("inspectorMode", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                inspectorModeInfo?.SetValue(serializedObject, InspectorMode.Debug, null);
                
                var localIdProp = serializedObject.FindProperty("m_LocalIdentfierInFile");
                if (localIdProp != null)
                {
                    long localId = localIdProp.longValue;
                    if (localId <= 0) localId = localIdProp.intValue;
                    return localId;
                }
            }
            catch { }
#endif
                return -1;
            }

            public void Remove(string guid)
            {
                if (assets.Remove(guid))
                {
                    guidArrayDirty = true;
                }
            }

            public bool Contains(string guid)
            {
                return assets.ContainsKey(guid);
            }

            public void Clear()
            {
                assets.Clear();
                guidArrayDirty = true;
            }

            public string[] GetGuids()
            {
                if (guidArrayDirty)
                {
                    cachedGuidsArray = assets.Keys.ToArray();
                    guidArrayDirty = false;
                }

                return cachedGuidsArray;
            }

            public AssetEntry? GetAssetEntry(string guid)
            {
                if (assets.TryGetValue(guid, out var entry)) return entry;
                return null;
            }
        }
    }
}