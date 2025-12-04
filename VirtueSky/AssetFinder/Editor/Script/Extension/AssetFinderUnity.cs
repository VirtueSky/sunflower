#if UNITY_5_3_OR_NEWER
#define UNITY_4_3_OR_NEWER
#define UNITY_4_4_OR_NEWER
#define UNITY_4_5_OR_NEWER
#define UNITY_4_6_OR_NEWER
#define UNITY_4_7_OR_NEWER
#define UNITY_5_0_OR_NEWER
#define UNITY_5_1_OR_NEWER
#define UNITY_5_2_OR_NEWER
#else
#if UNITY_5
#define UNITY_4_3_OR_NEWER
#define UNITY_4_4_OR_NEWER
#define UNITY_4_5_OR_NEWER
#define UNITY_4_6_OR_NEWER
#define UNITY_4_7_OR_NEWER
	
#if UNITY_5_0
#define UNITY_5_0_OR_NEWER
#elif UNITY_5_1
#define UNITY_5_0_OR_NEWER
#define UNITY_5_1_OR_NEWER
#elif UNITY_5_2
#define UNITY_5_0_OR_NEWER
#define UNITY_5_1_OR_NEWER
#define UNITY_5_2_OR_NEWER
#endif
#else
#if UNITY_4_3
#define UNITY_4_3_OR_NEWER
#elif UNITY_4_4
#define UNITY_4_3_OR_NEWER
#define UNITY_4_4_OR_NEWER
#elif UNITY_4_5
#define UNITY_4_3_OR_NEWER
#define UNITY_4_4_OR_NEWER
#define UNITY_4_5_OR_NEWER
#elif UNITY_4_6
#define UNITY_4_3_OR_NEWER
#define UNITY_4_4_OR_NEWER
#define UNITY_4_5_OR_NEWER
#define UNITY_4_6_OR_NEWER
#elif UNITY_4_7
#define UNITY_4_3_OR_NEWER
#define UNITY_4_4_OR_NEWER
#define UNITY_4_5_OR_NEWER
#define UNITY_4_6_OR_NEWER
#define UNITY_4_7_OR_NEWER
#endif
#endif
#endif


#if UNITY_5_3_OR_NEWER
#define UNITY_SCENE_MANAGER
#endif

