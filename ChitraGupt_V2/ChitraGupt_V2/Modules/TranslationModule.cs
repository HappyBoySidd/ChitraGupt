
using ChitraGupt.API.Interfaces;
using TranslationAPI.Services;

namespace ChitraGupt_V2.Modules
{
    public static class TranslationModule
    {
        public static IServiceCollection AddTranslationModule(this IServiceCollection services)
        {
            services.AddSingleton<ITranslate, GoogleTranslatorService>();
            return services;
        }
    }
}
