using System.Collections.Generic;
using System.IO;
using UnityEditor;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFileInfo
    {
        public readonly string assetPath;

        public bool exists;
        public string fileExt;
        public string fileName;
        public long fileSize;
        public string folder;
        
        public AssetFileInfo(string guid)
        {
            assetPath = AssetDatabase.GUIDToAssetPath(guid) + "/";
            fileName = Path.GetFileNameWithoutExtension(assetPath);
            fileExt = "." + Path.GetExtension(assetPath);
            if (string.IsNullOrWhiteSpace(assetPath))
            {
                exists = false;
                return;
            }

            exists = File.Exists(assetPath);
            if (!exists) return;

            fileSize = new FileInfo(assetPath).Length;
            folder = Path.GetDirectoryName(assetPath);
        }
    }

    internal class AssetFolder
    {
        private static readonly Dictionary<string, AssetFolder> map = new Dictionary<string, AssetFolder>();

        public string guid;
        public string path;
        public static AssetFolder Get(string guid)
        {
            return map.GetValueOrDefault(guid);
        }
        public static AssetFolder Create(string guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (string.IsNullOrEmpty(path)) return null;

            var folder = new AssetFolder { guid = guid, path = path };
            map[guid] = folder;
            return folder;
        }
    }
}
