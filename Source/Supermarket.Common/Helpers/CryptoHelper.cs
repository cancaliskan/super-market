using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Supermarket.Common.Helpers
{
    public class CryptoHelper
    {
        public int GetRandomNumber()
        {
            var byteArray = new byte[4];
            var provider = new RNGCryptoServiceProvider();
            provider.GetBytes(byteArray);

            var randomNumber = BitConverter.ToInt32(byteArray, 0);
            return Math.Abs(randomNumber);
        }

        public byte[] GetRandomData(int bits)
        {
            var result = new byte[bits / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(result);
            }

            return result;
        }

        public byte[] GetSalt()
        {
            return GetRandomData(128);
        }

        public string GetSaltAsString()
        {
            return Convert.ToBase64String(GetSalt());
        }

        public byte[] GetKey()
        {
            return GetRandomData(256);
        }

        public string GetKeyAsString()
        {
            return Convert.ToBase64String(GetKey());
        }

        public byte[] GetIV()
        {
            return GetRandomData(128);
        }

        public string GetIVAsString()
        {
            return Convert.ToBase64String(GetIV());
        }

        public string ConvertToString(byte[] text)
        {
            return Convert.ToBase64String(text);
        }

        public byte[] ConvertToByteArray(string text)
        {
            return Convert.FromBase64CharArray(text.ToCharArray(), 0, text.Length);
        }

        public virtual string Hash(string text, string salt)
        {
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(text, ConvertToByteArray(salt), KeyDerivationPrf.HMACSHA512, 28657, 256 / 8));
            return hashed;
        }

        public string Encrypt(string text, string keyString, string ivString)
        {
            var key = ConvertToByteArray(keyString);
            var iv = ConvertToByteArray(ivString);

            return Encrypt(text, key, iv);
        }

        public string Encrypt(string text, byte[] key, byte[] iv)
        {
            ValidateParameters(text, key, iv);

            var textInBytes = ConvertToByteArray(Convert.ToBase64String(Encoding.UTF8.GetBytes(text)));
            byte[] result;
            using (var aes = Aes.Create())
            {
                ValidateAesInstanceCreation(aes);

                aes.Key = key;
                aes.IV = iv;

                using (var encryptor = aes.CreateEncryptor(key, iv))
                {
                    using (var to = new MemoryStream())
                    {
                        using (var writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
                        {
                            writer.Write(textInBytes, 0, textInBytes.Length);
                            writer.FlushFinalBlock();
                            result = to.ToArray();
                        }
                    }
                }

                aes.Clear();
            }

            return ConvertToString(result);
        }

        public string Decrypt(string text, byte[] key, byte[] iv)
        {
            ValidateParameters(text, key, iv);

            var textInBytes = ConvertToByteArray(text);
            byte[] result;
            int decryptedByteCount;
            using (var aes = Aes.Create())
            {
                ValidateAesInstanceCreation(aes);

                aes.Key = key;
                aes.IV = iv;

                try
                {
                    using (var decryptor = aes.CreateDecryptor(key, iv))
                    {
                        using (var from = new MemoryStream(textInBytes))
                        {
                            using (var reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
                            {
                                result = new byte[textInBytes.Length];
                                decryptedByteCount = reader.Read(result, 0, result.Length);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return string.Empty;
                }

                aes.Clear();
            }

            return Encoding.UTF8.GetString(result, 0, decryptedByteCount);
        }

        public string Decrypt(string text, string keyString, string ivString)
        {
            var key = ConvertToByteArray(keyString);
            var iv = ConvertToByteArray(ivString);

            return Decrypt(text, key, iv);
        }

        private static void ValidateParameters(string text, byte[] key, byte[] iv)
        {
            if (text == null || text.Length <= 0)
            {
                throw new ArgumentNullException(nameof(text));
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException(nameof(iv));
            }
        }

        private static void ValidateAesInstanceCreation(Aes aes)
        {
            if (aes == null)
            {
                throw new Exception("Crypto algorithm not created!");
            }
        }
    }
}