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
            CreateAsset.CreateScriptableAssetsOnlyName<TransformVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Rect Variable")]
        public static void CreateVariableRect()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<RectVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Object Variable")]
        public static void CreateVariableObject()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ObjectVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Boolean Variable")]
        public static void CreateVariableBoolean()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<BooleanVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Short Double Variable")]
        public static void CreateVariableShortDouble()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<ShortDoubleVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Vector3 Variable")]
        public static void CreateVariableVector3()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<Vector3Variable>(pathVariable);
        }

        [MenuItem(menuVariable + "String Variable")]
        public static void CreateVariableString()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<StringVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Float Variable")]
        public static void CreateVariableFloat()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<FloatVariable>(pathVariable);
        }

        [MenuItem(menuVariable + "Integer Variable")]
        public static void CreateVariableInt()
        {
            CreateAsset.CreateScriptableAssetsOnlyName<IntegerVariable>(pathVariable);
        }

        #endregion
    }
#endif
}