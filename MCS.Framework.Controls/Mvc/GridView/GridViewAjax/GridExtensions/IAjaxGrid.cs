using System;
using System.Web.Mvc;

namespace MCS.Framework.Controls.Mvc
{
    public interface IAjaxGrid : IGrid
    {
        string ToJson(string gridPartialViewName, Controller controller);
        string ToJson(string gridPartialViewName, Object model, Controller controller);
        bool HasItems { get; }
    }
}