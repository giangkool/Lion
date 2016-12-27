using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lion.Sercurity
{
    /// <summary>
    /// Tên Lơp: Encryption
    /// Chức Năng: Mã hóa, giải mã theo chuẩn RSA
    /// </summary>
    public class Encryption
    {
        private static bool _AsymmetricEncryption = false;

        /// <summary>
        /// Hàm tạo key
        /// </summary>
        /// <param name="keySize"></param>
        /// <param name="publicKey"></param>
        /// <param name="publicAndPrivateKey"></param>
        public static void GenerateKeys(int keySize, out string publicKey, out string publicAndPrivateKey)
        {
            var provider = new RSACryptoServiceProvider(keySize);
            publicKey = provider.ToXmlString(false);
            publicAndPrivateKey = provider.ToXmlString(true);
        }

        /// <summary>
        /// Hàm mã hóa dữ liệu
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="keySize"></param>
        /// <param name="publicKeyXml"></param>
        /// <returns></returns>
        public static string EncryptText(string inputText, int keySize, string publicKeyXml)
        {
            var provider = new RSACryptoServiceProvider(keySize);
            provider.FromXmlString(publicKeyXml);
            byte[] inputdata = Encoding.UTF8.GetBytes(inputText);
            var encryptext = provider.Encrypt(inputdata, false);
            var encrypted = encryptext;
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Hàm giải mã dữ liệu
        /// </summary>
        /// <param name="inputText"></param>
        /// <param name="keySize"></param>
        /// <param name="publicAndPrivateKeyXml"></param>
        /// <returns></returns>
        public static string DecryptText(string inputText, int keySize, string publicAndPrivateKeyXml)
        {
            var provider = new RSACryptoServiceProvider(keySize);
            provider.FromXmlString(publicAndPrivateKeyXml);
            byte[] inputdata = Convert.FromBase64String(inputText);
            var decryptext = provider.Decrypt(inputdata, false);
            var decrypted = decryptext;
            return Encoding.UTF8.GetString(decrypted);
        }
    }
}
