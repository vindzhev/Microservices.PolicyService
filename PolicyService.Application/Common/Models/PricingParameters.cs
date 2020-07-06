namespace PolicyService.Application.Common.Models
{
    using System;
    using System.Collections.Generic;

    using PolicyService.Domain.Entities;

    public class PricingParameters
    {
        public string ProductCode { get; set; }

        public DateTime PolicyFrom { get; set; }

        public DateTime PolicyTo { get; set; }

        public ICollection<string> SelectedCovers { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
