using ShoppingBasket.Interfaces;

namespace ShoppingBasket.Specs
{
    public class TwoButterFiftyPercentOffBread : Offer, IOffer
    {
        public TwoButterFiftyPercentOffBread(IProductRepository products)
            : base(products,
                qualifierProduct: (name: "Butter", quantityNeeded: 2),
                discountedProduct: (name: "Bread", discountAmount: 0.5m)
            )
        { }
    }
}
