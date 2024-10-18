using VirtueSky.Localization;
using UnityEditor.IMGUI.Controls;

namespace VirtueSky.LocalizationEditor
{
    public class LocaleTreeViewItem : TreeViewItem
    {
        public LocaleItemBase LocaleItem { get; private set; }
        public AssetTreeViewItem Parent { get; private set; }

        public LocaleTreeViewItem(int id, int depth, LocaleItemBase localeItem, AssetTreeViewItem parent)
            : base(id, depth, "")
        {
            LocaleItem = localeItem;
            Parent = parent;
        }
    }
}