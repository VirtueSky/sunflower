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
        internal static FolderIconSettings folderIconSettings;

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
                    FileExtension.GetPathFileInCurrentEnvironment(AssetsPath))
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
            if (folderIconSettings == null)
            {
                folderIconSettings = CreateAsset.GetScriptableAsset<FolderIconSettings>();
            }


            if (folderIconSettings != null && folderIconSettings.enableFolderIcons &&
                folderIconSettings.enableAutoCustomIconsDefault)
            {
                var dir = new DirectoryInfo(FileExtension.GetPathFolderInCurrentEnvironment(AssetsPath));
                FileInfo[] info = dir.GetFiles("*.png");
                foreach (FileInfo f in info)
                {
                    var texture =
                        (Texture)AssetDatabase.LoadAssetAtPath(
                            FileExtension.GetPathFileInCurrentEnvironment($"{AssetsPath}/{f.Name}"),
                            typeof(Texture2D));
                    if (texture == null) continue;
                    if (!folderIconSettings.folderIconsDictionary.ContainsKey(Path.GetFileNameWithoutExtension(f.Name)))
                    {
                        folderIconSettings.folderIconsDictionary.Add(Path.GetFileNameWithoutExtension(f.Name), texture);
                    }
                }
            }
        }
    }
}