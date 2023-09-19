using UnityEditor;
using UnityEngine;
using VirtueSky.DataStorage;

namespace VirtueSky.EditorControl
{
    public class WindowControl : EditorWindow
    {
        [MenuItem("DataControl/Clear Data %F1")]
        public static void ClearAllData()
        {
            GameData.DelDataInStorage();
            GameData.Clear();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear data succeed</color>");
        }

        [MenuItem("DataControl/Save Data %F2")]
        public static void SaveData()
        {
            GameData.Save();
        }
    }
}