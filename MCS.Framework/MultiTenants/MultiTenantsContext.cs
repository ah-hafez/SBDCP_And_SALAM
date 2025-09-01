using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MCS.Framework.MultiTenants
{
    //public class MultiTenantsContext : IMultiTenantsContext
    //{
    //    public static readonly string LoggedInTenantSessionVariable = "__LoggedInTenant";

    //    public TTenant GetLoggedInTenant<TTenant>() where TTenant : ITenant
    //    {
    //        ITenant tenant = TryGetTenantFromSession();

    //        return tenant != null ? (TTenant)tenant : default(TTenant);
    //    }       

    //    public static ITenant LoggedInTenant
    //    {
    //        get { return TryGetTenantFromSession(); }
    //    }

    //    public static void SetLoggedInTenantInWebSession(ITenant tenant)
    //    {
    //        HttpContext.Current.GetOwinContext().Set(LoggedInTenantSessionVariable, tenant);
    //    }

    //    private static ITenant TryGetTenantFromSession()
    //    {
    //        HttpContext context = HttpContext.Current;

    //        if (context != null)
    //            return (ITenant)HttpContext.Current.GetOwinContext().Get<ITenant>(LoggedInTenantSessionVariable);

    //        return null;
    //    }
    //}
}
