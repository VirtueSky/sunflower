//#define AssetFinderDEBUG

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace VirtueSky.AssetFinder.Editor
{

    internal class AssetFinderSplitView
    {
        private const float SPLIT_SIZE = 2f;


        private readonly GUILayoutOption[] expandWH =
        {
            GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true)
        };


        private readonly IWindow window;
        private Rect _rect;

        private int _visibleCount;
        private bool dirty;

        public bool isHorz;

        private int resizeIndex = -1;
        public bool hasResize;
        public List<Info> splits = new List<Info>();

        public AssetFinderSplitView(IWindow w)
        {
            window = w;
        }

        public bool isVisible => _visibleCount > 0;

        public void CalculateWeight()
        {
            _visibleCount = 0;
            var _totalWeight = 0f;

            for (var i = 0; i < splits.Count; i++)
            {
                Info info = splits[i];
                if (!info.visible) continue;

                info.stIndex = _visibleCount;
                _totalWeight += info.weight;

                _visibleCount++;
            }

            if (_visibleCount == 0 || _totalWeight == 0)
            {
                Debug.LogWarning("Nothing visible!");
                return;
            }

            var cWeight = 0f;
            for (var i = 0; i < splits.Count; i++)
            {
                Info info = splits[i];
                if (!info.visible) continue;

                cWeight += info.weight;
                info.normWeight = info.weight / _totalWeight;
            }
        }

        public void Draw(Rect rect)
        {
            if (rect.width > 0 || rect.height > 0) _rect = rect;

            if (dirty)
            {
                dirty = false;
                CalculateWeight();
            }

            if (resizeIndex == -1) ApplySizePolicies();

            float sz = (_visibleCount - 1) * SPLIT_SIZE;
            float dx = _rect.x;
            float dy = _rect.y;

            for (var i = 0; i < splits.Count; i++)
            {
                Info info = splits[i];
                if (!info.visible) continue;

                var rr = new Rect
                (
                    dx, dy,
                    isHorz ? (_rect.width - sz) * info.normWeight : _rect.width,
                    isHorz ? _rect.height : (_rect.height - sz) * info.normWeight
                );

                if ((rr.width > 0) && (rr.height > 0)) info.rect = rr;

                if (info.draw != null) info.DoDraw();

                if (info.sizePolicy == Info.SizePolicy.KeepPixel && Event.current.type == EventType.Repaint)
                {
                    float current = isHorz ? rr.width : rr.height;
                    info.preferredPixel = Mathf.Max(info.minPixel, current);
                }

                if (info.stIndex < _visibleCount - 1) DrawSpliter(i, isHorz ? info.rect.xMax : info.rect.yMax);

                if (isHorz)
                {
                    dx += info.rect.width + SPLIT_SIZE;
                } else
                {
                    dy += info.rect.height + SPLIT_SIZE;
                }
            }
        }

        private void ApplySizePolicies()
        {
            int visible = 0;
            for (int i = 0; i < splits.Count; i++) if (splits[i].visible) visible++;
            if (visible == 0) return;

            float totalGaps = (visible - 1) * SPLIT_SIZE;
            float available = isHorz ? _rect.width : _rect.height;
            float content = Mathf.Max(0f, available - totalGaps);

            float fixedPixels = 0f;
            float flexibleBasis = 0f;
            for (int i = 0; i < splits.Count; i++)
            {
                var sp = splits[i];
                if (!sp.visible) continue;
                if (sp.sizePolicy == Info.SizePolicy.KeepPixel)
                {
                    float pref = sp.preferredPixel;
                    pref = Mathf.Max(sp.minPixel, pref);
                    fixedPixels += Mathf.Max(0f, pref);
                }
                else
                {
                    flexibleBasis += Mathf.Max(0.0001f, sp.weight);
                }
            }

            float scale = 1f;
            if (fixedPixels > content && fixedPixels > 0f) scale = content / fixedPixels;
            float remaining = Mathf.Max(0f, content - Mathf.Min(fixedPixels, content));

            for (int i = 0; i < splits.Count; i++)
            {
                var sp = splits[i];
                if (!sp.visible) continue;
                if (sp.sizePolicy == Info.SizePolicy.KeepPixel)
                {
                    float pref = sp.preferredPixel;
                    pref = Mathf.Max(sp.minPixel, pref);
                    sp.weight = Mathf.Max(0f, pref) * scale;
                }
            }

            if (remaining > 0f)
            {
                float basis = Mathf.Max(0.0001f, flexibleBasis);
                for (int i = 0; i < splits.Count; i++)
                {
                    var sp = splits[i];
                    if (!sp.visible) continue;
                    if (sp.sizePolicy == Info.SizePolicy.Flexible)
                    {
                        float share = Mathf.Max(0.0001f, sp.weight) / basis;
                        sp.weight = remaining * share;
                    }
                }
            }

            CalculateWeight();
        }

        public void DrawLayout()
        {
            Rect rect = StartLayout(isHorz);
            {
                Draw(rect);
            }
            EndLayout(isHorz);
        }


        private void RefreshSpliterPos(int index, float px)
        {
			Info sp1 = splits[index];
			int rightIndex = -1;
            for (int j = index + 1; j < splits.Count; j++)
            {
                if (splits[j].visible)
                {
                    rightIndex = j;
                    break;
                }
            }
            if (rightIndex < 0) return;
			Info sp2 = splits[rightIndex];

            Rect r1 = sp1.rect;
            Rect r2 = sp2.rect;

            float w1 = sp1.weight;
            float w2 = sp2.weight;
            float tt = w1 + w2;

			float dd = isHorz ? r2.xMax - r1.xMin - SPLIT_SIZE : r2.yMax - r1.yMin - SPLIT_SIZE;
			float m = isHorz ? Event.current.mousePosition.x - r1.x : Event.current.mousePosition.y - r1.y;

			// Enforce minimum pixel sizes for panels that prefer keeping pixel size
			float leftMin = 0f;
			float rightMin = 0f;
			if (sp1.sizePolicy == Info.SizePolicy.KeepPixel) leftMin = Mathf.Max(0f, sp1.minPixel);
			if (sp2.sizePolicy == Info.SizePolicy.KeepPixel) rightMin = Mathf.Max(0f, sp2.minPixel);
			float lower = Mathf.Min(dd - rightMin, leftMin);
			float upper = Mathf.Max(leftMin, dd - rightMin);
			m = Mathf.Clamp(m, lower, upper);
            float pct = Mathf.Min(0.9f, Mathf.Max(0.1f, m / dd));

            sp1.weight = tt * pct;
            sp2.weight = tt * (1 - pct);

            dirty = true;
            if (window != null) window.WillRepaint = true;
        }

        private void DrawSpliter(int index, float px)
        {
            Rect dRect = _rect;

            if (isHorz)
            {
                dRect.x = px;
                dRect.width = SPLIT_SIZE;
            } else
            {
                dRect.y = px;
                dRect.height = SPLIT_SIZE;
            }

            if (Event.current.type == EventType.Repaint || Event.current.type == EventType.MouseMove) GUI2.Rect(dRect, Color.black, 0.4f);

            var dRect2 = GUI2.Padding(dRect, -2f, -2f);
            EditorGUIUtility.AddCursorRect(dRect2, isHorz ? MouseCursor.ResizeHorizontal : MouseCursor.ResizeVertical);
            if ((Event.current.type == EventType.MouseDown) && dRect2.Contains(Event.current.mousePosition))
            {
                resizeIndex = index;
                RefreshSpliterPos(index, px);
                hasResize = true;
            }

            if (resizeIndex == index) RefreshSpliterPos(index, px);
            if (Event.current.type == EventType.MouseUp)
            {
                resizeIndex = -1;
                hasResize = false;
            }
        }

        private Rect StartLayout(bool horz)
        {
            return horz
                ? EditorGUILayout.BeginHorizontal(expandWH)
                : EditorGUILayout.BeginVertical(expandWH);
        }

        private void EndLayout(bool horz)
        {
            if (horz)
            {
                EditorGUILayout.EndHorizontal();
            } else
            {
                EditorGUILayout.EndVertical();
            }
        }

        [Serializable]
        internal class Info
        {
            public GUIContent title;
            public Rect rect;
            public float normWeight;
            public int stIndex;

            public bool visible = true;
            public float weight = 1f;
            public Action<Rect> draw;

            public enum SizePolicy { Flexible, KeepPixel }
            public SizePolicy sizePolicy = SizePolicy.Flexible;
            public float preferredPixel = 200f;
            public float minPixel = 50f;

            // Dynamic title support
            public Func<GUIContent> GetDynamicTitle;

            // Drawer dirty state support
            public Func<bool> GetDrawerDirtyState;
            
            // Refresh action support
            public Action OnRefresh;

            public void DoDraw()
            {
                Rect drawRect = rect;

                // Use dynamic title if available, otherwise use static title
                GUIContent baseTitle = GetDynamicTitle?.Invoke() ?? title;

                if (baseTitle != null)
                {
                    var titleRect = new Rect(rect.x, rect.y, rect.width, 20f);
                    GUI2.Rect(titleRect, Color.black, 0.2f);

                    titleRect.xMin += 4f;

                    // Check dirty state and modify title accordingly
                    Color originalColor = GUI.contentColor;
                    bool isDirty = GetDrawerDirtyState?.Invoke() ?? false;
                    
                    // Create title with asterisk if dirty
                    string titleText = baseTitle.text;
                    if (isDirty && !titleText.EndsWith("*"))
                    {
                        titleText += "*";
                        // Use theme-appropriate dirty indicator color instead of hardcoded yellow
                        GUI.contentColor = EditorGUIUtility.isProSkin 
                            ? AssetFinderTheme.Dark.DirtyIndicator 
                            : AssetFinderTheme.Light.DirtyIndicator;
                    }

                    // Calculate available space for refresh button only if OnRefresh is available
                    // Increased button width from 50f to 55f, adjusted status area accordingly
                    float statusAreaWidth = OnRefresh != null ? (isDirty ? 205f : 55f) : 0f;
                    
                    // Draw main title
                    var mainTitleRect = new Rect(titleRect.x, titleRect.y, titleRect.width - statusAreaWidth, titleRect.height);
                    GUI.Label(mainTitleRect, new GUIContent(titleText, baseTitle.image, baseTitle.tooltip), EditorStyles.label);

                    // Draw status message and refresh button only if OnRefresh is available
                    if (OnRefresh != null)
                    {
                        if (isDirty)
                        {
                            // Status message - shorter text to fit
                            var statusRect = new Rect(mainTitleRect.xMax + 5f, titleRect.y + 2f, 145f, titleRect.height);
                            GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 0.8f); // Very dim color
                            GUI.Label(statusRect, "* possibly incomplete result", EditorStyles.miniLabel);
                        }
                        
                        GUI.contentColor = originalColor;
                        // Increased refresh button width from 50f to 55f
                        var refreshRect = new Rect(titleRect.xMax - 55f, titleRect.y + 1f, 53f, 14f);
                        if (GUI.Button(refreshRect, "Refresh", EditorStyles.miniButtonRight))
                        {
                            OnRefresh.Invoke();
                        }
                    }
                    
                    GUI.contentColor = originalColor;
                    drawRect.yMin += 20f;
                }
                draw(drawRect);
            }
        }
    }
}
