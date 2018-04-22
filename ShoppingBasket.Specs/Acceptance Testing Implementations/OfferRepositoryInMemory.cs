using ShoppingBasket.Interfaces;
using System.Collections.Generic;

namespace ShoppingBasket.Specs
{
    public class OfferRepositoryInMemory : IOfferRepository
    {
        private IEnumerable<IOffer> _offers;

        public OfferRepositoryInMemory(IEnumerable<IOffer> offers)
        {
            _offers = offers;
        }

        public IEnumerable<IOffer> GetAll()
        {
            return _offers;
        }
    }
}
