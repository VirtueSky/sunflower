using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleTextComponent : LocaleComponentGeneric<LocaleText, string>
    {
        [SerializeField] private string[] formatArgs = Array.Empty<string>();

        public string[] FormatArgs
        {
            get => formatArgs;
            set
            {
                formatArgs = value ?? Array.Empty<string>();
                ForceUpdate();
            }
        }

        public void UpdateArgs(params string[] args)
        {
            FormatArgs = args;
        }

        protected override object GetLocaleValue()
        {
            var value = (string)base.GetLocaleValue();
            if (FormatArgs.Length > 0 && !string.IsNullOrEmpty(value))
            {
                return string.Format(value, FormatArgs.Cast<object>().ToArray());
            }

            return value;
        }

        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<Text>("text");
            TrySetComponentAndPropertyIfNotSet<TextMeshProUGUI>("text");
        }
    }
}