namespace PolicyService.Domain.Extensions
{
    using System;
    using System.Linq;
    using System.Collections.Generic;

    using PolicyService.Domain.Entities;

    public static class PolicyVersionsExtensions
    {
        public static PolicyVersion EffectiveOn(this IEnumerable<PolicyVersion> collection, DateTime date) =>
            collection.Where(x => x.IsEffectiveOn(date))
                .OrderByDescending(x => x.VersionNumber)
                .FirstOrDefault();

        public static PolicyVersion WithNumber(this IEnumerable<PolicyVersion> collection, int number) =>
            collection.FirstOrDefault(x => x.VersionNumber == number);

        public static PolicyVersion FirstVersion(this IEnumerable<PolicyVersion> collection) =>
            collection.FirstOrDefault(x => x.VersionNumber == 1);

        public static PolicyVersion LastVersion(this IEnumerable<PolicyVersion> collection) =>
            collection.OrderByDescending(x => x.VersionNumber).FirstOrDefault();
    }
}
