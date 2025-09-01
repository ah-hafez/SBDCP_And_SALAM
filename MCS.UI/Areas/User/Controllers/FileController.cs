using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Action;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.File;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Permission;
using MCS.UI.Areas.User.Mappers.Report;
using MCS.UI.Areas.User.Mappers.Search.TransactionCertificate;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Notifications;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Common;
using MCS.UI.TraysUISettings;
using System.Diagnostics;
using NPOI.POIFS.NIO;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MCS.UI.Areas.User
{
    [CustomAuthorizationAttribute(UserClaims.Files.File)]
    public class FileController : BaseController
    {

        public ActionResult VIPIndex()
        {
            return View();
        }
        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        public ActionResult Index()
        {
            try
            {
                GetResult<TrayDetailsDTO> trayDetailsDTOs =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTOs.StatusCode.ToString());
                }

                List<Tray> trayConfigElements = TraysConfig.Trays;

                ViewData["trayStyle"] = trayConfigElements;


                return View(TrayDetailsMapper.Map(trayDetailsDTOs.Result));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        public ActionResult OrgUnitIndex()
        {
            try
            {
                GetResult<TrayDetailsDTO> trayDetailsDTOs =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTOs.StatusCode.ToString());
                }

                List<Tray> trayConfigElements = TraysConfig.Trays;

                ViewData["trayStyle"] = trayConfigElements;


                return View(TrayDetailsMapper.Map(trayDetailsDTOs.Result));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        public ActionResult ArchiveIndex(int? page)
        {


            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllModules))
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllChildsModules))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchParentDepartment))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
                newList.Add(orgUnitDTOs.Result);
                ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
                newList.Add(orgUnitDTOs.Result);
                ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }


            return View("~/Areas/User/Views/File/ArchiveIndex.cshtml");
            //try
            //{
            //    GetResult<TrayDetailsDTO> trayDetailsDTOs =
            //         HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //    if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
            //    {
            //        throw new Exception(trayDetailsDTOs.StatusCode.ToString());
            //    }

            //    List<Tray> trayConfigElements = TraysConfig.Trays;

            //    ViewData["trayStyle"] = trayConfigElements;


            //    return View(TrayDetailsMapper.Map(trayDetailsDTOs.Result));
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        public ActionResult Classification(int? page)
        {


            return View("~/Areas/User/Views/File/_ClassificationCard.cshtml");
        }

        [HttpPost]
        public ActionResult RenderUserTrayTransactions(int trayId, TransactionDateType transactionDate)
        {
            try
            {
                string message = string.Empty;
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                GetResult<List<UserTransactionsTrayDTO>> userTransactionsTrayDTOs =
              HttpClientWrapper<GetResult<List<UserTransactionsTrayDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?orgUnitId={0}&transactionDate={1}&trayType={2}&PageIndex={3}&PageSize={4}&CultureName={5}", SessionInfo.OrgUnitId, transactionDate, trayId, 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userTransactionsTrayDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userTransactionsTrayDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["TrayType"] = trayId;
                ViewData["DataCount"] = (userTransactionsTrayDTOs.RowsCount.HasValue) ? userTransactionsTrayDTOs.RowsCount.Value : 0;
                ViewData["PageNumber"] = 1;
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/File/_TrayTransactionsPartial.cshtml", UserTransactionsTrayMapper.Map(userTransactionsTrayDTOs.Result)) }, JsonRequestBehavior.AllowGet);

            }

            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        public ActionResult MoveTransaction(int transactionId, TrayActionType trayActionType, int trayId, string pageSize, int? assignmentId, string remarks)
        {
            try
            {
                string message = string.Empty;
                PutResult putResult =
                    HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/MoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}",
                    transactionId, SessionInfo.OrgUnitId, (int)trayActionType, assignmentId, trayId, remarks, SessionInfo.CurrentUser.Id), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MoveTransactionsList(string TransactionsIds, TrayActionType TrayActionType, int TrayId, string pageSize, int? assignmentId, string remarks)
        {
            try
            {
                string message = string.Empty;
                if(remarks != null)
                {
                    int remarksId = int.Parse(remarks);
                    remarks = LookupsHelper.GetLookupItem(remarksId, SessionInfo.CultureShortName).Result.Text;
                }
                
                PostResult putResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/MoveTransactionsList?transactionsIds={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}", TransactionsIds, SessionInfo.OrgUnitId, (int)TrayActionType, assignmentId, TrayId, remarks, SessionInfo.CurrentUser.Id), null).Result;
                
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult MoveElectronicTransactionsCopies(string TransactionsIds, TrayActionType TrayActionType, int TrayId, string pageSize, int? assignmentId, string remarks)
        {
            try
            {
                string message = string.Empty;

                PostResult putResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/MoveTransactionsList?transactionsIds={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}", TransactionsIds, SessionInfo.OrgUnitId, (int)TrayActionType, assignmentId, TrayId, remarks, SessionInfo.CurrentUser.Id), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult OutboundCertificate(int transactionId)
        {
            try
            {
                GetResult<OutboundCertificateDTO> outboundCertificateDTO =
                    HttpClientWrapper<GetResult<OutboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetOutboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, transactionId)).Result;

                OutboundCertificateVM outboundCertificateVM = OutboundCertificateMapper.Map(outboundCertificateDTO.Result);
                outboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                IAjaxGrid names = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.Names, 1, outboundCertificateVM.Names.Count(), false);
                ViewData["NamesData"] = names;
                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                return View("~/Areas/User/Views/File/_OutboundCertificatePartial.cshtml", outboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SetTransactionAssignmentToViewedByTransactionId(int transactionId)
        {
            try
            {
                string message = string.Empty;
                PutResult putResult =
                    HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/SetTransactionAssignmentToViewedByTransactionId?transactionId={0}", transactionId), null).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult BatchTransactions(string transIds)
        {
            try
            {

                TransactionsBatchVM transactionsBatchVM = new TransactionsBatchVM();
                transactionsBatchVM.TransIds = new List<int>();

                string[] ids = transIds.Split(',');

                foreach (string id in ids)
                {
                    if (id != string.Empty)
                    {
                        transactionsBatchVM.TransIds.Add(Convert.ToInt32(id));
                    }
                }

                return View("~/Areas/User/Views/File/_BatchTransactionsPartial.cshtml", transactionsBatchVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult InboundCertificate(int transactionId)
        {
            try
            {
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                      HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetInboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, transactionId)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                IAjaxGrid names = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names, 1, inboundCertificateVM.Names.Count(), true);
                ViewData["NamesData"] = names;
                IAjaxGrid assignments = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments, 1, inboundCertificateVM.Assignments.Count(), true);
                ViewData["AssignmentsData"] = assignments;
                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                return View("~/Areas/User/Views/File/_InboundCertificatePartial.cshtml", inboundCertificateDTO.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Assignmnets

        [HttpPost]
        public ActionResult OpenAssignment(string transactionIds, int? assignmentId)
        {
            try
            {
                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;
                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);

                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);

                ViewData["hdnAssignmentTransactionId"] = transactionIds;

                ViewData["ActionData"] = GetActions();
                ViewData["AssignmentGroupData"] = GetUserAssignmentGroups();
                ViewData["HasAssignmentPaper"] = CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = CheckOrgUnitIsAllowedToCreateGroup();

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

                if (!string.IsNullOrEmpty(ViewData["ActionData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["ActionData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }

                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_AssignmentsPartial.cshtml", new TransactionAssignmentVM()) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult OpenManagerAssignment(string transactionIds, int assignmentId)
        {
            try
            {
                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;
                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);

                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);

                ViewData["hdnAssignmentTransactionId"] = transactionIds;
                ViewData["ActionData"] = GetActions();
                ViewData["AssignmentGroupData"] = GetUserAssignmentGroups();
                ViewData["HasAssignmentPaper"] = CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = CheckOrgUnitIsAllowedToCreateGroup();

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

                if (!string.IsNullOrEmpty(ViewData["ActionData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["ActionData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }

                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;

                ViewData["AssignmentId"] = assignmentId;
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_ManagerAssignmentsPartial.cshtml", new TransactionAssignmentVM()) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetAssignmentGroups(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                    HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLetterTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionType={0}", transactionCategory)).Result;

                List<LetterTypeVM> letterTypeVMs = LetterTypeMapper.Map(letterTypeDTOs.Result);
                if (letterTypeVMs != null)
                {
                    foreach (LetterTypeVM letterTypeVM in letterTypeVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = letterTypeVM.Id.ToString(),
                            Label = letterTypeVM.LocalName
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

        protected string GetActions()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);
            if (processVMs != null)
            {
                foreach (ActionVM actionVM in processVMs)
                {
                    AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                    {
                        Value = actionVM.Id.ToString(),
                        Label = actionVM.LocalName,
                        Parameters = new object[1]
                    };

                    autoCompleteDataSource.Parameters[0] = actionVM.TypeId;

                    dataSource.Add(autoCompleteDataSource);
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        protected string GetUserAssignmentGroups()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            GetResult<List<AssignmentGroupDTO>> assignmentGroupDTOs =
                    HttpClientWrapper<GetResult<List<AssignmentGroupDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserAssignmentGroups?cultureName={0}&userId={1}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id)).Result;

            List<AssignmentGroupVM> assignmentGroupVMs = AssignmentGroupMapper.Map(assignmentGroupDTOs.Result);
            if (assignmentGroupVMs != null)
            {
                foreach (AssignmentGroupVM assignmentGroupVM in assignmentGroupVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = assignmentGroupVM.Id.ToString(),
                        Label = assignmentGroupVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        protected bool CheckOrgUnitHasAssignmentPaper()
        {
            GetResult<bool> getResult =
               HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Transaction/CheckOrgUnitHasAssignmentPaper?orgUnitId={0}", SessionInfo.OrgUnitId)).Result;

            return getResult.Result;

        }


        protected bool CheckOrgUnitIsAllowedToCreateGroup()
        {
            GetResult<bool> getResult =
               HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Transaction/CheckOrgUnitIsAllowedToCreateGroup?orgUnitId={0}", SessionInfo.OrgUnitId)).Result;

            return getResult.Result;

        }

        public ActionResult AssignmentGroupAdd()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = cultureDTOs.Result;
                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentGroupDetailVM>(), 1, 0, true);
                ViewData["AssignmentGroupDetailData"] = grid;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);


                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);

                return PartialView("~/Areas/User/Views/Shared/_AssignmentCreateGroupPartial.cshtml");
            }


            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddGroup(AssignmentGroupVM assignmentGroupVM, string hdnAssignmentDetails, string hdnAssignmentGroups)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<AssignmentGroupDetailVM> assignmentGroupDetailVMs = new List<AssignmentGroupDetailVM>();
                List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();



                if (!string.IsNullOrEmpty(hdnAssignmentDetails))
                {
                    assignmentGroupDetailVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentDetails, typeof(List<AssignmentGroupDetailVM>)) as List<AssignmentGroupDetailVM>);
                }

                if (!string.IsNullOrEmpty(hdnAssignmentGroups))
                {
                    dataSource.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentGroups, typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }

                assignmentGroupVM.GroupDetails = new List<AssignmentGroupDetailVM>();

                assignmentGroupVM.GroupDetails.AddRange(assignmentGroupDetailVMs);

                PostResult postResult =
                  HttpClientWrapper<PostResult>.PostRequest(string.Format("api/UserProfile/PostAssignmentGroup?cultureName={0}", SessionInfo.CultureShortName), assignmentGroupVM).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string text = assignmentGroupVM.GroupName.Where(l => l.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;
                dataSource.Add(new AutoCompleteDataSource() { Label = text, Value = postResult.Id.ToString() });
                string data = JsonConvert.SerializeObject(dataSource);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.AssignmentGroup.CreateSucceeded");

                return Json(new { Text = text, GroupsData = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddAssignmentDetail(AssignmentGroupDetailVM assignmentGroupDetailVM, string hdnAssignmentDetails)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AssignmentGroupDetailVM> assignmentGroupDetailVMs = new List<AssignmentGroupDetailVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentDetails))
                {
                    assignmentGroupDetailVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentDetails, typeof(List<AssignmentGroupDetailVM>)) as List<AssignmentGroupDetailVM>);
                }
                bool checkDetail = true;
                assignmentGroupDetailVMs.ForEach(a =>
                {
                    if (a.OrgUnitId == assignmentGroupDetailVM.OrgUnitId && a.UserProfileId == assignmentGroupDetailVM.UserProfileId)
                    {
                        checkDetail = false;
                    }
                });
                if (checkDetail)
                {
                    assignmentGroupDetailVMs.Add(assignmentGroupDetailVM);
                }
                string data = JsonConvert.SerializeObject(assignmentGroupDetailVMs);
                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(assignmentGroupDetailVMs, 1, assignmentGroupDetailVMs.Count, true);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentGroupDetailsGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridAssignmentDetails(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AssignmentGroupDetailVM> assignmentGroupDetailVMs = new List<AssignmentGroupDetailVM>();
                if (!string.IsNullOrEmpty(param))
                {
                    assignmentGroupDetailVMs.AddRange(javaScriptSerializer.Deserialize(param, typeof(List<AssignmentGroupDetailVM>)) as List<AssignmentGroupDetailVM>);
                }
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(assignmentGroupDetailVMs, page.HasValue ? page.Value : 1, assignmentGroupDetailVMs.Count, page.HasValue);
                return Json(new { Html = grid.ToJson("_AssignmentGroupDetailsGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteAssignmentDetails(string ids, string hdnAssignmentDetails)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AssignmentGroupDetailVM> assignmentGroupDetailVMs = new List<AssignmentGroupDetailVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentDetails))
                {
                    assignmentGroupDetailVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentDetails, typeof(List<AssignmentGroupDetailVM>)) as List<AssignmentGroupDetailVM>);
                }
                List<int> index = ids.Split(',').Select(int.Parse).ToList();
                List<AssignmentGroupDetailVM> DeletedData = new List<AssignmentGroupDetailVM>();
                index.ForEach(i =>
                {
                    DeletedData.Add(assignmentGroupDetailVMs[i]);
                });
                DeletedData.ForEach(d => assignmentGroupDetailVMs.Remove(d));
                string data = JsonConvert.SerializeObject(assignmentGroupDetailVMs);
                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(assignmentGroupDetailVMs, 1, assignmentGroupDetailVMs.Count, true);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.AssignmentDetail.DeleteSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentGroupDetailsGridPartial", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult AddAssignmentIndividual(TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                if (!transactionAssignmentVMs.Any())
                {
                    transactionAssignmentVMs.Add(transactionAssignmentVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);
                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);
                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentIndividualGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetAssignmentIndividual(int id, string hdnAssignmentIndividualData)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();
                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                transactionAssignmentVM = transactionAssignmentVMs[id];
                //       GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                //HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //       List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
                //       ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, transactionAssignmentVM.ToOrgUnitId);
                ViewData["ToUserAssignment"] = GetUsersByOrgUnitId(transactionAssignmentVM.ToOrgUnitId, true);
                ViewData["ActionData"] = GetActions();
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentIndividualPartial", transactionAssignmentVM), Index = id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult EditAssignmentIndividual(int hdnIndexIndividual, TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                bool checkDetail = true;
                transactionAssignmentVMs.ForEach(a =>
                {
                    if (a.ToOrgUnitId == transactionAssignmentVM.ToOrgUnitId && a.ToUserId == transactionAssignmentVM.ToUserId
                        && transactionAssignmentVMs.IndexOf(a) != hdnIndexIndividual)
                    {
                        checkDetail = false;
                    }
                });
                if (checkDetail)
                {
                    transactionAssignmentVMs[hdnIndexIndividual] = transactionAssignmentVM;
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);
                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);
                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentIndividualGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridAssignmentIndividual(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(param))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(param, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, page.HasValue ? page.Value : 1, transactionAssignmentVMs.Count, page.HasValue);
                return Json(new { Html = grid.ToJson("_AssignmentIndividualGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteAssignmentIndividuals(string ids, string hdnAssignmentIndividualData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                List<int> index = ids.Split(',').Select(int.Parse).ToList();
                List<TransactionAssignmentVM> DeletedData = new List<TransactionAssignmentVM>();
                index.ForEach(i =>
                {
                    DeletedData.Add(transactionAssignmentVMs[i]);
                });
                DeletedData.ForEach(d => transactionAssignmentVMs.Remove(d));
                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);
                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.DeleteSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentIndividualGridPartial", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information, Ids = ids }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult AddAssignmentGroup(TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentGroupData, string hdnDetailAssignmentGroupData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                List<TransactionAssignmentVM> assignmentGroupDetails = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentGroupData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentGroupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                if (!string.IsNullOrEmpty(hdnDetailAssignmentGroupData))
                {
                    assignmentGroupDetails.AddRange(javaScriptSerializer.Deserialize(hdnDetailAssignmentGroupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                bool checkDetail = true;
                transactionAssignmentVMs.ForEach(a =>
                {
                    if (a.GroupId == transactionAssignmentVM.GroupId)
                    {
                        checkDetail = false;
                    }
                });
                if (checkDetail)
                {
                    GetResult<AssignmentGroupDTO> assignmentGroupDTO = HttpClientWrapper<GetResult<AssignmentGroupDTO>>.GetItemRequest(string.Format("api/UserProfile/GetAssignmentGroupById?groupId={0}&cultureName={1}", transactionAssignmentVM.GroupId, SessionInfo.CultureShortName)).Result;
                    AssignmentGroupVM assignmentGroupVM = AssignmentGroupMapper.Map(assignmentGroupDTO.Result);
                    if (assignmentGroupVM != null)
                    {
                        foreach (AssignmentGroupDetailVM assignmentGroupDetailVM in assignmentGroupVM.GroupDetails)
                        {
                            TransactionAssignmentVM groupDetails = new TransactionAssignmentVM()
                            {
                                Id = assignmentGroupDetailVM.Id,
                                GroupId = assignmentGroupVM.Id,
                                ToOrgUnitId = assignmentGroupDetailVM.OrgUnitId,
                                ToOrgUnitName = assignmentGroupDetailVM.OrgUnitName,
                                ToUserId = assignmentGroupDetailVM.UserProfileId,
                                ToUserName = assignmentGroupDetailVM.UserProfileName,
                                ActionId = (transactionAssignmentVM.ActionForAllId.HasValue) ? transactionAssignmentVM.ActionForAllId.Value : -1,
                                ActionTypeId = (transactionAssignmentVM.ActionTypeForAllId.Length > 0) ? transactionAssignmentVM.ActionTypeForAllId : null,
                                Remarks = transactionAssignmentVM.RemarksForAll
                            };
                            assignmentGroupDetails.Add(groupDetails);
                        }
                    }
                    transactionAssignmentVM.Count = assignmentGroupDetails.Where(a => a.GroupId == transactionAssignmentVM.GroupId).Count();
                    transactionAssignmentVMs.Add(transactionAssignmentVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);
                string detailData = JsonConvert.SerializeObject(assignmentGroupDetails);
                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);
                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentGroupGridPartial", grid), hdnValue = data, hdnDetailData = detailData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridAssignmentGroup(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(param))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(param, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, page.HasValue ? page.Value : 1, transactionAssignmentVMs.Count, page.HasValue);
                return Json(new { Html = grid.ToJson("_AssignmentGroupGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteAssignmentGroups(string ids, string hdnAssignmentGroupData, string hdnDetailAssignmentGroupData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                List<TransactionAssignmentVM> assignmentGroupDetails = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentGroupData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentGroupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                if (!string.IsNullOrEmpty(hdnDetailAssignmentGroupData))
                {
                    assignmentGroupDetails.AddRange(javaScriptSerializer.Deserialize(hdnDetailAssignmentGroupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                List<int> index = ids.Split(',').Select(int.Parse).ToList();
                index.ForEach(i =>
                {
                    transactionAssignmentVMs.Remove(transactionAssignmentVMs.Where(t => t.GroupId == i).SingleOrDefault());
                    assignmentGroupDetails.RemoveAll(a => a.GroupId == i);
                });
                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);
                string detailData = JsonConvert.SerializeObject(assignmentGroupDetails);
                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.DeleteSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentGroupGridPartial", grid), hdnValue = data, hdnDetailData = detailData, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Assignments.Assign)]
        [ValidateAntiForgeryToken()]
        public ActionResult SendAssignments(string hdnAssignmentIndividualData, string hdnDetailAssignmentGroupData, string hdnTransactionId, int trayId, string pageSize, int? dateType)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                if (!string.IsNullOrEmpty(hdnDetailAssignmentGroupData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnDetailAssignmentGroupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                int OrgUnitsCount = 0;
                transactionAssignmentVMs.ForEach(t => t.FromOrgUnitId = SessionInfo.OrgUnitId);
                transactionAssignmentVMs.RemoveAll(t => t.IsAssigned == false);
                for (int i = 0; i < transactionAssignmentVMs.Count(); i++)
                {
                    int count = 0;
                    if (!transactionAssignmentVMs[i].ToUserId.HasValue)
                    {
                        OrgUnitsCount++;
                    }
                    for (int j = 0; j < transactionAssignmentVMs.Count(); j++)
                    {
                        if (transactionAssignmentVMs[j].ToOrgUnitId == transactionAssignmentVMs[i].ToOrgUnitId
                            && transactionAssignmentVMs[j].ToUserId == transactionAssignmentVMs[i].ToUserId)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        transactionAssignmentVMs.Remove(transactionAssignmentVMs[i]);
                    }
                }

                string apiUrl = "api/Transaction/PostTransactionAssignments?cultureName={0}";

                List<int> transactionIds = hdnTransactionId.Split(',').Select(int.Parse).ToList();
                foreach (int id in transactionIds)
                {
                    apiUrl += "&transactionId=" + id;
                }
                PostObjectResult<List<TransactionReportInfoDTO>> postResult = HttpClientWrapper<PostObjectResult<List<TransactionReportInfoDTO>>>.PostRequest(string.Format(apiUrl, SessionInfo.CultureShortName), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                string url = MCS.UI.UrlHelper.GetBaseUri() + "/User/Home/Index";
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
                bool printDeliveryReport = false;
                bool oneDeliveryReport = false;
                int assignmentIndividualCount = transactionAssignmentVMs.Where(a => Convert.ToInt32(a.ActionTypeId[0]) != ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, SessionInfo.CultureShortName)).ToList().Count;
                if (assignmentIndividualCount > 1)
                {
                    printDeliveryReport = true;
                }
                else if (assignmentIndividualCount == 1)
                {
                    printDeliveryReport = true;
                    oneDeliveryReport = true;
                }
                string parameters = GetListTransactionParameters(null);
                TransactionDateType transactionDateType = TransactionDateType.Any;
                if (dateType.HasValue)
                {
                    transactionDateType = (TransactionDateType)dateType;
                }
                GetResult<List<TransactionTrayInfoDTO>> transactionTrayInfoDTOs =
                 HttpClientWrapper<GetResult<List<TransactionTrayInfoDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?{0}&orgUnitId={1}&trayType={2}&transactionDate={3}", parameters, SessionInfo.OrgUnitId, trayId, transactionDateType)).Result;
                if (transactionTrayInfoDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTrayInfoDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                int rowsCount = (transactionTrayInfoDTOs.RowsCount.HasValue) ? transactionTrayInfoDTOs.RowsCount.Value : 0;
                ViewData["RowsCount"] = rowsCount;
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                string partialPath = GetPartialPathByTrayId(trayId);
                GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<UserPreferenceDTO> userPreferenceResult =
                 HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
                if (userPreferenceResult != null
                    && userPreferenceResult.Result != null
                    && userPreferenceResult.StatusCode == StatusCode.Ok)
                {
                    trayDetailsDTOs.Result.ForEach(t =>
                    {
                        t.IsExcluded = userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault() != null ?
                            !userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault().IsSelected : false;
                    });
                }
                TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    url = url,
                    PrintDeliveryReport = printDeliveryReport,
                    OneDeliveryReport = oneDeliveryReport,
                    TransactionReportInfo = javaScriptSerializer.Serialize(TransactionReportInfoMapper.Map(postResult.Result)),
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result)),
                    FileMenuHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_UserFileMenuPartial.cshtml", null),
                    Count = transactionTrayInfoDTOs.RowsCount,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Assignments.Assign)]
        [ValidateAntiForgeryToken()]
        public ActionResult SendManagerAssignments(string hdnAssignmentIndividualData, string hdnDetailAssignmentGroupData, string hdnTransactionId, int trayId, string pageSize, int? dateType, int hdnAssignmentId)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                if (!string.IsNullOrEmpty(hdnDetailAssignmentGroupData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnDetailAssignmentGroupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                int OrgUnitsCount = 0;
                transactionAssignmentVMs.ForEach(t => t.FromOrgUnitId = SessionInfo.OrgUnitId);
                transactionAssignmentVMs.RemoveAll(t => t.IsAssigned == false);
                for (int i = 0; i < transactionAssignmentVMs.Count(); i++)
                {
                    int count = 0;
                    if (!transactionAssignmentVMs[i].ToUserId.HasValue)
                    {
                        OrgUnitsCount++;
                    }
                    for (int j = 0; j < transactionAssignmentVMs.Count(); j++)
                    {
                        if (transactionAssignmentVMs[j].ToOrgUnitId == transactionAssignmentVMs[i].ToOrgUnitId
                            && transactionAssignmentVMs[j].ToUserId == transactionAssignmentVMs[i].ToUserId)
                        {
                            count++;
                        }
                    }
                    if (count > 1)
                    {
                        transactionAssignmentVMs.Remove(transactionAssignmentVMs[i]);
                    }
                }
                PutResult putResult =
                                HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/MoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}", hdnTransactionId, SessionInfo.OrgUnitId, (int)TrayActionType.ManagerAssign, hdnAssignmentId, trayId), transactionAssignmentVMs).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                string url = UrlHelper.GetBaseUri() + "/User/Home/Index";
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
                bool printDeliveryReport = false;
                bool oneDeliveryReport = false;
                int assignmentIndividualCount = transactionAssignmentVMs.Where(a => Convert.ToInt32(a.ActionTypeId[0]) != ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, SessionInfo.CultureShortName)).ToList().Count;
                if (assignmentIndividualCount > 1)
                {
                    printDeliveryReport = true;
                }
                else if (assignmentIndividualCount == 1)
                {
                    printDeliveryReport = true;
                    oneDeliveryReport = true;
                }
                string parameters = GetListTransactionParameters(null);
                TransactionDateType transactionDateType = TransactionDateType.Any;
                if (dateType.HasValue)
                {
                    transactionDateType = (TransactionDateType)dateType;
                }
                GetResult<List<TransactionTrayInfoDTO>> transactionTrayInfoDTOs =
                 HttpClientWrapper<GetResult<List<TransactionTrayInfoDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?{0}&orgUnitId={1}&trayType={2}&transactionDate={3}", parameters, SessionInfo.OrgUnitId, trayId, transactionDateType)).Result;
                if (transactionTrayInfoDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTrayInfoDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                int rowsCount = (transactionTrayInfoDTOs.RowsCount.HasValue) ? transactionTrayInfoDTOs.RowsCount.Value : 0;
                ViewData["RowsCount"] = rowsCount;
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                string partialPath = GetPartialPathByTrayId(trayId);
                GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<UserPreferenceDTO> userPreferenceResult =
                HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
                if (userPreferenceResult != null
                    && userPreferenceResult.Result != null
                    && userPreferenceResult.StatusCode == StatusCode.Ok)
                {
                    trayDetailsDTOs.Result.ForEach(t =>
                    {
                        t.IsExcluded = userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault() != null ?
                            !userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault().IsSelected : false;
                    });
                }
                TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    url = url,
                    PrintDeliveryReport = printDeliveryReport,
                    OneDeliveryReport = oneDeliveryReport,
                    TransactionReportInfo = javaScriptSerializer.Serialize(putResult),
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, transactionTrayInfoDTOs.Result),
                    FileMenuHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_UserFileMenuPartial.cshtml", null),
                    Count = transactionTrayInfoDTOs.RowsCount,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult AssignmentGroupDetailsEdit(int groupId, string groupName, string groupData)
        {
            try
            {
                ViewData["GroupName"] = groupName;
                ViewData["ActionData"] = GetActions();
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(groupData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(groupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.Where(t => t.GroupId == groupId).ToList(), 1, 0, true);
                ViewData["AssignmentGroupGrid"] = grid;
                return Json(new { View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentGroupDetailEditPartial", transactionAssignmentVMs.Where(t => t.GroupId == groupId).ToList()) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateGroupDetails(List<TransactionAssignmentVM> transactionAssignmentVMs, string hdnGroupDataEdit, string hdnGroupEdit)
        {
            try
            {
                List<TransactionAssignmentVM> groupData = new List<TransactionAssignmentVM>();
                List<TransactionAssignmentVM> groups = new List<TransactionAssignmentVM>();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                if (!string.IsNullOrEmpty(hdnGroupDataEdit))
                {
                    groupData.AddRange(javaScriptSerializer.Deserialize(hdnGroupDataEdit, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                if (!string.IsNullOrEmpty(hdnGroupEdit))
                {
                    groups.AddRange(javaScriptSerializer.Deserialize(hdnGroupEdit, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                groupData.ForEach(g =>
                {
                    transactionAssignmentVMs.ForEach(t =>
                    {
                        if (g.GroupId == t.GroupId && g.Id == t.Id)
                        {
                            g.IsAssigned = t.IsAssigned;
                            g.ActionId = t.ActionId;
                        }
                    });
                }
                    );
                TransactionAssignmentVM transactionAssignmentVM = transactionAssignmentVMs.First();
                int groupId = transactionAssignmentVMs.First().GroupId;
                string actionName = transactionAssignmentVMs.First().ActionName;
                int count = transactionAssignmentVMs.Where(t => t.IsAssigned).Count();
                bool checkSameValue = transactionAssignmentVMs.All(s => s.ActionId == transactionAssignmentVM.ActionId);
                if (!checkSameValue)
                {
                    groups.Where(g => g.GroupId == groupId).FirstOrDefault().ActionNameForAll = null;
                }
                else
                {
                    groups.Where(g => g.GroupId == groupId).FirstOrDefault().ActionNameForAll = actionName;
                }
                groups.Where(g => g.GroupId == groupId).FirstOrDefault().Count = transactionAssignmentVMs.Where(t => t.IsAssigned).Count();
                string data = JsonConvert.SerializeObject(groupData);
                string groupJson = JsonConvert.SerializeObject(groups);
                return Json(new { Details = data, Groups = groupJson, Count = count, CheckSameValue = checkSameValue, ActionName = actionName, GroupId = groupId.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion Assignmnets
        #region Shared
        protected string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);
                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
                if (permissionVMs != null)
                {
                    foreach (PermissionVM permissionVM in permissionVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = permissionVM.Id.ToString(),
                            Label = permissionVM.Text
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
        protected string GetAllOrgUnitsForSearch()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                List<OrgUnitVM> OrgUnits = OrgUnitMapper.Map(orgUnitDTOs.Result);
                if (OrgUnits != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (OrgUnitVM OrgUnit in OrgUnits)
                    {
                        if (OrgUnit.ParentId == -1)
                        {
                            continue;
                        }
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = OrgUnit.Id.ToString(),
                            Label = OrgUnit.Name
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
        protected string GetAllOrgUnits()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                List<OrgUnitVM> OrgUnits = OrgUnitMapper.Map(orgUnitDTOs.Result);
                if (OrgUnits != null)
                {
                    foreach (OrgUnitVM OrgUnit in OrgUnits)
                    {
                        if (OrgUnit.ParentId == -1)
                        {
                            continue;
                        }
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = OrgUnit.Id.ToString(),
                            Label = OrgUnit.Name
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
        protected string GetConfidentialityForSearch()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
                if (permissionVMs != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (PermissionVM permissionVM in permissionVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = permissionVM.Id.ToString(),
                            Label = permissionVM.Text
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

        protected string GetPriorities()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(TransactionCategory.Inbound);
                if (priorityVMs != null)
                {
                    foreach (PriorityVM priorityVM in priorityVMs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = priorityVM.Id.ToString(),
                            Label = priorityVM.LocalName
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
        protected string GetPrioritiesForSearch()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(TransactionCategory.Inbound);

                if (priorityVMs != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (PriorityVM priorityVM in priorityVMs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = priorityVM.Id.ToString(),
                            Label = priorityVM.LocalName
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

        protected string GetTransactionCategories()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> transactionTypes = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionTypes.Result;
                if (lookupVMs != null)
                {
                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        if (transactionType.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) && transactionType.Id != (int)TransactionCategory.None)
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = transactionType.Id.ToString(),
                                Label = transactionType.Text
                            });
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetTransactionTypesForFilters()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> transactionTypes = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionTypes.Result;
                if (lookupVMs != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        //transactionType.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) &&
                        if (transactionType.Id != (int)TransactionCategory.None && transactionType.Id != TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = transactionType.Id.ToString(),
                                Label = transactionType.Text
                            });
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetTransactionSearchTypes()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        if (transactionType.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) && transactionType.Id != TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionStatus, SessionInfo.CultureShortName) && transactionType.Id != (int)TransactionCategory.None)
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = transactionType.Id.ToString(),
                                Label = transactionType.Text
                            });
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetTransactionDateTypes(TrayType trayType)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> transactionTypes = LookupsHelper.GetLookupItems(LookupCategory.TransactionDateType, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionTypes.Result;
                if (lookupVMs != null)
                {
                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        if (trayType == TrayType.MyTransactions)
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = transactionType.Id.ToString(),
                                Label = transactionType.Text
                            });
                        }
                        else
                        {
                            if (transactionType.Id != (int)TransactionDateType.HasDate
                            && transactionType.Id != (int)TransactionDateType.Late)
                            {
                                dataSource.Add(new AutoCompleteDataSource()
                                {
                                    Value = transactionType.Id.ToString(),
                                    Label = transactionType.Text
                                });
                            }
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private TreeViewModel BulidSuggestedTopicsTree(List<SuggestedTopicVM> suggestedTopicVMs)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();
            if (suggestedTopicVMs == null)
            {
                return tree;
            }
            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };
            suggestedTopicVMs.Where(s => suggestedTopicVMs.All(sc => sc.Id != s.ParentId)).ToList().ForEach(s =>
            {
                tree.RootNode.Childs.Add(AddSubjectClassificationsChilds(suggestedTopicVMs, s));
            });
            return tree;
        }
        private TreeNode AddSubjectClassificationsChilds(List<SuggestedTopicVM> suggestedTopicVMs, SuggestedTopicVM suggestedTopicVM)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = suggestedTopicVM.Id.ToString(),
                IsSelected = suggestedTopicVM.IsSelected,
                Selectable = !suggestedTopicVM.IsGroup,
                Name = suggestedTopicVM.LocalName,
                Id = suggestedTopicVM.Id
            };
            suggestedTopicVMs.Where(s => s.ParentId == suggestedTopicVM.Id).ToList().ForEach(s =>
            {
                treeNode.Childs.Add(AddSubjectClassificationsChilds(suggestedTopicVMs, s));
            });
            return treeNode;
        }
        private TreeViewModel BulidSubjectClassificationsTree(List<SubjectClassificationVM> subjectClassificationVMs)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();
            if (subjectClassificationVMs == null)
            {
                return tree;
            }
            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };
            subjectClassificationVMs.Where(s => subjectClassificationVMs.All(sc => sc.Id != s.ParentId)).ToList().ForEach(s =>
            {
                tree.RootNode.Childs.Add(AddSubjectClassificationsChilds(subjectClassificationVMs, s));
            });
            return tree;
        }
        private TreeNode AddSubjectClassificationsChilds(List<SubjectClassificationVM> subjectClassificationVMs, SubjectClassificationVM subjectClassificationVM)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = subjectClassificationVM.Id.ToString(),
                IsSelected = subjectClassificationVM.IsSelected,
                Selectable = !subjectClassificationVM.IsGroup,
                Name = subjectClassificationVM.LocalName,
                Id = subjectClassificationVM.Id
            };
            subjectClassificationVMs.Where(s => s.ParentId == subjectClassificationVM.Id).ToList().ForEach(s =>
            {
                treeNode.Childs.Add(AddSubjectClassificationsChilds(subjectClassificationVMs, s));
            });
            return treeNode;
        }
        [HttpPost]
        public string GetUsersByOrgUnitId(int? id, bool addSelectOption = false)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!id.HasValue || id == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (userProfileVMs != null)
                {
                    foreach (UserProfileVM userProfileVM in userProfileVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName
                        });
                    }
                }
                if (addSelectOption)
                {
                    dataSource = dataSource.Prepend(new AutoCompleteDataSource()
                    {
                        Value = (-1).ToString(),
                        Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Trays.Orgunit")
                    }).ToList();
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Shared

        #region OutboundExternal




        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateExternalOutbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOutboundExternal(AddOutboundExternalVM outboundExternalAddVM, string hdnDraftId, string hdnAttachments, string hdnNames, string hdnLinks, string hdnCopies, string hdnExternalCopies, string hdnArchivigdata, string hdnArchivigMainDocumentdata, int trayId, string pageSize, int? dateType)
        {
            try
            {
                string message = string.Empty;

                outboundExternalAddVM.OutboundExternalBasicInfo.PreparationEntityId = outboundExternalAddVM.OrgUnitId = SessionInfo.OrgUnitId;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                outboundExternalAddVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                outboundExternalAddVM.Names = javaScriptSerializer.Deserialize(hdnNames, typeof(List<TransactionNameVM>)) as List<TransactionNameVM>;
                outboundExternalAddVM.Links = javaScriptSerializer.Deserialize(hdnLinks, typeof(List<TransactionLinkVM>)) as List<TransactionLinkVM>;
                outboundExternalAddVM.Copies = javaScriptSerializer.Deserialize(hdnCopies, typeof(List<TransactionCopyVM>)) as List<TransactionCopyVM>;
                outboundExternalAddVM.ExternalCopies = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(List<TransactionExternalCopyVM>)) as List<TransactionExternalCopyVM>;

                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigMainDocumentdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;
                transactionArchiveVMs.AddRange(javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>);

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                if (documentData != null)
                {
                    transactionArchiveVMs.ForEach(t =>
                    {
                        if (t.IsMainDocument)
                        {
                            outboundExternalAddVM.DocumentVM = new DocumentVM();
                            outboundExternalAddVM.DocumentVM.Content = documentData[t.Id];
                            outboundExternalAddVM.DocumentVM.Size = documentData[t.Id].Length;
                            outboundExternalAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        }
                        else
                        {
                            outboundExternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM = new DocumentVM();
                            outboundExternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Content = documentData[t.Id];
                            outboundExternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Size = documentData[t.Id].Length;
                            outboundExternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        }
                    });
                }

                PostObjectResult<TransactionDetailsDTO> postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest(string.Format("api/Transaction/CreateOutboundExternal?transactionId={0}&trayId={1}", hdnDraftId, trayId), outboundExternalAddVM).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                string parameters = GetListTransactionParameters(null);
                TransactionDateType transactionDateType = TransactionDateType.Any;
                if (dateType.HasValue)
                {
                    transactionDateType = (TransactionDateType)dateType;
                }
                GetResult<List<TransactionTrayInfoDTO>> transactionTrayInfoDTOs =
                 HttpClientWrapper<GetResult<List<TransactionTrayInfoDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?{0}&orgUnitId={1}&trayType={2}&transactionDate={3}", parameters, SessionInfo.OrgUnitId, trayId, transactionDateType)).Result;
                if (transactionTrayInfoDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTrayInfoDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                int rowsCount = (transactionTrayInfoDTOs.RowsCount.HasValue) ? transactionTrayInfoDTOs.RowsCount.Value : 0;
                ViewData["RowsCount"] = rowsCount;
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                string partialPath = GetPartialPathByTrayId(trayId);
                GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<UserPreferenceDTO> userPreferenceResult =
                        HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
                List<TrayDetailsVM> trayDetailsVMs = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (userPreferenceResult != null
                    && userPreferenceResult.Result != null
                    && userPreferenceResult.StatusCode == StatusCode.Ok)
                {
                    trayDetailsVMs.ForEach(t =>
                    {
                        t.IsExcluded = userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault() != null ?
                            !userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault().IsSelected : false;
                    });
                }
                TempData["TrayDetails"] = trayDetailsVMs;
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundExternal.AddSucceeded");


                return Json(new { MessageText = message, MessageType = MessageType.Information, OutboundExternalNumber = postResult.Result.Number, Id = postResult.Result.Id, Date = postResult.Result.HijriDate }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void InitializeOutboundExternal()
        {
            TransactionCategory transactionCategory = TransactionCategory.ExternalOutbound;
            IAjaxGrid grid = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAttachmentVM>(), 1, 0, true);
            ViewData["AttachmentData"] = grid;
            IAjaxGrid gridNames = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionNameVM>(), 1, 0, true);
            ViewData["NamesData"] = gridNames;
            IAjaxGrid gridLinks = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionLinkVM>(), 1, 0, true);
            ViewData["LinksData"] = gridLinks;
            IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, true);
            ViewData["ArchivingData"] = gridArchiving;

            IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, true);
            //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

            ViewData["TransactionCategory"] = (int)transactionCategory;
            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);
            GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
            ViewData["ExternalPartiesData"] = (externalPartyDTOs.Result != null) ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;
            ViewData["LinkTypeData"] = TransactionHelper.GetLinkTypes(transactionCategory);
            ViewData["PrioritiesData"] = TransactionHelper.GetPriorities(transactionCategory);
            ViewData["AttachmentsTypeData"] = TransactionHelper.GetAttachmentTypes(transactionCategory);
            ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
            ViewData["LetterTypeData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
            ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
            //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(new List<TransactionAttachmentVM>());
            //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(new List<TransactionNameVM>());
            //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(new List<TransactionLinkVM>());
            //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
            //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(new List<AutoCompleteDataSource>());
            ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
            ViewData["OrgUnitsUsersData"] = null;
            ViewData["DocumentId"] = null;

            Session["DocumentData"] = null;
            ViewData["SessionArchiveDocumentKey"] = Guid.NewGuid().ToString();
            //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
            //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
            //      GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
            //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            //      ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));
        }
        [HttpGet]
        public string GetManagersByPartyId(int? id)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!id.HasValue || id == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }
                GetResult<List<ManagerDTO>> managerDTOs =
                HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetManagersByPartyId?cultureName={0}&partyId={1}", SessionInfo.CultureShortName, id)).Result;
                List<ManagerVM> managerVMs = ManagerMapper.Map(managerDTOs.Result);
                if (managerVMs != null)
                {
                    foreach (ManagerVM manager in managerVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = manager.Id.ToString(),
                            Label = manager.LocalName
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
        #endregion OutboundExternal
        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult Manager(int? page)
        {
            try
            {
                //LoadSideBarMenu();
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Manager))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InManagerTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                //  GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                //HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


                //  if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                //  {
                //      throw new Exception(trayDetailsDTOs.StatusCode.ToString());
                //  }

                //  if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.Manager))
                //  {
                //      return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                //  }

                GetResult<TrayDetailsDTO> trayDetailsDTO = HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                                                            SessionInfo.OrgUnitId,
                                                            (int)TrayType.Manager,
                                                            page ?? 1,
                                                            UIHelper.PageSize,
                                                            SessionInfo.CultureShortName)).Result;

                //List<ExternalPartyDTO> parties =
                // HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result.Result;

                //IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                //if (parties != null)
                //{
                //    dataSource.Add(UIHelper.GetDefaultSelect());

                //    foreach (ExternalPartyDTO item in parties)
                //    {
                //        dataSource.Add(new AutoCompleteDataSource()
                //        {
                //            Value = item.Id.ToString(),
                //            Label = item.LocalName
                //        });
                //    }
                //}
                //ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(dataSource);
                ViewData["PrioritiesData"] = GetPrioritiesForSearch();
                ViewData["ConfidentialityData"] = GetConfidentialityForSearch();
                ViewData["ConfidentialityDataList"] = GetConfidentialityLevel();
                ViewData["PrioritiesDataList"] = GetPriorities();
                ViewData["Orgunits"] = GetAllOrgUnitsForSearch();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.Manager);
                ViewData["FileTitle"] = DbRes.TResource("User.File.Manager");
                ViewData["dateType"] = TransactionDateType.Any;
                ViewData["RowsCount"] = (trayDetailsDTO.RowsCount.HasValue) ? trayDetailsDTO.RowsCount.Value : 0;
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["LetterTypeData"] = GetTransactionSearchTypes();
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["SourceTypeAscDesc"] = TransactionHelper.GetBySourceTypeAscDesc();
                ViewData["ConfidentialityAscDesc"] = TransactionHelper.GetByConfidentialityAscDesc();
                ViewData["PriorityAscDesc"] = TransactionHelper.GetByPriorityAscDesc();
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);
                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Manager_Item" : "_Manager_TableItem";

                    ViewData["GridName"] = "Manager";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult DraftOutbound(int? page)
        {
            try
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.DraftOutbound))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                //   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                //{
                //    var message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                //if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.DraftOutbound))
                //{
                //    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                //}
                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.DraftOutbound,
                     page ?? 1,
                     settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.DraftOutbound);
                ViewData["FileTitle"] = DbRes.TResource("User.File.OutboundTransactions");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);

                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);

                Dictionary<int, int> Ids = trayDetailsDTO.Result.TransactionTrayInfoDTOs != null ?
                   trayDetailsDTO.Result.TransactionTrayInfoDTOs.ToDictionary(i => i.TransactionDetailsInfoDTOs.Id, i => i.TransactionDetailsInfoDTOs.TransactionCategoryId)
                   : new Dictionary<int, int>();
                Session["OutBoundNextPreviousIds"] = Ids;
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_DraftOutbound_Item" : "_DraftOutbound_TableItem";
                    ViewData["GridName"] = "DraftOutboundGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception ex)
            {
                if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.DraftOutboundNotAuthorized)
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult ElcOutBound(int? page)
        {
            try
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.CopiesOutbound))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.ElcOutBound,
                     page ?? 1,
                     settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.ElcOutBound);
                ViewData["FileTitle"] = DbRes.TResource("User.File.OutboundTransactions");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);

                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_ElcOutbound_Item" : "_ElcOutbound_TableItem";
                    ViewData["GridName"] = "ElcOutboundGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception ex)
            {
                if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.DraftOutboundNotAuthorized)
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                throw;
            }
        }

        public ActionResult OutBoundExternal(int? page)
        {
            try
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.CopiesOutbound))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.OutboundExternal,
                     page ?? 1,
                     settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.OutboundExternal);
                ViewData["FileTitle"] = DbRes.TResource("User.File.OutboundTransactions");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);

                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_ElcOutbound_Item" : "_OutBoundExternal_TableItem";
                    ViewData["GridName"] = "OutBoundExternalGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception ex)
            {
                if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.DraftOutboundNotAuthorized)
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                throw;
            }
        }



        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult MyTransactions(int? transactionId, int? transactionTypeId)
        {
            try
            {
                DateTime before = DateTime.Now;

                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.MyTransactions))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                bool isVIPUser = (bool)SessionInfo.GetObjectFromSession(Constants.IsVIPUser);
                int pageSize = isVIPUser ? 20 : UIHelper.PageSize;

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}&OrderBy={5}",
                     SessionInfo.OrgUnitId, (int)TrayType.MyTransactions, 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : pageSize, SessionInfo.CultureShortName, "Number")).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }

                ViewData["ConfidentialityDataList"] = TransactionHelper.GetTransactionConfidentialityForSearch();
                ViewData["PrioritiesData"] = GetPrioritiesForSearch();
                ViewData["LetterTypeData"] = GetLetterTypesForSearch(TransactionCategory.Inbound);
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypesForSearch(TransactionCategory.Inbound);
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.MyTransactions);
                ViewData["TransactionTypesForFilters"] = GetTransactionTypesForFilters();

                ViewData["dateType"] = TransactionDateType.Any;
                ViewData["FileTitle"] = DbRes.TResource("User.File.MyTransactions");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
                ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;

                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ExternalPartyAscDesc"] = TransactionHelper.GetByExternalPartyAscDesc();
                ViewData["SourceTypeAscDesc"] = TransactionHelper.GetBySourceTypeAscDesc();
                ViewData["ConfidentialityAscDesc"] = TransactionHelper.GetByConfidentialityAscDesc();
                ViewData["PriorityAscDesc"] = TransactionHelper.GetByPriorityAscDesc();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["PrivecyLevelsData"] = TransactionHelper.GetPrivecyLevels(TransactionCategory.All);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                if (!SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.Private"))
                {
                    trayDetails.TransactionTrayInfoVMs = trayDetails.TransactionTrayInfoVMs.Where(f => f.TransactionDetailsInfoVM.PrivecyId.Value != (int)PrivacyOfTransactions.Private).ToList();
                }
                if (!SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.Limited"))
                {
                    trayDetails.TransactionTrayInfoVMs = trayDetails.TransactionTrayInfoVMs.Where(f => f.TransactionDetailsInfoVM.PrivecyId.Value != (int)PrivacyOfTransactions.Limited).ToList();
                }
                if (!SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.OpenByHand"))
                {
                    trayDetails.TransactionTrayInfoVMs = trayDetails.TransactionTrayInfoVMs.Where(f => f.TransactionDetailsInfoVM.PrivecyId.Value != (int)PrivacyOfTransactions.OpenByHand).ToList();
                }
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

                Dictionary<int, int> Ids = trayDetailsDTO.Result.TransactionTrayInfoDTOs != null ?
                    trayDetailsDTO.Result.TransactionTrayInfoDTOs.ToDictionary(i => i.TransactionDetailsInfoDTOs.Id, i => i.TransactionDetailsInfoDTOs.TransactionCategoryId)
                    : new Dictionary<int, int>();
                Session["InboundNextPreviousIds"] = Ids;
                DateTime after = DateTime.Now;

                Debug.WriteLine("Duration: " + (after - before).TotalMilliseconds);



                List<ExternalPartyDTO> parties =
              HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result.Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (parties != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());

                    foreach (ExternalPartyDTO item in parties)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.LocalName
                        });
                    }
                }
                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(dataSource);



                //trayDetails.IsVIPUser = (bool)SessionInfo.GetObjectFromSession(Constants.IsVIPUser);

                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);

            }
            catch (Exception ex)
            {
                if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.MyTransactionNotAuthorized)
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                throw;
            }
        }


        [CustomAuthorizationAttribute(UserClaims.Files.Withdrawal)]
        [HttpGet]
        public ActionResult Withdrawal()
        {
            try
            {

                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Withdrawal))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);


                GetResult<TrayDetailsDTO> trayDetailsDTO = new GetResult<TrayDetailsDTO>();
                //HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetWithdrawalData?transId={0}&transactionTypeId={1}&year={2}&PageIndex={3}&PageSize={4}&CultureName={5}&OrderBy={6}",
                //1, 1,1442, 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName, "Number")).Result;
                trayDetailsDTO.StatusCode = StatusCode.Ok;
                //if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                //{
                //    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                //}


                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypesForSearch(TransactionCategory.Inbound);
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.MyTransactions);
                ViewData["TransactionTypesForFilters"] = GetTransactionTypesForFilters();

                ViewData["dateType"] = TransactionDateType.Any;
                ViewData["FileTitle"] = DbRes.TResource("User.File.Withdrawal");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                //ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
                // ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;

                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ExternalPartyAscDesc"] = TransactionHelper.GetByExternalPartyAscDesc();
                ViewData["SourceTypeAscDesc"] = TransactionHelper.GetBySourceTypeAscDesc();
                ViewData["ConfidentialityAscDesc"] = TransactionHelper.GetByConfidentialityAscDesc();
                ViewData["PriorityAscDesc"] = TransactionHelper.GetByPriorityAscDesc();

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

                return View("~/Areas/User/Views/File/Withdrawal.cshtml");

            }
            catch (Exception ex)
            {
                if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.MyTransactionNotAuthorized)
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                }
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult Reservation(int? page)
        {
            try
            {
                //LoadSideBarMenu();
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Reservation))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);//hanaa no size in admin
                //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                //   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                //{
                //    var message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                //if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.Reservation))
                //{
                //    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                //}

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.Reservation,
                     page ?? 1,
                     settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }

                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["Orgunits"] = GetAllOrgUnitsForSearch();
                ViewData["OrgunitsForSearch"] = GetAllOrgUnitsForSearch();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionCategoriesData"] = GetTransactionCategories();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.Reservation);
                ViewData["FileTitle"] = DbRes.TResource("User.File.Reservation");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetailsDTO.RowsCount ?? 0, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                // TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Reservation_Item" : "_Reservation_TableItem";
                    ViewData["GridName"] = "ReservationGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult RedirectToCorrectView(string TransactionId, int TransactionTypeId)
        {
            string redirectToRouteResult = null;
            string controllerName = string.Empty;
            int trxId = int.Parse(StringCipher.Decrypt(TransactionId.Replace(" ", "+")));
            string url = $"api/Transaction/GetTransactionLight?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}";


            var editInboundDTO = HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest(url).Result;
            if (editInboundDTO.StatusCode == StatusCode.GeneralError)
            {
                string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Error };
                return RedirectToAction("DashboardHome", "Shared");
            }
            if (editInboundDTO.StatusCode == StatusCode.TransactionNotFound)
            {
                string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                return RedirectToAction("DashboardHome", "Shared");
            }
            //switch ((TransactionCategory)TransactionTypeId.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
            //{
            //    case TransactionCategory.Inbound:
            //        controllerName = "Inbound";
            //        redirectToRouteResult = Url.Action("NotificationEditor", "Inbound", new { id = TransactionId.ToString(), defaultTabId = "" });
            //        break;
            //    case TransactionCategory.InternalOutbound:
            //        controllerName = "OutboundInternal";
            //        redirectToRouteResult = Url.Action("NotificationEditor", "OutboundInternal", new { id = TransactionId.ToString(), defaultTabId = "" });
            //        break;
            //    case TransactionCategory.DraftOutbound:
            //        controllerName = "OutboundExternal";
            //        redirectToRouteResult = Url.Action("NotificationEditor", "OutboundExternal", new
            //        {
            //            id = TransactionId.ToString(),
            //            IsFromDraft = AESEncrytDecry.Base64Encode(true.ToString()),
            //            isHubEditable = AESEncrytDecry.Base64Encode(false.ToString())
            //        });
            //        break;
            //}
            redirectToRouteResult = Url.Action("MyTransactions", "File");
            return Redirect(redirectToRouteResult);
            if (editInboundDTO.Result.InboundBasicInfoEdit.DeliveryMethodId == DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && editInboundDTO.Result.InboundBasicInfoEdit.Viewed == false)
            {
                TempData["InfoMessage"] = new NotificationInformationVM
                {
                    TransactionId = trxId,
                    TransactionCategory = TransactionTypeId,
                    URL = redirectToRouteResult,
                    ControllerName = controllerName,
                    MessageType = MessageType.Information
                };
                return RedirectToAction("DashboardHome", "Shared");
            }

            return Redirect(redirectToRouteResult);
        }
        public ActionResult RedirectToOrgUnit(int TransactionId, int TransactionTypeId)
        {
            string redirectToRouteResult = Url.Action("OrgUnit", "File");
            string url = $"api/Transaction/GetTransactionAssignmentLight?transactionId={TransactionId}&orgUnitId={SessionInfo.OrgUnitId}";
            var getResult = HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest(url).Result;
            if (getResult.StatusCode == StatusCode.TransactionNotFound)
            {
                return RedirectToCorrectView(TransactionId.ToString(), TransactionTypeId);
            }
            else
            {
                return Redirect(redirectToRouteResult);
            }
        }
        //[CustomAuthorizationAttribute(UserClaims.Tasks.Tray)]
        public ActionResult RedirectToTask(int taskId, int transactonId)
        {
            string redirectToRouteResult = Url.Action("GetTasksByTabId", "File");
            string url = $"api/Transaction/GetTask?taskId={StringUtility.ValidateId(taskId.ToString())}&transactonId={transactonId}&orgUnitId={SessionInfo.OrgUnitId}";

            var getResult = HttpClientWrapper<GetResult<TaskLightDTO>>.GetItemRequest(url).Result;
            if (getResult.StatusCode == StatusCode.TaskNotFound)
            {
                string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                return RedirectToAction("DashboardHome", "Shared");
            }
            else
            {
                //Check the permisstion before
                //TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                //return RedirectToAction("DashboardHome", "Shared");

                if (getResult.Result.StatusId == TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, string.Empty) || getResult.Result.StatusId == TaskStatus.Received.LookupIdentity(LookupCategory.TaskStatus, string.Empty))
                {
                    return RedirectToAction("Tasks", "File", new { statusId = getResult.Result.StatusId });
                }

                return RedirectToAction("DashboardHome", "Shared");
            }
        }
        public ActionResult RedirectToCorrectTray(int TransactionId, int TransactionTypeId)
        {
            RedirectToRouteResult redirectToRouteResult = null;
            switch ((TransactionCategory)TransactionTypeId.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
            {
                case TransactionCategory.Inbound:
                    redirectToRouteResult = RedirectToAction("Edit", "Inbound", new { id = AESEncrytDecry.Base64Encode(TransactionId.ToString()) });
                    break;
                case TransactionCategory.InternalOutbound:
                    redirectToRouteResult = RedirectToAction("Edit", "OutboundInternal", new { id = AESEncrytDecry.Base64Encode(TransactionId.ToString()) });
                    break;
                case TransactionCategory.DraftOutbound:
                    redirectToRouteResult = RedirectToAction("Edit", "OutboundExternal", new
                    {
                        id = AESEncrytDecry.Base64Encode(TransactionId.ToString()),
                        IsFromDraft = AESEncrytDecry.Base64Encode(false.ToString()),
                        isHubEditable = AESEncrytDecry.Base64Encode(false.ToString())
                    });
                    break;
            }
            return redirectToRouteResult;
        }
        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult FollowUp(int? transactionId, int? transactionTypeId)
        {
            try
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.FollowUp))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }

                string message = string.Empty;
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InFollowUpTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}&OrderBy={5}",
                     SessionInfo.OrgUnitId, (int)TrayType.FollowUp, 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName, "AssignmentDate")).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }

                ViewData["dateType"] = TransactionDateType.Any;
                ViewData["TrayType"] = (int)TrayType.FollowUp;
                ViewData["FileTitle"] = DbRes.TResource("User.Trays.FollowUp");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
                ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ConfidentialityAscDesc"] = TransactionHelper.GetByConfidentialityAscDesc();
                ViewData["PriorityAscDesc"] = TransactionHelper.GetByPriorityAscDesc();
                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

                ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_FollowUp_Item" : "_FollowUp_TableItem";
                ViewData["GridName"] = "FollowUpGrid";

                var loggedInUserId = SessionInfo.CurrentUser.Id;
                Session["loggedInUserId]"] = loggedInUserId;

                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAction]
        public ActionResult PendingExecuting(List<int> transactionsIds, string type, int? transactionId, int? transactionTypeId, int? trayType, string tabId)
        {
            LoadSideBarMenu();
            string ids = string.Join(",", transactionsIds);
            ViewData["SaveReason"] = TransactionHelper.GetSaveReason(LookupCategory.SaveReason);

            GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetSelectedTransactions?transactionsIds={0}&CultureName={1}", ids, SessionInfo.CultureShortName)).Result;

            if (trayDetailsDTO.StatusCode != StatusCode.Ok)
            {
                throw new Exception(trayDetailsDTO.StatusCode.ToString());
            }

            if (trayType == (int)TrayType.DraftOutbound)
            {
                trayDetailsDTO.Result.TransactionTrayInfoDTOs = trayDetailsDTO.Result.TransactionTrayInfoDTOs.Where(s => s.TransactionDetailsInfoDTOs.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)).ToList();
            }
            ViewData["DeliveryMethod"] = GetDelivery(false);
            if (trayDetailsDTO.Result.TransactionTrayInfoDTOs.Count == 1)
            {

                if (trayDetailsDTO.Result.TransactionTrayInfoDTOs.FirstOrDefault().TransactionDetailsInfoDTOs.DeliveryMethodId != DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(true);
                }
            }
            else
            {
                var HasElectronic = trayDetailsDTO.Result.TransactionTrayInfoDTOs.Any(s => s.TransactionDetailsInfoDTOs.DeliveryMethodId != DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName));

                ViewData["DeliveryMethod"] = GetDelivery(!HasElectronic);
            }

            var isPaper = trayDetailsDTO.Result.TransactionTrayInfoDTOs.Any(a => a.TransactionDetailsInfoDTOs.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName));
            //ViewData["DeliveryMethod"] = GetDelivery(!isPaper);

            IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
            ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;


            ViewData["type"] = type;

            ViewData["ControllerName"] = "File";

            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }


            ViewData["hdnAssignmentTransactionId"] = transactionsIds;

            ViewData["ActionData"] = GetActions();
            ViewData["AllActionsData"] = TransactionHelper.GetAllActions();

            ViewData["AssignmentGroupData"] = GetUserAssignmentGroups();
            ViewData["HasAssignmentPaper"] = CheckOrgUnitHasAssignmentPaper();
            ViewData["IsAllowedToCreateGroup"] = CheckOrgUnitIsAllowedToCreateGroup();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

            if (!string.IsNullOrEmpty(ViewData["ActionData"].ToString()))
            {
                autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["ActionData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
            }

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            ViewData["HasActions"] = autoCompleteDataSources.Count > 0;
            ViewData["TrayType"] = type;
            ViewData["IsElcOutBound"] = trayType.HasValue && trayType.Value == (int)TrayType.ElcOutBound ? (int)TrayType.ElcOutBound : 1;
            ViewData["TabId"] = tabId;
            ViewData["ConfidentialityData"] = GetConfidentialityLevel();
            ViewData["PrioritiesData"] = GetPriorities();
            ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.MyTransactions);
            ViewData["FileTitle"] = DbRes.TResource("User.File.MyTransactions");
            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
            ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;
            var list = TrayDetailsMapper.Map(trayDetailsDTO.Result).TransactionTrayInfoVMs;
            ViewData["selectedTransIds"] = string.Empty;
            list.ForEach(tray =>
            {
                tray.TransactionDetailsInfoVM.isChecked = transactionsIds.Any(trayId => trayId == tray.TransactionDetailsInfoVM.Id);
                if (tray.TransactionDetailsInfoVM.isChecked)
                {
                    ViewData["selectedTransIds"] = ViewData["selectedTransIds"].ToString() != string.Empty ? ViewData["selectedTransIds"] + "," + tray.TransactionDetailsInfoVM.Id.ToString() : tray.TransactionDetailsInfoVM.Id.ToString();
                }
            });
            ViewData["Reporters"] = GetReporters();

            return View("~/Areas/User/Views/Editor/PendingExecuting/PendingExecutingPartial.cshtml", list);
        }

        private string GetDeliveryMethod(bool isYesseRregistered)
        {
            try
            {
                int[] yesserRegistered = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] notYesserRegistered = { DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isYesseRregistered)
                    {
                        lookups.Result = lookups.Result.Where(a => yesserRegistered.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        lookups.Result = lookups.Result.Where(a => notYesserRegistered.Contains(a.Id)).ToList();
                    }
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
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

        [HttpPost]
        public string GetDelivery(bool isPaper)
        {
            try
            {
                int[] ContainPaper = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] elctronic = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isPaper)
                    {
                        lookups.Result = lookups.Result.Where(a => ContainPaper.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        lookups.Result = lookups.Result.Where(a => elctronic.Contains(a.Id)).ToList();
                    }
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
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
        [HttpPost]
        public ActionResult AddEditorAssignmentIndividual(TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();


                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                //  bool checkDetail = true;

                //transactionAssignmentVMs.ForEach(a =>
                //{
                //    if (a.ToOrgUnitId == transactionAssignmentVM.ToOrgUnitId && a.ToUserId == transactionAssignmentVM.ToUserId)
                //    {
                //        checkDetail = false;
                //    }
                //});

                if (!transactionAssignmentVMs.Any())
                {
                    transactionAssignmentVMs.Add(transactionAssignmentVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");

                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);

                ViewData["ControllerName"] = "File";

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/PendingExecuting/_PendingAssignmentIndividualGridPartial.cshtml", grid),
                    hdnValue = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult WithdrawalAssignment(string TransactionId)
        {
            List<TransactionAssignmentVM> TransactionAssignments = new List<TransactionAssignmentVM>();
            TransactionAssignmentVM transactionAssignment = new TransactionAssignmentVM();
            string message = string.Empty;

            transactionAssignment.ToUserId = SessionInfo.CurrentUser.Id;
            transactionAssignment.ToOrgUnitId = SessionInfo.OrgUnitId;
            transactionAssignment.FromUserId = SessionInfo.CurrentUser.Id;
            transactionAssignment.FromOrgUnitId = SessionInfo.OrgUnitId;
            transactionAssignment.TrayId = (int)TrayType.MyTransactions;
            transactionAssignment.IsAssigned = true;
            transactionAssignment.IsCopy = false;
            transactionAssignment.ActionId = 7;
            transactionAssignment.DeliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
            TransactionAssignments.Add(transactionAssignment);


            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransactionWithdrawal?transactionId={0}", TransactionId), TransactionAssignmentMapper.Map(TransactionAssignments)).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");

            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                returnUrl = url
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult CheckOrgUser(TransactionAssignmentVM transactionAssignmentVM, string transIds, int trayId, string pageSize, int? dateType)
        {
            try
            {


                string message = string.Empty;

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
                string urls = $"api/Admin/GetOrgUnitById?cultureName={SessionInfo.CultureShortName}&id={transactionAssignmentVM.ToOrgUnitId}";
                GetResult<OrgStructureInfoDTO> orgStructureInfoDTO = HttpClientWrapper<GetResult<OrgStructureInfoDTO>>.GetItemRequest(urls).Result;
                if (orgStructureInfoDTO.Result.Users.Count == 0)
                {
                    message = "الادارة المحالة اليها ليس بها موظفين";
                    return Json(new
                    {
                        MessageText = message,
                        MessageType = 3,

                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,

                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }

        //GetAllowedUserAssignment(int ToUserId, int FromUserId)



        [HttpGet]
        public ActionResult GetAllowedUserAssignment(int FromUserId)
        {
            try
            {
                string message = string.Empty;
                bool isUserAllowAssignment = false;
                int selectedUserAllowedAssignmentCount = 0;
                GetResult<List<AllowedAssignmentDTO>> selectedUserAllowedAssignmentDTOs =
                HttpClientWrapper<GetResult<List<AllowedAssignmentDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAllowedAssignment?UserId={0}&cultureName={1}", FromUserId, SessionInfo.CultureShortName)).Result;
                selectedUserAllowedAssignmentCount = selectedUserAllowedAssignmentDTOs.Result != null ? selectedUserAllowedAssignmentDTOs.Result.Count : 0;
                GetResult<AllowedAssignmentDTO> AllowedUser = HttpClientWrapper<GetResult<AllowedAssignmentDTO>>.GetItemRequest(string.Format("api/UserProfile/GetAllowedUserAssignment?ToUserId={0}&FromUserId={1}", SessionInfo.CurrentUser.Id, FromUserId)).Result;
                if (AllowedUser.Result != null)
                {
                    isUserAllowAssignment = true;

                }
                AllowedUser.Result = new AllowedAssignmentDTO(); ;

                message = "لا يمكنك الأحالة للموظف الذي تم اختياره";

                return Json(new { result = AllowedUser.Result, MessageText = message, isUserAllowAssignment = isUserAllowAssignment, selectedUserAllowedAssignmentCount = selectedUserAllowedAssignmentCount }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpPost]
        public ActionResult SendAssignmentsByEditor(TransactionAssignmentVM transactionAssignmentVM, string transIds, int trayId, string pageSize, int? dateType, bool isConfirmed)
        {
            try
            {
                string message = string.Empty;

                if (transactionAssignmentVM.ToUserId == -1)
                {
                    transactionAssignmentVM.ToUserId = null;
                }
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                transactionAssignmentVMs.Add(transactionAssignmentVM);

                transactionAssignmentVMs.ForEach(t => t.FromOrgUnitId = SessionInfo.OrgUnitId);


                transactionAssignmentVMs.RemoveAll(t => t.IsAssigned == false);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", transIds.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
                bool hasPermission = SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.AssignTransactionToUnauthorize);

                if (postResult.StatusCode != StatusCode.Ok && !isConfirmed)
                {
                    if (postResult.StatusCode == StatusCode.NotSupported)
                    {
                        string Statuskey = hasPermission ? StatusCode.WarningNoPermissionToReceiveTransaction.ToString() : StatusCode.ErrorNoPermissionToReceiveTransaction.ToString();
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, Statuskey);
                        return Json(new { MessageText = message, MessageType = (hasPermission ? MessageType.Warning : MessageType.Error), isNeedConfimed = hasPermission }, JsonRequestBehavior.AllowGet);
                    }

                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransactions?sTransactionsIds={0}", transIds.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
                PutResult UpdateDelivary = HttpClientWrapper<PutResult>
                                           .PutRequest(string.Format("api/Transaction/UpdateTransactionsDelivary?transactionIds={0}&DeliveryMethodId={1}", transIds.Trim(','), transactionAssignmentVM.DeliveryMethodId), null).Result;

                if (!string.IsNullOrEmpty(transactionAssignmentVM.Remarks))
                {
                    List<string> transsIds = transIds.Trim(',').Split(',').ToList();
                    foreach (var transid in transsIds)
                    {


                        byte[] data = Encoding.Unicode.GetBytes(transactionAssignmentVM.Remarks.Trim());
                        ExplanationVM explanationVM = new ExplanationVM
                        {
                            EditorType = EditorType.Text,
                            FromUserId = SessionInfo.CurrentUser.Id,
                            Date = DateTime.Now,
                            ConfidentialityId = 30,
                            isCopies = false,
                            CanBeDeleted = false,
                            DocumentVM = new DocumentVM
                            {
                                MimeType = System.Net.Mime.MediaTypeNames.Text.Plain,
                                Content = data,
                                Size = data.Length,
                                FromEntityId = SessionInfo.OrgUnitId,
                                FromUserId = SessionInfo.CurrentUser.Id
                            }
                        };


                        PostResult postExpinationResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddTransactionExplanation?transactionId={0}",
                  transid), ExplanationMapper.Map(explanationVM)).Result;

                        if (postExpinationResult.StatusCode != StatusCode.Ok)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postExpinationResult.StatusCode.ToString());

                            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }




                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
                string urls = $"api/Admin/GetOrgUnitById?cultureName={SessionInfo.CultureShortName}&id={transactionAssignmentVM.ToOrgUnitId}";
                GetResult<OrgStructureInfoDTO> orgStructureInfoDTO = HttpClientWrapper<GetResult<OrgStructureInfoDTO>>.GetItemRequest(urls).Result;
                if (orgStructureInfoDTO.Result.Users.Count == 0)
                {
                    message = "الادارة المحالة اليها ليس بها موظفين";
                    return Json(new
                    {
                        MessageText = message,
                        MessageType = 3,
                        url = url
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    url = url
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }

        }





        [HttpPost]
        public ActionResult CancelElcOutboundWithAssignments(TransactionAssignmentVM transactionAssignmentVM, string transIds, int trayId, string pageSize, int? dateType, bool isConfirmed)
        {
            try
            {




                string message = string.Empty;

                List<int> ElctransactionsIds = transIds.Split(',').Select(int.Parse).ToList();
                if (!transactionAssignmentVM.ToUserId.HasValue || transactionAssignmentVM.ToUserId.Value < 0)
                {
                    transactionAssignmentVM.ToUserId = null;
                }
                foreach (int elctransid in ElctransactionsIds)
                {

                    PutResult putResult = HttpClientWrapper<PutResult>
                                                  .PutRequest(string.Format("api/Transaction/ConvertTransactionToDraft?TransactionId={0}", elctransid), new { })
                                                  .Result;

                    if (putResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    GetResult<DocumentDTO> oldmainDocumentDTO =
           HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetOldMainDocumentByTransactionId?transactionId={0}", elctransid)).Result;

                    if (oldmainDocumentDTO.Result?.Content != null)
                    {
                        var content = oldmainDocumentDTO.Result.Content;
                        byte[] pdf = null;

                        pdf = oldmainDocumentDTO.Result.MimeType == System.Net.Mime.MediaTypeNames.Application.Octet ? ConvertWordToPDF(Convert.ToBase64String(content)) : content;

                        putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/UpdateMainDocument_New?transactionId={0}", elctransid), pdf).Result;
                    }

                }


                string CancelELcOutBoundSuccessfully = DbRes.TValidation("User.ELcOutbound.CancelELcOutBoundSuccessfully");
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                transactionAssignmentVMs.Add(transactionAssignmentVM);

                transactionAssignmentVMs.ForEach(t => t.FromOrgUnitId = SessionInfo.OrgUnitId);
                transactionAssignmentVMs.ForEach(t => t.RemarksForAll = CancelELcOutBoundSuccessfully);

                transactionAssignmentVMs.RemoveAll(t => t.IsAssigned == false);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", transIds.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
                bool hasPermission = SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.AssignTransactionToUnauthorize);

                if (postResult.StatusCode != StatusCode.Ok && !isConfirmed)
                {
                    if (postResult.StatusCode == StatusCode.NotSupported)
                    {
                        string Statuskey = hasPermission ? StatusCode.WarningNoPermissionToReceiveTransaction.ToString() : StatusCode.ErrorNoPermissionToReceiveTransaction.ToString();
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, Statuskey);
                        return Json(new { MessageText = message, MessageType = (hasPermission ? MessageType.Warning : MessageType.Error), isNeedConfimed = hasPermission }, JsonRequestBehavior.AllowGet);
                    }

                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransactions?sTransactionsIds={0}", transIds.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
                PutResult UpdateDelivary = HttpClientWrapper<PutResult>
                                           .PutRequest(string.Format("api/Transaction/UpdateTransactionsDelivary?transactionIds={0}&DeliveryMethodId={1}", transIds.Trim(','), transactionAssignmentVM.DeliveryMethodId), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
                string urls = $"api/Admin/GetOrgUnitById?cultureName={SessionInfo.CultureShortName}&id={transactionAssignmentVM.ToOrgUnitId}";
                GetResult<OrgStructureInfoDTO> orgStructureInfoDTO = HttpClientWrapper<GetResult<OrgStructureInfoDTO>>.GetItemRequest(urls).Result;
                if (orgStructureInfoDTO.Result.Users.Count == 0)
                {
                    message = "الادارة المحالة اليها ليس بها موظفين";
                    return Json(new
                    {
                        MessageText = message,
                        MessageType = 3,
                        url = url
                    }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    url = url
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult DeleteEditorAssignmentIndividuals(string ids, string hdnAssignmentIndividualData)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                List<int> index = ids.Split(',').Select(int.Parse).ToList();

                List<TransactionAssignmentVM> DeletedData = new List<TransactionAssignmentVM>();
                index.ForEach(i =>
                {
                    DeletedData.Add(transactionAssignmentVMs[i]);
                });

                DeletedData.ForEach(d => transactionAssignmentVMs.Remove(d));

                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.DeleteSucceeded");

                ViewData["ControllerName"] = "File";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/PendingExecuting/_PendingAssignmentIndividualGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult GetEditorAssignmentIndividual(int id, string hdnAssignmentIndividualData)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();


                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }
                transactionAssignmentVM = transactionAssignmentVMs[id];

                //       GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                //HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //       ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), transactionAssignmentVM.ToOrgUnitId);
                ViewData["ToUserAssignment"] = GetUsersByOrgUnitId(transactionAssignmentVM.ToOrgUnitId, true);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["ControllerName"] = "File";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/PendingExecuting/_PendingAssignmentIndividualPartial.cshtml", transactionAssignmentVM), Index = id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult EditEditorAssignmentIndividual(int hdnIndexIndividual, TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentIndividualData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentIndividualData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }


                bool checkDetail = true;

                transactionAssignmentVMs.ForEach(a =>
                {
                    if (a.ToOrgUnitId == transactionAssignmentVM.ToOrgUnitId && a.ToUserId == transactionAssignmentVM.ToUserId
                        && transactionAssignmentVMs.IndexOf(a) != hdnIndexIndividual)
                    {
                        checkDetail = false;
                    }
                });
                if (checkDetail)
                {
                    transactionAssignmentVMs[hdnIndexIndividual] = transactionAssignmentVM;
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");

                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, true);

                ViewData["ControllerName"] = "File";

                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/PendingExecuting/_PendingAssignmentIndividualGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult SentTransactions(int? page)
        {
            try
            {
                //LoadSideBarMenu();
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.SentTransactions))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InSentTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                //   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                //{
                //    var message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                //if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.SentTransactions))
                //{
                //    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                //}

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.SentTransactions,
                     page ?? 1,
                     settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }

                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.SentTransactions);
                ViewData["FileTitle"] = DbRes.TResource("User.File.SentTransactions");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs,
                    1, trayDetailsDTO.RowsCount.HasValue ? trayDetailsDTO.RowsCount.Value : 0, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_SentTransaction_Item" : "_SentTransaction_TableItem";
                    ViewData["GridName"] = "SentTransactionGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult Saved(int? page)
        {
            try
            {
                //LoadSideBarMenu();
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Saved))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InCompleteTransactionsTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                //   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                //{
                //    var message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                //if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.Saved))
                //{
                //    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                //}
                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.Saved,
                     page ?? 1,
                     settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }
                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["ConfidentialityDataForSearch"] = GetConfidentialityForSearch();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesDataForSearch"] = GetPrioritiesForSearch();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.Saved);
                ViewData["FileTitle"] = DbRes.TResource("User.File.Saved");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Saved_Item" : "_Saved_TableItem";
                    ViewData["GridName"] = "Saved";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }

        // [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult OrgUnit(int? page, int? transactionId, int? transactionTypeId)
        {
            try
            {
                LoadSideBarMenu();
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.OrgUnit))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyOrgUnitTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                                HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>
                     .GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId, (int)TrayType.OrgUnit, page ?? 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName)).Result;
                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }
                ViewData["ConfidentialityAscDesc"] = TransactionHelper.GetByConfidentialityAscDesc();
                ViewData["PriorityAscDesc"] = TransactionHelper.GetByPriorityAscDesc();
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                ViewData["Orgunits"] = GetAllOrgUnitsForSearch();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["ConfidentialityDataList"] = GetConfidentialityForSearch();
                ViewData["PrioritiesData"] = GetPrioritiesForSearch();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.OrgUnit);
                ViewData["FileTitle"] = DbRes.TResource("User.File.OrgUnit");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
                ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;

                var transactionTrayInfoVM = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                transactionTrayInfoVM.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(transactionTrayInfoVM.TransactionTrayInfoVMs, page ?? 1, trayDetailsDTO.RowsCount ?? 0,
                    page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_OrgunitItem" : "_OrgunitTableItem";
                    ViewData["GridName"] = "OrgunitGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)transactionTrayInfoVM.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)transactionTrayInfoVM.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/OrgUnitIndex.cshtml", transactionTrayInfoVM);
            }
            catch (Exception ex)
            {
                if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.OrgUnitNotAuthorized)
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                throw;
            }
        }
        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        [HttpGet]
        public ActionResult Copies(int? page)
        {
            try
            {
                //LoadSideBarMenu();
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Copies))
                {
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InCopiesTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                //   HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
                //{
                //    var message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                //if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.Copies))
                //{
                //    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
                //}


                GetResult<TrayDetailsDTO> trayDetailsDTO =
                     HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                     SessionInfo.OrgUnitId,
                     (int)TrayType.InternalInboundCopies,
                     page ?? 1,
                      settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                     SessionInfo.CultureShortName)).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }
                ViewData["Orgunits"] = GetAllOrgUnitsForSearch();
                ViewData["OrgunitsForSearch"] = GetAllOrgUnitsForSearch();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = TransactionHelper.GetByPriorityAscDesc();
                ViewData["TransactionCategoriesData"] = GetTransactionCategories();
                ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.Copies);
                ViewData["FileTitle"] = DbRes.TResource("User.File.Copies");
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetailsDTO.RowsCount ?? 0, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);




                Dictionary<int, int?> Ids = new Dictionary<int, int?>();
                if (trayDetailsDTO.Result.TransactionTrayInfoDTOs != null)
                {
                    foreach (TransactionTrayInfoDTO transactionTrayInfoDTO in trayDetailsDTO.Result.TransactionTrayInfoDTOs)
                    {
                        if (!Ids.ContainsKey(transactionTrayInfoDTO.TransactionDetailsInfoDTOs.Id))
                            Ids.Add(transactionTrayInfoDTO.TransactionDetailsInfoDTOs.Id, transactionTrayInfoDTO.TransactionDetailsInfoDTOs.TransactionTypeId);
                    }
                }
                Session["InboundCopiesIds"] = Ids;



                if (page.HasValue)
                {
                    ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Copies_Item" : "_Copies_TableItem";
                    ViewData["GridName"] = "CopiesGrid";
                    return Json(new
                    {
                        Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                        ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
                    }, JsonRequestBehavior.AllowGet);
                }
                return View("~/Areas/User/Views/File/Index.cshtml", trayDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult RevertManagerTray(int assignmentId, int transactionId)
        {
            try
            {
                string message = string.Empty;
                GetResult<int?> tasksCount =
                    HttpClientWrapper<GetResult<int?>>.GetItemRequest(string.Format("api/Transaction/GetTasksCount?assignmentId={0}", assignmentId)).Result;
                if (tasksCount.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, tasksCount.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = DbRes.TResource("User.File.HasTasksMassage");
                return Json(new { MessageText = message, MessageType = MessageType.Information, TasksCount = tasksCount.Result.Value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult RenderUserTrayTransactions(int trayId)
        {
            try
            {
                string message = string.Empty;
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                GetResult<List<UserTransactionsTrayDTO>> userTransactionsTrayDTOs =
              HttpClientWrapper<GetResult<List<UserTransactionsTrayDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}", SessionInfo.OrgUnitId, trayId, 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName)).Result;
                if (userTransactionsTrayDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userTransactionsTrayDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["TrayType"] = trayId;
                ViewData["DataCount"] = (userTransactionsTrayDTOs.RowsCount.HasValue) ? userTransactionsTrayDTOs.RowsCount.Value : 0;
                ViewData["PageNumber"] = 1;
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/File/_TrayTransactionsPartial.cshtml", UserTransactionsTrayMapper.Map(userTransactionsTrayDTOs.Result)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetWithdrawalData(int? transId, int? transactionTypeId, int? year, int? page)
        {
            try
            {
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                //string parameters = GetListTransactionParameters(page, SortingOrders);
                //parameters += sortType != null ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
                string message = string.Empty;
                //TransactionDateType transactionDateType = TransactionDateType.Any;
                //if (dateType.HasValue)
                //{
                //    transactionDateType = (TransactionDateType)dateType;
                //}
                //else
                //{
                //    dateType = (int)TransactionDateType.Any;
                //}

                int? orgunitID = SessionInfo.OrgUnitId;
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Assignments.WithdrawTransactionFromAllCabins))
                {
                    orgunitID = null;
                }

                GetResult<TrayDetailsDTO> trayDetailsDTO =
                    HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetWithdrawalData?transId={0}&orgunitId={1}&transactionTypeId={2}&year={3}&PageIndex={4}&PageSize={5}&CultureName={6}&OrderBy={7}",
                    transId, orgunitID, transactionTypeId, year, page ?? 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName, "Number")).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(trayDetailsDTO.StatusCode.ToString());
                }

                string partialPath = null;

                if (!trayDetailsDTO.RowsCount.HasValue)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionNotFound.ToString());
                }

                // ViewData["dateType"] = dateType;
                //ViewData["TrayType"] = trayId;
                ViewData["RowsCount"] = (trayDetailsDTO.RowsCount.HasValue) ? trayDetailsDTO.RowsCount.Value : 0;


                partialPath = "~/Areas/User/Views/File/_WithdrawalBoxesPartial.cshtml";
                //TrayDetailsVM result = TrayDetailsMapper.Map(trayDetailsDTO.Result);

                //var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

                TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
                trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetailsDTO.RowsCount ?? 0, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);


                // ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Withdrawal_Item" : "_Withdrawal_TableItem";

                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["GridName"] = "WithdrawalGrid";
                return Json(new
                {
                    Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_WithdrawalBoxesPartial", this),
                    ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems,
                    Count = trayDetailsDTO.RowsCount,
                    PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                    MessageText = message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateTrayTransactions(int trayId, List<OrderingBy> SortingOrders, int? dateType, int? pageSize, string sortType, int? searchData, int? page, int? renderType)
        {
            try
            {
                if (renderType.HasValue)
                    SessionInfo.SetObjectInSession(renderType.Value, "DefaultDisplay");

                string parameters = GetListTransactionParameters(page, SortingOrders);
                parameters += sortType != null ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
                string message = string.Empty;
                TransactionDateType transactionDateType = TransactionDateType.Any;
                if (dateType.HasValue)
                {
                    transactionDateType = (TransactionDateType)dateType;
                }
                else
                {
                    dateType = (int)TransactionDateType.Any;
                }

                var transactionTrayInfoDTOs = HttpClientWrapper<GetResult<List<TransactionTrayInfoDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?{0}&orgUnitId={1}&trayType={2}&transactionDate={3}",
                    parameters,
                    SessionInfo.OrgUnitId,
                    trayId,
                    transactionDateType)).Result;

                if ((int)TrayType.SpecialCopies == trayId ||
                    (int)TrayType.Copies == trayId ||
                    (int)TrayType.CopiesOutbound == trayId ||
                    (int)TrayType.InternalInboundCopies == trayId ||
                    (int)TrayType.CopiesOutbound == trayId)
                {
                    StoreCopiesIds(transactionTrayInfoDTOs.Result, trayId);
                }

                if (transactionTrayInfoDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTrayInfoDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string partialPath = null;

                if (!transactionTrayInfoDTOs.RowsCount.HasValue)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionNotFound.ToString());
                }

                ViewData["dateType"] = dateType;
                ViewData["TrayType"] = trayId;
                ViewData["RowsCount"] = (transactionTrayInfoDTOs.RowsCount.HasValue) ? transactionTrayInfoDTOs.RowsCount.Value : 0;

                TrayType trayType = (TrayType)trayId;

                switch (trayType)
                {
                    case (TrayType.DraftOutbound):
                    case (TrayType.DeletedDraftOutbound):
                        {

                            partialPath = "~/Areas/User/Views/File/_TransactionDraftOutboundBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_DraftOutbound_Item" : "_DraftOutbound_TableItem";
                            ViewData["GridName"] = "DraftOutboundGrid";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);

                        }
                    case (TrayType.ElcOutBound):
                        {
                            partialPath = "~/Areas/User/Views/File/_TransactionElcOutboundBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_ElcOutbound_Item" : "_ElcOutbound_TableItem";
                            ViewData["GridName"] = "ElcOutboundGrid";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);

                        }

                    case (TrayType.OutboundExternal):
                        {
                            partialPath = "~/Areas/User/Views/File/_TransactionOutBoundExternalBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InOutboundTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_ElcOutbound_Item" : "_OutBoundExternal_TableItem";
                            ViewData["GridName"] = "OutBoundExternalGrid";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);

                        }
                    case (TrayType.Copies):
                    case (TrayType.CopiesOutbound):
                    case (TrayType.InternalInboundCopies):
                    case (TrayType.SpecialCopies):
                        {
                            partialPath = "~/Areas/User/Views/File/_TransactionsCopiesBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);

                            GetResult<SettingDTO> SettingValue = null;
                            if (trayType == TrayType.Copies)
                            {
                                SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InCopiesTray)).Result;
                            }
                            else
                            {
                                SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InCopiesTray)).Result;
                            }
                            var settingVM = SettingMapper.Map(SettingValue.Result);

                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Copies_Item" : "_Copies_TableItem";
                            ViewData["GridName"] = "CopiesGrid";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    case (TrayType.SentTransactions):
                        {
                            partialPath = "~/Areas/User/Views/File/_SentTransactionsBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InSentTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Sent_Item" : "_Sent_TableItem";
                            ViewData["GridName"] = "SentGrid";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    case (TrayType.Saved):
                    case (TrayType.SavedCopies):
                        {
                            partialPath = "~/Areas/User/Views/File/_SavedBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InCompleteTransactionsTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Saved_Item" : "_Saved_TableItem";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            ViewData["GridName"] = "SavedGrid";
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    case (TrayType.OrgUnit):
                        {
                            partialPath = "~/Areas/User/Views/File/_OrgunitBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyOrgUnitTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            string GridSize = settingVM.Value;
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, Convert.ToInt32(GridSize));
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Orgunit_Item" : "_Orgunit_TableItem";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            ViewData["GridName"] = "OrgunitGrid";
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    case (TrayType.Manager):
                        {
                            partialPath = "~/Areas/User/Views/File/_ManagerBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InManagerTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            string GridSize = settingVM.Value;
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Manager_Item" : "_Manager_TableItem";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            ViewData["GridName"] = "ManagerGrid";
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    case (TrayType.FollowUp):
                    case (TrayType.FollowUpReminder):
                    case (TrayType.FollowUpUnderProcess):
                    case (TrayType.FollowUpComplete):
                    case (TrayType.FollowUpCanceld):
                    case (TrayType.FollowUpEscalation):
                    case (TrayType.FollowUpLate):
                        {
                            partialPath = "~/Areas/User/Views/File/_TransactionsFollowUpBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InFollowUpTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_FollowUp_Item" : "_FollowUp_TableItem";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            ViewData["GridName"] = "FollowUpGrid";
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    case (TrayType.Reservation):
                    case (TrayType.ReservedExternalOutbound):
                        {
                            partialPath = "~/Areas/User/Views/File/_TransactionsReservationBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);//no max size in admin hanaa
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_Reservation_Item" : "_Reservation_TableItem";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            ViewData["GridName"] = "ReservationGrid";
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);
                        }
                    default:
                        {
                            partialPath = "~/Areas/User/Views/File/_TransactionsBoxesPartial.cshtml";
                            var result = TransactionTrayInfoMapper.Map(transactionTrayInfoDTOs.Result);
                            if (!SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.Private"))
                            {
                                result = result.Where(f => f.TransactionDetailsInfoVM.PrivecyId.Value != (int)PrivacyOfTransactions.Private).ToList();
                            }
                            if (!SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.Limited"))
                            {
                                result = result.Where(f => f.TransactionDetailsInfoVM.PrivecyId.Value != (int)PrivacyOfTransactions.Limited).ToList();
                            }
                            if (!SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.OpenByHand"))
                            {
                                result = result.Where(f => f.TransactionDetailsInfoVM.PrivecyId.Value != (int)PrivacyOfTransactions.OpenByHand).ToList();
                            }
                            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                            var settingVM = SettingMapper.Map(SettingValue.Result);
                            var resultGrid = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_MyTransactions_Item" : "_MyTransactions_TableItem";
                            ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            ViewData["GridName"] = "MyTransactionsGrid";
                            return Json(new
                            {
                                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                                Count = transactionTrayInfoDTOs.RowsCount,
                                PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                                MessageText = message
                            }, JsonRequestBehavior.AllowGet);

                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public string GetTransactionTypesForSearch(int transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<TransactionTypeVM>> transactionTypeVMs = LookupsHelper.GetTransactionTypes((TransactionCategory)transactionCategory);

                if (transactionTypeVMs.Result != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (TransactionTypeVM transactionTypeVM in transactionTypeVMs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = transactionTypeVM.Id.ToString(),
                            Label = transactionTypeVM.LocalName
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
        public string GetLetterTypesForSearch(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<LetterTypeVM>> letterTypeVMs = LookupsHelper.GetLetterTypes(transactionCategory);

                if (letterTypeVMs.Result != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (LetterTypeVM letterTypeVM in letterTypeVMs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = letterTypeVM.Id.ToString(),
                            Label = letterTypeVM.LocalName,
                            Parameters = new object[] { letterTypeVM.IsPopularization }
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
        public string GetListTransactionParameters(int? pageValue, List<OrderingBy> sortingOrders = null)
        {
            StringBuilder result = new StringBuilder();
            string filter = Request.Form["filter"];
            string sortColumnName = Request.Form["gridColumn"];
            string dir = Request.Form["dir"];
            string pageIndex = pageValue.HasValue ? pageValue.Value.ToString() : Request.Form["page"];
            string searchColumn = Request.Form["searchColumn"];
            string fromDate = Request.Form["fromDate"];
            string toDate = Request.Form["toDate"];
            string pageSize = Request.Form["pageSize"] != null ? Request.Form["pageSize"] : UIHelper.PageSize.ToString();
            result.Append("CultureName=").Append(SessionInfo.CultureShortName);
            FilterType filterType;
            if (!string.IsNullOrEmpty(filter))
            {
                string[] filterData = filter.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < filterData.Length; i++)
                {
                    string[] data = filterData[i].Split(new[] { "__" },
                    StringSplitOptions.RemoveEmptyEntries);
                    string filterValue = data.Count() == 3 ? data[2] : string.Empty;
                    string[] columnName = data[0].Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    if (!Enum.TryParse(data[1], true, out filterType))
                    {
                        filterType = FilterType.Equals;
                    }
                    if (Convert.ToInt32(data[1]) == 2)
                    {
                        filterType = FilterType.Contains;
                    }
                    result.Append("&Filters[").Append(i).Append("].ColumnName=")
                          .Append(columnName[0]).Append("&Filters[").Append(i)
                          .Append("].Type=").Append(filterType).Append("&Filters[")
                          .Append(i).Append("].Value=").Append(filterValue);
                }
            }
            if (sortingOrders != null)
            {
                for (int i = 0; i < sortingOrders.Count; i++)
                {
                    var data = sortingOrders[i];

                    result.Append("&MultipleOrderBy[").Append(i).Append("].ColumnName=")
                          .Append(data.ColunmName).Append("&MultipleOrderBy[").Append(i)
                          .Append("].IsAscending=").Append(data.Ascending == 1 ? true : false).Append("&MultipleOrderBy[")
                          .Append(i).Append("].Index=").Append(data.Index);
                }
            }
            if (!string.IsNullOrEmpty(searchColumn))
            {
                string[] searchData = searchColumn.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < searchData.Length; i++)
                {
                    string[] data = searchData[i].Split(new[] { "__" },
                    StringSplitOptions.RemoveEmptyEntries);
                    result.Append("&SearchColunms[").Append(i).Append("].ColunmName=")
                          .Append(data[0]).Append("&SearchColunms[").Append(i)
                          .Append("].ColunmValue=").Append(data[1]);
                }
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                result.Append("&FromDate=").Append(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                result.Append("&ToDate=").Append(toDate);
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                string[] sortData = sortColumnName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                if (sortData.Length > 1)
                {
                    result.Append("&OrderBy=").Append(sortData[0]);
                }
                else
                {
                    result.Append("&OrderBy=").Append(sortData[0]);
                }
            }
            if (!string.IsNullOrEmpty(pageSize))
            {
                result.Append("&PageSize=").Append(pageSize);
            }
            else
            {
                GetResult<SettingDTO> Setting = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var PageGrid = SettingMapper.Map(Setting.Result);
                pageSize = PageGrid.Value;
                result.Append("&PageSize=").Append(pageSize);
            }
            if (dir == "1")
            {
                result.Append("&Ascending=").Append(true);
            }
            else
            {
                result.Append("&Ascending=").Append(false);
            }
            if (!string.IsNullOrEmpty(pageIndex))
            {
                int page = Convert.ToInt32(pageIndex);
                result.Append("&PageIndex=").Append(page);
            }
            else
            {
                result.Append("&PageIndex=").Append(1);
            }
            return result.ToString();
        }
        private string GetPartialPathByTrayId(int trayId)
        {
            switch ((TrayType)trayId)
            {
                case TrayType.MyTransactions:
                    return "~/Areas/User/Views/File/_MyTransactionsPartial.cshtml";
                case TrayType.DraftOutbound:
                case TrayType.DeletedDraftOutbound:
                    return "~/Areas/User/Views/File/_DraftOutboundPartial.cshtml";
                case TrayType.ElcOutBound:
                case TrayType.OutboundExternal:
                    return "~/Areas/User/Views/File/_ElcOutboundPartial.cshtml";
                case TrayType.SentTransactions:
                    return "~/Areas/User/Views/File/_SentTransactionsPartial.cshtml";
                case TrayType.Saved:
                case TrayType.SavedCopies:
                    return "~/Areas/User/Views/File/_SavedPartial.cshtml";
                case TrayType.OrgUnit:
                    return "~/Areas/User/Views/File/_OrgunitPartial.cshtml";
                case TrayType.Manager:
                    return "~/Areas/User/Views/File/_ManagerPartial.cshtml";
                case TrayType.Copies:
                    return "~/Areas/User/Views/File/_CopiesPartial.cshtml";
                case TrayType.YESSER:
                    return "~/Areas/User/Views/File/_YESSERPartial.cshtml";
                case TrayType.FollowUp:
                case TrayType.FollowUpUnderProcess:
                case TrayType.FollowUpComplete:
                case TrayType.FollowUpCanceld:
                case TrayType.FollowUpEscalation:
                case TrayType.FollowUpLate:
                case TrayType.FollowUpReminder:
                    return "~/Areas/User/Views/File/_FollowUpPartial.cshtml";
            }
            return null;
        }


        [HttpGet]
        public ActionResult GetTransactionDetails(int transId, int? trayId)
        {
            try
            {
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                    HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetInboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, transId)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                IAjaxGrid assignments = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments, 1, inboundCertificateDTO.Result.Assignments.Count(), true);

                ViewData["AssignmentsData"] = assignments;

                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                ViewData["TrayType"] = trayId;


                foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                {
                    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                }

                return View("~/Areas/User/Views/File/_TransactionDetailsPartial.cshtml", inboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult ShowTransactionDetails(TransactionTrayInfoVM transactionTrayInfoVM)
        {

            return View("~/Areas/User/Views/File/_TransactionDetailsPartial.cshtml", transactionTrayInfoVM);
        }
        [HttpGet]
        public ActionResult MyTransactionsGridEventHandler(int? transactionId, int? transactionTypeId, int? page)
        {
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            var trayDetailsDTO = HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
                SessionInfo.OrgUnitId,
                (int)TrayType.MyTransactions,
                page ?? 1,
                settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                SessionInfo.CultureShortName)).Result;

            if (trayDetailsDTO.StatusCode != StatusCode.Ok)
            {
                throw new Exception(trayDetailsDTO.StatusCode.ToString());
            }
            ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
            ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;
            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_MyTransactions_Item" : "_MyTransactions_TableItem";
            ViewData["GridName"] = "MyTransactionsGrid";
            TrayDetailsVM trayDetails = TrayDetailsMapper.Map(trayDetailsDTO.Result);
            trayDetails.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(trayDetails.TransactionTrayInfoVMs, 1, trayDetails.AllTransactionCount, true);
            return Json(new
            {
                Html = ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
                ((AjaxGrid<TransactionTrayInfoVM>)trayDetails.TransactionTrayInfoVMs).HasItems
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetReporters()
        {
            var dataSource = new List<AutoCompleteDataSource>();
            var reporterDTOs = HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Common/GetReporter?cultureName={0}&orgUnitId={1}",
                SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
            var result = ReporterMapper.Map(reporterDTOs.Result);
            if (reporterDTOs.Result != null)
            {
                foreach (var itemVM in result)
                {
                    AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                    {
                        Value = itemVM.Id.ToString(),
                        Label = itemVM.LocalName
                    };
                    dataSource.Add(autoCompleteDataSource);
                }
            }
            return JsonConvert.SerializeObject(dataSource);
        }

        [HttpGet]
        public ActionResult GetTaskAttachments(int TransactionId, int TaskId)
        {

            GetResult<List<TaskAttachmentsDTO>> getResult = HttpClientWrapper<GetResult<List<TaskAttachmentsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTaskAttachments?TaskId={0}",
                Convert.ToInt32(StringUtility.ValidateId(TaskId.ToString())))).Result;

            List<DocumentVM> documentVMs = getResult.Result.Select(r => DocumentMapper.Map(r.Attachment)).ToList();
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            var grid = (AjaxGrid<DocumentVM>)new AjaxGridFactory().CreateAjaxGrid(documentVMs, 1, documentVMs.Count(), false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

            return PartialView("~/Areas/User/Views/Editor/TaskManagement/_TaskAttachmentsPartial.cshtml", grid);
        }

        [HttpPost]
        public ActionResult RejectTransaction(int id, int deliveryMethodId, int transactionCategoryId)
        {
            try
            {
                var transactionRejectAssignmentVM = new TransactionRejectAssignmentVM()
                {
                    Id = id,
                    DeliveryMethodId = deliveryMethodId,
                    TrayID = (int)TrayType.OrgUnit,
                    Title = "إرجاع المعاملة",
                    URLAction = Url.Action("OrgUnit", "File"),
                    Type = (TransactionCategory)transactionCategoryId.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName)
                };

                ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                ViewData["Reporters"] = GetReporters();

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_RejectTransactionPartial.cshtml", transactionRejectAssignmentVM)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetTasksByFilter(int TabId, List<OrderingBy> SortingOrders, int? pageSize, string sortType, int? searchData, int? page)
        {

            string parameters = GetListTransactionParameters(page, SortingOrders);
            parameters += sortType != null ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
            string message = string.Empty;

            GetResult<List<ReceivedTaskDTO>> receivedTaskDTO =
                  HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasksByFilter?{0}&pageIndex={1}&pageSize={2}&orgUnitId={3}&cultureName={4}&ReceivedTasksTypeId={5}", parameters, 1, 10, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, TabId)).Result;

            List<ReceivedTaskVM> receivedTaskVM = ReceivedTaskMapper.Map(receivedTaskDTO.Result);
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            receivedTaskVM.ForEach(rt => rt.ReceivedTaskType = (ReceivedTasksType)TabId);

            var ResultDataGrid = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVM, 1, receivedTaskVM.Count(), false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_TransactionTasksBoxesPartial.cshtml", ResultDataGrid) }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        //[CustomAuthorizationAttribute(UserClaims.Tasks.Tray)]
        public ActionResult Tasks(int? statusId)
        {
            // LoadSideBarMenu();
            if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Tasks))
            {
                return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
            }

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            //GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
            //       HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
            //{
            //    var message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

            //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            //}

            //if (!trayDetailsDTOs.Result.Any(a => (TrayType)a.Id == TrayType.Tasks))
            //{
            //    return RedirectToAction("Unauthorized", "Error", new { area = "", controller = "Error", action = "Unauthorized" });
            //}
            GetResult<List<ReceivedTaskDTO>> receivedTaskDTO =
                   HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}&ReceivedTasksTypeId={4}", 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, (int)ReceivedTasksType.AcceptedTasks)).Result;

            List<ReceivedTaskVM> receivedTaskVM = ReceivedTaskMapper.Map(receivedTaskDTO.Result);

            receivedTaskVM.ForEach(rt => rt.ReceivedTaskType = ReceivedTasksType.AcceptedTasks);

            var ResultDataGrid = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVM, 1, receivedTaskDTO.RowsCount.Value, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
            ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
            ViewData["StatusId"] = statusId;
            //TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);

            return View("~/Areas/User/Views/File/_TransactionTasksPartial.cshtml", ResultDataGrid);
        }

        [HttpGet]
        public ActionResult AssignToUser(int taskId)
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);

                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                ViewData["UsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                ViewData["hdnTaskArray"] = JsonConvert.SerializeObject(new List<TaskAddDTO>());

                ViewData["transactionId"] = 472;

                ViewData["ControllerName"] = "Editor";
                TaskAddVM taskAddVM = new TaskAddVM();

                GetResult<SentTaskDTO> sentTaskDTO =
                  HttpClientWrapper<GetResult<SentTaskDTO>>.GetItemRequest(string.Format("api/Transaction/GetSentTask?taskId={0}&cultureName={1}", taskId, SessionInfo.CultureShortName)).Result;

                SentTaskVM sentTaskVM = SentTaskMapper.Map(sentTaskDTO.Result);
                //sentTaskVM.Id = null;
                return View("~/Areas/User/Views/Editor/TaskManagement/_AssignTask.cshtml", sentTaskVM);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult EditTask(TaskAddVM taskAddVM)
        {
            string message = string.Empty;
            string data = string.Empty;
            try
            {
                //if (!TasksGrid.Any(copy => copy.SentToOrgUnitId == taskAddVM.SentToOrgUnitId &&
                //    copy.SentToUserId == taskAddVM.SentToUserId && copy.Key != taskAddVM.Key))
                //{
                //    TaskAddVM taskAdd = new TaskAddVM
                //    {
                //        Id = taskAddVM.Id,
                //        SentToOrgUnitId = taskAddVM.SentToOrgUnitId,
                //        SentToOrgUnitName = taskAddVM.SentToOrgUnitName,
                //        SentToUserId = taskAddVM.SentToUserId,
                //        SentToUserName = taskAddVM.SentToUserName,
                //        DeliveryDate = taskAddVM.DeliveryDate,
                //        DeliveryDateH = taskAddVM.DeliveryDateH,
                //        TaskDescription = taskAddVM.TaskDescription,
                //        Key = taskAddVM.Key,
                //        StatusId = (int)TaskStatus.Sent
                //    };

                //    TasksGrid.Remove(TasksGrid.FirstOrDefault(t => t.Key == taskAddVM.Key));
                //    TasksGrid.Insert(taskAddVM.Key, taskAdd);

                //    data = JsonConvert.SerializeObject(TasksGrid);

                //    bool saveResult = SaveTasks(TasksGrid, TransactionIdForTask);

                //    if (!saveResult)
                //    {
                //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, DbRes.TResource("User.Task.TaskAdd.TaskAddedFail"));
                //        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //    }
                //}
                //else
                //{
                //    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                //    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                //}


                message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult GetTasksByTabId(int receivedTasksTypeId)
        {
            GetResult<List<ReceivedTaskDTO>> receivedTaskDTO =
                   HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}&ReceivedTasksTypeId={4}", 1, 10, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, receivedTasksTypeId)).Result;
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            List<ReceivedTaskVM> receivedTaskVM = ReceivedTaskMapper.Map(receivedTaskDTO.Result);

            receivedTaskVM.ForEach(rt => rt.ReceivedTaskType = (ReceivedTasksType)receivedTasksTypeId);

            var ResultDataGrid = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVM, 1, receivedTaskVM.Count(), false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

            return Json(new { ReceivedTasksType = receivedTasksTypeId, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_TransactionTasksBoxesPartial.cshtml", ResultDataGrid) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult AcceptRejectTask(int TaskId, TaskAcceptanceStatus taskAcceptanceStatus, string RejectionReason)
        {
            string message = string.Empty;
            PostResult postResult = null;
            if (taskAcceptanceStatus == TaskAcceptanceStatus.Reject)
            {
                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AcceptRejectTask?TaskId={0}&taskAcceptanceStatus={1}&RejectionReason={2}",
                     TaskId, (int)TaskAcceptanceStatus.Reject, RejectionReason), null).Result;

            }
            else
            {
                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AcceptRejectTask?TaskId={0}&taskAcceptanceStatus={1}&RejectionReason={2}",
                    TaskId, (int)TaskAcceptanceStatus.Accept, null), null).Result;
            }


            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            //  return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            return RedirectToAction("GetTasksByTabId", new { receivedTasksTypeId = (int)ReceivedTasksType.NewTasks });
        }

        [HttpGet]
        public ActionResult TasksGridEventHandler(int? transactionId, int? transactionTypeId, int? page)
        {
            GetResult<List<ReceivedTaskDTO>> receivedTaskDTO =                                                                                          //int pageIndex, int pageSize, int orgUnitId, string cultureName
                   HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}&ReceivedTasksTypeId={4}", page, UIHelper.PageSize, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, (int)ReceivedTasksType.AcceptedTasks)).Result;


            if (receivedTaskDTO.StatusCode != StatusCode.Ok)
            {
                throw new Exception(receivedTaskDTO.StatusCode.ToString());
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
            ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;
            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_TransactionTasks_Item" : "_TransactionTasks_TableItem";
            ViewData["GridName"] = "TasksGrid";
            ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
            List<ReceivedTaskVM> receivedTaskVM = ReceivedTaskMapper.Map(receivedTaskDTO.Result);
            var ResultDataGrid = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVM, page.Value, receivedTaskDTO.RowsCount.Value, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);


            //var partialPath = "~/Areas/User/Views/File/_TransactionTasksPartial.cshtml";
            //return Json(new
            //{
            //    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, ResultDataGrid),
            //    Count = receivedTaskDTO.RowsCount,
            //    PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize



            //}, JsonRequestBehavior.AllowGet);
            //return View("~/Areas/User/Views/File/_TransactionTasksPartial.cshtml", ResultDataGrid);


            return Json(new
            {
                Html = ResultDataGrid.ToJson("_TasksGridWithout", this),
                ResultDataGrid.HasItems
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UploadTasksAttachmentPath()
        {
            if (Request.Files.Count <= 0)
            {
                return Json(new
                {
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            HttpPostedFileBase file;
            string FilePrefix;
            if (SystemConfigurations.MultiTenantEnabled)
            {
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_";
            }
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];
                file.SaveAs(SystemConfigurations.TasksAttachmentPath + FilePrefix + file.FileName);
            }

            return Json(new
            {
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveTasksAttachemntPhysically(string taskAttachmentsToDelete, int TaskId)
        {
            string[] AttachmentName = taskAttachmentsToDelete.TrimEnd(',').Split(',');
            string path = StringUtility.ValidateFileNames(SystemConfigurations.TasksAttachmentPath);
            string FilePrefix = $"Task_{StringUtility.ValidateId(TaskId.ToString())}_";
            string prePrefix = string.Empty;
            if (SystemConfigurations.MultiTenantEnabled)
            {
                prePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
            }
            else
            {
                prePrefix = "_" + SessionInfo.CurrentUser.Id + "_";
            }
            foreach (var item in AttachmentName)
            {
                if (System.IO.File.Exists(Path.Combine(path, prePrefix + FilePrefix + StringUtility.ValidateFileNames(item))))
                {
                    System.IO.File.Delete(Path.Combine(path, prePrefix + FilePrefix + StringUtility.ValidateFileNames(item)));
                }
            }
            return Json(new
            {
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Replay)]
        public ActionResult SendTaskReply(int ReplyTaskId, string ReplyTxt)
        {
            try
            {
                string path = SystemConfigurations.TasksAttachmentPath;
                string FilePrefix = $"Task_{StringUtility.ValidateId(ReplyTaskId.ToString())}_";
                string prePrefix = string.Empty;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    prePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
                }
                else
                {
                    prePrefix = "_" + SessionInfo.CurrentUser.Id + "_";
                }
                if (System.IO.File.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }

                var filteredByFilename = Directory
                .GetFiles(path)
                .Select(o => Path.GetFileName(o))
                .Where(o => o.StartsWith(prePrefix + FilePrefix));
                List<DocumentVM> documentVMs = new List<DocumentVM>();

                foreach (var item in filteredByFilename)
                {
                    byte[] fileContent = System.IO.File.ReadAllBytes(path + item);
                    string mimeType = "";
                    if (!IsValidMimeType(MimeMapping.GetMimeMapping(path + item)))
                    {
                        return Json(new
                        {
                            MessageType = MessageType.Error,
                            MessageText = DbRes.TResource("Task.File.MimeType")
                        });
                    }
                    mimeType = MimeMapping.GetMimeMapping(path + item);
                    FileInfo f = new FileInfo(Path.Combine(path, item));
                    long size = f.Length;
                    string name = item.Substring(item.LastIndexOf('_') + 1);

                    documentVMs.Add(new DocumentVM()
                    {
                        MimeType = mimeType,
                        Content = fileContent,
                        Size = size,
                        Name = name,
                        FromUserId = SessionInfo.CurrentUser.Id,
                        FromEntityId = SessionInfo.OrgUnitId
                    });

                    f.Delete();
                }

                TaskActionVM taskActionVM = new TaskActionVM
                {
                    TaskId = ReplyTaskId,
                    Description = ReplyTxt,
                    Document = documentVMs
                };


                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PutCompleteTransactionTask"), TaskActionMapper.Map(taskActionVM)).Result;


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new
                    {
                        MessageType = MessageType.Error
                    });
                }


                return RedirectToAction("GetTasksByTabId", new { receivedTasksTypeId = (int)ReceivedTasksType.AcceptedTasks });
            }
            catch (Exception)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    MessageText = DbRes.TResource("Task.Path.NotExists")
                });
            }

        }

        //[HttpGet]
        //[CustomAction]
        //public ActionResult DownloadFile(int DocumentId)
        //{
        //    //GetResult<DocumentDTO> getResult =
        //    //   HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetTaskAttachmentById?DocumentInfoId={0}", DocumentId)).Result;
        //    GetResult<DocumentDTO> getResult =
        //            HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, DocumentId)).Result;

        //    if (getResult.StatusCode == StatusCode.Ok && getResult.Result != null)
        //    {
        //        DocumentVM documentVMs = DocumentMapper.Map(getResult.Result);
        //        if (documentVMs.Name == null)
        //        {
        //            documentVMs.Name = "File" + documentVMs.Id.ToString() +".pdf";
        //        }
        //        return File(documentVMs.Content, System.Net.Mime.MediaTypeNames.Application.Pdf, documentVMs.Name);
        //        //return File(documentVMs.Content, documentVMs.MimeType, documentVMs.Name);
        //    }
        //    return Json(new { });
        //}



        #region FollowUp
        [HttpPost]
        public ActionResult FollowUpAddNote(int transactionId, string note)
        {
            try
            {

                string message = string.Empty;
                PostResult postResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/FollowUpDetailsAdd?transactionId={0}&orgUnitId={1}&userId={2}&note={3}", transactionId, SessionInfo.OrgUnitId, SessionInfo.CurrentUser.Id, note), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult GetFollowUpDetailsByTransId(int transactionId)
        {
            try
            {

                string message = string.Empty;
                var partialPath = "~/Areas/User/Views/File/_TransactionFollowUp.cshtml";
                GetResult<List<TransactionFollowUpDTO>> getResult =
                   HttpClientWrapper<GetResult<List<TransactionFollowUpDTO>>>.GetItemRequest(string.Format("api/Transaction/FollowUpDetailsByTransId?transId={0}&UserId={1}&OrgUnitId={2}&cultureName={3}", transactionId, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                if (getResult.StatusCode == StatusCode.Ok && getResult.Result != null)
                {
                    List<TransactionFollowUpVM> transactionFollowUpVMs = TransactionFollowUpMapper.Map(getResult.Result);

                    return Json(new
                    {
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, transactionFollowUpVMs)
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult GetFollowUpDetailsById(int id)
        {
            try
            {
                string message = string.Empty;
                var partialPath = "~/Areas/User/Views/File/_TransactionFollowUpNote.cshtml";
                GetResult<List<FollowUpDetailsDTO>> getResult =
                   HttpClientWrapper<GetResult<List<FollowUpDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/FollowUpDetailsById?id={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;
                if (getResult.StatusCode == StatusCode.Ok && getResult.Result != null && getResult.Result.Count != 0)
                {
                    List<FollowUpDetailsVM> transactionFollowUpVMs = TransactionFollowUpMapper.MapToFollowUpDetails(getResult.Result);

                    return Json(new
                    {

                        MessageType = MessageType.Information,
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, transactionFollowUpVMs)
                    }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult FollowUpUpdate(int transactionId)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult =
                    HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/FollowUpUpdateIsDeleted?transactionId={0}&userId={1}", transactionId, SessionInfo.CurrentUser.Id), null).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        public ActionResult MultiFollowUpUpdate(List<int> transactionIds)
        {
            try
            {
                string message = string.Empty;

                if (transactionIds.Count < 1)
                {
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                PostResult postResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/MultiFollowUpUpdateIsDeleted?userId={0}", SessionInfo.CurrentUser.Id), transactionIds).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        [HttpGet]
        public ActionResult SearchTransactionLinks(int hdnExternalPartyId, int? page)
        {
            try
            {
                string message = string.Empty;
                TransactionDateType transactionDateType = TransactionDateType.Any;

                var transactionTrayInfoDTOs = HttpClientWrapper<GetResult<List<TransactionDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionsByExternalPartyId?externalPartyId={0}&orgUnitId={1}",
                    hdnExternalPartyId, SessionInfo.OrgUnitId)).Result;

                if (transactionTrayInfoDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionTrayInfoDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string partialPath = null;

                if (!transactionTrayInfoDTOs.RowsCount.HasValue)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionNotFound.ToString());
                }
                else
                {
                    //SohaibZ, Convert Year to lookup ID
                    GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName);
                    transactionTrayInfoDTOs.Result.ForEach(item =>
                    {
                        item.Year = lookups.Result.Where(l => l.Text == item.Year.ToString()).FirstOrDefault().Id;
                    });
                    transactionTrayInfoDTOs.Result.ForEach(item =>
                    {
                        bool isPermition = false;
                        switch (item.ConfidentialityId)
                        {
                            case (int)Confedentiality.HandDelivered:
                                {
                                    if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.HandDelivered))
                                    {
                                        isPermition = true;
                                    }

                                    break;
                                }
                            case (int)Confedentiality.HighConfidential:
                                {
                                    if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.HandDelivered) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.ExtremlyConfidential))
                                    {
                                        isPermition = true;
                                    }

                                    break;
                                }
                            case (int)Confedentiality.Secret:
                                {
                                    if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.HandDelivered) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.ExtremlyConfidential) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ConfidentialityOfTransactions.Secret))
                                    {
                                        isPermition = true;
                                    }

                                    break;
                                }
                            case (int)Confedentiality.Normal:
                                {
                                    isPermition = true;
                                    break;
                                }
                        }

                        if (!isPermition)
                        {
                            item.Subject = "* * * *";
                        }
                    });
                }
                foreach (var item in transactionTrayInfoDTOs.Result)
                {


                }

                partialPath = "~/Areas/User/Views/Shared/_SearchTransactionLinksGridPartial.cshtml";
                var result = TransactionDetailsMapper.Map(transactionTrayInfoDTOs.Result);
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                var resultGrid = (AjaxGrid<TransactionDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(result, 1, transactionTrayInfoDTOs.RowsCount.Value, page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);

                ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_MyTransactions_Item" : "_MyTransactions_TableItem";
                ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                ViewData["GridName"] = "MyTransactionsGrid";

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, resultGrid),
                    Count = transactionTrayInfoDTOs.RowsCount,
                    PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize,
                    MessageText = message
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult GetTraysCounts()
        {
            try
            {
                GetResult<List<TrayDetailsDTO>> trayDetailsDTOs = HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                return Json(trayDetailsDTOs.Result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AddDirectory()
        {
            return View("~/Areas/User/Views/File/_AddDirectory.cshtml");
        }
        public ActionResult AddClassification()
        {

            return View("~/Areas/User/Views/File/_AddClassification.cshtml");
        }

        public ActionResult ReturnClassification()
        {
            return View("~/Areas/User/Views/File/_ReturnClassification.cshtml");
        }
        public ActionResult ClassificationEditor()
        {
            return View("~/Areas/User/Views/File/_ClassificationEditor.cshtml");
        }

        public ActionResult DirectoryEdit()
        {
            return View("~/Areas/User/Views/File/_DirectoryEdit.cshtml");
        }

        public ActionResult DirectoryEditor()
        {
            return View("~/Areas/User/Views/File/_DirectoryEditor.cshtml");
        }


        [HttpPost]
        public ActionResult MoveTransactionList(string transactionIds, int trayId)
        {
            try
            {
                string message = string.Empty;

                transactionIds = transactionIds.TrimEnd(',');

                string[] ArrayTranascationsID = new string[] { "" };
                ArrayTranascationsID = transactionIds.Split(',');

                for (int i = 0; i < ArrayTranascationsID.Length; i++)
                {
                    PutResult putResult =
                   HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/MoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}",
                   Convert.ToInt32(ArrayTranascationsID[i]), SessionInfo.OrgUnitId, (int)TrayActionType.Assign, null, trayId, string.Empty, SessionInfo.CurrentUser.Id), null).Result;

                    if (putResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        //public ActionResult Archives(int? page, int? transactionId, int? transactionTypeId)
        //{
        //    try
        //    {
        //        LoadSideBarMenu();
        //        if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Files.Archives))
        //        {
        //            return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
        //        }
        //        GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyArchivesTray)).Result;
        //        var settingVM = SettingMapper.Map(SettingValue.Result);
        //        GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
        //                        HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


        //        GetResult<TrayDetailsDTO> trayDetailsDTO =
        //             HttpClientWrapper<GetResult<TrayDetailsDTO>>
        //             .GetItemRequest(string.Format("api/Transaction/GetTrayDetailsInfo?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}",
        //             SessionInfo.OrgUnitId, (int)TrayType.Archives, page ?? 1, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize, SessionInfo.CultureShortName)).Result;
        //        if (trayDetailsDTO.StatusCode != StatusCode.Ok)
        //        {
        //            throw new Exception(trayDetailsDTO.StatusCode.ToString());
        //        }
        //        ViewData["ConfidentialityAscDesc"] = TransactionHelper.GetByConfidentialityAscDesc();
        //        ViewData["PriorityAscDesc"] = TransactionHelper.GetByPriorityAscDesc();
        //        ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
        //        ViewData["NumberAcsDesc"] = TransactionHelper.GetNumberAcsDesc();
        //        ViewData["Orgunits"] = GetAllOrgUnitsForSearch();
        //        ViewData["ConfidentialityData"] = GetConfidentialityLevel();
        //        ViewData["ConfidentialityDataList"] = GetConfidentialityForSearch();
        //        ViewData["PrioritiesData"] = GetPrioritiesForSearch();
        //        ViewData["TransactionDateTypesData"] = GetTransactionDateTypes(TrayType.Archives);
        //        ViewData["FileTitle"] = DbRes.TResource("User.File.OrgUnit");
        //        ViewData["PageSize"] = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
        //        ViewData["TransactionId"] = transactionId.HasValue ? transactionId.Value : 0;
        //        ViewData["TransactionTypeId"] = transactionTypeId.HasValue ? transactionTypeId.Value : 0;

        //        var transactionTrayInfoVM = TrayDetailsMapper.Map(trayDetailsDTO.Result);
        //        transactionTrayInfoVM.TransactionTrayInfoVMs = (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory()
        //            .CreateAjaxGrid(transactionTrayInfoVM.TransactionTrayInfoVMs, page ?? 1, trayDetailsDTO.RowsCount ?? 0,
        //            page.HasValue, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
        //        TempData["TrayDetails"] = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
        //        if (page.HasValue)
        //        {
        //            ViewData["ListItemName"] = Session["renderType"].ToString() == "cardItem" ? "_OrgunitItem" : "_OrgunitTableItem";
        //            ViewData["GridName"] = "OrgunitGrid";
        //            return Json(new
        //            {
        //                Html = ((AjaxGrid<TransactionTrayInfoVM>)transactionTrayInfoVM.TransactionTrayInfoVMs).ToJson("_FileGrid", this),
        //                ((AjaxGrid<TransactionTrayInfoVM>)transactionTrayInfoVM.TransactionTrayInfoVMs).HasItems
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //        return View("~/Areas/User/Views/File/ArchiveIndex.cshtml", transactionTrayInfoVM);
        //    }
        //    catch (Exception ex)
        //    {
        //        if ((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message) == StatusCode.OrgUnitNotAuthorized)
        //        {
        //            return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
        //        }
        //        throw;
        //    }
        //}

        [HttpGet]
        public ActionResult AddReleaseNote()
        {
            try
            {
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/ReleaaseNotesUsersAdd", null).Result;

                return RedirectToAction("MyTransactions");

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetReleaaseNotes()
        {
            try
            {
                GetResult<List<ReleaseNotesDTO>> releaseNotesList =
                   HttpClientWrapper<GetResult<List<ReleaseNotesDTO>>>.GetItemRequest("api/Transaction/ReleaaseNotesUsersSelect").Result;

                if (releaseNotesList.Result.Count == 0)
                {
                    return Json(new
                    {
                        Count = 0,

                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    Count = releaseNotesList.Result != null ? releaseNotesList.Result.Count : 0,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/ReleaseNotes.cshtml", releaseNotesList.Result),
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void StoreCopiesIds(List<TransactionTrayInfoDTO> transactionTrayInfoDTOs, int trayType)
        {


            Dictionary<int, int?> Ids = new Dictionary<int, int?>();
            if (transactionTrayInfoDTOs != null)
            {
                foreach (TransactionTrayInfoDTO transactionTrayInfoDTO in transactionTrayInfoDTOs)
                {
                    if (!Ids.ContainsKey(transactionTrayInfoDTO.TransactionDetailsInfoDTOs.Id))
                        Ids.Add(transactionTrayInfoDTO.TransactionDetailsInfoDTOs.Id, transactionTrayInfoDTO.TransactionDetailsInfoDTOs.TransactionTypeId);
                }
            }
            if ((int)TrayType.Copies == trayType)
                Session["InboundCopiesIds"] = Ids;
            else if ((int)TrayType.CopiesOutbound == trayType)
                Session["OutboundCopiesIds"] = Ids;
            else if ((int)TrayType.InternalInboundCopies == trayType)
                Session["InternalCopiesIds"] = Ids;
            else if ((int)TrayType.SpecialCopies == trayType)
                Session["SpecialCopiesIds"] = Ids;
        }
    }
}