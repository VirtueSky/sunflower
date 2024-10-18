using VirtueSky.Localization;
using UnityEditor.IMGUI.Controls;

namespace VirtueSky.LocalizationEditor
{
    public class AssetTreeViewItem : TreeViewItem
    {
        private bool _isDirty;

        /// <summary>
        /// Gets or sets item as dirty. Added "*" postfix to the display name if is dirty.
        /// </summary>
        public bool IsDirty
        {
            get => _isDirty;
            set
            {
                _isDirty = value;
                if (value)
                {
                    displayName = Asset.name + "*";
                }
                else
                {
                    displayName = Asset.name;
                }
            }
        }

        public ScriptableLocaleBase Asset { get; private set; }

        public AssetTreeViewItem(int depth, ScriptableLocaleBase data)
            : base(data.GetInstanceID(), depth, data.name)
        {
            Asset = data;
        }
    }
}