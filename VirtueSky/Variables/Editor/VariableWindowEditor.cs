using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Variables
{
#if UNITY_EDITOR
    public class VariableWindowEditor : EditorWindow
    {
        #region Create ScriptableObject Variable

        private const string pathVariable = "/Variable";
        private const string menuVariable = "Sunflower/Create Variable/";

        [MenuItem(menuVariable + "Transform Variable")]
        public static void CreateVariableTransform()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<TransformVariable>(pathVariable, "so_transform_variable");
        }

        [MenuItem(menuVariable + "Rect Variable")]
        public static void CreateVariableRect()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<RectVariable>(pathVariable, "so_rect_variable");
        }

        [MenuItem(menuVariable + "Object Variable")]
        public static void CreateVariableObject()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectVariable>(pathVariable, "so_object_variables");
        }

        [MenuItem(menuVariable + "Boolean Variable")]
        public static void CreateVariableBoolean()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BooleanVariable>(pathVariable, "so_bool_variables");
        }

        [MenuItem(menuVariable + "Short Double Variable")]
        public static void CreateVariableShortDouble()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ShortDoubleVariable>(pathVariable, "so_short_double_variable");
        }

        [MenuItem(menuVariable + "Vector3 Variable")]
        public static void CreateVariableVector3()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3Variable>(pathVariable, "so_vector3_variable");
        }

        [MenuItem(menuVariable + "String Variable")]
        public static void CreateVariableString()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringVariable>(pathVariable, "so_string_variable");
        }

        [MenuItem(menuVariable + "Float Variable")]
        public static void CreateVariableFloat()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatVariable>(pathVariable, "so_float_variables");
        }

        [MenuItem(menuVariable + "Integer Variable")]
        public static void CreateVariableInt()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntegerVariable>(pathVariable, "so_int_variable");
        }

        #endregion
    }
#endif
}