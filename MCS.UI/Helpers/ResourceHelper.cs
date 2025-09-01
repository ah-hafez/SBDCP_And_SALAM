using Resources;
using System.Globalization;
using MCS.Framework.Localization;

namespace MCS.UI
{
    public class ResourceHelper
    {
        public static string GetResourceValue(ResourceSet resourceSet, string messageKey)
        {
            switch (resourceSet)
            {
                case ResourceSet.StatusCode:
                    return GetStatucCodeValue(messageKey);
                case ResourceSet.Message:
                    return DbRes.TResource(messageKey);
            }

            return string.Empty;
        }
        public static string GetResourceValue(ResourceSet resourceSet, string messageKey, string cultureName)
        {
            return GetStatucCodeValue(messageKey, cultureName);
        }
        private static string GetStatucCodeValue(string messageKey)
        {
            return StatusCode.ResourceManager.GetString(messageKey);
        }
        private static string GetStatucCodeValue(string messageKey, string cultureName)
        {
            cultureName = cultureName.Replace("JO", "US");
            CultureInfo cultureInfo = new CultureInfo(cultureName);
            return StatusCode.ResourceManager.GetString(messageKey, cultureInfo);
        }
    }
}