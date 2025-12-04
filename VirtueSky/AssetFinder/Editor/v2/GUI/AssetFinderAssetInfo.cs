using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    public class AssetFinderAssetInfo
    {
        [NonSerialized] internal static readonly Dictionary<string, AssetFinderAssetInfo> infoMap = new Dictionary<string, AssetFinderAssetInfo>();
        
        internal static AssetFinderAssetInfo Get(string guid) => infoMap.GetValueOrDefault(guid);
        internal static AssetFinderAssetInfo GetOrCreate(string guid) => infoMap.GetValueOrDefault(guid) ?? new AssetFinderAssetInfo(guid);
        internal static void Clear() => infoMap.Clear();
        
        public string guid;
        public string assetPath;

        public string folder;
        public string fileName;
        public string fileExt;

        // GUIContent
        public GUIContent folderContent;
        public GUIContent fileNameContent;
        public GUIContent fileExtContent;

        private AssetFinderAssetInfo(string guid)
        {
            this.guid = guid;
            assetPath = AssetDatabase.GUIDToAssetPath(guid);
            folder = System.IO.Path.GetDirectoryName(assetPath) + "/";
            fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
            fileExt = System.IO.Path.GetExtension(assetPath);

            infoMap.Add(guid, this);
        }

        public void RefreshGUIContent()
        {
            folderContent = AssetFinderGUIContent.From(folder);
            fileNameContent = AssetFinderGUIContent.From(fileName);
            fileExtContent = string.IsNullOrEmpty(fileExt) ? GUIContent.none : AssetFinderGUIContent.From(fileExt);
        }
    }
}
