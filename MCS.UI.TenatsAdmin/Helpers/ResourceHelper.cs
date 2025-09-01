using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Framework.Localization;

namespace MCS.UI.TenantsAdmin
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

        private static string GetStatucCodeValue(string messageKey)
        {
            return StatusCode.ResourceManager.GetString(messageKey);
        }
    }
}