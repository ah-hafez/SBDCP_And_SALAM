using System;
using System.Text;

namespace MCS.Common.Utility
{
    public class ArabicDigitConverter
    {
        public static string ConvertToArabic(string sIn)
        {
            if (sIn.ToString().Contains("/"))
            {
                sIn = GetFormattedDate(sIn.ToString());
            }

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            System.Text.Decoder utf8Decoder = null;
            utf8Decoder = enc.GetDecoder();
            StringBuilder sTranslated = new System.Text.StringBuilder();
            char[] cTransChar = new char[2];
            byte[] bytes = { 217, 160 };

            // Start Converting characters into Arabic mode.
            char[] aChars = sIn.ToCharArray();
            foreach (char chr in aChars)
            {
                if (char.IsDigit(chr))
                {
                    bytes[1] = Byte.Parse((160 + Convert.ToInt32(char.GetNumericValue(chr))).ToString());
                    utf8Decoder.GetChars(bytes, 0, 2, cTransChar, 0);
                    sTranslated.Append(cTransChar[0].ToString());
                }
                else
                {
                    sTranslated.Append(chr.ToString());
                }
            }

            return sTranslated.ToString();
        }

        private static string GetFormattedDate(string sDate)
        {
            string sTempDate = sDate;
            try
            {
                if (sDate.Length >= 8 && sDate.Length <= 10)
                {
                    string[] sArrDate = sDate.Split('/');
                    if (sArrDate.Length.Equals(3) && sArrDate[2].Contains("14"))
                    {
                        sTempDate = sArrDate[2] + "/" + sArrDate[1] + "/" + sArrDate[0];
                    }
                }
            }
            catch
            {
                sTempDate = sDate;
            }

            return sTempDate;
        }
    }
}
