using System;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Sunflower/Localization/Font", fileName = "font_localizevalue", order = 1)]
    [EditorIcon("scriptable_yellow_font")]
    public class LocaleFont : LocaleVariable<Font>
    {
        [Serializable]
        private class FontLocaleItem : LocaleItem<Font>
        {
        };

        [SerializeField] private FontLocaleItem[] items = new FontLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;
    }
}