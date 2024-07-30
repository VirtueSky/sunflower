using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace Virtuesky.FolderIcon.Editor
{
    public class IconDictionaryCreator : AssetPostprocessor
    {
        private const string AssetsPath = "/VirtueSky/FolderIcon/Icons";
        internal static Dictionary<string, Texture> IconDictionary;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (!ContainsIconAsset(importedAssets) &&
                !ContainsIconAsset(deletedAssets) &&
                !ContainsIconAsset(movedAssets) &&
                !ContainsIconAsset(movedFromAssetPaths))
            {
                return;
            }

            BuildDictionary();
        }

        private static bool ContainsIconAsset(string[] assets)
        {
            foreach (string str in assets)
            {
                if (ReplaceSeparatorChar(Path.GetDirectoryName(str)) ==
                    FileExtension.GetPathInCurrentEnvironent(AssetsPath))
                {
                    return true;
                }
            }

            return false;
        }

        private static string ReplaceSeparatorChar(string path)
        {
            return path.Replace("\\", "/");
        }

        internal static void BuildDictionary()
        {
            var dictionary = new Dictionary<string, Texture>();
            var folderIconSettings = CreateAsset.GetScriptableAsset<FolderIconSettings>();
            if (folderIconSettings != null)
            {
                if (folderIconSettings.setupIconDefault)
                {
                    var dir = new DirectoryInfo(FileExtension.GetPathInCurrentEnvironent(AssetsPath));
                    FileInfo[] info = dir.GetFiles("*.png");
                    foreach (FileInfo f in info)
                    {
                        var texture =
                            (Texture)AssetDatabase.LoadAssetAtPath(
                                FileExtension.GetPathInCurrentEnvironent($"{AssetsPath}/{f.Name}"),
                                typeof(Texture2D));
                        dictionary.Add(Path.GetFileNameWithoutExtension(f.Name), texture);
                    }
                }

                if (folderIconSettings.customIcon)
                {
                    foreach (var folderIconData in folderIconSettings.folderIconDatas)
                    {
                        if (folderIconData == null) continue;
                        if (folderIconData.icon == null || folderIconData.folderName == null) continue;
                        dictionary.TryAdd(folderIconData.folderName, folderIconData.icon);
                    }
                }
            }

            IconDictionary = dictionary;
        }
    }
}