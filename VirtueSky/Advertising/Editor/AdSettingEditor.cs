#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.UtilsEditor;

namespace VirtueSky.Ads
{
    [CustomEditor(typeof(AdSetting), true)]
    public class AdSettingEditor : Editor
    {
        private AdSetting _adSetting;
        private SerializedProperty _adNetwork;
        private SerializedProperty _adCheckingInterval;
        private SerializedProperty _adLoadingInterval;

        private SerializedProperty _sdkKey;
        private SerializedProperty _applovinEnableAgeRestrictedUser;
        private SerializedProperty _maxAdClient;
        private SerializedProperty _maxBannerVariable;
        private SerializedProperty _maxInterVariable;
        private SerializedProperty _maxRewardVariable;
        private SerializedProperty _maxRewardInterVariable;
        private SerializedProperty _maxAppOpenVariable;

        private SerializedProperty _admobAdClient;
        private SerializedProperty _admobBannerVariable;
        private SerializedProperty _admobInterVariable;
        private SerializedProperty _admobRewardVariable;
        private SerializedProperty _admobRewardInterVariable;
        private SerializedProperty _admobAppOpenVariable;
        private SerializedProperty _admobEnableTestMode;
        private SerializedProperty _admobEnableGDPR;
        private SerializedProperty _isTestGDPR;
        private SerializedProperty _admobDevicesTest;
        const string pathMax = "/Ads/Applovin";
        const string pathAdmob = "/Ads/Admob";

        void Initialize()
        {
            _adSetting = target as AdSetting;
            _adNetwork = serializedObject.FindProperty("adNetwork");
            _adCheckingInterval = serializedObject.FindProperty("adCheckingInterval");
            _adLoadingInterval = serializedObject.FindProperty("adLoadingInterval");

            _sdkKey = serializedObject.FindProperty("sdkKey");
            _applovinEnableAgeRestrictedUser =
                serializedObject.FindProperty("applovinEnableAgeRestrictedUser");
            _maxAdClient = serializedObject.FindProperty("maxAdClient");
            _maxBannerVariable = serializedObject.FindProperty("maxBannerVariable");
            _maxInterVariable = serializedObject.FindProperty("maxInterVariable");
            _maxRewardVariable = serializedObject.FindProperty("maxRewardVariable");
            _maxRewardInterVariable = serializedObject.FindProperty("maxRewardInterVariable");
            _maxAppOpenVariable = serializedObject.FindProperty("maxAppOpenVariable");

            _admobAdClient = serializedObject.FindProperty("admobAdClient");
            _admobBannerVariable = serializedObject.FindProperty("admobBannerVariable");
            _admobInterVariable = serializedObject.FindProperty("admobInterVariable");
            _admobRewardVariable = serializedObject.FindProperty("admobRewardVariable");
            _admobRewardInterVariable = serializedObject.FindProperty("admobRewardInterVariable");
            _admobAppOpenVariable = serializedObject.FindProperty("admobAppOpenVariable");
            _admobEnableTestMode = serializedObject.FindProperty("admobEnableTestMode");
            _admobEnableGDPR = serializedObject.FindProperty("admobEnableGDPR");
            _isTestGDPR = serializedObject.FindProperty("isTestGDPR");
            _admobDevicesTest = serializedObject.FindProperty("admobDevicesTest");
        }

        void Draw()
        {
            serializedObject.Update();
            Initialize();
            EditorGUILayout.LabelField("ADS SETTING", EditorStyles.boldLabel);
            GuiLine(1);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_adCheckingInterval);
            EditorGUILayout.PropertyField(_adLoadingInterval);
            EditorGUILayout.PropertyField(_adNetwork);
            GUILayout.Space(10);
            SetupMax();
            SetupAdmob();
            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }

        public override void OnInspectorGUI()
        {
            Draw();
        }

        void SetupMax()
        {
            if (_adNetwork.enumValueIndex == (int)AdNetwork.Max)
            {
                EditorGUILayout.LabelField("MAX", EditorStyles.boldLabel);
                GuiLine(1);
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(_sdkKey);
                EditorGUILayout.PropertyField(_applovinEnableAgeRestrictedUser);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_maxAdClient);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxAdClient.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxAdClient>(pathMax);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_maxBannerVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxBannerVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxBannerVariable>(pathMax);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_maxInterVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxInterVariable>(pathMax);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_maxRewardVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxRewardVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxRewardVariable>(pathMax);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_maxRewardInterVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxRewardInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxRewardInterVariable>(pathMax);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_maxAppOpenVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxAppOpenVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxAppOpenVariable>(pathMax);
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        void SetupAdmob()
        {
            if (_adNetwork.enumValueIndex == (int)AdNetwork.Admob)
            {
                EditorGUILayout.LabelField("ADMOB", EditorStyles.boldLabel);
                GuiLine(1);
                GUILayout.Space(10);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_admobAdClient);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobAdClient.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobAdClient>(pathAdmob);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_admobBannerVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobBannerVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobBannerVariable>(pathAdmob);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_admobInterVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobInterVariable>(pathAdmob);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_admobRewardVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobRewardVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobRewardVariable>(pathAdmob);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_admobRewardInterVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobRewardInterVariable.objectReferenceValue =
                        CreateAsset
                            .CreateAndGetScriptableAsset<AdmobRewardInterVariable>(pathAdmob);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(_admobAppOpenVariable);
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobAppOpenVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobAppOpenVariable>(pathAdmob);
                }

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.PropertyField(_admobEnableTestMode);
                EditorGUILayout.PropertyField(_admobEnableGDPR);
                if (_admobEnableGDPR.boolValue)
                {
                    EditorGUILayout.PropertyField(_isTestGDPR);
                }

                EditorGUILayout.PropertyField(_admobDevicesTest);
                GUILayout.Space(10);
                if (GUILayout.Button("Open GoogleAdmobSetting", GUILayout.Height(20)))
                {
                    EditorApplication.ExecuteMenuItem("Assets/Google Mobile Ads/Settings...");
                }
            }
        }

        void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
        }
    }
}
#endif