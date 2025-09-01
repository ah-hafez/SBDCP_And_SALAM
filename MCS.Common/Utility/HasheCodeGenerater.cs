using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Common.Utility
{
    public static class HasheCodeGenerater
    {
        public static string GenerateHashCode()
        {
            byte[] salt;
            byte[] buffer2;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var RandomString = new string(Enumerable.Repeat(chars, 4)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            //using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(RandomString, 0x10, 0x3e8))
            //{
            //    salt = bytes.Salt;
            //    buffer2 = bytes.GetBytes(0x20);
            //}
            //byte[] dst = new byte[0x31];
            //Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            //Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            //return Convert.ToBase64String(dst);
            return RandomString;
        }

        public static bool VerifyHashedCode(string HashedCode, string Code)
        {
            byte[] buffer4;
            if (HashedCode == null)
            {
                return false;
            }
            if (Code == null)
            {
                throw new ArgumentNullException("Code Is Null");
            }
            byte[] src = Convert.FromBase64String(HashedCode);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(Code, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }

    }
}
