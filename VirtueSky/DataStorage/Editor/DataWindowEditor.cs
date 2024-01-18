using UnityEditor;
using UnityEngine;

namespace VirtueSky.DataStorage
{
#if UNITY_EDITOR
    public class DataWindowEditor : EditorWindow
    {
        #region Data

        [MenuItem("GameBase/Clear Data")]
        public static void ClearAllData()
        {
            GameData.DelDataInStorage();
            GameData.Clear();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear data succeed</color>");
        }

        [MenuItem("GameBase/Save Data")]
        public static void SaveData()
        {
            GameData.Save();
        }

        #endregion
    }
#endif
}