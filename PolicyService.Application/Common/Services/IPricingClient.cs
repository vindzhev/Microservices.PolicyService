namespace PolicyService.Application.Common.Services
{
    using System.Threading.Tasks;

    using RestEase;

    using MicroservicesPOC.Shared.Common.Models;

    public interface IPricingClient
    {
        [Post]
        Task<CalculatePriceResult> CalculatePrice([Body]CalculationData data);
    }
}
