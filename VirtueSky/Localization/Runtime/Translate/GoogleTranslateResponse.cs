using System;

namespace VirtueSky.Localization
{
    [Serializable]
    public class GoogleTranslateResponse
    {
        public string translatedText;
        public string detectedSourceLanguage;

        public GoogleTranslateResponse()
        {
        }

        public GoogleTranslateResponse(string translatedText)
        {
            this.translatedText = translatedText;
        }
    }
}