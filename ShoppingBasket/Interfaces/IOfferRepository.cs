using System.Collections.Generic;

namespace ShoppingBasket.Interfaces
{
    public interface IOfferRepository
    {
        /// <summary>
        /// Returns all existing offers
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOffer> GetAll();
    }
}
