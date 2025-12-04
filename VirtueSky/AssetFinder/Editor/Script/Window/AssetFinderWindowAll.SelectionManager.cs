using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        internal UnityObject[] _cachedSelection;
        internal int _cachedSelectionFrame = -1;
        private string[] ids;
        
        private void OnSelectionManagerChanged()
        {
            if (selection == null) return;
            if (selection.isLock) return;
            
            // Check and consume ping lock state - if active, skip sync but hide warnings
            bool hadPingLock = smartLock?.ConsumePingLockState() ?? false;
            if (hadPingLock)
            {
                AssetFinderLOG.Log("Skipped refresh: Ping lock was active - keeping current FR2 selection!");
                
                // Still need to check if selection is out of sync for UI highlighting
                var unitySelection = AssetFinderSelectionManager.Instance.GetUnitySelection();
                var fr2Selection = selection.GetUnityObjects();
                isSelectionOutOfSync = !AreSelectionsEqual(unitySelection, fr2Selection);
                
                WillRepaint = true;
                Repaint();
                return;
            }
            
            // IMPORTANT: Always update FR2 selection regardless of cache readiness
            // Selection listening must work independently of cache status
            
            var shouldRefresh = smartLock.ShouldRefreshWithSmartLogic(this, selection.GetUnityObjects());
            if (shouldRefresh)
            {
                selection.SyncFromGlobalSelection();
                isSelectionOutOfSync = false; // Selection is now in sync
                
                // Only refresh FR2 view if cache is ready - this prevents errors but keeps selection updated
                if (AssetFinderCache.isReady)
                {
                    RefreshFR2View();
                }
                else
                {
                    AssetFinderLOG.Log("Cache not ready - selection updated but view refresh skipped");
                }
            }
            else
            {
                isSelectionOutOfSync = true; // Selection is now out of sync
            }
            
            WillRepaint = true;
            Repaint();
        }
        
        
        private void OnDisableSelectionManager()
        {
            AssetFinderSelectionManager.SelectionChanged -= OnSelectionManagerChanged;
        }
        
        public override void OnSelectionChange()
        {
            // DO NOTHING
        }

        void OnPanelSelectionChanged()
        {
            if (!AssetFinderCache.isReady) return;
            if (SceneUsesDrawer == null) InitIfNeeded();
            if (UsesDrawer == null) InitIfNeeded();
            if (selection == null) return; // Additional safety check
            
            navigationHistory.SetWindow(this);

            // Use unified selection manager (static access only)
            UnityObject[] currentSelection = AssetFinderSelectionManager.Instance.GetUnitySelection();
            
            if (currentSelection.Length > 0)
            {
                navigationHistory.RecordSelection(currentSelection);
            }
            
            if (isFocusingGUIDs)
            {
                if (guidObjs == null) guidObjs = new Dictionary<string, UnityObject>();
                else guidObjs.Clear();
                
                for (var i = 0; i < currentSelection.Length; i++)
                {
                    UnityObject item = currentSelection[i];
#if UNITY_2018_1_OR_NEWER
                    {
                        var guid = "";
                        long fileid = -1;
                        try
                        {
                            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(item, out guid, out fileid))
                            {
                                guidObjs.Add(guid + "/" + fileid, currentSelection[i]);
                            }
                        }
                        catch (Exception e)
                        {
                            AssetFinderLOG.LogWarning($"TryGetGUIDAndLocalFileIdentifier {item}\nException: {e}");
                        }
                    }
#else
                    {
                        var path = AssetDatabase.GetAssetPath(item);
                        if (string.IsNullOrEmpty(path)) continue;
                        var guid = AssetDatabase.AssetPathToGUID(path);
                        System.Reflection.PropertyInfo inspectorModeInfo =
                        typeof(SerializedObject).GetProperty("inspectorMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

                        SerializedObject serializedObject = new SerializedObject(item);
                        inspectorModeInfo.SetValue(serializedObject, InspectorMode.Debug, null);

                        SerializedProperty localIdProp =
                            serializedObject.FindProperty("m_LocalIdentfierInFile");

                        var localId = localIdProp.longValue;
                        if (localId <= 0)
                        {
                            localId = localIdProp.intValue;
                        }
                        if (localId <= 0)
                        {
                            continue;
                        }
                        if (!string.IsNullOrEmpty(guid)) guidObjs.Add(guid + "/" + localId, currentSelection[i]);
                    }
#endif
                }
            }

            if (isFocusingUnused)
            {
                RefUnUse.ResetUnusedAsset(settings.recursiveUnusedScan);
            }
        }

        internal void SetFR2Selection(UnityObject[] objects)
        {
            selection?.SetUnityObjects(objects);
            
            // Only refresh FR2 view if cache is ready - this prevents errors but keeps selection updated
            if (AssetFinderCache.isReady)
            {
                RefreshFR2View();
            }
            else
            {
                AssetFinderLOG.Log("Cache not ready - selection updated but view refresh skipped in SetFR2Selection");
            }
        }
        
        internal UnityObject[] GetFR2Selection()
        {
            return selection?.GetUnityObjects() ?? Array.Empty<UnityObject>();
        }
        
        private void RefreshFR2View()
        {
            OnPanelSelectionChanged();
            
            // Only refresh selection view if we have a selection object
            if (selection != null)
            {
                selection.RefreshView();
            }
            
            // Refresh contextual messages based on current selection
            if (this is AssetFinderWindowAll window)
            {
                window.RefreshContextualMessages();
            }
            
            ids = Array.Empty<string>();
            RefreshPanelVisible();

            if (selection.isSelectingSceneObject)
            {
                // Get GameObjects from selection's instance IDs
                var gameObjects = new List<UnityObject>();
                foreach (string instIdStr in selection.instSet)
                {
                    if (int.TryParse(instIdStr, out int instId))
                    {
                        var obj = EditorUtility.InstanceIDToObject(instId);
                        if (obj != null) gameObjects.Add(obj);
                    }
                }
                
                RefSceneInScene.ResetSceneInScene(gameObjects.OfType<GameObject>().ToArray());
                SceneToAssetDrawer.Reset(gameObjects.OfType<GameObject>().ToArray(), true, true);
                SceneUsesDrawer.ResetSceneUseSceneObjects(gameObjects.OfType<GameObject>().ToArray());
            }
            else if (selection.isSelectingAsset)
            {
                ids = selection.guidSet.ToArray();
                
                // These are the key calls that refresh the Uses/Used By tabs
                UsesDrawer.Reset(ids, true);
                UsedByDrawer.Reset(ids, false);
                RefInScene.Reset(ids);
                AddressableDrawer.RefreshView();
            }
        }

#if UNITY_2018_OR_NEWER
        private void OnSceneChanged(Scene arg0, Scene arg1)
        {
            if (IsFocusingFindInScene || IsFocusingSceneToAsset || IsFocusingSceneInScene)
            {
                OnSelectionChange();
            }
        }
#endif
    }
} 