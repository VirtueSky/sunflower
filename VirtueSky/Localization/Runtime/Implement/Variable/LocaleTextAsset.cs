using System;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Sunflower/Localization/TextAsset", fileName = "textasset_localizevalue", order = 5)]
    [EditorIcon("scriptable_yellow_textasset")]
    public class LocaleTextAsset : LocaleVariable<TextAsset>
    {
        [Serializable]
        private class TextAssetLocaleItem : LocaleItem<TextAsset>
        {
        };

        [SerializeField] private TextAssetLocaleItem[] items = new TextAssetLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;
    }
}