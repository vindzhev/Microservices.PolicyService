namespace PolicyService.Application.Offer.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using MediatR;
    using AutoMapper;
    
    using MicroservicesPOC.Shared.Common.Models;

    using PolicyService.Domain.Entities;
    
    using PolicyService.Application.Common.Models;
    using PolicyService.Application.Common.Services;
    using PolicyService.Application.Common.Interfaces;

    public class CreateOfferByAgentCommand : CreateOfferCommand, IRequest<CreateOfferResult>
    {
        public CreateOfferByAgentCommand(string productCode, DateTime policyFrom, DateTime policyTo, ICollection<string> selectedCovers, ICollection<QuestionAnswerDTO> answers, string agentLogin) : 
            base(productCode, policyFrom, policyTo, selectedCovers, answers) => this.AgentLogin = agentLogin;

        public string AgentLogin { get; set; }

        public class CreateOfferByAgentCommandHandler : IRequestHandler<CreateOfferByAgentCommand, CreateOfferResult>
        {
            private readonly IMapper _mapper;
            private readonly IPricing _pricingService;
            private readonly IOfferRepository _offerRepository;

            public CreateOfferByAgentCommandHandler(IMapper mapper, IOfferRepository offerRepository, IPricing pricingService)
            {
                this._mapper = mapper;
                this._pricingService = pricingService;
                this._offerRepository = offerRepository;
            }

            public async Task<CreateOfferResult> Handle(CreateOfferByAgentCommand request, CancellationToken cancellationToken)
            {
                var priceParameters = this._mapper.Map<PricingParameters>(request);
                var price = await this._pricingService.CalculatePrice(priceParameters);

                var offer = Offer.ForPriceAndAgent(priceParameters.ProductCode, priceParameters.PolicyFrom, priceParameters.PolicyTo, null, price, request.AgentLogin);

                this._offerRepository.Add(offer);
                await this._offerRepository.SaveChangesAsync();

                return this._mapper.Map<CreateOfferResult>(offer);
            }
        }
    }    
}
