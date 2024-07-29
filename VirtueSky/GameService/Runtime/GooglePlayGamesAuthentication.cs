#if UNITY_ANDROID && VIRTUESKY_GPGS
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif

using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.GameService
{
    [EditorIcon("icon_authentication")]
    public class GooglePlayGamesAuthentication : ServiceAuthentication
    {
        protected override void Init()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            serverCode.Value = "";
            status.SetNotLoggedIn();
            PlayGamesPlatform.Activate();
            loginEvent.AddListener(Login);
#endif
        }

        protected override void Login()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
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
                                    status.SetSuccessful();
                                });
                        }
                        else
                        {
                            Debug.Log("Login Unsuccessful");
                            serverCode.Value = "";
                            status.SetFailed();
                        }
                    });
                }
            });
#endif
        }

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