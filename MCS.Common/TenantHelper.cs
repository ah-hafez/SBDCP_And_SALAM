using System;
using System.Configuration;
using MCS.Framework.Cache;
using MCS.Framework.MultiTenants;
using MCS.Framework.Web;

namespace MCS.Common
{
    public static class TenantHelper
    {
        public static string GetTenantDatabaseNameFromHeader()
        {
            return HttpContextHelper.GetHeaderValue(Constants.TenantDatabaseName);
        }

        public static string GetECMProfileIdFromHeader()
        {
            return HttpContextHelper.GetHeaderValue(Constants.ECMProfileId);
        }

        public static string GetECMCategoryIdFromHeader()
        {
            return HttpContextHelper.GetHeaderValue(Constants.ECMCategoryId);
        }
    }
}
