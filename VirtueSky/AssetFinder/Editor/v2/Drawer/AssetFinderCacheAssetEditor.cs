using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    [CustomEditor(typeof(AssetFinderCacheAsset))]
    internal class AssetFinderCacheAssetEditor : UnityEditor.Editor
    {
        private FileUI fileUI;
        private AssetFinderAssetFile file;
        private readonly List<AssetFinderIDRef> fileUsage = new List<AssetFinderIDRef>();
        private readonly List<AssetRefUI> usages = new List<AssetRefUI>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (file == null) return;

            Rect rect = EditorGUILayout.GetControlRect();
            if (fileUI == null) fileUI = AssetUI.Get<FileUI>(file.guid, true);
            if (fileUI == null) return; // Invalid FileUI

            var result = rect.ExtractRight(50f);
            var lbRect = result.Item1;
            var r = result.Item2;
            GUI.Label(lbRect, $"({usages.Count})", EditorStyles.miniLabel);

            var r1 = rect;
            fileUI.DrawAsset(ref r1, true, false);
            
            rect.xMin += 18f;
            rect.y += 18f;
            for (var i = 0; i < usages.Count; i++)
            {
                var item = usages[i];
                item.Draw(ref rect);
            }
        }

        private void OnEnable()
        {
            Selection.selectionChanged -= OnChangeSelection;
            Selection.selectionChanged += OnChangeSelection;
        }

        void OnChangeSelection()
        {
            if (!AssetFinderCacheAsset.isReady) return;

            var obj = Selection.activeObject;
            if (obj == null) return;

            if (!EditorUtility.IsPersistent(obj))
            {
                // Unsupported: SceneObject
                return;
            }

            if (!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(obj, out string guid, out long localId))
            {
                return;
            }

            file = AssetFinderCacheAsset.GetFile(guid);
            
            fileUsage.Clear();
            AssetFinderCacheAsset.CollectUsage(guid, fileUsage);
            
            usages.Clear();
            usages.AddRange(fileUsage
                .Select(item => AssetFinderCacheAsset.GetGuidAndFileId(item.toId))
                .Select(item =>
                {
                    AssetFinderAssetFile asset = AssetFinderCacheAsset.GetFile(item.guid);
                    asset.LoadAllSubAssetsIfNeeded();
                    return item;
                })
                .GroupBy(item => item.guid)
                
                .Select(g => new AssetRefUI(
                    g.Key, AssetDatabase.GUIDToAssetPath(g.Key),
                    g.Select(item => item.fileId).ToList())));
            
            GC.Collect();
            Resources.UnloadUnusedAssets();
            Repaint();
        }
    }
}
