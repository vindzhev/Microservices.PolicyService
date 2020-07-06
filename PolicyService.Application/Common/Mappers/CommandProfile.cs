namespace PolicyService.Application.Common.Mappers
{
    using System.Linq;

    using AutoMapper;

    using PolicyService.Domain.Entities;

    using PolicyService.Application.Common.Models;
    using PolicyService.Application.Offer.Commands;

    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            this.CreateMap<CreateOfferCommand, PricingParameters>()
                .ForMember(x => x.Answers, opt => opt.MapFrom(src =>
                    src.Answers.Select(a => Answer.Create(a.Type, a.QuestionCode, a.GetAnswer()))))
                .IncludeAllDerived();

            this.CreateMap<CreateOfferByAgentCommand, PricingParameters>();

            //this.CreateMap<PricingParameters, CalculationData>()
            //    .ForMember(x => x.Answers, opt => opt.<CustomResolver>());

            this.CreateMap<Offer, CreateOfferResult>()
                .ForMember(x => x.OfferNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(x => x.CoverPrices, opt => opt.MapFrom(src => src.Covers.ToDictionary(c => c.Code, c => c.Price)));
        }

        //public class CustomResolver : IValueResolver<PricingParameters, CalculationData, QuestionAnswerDTO>
        //{
        //    public QuestionAnswerDTO Resolve(PricingParameters source, CalculationData destination, QuestionAnswerDTO destMember, ResolutionContext context) =>
        //        source.Answers switch
        //        {
        //            TextAnswer _ => new TextQuestionAnswerDTO() { QuestionCode = source.QuestionCode, Answer = (string)source.GetAnswerValue() },
        //            ChoiceAnswer _ => new ChoiceQuestionAnswerDTO() { QuestionCode = source.QuestionCode, Answer = (string)source.GetAnswerValue() },
        //            NumericAnswer _ => new NumericQuestionAnswerDTO() { QuestionCode = source.QuestionCode, Answer = (decimal)source.GetAnswerValue() },
        //            _ => throw new ArgumentException("Unexpectd answer type " + source.GetType().Name),
        //        };
        //}
    }
}
