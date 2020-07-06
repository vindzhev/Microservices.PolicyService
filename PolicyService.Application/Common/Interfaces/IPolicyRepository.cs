namespace PolicyService.Application.Common.Interfaces
{
    using System;
    using System.Threading.Tasks;

    using PolicyService.Domain.Entities;

    public interface IPolicyRepository
    {
        void Add(Policy policy);

        Task<Policy> WithNumber(Guid number);

        Task SaveChangesAsync();
    }
}
