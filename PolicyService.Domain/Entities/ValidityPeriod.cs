namespace PolicyService.Domain.Entities
{
    using System;

    using MicroservicesPOC.Shared.Common.Entities;

    public class ValidityPeriod : Entity<Guid>, ICloneable
    {
        public ValidityPeriod(DateTime validFrom, DateTime validTo)
        {
            this.ValidFrom = validFrom;
            this.ValidTo = validTo;
        }

        public DateTime ValidFrom { get; private set; }

        public DateTime ValidTo { get; private set; }

        public int Days => this.ValidTo.Subtract(this.ValidFrom).Days;

        public static ValidityPeriod Between(DateTime from, DateTime to) => new ValidityPeriod(from, to);

        public ValidityPeriod Clone() => new ValidityPeriod(this.ValidFrom, this.ValidTo);

        public bool Contains(DateTime date) => (this.ValidFrom <= date && date <= this.ValidTo);

        public ValidityPeriod EndsOn(DateTime date) => new ValidityPeriod(this.ValidFrom, date);

        object ICloneable.Clone() => this.Clone();
    }
}
