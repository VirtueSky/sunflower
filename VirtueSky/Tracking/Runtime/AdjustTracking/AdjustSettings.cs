#if VIRTUESKY_ADJUST
using com.adjust.sdk;
#endif
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Tracking
{
    [EditorIcon("icon_scriptable")]
    public class AdjustSettings : ScriptableObject
    {
        [SerializeField] private string appToken;
#if VIRTUESKY_ADJUST
        [SerializeField] private AdjustEnvironment adjustEnvironment = AdjustEnvironment.Production;
        [SerializeField] private AdjustLogLevel logLevel = AdjustLogLevel.Error;
#endif
        public string AppToken => appToken;
#if VIRTUESKY_ADJUST
        public AdjustEnvironment AdjustEnvironment => adjustEnvironment;
        public AdjustLogLevel LogLevel => logLevel;
#endif
    }
}