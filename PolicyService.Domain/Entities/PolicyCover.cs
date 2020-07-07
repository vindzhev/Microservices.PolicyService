namespace PolicyService.Domain.Entities
{
    using System;
    
    using MicroservicesPOC.Shared.Domain;

    public class PolicyCover : Entity<Guid>
    {
        public PolicyCover() { }

        public PolicyCover(Cover cover, ValidityPeriod coverPeriod)
        {
            Code = cover.Code;
            Premium = cover.Price;
            CoverPeriod = coverPeriod;
        }

        public string Code { get; private set; }

        public decimal Premium { get; private set; }

        public ValidityPeriod CoverPeriod { get; private set; }

        public PolicyCover EndsOn(DateTime endDate)
        {
            var coverPeriod = this.CoverPeriod.Days;
            var daysRemaining = coverPeriod - this.CoverPeriod.EndsOn(endDate).Days;
            var premium = decimal.Round(this.Premium - (this.Premium * decimal.Divide(daysRemaining, coverPeriod)), 2);

            return new PolicyCover 
            { 
                Code = this.Code, 
                Premium = premium, 
                CoverPeriod = this.CoverPeriod.EndsOn(endDate) 
            };
        }
    }
}
