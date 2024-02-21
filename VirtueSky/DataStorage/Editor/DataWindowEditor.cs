using UnityEditor;
using UnityEngine;

namespace VirtueSky.DataStorage
{
#if UNITY_EDITOR
    public class DataWindowEditor : EditorWindow
    {
        #region Data

        [MenuItem("Sunflower/Game Data/Clear Data")]
        public static void ClearAllData()
        {
            GameData.DelDataInStorage();
            GameData.Clear();
            Data.DeleteAll();
            Data.DeleteFileData();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear data succeed</color>");
        }

        [MenuItem("Sunflower/Game Data/Save Data")]
        public static void SaveData()
        {
            GameData.Save();
            Debug.Log($"<color=Green>Save data succeed</color>");
        }

        #endregion
    }
#endif
}