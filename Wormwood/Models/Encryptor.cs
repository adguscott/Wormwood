using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace Wormwood.Models
{
    public static class Encryptor
    {
        public static byte[] CreateKeyFromPassword(string password)
        {
            byte[] encodedPassword = System.Text.Encoding.UTF8.GetBytes(password);
            using var sha = SHA256.Create();
            return sha.ComputeHash(encodedPassword);
        }

        public static void Encrypt(string sourceFile, string destinationFile, byte[] key)
        {
            using var sourceStream = File.OpenRead(sourceFile);
            using var destinationStream = File.Create(destinationFile);
            using var provider = new AesCryptoServiceProvider();
            provider.GenerateIV();

            using var cryptoTransform = provider.CreateEncryptor(key, provider.IV);
            using var cryptoStream = new CryptoStream(destinationStream, cryptoTransform, CryptoStreamMode.Write);
            destinationStream.Write(provider.IV, 0, provider.IV.Length);
            try
            {
                sourceStream.CopyTo(cryptoStream);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void Decrypt(string sourceFile, string destinationFile, byte[] key)
        {
            using var sourceStream = File.OpenRead(sourceFile);
            using var destinationStream = File.Create(destinationFile);
            using var provider = new AesCryptoServiceProvider();
            var IV = new byte[provider.IV.Length];
            sourceStream.Read(IV, 0, IV.Length);
            using var cryptoTransform = provider.CreateDecryptor(key, IV);
            using var cryptoStream = new CryptoStream(sourceStream, cryptoTransform, CryptoStreamMode.Read);
            {
                cryptoStream.CopyTo(destinationStream);
            }


        }
    }
}
