namespace ShoppingBasket.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Adds a product to the repository
        /// </summary>
        /// <param name="productName"></param>
        /// <param name="price"></param>
        void Add(string productName, decimal price);

        /// <summary>
        /// Returns true if the product is found
        /// and false otherwise
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        bool Contains(string productName);

        /// <summary>
        /// Returns the price for a specfic product
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        decimal GetPrice(string productName);
    }
}
