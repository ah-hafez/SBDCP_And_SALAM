using System.Web.Mvc;

namespace  MCS.UI.TenantsAdmin.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

    }
}
