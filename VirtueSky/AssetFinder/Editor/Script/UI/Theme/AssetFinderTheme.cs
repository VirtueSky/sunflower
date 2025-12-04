using UnityEngine;
using UnityEditor;

namespace VirtueSky.AssetFinder.Editor
{
    internal partial class AssetFinderTheme
    {
        public static AssetFinderTheme Current => Dark;
        public static AssetFinderTheme Dark { get; } = CreateDarkTheme();
        public static AssetFinderTheme Light { get; } = CreateLightTheme();

        private AssetFinderTheme()
        {
            InitializeLayoutOptions();
        }

        private void InitializeLayoutOptions()
        {
            ToolbarHeightOption = GUILayout.Height(ToolbarHeight);
            CompactButtonHeight = GUILayout.Height(16f);
            StandardButtonHeight = GUILayout.Height(18f);
            ActionButtonHeight = GUILayout.Height(30f);
            CancelButtonHeight = GUILayout.Height(25f);
            WarningCloseButtonHeight = GUILayout.Height(38f);
            CloseButtonWidth = GUILayout.Width(20f);
            ToolbarButtonWidth = GUILayout.Width(24f);
            IconButtonWidth = GUILayout.Width(IconButtonSize);
            RefreshButtonWidth = GUILayout.Width(RefreshButtonSize);
            ApplyButtonWidth = GUILayout.Width(100f);
            TabPanelWidth = GUILayout.Width(160f);
            RecursiveSearchLabelWidth = GUILayout.Width(100f);
            ToggleWidth = GUILayout.Width(20f);
            SelectionPanelWidth = GUILayout.Width(150f);
            ViewModeSelectorWidth = GUILayout.Width(200f);
            SettingsPanelHeight = GUILayout.Height(100f);
            DropdownWidth = GUILayout.Width(320f);
            LockButtonWidth = GUILayout.Width(150f);
        }

        // ============ METRICS ============
        public readonly float ToolbarHeight = 30f;
        public readonly float TreeItemHeight = 18f;
        public readonly float TreeItemSpacing = 1f;
        public readonly float TreeContentPadding = 2f;
        public readonly float TreeIndentSize = 18f;
        public readonly float TreeToggleSize = 16f;
        public readonly float AssetIconSize = 16f;
        public readonly float IconButtonSize = 24f;
        public readonly float RefreshButtonSize = 18f;
        public readonly float ScrollBarWidth = 18f;
        public readonly float CompactSpacing = 4f;
        public readonly float FooterButtonsOffset = 100f;
        public readonly float TreeItemOffset = 18f;
        public readonly float ViewModeSelectorWidthValue = 200f;

        // ============ LAYOUT OPTIONS ============
        public GUILayoutOption ToolbarHeightOption;
        public GUILayoutOption CompactButtonHeight;
        public GUILayoutOption StandardButtonHeight;
        public GUILayoutOption ActionButtonHeight;
        public GUILayoutOption CancelButtonHeight;
        public GUILayoutOption WarningCloseButtonHeight;
        public GUILayoutOption CloseButtonWidth;
        public GUILayoutOption ToolbarButtonWidth;
        public GUILayoutOption IconButtonWidth;
        public GUILayoutOption RefreshButtonWidth;
        public GUILayoutOption ApplyButtonWidth;
        public GUILayoutOption TabPanelWidth;
        public GUILayoutOption RecursiveSearchLabelWidth;
        public GUILayoutOption ToggleWidth;
        public GUILayoutOption SelectionPanelWidth;
        public GUILayoutOption ViewModeSelectorWidth;
        public GUILayoutOption SettingsPanelHeight;
        public GUILayoutOption DropdownWidth;
        public GUILayoutOption LockButtonWidth;

        // ============ COLORS ============
        public Color SelectionHighlight;
        public Color SelectionHighlightInactive;
        public Color ErrorColor;
        public Color WarningColor;
        public Color InfoColor;
        public Color SuccessColor;
        public Color SceneHighlight;
        public Color SeparatorLine;
        public Color DirtyIndicator;
        
        public Rect GetProgressBarRect()
        {
            return GUILayoutUtility.GetRect(1f, Screen.width, 18f, 18f);
        }

        public Rect GetRefreshButtonRect(Rect parentRect)
        {
            return new Rect(parentRect.xMax - AssetIconSize, parentRect.yMin - 14f, RefreshButtonSize, RefreshButtonSize);
        }
    }
}