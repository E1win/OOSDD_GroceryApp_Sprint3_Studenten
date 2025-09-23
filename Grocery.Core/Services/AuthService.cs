using Grocery.Core.Helpers;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;

namespace Grocery.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IClientService _clientService;
        public AuthService(IClientService clientService)
        {
            _clientService = clientService;
        }
        public Client? Login(string email, string password)
        {
            Client? client = _clientService.Get(email);
            if (client == null) return null;
            if (PasswordHelper.VerifyPassword(password, client.Password)) return client;
            return null;
        }

        public Client? Register(string email, string password, string name)
        {
            // Check if all fields are filled.
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(password)) return null;

            // Check if email does not already exist.
            if (_clientService.Get(email) != null) return null;

            // Verify email if email is valid
            if (!EmailHelper.ValidateEmail(email)) return null;

            // Verify if password has required complexity
            if (!PasswordHelper.ValidatePasswordComplexity(password)) return null;

            // Create the new client
            Client c = _clientService.Create(email, PasswordHelper.HashPassword(password), name);

            return c;
        }
    }
}
