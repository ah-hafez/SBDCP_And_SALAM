using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Framework.Localization.SupportClasses
{
    public static class LocalizationExtensions
    {
        public static string LocalText<T>(this IEnumerable<T> source) where T : IText
        {
            T firstOrDefault = source.FirstOrDefault();
            string localText = string.Empty;

            if (firstOrDefault != null)
            {
                localText = firstOrDefault.Text;
            }

            return localText;
        }
    }
}
