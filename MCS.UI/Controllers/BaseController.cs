using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using MCS.Framework.Controls;
using MCS.Framework.Exceptions;
using MCS.Framework.Logging;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers.Collaboration;
using MCS.UI.Areas.User.Mappers.File;
using MCS.UI.Areas.User.Mappers.Notification;
using MCS.UI.Areas.User.Mappers.Search.TransactionCertificate;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models.Collaboration;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Notifications;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using System.Configuration;
using MCS.UI.Areas.User.Controllers;
using MCS.UI.Areas.User.Models.Transaction;
using System.IO;
using MCS.UI.Areas.User.Mappers.Transaction;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;
using System.Web;

namespace MCS.UI
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Result is JsonResult)
            {
                ((JsonResult)filterContext.Result).MaxJsonLength = int.MaxValue;
            }
            base.OnActionExecuted(filterContext);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (SystemConfigurations.MultiTenantEnabled && SessionInfo.GetObjectFromSession(Constants.TenantKey) != null)
            {
                if (HttpContext.Request.Headers.Get(Constants.TenantDatabaseName) == null)
                {
                    HttpContext.Request.Headers.Set(Constants.TenantDatabaseName, (SessionInfo.GetObjectFromSession(Constants.TenantKey) as Framework.MultiTenants.TenantInfo).DatabaseName);
                }
            }

            ViewBag.MessageToShow = "";

            GridHelper.ResetPageSize();
            base.OnActionExecuting(filterContext);

            if (Session["IsOTPValidated"] != null
                && Session["IsOTPValidated"].ToString().ToLower() == Boolean.FalseString.ToLower()
                && filterContext.ActionDescriptor.ControllerDescriptor.ControllerName != "Login")
            {
                if (ControllerContext.RouteData.DataTokens["area"].ToString() == "User")
                {
                    filterContext.Result = new RedirectResult("~/User/Login/ValidationNumber");
                }
                else
                {
                    filterContext.Result = new RedirectResult("~/User/Login");
                }
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            Exception exception = filterContext.Exception;

            TempData["Exception"] = exception;

            Logger.WriteException(exception);

            filterContext.ExceptionHandled = true;

            HandleErrorInfo handleErrorInfo =
                new HandleErrorInfo(exception, filterContext.RouteData.Values["controller"].ToString(),
                    filterContext.RouteData.Values["action"].ToString());

            string actionName = "Error";
            string controllerName = "Error";

            if (!Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    Action = actionName,
                    Controller = controllerName,
                    Area = filterContext.RouteData.DataTokens["area"]
                }));
            }
            else
            {
                bool errorOccurred = true;
                string url = MCS.UI.UrlHelper.GetBaseUri() + $"/{filterContext.RouteData.DataTokens["area"]}/Error/Error";

                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,

                    Data = new
                    {
                        errorOccurred,
                        url
                    }
                };
            }

            ExceptionHelper.HandleException(exception);

            base.OnException(filterContext);
        }
        [HttpPost]
        public ActionResult GetCertificate(string transactionCode)
        {

            try
            {
                GetResultExtraData<Object> transactionCertificateDTO =
                          HttpClientWrapper<GetResultExtraData<Object>>.GetItemRequest(string.Format("api/Transaction/GetTransactionCertificateByReference?referenceCode={0}&orgUnitId={1}&cultureName={2}", transactionCode, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                string message = string.Empty;

                if (transactionCertificateDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Open.TransactionNotFound");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int type = Convert.ToInt32(transactionCertificateDTO.ExtraData.ToString());

                if (type == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    InboundCertificateDTO inboundCertificateDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<InboundCertificateDTO>(transactionCertificateDTO.Result.ToString());

                    inboundCertificateDTO.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                    IAjaxGrid inboundNames = (AjaxGrid<TransactionNameDTO>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateDTO.Names, 1, inboundCertificateDTO.Names.Count(), true);
                    ViewData["NamesData"] = inboundNames;

                    IAjaxGrid assignments = (AjaxGrid<TransactionAssignmentDTO>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateDTO.Assignments, 1, inboundCertificateDTO.Assignments.Count(), true);
                    ViewData["AssignmentsData"] = assignments;

                    //return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundCertificatePartial.cshtml", inboundCertificateDTO);

                    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_InboundCertificatePartial.cshtml", inboundCertificateDTO), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

                }

                OutboundCertificateDTO outboundCertificateDTO = Newtonsoft.Json.JsonConvert.DeserializeObject<OutboundCertificateDTO>(transactionCertificateDTO.Result.ToString());

                outboundCertificateDTO.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                IAjaxGrid outboundNames = (AjaxGrid<TransactionNameDTO>)new AjaxGridFactory().CreateAjaxGrid(outboundCertificateDTO.Names, 1, outboundCertificateDTO.Names.Count(), true);
                ViewData["NamesData"] = outboundNames;

                return View("~/Areas/User/Views/Shared/TransactionCertificate/_OutboundCertificatePartial.cshtml", OutboundCertificateMapper.Map(outboundCertificateDTO));
            }

            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetNotifications(int index, int pageSize, string fromDate, string toDate, bool isRead)
        {
            try
            {
                GetResult<List<NotificationDTO>> notificationDTO = HttpClientWrapper<GetResult<List<NotificationDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetNotifications?PageIndex={0}&pageSize={1}&FromDate={2}&ToDate={3}&isRead={4}&CultureName={5}",
                    index, pageSize, fromDate, toDate, isRead, SessionInfo.CultureShortName)).Result;

                var countUnReadNotification = HttpClientWrapper<GetResult<List<NotificationDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetNotifications?PageIndex={0}&pageSize={1}&FromDate={2}&ToDate={3}&isRead={4}&CultureName={5}",
                    index, pageSize, fromDate, toDate, false, SessionInfo.CultureShortName)).Result.RowsCount;

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NotificationHeaderPartial", NotificationMapper.Map(notificationDTO.Result)),
                    Count = countUnReadNotification
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public /*async Task<*/ActionResult/*>*/ GetChatNotifications()
        {
            try
            {
                GetResult<ChatNotificationsInfoDTO> chatNotificationsInfoDTO = /*await*/
                    HttpClientWrapper<GetResult<ChatNotificationsInfoDTO>>.GetItemRequest("api/Common/GetChatNotifications").Result;

                return Json(new { ChatNotifications = chatNotificationsInfoDTO }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public /*async Task<*/ActionResult/*>*/ GetCollaborationUsers()
        {
            try
            {
                GetResult<List<CollaborationUserInfoDTO>> collaborationUserInfoDTOs = /*await*/
                    HttpClientWrapper<GetResult<List<CollaborationUserInfoDTO>>>.GetItemRequest(string.Format("api/Common/GetCollaborationUsers?cultureName={0}", SessionInfo.CultureShortName)).Result;

                return Json(new { Users = collaborationUserInfoDTOs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult AllNotification(int? page)
        {
            try
            {
                LoadSideBarMenu();
                var getResult = HttpClientWrapper<GetResult<List<NotificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetNotifications?pageIndex={0}&pageSize={1}&isRead={2}",
                                page ?? 1, GridHelper.PageSize, true)).Result;

                var grid = (AjaxGrid<NotificationVM>)new AjaxGridFactory().CreateAjaxGrid(NotificationMapper.Map(getResult.Result), page ?? 1,
                            getResult.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                if (page.HasValue)
                {
                    grid = (AjaxGrid<NotificationVM>)new AjaxGridFactory().CreateAjaxGrid(NotificationMapper.Map(getResult.Result), page ?? 1, getResult.RowsCount.Value,
                        page.HasValue, GridHelper.PageSize);
                    return Json(new
                    {
                        Html = grid.ToJson("~/Areas/User/Views/Shared/_NotificationGridPartial.cshtml", this),
                        grid.HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/Shared/AllNotifications.cshtml", grid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult SearchNotifications(int index, int pageSize, string fromDate, string toDate)
        {
            try
            {
                GetResult<List<NotificationDTO>> notificationDTO =
                    HttpClientWrapper<GetResult<List<NotificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetNotifications?PageIndex={0}&pageSize={1}&FromDate={2}&ToDate={3}&cultureName={4}", index, pageSize, fromDate, toDate, SessionInfo.CultureShortName)).Result;
                return View("~/Areas/User/Views/Shared/_AllNotificationsPartial.cshtml", notificationDTO.Result);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteNotifications(string ids)
        {
            try
            {
                string message = string.Empty;

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Transaction/DeleteNotifications?ids={0}", ids)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.DeleteSucceeded");

                return Json(new { ids = ids.Split(',').Select(int.Parse).ToList(), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult MarkAsReadNotification(string ids)
        {
            try
            {
                string message = string.Empty;
                var postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/MarkAsReadNotification?ids={0}", ids), null).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.DeleteSucceeded");
                return Json(new { ids = ids.Split(',').Select(int.Parse).ToList(), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetIntitialChatDataHistory(int toUserId, int pageSize, string toUserName)
        {
            try
            {
                GetResult<List<ConversationDTO>> conversationDTOs =
                   HttpClientWrapper<GetResult<List<ConversationDTO>>>.GetItemRequest(string.Format("api/Common/GetIntitialChatHistory?toUserId={0}&pageSize={1}&cultureName={2}", toUserId, pageSize, SessionInfo.CultureShortName)).Result;

                ViewData["RequestedUser"] = toUserName.ToString();

                ViewData["RequestedUserId"] = toUserId.ToString();
                List<ConversationVM> conversationVMs = ConversationMapper.Map(conversationDTOs.Result);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Views/Shared/_ConversationWindowPartial.cshtml", conversationVMs), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetChatHistory(int toUserId, int pageSize, int startId)
        {
            try
            {
                GetResult<List<ConversationDTO>> conversationDTOs =
                   HttpClientWrapper<GetResult<List<ConversationDTO>>>.GetItemRequest(string.Format("api/Common/GetChatHistory?toUserId={0}&pageSize={1}&startId={2}&cultureName={3}", toUserId, pageSize, startId, SessionInfo.CultureShortName)).Result;

                return Json(new { Conversations = conversationDTOs }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetExternalPartyNodes(string letterId, int? parentId, string containerName, string onClickFunc)
        {

            try
            {
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                   HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, parentId)).Result;

                TreeViewModel tree = new TreeViewModel();

                List<TreeNode> nodes = new List<TreeNode>();

                tree.RootNode = new TreeNode { Id = parentId.Value, Mode = tree.Mode };

                foreach (ExternalPartyDTO externalPartyDTO in externalPartyDTOs.Result)
                {
                    TreeNode treeNode = new TreeNode()
                    {
                        DepartmentNumber = externalPartyDTO.Number,
                        IsSelected = externalPartyDTO.IsSelected,
                        Name = externalPartyDTO.LocalName,
                        Id = externalPartyDTO.Id,
                        HasChilds = externalPartyDTO.HasChilds,
                        ParentId = parentId.Value,
                        Selectable = true,
                        IsYesserRegistered = externalPartyDTO.YasserRegistered
                    };

                    tree.RootNode.Childs.Add(treeNode);
                }
                ViewData["containerName"] = containerName;
                ViewData["onClickFunc"] = onClickFunc;
                return PartialView("~/Areas/User/Views/Shared/_SubDepartmentsOnDemandTreePartial.cshtml", tree);
            }

            catch (Exception)
            {
                throw;
            }

        }

        public ActionResult GetExternalPartyMultyNodes(string letterId, int? parentId, string containerName, string onClickFunc, string checkboxFunction,string additionalClass ="")
        {

            try
            {
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                   HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, parentId)).Result;

                TreeViewModel tree = new TreeViewModel();

                List<TreeNode> nodes = new List<TreeNode>();

                tree.RootNode = new TreeNode { Id = parentId.Value, Mode = tree.Mode };

                foreach (ExternalPartyDTO externalPartyDTO in externalPartyDTOs.Result)
                {
                    TreeNode treeNode = new TreeNode()
                    {
                        DepartmentNumber = externalPartyDTO.Number,
                        IsSelected = externalPartyDTO.IsSelected,
                        Name = externalPartyDTO.LocalName,
                        Id = externalPartyDTO.Id,
                        HasChilds = externalPartyDTO.HasChilds,
                        ParentId = parentId.Value,
                        Selectable = true,
                        IsYesserRegistered = externalPartyDTO.YasserRegistered,
                        CheckboxFunction = checkboxFunction

                    };

                    tree.RootNode.Childs.Add(treeNode);
                }
                tree.RootNode.CheckboxFunction = checkboxFunction;
                ViewData["containerName"] = containerName;
                ViewData["onClickFunc"] = onClickFunc;
                ViewData["additionalClass"] = additionalClass;
                return PartialView("~/Areas/User/Views/Shared/_SubDepartmentsOnDemandMultyTreePartial.cshtml", tree);
            }

            catch (Exception)
            {
                throw;
            }

        }


        public ActionResult GetExternalPartyNodesAutoComplete(string searchQuery)
        {
            try
            {
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                    HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartiesAutoComplete?cultureName={0}&searchQuery={1}&resultSize={2}", SessionInfo.CultureShortName, searchQuery, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AutoCompleteResultSize"].ToString()))).Result;

                return Json(new { Parties = externalPartyDTOs.Result, externalPartyDTOs.RowsCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetExternalPartyNodeById(int partyId)
        {
            try
            {
                GetResult<ExternalPartyEditDTO> externalPartyEditDTO =
                HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(string.Format("api/Common/GetExternalParty?id={0}", partyId)).Result;

                if (externalPartyEditDTO.Result != null)
                {
                    return Json(new { Party = externalPartyEditDTO.Result, isExist = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { isExist = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetOrgUnitsNodes(int? parentId, string containerName, string onClickFunc, TreeMode treeMode = TreeMode.Single)
        {

            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsByParentId?parentId={0}&cultureName={1}", parentId, SessionInfo.CultureShortName)).Result;
                TreeViewModel tree = new TreeViewModel();
                tree.Mode = treeMode;
                List<TreeNode> nodes = new List<TreeNode>();

                tree.RootNode = new TreeNode { Id = parentId.Value, Mode = tree.Mode };

                foreach (OrgUnitDTO orgUnitDTO in orgUnitDTOs.Result)
                {
                    TreeNode treeNode = new TreeNode()
                    {
                        DepartmentNumber = orgUnitDTO.Number.ToString(),
                        IsSelected = orgUnitDTO.IsSelected,
                        Name = orgUnitDTO.Name,
                        Id = orgUnitDTO.Id,
                        HasChilds = orgUnitDTO.HasChilds,
                        ParentId = parentId.Value,
                        Selectable = true
                    };

                    tree.RootNode.Childs.Add(treeNode);
                }
                ViewData["containerName"] = containerName;
                ViewData["onClickFunc"] = onClickFunc;
                return PartialView("~/Areas/User/Views/Shared/_SubDepartmentsTreePartial.cshtml", tree);
            }

            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult GetOrgUnitsMultiNodes(int? parentId, string containerName, string onClickFunc, TreeMode treeMode = TreeMode.Single,string checkboxFunction = "CheckboxChange", string additionalClass = "")
        {

            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsByParentId?parentId={0}&cultureName={1}", parentId, SessionInfo.CultureShortName)).Result;
                TreeViewModel tree = new TreeViewModel();
                tree.Mode = treeMode;
                List<TreeNode> nodes = new List<TreeNode>();

                tree.RootNode = new TreeNode { Id = parentId.Value, Mode = tree.Mode };

                foreach (OrgUnitDTO orgUnitDTO in orgUnitDTOs.Result)
                {
                    TreeNode treeNode = new TreeNode()
                    {
                        DepartmentNumber = orgUnitDTO.Number.ToString(),
                        IsSelected = orgUnitDTO.IsSelected,
                        Name = orgUnitDTO.Name,
                        Id = orgUnitDTO.Id,
                        HasChilds = orgUnitDTO.HasChilds,
                        ParentId = parentId.Value,
                        Selectable = true
                    };

                    tree.RootNode.Childs.Add(treeNode);
                }
                ViewData["containerName"] = containerName;
                ViewData["additionalClass"] = additionalClass;
                ViewData["onClickFunc"] = onClickFunc;
                ViewData["checkboxFunction"] = checkboxFunction;
                return PartialView("~/Areas/User/Views/Shared/_SubDepartmentsTreeMultiPartial.cshtml", tree);
            }

            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult GetOrgUnitsAutoComplete(string searchQuery)
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTO = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsAutoComplete?cultureName={0}&searchQuery={1}&resultSize={2}&orgUnitId={3}",
                                                SessionInfo.CultureShortName, searchQuery, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AutoCompleteResultSize"].ToString()), SessionInfo.OrgUnitId)).Result;

                return Json(new { Parties = orgUnitDTO.Result, orgUnitDTO.RowsCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetOrgUnitNodeById(int? orgUnitId)
        {
            try
            {

                if (!orgUnitId.HasValue || orgUnitId == 0)
                {
                    return Json(new { isExist = false }, JsonRequestBehavior.AllowGet);
                }
                GetResult<OrgUnitDTO> orgUnitDTO =
                HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", orgUnitId, SessionInfo.CultureShortName)).Result;

                if (orgUnitDTO.Result != null)
                {
                    return Json(new { Party = orgUnitDTO.Result, isExist = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { isExist = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult GetOrgUnitsNodesByIds(List<int> orgUnitIds)
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsByIds?orgUnitIds={0}&cultureName={1}", string.Join(",", orgUnitIds), SessionInfo.CultureShortName)).Result;

                if (orgUnitDTOs.Result != null)
                {
                    return Json(new { Party = orgUnitDTOs.Result, isExist = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { isExist = false }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public List<UserProfileVM> GetAllUserProfiles(int? entityId, string searchQuery = null)
        {
            GetResult<List<UserProfileDTO>> getResult =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAllUsers?cultureName={0}&searchQuery={1}&entityId={2}", SessionInfo.CultureShortName, searchQuery, entityId)).Result;

            List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(getResult.Result);

            return userProfileVMs;
        }
        public string ConvertUsersListToDataSource(List<UserProfileVM> userProfileVMs)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (userProfileVMs != null)
                {
                    foreach (var user in userProfileVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = user.Id.ToString(),
                            Label = user.LocalName
                        });
                    }
                }

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsValidMimeType(string MimeType)
        {
            Dictionary<string, string> AllowedMimeTypes = new Dictionary<string, string>
            {
                { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "docx" },
                { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "xlsx" },
                { "application/pdf", "pdf" },
                { "image/png", "png" },
                { "image/jpg", "jpg" },
                { "image/jpeg", "jpeg" },
                { "image/gif", "gif" },
                { "image/bmp", "bmp" },
                { "application/vnd.ms-excel", "xls" },
                { "application/msword", "doc" },
                { "image/tiff", "tif" }
            };

            if (!AllowedMimeTypes.Keys.Contains(MimeType))
            {
                return false;
            }
            return true;
        }

        public string GetMimeType(string MimeType)
        {

            switch (MimeType)
            {
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    return "docx";
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    return "xlsx";
                case "application/pdf":
                    return "pdf";
                case "image/png": return "png";
                case "image/jpg": return "jpg";
                case "image/jpeg": return "jpeg";
                case "image/gif": return "gif";
                case "image/bmp": return "bmp";
                case "application/vnd.ms-excel": return "xls";
                case "application/msword": return "doc";
                case "image/tiff": return "tif";
                default:
                    return "pdf";
            }
        }
        public string GetAttchementMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        public List<AutoCompleteDataSource> GetDateLookups(LookupCategory lookupCategory)
        {
            try
            {

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
                        });
                    }
                }
                return dataSource.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void LoadSideBarMenu()
        {
            //string message = string.Empty;

            //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
            // HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);

        }
        public string GetAssignmentPaperGroups(int userId)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<AssignmentPaperGroupVM>> assignmentPaperGroupVMs = GetAssignmentPaperGroupList(userId);
                if (assignmentPaperGroupVMs.Result != null)
                {
                    foreach (AssignmentPaperGroupVM assignmentPaperGroupVM in assignmentPaperGroupVMs.Result.OrderBy(x => x.OrderNo).ToList())
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = assignmentPaperGroupVM.Id.ToString(),
                            Label = assignmentPaperGroupVM.Name
                        });
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static GetResult<List<AssignmentPaperGroupVM>> GetAssignmentPaperGroupList(int userId)
        {
            GetResult<List<AssignmentPaperGroupDTO>> assignmentPaperGroupDTOs = HttpClientWrapper<GetResult<List<AssignmentPaperGroupDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperGroupsByUserId?userId={0}&cultureName={1}", userId, SessionInfo.CultureShortName)).Result;

            return new GetResult<List<AssignmentPaperGroupVM>>
            {
                Result = AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTOs.Result),
                RowsCount = assignmentPaperGroupDTOs.Result.Count,
                StatusCode = assignmentPaperGroupDTOs.StatusCode
            };
        }

        public byte[] ConvertWordToPDF(string document)
        {
            byte[] bPDF = null;
            string guid = Guid.NewGuid().ToString("N");
            string basePath = ConfigurationManager.AppSettings["DocsPath"];
            // create temporary ServerTextControl
            string fileName = basePath + guid + ".doc";
            var result = "";
            try
            {
                System.IO.File.WriteAllBytes(fileName, Convert.FromBase64String(document));
                result = DocumentViewerHelper.CreatePDF(fileName, basePath);
            }
            catch (Exception ex)
            {
                result = null;
                System.IO.File.Delete(fileName);
            }

            if (!string.IsNullOrWhiteSpace(result))
            {
                bPDF = System.IO.File.ReadAllBytes(result);
                System.IO.File.Delete(result);
            }

            // return as Base64 encoded string
            return bPDF;
        }

        public byte[] NewConvertWordToPDF(string document)
        {
            var result = DocumentViewerHelper.ConvertToPDF(Convert.FromBase64String(document));
            return Convert.FromBase64String(result);
        }
        public byte[] GetBarcodeByte(int transactionId, bool ignoreLogging = false)
        {
            string messageText = "";
            MessageType messageType = MessageType.Information;

            GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
      HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}&ignoreLogging={3}", SessionInfo.CultureShortName, transactionId, SessionInfo.OrgUnitId, ignoreLogging)).Result;
            byte[] barcodeImg = null;
            if (transactionBarcodesDTOs.StatusCode != StatusCode.Ok)
            {
                return null;
            }
            BarcodeVM barcode = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result).BarcodeVMs.Where(b => b.Type == BarcodePrintType.Transaction).FirstOrDefault();
            TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);
            if (barcode != null)
            {
                SharedController sharedController = new SharedController();
                sharedController.FillBarcodeDesign(transactionBarcodesVM.TransactionBarcodeHtmlDesign, barcode, transactionBarcodesVM, transactionBarcodesVM.TransactionDesignWidth, transactionBarcodesVM.TransactionDesignHeight);
                barcodeImg = barcode.Content;
            }
            return barcodeImg;
        }
        //public byte[] addImageToPDF(byte[] pdf, byte[] image)
        //{
        //    using (MemoryStream outputPdfStream = new MemoryStream())
        //    {
        //        var reader = new iTextSharp.text.pdf.PdfReader(pdf);
        //        var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);
        //        var pdfContentByte = stamper.GetOverContent(1);

        //        iTextSharp.text.Rectangle rect = reader.GetPageSizeWithRotation(1);
        //        long centerWidth = Convert.ToInt64(0.05 * rect.Width);
        //        long centerHeight = Convert.ToInt64(0.8 * rect.Height);

        //        iTextSharp.text.Image imageAdd = iTextSharp.text.Image.GetInstance(image);
        //        imageAdd.SetAbsolutePosition(centerWidth, centerHeight);
        //        pdfContentByte.AddImage(imageAdd);
        //        stamper.Close();

        //        return outputPdfStream.ToArray();
        //    }
        //}

        public byte[] addImageToPDF(byte[] pdf, byte[] image, int imageWidth, int imageHeight)
        {
            using (MemoryStream outputPdfStream = new MemoryStream())
            {
                var reader = new iTextSharp.text.pdf.PdfReader(pdf);
                var stamper = new iTextSharp.text.pdf.PdfStamper(reader, outputPdfStream);
                var pdfContentByte = stamper.GetOverContent(1);

                iTextSharp.text.Rectangle rect = reader.GetPageSizeWithRotation(1);
                long centerWidth = Convert.ToInt64(0.05 * rect.Width);
                long centerHeight = Convert.ToInt64(0.88 * rect.Height);
                iTextSharp.text.Image imageAdd = iTextSharp.text.Image.GetInstance(image);
                imageAdd.ScaleAbsoluteHeight(imageHeight);
                imageAdd.ScaleAbsoluteWidth(imageWidth);
                imageAdd.SetAbsolutePosition(centerWidth, centerHeight);
                pdfContentByte.AddImage(imageAdd);
                stamper.Close();

                return outputPdfStream.ToArray();
            }
        }
        public System.Drawing.Image ResizeImage(System.Drawing.Image image, int width, int height)
        {
            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format64bppArgb);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.Clear(System.Drawing.Color.White);
                //graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;



                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public byte[] ResizeImageByte(byte[] imageByte, int width, int height)
        {
            System.Drawing.Image image;
            using (MemoryStream ms = new MemoryStream(imageByte))
            {
                image = System.Drawing.Image.FromStream(ms);
            }

            var destRect = new System.Drawing.Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            using (var stream = new MemoryStream())
            {
                destImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }

        }
        public Image[] SaveAsImage(Spire.Pdf.PdfDocument document)
        {
            Image[] images = new Image[document.Pages.Count];
            for (int i = 0; i < document.Pages.Count; i++)
            {
                // use the document.SaveAsImage() method save the pdf as image
                images[i] = document.SaveAsImage(i);
            }
            return images;
        }

        public string ImageToBase64(Image image)
        {
            System.Drawing.Imaging.ImageFormat format = ImageFormat.Png;
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

    }
}