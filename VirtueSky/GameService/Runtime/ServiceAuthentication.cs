using UnityEngine;
using VirtueSky.Events;
using VirtueSky.Variables;
#if UNITY_EDITOR
using VirtueSky.UtilsEditor;
#endif

namespace VirtueSky.GameService
{
    public abstract class ServiceAuthentication : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad;
        [SerializeField] protected StatusLoginVariable status;
        [SerializeField] protected StringVariable serverCode;
        [SerializeField] protected StringVariable nameVariable;
        [SerializeField] protected EventNoParam loginEvent;

        protected virtual void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            Init();
        }

        protected abstract void Init();
        protected abstract void Login();
#if UNITY_EDITOR
        private void Reset()
        {
            status = CreateAsset.CreateAndGetScriptableAsset<StatusLoginVariable>("/GameService",
                "status_login_variables");
        }
#endif
    }
}