using Grocery.Core.Models;

namespace Grocery.Core.Interfaces.Services
{
    public interface IClientService
    {
        public Client? Get(string email);

        public Client? Get(int id);

        public Client Create(string email, string password, string name);

        public List<Client> GetAll();
    }
}
