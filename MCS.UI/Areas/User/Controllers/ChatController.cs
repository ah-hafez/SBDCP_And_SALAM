using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.MultiTenants;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Controllers
{
    [CustomAuthorize]
    public class ChatController : BaseController
    {
        public ActionResult GetUserToken()
        {
            var token = SessionInfo.AccessToken;
            var tanentId = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey))?.Id;
            var tenantDatabaseName = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey))?.DatabaseName;

            return Json(new { Token = token, TanentId = tanentId, TenantDatabaseName = tenantDatabaseName }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTransactionConversations(int transactionId, int pageIndex)
        {
            int pageSize = GridHelper.PageSize;
            GetResult<List<ConversationChatDTO>> transactionConversations =
                        HttpClientWrapper<GetResult<List<ConversationChatDTO>>>.GetItemRequest(string.Format("api/Common/GatTransactionChats?transactionId={0}&pageIndex={1}&pageSize={2}&cultureName={3}", transactionId, pageIndex, pageSize, SessionInfo.CultureShortName)).Result;

            List<TransactionChatVM> transactionChatVMs = new List<TransactionChatVM>();
            var chatList = transactionConversations.Result;
            foreach (var item in chatList)
            {
                TransactionChatVM transactionChatVM = new TransactionChatVM();
                transactionChatVM.Id = item.Id;
                transactionChatVM.DateTimeHJ = item.SendTime;
                transactionChatVM.ChatUsers = item.Name;
                transactionChatVMs.Add(transactionChatVM);
            }


            IAjaxGrid grid = (AjaxGrid<TransactionChatVM>)new AjaxGridFactory().CreateAjaxGrid(transactionChatVMs, 1, transactionConversations.RowsCount.Value, false);

            return Json(
                new {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Chat/_TransactionChatGridPartial.cshtml", grid),
                    TotalCount = transactionConversations.RowsCount.Value
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateGridChats(int? page, int transactionId, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<ConversationChatDTO>> transactionConversations =
                            HttpClientWrapper<GetResult<List<ConversationChatDTO>>>.GetItemRequest(string.Format("api/Common/GatTransactionChats?transactionId={0}&pageIndex={1}&pageSize={2}&cultureName={3}", transactionId, page.Value, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<TransactionChatVM> transactionChatVMs = new List<TransactionChatVM>();
                var chatList = transactionConversations.Result;
                foreach (var item in chatList)
                {
                    TransactionChatVM transactionChatVM = new TransactionChatVM();
                    transactionChatVM.Id = item.Id;
                    transactionChatVM.DateTimeHJ = item.SendTime;
                    transactionChatVM.ChatUsers = item.Name;
                    transactionChatVMs.Add(transactionChatVM);
                }


                IAjaxGrid grid = (AjaxGrid<TransactionChatVM>)new AjaxGridFactory().CreateAjaxGrid(transactionChatVMs, page.HasValue ? page.Value : 1, transactionConversations.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Chat/_TransactionChatGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetConversationMessages(int roomId, string timeZone)
        {
            GetResult<List<MessageResultDTO>> messages =
                        HttpClientWrapper<GetResult<List<MessageResultDTO>>>.GetItemRequest(string.Format("api/Common/GetConversationMessages?roomId={0}&timeZone={1}", roomId, int.Parse(timeZone) * -1)).Result;

            return Json(
                new
                {
                    Messages = messages.Result,
                }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetPreviousMessages(int messageId, string timeZone)
        {
            GetResult<List<MessageResultDTO>> messages =
                        HttpClientWrapper<GetResult<List<MessageResultDTO>>>.GetItemRequest(string.Format("api/Common/GetPreviousMessages?messageId={0}&timeZone={1}", messageId, int.Parse(timeZone) * -1)).Result;

            return Json(
                new
                {
                    Messages = messages.Result,
                }, JsonRequestBehavior.AllowGet);
        }
    }
}