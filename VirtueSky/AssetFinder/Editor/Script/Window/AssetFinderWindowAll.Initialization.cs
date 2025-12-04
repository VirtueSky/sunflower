using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderWindowAll
    {
        private void InitializeComponents()
        {
            
            // Initialize UI components first to ensure selection exists before selection manager events
            InitializeUIComponents();
            InitializeNavigationHistory();
            InitializeDrawers();
            InitializeTools();
            InitializeDrawerProperties();
            
            InitTabs();
            InitPanes();
            
            // Initialize selection manager AFTER everything else is ready
            InitializeSelectionManager();
            
            if (AssetFinderCache.isReady)
            {
                RefreshActiveTab();
                RefreshFR2View();
            }
            else
            {
                // Debug.LogWarning("FR2 is not Ready just yet!");    
                AssetFinderCache.onReady -= RefreshActiveTab;
                AssetFinderCache.onReady += RefreshActiveTab;
                AssetFinderCache.onReady -= RefreshFR2View;
                AssetFinderCache.onReady += RefreshFR2View;
            }
            
            Repaint();
        }

        void RefreshActiveTab()
        {
            AssetFinderCache.onReady -= RefreshActiveTab;
            AssetFinderCache.onReady -= RefreshFR2View;
            
            // If tabs are not initialized yet, we'll defer this call
            // OnGUI2 will handle calling tab changes when tabs are initialized
            if (tabs == null || toolTabs == null)
            {
                return;
            }
            
            if (settings.toolMode)
            {
                toolTabs.onTabChange?.Invoke();
            }
            else
            {
                tabs.onTabChange?.Invoke();
            }
            
            // If selection was out of sync due to cache not being ready, sync now
            if (isSelectionOutOfSync && selection != null && !selection.isLock)
            {
                selection.SyncFromGlobalSelection();
                RefreshFR2View();
                isSelectionOutOfSync = false;
            }
        }
        

        private void InitializeNavigationHistory()
        {
            if (navigationHistory == null) navigationHistory = new AssetFinderNavigationHistory();
            navigationHistory.SetWindow(this);
        }

        private void InitializeSelectionManager()
        {
            AssetFinderSelectionManager.SelectionChanged -= OnSelectionManagerChanged;
            AssetFinderSelectionManager.SelectionChanged += OnSelectionManagerChanged;
        }

        private void InitializeDrawers()
        {
            UsesDrawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.groupMode,
                showFullPath = settings.showFullPath,
                showFileSize = settings.showFileSize,
                showExtension = settings.showFileExtension,
                showUsageType = settings.showUsageType,
                showAssetBundleName = AssetFinderSetting.s.displayAssetBundleName,
                showAtlasName = AssetFinderSetting.s.displayAtlasName,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => true,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "[Selected Assets] are not [USING] (depends on / contains reference to) any other assets!",
                GetContextualEmptyMessage = () => cachedUsesMessage
            };

            UsedByDrawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.groupMode,
                showFullPath = settings.showFullPath,
                showFileSize = settings.showFileSize,
                showExtension = settings.showFileExtension,
                showUsageType = settings.showUsageType,
                showAssetBundleName = AssetFinderSetting.s.displayAssetBundleName,
                showAtlasName = AssetFinderSetting.s.displayAtlasName,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => !isFocusingUsedBy,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "[Selected Assets] are not [USED BY] any other assets!",
                GetContextualEmptyMessage = () => cachedUsedByMessage
            };

            AddressableDrawer = new AssetFinderAddressableDrawer(this, () => settings.sortMode, () => settings.groupMode);

            Duplicated = new AssetFinderDuplicateTree2(this, () => settings.sortMode, () => settings.toolGroupMode);

            RefInScene = new AssetFinderRefDrawer(new AssetFinderRefDrawer.SceneDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.groupMode,
                showFullPath = settings.showFullPath,
                showDetails = true,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => true,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "[Selected Assets] are not [USED BY] any GameObjects in current scene!",
                GetContextualEmptyMessage = () => cachedRefInSceneMessage
            };

            RefSceneInScene = new AssetFinderRefDrawer(new AssetFinderRefDrawer.SceneDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.groupMode,
                showFullPath = settings.showFullPath,
                showDetails = true,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => true,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "[Selected GameObjects] are not [USED BY] any GameObjects in current scene!",
                GetContextualEmptyMessage = () => cachedSceneInSceneMessage
            };

            SceneUsesDrawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.SceneDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.groupMode,
                showFullPath = settings.showFullPath,
                showDetails = true,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => true,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "[Selected GameObjects] are not [USING] any GameObjects in current scene!",
                GetContextualEmptyMessage = () => cachedSceneUsesMessage
            };

            SceneToAssetDrawer = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.groupMode,
                showFullPath = settings.showFullPath,
                showFileSize = settings.showFileSize,
                showExtension = settings.showFileExtension,
                showUsageType = settings.showUsageType,
                showAssetBundleName = AssetFinderSetting.s.displayAssetBundleName,
                showAtlasName = AssetFinderSetting.s.displayAtlasName,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => true,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "[Selected GameObjects] are not [USING] any assets!",
                GetContextualEmptyMessage = () => cachedSceneToAssetMessage
            };

            RefUnUse = new AssetFinderRefDrawer(new AssetFinderRefDrawer.AssetDrawingConfig
            {
                window = this,
                getSortMode = () => settings.sortMode,
                getGroupMode = () => settings.toolGroupMode,
                showFullPath = settings.showFullPath,
                showFileSize = settings.showFileSize,
                showExtension = settings.showFileExtension,
                showUsageType = settings.showUsageType,
                showAssetBundleName = AssetFinderSetting.s.displayAssetBundleName,
                showAtlasName = AssetFinderSetting.s.displayAtlasName,
                showToggle = true,
                shouldShowExtension = () => settings.showFileExtension,
                shouldShowDetailButton = () => !isFocusingUnused,
                onCacheInvalidated = () => { }
            })
            {
                messageEmpty = "Wow! No unused assets found!",
                // RefUnUse doesn't need contextual messages as it's not selection-dependent
            };
        }

        private void InitializeTools()
        {
            UsedInBuild = new AssetFinderUsedInBuild(this, () => settings.sortMode, () => settings.toolGroupMode);
            MissingReference = new AssetFinderMissingReference(this, () => settings.sortMode, () => settings.toolGroupMode);
            AssetOrganizer = new AssetFinderAssetOrganizer(this, () => settings.sortMode, () => settings.toolGroupMode);
            DeleteEmptyFolder = new AssetFinderDeleteEmptyFolder(this, () => settings.sortMode, () => settings.toolGroupMode);
        }

        private void InitializeUIComponents()
        {
            selection = new AssetFinderSelection(this, () => settings.sortMode, () => settings.groupMode);
            selection.OnSelectionChanged -= OnLocalSelectionChanged;
            selection.OnSelectionChanged += OnLocalSelectionChanged;
            bookmark = new AssetFinderBookmark(this, () => settings.sortMode, () => settings.groupMode);
            
            // Setup bookmark cache invalidation callback - each drawer will invalidate its own cache
            AssetFinderBookmark.OnBookmarkChanged = () => {
                UsesDrawer?.InvalidateGroupCache();
                UsedByDrawer?.InvalidateGroupCache();
                RefUnUse?.InvalidateGroupCache();
                RefInScene?.InvalidateGroupCache();
                SceneToAssetDrawer?.InvalidateGroupCache();
                SceneUsesDrawer?.InvalidateGroupCache();
                RefSceneInScene?.InvalidateGroupCache();
            };
            
            // Initial sync with Unity selection - delay until after full initialization
            EditorApplication.delayCall += () =>
            {
                if (selection == null) return;
                if (!AssetFinderCache.isReady) return;
                
                selection.SyncFromGlobalSelection();
                isSelectionOutOfSync = false; // Reset flag after initial sync
                RefreshFR2View();
            };
        }

        private void OnLocalSelectionChanged()
        {
            // When local selection changes (user interacts with selection panel), 
            // refresh the Uses/Used By tabs to reflect the current selection
            if (selection != null)
            {
                // Debug.Log($"OnLocalSelectionChanged - Count: {selection.Count}, IsSelectingAsset: {selection.isSelectingAsset}, GuidCount: {selection.guidSet.Count}");
                RefreshFR2View();
            }
        }

        private void InitializeDrawerProperties()
        {
            this.CacheAllDrawers();
            this.RefreshShowFileExtension();
            this.RefreshShowFullPath();
            this.RefreshShowFileSize();
            this.RefreshShowUsageType();
        }

        private void InitPanes()
        {
            sp2 = new AssetFinderSplitView(this)
            {
                isHorz = false,
                splits = new List<AssetFinderSplitView.Info>
                {
                    new AssetFinderSplitView.Info
                    {
                        title = new GUIContent("Scene", AssetFinderIcon.Scene.image),
                        draw = DrawScene,
                        visible = settings.scene,
                        GetDynamicTitle = GetScenePanelTitle,
                        GetDrawerDirtyState = IsScenePanelDirty,
                        OnRefresh = () => AssetFinderSceneCache.Api.ForceRefresh()
                    },
                    new AssetFinderSplitView.Info
                    {
                        title = new GUIContent("Assets", AssetFinderIcon.Asset.image),
                        draw = DrawAsset,
                        visible = settings.asset,
                        GetDynamicTitle = GetAssetPanelTitle,
                        GetDrawerDirtyState = IsAssetPanelDirty,
                        OnRefresh = () => AssetFinderCache.Api.IncrementalRefresh()
                    },
                    new AssetFinderSplitView.Info
                        { title = null, draw = rect => AddressableDrawer.Draw(rect), visible = false }
                }
            };

            sp2.CalculateWeight();

            sp1 = new AssetFinderSplitView(this)
            {
                isHorz = true,
                splits = new List<AssetFinderSplitView.Info>
                {
                    new AssetFinderSplitView.Info
                    {
                        title = null, //new GUIContent("Selection"),
                        draw = DrawSelectionPanel,
                        weight = 0f,
                        visible = settings.selection,
                        sizePolicy = AssetFinderSplitView.Info.SizePolicy.KeepPixel,
                        preferredPixel = settings.selectionPanelPixel
                    },
                    new AssetFinderSplitView.Info
                    {
                        title = null,
                        draw = _ => sp2.Draw(_),
                        weight = 1f,
                        visible = true,
                        sizePolicy = AssetFinderSplitView.Info.SizePolicy.Flexible
                    },
                    new AssetFinderSplitView.Info
                    {
                        title = new GUIContent("Details", AssetFinderIcon.Hierarchy.image),
                        draw = DrawDetailsPanel,
                        weight = 0f,
                        visible = settings.details,
                        sizePolicy = AssetFinderSplitView.Info.SizePolicy.KeepPixel,
                        preferredPixel = settings.detailsPanelPixel
                    },
                    new AssetFinderSplitView.Info
                    {
                        title = new GUIContent("Bookmark", AssetFinderIcon.Favorite.image),
                        draw = _ => bookmark.Draw(_),
                        weight = 0f,
                        visible = settings.bookmark,
                        sizePolicy = AssetFinderSplitView.Info.SizePolicy.KeepPixel,
                        preferredPixel = settings.bookmarkPanelPixel
                    }
                }
            };

            sp1.CalculateWeight();
        }

        private void InitTabs()
        {
            bottomTabs = AssetFinderTabView.Create(this, true,
                new GUIContent(AssetFinderIcon.Setting.image, "Settings"),
                new GUIContent(AssetFinderIcon.Ignore.image, "Ignore"),
                new GUIContent(AssetFinderIcon.Filter.image, "Filter by Type")
            );
            bottomTabs.current = -1;
            bottomTabs.flexibleWidth = false;
            bottomTabs.onTabChange = () => { 
                // Bottom tab changes work directly on FR2 selection - no locks needed
            };

            toolTabs = AssetFinderTabView.Create(this, false, "Duplicate", "GUID", "Unused", "In Build", "Others");
            toolTabs.current = settings.toolTabIndex;
            toolTabs.onTabChange = () =>
            {
                settings.toolTabIndex = toolTabs.current;

                if (toolTabs.current == 0) // Duplicate
                {
                    if (Duplicated != null)
                    {
                        Duplicated.SetDirty();
                        Duplicated.RefreshSort();
                    }
                }

                if (toolTabs.current == 1) // GUID
                {
                    // GUIDs tool doesn't use drawer system, no action needed
                }

                if (toolTabs.current == 2) // Unused
                {
                    if (RefUnUse != null)
                    {
                        RefUnUse.ResetUnusedAsset(settings.recursiveUnusedScan);
                        RefUnUse.SetDirty();
                        RefUnUse.RefreshSort();
                    }
                }

                if (toolTabs.current == 3) // UsedInBuild
                {
                    if (UsedInBuild != null)
                    {
                        UsedInBuild.SetDirty();
                        UsedInBuild.RefreshSort();
                    }
                }

                if (toolTabs.current == 4) // Others
                {
                    // Others tab has its own internal tab system, no action needed
                }
                
                // Ensure proper group mode restrictions for tools that need them
                if (toolTabs.IsFocusingAny(2, 3)) // Unused or UsedInBuild
                {
                    if (!allowedModes.Contains(settings.toolGroupMode))
                    {
                        settings.toolGroupMode = AssetFinderRefDrawer.Mode.Type;
                    }
                }
                
                Repaint();
            };
            
            if (AssetFinderAddressable.asmStatus == AssetFinderAddressable.ASMStatus.AsmNotFound)
            { // No Addressable
                tabs = AssetFinderTabView.Create(this, false, // , "Tools"
                    "Uses", "Used By"
                );
            } else
            {
                tabs = AssetFinderTabView.Create(this, false, // , "Tools"
                    "Uses", "Used By", "Addressables"
                );
            }
            
            tabs.onTabChange = () =>
            {
                settings.mainTabIndex = tabs.current;
                OnTabChange();
            };
            tabs.current = settings.mainTabIndex;
            
            const float IconW = 24f;
            const float LockButtonW = 150f; // Fixed width for lock button with text
            const float BookmarkW = 44f;
            tabs.offsetFirst = IconW * 2 + LockButtonW; // prev, next, lock(with text)
            tabs.offsetLast = IconW * 3 + BookmarkW;

            tabs.callback = new DrawCallback
            {
                BeforeDraw = rect =>
                {
                    if (navigationHistory == null) navigationHistory = new AssetFinderNavigationHistory();
                    
                    rect.width = IconW;
                    
                    // Previous button
                    bool canGoBack = navigationHistory.CanGoBack;
                    EditorGUI.BeginDisabledGroup(!canGoBack);
                    if (GUI.Button(rect, "<", EditorStyles.toolbarButton))
                    {
                        navigationHistory.GoBack();
                        GUIUtility.ExitGUI(); // Prevent layout errors
                    }
                    EditorGUI.EndDisabledGroup();
                    rect.x += IconW;
                    
                    // Next button  
                    bool canGoForward = navigationHistory.CanGoForward;
                    EditorGUI.BeginDisabledGroup(!canGoForward);
                    if (GUI.Button(rect, ">", EditorStyles.toolbarButton))
                    {
                        navigationHistory.GoForward();
                        GUIUtility.ExitGUI(); // Prevent layout errors
                    }
                    EditorGUI.EndDisabledGroup();
                    rect.x += IconW;
                    
                    // Lock/SmartLock button area - fixed width with text content
                    rect.width = LockButtonW;
                    
                    {
                        // Normal lock button with selection count
                        UnityObject[] fr2CurrentSelection = GetFR2Selection();
                        int selectionCount = fr2CurrentSelection?.Length ?? 0;
                        
                        // Split the button area - left side for selection info, right side for lock icon
                        Rect selectionRect = rect;
                        selectionRect.width = LockButtonW - 30f; // Leave space for lock icon
                        
                        Rect lockIconRect = rect;
                        lockIconRect.x = selectionRect.xMax;
                        lockIconRect.width = 30f;
                        
                        // Selection info button (clicking toggles selection visibility)
                        string selectionText = selectionCount > 0 ? $"Selection ({selectionCount})" : "Selection";
                        GUIContent selectionContent = new GUIContent(selectionText, "Click to toggle selection panel");
                        
                        // Highlight with subtle yellow if selection is out of sync
                        Color originalBgColor = GUI.color;
                        if (isSelectionOutOfSync)
                        {
                            GUI.color = new Color(1f, 1f, 0f, 1f); // Subtle yellow tint
                        }
                        
                        if (GUI.Button(selectionRect, selectionContent, EditorStyles.toolbarButton))
                        {
                            settings.selection = !settings.selection;
                            sp1.splits[0].visible = settings.selection;
                            sp1.CalculateWeight();
                            WillRepaint = true;
                        }
                        
                        // Restore original background color
                        GUI.color = originalBgColor;
                        
                        // Lock icon button (clicking locks/unlocks selection)
                        GUIContent lockIconContent = new GUIContent(
                            selection.isLock ? AssetFinderIcon.Lock.image : AssetFinderIcon.Unlock.image,
                            selection.isLock ? "Unlock Selection" : "Lock Selection"
                        );
                        
                        // Set green background when locked, similar to other toggle buttons
                        Color originalBgColor2 = GUI.backgroundColor;
                        if (selection.isLock)
                        {
                            GUI.backgroundColor = new Color(0.7f, 1f, 0.7f, 1f); // Same green as other toggle buttons
                        }
                        
                        if (GUI.Button(lockIconRect, lockIconContent, EditorStyles.toolbarButton))
                        {
                            selection.isLock = !selection.isLock;
                            WillRepaint = true;
                        }
                        
                        // Restore original background color
                        GUI.backgroundColor = originalBgColor2;
                    }
                },

                AfterDraw = rect =>
                {
                    rect.xMin = rect.xMax - (IconW * 3 + BookmarkW);
                    rect.width = IconW;

                    // Scene toggle with content indication
                    if (GUI2.ToolbarToggle(ref settings.scene, AssetFinderIcon.Scene.image, Vector2.zero, "Show / Hide Scene References", rect, ScenePanelHasContent()))
                    {
                        if ((settings.asset == false) && (settings.scene == false))
                        {
                            settings.asset = true;
                            sp2.splits[1].visible = settings.asset;
                        }

                        RefreshPanelVisible();
                        Repaint();
                    }

                    rect.x += IconW;
                    if (GUI2.ToolbarToggle(ref settings.asset, AssetFinderIcon.Asset.image, Vector2.zero, "Show / Hide Asset References", rect, AssetPanelHasContent()))
                    {
                        if ((settings.asset == false) && (settings.scene == false))
                        {
                            settings.scene = true;
                            sp2.splits[0].visible = settings.scene;
                        }

                        RefreshPanelVisible();
                        Repaint();
                    }

                    rect.x += IconW;
                    if (GUI2.ToolbarToggle(ref settings.details, AssetFinderIcon.Details.image, Vector2.zero, "Show / Hide Details", rect))
                    {
                        sp1.splits[2].visible = settings.details;
                        sp1.CalculateWeight();
                        Repaint();
                    }

                    rect.x += IconW;
                    {
                        rect.width = BookmarkW;
                        int bookmarkCount = AssetFinderBookmark.Count;
                        bool hasBookmarks = bookmarkCount > 0;

                        Color originalBg = GUI.backgroundColor;
                        if (hasBookmarks)
                        {
                            GUI.backgroundColor = new Color(0.7f, 1f, 0.7f, 1f);
                        }

                        var bookmarkTitle = AssetFinderGUIContent.FromTexture(AssetFinderIcon.Favorite.image, "Show / Hide Bookmarks");
                        bookmarkTitle.text = hasBookmarks
                            ? (bookmarkCount > 99 ? "99+" : $"{bookmarkCount}")
                            : string.Empty;

                        if (GUI2.ToolbarToggle(rect, ref settings.bookmark, bookmarkTitle))
                        {
                            sp1.splits[3].visible = settings.bookmark;
                            sp1.CalculateWeight();
                            Repaint();
                        }
                        
                        GUI.backgroundColor = originalBg;
                    }
                }
            };
        }



    }
}