

using ChitraGupt_V2.Features.Prediction.Interface;
using ChitraGupt_V2.Features.Prediction.Services;

namespace ChitraGupt_V2.Modules
{
    public static class PredictionModule
    {
        public static IServiceCollection AddPredictionModule(this IServiceCollection services)
        {
            services.AddSingleton<IPredictionService, PredictionService>();
            return services;
        }
    }
}