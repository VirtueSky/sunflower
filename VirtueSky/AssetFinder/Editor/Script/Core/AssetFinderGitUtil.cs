using System.IO;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class AssetFinderGitUtil
    {
        private static string gitRootPath;

        public static bool IsGitProject()
        {
            if (!string.IsNullOrEmpty(gitRootPath)) return true;

            string currentPath = Application.dataPath;
            DirectoryInfo dir = new DirectoryInfo(currentPath);

            var maxDepth = 10;
            while (dir != null && maxDepth > 0) // Prevent infinite loop
            {
                maxDepth--;
                if (Directory.Exists(Path.Combine(dir.FullName, ".git")))
                {
                    gitRootPath = dir.FullName;
                    return true;
                }
                dir = dir.Parent;
            }

            return false;
        }

        public static bool CheckGitIgnoreContainsFR2Cache()
        {
            if (string.IsNullOrEmpty(gitRootPath)) IsGitProject();
            if (string.IsNullOrEmpty(gitRootPath)) return false;

            string gitIgnorePath = Path.Combine(gitRootPath, ".gitignore");
            if (!File.Exists(gitIgnorePath)) return false;

            string[] lines = File.ReadAllLines(gitIgnorePath);
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine) || trimmedLine.StartsWith("#")) continue;
                
                if (trimmedLine == "**/AssetFinderCache.asset*" || trimmedLine == "AssetFinderCache.asset*" || trimmedLine == "*AssetFinderCache.asset*")
                {
                    return true;
                }
            }
            
            return false;
        }

        public static void AddFR2CacheToGitIgnore()
        {
            try
            {
                string content = File.Exists(".gitignore") ? File.ReadAllText(".gitignore") : "";
                
                // Make sure the file ends with a newline
                if (!string.IsNullOrEmpty(content) && !content.EndsWith("\n"))
                {
                    content += "\n";
                }
                
                content += "**/AssetFinderCache.asset*\n";
                File.WriteAllText(".gitignore", content);
            }
            catch (System.Exception e)
            {
                AssetFinderLOG.LogError($"Failed to update .gitignore: {e.Message}");
            }
        }
    }
} 