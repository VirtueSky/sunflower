using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.AssetFinder.Editor
{
    public interface IWindow
    {
        bool WillRepaint { get; set; }
        void Repaint();
        void OnSelectionChange();
    }

    internal interface IRefDraw
    {
        IWindow window { get; }
        int ElementCount();
        bool DrawLayout();
        bool Draw(Rect rect);
    }

    public abstract class AssetFinderWindowBase : EditorWindow, IWindow
    {
        public bool WillRepaint { get; set; }
        protected bool showFilter, showIgnore;

        //[NonSerialized] protected bool lockSelection;
        //[NonSerialized] internal List<AssetFinderAsset> Selected;

        public static bool isNoticeIgnore;

        public void AddItemsToMenu(GenericMenu menu)
        {
            AssetFinderCache api = AssetFinderCache.Api;
            if (api == null)
            {
                return;
            }

            menu.AddDisabledItem(new GUIContent("Asset Finder - v2.5.1"));
            menu.AddSeparator(string.Empty);

            menu.AddItem(new GUIContent("Enable"), !api.disabled,
                () => { api.disabled = !api.disabled; });
            menu.AddItem(new GUIContent("Refresh"), false, () =>
            {
                //AssetFinderAsset.lastRefreshTS = Time.realtimeSinceStartup;
                AssetFinderCache.Api.Check4Changes(true);
                AssetFinderSceneCache.Api.SetDirty();
            });

#if AssetFinderDEV
            menu.AddItem(new GUIContent("Refresh Usage"), false, () => AssetFinderCache.Api.Check4Usage());
            menu.AddItem(new GUIContent("Refresh Selected"), false, ()=> AssetFinderCache.Api.RefreshSelection());
            menu.AddItem(new GUIContent("Clear Cache"), false, () => AssetFinderCache.Api.Clear());
#endif
        }

        public abstract void OnSelectionChange();
        protected abstract void OnGUI();

#if UNITY_2018_OR_NEWER
        protected void OnSceneChanged(Scene arg0, Scene arg1)
        {
            if (IsFocusingFindInScene || IsFocusingSceneToAsset || IsFocusingSceneInScene)
            {
                OnSelectionChange();
            }
        }
#endif

        protected bool DrawEnable()
        {
            AssetFinderCache api = AssetFinderCache.Api;
            if (api == null)
            {
                return false;
            }

            bool v = api.disabled;
            if (v)
            {
                EditorGUILayout.HelpBox("Find References 2 is disabled!", MessageType.Warning);

                if (GUILayout.Button("Enable"))
                {
                    api.disabled = !api.disabled;
                    Repaint();
                }

                return !api.disabled;
            }

            return !api.disabled;
        }
    }
}