using System.IO;
using UnityEditor;
using UnityEngine;

namespace Virtuesky.FolderIcon.Editor
{
    [InitializeOnLoad]
    public class CustomFolder
    {
        static CustomFolder()
        {
            IconDictionaryCreator.BuildDictionary();
            EditorApplication.projectWindowItemOnGUI -= DrawFolderIcon;
            EditorApplication.projectWindowItemOnGUI += DrawFolderIcon;
        }

        static void DrawFolderIcon(string guid, Rect rect)
        {
            if (IconDictionaryCreator.folderIconSettings == null ||
                !IconDictionaryCreator.folderIconSettings.enableFolderIcons) return;
            var path = AssetDatabase.GUIDToAssetPath(guid);
            if (path == "" ||
                Event.current.type != EventType.Repaint ||
                !File.GetAttributes(path).HasFlag(FileAttributes.Directory) ||
                !IconDictionaryCreator.folderIconSettings.folderIconsDictionary.ContainsKey(Path.GetFileName(path)))
            {
                return;
            }


            Rect imageRect;

            if (rect.height > 20)
            {
                imageRect = new Rect(rect.x - 1, rect.y - 1, rect.width + 2, rect.width + 2);
            }
            else if (rect.x > 20)
            {
                imageRect = new Rect(rect.x - 1, rect.y - 1, rect.height + 2, rect.height + 2);
            }
            else
            {
                imageRect = new Rect(rect.x + 2, rect.y - 1, rect.height + 2, rect.height + 2);
            }

            var texture = IconDictionaryCreator.folderIconSettings.folderIconsDictionary[Path.GetFileName(path)];

            if (texture == null)
            {
                return;
            }

            GUI.DrawTexture(imageRect, texture);
        }
    }
}