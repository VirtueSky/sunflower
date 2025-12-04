using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderAsset
    {
        // ------------------------------ CONSTANTS ---------------------------

        public const int MIN_FILE_SIZE_2LOG = 1024 * 1024; // 1 MB
        internal static bool shouldWriteImportLog;

        private static readonly HashSet<string> SCRIPT_EXTENSIONS = new HashSet<string>
        {
            ".cs", ".js", ".boo", ".h", ".java", ".cpp", ".m", ".mm", ".cginclude", ".shadersubgraph"
        };

        private static readonly HashSet<string> REFERENCABLE_EXTENSIONS = new HashSet<string>
        {
            ".anim", ".controller", ".mat", ".unity", ".guiskin", ".prefab",
            ".overridecontroller", ".mask", ".rendertexture", ".cubemap", ".flare", ".playable",
            ".mat", ".physicsmaterial", ".fontsettings", ".asset", ".prefs", ".spriteatlas",
            ".terrainlayer", ".asmdef", ".preset", ".spriteLib", ".shader", ".hlsl", ".cginc", ".glsl"
        };

        private static readonly HashSet<string> REFERENCABLE_JSON = new HashSet<string>
        {
            ".shadergraph", ".shadersubgraph"
        };

        private static readonly HashSet<string> UI_TOOLKIT = new HashSet<string>
        {
            ".uss", ".uxml", ".tss"
        };

        private static readonly HashSet<string> REFERENCABLE_META = new HashSet<string>
        {
            ".texture2darray",
            ".png", ".jpg", ".jpeg", ".tga", ".tif", ".tiff", ".psd", ".bmp", ".exr", ".gif",
        };

        /// <summary>
        ///     Extensions that rarely contain references to other assets and can be
        ///     ignored when deciding if an asset is "critical".
        /// </summary>
        private static readonly HashSet<string> NON_REFERENCE_EXTENSIONS =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".wav", ".mp3", ".ogg", ".aif", ".aiff",
                ".txt", ".json", ".xml", ".csv",
                ".html", ".htm", ".yaml", ".md"
            };

        internal static readonly HashSet<string> BUILT_IN_ASSETS = new HashSet<string>
        {
            "0000000000000000f000000000000000",
            "0000000000000000e000000000000000",
            "0000000000000000d000000000000000"
        };

        private static readonly Dictionary<long, Type> HashClasses = new Dictionary<long, Type>();
        internal static Dictionary<string, GUIContent> cacheImage = new Dictionary<string, GUIContent>();

        public static float ignoreTS;
        private static string _logPath;
        private static int binaryLoaded;
        private static DateTime scanStartTime;

        private static string logPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_logPath)) return _logPath;
                _logPath = System.IO.Path.Combine(Application.dataPath, "../fr2-import.log");
                return _logPath;
            }
        }
    }
} 