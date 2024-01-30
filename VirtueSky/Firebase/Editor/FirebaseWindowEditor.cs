using UnityEditor;
using VirtueSky.FirebaseTraking;
using VirtueSky.UtilsEditor;

public class FirebaseWindowEditor : EditorWindow
{
    private const string path = "/FirebaseAnalytic_LogEvent";

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase No Param")]
    public static void CreateLogEventFirebaseNoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseNoParam>(
            path,
            "log_event_firebase_no_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 1 Param")]
    public static void CreateLogEventFirebaseOneParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseOneParam>(
            path,
            "log_event_firebase_1_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 2 Param")]
    public static void CreateLogEventFirebaseTwoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseTwoParam>(
            path,
            "log_event_firebase_2_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 3 Param")]
    public static void CreateLogEventFirebaseThreeParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseThreeParam>(
            path,
            "log_event_firebase_3_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 4 Param")]
    public static void CreateLogEventFirebaseFourParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseFourParam>(
            path,
            "log_event_firebase_4_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 5 Param")]
    public static void CreateLogEventFirebaseFiveParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseFiveParam>(
            path,
            "log_event_firebase_5_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 6 Param")]
    public static void CreateLogEventFirebaseSixParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseSixParam>(
            path,
            "log_event_firebase_6_param");
    }
}