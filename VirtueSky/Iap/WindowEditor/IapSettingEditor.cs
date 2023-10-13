using UnityEditor;
using VirtueSky.Events;

namespace VirtueSky.Iap
{
#if UNITY_EDITOR
    [CustomEditor(typeof(IapSetting), true)]
    public class IapSettingEditor : UnityEditor.Editor
    {
    }
#endif
}