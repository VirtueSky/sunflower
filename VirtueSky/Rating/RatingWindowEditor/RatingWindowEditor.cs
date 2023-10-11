using UnityEditor;
using VirtueSky.Utils;

namespace VirtueSky.Rating
{
#if UNITY_EDITOR
    public class RatingWindowEditor : EditorWindow
    {
        #region In App Review

        [MenuItem("Sunflower/ScriptableObject/InAppReview")]
        public static void CreateInAppReview()
        {
            CreateAsset.CreateScriptableAssets<InAppReview>("/InAppReview", "in_app_review");
        }

        #endregion
    }

#endif
}