using System;
using System.Linq;
using UnityEngine;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [CreateAssetMenu(menuName = "Sunflower/Localization/AudioClip", fileName = "audioclip_localizevalue", order = 7)]
    [EditorIcon("scriptable_yellow_audioclip")]
    public class LocaleAudioClip : LocaleVariable<AudioClip>
    {
        [Serializable]
        private class AudioClipLocaleItem : LocaleItem<AudioClip>
        {
        };

        [SerializeField] private AudioClipLocaleItem[] items = new AudioClipLocaleItem[1];

        // ReSharper disable once CoVariantArrayConversion
        public override LocaleItemBase[] LocaleItems => items;
    }
}