using UnityEditor;


namespace VirtueSky.Events
{
#if UNITY_EDITOR
    using VirtueSky.UtilsEditor;

    public class EventWindowEditor : EditorWindow
    {
        #region Create ScriptableObject Event

        private const string pathEvent = "/Event";
        private const string menuEvent = "Sunflower/Create Event/";

        [MenuItem(menuEvent + "EventNoParam")]
        public static void CreateEventNoParam()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParam>(pathEvent);
        }

        [MenuItem(menuEvent + "String Event")]
        public static void CreateEventString()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Float Event")]
        public static void CreateEventFloat()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Integer Event")]
        public static void CreateEventInt()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntegerEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Boolean Event")]
        public static void CreateEventBoolean()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BooleanEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Object Event")]
        public static void CreateEventObject()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Short Double Event")]
        public static void CreateEventShortDouble()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ShortDoubleEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Dictionary Event")]
        public static void CreateEventDictionary()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<DictionaryEvent>(pathEvent);
        }

        [MenuItem(menuEvent + "Vector3 Event")]
        public static void CreateEventVector3()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3Event>(pathEvent);
        }

        #endregion
    }
#endif
}