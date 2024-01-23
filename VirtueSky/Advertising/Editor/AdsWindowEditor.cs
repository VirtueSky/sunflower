#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Ads
{
    public class AdsWindowEditor : EditorWindow
    {
        private Vector2 _scrollPosition;
        private Editor _editor;

        private AdSetting _adSetting;

        private bool isSetupTheme = false;

        [MenuItem("Sunflower/Ads/AdSetting &4", false)]
        public static void OpenAdSettingsWindows()
        {
            var adSetting =
                CreateAsset.CreateAndGetScriptableAsset<VirtueSky.Ads.AdSetting>("/Ads");
            AdsWindowEditor adWindow = GetWindow<AdsWindowEditor>("Ads Settings");
            adWindow._adSetting = adSetting;
            if (adWindow == null)
            {
                Debug.LogError("Couldn't open the ads settings window!");
                return;
            }

            adWindow.minSize = new Vector2(275, 0);
            adWindow.Show();
            EditorGUIUtility.PingObject(adSetting);
        }

        private void OnGUI()
        {
            EditorGUI.DrawRect(new Rect(0, 0, position.width, position.height),
                GameDataEditor.ColorBackgroundRectWindowSunflower.ToColor());
            GUI.contentColor = GameDataEditor.ColorTextContentWindowSunflower.ToColor();
            GUI.backgroundColor = GameDataEditor.ColorContentWindowSunflower.ToColor();
            if (_editor == null) _editor = UnityEditor.Editor.CreateEditor(_adSetting);

            if (_editor == null)
            {
                EditorGUILayout.HelpBox("Couldn't create the settings resources editor.",
                    MessageType.Error);
                return;
            }

            //    _editor.DrawHeader();
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            EditorGUILayout.BeginVertical(new GUIStyle { padding = new RectOffset(6, 3, 3, 3) });
            _editor.OnInspectorGUI();

            GUILayout.Space(10);
            Handles.color = Color.black;
            Handles.DrawAAPolyLine(3, new Vector3(0, GUILayoutUtility.GetLastRect().y + 10),
                new Vector3(position.width, GUILayoutUtility.GetLastRect().y + 10));
            GUILayout.Space(10);


            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        #region Applovin

        private const string pathMax = "/Ads/Applovin";

        [MenuItem("Sunflower/Ads/Applovin/Max Client")]
        public static void CreateMaxClient()
        {
            CreateAsset.CreateScriptableAssets<MaxAdClient>(pathMax, "max_ad_client");
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Banner")]
        public static void CreateMaxBanner()
        {
            CreateAsset.CreateScriptableAssets<MaxBannerVariable>(pathMax, "max_banner_variable");
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Inter")]
        public static void CreateMaxInter()
        {
            CreateAsset.CreateScriptableAssets<MaxInterVariable>(pathMax, "max_inter_variable");
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Reward")]
        public static void CreateMaxReward()
        {
            CreateAsset.CreateScriptableAssets<MaxRewardVariable>(pathMax, "max_reward_variable");
        }

        [MenuItem("Sunflower/Ads/Applovin/Max App Open")]
        public static void CreateMaxAppOpen()
        {
            CreateAsset.CreateScriptableAssets<MaxAppOpenVariable>(pathMax,
                "max_app_open_variable");
        }

        [MenuItem("Sunflower/Ads/Applovin/Max Reward Inter")]
        public static void CreateMaxRewardInter()
        {
            CreateAsset.CreateScriptableAssets<MaxRewardInterVariable>(pathMax,
                "max_reward_inter_variable");
        }

        #endregion

        #region Admob

        private const string pathAdmob = "/Ads/Admob";

        [MenuItem("Sunflower/Ads/Admob/Admob Client")]
        public static void CreateAdmobClient()
        {
            CreateAsset.CreateScriptableAssets<AdmobAdClient>(pathAdmob, "admob_ad_client");
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Banner")]
        public static void CreateAdmobBanner()
        {
            CreateAsset.CreateScriptableAssets<AdmobBannerVariable>(pathAdmob,
                "admob_banner_variable");
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Inter")]
        public static void CreateAdmobInter()
        {
            CreateAsset.CreateScriptableAssets<AdmobInterVariable>(pathAdmob,
                "admob_inter_variable");
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Reward")]
        public static void CreateAdmobReward()
        {
            CreateAsset.CreateScriptableAssets<AdmobRewardVariable>(pathAdmob,
                "admob_reward_variable");
        }

        [MenuItem("Sunflower/Ads/Admob/Admob App Open")]
        public static void CreateAdmobAppOpen()
        {
            CreateAsset.CreateScriptableAssets<AdmobAppOpenVariable>(pathAdmob,
                "admob_app_open_variable");
        }

        [MenuItem("Sunflower/Ads/Admob/Admob Reward Inter")]
        public static void CreateAdmobRewardInter()
        {
            CreateAsset.CreateScriptableAssets<AdmobRewardInterVariable>(pathAdmob,
                "admob_reward_inter_variable");
        }

        #endregion
    }
}
#endif