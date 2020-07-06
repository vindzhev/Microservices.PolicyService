namespace PolicyService.Infrastructure.Persistance.Repositories
{
    using System.Threading.Tasks;
    
    using Microsoft.EntityFrameworkCore;
    
    using PolicyService.Domain.Entities;
    
    using PolicyService.Application.Common.Interfaces;

    public class OfferRepository : IOfferRepository
    {
        private readonly PolicyDbContext _context;

        public OfferRepository(PolicyDbContext context) => this._context = context;

        public void Add(Offer offer) => this._context.Offers.Add(offer);

        public async Task<Offer> WithNumber(string number) =>
            await this._context.Offers
                .Include("Covers")
                .Include("PolicyHolder")
                .Include("PolicyValidityPeriod")
                .FirstOrDefaultAsync(x => x.Number == number);

        public async Task SaveChangesAsync() => await this._context.SaveChangesAsync();
    }
}
