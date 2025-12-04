using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderSelection : IRefDraw
    {
        internal readonly AssetFinderRefDrawer drawer;
        internal readonly HashSet<string> guidSet = new HashSet<string>();
        internal readonly HashSet<string> instSet = new HashSet<string>(); // Do not reference directly to SceneObject (which might be destroyed anytime)

        // ------------ instance

        private bool dirty;
        internal bool isLock;
        internal Dictionary<string, AssetFinderRef> refs;

        public AssetFinderSelection(IWindow window, Func<AssetFinderRefDrawer.Sort> getSortMode, Func<AssetFinderRefDrawer.Mode> getGroupMode)
        {
            this.window = window;
            drawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = window,
                getSortMode = getSortMode,
                getGroupMode = getGroupMode,
                showFullPath = false,
                showFileSize = false,
                showExtension = true,
                showUsageType = false,
                showAssetBundleName = false,
                showAtlasName = false,
                showToggle = false,
                shouldShowExtension = () => true, // Selection panel always shows extensions
                shouldShowDetailButton = () => false, // Selection panel never shows detail buttons
                onCacheInvalidated = () => { } // Selection panel doesn't need cache invalidation
            })
            {
                groupDrawer = { hideGroupIfPossible = true },
                level0Group = string.Empty,
                paddingLeft = -16f
            };

            dirty = true;
            drawer.SetDirty();
        }

        public int Count => guidSet.Count + instSet.Count;

        public bool isSelectingAsset => guidSet.Count > 0 && instSet.Count == 0;
        public bool isSelectingSceneObject => instSet.Count > 0 && guidSet.Count == 0;

        public IWindow window { get; set; }

        public int ElementCount()
        {
            return refs?.Count ?? 0;
        }

        public bool DrawLayout()
        {
            if (dirty) RefreshView();
            return drawer.DrawLayout();
        }

        public bool Draw(Rect rect)
        {
            if (dirty) RefreshView();
            if (refs == null) return false;
            rect.yMax -= 16f;
            return drawer.Draw(rect);
        }

        public void Add(UnityObject sceneObject)
        {
            if (sceneObject == null) return;
            var id = sceneObject.GetInstanceID().ToString();
            instSet.Add(id);
            dirty = true;
        }

        public void Add(string guid)
        {
            if (guidSet.Contains(guid)) return;
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(assetPath))
            {
                AssetFinderLOG.LogWarning("Invalid GUID: " + guid);
                return;
            }

            guidSet.Add(guid);
            dirty = true;
        }

        public void AddRange(params string[] guids)
        {
            foreach (string id in guids)
            {
                Add(id);
            }
            dirty = true;
        }

        public void Remove(UnityObject sceneObject)
        {
            if (sceneObject == null) return;
            var id = sceneObject.GetInstanceID().ToString();
            instSet.Remove(id);
            dirty = true;
        }

        public void Remove(string guidOrInstID)
        {
            guidSet.Remove(guidOrInstID);
            instSet.Remove(guidOrInstID);

            dirty = true;
        }

        public void Clear()
        {
            guidSet.Clear();
            instSet.Clear();
            dirty = true;
        }

        public void Add(AssetFinderRef rf)
        {
            if (rf.isSceneRef)
            {
                Add(rf.component);
            } else
            {
                Add(rf.asset.guid);
            }
        }

        public void Remove(AssetFinderRef rf)
        {
            if (rf.isSceneRef)
            {
                Remove(rf.component);
            } else
            {
                Remove(rf.asset.guid);
            }
        }

        public void SetDirty()
        {
            drawer.SetDirty();
        }

        public event Action OnSelectionChanged;

        public void SyncFromGlobalSelection()
        {
            var manager = AssetFinderSelectionManager.Instance;
            
            Clear();
            
            // Copy from global selection to local selection
            if (manager.IsSelectingSceneObjects)
            {
                foreach (var go in manager.SceneSelection.GameObjects)
                {
                    if (go != null) Add(go);
                }
            }
            else if (manager.IsSelectingAssets)
            {
                foreach (var entry in manager.AssetSelection.AssetEntries)
                {
                    Add(entry.guid);
                }
            }
            
            dirty = true;
        }

        public UnityObject[] GetUnityObjects()
        {
            var result = new System.Collections.Generic.List<UnityObject>();
            
            // Add scene objects from local selection
            foreach (string instIdStr in instSet)
            {
                if (int.TryParse(instIdStr, out int instId))
                {
                    var obj = EditorUtility.InstanceIDToObject(instId);
                    if (obj != null) result.Add(obj);
                }
            }
            
            // Add asset objects from local selection
            foreach (string guid in guidSet)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                if (!string.IsNullOrEmpty(assetPath))
                {
                    var obj = AssetDatabase.LoadAssetAtPath<UnityObject>(assetPath);
                    if (obj != null) result.Add(obj);
                }
            }
            
            return result.ToArray();
        }

        public void SetUnityObjects(UnityObject[] objects)
        {
            Clear();
            
            if (objects != null)
            {
                foreach (var obj in objects)
                {
                    if (obj == null) continue;
                    
                    if (obj.IsSceneObject())
                    {
                        Add(obj);
                    }
                    else
                    {
                        string assetPath = AssetDatabase.GetAssetPath(obj);
                        if (!string.IsNullOrEmpty(assetPath))
                        {
                            string guid = AssetDatabase.AssetPathToGUID(assetPath);
                            if (!string.IsNullOrEmpty(guid))
                            {
                                Add(guid);
                            }
                        }
                    }
                }
            }
            
            dirty = true;
            // Trigger refresh after manually setting selection
            OnSelectionChanged?.Invoke();
        }
        public void RefreshView()
        {
            if (refs == null) refs = new Dictionary<string, AssetFinderRef>();
            refs.Clear();

            if (instSet.Count > 0)
            {
                foreach (string instId in instSet)
                {
                    refs.Add(instId, new AssetFinderSceneRef(0, EditorUtility.InstanceIDToObject(int.Parse(instId))));
                }
            } else
            {
                foreach (string guid in guidSet)
                {
                    AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                    refs.Add(guid, new AssetFinderRef(0, 0, asset, null)
                    {
                        isSceneRef = false
                    });
                }
            }

            drawer.SetRefs(refs);
            dirty = false;
        }
    }
}
