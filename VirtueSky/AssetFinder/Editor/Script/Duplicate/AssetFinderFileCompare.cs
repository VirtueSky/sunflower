using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderFileCompare
    {
        public static HashSet<AssetFinderChunk> HashChunksNotComplete;
        internal static int streamClosedCount;
        private List<List<string>> cacheList;
        public List<AssetFinderHead> deads = new List<AssetFinderHead>();
        public List<AssetFinderHead> heads = new List<AssetFinderHead>();

        public int nChunks;
        public int nChunks2;
        public int nScaned;
        public Action<List<List<string>>> OnCompareComplete;
        public Action<List<List<string>>> OnCompareUpdate;

        // Verification tracking
        private Dictionary<string, float> verificationProgress = new Dictionary<string, float>();
        private Dictionary<string, int> verificationOrder = new Dictionary<string, int>();
        private Queue<string> verificationQueue = new Queue<string>();
        private string currentlyVerifying = null;
        private bool signatureScanComplete = false;

        public void Reset(List<List<string>> list, Action<List<List<string>>> onUpdate, Action<List<List<string>>> onComplete)
        {
            nChunks = 0;
            nScaned = 0;
            nChunks2 = 0;

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
            verificationProgress.Clear();
            verificationOrder.Clear();
            verificationQueue.Clear();
            currentlyVerifying = null;
            signatureScanComplete = true; // Since we're using fileInfoHash, signatures are already computed

            OnCompareUpdate = onUpdate;
            OnCompareComplete = onComplete;
            
            if (list == null || list.Count <= 0)
            {
                OnCompareComplete(new List<List<string>>());
                return;
            }

            cacheList = list;

            // Sort groups by file size (smallest first for quicker processing)
            cacheList.Sort((a, b) => 
            {
                long sizeA = new FileInfo(a[0]).Length;
                long sizeB = new FileInfo(b[0]).Length;
                return sizeA.CompareTo(sizeB);
            });
            
            // Setup verification queue
            PrepareVerificationQueue();
        }
        
        private void PrepareVerificationQueue()
        {
            // Create verification queue
            for (int i = 0; i < cacheList.Count; i++)
            {
                string groupKey = "Group-" + i;
                verificationQueue.Enqueue(groupKey);
                verificationOrder[groupKey] = i + 1;
                verificationProgress[groupKey] = 0f;
            }
            
            // Trigger initial update with hash-based results
            OnCompareUpdate?.Invoke(cacheList);
            
            // Start verification
            if (verificationQueue.Count > 0)
            {
                StartNextVerification();
            }
            else
            {
                OnCompareComplete?.Invoke(new List<List<string>>());
            }
        }
        
        private void StartNextVerification()
        {
            if (verificationQueue.Count > 0)
            {
                currentlyVerifying = verificationQueue.Dequeue();
                
                // Use existing byte-by-byte verification
                AddHead(cacheList[cacheList.Count - 1]);
                cacheList.RemoveAt(cacheList.Count - 1);
                
                EditorApplication.update -= ReadChunkAsync;
                EditorApplication.update += ReadChunkAsync;
            }
            else
            {
                // All verifications complete
                currentlyVerifying = null;
                OnCompareComplete?.Invoke(deads.Select(item => item.GetFiles()).ToList());
            }
        }
        
        public void PrioritizeGroup(string groupKey)
        {
            if (verificationQueue.Contains(groupKey))
            {
                // Remove from the queue
                List<string> newQueue = verificationQueue.Where(k => k != groupKey).ToList();
                verificationQueue.Clear();
                
                // Add the prioritized group at the front
                verificationQueue.Enqueue(groupKey);
                
                // Add the rest back
                foreach (string key in newQueue)
                {
                    verificationQueue.Enqueue(key);
                }
                
                // Update order numbers
                int position = 1;
                foreach (string key in verificationQueue)
                {
                    verificationOrder[key] = position++;
                }
            }
        }
        
        public Dictionary<string, float> GetVerificationProgress()
        {
            return verificationProgress;
        }
        
        public Dictionary<string, int> GetVerificationOrder()
        {
            return verificationOrder;
        }
        
        public string GetCurrentlyVerifying()
        {
            return currentlyVerifying;
        }
        
        public bool IsSignatureScanComplete()
        {
            return signatureScanComplete;
        }

        public AssetFinderFileCompare AddHead(List<string> files)
        {
            if (files.Count < 2) Debug.LogWarning("Something wrong ! head should not contains < 2 elements");

            var chunkList = new List<AssetFinderChunk>();
            for (var i = 0; i < files.Count; i++)
            {
                chunkList.Add(new AssetFinderChunk
                {
                    file = files[i],
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
                chunkList = chunkList,
                groupKey = currentlyVerifying
            });

            nChunks += nChunk;

            return this;
        }

        private void ReadChunkAsync()
        {
            bool alive = ReadChunk();
            if (alive) 
            {
                // Update verification progress
                if (currentlyVerifying != null && nChunks > 0)
                {
                    verificationProgress[currentlyVerifying] = (float)nScaned / nChunks;
                }
                return;
            }

            var update = false;
            for (int i = heads.Count - 1; i >= 0; i--)
            {
                AssetFinderHead h = heads[i];
                if (!h.isDead) continue;

                h.CloseChunk();
                heads.RemoveAt(i);
                if (h.chunkList.Count > 1)
                {
                    update = true;
                    deads.Add(h);
                }
            }

            if (update) Trigger(OnCompareUpdate);

            // Set verification as complete for current group
            if (currentlyVerifying != null)
            {
                verificationProgress[currentlyVerifying] = 1f;
            }

            if (cacheList.Count == 0)
            {
                foreach (AssetFinderChunk item in HashChunksNotComplete)
                {
                    if (item.stream == null || !item.stream.CanRead) continue;
                    item.stream.Close();
                    item.stream = null;
                }

                HashChunksNotComplete.Clear();
                nScaned = nChunks;
                EditorApplication.update -= ReadChunkAsync;
                
                // Verification complete, final callback
                Trigger(OnCompareComplete);
            }
            else
            {
                // Continue with next group
                StartNextVerification();
            }
        }

        private void Trigger(Action<List<List<string>>> cb)
        {
            if (cb == null) return;

            List<List<string>> list = deads.Select(item => item.GetFiles()).ToList();
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
                    continue;
                }

                nScaned++;
                alive = true;
                h.ReadChunk();
                h.CompareChunk(heads);
                break;
            }

            return alive;
        }
    }
}
