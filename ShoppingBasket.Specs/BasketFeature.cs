using FluentAssertions;
using ShoppingBasket.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Xbehave;

namespace ShoppingBasket.Specs
{
    // In order to make an order
    // As a customer
    // I want a basket that calculates the total cost,
    // of my purchased products and applies discounts from offers
    public class BasketFeature
    {
        private readonly IProductRepository _products;
        private readonly IOfferRepository _offers;

        // Configuration for all Scenarios
        public BasketFeature()
        {
            var productsWithPrices = new[]
            {
                ("Butter", 0.80m),
                ("Milk", 1.15m),
                ("Bread", 1.00m)
            };
            _products = new ProductRepositoryInMemory(productsWithPrices);

            var offers = new List<IOffer>()
            {
                new TwoButterFiftyPercentOffBread(_products),
                new ThreeMilkFourthForFree(_products)
            };
            _offers = new OfferRepositoryInMemory(offers);
        }

        [Scenario]
        [Example(1, 1, 1, 2.95)]
        [Example(2, 0, 2, 3.10)]
        [Example(0, 4, 0, 3.45)]
        [Example(2, 8, 1, 9.00)]
        public void Total_cost_calculation(int butterQuantity, int milkQuantity, int breadQuantity,
            decimal expectedResult,
            IBasket basket,
            decimal result)
        {
            $"Given the basket has {butterQuantity} butter, {milkQuantity} milk and {breadQuantity} bread"
                .x(() =>
                {
                    basket = new Basket(_products, _offers);

                    Enumerable.Range(1, butterQuantity).ToList().ForEach(_=> basket.BuyProduct("butter"));
                    Enumerable.Range(1, milkQuantity).ToList().ForEach(_=> basket.BuyProduct("milk"));
                    Enumerable.Range(1, breadQuantity).ToList().ForEach(_=> basket.BuyProduct("bread"));
                });

            "When I total the basket"
                .x(() => result = basket.CalculateTotalCost());

            $"Then the total should be {expectedResult}"
                .x(() => result.Should().Be(expectedResult));
        }
    }
}
