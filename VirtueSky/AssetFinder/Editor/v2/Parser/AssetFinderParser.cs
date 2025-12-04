// #define AssetFinderPARSER_DEBUG

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using AddUsageCB = System.Action<string, long>;

namespace VirtueSky.AssetFinder.Editor
{
    // Public APIs
    internal static partial class AssetFinderParser 
    {
        private static readonly HashSet<string> META_FILES = new HashSet<string>()
        {
            ".texture2darray",".png", ".jpg", ".jpeg", ".tga", ".tif", ".tiff", ".psd", ".bmp", ".exr", ".gif",
            
            // Support static references
            ".shader", ".cs",
            
            // Custom importers
            ".shadergraph"
        };
        
        private static readonly HashSet<string> SHADER_FILES = new HashSet<string>()
        {
            ".shader", ".hlsl", ".cginc", ".glsl"
        };
        
        public static bool IsReadable(string assetPath)
        {
            string ext = Path.GetExtension(assetPath).ToLowerInvariant();
            return IsReadableExtension(ext);
        }
        public static bool IsReadableExtension(string ext)
        {
            return YAML_FILES.Contains(ext) 
                || UI_TOOLKIT_FILES.ContainsKey(ext)
                || SHADER_GRAPH_FILES.ContainsKey(ext)
                || META_FILES.Contains(ext)
                || SHADER_FILES.Contains(ext);
        }

        [Conditional("AssetFinderPARSER_DEBUG")]
        public static void LogWarning(string message, UnityEngine.Object target = null)
            => UnityEngine.Debug.LogWarning(message, target);
        
        [Conditional("AssetFinderPARSER_DEBUG")]
        public static void Log(string message, UnityEngine.Object target = null)
            => UnityEngine.Debug.Log(message, target);
        
        public static void ReadContent(string filePath, AddUsageCB callback)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            bool readMeta = META_FILES.Contains(ext);
            if (readMeta) // Also need to read .meta file
            {
                ReadContent_YAML(filePath + ".meta", callback);
            }
            
            if (YAML_FILES.Contains(ext))
            {
                ReadContent_YAML(filePath, callback); 
                return;
            }
            
            if (SHADER_GRAPH_FILES.ContainsKey(ext))
            {
                ReadContent_ShaderGraph(ext, filePath, callback);
                return;
            }

            if (UI_TOOLKIT_FILES.ContainsKey(ext))
            {
                ReadContent_UIToolkit(ext, filePath, callback);
                return;
            }
            
            if (SHADER_FILES.Contains(ext))
            {
                // TODO: VALIDATE
                // ReadContent_Shader(filePath, callback);
                return;
            }
            
            if (!readMeta) LogWarning("Unknown file type: " + filePath);
        }
    }
    
    
    internal static partial class AssetFinderParser
    {
        private static string parsingFilePath;
        private static void AddObjectUsage(UnityEngine.Object refObj, AddUsageCB callback)
        {
            if (refObj == null) return;
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(refObj, out string refGUID, out long fileId))
            {
                callback(refGUID, fileId);
            }
        }

        private static void Read(string filePath, Func<string, (string, long)> lineHandler, Action<string, long> add, bool doubleCheck = true)
        {
            if (!File.Exists(filePath)) return;

            parsingFilePath = filePath;
            
            // Use a buffer to reduce file I/O overhead
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096))
            using (var sr = new StreamReader(fs, Encoding.UTF8, false, 4096))
            {
                string line;

                while ((line = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line)) continue;

                    var (guid, fileId) = lineHandler(line);
                    if (!string.IsNullOrEmpty(guid))
                    {
                        add(guid, fileId);

                        // Debug.Log($"Found: <{guid}:{fileId}>");
                        continue;
                    }

                    if (!doubleCheck) continue;
                    guid = ExtractGuid(line);
                    if (!string.IsNullOrEmpty(guid))
                    {
                        LogWarning($"Missed GUID <{guid}>?\n{filePath}\n{line}\n");
                        add(guid, 0);
                    }
                }
            }
        }

        private static string ExtractGuid(string line)
        {
            const int GuidLength = 32;
            var validCharCount = 0;

            for (var i = 0; i < line.Length; i++)
            {
                // Check if the character is a valid hex character (0-9, a-f, A-F)
                if (IsHexChar(line[i]))
                {
                    validCharCount++;

                    if (validCharCount == GuidLength)
                    {
                        // Return substring from the start of the valid sequence
                        return line.Substring(i - GuidLength + 1, GuidLength);
                    }
                } else
                {
                    // Reset count if a non-hex character interrupts the sequence
                    validCharCount = 0;
                }
            }

            return null; // No valid GUID found
        }

        // Helper method to check if a character is a hex character
        private static bool IsHexChar(char c)
        {
            return ((c >= '0') && (c <= '9')) || ((c >= 'a') && (c <= 'f')) || ((c >= 'A') && (c <= 'F'));
        }
        
        
        
        private static (string guid, long fileId) FindRef(string source, string guidPattern, string fileIdPattern, string separatorPattern)
        {
            string guid = Find(source, guidPattern, separatorPattern);
            if (string.IsNullOrEmpty(guid)) return (null, -1);

            if (string.IsNullOrEmpty(fileIdPattern)) return (guid, -1);
            string fileIdStr = Find(source, fileIdPattern, separatorPattern);
            if (string.IsNullOrEmpty(fileIdStr)) return (null, -1);

            if (!long.TryParse(fileIdStr, out long fileId)) fileId = -1;

            // Debug.Log($"Found: {guid}/{fileId}\t\t {source}");
            return (guid, fileId);
        }

        private static string Find(string source, string str_begin, string str_end)
        {
            int st = source.IndexOf(str_begin, StringComparison.Ordinal);
            if (st == -1) return null;
            st += str_begin.Length;
            while (st < source.Length && char.IsWhiteSpace(source[st]))
            {
                st++;
            }
            
            if (string.IsNullOrEmpty(str_end)) // no end: determine by length
            {
                // Check if we have enough characters left for a valid GUID (32 characters)
                int remainingLength = source.Length - st;
                if (remainingLength < 32) return null; // Not enough characters for a valid GUID
                
                return source.Substring(st, 32);
            }

            int ed = source.IndexOf(str_end, st, StringComparison.Ordinal);
            if (ed == -1) return null;
            while (ed > st && char.IsWhiteSpace(source[ed - 1]))
            {
                ed--;
            }
            return source.Substring(st, ed - st);
        }
    }
}
