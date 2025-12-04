using System;
using System.Text.RegularExpressions;

namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderParser // Shader
    {
        private static void ReadContent_Shader(string filePath, Action<string, long> callback)
        {
            Log($"[FR2] Reading shader file: {filePath}");
            Read(filePath, ParseLine_Shader, callback, false); // Don't use double-check for shader files
        }

        private static (string guid, long fileId) ParseLine_Shader(string line)
        {
#if AssetFinderDEV
            if (line.TrimStart().StartsWith("#include"))
            {
                Log($"[FR2] Processing include line: {line.Trim()}");
            }
#endif
            
            // Pattern 1: #include directives with quotes
            // Example: #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            if (line.TrimStart().StartsWith("#include"))
            {
                string includePath = Find(line, "#include \"", "\"");
                Log($"[FR2] Found quoted include path: '{includePath}'");
                if (!string.IsNullOrEmpty(includePath))
                {
                    string resolvedPath = ResolveShaderIncludePath(includePath);
                    Log($"[FR2] Resolved path: '{resolvedPath}'");
                    if (!string.IsNullOrEmpty(resolvedPath))
                    {
                        string guid = UnityEditor.AssetDatabase.AssetPathToGUID(resolvedPath);
                        Log($"[FR2] Path to GUID: '{resolvedPath}' -> '{guid}'");
                        if (!string.IsNullOrEmpty(guid))
                        {
                            Log($"[FR2] SUCCESS - Shader Include: {includePath} -> {resolvedPath} -> {guid}");
                            return (guid, -1);
                        }
                    }
                }
            }

            // Pattern 2: #include directives with angle brackets  
            // Example: #include <HLSLSupport.cginc>
            if (line.TrimStart().StartsWith("#include"))
            {
                string includePath = Find(line, "#include <", ">");
                if (!string.IsNullOrEmpty(includePath))
                {
                    string resolvedPath = ResolveShaderIncludePath(includePath);
                    if (!string.IsNullOrEmpty(resolvedPath))
                    {
                        string guid = UnityEditor.AssetDatabase.AssetPathToGUID(resolvedPath);
                        if (!string.IsNullOrEmpty(guid))
                        {
                            Log($"[FR2] Shader Include: {includePath} -> {resolvedPath} -> {guid}");
                            return (guid, -1);
                        }
                    }
                }
            }

            return (null, -1);
        }

        private static string ResolveShaderIncludePath(string includePath)
        {
            if (string.IsNullOrEmpty(includePath)) return null;

            Log($"[FR2] Resolving shader include path: '{includePath}'");

            // Pattern 1: Packages/ references
            // Example: "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            if (includePath.StartsWith("Packages/"))
            {
                // Check if the path exists directly
                if (System.IO.File.Exists(includePath))
                {
                    Log($"[FR2] Found direct package path: {includePath}");
                    return includePath;
                }
            }

            // Pattern 2: Relative paths from project
            // Example: "Assets/Shaders/Common.hlsl"
            if (includePath.StartsWith("Assets/"))
            {
                if (System.IO.File.Exists(includePath))
                {
                    return includePath;
                }
            }

            // Pattern 3: Search in common Unity shader include directories
            string[] searchDirectories = {
                "Assets/",
                "Assets/Shaders/",
                "Assets/Scripts/Shaders/",
                "Packages/com.unity.render-pipelines.core/ShaderLibrary/",
                "Packages/com.unity.render-pipelines.universal/ShaderLibrary/",
                "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/",
                "Packages/com.unity.shadergraph/ShaderGraphLibrary/"
            };

            foreach (string searchDir in searchDirectories)
            {
                string fullPath = searchDir + includePath;
                if (System.IO.File.Exists(fullPath))
                {
                    return fullPath;
                }

                // Also try without directory structure (flat search)
                string fileName = System.IO.Path.GetFileName(includePath);
                string flatPath = searchDir + fileName;
                if (System.IO.File.Exists(flatPath))
                {
                    return flatPath;
                }
            }

            // Pattern 4: Search using Unity's AssetDatabase for filename matches
            string targetFileName = System.IO.Path.GetFileNameWithoutExtension(includePath);
            string targetExtension = System.IO.Path.GetExtension(includePath);
            
            if (string.IsNullOrEmpty(targetExtension))
            {
                // Try common shader extensions if no extension provided
                string[] extensions = { ".hlsl", ".cginc", ".shader", ".glsl" };
                foreach (string ext in extensions)
                {
                    string[] matchingGuids = UnityEditor.AssetDatabase.FindAssets($"{targetFileName} t:DefaultAsset");
                    foreach (string guid in matchingGuids)
                    {
                        string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                        if (assetPath.EndsWith(ext) && System.IO.Path.GetFileNameWithoutExtension(assetPath) == targetFileName)
                        {
                            return assetPath;
                        }
                    }
                }
            }
            else
            {
                // Search for exact filename match
                string[] matchingGuids = UnityEditor.AssetDatabase.FindAssets($"{targetFileName} t:DefaultAsset");
                foreach (string guid in matchingGuids)
                {
                    string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                    if (System.IO.Path.GetFileName(assetPath).Equals(System.IO.Path.GetFileName(includePath), StringComparison.OrdinalIgnoreCase))
                    {
                        return assetPath;
                    }
                }
            }

            return null;
        }
    }
} 