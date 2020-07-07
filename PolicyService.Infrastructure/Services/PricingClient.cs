namespace PolicyService.Infrastructure.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    
    using Polly;
    using Polly.Retry;

    using RestEase;
    
    using Microsoft.Extensions.Configuration;
    
    using MicroservicesPOC.Shared.Common.Models;
    
    using PolicyService.Application.Common.Services;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    using System.Linq;

    public class PricingClient : IPricingClient
    {
        private readonly IPricingClient _client;
        private static readonly AsyncRetryPolicy _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

        public PricingClient(IConfiguration configuration, Consul.IConsulClient consulClient)
        {
            //TODO: find better way to resolve hostname in http handler
            string baseUrl = GetServiceUrl(consulClient, "PricingService");
            string pricingServiceUrl = configuration.GetValue<string>("PricingServiceUri").Replace("PricingService", baseUrl);

            HttpClient httpClient = new HttpClient() { BaseAddress = new Uri(pricingServiceUrl) };
            JsonSerializerSettings settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            this._client = new RestClient(httpClient) { JsonSerializerSettings = settings }.For<IPricingClient>();
        }

        private string GetServiceUrl(Consul.IConsulClient consulClient, string requestedServiceName)
        {
            var services = consulClient.Agent.Services().Result;
            Consul.AgentService service = services.Response.Values.FirstOrDefault(x => x.Service == requestedServiceName);

            return (service != null) ? 
                $"{service.Address}:{service.Port}" : throw new ArgumentNullException(nameof(service));
        }

        public Task<CalculatePriceResult> CalculatePrice([Body] CalculationData data) => 
            _retryPolicy.ExecuteAsync(async () => await this._client.CalculatePrice(data));
    }
}
