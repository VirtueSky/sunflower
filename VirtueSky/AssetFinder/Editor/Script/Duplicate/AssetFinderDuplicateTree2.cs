using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderDuplicateTree2 : IRefDraw
    {
        private const float TimeDelayDelete = .5f;

        private static readonly AssetFinderFileCompare fc = new AssetFinderFileCompare();
        private readonly Func<AssetFinderRefDrawer.Mode> getGroupMode;

        private readonly Func<AssetFinderRefDrawer.Sort> getSortMode;
        private readonly AssetFinderTreeUI2.GroupDrawer groupDrawer;
        private readonly string searchTerm = "";
        private List<List<string>> cacheAssetList;
        public bool caseSensitive = false;
        private Dictionary<string, List<AssetFinderRef>> dicIndex; //index, list

        private bool dirty;
        private int excludeCount;
        private string guidPressDelete;

        internal List<AssetFinderRef> list;
        internal Dictionary<string, AssetFinderRef> refs;
        public int scanExcludeByIgnoreCount;
        public int scanExcludeByTypeCount;
        private float TimePressDelete;
        
        // New fields for verification UI
        private Dictionary<string, string> groupVerificationStatus = new Dictionary<string, string>();
        private Dictionary<string, float> groupVerificationProgress = new Dictionary<string, float>();
        private Dictionary<string, int> groupVerificationOrder = new Dictionary<string, int>();
        private bool isSignatureScanComplete = false;

        // Add enum for progress state
        private enum ProgressState
        {
            Idle,
            Scanning,
            Verifying,
            Complete
        }
        private ProgressState progressState = ProgressState.Idle;

        public AssetFinderDuplicateTree2(IWindow window, Func<AssetFinderRefDrawer.Sort> getSortMode, Func<AssetFinderRefDrawer.Mode> getGroupMode)
        {
            this.window = window;
            this.getSortMode = getSortMode;
            this.getGroupMode = getGroupMode;
            groupDrawer = new AssetFinderTreeUI2.GroupDrawer(DrawGroup, DrawAsset);
        }

        public IWindow window { get; set; }

        public bool Draw(Rect rect)
        {
            return false;
        }

        public bool DrawLayout()
        {
            if (dirty) RefreshView(cacheAssetList);

            // Show progress bar on top based on progressState
            if (progressState == ProgressState.Scanning || progressState == ProgressState.Verifying)
            {
                float p = fc.nScaned / (float)Mathf.Max(1, fc.nChunks2);
                string label = progressState == ProgressState.Scanning ? "Scanning" : "Verifying";
                Rect progressRect = GUILayoutUtility.GetRect(1, Screen.width, 18f, 18f);
                EditorGUI.ProgressBar(progressRect, p, string.Format($"{label} {{0}} / {{1}}", fc.nScaned, fc.nChunks2));
                GUILayout.Space(2);
            }

            // Update progress state based on fc
            if (fc.nChunks2 > 0 && fc.nScaned < fc.nChunks2)
            {
                if (progressState != ProgressState.Scanning && progressState != ProgressState.Verifying)
                    progressState = ProgressState.Scanning;
            }
            else if (fc.nChunks2 > 0 && fc.nScaned >= fc.nChunks2)
            {
                if (progressState != ProgressState.Complete)
                    progressState = ProgressState.Complete;
            }
            else
            {
                progressState = ProgressState.Idle;
            }

            if (progressState == ProgressState.Complete || progressState == ProgressState.Idle)
            {
                if (groupDrawer.hasValidTree) groupDrawer.tree.itemPaddingRight = 60f;
                groupDrawer.DrawLayout();
            }

            DrawHeader();
            return false;
        }

        public int ElementCount()
        {
            return list?.Count ?? 0;
        }

        private void DrawAsset(Rect r, string guid)
        {
            if (!refs.TryGetValue(guid, out AssetFinderRef rf)) return;
            var assetRect = r;
            assetRect.width -= 70f;
            rf.asset.Draw(
                assetRect,
                new AssetFinderAsset.AssetFinderAssetDrawConfig(
                    false,
                getGroupMode() != AssetFinderRefDrawer.Mode.Folder,
                AssetFinderSetting.ShowFileSize,
                AssetFinderSetting.s.displayAssetBundleName,
                AssetFinderSetting.s.displayAtlasName,
                AssetFinderSetting.s.showUsedByClassed,
                    window,
                    true
                )
            );

            Texture tex = AssetDatabase.GetCachedIcon(rf.asset.assetPath);
            if (tex == null) return;

            Rect drawR = r;
            drawR.x += drawR.width; // (groupDrawer.TreeNoScroll() ? 60f : 70f) ;
            drawR.width = 40f;
            drawR.y += 1;
            drawR.height -= 2;

            if (GUI.Button(drawR, "Use", EditorStyles.miniButton))
            {
                if (AssetFinderExport.IsMergeProcessing)
                {
                    AssetFinderLOG.LogWarning("Previous merge is processing");
                } 
                else
                {
                    int index = rf.index;
                    Selection.objects = list.Where(x => x.index == index)
                        .Select(x => AssetFinderUnity.LoadAssetAtPath<Object>(x.asset.assetPath)).ToArray();
                    AssetFinderExport.MergeDuplicate(rf.asset.guid);
                }
            }

            if (rf.asset.UsageCount() > 0) return;

            drawR.x -= 25;
            drawR.width = 20;
            if (wasPreDelete(guid))
            {
                Color col = GUI.color;
                GUI.color = Color.red;
                if (GUI.Button(drawR, "X", EditorStyles.miniButton))
                {
                    guidPressDelete = null;
                    AssetDatabase.DeleteAsset(rf.asset.assetPath);
                }

                GUI.color = col;
                window.WillRepaint = true;
            } 
            else
            {
                if (GUI.Button(drawR, "X", EditorStyles.miniButton))
                {
                    guidPressDelete = guid;
                    TimePressDelete = Time.realtimeSinceStartup;
                    window.WillRepaint = true;
                }
            }
        }

        private bool wasPreDelete(string guid)
        {
            if (guidPressDelete == null || guid != guidPressDelete) return false;

            if (Time.realtimeSinceStartup - TimePressDelete < TimeDelayDelete) return true;

            guidPressDelete = null;
            return false;
        }

        private void DrawGroup(Rect r, string label, int childCount)
        {
            AssetFinderAsset asset = dicIndex[label][0].asset;

            Texture tex = AssetDatabase.GetCachedIcon(asset.assetPath);
            Rect rect = r;

            if (tex != null)
            {
                rect.width = 16f;
                GUI.DrawTexture(rect, tex);
            }

            rect = r;
            rect.xMin += 16f;
            GUI.Label(rect, asset.assetName, EditorStyles.boldLabel);

            rect = r;
            rect.xMin += rect.width - 50f;
            GUI.Label(rect, AssetFinderHelper.GetfileSizeString(asset.fileSize), EditorStyles.miniLabel);

            rect = r;
            rect.xMin += rect.width - 100f;
            GUI.Label(rect, childCount.ToString(), EditorStyles.miniLabel);
            // Removed: status/progress/queue UI
        }

        public void Reset(List<List<string>> assetList)
        {
            progressState = ProgressState.Scanning;
            groupVerificationStatus.Clear();
            groupVerificationProgress.Clear();
            groupVerificationOrder.Clear();
            
            fc.Reset(assetList, OnUpdateView, RefreshView);
        }

        private void OnUpdateView(List<List<string>> assetList)
        {
            // This is called during verification to update the view with current results
            if (assetList != null)
            {
                cacheAssetList = assetList;
                dirty = true;
                window.WillRepaint = true;
            }
        }

        public bool isExclueAnyItem()
        {
            return excludeCount > 0 || scanExcludeByTypeCount > 0;
        }

        public bool isExclueAnyItemByIgnoreFolder()
        {
            return scanExcludeByIgnoreCount > 0;
        }

        private void RefreshView(List<List<string>> assetList)
        {
            cacheAssetList = assetList;
            dirty = false;
            list = new List<AssetFinderRef>();
            refs = new Dictionary<string, AssetFinderRef>();
            dicIndex = new Dictionary<string, List<AssetFinderRef>>();
            if (assetList == null) return;

            int minScore = searchTerm.Length;
            string term1 = searchTerm;
            if (!caseSensitive) term1 = term1.ToLower();

            string term2 = term1.Replace(" ", string.Empty);
            excludeCount = 0;

            for (var i = 0; i < assetList.Count; i++)
            {
                var lst = new List<AssetFinderRef>();
                for (var j = 0; j < assetList[i].Count; j++)
                {
                    string path = assetList[i][j];
                    if (!path.StartsWith("Assets/"))
                    {
                        AssetFinderLOG.LogWarning("Ignore asset: " + path);
                        continue;
                    }

                    string guid = AssetDatabase.AssetPathToGUID(path);
                    if (string.IsNullOrEmpty(guid)) continue;

                    if (refs.ContainsKey(guid)) continue;

                    AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                    if (asset == null) continue;
                    if (!asset.assetPath.StartsWith("Assets/")) continue; // ignore builtin, packages, ...

                    var fr2 = new AssetFinderRef(i, 0, asset, null);

                    if (AssetFinderSetting.IsTypeExcluded(fr2.type))
                    {
                        excludeCount++;
                        continue; //skip this one
                    }

                    if (string.IsNullOrEmpty(searchTerm))
                    {
                        fr2.matchingScore = 0;
                        list.Add(fr2);
                        lst.Add(fr2);
                        refs.Add(guid, fr2);
                        continue;
                    }

                    //calculate matching score
                    string name1 = fr2.asset.assetName;
                    if (!caseSensitive) name1 = name1.ToLower();

                    string name2 = name1.Replace(" ", string.Empty);

                    int score1 = AssetFinderUnity.StringMatch(term1, name1);
                    int score2 = AssetFinderUnity.StringMatch(term2, name2);

                    fr2.matchingScore = Mathf.Max(score1, score2);
                    if (fr2.matchingScore > minScore)
                    {
                        list.Add(fr2);
                        lst.Add(fr2);
                        refs.Add(guid, fr2);
                    }
                }

                dicIndex.Add(i.ToString(), lst);
                
                // Initialize verification status for the group
                if (isSignatureScanComplete)
                {
                    groupVerificationStatus[i.ToString()] = "Pending";
                }
            }

            ResetGroup();
        }

        private void ResetGroup()
        {
            groupDrawer.Reset(list,
                rf => rf.asset.guid
                , GetGroup, SortGroup);
            if (window != null) window.Repaint();
        }

        private string GetGroup(AssetFinderRef rf)
        {
            return rf.index.ToString();
        }

        private void SortGroup(List<string> groups)
        {
            // Sort by verification status, then by size
            if (isSignatureScanComplete)
            {
                groups.Sort((a, b) => {
                    // First check if either is currently verifying
                    if (groupVerificationStatus.ContainsKey(a) && groupVerificationStatus[a] == "Verifying")
                        return -1;
                    if (groupVerificationStatus.ContainsKey(b) && groupVerificationStatus[b] == "Verifying")
                        return 1;
                    
                    // Then check if verified
                    bool aVerified = groupVerificationStatus.ContainsKey(a) && groupVerificationStatus[a] == "Verified";
                    bool bVerified = groupVerificationStatus.ContainsKey(b) && groupVerificationStatus[b] == "Verified";
                    if (aVerified && !bVerified) return -1;
                    if (!aVerified && bVerified) return 1;
                    
                    // Then check queue position
                    if (groupVerificationOrder.ContainsKey(a) && groupVerificationOrder.ContainsKey(b))
                        return groupVerificationOrder[a].CompareTo(groupVerificationOrder[b]);
                    
                    // Default to standard order
                    return a.CompareTo(b);
                });
            }
        }

        public void SetDirty()
        {
            dirty = true;
        }

        public void RefreshSort()
        {
            if (groupDrawer.hasValidTree)
            {
                SortGroup(groupDrawer.tree.rootItem.children.Select(item => item.id).ToList());
                groupDrawer.Reset(list,
                    rf => rf.asset.guid,
                    GetGroup, SortGroup);
                if (window != null) window.Repaint();
            }
        }

        private void DrawHeader()
        {
            string text = groupDrawer.hasValidTree ? "Rescan" : "Scan";

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(text))
            {
                OnCacheReady();
            }
            // Removed: Refresh Sort button
            EditorGUILayout.EndHorizontal();
            // Removed: bottom status text
        }

        private void OnCacheReady()
        {
            scanExcludeByTypeCount = 0;
            Reset(AssetFinderCache.Api.ScanSimilar(IgnoreTypeWhenScan, IgnoreFolderWhenScan));
            AssetFinderCache.onReady -= OnCacheReady;
        }

        private void IgnoreTypeWhenScan()
        {
            scanExcludeByTypeCount++;
        }

        private void IgnoreFolderWhenScan()
        {
            scanExcludeByIgnoreCount++;
        }
    }
}
