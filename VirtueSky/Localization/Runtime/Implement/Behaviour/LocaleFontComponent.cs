using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleFontComponent : LocaleComponentGeneric<LocaleFont, Font>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<Text>("font");
            TrySetComponentAndPropertyIfNotSet<TextMesh>("font");
        }
    }
}