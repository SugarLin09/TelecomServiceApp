using Microsoft.Extensions.DependencyInjection;
using TelecomService.Application.Interfaces;
using TelecomService.Application.Services;

namespace TelecomService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IPhoneNumberService, PhoneNumberService>();

            return services;
        }
    }
}
