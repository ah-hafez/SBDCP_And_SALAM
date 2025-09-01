using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Common.Utility
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
        public static int TryGetIntValue(this string str, int defualtValue = 0)
        {
            int result = defualtValue;
            if (str.IsNullOrEmpty() == false)
            {
                int.TryParse(str, out result);
            }
            return result;
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
