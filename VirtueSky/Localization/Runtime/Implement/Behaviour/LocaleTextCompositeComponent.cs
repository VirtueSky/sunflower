using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VirtueSky.Inspector;

namespace VirtueSky.Localization
{
    [EditorIcon("icon_csharp")]
    public class LocaleTextCompositeComponent : LocaleComponentGenericBase
    {
        [SerializeField] private string seperate = ", ";
        [SerializeField] private LocaleText[] variables;
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

        public LocaleText[] Variables
        {
            get => variables;
            set
            {
                variables = value;
                ForceUpdate();
            }
        }

        public void UpdateArgs(params string[] args)
        {
            FormatArgs = args;
        }

        public void UpdateVariables(params LocaleText[] args)
        {
            Variables = args;
        }

        protected override Type GetValueType() => typeof(string);

        protected override bool HasLocaleValue() => variables is { Length: > 0 };

        protected override object GetLocaleValue()
        {
            (string value, int totalArgs) = CompositeString(seperate);
            if (FormatArgs.Length >= totalArgs && !string.IsNullOrEmpty(value))
            {
                return string.Format(value, FormatArgs.Cast<object>().ToArray());
            }

            return value;
        }

        private (string, int) CompositeString(string seperate = ", ")
        {
            var index = 0;
            var result = string.Empty;
            var stringBuilder = new StringBuilder();
            foreach (var text in variables)
            {
                string temp = GetValueOrDefault(text);
                const string pattern = @"{(.*?)}";
                int count = Regex.Matches(temp, pattern).OfType<Match>().Select(m => m.Value).Distinct().Count();
                int j = count - 1 + index;
                for (int i = count - 1; i >= 0; i--)
                {
                    stringBuilder.Clear();
                    stringBuilder.Append("{");
                    stringBuilder.Append(i);
                    var old = stringBuilder.ToString();
                    stringBuilder.Clear();
                    stringBuilder.Append("{");
                    stringBuilder.Append(j);
                    temp = temp.Replace(old, stringBuilder.ToString());
                    index++;
                    j--;
                }

                stringBuilder.Clear();
                if (!string.IsNullOrEmpty(result))
                {
                    stringBuilder.Append(result);
                    stringBuilder.Append(seperate);
                }

                stringBuilder.Append(temp);
                result = stringBuilder.ToString();
            }

            return (result, index);
        }

        private void Reset()
        {
            TrySetComponentAndPropertyIfNotSet<Text>("text");
            TrySetComponentAndPropertyIfNotSet<TextMeshProUGUI>("text");
        }
    }
}