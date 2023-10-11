using UnityEditor;
using UnityEngine;
using VirtueSky.DataStorage;
using VirtueSky.EditorUtils;
using VirtueSky.Events;
using VirtueSky.ObjectPooling;
using VirtueSky.Rating;
using VirtueSky.Variables;

namespace VirtueSky.EditorControl
{
#if UNITY_EDITOR
    public class WindowControl : EditorWindow
    {
        #region Data

        [MenuItem("Sunflower/Clear Data")]
        public static void ClearAllData()
        {
            GameData.DelDataInStorage();
            GameData.Clear();
            PlayerPrefs.DeleteAll();
            Debug.Log($"<color=Green>Clear data succeed</color>");
        }

        [MenuItem("Sunflower/Save Data")]
        public static void SaveData()
        {
            GameData.Save();
        }

        #endregion

        #region Create ScriptableObject Event

        private const string pathEvent = "/Event";
        private const string menuEvent = "Sunflower/Create Event/";

        [MenuItem(menuEvent + "EventNoParam")]
        public static void CreateEventNoParam()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<EventNoParam>(pathEvent);
        }

        [MenuItem(menuEvent + "String Event")]
        public static void CreateEventString()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<StringEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Float Event")]
        public static void CreateEventFloat()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<FloatEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Integer Event")]
        public static void CreateEventInt()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<IntegerEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Boolean Event")]
        public static void CreateEventBoolean()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<BooleanEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Object Event")]
        public static void CreateEventObject()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<ObjectEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Play Audio Event")]
        public static void CreateEventPlayAudio()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<PlayAudioEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Short Double Event")]
        public static void CreateEventShortDouble()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<ShortDoubleEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Dictionary Event")]
        public static void CreateEventDictionary()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<DictionaryEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Vector3 Event")]
        public static void CreateEventVector3()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<Vector3Event>(pathEvent);
        }

        #endregion

        #region Create ScriptableObject Variable

        private const string pathVariable = "/Variable";
        private const string menuVariable = "Sunflower/Create Variable/";

        [MenuItem(menuVariable + "Transform Variable")]
        public static void CreateVariableTransform()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<TransformVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Rect Variable")]
        public static void CreateVariableRect()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<RectVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Object Variable")]
        public static void CreateVariableObject()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<ObjectVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Boolean Variable")]
        public static void CreateVariableBoolean()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<BooleanVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Short Double Variable")]
        public static void CreateVariableShortDouble()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<ShortDoubleVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Vector3 Variable")]
        public static void CreateVariableVector3()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<Vector3Variable>(pathVariable);
        }

        [MenuItem(menuVariable + "String Variable")]
        public static void CreateVariableString()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<StringVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Float Variable")]
        public static void CreateVariableFloat()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<FloatVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Integer Variable")]
        public static void CreateVariableInt()
        {
            ScriptableSetting.CreateScriptableAssetsOnlyName<IntegerVariable>(pathVariable);
        }

        #endregion

        #region Create ScriptableObject Pools

        [MenuItem("Sunflower/Create Pools")]
        public static void CreatePools()
        {
            ScriptableSetting.CreateScriptableAssets<Pools>("/Pools");
        }

        #endregion

        #region In App Review

        [MenuItem("Sunflower/ScriptableObject/Create InAppReview")]
        public static void CreateInAppReview()
        {
            ScriptableSetting.CreateScriptableAssets<InAppReview>("/InAppReview", "in_app_review");
        }

        #endregion
    }
#endif
}