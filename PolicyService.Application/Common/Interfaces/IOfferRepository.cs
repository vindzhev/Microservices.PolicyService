namespace PolicyService.Application.Common.Interfaces
{
    using System.Threading.Tasks;

    using PolicyService.Domain.Entities;

    public interface IOfferRepository
    {
        void Add(Offer offer);

        Task<Offer> WithNumber(string number);

        Task SaveChangesAsync();
    }
}
