using TMPro;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    using UnityEngine;

    [EditorIcon("icon_csharp")]
    public class LocaleSharedMaterialComponent : LocaleComponentGeneric<LocaleMaterial, Material>
    {
        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<Renderer>("sharedMaterial");
            TrySetComponentAndPropertyIfNotSet<TMP_Text>("fontSharedMaterial");
        }
    }
}