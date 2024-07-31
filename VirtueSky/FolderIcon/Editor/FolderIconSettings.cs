using UnityEngine;
using VirtueSky.DataType;
using VirtueSky.Inspector;

namespace Virtuesky.FolderIcon.Editor
{
    [HideMonoScript]
    public class FolderIconSettings : ScriptableObject
    {
        public bool enableFolderIcons;
        public bool enableAutoCustomIconsDefault;

        [ShowIf(nameof(enableFolderIcons)), SerializeField]
        internal DictionaryCustom<string, Texture2D> folderIconsDictionary;

        public void ClearCache()
        {
            folderIconsDictionary.Clear();
        }

        public void OnValidate()
        {
            IconDictionaryCreator.BuildDictionary();
        }
    }
}