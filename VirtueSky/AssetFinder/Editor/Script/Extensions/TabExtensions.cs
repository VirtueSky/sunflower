using System;

namespace VirtueSky.AssetFinder.Editor
{
    internal static class TabExtensions
    {
        internal static bool IsFocusing(this AssetFinderTabView tabView, int index)
        {
            return tabView != null && tabView.current == index;
        }

        internal static bool IsFocusingAny(this AssetFinderTabView tabView, params int[] indices)
        {
            if (tabView == null) return false;
            foreach (int index in indices)
            {
                if (tabView.current == index) return true;
            }
            return false;
        }
    }
} 