using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleTextureComponent : LocaleComponentGeneric<LocaleTexture, Texture>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<RawImage>("texture");
        }
    }
}