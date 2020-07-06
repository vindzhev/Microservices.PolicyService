namespace PolicyService.Infrastructure.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    
    using MicroservicesPOC.Shared.Common.Models;
    
    using PolicyService.Domain.Entities;
    
    using PolicyService.Application.Common.Models;
    using PolicyService.Application.Common.Services;

    public class PricingService : IPricing
    {
        private readonly IPricingClient _pricingClient;

        public PricingService(IPricingClient pricingClient) =>
            this._pricingClient = pricingClient ?? throw new ArgumentNullException(nameof(pricingClient));

        private QuestionAnswerDTO ToQuestionAnswer(Answer a)
        {
            if (a is TextAnswer)
                return new TextQuestionAnswerDTO { QuestionCode = a.QuestionCode, Answer = (string)a.GetAnswerValue() };

            if (a is ChoiceAnswer)
                return new ChoiceQuestionAnswerDTO { QuestionCode = a.QuestionCode, Answer = (string)a.GetAnswerValue() };

            if (a is NumericAnswer)
                return new NumericQuestionAnswerDTO { QuestionCode = a.QuestionCode, Answer = (decimal)a.GetAnswerValue() };

            throw new ArgumentException("Unexpectd answer type " + a.GetType().Name);
        }

        public async Task<Price> CalculatePrice(PricingParameters parameters)
        {
            //TODO: move to automapper
            var data = new CalculationData(parameters.ProductCode, parameters.PolicyFrom, 
                parameters.PolicyTo, parameters.SelectedCovers, parameters.Answers.Select(a => ToQuestionAnswer(a)).ToList());

            var result = await this._pricingClient.CalculatePrice(data);

            return new Price(result.CoverPrices);
        }
    }
}
