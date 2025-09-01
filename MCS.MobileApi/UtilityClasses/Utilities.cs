using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;


namespace MobileApi.UtilityClasses
{
    public class Utilities
    {
        public static string GetDateFormatString(string kDate, bool bIgnoreTime, bool bFullFormat)
        {
            if (kDate.Length >= 8)
            {
                string strDate = strDate = kDate.Substring(6, 2) + "/" + kDate.Substring(4, 2) + "/" + kDate.Substring(0, 4);

                if (bFullFormat)
                {
                    strDate += " هـ";
                }
                else if (bFullFormat)
                {
                    strDate += " م";
                }

                if (!bIgnoreTime)
                {
                    int nHijriHour = Convert.ToInt32(kDate.Substring(8, 2));
                    if (nHijriHour >= 12)
                    {
                        strDate += nHijriHour - 12 + ":" + kDate.Substring(10, 2) + " مساء ";
                    }
                    else
                    {
                        strDate += nHijriHour + ":" + kDate.Substring(10, 2) + " صباحا ";
                    }
                }


                return strDate;
            }
            else
                return kDate;
        }

        public static string GetDateString(string kHijriDate, DateTime kTime)
        {
            string strDate = string.Format("{0}{1}{2}", (kTime.Hour < 10) ? "0" + kTime.Hour.ToString() : kTime.Hour.ToString(),
                (kTime.Minute < 10) ? "0" + kTime.Minute.ToString() : kTime.Minute.ToString(),
                (kTime.Second < 10) ? "0" + kTime.Second.ToString() : kTime.Second.ToString());
            strDate = kHijriDate.Substring(0, 8) + strDate;
            return strDate;
        }

        public static string GetDateHijriString(string kDate, bool bIgnoreTime)
        {
            string strDate = "00010101000000";
            if (bIgnoreTime)
            {
                if (kDate.Split('/').Length > 1)
                {
                    string strDay = kDate.Split('/')[0].ToString();
                    string strMonth = kDate.Split('/')[1].ToString();
                    string strYear = kDate.Split('/')[2].ToString();
                    if ((strMonth.Length == 1) && (Convert.ToInt32(strMonth) < 10)) strMonth = "0" + strMonth;
                    if ((strDay.Length == 1) && (Convert.ToInt32(strDay) < 10)) strDay = "0" + strDay;
                    strDate = string.Format(strYear, "0000") + string.Format(strMonth, "00") + string.Format(strDay, "00") + "000000";
                }
            }
            return strDate;
        }

        public static DateTime FormatDateTimeNow()
        {
            return DateTime.Now;
        }

        public static string Hash(string ToHash)
        {
            // First we need to convert the string into bytes, which means using a text encoder.
            Encoder enc = System.Text.Encoding.ASCII.GetEncoder();

            // Create a buffer large enough to hold the string
            byte[] data = new byte[ToHash.Length];
            enc.GetBytes(ToHash.ToCharArray(), 0, ToHash.Length, data, 0, true);

            // This is one implementation of the abstract class MD5.
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] result = md5.ComputeHash(data);

            return BitConverter.ToString(result).Replace("-", "").ToLower();
        }

        public static string GenerateToken(string userName, DateTime dt)
        {
            string hash = userName + "|" + dt.ToString();
            return Hash(hash);
        }

        public static bool IsTokenTimedout(DateTime lastLoginDate)
        {
            int nTokenTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["TokenTimeoutInMinutes"]);
            DateTime dtNow = FormatDateTimeNow();
            return (lastLoginDate.AddMinutes(nTokenTimeout) >= dtNow);
        }

        public static bool AuthenticateToken(string token, string userName, DateTime lastLoginDate)
        {
            int nTokenTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["TokenTimeout"]);

            for (int i = 0; i <= nTokenTimeout; i++)
            {
                string generatedToken = GenerateToken(userName, lastLoginDate.AddMinutes(i));
                if (generatedToken.Equals(token))
                {
                    return true;
                }
            }

            return false;
        }

        public static byte[] ToByteArray(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static string GetIP4Address(string userHostAddress)
        {
            string IP4Address = String.Empty;

            foreach (IPAddress IPA in Dns.GetHostAddresses(userHostAddress))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    IP4Address = IPA.ToString();

                    if (IP4Address != "127.0.0.1")
                    {
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(IP4Address))
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
            }

            return IP4Address;
        }

        public static bool IsAuthenticated(string username, string pwd)
        {
            if (ConfigurationManager.AppSettings["EnableAuthenticationAgainstAD"].ToString() == "true")
            {
                string ldapDomainName = ConfigurationManager.AppSettings["LDAPDomainName"].ToString();
                string ldapServerName = ConfigurationManager.AppSettings["LDAPServerName"].ToString();
                string ldapUserName = ConfigurationManager.AppSettings["LDAPUserName"].ToString();
                string ldapPassword = ConfigurationManager.AppSettings["LDAPPassword"].ToString();

                try
                {
                    using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, ldapServerName, ldapUserName, ldapPassword))
                    {
                        return pc.ValidateCredentials(username, pwd);
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        internal static string FormatTextFileName(string transNo, DateTime dtDate)
        {
            string strFileName;
            strFileName = transNo + "_" + dtDate.ToString("yyyyMMddHHmmss") + ".txt";
            return strFileName;
        }

        static public string FormatHijriDateTime(string strHijriDate, string strTime)
        {
            strHijriDate = strHijriDate.Substring(0, 14);
            strHijriDate = strHijriDate.Replace(" ", "");
            string[] strHijri = strHijriDate.Split('/');
            string[] strHijriTime = strTime.Split(':');
            string strHijriDateTime = string.Format("{0}{1}{2}{3}{4}{5}",
                strHijri[0],
                strHijri[1],
                strHijri[2],
                strHijriTime[0],
                strHijriTime[1],
                "00");
            return strHijriDateTime;
        }
    }
}