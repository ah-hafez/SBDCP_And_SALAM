using System.Web.Mvc;
using MCS.UI.Common;

namespace MCS.UI
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            //filters.Add(new CustomAuthorizationAttribute());
            filters.Add(new ValidateAntiForgeryTokenOnPost());
        }
    }
}