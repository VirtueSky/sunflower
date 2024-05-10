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

        [MenuItem(menuEvent + "GameObject Event")]
        public static void CreateEventGameObject()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3Event>(pathEvent, "so_gameobject_event");
        }

        [MenuItem(menuEvent + "Transform Event")]
        public static void CreateEventTransform()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3Event>(pathEvent, "so_transform_event");
        }

        #endregion

        #region Create Scriptable Event Result

        private const string pathEventResult = "/Event_Result";

        private const string menuEventResult = "Sunflower/Scriptable/Create Event-Result/";


        #region Bool Event - Result

        private const string menuBoolEventResult = "Bool Event/";
        private const string pathBoolEventResult = "/Bool_Event_Result";

        [MenuItem(menuEventResult + menuBoolEventResult + "Bool Result")]
        public static void CreateBoolEventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventBoolResult>(pathEventResult + pathBoolEventResult,
                "bool_event_bool_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "Float Result")]
        public static void CreateBoolEventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventFloatResult>(pathEventResult + pathBoolEventResult,
                "bool_event_float_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "Int Result")]
        public static void CreateBoolEventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventIntResult>(pathEventResult + pathBoolEventResult,
                "bool_event_int_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "Object Result")]
        public static void CreateBoolEventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventObjectResult>(pathEventResult + pathBoolEventResult,
                "bool_event_object_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "String Result")]
        public static void CreateBoolEventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventStringResult>(pathEventResult + pathBoolEventResult,
                "bool_event_string_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "Vector3 Result")]
        public static void CreateBoolEventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventStringResult>(pathEventResult + pathBoolEventResult,
                "bool_event_vector3_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "GameObject Result")]
        public static void CreateBoolEventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventGameObjectResult>(pathEventResult + pathBoolEventResult,
                "bool_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuBoolEventResult + "Transform Result")]
        public static void CreateBoolEventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BoolEventTransformResult>(pathEventResult + pathBoolEventResult,
                "bool_event_transform_result");
        }

        #endregion

        #region Float Event - Result

        private const string menuFloatEventResult = "Float Event/";
        private const string pathFloatEventResult = "/Float_Event_Result";

        [MenuItem(menuEventResult + menuFloatEventResult + "Bool Result")]
        public static void CreateFloatEventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventBoolResult>(pathEventResult + pathFloatEventResult,
                "float_event_bool_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "Float Result")]
        public static void CreateFloatEventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventFloatResult>(pathEventResult + pathFloatEventResult,
                "float_event_float_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "Int Result")]
        public static void CreateFloatEventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventIntResult>(pathEventResult + pathFloatEventResult,
                "float_event_int_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "Object Result")]
        public static void CreateFloatEventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventObjectResult>(pathEventResult + pathFloatEventResult,
                "float_event_object_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "String Result")]
        public static void CreateFloatEventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventStringResult>(pathEventResult + pathFloatEventResult,
                "float_event_string_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "Vector3 Result")]
        public static void CreateFloatEventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventStringResult>(pathEventResult + pathFloatEventResult,
                "float_event_vector3_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "GameObject Result")]
        public static void CreateFloatEventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventGameObjectResult>(
                pathEventResult + pathFloatEventResult,
                "float_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuFloatEventResult + "Transform Result")]
        public static void CreateFloatEventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatEventTransformResult>(
                pathEventResult + pathFloatEventResult,
                "float_event_transform_result");
        }

        #endregion

        #region Int Event - Result

        private const string menuIntEventResult = "Int Event/";
        private const string pathIntEventResult = "/Int_Event_Result";

        [MenuItem(menuEventResult + menuIntEventResult + "Bool Result")]
        public static void CreateIntEventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventBoolResult>(pathEventResult + pathIntEventResult,
                "int_event_bool_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "Float Result")]
        public static void CreateIntEventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventFloatResult>(pathEventResult + pathIntEventResult,
                "int_event_float_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "Int Result")]
        public static void CreateIntEventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventIntResult>(pathEventResult + pathIntEventResult,
                "int_event_int_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "Object Result")]
        public static void CreateIntEventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventObjectResult>(pathEventResult + pathIntEventResult,
                "int_event_object_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "String Result")]
        public static void CreateIntEventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventStringResult>(pathEventResult + pathIntEventResult,
                "int_event_string_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "Vector3 Result")]
        public static void CreateIntEventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventObjectResult>(pathEventResult + pathIntEventResult,
                "int_event_vector3_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "GameObject Result")]
        public static void CreateIntEventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventGameObjectResult>(pathEventResult + pathIntEventResult,
                "int_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuIntEventResult + "Transform Result")]
        public static void CreateIntEventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntEventTransformResult>(pathEventResult + pathIntEventResult,
                "int_event_transform_result");
        }

        #endregion

        #region Object Event - Result

        private const string menuObjectEventResult = "Object Event/";
        private const string pathObjectEventResult = "/Object_Event_Result";

        [MenuItem(menuEventResult + menuObjectEventResult + "Bool Result")]
        public static void CreateObjectEventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventBoolResult>(pathEventResult + pathObjectEventResult,
                "object_event_bool_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "Float Result")]
        public static void CreateObjectEventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventFloatResult>(pathEventResult + pathObjectEventResult,
                "object_event_float_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "Int Result")]
        public static void CreateObjectEventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventIntResult>(pathEventResult + pathObjectEventResult,
                "object_event_int_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "Object Result")]
        public static void CreateObjectEventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventObjectResult>(pathEventResult + pathObjectEventResult,
                "object_event_object_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "String Result")]
        public static void CreateObjectEventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventStringResult>(pathEventResult + pathObjectEventResult,
                "object_event_string_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "Vector3 Result")]
        public static void CreateObjectEventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventVector3Result>(
                pathEventResult + pathObjectEventResult,
                "object_event_vector3_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "GameObject Result")]
        public static void CreateObjectEventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventGameObjectResult>(
                pathEventResult + pathObjectEventResult,
                "object_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuObjectEventResult + "Transform Result")]
        public static void CreateObjectEventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectEventTransformResult>(
                pathEventResult + pathObjectEventResult,
                "object_event_transform_result");
        }

        #endregion

        #region String Event - Result

        private const string menuStringEventResult = "String Event/";
        private const string pathStringEventResult = "/String_Event_Result";

        [MenuItem(menuEventResult + menuStringEventResult + "Bool Result")]
        public static void CreateStringEventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventBoolResult>(pathEventResult + pathStringEventResult,
                "string_event_bool_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "Float Result")]
        public static void CreateStringEventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventFloatResult>(pathEventResult + pathStringEventResult,
                "string_event_float_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "Int Result")]
        public static void CreateStringEventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventIntResult>(pathEventResult + pathStringEventResult,
                "string_event_int_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "Object Result")]
        public static void CreateStringEventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventObjectResult>(pathEventResult + pathStringEventResult,
                "string_event_object_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "String Result")]
        public static void CreateStringEventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventStringResult>(pathEventResult + pathStringEventResult,
                "string_event_string_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "Vector3 Result")]
        public static void CreateStringEventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventVector3Result>(
                pathEventResult + pathStringEventResult,
                "string_event_vector3_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "GameObject Result")]
        public static void CreateStringEventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventGameObjectResult>(
                pathEventResult + pathStringEventResult,
                "string_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuStringEventResult + "Transform Result")]
        public static void CreateStringEventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringEventTransformResult>(
                pathEventResult + pathStringEventResult,
                "string_event_transform_result");
        }

        #endregion

        #region Vector3 Event - Result

        private const string menuVector3EventResult = "Vector3 Event/";
        private const string pathVector3EventResult = "/Vector3_Event_Result";

        [MenuItem(menuEventResult + menuVector3EventResult + "Bool Result")]
        public static void CreateVector3EventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventBoolResult>(pathEventResult + pathVector3EventResult,
                "vector3_event_bool_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "Float Result")]
        public static void CreateVector3EventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventFloatResult>(
                pathEventResult + pathVector3EventResult,
                "vector3_event_float_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "Int Result")]
        public static void CreateVector3EventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventIntResult>(pathEventResult + pathVector3EventResult,
                "vector3_event_int_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "Object Result")]
        public static void CreateVector3EventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventObjectResult>(
                pathEventResult + pathVector3EventResult,
                "vector3_event_object_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "String Result")]
        public static void CreateVector3EventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventStringResult>(
                pathEventResult + pathVector3EventResult,
                "vector3_event_string_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "Vector3 Result")]
        public static void CreateVector3EventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventVector3Result>(
                pathEventResult + pathVector3EventResult,
                "vector3_event_vector3_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "GameObject Result")]
        public static void CreateVector3EventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventGameObjectResult>(
                pathEventResult + pathVector3EventResult,
                "vector3_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuVector3EventResult + "Transform Result")]
        public static void CreateVector3EventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3EventTransformResult>(
                pathEventResult + pathVector3EventResult,
                "vector3_event_transform_result");
        }

        #endregion

        #region Event No Param - Result

        private const string menuEventNoParamResult = "Event No Param/";
        private const string pathEventNoParamResult = "/Event_No_Param_Result";

        [MenuItem(menuEventResult + menuEventNoParamResult + "Bool Result")]
        public static void CreateEventNoParamBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamBoolResult>(pathEventResult + pathEventNoParamResult,
                "event_no_param_bool_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "Float Result")]
        public static void CreateEventNoParamFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamFloatResult>(
                pathEventResult + pathEventNoParamResult,
                "event_no_param_float_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "Int Result")]
        public static void CreateEventNoParamIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamIntResult>(pathEventResult + pathEventNoParamResult,
                "event_no_param_int_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "Object Result")]
        public static void CreateEventNoParamObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamObjectResult>(
                pathEventResult + pathEventNoParamResult,
                "event_no_param_object_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "String Result")]
        public static void CreateEventNoParamStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamStringResult>(
                pathEventResult + pathEventNoParamResult,
                "event_no_param_string_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "Vector3 Result")]
        public static void CreateEventNoParamVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamVector3Result>(
                pathEventResult + pathEventNoParamResult,
                "event_no_param_vector3_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "GameObject Result")]
        public static void CreateEventNoParamGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamGameObjectResult>(
                pathEventResult + pathEventNoParamResult,
                "event_no_param_gameobject_result");
        }

        [MenuItem(menuEventResult + menuEventNoParamResult + "Transform Result")]
        public static void CreateEventNoParamTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<EventNoParamTransformResult>(
                pathEventResult + pathEventNoParamResult,
                "event_no_param_transform_result");
        }

        #endregion

        #region GameObject Event - Result

        private const string menuGameObjectEventResult = "GameObject Event/";
        private const string pathGameObjectEventResult = "/GameObject_Event_Result";

        #endregion

        #region Transform Event - Result

        private const string menuTransformEventResult = "Transform Event/";
        private const string pathTransformEventResult = "/Transform_Event_Result";

        [MenuItem(menuEventResult + menuTransformEventResult + "Bool Result")]
        public static void CreateTransformEventBoolResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventBoolResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_bool_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "Float Result")]
        public static void CreateTransformEventFloatResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventFloatResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_float_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "GameObject Result")]
        public static void CreateTransformEventGameObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventGameObjectResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_gameobject_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "Int Result")]
        public static void CreateTransformEventIntResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventIntResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_int_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "Object Result")]
        public static void CreateTransformEventObjectResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventObjectResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_object_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "String Result")]
        public static void CreateTransformEventStringResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventStringResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_string_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "Transform Result")]
        public static void CreateTransformEventTransformResult()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventTransformResult>(
                pathEventResult + pathTransformEventResult,
                "transform_event_transform_result");
        }

        [MenuItem(menuEventResult + menuTransformEventResult + "Vector3 Result")]
        public static void CreateTransformEventVector3Result()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformEventVector3Result>(
                pathEventResult + pathTransformEventResult,
                "transform_event_vector3_result");
        }

        #endregion

        #endregion
    }
#endif
}