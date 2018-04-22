using FluentAssertions;
using Moq;
using ShoppingBasket;
using ShoppingBasket.Interfaces;
using System;
using Xunit;

namespace UnitTests
{
    public class Offer_Should
    {
        private readonly Mock<IProductRepository> _products;
        private readonly Mock<IBasket> _basket;

        public Offer_Should()
        {
            _products = new Mock<IProductRepository>();
            _basket = new Mock<IBasket>();
        }

        [Theory]
        [InlineData("qualifierProductName", 1, 2, "discountedProductName", 1,   2, 0.0, 0)]
        [InlineData("qualifierProductName", 1, 0, "discountedProductName", 1,   2, 1.0, 0)]
        [InlineData("qualifierProductName", 1, 4, "discountedProductName", 1,   0, 1.0, 0)]
        [InlineData("qualifierProductName", 2, 1, "discountedProductName", 1,   2, 1.0, 0)]
        [InlineData("qualifierProductName", 1, 5, "discountedProductName", 3,   2, 1.0, 0)]
        [InlineData("qualifierProductName", 1, 1, "discountedProductName", 1,   1, 1.2, 1.2)]
        [InlineData("qualifierProductName", 2, 2, "discountedProductName", 0.5, 1, 2.2, 1.1)]
        [InlineData("qualifierProductName", 1, 6, "discountedProductName", 0.5, 2, 2.2, 4.4)]
        [InlineData("qualifierProductName", 2, 4, "discountedProductName", 1,   1, 2.0, 2.0)]
        [InlineData("qualifierProductName", 3, 3, "discountedProductName", 1,   6, 2.2, 2.2)]
        public void Return_correct_discount_price(
            string qualifierProductName,
            int qualifierProductQuantityNeeded,
            int qualifierProductPurchasedQuantity,
            string discountedProductName,
            decimal discountedProductDiscountAmount,
            int discountedProductPurchasedQuantity,
            decimal discountedProductPrice,
            decimal expectedDiscount)
        {
            // arrange
            var offerQualifierProduct = (name: qualifierProductName, qualifierProductQuantityNeeded);
            var offerDiscountedProduct = (name: discountedProductName, discountedProductDiscountAmount);

            _products.Setup(x => x.GetPrice(discountedProductName))
                .Returns(discountedProductPrice);
            _basket.Setup(x => x.GetPurchasedQuantity(offerQualifierProduct.name))
                .Returns(qualifierProductPurchasedQuantity);
            _basket.Setup(x => x.GetPurchasedQuantity(offerDiscountedProduct.name))
                .Returns(discountedProductPurchasedQuantity);
            var offer = new Offer(_products.Object, offerQualifierProduct, offerDiscountedProduct);

            // act
            var discount = offer.GetDiscount(_basket.Object);

            // assert
            discount.Should().Be(expectedDiscount);
        }

        [Theory]
        [InlineData(0, 1)]
        [InlineData(1, 0)]
        [InlineData(0, 0)]
        public void Should_throw_exception_when_create_an_offer_with_qualifierProduct_quantity_or_discountedProduct_discount_amount_less_or_equal_to_zero(
            int quantityNeeded,
            decimal discountAmount)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => 
                new Offer(_products.Object,
                    qualifierProduct: (name: "testName", quantityNeeded: quantityNeeded),
                    discountedProduct: (name: "testName", discountAmount: discountAmount)
                ));
        }
    }
}
