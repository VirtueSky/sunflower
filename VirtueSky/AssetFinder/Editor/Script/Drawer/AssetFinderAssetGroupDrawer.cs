using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{


    internal static class AssetFinderAssetGroupDrawer
    {
        // ------------------------------- STATIC -----------------------------

        internal static readonly AssetFinderAssetGroup[] FILTERS =
        {
            new AssetFinderAssetGroup("Scene", ".unity"),
            new AssetFinderAssetGroup("Prefab", ".prefab"),
            new AssetFinderAssetGroup("Model", ".3df", ".3dm", ".3dmf", ".3dv", ".3dx", ".c5d", ".lwo", ".lws", ".ma", ".mb",
                ".mesh", ".vrl", ".wrl", ".wrz", ".fbx", ".dae", ".3ds", ".dxf", ".obj", ".skp", ".max", ".blend"),
            new AssetFinderAssetGroup("Material", ".mat", ".cubemap", ".physicsmaterial"),
            new AssetFinderAssetGroup("Texture", ".ai", ".apng", ".png", ".bmp", ".cdr", ".dib", ".eps", ".exif", ".ico", ".icon",
                ".j", ".j2c", ".j2k", ".jas", ".jiff", ".jng", ".jp2", ".jpc", ".jpe", ".jpeg", ".jpf", ".jpg", "jpw",
                "jpx", "jtf", ".mac", ".omf", ".qif", ".qti", "qtif", ".tex", ".tfw", ".tga", ".tif", ".tiff", ".wmf",
                ".psd", ".exr", ".rendertexture"),
            new AssetFinderAssetGroup("Video", ".asf", ".asx", ".avi", ".dat", ".divx", ".dvx", ".mlv", ".m2l", ".m2t", ".m2ts",
                ".m2v", ".m4e", ".m4v", "mjp", ".mov", ".movie", ".mp21", ".mp4", ".mpe", ".mpeg", ".mpg", ".mpv2",
                ".ogm", ".qt", ".rm", ".rmvb", ".wmv", ".xvid", ".flv"),
            new AssetFinderAssetGroup("Audio", ".mp3", ".wav", ".ogg", ".aif", ".aiff", ".mod", ".it", ".s3m", ".xm"),
            new AssetFinderAssetGroup("Script", ".cs", ".js", ".boo", ".h"),
            new AssetFinderAssetGroup("Text", ".txt", ".json", ".xml", ".bytes", ".sql"),
            new AssetFinderAssetGroup("Shader", ".shader", ".cginc", ".shadervariants"),
            new AssetFinderAssetGroup("Animation", ".anim", ".controller", ".overridecontroller", ".mask"),
            new AssetFinderAssetGroup("Font", ".ttf", ".otf", ".dfont", ".ttc"),
            new AssetFinderAssetGroup("Unity Asset", ".asset", ".guiskin", ".flare", ".fontsettings", ".prefs", ".playable", ".signal"),
            new AssetFinderAssetGroup("Others") //
        };

        private static AssetFinderIgnoreDrawer _ignore;
        private static AssetFinderIgnoreDrawer ignore
        {
            get
            {
                if (_ignore == null) _ignore = new AssetFinderIgnoreDrawer();

                return _ignore;
            }
        }

        public static int GetIndex(string ext)
        {
            // Normalize extension to lowercase for case-insensitive comparison
            string normalizedExt = ext?.ToLowerInvariant() ?? "";
            
            for (var i = 0; i < FILTERS.Length - 1; i++)
            {
                if (FILTERS[i].extension.Contains(normalizedExt)) return i;
            }

            return FILTERS.Length - 1; //Others
        }

        public static bool DrawSearchFilter()
        {
            int n = FILTERS.Length;
            var nCols = 4;
            int nRows = Mathf.CeilToInt(n / (float)nCols);
            var result = false;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button("All", EditorStyles.toolbarButton) && !AssetFinderSetting.IsIncludeAllType())
                {
                    AssetFinderSetting.IncludeAllType();
                    result = true;
                }

                if (GUILayout.Button("None", EditorStyles.toolbarButton) && (AssetFinderSetting.GetExcludeType() != -1))
                {
                    AssetFinderSetting.ExcludeAllType();
                    result = true;
                }
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            for (var i = 0; i < nCols; i++)
            {
                GUILayout.BeginVertical();
                for (var j = 0; j < nRows; j++)
                {
                    int idx = i * nCols + j;
                    if (idx >= n) break;

                    bool s = !AssetFinderSetting.IsTypeExcluded(idx);
                    bool s1 = GUILayout.Toggle(s, FILTERS[idx].name);
                    if (s1 != s)
                    {
                        result = true;
                        AssetFinderSetting.ToggleTypeExclude(idx);
                    }
                }

                GUILayout.EndVertical();
                if ((i + 1) * nCols >= n) break;
            }

            GUILayout.EndHorizontal();

            return result;
        }

        public static void SetDirtyIgnore()
        {
            ignore.SetDirty();
        }

        public static bool DrawIgnoreFolder()
        {
            var change = false;
            ignore.Draw();
            return change;
        }
    }
}
