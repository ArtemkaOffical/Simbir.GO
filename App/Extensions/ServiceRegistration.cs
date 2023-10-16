using Microsoft.Extensions.DependencyInjection.Extensions;
using Simbir.GO.App.MiddleWare;
using Simbir.GO.App.Services;

namespace Simbir.GO.App.Extensions
{
    public static class ServiceRegistration
    {
        public static void RegisterCustomServices(this IServiceCollection services)
        {
            services.AddScoped<AccountService>();
            services.AddScoped<TransportService>();
            services.AddScoped<PaymentService>();
            services.AddScoped<RentService>();
            services.AddScoped<JWTService>();
        }
    }
}
