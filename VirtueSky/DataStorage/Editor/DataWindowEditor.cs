using UnityEditor;
using UnityEngine;
using VirtueSky.UtilsEditor;

namespace VirtueSky.DataStorage
{
#if UNITY_EDITOR
    public class DataWindowEditor : EditorWindow
    {
        [MenuItem("Sunflower/Game Data/Clear All Data")]
        public static void ClearAllData()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear all data succeed</color>");
        }

        [MenuItem("Sunflower/Game Data/Save Data")]
        public static void SaveData()
        {
            GameData.Save();
            Debug.Log($"<color=Green>Save data succeed</color>");
        }

        [MenuItem("Sunflower/Game Data/Clear Sun Path Data")]
        public static void ClearSunDataPath()
        {
            GameData.DeleteAll();
            GameData.DeleteFileData();
            Debug.Log($"<color=Green>Clear sun path data succeed</color>");
        }

        [MenuItem("Sunflower/Game Data/Clear PlayerPrefs Data")]
        public static void ClearPlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear data PlayerPrefs succeed</color>");
        }

        [MenuItem("Sunflower/Game Data/Open Sun Path Data")]
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