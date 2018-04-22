using ShoppingBasket.Interfaces;

namespace ShoppingBasket.Specs
{
    public class ThreeMilkFourthForFree : Offer, IOffer
    {
        public ThreeMilkFourthForFree(IProductRepository products)
            : base(products,
                qualifierProduct: (name: "Milk", quantityNeeded: 3),
                discountedProduct: (name: "Milk", discountAmount: 1)
            )
        { }
    }
}
