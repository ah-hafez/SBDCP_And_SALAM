using System.Web.Mvc;

namespace MCS.Framework.Controls.Mvc
{
    public interface IHtmlHelpers
    {
        string RenderPartialViewToString(string viewName, object model, Controller controller);
    }
}
