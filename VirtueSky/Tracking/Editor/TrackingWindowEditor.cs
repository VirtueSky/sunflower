using UnityEditor;
using VirtueSky.Tracking;
using VirtueSky.UtilsEditor;

public class TrackingWindowEditor : EditorWindow
{
    private const string pathFirebaseTracking = "/FirebaseAnalytic_Tracking";
    private const string pathAdjustTracking = "/AdjustTracking";
    private const string pathAppsFlyerTracking = "/AppsFlyerTracking";

    #region Firebase Tracking

    // [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase No Param")]
    public static void CreateLogEventFirebaseNoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseNoParam>(
            pathFirebaseTracking,
            "tracking_firebase_no_param");
    }

    //  [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 1 Param")]
    public static void CreateLogEventFirebaseOneParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseOneParam>(
            pathFirebaseTracking,
            "tracking_firebase_1_param");
    }

    // [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 2 Param")]
    public static void CreateLogEventFirebaseTwoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseTwoParam>(
            pathFirebaseTracking,
            "tracking_firebase_2_param");
    }

    //[MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 3 Param")]
    public static void CreateLogEventFirebaseThreeParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseThreeParam>(
            pathFirebaseTracking,
            "tracking_firebase_3_param");
    }

    //[MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 4 Param")]
    public static void CreateLogEventFirebaseFourParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseFourParam>(
            pathFirebaseTracking,
            "tracking_firebase_4_param");
    }

    // [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 5 Param")]
    public static void CreateLogEventFirebaseFiveParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseFiveParam>(
            pathFirebaseTracking,
            "tracking_firebase_5_param");
    }

    //  [MenuItem("Sunflower/Firebase Analytic/Log Event Firebase 6 Param")]
    public static void CreateLogEventFirebaseSixParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingFirebaseSixParam>(
            pathFirebaseTracking,
            "tracking_firebase_6_param");
    }

    #endregion

    #region AppsFlyer Tracking

    public static void CreateTrackingAfNoParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerNoParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_no_param");
    }

    public static void CreateTrackingAf1Param()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerOneParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_1_param");
    }

    public static void CreateTrackingAf2Param()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerTwoParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_2_param");
    }

    public static void CreateTrackingAf3Param()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerThreeParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_3_param");
    }

    public static void CreateTrackingAf4Param()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerFourParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_4_param");
    }

    public static void CreateTrackingAf5Param()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerFiveParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_5_param");
    }

    public static void CreateTrackingAfHasParam()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAppsFlyerHasParam>(pathAppsFlyerTracking,
            "tracking_appsflyer_has_param");
    }

    #endregion

    public static void CreateTrackingAdjust()
    {
        CreateAsset.CreateScriptableAssetsOnlyName<TrackingAdjust>(pathAdjustTracking, "tracking_adjust");
    }
}