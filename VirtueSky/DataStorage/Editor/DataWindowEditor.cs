using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.DataStorage
{
#if UNITY_EDITOR
    public class DataWindowEditor : EditorWindow
    {
        [MenuItem("Sunflower/Clear All Data", priority = 501)]
        public static void ClearAllData()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear all data succeed</color>");
        }

        [MenuItem("Sunflower/Save Data", priority = 504)]
        public static void SaveData()
        {
            GameData.Save();
            Debug.Log($"<color=Green>Save data succeed</color>");
        }

        [MenuItem("Sunflower/Clear Path Data", priority = 502)]
        public static void ClearSunDataPath()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            Debug.Log($"<color=Green>Clear sun path data succeed</color>");
        }

        [MenuItem("Sunflower/Open Path Data", priority = 503)]
        public static void OpenSunPathData()
        {
            string path = GameData.GetPersistentDataPath();
            switch (SystemInfo.operatingSystemFamily)
            {
                case OperatingSystemFamily.Windows:
                    FileExtension.OpenFolderInExplorer(path);
                    break;
                case OperatingSystemFamily.MacOSX:
                    FileExtension.OpenFolderInFinder(path);
                    break;
            }
        }
    }
#endif
}