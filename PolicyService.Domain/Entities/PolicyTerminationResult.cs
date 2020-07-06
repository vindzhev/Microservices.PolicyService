namespace PolicyService.Domain.Entities
{
    public class PolicyTerminationResult
    {
        public PolicyTerminationResult(PolicyVersion terminatedVersion, decimal amountToReturn)
        {
            this.TerminatedVersion = terminatedVersion;
            this.AmountToReturn = amountToReturn;
        }

        public PolicyVersion TerminatedVersion { get; private set; }

        public decimal AmountToReturn { get; private set; }
    }
}
