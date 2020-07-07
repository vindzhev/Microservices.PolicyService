namespace PolicyService.Domain.Entities
{
    using System;

    using MicroservicesPOC.Shared.Domain;

    public class Cover : Entity<Guid>, ICloneable
    {
        public Cover(string code, decimal price)
        {
            this.Code = code;
            this.Price = price;
        }

        public string Code { get; private set; }

        public decimal Price { get; private set; }

        public Cover Clone() => new Cover(this.Code, this.Price);

        object ICloneable.Clone() => this.Clone();
    }
}
