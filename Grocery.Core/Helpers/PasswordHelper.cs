using System.Security.Cryptography;
using System.Text;

namespace Grocery.Core.Helpers
{
    public static class PasswordHelper
    {
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100000, HashAlgorithmName.SHA256, 32);
            return Convert.ToBase64String(salt) + "." + Convert.ToBase64String(hash);
        }

        public static bool VerifyPassword(string password, string storedHash)
        {
            var parts = storedHash.Split('.');
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var hash = Convert.FromBase64String(parts[1]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), salt, 100000, HashAlgorithmName.SHA256, 32);

            return CryptographicOperations.FixedTimeEquals(inputHash, hash);
        }

        public static bool ValidatePasswordComplexity(string password)
        {
            // Check if password is between minimum and maximum required length
            if (password.Length < 8 || password.Length > 255) return false;

            // Password must contain number
            if (!password.Any(char.IsDigit)) return false;

            // Password must contain uppercase and lowercase letter
            if (!password.Any(char.IsLower) || !password.Any(char.IsUpper)) return false;

            // Password must contain special character
            if (!password.Any(ch => char.IsSymbol(ch) || char.IsPunctuation(ch))) return false;

            // Password may not contain whitespace
            if (password.Any(char.IsWhiteSpace)) return false;

            return true;
        }
    }
}
