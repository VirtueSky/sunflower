using Microsoft.Win32.SafeHandles;
using UnityEditor;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Variables
{
#if UNITY_EDITOR
    public class VariableWindowEditor : EditorWindow
    {
        #region Create ScriptableObject Variable

        private const string pathVariable = "/Variable";
        private const string menuVariable = "Sunflower/Scriptable/Create Variable/";

        [MenuItem(menuVariable + "Transform Variable", priority = 201)]
        public static void CreateVariableTransform()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<TransformVariable>(pathVariable,
                "so_transform_variable");
        }

        [MenuItem(menuVariable + "Rect Variable", priority = 201)]
        public static void CreateVariableRect()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<RectVariable>(pathVariable, "so_rect_variable");
        }

        [MenuItem(menuVariable + "Object Variable", priority = 201)]
        public static void CreateVariableObject()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<ObjectVariable>(pathVariable, "so_object_variables");
        }

        [MenuItem(menuVariable + "Boolean Variable", priority = 201)]
        public static void CreateVariableBoolean()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<BooleanVariable>(pathVariable, "so_bool_variables");
        }

        [MenuItem(menuVariable + "Short Double Variable", priority = 201)]
        public static void CreateVariableShortDouble()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<ShortDoubleVariable>(pathVariable,
                "so_short_double_variable");
        }

        [MenuItem(menuVariable + "Vector3 Variable", priority = 201)]
        public static void CreateVariableVector3()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<Vector3Variable>(pathVariable, "so_vector3_variable");
        }

        [MenuItem(menuVariable + "Vector2 Variable", priority = 201)]
        public static void CreateVariableVector2()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<Vector2Variable>(pathVariable, "so_vector2_variable");
        }

        [MenuItem(menuVariable + "String Variable", priority = 201)]
        public static void CreateVariableString()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<StringVariable>(pathVariable, "so_string_variable");
        }

        [MenuItem(menuVariable + "Float Variable", priority = 201)]
        public static void CreateVariableFloat()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<FloatVariable>(pathVariable, "so_float_variables");
        }

        [MenuItem(menuVariable + "Integer Variable", priority = 201)]
        public static void CreateVariableInt()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<IntegerVariable>(pathVariable, "so_int_variable");
        }

        #endregion
    }
#endif
}