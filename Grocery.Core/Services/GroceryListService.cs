using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class GroceryListService : IGroceryListService
    {
        private readonly IGroceryListRepository _groceryRepository;
        public GroceryListService(IGroceryListRepository groceryRepository)
        {
            _groceryRepository = groceryRepository;
        }
        public List<GroceryList> GetAll(int clientId)
        {
            return _groceryRepository.GetAll().Where(p => p.ClientId == clientId).ToList(); ;
        }
        public GroceryList Add(GroceryList item)
        {
            throw new NotImplementedException();
        }

        public GroceryList? Delete(GroceryList item)
        {
            throw new NotImplementedException();
        }

        public GroceryList? Get(int id)
        {
            return _groceryRepository.Get(id);
        }

        public GroceryList? Update(GroceryList item)
        {
            return _groceryRepository.Update(item);
        }
    }
}
