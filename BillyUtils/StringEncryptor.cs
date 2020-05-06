using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace BillyUtils
{
    public static class StringEncryptor
    {
        private const int KeySize = 256;

        private static byte[] Get256BitKey(string passphrase)
        {
            var result = Encoding.UTF8.GetBytes(passphrase);
            List<byte> temp = new List<byte>(result);
            for (int i = result.Length; i < KeySize / 8; i++)
                temp.Add(0);
            result = temp.ToArray();
            return result;
        }

        private static void LogArray(byte[] input)
        {
            foreach (var _ in input)
                Console.Write(_.ToString() + " ");
        }

        public static string Encrypt(string data, string passphrase)
        {
            var dataBytes = Encoding.UTF8.GetBytes(data);
            var keyBytes = Get256BitKey(passphrase);
            byte[] encrypted;

            using (var myAes = Aes.Create())
            {
                myAes.GenerateIV();
                myAes.Key = keyBytes;
                myAes.Padding = PaddingMode.PKCS7;
                ICryptoTransform encryptor = myAes.CreateEncryptor();
                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cryptoStream))
                        {
                            sw.Write(data);
                        }
                        encrypted = memoryStream.ToArray();
                    }
                }
                encrypted = encrypted.Concat(myAes.IV).ToArray();
            }
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string data, string passphrase)
        {
            var dataBytesWithIV = Convert.FromBase64String(data);
            var iv = dataBytesWithIV.Skip(dataBytesWithIV.Length - KeySize / 16).Take(KeySize / 16).ToArray();
            var cipher = dataBytesWithIV.Take(dataBytesWithIV.Length - KeySize / 16).ToArray();
            var keyBytes = Get256BitKey(passphrase);
            string result = "";
            using (var myAes = Aes.Create())
            {
                myAes.Key = keyBytes;
                myAes.IV = iv;
                myAes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = myAes.CreateDecryptor(myAes.Key, myAes.IV);
                using (var memoryStream = new MemoryStream(cipher))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        var tmp = new byte[cipher.Length];
                        var lenth = cryptoStream.Read(tmp, 0, tmp.Length);

                        result = Encoding.UTF8.GetString(tmp, 0, lenth);

                        cryptoStream.Close();
                        memoryStream.Close();
                    }
                }
            }
            return result;
        }
    }
}