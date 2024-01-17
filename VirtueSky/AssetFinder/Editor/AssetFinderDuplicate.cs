//#define AssetFinderDEBUG

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using CBParams = System.Collections.Generic.List<System.Collections.Generic.List<string>>;
using Object = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderDuplicateTree2 : IRefDraw
    {
        private const float TimeDelayDelete = .5f;

        private static readonly AssetFinderFileCompare fc = new AssetFinderFileCompare();
        private readonly AssetFinderTreeUI2.GroupDrawer groupDrawer;
        private CBParams cacheAssetList;
        public bool caseSensitive = false;
        private Dictionary<string, List<AssetFinderRef>> dicIndex; //index, list

        private bool dirty;
        private int excludeCount;
        private string guidPressDelete;
        internal List<AssetFinderRef> list;
        internal Dictionary<string, AssetFinderRef> refs;
        public int scanExcludeByIgnoreCount;
        public int scanExcludeByTypeCount;
        private string searchTerm = "";
        private float TimePressDelete;

        public AssetFinderDuplicateTree2(IWindow window)
        {
            this.window = window;
            groupDrawer = new AssetFinderTreeUI2.GroupDrawer(DrawGroup, DrawAsset);
        }

        public IWindow window { get; set; }

        public bool Draw(Rect rect)
        {
            return false;
        }

        public bool DrawLayout()
        {
            if (dirty)
            {
                RefreshView(cacheAssetList);
            }

            if (fc.nChunks2 > 0 && fc.nScaned < fc.nChunks2)
            {
                Rect rect = GUILayoutUtility.GetRect(1, Screen.width, 18f, 18f);
                float p = fc.nScaned / (float)fc.nChunks2;

                EditorGUI.ProgressBar(rect, p,
                    string.Format("Scanning {0} / {1}", fc.nScaned, fc.nChunks2));
                GUILayout.FlexibleSpace();
                return true;
            }

            if (groupDrawer.hasValidTree)
            {
                groupDrawer.tree.itemPaddingRight = 60f;
            }

            groupDrawer.DrawLayout();
            DrawHeader();
            return false;
        }

        public int ElementCount()
        {
            return list == null ? 0 : list.Count;
        }

        private void DrawAsset(Rect r, string guid)
        {
            AssetFinderRef rf;
            if (!refs.TryGetValue(guid, out rf))
            {
                return;
            }

            rf.asset.Draw(r, false,
                AssetFinderSetting.GroupMode != AssetFinderRefDrawer.Mode.Folder,
                AssetFinderSetting.ShowFileSize,
                AssetFinderSetting.s.displayAssetBundleName,
                AssetFinderSetting.s.displayAtlasName,
                AssetFinderSetting.s.showUsedByClassed,
                window);

            Texture tex = AssetDatabase.GetCachedIcon(rf.asset.assetPath);
            if (tex == null)
            {
                return;
            }

            Rect drawR = r;
            drawR.x = drawR.x + drawR.width; // (groupDrawer.TreeNoScroll() ? 60f : 70f) ;
            drawR.width = 40f;
            drawR.y += 1;
            drawR.height -= 2;

            if (GUI.Button(drawR, "Use", EditorStyles.miniButton))
            {
                if (AssetFinderExport.IsMergeProcessing)
                {
                    Debug.LogWarning("Previous merge is processing");
                }
                else
                {
                    //AssetDatabase.SaveAssets();
                    //EditorGUIUtility.systemCopyBuffer = rf.asset.guid;
                    //EditorGUIUtility.systemCopyBuffer = rf.asset.guid;
                    // Debug.Log("guid: " + rf.asset.guid + "  systemCopyBuffer " + EditorGUIUtility.systemCopyBuffer);
                    int index = rf.index;
                    Selection.objects = list.Where(x => x.index == index)
                        .Select(x => AssetFinderUnity.LoadAssetAtPath<Object>(x.asset.assetPath))
                        .ToArray();
                    AssetFinderExport.MergeDuplicate(rf.asset.guid);
                }
            }

            if (rf.asset.UsageCount() > 0)
            {
                return;
            }

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
            if (guidPressDelete == null || guid != guidPressDelete)
            {
                return false;
            }

            if (Time.realtimeSinceStartup - TimePressDelete < TimeDelayDelete)
            {
                return true;
            }

            guidPressDelete = null;
            return false;
        }

        private void DrawGroup(Rect r, string label, int childCount)
        {
            // GUI.Label(r, label + " (" + childCount + ")", EditorStyles.boldLabel);
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
            GUI.Label(rect, AssetFinderHelper.GetfileSizeString(asset.fileSize),
                EditorStyles.miniLabel);

            rect = r;
            rect.xMin += rect.width - 70f;
            GUI.Label(rect, childCount.ToString(), EditorStyles.miniLabel);

            rect = r;
            rect.xMin += rect.width - 70f;
        }


        // private List<AssetFinderDuplicateFolder> duplicated;

        public void Reset(CBParams assetList)
        {
            fc.Reset(assetList, OnUpdateView, RefreshView);
        }

        private void OnUpdateView(CBParams assetList)
        {
        }

        public bool isExclueAnyItem()
        {
            return excludeCount > 0 || scanExcludeByTypeCount > 0;
        }

        public bool isExclueAnyItemByIgnoreFolder()
        {
            return scanExcludeByIgnoreCount > 0;
        }

        // void OnActive
        private void RefreshView(CBParams assetList)
        {
            cacheAssetList = assetList;
            dirty = false;
            list = new List<AssetFinderRef>();
            refs = new Dictionary<string, AssetFinderRef>();
            dicIndex = new Dictionary<string, List<AssetFinderRef>>();
            if (assetList == null)
            {
                return;
            }

            int minScore = searchTerm.Length;
            string term1 = searchTerm;
            if (!caseSensitive)
            {
                term1 = term1.ToLower();
            }

            string term2 = term1.Replace(" ", string.Empty);
            excludeCount = 0;

            for (var i = 0; i < assetList.Count; i++)
            {
                var lst = new List<AssetFinderRef>();
                for (var j = 0; j < assetList[i].Count; j++)
                {
                    string guid = AssetDatabase.AssetPathToGUID(assetList[i][j]);
                    if (string.IsNullOrEmpty(guid))
                    {
                        continue;
                    }

                    if (refs.ContainsKey(guid))
                    {
                        continue;
                    }

                    AssetFinderAsset asset = AssetFinderCache.Api.Get(guid);
                    if (asset == null) continue;
                    if (!asset.assetPath.StartsWith("Assets/"))
                        continue; // ignore builtin, packages, ...

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
                    if (!caseSensitive)
                    {
                        name1 = name1.ToLower();
                    }

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
            }

            ResetGroup();
        }

        private void ResetGroup()
        {
            groupDrawer.Reset(list,
                rf => rf.asset.guid
                , GetGroup, SortGroup);
            if (window != null)
            {
                window.Repaint();
            }
        }

        private string GetGroup(AssetFinderRef rf)
        {
            return rf.index.ToString();
        }

        private void SortGroup(List<string> groups)
        {
            // groups.Sort( (item1, item2) =>
            // {
            // 	if (item1 == "Others" || item2 == "Selection") return 1;
            // 	if (item2 == "Others" || item1 == "Selection") return -1;
            // 	return item1.CompareTo(item2);
            // });
        }

        public void SetDirty()
        {
            dirty = true;
        }

        public void RefreshSort()
        {
        }

        private void DrawHeader()
        {
            var text = groupDrawer.hasValidTree ? "Rescan" : "Scan";

            if (GUILayout.Button(text))
            {
                AssetFinderCache.onReady -= OnCacheReady;
                AssetFinderCache.onReady += OnCacheReady;
                AssetFinderCache.Api.Check4Changes(false);
            }
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

    internal class AssetFinderFileCompare
    {
        public static HashSet<AssetFinderChunk> HashChunksNotComplete;
        internal static int streamClosedCount;
        private CBParams cacheList;
        public List<AssetFinderHead> deads = new List<AssetFinderHead>();
        public List<AssetFinderHead> heads = new List<AssetFinderHead>();

        public int nChunks;
        public int nChunks2;
        public int nScaned;
        public Action<CBParams> OnCompareComplete;

        public Action<CBParams> OnCompareUpdate;
        private int streamCount;

        public void Reset(CBParams list, Action<CBParams> onUpdate, Action<CBParams> onComplete)
        {
            nChunks = 0;
            nScaned = 0;
            nChunks2 = 0;
            streamCount = streamClosedCount = 0;
            HashChunksNotComplete = new HashSet<AssetFinderChunk>();

            if (heads.Count > 0)
            {
                for (var i = 0; i < heads.Count; i++)
                {
                    heads[i].CloseChunk();
                }
            }

            deads.Clear();
            heads.Clear();

            OnCompareUpdate = onUpdate;
            OnCompareComplete = onComplete;
            if (list.Count <= 0)
            {
                OnCompareComplete(new CBParams());
                return;
            }

            cacheList = list;
            for (var i = 0; i < list.Count; i++)
            {
                var file = new FileInfo(list[i][0]);
                int nChunk = Mathf.CeilToInt(file.Length / (float)AssetFinderHead.chunkSize);
                nChunks2 += nChunk;
            }

            // for(int i =0;i< list.Count;i++)
            // {
            //     AddHead(list[i]);
            // }
            AddHead(cacheList[cacheList.Count - 1]);
            cacheList.RemoveAt(cacheList.Count - 1);

            EditorApplication.update -= ReadChunkAsync;
            EditorApplication.update += ReadChunkAsync;
        }

        public AssetFinderFileCompare AddHead(List<string> files)
        {
            if (files.Count < 2)
            {
                Debug.LogWarning("Something wrong ! head should not contains < 2 elements");
            }

            var chunkList = new List<AssetFinderChunk>();
            for (var i = 0; i < files.Count; i++)
            {
                streamCount++;
                //  Debug.Log("new stream " + files[i]);
                chunkList.Add(new AssetFinderChunk
                {
                    file = files[i],
                    stream = new FileStream(files[i], FileMode.Open, FileAccess.Read),
                    buffer = new byte[AssetFinderHead.chunkSize]
                });
            }

            var file = new FileInfo(files[0]);
            int nChunk = Mathf.CeilToInt(file.Length / (float)AssetFinderHead.chunkSize);

            heads.Add(new AssetFinderHead
            {
                fileSize = file.Length,
                currentChunk = 0,
                nChunk = nChunk,
                chunkList = chunkList
            });

            nChunks += nChunk;

            return this;
        }

        private bool checkCompleteAllCurFile()
        {
            return streamClosedCount + HashChunksNotComplete.Count >= streamCount; //-1 for safe
        }

        private void ReadChunkAsync()
        {
            bool alive = ReadChunk();
            HashChunksNotComplete.RemoveWhere(x => x.stream == null || !x.stream.CanRead);
            if (cacheList.Count > 0 && checkCompleteAllCurFile()) //complete all chunk
            {
                int numCall = AssetFinderCache.priority; // - 2;
                if (numCall <= 0)
                {
                    numCall = 1;
                }

                for (var i = 0; i < numCall; i++)
                {
                    if (cacheList.Count <= 0)
                    {
                        break;
                    }

                    AddHead(cacheList[cacheList.Count - 1]);
                    cacheList.RemoveAt(cacheList.Count - 1);
                }
            }

            var update = false;

            for (int i = heads.Count - 1; i >= 0; i--)
            {
                AssetFinderHead h = heads[i];
                if (h.isDead)
                {
                    h.CloseChunk();
                    heads.RemoveAt(i);
                    if (h.chunkList.Count > 1)
                    {
                        update = true;
                        deads.Add(h);
                    }
                }
            }

            if (update)
            {
                Trigger(OnCompareUpdate);
            }

            if (!alive && cacheList.Count <= 0 && checkCompleteAllCurFile()
               ) //&& cacheList.Count <= 0 complete all chunk and cache list empty
            {
                foreach (AssetFinderChunk item in HashChunksNotComplete)
                {
                    if (item.stream != null && item.stream.CanRead)
                    {
                        item.stream.Close();
                        item.stream = null;
                    }
                }

                HashChunksNotComplete.Clear();
                // Debug.Log("complete ");
                nScaned = nChunks;
                EditorApplication.update -= ReadChunkAsync;
                Trigger(OnCompareComplete);
            }
        }

        private void Trigger(Action<CBParams> cb)
        {
            if (cb == null)
            {
                return;
            }

            CBParams list = deads.Select(item => item.GetFiles()).ToList();

//#if AssetFinderDEBUG
//        Debug.Log("Callback ! " + deads.Count + ":" + heads.Count);
//#endif
            cb(list);
        }

        private bool ReadChunk()
        {
            var alive = false;
            for (var i = 0; i < heads.Count; i++)
            {
                AssetFinderHead h = heads[i];
                if (h.isDead)
                {
                    Debug.LogWarning("Should never be here : " + h.chunkList[0].file);
                    continue;
                }

                nScaned++;
                alive = true;
                h.ReadChunk();
                h.CompareChunk(heads);
                break;
            }

            //if (!alive) return false;

            //alive = false;
            //for (var i = 0; i < heads.Count; i++)
            //{
            //    var h = heads[i];
            //    if (h.isDead) continue;

            //    h.CompareChunk(heads);
            //    alive |= !h.isDead;
            //}

            return alive;
        }
    }

    internal class AssetFinderHead
    {
        public const int chunkSize = 10240;

        public List<AssetFinderChunk> chunkList;
        public int currentChunk;

        public long fileSize;

        public int nChunk;
        public int size; //last stream read size

        public bool isDead
        {
            get { return currentChunk == nChunk || chunkList.Count == 1; }
        }

        public List<string> GetFiles()
        {
            return chunkList.Select(item => item.file).ToList();
        }

        public void AddToDict(byte b, AssetFinderChunk chunk,
            Dictionary<byte, List<AssetFinderChunk>> dict)
        {
            List<AssetFinderChunk> list;
            if (!dict.TryGetValue(b, out list))
            {
                list = new List<AssetFinderChunk>();
                dict.Add(b, list);
            }

            list.Add(chunk);
        }

        public void CloseChunk()
        {
            for (var i = 0; i < chunkList.Count; i++)
            {
                // Debug.Log("stream close");
                AssetFinderFileCompare.streamClosedCount++;
                chunkList[i].stream.Close();
                chunkList[i].stream = null;
            }
        }

        public void ReadChunk()
        {
#if AssetFinderDEBUG
        if (currentChunk == 0) Debug.LogWarning("Read <" + chunkList[0].file + "> " + currentChunk + ":" + nChunk);
#endif
            if (currentChunk == nChunk)
            {
                Debug.LogWarning("Something wrong, should dead <" + isDead + ">");
                return;
            }

            int from = currentChunk * chunkSize;
            size = (int)Mathf.Min(fileSize - from, chunkSize);

            for (var i = 0; i < chunkList.Count; i++)
            {
                AssetFinderChunk chunk = chunkList[i];
                chunk.size = size;
                chunk.stream.Read(chunk.buffer, 0, size);
            }

            currentChunk++;
        }

        public void CompareChunk(List<AssetFinderHead> heads)
        {
            int idx = chunkList.Count;
            byte[] buffer = chunkList[idx - 1].buffer;

            while (--idx >= 0)
            {
                AssetFinderChunk chunk = chunkList[idx];
                int diff = FirstDifferentIndex(buffer, chunk.buffer, size);
                if (diff == -1)
                {
                    continue;
                }
#if AssetFinderDEBUG
            Debug.Log(string.Format(
                " --> Different found at : idx={0} diff={1} size={2} chunk={3}",
            idx, diff, size, currentChunk));
#endif

                byte v = buffer[diff];
                var d = new Dictionary<byte, List<AssetFinderChunk>>(); //new heads
                chunkList.RemoveAt(idx);
                AssetFinderFileCompare.HashChunksNotComplete.Add(chunk);

                AddToDict(chunk.buffer[diff], chunk, d);

                for (int j = idx - 1; j >= 0; j--)
                {
                    AssetFinderChunk tChunk = chunkList[j];
                    byte tValue = tChunk.buffer[diff];
                    if (tValue == v)
                    {
                        continue;
                    }

                    idx--;
                    AssetFinderFileCompare.HashChunksNotComplete.Add(tChunk);
                    chunkList.RemoveAt(j);
                    AddToDict(tChunk.buffer[diff], tChunk, d);
                }

                foreach (KeyValuePair<byte, List<AssetFinderChunk>> item in d)
                {
                    List<AssetFinderChunk> list = item.Value;
                    if (list.Count == 1)
                    {
#if AssetFinderDEBUG
                    Debug.Log(" --> Dead head found for : " + list[0].file);
#endif
                    }
                    else if (list.Count > 1) // 1 : dead head
                    {
#if AssetFinderDEBUG
                    Debug.Log(" --> NEW HEAD : " + list[0].file);
#endif
                        heads.Add(new AssetFinderHead
                        {
                            nChunk = nChunk,
                            fileSize = fileSize,
                            currentChunk = currentChunk - 1,
                            chunkList = list
                        });
                    }
                }
            }
        }

        internal static int FirstDifferentIndex(byte[] arr1, byte[] arr2, int maxIndex)
        {
            for (var i = 0; i < maxIndex; i++)
            {
                if (arr1[i] != arr2[i])
                {
                    return i;
                }
            }

            return -1;
        }
    }

    internal class AssetFinderChunk
    {
        public byte[] buffer;
        public string file;
        public long size;
        public FileStream stream;
    }
}