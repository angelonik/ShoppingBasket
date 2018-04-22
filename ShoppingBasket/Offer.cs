using ShoppingBasket.Interfaces;
using System;

namespace ShoppingBasket
{
    public class Offer
    {
        private readonly IProductRepository _products;
        private readonly (string name, int quantityNeeded) _qualifierProduct;
        private readonly (string name, decimal discountAmount) _discountedProduct;

        public Offer(IProductRepository products,
            (string name, int quantityNeeded) qualifierProduct,
            (string name, decimal discountAmount) discountedProduct)
        {
            if (qualifierProduct.quantityNeeded <= 0)
            {
                throw new ArgumentOutOfRangeException("Quantity for the qualifier product has to be greater than 0");
            }
            if (discountedProduct.discountAmount <= 0)
            {
                throw new ArgumentOutOfRangeException("Discount amount for the discounted product has to be greater than 0");
            }
            _products = products;
            _qualifierProduct = qualifierProduct;
            _discountedProduct = discountedProduct;
        }

        public decimal GetDiscount(IBasket basket)
        {
            var timesOfferWillBeApplied = GetTimesToBeApplied(basket);

            return timesOfferWillBeApplied * _discountedProduct.discountAmount * _products.GetPrice(_discountedProduct.name);
        }

        private decimal GetTimesToBeApplied(IBasket basket)
        {
            var qualifierProductQuantityPurchased = basket.GetPurchasedQuantity(_qualifierProduct.name);
            var discountedProductQuantityPurchased = basket.GetPurchasedQuantity(_discountedProduct.name);

            var timesQualifierProductFound = qualifierProductQuantityPurchased / _qualifierProduct.quantityNeeded;
            var timesDiscountedProductFound = (int)(discountedProductQuantityPurchased / _discountedProduct.discountAmount);

            var timesOfferWillBeApplied = Math.Min(timesQualifierProductFound, timesDiscountedProductFound);

            return timesOfferWillBeApplied;
        }
    }
}
