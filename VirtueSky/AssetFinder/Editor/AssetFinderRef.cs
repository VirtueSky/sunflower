using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    public class AssetFinderSceneRef : AssetFinderRef
    {
        internal static Dictionary<string, Type> CacheType = new Dictionary<string, Type>();


        // ------------------------- Ref in scene
        private static Action<Dictionary<string, AssetFinderRef>> onFindRefInSceneComplete;

        private static Dictionary<string, AssetFinderRef> refs =
            new Dictionary<string, AssetFinderRef>();

        private static string[] cacheAssetGuids;
        public string sceneFullPath = "";
        public string scenePath = "";
        public string targetType;
        public HashSet<string> usingType = new HashSet<string>();

        public AssetFinderSceneRef(int index, int depth, AssetFinderAsset asset,
            AssetFinderAsset by) : base(index,
            depth, asset, by)
        {
            isSceneRef = false;
        }

//		public override string ToString()
//		{
//			return "SceneRef: " + sceneFullPath;
//		}

        public AssetFinderSceneRef(int depth, Object target) : base(0, depth, null, null)
        {
            component = target;
            this.depth = depth;
            isSceneRef = true;
            var obj = target as GameObject;
            if (obj == null)
            {
                var com = target as Component;
                if (com != null)
                {
                    obj = com.gameObject;
                }
            }

            scenePath = AssetFinderUnity.GetGameObjectPath(obj, false);
            if (component == null)
            {
                return;
            }

            sceneFullPath = scenePath + component.name;
            targetType = component.GetType().Name;
        }

        public static IWindow window { get; set; }

        public override bool isSelected()
        {
            return component == null ? false : AssetFinderBookmark.Contains(component);
        }

        public void Draw(Rect r, IWindow window, bool showDetails)
        {
            bool selected = isSelected();
            DrawToogleSelect(r);

            var margin = 2;
            var left = new Rect(r);
            left.width = r.width / 3f;

            var right = new Rect(r);
            right.xMin += left.width + margin;

            //Debug.Log("draw scene "+ selected);
            if ( /* AssetFinderSetting.PingRow && */ Event.current.type == EventType.MouseDown &&
                                                     Event.current.button == 0)
            {
                Rect pingRect = AssetFinderSetting.PingRow
                    ? new Rect(0, r.y, r.x + r.width, r.height)
                    : left;

                if (pingRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.control || Event.current.command)
                    {
                        if (selected)
                        {
                            AssetFinderBookmark.Remove(this);
                        }
                        else
                        {
                            AssetFinderBookmark.Add(this);
                        }

                        if (window != null) window.Repaint();
                    }
                    else
                    {
                        EditorGUIUtility.PingObject(component);
                    }

                    Event.current.Use();
                }
            }

            EditorGUI.ObjectField(showDetails ? left : r, GUIContent.none, component,
                typeof(GameObject), true);
            if (!showDetails) return;

            bool drawPath = AssetFinderSetting.GroupMode != AssetFinderRefDrawer.Mode.Folder;
            float pathW =
                drawPath ? EditorStyles.miniLabel.CalcSize(new GUIContent(scenePath)).x : 0;
            string assetName = component.name;
            // if(usingType!= null && usingType.Count > 0)
            // {
            // 	assetName += " -> ";
            // 	foreach(var item in usingType)
            // 	{
            // 		assetName += item + " - ";
            // 	}
            // 	assetName = assetName.Substring(0, assetName.Length - 3);
            // }
            Color cc = AssetFinderCache.Api.setting.SelectedColor;

            var lableRect = new Rect(
                right.x,
                right.y,
                pathW + EditorStyles.boldLabel.CalcSize(new GUIContent(assetName)).x,
                right.height);
            if (selected)
            {
                Color c = GUI.color;
                GUI.color = cc;
                GUI.DrawTexture(lableRect, EditorGUIUtility.whiteTexture);
                GUI.color = c;
            }

            if (drawPath)
            {
                GUI.Label(LeftRect(pathW, ref right), scenePath, EditorStyles.miniLabel);
                right.xMin -= 4f;
                GUI.Label(right, assetName, EditorStyles.boldLabel);
            }
            else
            {
                GUI.Label(right, assetName);
            }


            if (!AssetFinderSetting.ShowUsedByClassed || usingType == null)
            {
                return;
            }

            float sub = 10;
            var re = new Rect(r.x + r.width - sub, r.y, 20, r.height);
            Type t = null;
            foreach (string item in usingType)
            {
                string name = item;
                if (!CacheType.TryGetValue(item, out t))
                {
                    t = AssetFinderUnity.GetType(name);
                    // if (t == null)
                    // {
                    // 	continue;
                    // } 
                    CacheType.Add(item, t);
                }

                GUIContent content;
                var width = 0.0f;
                if (!AssetFinderAsset.cacheImage.TryGetValue(name, out content))
                {
                    if (t == null)
                    {
                        content = new GUIContent(name);
                    }
                    else
                    {
                        Texture text = EditorGUIUtility.ObjectContent(null, t).image;
                        if (text == null)
                        {
                            content = new GUIContent(name);
                        }
                        else
                        {
                            content = new GUIContent(text, name);
                        }
                    }


                    AssetFinderAsset.cacheImage.Add(name, content);
                }

                if (content.image == null)
                {
                    width = EditorStyles.label.CalcSize(content).x;
                }
                else
                {
                    width = 20;
                }

                re.x -= width;
                re.width = width;

                GUI.Label(re, content);
                re.x -= margin; // margin;
            }


            // var nameW = EditorStyles.boldLabel.CalcSize(new GUIContent(assetName)).x;
        }

        private Rect LeftRect(float w, ref Rect rect)
        {
            rect.x += w;
            rect.width -= w;
            return new Rect(rect.x - w, rect.y, w, rect.height);
        }

        // ------------------------- Scene use scene objects
        public static Dictionary<string, AssetFinderRef> FindSceneUseSceneObjects(
            GameObject[] targets)
        {
            var results = new Dictionary<string, AssetFinderRef>();
            GameObject[] objs = Selection.gameObjects;
            for (var i = 0; i < objs.Length; i++)
            {
                if (AssetFinderUnity.IsInAsset(objs[i]))
                {
                    continue;
                }

                string key = objs[i].GetInstanceID().ToString();
                if (!results.ContainsKey(key))
                {
                    results.Add(key, new AssetFinderSceneRef(0, objs[i]));
                }

                Component[] coms = objs[i].GetComponents<Component>();
                Dictionary<Component, HashSet<AssetFinderSceneCache.HashValue>> SceneCache =
                    AssetFinderSceneCache.Api.cache;
                for (var j = 0; j < coms.Length; j++)
                {
                    HashSet<AssetFinderSceneCache.HashValue> hash = null;
                    if (coms[j] == null) continue; // missing component

                    if (SceneCache.TryGetValue(coms[j], out hash))
                    {
                        foreach (AssetFinderSceneCache.HashValue item in hash)
                        {
                            if (item.isSceneObject)
                            {
                                Object obj = item.target;
                                string key1 = obj.GetInstanceID().ToString();
                                if (!results.ContainsKey(key1))
                                {
                                    results.Add(key1, new AssetFinderSceneRef(1, obj));
                                }
                            }
                        }
                    }
                }
            }

            return results;
        }

        // ------------------------- Scene in scene
        public static Dictionary<string, AssetFinderRef> FindSceneInScene(GameObject[] targets)
        {
            var results = new Dictionary<string, AssetFinderRef>();
            GameObject[] objs = Selection.gameObjects;
            for (var i = 0; i < objs.Length; i++)
            {
                if (AssetFinderUnity.IsInAsset(objs[i]))
                {
                    continue;
                }

                string key = objs[i].GetInstanceID().ToString();
                if (!results.ContainsKey(key))
                {
                    results.Add(key, new AssetFinderSceneRef(0, objs[i]));
                }


                foreach (KeyValuePair<Component, HashSet<AssetFinderSceneCache.HashValue>> item in
                         AssetFinderSceneCache.Api.cache)
                foreach (AssetFinderSceneCache.HashValue item1 in item.Value)
                {
                    // if(item.Key.gameObject.name == "ScenesManager")
                    // Debug.Log(item1.objectReferenceValue);
                    GameObject ob = null;
                    if (item1.target is GameObject)
                    {
                        ob = item1.target as GameObject;
                    }
                    else
                    {
                        var com = item1.target as Component;
                        if (com == null)
                        {
                            continue;
                        }

                        ob = com.gameObject;
                    }

                    if (ob == null)
                    {
                        continue;
                    }

                    if (ob != objs[i])
                    {
                        continue;
                    }

                    key = item.Key.GetInstanceID().ToString();
                    if (!results.ContainsKey(key))
                    {
                        results.Add(key, new AssetFinderSceneRef(1, item.Key));
                    }

                    (results[key] as AssetFinderSceneRef).usingType.Add(item1.target.GetType()
                        .FullName);
                }
            }

            return results;
        }

        public static Dictionary<string, AssetFinderRef> FindRefInScene(string[] assetGUIDs,
            bool depth,
            Action<Dictionary<string, AssetFinderRef>> onComplete, IWindow win)
        {
            // var watch = new System.Diagnostics.Stopwatch();
            // watch.Start();
            window = win;
            cacheAssetGuids = assetGUIDs;
            onFindRefInSceneComplete = onComplete;
            if (AssetFinderSceneCache.ready)
            {
                FindRefInScene();
            }
            else
            {
                AssetFinderSceneCache.onReady -= FindRefInScene;
                AssetFinderSceneCache.onReady += FindRefInScene;
            }

            return refs;
        }

        private static void FindRefInScene()
        {
            refs = new Dictionary<string, AssetFinderRef>();
            for (var i = 0; i < cacheAssetGuids.Length; i++)
            {
                AssetFinderAsset asset = AssetFinderCache.Api.Get(cacheAssetGuids[i]);
                if (asset == null)
                {
                    continue;
                }

                Add(refs, asset, 0);

                ApplyFilter(refs, asset);
            }

            if (onFindRefInSceneComplete != null)
            {
                onFindRefInSceneComplete(refs);
            }

            AssetFinderSceneCache.onReady -= FindRefInScene;
            //    UnityEngine.Debug.Log("Time find ref in scene " + watch.ElapsedMilliseconds);
        }

        private static void FilterAll(Dictionary<string, AssetFinderRef> refs, Object obj,
            string targetPath)
        {
            // ApplyFilter(refs, obj, targetPath);
        }

        private static void ApplyFilter(Dictionary<string, AssetFinderRef> refs,
            AssetFinderAsset asset)
        {
            string targetPath = AssetDatabase.GUIDToAssetPath(asset.guid);
            if (string.IsNullOrEmpty(targetPath))
            {
                return; // asset not found - might be deleted!
            }

            //asset being moved!
            if (targetPath != asset.assetPath)
            {
                asset.MarkAsDirty(true, false);
            }

            Object target = AssetDatabase.LoadAssetAtPath(targetPath, typeof(Object));
            if (target == null)
            {
                //Debug.LogWarning("target is null");
                return;
            }

            bool targetIsGameobject = target is GameObject;

            if (targetIsGameobject)
            {
                foreach (GameObject item in AssetFinderUnity.getAllObjsInCurScene())
                {
                    if (AssetFinderUnity.CheckIsPrefab(item))
                    {
                        string itemGUID = AssetFinderUnity.GetPrefabParent(item);
                        // Debug.Log(item.name + " itemGUID: " + itemGUID);
                        // Debug.Log(target.name + " asset.guid: " + asset.guid);
                        if (itemGUID == asset.guid)
                        {
                            Add(refs, item, 1);
                        }
                    }
                }
            }

            string dir = Path.GetDirectoryName(targetPath);
            if (AssetFinderSceneCache.Api.folderCache.ContainsKey(dir))
            {
                foreach (Component item in AssetFinderSceneCache.Api.folderCache[dir])
                {
                    if (AssetFinderSceneCache.Api.cache.ContainsKey(item))
                    {
                        foreach (AssetFinderSceneCache.HashValue item1 in AssetFinderSceneCache.Api
                                     .cache[item])
                        {
                            if (targetPath == AssetDatabase.GetAssetPath(item1.target))
                            {
                                Add(refs, item, 1);
                            }
                        }
                    }
                }
            }
        }

        private static void Add(Dictionary<string, AssetFinderRef> refs, AssetFinderAsset asset,
            int depth)
        {
            string targetId = asset.guid;
            if (!refs.ContainsKey(targetId))
            {
                refs.Add(targetId, new AssetFinderRef(0, depth, asset, null));
            }
        }

        private static void Add(Dictionary<string, AssetFinderRef> refs, Object target, int depth)
        {
            string targetId = target.GetInstanceID().ToString();
            if (!refs.ContainsKey(targetId))
            {
                refs.Add(targetId, new AssetFinderSceneRef(depth, target));
            }
        }
    }

    public class AssetFinderRef
    {
        static int CSVSorter(AssetFinderRef item1, AssetFinderRef item2)
        {
            var r = item1.depth.CompareTo(item2.depth);
            if (r != 0) return r;

            var t = item1.type.CompareTo(item2.type);
            if (t != 0) return t;

            return item1.index.CompareTo(item2.index);
        }


        public static AssetFinderRef[] FromDict(Dictionary<string, AssetFinderRef> dict)
        {
            if (dict == null || dict.Count == 0) return null;

            var result = new List<AssetFinderRef>();

            foreach (var kvp in dict)
            {
                if (kvp.Value == null) continue;
                if (kvp.Value.asset == null) continue;

                result.Add(kvp.Value);
            }

            result.Sort(CSVSorter);


            return result.ToArray();
        }

        public static AssetFinderRef[] FromList(List<AssetFinderRef> list)
        {
            if (list == null || list.Count == 0) return null;

            list.Sort(CSVSorter);
            var result = new List<AssetFinderRef>();
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].asset == null) continue;
                result.Add(list[i]);
            }

            return result.ToArray();
        }

        public AssetFinderAsset addBy;
        public AssetFinderAsset asset;
        public Object component;
        public int depth;
        public string group;
        public int index;

        public bool isSceneRef;
        public int matchingScore;
        public int type;

        public override string ToString()
        {
            if (isSceneRef)
            {
                var sr = (AssetFinderSceneRef)this;
                return sr.scenePath;
            }

            return asset.assetPath;
        }

        public AssetFinderRef(int index, int depth, AssetFinderAsset asset, AssetFinderAsset by)
        {
            this.index = index;
            this.depth = depth;

            this.asset = asset;
            if (asset != null)
            {
                type = AssetType.GetIndex(asset.extension);
            }

            addBy = by;
            // isSceneRef = false;
        }

        public AssetFinderRef(int index, int depth, AssetFinderAsset asset, AssetFinderAsset by,
            string group) : this(
            index, depth, asset,
            by)
        {
            this.group = group;
            // isSceneRef = false;
        }

        public string GetSceneObjId()
        {
            if (component == null)
            {
                return string.Empty;
            }

            return component.GetInstanceID().ToString();
        }

        public virtual bool isSelected()
        {
            return AssetFinderBookmark.Contains(asset.guid);
        }

        public virtual void DrawToogleSelect(Rect r)
        {
            var s = isSelected();
            r.width = 16f;

            if (!GUI2.Toggle(r, ref s)) return;

            if (s)
            {
                AssetFinderBookmark.Add(this);
            }
            else
            {
                AssetFinderBookmark.Remove(this);
            }
        }

        // public AssetFinderRef(int depth, UnityEngine.Object target)
        // {
        // 	this.component = target;
        // 	this.depth = depth;
        // 	// isSceneRef = true;
        // }
        internal List<AssetFinderRef> Append(Dictionary<string, AssetFinderRef> dict,
            params string[] guidList)
        {
            var result = new List<AssetFinderRef>();

            if (AssetFinderCache.Api.disabled)
            {
                return result;
            }

            if (!AssetFinderCache.isReady)
            {
                Debug.LogWarning("Cache not yet ready! Please wait!");
                return result;
            }

            //filter to remove items that already in dictionary
            for (var i = 0; i < guidList.Length; i++)
            {
                string guid = guidList[i];
                if (dict.ContainsKey(guid))
                {
                    continue;
                }

                AssetFinderAsset child = AssetFinderCache.Api.Get(guid);
                if (child == null)
                {
                    continue;
                }

                var r = new AssetFinderRef(dict.Count, depth + 1, child, asset);
                if (!asset.IsFolder)
                {
                    dict.Add(guid, r);
                }

                result.Add(r);
            }

            return result;
        }

        internal void AppendUsedBy(Dictionary<string, AssetFinderRef> result, bool deep)
        {
            // var list = Append(result, AssetFinderAsset.FindUsedByGUIDs(asset).ToArray());
            // if (!deep) return;

            // // Add next-level
            // for (var i = 0;i < list.Count;i ++)
            // {
            // 	list[i].AppendUsedBy(result, true);
            // }

            Dictionary<string, AssetFinderAsset> h = asset.UsedByMap;
            List<AssetFinderRef> list = deep ? new List<AssetFinderRef>() : null;

            if (asset.UsedByMap == null) return;

            foreach (KeyValuePair<string, AssetFinderAsset> kvp in h)
            {
                string guid = kvp.Key;
                if (result.ContainsKey(guid))
                {
                    continue;
                }

                AssetFinderAsset child = AssetFinderCache.Api.Get(guid);
                if (child == null)
                {
                    continue;
                }

                if (child.IsMissing)
                {
                    continue;
                }

                var r = new AssetFinderRef(result.Count, depth + 1, child, asset);
                if (!asset.IsFolder)
                {
                    result.Add(guid, r);
                }

                if (deep)
                {
                    list.Add(r);
                }
            }

            if (!deep)
            {
                return;
            }

            foreach (AssetFinderRef item in list)
            {
                item.AppendUsedBy(result, true);
            }
        }

        internal void AppendUsage(Dictionary<string, AssetFinderRef> result, bool deep)
        {
            Dictionary<string, HashSet<int>> h = asset.UseGUIDs;
            List<AssetFinderRef> list = deep ? new List<AssetFinderRef>() : null;

            foreach (KeyValuePair<string, HashSet<int>> kvp in h)
            {
                string guid = kvp.Key;
                if (result.ContainsKey(guid))
                {
                    continue;
                }

                AssetFinderAsset child = AssetFinderCache.Api.Get(guid);
                if (child == null)
                {
                    continue;
                }

                if (child.IsMissing)
                {
                    continue;
                }

                var r = new AssetFinderRef(result.Count, depth + 1, child, asset);
                if (!asset.IsFolder)
                {
                    result.Add(guid, r);
                }

                if (deep)
                {
                    list.Add(r);
                }
            }

            if (!deep)
            {
                return;
            }

            foreach (AssetFinderRef item in list)
            {
                item.AppendUsage(result, true);
            }
        }

        // --------------------- STATIC UTILS -----------------------

        internal static Dictionary<string, AssetFinderRef> FindRefs(string[] guids,
            bool usageOrUsedBy,
            bool addFolder)
        {
            var dict = new Dictionary<string, AssetFinderRef>();
            var list = new List<AssetFinderRef>();

            for (var i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                if (dict.ContainsKey(guid))
                {
                    continue;
                }

                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset == null)
                {
                    continue;
                }

                var r = new AssetFinderRef(i, 0, asset, null);
                if (!asset.IsFolder || addFolder)
                {
                    dict.Add(guid, r);
                }

                list.Add(r);
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (usageOrUsedBy)
                {
                    list[i].AppendUsage(dict, true);
                }
                else
                {
                    list[i].AppendUsedBy(dict, true);
                }
            }

            //var result = dict.Values.ToList();
            //result.Sort((item1, item2)=>{
            //	return item1.index.CompareTo(item2.index);
            //});

            return dict;
        }


        public static Dictionary<string, AssetFinderRef> FindUsage(string[] guids)
        {
            return FindRefs(guids, true, true);
        }

        public static Dictionary<string, AssetFinderRef> FindUsedBy(string[] guids)
        {
            return FindRefs(guids, false, true);
        }

        public static Dictionary<string, AssetFinderRef> FindUsageScene(GameObject[] objs,
            bool depth)
        {
            var dict = new Dictionary<string, AssetFinderRef>();
            // var list = new List<AssetFinderRef>();

            for (var i = 0; i < objs.Length; i++)
            {
                if (AssetFinderUnity.IsInAsset(objs[i]))
                {
                    continue; //only get in scene 
                }

                //add selection
                if (!dict.ContainsKey(objs[i].GetInstanceID().ToString()))
                {
                    dict.Add(objs[i].GetInstanceID().ToString(),
                        new AssetFinderSceneRef(0, objs[i]));
                }

                foreach (Object item in AssetFinderUnity.GetAllRefObjects(objs[i]))
                {
                    AppendUsageScene(dict, item);
                }

                if (depth)
                {
                    foreach (GameObject child in AssetFinderUnity.getAllChild(objs[i]))
                    foreach (Object item2 in AssetFinderUnity.GetAllRefObjects(child))
                    {
                        AppendUsageScene(dict, item2);
                    }
                }
            }

            return dict;
        }

        private static void AppendUsageScene(Dictionary<string, AssetFinderRef> dict, Object obj)
        {
            string path = AssetDatabase.GetAssetPath(obj);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            string guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid))
            {
                return;
            }

            if (dict.ContainsKey(guid))
            {
                return;
            }

            AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
            if (asset == null)
            {
                return;
            }

            var r = new AssetFinderRef(0, 1, asset, null);
            dict.Add(guid, r);
        }
    }


    public class AssetFinderRefDrawer : IRefDraw
    {
        public enum Mode
        {
            Dependency,
            Depth,
            Type,
            Extension,
            Folder,
            Atlas,
            AssetBundle,

            None
        }

        public enum Sort
        {
            Type,
            Path,
            Size
        }

        public static GUIStyle toolbarSearchField;
        public static GUIStyle toolbarSearchFieldCancelButton;
        public static GUIStyle toolbarSearchFieldCancelButtonEmpty;

        internal readonly AssetFinderTreeUI2.GroupDrawer groupDrawer;
        private readonly bool showSearch = true;
        public bool caseSensitive = false;

        // STATUS
        private bool dirty;
        private int excludeCount;

        public string level0Group;
        public bool forceHideDetails;

        public string Lable;
        internal List<AssetFinderRef> list;
        internal Dictionary<string, AssetFinderRef> refs;

        // FILTERING
        private string searchTerm = string.Empty;
        private bool selectFilter;
        private bool showIgnore;


        // ORIGINAL
        internal AssetFinderRef[] source
        {
            get { return AssetFinderRef.FromList(list); }
        }


        public AssetFinderRefDrawer(IWindow window)
        {
            this.window = window;
            groupDrawer = new AssetFinderTreeUI2.GroupDrawer(DrawGroup, DrawAsset);
        }

        public string messageNoRefs = "Do select something!";
        public string messageEmpty = "It's empty!";

        public IWindow window { get; set; }

        void DrawEmpty(Rect rect, string text)
        {
            rect = GUI2.Padding(rect, 2f, 2f);
            rect.height = 40f;

            EditorGUI.HelpBox(rect, text, MessageType.Info);
        }

        public bool Draw(Rect rect)
        {
            if (refs == null || refs.Count == 0)
            {
                DrawEmpty(rect, messageNoRefs);
                return false;
            }

            if (dirty || list == null)
            {
                ApplyFilter();
            }

            if (!groupDrawer.hasChildren)
            {
                DrawEmpty(rect, messageEmpty);
            }
            else
            {
                groupDrawer.Draw(rect);
            }

            return false;
        }

        public bool DrawLayout()
        {
            if (refs == null || refs.Count == 0)
            {
                return false;
            }

            if (dirty || list == null)
            {
                ApplyFilter();
            }

            groupDrawer.DrawLayout();
            return false;
        }

        public int ElementCount()
        {
            if (refs == null)
            {
                return 0;
            }

            return refs.Count;
            // return refs.Where(x => x.Value.depth != 0).Count();
        }

        public void SetRefs(Dictionary<string, AssetFinderRef> dictRefs)
        {
            refs = dictRefs;
            dirty = true;
        }

        void SetBookmarkGroup(string groupLabel, bool willbookmark)
        {
            string[] ids = groupDrawer.GetChildren(groupLabel);
            var info = GetBMInfo(groupLabel);

            for (var i = 0; i < ids.Length; i++)
            {
                AssetFinderRef rf;
                if (!refs.TryGetValue(ids[i], out rf))
                {
                    continue;
                }

                if (willbookmark)
                {
                    AssetFinderBookmark.Add(rf);
                }
                else
                {
                    AssetFinderBookmark.Remove(rf);
                }
            }

            info.count = willbookmark ? info.total : 0;
        }

        internal class BookmarkInfo
        {
            public int count;
            public int total;
        }

        private Dictionary<string, BookmarkInfo> gBookmarkCache =
            new Dictionary<string, BookmarkInfo>();

        BookmarkInfo GetBMInfo(string groupLabel)
        {
            BookmarkInfo info = null;
            if (!gBookmarkCache.TryGetValue(groupLabel, out info))
            {
                string[] ids = groupDrawer.GetChildren(groupLabel);

                info = new BookmarkInfo();
                for (var i = 0; i < ids.Length; i++)
                {
                    AssetFinderRef rf;
                    if (!refs.TryGetValue(ids[i], out rf))
                    {
                        continue;
                    }

                    info.total++;

                    var isBM = AssetFinderBookmark.Contains(rf);
                    if (isBM) info.count++;
                }

                gBookmarkCache.Add(groupLabel, info);
            }

            return info;
        }

        void DrawToggleGroup(Rect r, string groupLabel)
        {
            var info = GetBMInfo(groupLabel);
            var selectAll = info.count == info.total;
            r.width = 16f;
            if (GUI2.Toggle(r, ref selectAll))
            {
                SetBookmarkGroup(groupLabel, selectAll);
            }

            if (!selectAll && info.count > 0)
            {
                //GUI.DrawTexture(r, EditorStyles.
            }
        }

        private void DrawGroup(Rect r, string label, int childCount)
        {
            if (AssetFinderSetting.GroupMode == Mode.Folder)
            {
                Texture tex = AssetDatabase.GetCachedIcon("Assets");
                GUI.DrawTexture(new Rect(r.x - 2f, r.y - 2f, 16f, 16f), tex);
                r.xMin += 16f;
            }

            DrawToggleGroup(r, label);
            r.xMin += 18f;
            GUI.Label(r, label + " (" + childCount + ")", EditorStyles.boldLabel);

            bool hasMouse = Event.current.type == EventType.MouseUp &&
                            r.Contains(Event.current.mousePosition);
            if (hasMouse && Event.current.button == 1)
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Add Bookmark"), false,
                    () => { SetBookmarkGroup(label, true); });
                menu.AddItem(new GUIContent("Remove Bookmark"), false,
                    () => { SetBookmarkGroup(label, false); });

                menu.ShowAsContext();
                Event.current.Use();
            }
        }

        private void DrawAsset(Rect r, string guid)
        {
            AssetFinderRef rf;
            if (!refs.TryGetValue(guid, out rf))
            {
                return;
            }

            if (rf.isSceneRef)
            {
                if (rf.component == null)
                {
                    return;
                }

                var re = rf as AssetFinderSceneRef;
                if (re != null)
                {
                    r.x -= 16f;
                    rf.DrawToogleSelect(r);
                    r.xMin += 32f;
                    re.Draw(r, window, !forceHideDetails);
                }
            }
            else
            {
                r.xMin -= 16f;
                rf.DrawToogleSelect(r);
                r.xMin += 32f;
                rf.asset.Draw(r,
                    rf.depth == 1,
                    !forceHideDetails && (AssetFinderSetting.GroupMode != Mode.Folder),
                    !forceHideDetails && AssetFinderSetting.s.displayFileSize,
                    !forceHideDetails && AssetFinderSetting.s.displayAssetBundleName,
                    !forceHideDetails && AssetFinderSetting.s.displayAtlasName,
                    !forceHideDetails && AssetFinderSetting.s.showUsedByClassed,
                    window
                );
            }
        }

        private string GetGroup(AssetFinderRef rf)
        {
            if (rf.depth == 0)
            {
                return level0Group;
            }

            if (AssetFinderSetting.GroupMode == Mode.None)
            {
                return "(no group)";
            }

            AssetFinderSceneRef sr = null;
            if (rf.isSceneRef)
            {
                sr = rf as AssetFinderSceneRef;
                if (sr == null) return null;
            }

            if (!rf.isSceneRef)
            {
                if (rf.asset.IsExcluded) return null; // "(ignored)"
            }

            switch (AssetFinderSetting.GroupMode)
            {
                case Mode.Extension: return rf.isSceneRef ? sr.targetType : rf.asset.extension;
                case Mode.Type:
                {
                    return rf.isSceneRef ? sr.targetType : AssetType.FILTERS[rf.type].name;
                }

                case Mode.Folder: return rf.isSceneRef ? sr.scenePath : rf.asset.assetFolder;

                case Mode.Dependency:
                {
                    return rf.depth == 1 ? "Direct Usage" : "Indirect Usage";
                }

                case Mode.Depth:
                {
                    return "Level " + rf.depth.ToString();
                }

                case Mode.Atlas:
                    return rf.isSceneRef
                        ? "(not in atlas)"
                        : (string.IsNullOrEmpty(rf.asset.AtlasName)
                            ? "(not in atlas)"
                            : rf.asset.AtlasName);
                case Mode.AssetBundle:
                    return rf.isSceneRef
                        ? "(not in assetbundle)"
                        : (string.IsNullOrEmpty(rf.asset.AssetBundleName)
                            ? "(not in assetbundle)"
                            : rf.asset.AssetBundleName);
            }

            return "(others)";
        }

        private void SortGroup(List<string> groups)
        {
            groups.Sort((item1, item2) =>
            {
                if (item1.Contains("(")) return 1;
                if (item2.Contains("(")) return -1;

                return item1.CompareTo(item2);
            });
        }

        public AssetFinderRefDrawer Reset(string[] assetGUIDs, bool isUsage)
        {
            //Debug.Log("Reset :: " + assetGUIDs.Length + "\n" + string.Join("\n", assetGUIDs));
            gBookmarkCache.Clear();

            if (isUsage)
            {
                refs = AssetFinderRef.FindUsage(assetGUIDs);
            }
            else
            {
                refs = AssetFinderRef.FindUsedBy(assetGUIDs);
            }

            dirty = true;
            if (list != null)
            {
                list.Clear();
            }

            return this;
        }

        public AssetFinderRefDrawer Reset(GameObject[] objs, bool findDept, bool findPrefabInAsset)
        {
            refs = AssetFinderRef.FindUsageScene(objs, findDept);

            var guidss = new List<string>();
            var dependent = AssetFinderSceneCache.Api.prefabDependencies;
            foreach (var gameObject in objs)
            {
                HashSet<string> hash;
                if (!dependent.TryGetValue(gameObject, out hash))
                {
                    continue;
                }

                foreach (string guid in hash)
                {
                    guidss.Add(guid);
                }
            }

            Dictionary<string, AssetFinderRef> usageRefs1 =
                AssetFinderRef.FindUsage(guidss.ToArray());
            foreach (KeyValuePair<string, AssetFinderRef> kvp in usageRefs1)
            {
                if (refs.ContainsKey(kvp.Key))
                {
                    continue;
                }

                if (guidss.Contains(kvp.Key))
                {
                    kvp.Value.depth = 1;
                }

                refs.Add(kvp.Key, kvp.Value);
            }


            if (findPrefabInAsset)
            {
                var guids = new List<string>();
                for (var i = 0; i < objs.Length; i++)
                {
                    string guid = AssetFinderUnity.GetPrefabParent(objs[i]);
                    if (string.IsNullOrEmpty(guid))
                    {
                        continue;
                    }

                    guids.Add(guid);
                }

                Dictionary<string, AssetFinderRef> usageRefs =
                    AssetFinderRef.FindUsage(guids.ToArray());
                foreach (KeyValuePair<string, AssetFinderRef> kvp in usageRefs)
                {
                    if (refs.ContainsKey(kvp.Key))
                    {
                        continue;
                    }

                    if (guids.Contains(kvp.Key))
                    {
                        kvp.Value.depth = 1;
                    }

                    refs.Add(kvp.Key, kvp.Value);
                }
            }

            dirty = true;
            if (list != null)
            {
                list.Clear();
            }

            return this;
        }

        //ref in scene
        public AssetFinderRefDrawer Reset(string[] assetGUIDs, IWindow window)
        {
            refs = AssetFinderSceneRef.FindRefInScene(assetGUIDs, true, SetRefInScene, window);
            dirty = true;
            if (list != null)
            {
                list.Clear();
            }

            return this;
        }

        private void SetRefInScene(Dictionary<string, AssetFinderRef> data)
        {
            refs = data;
            dirty = true;
            if (list != null)
            {
                list.Clear();
            }
        }

        //scene in scene
        public AssetFinderRefDrawer ResetSceneInScene(GameObject[] objs)
        {
            refs = AssetFinderSceneRef.FindSceneInScene(objs);
            dirty = true;
            if (list != null)
            {
                list.Clear();
            }

            return this;
        }

        public AssetFinderRefDrawer ResetSceneUseSceneObjects(GameObject[] objs)
        {
            refs = AssetFinderSceneRef.FindSceneUseSceneObjects(objs);
            dirty = true;
            if (list != null)
            {
                list.Clear();
            }

            return this;
        }

        public AssetFinderRefDrawer ResetUnusedAsset()
        {
            List<AssetFinderAsset> lst = AssetFinderCache.Api.ScanUnused();

            refs = lst.ToDictionary(x => x.guid, x => new AssetFinderRef(0, 1, x, null));
            dirty = true;
            if (list != null)
            {
                list.Clear();
            }

            return this;
        }

        public void RefreshSort()
        {
            if (list == null)
            {
                return;
            }

            if (list.Count > 0 && list[0].isSceneRef == false &&
                AssetFinderSetting.SortMode == Sort.Size)
            {
                list = list.OrderByDescending(x => x.asset != null ? x.asset.fileSize : 0).ToList();
            }
            else
            {
                list.Sort((r1, r2) =>
                {
                    var isMixed = r1.isSceneRef ^ r2.isSceneRef;
                    if (isMixed)
                    {
#if AssetFinderDEBUG
						var sb = new StringBuilder();
						sb.Append("r1: " + r1.ToString());
						sb.AppendLine();
						sb.Append("r2: " +r2.ToString());
						Debug.LogWarning("Mixed compared!\n" + sb.ToString());
#endif

                        var v1 = r1.isSceneRef ? 1 : 0;
                        var v2 = r2.isSceneRef ? 1 : 0;
                        return v2.CompareTo(v1);
                    }

                    if (r1.isSceneRef)
                    {
                        var rs1 = (AssetFinderSceneRef)r1;
                        var rs2 = (AssetFinderSceneRef)r2;

                        return SortAsset(rs1.sceneFullPath, rs2.sceneFullPath,
                            rs1.targetType, rs2.targetType,
                            AssetFinderSetting.SortMode == Sort.Path);
                    }

                    return SortAsset(
                        r1.asset.assetPath, r2.asset.assetPath,
                        r1.asset.extension, r2.asset.extension,
                        false
                    );
                });
            }

            // clean up list
            int invalidCount = 0;
            for (int i = list.Count - 1; i >= 0; i--)
            {
                var item = list[i];

                if (item.isSceneRef)
                {
                    if (string.IsNullOrEmpty(item.GetSceneObjId()))
                    {
                        invalidCount++;
                        list.RemoveAt(i);
                    }

                    continue;
                }

                if (item.asset == null)
                {
                    invalidCount++;
                    list.RemoveAt(i);
                    continue;
                }
            }

#if AssetFinderDEBUG
			if (invalidCount > 0) Debug.LogWarning("Removed [" + invalidCount + "] invalid assets / objects");
#endif

            groupDrawer.Reset(list,
                rf =>
                {
                    if (rf == null) return null;
                    if (rf.isSceneRef) return rf.GetSceneObjId();
                    return rf.asset == null ? null : rf.asset.guid;
                }, GetGroup, SortGroup);
        }

        public bool isExclueAnyItem()
        {
            return excludeCount > 0;
        }

        private void ApplyFilter()
        {
            dirty = false;

            if (refs == null)
            {
                return;
            }

            if (list == null)
            {
                list = new List<AssetFinderRef>();
            }
            else
            {
                list.Clear();
            }

            int minScore = searchTerm.Length;

            string term1 = searchTerm;
            if (!caseSensitive)
            {
                term1 = term1.ToLower();
            }

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
                string name1 = r.isSceneRef
                    ? (r as AssetFinderSceneRef).sceneFullPath
                    : r.asset.assetName;
                if (!caseSensitive)
                {
                    name1 = name1.ToLower();
                }

                string name2 = name1.Replace(" ", string.Empty);

                int score1 = AssetFinderUnity.StringMatch(term1, name1);
                int score2 = AssetFinderUnity.StringMatch(term2, name2);

                r.matchingScore = Mathf.Max(score1, score2);
                if (r.matchingScore > minScore)
                {
                    list.Add(r);
                }
            }

            RefreshSort();
        }

        public void SetDirty()
        {
            dirty = true;
        }

        private int SortAsset(string term11, string term12, string term21, string term22, bool swap)
        {
//			if (term11 == null) term11 = string.Empty;
//			if (term12 == null) term12 = string.Empty;
//			if (term21 == null) term21 = string.Empty;
//			if (term22 == null) term22 = string.Empty;
            var v1 = String.Compare(term11, term12, StringComparison.Ordinal);
            var v2 = String.Compare(term21, term22, StringComparison.Ordinal);
            return swap ? (v1 == 0) ? v2 : v1 : (v2 == 0) ? v1 : v2;
        }

        public Dictionary<string, AssetFinderRef> getRefs()
        {
            return refs;
        }
    }
}