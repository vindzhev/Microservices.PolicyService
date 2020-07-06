namespace PolicyService.Application.Policy.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    
    using MediatR;
    using AutoMapper;
    
    using MicroservicesPOC.Shared.Common.Models;
    using MicroservicesPOC.Shared.Messaging.Events;
    
    using PolicyService.Domain.Entities;
    using PolicyService.Application.Common.Models;
    using PolicyService.Application.Common.Interfaces;

    public class CreatePolicyCommand : IRequest<Guid>
    {
        public string OfferNumber { get; set; }

        public PersonDTO PolicyHolder { get; set; }

        public AddressDTO PolicyHolderAddress { get; set; }


        public class CreatePolicyCommandHandler : IRequestHandler<CreatePolicyCommand, Guid>
        {
            private readonly IMapper _mapper;
            private readonly IEventPublisher _eventPublisher;
            private readonly IOfferRepository _offerRepository;
            private readonly IPolicyRepository _policyRepository;

            public CreatePolicyCommandHandler(IMapper mapper, IEventPublisher eventPublisher, IOfferRepository offerRepository, IPolicyRepository policyRepository)
            {
                this._mapper = mapper;
                this._eventPublisher = eventPublisher;
                this._offerRepository = offerRepository;
                this._policyRepository = policyRepository;
            }

            public async Task<Guid> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
            {
                var offer = await this._offerRepository.WithNumber(request.OfferNumber);
                var customer = new PolicyHolder(request.PolicyHolder.FirstName, request.PolicyHolder.LastName, request.PolicyHolder.TaxId,
                    Address.Of(request.PolicyHolderAddress.Country, request.PolicyHolderAddress.ZipCode, request.PolicyHolderAddress.City, request.PolicyHolderAddress.Street));

                var policy = offer.Buy(customer);

                this._policyRepository.Add(policy);
                this._eventPublisher.PublishMessage(this._mapper.Map<PolicyCreatedEvent>(policy));

                await this._policyRepository.SaveChangesAsync();

                return policy.Number;
            }
        }
    }
}
