using System;
using System.Collections.Generic;
using UnityEngine;
using VirtueSky.Inspector;

namespace Virtuesky.FolderIcon.Editor
{
    [HideMonoScript]
    public class FolderIconSettings : ScriptableObject
    {
        public bool setupIconDefault;
        public bool customIcon;

        [ShowIf(nameof(customIcon)), TableList]
        public List<FolderIconData> folderIconDatas;

        public void OnValidate()
        {
            IconDictionaryCreator.BuildDictionary();
        }
    }

    [Serializable]
    public class FolderIconData
    {
        public Texture2D icon;
        public string folderName;
    }
}