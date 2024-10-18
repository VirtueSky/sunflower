using System;

namespace VirtueSky.Localization
{
    [Serializable]
    public class GoogleTranslateRequest
    {
        public Language source;
        public Language target;
        public string value;

        public GoogleTranslateRequest()
        {
        }

        public GoogleTranslateRequest(Language source, Language target, string value)
        {
            this.source = source;
            this.target = target;
            this.value = value;
        }
    }
}