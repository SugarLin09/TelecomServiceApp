using Microsoft.Extensions.DependencyInjection;
using TelecomService.Application.Interfaces;
using TelecomService.Infrastructure.Data;
using TelecomService.Infrastructure.Repositories;

namespace TelecomService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<AppDbContext>()
            .AddScoped<ICustomerRepository, CustomerRepository>()
            .AddScoped<IPhoneNumberRepository, PhoneNumberRepository>();

            return services;
        }
    }
}
