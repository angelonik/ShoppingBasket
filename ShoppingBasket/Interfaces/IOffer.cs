namespace ShoppingBasket.Interfaces
{
    public interface IOffer
    {
        /// <summary>
        /// Returns the price discount for a basket
        /// based on this specific offer
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        decimal GetDiscount(IBasket basket);
    }
}
