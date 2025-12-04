using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    public abstract partial class AssetUI
    {
        internal static readonly FolderUI NO_PARENT = new FolderUI(string.Empty, string.Empty);
        
        // Cache
        internal static readonly Dictionary<string, AssetUI> cache = new Dictionary<string, AssetUI>();
        internal static AssetUI Get(string guid, bool autoNew = false)
        {
            AssetUI result = cache.GetValueOrDefault(guid);
            if (!autoNew || result != null) return result;

            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return null;
            
            bool isFolder = AssetDatabase.IsValidFolder(path);
            result = isFolder ? (AssetUI)new FolderUI(guid, path) : (AssetUI)new FileUI(guid, path);
            cache[guid] = result;
            return result;
        }

        internal static T Get<T>(string guid, bool autoNew = false) where T: AssetUI
        {
            return Get(guid, autoNew) as T;
        }
        
        
        // Instance variables
        public string path;
        public readonly string guid;
        [NonSerialized] internal AssetUI _parent;
        
        public void RefreshPath()
        {
            path = AssetDatabase.GUIDToAssetPath(guid);
            _parent = null;
            
            // also clear cached content
            nameContent?.Clear();
            pathContent?.Clear();
        }
        
        public AssetUI Parent
        {
            get
            {
                if (_parent != null) return _parent;
                int idx = path.LastIndexOf("/", StringComparison.Ordinal);
                if (idx == -1) return _parent = NO_PARENT;
                
                string pPath = path.Substring(0, idx);
                string pGUID = AssetDatabase.AssetPathToGUID(pPath);
                _parent = Get(pGUID, true) ?? NO_PARENT;
                return _parent;
            }
        }
        
        protected AssetUI(string guid, string path)
        {
            this.guid = guid;
            this.path = path;
        }
    }

    internal class AssetNameContent
    {
        internal GUIContent nameContent;
        internal GUIContent extContent;
        internal float nameWidth;
        internal float extWidth;
        internal bool isValid => nameContent != null;
        internal bool hasExt => extContent != null;
        
        internal void Clear()
        {
            nameContent = null;
            extContent = null;
            nameWidth = 0;
            extWidth = 0;
        }
        
        internal void Refresh(string path)
        {
            string assetName = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path);
            
            // do not use tooltip (as many files with the same name may shared this content)
            nameContent = AssetFinderGUIContent.FromString(assetName); 
            nameWidth = EditorStyles.label.CalcSize(nameContent).x;
            extContent = string.IsNullOrEmpty(ext) ? null : AssetFinderGUIContent.FromString(ext);
            extWidth = string.IsNullOrEmpty(ext) ? 0 : EditorStyles.label.CalcSize(extContent).x;
        }
    }
    
    internal class AssetPathContent
    {
        internal GUIContent pathContent;
        internal float pathWidth;
        internal bool isValid => pathContent != null;
        internal void Clear()
        {
            pathContent = null;
            pathWidth = 0;
        }
        
        internal void Refresh(string path)
        {            
            // do not use tooltip (as many files with the same name may shared this content)
            pathContent = AssetFinderGUIContent.FromString(path); 
            pathWidth = EditorStyles.label.CalcSize(pathContent).x;
        }
    }
    
    

    partial class AssetUI // GUID Content
    {
        // CONST
        public const float GUID_WIDTH = 250f;
        internal GUIContent guidContent;
        
        public bool DrawGuid(ref Rect rect)
        {
            if (guidContent == null) guidContent = new GUIContent(guid, "Click to copy GUID");
            Rect guidRect = GUI2.LeftRect(GUID_WIDTH, ref rect);
            if (!GUI.Button(guidRect, guidContent)) return false;
            
            // Clicked
            EditorGUIUtility.systemCopyBuffer = guid;
            AssetFinderLOG.Log("Copied AssetFinderID Value: " + EditorGUIUtility.systemCopyBuffer);
            return true;
        }
    }

    partial class AssetUI
    {
        [NonSerialized] internal AssetNameContent nameContent;
        [NonSerialized] internal AssetPathContent pathContent;
        
        public void DrawAssetPath(ref Rect rect)
        {
            if (this == NO_PARENT) return;
            if (pathContent == null) pathContent = new AssetPathContent();
            if (!pathContent.isValid) pathContent.Refresh(path);
            
            using (AssetFinderGUI.Color(GUI.color.Alpha(0.5f)))
            {
                Rect pathRect = GUI2.LeftRect(pathContent.pathWidth, ref rect); 
                GUI.Label(pathRect, pathContent.pathContent, EditorStyles.label);    
            }
        }
        
        public void DrawAsset(ref Rect rect, bool withExt, bool withPath)
        {
            if (withPath)
            {
                AssetUI p = Parent;
                if (p != NO_PARENT) p.DrawAssetPath(ref rect);
            }
            
            // Draw AssetIcon
            Rect iconRect = GUI2.LeftRect(16f, ref rect);
            if (Event.current.type == EventType.Repaint)
            {
                Texture icon = AssetDatabase.GetCachedIcon(path);
                if (icon != null) GUI.DrawTexture(iconRect, icon, ScaleMode.ScaleToFit);
            }
            
            // Draw AssetName
            if (nameContent == null) nameContent = new AssetNameContent();
            if (!nameContent.isValid) nameContent.Refresh(path);
            
            Rect nameRect = GUI2.LeftRect(nameContent.nameWidth, ref rect);
            GUI.Label(nameRect, nameContent.nameContent, EditorStyles.label);

            // Draw extension
            if (!withExt || !nameContent.hasExt) return;
            using (AssetFinderGUI.Color(GUI.color.Alpha(0.5f)))
            {
                Rect extRect = GUI2.LeftRect(nameContent.extWidth, ref rect);
                GUI.Label(extRect, nameContent.extContent, EditorStyles.label);     
            }
        }
    }
}
