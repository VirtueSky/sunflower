using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Variables;

namespace VirtueSky.GameService
{
    [CreateAssetMenu(menuName = "Sunflower/Scriptable/Game Service/Status Login Variable",
        fileName = "status_login_variables")]
    [EditorIcon("scriptable_variable")]
    public class StatusLoginVariable : BaseVariable<StatusLogin>
    {
        public void SetNotLoggedIn()
        {
            Value = StatusLogin.NotLoggedIn;
        }

        public void SetSuccessful()
        {
            Value = StatusLogin.Successful;
        }

        public void SetFailed()
        {
            Value = StatusLogin.Failed;
        }
    }

    public enum StatusLogin
    {
        NotLoggedIn,
        Successful,
        Failed
    }
}