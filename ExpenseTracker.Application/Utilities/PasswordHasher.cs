using System.Security.Cryptography;

namespace ExpenseTracker.Application.Utilities
{
    public static class PasswordHasher
    {
        private const int GetSaltSize = 16; 
        private const int GetHashSize = 32; 
        private const int GetIterations = 50000; 

        public static string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            byte[] salt = RandomNumberGenerator.GetBytes(GetSaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, GetIterations, HashAlgorithmName.SHA256, GetHashSize);
            
            byte[] hashBytes = new byte[GetSaltSize + GetHashSize];
            Array.Copy(salt, 0, hashBytes, 0, GetSaltSize);
            Array.Copy(hash, 0, hashBytes, GetSaltSize, GetHashSize);
            
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string password, string hash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hash))
                return false;

            try
            {
                byte[] hashBytes = Convert.FromBase64String(hash);
                
                if (hashBytes.Length != GetSaltSize + GetHashSize)
                    return false;

                byte[] salt = hashBytes[..GetSaltSize];
                byte[] storedHash = hashBytes[GetSaltSize..];

                byte[] computedHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, GetIterations, HashAlgorithmName.SHA256, GetHashSize);
                
                return CryptographicOperations.FixedTimeEquals(storedHash, computedHash);
            }
            catch
            {
                return false;
            }
        }
    }
}
