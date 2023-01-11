using System.Security.Cryptography;
using System.Text;

namespace Pokedex.Business.Core.Auth
{
    public static class CryptographyHelper
    {
        public static string EncryptToMD5(string text)
        {
            var hash = MD5.HashData(Encoding.UTF8.GetBytes(text));
            return hash.ToHexadecimal();
        }

        private static string ToHexadecimal(this byte[] bytes)
        {
            return string.Join(string.Empty, bytes.Select(b => b.ToString("x2")));
        }
    }
}