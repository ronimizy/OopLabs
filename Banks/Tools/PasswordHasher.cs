using System.Security.Cryptography;
using System.Text;
using Utility.Extensions;

namespace Banks.Tools
{
    public static class PasswordHasher
    {
        private static readonly SHA1CryptoServiceProvider Provider = new SHA1CryptoServiceProvider();

        public static string Hash(string password)
        {
            password.ThrowIfNull(nameof(password));

            byte[] data = Encoding.UTF8.GetBytes(password);
            byte[] hashed = Provider.ComputeHash(data);

            return Encoding.UTF8.GetString(hashed);
        }
    }
}