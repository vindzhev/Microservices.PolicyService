namespace PolicyService.Domain.Entities
{
    using System;

    using MicroservicesPOC.Shared.Common.Entities;

    public class PolicyHolder : Entity<Guid>
    {
        public PolicyHolder() { }

        public PolicyHolder(string firstName, string lastName, string pesel, Address address)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Pesel = pesel;
            this.Address = address;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Pesel { get; private set; } //TaxId

        public Address Address { get; private set; }
    }
}
