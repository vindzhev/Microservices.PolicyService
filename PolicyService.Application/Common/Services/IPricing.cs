namespace PolicyService.Application.Common.Services
{
    using System.Threading.Tasks;

    using PolicyService.Domain.Entities;
    using PolicyService.Application.Common.Models;

    using MicroservicesPOC.Shared.Common.Services;

    public interface IPricing
    {
        Task<Price> CalculatePrice(PricingParameters parameters);
    }
}
