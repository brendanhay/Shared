using System;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Web.Security;

namespace Infrastructure
{
    public interface ICryptoService
    {
        string CreateSalt();

        string CreateSalt(int size);

        string CreatePasswordHash(string password, string email, string salt);
    }

    public sealed class CryptoService : ICryptoService
    {
        public const int DEFAULT_SALT_SIZE = 12;

        private static readonly RNGCryptoServiceProvider _random = new RNGCryptoServiceProvider();

        private static readonly ICryptoService _instance = new CryptoService();

        public static ICryptoService Instance { get { return _instance; } }

        static CryptoService() { }

        private CryptoService() { }

        #region ICryptoService Members

        public string CreateSalt()
        {
            return CreateSalt(DEFAULT_SALT_SIZE);
        }

        public string CreateSalt(int size)
        {
            var buffer = new byte[size];

            _random.GetBytes(buffer);

            return Convert.ToBase64String(buffer);
        }

        public string CreatePasswordHash(string password, string email, string salt)
        {
            var saltedPassword = string.Join("", salt, password, email);

            return FormsAuthentication.HashPasswordForStoringInConfigFile(saltedPassword,
                FormsAuthPasswordFormat.SHA1.ToString());
        }

        #endregion
    }
}
