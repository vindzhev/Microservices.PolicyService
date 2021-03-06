﻿namespace PolicyService.API
{
    using MicroservicesPOC.Shared.Common;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceRegistration
    {
        public static IServiceCollection AddApiComponents(this IServiceCollection services) =>
            services.AddHttpContextAccessor()
                .AddConventionalServices(typeof(ServiceRegistration).Assembly);
    }
}
