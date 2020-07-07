namespace PolicyService.API
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json.Serialization;

    using PolicyService.Application;
    using PolicyService.Infrastructure;
    using PolicyService.API.Extensions;

    using MicroservicesPOC.Shared.Extensions;
    using MicroservicesPOC.Shared.API.Extensions;

    public class Startup
    {
        public Startup(IConfiguration configuration) => this.Configuration = configuration;

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddConsulConfig(Configuration);

            services
                .AddApplication()
                .AddInfrastructure(this.Configuration)
                .AddApiComponents();

            services.AddHealthChecks();

            services
                .AddControllers(setupAction => setupAction.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson(setupAction =>
                {
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    setupAction.SerializerSettings.Converters.Add(new QuestionAnswerConverter());
                })
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(setupAction => setupAction.UseCustomInvalidModelUnprocessableEntityResponse());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else app.UseHsts();

            //app.UseCustomExceptionHandler();
            app.UseHealthChecks("/health");
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseConsul(Configuration);

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
