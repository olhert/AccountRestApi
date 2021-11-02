using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using AccountRestApi.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AccountRestApi
{
    public class EncodeDecode
    {
         public class AesEncodeDecode
    {
         public static byte[] Encode(byte[] data, byte[] key)
        {
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(key), 0, aesKey, 0, 24);

            using var aes = Aes.Create();
            
            if (aes == null)
                throw new ArgumentException("Parameter must not be null.", nameof(aes));

            aes.Key = aesKey;

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var resultStream = new MemoryStream();
            using (var aesStream = new CryptoStream(resultStream, encryptor, CryptoStreamMode.Write))
            {
                using var plainStream = new MemoryStream(data);
                plainStream.CopyTo(aesStream);
            }

            var result = resultStream.ToArray();
            var combined = new byte[aes.IV.Length + result.Length];
            Array.ConstrainedCopy(aes.IV, 0, combined, 0, aes.IV.Length);
            Array.ConstrainedCopy(result, 0, combined, aes.IV.Length, result.Length);

            return combined;
        }

        public static byte[] Decode(byte[] encryptedData, byte[] key)
        {
            var buffer = new byte[encryptedData.Length];
            var hash = new SHA512CryptoServiceProvider();
            var aesKey = new byte[24];
            Buffer.BlockCopy(hash.ComputeHash(key), 0, aesKey, 0, 24);

            using var aes = Aes.Create();
            if (aes == null)
                throw new ArgumentException("Parameter must not be null.", nameof(aes));

            aes.Key = aesKey;

            var iv = new byte[aes.IV.Length];
            var ciphertext = new byte[buffer.Length - iv.Length];

            Array.ConstrainedCopy(encryptedData, 0, iv, 0, iv.Length);
            Array.ConstrainedCopy(encryptedData, iv.Length, ciphertext, 0, ciphertext.Length);

            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var resultStream = new MemoryStream();
            using (var aesStream = new CryptoStream(resultStream, decryptor, CryptoStreamMode.Write))
            {
                using var plainStream = new MemoryStream(ciphertext);
                plainStream.CopyTo(aesStream);
            }

            return resultStream.ToArray();
        }

        public void DecodeUser(string baseString, string key)
        {
            var fromBaseToBytes = Convert.FromBase64String(baseString);
            var decodedDataBytes = EncodeDecode.AesEncodeDecode.Decode(fromBaseToBytes, Encoding.ASCII.GetBytes(key));
            var decodedString = Encoding.ASCII.GetString(decodedDataBytes);

            var decodeUser = AuthToken.FromJson(decodedString);
        }

    } 
    }
    
    
}