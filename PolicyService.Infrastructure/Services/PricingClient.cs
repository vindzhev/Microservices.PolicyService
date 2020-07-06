namespace PolicyService.Infrastructure.Services
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    
    using Polly;
    using Polly.Retry;

    using RestEase;
    
    using Steeltoe.Common.Discovery;
    
    using Microsoft.Extensions.Configuration;
    
    using MicroservicesPOC.Shared.Common.Models;
    
    using PolicyService.Application.Common.Services;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public class PricingClient : IPricingClient
    {
        private readonly IPricingClient _client;
        private static readonly AsyncRetryPolicy _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

        public PricingClient(IConfiguration configuration, IDiscoveryClient discoveryClient)
        {
            DiscoveryHttpClientHandler handler = new DiscoveryHttpClientHandler(discoveryClient);
            HttpClient httpClient = new HttpClient(handler, false) { BaseAddress = new Uri(configuration.GetValue<string>("PricingServiceUri")) };
            JsonSerializerSettings settings = new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            this._client = new RestClient(httpClient) { JsonSerializerSettings = settings }.For<IPricingClient>();
        }

        public Task<CalculatePriceResult> CalculatePrice([Body] CalculationData data) => 
            _retryPolicy.ExecuteAsync(async () => await this._client.CalculatePrice(data));
    }
}
