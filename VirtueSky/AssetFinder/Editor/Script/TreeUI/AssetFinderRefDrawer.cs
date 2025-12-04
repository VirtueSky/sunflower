using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;
namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderRefDrawer : IRefDraw
    {
        internal class RefDrawerConfig
        {
            public IWindow window;
            public Func<Sort> getSortMode;
            public Func<Mode> getGroupMode;
            public bool showFullPath;
            public bool showToggle = true;
            public bool showHighlight = true;
            public Func<bool> shouldShowExtension;
            public Func<bool> shouldShowDetailButton;
            public Action onCacheInvalidated;
        }

        internal class AssetDrawingConfig : RefDrawerConfig
        {
            public bool showFileSize;
            public bool showExtension;
            public bool showUsageType;
            public bool showAssetBundleName;
            public bool showAtlasName;
        }

        internal class SceneDrawingConfig : RefDrawerConfig
        {
            public bool showDetails;
        }

        public static GUIStyle toolbarSearchField;
        public static GUIStyle toolbarSearchFieldCancelButton;
        public static GUIStyle toolbarSearchFieldCancelButtonEmpty;
        private readonly Dictionary<string, BookmarkInfo> gBookmarkCache = new Dictionary<string, BookmarkInfo>();
        private readonly Func<Mode> getGroupMode;

        private readonly Func<Sort> getSortMode;

        internal readonly AssetFinderTreeUI2.GroupDrawer groupDrawer;

        public readonly List<AssetFinderAsset> highlight = new List<AssetFinderAsset>();

        // FILTERING
        private readonly string searchTerm = string.Empty;
        private readonly bool showSearch = true;
        public Action<Rect, AssetFinderRef> afterItemDraw;
        public Action<Rect, AssetFinderRef> beforeItemDraw;
        public bool caseSensitive = false;

        public Action<Rect, string, int> customDrawGroupLabel;

        public Func<AssetFinderRef, string> customGetGroup;

        // STATUS
        private bool dirty;
        public RefDrawerConfig Config { get; private set; }
        private int excludeCount;
        public AssetDrawingConfig AssetConfig { get; private set; }
        public SceneDrawingConfig SceneConfig { get; private set; }

        public string level0Group;
        internal List<AssetFinderRef> list;
        public string messageEmpty = "It's empty!";

        public string messageNoRefs = "Do select something!";
        internal Dictionary<string, AssetFinderRef> refs;
        private bool selectFilter;
        public bool showDetail;
        private bool showIgnore;
        public float paddingLeft = -4f;
        public float paddingRight = 0f;
        
        // Track whether we have a valid selection (independent of refs count)
        private bool hasValidSelection = false;
        
        // Hook for getting contextual empty message from external source (e.g. window)
        public Func<string> GetContextualEmptyMessage;


        public AssetFinderRefDrawer(RefDrawerConfig config)
        {
            this.window = config.window;
            this.getSortMode = config.getSortMode;
            this.getGroupMode = config.getGroupMode;
            this.Config = config;
            this.AssetConfig = config as AssetDrawingConfig ?? new AssetDrawingConfig();
            this.SceneConfig = config as SceneDrawingConfig ?? new SceneDrawingConfig();
            groupDrawer = new AssetFinderTreeUI2.GroupDrawer(DrawGroup, DrawAsset);
        }



        // ORIGINAL
        internal AssetFinderRef[] source => AssetFinderRef.FromList(list);

        public IWindow window { get; set; }
        
        public bool IsDirty => dirty;
        public bool Draw(Rect rect)
        {
            if (refs == null || refs.Count == 0)
            {
                // Show appropriate message based on whether we have a valid selection
                string message = hasValidSelection ? (GetContextualEmptyMessage?.Invoke() ?? messageEmpty) : messageNoRefs;
                DrawEmpty(rect, message);
                return false;
            }

            if (dirty || list == null) ApplyFilter();

            rect.xMin += paddingLeft;
            rect.xMax += paddingRight;

            if (!groupDrawer.hasChildren)
            {
                DrawEmpty(rect, GetContextualEmptyMessage?.Invoke() ?? messageEmpty);
            } else
            {
                groupDrawer.Draw(rect);
            }
            return false;
        }

        public bool DrawLayout()
        {
            if (refs == null || refs.Count == 0) 
            {
                // Only return false if we have no valid selection, otherwise continue to show empty message
                return !hasValidSelection;
            }

            if (dirty || list == null) ApplyFilter();

            if (groupDrawer.tree != null)
            {
                groupDrawer.tree.itemPaddingLeft = paddingLeft;
                groupDrawer.tree.itemPaddingRight = paddingRight;
            }
            groupDrawer.DrawLayout();
            return false;
        }

        public int ElementCount()
        {
            if (refs == null) return 0;

            return refs.Count;

            // return refs.Where(x => x.Value.depth != 0).Count();
        }

        private void DrawEmpty(Rect rect, string text)
        {
            rect = GUI2.Padding(rect, 2f, 2f);
            rect.height = 45f;

            // Determine message type based on message content
            MessageType messageType = MessageType.Info;
            if (text.Contains("not scanned") || text.Contains("content changed") || text.Contains("refresh cache"))
            {
                messageType = MessageType.Warning;
            }

            EditorGUI.HelpBox(rect, text, messageType);
        }
        public void SetRefs(Dictionary<string, AssetFinderRef> dictRefs)
        {
            ValidateRefs(dictRefs);
            refs = dictRefs;
            SetupRefCallbacks();
            
            // When setting refs directly, we consider this a valid selection
            hasValidSelection = true;
            dirty = true;
        }
        
        private void SetupRefCallbacks()
        {
            // Setup callbacks for each ref to point to this drawer
            if (refs != null)
            {
                foreach (var kvp in refs)
                {
                    var rf = kvp.Value;
                    rf.OnCtrlClick = HandleRefCtrlClick;
                    rf.OnAltClick = HandleRefAltClick;
                    rf.OnShiftClick = HandleRefShiftClick;
                }
            }
        }

        void ValidateRefs(Dictionary<string, AssetFinderRef> dictRefs)
        {
            var sceneRef = 0;
            var assetRef = 0;
            foreach (var kvp in dictRefs)
            {
                if (kvp.Value.isSceneRef) sceneRef++;
                if (!kvp.Value.isSceneRef) assetRef++;
                if (sceneRef > 0 && assetRef > 0)
                {
                    AssetFinderLOG.LogWarning("Mixed content???");
                }
            }
        }

        private void SetBookmarkGroup(string groupLabel, bool willbookmark)
        {
            string[] ids = groupDrawer.GetChildren(groupLabel);
            if (ids == null) return; // Handle case where group doesn't exist in groupDict
            
            for (var i = 0; i < ids.Length; i++)
            {
                AssetFinderRef rf;
                if (!refs.TryGetValue(ids[i], out rf)) continue;

                if (willbookmark)
                {
                    AssetFinderBookmark.Add(rf);
                } else
                {
                    AssetFinderBookmark.Remove(rf);
                }
            }

            // Invalidate cache so group toggles reflect the correct state
            InvalidateGroupCache();
        }

        private BookmarkInfo GetBMInfo(string groupLabel)
        {
            BookmarkInfo info = null;
            if (!gBookmarkCache.TryGetValue(groupLabel, out info))
            {
                string[] ids = groupDrawer.GetChildren(groupLabel);

                info = new BookmarkInfo();
                if (ids != null)
                {
                    for (var i = 0; i < ids.Length; i++)
                    {
                        AssetFinderRef rf;
                        if (!refs.TryGetValue(ids[i], out rf)) continue;
                        info.total++;

                        bool isBM = AssetFinderBookmark.Contains(rf);
                        if (isBM) info.count++;
                    }
                }

                gBookmarkCache.Add(groupLabel, info);
            }

            return info;
        }

        private void DrawToggleGroup(Rect r, string groupLabel)
        {
            BookmarkInfo info = GetBMInfo(groupLabel);
            bool selectAll = info.count == info.total;
            r.width = 16f;
            if (GUI2.Toggle(r, ref selectAll)) SetBookmarkGroup(groupLabel, selectAll);

            if (!selectAll && (info.count > 0))
            {
                //GUI.DrawTexture(r, EditorStyles.
            }
        }

        private void DrawGroup(Rect r, string label, int childCount)
        {
            if (string.IsNullOrEmpty(label)) label = "(none)";
            DrawToggleGroup(r, label);
            r.xMin += 18f;

            Mode groupMode = getGroupMode();
            if (groupMode == Mode.Folder)
            {
                Texture tex = AssetDatabase.GetCachedIcon("Assets");
                GUI.DrawTexture(new Rect(r.x, r.y, 16f, 16f), tex);
                r.xMin += 16f;
            }

            if (customDrawGroupLabel != null)
            {
                customDrawGroupLabel.Invoke(r, label, childCount);
            } else
            {
                GUIContent lbContent = AssetFinderGUIContent.FromString(label);
                GUI.Label(r, lbContent, EditorStyles.label);

                Rect cRect = r;
                cRect.x += EditorStyles.label.CalcSize(lbContent).x;
                cRect.y += 1f;
                GUI.Label(cRect, AssetFinderGUIContent.FromString($"({childCount})"), EditorStyles.miniLabel);
            }

            bool hasMouse = (Event.current.type == EventType.MouseUp) && r.Contains(Event.current.mousePosition);
            if (hasMouse && (Event.current.button == 1))
            {
                var menu = new GenericMenu();
                menu.AddItem(AssetFinderGUIContent.FromString("Add Bookmark"), false, () => { SetBookmarkGroup(label, true); });
                menu.AddItem(AssetFinderGUIContent.FromString("Remove Bookmark"), false, () =>
                {
                    SetBookmarkGroup(label, false);
                });

                menu.ShowAsContext();
                Event.current.Use();
            }
        }

        public void DrawDetails(Rect rect)
        {
            Rect r = rect;
            r.xMin += 18f;
            r.height = 18f;

            for (var i = 0; i < highlight.Count; i++)
            {
                highlight[i].Draw(
                    r,
                    new AssetFinderAsset.AssetFinderAssetDrawConfig(
                        false,
                    false,
                    false,
                    false,
                    false,
                    false,
                        window,
                        false
                    )
                );
                r.y += 18f;
                r.xMin += 18f;
            }
        }

        private void DrawAsset(Rect r, string guid)
        {
            if (!refs.TryGetValue(guid, out AssetFinderRef rf)) return;

            if (rf.isSceneRef)
            {
                if (rf.component == null) return;
                if (!(rf is AssetFinderSceneRef re)) return;
                beforeItemDraw?.Invoke(r, rf);

                if (Config.showToggle)
                {
                rf.DrawToogleSelect(r);
                r.xMin += 32f;
                }
                re.Draw(r, getGroupMode(), SceneConfig.showDetails, Config.showFullPath);
            } else
            {
                beforeItemDraw?.Invoke(r, rf);

                // Draw content first so right-side buttons handle mouse events before the bookmark toggle
                Rect contentRect = r;
                if (Config.showToggle) contentRect.xMin += 32f;

                bool isHighlight = Config.showHighlight && highlight.Contains(rf.asset);

                // if (isHighlight)
                // {
                //     var hlRect = new Rect(-20, r.y, 15f, r.height);
                //     GUI2.Rect(hlRect, GUI2.darkGreen);
                // }

                // Use configurable delegates for behavior
                bool shouldShowExtension = Config.shouldShowExtension?.Invoke() ?? AssetConfig.showExtension;
                bool shouldShowDetailBtn = Config.shouldShowDetailButton?.Invoke() ?? true;
                Action onShowDetail = () => {
                        showDetail = true;
                        highlight.Clear();
                        highlight.Add(rf.asset);

                        AssetFinderAsset p = rf.addBy;
                        var cnt = 0;
                        while ((p != null) && refs.ContainsKey(p.guid))
                        {
                            highlight.Add(p);
                            AssetFinderRef AssetFinderref = refs[p.guid];
                            if (AssetFinderref != null) p = AssetFinderref.addBy;
                            if (++cnt > 100)
                            {
                                AssetFinderLOG.LogWarning("Break on depth 1000????");
                                break;
                            }
                        }

                        highlight.Sort((item1, item2) =>
                        {
                            int d1 = refs[item1.guid].depth;
                            int d2 = refs[item2.guid].depth;
                            return d1.CompareTo(d2);
                        });
                        Event.current.Use();
                    };
                
                rf.asset.Draw(
                    contentRect,
                    new AssetFinderAsset.AssetFinderAssetDrawConfig(
                        isHighlight,
                        Config.showFullPath,
                        AssetConfig.showFileSize,
                        AssetConfig.showAssetBundleName && AssetFinderSetting.s.displayAssetBundleName,
                        AssetConfig.showAtlasName && AssetFinderSetting.s.displayAtlasName,
                        AssetConfig.showUsageType,
                        window,
                        shouldShowExtension,
                        shouldShowDetailBtn ? onShowDetail : null
                    )
                );

                if (Config.showToggle)
                {
                    rf.DrawToogleSelect(r);
                }
            }

            afterItemDraw?.Invoke(r, rf);
        }

        private string GetGroup(AssetFinderRef rf)
        {
            if (customGetGroup != null) return customGetGroup(rf);

            if (rf.depth == 0) return level0Group;

            if (getGroupMode() == Mode.None) return "(no group)";

            AssetFinderSceneRef sr = null;
            if (rf.isSceneRef)
            {
                sr = rf as AssetFinderSceneRef;
                if (sr == null) return null;
            }

            if (!rf.isSceneRef)
            {
                if (rf.asset.IsExcluded)
                {
                    return null; // "(ignored)"
                }
            }

            switch (getGroupMode())
            {
            case Mode.Extension:
                {
                    // if (!rf.isSceneRef) Debug.Log($"Extension: {rf.asset.assetPath} | {rf.asset.extension}");
                    return rf.isSceneRef ? sr.targetType
                        : string.IsNullOrEmpty(rf.asset.extension) ? "(no extension)" : rf.asset.extension;
                }
            case Mode.Type:
                {
                    return rf.isSceneRef ? sr.targetType : AssetFinderAssetGroupDrawer.FILTERS[rf.type].name;
                }

            case Mode.Folder: return rf.isSceneRef ? sr.scenePath : rf.asset.assetFolder;

            case Mode.Dependency:
                {
                    return rf.depth == 1 ? "Direct Usage" : "Indirect Usage";
                }

            case Mode.Depth:
                {
                    return "Level " + rf.depth;
                }

            case Mode.Atlas: return rf.isSceneRef ? "(not in atlas)" : string.IsNullOrEmpty(rf.asset.AtlasName) ? "(not in atlas)" : rf.asset.AtlasName;
            case Mode.AssetBundle: return rf.isSceneRef ? "(not in assetbundle)" : string.IsNullOrEmpty(rf.asset.AssetBundleName) ? "(not in assetbundle)" : rf.asset.AssetBundleName;
            }

            return "(others)";
        }

        private void SortGroup(List<string> groups)
        {
            groups.Sort((item1, item2) =>
            {
                if (item1.Contains("(")) return 1;
                if (item2.Contains("(")) return -1;

                return string.Compare(item1, item2, StringComparison.Ordinal);
            });
        }

        public AssetFinderRefDrawer Reset(string[] assetGUIDs, bool isUsage) //, bool isSceneRef
        {
            gBookmarkCache.Clear();
            
            // Set hasValidSelection based on whether we received valid asset GUIDs
            hasValidSelection = assetGUIDs != null && assetGUIDs.Length > 0;
            refs = isUsage ? AssetFinderRef.FindUsage(assetGUIDs) : AssetFinderRef.FindUsedBy(assetGUIDs);
            SetupRefCallbacks();
            
            // ValidateRefs(dictRefs);
            
            dirty = true;
            if (list != null) list.Clear();
            return this;
        }

        public void Reset(Dictionary<string, AssetFinderRef> newRefs)
        {
            if (refs == null) refs = new Dictionary<string, AssetFinderRef>();
            refs.Clear();
            ValidateRefs(newRefs);
            
            foreach (KeyValuePair<string, AssetFinderRef> kvp in newRefs)
            {
                refs.Add(kvp.Key, kvp.Value);
            }
            SetupRefCallbacks();
            
            // When resetting with refs directly, we consider this a valid selection
            hasValidSelection = true;
            
            dirty = true;
            if (list != null) list.Clear();
        }

        public AssetFinderRefDrawer Reset(GameObject[] objs, bool findDept, bool findPrefabInAsset)
        {
            // Set hasValidSelection based on whether we received valid GameObjects
            hasValidSelection = objs != null && objs.Length > 0;
            refs = AssetFinderRef.FindUsageScene(objs, findDept)
                .Where(item=> !item.Value.isSceneRef)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var guidss = new List<string>();
            Dictionary<GameObject, HashSet<string>> dependent = AssetFinderSceneCache.Api.prefabDependencies;
            foreach (GameObject gameObject in objs)
            {
                if (!dependent.TryGetValue(gameObject, out HashSet<string> hash)) continue;
                foreach (string guid in hash)
                {
                    guidss.Add(guid);
                }
            }

            Dictionary<string, AssetFinderRef> usageRefs1 = AssetFinderRef.FindUsage(guidss.ToArray());
            foreach (KeyValuePair<string, AssetFinderRef> kvp in usageRefs1)
            {
                if (refs.ContainsKey(kvp.Key)) continue;

                if (guidss.Contains(kvp.Key)) kvp.Value.depth = 1;

                refs.Add(kvp.Key, kvp.Value);
            }


            if (findPrefabInAsset)
            {
                var guids = new List<string>();
                for (var i = 0; i < objs.Length; i++)
                {
                    string guid = AssetFinderUnity.GetPrefabParent(objs[i]);
                    if (string.IsNullOrEmpty(guid)) continue;

                    guids.Add(guid);
                }

                Dictionary<string, AssetFinderRef> usageRefs = AssetFinderRef.FindUsage(guids.ToArray());
                foreach (KeyValuePair<string, AssetFinderRef> kvp in usageRefs)
                {
                    if (refs.ContainsKey(kvp.Key)) continue;

                    if (guids.Contains(kvp.Key)) kvp.Value.depth = 1;

                    refs.Add(kvp.Key, kvp.Value);
                }
            }

            SetupRefCallbacks();
            dirty = true;
            if (list != null) list.Clear();

            return this;
        }

        //ref in scene
        public AssetFinderRefDrawer Reset(string[] assetGUIDs)
        {
            // Set hasValidSelection based on whether we received valid asset GUIDs
            hasValidSelection = assetGUIDs != null && assetGUIDs.Length > 0;
            
            refs = AssetFinderSceneRef.FindRefInScene(assetGUIDs, true, SetRefInScene);
            SetupRefCallbacks();
            dirty = true;
            if (list != null) list.Clear();

            return this;
        }

        private void SetRefInScene(Dictionary<string, AssetFinderRef> data)
        {
            refs = data;
            SetupRefCallbacks();
            dirty = true;
            if (list != null) list.Clear();
        }

        //scene in scene
        public AssetFinderRefDrawer ResetSceneInScene(GameObject[] objs)
        {
            // Set hasValidSelection based on whether we received valid GameObjects
            hasValidSelection = objs != null && objs.Length > 0;
            
            refs = AssetFinderSceneRef.FindSceneInScene(objs);
            SetupRefCallbacks();
            dirty = true;
            if (list != null) list.Clear();

            return this;
        }

        public AssetFinderRefDrawer ResetSceneUseSceneObjects(GameObject[] objs)
        {
            // Set hasValidSelection based on whether we received valid GameObjects
            hasValidSelection = objs != null && objs.Length > 0;
            
            refs = AssetFinderSceneRef.FindSceneUseSceneObjects(objs);
            SetupRefCallbacks();
            dirty = true;
            if (list != null) list.Clear();

            return this;
        }

        public AssetFinderRefDrawer ResetUnusedAsset(bool recursive = true)
        {
            List<AssetFinderAsset> lst = AssetFinderCache.Api.ScanUnused(recursive);

            refs = lst.ToDictionary(x => x.guid, x => new AssetFinderRef(0, 1, x, null));
            SetupRefCallbacks();
            
            // For unused assets, we always consider this a valid operation
            hasValidSelection = true;
            
            dirty = true;
            if (list != null) list.Clear();

            return this;
        }

        public void RefreshSort()
        {
            if (list == null) return;
            list.RemoveAll(item => item == null ||
                 (item.isSceneRef 
                     ? (item.component == null)
                     : (item.asset == null)
                 ));
            
            if (list.Count == 0) return;
            list.Sort((r1, r2) =>
            {
                bool isMixed = r1.isSceneRef != r2.isSceneRef;
                if (isMixed)
                {
#if AssetFinderDEBUG
					var sb = new System.Text.StringBuilder();
					sb.Append("r1: " + r1.ToString());
					sb.AppendLine();
					sb.Append("r2: " +r2.ToString());
					AssetFinderLOG.LogWarning($"Mixed compared!\n{sb}");
#endif

                    int v1 = r1.isSceneRef ? 1 : 0;
                    int v2 = r2.isSceneRef ? 1 : 0;
                    return v2.CompareTo(v1);
                }

                if (r1.isSceneRef)
                {
                    var rs1 = (AssetFinderSceneRef)r1;
                    var rs2 = (AssetFinderSceneRef)r2;

                    return SortAsset(rs1.sceneFullPath, rs2.sceneFullPath,
                        rs1.targetType, rs2.targetType,
                        getSortMode() == Sort.Path);
                }

                if (r1.asset == null) return -1;
                if (r2.asset == null) return 1;

                return SortAsset(
                    r1.asset.assetPath, r2.asset.assetPath,
                    r1.asset.extension, r2.asset.extension,
                    false
                );
            });
            
            groupDrawer.Reset(list,
                rf =>
                {
                    if (rf == null) return null;
                    return rf.isSceneRef ? rf.GetSceneObjId() : rf.asset?.guid;
                }, GetGroup, SortGroup);
        }

        public bool isExclueAnyItem()
        {
            return excludeCount > 0;
        }

        private void ApplyFilter()
        {
            dirty = false;

            if (refs == null) return;

            if (list == null)
            {
                list = new List<AssetFinderRef>();
            } else
            {
                list.Clear();
            }

            int minScore = searchTerm.Length;

            string term1 = searchTerm;
            if (!caseSensitive) term1 = term1.ToLower();

            string term2 = term1.Replace(" ", string.Empty);

            excludeCount = 0;

            foreach (KeyValuePair<string, AssetFinderRef> item in refs)
            {
                AssetFinderRef r = item.Value;

                if (AssetFinderSetting.IsTypeExcluded(r.type))
                {
                    excludeCount++;
                    continue; //skip this one
                }

                if (!showSearch || string.IsNullOrEmpty(searchTerm))
                {
                    r.matchingScore = 0;
                    list.Add(r);
                    continue;
                }

                //calculate matching score
                string name1 = r.isSceneRef ? (r as AssetFinderSceneRef)?.sceneFullPath : r.asset.assetName;
                if (!caseSensitive) name1 = name1?.ToLower();

                string name2 = name1?.Replace(" ", string.Empty);

                int score1 = AssetFinderUnity.StringMatch(term1, name1);
                int score2 = AssetFinderUnity.StringMatch(term2, name2);

                r.matchingScore = Mathf.Max(score1, score2);
                if (r.matchingScore > minScore) list.Add(r);
            }

            RefreshSort();
        }

        public void SetDirty()
        {
            dirty = true;
        }

        
        public void InvalidateGroupCache()
        {
            gBookmarkCache.Clear();
            Config.onCacheInvalidated?.Invoke();
        }
        
        public string GetGroupForRef(AssetFinderRef rf)
        {
            return GetGroup(rf);
        }
        
        public void ToggleAllItems()
        {
            if (refs == null) return;
            
            // Determine the new state based on majority rule
            int bookmarkedCount = 0;
            int totalCount = 0;
            
            foreach (var kvp in refs)
            {
                totalCount++;
                if (AssetFinderBookmark.Contains(kvp.Value))
                {
                    bookmarkedCount++;
                }
            }
            
            bool newState = bookmarkedCount < totalCount / 2; // If less than half are bookmarked, bookmark all
            
            foreach (var kvp in refs)
            {
                if (newState)
                {
                    AssetFinderBookmark.Add(kvp.Value);
                }
                else
                {
                    AssetFinderBookmark.Remove(kvp.Value);
                }
            }
            
            InvalidateGroupCache();
        }
        
        public void ToggleGroupItems(string groupLabel)
        {
            string[] ids = groupDrawer.GetChildren(groupLabel);
            if (ids == null) return;
            
            // Toggle each sibling individually - if it's on, turn it off; if it's off, turn it on
            for (var i = 0; i < ids.Length; i++)
            {
                if (!refs.TryGetValue(ids[i], out AssetFinderRef rf)) continue;
                
                if (AssetFinderBookmark.Contains(rf))
                {
                    AssetFinderBookmark.Remove(rf);
                }
                else
                {
                    AssetFinderBookmark.Add(rf);
                }
            }
            
            InvalidateGroupCache();
        }
        
        public void SetGroupItemsState(string groupLabel, bool willBookmark)
        {
            string[] ids = groupDrawer.GetChildren(groupLabel);
            if (ids == null) return;
            
            for (var i = 0; i < ids.Length; i++)
            {
                if (!refs.TryGetValue(ids[i], out AssetFinderRef rf)) continue;
                
                if (willBookmark)
                {
                    AssetFinderBookmark.Add(rf);
                }
                else
                {
                    AssetFinderBookmark.Remove(rf);
                }
            }
        }

        
        private void HandleRefCtrlClick(AssetFinderRef rf)
        {
            // Toggle self and set all siblings to the same new state
            bool newState = !rf.isSelected();
            if (newState)
            {
                AssetFinderBookmark.Add(rf);
            } else
            {
                AssetFinderBookmark.Remove(rf);
            }
            
            // Then set all siblings to the same state
            string groupName = GetGroupForRef(rf);
            if (!string.IsNullOrEmpty(groupName) && groupDrawer.GetChildren(groupName) != null)
            {
                SetGroupItemsState(groupName, newState);
            }
            
            InvalidateGroupCache();
        }
        
        private void HandleRefAltClick(AssetFinderRef rf)
        {
            // Toggle all siblings in the same group
            string groupName = GetGroupForRef(rf);
            if (!string.IsNullOrEmpty(groupName) && groupDrawer.GetChildren(groupName) != null)
            {
                ToggleGroupItems(groupName);
            }
        }
        
        private void HandleRefShiftClick(AssetFinderRef rf)
        {
            // Toggle all items in all groups
            ToggleAllItems();
        }
        
        public void ClearSelection()
        {
            hasValidSelection = false;
            refs = null;
            if (list != null) list.Clear();
            dirty = true;
        }

        private int SortAsset(string term11, string term12, string term21, string term22, bool swap)
        {
            //			if (term11 == null) term11 = string.Empty;
            //			if (term12 == null) term12 = string.Empty;
            //			if (term21 == null) term21 = string.Empty;
            //			if (term22 == null) term22 = string.Empty;
            int v1 = string.Compare(term11, term12, StringComparison.Ordinal);
            int v2 = string.Compare(term21, term22, StringComparison.Ordinal);
            return swap ? v1 == 0 ? v2 : v1 : v2 == 0 ? v1 : v2;
        }

        public Dictionary<string, AssetFinderRef> getRefs()
        {
            return refs;
        }

        internal class BookmarkInfo
        {
            public int count;
            public int total;
        }
    }
}
