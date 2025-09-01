using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.WindowsService.Utility
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
        public static double ConvertMinutesToMilliseconds(double minutes)
        {
            return TimeSpan.FromMinutes(minutes).TotalMilliseconds;
        }
    }
}
