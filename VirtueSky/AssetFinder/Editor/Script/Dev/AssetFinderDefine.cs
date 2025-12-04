namespace VirtueSky.AssetFinder.Editor
{
    internal static class AssetFinderDefine
    {
        internal static bool IsDebugModeEnabled()
        {
            string cscPath = GetCscFilePath();
            return System.IO.File.Exists(cscPath) && HasDefine(System.IO.File.ReadAllText(cscPath), "AssetFinderDEBUG");
        }
        
        internal static void ToggleDebugMode(bool enable)
        {
            string cscPath = GetCscFilePath();
            string content = System.IO.File.Exists(cscPath) ? System.IO.File.ReadAllText(cscPath) : "";
            
            content = enable ? AddDefine(content, "AssetFinderDEBUG") : RemoveDefine(content, "AssetFinderDEBUG");
            
            if (!string.IsNullOrWhiteSpace(content))
            {
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(cscPath));
                System.IO.File.WriteAllText(cscPath, content);
            }
            else if (System.IO.File.Exists(cscPath))
            {
                System.IO.File.Delete(cscPath);
            }
            
            UnityEditor.AssetDatabase.Refresh();
            UnityEngine.Debug.Log($"FR2 Developer Mode {(enable ? "enabled" : "disabled")}. Unity will recompile.");
        }
        
        private static string GetCscFilePath()
        {
            return System.IO.Path.Combine("Assets", "csc.rsp");
        }
        
        internal static bool HasDefine(string content, string define)
        {
            if (string.IsNullOrWhiteSpace(content)) return false;
            var defines = ReadDefines(content);
            return defines.Contains(define);
        }
        
        internal static string AddDefine(string content, string define)
        {
            var defines = ReadDefines(content);
            if (!defines.Contains(define))
            {
                defines.Add(define);
            }
            return WriteDefines(defines);
        }
        
        internal static string RemoveDefine(string content, string define)
        {
            var defines = ReadDefines(content);
            defines.Remove(define);
            return WriteDefines(defines);
        }
        
        private static System.Collections.Generic.List<string> ReadDefines(string content)
        {
            var result = new System.Collections.Generic.List<string>();
            if (string.IsNullOrWhiteSpace(content)) return result;
            
            string[] lines = content.Split('\n');
            foreach (string line in lines)
            {
                string trimmedLine = line.Trim();
                if (trimmedLine.StartsWith("-define:"))
                {
                    // Extract existing symbols from -define: (same logic as GDK)
                    string definesString = trimmedLine.Substring(8); // Skip "-define:"
                    if (!string.IsNullOrEmpty(definesString))
                    {
                        result.AddRange(definesString.Split(';'));
                    }
                }
            }
            return result;
        }
        
        private static string WriteDefines(System.Collections.Generic.List<string> defines)
        {
            // Clean up empty/whitespace defines
            var cleanDefines = new System.Collections.Generic.List<string>();
            foreach (string define in defines)
            {
                string trimmed = define?.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                {
                    cleanDefines.Add(trimmed);
                }
            }
            
            // Write exactly like GDK does
            return cleanDefines.Count > 0 ? $"-define:{string.Join(";", cleanDefines)}" : string.Empty;
        }
        
    }
}