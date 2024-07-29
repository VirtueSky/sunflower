using System.Text;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Extensions;
using AppleAuth.Interfaces;
using AppleAuth.Native;
#endif


namespace VirtueSky.GameService
{
    [EditorIcon("icon_authentication")]
    public class AppleAuthentication : ServiceAuthentication
    {
        [SerializeField] private StringVariable authorizationCodeVariable;
        [SerializeField] private StringVariable userIdVariable;

#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
        private IAppleAuthManager _iAppleAuthManager;
#endif


        protected override void Init()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            serverCode.Value = "";
            authorizationCodeVariable.Value = "";
            status.SetNotLoggedIn();
            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
                var deserializer = new PayloadDeserializer();
                // Creates an Apple Authentication manager with the deserializer
                this._iAppleAuthManager = new AppleAuthManager(deserializer);
                loginEvent.AddListener(Login);
            }
#endif
        }

        private void Update()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            // Updates the AppleAuthManager instance to execute
            // pending callbacks inside Unity's execution loop
            if (this._iAppleAuthManager != null)
            {
                this._iAppleAuthManager.Update();
            }
#endif
        }

        protected override void Login()
        {
#if UNITY_IOS && VIRTUESKY_APPLE_AUTH
            var loginArgs =
                new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

            this._iAppleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    // Obtained credential, cast it to IAppleIDCredential
                    if (credential is IAppleIDCredential appleIdCredential)
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
                        serverCode.Value = identityToken;
                        authorizationCodeVariable.Value = authorizationCode;
                        userIdVariable.Value = userId;
                        status.SetSuccessful();
                    }
                    else
                    {
                        status.SetFailed();
                    }
                },
                error =>
                {
                    // Something went wrong
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    status.SetFailed();
                });
#endif
        }
    }
}