using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.ObjectPooling
{
#if UNITY_EDITOR
    public class PoolWindowEditor : EditorWindow
    {
        #region Create ScriptableObject Pools

        [MenuItem("Sunflower/Create Pools")]
        public static void CreatePools()
        {
            CreateAsset.CreateScriptableAssets<Pools>("/Pools");
        }

        #endregion
    }
#endif
}