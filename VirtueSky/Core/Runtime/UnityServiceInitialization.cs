using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace VirtueSky.Core
{
    public class UnityServiceInitialization : MonoBehaviour
    {
        private enum Environment
        {
            Production,
            Development,
        }

        [SerializeField] private Environment environment = Environment.Production;

        public static bool IsUnityServiceReady { get; private set; }

        private void Awake()
        {
            Init();
        }

        private async void Init()
        {
            IsUnityServiceReady = false;
            var options = new InitializationOptions();
            options.SetEnvironmentName(environment.ToString().ToLower());
            await UnityServices.InitializeAsync(options);
            IsUnityServiceReady = true;
        }
    }
}