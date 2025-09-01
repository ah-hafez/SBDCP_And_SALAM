using System;
using System.Globalization;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Windows.Input;
using MCS.Common;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class HomeController : AdminControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public void SetCultureName(string cultureName)
        {
            try
            {
                CultureInfo cultureInfo = null;

                if (Constants.Languages.English == cultureName)
                {
                    cultureInfo = new CultureInfo("en-US");

                    SessionInfo.SetObjectInSession(Constants.Languages.English, Constants.CultureNameKey);
                    System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                }
                else
                {
                    cultureInfo = new CultureInfo("ar-SA");

                    SessionInfo.SetObjectInSession(Constants.Languages.Arabic, Constants.CultureNameKey);
                    System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
                    System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;
                    CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                }
            }

            catch (Exception)
            {
                throw;
            }
        }
         
     }
}