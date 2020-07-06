namespace PolicyService.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using MicroservicesPOC.Shared.Common.Entities;

    public class Price
    {
        private IDictionary<string, decimal> _prices;

        public Price(IDictionary<string, decimal> prices) => this._prices = prices;

        public IReadOnlyDictionary<string, decimal> Prices => new ReadOnlyDictionary<string, decimal>(this._prices);
    }
}
