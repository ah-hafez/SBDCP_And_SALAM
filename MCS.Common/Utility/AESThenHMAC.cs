using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Common.Utility
{
    public class AESThenHMAC
    {
        public static bool IsValidateKey(string StringToEncrypt, string signiture, string encryptionKey)
        {
            var reuqestKey = GeneratPKey(StringToEncrypt, encryptionKey);
            return reuqestKey == signiture;
        }
        public static string GeneratPKey(string StringToEncrypt, string encryptionKey)
        {

            var newplan = encryptionKey + StringToEncrypt + encryptionKey;
            SHA512 sHA512 = System.Security.Cryptography.SHA512.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(newplan);
            byte[] hash = sHA512.ComputeHash(inputBytes);

            // CONVERT BYTE ARRAY TO HEX STRING
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

    }
}
