using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MCS.Framework.Notifications
{
    /// <summary>
    /// Helper class contain common functionality.
    /// </summary>
    public static class Helper
    {
        /// <summary>
        /// Check if context contain any english letter.
        /// </summary>
        /// <param name="context">string message.</param>
        /// <returns>return true if context conatin any english letter. </returns>
        public static bool CheckEnglishChars(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return false;
            }

            Regex regex = new Regex("^[0-9a-zA-Z]+$");
            return regex.IsMatch(context);
        }

        /// <summary>
        /// Check if context contain any arabic letter.
        /// </summary>
        /// <param name="context">string message.</param>
        /// <returns>return true if context conatin any arabic letter. </returns>
        public static bool CheckIfContainArabicChars(string context)
        {
            if (string.IsNullOrEmpty(context))
            {
                return false;
            }

            Regex regex = new Regex("[\u0600-\u06ff]|[\u0750-\u077f]|[\ufb50-\ufc3f]|[\ufe70-\ufefc]");
            return regex.IsMatch(context);
        }

        /// <summary>
        ///  Converts the string representation of a enumValue string to its enumeration equivalent. 
        ///  A return value indicates whether the operation succeeded.
        /// </summary>
        /// <typeparam name="T">Strongly typed to any enumeration type.</typeparam>
        /// <param name="enumValue">A string containing the name or value to convert.</param>
        /// <param name="outputEnum">An object of type T whose value is represented by enumValue.</param>
        /// <returns>True if conversion succeeded, flase if there is no equivelent enum value.</returns>
        internal static bool EnumTryParse<T>(string enumValue, out T outputEnum)
        {
            string strTypeFixed = enumValue.Replace(' ', '_');
            if (Enum.IsDefined(typeof(T), strTypeFixed))
            {
                outputEnum = (T)Enum.Parse(typeof(T), strTypeFixed, true);
                return true;
            }
            else
            {
                foreach (string value in Enum.GetNames(typeof(T)))
                {
                    if (value.Equals(strTypeFixed, StringComparison.OrdinalIgnoreCase))
                    {
                        outputEnum = (T)Enum.Parse(typeof(T), value);
                        return true;
                    }
                }

                outputEnum = default(T);
                return false;
            }
        }       
    }
}
