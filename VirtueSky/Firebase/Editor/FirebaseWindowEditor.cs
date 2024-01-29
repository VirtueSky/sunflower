using UnityEditor;
using VirtueSky.Firebase;
using VirtueSky.UtilsEditor;

public class FirebaseWindowEditor : EditorWindow
{
    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase Analytic")]
    public static void CreateLogEventFirebaseAnalytic()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseAnalytic>("/FirebaseAnalytic",
            "log_event_firebase_analytic");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase Analytic No Param")]
    public static void CreateLogEventFirebaseAnalyticNoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseAnalyticNoParam>(
            "/FirebaseAnalytic",
            "log_event_firebase_analytic_no_param");
    }

    [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase Analytic Has Param")]
    public static void CreateLogEventFirebaseAnalyticHasParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<LogEventFirebaseAnalyticHasParam>(
            "/FirebaseAnalytic",
            "log_event_firebase_analytic_has_param");
    }
}