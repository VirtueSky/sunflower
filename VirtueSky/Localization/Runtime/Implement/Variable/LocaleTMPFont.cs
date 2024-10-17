using System;
using TMPro;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Pancake/Localization/TMPFont", fileName = "font_asset_localizevalue", order = 1)]
    [EditorIcon("scriptable_yellow_fontasset")]
    public class LocaleTMPFont : LocaleVariable<TMP_FontAsset>
    {
        [Serializable]
        private class TMPFontLocaleItem : LocaleItem<TMP_FontAsset>
        {
        };

        [SerializeField] private TMPFontLocaleItem[] items = new TMPFontLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;
    }
}