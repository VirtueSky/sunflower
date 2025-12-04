using System;
using System.Collections.Generic;
using System.IO;

namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderParser // Yaml
    {
        private static readonly HashSet<string> YAML_FILES = new HashSet<string>()
        {
            ".anim", ".controller", ".mat", ".unity", ".guiskin", ".prefab", 
            ".overridecontroller", ".mask", ".rendertexture", ".cubemap", ".flare", 
            ".playable", ".physicsmaterial", ".fontsettings", ".asset", ".prefs", 
            ".spriteatlas", ".terrainlayer", ".asmdef", ".preset", ".spriteLib", ".texture2darray"
        };
        
        
        private static void ReadContent_YAML(string filePath, Action<string, long> callback)
        {
            string ext = Path.GetExtension(filePath).ToLowerInvariant();
            bool isBinary = BINARY_ASSET.Contains(ext) && Read_VerifyBinaryAsset(filePath);

            if (isBinary)
            {
                ReadContent_BinaryAsset(filePath, callback);
            } else
            {
                Read(filePath, ParseLine_Yaml, callback);    
            }
        }
        
        private static (string guid, long fileId) ParseLine_Yaml(string line)
        {
            // Check for both 'guid:' and 'm_AssetGUID:' patterns
            (string guid, long fileId) result = FindRef(line, "guid:", "fileID:", ",");
            if (result.guid != null) return result;
            result = FindRef(line, "m_AssetGUID:", null, null);
            return string.IsNullOrEmpty(result.guid) 
                ? FindRef(line, "GUID:", null, null)
                : result;
        }
    }
}
