using ShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingBasket
{
    public class Basket : IBasket
    {
        private readonly IProductRepository _products;
        private readonly IOfferRepository _offers;
        private const int DecimalsToShow = 2;

        public Dictionary<string, int> _purchasedProducts;

        public Basket(IProductRepository products, IOfferRepository offers)
        {
            _purchasedProducts = new Dictionary<string, int>
                (StringComparer.InvariantCultureIgnoreCase);
            _products = products;
            _offers = offers;
        }

        public void BuyProduct(string productName)
        {
            if (!_products.Contains(productName))
            {
                throw new ArgumentException("Product is not available on price catalog.");
            }
            _purchasedProducts.TryGetValue(productName, out var quantity);
            _purchasedProducts[productName] = quantity + 1;
        }

        public int GetPurchasedQuantity(string productName)
        {
            _purchasedProducts.TryGetValue(productName, out var quantity);
            return quantity;
        }

        public decimal CalculateTotalCost()
        {
            var sum = _purchasedProducts.Sum(pair => _products.GetPrice(pair.Key) * pair.Value);

            var discounts = _offers.GetAll().Sum(offer => offer.GetDiscount(this));

            sum -= discounts;

            return Math.Round(sum, DecimalsToShow);
        }
    }
}
