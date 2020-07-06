namespace PolicyService.Infrastructure
{
    using Microsoft.EntityFrameworkCore;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MicroservicesPOC.Shared;

    using PolicyService.Application.Common.Services;
    using PolicyService.Application.Common.Interfaces;
    
    using PolicyService.Infrastructure.Services;
    using PolicyService.Infrastructure.Messaging;
    using PolicyService.Infrastructure.Persistance;
    using PolicyService.Infrastructure.Persistance.Repositories;
    using MicroservicesPOC.Shared.Messaging;

    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMQConfigurations>(configuration.GetSection("RabbitMQ"));

            services
                .AddDbContext<PolicyDbContext>(options =>
                    options.UseNpgsql(configuration.GetConnectionString("PolicyServiceConnection"),
                    x => x.MigrationsAssembly(typeof(PolicyDbContext).Assembly.FullName)))
                .AddScoped<IOfferRepository, OfferRepository>()
                .AddScoped<IPolicyRepository, PolicyRepository>()
                .AddSingleton<IPricingClient, PricingClient>()
                .AddSingleton<IPricing, PricingService>()
                .AddSingleton<IEventPublisher, RabbitEventPublisher>();

            services.AddConventionalServices(typeof(ServiceRegistration).Assembly);

            return services;
        }
    }
}
