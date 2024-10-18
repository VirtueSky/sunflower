using System;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Sunflower/Localization/Sprite", fileName = "sprite_localizevalue", order = 3)]
    [EditorIcon("scriptable_yellow_sprite")]
    public class LocaleSprite : LocaleVariable<Sprite>
    {
        [Serializable]
        private class SpriteLocaleItem : LocaleItem<Sprite>
        {
        };

        [SerializeField] private SpriteLocaleItem[] items = new SpriteLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items.ToArray<LocaleItemBase>();
    }
}