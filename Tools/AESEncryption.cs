using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace android_photo_syncr.Tools
{
    class AESEncryption
    {

        private static byte[] key = new byte[] { 1,2,3,4 };
        private static byte[] vector = new byte[] { 5,6,7,8 };

        internal static string Encrypt(string plaintext)
        {
            var aes = Aes.Create();

            ICryptoTransform encryptor = aes.CreateEncryptor(key, vector);

            UTF8Encoding encoder = new UTF8Encoding();
            byte[] buffer = encoder.GetBytes(plaintext);

            MemoryStream encryptStream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(encryptStream, encryptor, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }
            var result = encryptStream.ToArray();

            return Convert.ToBase64String(result);
        }

        internal static string Decrypt(string encrypted)
        {
            var aes = Aes.Create();

            ICryptoTransform decryptor = aes.CreateDecryptor(key, vector);

            byte[] buffer = Convert.FromBase64String(encrypted);

            MemoryStream decryptStream = new MemoryStream();
            using (CryptoStream cs = new CryptoStream(decryptStream, decryptor, CryptoStreamMode.Write))
            {
                cs.Write(buffer, 0, buffer.Length);
            }

            UTF8Encoding encoder = new UTF8Encoding();
            return encoder.GetString(decryptStream.ToArray());
        }
    }
}