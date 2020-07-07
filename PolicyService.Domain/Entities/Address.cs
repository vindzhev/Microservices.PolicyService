namespace PolicyService.Domain.Entities
{
    using System;

    using MicroservicesPOC.Shared.Domain;

    public class Address : Entity<Guid>
    {
        public Address(string country, string zipCode, string city, string street)
        {
            this.Country = country;
            this.ZipCode = zipCode;
            this.City = city;
            this.Street = street;
        }

        public string Country { get; private set; }

        public string ZipCode { get; private set; }

        public string City { get; private set; }

        public string Street { get; private set; }

        public static Address Of(string country, string zipCode, string city, string street) => new Address(country, zipCode, city, street);
    }
}
