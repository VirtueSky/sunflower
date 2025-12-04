using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderParser // UIToolkit
    {
        private static readonly Dictionary<string, Func<string, (string, long)>> UI_TOOLKIT_FILES
            = new Dictionary<string, Func<string, (string, long)>>()
            {
                {".tss", ParseLine_Tss},
                {".uxml", ParseLine_Uxml_Uss},
                {".uss", ParseLine_Uxml_Uss}
            };

        private static void ReadContent_UIToolkit(string ext, string assetPath, Action<string, long> callback)
        {
            Func<string, (string, long)> lineParser = UI_TOOLKIT_FILES[ext];
            Read(assetPath, lineParser, callback);
        }
        
        private static (string guid, long fileId) ParseLine_Uxml_Uss(string line)
        {
            // Check for traditional GUID references first
            var result = FindRef(line, "guid=", "fileID=", "&");
            if (result.guid != null) return result;
            
            // Handle UXML Style src references with project://database/ URIs
            if (line.Contains("<Style") && line.Contains("src="))
            {
                string srcPath = Find(line, "src=\"", "\"");
                if (!string.IsNullOrEmpty(srcPath))
                {
                    string resolvedPath = ResolveProjectDatabasePath(srcPath);
                    if (!string.IsNullOrEmpty(resolvedPath))
                    {
                        string guid = AssetDatabase.AssetPathToGUID(resolvedPath);
                        if (!string.IsNullOrEmpty(guid))
                        {
                            // AssetFinderLOG.Log($"[FR2] UXML->USS: {srcPath} -> {resolvedPath} -> {guid}");
                            return (guid, -1);
                        }
                    }
                    else
                    {
                        AssetFinderLOG.LogWarning($"[FR2] Failed to resolve UXML Style src: {srcPath}");
                    }
                }
            }
            
            // Handle resource() references for USS files
            if (line.Contains("resource("))
            {
                string resourcePath = Find(line, "resource(\"", "\")");
                if (string.IsNullOrEmpty(resourcePath))
                {
                    resourcePath = Find(line, "resource('", "')");
                }
                
                if (!string.IsNullOrEmpty(resourcePath))
                {
                    // Unity resource paths can be relative to various locations
                    string guid = ResolveResourcePath(resourcePath);
                    if (!string.IsNullOrEmpty(guid))
                    {
                        return (guid, -1);
                    }
                }
            }
            
            // Handle @import ""; references for USS files : @import "Base.uss";
            if (line.Contains("@import"))
            {
                string importPath = Find(line, "@import \"", "\";");
                if (string.IsNullOrEmpty(importPath))
                {
                    importPath = Find(line, "@import '", "';");
                }
                
                
                if (!string.IsNullOrEmpty(importPath))
                {
                    // Unity resource paths can be relative to various locations
                    string resolvedPath = ResolveProjectDatabasePath(importPath);
                    if (!string.IsNullOrEmpty(resolvedPath))
                    {
                        string guid = AssetDatabase.AssetPathToGUID(resolvedPath);
                        if (!string.IsNullOrEmpty(guid))
                        {
                            // AssetFinderLOG.Log($"[FR2] USS->USS: {importPath} -> {resolvedPath} -> {guid}");
                            return (guid, -1);
                        }
                    }
                    else
                    {
                        AssetFinderLOG.LogWarning($"[FR2] Failed to resolve USS import: {importPath}");
                    }
                }
            }
            
            return (null, -1);
        }
        
        private static string ResolveProjectDatabasePath(string path)
        {
            // Handle project://database/ URIs
            if (path.StartsWith("project://database/"))
            {
                // Remove the project://database/ prefix to get the actual asset path
                string assetPath = path.Substring("project://database/".Length);
                
                // Check if this path exists in the project
                if (System.IO.File.Exists(assetPath))
                {
                    return assetPath;
                }
            }
            
            // full path
            if (System.IO.File.Exists(path)) return path;
            
            // relative path
            var folder = parsingFilePath.Substring(0, parsingFilePath.LastIndexOf('/'));
            var fullRelativePath = Path.Combine(folder, path);
            if (System.IO.File.Exists(fullRelativePath))
            {
                return fullRelativePath;
            }
            
            return null;
        }
        
        private static string ResolveResourcePath(string resourcePath)
        {
            // Common Unity resource locations to search
            string[] searchPaths = {
                $"Assets/Editor Default Resources/{resourcePath}",
                $"Assets/Resources/{resourcePath}",
                $"Assets/{resourcePath}",
                resourcePath
            };
            
            foreach (string searchPath in searchPaths)
            {
                if (System.IO.File.Exists(searchPath))
                {
                    return AssetDatabase.AssetPathToGUID(searchPath);
                }
            }
            
            // Check in all package locations
            string[] packageGuids = AssetDatabase.FindAssets($"t:DefaultAsset {System.IO.Path.GetFileNameWithoutExtension(resourcePath)}");
            foreach (string guid in packageGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(resourcePath))
                {
                    return guid;
                }
            }
            
            return null;
        }

        private static (string guid, long fileId) ParseLine_Tss(string line)
        {
            string assetPath = Find(line, "@importurl(\"/", "\")");
            return string.IsNullOrEmpty(assetPath)
                ? (null, -1)
                : (AssetDatabase.AssetPathToGUID(assetPath), 0);
        }
    }
}
