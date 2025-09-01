using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DocRepository.Provider.DB
{
    class AesWrapper
    {
        public static byte[] encrypt(byte[] plain, string sKey, string sIV)
        {
            byte[] encrypted;
            byte[] Key = Encoding.Unicode.GetBytes(sKey);
            byte[] IV = Encoding.Unicode.GetBytes(sIV);

            using (MemoryStream mstream = new MemoryStream())
            {
                using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(mstream,
                        aesProvider.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plain, 0, plain.Length);
                    }
                }
                encrypted = mstream.ToArray();
            }
            return encrypted;
        }

        public static byte[] decrypt(byte[] encrypted, string sKey, string sIV)
        {
            byte[] plain;
            byte[] Key = Encoding.Unicode.GetBytes(sKey);
            byte[] IV = Encoding.Unicode.GetBytes(sIV);
            using (MemoryStream mStream = new MemoryStream(encrypted))
            {
                using (var decryptedStream = new MemoryStream())
                {
                    using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(mStream,
                            aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                        {
                            using (StreamReader stream = new StreamReader(cryptoStream))
                            {
                                int data;
                                while ((data = cryptoStream.ReadByte()) != -1)
                                    decryptedStream.WriteByte((byte)data);
                            }
                        }
                    }
                    decryptedStream.Position = 0;
                    plain = decryptedStream.ToArray();
                }
            }
            return plain;
        }
    }
}
