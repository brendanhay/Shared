using System;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Web.Security;

namespace Infrastructure
{
    public static class CryptoHelper
    {
        public const int DEFAULT_SALT_SIZE = 12;

        private static readonly RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();

        public static string CreateSalt()
        {
            return CreateSalt(DEFAULT_SALT_SIZE);
        }

        public static string CreateSalt(int size)
        {
            var buffer = new byte[size];

            _random.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        public static string CreatePasswordHash(string password, string email, string salt)
        {
            var saltedPassword = string.Join("", salt, password, email);

            return FormsAuthentication.HashPasswordForStoringInConfigFile(saltedPassword,
                FormsAuthPasswordFormat.SHA1.ToString());
        }
    }
}
