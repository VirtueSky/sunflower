using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#if VIRTUESKY_FIREBASE
using Firebase;
using Firebase.Extensions;
#endif
#if VIRTUESKY_FIREBASE_REMOTECONFIG
using Firebase.RemoteConfig;
#endif

using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace VirtueSky.RemoteConfigs
{
    [EditorIcon("icon_controller"), HideMonoScript]
    public class FirebaseRemoteConfigManager : MonoBehaviour
    {
        [SerializeField] private TypeInitRemoteConfig typeInitRemoteConfig;
#if VIRTUESKY_FIREBASE
        [Space, ReadOnly, SerializeField] private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif

        [Space, SerializeField] private BooleanVariable isFetchRemoteConfigCompleted;
        [Space, SerializeField] private List<FirebaseRemoteConfigData> listRemoteConfigData;

        private void Awake()
        {
            if (typeInitRemoteConfig == TypeInitRemoteConfig.InitOnAwake)
            {
                InitRemoteConfig();
            }
        }

        private void Start()
        {
            if (typeInitRemoteConfig == TypeInitRemoteConfig.InitOnStart)
            {
                InitRemoteConfig();
            }
        }

        private void InitRemoteConfig()
        {
#if VIRTUESKY_FIREBASE
            if (isFetchRemoteConfigCompleted != null)
            {
                isFetchRemoteConfigCompleted.Value = false;
            }

            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
#if VIRTUESKY_FIREBASE_REMOTECONFIG
                    FetchDataAsync();
#endif
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " +
                                   dependencyStatus);
                }
            });
#endif
        }

#if VIRTUESKY_FIREBASE_REMOTECONFIG && VIRTUESKY_FIREBASE
        public Task FetchDataAsync()
        {
            Debug.Log("Fetching data...");
            Task fetchTask = FirebaseRemoteConfig.DefaultInstance
                .FetchAsync(TimeSpan.Zero);
            if (fetchTask.IsCanceled)
            {
                Debug.Log("Fetch canceled.");
            }
            else if (fetchTask.IsFaulted)
            {
                Debug.Log("Fetch encountered an error.");
            }
            else if (fetchTask.IsCompleted)
            {
                Debug.Log("Fetch completed successfully!");
            }

            return fetchTask.ContinueWithOnMainThread(tast =>
            {
                var info = FirebaseRemoteConfig.DefaultInstance.Info;
                if (info.LastFetchStatus == LastFetchStatus.Success)
                {
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync().ContinueWithOnMainThread(
                        task =>
                        {
                            Debug.Log(String.Format(
                                "Remote data loaded and ready (last fetch time {0}).",
                                info.FetchTime));
                            foreach (var rmcData in listRemoteConfigData)
                            {
                                if (string.IsNullOrEmpty(rmcData.key) ||
                                    (rmcData.typeRemoteConfigData == TypeRemoteConfigData.BooleanData &&
                                     rmcData.boolValue == null) ||
                                    (rmcData.typeRemoteConfigData == TypeRemoteConfigData.StringData &&
                                     rmcData.stringValue == null) ||
                                    (rmcData.typeRemoteConfigData == TypeRemoteConfigData.IntData &&
                                     rmcData.intValue == null)) continue;

                                rmcData.SetUpData(FirebaseRemoteConfig.DefaultInstance
                                    .GetValue(rmcData.key));
                            }

                            if (isFetchRemoteConfigCompleted != null)
                            {
                                isFetchRemoteConfigCompleted.Value = true;
                            }
                        });

                    Debug.Log("<color=Green>Firebase Remote Config Fetching completed!</color>");
                }
                else
                {
                    Debug.Log("Fetching data did not completed!");
                }
            });
        }
#endif
    }

    enum TypeInitRemoteConfig
    {
        InitOnAwake,
        InitOnStart
    }
}