using System.Web.Mvc;
namespace MCS.UI
{
    public static class AutoCompleteExtensions
    {
        public static MvcHtmlString AutoComplete(this HtmlHelper html, string autoCompleteControlid, string hdnIdToSaveValue, string items, bool matchAnywhere, string content = "", string inputClassName = "", string ulClassName = "", string buttonId = "", string hdnExtraParametersId = "", bool selectFirstIndex = false, string validationClass = "", string waterMarkText = "", int maxLengthText = 50, string onChangeCallback = "", string onKeyUpCallback = "")
        {
            return MvcHtmlString.Create(MCS.UI.Controls.AutoComplete.RenderAutoComplete(autoCompleteControlid, hdnIdToSaveValue, items, matchAnywhere, content, inputClassName, ulClassName, buttonId, hdnExtraParametersId, selectFirstIndex, validationClass, waterMarkText, maxLengthText, onChangeCallback, onKeyUpCallback));
        }

        public static MvcHtmlString ResetAutoComplete(string autoCompleteControlid)
        {
            return MvcHtmlString.Create(MCS.UI.Controls.AutoComplete.RenderResetAutoComplete(autoCompleteControlid));
        }

        public static MvcHtmlString AutoCompleteChangeList(string autoCompleteControlid, string newList)
        {
            return MvcHtmlString.Create(MCS.UI.Controls.AutoComplete.AutoCompleteChangeList(autoCompleteControlid, newList));
        }
    }
}