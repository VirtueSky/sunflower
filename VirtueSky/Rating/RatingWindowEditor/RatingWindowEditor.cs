using UnityEditor;


namespace VirtueSky.Rating
{
#if UNITY_EDITOR
    using VirtueSky.UtilsEditor;

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