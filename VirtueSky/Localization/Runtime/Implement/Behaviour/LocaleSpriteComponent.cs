using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleSpriteComponent : LocaleComponentGeneric<LocaleSprite, Sprite>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<Image>("sprite");
        }
    }
}