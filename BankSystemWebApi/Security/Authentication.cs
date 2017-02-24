using System;
using System.Security.Cryptography;

namespace BankSystemWebApi.Security
{
    public static class Authentication
    {
        public static string GetApiKey()
        {
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                var secretKeyByteArray = new byte[32];
                cryptoProvider.GetBytes(secretKeyByteArray);
                var apiKey = Convert.ToBase64String(secretKeyByteArray);
                return apiKey;
            }
        }

        public static string GetAppId()
        {
            return new Guid().ToString();
        }


    }
}