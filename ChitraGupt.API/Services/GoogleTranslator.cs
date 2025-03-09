using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using ChitraGupt.API.Interfaces;

namespace TranslationAPI.Services
{
    public class GoogleTranslatorService : ITranslate
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<string> TranslateText(string text, string targetLanguage = "en")
        {
            if (string.IsNullOrWhiteSpace(text)) return text; // Skip empty values

            try
            {
                string encodedText = HttpUtility.UrlEncode(text);
                string url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl=auto&tl={targetLanguage}&dt=t&q={encodedText}";

                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                return ExtractTranslatedText(jsonResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Translation failed: {ex.Message}");
                return text; // Return original text if translation fails
            }
        }

        private string ExtractTranslatedText(string jsonResponse)
        {
            try
            {
                var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(jsonResponse);
                if (parsed == null)
                    return string.Empty;
                return parsed[0][0][0].ToString();
            }
            catch
            {
                return jsonResponse; // Fallback to raw response
            }
        }
    }
}
