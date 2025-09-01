using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MCS.Framework.Encryption
{
    public class AESEncrytDecry
    {
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 32;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            try
            {
                // Check arguments.  
                if (plainText == null || plainText.Length <= 0)
                {
                    throw new ArgumentNullException("plainText");
                }
                if (key == null || key.Length <= 0)
                {
                    throw new ArgumentNullException("key");
                }
                if (iv == null || iv.Length <= 0)
                {
                    throw new ArgumentNullException("key");
                }
                byte[] encrypted;
                // Create a RijndaelManaged object  
                // with the specified key and IV.  
                using (var rijAlg = new RijndaelManaged())
                {
                    rijAlg.Mode = CipherMode.CBC;
                    rijAlg.Padding = PaddingMode.PKCS7;
                    rijAlg.FeedbackSize = 32;

                    rijAlg.Key = key;
                    rijAlg.IV = iv;

                    // Create a decrytor to perform the stream transform.  
                    var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                    // Create the streams used for encryption.  
                    using (var msEncrypt = new MemoryStream())
                    {
                        using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                //Write all data to the stream.  
                                swEncrypt.Write(plainText);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }

                // Return the encrypted bytes from the memory stream.  
                return encrypted;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static string DecryptStringAES(string cipherText)
        {
            return cipherText;
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }
        public static string EncryptStringAES(string plainText)
        {
            return plainText;
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");

            var encrypted = EncryptStringToBytes(plainText, keybytes, iv);
            return Convert.ToBase64String(encrypted);
        }
        public static string EncryptData(string plainText)
        {
            return plainText;
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindByThumbprint, ConfigurationManager.AppSettings["EncryptionCertificateThumbprint"], false)[0];
            store.Close();

            // GetRSAPublicKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            byte[] byteArray = Encoding.UTF8.GetBytes(plainText);
            using (RSA rsa = cert.GetRSAPublicKey())
            {
                // OAEP allows for multiple hashing algorithms, what was formermly just "OAEP" is
                // now OAEP-SHA1.
                return Convert.ToBase64String(rsa.Encrypt(byteArray, RSAEncryptionPadding.Pkcs1));
            }
        }
        public static string DecryptData(string cipherText)
        {
            return cipherText;
               X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            var cert = store.Certificates.Find(X509FindType.FindByThumbprint, ConfigurationManager.AppSettings["EncryptionCertificateThumbprint"], false)[0];
            // GetRSAPrivateKey returns an object with an independent lifetime, so it should be
            // handled via a using statement.
            byte[] byteArray = Convert.FromBase64String(cipherText);
            using (RSA rsa = cert.GetRSAPrivateKey())
            {
                return Encoding.UTF8.GetString(rsa.Decrypt(byteArray, RSAEncryptionPadding.Pkcs1));
            }
        }
        public static string Base64Encode(string plainText)
        {
            return plainText;
            //var cipherText = Encoding.UTF8.GetBytes(plainText);
            //return Convert.ToBase64String(cipherText);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptStringAES(plainText)));
        }
        public static string Base64Decode(string cipherText)
        {
            return cipherText;
            //byte[] plainTextBytes = Convert.FromBase64String(cipherText);
            //return Encoding.UTF8.GetString(plainTextBytes);
            return DecryptStringAES(Encoding.UTF8.GetString(Convert.FromBase64String(cipherText)));
        }
    }
}
