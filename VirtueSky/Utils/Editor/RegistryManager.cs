using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEditor.PackageManager;
using UnityEngine;
using VirtueSky.SimpleJSON;


namespace VirtueSky.UtilsEditor
{
    public static class RegistryManager
    {
        private static readonly string ManifestPath =
            Path.Combine(Application.dataPath, "..", "Packages", "manifest.json");

        public static void Add(string name, string version)
        {
            var json = JObject.Parse(File.ReadAllText(ManifestPath));
            var dependencies = (JObject)json["dependencies"];

            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    if (dependency.Key.Equals(name)) return;
                }

                dependencies.Add(name, version);
            }

            Write(json);
        }

        public static void AddOverrideVersion(string name, string version)
        {
            (bool isInstall, string currentVersion) = IsInstalled(name);
            if (currentVersion == "")
            {
                Add(name, version);
                Resolve();
            }
            else
            {
                if (version.Equals(currentVersion))
                {
                    Debug.Log($"This version of <color=Green>{name}</color> is installed");
                }
                else
                {
                    Remove(name);
                    Add(name, version);
                    Resolve();
                }
            }
        }

        public static void Remove(string name)
        {
            var json = JObject.Parse(File.ReadAllText(ManifestPath));
            var dependencies = (JObject)json["dependencies"];
            dependencies?.Remove(name);
            Write(json);
        }

        public static (bool, string) IsInstalled(string name)
        {
            var json = JObject.Parse(File.ReadAllText(ManifestPath));
            var dependencies = (JObject)json["dependencies"];
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    if (dependency.Key.Equals(name)) return (true, dependency.Value.ToString());
                }
            }

            return (false, "");
        }

        public static bool IsInstalledPackage(string name)
        {
            var json = JObject.Parse(File.ReadAllText(ManifestPath));
            var dependencies = (JObject)json["dependencies"];
            if (dependencies != null)
            {
                foreach (var dependency in dependencies)
                {
                    if (dependency.Key.Equals(name)) return true;
                }
            }

            return false;
        }

        public static void Resolve()
        {
            Client.Resolve();
        }

        private static void Write(JObject json)
        {
            File.WriteAllText(ManifestPath, json.ToString());
        }

        public static (string, string) GetPackageInManifestByPackageName(string packageName)
        {
            string manifestContent = GetManifestContent();
            if (manifestContent == null)
            {
                Debug.LogError("Could not find fileManifest.json");
                return (null, null);
            }

            JSONNode manifestJson = JSON.Parse(manifestContent);
            JSONNode dependencies = manifestJson["dependencies"];
            if (dependencies != null && dependencies.Count > 0)
            {
                //  List<string> libraries = new List<string>();
                foreach (KeyValuePair<string, JSONNode> dep in dependencies.AsObject)
                {
                    if (packageName == $"\"{dep.Key}\"")
                    {
                        // packageName and packageVersion
                        return ($"\"{dep.Key}\"", $": {dep.Value}");
                    }
                    // libraries.Add($"\"{dep.Key}\": {dep.Value}");
                }
            }
            else
            {
                Debug.LogError("Could not find dependencies or dependencies null.");
                return (null, null);
            }

            return (null, null);
        }

        public static void AddPackageInManifest(string packageFullName)
        {
            string manifestContent = GetManifestContent();
            if (manifestContent != null)
            {
                int dependenciesIndex = manifestContent.IndexOf("\"dependencies\": {") +
                                        "\"dependencies\": {".Length;

                manifestContent = manifestContent.Insert(dependenciesIndex,
                    packageFullName);
                WriteAllManifestContent(manifestContent);
                Debug.Log($"<color=Green>Add {packageFullName} to manifest</color>");
            }
        }

        public static void RemovePackageInManifest(string packageFullName)
        {
            string manifestContent = GetManifestContent();
            if (manifestContent != null)
            {
                // int dependenciesIndex = manifestContent.IndexOf("\"dependencies\": {") +
                //                         "\"dependencies\": {".Length;

                manifestContent = manifestContent.Replace(packageFullName,
                    "");
                WriteAllManifestContent(manifestContent);
                Debug.Log($"<color=Green>Remove {packageFullName} to manifest</color>");
            }
        }

        public static string GetManifestContent()
        {
            if (File.Exists(ManifestPath))
            {
                return File.ReadAllText(ManifestPath);
            }

            return null;
        }

        public static void WriteAllManifestContent(string manifestContent)
        {
            File.WriteAllText(ManifestPath, FileExtension.FormatJson(manifestContent));
        }
    }
}