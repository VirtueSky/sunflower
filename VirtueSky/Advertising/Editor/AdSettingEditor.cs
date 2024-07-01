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

        private SerializedProperty _maxBannerVariable;
        private SerializedProperty _maxInterVariable;
        private SerializedProperty _maxRewardVariable;
        private SerializedProperty _maxRewardInterVariable;
        private SerializedProperty _maxAppOpenVariable;


        private SerializedProperty _admobBannerVariable;
        private SerializedProperty _admobInterVariable;
        private SerializedProperty _admobRewardVariable;
        private SerializedProperty _admobRewardInterVariable;
        private SerializedProperty _admobAppOpenVariable;
        private SerializedProperty _admobEnableTestMode;
        private SerializedProperty _enableGDPR;
        private SerializedProperty _enableGDPRTestMode;
        private SerializedProperty _admobDevicesTest;

        private SerializedProperty _androidAppKey;
        private SerializedProperty _iOSAppKey;
        private SerializedProperty _useTestAppKey;
        private SerializedProperty _ironSourceBannerVariable;
        private SerializedProperty _ironSourceInterVariable;
        private SerializedProperty _ironSourceRewardVariable;

        const string pathMax = "/Ads/Applovin";
        const string pathAdmob = "/Ads/Admob";
        const string pathIronSource = "/Ads/IronSource";

        void Initialize()
        {
            _adSetting = target as AdSetting;
            _adNetwork = serializedObject.FindProperty("adNetwork");
            _adCheckingInterval = serializedObject.FindProperty("adCheckingInterval");
            _adLoadingInterval = serializedObject.FindProperty("adLoadingInterval");

            _sdkKey = serializedObject.FindProperty("sdkKey");
            _applovinEnableAgeRestrictedUser =
                serializedObject.FindProperty("applovinEnableAgeRestrictedUser");
            _maxBannerVariable = serializedObject.FindProperty("maxBannerVariable");
            _maxInterVariable = serializedObject.FindProperty("maxInterVariable");
            _maxRewardVariable = serializedObject.FindProperty("maxRewardVariable");
            _maxRewardInterVariable = serializedObject.FindProperty("maxRewardInterVariable");
            _maxAppOpenVariable = serializedObject.FindProperty("maxAppOpenVariable");

            _admobBannerVariable = serializedObject.FindProperty("admobBannerVariable");
            _admobInterVariable = serializedObject.FindProperty("admobInterVariable");
            _admobRewardVariable = serializedObject.FindProperty("admobRewardVariable");
            _admobRewardInterVariable = serializedObject.FindProperty("admobRewardInterVariable");
            _admobAppOpenVariable = serializedObject.FindProperty("admobAppOpenVariable");
            _admobEnableTestMode = serializedObject.FindProperty("admobEnableTestMode");
            _admobDevicesTest = serializedObject.FindProperty("admobDevicesTest");
            _enableGDPR = serializedObject.FindProperty("enableGDPR");
            _enableGDPRTestMode = serializedObject.FindProperty("enableGDPRTestMode");

            _androidAppKey = serializedObject.FindProperty("androidAppKey");
            _iOSAppKey = serializedObject.FindProperty("iOSAppKey");
            _useTestAppKey = serializedObject.FindProperty("useTestAppKey");
            _ironSourceBannerVariable = serializedObject.FindProperty("ironSourceBannerVariable");
            _ironSourceInterVariable = serializedObject.FindProperty("ironSourceInterVariable");
            _ironSourceRewardVariable = serializedObject.FindProperty("ironSourceRewardVariable");
        }

        void Draw()
        {
            serializedObject.Update();
            Initialize();
            // EditorGUILayout.LabelField("ADS SETTING", EditorStyles.boldLabel);
            GuiLine(2);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_adCheckingInterval);
            EditorGUILayout.PropertyField(_adLoadingInterval);
            EditorGUILayout.PropertyField(_adNetwork);
            GUILayout.Space(10);
            switch (_adNetwork.enumValueIndex)
            {
                case (int)AdNetwork.Max:
                    SetupMax();
                    break;
                case (int)AdNetwork.Admob:
                    SetupAdmob();
                    break;
                case (int)AdNetwork.IronSource:
                    SetupIronSource();
                    break;
            }

            EditorUtility.SetDirty(target);
            serializedObject.ApplyModifiedProperties();
        }


        public override void OnInspectorGUI()
        {
            Draw();
        }

        void SetupMax()
        {
            EditorGUILayout.LabelField("MAX", EditorStyles.boldLabel);
            GuiLine(2);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_sdkKey);
            EditorGUILayout.PropertyField(_applovinEnableAgeRestrictedUser);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_maxBannerVariable);
            if (_maxBannerVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxBannerVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxBannerVariable>(pathMax);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_maxInterVariable);
            if (_maxInterVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxInterVariable>(pathMax);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_maxRewardVariable);
            if (_maxRewardVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxRewardVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxRewardVariable>(pathMax);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_maxRewardInterVariable);
            if (_maxRewardInterVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxRewardInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxRewardInterVariable>(pathMax);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_maxAppOpenVariable);
            if (_maxAppOpenVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _maxAppOpenVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<MaxAppOpenVariable>(pathMax);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        void SetupAdmob()
        {
            EditorGUILayout.LabelField("ADMOB", EditorStyles.boldLabel);
            GuiLine(2);
            GUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_admobBannerVariable);
            if (_admobBannerVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobBannerVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobBannerVariable>(pathAdmob);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_admobInterVariable);
            if (_admobInterVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobInterVariable>(pathAdmob);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_admobRewardVariable);
            if (_admobRewardVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobRewardVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobRewardVariable>(pathAdmob);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_admobRewardInterVariable);
            if (_admobRewardInterVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobRewardInterVariable.objectReferenceValue =
                        CreateAsset
                            .CreateAndGetScriptableAsset<AdmobRewardInterVariable>(pathAdmob);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_admobAppOpenVariable);
            if (_admobAppOpenVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _admobAppOpenVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<AdmobAppOpenVariable>(pathAdmob);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.PropertyField(_admobEnableTestMode);
            EditorGUILayout.PropertyField(_admobDevicesTest);
            GUI.enabled = false;
            EditorGUILayout.TextField("App Id Test", "ca-app-pub-3940256099942544~3347511713");
            GUI.enabled = true;
            EditorGUILayout.PropertyField(_enableGDPR);
            if (_enableGDPR.boolValue)
            {
                EditorGUILayout.PropertyField(_enableGDPRTestMode);
            }

            GUILayout.Space(10);
            if (GUILayout.Button("Open GoogleAdmobSetting", GUILayout.Height(20)))
            {
                EditorApplication.ExecuteMenuItem("Assets/Google Mobile Ads/Settings...");
            }
        }

        void SetupIronSource()
        {
            EditorGUILayout.LabelField("IRON-SOURCE", EditorStyles.boldLabel);
            GuiLine(2);
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(_androidAppKey);
            EditorGUILayout.PropertyField(_iOSAppKey);
            EditorGUILayout.PropertyField(_useTestAppKey);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_ironSourceBannerVariable);
            if (_ironSourceBannerVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _ironSourceBannerVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<IronSourceBannerVariable>(pathIronSource);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_ironSourceInterVariable);
            if (_ironSourceInterVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _ironSourceInterVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<IronSourceInterVariable>(pathIronSource);
                }
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(_ironSourceRewardVariable);
            if (_ironSourceRewardVariable.objectReferenceValue == null)
            {
                GUILayout.Space(2);
                if (GUILayout.Button("Create", GUILayout.Width(55)))
                {
                    _ironSourceRewardVariable.objectReferenceValue =
                        CreateAsset.CreateAndGetScriptableAsset<IronSourceRewardVariable>(pathIronSource);
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        void GuiLine(int i_height = 1)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, i_height);

            rect.height = i_height;

            EditorGUI.DrawRect(rect, new Color32(0, 0, 0, 255));
        }
    }
}
#endif