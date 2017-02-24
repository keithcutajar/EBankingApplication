using System;
using System.Text;

namespace SecureKeyApp
{
    public sealed class SecureKey
    {
        public ushort GetSecureKey(string username, string password)
        {
            var date = DateTime.Now.ToString("g");
            var details = string.Concat(username, password, date);
            return GetCode(details);
        }

        private static ushort GetCode(string details)
        {
            ushort secureKey = 0;
            if (string.IsNullOrEmpty(details))
            {
                return secureKey;
            }

            var byteContent = Encoding.Unicode.GetBytes(details);
            var hash = new System.Security.Cryptography.SHA256CryptoServiceProvider();
            var hashText = hash.ComputeHash(byteContent);

            var hashCodeStart = BitConverter.ToInt64(hashText, 0);
            var hashCodeMedium = BitConverter.ToInt64(hashText, 8);
            var hashCodeEnd = BitConverter.ToInt64(hashText, 24);
            var hashCode = hashCodeStart ^ hashCodeMedium ^ hashCodeEnd;

            secureKey = ushort.Parse(Math.Abs(hashCode).ToString().Substring(0, 4));
            return secureKey;
        }

    }
}
