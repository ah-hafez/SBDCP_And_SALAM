using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MCS.Framework.Localization;

namespace MCS.UI.TenantsAdmin
{
    public enum OperationType
    {
        Save = 1,
        Delete = 2,
        Update = 3

    }
    public class AjaxContentType
    {
        public const string Josn = "application/json; charset=utf-8";
    }

    public static class ActionButtonHelperExt
    {
        private static string GetComfirmMessageByActionType(OperationType actionType)
        {
            string result = string.Empty;
            switch (actionType)
            {
                case OperationType.Save:
                result = DbRes.TResource("Global.ComfirmMessage.Save");
                break;
                case OperationType.Delete:
                result = DbRes.TResource("Global.ComfirmMessage.Delete");
                break;
                case OperationType.Update:
                result = DbRes.TResource("Global.ComfirmMessage.Update");
                break;
            }
            return result;
        }

        private static string GetMessageByID(string messageID)
        {


            return "Message ID";
        }

        private static readonly string confirmResource = DbRes.TResource("Global.ComfirmMessage.Confirm");
        private static readonly string cancelResource = DbRes.TResource("Global.ComfirmMessage.Cancel");

        public static MvcHtmlString ConfirmAction(this HtmlHelper helper, OperationType operationType,
            string buttonID, string buttonText, string actionUrl, string divUpdatData, object paramObj,
            string contentType = AjaxContentType.Josn, string onAjaxSuccess = "", string onAjaxFaild = "",
            string confirmMsg = "", string actionType = "post", string formId = "", string validationGroup = "",
            string cssClass = "", string clientFunctionToCall = "", string permissionCode = "", string notValidFunctionCall = "")
        {
            if (string.IsNullOrEmpty(confirmMsg))
            {
                confirmMsg = GetComfirmMessageByActionType(operationType);
            }

            string actionFunc = string.Format("ConfirmAction('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}', '{12}', '{13}')", actionUrl, divUpdatData, paramObj, contentType, onAjaxSuccess, onAjaxFaild, confirmMsg, actionType, formId, buttonID, confirmResource, cancelResource, clientFunctionToCall, notValidFunctionCall);

            TagBuilder input = new TagBuilder("input");

            input.Attributes.Add("type", "submit");
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("name", validationGroup);
            input.Attributes.Add("onclick", "return " + actionFunc);
            input.Attributes.Add("class", cssClass);
            input.Attributes.Add("id", buttonID);

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ConfirmAction(this HtmlHelper helper, OperationType operationType, string buttonText,
            string divDialogConfirm, string confirmMsg = "", string validationGroup = "", string cssClass = "",
            string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            if (string.IsNullOrEmpty(confirmMsg))
            {
                confirmMsg = GetComfirmMessageByActionType(operationType);
            }
            string actionFunc = string.Format("ConfirmCustomAction('{0}','{1}','{2}','{3}')", divDialogConfirm, confirmMsg, confirmResource, cancelResource);

            var input = new TagBuilder("input");
            input.Attributes.Add("type", "button");
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("validationGroup", validationGroup);
            input.Attributes.Add("onclick", actionFunc);
            input.Attributes.Add("class", cssClass);

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ConfirmAction(this HtmlHelper helper, OperationType operationType,
            string divDailogContainerId, string buttonText, string partialViewUrl, string confirmMsg = "",
            string validationGroup = "", string cssClass = "", string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            if (string.IsNullOrEmpty(confirmMsg))
            {
                confirmMsg = GetComfirmMessageByActionType(operationType);
            }

            var div = new TagBuilder("div");
            div.Attributes.Add("id", divDailogContainerId);
            div.Attributes.Add("style", "display:none");
            div.InnerHtml = helper.Partial(partialViewUrl).ToHtmlString();
            string actionFunc = string.Format("ConfirmPartialAction('{0}','{1}','{2}','{3}')", divDailogContainerId, confirmMsg, confirmResource, cancelResource);
            var form = new TagBuilder("form");
            var input = new TagBuilder("input");
            input.Attributes.Add("type", "button");
            input.Attributes.Add("validationGroup", validationGroup);
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("onclick", actionFunc);
            input.Attributes.Add("class", cssClass);

            form.InnerHtml = input.ToString(TagRenderMode.SelfClosing) + div.ToString();

            return MvcHtmlString.Create(form.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="actionButtonType"></param>
        /// <param name="buttonText"></param>
        /// <param name="actionUrl">the action url Like : Controller.Action ex InboundController.Save </param>
        /// /// <param name="divUpdatData"></param>
        /// <param name="paramObj">An object that contains the parameters that will send to action  </param>
        /// <param name="contentType"></param>
        /// <param name="onAjaxSuccess"></param>
        /// <param name="onAjaxFaild"></param>
        /// <param name="actionType"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionButton(this HtmlHelper helper, string buttonText, string actionUrl,
            string onAjaxSuccess = "", string cssClass = "", string formId = "")
        {
            TagBuilder input = new TagBuilder("button");

            string actionFunc = string.Format("Action('{0}','{1}','{2}')", actionUrl, onAjaxSuccess, formId);

            input = new TagBuilder("button");
            input.Attributes.Add("onclick", "return " + actionFunc);

            input.Attributes.Add("type", "button");
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("class", cssClass);
            input.InnerHtml = buttonText;

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ActionButton(this HtmlHelper helper, string buttonText, string actionUrl,
            string divUpdatData, object paramObj, string contentType, string onAjaxSuccess = "", string onAjaxFaild = "",
            string actionType = "get", string validationGroup = "", string buttonID = "", string formId = "",
            string cssClass = "", string clientFunctionToCall = "", string permissionCode = "", string notValidFunctionCall = "", string validFunctionCall = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            TagBuilder input = new TagBuilder("button");

            string actionFunc = string.Format("ActionButton('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}', '{10}', '{11}')", actionUrl, divUpdatData, paramObj, contentType, onAjaxSuccess, onAjaxFaild, actionType, buttonID, formId, clientFunctionToCall, notValidFunctionCall, validFunctionCall);

            if (actionType == "post")
            {
                input = new TagBuilder("button");
                input.Attributes.Add("name", validationGroup);
                input.Attributes.Add("onclick", "return " + actionFunc);
            }
            else
            {
                input.Attributes.Add("onclick", actionFunc);
            }

            input.Attributes.Add("type", "button");
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("class", cssClass);
            input.Attributes.Add("id", buttonID);
            input.InnerHtml = buttonText;

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ClientActionButton(this HtmlHelper helper, string buttonText, string actionUrl,
            string divUpdatData, string onAjaxSuccess = "", string onAjaxFaild = "", string validationGroup = "",
            string buttonID = "", string formId = "", string CssClass = "", object paramObj = null,
            string clientFunctionToCall = "", string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            string actionFunc = string.Format("ClientActionButton('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}')", actionUrl, divUpdatData, onAjaxSuccess, onAjaxFaild, buttonID, formId, paramObj, clientFunctionToCall);

            var input = new TagBuilder("button");

            input.Attributes.Add("type", "button");
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("name", validationGroup);
            input.Attributes.Add("onclick", "return " + actionFunc);
            input.Attributes.Add("class", CssClass);
            input.Attributes.Add("id", buttonID);
            input.InnerHtml = buttonText;

            if (validationGroup != null && validationGroup != "")
                input.Attributes.Add("validationGroup", validationGroup);

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ClientButton(this HtmlHelper helper, string buttonText,
            string Id = "", string CssClass = "", string clientFunctionToCall = ""
            , string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            var input = new TagBuilder("button");

            if (clientFunctionToCall != null && clientFunctionToCall != string.Empty)
                input.Attributes.Add("onclick", "return " + clientFunctionToCall + "()");

            input.Attributes.Add("type", "button");
            input.Attributes.Add("value", buttonText);
            input.Attributes.Add("class", CssClass);
            input.Attributes.Add("id", Id);
            input.InnerHtml = buttonText;

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ClientAnchor(this HtmlHelper helper, string buttonText,
      string Id = "", string CssClass = "", string clientFunctionToCall = ""
      , string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            var input = new TagBuilder("a");

            if (clientFunctionToCall != null && clientFunctionToCall != string.Empty)
                input.Attributes.Add("onclick", "return " + clientFunctionToCall + "()");

            input.Attributes.Add("type", "button");
            input.Attributes.Add("class", CssClass);
            input.Attributes.Add("id", Id);
            input.InnerHtml = buttonText;

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ClientLinkTag(this HtmlHelper helper, string text,
            string Id = "", string cssClass = "", string style = "", string title = "", string clientFunctionToCall = ""
            , string permissionCode = "", string innerHtml = "<i class='fa fa-group'></i>")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            var input = new TagBuilder("a");

            if (clientFunctionToCall != null && clientFunctionToCall != string.Empty)
                input.Attributes.Add("onclick", "return " + clientFunctionToCall);

            input.Attributes.Add("class", cssClass);
            input.Attributes.Add("style", style);
            input.Attributes.Add("id", Id);
            input.Attributes.Add("data-original-title", title);
            input.Attributes.Add("data-toggle", "tooltip");
            input.Attributes.Add("data-placement", "top");

            input.InnerHtml = innerHtml + text;

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString PrintButton(this HtmlHelper helper, string buttonText, string actionUrl,
            string cssClass = "", string buttonID = "", string permissionCode = "", string callbackFunc = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            string actionFunc = string.Empty;

            if (callbackFunc == string.Empty)
            {
                actionFunc = string.Format("PrintButton({0})", actionUrl);
            }
            else
            {
                actionFunc = string.Format("PrintButton({0},{1})", actionUrl, callbackFunc);
            }

            var input = new TagBuilder("input");

            input.Attributes.Add("type", "button");
            input.Attributes.Add("onclick", "return " + actionFunc);
            input.Attributes.Add("class", cssClass);
            input.Attributes.Add("id", buttonID);
            input.Attributes.Add("value", buttonText);

            return MvcHtmlString.Create(input.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString GridImageButton(this HtmlHelper helper, string actionUrl, string gridName, int columnIndex, OperationType actionButtonType, string divUpdatData = "",
            string formId = "", string contentType = AjaxContentType.Josn, string onAjaxSuccess = "", string onAjaxFaild = "",
            string actionType = "post", string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            var anchor = new TagBuilder("a");
            string actionFunc = null;
            string id = Guid.NewGuid().ToString();
            if (actionButtonType == OperationType.Delete)
            {
                actionFunc = string.Format("DeleteButton('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", actionUrl, formId, gridName,
                   contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData);

                anchor.InnerHtml = "<i class='glyphicon glyphicon-trash'></i>";
                anchor.Attributes.Add("class", "edit_trash");
                anchor.Attributes.Add("title", DbRes.TResource("Grid.Delete"));
                anchor.Attributes.Add("alt", DbRes.TResource("Grid.Delete"));
            }
            if (actionButtonType == OperationType.Update)
            {
                actionFunc = string.Format("UpdateButton('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", actionUrl, formId, gridName,
                    contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData);

                anchor.InnerHtml = "<i class='fa fa-edit'></i>";
                anchor.Attributes.Add("class", "edit_link");
                anchor.Attributes.Add("title", DbRes.TResource("Grid.Edit"));
                anchor.Attributes.Add("alt", DbRes.TResource("Grid.Edit"));
            }

            anchor.Attributes.Add("id", id);
            anchor.Attributes.Add("onclick", "return " + actionFunc);

            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ConfirmImageButton(this HtmlHelper helper, string actionUrl, string gridName, int columnIndex, string divUpdatData = "",
            string formId = "", string contentType = AjaxContentType.Josn, string onAjaxSuccess = "", string onAjaxFaild = "",
            string actionType = "post", string confirmMsg = "", string permissionCode = "", string validFunctionCall = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            if (string.IsNullOrEmpty(confirmMsg))
            {
                confirmMsg = GetComfirmMessageByActionType(OperationType.Delete);
            }

            var anchor = new TagBuilder("a");
            string actionFunc = null;
            string id = Guid.NewGuid().ToString();

            actionFunc = string.Format("DeleteActionButton('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}', '{13}')", actionUrl, formId, gridName,
               contentType, onAjaxSuccess, onAjaxFaild, actionType, id, columnIndex, divUpdatData, confirmMsg, confirmResource, cancelResource, validFunctionCall);

            anchor.InnerHtml = "<i class='glyphicon glyphicon-trash'></i>";
            anchor.Attributes.Add("class", "edit_trash");
            anchor.Attributes.Add("title", DbRes.TResource("Grid.Delete"));
            anchor.Attributes.Add("alt", DbRes.TResource("Grid.Delete"));
            anchor.Attributes.Add("id", id);
            anchor.Attributes.Add("onclick", "return " + actionFunc);

            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ResetButton(this HtmlHelper helper, string function, string id = "")
        {
            TagBuilder button = new TagBuilder("button");

            button.Attributes.Add("class", "btn btn-default");
            button.Attributes.Add("id", id);
            button.Attributes.Add("onclick", function + "; return false;");
            button.InnerHtml = DbRes.TResource("Global.ResetButton");

            return MvcHtmlString.Create(button.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ResetButton(this HtmlHelper helper)
        {
            TagBuilder button = new TagBuilder("button");

            button.Attributes.Add("class", "btn btn-default");
            button.Attributes.Add("onclick", "ClearFormInputs(); return false;");
            button.InnerHtml = DbRes.TResource("Global.ResetButton");

            return MvcHtmlString.Create(button.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString DownloadButton(this HtmlHelper helper, string url, string permissionCode = "")
        {
            TagBuilder anchor = new TagBuilder("a");

            anchor.Attributes.Add("class", "icon_the_download");
            anchor.Attributes.Add("href", url);
            anchor.Attributes.Add("title", DbRes.TResource("Global.Download"));
            anchor.Attributes.Add("alt", DbRes.TResource("Global.Download"));
            anchor.InnerHtml = "<i class='glyphicon glyphicon-download-alt'></i>";

            return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
        }

        public static MvcHtmlString ImageButton(this HtmlHelper helper, string onClickFunction, string cssClass,
            string toolTip, string path = "", string permissionCode = "")
        {
            if (!string.IsNullOrEmpty(permissionCode))
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(permissionCode))
                {
                    return new MvcHtmlString(string.Empty);
                }
            }

            if (string.IsNullOrEmpty(path))
            {
                TagBuilder anchor = new TagBuilder("a");

                anchor.Attributes.Add("class", "icon_the_download");
                anchor.Attributes.Add("onclick", onClickFunction);
                anchor.Attributes.Add("title", toolTip);
                anchor.Attributes.Add("alt", toolTip);
                anchor.InnerHtml = string.Format("<i class='{0}'></i>", cssClass);

                return MvcHtmlString.Create(anchor.ToString(TagRenderMode.Normal));
            }
            else
            {
                TagBuilder image = new TagBuilder("img");

                image.Attributes.Add("src", path);
                image.Attributes.Add("onclick", onClickFunction);
                image.Attributes.Add("class", cssClass);
                image.Attributes.Add("alt", toolTip);

                return MvcHtmlString.Create(image.ToString(TagRenderMode.Normal));
            }
        }
    }
}