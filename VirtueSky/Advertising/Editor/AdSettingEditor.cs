#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

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
        private SerializedProperty _admobDevicesTest;

        private void OnEnable()
        {
            _adSetting = target as AdSetting;
            _adNetwork = serializedObject.FindProperty("adNetwork");
            _adCheckingInterval = serializedObject.FindProperty("adCheckingInterval");
            _adLoadingInterval = serializedObject.FindProperty("adLoadingInterval");

            _sdkKey = serializedObject.FindProperty("sdkKey");
            _applovinEnableAgeRestrictedUser = serializedObject.FindProperty("applovinEnableAgeRestrictedUser");
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
            _admobDevicesTest = serializedObject.FindProperty("admobDevicesTest");
        }

        public override void OnInspectorGUI()
        {
            // base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(_adCheckingInterval);
            EditorGUILayout.PropertyField(_adLoadingInterval);
            EditorGUILayout.PropertyField(_adNetwork);

            SetupMax();
            SetupAdmob();
            SetUpButton();

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        void SetupMax()
        {
            if (_adNetwork.enumValueIndex == (int)AdNetwork.Applovin)
            {
                EditorGUILayout.PropertyField(_sdkKey);
                EditorGUILayout.PropertyField(_applovinEnableAgeRestrictedUser);
                EditorGUILayout.PropertyField(_maxAdClient);
                EditorGUILayout.PropertyField(_maxBannerVariable);
                EditorGUILayout.PropertyField(_maxInterVariable);
                EditorGUILayout.PropertyField(_maxRewardVariable);
                EditorGUILayout.PropertyField(_maxRewardInterVariable);
                EditorGUILayout.PropertyField(_maxAppOpenVariable);
            }
        }

        void SetupAdmob()
        {
            if (_adNetwork.enumValueIndex == (int)AdNetwork.Admob)
            {
                EditorGUILayout.PropertyField(_admobAdClient);
                EditorGUILayout.PropertyField(_admobBannerVariable);
                EditorGUILayout.PropertyField(_admobInterVariable);
                EditorGUILayout.PropertyField(_admobRewardVariable);
                EditorGUILayout.PropertyField(_admobRewardInterVariable);
                EditorGUILayout.PropertyField(_admobAppOpenVariable);
                EditorGUILayout.PropertyField(_admobEnableTestMode);
                EditorGUILayout.PropertyField(_admobDevicesTest);
            }
        }

        void SetUpButton()
        {
            if (_adNetwork.enumValueIndex == (int)AdNetwork.Applovin && GUILayout.Button("Create MaxClient And MaxVariable"))
            {
                _adSetting.CreateMax();
            }

            if (_adNetwork.enumValueIndex == (int)AdNetwork.Admob && GUILayout.Button("Create AdmobClient And AdmobVariable"))
            {
                _adSetting.CreateAdmob();
            }
        }
    }
}
#endif