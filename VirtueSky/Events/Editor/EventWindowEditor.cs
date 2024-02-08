using UnityEditor;


namespace VirtueSky.Events
{
#if UNITY_EDITOR
    using VirtueSky.UtilsEditor;

    public class EventWindowEditor : EditorWindow
    {
        #region Create ScriptableObject Event

        private const string pathEvent = "/Event";
        private const string menuEvent = "Sunflower/Scriptable/Create Event/";

        [MenuItem(menuEvent + "EventNoParam")]
        public static void CreateEventNoParam()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParam>(pathEvent, "so_event_no_param");
        }

        [MenuItem(menuEvent + "String Event")]
        public static void CreateEventString()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEvent>(pathEvent, "so_string_event");
        }

        [MenuItem(menuEvent + "Float Event")]
        public static void CreateEventFloat()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEvent>(pathEvent, "so_float_event");
        }

        [MenuItem(menuEvent + "Integer Event")]
        public static void CreateEventInt()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntegerEvent>(pathEvent, "so_int_event");
        }

        [MenuItem(menuEvent + "Boolean Event")]
        public static void CreateEventBoolean()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BooleanEvent>(pathEvent, "so_bool_event");
        }

        [MenuItem(menuEvent + "Object Event")]
        public static void CreateEventObject()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEvent>(pathEvent, "so_object_event");
        }

        [MenuItem(menuEvent + "Short Double Event")]
        public static void CreateEventShortDouble()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ShortDoubleEvent>(pathEvent, "so_short_double_event");
        }

        [MenuItem(menuEvent + "Dictionary Event")]
        public static void CreateEventDictionary()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<DictionaryEvent>(pathEvent, "so_dictionary_event");
        }

        [MenuItem(menuEvent + "Vector3 Event")]
        public static void CreateEventVector3()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3Event>(pathEvent, "so_vector3_event");
        }

        #endregion
    }
#endif
}