using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using VirtueSky.Localization;
using UnityEditor;
using UnityEngine;

namespace VirtueSky.LocalizationEditor
{
    public class CsvSerialization
    {
        private const string KEY_COLUMN = "Key";

        public void Serialize(Stream stream)
        {
            var languages = LocaleSettings.AvailableLanguages;
            var localizedTexts = Locale.FindAllLocalizedAssets<LocaleText>();

            using (var writer = new StreamWriter(stream))
            {
                // Write key column.
                writer.Write(DoubleQuote(KEY_COLUMN));

                if (languages.Count > 0) writer.Write(",");

                // Write used language columns.
                for (var i = 0; i < languages.Count; i++)
                {
                    writer.Write(DoubleQuote("{0} ({1})"), languages[i].Code, languages[i].Name);

                    if (i != languages.Count - 1) writer.Write(",");
                }

                writer.WriteLine();

                // Write localized assets.
                foreach (var localizedText in localizedTexts)
                {
                    // Write key.
                    writer.Write(DoubleQuote(localizedText.name));

                    if (languages.Count > 0) writer.Write(",");

                    for (var i = 0; i < languages.Count; i++)
                    {
                        if (!localizedText.TryGetLocaleValue(languages[i], out string value)) value = "";

                        writer.Write(DoubleQuote(value));

                        if (i != languages.Count - 1) writer.Write(",");
                    }

                    writer.WriteLine();
                }
            }
        }

        public void Deserialize(Stream stream)
        {
            string importLocation = LocaleSettings.ImportLocation;
            var localizedTexts = Locale.FindAllLocalizedAssets<LocaleText>();

            using (var reader = new StreamReader(stream))
            {
                var languages = ReadImportLanguages(reader);

                while (!reader.EndOfStream)
                {
                    string[] tokens = ReadNextTokens(reader);

                    if (tokens.Length != languages.Count + 1) throw new IOException("Invalid row");

                    string key = tokens[0];
                    if (string.IsNullOrEmpty(key)) throw new IOException("Key field must not be empty");

                    var localizedText = localizedTexts.FirstOrDefault(x => x.name == key);
                    if (localizedText == null)
                    {
                        localizedText = ScriptableObject.CreateInstance<LocaleText>();

                        string assetPath = Path.Combine(importLocation, $"{key}.asset");
                        AssetDatabase.CreateAsset(localizedText, assetPath);
                        AssetDatabase.SaveAssets();
                    }

                    // Read languages by ignoring first column (Key).
                    for (var i = 1; i < tokens.Length; i++)
                    {
                        ScriptableLocaleEditor.AddOrUpdateLocale(localizedText, languages[i - 1], tokens[i]);
                    }

                    EditorUtility.SetDirty(localizedText);
                }
            }

            AssetDatabase.Refresh();
        }

        private string[] ReadNextTokens(StreamReader reader)
        {
            var line = "";

            do
            {
                if (line.Length != 0) line += "\n";

                line += reader.ReadLine();
            } while (!reader.EndOfStream && line.Length > 0 && line[^1] != '\"');

            if (line != null)
            {
                string[] tokens = Regex.Split(line, @""",""");
                if (tokens.Length > 0)
                {
                    string token = tokens[0];

                    if (token.Length > 0 && token[0] == '\"') token = token.Remove(0, 1);

                    tokens[0] = token;
                }

                if (tokens.Length > 1)
                {
                    string token = tokens[^1];

                    if (token.Length > 0 && token[^1] == '\"') token = token.Remove(token.Length - 1, 1);

                    tokens[^1] = token;
                }

                return tokens;
            }

            return Array.Empty<string>();
        }

        private List<Language> ReadImportLanguages(StreamReader reader)
        {
            var availableLanguages = LocaleSettings.AllLanguages;
            var importLanguages = new List<Language>();

            string[] columnTokens = ReadNextTokens(reader);
            if (columnTokens.Length == 0)
            {
                throw new IOException("Column size must be greater than zero");
            }

            // Read languages by ignoring first column (Key).
            for (var i = 1; i < columnTokens.Length; i++)
            {
                string token = columnTokens[i].Trim();
                if (token.Length == 0) throw new IOException("Invalid language code column");

                // Read only language code.
                // Emit language name if exist.
                string[] tokens = token.Split(' ');
                if (tokens.Length > 0) token = tokens[0].Trim();

                var language = availableLanguages.FirstOrDefault(x => x.Code == token);
                if (language == null)
                {
                    Debug.LogWarning("Language code (" + token + ") not exist in localization system.");
                }

                // Add null language as well to maintain order.
                importLanguages.Add(language);
            }

            return importLanguages;
        }

        private static string DoubleQuote(string s)
        {
            return $"\"{s}\"";
        }
    }
}