using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleTextureMaterialComponent : LocaleComponent
    {
        public Material material;
        public string propertyName = "_MainTex";
        public LocaleTexture localeTexture;

        protected override bool TryUpdateComponentLocalization(bool isOnValidate)
        {
            if (material != null && localeTexture != null)
            {
                material.SetTexture(propertyName, GetValueOrDefault(localeTexture));
                return true;
            }

            return false;
        }
    }
}