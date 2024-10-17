using TMPro;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleTMPFontComponent : LocaleComponentGeneric<LocaleTMPFont, TMP_FontAsset>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<TextMeshProUGUI>("font");
            TrySetComponentAndPropertyIfNotSet<TextMeshPro>("font");
        }
    }
}