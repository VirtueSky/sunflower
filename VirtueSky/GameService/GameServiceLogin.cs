using System;
using System.Text;
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif
#if UNITY_ANDROID && VIRTUESKY_GPGS
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine;
using VirtueSky.Events;

namespace VirtueSky.GameService
{
    public class GameServiceLogin : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad;
        [SerializeField] private StringEvent loginAccountSuccess;
        [SerializeField] private EventNoParam loginAccountFail;

#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
        private IAppleAuthManager appleAuthManager;

#endif
        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        private void Start()
        {
            Init();
        }

        public void Init()
        {
#if UNITY_ANDROID && VIRTUESKY_GPGS
            PlayGamesPlatform.Activate();
#endif
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
                var deserializer = new PayloadDeserializer();
                // Creates an Apple Authentication manager with the deserializer
                this.appleAuthManager = new AppleAuthManager(deserializer);
            }
#endif
        }

        private void Update()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            // Updates the AppleAuthManager instance to execute
            // pending callbacks inside Unity's execution loop
            if (this.appleAuthManager != null)
            {
                this.appleAuthManager.Update();
            }
#endif
        }

        public void LoginGooglePlayGames()
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
                            loginAccountSuccess.Raise(code);
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
                                    loginAccountSuccess.Raise(code);
                                });
                        }
                        else
                        {
                            Debug.Log("Login Unsuccessful");
                            loginAccountFail.Raise();
                        }
                    });
                }
            });
#endif
        }

        public void LoginApple()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            var loginArgs =
                new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

            this.appleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    // Obtained credential, cast it to IAppleIDCredential
                    var appleIdCredential = credential as IAppleIDCredential;
                    if (appleIdCredential != null)
                    {
                        // Apple User ID
                        // You should save the user ID somewhere in the device
                        var userId = appleIdCredential.User;

                        // Email (Received ONLY in the first login)
                        var email = appleIdCredential.Email;

                        // Full name (Received ONLY in the first login)
                        var fullName = appleIdCredential.FullName;

                        // Identity token
                        var identityToken = Encoding.UTF8.GetString(
                            appleIdCredential.IdentityToken,
                            0,
                            appleIdCredential.IdentityToken.Length);

                        // Authorization code
                        var authorizationCode = Encoding.UTF8.GetString(
                            appleIdCredential.AuthorizationCode,
                            0,
                            appleIdCredential.AuthorizationCode.Length);

                        // And now you have all the information to create/login a user in your system
                        loginAccountSuccess.Raise(identityToken);
                    }
                    else
                    {
                        loginAccountFail.Raise();
                    }
                },
                error =>
                {
                    // Something went wrong
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    loginAccountFail.Raise();
                });
#endif
        }
    }
}