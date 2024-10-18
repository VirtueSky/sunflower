using System;
using UnityEngine;

namespace VirtueSky.Localization
{
    [Serializable]
    public class LocaleItem<T> : LocaleItemBase
    {
        [SerializeField] private T value;

        public T Value
        {
            get => value;
            set => this.value = value;
        }

        public override object ObjectValue
        {
            get => value;
            set => this.value = (T)value;
        }

        public LocaleItem()
        {
        }

        public LocaleItem(Language language, T value)
        {
            Language = language;
            Value = value;
        }
    }
}