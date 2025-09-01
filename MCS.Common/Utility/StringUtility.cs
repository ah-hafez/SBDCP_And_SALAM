using System;
using System.Text.RegularExpressions;

namespace MCS.Common
{
    public static class StringUtility
    {
        public static string ClearTokenInput(string token)
        {
            try
            {
                string str = Regex.Replace(token, @"[^\w\.@]$", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));

                return str.Length <= 40 ? str : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateStringInput(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"^[a-zA-Z0-9 ]*$", "",
                                       RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateUserNameInput(string strIn)
        {
            try
            {
                string str = Regex.Replace(strIn, @"[^\w\<>']$", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
                return str.Length <= 50 ? str : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateFileNames(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"^[^,\\//:._^]*$", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateGridDataTray(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"^[.]*$", "", RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateGridData(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"^[^.<>^]*$", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateBarcodeDesign(string strIn)
        {
            try
            {
                string str = Regex.Replace(strIn, @"^[^+@~^]*$", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));

                return str.Length <= 4000 ? str : string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static string ValidateId(string strIn)
        {
            try
            {
                return Regex.Replace(strIn, @"[^0-9]", "",
                                     RegexOptions.None, TimeSpan.FromSeconds(1.5));

            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string ValidateDate(string strIn)
        {
            try
            {
                if (Regex.IsMatch(strIn, @"^([0-2][0-9]|(3)[0-1])(\/)(([0-9])|((0)[0-9])|((1)[0-2]))(\/)\d{4}$"))
                {
                    return strIn;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
