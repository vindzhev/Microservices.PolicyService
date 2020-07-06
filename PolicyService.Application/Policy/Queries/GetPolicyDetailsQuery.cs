namespace PolicyService.Application.Policy.Queries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using MediatR;
    using AutoMapper;

    using MicroservicesPOC.Shared.Common.Models;

    using PolicyService.Domain.Entities;
    using PolicyService.Domain.Extensions;
    using PolicyService.Application.Common.Interfaces;

    public class GetPolicyDetailsQuery : IRequest<PolicyDTO>
    {
        public GetPolicyDetailsQuery(Guid id) => this.PolicyNumber = id;

        public Guid PolicyNumber { get; set; }

        public class GetPolicyDetailsQueryHandler : IRequestHandler<GetPolicyDetailsQuery, PolicyDTO>
        {
            private readonly IMapper _mapper;
            private readonly IPolicyRepository _policyRepository;

            public GetPolicyDetailsQueryHandler(IMapper mapper, IPolicyRepository policyRepository)
            {
                this._mapper = mapper;
                this._policyRepository = policyRepository;
            }

            public async Task<PolicyDTO> Handle(GetPolicyDetailsQuery request, CancellationToken cancellationToken)
            {
                Policy policy = await this._policyRepository.WithNumber(request.PolicyNumber);
                PolicyVersion policyVersion = policy.Versions.FirstVersion();

                return this._mapper.Map<PolicyDTO>(policyVersion);
            }
        }
    }
}
