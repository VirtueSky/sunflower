using System;

#if VIRTUESKY_FIREBASE_REMOTECONFIG
using Firebase.RemoteConfig;
#endif

using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Misc;
using VirtueSky.Variables;

namespace VirtueSky.RemoteConfigs
{
    [Serializable]
    public class FirebaseRemoteConfigData
    {
        public string key;
        public TypeRemoteConfigData typeRemoteConfigData;

        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.StringData)]
        public StringVariable stringValue;

        [GUIColor(0.8f, 1.0f, 0.6f)] [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.StringData)] [ReadOnly]
        public string resultStringValue;


        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.BooleanData)]
        public BooleanVariable boolValue;

        [GUIColor(0.8f, 1.0f, 0.6f)] [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.BooleanData)] [ReadOnly]
        public bool resultBoolValue;


        [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.IntData)]
        public IntegerVariable intValue;

        [GUIColor(0.8f, 1.0f, 0.6f)] [ShowIf(nameof(typeRemoteConfigData), TypeRemoteConfigData.IntData)] [ReadOnly]
        public int resultIntValue;


#if VIRTUESKY_FIREBASE_REMOTECONFIG
        public void SetUpData(ConfigValue result)
        {
            switch (typeRemoteConfigData)
            {
                case TypeRemoteConfigData.StringData:
                    if (result.Source == ValueSource.RemoteValue)
                    {
                        stringValue.Value = result.StringValue;
                    }

                    resultStringValue = stringValue.Value;
                    Debug.Log($"{key}: {resultStringValue}".SetColor(Color.green));
                    break;
                case TypeRemoteConfigData.BooleanData:
                    if (result.Source == ValueSource.RemoteValue)
                    {
                        boolValue.Value = result.BooleanValue;
                    }

                    resultBoolValue = boolValue.Value;
                    Debug.Log($"{key}: {resultBoolValue}".SetColor(Color.green));
                    break;
                case TypeRemoteConfigData.IntData:
                    if (result.Source == ValueSource.RemoteValue)
                    {
                        intValue.Value = int.Parse(result.StringValue);
                    }

                    resultIntValue = intValue.Value;
                    Debug.Log($"{key}: {resultIntValue}".SetColor(Color.green));
                    break;
            }
        }
#endif
    }

    public enum TypeRemoteConfigData
    {
        StringData,
        BooleanData,
        IntData
    }
}