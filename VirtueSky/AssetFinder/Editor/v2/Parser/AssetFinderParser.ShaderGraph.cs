using System;
using System.Collections.Generic;

namespace VirtueSky.AssetFinder.Editor
{
    internal static partial class AssetFinderParser // ShaderGraph
    {
        private static readonly Dictionary<string, Func<string, (string, long)>> SHADER_GRAPH_FILES
            = new Dictionary<string, Func<string, (string, long)>>()
            {
                { ".shadergraph", ParseLine_Json },
                { ".shadersubgraph", ParseLine_Json },
            };

        private static void ReadContent_ShaderGraph(string ext, string assetPath, Action<string, long> callback)
        {
            Func<string, (string, long)> lineParser = SHADER_GRAPH_FILES[ext];
            Read(assetPath, lineParser, callback);
        }

        private static (string guid, long fileId) ParseLine_Json(string line)
        {
            string guid = Find(line, "\\\"guid\\\":\\\"", "\\\",");
            if (string.IsNullOrEmpty(guid)) return (null, -1);

            string fileIdStr = Find(line, "\"fileID\\\":", ",");
            if (string.IsNullOrEmpty(fileIdStr)) return (null, -1);

            if (!long.TryParse(fileIdStr, out long fileId)) fileId = -1;
            return (guid, fileId);
        }
    }
}
