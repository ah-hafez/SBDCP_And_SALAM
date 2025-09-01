using System.Web.Mvc;

namespace MCS.UI.Areas.User.Controllers
{
    public class ErrorController : BaseController
    {
        public ActionResult Error()
        {
            return RedirectToAction("CustomError", "Error");
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult CustomError()
        {
            return View("~/Views/Error/Error.cshtml");
        }
    }
}