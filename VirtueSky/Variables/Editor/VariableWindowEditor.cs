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

        [MenuItem(menuVariable + "Transform Variable")]
        public static void CreateVariableTransform()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<TransformVariable>(pathVariable,
                "so_transform_variable");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Rect Variable")]
        public static void CreateVariableRect()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<RectVariable>(pathVariable, "so_rect_variable");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Object Variable")]
        public static void CreateVariableObject()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<ObjectVariable>(pathVariable, "so_object_variables");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Boolean Variable")]
        public static void CreateVariableBoolean()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<BooleanVariable>(pathVariable, "so_bool_variables");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Short Double Variable")]
        public static void CreateVariableShortDouble()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<ShortDoubleVariable>(pathVariable,
                "so_short_double_variable");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Vector3 Variable")]
        public static void CreateVariableVector3()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<Vector3Variable>(pathVariable, "so_vector3_variable");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "String Variable")]
        public static void CreateVariableString()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<StringVariable>(pathVariable, "so_string_variable");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Float Variable")]
        public static void CreateVariableFloat()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<FloatVariable>(pathVariable, "so_float_variables");
            so.GetGuid();
        }

        [MenuItem(menuVariable + "Integer Variable")]
        public static void CreateVariableInt()
        {
            var so = CreateAsset.CreateScriptableAssetsOnlyName<IntegerVariable>(pathVariable, "so_int_variable");
            so.GetGuid();
        }

        #endregion
    }
#endif
}