using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderSceneRef : AssetFinderRef
    {
        internal static readonly Dictionary<string, Type> CacheType = new Dictionary<string, Type>();

        private static Action<Dictionary<string, AssetFinderRef>> onFindRefInSceneComplete;
        private static Dictionary<string, AssetFinderRef> refs = new Dictionary<string, AssetFinderRef>();
        private static string[] cacheAssetGuids;
        private GUIContent assetNameGC;
        private GUIContent assetTypeGC;

        public Func<bool> drawFullPath;
        public string sceneFullPath = "";
        public string scenePath = "";
        public string targetType;
        
        public List<SceneRefInfo> sourceRefs;
        public List<SceneRefInfo> backwardRefs;

        public AssetFinderSceneRef(int index, int depth, AssetFinderAsset asset, AssetFinderAsset by) : base(index, depth, asset, by)
        {
            isSceneRef = false;
            sourceRefs = new List<SceneRefInfo>();
            backwardRefs = new List<SceneRefInfo>();
            // Ensure tooltip always shows full path with proper slashes
            string tooltipPath = asset?.assetPath ?? "Unknown";
            assetNameGC = new GUIContent(asset?.assetName ?? "Unknown", tooltipPath);
            assetTypeGC = new GUIContent("");
        }
        
        public AssetFinderSceneRef(int depth, UnityObject target) : base(0, depth, null, null)
        {
            component = target;
            this.depth = depth;
            isSceneRef = true;
            sourceRefs = new List<SceneRefInfo>();
            backwardRefs = new List<SceneRefInfo>();
            InitializeTargetInfo(target);
        }

        void InitializeTargetInfo(UnityObject target)
        {
            if (target == null)
            {
                targetType = "Missing";
                scenePath = "";
                sceneFullPath = "Missing Object";
                assetNameGC = new GUIContent("Missing Object", "Object has been destroyed");
                assetTypeGC = new GUIContent("Missing");
                return;
            }

            if (target is GameObject obj)
            {
                targetType = nameof(GameObject);
                scenePath = AssetFinderUnity.GetGameObjectPath(obj, false);
                // Add trailing slash if scenePath is not empty
                string pathWithSlash = string.IsNullOrEmpty(scenePath) ? "" : scenePath + "/";
                sceneFullPath = pathWithSlash + obj.name;
                assetNameGC = new GUIContent(obj.name, sceneFullPath);
                assetTypeGC = GUIContent.none;
            }
            else if (target is Component com)
            {
                targetType = component.GetType().Name;
                scenePath = AssetFinderUnity.GetGameObjectPath(com.gameObject, false);
                // Add trailing slash if scenePath is not empty
                string pathWithSlash = string.IsNullOrEmpty(scenePath) ? "" : scenePath + "/";
                sceneFullPath = pathWithSlash + com.gameObject.name;
                assetNameGC = new GUIContent(com.gameObject.name, sceneFullPath);
                assetTypeGC = new GUIContent(component.GetType().Name);
            }
        }
        


        public override bool isSelected()
        {
            return (component != null) && AssetFinderBookmark.Contains(component);
        }

        public void Draw(Rect r, AssetFinderRefDrawer.Mode groupMode, bool showDetails, bool drawFullPath = false)
        {
            r.xMin -= 12f;
            r.xMax -= 12f;
            
            var margin = 2;
            var pingRect = r;
            Rect iconRect = GUI2.LeftRect(16f, ref r);
            
            // Right-click context menu for scene objects
            if ((Event.current.type == EventType.MouseUp) && (Event.current.button == 1) && pingRect.Contains(Event.current.mousePosition))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Open"), false, () => { if (component != null) EditorGUIUtility.PingObject(component); });
                menu.AddItem(new GUIContent("Ping"), false, () => { if (component != null) EditorGUIUtility.PingObject(component); });
#if UNITY_2022_3_OR_NEWER
                menu.AddItem(new GUIContent("Properties..."), false, () => { if (component != null) EditorUtility.OpenPropertyEditor(component); });
#else
                menu.AddDisabledItem(new GUIContent("Properties..."));
#endif
                menu.ShowAsContext();
                Event.current.Use();
            }
            
            var (icon, iconTooltip) = GetTargetIcon();
            
            // Calculate sizes based on whether we're showing full path
            float pathW = 0f;
            float nameW = 0f;
            
            if (drawFullPath && !string.IsNullOrEmpty(scenePath))
            {
                pathW = EditorStyles.miniLabel.CalcSize(AssetFinderGUIContent.FromString(scenePath + "/")).x;
                nameW = EditorStyles.label.CalcSize(assetNameGC).x;
            }
            else
            {
                nameW = EditorStyles.label.CalcSize(assetNameGC).x;
            }
            
            Rect pathRect = drawFullPath && pathW > 0 ? GUI2.LeftRect(pathW, ref r) : new Rect();
            Rect nameRect = GUI2.LeftRect(nameW, ref r);
            
            float typeW = EditorStyles.miniLabel.CalcSize(assetTypeGC).x;
            Rect typeRect = GUI2.LeftRect(typeW + 4f, ref r);
            
            pingRect.width = 16f + margin + pathW + nameW + typeW + 4f;
            
            DrawPingRect(pingRect);
            DrawTargetIcon(iconRect, icon, iconTooltip);
            DrawScenePath(pathRect, drawFullPath);
            DrawTargetName(nameRect, assetNameGC);
            DrawTargetType(typeRect, assetTypeGC);

#if UNITY_2022_3_OR_NEWER
            // Draw P only on hover and repaint on mouse move for responsiveness
            Rect rowRect = new Rect(r.x, r.y, r.width, AssetFinderTheme.Current.TreeItemHeight);
            bool isHover = rowRect.Contains(Event.current.mousePosition);
            if (Event.current.type == EventType.MouseMove)
            {
                var focused = EditorWindow.focusedWindow;
                if (focused != null) focused.Repaint();
            }
            if (isHover)
            {
                var propRect = new Rect(r.x + r.width - 22f, r.y, 22f, r.height);
                r.width -= 22f;
                float miniH = 16f;
                propRect.y += Mathf.Floor((AssetFinderTheme.Current.TreeItemHeight - miniH) / 2f);
                propRect.height = miniH;
                if (GUI.Button(propRect, new GUIContent("P", "Open Properties"), EditorStyles.miniButton))
                {
                    if (component != null) EditorUtility.OpenPropertyEditor(component);
                }
            }
#endif

            if (AssetFinderSetting.ShowUsedByClassed)
            {
                DrawReferenceIcons(r);
            }
        }

        (Texture icon, string tooltip) GetTargetIcon()
        {
            if (component == null) return (null, "");
            
            if (component is GameObject go)
                return (EditorGUIUtility.ObjectContent(go, typeof(GameObject)).image, "GameObject");
            if (component is Component comp)
                return (EditorGUIUtility.ObjectContent(comp, comp.GetType()).image, comp.GetType().Name);
            
            return (EditorGUIUtility.ObjectContent(component, component.GetType()).image, component.GetType().Name);
        }

        void DrawTargetIcon(Rect iconRect, Texture icon, string iconTooltip)
        {
            if (icon != null)
            {
                var iconContent = new GUIContent(icon, iconTooltip);
                GUI.Label(iconRect, iconContent, EditorStyles.label);
            }
        }

        void DrawScenePath(Rect pathRect, bool drawFullPath)
        {
            if (!drawFullPath || string.IsNullOrEmpty(scenePath)) return;
            
            Color c = GUI.color;
            GUI.color = new Color(c.r, c.g, c.b, c.a * 0.5f); // Dim the path like Unity's Project panel
            GUI.Label(pathRect, AssetFinderGUIContent.FromString(scenePath + "/"), EditorStyles.miniLabel);
            GUI.color = c;
        }

        void DrawTargetName(Rect nameRect, GUIContent displayContent)
        {
            if (isSelected())
            {
                Color c = GUI.color;
                GUI.color = GUI.skin.settings.selectionColor;
                GUI.DrawTexture(nameRect, EditorGUIUtility.whiteTexture);
                GUI.color = c;
            }
            GUI.Label(nameRect, displayContent, EditorStyles.label);
        }

        void DrawTargetType(Rect typeRect, GUIContent typeContent)
        {
            if (!string.IsNullOrEmpty(typeContent.text))
            {
                // Always dim component type, no hover effects
                Color c = GUI.color;
                GUI.color = new Color(c.r, c.g, c.b, c.a * 0.6f);
                GUI.Label(typeRect, typeContent, EditorStyles.miniLabel);
                GUI.color = c;
            }
        }

        void DrawReferenceIcons(Rect r)
        {
            if (sourceRefs?.Count > 0)
            {
                DrawComponentIcons(r, sourceRefs, (refInfo, iconRect) => {
                    EditorGUIUtility.PingObject(refInfo.sourceComponent);
                    AssetFinderUnity.PingAndHighlight(refInfo.sourceComponent, refInfo.propertyPath); 
                    GUIUtility.ExitGUI();
                });
            }
            else if (backwardRefs?.Count > 0)
            {
                DrawComponentIcons(r, backwardRefs, (refInfo, iconRect) => {
                    EditorGUIUtility.PingObject(refInfo.sourceComponent);
                    AssetFinderUnity.PingAndHighlight(refInfo.sourceComponent, refInfo.propertyPath); 
                    GUIUtility.ExitGUI();
                });
            }
        }

        void DrawComponentIcons(Rect r, List<SceneRefInfo> refInfos, Action<SceneRefInfo, Rect> onIconClick)
        {
            if (refInfos == null || refInfos.Count == 0) return;
            
            float width = 18f;
            float totalWidth = width * refInfos.Count;
            
            if (refInfos.Count == 1)
            {
                var refInfo = refInfos[0];
                var beautifiedPath = BeautifyPropertyPath(refInfo.propertyPath);
                var labelWidth = EditorStyles.miniLabel.CalcSize(new GUIContent(beautifiedPath)).x;
                totalWidth += labelWidth + 4f;
            }
            
            float startX = r.x + r.width - totalWidth;
            
            for (int i = 0; i < refInfos.Count; i++)
            {
                var refInfo = refInfos[i];
                var targetComponent = refInfo.GetTargetComponent();
                if (targetComponent == null) continue;
                
                float currentX = startX;
                
                if (refInfos.Count == 1)
                {
                    var beautifiedPath = BeautifyPropertyPath(refInfo.propertyPath);
                    var labelWidth = EditorStyles.miniLabel.CalcSize(new GUIContent(beautifiedPath)).x;
                    var labelRect = new Rect(currentX, r.y, labelWidth, r.height);
                    
                    // Always dim property path, no hover effects
                    Color c = GUI.color;
                    GUI.color = new Color(c.r, c.g, c.b, c.a * 0.6f);
                    GUI.Label(labelRect, beautifiedPath, EditorStyles.miniLabel);
                    GUI.color = c;
                    
                    currentX += labelWidth + 4f;
                }
                else
                {
                    currentX += i * width;
                }
                
                var iconRect = new Rect(currentX, r.y, width, r.height);
                var icon = EditorGUIUtility.ObjectContent(targetComponent, targetComponent.GetType()).image;
                var tooltipText = refInfo.IsBackwardRef 
                    ? refInfo.propertyPath 
                    : $"{refInfo.sourceComponent.GetType().Name}.{refInfo.propertyPath}";
                var content = new GUIContent(icon, tooltipText);
                
                GUI.Label(iconRect, content);
                if (GUI.Button(iconRect, content, EditorStyles.label))
                {
                    onIconClick(refInfo, iconRect);
                }
            }
        }

        string BeautifyPropertyPath(string propertyPath)
        {
            if (string.IsNullOrEmpty(propertyPath)) return "";
            
            string result = propertyPath;
            result = result.Replace(".Array.data[", "[");
            result = result.Replace("].Array.data[", "][");
            
            return result;
        }

        void DrawPingRect(Rect pingRect)
        {
            bool selected = isSelected();
            if ((Event.current.type == EventType.MouseDown) && (Event.current.button == 0))
            {
                if (pingRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.control || Event.current.command)
                    {
                        if (selected)
                        {
                            AssetFinderBookmark.Remove(this);
                        } else
                        {
                            AssetFinderBookmark.Add(this);
                        }
                    } else
                    {
                        // Set smart lock state for scene ping to allow selection change
                        var fr2Window = EditorWindow.GetWindow<AssetFinderWindowAll>(false, null, false);
                        if (fr2Window != null && fr2Window.smartLock != null)
                        {
                            fr2Window.smartLock.SetPingLockState(AssetFinderSmartLock.PingLockState.Scene);
                        }
                        
                        EditorGUIUtility.PingObject(component);
                    }
                    Event.current.Use();
                }
            }
        }

        public static Dictionary<string, AssetFinderRef> FindSceneUseSceneObjects(GameObject[] targets)
        {
            return AssetFinderSceneCache.FindSceneUseSceneObjects(targets);
        }

        public static Dictionary<string, AssetFinderRef> FindSceneBackwardReferences(GameObject[] targets)
        {
            return AssetFinderSceneCache.FindSceneBackwardReferences(targets);
        }

        public static Dictionary<string, AssetFinderRef> FindSceneInScene(GameObject[] targets)
        {
            return AssetFinderSceneCache.FindSceneInScene(targets);
        }

        public static Dictionary<string, AssetFinderRef> FindRefInScene(
            string[] assetGUIDs, bool depth,
            Action<Dictionary<string, AssetFinderRef>> onComplete)
        {
            cacheAssetGuids = assetGUIDs;
            onFindRefInSceneComplete = onComplete;
            if (AssetFinderSceneCache.isReady)
            {
                FindRefInScene();
            } else
            {
                AssetFinderSceneCache.onReady -= FindRefInScene;
                AssetFinderSceneCache.onReady += FindRefInScene;
            }

            return refs;
        }

        private static void FindRefInScene()
        {
            if (refs == null) refs = new Dictionary<string, AssetFinderRef>();
            else refs.Clear(); // Reuse existing dictionary
            
            for (var i = 0; i < cacheAssetGuids.Length; i++)
            {
                AssetFinderAsset asset = AssetFinderCache.Api.Get(cacheAssetGuids[i]);
                if (asset == null) continue;

                Add(refs, asset, 0);
                ApplyFilter(refs, asset);
            }

            if (onFindRefInSceneComplete != null) onFindRefInSceneComplete(refs);
            AssetFinderSceneCache.onReady -= FindRefInScene;
        }

        private static void ApplyFilter(Dictionary<string, AssetFinderRef> refs, AssetFinderAsset asset)
        {
            string targetPath = AssetDatabase.GUIDToAssetPath(asset.guid);
            if (string.IsNullOrEmpty(targetPath)) return;

            if (targetPath != asset.assetPath) asset.MarkAsDirty();

            UnityObject target = AssetDatabase.LoadAssetAtPath(targetPath, typeof(UnityObject));
            if (target == null) return;

            if (target is GameObject)
            {
                foreach (GameObject item in AssetFinderUnity.getAllObjsInCurScene())
                {
                    if (AssetFinderUnity.CheckIsPrefab(item))
                    {
                        string itemGUID = AssetFinderUnity.GetPrefabParent(item);
                        if (itemGUID == asset.guid) Add(refs, item, 1);
                    }
                }
            }

            // Search through all cached components for references to this asset
            foreach (var cacheEntry in AssetFinderSceneCache.Api.cache)
            {
                foreach (var hashValue in cacheEntry.Value)
                {
                    if (targetPath == AssetDatabase.GetAssetPath(hashValue.target))
                    {
                        Add(refs, cacheEntry.Key, 1);
                        break;
                    }
                }
            }
        }

        private static void Add(Dictionary<string, AssetFinderRef> refs, AssetFinderAsset asset, int depth)
        {
            string targetId = asset.guid;
            if (!refs.ContainsKey(targetId)) refs.Add(targetId, new AssetFinderRef(0, depth, asset, null));
        }

        private static void Add(Dictionary<string, AssetFinderRef> refs, UnityObject target, int depth)
        {
            if (target == null) return;
            var targetId = target.GetInstanceID().ToString();
            if (!refs.ContainsKey(targetId)) refs.Add(targetId, new AssetFinderSceneRef(depth, target));
        }
    }
    

    

}
