using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace VirtueSky.AssetFinder.Editor
{
    public static partial class AssetFinderAddressable
    {

        private static Assembly asm;
        private static Type addressableAssetGroupType;
        private static Type addressableAssetEntryType;

        private static PropertyInfo entriesProperty;
        private static PropertyInfo groupNameProperty;
        private static PropertyInfo addressProperty;
        private static PropertyInfo guidProperty;
        private static PropertyInfo settingsProperty;
        private static PropertyInfo groupsProperty;

        static AssetFinderAddressable()
        {
            Scan();
        }

        public static bool isOk => (asmStatus == ASMStatus.AsmOK) && (projectStatus == ProjectStatus.Ok);

        public static ASMStatus asmStatus { get; private set; }
        public static ProjectStatus projectStatus { get; private set; }

        public static void Scan()
        {
            asm = GetAssembly();
            if (asm == null)
            {
                asmStatus = ASMStatus.AsmNotFound;
                return;
            }

            Type addressableSettingsType = GetAddressableType("UnityEditor.AddressableAssets.Settings.AddressableAssetSettings");
            Type addressableSettingsDefaultObjectType = GetAddressableType("UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject");
            addressableAssetGroupType = GetAddressableType("UnityEditor.AddressableAssets.Settings.AddressableAssetGroup");
            addressableAssetEntryType = GetAddressableType("UnityEditor.AddressableAssets.Settings.AddressableAssetEntry");

            if (addressableSettingsType == null || addressableSettingsDefaultObjectType == null || addressableAssetGroupType == null || addressableAssetEntryType == null)
            {
                asmStatus = ASMStatus.TypeNotFound;
                return;
            }

            entriesProperty = addressableAssetGroupType.GetProperty("entries", BindingFlags.Public | BindingFlags.Instance);
            groupNameProperty = addressableAssetGroupType.GetProperty("Name", BindingFlags.Public | BindingFlags.Instance);
            addressProperty = addressableAssetEntryType.GetProperty("address", BindingFlags.Public | BindingFlags.Instance);
            guidProperty = addressableAssetEntryType.GetProperty("guid", BindingFlags.Public | BindingFlags.Instance);
            settingsProperty = addressableSettingsDefaultObjectType.GetProperty("Settings", BindingFlags.Public | BindingFlags.Static);
            groupsProperty = addressableSettingsType.GetProperty("groups", BindingFlags.Public | BindingFlags.Instance);

            if (entriesProperty == null || groupNameProperty == null || addressProperty == null || guidProperty == null)
            {
                asmStatus = ASMStatus.FieldNotFound;
                return;
            }

            asmStatus = ASMStatus.AsmOK;
            projectStatus = ProjectStatus.None;
        }

        private static Assembly GetAssembly()
        {
            const string DLL = "Unity.Addressables.Editor";
            Assembly[] allAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly item in allAssemblies)
            {
                if (item.GetName().Name != DLL) continue;
                
                return item;
            }
            
            return null;
        }

        private static Type GetAddressableType(string typeName)
        {
            return asm == null ? null : asm.GetType(typeName);
        }

        /// <summary>
        ///     Get a map between address -> AddressInfo (assetGUIDs + childGUIDs)
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, AddressInfo> GetAddresses()
        {
            if (asmStatus != ASMStatus.AsmOK) return null;

            // Get the AddressableAssetSettings instance
            object settings = settingsProperty?.GetValue(null);

            if (settings == null)
            {
                projectStatus = ProjectStatus.NoSettings;
                return null;
            }

            var addresses = new Dictionary<string, AddressInfo>();
            var groups = groupsProperty?.GetValue(settings) as IEnumerable<object>;

            if (groups == null)
            {
                projectStatus = ProjectStatus.NoGroup;
                return null;
            }

            projectStatus = ProjectStatus.Ok;

            // Loop through each group
            foreach (object group in groups)
            {
                if (group == null || addressableAssetGroupType == null || addressableAssetEntryType == null) continue;

                // Get the group's 'entries' property
                var entries = entriesProperty?.GetValue(group) as IEnumerable<object>;

                if (entries == null) continue;

                // Get the group's 'Name' property
                var groupName = groupNameProperty?.GetValue(group)?.ToString();

                // Loop through each entry in the group
                foreach (object entry in entries)
                {
                    if (entry == null) continue;

                    // Get the entry's 'address' and 'guid' properties
                    var address = addressProperty?.GetValue(entry)?.ToString();
                    var guid = guidProperty?.GetValue(entry)?.ToString();

                    if (address == null || guid == null) continue;

                    if (!addresses.TryGetValue(address, out AddressInfo fr2Address))
                    {
                        // New address entry
                        fr2Address = new AddressInfo
                        {
                            address = address,
                            bundleGroup = groupName,
                            assetGUIDs = new HashSet<string>(),
                            childGUIDs = new HashSet<string>()
                        };

                        addresses.Add(address, fr2Address);
                    }

                    if (fr2Address.assetGUIDs.Add(guid)) // folder?
                    {
                        AppendChildGUIDs(fr2Address.childGUIDs, guid);
                    }
                }
            }

            return addresses;
        }

        private static void AppendChildGUIDs(HashSet<string> h, string guid)
        {
            string folderPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!AssetDatabase.IsValidFolder(folderPath)) return;
            string[] allGUIDs = AssetDatabase.FindAssets("*", new[] { folderPath });
            foreach (string child in allGUIDs)
            {
                AssetFinderAsset asset = AssetFinderCache.Api.Get(child, true);
                if (asset == null)
                {
                    AssetFinderLOG.LogWarning($"Why asset is null? {guid}\n{folderPath}");
                    continue;
                }
                
                if (asset.IsExcluded || asset.IsMissing || asset.IsScript || asset.type == AssetFinderAsset.AssetType.UNKNOWN) continue;
                if (asset.inEditor || asset.inResources || asset.inStreamingAsset || asset.inPlugins) continue;
                if (asset.extension == ".asmdef") continue;
                if (asset.extension == ".wlt") continue;
                if (asset.IsFolder) continue;
                h.Add(child);
            }
        }
    }
}
