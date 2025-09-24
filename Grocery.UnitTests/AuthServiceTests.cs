using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Exceptions;
using Grocery.Core.Models;
using Grocery.Core.Services;
using Grocery.UnitTests.Mocks;
using Microsoft.Maui.ApplicationModel.Communication;

namespace Grocery.UnitTests
{
    public class AuthServiceTests
    {
        private readonly IAuthService _authService;

        private const string _validUsedEmail = "user1@mail.com";
        private const string _validUnusedEmail = "user5@mail.com";
        private const string _validPassword = "Useruser1!";
        private const string _validName = "Bob Bladerdeeg";

        // Do "Arrange" section of unit tests in constructor to avoid code duplication.
        public AuthServiceTests()
        {
            // Arrange
            IClientRepository clientRepository = new MockClientRepository();
            IClientService clientService = new ClientService(clientRepository);
            _authService = new AuthService(clientService);
        }

        [Fact]
        public void Login_ValidCredentials_ReturnsClient()
        {
            // Act
            Client? c = _authService.Login(_validUsedEmail, "user1");
            // Assert
            Assert.NotNull(c);
        }

        [Fact]
        public void Login_UnusedEmail_ReturnsNull()
        {
            // Act
            Client? c = _authService.Login(_validUnusedEmail, "user2");
            // Assert
            Assert.Null(c);
        }

        [Theory]
        [InlineData(_validUsedEmail, "", "Empty password with valid email")]
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
            Client? c = _authService.Login(_validUsedEmail, password);
            // Assert
            Assert.True(c == null, message);
        }

        [Theory]
        [InlineData("", _validPassword, _validName, "Empty email")]
        [InlineData(_validUnusedEmail, "", _validName, "Empty password")]
        [InlineData(_validUnusedEmail, _validPassword, "", "Empty name")]
        [InlineData("  ", _validPassword, _validName, "Email with only whitespace")]
        [InlineData(_validUnusedEmail, "     ", _validName, "Password with only whitespace")]
        [InlineData(_validUnusedEmail, _validPassword, "   ", "Name with only whitespace")]
        public void Register_EmptyFields_ThrowsException(string email, string password, string name, string message)
        {
            // Act & Assert
            var ex = Record.Exception(() => _authService.Register(email, password, name));
            Assert.True(ex is ArgumentException, message);
        }

        [Theory]
        [InlineData(_validUsedEmail, "Used mail")]
        [InlineData($" {_validUsedEmail}", "Used mail with leading space")]
        [InlineData($"{_validUsedEmail} ", "Used mail with trailing space")]
        public void Register_UsedEmail_ThrowsException(string email, string message)
        {
            // Act & Assert
            var ex = Record.Exception(() => _authService.Register(email, _validPassword, _validName));
            Assert.True(ex is UsedEmailException, message);
        }

        [Theory]
        [InlineData("use", "No @ symbol and domain")]
        [InlineData("use@", "nothing behing the @")]
        [InlineData("user1@@mail.com", "Two @ symbols")]
        public void Register_InvalidEmail_ThrowsException(string email, string message)
        {
            // Act & Assert
            var ex = Record.Exception(() => _authService.Register(email, _validPassword, _validName));
            Assert.True(ex is InvalidEmailException, message);
        }

        [Theory]
        [InlineData("Aa1!", "Too short")]
        [InlineData("AAAAAAAA1!", "No lowercase")]
        [InlineData("aaaaaaa1!", "No uppercase")]
        [InlineData("AAAAaaaaa!", "No number")]
        [InlineData("AAAAAaaaa1", "No special character")]
        [InlineData(" AAAAAaaaa1!", "Contains space")]
        [InlineData("AAAAA  aaaa1!", "Contains tab")]
        public void Register_InvalidPassword_ThrowsException(string password, string message)
        {
            // Act & Assert
            var ex = Record.Exception(() => _authService.Register(_validUnusedEmail, password, _validName));
            Assert.True(ex is InvalidPasswordException, message);
        }

        [Fact]
        public void Register_ValidCredentials_ReturnsClient()
        {
            // Act
            Client? c = _authService.Register(_validUnusedEmail, _validPassword, _validName);
            // Assert
            Assert.NotNull(c);
        }
    }
}