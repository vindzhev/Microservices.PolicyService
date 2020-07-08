namespace PolicyService.Application.Common.Mappers
{
    using System.Linq;

    using AutoMapper;

    using MicroservicesPOC.Shared.Common.Models;
    using MicroservicesPOC.Shared.Messaging.Events;

    using PolicyService.Domain.Entities;
    using PolicyService.Application.Policy.Commands;
    using PolicyService.Domain.Extensions;

    public class PolicyProfile : Profile
    {
        public PolicyProfile()
        {
            this.CreateMap<PolicyVersion, PolicyDTO>()
                .ForMember(x => x.Number, opt => opt.MapFrom(src => src.Policy.Number))
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(src => src.Policy.ProductCode))
                .ForMember(x => x.DateFrom, opt => opt.MapFrom(src => src.CoverPeriod.ValidFrom))
                .ForMember(x => x.DateTo, opt => opt.MapFrom(src => src.CoverPeriod.ValidTo))
                .ForMember(x => x.PolicyHolder, opt => opt.MapFrom(src => $"{src.PolicyHolder.FirstName} {src.PolicyHolder.FirstName}"))
                .ForMember(x => x.TotalPremum, opt => opt.MapFrom(src => src.TotalPremiumAmount))
                .ForMember(x => x.Covers, opt => opt.MapFrom(src => src.Covers.Select(x => x.Code)));

            this.CreateMap<PolicyTerminationResult, PolicyTerminatedEvent>()
                .ForMember(x => x.PolicyNumber, opt => opt.MapFrom(src => src.TerminatedVersion.Policy.Number))
                .ForMember(x => x.PolicyFrom, opt => opt.MapFrom(src => src.TerminatedVersion.CoverPeriod.ValidFrom))
                .ForMember(x => x.PolicyTo, opt => opt.MapFrom(src => src.TerminatedVersion.CoverPeriod.ValidTo))
                .ForMember(x => x.ProductCode, opt => opt.MapFrom(src => src.TerminatedVersion.Policy.ProductCode))
                .ForMember(x => x.TotalPremium, opt => opt.MapFrom(src => src.TerminatedVersion.TotalPremiumAmount))
                .ForMember(x => x.AmountToReturn, opt => opt.MapFrom(src => src.AmountToReturn))
                .ForMember(x => x.PolicyHolder, opt => opt.MapFrom(src => new PersonDTO() 
                { 
                    //TODO: Move to automapping
                    FirstName = src.TerminatedVersion.PolicyHolder.FirstName,
                    LastName = src.TerminatedVersion.PolicyHolder.LastName, 
                    TaxId = src.TerminatedVersion.PolicyHolder.Pesel 
                }));

            this.CreateMap<PolicyTerminationResult, TerminatePolicyResult>()
                .ForMember(x => x.PolicyNumber, opt => opt.MapFrom(src => src.TerminatedVersion.Policy.Number))
                .ForMember(x => x.AmountToReturn, opt => opt.MapFrom(src => src.AmountToReturn));

            this.CreateMap<Policy, PolicyCreatedEvent>()
                .ForMember(x => x.PolicyNumber, opt => opt.MapFrom(src => src.Number))
                .ForMember(x => x.PolicyFrom, opt => opt.MapFrom(src => src.Versions.LastVersion().CoverPeriod.ValidFrom))
                .ForMember(x => x.PolicyTo, opt => opt.MapFrom(src => src.Versions.LastVersion().CoverPeriod.ValidTo))
                .ForMember(x => x.TotalPremium, opt => opt.MapFrom(src => src.Versions.LastVersion().TotalPremiumAmount))
                .ForMember(x => x.PolicyHolder, opt => opt.MapFrom(src => new PersonDTO()
                {
                    //TODO: Move to automapping
                    FirstName = src.Versions.LastVersion().PolicyHolder.FirstName,
                    LastName = src.Versions.LastVersion().PolicyHolder.LastName,
                    TaxId = src.Versions.LastVersion().PolicyHolder.Pesel
                }));
        }
    }
}
