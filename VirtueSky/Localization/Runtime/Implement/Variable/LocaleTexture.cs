using System;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Sunflower/Localization/Texture", fileName = "texture_localizevalue", order = 6)]
    [EditorIcon("scriptable_yellow_texture")]
    public class LocaleTexture : LocaleVariable<Texture>
    {
        [Serializable]
        private class TextureLocaleItem : LocaleItem<Texture>
        {
        };

        [SerializeField] private TextureLocaleItem[] items = new TextureLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;
    }
}