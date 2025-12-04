using System;
using System.Collections.Generic;
namespace VirtueSky.AssetFinder.Editor
{
    partial class AssetFinderWindowAll
    {
        [Serializable] internal class PanelSettings
        {
            public bool selection;
            public bool horzLayout;
            public bool scene = true;
            public bool asset = true;
            public bool details;
            public bool bookmark;
            public bool toolMode;

            public bool showFullPath = true;
            public bool showFileSize;
            public bool showFileExtension;
            public bool showUsageType = true;
            public bool writeImportLog;
            public bool recursiveUnusedScan = true;

            public AssetFinderRefDrawer.Mode toolGroupMode = AssetFinderRefDrawer.Mode.Type;
            public AssetFinderRefDrawer.Mode groupMode = AssetFinderRefDrawer.Mode.Dependency;
            public AssetFinderRefDrawer.Sort sortMode = AssetFinderRefDrawer.Sort.Path;

            public int mainTabIndex = 0; // For main tabs (e.g. Uses/Used By/Addressables)
            public int toolTabIndex = 0; // For toolTabs (Duplicate/GUID/Unused/In Build/Others)
            public int othersTabIndex = 0; // For vertical tab bar in 'Others' section

            // Remember pixel sizes for fixed panels
            public float selectionPanelPixel = 200f;
            public float detailsPanelPixel = 150f;  
            public float bookmarkPanelPixel = 150f;
        }
    }
}
