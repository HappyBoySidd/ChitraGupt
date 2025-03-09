namespace ChitraGupt.API.Interfaces
{
    public interface ITranslate
    {
        Task<string> TranslateText(string text, string targetLanguage = "en");
    }
}
