using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderAddressableDrawer : IRefDraw
    {
        private const string AUTO_DEPEND_TITLE = "(Auto dependency)";

        private readonly Dictionary<AssetFinderAddressable.ASMStatus, string> AsmMessage = new Dictionary<AssetFinderAddressable.ASMStatus, string>
        {
            { AssetFinderAddressable.ASMStatus.None, "-" },
            { AssetFinderAddressable.ASMStatus.AsmNotFound, "Addressable Package not imported!" },
            { AssetFinderAddressable.ASMStatus.TypeNotFound, "Addressable Classes not found (addressable library code changed?)!" },
            { AssetFinderAddressable.ASMStatus.FieldNotFound, "Addressable Fields not found (addressable library code changed?)!" },
            { AssetFinderAddressable.ASMStatus.AsmOK, "-" }
        };

        internal readonly AssetFinderRefDrawer drawer;
        internal readonly Dictionary<string, AssetFinderAddressable.AddressInfo> map = new Dictionary<string, AssetFinderAddressable.AddressInfo>();

        private readonly Dictionary<AssetFinderAddressable.ProjectStatus, string> ProjectStatusMessage = new Dictionary<AssetFinderAddressable.ProjectStatus, string>
        {
            { AssetFinderAddressable.ProjectStatus.None, "-" },
            { AssetFinderAddressable.ProjectStatus.NoSettings, "No Addressables Settings found!\nOpen [Window/Asset Management/Addressables/Groups] to create new Addressables Settings!\n \n" },
            { AssetFinderAddressable.ProjectStatus.NoGroup, "No AssetBundle Group created!" },
            { AssetFinderAddressable.ProjectStatus.Ok, "-" }
        };
        private bool dirty;
        internal List<string> groups;
        internal float maxWidth;
        internal Dictionary<string, AssetFinderRef> refs;

        public AssetFinderAddressableDrawer(IWindow window, Func<AssetFinderRefDrawer.Sort> getSortMode, Func<AssetFinderRefDrawer.Mode> getGroupMode)
        {
            this.window = window;
            drawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = window,
                getSortMode = getSortMode,
                getGroupMode = getGroupMode,
                showFullPath = false,
                showFileSize = true,
                showExtension = true,
                showUsageType = false,
                showAssetBundleName = false,
                showAtlasName = false
            })
            {
                messageNoRefs = "No Addressable Asset",
                messageEmpty = "No Addressable Asset",
                customGetGroup = GetGroup,

                customDrawGroupLabel = DrawGroupLabel,
                beforeItemDraw = BeforeDrawItem,
                afterItemDraw = AfterDrawItem
            };

            dirty = true;
            drawer.SetDirty();
        }

        public IWindow window { get; set; }


        public int ElementCount()
        {
            return refs?.Count ?? 0;
        }

        public bool Draw(Rect rect)
        {
            if (dirty) RefreshView();
            if (refs == null) return false;

            rect.yMax -= 24f;
            bool result = drawer.Draw(rect);

            Rect btnRect = rect;
            btnRect.xMin = btnRect.xMax - 24f;
            btnRect.yMin = btnRect.yMax;
            btnRect.height = 24f;

            if (GUI.Button(btnRect, AssetFinderIcon.Refresh.image))
            {
                AssetFinderAddressable.Scan();
                RefreshView();
            }

            return result;
        }

        public bool DrawLayout()
        {
            if (dirty) RefreshView();
            return drawer.DrawLayout();
        }

        private string GetGroup(AssetFinderRef rf)
        {
            return rf.group;
        }

        private void DrawGroupLabel(Rect r, string label, int childCount)
        {
            Color c = GUI.contentColor;
            if (label == AUTO_DEPEND_TITLE)
            {
                Color c1 = c;
                c1.a = 0.5f;
                GUI.contentColor = c1;
            }

            GUI.Label(r, AssetFinderGUIContent.FromString(label), EditorStyles.boldLabel);
            GUI.contentColor = c;
        }

        private void BeforeDrawItem(Rect r, AssetFinderRef rf)
        {
            string guid = rf.asset.guid;
            if (map.TryGetValue(guid, out AssetFinderAddressable.AddressInfo address)) return;

            Color c = GUI.contentColor;
            c.a = 0.35f;
            GUI.contentColor = c;
        }

        private void AfterDrawItem(Rect r, AssetFinderRef rf)
        {
            string guid = rf.asset.guid;
            if (!map.TryGetValue(guid, out AssetFinderAddressable.AddressInfo address))
            {
                Color c2 = GUI.contentColor;
                c2.a = 1f;
                GUI.contentColor = c2;
                return;
            }

            Color c = GUI.contentColor;
            Color c1 = c;
            c1.a = 0.5f;

            GUI.contentColor = c1;
            {
                r.xMin = r.xMax - maxWidth;
                GUI.Label(r, AssetFinderGUIContent.FromString(address.address), EditorStyles.miniLabel);
            }
            GUI.contentColor = c;

        }

        public void SetDirty()
        {
            dirty = true;
            drawer.SetDirty();
        }



        public void RefreshView()
        {
            if (refs == null) refs = new Dictionary<string, AssetFinderRef>();
            refs.Clear();

            Dictionary<string, AssetFinderAddressable.AddressInfo> addresses = AssetFinderAddressable.GetAddresses();
            if (AssetFinderAddressable.asmStatus != AssetFinderAddressable.ASMStatus.AsmOK)
            {
                drawer.messageNoRefs = AsmMessage[AssetFinderAddressable.asmStatus];
            } else if (AssetFinderAddressable.projectStatus != AssetFinderAddressable.ProjectStatus.Ok)
            {
                drawer.messageNoRefs = ProjectStatusMessage[AssetFinderAddressable.projectStatus];
            }
            drawer.messageEmpty = drawer.messageNoRefs;

            if (addresses == null) addresses = new Dictionary<string, AssetFinderAddressable.AddressInfo>();
            groups = addresses.Keys.ToList();
            map.Clear();

            if (addresses.Count > 0)
            {
                var maxLengthGroup = string.Empty;
                foreach (KeyValuePair<string, AssetFinderAddressable.AddressInfo> kvp in addresses)
                {
                    foreach (string guid in kvp.Value.assetGUIDs)
                    {
                        if (refs.ContainsKey(guid)) continue;

                        AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                        refs.Add(guid, new AssetFinderRef(0, 1, asset, null, null)
                        {
                            isSceneRef = false,
                            group = kvp.Value.bundleGroup
                        });

                        map.Add(guid, kvp.Value);
                        if (maxLengthGroup.Length < kvp.Value.address.Length)
                        {
                            maxLengthGroup = kvp.Value.address;
                        }
                    }

                    foreach (string guid in kvp.Value.childGUIDs)
                    {
                        if (refs.ContainsKey(guid)) continue;

                        AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                        refs.Add(guid, new AssetFinderRef(0, 1, asset, null, null)
                        {
                            isSceneRef = false,
                            group = kvp.Value.bundleGroup
                        });

                        map.Add(guid, kvp.Value);
                        if (maxLengthGroup.Length < kvp.Value.address.Length)
                        {
                            maxLengthGroup = kvp.Value.address;
                        }
                    }
                }

                var miniLabelStyle = EditorStyles.miniLabel ?? new GUIStyle();
                maxWidth = miniLabelStyle.CalcSize(
                    AssetFinderGUIContent.FromString(maxLengthGroup)
                ).x + 16f;


                // Find usage
                Dictionary<string, AssetFinderRef> usages = AssetFinderRef.FindUsage(map.Keys.ToArray());
                foreach (KeyValuePair<string, AssetFinderRef> kvp in usages)
                {
                    if (refs.ContainsKey(kvp.Key)) continue;
                    AssetFinderRef v = kvp.Value;

                    // do not take script
                    if (v.asset.IsScript) continue;
                    if (v.asset.IsExcluded) continue;

                    refs.Add(kvp.Key, kvp.Value);
                    kvp.Value.depth = 1;
                    kvp.Value.group = AUTO_DEPEND_TITLE;
                }
            }

            dirty = false;
            drawer.SetRefs(refs);
        }

        internal void RefreshSort()
        {
            drawer.RefreshSort();
        }
    }
}
