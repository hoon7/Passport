using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Passport
{
    class AES256
    {
        // Coded By retn0@naver.com
        // Original Source Code : http://www.codeproject.com/Articles/769741/Csharp-AES-bits-Encryption-Library-with-Salt

        static byte[] keyBytes = null;
        static byte[] saltBytes = null;     // The salt bytes must be at least 8 bytes.

        public byte[] KeyBytes { set { keyBytes = SHA256.Create().ComputeHash(value); } }
        public byte[] SaltBytes { set { saltBytes = value; } }
        public string KeyString { set { keyBytes = SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(value)); } }
        public string SaltString { set { saltBytes = Encoding.UTF8.GetBytes(value); } }

        // Encrypt Byte Array
        public byte[] EncryptByte(byte[] plainBytes)
        {
            byte[] encryptedBytes = null;

            using (MemoryStream MS = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var CS = new CryptoStream(MS, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        CS.Write(plainBytes, 0, plainBytes.Length);
                        CS.Close();
                    }

                    encryptedBytes = MS.ToArray();
                }
            }

            return encryptedBytes;
        }

        // Decrypt Byte Array
        public byte[] DecryptByte(byte[] cipherBytes)
        {
            byte[] decryptedBytes = null;

            using (MemoryStream MS = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(keyBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var CS = new CryptoStream(MS, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        CS.Write(cipherBytes, 0, cipherBytes.Length);
                        CS.Close();
                    }

                    decryptedBytes = MS.ToArray();
                }
            }

            return decryptedBytes;
        }
        
        // Encrypt String
        public string EncryptString(string plainString)
        {
            return Convert.ToBase64String(EncryptByte(Encoding.UTF8.GetBytes(plainString)));
        }

        // Decrypt String
        public string DecryptString(string cipherString)
        {
            try { return Encoding.UTF8.GetString(DecryptByte(Convert.FromBase64String(cipherString))); } catch { return null; }
        }
    }
}