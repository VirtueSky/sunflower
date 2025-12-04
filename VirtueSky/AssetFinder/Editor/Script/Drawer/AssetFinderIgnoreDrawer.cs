using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{
    internal class AssetFinderIgnoreDrawer
    {
        private readonly AssetFinderTreeUI2.GroupDrawer groupIgnore;
        private bool dirty;
        private Dictionary<string, AssetFinderRef> refs;

        public AssetFinderIgnoreDrawer()
        {
            groupIgnore = new AssetFinderTreeUI2.GroupDrawer(DrawGroup, DrawItem)
            {
                hideGroupIfPossible = true
            };

            ApplyFiter();
        }

        private void DrawItem(Rect r, string guid)
        {
            AssetFinderRef rf;
            if (!refs.TryGetValue(guid, out rf)) return;

            if (rf.depth == 1) //mode != Mode.Dependency && 
            {
                Color c = GUI.color;
                GUI.color = Color.blue;
                GUI.DrawTexture(new Rect(r.x - 4f, r.y + 2f, 2f, 2f), EditorGUIUtility.whiteTexture);
                GUI.color = c;
            }

            rf.asset.Draw(
                r,
                new AssetFinderAsset.AssetFinderAssetDrawConfig(
                    false,
                true,
                    false,
                    false,
                    false,
                    false,
                    null,
                    true
                )
            );

            Rect drawR = r;
            drawR.x = drawR.x + drawR.width - 50f; // (groupDrawer.TreeNoScroll() ? 60f : 70f) ;
            drawR.width = 30;
            drawR.y += 1;
            drawR.height -= 2;

            if (GUI.Button(drawR, "X", EditorStyles.miniButton)) AssetFinderSetting.RemoveIgnore(rf.asset.assetPath);
        }

        private void DrawGroup(Rect r, string id, int childCound)
        {
            GUI.Label(r, AssetFinderGUIContent.FromString(id), EditorStyles.boldLabel);
            if (childCound <= 1) return;

            Rect drawR = r;
            drawR.x = drawR.x + drawR.width - 50f; // (groupDrawer.TreeNoScroll() ? 60f : 70f) ;
            drawR.width = 30;
            drawR.y += 1;
            drawR.height -= 2;
        }

        public void SetDirty()
        {
            dirty = true;
        }

        //private float sizeRatio {
        //    get{
        //        if(AssetFinderWindow.window != null)
        //            return AssetFinderWindow.window.sizeRatio;
        //        return .3f;
        //    }
        //}

        public void Draw()
        {
            if (dirty) ApplyFiter();

            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(4f);
                Object[] drops = GUI2.DropZone("Drag & Drop folders here to exclude", 100, 95);
                if ((drops != null) && (drops.Length > 0))
                {
                    for (var i = 0; i < drops.Length; i++)
                    {
                        string path = AssetDatabase.GetAssetPath(drops[i]);
                        if (path.Equals(AssetFinderCache.DEFAULT_CACHE_PATH)) continue;
                        AssetFinderSetting.AddIgnore(path);
                    }
                }

                groupIgnore.DrawLayout();
            }
            GUILayout.EndHorizontal();
        }


        private void ApplyFiter()
        {
            dirty = false;
            refs = new Dictionary<string, AssetFinderRef>();

            //foreach (KeyValuePair<string, List<string>> item in AssetFinderSetting.IgnoreFiltered)
            foreach (string item2 in AssetFinderSetting.s.listIgnore)
            {
                string guid = AssetDatabase.AssetPathToGUID(item2);
                if (string.IsNullOrEmpty(guid)) continue;

                AssetFinderAsset asset = AssetFinderCache.Api.Get(guid, true);
                var r = new AssetFinderRef(0, 0, asset, null, "Ignore");
                refs.Add(guid, r);
            }

            groupIgnore.Reset
            (
                refs.Values.ToList(),
                rf => rf.asset != null ? rf.asset.guid : "",
                GetGroup,
                SortGroup
            );
        }

        private string GetGroup(AssetFinderRef rf)
        {
            return "Ignore";
        }

        private void SortGroup(List<string> groups)
        { }
    }
}
