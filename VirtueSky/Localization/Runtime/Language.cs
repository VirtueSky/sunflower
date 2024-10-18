using System;
using UnityEngine;

namespace VirtueSky.Localization
{
    [Serializable]
    public class Language : IEquatable<Language>
    {
        public static Language[] BuiltInLanguages
        {
            get
            {
                return new[]
                {
                    Afrikaans, Arabic, Basque, Belarusian, Bulgarian, Catalan, Chinese, Czech, Danish, Dutch, English, Estonian, Faroese, Finnish, French, German,
                    Greek, Hebrew, Hungarian, Icelandic, Indonesian, Italian, Japanese, Korean, Latvian, Lithuanian, Norwegian, Polish, Portuguese, Romanian, Russian,
                    SerboCroatian, Slovak, Slovenian, Spanish, Swedish, Thai, Turkish, Ukrainian, Vietnamese
                };
            }
        }

        public static Language Afrikaans => new(SystemLanguage.Afrikaans.ToString(), "af");
        public static Language Arabic => new(SystemLanguage.Arabic.ToString(), "ar");
        public static Language Basque => new(SystemLanguage.Basque.ToString(), "eu");
        public static Language Belarusian => new(SystemLanguage.Belarusian.ToString(), "be");
        public static Language Bulgarian => new(SystemLanguage.Bulgarian.ToString(), "bg");
        public static Language Catalan => new(SystemLanguage.Catalan.ToString(), "ca");
        public static Language Chinese => new(SystemLanguage.Chinese.ToString(), "zh");
        public static Language Czech => new(SystemLanguage.Czech.ToString(), "cs");
        public static Language Danish => new(SystemLanguage.Danish.ToString(), "da");
        public static Language Dutch => new(SystemLanguage.Dutch.ToString(), "nl");
        public static Language English => new(SystemLanguage.English.ToString(), "en");
        public static Language Estonian => new(SystemLanguage.Estonian.ToString(), "et");
        public static Language Faroese => new(SystemLanguage.Faroese.ToString(), "fo");
        public static Language Finnish => new(SystemLanguage.Finnish.ToString(), "fi");
        public static Language French => new(SystemLanguage.French.ToString(), "fr");
        public static Language German => new(SystemLanguage.German.ToString(), "de");
        public static Language Greek => new(SystemLanguage.Greek.ToString(), "el");
        public static Language Hebrew => new(SystemLanguage.Hebrew.ToString(), "he");
        public static Language Hungarian => new(SystemLanguage.Hungarian.ToString(), "hu");
        public static Language Icelandic => new(SystemLanguage.Icelandic.ToString(), "is");
        public static Language Indonesian => new(SystemLanguage.Indonesian.ToString(), "id");
        public static Language Italian => new(SystemLanguage.Italian.ToString(), "it");
        public static Language Japanese => new(SystemLanguage.Japanese.ToString(), "ja");
        public static Language Korean => new(SystemLanguage.Korean.ToString(), "ko");
        public static Language Latvian => new(SystemLanguage.Latvian.ToString(), "lv");
        public static Language Lithuanian => new(SystemLanguage.Lithuanian.ToString(), "lt");
        public static Language Norwegian => new(SystemLanguage.Norwegian.ToString(), "no");
        public static Language Polish => new(SystemLanguage.Polish.ToString(), "pl");
        public static Language Portuguese => new(SystemLanguage.Portuguese.ToString(), "pt");
        public static Language Romanian => new(SystemLanguage.Romanian.ToString(), "ro");
        public static Language Russian => new(SystemLanguage.Russian.ToString(), "ru");
        public static Language SerboCroatian => new(SystemLanguage.SerboCroatian.ToString(), "hr");
        public static Language Slovak => new(SystemLanguage.Slovak.ToString(), "sk");
        public static Language Slovenian => new(SystemLanguage.Slovenian.ToString(), "sl");
        public static Language Spanish => new(SystemLanguage.Spanish.ToString(), "es");
        public static Language Swedish => new(SystemLanguage.Swedish.ToString(), "sv");
        public static Language Thai => new(SystemLanguage.Thai.ToString(), "th");
        public static Language Turkish => new(SystemLanguage.Turkish.ToString(), "tr");
        public static Language Ukrainian => new(SystemLanguage.Ukrainian.ToString(), "uk");
        public static Language Vietnamese => new(SystemLanguage.Vietnamese.ToString(), "vi");

        [SerializeField] private string name;
        [SerializeField] private string code;
        [SerializeField] private bool custom;

        public string Name => name;
        public string Code => code;
        public bool Custom => custom;

        public Language(string name, string code, bool custom = false)
        {
            this.name = name ?? "";
            this.code = code ?? "";
            this.custom = custom;
        }

        public bool Equals(Language other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Code == other.Code;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Language)obj);
        }

        public override int GetHashCode()
        {
            return Code.GetHashCode();
        }

        public static bool operator ==(Language left, Language right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Language left, Language right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Name;
        }

        public static implicit operator Language(SystemLanguage systemLanguage)
        {
            int index = Array.FindIndex(BuiltInLanguages, x => x.name == systemLanguage.ToString());
            return index >= 0 ? BuiltInLanguages[index] : English;
        }

        public static explicit operator SystemLanguage(Language language)
        {
            if (language.custom) return SystemLanguage.Unknown;

            var systemLanguages = (SystemLanguage[])Enum.GetValues(typeof(SystemLanguage));
            int index = Array.FindIndex(systemLanguages, x => x.ToString() == language.Name);
            return index >= 0 ? systemLanguages[index] : SystemLanguage.Unknown;
        }
    }
}