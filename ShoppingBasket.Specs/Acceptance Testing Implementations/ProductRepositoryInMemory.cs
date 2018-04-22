using ShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;

namespace ShoppingBasket.Specs
{
    public class ProductRepositoryInMemory : IProductRepository
    {
        private Dictionary<string, decimal> _productsWithPrices;
        
        public ProductRepositoryInMemory(
            IEnumerable<(string name, decimal price)> productsWithPrices)
        {
            _productsWithPrices = new Dictionary<string, decimal>
                (StringComparer.InvariantCultureIgnoreCase);

            foreach (var (name, price) in productsWithPrices)
            {
                _productsWithPrices.Add(name, price);
            }
        }

        public void Add(string name, decimal price)
        {
            _productsWithPrices.Add(name, price);
        }

        public bool Contains(string name)
        {
            return _productsWithPrices.ContainsKey(name);
        }

        public decimal GetPrice(string product)
        {
            _productsWithPrices.TryGetValue(product, out var price);
            return price;
        }
    }
}
