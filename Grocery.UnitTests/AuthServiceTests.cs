using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Grocery.UnitTests.Mocks;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Grocery.UnitTests
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;

        // Do "Arrange" section of unit tests in constructor to avoid code duplication.
        public AuthServiceTests()
        {
            IClientRepository clientRepository = new MockClientRepository();
            IClientService clientService = new ClientService(clientRepository);
            _authService = new AuthService(clientService);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsClient()
        {
            // Act
            Client? c = _authService.Login("user1@mail.com", "user1");
            // Assert
            Assert.NotNull(c);
        }

        [Fact]
        public void Login_UnusedEmail_ReturnsNull()
        {
            // Act
            Client? c = _authService.Login("user5@mail.com", "user2");
            // Assert
            Assert.Null(c);
        }

        [Theory]
        [InlineData("user1@mail.com", "", "Empty password with valid email")]
        [InlineData("", "user1", "Empty email")]
        [InlineData("", "", "Both fields empty")]
        public void Login_EmptyFields_ReturnsNull(string email, string password, string message)
        {
            // Act
            Client? c = _authService.Login(email, password);
            // Assert
            Assert.True(c == null, message);
        }

        [Theory]
        [InlineData("user1 ", "Valid password with trailing space")]
        [InlineData(" user1", "Valid password with leading space")]
        [InlineData("user", "Part of a valid password")]
        [InlineData("user2", "Password valid for other user")]
        public void Login_InvalidPasswords_ReturnsNull(string password, string message)
        {
            // Act
            Client? c = _authService.Login("user1@mail.com", password);
            // Assert
            Assert.True(c == null, message);
        }
    }
}