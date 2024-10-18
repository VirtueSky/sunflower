using TMPro;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    using UnityEngine;

    [EditorIcon("icon_csharp")]
    public class LocaleMaterialComponent : LocaleComponentGeneric<LocaleMaterial, Material>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<Renderer>("material");
            TrySetComponentAndPropertyIfNotSet<TMP_Text>("fontMaterial");
        }
    }
}