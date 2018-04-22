namespace ShoppingBasket.Interfaces
{
    public interface IBasket
    {
        /// <summary>
        /// Adds a product to the basket to be purchased
        /// will throw exception if the product name is not found
        /// in the product repository
        /// </summary>
        /// <param name="productName"></param>
        void BuyProduct(string productName);

        /// <summary>
        /// Returns the amount of purchased products on the basket
        /// will return 0 if the product is not in basket
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        int GetPurchasedQuantity(string productName);

        /// <summary>
        /// Calculates the total cost of the purchased products
        /// in the basket. It will also calculate any existing offers
        /// </summary>
        /// <returns></returns>
        decimal CalculateTotalCost();
    }
}
