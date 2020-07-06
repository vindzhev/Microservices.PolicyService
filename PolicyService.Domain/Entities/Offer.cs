namespace PolicyService.Domain.Entities
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    
    using MicroservicesPOC.Shared.Common.Models;
    using MicroservicesPOC.Shared.Common.Entities;

    //TODO: see is IAuditableEntity is more suitable 
    public class Offer : Entity<Guid>
    {
        private readonly List<Cover> _covers = new List<Cover>();

        public Offer() { }

        protected Offer(string productCode, DateTime policyFrom, DateTime policyTo, PolicyHolder policyHolder, Price price, string agentLogin)
        {
            this._covers = price.Prices
                .Select(c => new Cover(c.Key, c.Value))
                .ToList();

            this.Number = Guid.NewGuid().ToString();
            this.ProductCode = productCode;
            this.PolicyValidityPeriod = ValidityPeriod.Between(policyFrom, policyTo);
            this.PolicyHolder = policyHolder;
            this.Status = OfferStatus.New;
            this.TotalPrice = price.Prices.Sum(c => c.Value);
            this.AgentLogin = agentLogin;
        }

        public string Number { get; private set; }

        public string ProductCode { get; private set; }

        public ValidityPeriod PolicyValidityPeriod { get; private set; }

        public PolicyHolder PolicyHolder { get; protected set; }

        public decimal TotalPrice { get; private set; }

        public OfferStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public IReadOnlyCollection<Cover> Covers => this._covers.AsReadOnly();

        public string AgentLogin { get; private set; }

        public static Offer ForPrice(string productCode, DateTime policyFrom, DateTime policyTo, PolicyHolder policyHolder, Price price) =>
            new Offer(productCode, policyFrom, policyTo, policyHolder, price, null);

        public static Offer ForPriceAndAgent(string productCode, DateTime policyFrom, DateTime policyTo, PolicyHolder policyHolder, Price price, string agent) =>
            new Offer(productCode, policyFrom, policyTo, policyHolder, price, agent);

        public virtual Policy Buy(PolicyHolder customer)
        {
            //Switch DateTime with the IDateTime service...
            if (IsExpired(DateTime.Now))
                throw new ApplicationException($"Offer {Number} has expired");

            if (Status != OfferStatus.New)
                throw new ApplicationException($"Offer {Number} is not in new status and cannot be bought");

            this.Status = OfferStatus.Converted;

            return Policy.FromOffer(customer, this);
        }

        public bool IsExpired(DateTime theDate) => this.CreatedAt.AddDays(30) < theDate;
    }
}
