using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;
using VirtueSky.Linq;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Pancake/Localization/Text", fileName = "text_localizevalue", order = 4)]
    [EditorIcon("scriptable_yellow_text")]
    [Serializable]
    public class LocaleText : LocaleVariable<string>
    {
        [Serializable]
        private class TextLocaleItem : LocaleItem<string>
        {
        };

        [SerializeField] private TextLocaleItem[] items = new TextLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;

        /// <summary>
        /// Sets locale items in Editor or Playmode.
        /// </summary>
        public void SetLocaleItems(List<LocaleItem<string>> items)
        {
            if (items == null) throw new ArgumentNullException(nameof(items));

            this.items = items.Map(i => new TextLocaleItem { Language = i.Language, Value = i.Value }).ToArray();
        }

        /// <summary>
        /// Sets locale items in Editor or Playmode.
        /// </summary>
        public void SetLocaleItems(LocaleItem<string>[] items)
        {
            if (items == null) throw new ArgumentException(nameof(items));

            this.items = items.Map(i => new TextLocaleItem { Language = i.Language, Value = i.Value }).ToArray();
        }
    }
}