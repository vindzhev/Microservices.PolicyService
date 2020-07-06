namespace PolicyService.Application.Offer.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    
    using MediatR;
    using AutoMapper;
    
    using MicroservicesPOC.Shared.Common.Models;
    
    using PolicyService.Domain.Entities;
    using PolicyService.Application.Common.Models;
    using PolicyService.Application.Common.Services;
    using PolicyService.Application.Common.Interfaces;

    public class CreateOfferCommand : IRequest<CreateOfferResult>
    {
        public CreateOfferCommand(string productCode, DateTime policyFrom, DateTime policyTo, ICollection<string> selectedCovers, ICollection<QuestionAnswerDTO> answers)
        {
            this.ProductCode = productCode;
            this.PolicyFrom = policyFrom;
            this.PolicyTo = policyTo;
            this.SelectedCovers = selectedCovers;
            this.Answers = answers;
        }

        public string ProductCode { get; set; }

        public DateTime PolicyFrom { get; set; }

        public DateTime PolicyTo { get; set; }

        public ICollection<string> SelectedCovers { get; set; }

        public ICollection<QuestionAnswerDTO> Answers { get; set; }

        public class CreateOfferCommandHandler : IRequestHandler<CreateOfferCommand, CreateOfferResult>
        {
            private readonly IMapper _mapper;
            private readonly IPricing _pricingService;
            private readonly IOfferRepository _offerRepository;

            public CreateOfferCommandHandler(IMapper mapper, IOfferRepository offerRepository, IPricing pricingService)
            {
                this._mapper = mapper;
                this._pricingService = pricingService;
                this._offerRepository = offerRepository;
            }

            public async Task<CreateOfferResult> Handle(CreateOfferCommand request, CancellationToken cancellationToken)
            {
                var priceParameters = this._mapper.Map<PricingParameters>(request);
                var price = await this._pricingService.CalculatePrice(priceParameters);

                var offer = Offer.ForPrice(priceParameters.ProductCode, priceParameters.PolicyFrom, priceParameters.PolicyTo, null, price);

                this._offerRepository.Add(offer);
                await this._offerRepository.SaveChangesAsync();

                return this._mapper.Map<CreateOfferResult>(offer);
            }
        }
    }

    public class CreateOfferResult
    {
        public string OfferNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public IDictionary<string, decimal> CoverPrices { get; set; }
    }
}
