namespace PolicyService.Infrastructure.Persistance.Repositories
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    
    using PolicyService.Domain.Entities;
    
    using PolicyService.Application.Common.Interfaces;

    public class PolicyRepository : IPolicyRepository
    {
        private readonly PolicyDbContext _context;

        public PolicyRepository(PolicyDbContext context) => _context = context;

        public void Add(Policy policy) => this._context.Policies.Add(policy);

        public async Task<Policy> WithNumber(Guid number) =>
            await this._context.Policies
                .Include("Versions")
                .Include("Versions.Covers")
                .Include("Versions.CoverPeriod")
                .Include("Versions.PolicyHolder")
                .Include("Versions.VersionValidityPeriod")
                .FirstOrDefaultAsync(x => x.Number == number);

        public async Task SaveChangesAsync() => await this._context.SaveChangesAsync();
    }
}
