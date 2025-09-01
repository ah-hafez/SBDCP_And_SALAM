using System;

namespace MobileAPIs.UtilityClasses
{
    public static class Extensions
    {
        public static bool TryGetBoolValue(this string str, bool defualtValue = false)
        {
            bool result = defualtValue;
            if (str.IsNullOrEmpty() == false)
            {
                bool.TryParse(str, out result);
            }
            return result;
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}