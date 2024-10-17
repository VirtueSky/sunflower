using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace VirtueSky.Localization
{
    public sealed class GoogleTranslator
    {
        private const string REQUEST_URL_V2 = "https://translation.googleapis.com/language/translate/v2?key={0}";
        private const string REQUEST_KEY_INPUT_TEXT = "q";
        private const string REQUEST_KEY_SOURCE_LANGUAGE = "source";
        private const string REQUEST_KEY_TARGET_LANGUAGE = "target";

        /// <summary>
        /// Gets or sets the google cloud API key.
        /// </summary>
        /// <seealso cref="http://cloud.google.com/docs/authentication/api-keys"/>
        public string AuthCredential { get; set; }

        public GoogleTranslator(string authCredential)
        {
            AuthCredential = authCredential;
        }

        /// <summary>
        /// Performs translation with given translate request asynchronous.
        /// </summary>
        /// <param name="request">Translate request.</param>
        /// <param name="onCompleted">Completed action.</param>
        /// <param name="onError">Error action.</param>
        public IEnumerator TranslateAsync(
            GoogleTranslateRequest request,
            Action<TranslationCompletedEventArgs> onCompleted = null,
            Action<TranslationErrorEventArgs> onError = null)
        {
            using (var www = PrepareRequest(request))
            {
#if UNITY_2017_2_OR_NEWER
                yield return www.SendWebRequest();
#else
                yield return www.Send();
#endif
                ProcessResponse(request, www, onCompleted, onError);
            }
        }

        /// <summary>
        /// Useful for Editor scripts. Otherwise, recommended to use <see cref="TranslateAsync"/>.
        /// </summary>
        /// <param name="request">Translate request.</param>
        /// <param name="onCompleted">Completed action.</param>
        /// <param name="onError">Error action.</param>
        public void Translate(GoogleTranslateRequest request, Action<TranslationCompletedEventArgs> onCompleted = null, Action<TranslationErrorEventArgs> onError = null)
        {
            using (var www = PrepareRequest(request))
            {
#if UNITY_2017_2_OR_NEWER
                www.SendWebRequest();
#else
                www.Send();
#endif

                // Wait request completion.
                while (!www.isDone)
                {
                }

                ProcessResponse(request, www, onCompleted, onError);
            }
        }

        private UnityWebRequest PrepareRequest(GoogleTranslateRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrEmpty(AuthCredential))
            {
                // ReSharper disable once NotResolvedInText
                throw new ArgumentNullException("AuthCredential", "Auth credential missing!");
            }

            var form = new WWWForm();
            form.AddField(REQUEST_KEY_INPUT_TEXT, request.value);
            form.AddField(REQUEST_KEY_SOURCE_LANGUAGE, request.source.Code);
            form.AddField(REQUEST_KEY_TARGET_LANGUAGE, request.target.Code);

            var url = string.Format(REQUEST_URL_V2, AuthCredential);
            return UnityWebRequest.Post(url, form);
        }

        private void ProcessResponse(
            GoogleTranslateRequest request,
            UnityWebRequest www,
            Action<TranslationCompletedEventArgs> onCompleted,
            Action<TranslationErrorEventArgs> onError)
        {
            if (www.result is UnityWebRequest.Result.ConnectionError or UnityWebRequest.Result.ProtocolError)
            {
                onError?.Invoke(new TranslationErrorEventArgs(www.error, www.responseCode));
            }
            else
            {
                var response = JsonUtility.FromJson<JsonResponse>(www.downloadHandler.text);
                if (response is { data: { translations: { Length: > 0 } } })
                {
                    var requests = new[] { request };

                    var translateResponse = new GoogleTranslateResponse { translatedText = response.data.translations[0].translatedText };
                    var responses = new[] { translateResponse };

                    onCompleted?.Invoke(new TranslationCompletedEventArgs(requests, responses));
                }
                else
                {
                    if (response != null && response.error != null)
                    {
                        onError?.Invoke(new TranslationErrorEventArgs(response.error.message, response.error.code));
                    }

                    onError?.Invoke(new TranslationErrorEventArgs("Response data could not be read.", -1));
                }
            }
        }

        [Serializable]
        private class JsonResponse
        {
            public JsonData data = null;
            public JsonError error = null;
        }

        [Serializable]
        private class JsonData
        {
            public JsonTranslation[] translations = null;
        }

        [Serializable]
        private class JsonError
        {
            public int code = 0;
            public string message = null;
        }

        [Serializable]
        private class JsonTranslation
        {
            public string translatedText = "";
            public string detectedSourceLanguage = "";
        }
    }

    /// <summary>
    /// Provides the requests and translation responses.
    /// </summary>
    public class TranslationCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Translate requests.
        /// </summary>
        public GoogleTranslateRequest[] Requests { get; private set; }

        /// <summary>
        /// Translate responses.
        /// </summary>
        public GoogleTranslateResponse[] Responses { get; private set; }

        public TranslationCompletedEventArgs(GoogleTranslateRequest[] requests, GoogleTranslateResponse[] responses)
        {
            Debug.Assert(requests != null);
            Debug.Assert(responses != null);
            Requests = requests;
            Responses = responses;
        }
    }

    /// <summary>
    /// Provides detailed information upon translation errors.
    /// </summary>
    public class TranslationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Error code.
        /// </summary>
        public long ResponseCode { get; private set; }

        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; private set; }

        public TranslationErrorEventArgs(string message, long responseCode)
        {
            ResponseCode = responseCode;
            Message = message;
        }
    }
}