using System.Web.Mvc;

namespace MCS.GridMvc.Ajax.Helpers
{
    public interface IHtmlHelpers
    {
        string RenderPartialViewToString(string viewName, object model, Controller controller);
    }
}
