using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MCS.Framework.ObjectExtensions
{
    public static class StringExtentions
    {
        public static SecureString ToSecureString(this string plainText)
        {
            var secureStr = new SecureString();
            if (plainText.Length > 0)
            {
                foreach (var c in plainText.ToCharArray()) secureStr.AppendChar(c);
            }
            return secureStr;
        }

        public static string ToUNSecureString(this SecureString secureText)
        {
            IntPtr unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(secureText);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }
    }
}
