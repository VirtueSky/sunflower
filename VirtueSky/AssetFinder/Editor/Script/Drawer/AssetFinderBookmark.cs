using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderBookmark : IRefDraw
    {
        internal static readonly HashSet<string> guidSet = new HashSet<string>();
        internal static readonly HashSet<string> instSet = new HashSet<string>(); // Do not reference directly to SceneObject (which might be destroyed anytime)

        // ------------ instance
        private static bool dirty;
        private readonly AssetFinderRefDrawer drawer;
        internal Dictionary<string, AssetFinderRef> refs = new Dictionary<string, AssetFinderRef>();

        public AssetFinderBookmark(IWindow window, Func<AssetFinderRefDrawer.Sort> getSortMode, Func<AssetFinderRefDrawer.Mode> getGroupMode)
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
				showToggle = true,
				showHighlight = false,
				shouldShowExtension = () => true,
				shouldShowDetailButton = () => true,
				onCacheInvalidated = () => { } // Bookmark panel manages its own state
			})
            {
                messageNoRefs = "Do bookmark something!",
                groupDrawer =
                {
                    hideGroupIfPossible = true
                },
                level0Group = string.Empty,
                paddingLeft = -16f
            };

            dirty = true;
            drawer.SetDirty();
        }

        public static int Count => guidSet.Count + instSet.Count;

        public IWindow window { get; set; }

        public int ElementCount()
        {
            return refs == null ? 0 : refs.Count;
        }

        public bool DrawLayout()
        {
            if (dirty) RefreshView();
            return drawer.DrawLayout();
        }

        public bool Draw(Rect rect)
        {
            if (dirty) RefreshView();
            if (refs == null)
            {
                AssetFinderLOG.LogWarning("Refs is null!");
                return false;
            }

            var bottomRect = new Rect(rect.x + 1f, rect.yMax - 16f, rect.width - 2f, 16f);
            DrawButtons(bottomRect);

            rect.yMax -= 16f;
            return drawer.Draw(rect);
        }

        public static bool Contains(string guidOrInstID)
        {
            return guidSet.Contains(guidOrInstID) || instSet.Contains(guidOrInstID);
        }

        public static bool Contains(UnityObject sceneObject)
        {
            var id = sceneObject.GetInstanceID().ToString();
            return instSet.Contains(id);
        }
        public static bool Contains(AssetFinderRef rf)
        {
            if (rf.isSceneRef)
            {
                if (instSet == null) return false;
                return instSet.Contains(rf.component.GetInstanceID().ToString());
            }
            if (guidSet == null) return false;
            return guidSet.Contains(rf.asset.guid);
        }
        public static void Add(UnityObject sceneObject)
        {
            if (sceneObject == null) return;
            var id = sceneObject.GetInstanceID().ToString();
            instSet.Add(id);
            dirty = true;
        }

        public static void Add(string guid)
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

        public static void Remove(UnityObject sceneObject)
        {
            if (sceneObject == null) return;
            var id = sceneObject.GetInstanceID().ToString();
            instSet.Remove(id);
            dirty = true;
        }

        public static void Remove(string guidOrInstID)
        {
            guidSet.Remove(guidOrInstID);
            instSet.Remove(guidOrInstID);
            dirty = true;
        }

        public static void Clear()
        {
            guidSet.Clear();
            instSet.Clear();
            dirty = true;
        }

        public static void Add(AssetFinderRef rf)
        {

            if (rf.isSceneRef)
            {
                Add(rf.component);
            } else
            {
                Add(rf.asset.guid);
            }
            
            // Invalidate all drawer caches so group toggles update correctly
            InvalidateAllDrawerCaches();
        }

        public static void Remove(AssetFinderRef rf)
        {

            if (rf.isSceneRef)

                //Debug.Log("remove: " + rf.component);
            {
                Remove(rf.component);
            } else
            {
                Remove(rf.asset.guid);
            }
            
            // Invalidate all drawer caches so group toggles update correctly
            InvalidateAllDrawerCaches();
        }

        public static void Commit()
        {
            var list = new HashSet<UnityObject>();

            foreach (string guid in guidSet)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                UnityObject obj = AssetDatabase.LoadAssetAtPath(path, typeof(UnityObject));
                if (obj != null) list.Add(obj);
            }

            foreach (string instID in instSet)
            {
                int id = int.Parse(instID);
                UnityObject obj = EditorUtility.InstanceIDToObject(id);
                if (obj == null) continue;
                list.Add(obj is Component c ? c.gameObject : obj);
            }

            Selection.objects = list.ToArray();
        }

        public void SetDirty()
        {
            drawer.SetDirty();
        }

        private void DrawButtons(Rect rect)
        {
            var (selectRect, exportRect) = rect.ExtractLeft(64f, 4f);
            GUI.enabled = (refs != null) && (refs.Count > 0);
            {
                if (GUI.Button(selectRect, AssetFinderGUIContent.FromString("Select", "Select items in Project or Hierarchy panel"))) Commit();
                if (GUI.Button(exportRect, AssetFinderGUIContent.FromString("CSV", "Export bookmarked items as CSV"))) AssetFinderExport.ExportCSV(AssetFinderRef.FromDict(refs));    
            }
			GUI.enabled = true;
            
			// if (GUI.Button(right, AssetFinderIcon.Refresh.image)) RefreshView();
        }

        public void RefreshView()
        {
			refs = new Dictionary<string, AssetFinderRef>();

			//foreach (KeyValuePair<string, List<string>> item in AssetFinderSetting.IgnoreFiltered)
            foreach (string guid in guidSet)
            {
				AssetFinderAsset asset = AssetFinderCache.Api.Get(guid, false);
				if (asset == null)
				{
					AssetFinderLOG.LogWarning("Invalid asset guid: " + guid);
					continue;
				}
				refs.Add(guid, new AssetFinderRef(0, 1, asset, null));
            }

			foreach (string instID in instSet)
            {
				int id;
				if (!int.TryParse(instID, out id)) continue;
				var obj = EditorUtility.InstanceIDToObject(id);
				if (obj == null) continue;
				refs.Add(instID, new AssetFinderRef(0, 1, null, null) { component = obj, isSceneRef = true });
            }

            drawer.SetRefs(refs);
            dirty = false;
        }

        internal void RefreshSort()
        {
            drawer.RefreshSort();
        }

        
        // Callback for cache invalidation - set by AssetFinderWindowAll
        public static System.Action OnBookmarkChanged;
        
        private static void InvalidateAllDrawerCaches()
        {
            OnBookmarkChanged?.Invoke();
        }
    }
}
