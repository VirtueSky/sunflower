using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{

    internal class AssetFinderRef
    {

        public AssetFinderAsset addBy;
        
        // Callback delegates for advanced bookmark operations (set by the drawer)
        public System.Action<AssetFinderRef> OnCtrlClick;
        public System.Action<AssetFinderRef> OnAltClick;
        public System.Action<AssetFinderRef> OnShiftClick;
        public AssetFinderAsset asset;
        public Object component;
        public int depth;
        public string group;
        public int index;

        public bool isSceneRef;
        public int matchingScore;
        public int type;

        public AssetFinderRef()
        { }

        public AssetFinderRef(int index, int depth, AssetFinderAsset asset, AssetFinderAsset by)
        {
            this.index = index;
            this.depth = depth;

            this.asset = asset;
            if (asset != null) type = AssetFinderAssetGroupDrawer.GetIndex(asset.extension);

            addBy = by;

            // isSceneRef = false;
        }

        public AssetFinderRef(int index, int depth, AssetFinderAsset asset, AssetFinderAsset by, string group) : this(index, depth, asset,
            by)
        {
            this.group = group;

            // isSceneRef = false;
        }
        private static int CSVSorter(AssetFinderRef item1, AssetFinderRef item2)
        {
            int r = item1.depth.CompareTo(item2.depth);
            if (r != 0) return r;

            int t = item1.type.CompareTo(item2.type);
            if (t != 0) return t;

            return item1.index.CompareTo(item2.index);
        }


        public static AssetFinderRef[] FromDict(Dictionary<string, AssetFinderRef> dict)
        {
            if (dict == null || dict.Count == 0) return null;

            var result = new List<AssetFinderRef>();

            foreach (KeyValuePair<string, AssetFinderRef> kvp in dict)
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
            for (var i = 0; i < list.Count; i++)
            {
                if (list[i].asset == null) continue;
                result.Add(list[i]);
            }
            return result.ToArray();
        }

        public override string ToString()
        {
            if (isSceneRef)
            {
                var sr = (AssetFinderSceneRef)this;
                return sr.scenePath;
            }

            return asset.assetPath;
        }

        public string GetSceneObjId()
        {
            if (component == null) return string.Empty;

            return component.GetInstanceID().ToString();
        }

        public virtual bool isSelected()
        {
            return AssetFinderBookmark.Contains(asset.guid);
        }
        public virtual void DrawToogleSelect(Rect r)
        {
            bool s = isSelected();
            r.width = 16f;
            
            Event evt = Event.current;
            bool isMouseOver = r.Contains(evt.mousePosition);
            bool isMouseDown = evt.type == EventType.MouseDown && evt.button == 0 && isMouseOver;
            
            // Handle modifier keys for advanced selection
            if (isMouseDown)
            {
                bool ctrl = Application.platform == RuntimePlatform.OSXEditor ? evt.command : evt.control;
                bool alt = evt.alt;
                bool shift = evt.shift;
                
                if (shift)
                {
                    // Shift+click: Toggle all items in all groups
                    OnShiftClick?.Invoke(this);
                    evt.Use();
                    return;
                }
                else if (alt)
                {
                    // Alt+click: Toggle all siblings in the same group
                    OnAltClick?.Invoke(this);
                    evt.Use();
                    return;
                }
                else if (ctrl)
                {
                    // Cmd+click (Mac) / Ctrl+click (PC): Toggle self and set all siblings to the same new state
                    OnCtrlClick?.Invoke(this);
                    evt.Use();
                    return;
                }
            }
            
            // Normal toggle behavior
            if (!GUI2.Toggle(r, ref s)) return;

            if (s)
            {
                AssetFinderBookmark.Add(this);
            } else
            {
                AssetFinderBookmark.Remove(this);
            }
        }

        // Removed - now handled via instance callbacks
        
        // Removed - now handled via instance callbacks
        
        // Removed - now handled via instance callbacks
        
        // Removed - now handled via instance callbacks
        


        // public AssetFinderRef(int depth, UnityEngine.Object target)
        // {
        // 	this.component = target;
        // 	this.depth = depth;
        // 	// isSceneRef = true;
        // }
        internal List<AssetFinderRef> Append(Dictionary<string, AssetFinderRef> dict, params string[] guidList)
        {
            var result = new List<AssetFinderRef>();
            if (!AssetFinderCache.isReady)
            {
                AssetFinderLOG.LogWarning("Cache not yet ready! Please wait!");
                return result;
            }

            // var excludePackage = !AssetFinderCache.Api.setting.showPackageAsset;
            //filter to remove items that already in dictionary
            for (var i = 0; i < guidList.Length; i++)
            {
                string guid = guidList[i];
                if (dict.ContainsKey(guid)) continue;

                AssetFinderAsset child = AssetFinderCache.Api.Get(guid);
                if (child == null) continue;
                // if (excludePackage && child.inPackages) continue;

                var r = new AssetFinderRef(dict.Count, depth + 1, child, asset);
                dict.Add(guid, r);
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
            // bool excludePackage = !AssetFinderCache.Api.setting.showPackageAsset;

            foreach (KeyValuePair<string, AssetFinderAsset> kvp in h)
            {
                string guid = kvp.Key;
                if (result.ContainsKey(guid)) continue;

                AssetFinderAsset child = AssetFinderCache.Api.Get(guid);
                if (child == null) continue;
                if (child.IsMissing) continue;
                // if (excludePackage && child.inPackages) continue;

                var r = new AssetFinderRef(result.Count, depth + 1, child, asset);
                result.Add(guid, r);

                if (deep) list.Add(r);
            }

            if (!deep) return;

            foreach (AssetFinderRef item in list)
            {
                item.AppendUsedBy(result, true);
            }
        }

        internal void AppendUsage(Dictionary<string, AssetFinderRef> result, bool deep)
        {
            Dictionary<string, HashSet<long>> h = asset.UseGUIDs;
            List<AssetFinderRef> list = deep ? new List<AssetFinderRef>() : null;
            // bool excludePackage = !AssetFinderCache.Api.setting.showPackageAsset;
            foreach (KeyValuePair<string, HashSet<long>> kvp in h)
            {
                string guid = kvp.Key;
                if (result.ContainsKey(guid)) continue;

                AssetFinderAsset child = AssetFinderCache.Api.Get(guid);
                if (child == null) continue;
                if (child.IsMissing) continue;
                // if (excludePackage && child.inPackages) continue;

                var r = new AssetFinderRef(result.Count, depth + 1, child, asset);
                result.Add(guid, r);

                if (deep) list.Add(r);
            }

            if (!deep) return;

            foreach (AssetFinderRef item in list)
            {
                item.AppendUsage(result, true);
            }
        }

        // --------------------- STATIC UTILS -----------------------


        internal static Dictionary<string, AssetFinderRef> FindRefs(string[] guids, bool usageOrUsedBy, bool addFolder)
        {
            var dict = new Dictionary<string, AssetFinderRef>();
            var list = new List<AssetFinderRef>();
            var selectedGuids = new HashSet<string>(guids);
            // bool excludePackage = !AssetFinderCache.Api.setting.showPackageAsset;

            for (var i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];
                if (dict.ContainsKey(guid)) continue;

                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                if (asset == null) continue;
                // if (excludePackage && asset.inPackages) continue;

                var r = new AssetFinderRef(i, 0, asset, null);
                if (!asset.IsFolder || addFolder) dict.Add(guid, r);

                list.Add(r);
            }

            for (var i = 0; i < list.Count; i++)
            {
                if (usageOrUsedBy)
                {
                    list[i].AppendUsage(dict, true);
                } else
                {
                    list[i].AppendUsedBy(dict, true);
                }
            }

            // Remove selected objects themselves from results (depth 0)
            var filteredDict = new Dictionary<string, AssetFinderRef>();
            foreach (KeyValuePair<string, AssetFinderRef> kvp in dict)
            {
                if (kvp.Value.depth == 0) continue; // Exclude selected objects
                if (selectedGuids.Contains(kvp.Key)) continue; // Double-check exclusion
                filteredDict.Add(kvp.Key, kvp.Value);
            }

            return filteredDict;
        }


        public static Dictionary<string, AssetFinderRef> FindUsage(string[] guids)
        {
            return FindRefs(guids, true, true);
        }

        public static Dictionary<string, AssetFinderRef> FindUsedBy(string[] guids)
        {
            return FindRefs(guids, false, true);
        }

        public static Dictionary<string, AssetFinderRef> FindUsageScene(GameObject[] objs, bool depth)
        {
            var dict = new Dictionary<string, AssetFinderRef>();

            // var list = new List<AssetFinderRef>();

            for (var i = 0; i < objs.Length; i++)
            {
                if (objs[i].IsAssetObject()) continue; //only get in scene 

                //add selection
                if (!dict.ContainsKey(objs[i].GetInstanceID().ToString())) dict.Add(objs[i].GetInstanceID().ToString(), new AssetFinderSceneRef(0, objs[i]));

                foreach (Object item in AssetFinderUnity.GetAllRefObjects(objs[i]))
                {
                    AppendUsageScene(dict, item);
                }

                if (!depth) continue;
                foreach (GameObject child in AssetFinderUnity.getAllChild(objs[i]))
                {
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
            if (string.IsNullOrEmpty(path)) return;

            string guid = AssetDatabase.AssetPathToGUID(path);
            if (string.IsNullOrEmpty(guid)) return;

            if (dict.ContainsKey(guid)) return;

            AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
            if (asset == null) return;

            // if (!AssetFinderCache.Api.setting.showPackageAsset && asset.inPackages) return;
            var r = new AssetFinderRef(0, 1, asset, null);
            dict.Add(guid, r);
        }
    }


}
