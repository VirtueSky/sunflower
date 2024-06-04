using UnityEditor;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

public class TrackingWindowEditor : EditorWindow
{
    private const string path = "/FirebaseAnalytic_LogEvent";

    // [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase No Param")]
    public static void CreateLogEventFirebaseNoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseNoParam>(
            path,
            "tracking_firebase_no_param");
    }

    //  [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 1 Param")]
    public static void CreateLogEventFirebaseOneParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseOneParam>(
            path,
            "tracking_firebase_1_param");
    }

    // [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 2 Param")]
    public static void CreateLogEventFirebaseTwoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseTwoParam>(
            path,
            "tracking_firebase_2_param");
    }

    //[MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 3 Param")]
    public static void CreateLogEventFirebaseThreeParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseThreeParam>(
            path,
            "tracking_firebase_3_param");
    }

    //[MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 4 Param")]
    public static void CreateLogEventFirebaseFourParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseFourParam>(
            path,
            "tracking_firebase_4_param");
    }

    // [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 5 Param")]
    public static void CreateLogEventFirebaseFiveParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseFiveParam>(
            path,
            "tracking_firebase_5_param");
    }

    //  [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 6 Param")]
    public static void CreateLogEventFirebaseSixParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseSixParam>(
            path,
            "tracking_firebase_6_param");
    }
}