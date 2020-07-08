namespace PolicyService.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    
    using MicroservicesPOC.Shared.Domain;
    using MicroservicesPOC.Shared.Common.Models;

    using PolicyService.Domain.Extensions;

    public class Policy : Entity<Guid>
    {
        //TODO: Created at to auditable entity
        private readonly IList<PolicyVersion> _versions = new List<PolicyVersion>();

        public Policy() { }

        protected Policy(PolicyHolder policyHolder, Offer offer)
        {
            this.Number = Guid.NewGuid();
            this.ProductCode = offer.ProductCode;
            this.Status = PolicyStatus.Active;
            this.AgentLogin = offer.AgentLogin;

            this._versions.Add(PolicyVersion.FromOffer(this, 1, policyHolder, offer));
        }

        public Guid Number { get; private set; }

        public string ProductCode { get; set; }

        public PolicyStatus Status { get; private set; }

        public DateTime CreatedAt { get; private set; } = DateTime.Now;

        public string AgentLogin { get; private set; }

        public ICollection<PolicyVersion> Versions => new ReadOnlyCollection<PolicyVersion>(this._versions);

        public static Policy FromOffer(PolicyHolder policyHolder, Offer offer) => new Policy(policyHolder, offer);

        public PolicyTerminationResult Terminate(DateTime terminationDate)
        {
            //ensure is not already terminated
            if (Status != PolicyStatus.Active)
                throw new ApplicationException($"Policy {Number} is already terminated");

            //get version valid at term date
            var versionAtTerminationDate = this._versions.EffectiveOn(terminationDate);

            if (versionAtTerminationDate == null)
                throw new ApplicationException($"No valid policy {Number} version exists at {terminationDate}. Policy cannot be terminated.");

            if (!versionAtTerminationDate.CoverPeriod.Contains(terminationDate))
                throw new ApplicationException($"Policy {Number} does not cover {terminationDate}. Policy cannot be terminated at this date.");

            //create terminal version
            this._versions.Add(versionAtTerminationDate.EndsOn(terminationDate));

            //change status
            this.Status = PolicyStatus.Terminated;

            //return term version
            var terminalVersion = this._versions.LastVersion();
            return new PolicyTerminationResult(terminalVersion, versionAtTerminationDate.TotalPremiumAmount - terminalVersion.TotalPremiumAmount);
        }

        public virtual int NextVersionNumber() => this._versions.Count == 0 ? 1 : this._versions.LastVersion().VersionNumber + 1;
    }
}
