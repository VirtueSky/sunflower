using UnityEngine.Video;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleVideoClipComponent : LocaleComponentGeneric<LocaleVideoClip, VideoClip>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<VideoPlayer>("video");
        }
    }
}