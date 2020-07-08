namespace PolicyService.Domain.Entities
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using MicroservicesPOC.Shared.Domain;

    public class PolicyVersion : Entity<Guid>
    {
        private IList<PolicyCover> _covers = new List<PolicyCover>();

        public PolicyVersion() { }

        private PolicyVersion(Policy policy, int version, PolicyHolder policyHolder, Offer offer)
        {
            this.Policy = policy;
            this.VersionNumber = version;
            this.PolicyHolder = policyHolder;
            this.CoverPeriod = offer.PolicyValidityPeriod.Clone();
            this.VersionValidityPeriod = offer.PolicyValidityPeriod.Clone();

            this._covers = offer.Covers.Select(x => new PolicyCover(x, offer.PolicyValidityPeriod.Clone())).ToList();

            this.TotalPremiumAmount = this._covers.Sum(x => x.Premium);
        }

        public Guid PolicyId { get; private set; }
        public Policy Policy { get; private set; }

        public int VersionNumber { get; private set; }

        public PolicyHolder PolicyHolder { get; private set; }

        public ValidityPeriod CoverPeriod { get; private set; }

        public ValidityPeriod VersionValidityPeriod { get; private set; }

        public ICollection<PolicyCover> Covers => new ReadOnlyCollection<PolicyCover>(this._covers);

        public decimal TotalPremiumAmount { get; private set; }

        public static PolicyVersion FromOffer(Policy policy, int version, PolicyHolder policyHolder, Offer offer) => new PolicyVersion(policy, version, policyHolder, offer);

        public bool IsEffectiveOn(DateTime date) => this.VersionValidityPeriod.Contains(date);

        public PolicyVersion EndsOn(DateTime date)
        {
            var endedCovers = this._covers.Select(x => x.EndsOn(date)).ToList();

            return new PolicyVersion
            {
                Policy = this.Policy,
                VersionNumber = this.VersionNumber,
                PolicyHolder = this.PolicyHolder,
                CoverPeriod = this.CoverPeriod,
                VersionValidityPeriod = ValidityPeriod.Between(date.AddDays(1), this.VersionValidityPeriod.ValidTo),
                _covers = endedCovers,
                TotalPremiumAmount = endedCovers.Sum(x => x.Premium)
            };
        }
    }
}
