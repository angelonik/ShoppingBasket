using FluentAssertions;
using Moq;
using ShoppingBasket;
using ShoppingBasket.Interfaces;
using System;
using System.Linq;
using Xunit;

namespace UnitTests
{
    public class Basket_Should
    {
        private readonly Mock<IProductRepository> _products;
        private readonly Mock<IOfferRepository> _offers;
        private readonly Basket _basket;

        public Basket_Should()
        {
            _products = new Mock<IProductRepository>();
            _offers = new Mock<IOfferRepository>();
            _basket = new Basket(_products.Object, _offers.Object);
        }

        [Fact]
        public void Throw_exception_when_trying_to_add_product_that_not_exists_in_product_repository()
        {
            // arrange
            var productName = "test Product";
            _products.Setup(x => x.Contains(productName)).Returns(false);

            // act & assert
            Assert.Throws<ArgumentException>(() => _basket.BuyProduct(productName));
        }

        [Fact]
        public void Return_quantity_zero_when_product_not_in_the_basket()
        {
            // arrange
            var productName = "test Product";
            _products.Setup(x => x.Contains(productName)).Returns(true);

            // act
            var quantity = _basket.GetPurchasedQuantity(productName);

            // assert
            quantity.Should().Be(0);
        }

        [Fact]
        public void Return_quantity_one_when_adding_a_new_product_to_basket()
        {
            // arrange
            var productName = "test Product";
            _products.Setup(x => x.Contains(productName)).Returns(true);
            _basket.BuyProduct(productName);

            // act
            var quantity = _basket.GetPurchasedQuantity(productName);

            // assert
            quantity.Should().Be(1);
        }

        [Fact]
        public void Return_quantity_two_when_adding_product_that_exists_once()
        {
            // arrange
            var productName = "test Product";
            _products.Setup(x => x.Contains(productName)).Returns(true);
            _basket.BuyProduct(productName);
            _basket.BuyProduct(productName);

            // act
            var quantity = _basket.GetPurchasedQuantity(productName);

            // assert
            quantity.Should().Be(2);
        }

        [Fact]
        public void Calculate_zero_total_cost_when_no_products_on_the_basket()
        {
            // arrange
            _offers.Setup(x => x.GetAll()).Returns(Enumerable.Empty<IOffer>());

            // act
            var totalCost = _basket.CalculateTotalCost();

            // assert
            totalCost.Should().Be(0);
        }

        [Theory]
        [InlineData("first", 2.3, 1, "second", 5.6, 3)]
        [InlineData("first", 7, 3, "second", 7.2, 0)]
        [InlineData("first", 2, 1, "second", 4, 1)]
        public void Calculate_correct_total_cost_when_no_available_offers(
            string firstProductName, 
            decimal firstProductPrice,
            int firstProductQuantity,
            string secondProductName,
            decimal secondProductPrice,
            int secondProductQuantity)
        {
            // arrange
            _products.Setup(x => x.Contains(firstProductName)).Returns(true);
            _products.Setup(x => x.GetPrice(firstProductName)).Returns(firstProductPrice);
            _products.Setup(x => x.Contains(secondProductName)).Returns(true);
            _products.Setup(x => x.GetPrice(secondProductName)).Returns(secondProductPrice);
            _offers.Setup(x => x.GetAll()).Returns(Enumerable.Empty<IOffer>());

            for (int i = 0; i < firstProductQuantity; i++)
            {
                _basket.BuyProduct(firstProductName);
            }
            for (int i = 0; i < secondProductQuantity; i++)
            {
                _basket.BuyProduct(secondProductName);
            }

            var expectedCost = firstProductPrice * firstProductQuantity 
                + secondProductPrice * secondProductQuantity;

            // act
            var totalCost = _basket.CalculateTotalCost();

            // assert
            totalCost.Should().Be(expectedCost);
        }

        [Theory]
        [InlineData("first", 2.3, 1, "second", 5.6, 3, 2.5, 0)]
        [InlineData("first", 7, 3, "second", 7.2, 0, 1.2, 2.7)]
        [InlineData("first", 2, 1, "second", 4, 1, 0, 0)]
        public void Calculate_correct_total_cost_when_offers_exist(
            string firstProductName,
            decimal firstProductPrice,
            int firstProductQuantity,
            string secondProductName,
            decimal secondProductPrice,
            int secondProductQuantity,
            decimal offer1Discount,
            decimal offer2Discount)
        {
            // arrange
            _products.Setup(x => x.Contains(firstProductName)).Returns(true);
            _products.Setup(x => x.GetPrice(firstProductName)).Returns(firstProductPrice);
            _products.Setup(x => x.Contains(secondProductName)).Returns(true);
            _products.Setup(x => x.GetPrice(secondProductName)).Returns(secondProductPrice);

            var offer1 = new Mock<IOffer>();
            offer1.Setup(x => x.GetDiscount(_basket)).Returns(offer1Discount);

            var offer2 = new Mock<IOffer>();
            offer2.Setup(x => x.GetDiscount(_basket)).Returns(offer2Discount);

            _offers.Setup(x => x.GetAll()).Returns(new[] { offer1.Object, offer2.Object });

            for (int i = 0; i < firstProductQuantity; i++)
            {
                _basket.BuyProduct(firstProductName);
            }
            for (int i = 0; i < secondProductQuantity; i++)
            {
                _basket.BuyProduct(secondProductName);
            }

            var expectedCost = firstProductPrice * firstProductQuantity
                + secondProductPrice * secondProductQuantity - offer1Discount - offer2Discount;

            // act
            var totalCost = _basket.CalculateTotalCost();

            // assert
            totalCost.Should().Be(expectedCost);
        }
    }
}
