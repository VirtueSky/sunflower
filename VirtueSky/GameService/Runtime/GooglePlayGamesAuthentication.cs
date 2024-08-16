#if UNITY_ANDROID && VIRTUESKY_GPGS
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

using System.Threading.Tasks;
using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Inspector;

namespace VirtueSky.GameService
{
    [EditorIcon("icon_authentication")]
    public class GooglePlayGamesAuthentication : ServiceAuthentication
    {
        [SerializeField] private EventNoParam getNewServerCodeEvent;

        protected override void Init()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            serverCode.Value = "";
            status.SetNotLoggedIn();
            PlayGamesPlatform.Activate();
            loginEvent.AddListener(Login);
            getNewServerCodeEvent.AddListener(OnGetNewServerCode);
#endif
        }
#if UNITY_ANDROID && VIRTUESKY_GPGS
        private async void OnGetNewServerCode()
        {
            if (!PlayGamesPlatform.Instance.IsAuthenticated()) return;
            (serverCode.Value, status.Value) = await GetNewServerCode();
        }

        private Task<(string, StatusLogin)> GetNewServerCode()
        {
            var taskSource = new TaskCompletionSource<(string, StatusLogin)>();

            PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                code => taskSource.SetResult((code, StatusLogin.Successful)));
            return taskSource.Task;
        }

        protected override void Login()
        {
            PlayGamesPlatform.Instance.Authenticate((success) =>
            {
                if (success == SignInStatus.Success)
                {
                    Debug.Log("Login with Google Play games successful.");
                    PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                        code =>
                        {
                            Debug.Log("Authorization code: " + code);
                            serverCode.Value = code;
                            nameVariable.Value = PlayGamesPlatform.Instance.GetUserDisplayName();
                            status.SetSuccessful();
                        });
                }
                else
                {
                    PlayGamesPlatform.Instance.ManuallyAuthenticate(success =>
                    {
                        if (success == SignInStatus.Success)
                        {
                            PlayGamesPlatform.Instance.RequestServerSideAccess(true,
                                code =>
                                {
                                    Debug.Log("Authorization code: " + code);
                                    serverCode.Value = code;
                                    nameVariable.Value = PlayGamesPlatform.Instance.GetUserDisplayName();
                                    status.SetSuccessful();
                                });
                        }
                        else
                        {
                            Debug.Log("Login Failed");
                            serverCode.Value = "";
                            status.SetFailed();
                        }
                    });
                }
            });
        }
#endif
        public static bool IsSignIn()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            return PlayGamesPlatform.Instance.IsAuthenticated();

#else
            return false;
#endif
        }
    }
}