#if AssetFinderADDRESSABLE
using UnityEditor.AddressableAssets;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_SCENE_MANAGER
using UnityEngine.SceneManagement;
using System.IO;
#endif

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderUnity
    {
        internal static bool isEditorPlaying;
        internal static bool isEditorUpdating;
        internal static bool isEditorCompiling;
        internal static bool isEditorPlayingOrWillChangePlaymode;

        private static int highlightCounter = 0;

        public static void RefreshEditorStatus()
        {
            isEditorPlaying = EditorApplication.isPlaying;
            isEditorUpdating = EditorApplication.isUpdating;
            isEditorCompiling = EditorApplication.isCompiling;
            isEditorPlayingOrWillChangePlaymode = EditorApplication.isPlayingOrWillChangePlaymode;
        }

        public static HashSet<string> _Selection_AssetGUIDs;

        public static bool StringStartsWith(string source, params string[] prefixes)
        {
            if (string.IsNullOrEmpty(source)) return false;
            for (var i = 0; i < prefixes.Length; i++)
            {
                if (source.StartsWith(prefixes[i])) return true;
            }

            return false;
        }

        public static void SplitPath(string assetPath, out string assetName, out string assetExtension, out string assetFolder)
        {
            assetName = string.Empty;
            assetFolder = string.Empty;
            assetExtension = string.Empty;

            if (string.IsNullOrEmpty(assetPath)) return;

            assetExtension = Path.GetExtension(assetPath);
            assetName = Path.GetFileNameWithoutExtension(assetPath);
            int lastSlash = assetPath.LastIndexOf("/", StringComparison.Ordinal) + 1;
            assetFolder = assetPath.Substring(0, lastSlash);

            // Debug.Log($"{assetPath} --> \n{assetName}\n{assetExtension}\n{assetFolder}");
        }

        public static string[] Selection_AssetGUIDs
        {
            get
            {
#if UNITY_5_0_OR_NEWER
                Object[] objs = Selection.objects;

                _Selection_AssetGUIDs = new HashSet<string>();
                foreach (Object item in objs)
                {
#if UNITY_2018_1_OR_NEWER
                    {
                        var guid = "";
                        long fileid = -1;

                        try
                        { // missing references will cause null exception
                            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(item, out guid, out fileid)) _Selection_AssetGUIDs.Add(guid + "/" + fileid);

                            //Debug.Log("guid: " + guid + "  fileID: " + fileid);
                        } catch { }

                    }
#else
	                {
                    	var path = AssetDatabase.GetAssetPath(item);
                        if (string.IsNullOrEmpty(path)) continue;
                        var guid = AssetDatabase.AssetPathToGUID(path);
                        System.Reflection.PropertyInfo inspectorModeInfo =
                        typeof(SerializedObject).GetProperty("inspectorMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                        SerializedObject serializedObject = new SerializedObject(item);
                        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

                        SerializedProperty localIdProp =
                            serializedObject.FindProperty("m_LocalIdentfierInFile");   //note the misspelling!

                        var localId = localIdProp.longValue;
                        if (localId <= 0)
                        {
                            localId = localIdProp.intValue;
                        }
                        if (localId <= 0)
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(guid)) _Selection_AssetGUIDs.Add(guid + "/" + localId);
	                }
#endif

                }


                return Selection.assetGUIDs;
#else
			var mInfo =
 typeof(Selection).GetProperty("assetGUIDs", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
			if (mInfo != null){
				return (string[]) mInfo.GetValue(null, null);
			}
			AssetFinderLOG.LogWarning("Unity changed ! Selection.assetGUIDs not found !");
		    return new string[0];
#endif
            }
        }

        public static void PingAndHighlight(
            Component component,
            string propertyPath,
            float glowSeconds = 5f)
        {
            if (component == null) return;
            
            // auto-lock
            var window = EditorWindow.focusedWindow as AssetFinderWindowAll;
            if (window != null) window.smartLock.SetPingLockState(AssetFinderSmartLock.PingLockState.Scene);
            
            Selection.activeGameObject = component.gameObject;
            ExpandPropertyPath(component, propertyPath);
            SetEditorsExpanded(component);
            
            EditorApplication.delayCall += () =>
            {
                if (window != null) window.smartLock.SetPingLockState(AssetFinderSmartLock.PingLockState.Scene);
                Selection.activeGameObject = component.gameObject;
                
                EditorApplication.delayCall += () =>
                {
                    highlightCounter++;
                    var currentHighlightId = highlightCounter;
                    var success = TriggerHighlight(propertyPath);
                    if (success)
                    {
                        StopHighlightAt(currentHighlightId, EditorApplication.timeSinceStartup + glowSeconds);
                        return;
                    }
                    
                    Debug.Log($"Can not highlight property <{propertyPath}> in Inspector <{component.GetType().Name}>\nThe Inspector might be locked or the property is hidden\n\n");
                };
            };
        }

        private static bool TriggerHighlight(string propertyPath)
        {
            var result = false;
            using (AssetFinderDev.NoLog)
            {
                result = Highlight(propertyPath);
                if (!result) // Array?
                {
                    var path2 = propertyPath.Replace(".Array.data[", "[");
                    result = Highlight(path2);
                }
                if (!result) result = Highlight(propertyPath.Split('.')[0]);
            }
            return result;
        }
        
        private static bool Highlight(string propertyPath)
        {
            var  result = Highlighter.Highlight("Inspector", propertyPath, HighlightSearchMode.Auto);
            if (!result) result = Highlighter.Highlight("Inspector", ObjectNames.NicifyVariableName(propertyPath), HighlightSearchMode.Auto);
            return result;
        }

        private static void StopHighlightAt(int highlightId, double stopTime)
        {
            void Check()
            {
                if (highlightCounter > highlightId)
                {
                    EditorApplication.update -= Check;
                    return;
                }

                if (EditorApplication.timeSinceStartup < stopTime) return;
                Highlighter.Stop();
                EditorApplication.update -= Check;
            }
            
            EditorApplication.update += Check;
        }
        
        private static void CollapseAllComponentsExcept(Component targetComponent)
        {
            if (targetComponent == null) return;
            var gameObject = targetComponent.gameObject;
            var allComponents = gameObject.GetComponents<Component>();
            foreach (var comp in allComponents)
            {
                var expect = comp == targetComponent;
                var before = InternalEditorUtility.GetIsInspectorExpanded(comp);
                InternalEditorUtility.SetIsInspectorExpanded(comp, expect);
                var after = InternalEditorUtility.GetIsInspectorExpanded(comp);

                if (after != expect)
                {
                    Debug.LogWarning($"comp: {comp.GetType().Name} |  before: {before} --> after: {InternalEditorUtility.GetIsInspectorExpanded(comp)} | expect: {expect}");    
                }
            }
        }

        private static void DoExpand(Component c)
        {
            var tracker = ActiveEditorTracker.sharedTracker;
            for (var i = 0; i < tracker.activeEditors.Length; i++)
            {
                var editor = tracker.activeEditors[i];
                var isExpand = editor.target == c;
                InternalEditorUtility.SetIsInspectorExpanded(editor.target, isExpand);
                editor.serializedObject.ApplyModifiedProperties();
                editor.Repaint();
            }
        }

        private static void SetEditorsExpanded(Component c)
        {
            DoExpand(c);
            Selection.activeGameObject = null;
        }
        
        private static void SetEditorsExpanded2(Component c)
        {
            var tracker = ActiveEditorTracker.sharedTracker;
            for (int i = 0; i < tracker.activeEditors.Length; i++)
            {
                var expanded = c == tracker.activeEditors[i].target;
                tracker.SetVisible(i, expanded ? 1 : 0);
            }
            
            var inspectorWindow = GetInspectorWindow();
            if (inspectorWindow != null) inspectorWindow.Repaint();
        }
    
        private static EditorWindow GetInspectorWindow()
        {
            System.Type inspectorType = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            return EditorWindow.GetWindow(inspectorType);
        }
        
        private static void ExpandPropertyPath(Component component, string propertyPath)
        {
            if (component == null) return;
            var serializedObject = new SerializedObject(component);
            serializedObject.Update();
            var prop = serializedObject.GetIterator();
            var deep = true;
            while (prop.NextVisible(deep))
            {
                if (!propertyPath.Contains(prop.propertyPath))
                {
                    prop.isExpanded = false;
                    deep = false;
                    continue;
                }
                
                prop.isExpanded = true;
                deep = true;
            }
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static readonly Lazy<Dictionary<int, string>> HashClassesNormalLazy =
            new Lazy<Dictionary<int, string>>(() => new Dictionary<int, string>
            {
                { 1, "UnityEngine.GameObject" },
                { 2, "UnityEngine.Component" },
                { 4, "UnityEngine.Transform" },
                { 8, "UnityEngine.Behaviour" },
                { 12, "UnityEngine.ParticleAnimator" },
                { 15, "UnityEngine.EllipsoidParticleEmitter" },
                { 20, "UnityEngine.Camera" },
                { 21, "UnityEngine.Material" },
                { 23, "UnityEngine.MeshRenderer" },
                { 25, "UnityEngine.Renderer" },
                { 26, "UnityEngine.ParticleRenderer" },
                { 27, "UnityEngine.Texture" },
                { 28, "UnityEngine.Texture2D" },
                { 33, "UnityEngine.MeshFilter" },
                { 41, "UnityEngine.OcclusionPortal" },
                { 43, "UnityEngine.Mesh" },
                { 45, "UnityEngine.Skybox" },
                { 47, "UnityEngine.QualitySettings" },
                { 48, "UnityEngine.Shader" },
                { 49, "UnityEngine.TextAsset" },
                { 50, "UnityEngine.Rigidbody2D" },
                { 53, "UnityEngine.Collider2D" },
                { 54, "UnityEngine.Rigidbody" },
                { 56, "UnityEngine.Collider" },
                { 57, "UnityEngine.Joint" },
                { 58, "UnityEngine.CircleCollider2D" },
                { 59, "UnityEngine.HingeJoint" },
                { 60, "UnityEngine.PolygonCollider2D" },
                { 61, "UnityEngine.BoxCollider2D" },
                { 62, "UnityEngine.PhysicsMaterial2D" },
                { 64, "UnityEngine.MeshCollider" },
                { 65, "UnityEngine.BoxCollider" },
                { 68, "UnityEngine.EdgeCollider2D" },
                { 72, "UnityEngine.ComputeShader" },
                { 74, "UnityEngine.AnimationClip" },
                { 75, "UnityEngine.ConstantForce" },
                { 81, "UnityEngine.AudioListener" },
                { 82, "UnityEngine.AudioSource" },
                { 83, "UnityEngine.AudioClip" },
                { 84, "UnityEngine.RenderTexture" },
                { 87, "UnityEngine.MeshParticleEmitter" },
                { 88, "UnityEngine.ParticleEmitter" },
                { 89, "UnityEngine.Cubemap" },
                { 90, "Avatar" },
                { 92, "UnityEngine.GUILayer" },
                { 93, "UnityEngine.RuntimeAnimatorController" },
                { 95, "UnityEngine.Animator" },
                { 96, "UnityEngine.TrailRenderer" },
                { 102, "UnityEngine.TextMesh" },
                { 104, "UnityEngine.RenderSettings" },
                { 108, "UnityEngine.Light" },
                { 111, "UnityEngine.Animation" },
                { 114, "UnityEngine.MonoBehaviour" },
                { 115, "UnityEditor.MonoScript" },
                { 117, "UnityEngine.Texture3D" },
                { 119, "UnityEngine.Projector" },
                { 120, "UnityEngine.LineRenderer" },
                { 121, "UnityEngine.Flare" },
                { 123, "UnityEngine.LensFlare" },
                { 124, "UnityEngine.FlareLayer" },
                { 128, "UnityEngine.Font" },
                { 129, "UnityEditor.PlayerSettings" },
                { 131, "UnityEngine.GUITexture" },
                { 132, "UnityEngine.GUIText" },
                { 133, "UnityEngine.GUIElement" },
                { 134, "UnityEngine.PhysicMaterial" },
                { 135, "UnityEngine.SphereCollider" },
                { 136, "UnityEngine.CapsuleCollider" },
                { 137, "UnityEngine.SkinnedMeshRenderer" },
                { 138, "UnityEngine.FixedJoint" },
                { 142, "UnityEngine.AssetBundle" },
                { 143, "UnityEngine.CharacterController" },
                { 144, "UnityEngine.CharacterJoint" },
                { 145, "UnityEngine.SpringJoint" },
                { 146, "UnityEngine.WheelCollider" },
                { 152, "UnityEngine.MovieTexture" },
                { 153, "UnityEngine.ConfigurableJoint" },
                { 154, "UnityEngine.TerrainCollider" },
                { 156, "UnityEngine.TerrainData" },
                { 157, "UnityEngine.LightmapSettings" },
                { 158, "UnityEngine.WebCamTexture" },
                { 159, "UnityEditor.EditorSettings" },
                { 162, "UnityEditor.EditorUserSettings" },
                { 164, "UnityEngine.AudioReverbFilter" },
                { 165, "UnityEngine.AudioHighPassFilter" },
                { 166, "UnityEngine.AudioChorusFilter" },
                { 167, "UnityEngine.AudioReverbZone" },
                { 168, "UnityEngine.AudioEchoFilter" },
                { 169, "UnityEngine.AudioLowPassFilter" },
                { 170, "UnityEngine.AudioDistortionFilter" },
                { 171, "UnityEngine.SparseTexture" },
                { 180, "UnityEngine.AudioBehaviour" },
                { 182, "UnityEngine.WindZone" },
                { 183, "UnityEngine.Cloth" },
                { 192, "UnityEngine.OcclusionArea" },
                { 193, "UnityEngine.Tree" },
                { 198, "UnityEngine.ParticleSystem" },
                { 199, "UnityEngine.ParticleSystemRenderer" },
                { 200, "UnityEngine.ShaderVariantCollection" },
                { 205, "UnityEngine.LODGroup" },
                { 207, "UnityEngine.Motion" },
                { 212, "UnityEngine.SpriteRenderer" },
                { 213, "UnityEngine.Sprite" },
                { 215, "UnityEngine.ReflectionProbe" },
                { 218, "UnityEngine.Terrain" },
                { 220, "UnityEngine.LightProbeGroup" },
                { 221, "UnityEngine.AnimatorOverrideController" },
                { 222, "UnityEngine.CanvasRenderer" },
                { 223, "UnityEngine.Canvas" },
                { 224, "UnityEngine.RectTransform" },
                { 225, "UnityEngine.CanvasGroup" },
                { 226, "UnityEngine.BillboardAsset" },
                { 227, "UnityEngine.BillboardRenderer" },
                { 229, "UnityEngine.AnchoredJoint2D" },
                { 230, "UnityEngine.Joint2D" },
                { 231, "UnityEngine.SpringJoint2D" },
                { 232, "UnityEngine.DistanceJoint2D" },
                { 233, "UnityEngine.HingeJoint2D" },
                { 234, "UnityEngine.SliderJoint2D" },
                { 235, "UnityEngine.WheelJoint2D" },
                { 246, "UnityEngine.PhysicsUpdateBehaviour2D" },
                { 247, "UnityEngine.ConstantForce2D" },
                { 248, "UnityEngine.Effector2D" },
                { 249, "UnityEngine.AreaEffector2D" },
                { 250, "UnityEngine.PointEffector2D" },
                { 251, "UnityEngine.PlatformEffector2D" },
                { 252, "UnityEngine.SurfaceEffector2D" },
                { 258, "UnityEngine.LightProbes" },
                { 290, "UnityEngine.AssetBundleManifest" },
                { 1003, "UnityEditor.AssetImporter" },
                { 1004, "UnityEditor.AssetDatabase" },
                { 1006, "UnityEditor.TextureImporter" },
                { 1007, "UnityEditor.ShaderImporter" },
                { 1011, "UnityEngine.AvatarMask" },
                { 1020, "UnityEditor.AudioImporter" },
                { 1029, "UnityEditor.DefaultAsset" },
                { 1032, "UnityEditor.SceneAsset" },
                { 1035, "UnityEditor.MonoImporter" },
                { 1040, "UnityEditor.ModelImporter" },
                { 1042, "UnityEditor.TrueTypeFontImporter" },
                { 1044, "UnityEditor.MovieImporter" },
                { 1045, "UnityEditor.EditorBuildSettings" },
                { 1050, "UnityEditor.PluginImporter" },
                { 1051, "UnityEditor.EditorUserBuildSettings" },
                { 1105, "UnityEditor.HumanTemplate" },
                { 1110, "UnityEditor.SpeedTreeImporter" },
                { 1113, "UnityEditor.LightmapParameters" }
            });

        public static Dictionary<int, string> HashClassesNormal => HashClassesNormalLazy.Value;

        //private static Texture2D _whiteTexture;
        //public static Texture2D whiteTexture {
        //	get {
        //		return EditorGUIUtility.whiteTexture;

        //		#if UNITY_5_0_OR_NEWER
        //		return EditorGUIUtility.whiteTexture;
        //		#else
        //		if (_whiteTexture != null) return _whiteTexture;
        //		_whiteTexture = new Texture2D(1,1, TextureFormat.RGBA32, false);
        //        _whiteTexture.SetPixel(0, 0, Color.white);
        //		_whiteTexture.hideFlags = HideFlags.DontSave;
        //		return _whiteTexture;
        //		#endif
        //	}
        //}

        public static T LoadAssetAtPath<T>(string path) where T : Object
        {
#if UNITY_5_1_OR_NEWER
            return AssetDatabase.LoadAssetAtPath<T>(path);
#else
			return (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
#endif
        }

        public static void SetWindowTitle(EditorWindow window, string title)
        {
#if UNITY_5_1_OR_NEWER
            window.titleContent = AssetFinderGUIContent.FromString(title);
#else
	        window.title = title;
#endif
        }

        public static void GetCompilingPhase(string path, out bool isPlugin, out bool isEditor)
        {
#if (UNITY_5_2_0 || UNITY_5_2_1) && !UNITY_5_2_OR_NEWER
			bool oldSystem = true;
#else
            var oldSystem = false;
#endif

            // ---- Old system: Editor for the plugin should be Plugins/Editor
            if (oldSystem)
            {
                bool isPluginEditor = path.StartsWith("Assets/Plugins/Editor/", StringComparison.Ordinal)
                    || path.StartsWith("Assets/Standard Assets/Editor/", StringComparison.Ordinal)
                    || path.StartsWith("Assets/Pro Standard Assets/Editor/",
                        StringComparison.Ordinal);

                if (isPluginEditor)
                {
                    isPlugin = true;
                    isEditor = true;
                    return;
                }
            }

            isPlugin = path.StartsWith("Assets/Plugins/", StringComparison.Ordinal)
                || path.StartsWith("Assets/Standard Assets/", StringComparison.Ordinal)
                || path.StartsWith("Assets/Pro Standard Assets/", StringComparison.Ordinal);

            isEditor = oldSystem && isPlugin ? false : path.Contains("/Editor/");
        }

        public static T LoadAssetWithGUID<T>(string guid) where T : Object
        {
            if (string.IsNullOrEmpty(guid)) return null;

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return null;

#if UNITY_5_1_OR_NEWER
            return AssetDatabase.LoadAssetAtPath<T>(path);
#else
			return (T)AssetDatabase.LoadAssetAtPath(path, typeof(T));
#endif
        }

        public static void UnloadUnusedAssets()
        {
#if UNITY_5_0_OR_NEWER
            EditorUtility.UnloadUnusedAssetsImmediate();
#else
			EditorUtility.UnloadUnusedAssets();
#endif
            Resources.UnloadUnusedAssets();
        }

        internal static int Epoch(DateTime time)
        {
            return (int)(time.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
        }

        internal static bool DrawToggle(ref bool v, string label)
        {
            bool v1 = GUILayout.Toggle(v, label);
            if (v1 != v)
            {
                v = v1;
                return true;
            }

            return false;
        }

        internal static bool DrawToggleToolbar(ref bool v, string label, float width)
        {
            bool v1 = GUILayout.Toggle(v, label, EditorStyles.toolbarButton, GUILayout.Width(width));
            if (v1 != v)
            {
                v = v1;
                return true;
            }

            return false;
        }

        internal static bool DrawToggleToolbar(ref bool v, GUIContent icon, float width)
        {
            bool v1 = GUILayout.Toggle(v, icon, EditorStyles.toolbarButton, GUILayout.Width(width));
            if (v1 != v)
            {
                v = v1;
                return true;
            }

            return false;
        }

        public static string GetAddressable(string guid)
        {
#if AssetFinderADDRESSABLE
			var aaSettings = AddressableAssetSettingsDefaultObject.GetSettings(true);
			var entry = aaSettings.FindAssetEntry(guid);
			return entry != null ? entry.address : string.Empty;
#endif

            return null;
        }

        internal static EditorWindow FindEditor(string className)
        {
            EditorWindow[] list = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (EditorWindow item in list)
            {
                if (item.GetType().FullName == className) return item;
            }

            return null;
        }

        internal static void RepaintAllEditor(string className)
        {
            EditorWindow[] list = Resources.FindObjectsOfTypeAll<EditorWindow>();

            foreach (EditorWindow item in list)
            {
#if AssetFinderDEV
			Debug.Log(item.GetType().FullName);
#endif

                if (item.GetType().FullName != className) continue;

                item.Repaint();
            }
        }

        internal static void RepaintProjectWindows()
        {
            RepaintAllEditor("UnityEditor.ProjectBrowser");
        }

        internal static void RepaintFR2Windows()
        {
            RepaintAllEditor("vietlabs.fr2.AssetFinderWindow");
        }

        internal static void ExportSelection()
        {
            Type packageExportT = null;

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                packageExportT = assembly.GetType("UnityEditor.PackageExport");
                if (packageExportT != null) break;
            }

            if (packageExportT == null)
            {
                AssetFinderLOG.LogWarning("Export Package Error : UnityEditor.PackageExport not found !");
                return;
            }

            EditorWindow panel = EditorWindow.GetWindow(packageExportT, true, "Exporting package");
#if UNITY_5_2_OR_NEWER
            var prop = "m_IncludeDependencies";
#else
			var prop = "m_bIncludeDependencies";
#endif

            FieldInfo fieldInfo = packageExportT.GetField(prop, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
            {
                AssetFinderLOG.LogWarning("Export Package error : " + prop + " not found !");
                return;
            }

            MethodInfo methodInfo =
                packageExportT.GetMethod("BuildAssetList", BindingFlags.NonPublic | BindingFlags.Instance);
            if (methodInfo == null)
            {
                AssetFinderLOG.LogWarning("Export Package error : BuildAssetList method not found !");
                return;
            }

            fieldInfo.SetValue(panel, false);
            methodInfo.Invoke(panel, null);
            panel.Repaint();
        }


        public static Type GetType(string typeName)
        {
            var type = Type.GetType(typeName);
            if (type != null) return type;

            foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
            {
                type = a.GetType(typeName);
                if (type != null) return type;
            }

            return null;
        }

        public static IEnumerable<Transform> GetAllChild(Transform root)
        {
            yield return root;
            if (root.childCount <= 0) yield break;

            for (var i = 0; i < root.childCount; i++)
            {
                foreach (Transform item in GetAllChild(root.GetChild(i)))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<GameObject> getAllObjsInCurScene()
        {
#if UNITY_SCENE_MANAGER
            for (var j = 0; j < SceneManager.sceneCount; j++)
            {
                Scene scene = SceneManager.GetSceneAt(j);
                foreach (GameObject item in GetGameObjectsInScene(scene))
                {
                    yield return item;
                }
            }

            if (EditorApplication.isPlaying)
            {
                //dont destroy scene
                GameObject temp = null;
                try
                {
                    temp = new GameObject();
                    Object.DontDestroyOnLoad(temp);
                    Scene dontDestroyOnLoad = temp.scene;
                    Object.DestroyImmediate(temp);
                    temp = null;

                    foreach (GameObject item in GetGameObjectsInScene(dontDestroyOnLoad))
                    {
                        yield return item;
                    }
                } finally
                {
                    if (temp != null) Object.DestroyImmediate(temp);
                }
            }
#else
			foreach (Transform obj in Resources.FindObjectsOfTypeAll(typeof(Transform)))
            {
				GameObject o = obj.gameObject;
               yield return o;
            }
#endif
        }
#if UNITY_SCENE_MANAGER
        private static IEnumerable<GameObject> GetGameObjectsInScene(Scene scene)
        {
            var rootObjects = new List<GameObject>();
            if (!scene.isLoaded) yield break;

            scene.GetRootGameObjects(rootObjects);

            // iterate root objects and do something
            for (var i = 0; i < rootObjects.Count; ++i)
            {
                GameObject gameObject = rootObjects[i];

                foreach (GameObject item in getAllChild(gameObject))
                {
                    yield return item;
                }

                yield return gameObject;
            }
        }
#endif
        public static IEnumerable<GameObject> getAllChild(GameObject target, bool returnMe = false)
        {
            return target.GetAllChildren(returnMe);
        }

        public static IEnumerable<Object> GetAllRefObjects(GameObject obj)
        {
            return obj.GetAllObjectReferences();
        }

        public static int StringMatch(string pattern, string input)
        {
            if (string.IsNullOrEmpty(pattern) || string.IsNullOrEmpty(input)) return 0;

            pattern = pattern.ToLower();
            input = input.ToLower();

            if (input.Contains(pattern)) return 100;

            int score = 0;
            int patternIdx = 0;

            for (int i = 0; i < input.Length && patternIdx < pattern.Length; i++)
            {
                if (input[i] == pattern[patternIdx])
                {
                    score += 10;
                    patternIdx++;
                }
            }

            return patternIdx == pattern.Length ? score : 0;
        }
        
#if AssetFinderDEBUG
		[MenuItem("Tools/Test Prefab")]
		static void TestPrefab()
		{
			GetPrefabParent(Selection.activeGameObject);
		}
#endif

        public static string GetPrefabParent(Object obj)
        {
            if (obj is GameObject go) return GetPrefabGUID(go);
            if (obj is Component comp) return GetPrefabGUID(comp.gameObject);
            return string.Empty;
        }

        public static string GetGameObjectPath(GameObject obj, bool includeMe = true)
        {
            return GetHierarchyPath(obj, includeMe);
        }

        public static bool CheckIsPrefab(GameObject obj)
        {
            return IsPrefabInstance(obj);
        }

        private static string GetPrefabGUID(GameObject obj)
        {
#if UNITY_2020_3_OR_NEWER
            var prefabParent = PrefabUtility.GetCorrespondingObjectFromSource(obj);
            if (prefabParent != null)
            {
                return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefabParent));
            }
#elif UNITY_2018_3_OR_NEWER
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if (prefabStage != null && prefabStage.prefabContentsRoot == obj)
            {
                return AssetDatabase.AssetPathToGUID(prefabStage.prefabAssetPath);
            }

            var prefabRoot = PrefabUtility.GetOutermostPrefabInstanceRoot(obj);
            if (prefabRoot != null)
            {
                var prefabAsset = PrefabUtility.GetCorrespondingObjectFromSource(prefabRoot);
                if (prefabAsset != null)
                {
                    return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefabAsset));
                }
            }
#else
            var prefabParent = PrefabUtility.GetPrefabParent(obj);
            if (prefabParent != null)
            {
                return AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefabParent));
            }
#endif
            return string.Empty;
        }

        private static string GetHierarchyPath(GameObject obj, bool includeMe = true)
        {
            if (obj == null) return string.Empty;

            var path = string.Empty;
            var current = obj.transform;

            while (current != null)
            {
                if (current == obj.transform && !includeMe)
                {
                    current = current.parent;
                    continue;
                }

                path = current.name + (string.IsNullOrEmpty(path) ? "" : "/" + path);
                current = current.parent;
            }

            return path;
        }

        private static bool IsPrefabInstance(GameObject obj)
        {
#if UNITY_2018_3_OR_NEWER
            return PrefabUtility.IsPartOfPrefabInstance(obj);
#else
            return PrefabUtility.GetPrefabType(obj) == PrefabType.PrefabInstance;
#endif
        }





        public static void Clear<T1, T2>(ref Dictionary<T1, T2> dict)
        {
            GameObjectExtensions.InitializeOrClear(ref dict);
        }

        public static void Clear<T>(ref List<T> list)
        {
            GameObjectExtensions.InitializeOrClear(ref list);
        }

        public static SerializedProperty[] xGetSerializedProperties(Object go, bool processArray)
        {
            var so = new SerializedObject(go);
            return so.GetAllProperties(processArray);
        }

        public static List<SerializedProperty> xGetSOArray(SerializedProperty prop)
        {
            int size = prop.arraySize;
            var result = new List<SerializedProperty>();

            for (var i = 0; i < size; i++)
            {
                SerializedProperty p = prop.GetArrayElementAtIndex(i);

                if (p.isArray)
                {
                    result.AddRange(xGetSOArray(p.Copy()));
                } else
                {
                    result.Add(p.Copy());
                }
            }

            return result;
        }




        public static void BackupAndDeleteAssets(AssetFinderRef[] assets)
        {
            var fileName = DateTime.Now.ToString("yyMMdd_hhmmss");

            AssetFinderRef[] list = assets;
            var result = new List<string>();


            var selectedList = new List<string>();

            foreach (AssetFinderRef item in list)
            {
                if (item.asset == null) continue;
                string oPath = item.asset.assetPath.Replace("\\", "/");
                if (!oPath.StartsWith("Assets/")) continue;
                result.Add(item.asset.assetPath);

                if (item.isSelected()) selectedList.Add(item.asset.assetPath);
            }
            if (selectedList.Count != 0) result = selectedList;
            Directory.CreateDirectory("Library/FR2/");
            AssetDatabase.ExportPackage(result.ToArray(), "Library/FR2/bk_" + fileName + ".unitypackage");

            AssetDatabase.StartAssetEditing();
            try
            {
                for (var i = 0; i < result.Count; i++)
                {
                    AssetDatabase.DeleteAsset(result[i]);
                }
            } finally
            {
                AssetDatabase.StopAssetEditing();
                AssetDatabase.Refresh();
            }

            AssetFinderCache.DelayCheck4Changes();
        }




    }
}
