namespace PolicyService.Application.Policy.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    
    using MediatR;
    using AutoMapper;
    
    using MicroservicesPOC.Shared.Messaging.Events;
    
    using PolicyService.Domain.Entities;
    using PolicyService.Application.Common.Interfaces;

    public class TerminatePolicyCommand : IRequest<TerminatePolicyResult>
    {
        public Guid PolicyNumber { get; set; }

        public DateTime TerminationDate { get; set; }

        public class TerminatePolicyCommandHandler : IRequestHandler<TerminatePolicyCommand, TerminatePolicyResult>
        {
            private readonly IMapper _mapper;
            private readonly IEventPublisher _eventPublisher;
            private readonly IPolicyRepository _policyRepository;

            public TerminatePolicyCommandHandler(IPolicyRepository policyRepository, IMapper mapper, IEventPublisher eventPublisher)
            {
                this._mapper = mapper;
                this._eventPublisher = eventPublisher;
                this._policyRepository = policyRepository;
            }

            public async Task<TerminatePolicyResult> Handle(TerminatePolicyCommand request, CancellationToken cancellationToken)
            {
                Policy policy = await this._policyRepository.WithNumber(request.PolicyNumber);
                PolicyTerminationResult terminationResult = policy.Terminate(request.TerminationDate);

                await this._policyRepository.SaveChangesAsync();

                this._eventPublisher.PublishMessage(this._mapper.Map<PolicyTerminatedEvent>(terminationResult));

                return this._mapper.Map<TerminatePolicyResult>(terminationResult);
            }
        }
    }

    public class TerminatePolicyResult
    {
        public string PolicyNumber { get; set; }

        public decimal AmountToReturn { get; set; }
    }
}
