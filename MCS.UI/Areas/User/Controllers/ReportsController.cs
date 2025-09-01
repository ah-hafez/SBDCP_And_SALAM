using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Permission;
using MCS.UI.Areas.User.Mappers.Report;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Report;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Common;
using MCS.UI.Helpers;
using ZXing;
using LookupModels = MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.User.Mappers.Lookups;
using System.Data;
using System.IO;
using OfficeOpenXml.Table;
using OfficeOpenXml;

namespace MCS.UI.Areas.User
{
    //[CustomAuthorizationAttribute(UserClaims.Reports.DisplayReports)]
    public class ReportsController : BaseController
    {
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult TransactionsDeliveryReport()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                   HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);

                ViewData["ConfidentialityData"] = GetConfidentialityLevel();

                ViewData["PrioritiesData"] = GetPriorities(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));

                var users = GetUsersBasedPermisstion(SessionInfo.OrgUnitId);

                ViewData["Users"] = users;

                ViewData["LetterTypeData"] = GetLetterTypes(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));

                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(TransactionCategory.Inbound);

                ViewData["TransactionsDeliveryReportGridData"] = new AjaxGridFactory().CreateAjaxGrid(new List<TransactionDeliveryReportVM>(), 1, 0, true);

                //     GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                //HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                //ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                ViewData["Reporters"] = GetReporters();

                TransactionsDeliveryReportVM transactionsDeliveryReportVM = new TransactionsDeliveryReportVM();

                return View("~/Areas/User/Views/Reports/TransactionsDeliveryReport.cshtml", transactionsDeliveryReportVM);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult TransactionDeliveryReportSearch(TransactionsSignedDeliveryReportVM transactionsSignedDeliveryReportVM)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                string message = string.Empty;

                //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string DateH = null;
                if (transactionsSignedDeliveryReportVM.Date.HasValue)
                {
                    DateH = transactionsSignedDeliveryReportVM.Date.Value.ToString("dd/MM/yyyy");
                }

                //string strDeliveryReportObj = javaScriptSerializer.Serialize(transactionsSignedDeliveryReportVM);

                GetResult<List<TransactionDeliveryReportDTO>> transactionDeliveryReportDTOs = new GetResult<List<TransactionDeliveryReportDTO>>();

                transactionDeliveryReportDTOs =
                    HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>.GetItemRequest(String.Format("api/Transaction/SearchDeliveryReportByNumberAndYear?NumberTran={0}&Year={1}&numberD={2}&cultureName={3}", transactionsSignedDeliveryReportVM.TransactionNumber, DateH, transactionsSignedDeliveryReportVM.Number, SessionInfo.CultureShortName)).Result;

                if (transactionDeliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionDeliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int docId = 0;

                if (transactionDeliveryReportDTOs.Result != null && transactionDeliveryReportDTOs.Result.Any())
                {
                    if (transactionDeliveryReportDTOs.Result.FirstOrDefault().Document != null)
                    {
                        docId = transactionDeliveryReportDTOs.Result.FirstOrDefault().Document.Id;
                        GetResult<DocumentDTO> documentDTO = HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetDeliveryReportDocument?documentId={0}", docId)).Result;

                        List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();

                        if (documentDTO.Result != null)
                        {
                            string documentId = Guid.NewGuid().ToString();
                            transactionArchiveVMs.Add(new TransactionArchiveVM
                            {
                                Id = documentId,
                                EncryptDocumentId = AESEncrytDecry.Base64Encode(docId.ToString()),
                                IsMainDocument = true,
                                DocumentId = docId,
                                ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                            });
                            Session["DocoNutDocument"] = documentDTO.Result.Content;
                        }
                        ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);

                    }
                    else
                    {
                        transactionDeliveryReportDTOs.Result.FirstOrDefault().Document = null;
                        ViewData["hdnArchivingMainDocumentArray"] = null;
                        Session["DocoNutDocument"] = null;
                    }
                }

                IList<TransactionDeliveryReportVM> transactionDeliveryReportVMs = TransactionDeliveryReportMapper.Map(transactionDeliveryReportDTOs.Result);

                string DeliveryReportNumber = string.Empty;
                string Date = string.Empty;

                if (transactionDeliveryReportVMs.Any())
                {
                    DeliveryReportNumber = transactionDeliveryReportVMs.FirstOrDefault().Number;
                    Date = transactionDeliveryReportVMs.FirstOrDefault().Date.ToShortDateString();
                }

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionDeliveryReportVMs, 1, transactionDeliveryReportVMs.Count(), true);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionsDeliveryReportGridPartial", grid),
                    MessageText = message,
                    MessageType = MessageType.Information,
                    DeliveryReportNumber = DeliveryReportNumber,
                    Date = Date,
                    DeliveryReportsCount = transactionDeliveryReportVMs.Count
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult GetSignedDeliveryReport(TransactionsSignedDeliveryReportVM transactionsSignedDeliveryReportVM)
        {
            try
            {
                string message = string.Empty;

                string dateH = string.Empty;
                               if (transactionsSignedDeliveryReportVM.Date.HasValue)
                {
                    dateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionsSignedDeliveryReportVM.Date.Value);
                }

                GetResult<List<SignedDeliveryReportDTO>> signedDeliveryReportDTOs = new GetResult<List<SignedDeliveryReportDTO>>();
                signedDeliveryReportDTOs =
                    HttpClientWrapper<GetResult<List<SignedDeliveryReportDTO>>>.GetItemRequest(String.Format("api/Transaction/GetSignedDeliveryReport?date={0}&orgunitId={1}", dateH , transactionsSignedDeliveryReportVM.OrgUnitId)).Result;

                if (signedDeliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, signedDeliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IList<SignedDeliveryReportVM> signedDeliveryReportVMs = SignedDeliveryReportMapper.Map(signedDeliveryReportDTOs.Result.OrderByDescending(e => e.Id).ToList());



                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(signedDeliveryReportVMs, 1, signedDeliveryReportVMs.Count(), true);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SignedDeliveryReportGridPartial", grid),
                    MessageText = message,
                    MessageType = MessageType.Information,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult UploadTransactionDeliveryReport()
        {
            try
            {
                LoadSideBarMenu();
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
                ViewData["MainDocumentId"] = null;
                ViewData["DocumentId"] = null;
                ViewData["DeliveryReportId"] = string.Empty;
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(new List<TransactionDeliveryReportVM>(), 1, 0, true);
                ViewData["TransactionsDeliveryReportGridData"] = grid;
                TransactionsSignedDeliveryReportVM transactionsSignedDeliveryReportVM = new TransactionsSignedDeliveryReportVM();
                return View("~/Areas/User/Views/Reports/UploadTransactionDeliveryReport.cshtml", transactionsSignedDeliveryReportVM);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult searchDeliveryReport()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                LoadSideBarMenu();
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
                ViewData["MainDocumentId"] = null;
                ViewData["DocumentId"] = null;
                ViewData["DeliveryReportId"] = string.Empty;
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(new List<TransactionDeliveryReportVM>(), 1, 0, true);
                ViewData["TransactionsDeliveryReportGridData"] = grid;
                TransactionsSignedDeliveryReportVM transactionsSignedDeliveryReportVM = new TransactionsSignedDeliveryReportVM();
                return View("~/Areas/User/Views/Reports/SearchDeliveryReport.cshtml", transactionsSignedDeliveryReportVM);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        [ValidateAntiForgeryToken]
        public ActionResult UploadSignedDeliveryReport(string deliveryReportNumber, string mainDocumentToken, DateTime date)
        {
            try
            {
                string DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(date);
                string message = string.Empty;

                byte[] data = DocumentViewerHelper.GetPDFFile(mainDocumentToken);

                DocumentVM documentVM = new DocumentVM
                {
                    Content = data,
                    Size = data.Length,
                    MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,
                };

                int userId = SessionInfo.CurrentUser.Id;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/UploadSignedDeliveryReportByReportNumber?DeliveryReportNumber={0}&userId={1}&cultureName={2}&DateH={3}", deliveryReportNumber, userId, SessionInfo.CultureShortName, DateH), DocumentMapper.Map(documentVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.AssignmentPaper.SaveSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult CheckIfHasMainArchive(string param)
        {
            try
            {
                bool _hasArchive = true;

                byte[] data = DocumentViewerHelper.GetPDFFile(param);

                if (data.Length == 12397)
                {
                    _hasArchive = false;
                }

                return Json(new { hasArchive = _hasArchive.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        [ValidateAntiForgeryToken()]
        public ActionResult TransactionsDeliveryReportSearchPrintAgain(TransactionsDeliveryReportVM transactionsDeliveryReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                string dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
                DateTime date = DateTime.Now;
                int currentYear = date.Year;
                DateTime startDate = new DateTime(currentYear, 1, 1);
                SearchCriteria searchCriteria = new SearchCriteria();

                searchCriteria.SearchColunms = new List<SearchColunm>();
                if (transactionsDeliveryReportVM.DateFrom.HasValue)
                {
                    searchCriteria.FromDate = transactionsDeliveryReportVM.DateFrom.ToString();
                }
                else
                {
                    searchCriteria.FromDate = startDate.ToString();
                }

                if (transactionsDeliveryReportVM.DateTo.HasValue)
                {
                    searchCriteria.ToDate = transactionsDeliveryReportVM.DateTo.ToString();
                }
                else
                {
                    searchCriteria.ToDate = date.ToString();
                }

                if (transactionsDeliveryReportVM.HourFrom.HasValue)
                {
                    TimeSpan fromTime = new TimeSpan(transactionsDeliveryReportVM.HourFrom.Value, transactionsDeliveryReportVM.MinuteFrom ?? 0, 0);
                    searchCriteria.FromDate = searchCriteria.FromDateTime.Value.Add(fromTime).ToString(dateTimeFormat);
                }
                else
                {
                    TimeSpan fromTime = new TimeSpan(0, 01, 01);
                    searchCriteria.FromDate = searchCriteria.FromDateTime.Value.Add(fromTime).ToString(dateTimeFormat);
                }
                if (transactionsDeliveryReportVM.HourTo.HasValue)
                {
                    TimeSpan toTime = new TimeSpan(transactionsDeliveryReportVM.HourTo.Value, transactionsDeliveryReportVM.MinuteTo ?? 59, 59);
                    searchCriteria.ToDate = searchCriteria.ToDateTime.Value.Add(toTime).ToString(dateTimeFormat);
                }
                else
                {
                    TimeSpan toTime = new TimeSpan(23, 59, 59);
                    searchCriteria.ToDate = searchCriteria.ToDateTime.Value.Add(toTime).ToString(dateTimeFormat);
                }

                searchCriteria.SearchColunms.Add(AddColunm("TransactionCategory", transactionsDeliveryReportVM.TransactionCategoryId.ToString()));

                if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    if (transactionsDeliveryReportVM.ToEntity.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToEntity", transactionsDeliveryReportVM.ToEntity.Value.ToString()));
                    }
                }
                else if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
                {
                    if (transactionsDeliveryReportVM.ToEntity.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToEntity", transactionsDeliveryReportVM.ToEntity.Value.ToString()));
                    }
                }
                else
                {
                    if (transactionsDeliveryReportVM.FromOrgUnit.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("FromOrgUnit", transactionsDeliveryReportVM.FromOrgUnit.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.ToOrgUnit.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToOrgUnit", transactionsDeliveryReportVM.ToOrgUnit.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.FromUser.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("FromUser", transactionsDeliveryReportVM.FromUser.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.ToUser.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToUser", transactionsDeliveryReportVM.ToUser.Value.ToString()));
                    }
                }

                if (transactionsDeliveryReportVM.SourceId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("SourceId", transactionsDeliveryReportVM.SourceId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.FromTransactionNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("FromTransactionNumber", transactionsDeliveryReportVM.FromTransactionNumber.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.ToTransactionNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("ToTransactionNumber", transactionsDeliveryReportVM.ToTransactionNumber.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) && transactionsDeliveryReportVM.LetterTypeId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("LetterTypeId", transactionsDeliveryReportVM.LetterTypeId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.PriorityLevelId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("Priority", transactionsDeliveryReportVM.PriorityLevelId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.ConfidentialityLevelId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("Confidentiality", transactionsDeliveryReportVM.ConfidentialityLevelId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.UserId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("UserId", transactionsDeliveryReportVM.UserId.Value.ToString()));
                }
                else
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("UserId", SessionInfo.CurrentUser.Id.ToString()));
                }

                if (transactionsDeliveryReportVM.DeliveryReportNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("DeliveryReportNumber", transactionsDeliveryReportVM.DeliveryReportNumber.Value.ToString()));
                }
                //searchCriteria.SearchColunms.Add(
                //    AddColunm("IsPrinted", transactionsDeliveryReportVM.RePrint.ToString()));

                //   searchCriteria.SearchColunms.Add(AddColunm("DeliveryMethodId", (DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName)).ToString()));

                searchCriteria.PageIndex = page ?? 1;
                searchCriteria.PageSize = GridHelper.PageSize;
                searchCriteria.CultureName = SessionInfo.CultureShortName;

                GetResult<List<TransactionDeliveryReportDTO>> transactionDeliveryReportDTOs = new GetResult<List<TransactionDeliveryReportDTO>>();

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                string strSearchCriteria = javaScriptSerializer.Serialize(searchCriteria);

                transactionDeliveryReportDTOs =
                    HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>.GetItemRequest(String.Format("api/Transaction/SearchDeliveryReport?strSearchCriteria={0}", strSearchCriteria)).Result;

                int AllCount = transactionDeliveryReportDTOs.RowsCount ?? 0;
                if (AllCount == 0 & transactionDeliveryReportDTOs.StatusCode == StatusCode.Ok)
                {
                    message = DbRes.TValidation("User.TransactionDeliveryReport.NoResult");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }
                if (transactionDeliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionDeliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (AllCount == 0)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "NoDeliveryReportFound");

                    return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

                }
                IList<TransactionDeliveryReportVM> transactionDeliveryReportVMs = TransactionDeliveryReportMapper.Map(transactionDeliveryReportDTOs.Result);
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionDeliveryReportVMs, page ?? 1, AllCount, page.HasValue, pageSize);

                ViewData["TransactionsDeliveryReportGridData"] = grid;

                return Json(new { count = AllCount, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionsDeliveryReportGridPartial", grid), MessageText = message, MessageType = MessageType.Information, Param = strSearchCriteria }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        [ValidateAntiForgeryToken()]
        public ActionResult TransactionsDeliveryReportSearch(TransactionsDeliveryReportVM transactionsDeliveryReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                string dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
                DateTime date = DateTime.Now;
                int currentYear = date.Year;
                DateTime startDate = new DateTime(currentYear, 1, 1);
                SearchCriteria searchCriteria = new SearchCriteria();

                searchCriteria.SearchColunms = new List<SearchColunm>();
                if (transactionsDeliveryReportVM.DateFrom.HasValue)
                {
                    searchCriteria.FromDate = transactionsDeliveryReportVM.DateFrom.ToString();
                }
                else
                {
                    searchCriteria.FromDate = startDate.ToString();
                }

                if (transactionsDeliveryReportVM.DateTo.HasValue)
                {
                    searchCriteria.ToDate = transactionsDeliveryReportVM.DateTo.ToString();
                }
                else
                {
                    searchCriteria.ToDate = date.ToString();
                }

                if (transactionsDeliveryReportVM.HourFrom.HasValue)
                {
                    TimeSpan fromTime = new TimeSpan(transactionsDeliveryReportVM.HourFrom.Value, transactionsDeliveryReportVM.MinuteFrom ?? 0, 0);
                    searchCriteria.FromDate = searchCriteria.FromDateTime.Value.Add(fromTime).ToString(dateTimeFormat);
                }
                else
                {
                    TimeSpan fromTime = new TimeSpan(0, 01, 01);
                    searchCriteria.FromDate = searchCriteria.FromDateTime.Value.Add(fromTime).ToString(dateTimeFormat);
                }
                if (transactionsDeliveryReportVM.HourTo.HasValue)
                {
                    TimeSpan toTime = new TimeSpan(transactionsDeliveryReportVM.HourTo.Value, transactionsDeliveryReportVM.MinuteTo ?? 59, 59);
                    searchCriteria.ToDate = searchCriteria.ToDateTime.Value.Add(toTime).ToString(dateTimeFormat);
                }
                else
                {
                    TimeSpan toTime = new TimeSpan(23, 59, 59);
                    searchCriteria.ToDate = searchCriteria.ToDateTime.Value.Add(toTime).ToString(dateTimeFormat);
                }

                searchCriteria.SearchColunms.Add(AddColunm("TransactionCategory", transactionsDeliveryReportVM.TransactionCategoryId.ToString()));

                if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    if (transactionsDeliveryReportVM.ToEntity.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToEntity", transactionsDeliveryReportVM.ToEntity.Value.ToString()));
                    }
                }
                else if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
                {
                    if (transactionsDeliveryReportVM.ToEntity.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToEntity", transactionsDeliveryReportVM.ToEntity.Value.ToString()));
                    }
                }
                else
                {
                    if (transactionsDeliveryReportVM.FromOrgUnit.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("FromOrgUnit", transactionsDeliveryReportVM.FromOrgUnit.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.ToOrgUnit.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToOrgUnit", transactionsDeliveryReportVM.ToOrgUnit.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.FromUser.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("FromUser", transactionsDeliveryReportVM.FromUser.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.ToUser.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToUser", transactionsDeliveryReportVM.ToUser.Value.ToString()));
                    }
                }

                if (transactionsDeliveryReportVM.SourceId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("SourceId", transactionsDeliveryReportVM.SourceId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.FromTransactionNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("FromTransactionNumber", transactionsDeliveryReportVM.FromTransactionNumber.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.ToTransactionNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("ToTransactionNumber", transactionsDeliveryReportVM.ToTransactionNumber.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) && transactionsDeliveryReportVM.LetterTypeId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("LetterTypeId", transactionsDeliveryReportVM.LetterTypeId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.PriorityLevelId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("Priority", transactionsDeliveryReportVM.PriorityLevelId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.ConfidentialityLevelId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("Confidentiality", transactionsDeliveryReportVM.ConfidentialityLevelId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.UserId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("UserId", transactionsDeliveryReportVM.UserId.Value.ToString()));
                }
                else
                {
                    //searchCriteria.SearchColunms.Add(
                    //    AddColunm("UserId", SessionInfo.CurrentUser.Id.ToString()));

                    searchCriteria.SearchColunms.Add(
                        AddColunm("OrgunitId", SessionInfo.OrgUnitId.ToString()));
                }

                if (transactionsDeliveryReportVM.DeliveryReportNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("DeliveryReportNumber", transactionsDeliveryReportVM.DeliveryReportNumber.Value.ToString()));
                }


                searchCriteria.SearchColunms.Add(
                    AddColunm("IsPrinted", transactionsDeliveryReportVM.RePrint.ToString()));

                //   searchCriteria.SearchColunms.Add(AddColunm("DeliveryMethodId", (DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName)).ToString()));

                searchCriteria.PageIndex = page ?? 1;
                searchCriteria.PageSize = GridHelper.PageSize;
                searchCriteria.CultureName = SessionInfo.CultureShortName;

                GetResult<List<TransactionDeliveryReportDTO>> transactionDeliveryReportDTOs = new GetResult<List<TransactionDeliveryReportDTO>>();

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                string strSearchCriteria = javaScriptSerializer.Serialize(searchCriteria);

                transactionDeliveryReportDTOs =
                    HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>.GetItemRequest(String.Format("api/Transaction/SearchDeliveryReport?strSearchCriteria={0}", strSearchCriteria)).Result;

                int AllCount = transactionDeliveryReportDTOs.RowsCount ?? 0;
                if (AllCount == 0 & transactionDeliveryReportDTOs.StatusCode == StatusCode.Ok)
                {
                    message = DbRes.TValidation("User.TransactionDeliveryReport.NoResult");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }
                if (transactionDeliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionDeliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (AllCount == 0)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "NoDeliveryReportFound");

                    return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

                }
                IList<TransactionDeliveryReportVM> transactionDeliveryReportVMs = TransactionDeliveryReportMapper.Map(transactionDeliveryReportDTOs.Result);
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                //TransactionDeliveryReportResult transactionDeliveryReportResult = new TransactionDeliveryReportResult();
                //transactionDeliveryReportResult.Parties = new Dictionary<int, string>();
                //foreach (TransactionDeliveryReportVM transactionDeliveryReportVM in transactionDeliveryReportVMs)
                //{
                //    if(transactionDeliveryReportVM.TransactionCategoryId == 
                //        TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) ||
                //        transactionDeliveryReportVM.TransactionCategoryId ==
                //        TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                //    {
                //        transactionDeliveryReportVM.PartyId = transactionDeliveryReportVM.ExternalPartyId;
                //        transactionDeliveryReportVM.PartyName = transactionDeliveryReportVM.ExternalPartyName;
                //    }
                //    else
                //    {
                //        transactionDeliveryReportVM.PartyId = transactionDeliveryReportVM.ToEntityId;
                //        transactionDeliveryReportVM.PartyName = transactionDeliveryReportVM.ToEntity;
                //    }
                //    if(!transactionDeliveryReportResult.Parties.ContainsKey(transactionDeliveryReportVM.PartyId))
                //    {
                //        transactionDeliveryReportResult.Parties.Add(transactionDeliveryReportVM.PartyId, transactionDeliveryReportVM.PartyName);
                //    }
                //}

                //var grid = (AjaxGrid<TransactionDeliveryReportVM>)new AjaxGridFactory().CreateAjaxGrid(transactionDeliveryReportVMs, page ?? 1, AllCount, false, pageSize);
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionDeliveryReportVMs, page ?? 1, AllCount, page.HasValue, pageSize);

                ViewData["TransactionsDeliveryReportGridData"] = grid;
                //transactionDeliveryReportResult.TransactionGridResultVMs = grid;

                //return Json(new
                //{
                //    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionDeliveryReportResultPartial", transactionDeliveryReportResult),
                //    UserHasTransactions = true,
                //    MessageText = message,
                //    MessageType = MessageType.Information
                //}, JsonRequestBehavior.AllowGet);
                return Json(new { count = AllCount, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionsDeliveryReportGridPartial", grid), MessageText = message, MessageType = MessageType.Information, Param = strSearchCriteria }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        [ValidateAntiForgeryToken()]
        public ActionResult TransactionsDeliveryReportSearchGroupingByParty(TransactionsDeliveryReportVM transactionsDeliveryReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                string dateTimeFormat = "dd/MM/yyyy HH:mm:ss";
                DateTime date = DateTime.Now;
                int currentYear = date.Year;
                DateTime startDate = new DateTime(currentYear, 1, 1);
                SearchCriteria searchCriteria = new SearchCriteria();

                searchCriteria.SearchColunms = new List<SearchColunm>();
                if (transactionsDeliveryReportVM.DateFrom.HasValue)
                {
                    searchCriteria.FromDate = transactionsDeliveryReportVM.DateFrom.ToString();
                }
                else
                {
                    searchCriteria.FromDate = startDate.ToString();
                }

                if (transactionsDeliveryReportVM.DateTo.HasValue)
                {
                    searchCriteria.ToDate = transactionsDeliveryReportVM.DateTo.ToString();
                }
                else
                {
                    searchCriteria.ToDate = date.ToString();
                }

                if (transactionsDeliveryReportVM.HourFrom.HasValue)
                {
                    TimeSpan fromTime = new TimeSpan(transactionsDeliveryReportVM.HourFrom.Value, transactionsDeliveryReportVM.MinuteFrom ?? 0, 0);
                    searchCriteria.FromDate = searchCriteria.FromDateTime.Value.Add(fromTime).ToString(dateTimeFormat);
                }
                else
                {
                    TimeSpan fromTime = new TimeSpan(0, 01, 01);
                    searchCriteria.FromDate = searchCriteria.FromDateTime.Value.Add(fromTime).ToString(dateTimeFormat);
                }
                if (transactionsDeliveryReportVM.HourTo.HasValue)
                {
                    TimeSpan toTime = new TimeSpan(transactionsDeliveryReportVM.HourTo.Value, transactionsDeliveryReportVM.MinuteTo ?? 59, 59);
                    searchCriteria.ToDate = searchCriteria.ToDateTime.Value.Add(toTime).ToString(dateTimeFormat);
                }
                else
                {
                    TimeSpan toTime = new TimeSpan(23, 59, 59);
                    searchCriteria.ToDate = searchCriteria.ToDateTime.Value.Add(toTime).ToString(dateTimeFormat);
                }

                searchCriteria.SearchColunms.Add(AddColunm("TransactionCategory", transactionsDeliveryReportVM.TransactionCategoryId.ToString()));

                if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    if (transactionsDeliveryReportVM.ToEntity.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToEntity", transactionsDeliveryReportVM.ToEntity.Value.ToString()));
                    }
                }
                else if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
                {
                    if (transactionsDeliveryReportVM.ToEntity.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToEntity", transactionsDeliveryReportVM.ToEntity.Value.ToString()));
                    }
                }
                else
                {
                    if (transactionsDeliveryReportVM.FromOrgUnit.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("FromOrgUnit", transactionsDeliveryReportVM.FromOrgUnit.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.ToOrgUnit.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToOrgUnit", transactionsDeliveryReportVM.ToOrgUnit.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.FromUser.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("FromUser", transactionsDeliveryReportVM.FromUser.Value.ToString()));
                    }

                    if (transactionsDeliveryReportVM.ToUser.HasValue)
                    {
                        searchCriteria.SearchColunms.Add(AddColunm("ToUser", transactionsDeliveryReportVM.ToUser.Value.ToString()));
                    }
                }

                if (transactionsDeliveryReportVM.SourceId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("SourceId", transactionsDeliveryReportVM.SourceId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.FromTransactionNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("FromTransactionNumber", transactionsDeliveryReportVM.FromTransactionNumber.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.ToTransactionNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("ToTransactionNumber", transactionsDeliveryReportVM.ToTransactionNumber.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) && transactionsDeliveryReportVM.LetterTypeId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("LetterTypeId", transactionsDeliveryReportVM.LetterTypeId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.PriorityLevelId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("Priority", transactionsDeliveryReportVM.PriorityLevelId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.ConfidentialityLevelId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("Confidentiality", transactionsDeliveryReportVM.ConfidentialityLevelId.Value.ToString()));
                }

                if (transactionsDeliveryReportVM.UserId.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("UserId", transactionsDeliveryReportVM.UserId.Value.ToString()));
                }
                else
                {
                    //searchCriteria.SearchColunms.Add(
                    //    AddColunm("UserId", SessionInfo.CurrentUser.Id.ToString()));

                    searchCriteria.SearchColunms.Add(
                        AddColunm("OrgunitId", SessionInfo.OrgUnitId.ToString()));
                }

                if (transactionsDeliveryReportVM.DeliveryReportNumber.HasValue)
                {
                    searchCriteria.SearchColunms.Add(
                        AddColunm("DeliveryReportNumber", transactionsDeliveryReportVM.DeliveryReportNumber.Value.ToString()));
                }


                searchCriteria.SearchColunms.Add(
                    AddColunm("IsPrinted", transactionsDeliveryReportVM.RePrint.ToString()));

                searchCriteria.PageIndex = page ?? 1;
                searchCriteria.PageSize = GridHelper.PageSize;
                searchCriteria.CultureName = SessionInfo.CultureShortName;

                GetResult<List<TransactionDeliveryReportDTO>> transactionDeliveryReportDTOs = new GetResult<List<TransactionDeliveryReportDTO>>();

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                string strSearchCriteria = javaScriptSerializer.Serialize(searchCriteria);

                transactionDeliveryReportDTOs =
                    HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>.GetItemRequest(String.Format("api/Transaction/SearchDeliveryReport?strSearchCriteria={0}", strSearchCriteria)).Result;

                int AllCount = transactionDeliveryReportDTOs.RowsCount ?? 0;
                if (AllCount == 0 & transactionDeliveryReportDTOs.StatusCode == StatusCode.Ok)
                {
                    message = DbRes.TValidation("User.TransactionDeliveryReport.NoResult");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }
                if (transactionDeliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionDeliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (AllCount == 0)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "NoDeliveryReportFound");

                    return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

                }
                IList<TransactionDeliveryReportVM> transactionDeliveryReportVMs = TransactionDeliveryReportMapper.Map(transactionDeliveryReportDTOs.Result);
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;



                TransactionDeliveryReportResult transactionDeliveryReportResult = new TransactionDeliveryReportResult();

                transactionDeliveryReportResult.Parties = new Dictionary<int, string>();
                foreach (TransactionDeliveryReportVM transactionDeliveryReportVM in transactionDeliveryReportVMs)
                {

                    if (transactionDeliveryReportVM.IsCopy)
                    {
                        if (transactionDeliveryReportVM.InternalPartyId != -1)
                        {

                            transactionDeliveryReportVM.PartyId = transactionDeliveryReportVM.InternalPartyId;
                            transactionDeliveryReportVM.PartyName = transactionDeliveryReportVM.InternalPartyName;
                        }
                        else if (transactionDeliveryReportVM.ExternalPartyId != -1)
                        {

                            transactionDeliveryReportVM.PartyId = transactionDeliveryReportVM.ExternalPartyId;
                            transactionDeliveryReportVM.PartyName = transactionDeliveryReportVM.ExternalPartyName;

                        }
                    }
                    else
                    {
                        if (transactionDeliveryReportVM.TransactionCategoryId ==
                        TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) ||
                        transactionDeliveryReportVM.TransactionCategoryId ==
                        TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                        {

                            transactionDeliveryReportVM.PartyId = transactionDeliveryReportVM.ExternalPartyId;
                            transactionDeliveryReportVM.PartyName = transactionDeliveryReportVM.ExternalPartyName;
                        }
                        else
                        {
                            transactionDeliveryReportVM.PartyId = transactionDeliveryReportVM.ToEntityId;
                            transactionDeliveryReportVM.PartyName = transactionDeliveryReportVM.ToEntity;

                        }

                    }
                    if (!transactionDeliveryReportResult.Parties.ContainsKey(transactionDeliveryReportVM.PartyId))
                    {
                        transactionDeliveryReportResult.Parties.Add(transactionDeliveryReportVM.PartyId, transactionDeliveryReportVM.PartyName);
                    }

                }
                List<TransactionDeliveryReportResultGrid> transactionDeliveryReportResultGrids = new List<TransactionDeliveryReportResultGrid>();


                foreach (var party in transactionDeliveryReportResult.Parties)
                {
                    int count = transactionDeliveryReportVMs.Where(p => p.PartyId == party.Key).Count();
                    var gridParty = (AjaxGrid<TransactionDeliveryReportVM>)new AjaxGridFactory().CreateAjaxGrid(
                        transactionDeliveryReportVMs.Where(p => p.PartyId == party.Key).ToList(), page ?? 1, count, false, pageSize);
                    TransactionDeliveryReportResultGrid transactionDeliveryReportResultGrid = new TransactionDeliveryReportResultGrid()
                    {
                        PartyId = party.Key,
                        TransactionGridResultVMs = gridParty
                    };
                    transactionDeliveryReportResultGrids.Add(transactionDeliveryReportResultGrid);
                }

                transactionDeliveryReportResult.transactionDeliveryReportResultGrids = transactionDeliveryReportResultGrids;
                return Json(new
                {
                    count = AllCount,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionDeliveryReportResultPartial", transactionDeliveryReportResult),
                    UserHasTransactions = true,
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridTransactionsDeliveryReport(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                SearchCriteria searchCriteria = javaScriptSerializer.Deserialize(param, typeof(SearchCriteria)) as SearchCriteria;

                string orderBy = System.Web.HttpContext.Current.Request.QueryString["gridColumn"];

                if (orderBy == SearchFields.DateH)
                {
                    orderBy = SearchFields.Date;
                }

                searchCriteria.OrderBy = orderBy;
                searchCriteria.Ascending = Convert.ToBoolean(Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["dir"]));
                searchCriteria.PageIndex = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["page"]);
                if (searchCriteria.PageIndex == 0)
                {
                    searchCriteria.PageIndex = 1;
                }

                string strSearchCriteria = javaScriptSerializer.Serialize(searchCriteria);

                GetResult<List<TransactionDeliveryReportDTO>> transactionDeliveryReportDTOs =
                                  HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>.GetItemRequest(String.Format("api/Transaction/SearchDeliveryReport?strSearchCriteria={0}", strSearchCriteria)).Result;

                var grid = new AjaxGridFactory().CreateAjaxGrid(TransactionDeliveryReportMapper.Map(transactionDeliveryReportDTOs.Result), page.HasValue ? page.Value : 1, transactionDeliveryReportDTOs.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("_TransactionsDeliveryReportGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult DeliveryReport(int transactionId, bool isPerTransaction = true)
        {
            try
            {
                GetResult<List<DeliveryReportDTO>> deliveryReportDTOs =
                   HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReport?transactionId={0}&cultureName={1}&perTransaction={2}", transactionId, SessionInfo.CultureShortName, isPerTransaction)).Result;

                List<DeliveryReportVM> deliveryReportVMs = DeliveryReportMapper.Map(deliveryReportDTOs.Result);
                if (deliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int count = 0;
                string orgUnitName = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                deliveryReportVMs.ForEach(d =>
                {
                    d.OrgUnitName = orgUnitName;

                    d.DeliveryReportTransactions.ForEach(t =>
                    {
                        t.FromEntity = orgUnitName;
                    });

                    IAjaxGrid gridDeliveryReport = (AjaxGrid<DeliveryReportTransactionVM>)new AjaxGridFactory().CreateAjaxGrid(d.DeliveryReportTransactions, 1, d.DeliveryReportTransactions.Count(), true);
                    ViewData["DeliveryReportData" + count] = gridDeliveryReport;

                    count++;
                });

                TempData["DeliveryReport"] = deliveryReportVMs;

                TempData.Keep();


                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/DeliveryReport.cshtml", DeliveryReportMapper.Map(deliveryReportDTOs.Result)), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult DeliveryReport(List<TransactionReportInfoVM> transactionReportInfo)
        {
            try
            {
                GetResult<List<DeliveryReportDTO>> deliveryReportDTOs =
                   HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReportById?strTransactionReportInfos={0}&cultureName={1}&perTransaction={2}", transactionReportInfo, SessionInfo.CultureShortName, true)).Result;


                List<DeliveryReportVM> deliveryReportVMs = DeliveryReportMapper.Map(deliveryReportDTOs.Result);

                if (deliveryReportDTOs.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deliveryReportDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int count = 0;
                string orgUnitName = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                deliveryReportVMs.ForEach(d =>
                {
                    d.OrgUnitName = orgUnitName;

                    d.DeliveryReportTransactions.ForEach(t =>
                    {
                        t.FromEntity = orgUnitName;
                    });

                    IAjaxGrid gridDeliveryReport = (AjaxGrid<DeliveryReportTransactionVM>)new AjaxGridFactory().CreateAjaxGrid(d.DeliveryReportTransactions, 1, d.DeliveryReportTransactions.Count(), true);
                    ViewData["DeliveryReportData" + count] = gridDeliveryReport;

                    count++;
                });

                TempData["DeliveryReport"] = deliveryReportVMs;

                TempData.Keep();


                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/DeliveryReport.cshtml", deliveryReportDTOs.Result), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult PrintDeliveryReport(int transactionId, bool isPerTransaction = true)
        {
            try
            {

                GetResult<List<DeliveryReportDTO>> deliveryReportDTOResult =
                   HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReport?transactionId={0}&cultureName={1}&perTransaction={2}", transactionId, SessionInfo.CultureShortName, isPerTransaction)).Result;

                List<DeliveryReportVM> deliveryReportVMs = DeliveryReportMapper.Map(deliveryReportDTOResult.Result);

                List<DeliveryReportVM> deliveryReportVM = deliveryReportVMs;


                int count = 0;
                string orgUnitName = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                deliveryReportVMs.ForEach(d =>
                {
                    d.OrgUnitName = orgUnitName;

                    d.DeliveryReportTransactions.ForEach(t =>
                    {
                        t.FromEntity = orgUnitName;
                    });

                    IAjaxGrid gridDeliveryReport = (AjaxGrid<DeliveryReportTransactionVM>)new AjaxGridFactory().CreateAjaxGrid(d.DeliveryReportTransactions, 1, d.DeliveryReportTransactions.Count(), true);
                    ViewData["DeliveryReportData" + count] = gridDeliveryReport;

                    count++;
                });

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/DeliveryReport.cshtml", deliveryReportVMs), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult PrintDeliveryReportsByIds(string transactionReportInfo, 
            bool isPerTransaction = true, bool group = false, 
            int reportType = (int)DeliveryReportType.DeliveryReport,
            string reporterName = "", bool IsNew = false, int printCount = 1)
        {
            try
            {
                List<DeliveryReportDTO> deliveryReportDTOs = new List<DeliveryReportDTO>();

                if (group)
                {
                    GetResult<DeliveryReportDTO> deliveryReportDTOResult =
                       HttpClientWrapper<GetResult<DeliveryReportDTO>>
                       .GetItemRequest(string
                       .Format("api/Transaction/PrintTransactionsDeliveryReport?strTransactionReportInfos={0}&cultureName={1}&userId={2}&perTransaction={3}",
                       transactionReportInfo, SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, true)).Result;

                    if (deliveryReportDTOResult.Result != null)
                    {
                        if (deliveryReportDTOResult.Result.DeliveryReportTransactions != null && deliveryReportDTOResult.Result.DeliveryReportTransactions.Count > 0)
                        {
                            foreach (var item in deliveryReportDTOResult.Result.DeliveryReportTransactions)
                            {
                                if (string.IsNullOrEmpty(reporterName) == false)
                                {
                                    item.Receiver = reporterName;
                                }
                            }

                        }
                    }

                    deliveryReportDTOs.Add(deliveryReportDTOResult.Result);
                }
                else
                {
                    GetResult<List<DeliveryReportDTO>> deliveryReportDTOResult =
                       HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReportById?strTransactionReportInfos={0}&cultureName={1}&perTransaction={2}&IsNew={3}", transactionReportInfo, SessionInfo.CultureShortName, true, IsNew)).Result;

                    deliveryReportDTOs = deliveryReportDTOResult.Result;

                    foreach (var item in deliveryReportDTOResult.Result)
                    {
                        foreach (var deliveryReportTransaction in item.DeliveryReportTransactions)
                        {
                            if (string.IsNullOrEmpty(reporterName) == false)
                            {
                                deliveryReportTransaction.Receiver = reporterName;
                            }
                        }
                    }
                }

                List<DeliveryReportVM> deliveryReportVMs = DeliveryReportMapper.Map(deliveryReportDTOs);

                int count = 0;
                string orgUnitName = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                deliveryReportVMs.ForEach(d =>
                {
                    d.OrgUnitName = orgUnitName;
                    d.ReporterName = reporterName;

                    d.DeliveryReportTransactions.ForEach(t =>
                    {
                        t.FromEntity = orgUnitName;
                        t.TransactionNumberString = ArabicDigitConverter.ConvertToArabic(t.TransactionNumber.ToString());
                    });

                    d.Transactions = d.DeliveryReportTransactions;

                    /* d.DateH = ArabicDigitConverter.ConvertToArabic(d.DateH);*/
                    if (d.ReportNumber != null)
                    {
                        d.ReportNumber = ArabicDigitConverter.ConvertToArabic(d.ReportNumber);
                    }

                    count++;
                });

                switch (reportType)
                {
                    case (int)DeliveryReportType.DeliveryReport:
                        PrintDeliveryReportVM printDeliveryReportVM = new PrintDeliveryReportVM()
                        {
                            PrintCount = printCount,
                            DeliveryReportVM = deliveryReportVMs
                        };
                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/PrintDeliveryReport.cshtml", printDeliveryReportVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                    case (int)DeliveryReportType.LetterOfficialMail:
                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/OfficialMailDeliveryReport.cshtml", deliveryReportVMs), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                    case (int)DeliveryReportType.PackageOfficialMail:
                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/OfficialMailDeliveryPackReport.cshtml", deliveryReportVMs), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                    default:
                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Reports/DeliveryReport.cshtml", deliveryReportVMs), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }



        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintDeliveryData)]
        public ActionResult OpenDeliveryReport(string transactionReportInfo)
        {
            try
            {
                ViewData["TransactionReportInfo"] = transactionReportInfo;

                return View("~/Areas/User/Views/Reports/_DileveryReportDialogPartial.cshtml");
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Reports.StatisticalReportsOfTransactions)]
        public ActionResult Statistically()
        {
            try
            {
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
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
                StatisticallyDashboardViewModel statisticallyDashboardVM = new StatisticallyDashboardViewModel();

                return View(statisticallyDashboardVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.Reports.StatisticalReportsOfTransactions)]
        public ActionResult StatisticallySearch(StatisticallyDashboardViewModel statisticallyDashboardDTO)
        {
            return null;
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Reports.InboundTransactionsReports)]
        public ActionResult InboundTransactions()
        {
            try
            {
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
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

                ViewData["TransactionTypesData"] = GetTransactionTypes(TransactionCategory.Inbound);
                ViewData["PrioritiesData"] = GetPriorities(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();

                InboundDashboardViewModel inboundDashboardVM = new InboundDashboardViewModel();
                inboundDashboardVM.TransactionTypeId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                return View(inboundDashboardVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Reports.InboundTransactionsReports)]
        public ActionResult OutboundInternalTransactions()
        {
            try
            {
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
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

                ViewData["TransactionTypesData"] = GetTransactionTypes(TransactionCategory.InternalOutbound);
                ViewData["PrioritiesData"] = GetPriorities(TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();

                OutboundInternalDashboardViewModel outboundInternalDashboardVM = new OutboundInternalDashboardViewModel();
                outboundInternalDashboardVM.TransactionTypeId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                return View(outboundInternalDashboardVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.Reports.InboundTransactionsReports)]
        public ActionResult InboundTransactionsSearch(InboundDashboardViewModel inboundDashboardDTO)
        {
            return Redirect("~/ReportViewer.aspx");
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Reports.OutboundTransactionsReports)]
        public ActionResult OutboundTransactions()
        {
            try
            {
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
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

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                      HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                List<ExternalPartyVM> externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                ViewData["ExternalPartiesData"] = (externalPartyVMs != null) ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;

                ViewData["TransactionTypesData"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);
                ViewData["PrioritiesData"] = GetPriorities(TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();

                OutboundDashboardViewModel outboundDashboardVM = new OutboundDashboardViewModel();

                return View(outboundDashboardVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.Reports.OutboundTransactionsReports)]
        public ActionResult OutboundTransactionsSearch(OutboundDashboardViewModel outboundDashboardDTO)
        {
            return null;
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Reports.UserPerformanceReports)]
        public ActionResult UsersPerformance()
        {
            try
            {
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
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

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                UserPerformanceReportViewModel userPerformanceReportDTO = new UserPerformanceReportViewModel();

                return View(userPerformanceReportDTO);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult OrgUnitPerformance()
        {
            try
            {
                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
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
                OrgUnitPerformanceReportViewModel orgUnitPerformanceReportVM = new OrgUnitPerformanceReportViewModel();

                return View(orgUnitPerformanceReportVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Reports.UserPerformanceReports)]
        public ActionResult UserPerformanceSearch(UserPerformanceReportViewModel userPerformanceReportDTO)
        {
            return null;
        }

        [HttpGet]
        public string GetPriorities(int transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities((TransactionCategory)transactionCategory);
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

        private string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

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

        public string GetTransactionTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<TransactionTypeVM>> transactionTypeVMs = LookupsHelper.GetTransactionTypes(transactionCategory);


                if (transactionTypeVMs != null)
                {
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
        public string GetUsersBasedPermisstion(int id)
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (!SessionInfo.CurrentUser.Claims.Any(permisstion => permisstion.StartsWith("Reports.UserTransactionsReports")))
                {

                    var userProfileVM = userProfileVMs.FirstOrDefault(a => a.Id == SessionInfo.CurrentUser.Id);
                    if (userProfileVM != null)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName
                        });
                    }

                }
                else
                {
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
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUsersByOrgUnitId(int? id, int? level = null)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!id.HasValue || id == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }
                GetResult<List<UserProfileDTO>> userProfileDTOs = null;
                List<UserProfileVM> userProfileVMs = null;

                if (level.HasValue && (level == 3 || level == 4))
                {
                    userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetChildEntityUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;
                    dataSource = new List<AutoCompleteDataSource>();
                    userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
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
                    return JsonConvert.SerializeObject(dataSource);
                }

                userProfileDTOs =
              HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;
                dataSource = new List<AutoCompleteDataSource>();
                userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
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
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public string GetLetterTypes(int transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<LetterTypeVM>> letterTypeVMs = LookupsHelper.GetLetterTypes((TransactionCategory)transactionCategory);

                if (letterTypeVMs != null)
                {
                    foreach (LetterTypeVM letterTypeVM in letterTypeVMs.Result)
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

        private SearchColunm AddColunm(string name, string value)
        {
            return new SearchColunm()
            {
                ColunmName = name,
                ColunmValue = value,
            };
        }

        #region Transaction Report and Performance Measurement Report
        [CustomAuthorizationAttribute(UserClaims.Reports.TransactionReports)]
        public ActionResult TransactionReport()
        {
            ViewData["TransactionCategoriesForFilters"] = GetTransactionCategoriesWithSentForFilters();
            return View("~/Areas/User/Views/Reports/_TransactionReportPartial.cshtml", new TransactionReportVM());
        }

        [CustomAuthorizationAttribute(UserClaims.Reports.TransactionReports)]
        public ActionResult SecretaryTransactionReport()
        {

            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
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





            ViewData["TransactionCategoriesForFilters"] = GetTransactionCategoriesWithSentForFilters();
            return View("~/Areas/User/Views/Reports/_SecretaryTransactionReportPartial.cshtml", new TransactionReportVM
            {
                OrgUnitId = SessionInfo.OrgUnitId
            });
        }

        [CustomAuthorizationAttribute(UserClaims.Reports.TransactionReports)]
        public ActionResult TasksReport()
        {


            ViewData["TasksStatus"] = GeTaskStatusForFilters();
            ViewData["TransactionCategoriesForFilters"] = GetTransactionCategoriesForFilters();
            GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                    HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

            List<ExternalPartyVM> externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
            ViewData["ExternalPartiesData"] = (externalPartyVMs != null) ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;
            return View("~/Areas/User/Views/Reports/_TasksReportPartial.cshtml", new TransactionReportVM());
        }

        [CustomAuthorizationAttribute(UserClaims.Reports.TransactionReports)]
        public ActionResult FollowupReport()
        {
            ViewData["TransactionCategoriesForFilters"] = GetTransactionCategoriesForFilters();
            ViewData["FollowupStatus"] = GeFollowupkStatusForFilters();
            return View("~/Areas/User/Views/Reports/_FollowupReportPartial.cshtml", new FollowupReportVM());
        }


        [CustomAuthorizationAttribute(UserClaims.Reports.TransactionReports)]
        public ActionResult SentTransactionReport()
        {
            ViewData["TransactionCategoriesForFilters"] = GetTransactionCategoriesForFilters();

            GetResult<List<ExternalPartyDTO>> externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                  .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
            ViewData["ExternalPartiesData"] = (externalPartyDTOs.Result != null) ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;

            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
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

            ViewData["OrgUnitsUsersData"] = "";


            return View("~/Areas/User/Views/Reports/_SentTransactionReportPartial.cshtml", new SentTransactionReportVM());
        }
        [CustomAuthorizationAttribute(UserClaims.Reports.TransactionReports)]
        public ActionResult SentTransactionSatutsReport()
        {
            ViewData["TransactionCategoriesForFilters"] = GetTransactionCategoriesForFilters();

            GetResult<List<ExternalPartyDTO>> externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                  .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
            ViewData["ExternalPartiesData"] = (externalPartyDTOs.Result != null) ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;

            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
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

            ViewData["OrgUnitsUsersData"] = "";


            return View("~/Areas/User/Views/Reports/_SentTransactionReportStatusPartial.cshtml", new SentTransactionReportVM());
        }
        public ActionResult PowerBiDashboard()
        {
            return View("~/Areas/User/Views/Reports/_PowerBiDashboardPartial.cshtml", null);
        }


        [HttpPost]
        public ActionResult TransactionSearch(TransactionReportVM transactionReportVM, int? page)
        {
            string message = string.Empty;

            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            GetResult<List<TransactionReportResultDTO>> getResult = GetTransactionReportResult(transactionReportVM, page, level);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            TransactionReportResult transactionReportResult = new TransactionReportResult();
            GetResult<ExternalPartyEditDTO> parentPartyDTO = null;
            if ((TransactionCategory)transactionReportVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) == TransactionCategory.Inbound
                && transactionReportVM.AdditionalFieldsInboundVM.ExternalPartiesId != null && transactionReportVM.AdditionalFieldsInboundVM.ExternalPartiesId != 0)
            {
                parentPartyDTO = HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", transactionReportVM.AdditionalFieldsInboundVM.ExternalPartiesId)).Result;
                transactionReportResult.TransactionBasicResultVM.ExternalParty = parentPartyDTO.Result != null ? "المعاملات الواردة من / " + parentPartyDTO.Result.Name[0].Text : "";
            }
            else if ((TransactionCategory)transactionReportVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) == TransactionCategory.ExternalOutbound
                && transactionReportVM.AdditionalFieldsOutboundVM.ExternalPartiesId != null && transactionReportVM.AdditionalFieldsOutboundVM.ExternalPartiesId != 0)
            {
                parentPartyDTO = HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", transactionReportVM.AdditionalFieldsOutboundVM.ExternalPartiesId)).Result;
                transactionReportResult.TransactionBasicResultVM.ExternalParty = parentPartyDTO.Result != null ? "المعاملات الصادرة من / " + parentPartyDTO.Result.Name[0].Text : "";

            }
           
            transactionReportResult.TransactionBasicResultVM.Number = transactionReportVM.Number;
            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            transactionReportResult.TransactionBasicResultVM.CreatedEntity = SessionInfo.OrgUnitInfo.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
            transactionReportResult.TransactionBasicResultVM.DateFromG = transactionReportVM.From;
            transactionReportResult.TransactionBasicResultVM.DateToG = transactionReportVM.To;
            if (transactionReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = transactionReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = transactionReportVM.To.ToShortDateString();
            }


            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }

            transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

            GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<LookupVM> lookupVMs = transactionCategories.Result;
            if (lookupVMs != null)
            {
                var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                if (value != null)
                {
                    transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                }
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            foreach (TransactionReportResultDTO transactionReportResultDTO in getResult.Result)
            {
                transactionReportResultDTO.DelayedDaysCount = Int32.Parse((DateTime.Now.Date - transactionReportResultDTO.Date.Date).Days.ToString()).ToString();

            }
            List<TransactionGridResultVM> transactionGridResultVMList = TransactionReportMapper.Map(getResult.Result);
           
            HandleSubject(transactionGridResultVMList);

            var grid = (AjaxGrid<TransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(transactionGridResultVMList, page ?? 1, getResult.RowsCount.Value,
            page.HasValue, pageSize);

            transactionReportResult.TransactionGridResultVMs = grid;
            ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;
            ViewData["PageSize"] = pageSize;
            ViewData["IsPrint"] = transactionReportVM.IsPrint ?? false;

            ViewData["TransactionCategory"] = transactionReportVM.TransactionCategory;



            if (page.HasValue)
            {
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Reports/_TransactionGridResultPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value)
            {

                var printResult = TransactionReportMapper.Map(getResult.Result);
                HandleSubject(printResult);
                transactionReportResult.TransactionPrintResultVMs = printResult;

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintTransactionReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionReportResultPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }


        public static void HandleSubject(List<TransactionGridResultVM> transactionGridResultVMList)
        {
            foreach (TransactionGridResultVM item in transactionGridResultVMList)
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
                    item.TransactioDescription = "* * * *";
                }
                item.HasPermission = isPermition;
            }
        }


        public static void HandleSubject(List<TransactionReportResultDTO> transactionGridResultVMList)
        {
            foreach (TransactionReportResultDTO item in transactionGridResultVMList)
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
                    item.TransactioDescription = "* * * *";
                }

            }
        }


        [HttpPost]
        public ActionResult SecretaryTransactionSearch(TransactionReportVM transactionReportVM, int? page)
        {
            string message = string.Empty;

            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            GetResult<List<TransactionReportResultDTO>> getResult = GetSecretaryTransactionReportResult(transactionReportVM, page, level, transactionReportVM.OrgUnitId);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            foreach (TransactionReportResultDTO transactionReportResultDTO in getResult.Result)
            {
                if (int.Parse(transactionReportResultDTO.DelayedDaysCount) > 0)
                {
                    transactionReportResultDTO.TransactionStatusText = DbRes.TResource("User.Transaction.Report.Late");
                }
                else
                {
                    transactionReportResultDTO.DelayedDaysCount = "---";
                }
            }

            TransactionReportResult transactionReportResult = new TransactionReportResult();
            transactionReportResult.TransactionBasicResultVM.Number = transactionReportVM.Number;
            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;

            if (transactionReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = transactionReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = transactionReportVM.To.ToShortDateString();
            }


            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }

            transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

            GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<LookupVM> lookupVMs = transactionCategories.Result;
            if (lookupVMs != null)
            {
                var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                if (value != null)
                {
                    transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                }
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            foreach (TransactionReportResultDTO transactionReportResultDTO in getResult.Result)
            {
                transactionReportResultDTO.DelayedDaysCount = Int32.Parse((DateTime.Now.Date - transactionReportResultDTO.Date.Date).Days.ToString()).ToString();

            }
            List<TransactionGridResultVM> transactionGridResultVMList = TransactionReportMapper.Map(getResult.Result);

            HandleSubject(transactionGridResultVMList);

            var grid = (AjaxGrid<TransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(transactionGridResultVMList, page ?? 1, getResult.RowsCount.Value,
            page.HasValue, pageSize);

            transactionReportResult.TransactionGridResultVMs = grid;

            ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;
            ViewData["PageSize"] = pageSize;
            ViewData["IsPrint"] = transactionReportVM.IsPrint ?? false;



            if (page.HasValue)
            {
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Reports/_SecretaryTransactionGridResultPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value)
            {
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintSecretaryTransactionReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SecretaryTransactionReportResultPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TaskSearch(TaskReportVM transactionReportVM, int? page)
        {
            transactionReportVM.TransactionCategory = transactionReportVM.TransactionTypeId;
            string message = string.Empty;

            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            GetResult<List<TaskReportResultDTO>> getResult = GetTasksReportResult(transactionReportVM, page, level);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            TaskReportResult transactionReportResult = new TaskReportResult();
            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
            transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
            transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }

            transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

            GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<LookupVM> lookupVMs = transactionCategories.Result;
            if (lookupVMs != null)
            {
                var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                if (value != null)
                {
                    transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                }
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            var grid = (AjaxGrid<TaskGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), page ?? 1, getResult.RowsCount.Value,
            page.HasValue, pageSize);

            transactionReportResult.TransactionGridResultVMs = grid;
            ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;
            ViewData["PageSize"] = pageSize;
            ViewData["IsPrint"] = transactionReportVM.IsPrint ?? false;
            if (page.HasValue)
            {
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Reports/_TasksGridResultPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value)
            {
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintTaskReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }
            if (transactionReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = transactionReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = transactionReportVM.To.ToShortDateString();
            }
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TasksReportResultPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SentTransactionSearch(SentTransactionReportVM transactionReportVM, int? page)
        {
            string message = string.Empty;

            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            GetResult<List<SentTransactionReportResultDTO>> getResult = GetSentTransactionReportResult(transactionReportVM, page, level);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            SentTransactionReportResult transactionReportResult = new SentTransactionReportResult();
            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
            transactionReportResult.TransactionBasicResultVM.DateFromG = transactionReportVM.From;
            transactionReportResult.TransactionBasicResultVM.DateToG = transactionReportVM.To;
            if (transactionReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = transactionReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = transactionReportVM.To.ToShortDateString();
            }
            //transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
            //transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }

            transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

            GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<LookupVM> lookupVMs = transactionCategories.Result;
            if (lookupVMs != null)
            {
                var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                if (value != null)
                {
                    transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                }
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            var grid = (AjaxGrid<SentTransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), page ?? 1, getResult.RowsCount.Value,
            page.HasValue, pageSize);

            transactionReportResult.TransactionGridResultVMs = grid;
            ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;
            ViewData["PageSize"] = pageSize;
            ViewData["IsPrint"] = transactionReportVM.IsPrint ?? false;
            if (page.HasValue)
            {
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Reports/_SentTransactionGridResultPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value)
            {
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintSentTransactionReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SentTransactionReportResultPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult SentTransactionStatusSearch(SentTransactionReportVM transactionReportVM, int? page)
        {
            string message = string.Empty;

            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            GetResult<List<SentTransactionReportResultDTO>> getResult = GetSentTransactionReporStatustResult(transactionReportVM, page, level);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            SentTransactionReportResult transactionReportResult = new SentTransactionReportResult();
            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
            transactionReportResult.TransactionBasicResultVM.DateFromG = transactionReportVM.From;
            transactionReportResult.TransactionBasicResultVM.DateToG = transactionReportVM.To;
            if (transactionReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = transactionReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = transactionReportVM.To.ToShortDateString();
            }
           
            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }

            transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

            GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<LookupVM> lookupVMs = transactionCategories.Result;
            if (lookupVMs != null)
            {
                var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                if (value != null)
                {
                    transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                }
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            var grid = (AjaxGrid<SentTransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), page ?? 1, getResult.RowsCount.Value,
            page.HasValue, pageSize);

            transactionReportResult.TransactionGridResultVMs = grid;
            ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;
            ViewData["PageSize"] = pageSize;
            ViewData["IsPrint"] = transactionReportVM.IsPrint ?? false;
            if (page.HasValue)
            {
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Reports/_SentTransactionStatusGridResultPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value)
            {
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintSentTransactionReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SentTransactionStautsResultReportPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult FollowupSearch(FollowupReportVM transactionReportVM, int? page)
        {
            string message = string.Empty;

            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            GetResult<List<FollowupReportResultDTO>> getResult = GetFollowupReportResult(transactionReportVM, page, level);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            FollowupReportResult transactionReportResult = new FollowupReportResult();

            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
            transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
            transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }

            transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

            GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<LookupVM> lookupVMs = transactionCategories.Result;
            if (lookupVMs != null)
            {
                var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                if (value != null)
                {
                    transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                }
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            var grid = (AjaxGrid<FollowupGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), page ?? 1, getResult.RowsCount.Value,
            page.HasValue, pageSize);
            if (transactionReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = transactionReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = transactionReportVM.To.ToShortDateString();
            }
            transactionReportResult.TransactionGridResultVMs = grid;
            ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;
            ViewData["PageSize"] = pageSize;
            ViewData["IsPrint"] = transactionReportVM.IsPrint ?? false;
            if (page.HasValue)
            {
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Reports/_FollowupGridResultPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value)
            {
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintFollowupReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_FollowupReportResultPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public ActionResult GetTransactionSearchChooser(int transactionCategory)
        {
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionSearchChooser", (TransactionCategory)transactionCategory)
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetSearchSection(int type)
        {
            string viewName = string.Empty;
            object model = null;
            switch ((SearchChosser)type)
            {
                #region Common
                case SearchChosser.Common:
                    ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(TransactionCategory.ExternalOutbound);
                    ViewData["TransactionStatus"] = TransactionHelper.GetLookupItemsForAutoComplete(LookupCategory.TransactionStatus);
                    ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                    ViewData["PrioritiesData"] = TransactionHelper.GetPriorities(TransactionCategory.ExternalOutbound);
                    ViewData["LetterTypeData"] = GetLetterTypes(TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));
                    ViewData["DeliveryMethod"] = GetLookupItemsByType(LookupCategory.DeliveryMethod);
                    viewName = "_SearchCommon";
                    model = new CommonVM();
                    break;
                #endregion

                #region Names
                case SearchChosser.Names:
                    viewName = "_SearchNames";
                    model = new NamesVM();
                    break;
                #endregion

                #region External
                case SearchChosser.External:
                    GetResult<List<ExternalPartyDTO>> externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                        .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                    ViewData["ExternalPartiesData"] = (externalPartyDTOs.Result != null) ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;
                    viewName = "_AdditionalFieldsOutbound";
                    model = new AdditionalFieldsOutboundVM();
                    break;
                #endregion

                #region Inbound,Internal,Draft
                case SearchChosser.Inbound:
                case SearchChosser.Internal:
                case SearchChosser.Draft:
                    GetResult<List<ExternalPartyDTO>> inboundPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                    ViewData["ExternalPartiesData"] = (inboundPartyDTOs.Result != null) ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(inboundPartyDTOs.Result)) : null;
                    ViewData["EntitiesType"] = GetLookupItemsByType(LookupCategory.Entity);
                    viewName = "_AdditionalFieldsInbound";
                    model = new AdditionalFieldsInboundVM();
                    break;
                #endregion

                #region Transferrd
                case SearchChosser.Transferrd:
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                .GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                    ViewData["OrgUnitsUsersData"] = "";
                    viewName = "_AdditionalFieldsTransactionAssignment";
                    model = new SearchAssignmentVM();
                    break;
                #endregion

                #region Employees
                case SearchChosser.Employees:
                    viewName = "_SearchEmployees";
                    ViewData["OrgUnitsUsersData"] = "";
                    model = new EmployeeVM();
                    break;
                    #endregion
            }
            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, viewName, model) }, JsonRequestBehavior.AllowGet);
        }
        [CustomAuthorizationAttribute(UserClaims.Reports.SaveReports)]
        public ActionResult ExportToPdf(TransactionReportVM transactionReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                transactionReportVM.IsPrint = true;
                GetResult<List<TransactionReportResultDTO>> getResult = GetTransactionReportResult(transactionReportVM, page);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TransactionReportResult transactionReportResult = new TransactionReportResult();
                transactionReportResult.TransactionBasicResultVM.Number = transactionReportVM.Number;
                transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
                var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
                transactionReportResult.TransactionBasicResultVM.DateFromG = transactionReportVM.From;
                transactionReportResult.TransactionBasicResultVM.DateToG = transactionReportVM.To;
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
                transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;


                transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                    transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                    if (value != null)
                    {
                        transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                    }
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                foreach (TransactionReportResultDTO transactionReportResultDTO in getResult.Result)
                {
                    transactionReportResultDTO.DelayedDaysCount = Int32.Parse((DateTime.Now.Date - transactionReportResultDTO.Date.Date).Days.ToString()).ToString();

                }
                var transactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                var grid = (AjaxGrid<TransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(transactionPrintResultVMs, 1, getResult.RowsCount.Value,
                page.HasValue, pageSize);

                HandleSubject(transactionPrintResultVMs);
                transactionReportResult.TransactionPrintResultVMs = transactionPrintResultVMs;
                transactionReportResult.TransactionGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintTransactionReport", transactionReportResult);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "TransactionReport.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult ExportToExcel(TransactionReportVM transactionReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                transactionReportVM.IsPrint = true;
                GetResult<List<TransactionReportResultDTO>> getResult = GetTransactionReportResult(transactionReportVM, page);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                var transactions = getResult.Result;
                HandleSubject(transactions);
                string handle = Guid.NewGuid().ToString();
                var excel = ConvertReportToExcel(transactions, transactionReportVM.ColumnsToGrid);

                TempData[handle] = excel;

                FileResult fileResult = new FileContentResult(excel, "aapplication/ms-excel");
                fileResult.FileDownloadName = "TransactionReport.xls";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static byte[] ConvertReportToExcel(List<TransactionReportResultDTO> list, List<int> columnsToGrid)
        {
            DataTable dt = new DataTable();
            List<TransactionReportGridColumn> collsIndexList = new List<TransactionReportGridColumn>();

            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.Number)))
            {
                dt.Columns.Add(DbRes.TResource("User.Inbound.Open.InboundNumber"));
                collsIndexList.Add(TransactionReportGridColumn.Number);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.TransactionType)))
            {
                dt.Columns.Add(DbRes.TResource("User.File.TransactionType"));
                collsIndexList.Add(TransactionReportGridColumn.TransactionType);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.OrgUnit)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.CreationEntity"));
                collsIndexList.Add(TransactionReportGridColumn.OrgUnit);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.Date)))
            {
                dt.Columns.Add(DbRes.TResource("User.InboundSearch.Date"));
                collsIndexList.Add(TransactionReportGridColumn.Date);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.SourceType)))
            {
                dt.Columns.Add(DbRes.TResource("User.OutboundExternal.BasicInfo.Type"));
                collsIndexList.Add(TransactionReportGridColumn.SourceType);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.TransactioDescription)))
            {
                dt.Columns.Add(DbRes.TResource("User.SubjectSearch.Subject"));
                collsIndexList.Add(TransactionReportGridColumn.TransactioDescription);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.Confidentiality)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.ConfidentialityLevel"));
                collsIndexList.Add(TransactionReportGridColumn.Confidentiality);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.Priority)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.PriorityLevel"));
                collsIndexList.Add(TransactionReportGridColumn.Priority);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.SubjectClassification)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.BasicInfo.SubjectClassifications"));
                collsIndexList.Add(TransactionReportGridColumn.SubjectClassification);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.Remarks)))
            {
                dt.Columns.Add(DbRes.TResource("User.Inbound.BasicInfo.Remarks"));
                collsIndexList.Add(TransactionReportGridColumn.Remarks);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.DeliveryMethod)))
            {
                dt.Columns.Add(DbRes.TResource("User.Inbound.BasicInfo.ReceiveMethod"));
                collsIndexList.Add(TransactionReportGridColumn.DeliveryMethod);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.FullName)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Name.FullName"));
                collsIndexList.Add(TransactionReportGridColumn.FullName);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.CivilID)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Name.CivilID"));
                collsIndexList.Add(TransactionReportGridColumn.CivilID);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.MobileNumber)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Name.MobileNumber"));
                collsIndexList.Add(TransactionReportGridColumn.MobileNumber);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.ExternalParty)))
            {
                dt.Columns.Add(DbRes.TResource("User.OutboundAdvancedSearch.ExternaParty"));
                collsIndexList.Add(TransactionReportGridColumn.ExternalParty);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.InboundDateH)))
            {
                dt.Columns.Add(DbRes.TResource("User.Inbound.BasicInfo.date"));
                collsIndexList.Add(TransactionReportGridColumn.InboundDateH);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.FromEntity)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Search.FromOrg"));
                collsIndexList.Add(TransactionReportGridColumn.FromEntity);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.FromUser)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Search.FromEmployee"));
                collsIndexList.Add(TransactionReportGridColumn.FromUser);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.ToEntity)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Search.ToOrg"));
                collsIndexList.Add(TransactionReportGridColumn.ToEntity);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.ToUser)))
            {
                dt.Columns.Add(DbRes.TResource("User.Transaction.Search.ToEmployee"));
                collsIndexList.Add(TransactionReportGridColumn.ToUser);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.CreatedOn)))
            {
                dt.Columns.Add(DbRes.TResource("User.OutboundInternal.RecordDate"));
                collsIndexList.Add(TransactionReportGridColumn.CreatedOn);
            }
            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.RemindDate)))
            {
                dt.Columns.Add(DbRes.TResource("Admin.Priority.HasDate"));
                collsIndexList.Add(TransactionReportGridColumn.RemindDate);
            }

            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.TransactionStatus)))
            {
                dt.Columns.Add(DbRes.TResource("User.SubjectSearch.TransactionStatus"));
                collsIndexList.Add(TransactionReportGridColumn.TransactionStatus);

                dt.Columns.Add("اخر اجراء / سبب الحفظ");
                collsIndexList.Add(TransactionReportGridColumn.TransactionStatus  + 1);
            }


            if (columnsToGrid.Contains(Convert.ToInt32(TransactionReportGridColumn.DelayText)))
            {
                dt.Columns.Add("متأخرة");
                collsIndexList.Add(TransactionReportGridColumn.DelayText);

                dt.Columns.Add("عدد ايام التأخير");
                collsIndexList.Add(TransactionReportGridColumn.DelayText + 1);
            }
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                if (collsIndexList.Contains(TransactionReportGridColumn.Number))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.Number)] = item.Number;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.TransactionType))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.TransactionType)] = item.TransactionTypeText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.OrgUnit))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.OrgUnit)] = item.OrgUnitText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.Date))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.Date)] = item.Date;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.SourceType))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.SourceType)] = item.TransactionTypeText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.TransactioDescription))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.TransactioDescription)] = item.TransactioDescription;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.Confidentiality))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.Confidentiality)] = item.ConfidentialityText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.Priority))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.Priority)] = item.PriorityText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.SubjectClassification))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.SubjectClassification)] = item.Subject;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.Remarks))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.Remarks)] = item.Remarks;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.DeliveryMethod))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.DeliveryMethod)] = item.DeliveryMethodText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.FullName))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.FullName)] = item.FirstName;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.CivilID))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.CivilID)] = item.CivilID;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.MobileNumber))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.MobileNumber)] = item.MobileNumber;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.ExternalParty))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.ExternalParty)] = item.ExternalPartyText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.InboundDateH))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.InboundDateH)] = item.InboundDateH;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.FromEntity))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.FromEntity)] = item.FromEntityText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.FromUser))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.FromUser)] = item.FromUserText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.ToEntity))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.ToEntity)] = item.ToEntityText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.ToUser))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.ToUser)] = item.ToUserText;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.CreatedOn))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.CreatedOn)] = item.CreatedOn;
                }
                if (collsIndexList.Contains(TransactionReportGridColumn.RemindDate))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.RemindDate)] = item.RemindDate;
                }


                if (collsIndexList.Contains(TransactionReportGridColumn.RemindDate))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.RemindDate)] = item.RemindDate;
                }


                if (collsIndexList.Contains(TransactionReportGridColumn.DelayText))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.DelayText)] = item.DelayText;
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.DelayText + 1)] = Int32.Parse((DateTime.Now.Date - item.Date.Date).Days.ToString()).ToString();
                }


                if (collsIndexList.Contains(TransactionReportGridColumn.TransactionStatus))
                {
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.TransactionStatus)] = item.TransactionStatusText;
                    dr[collsIndexList.IndexOf(TransactionReportGridColumn.TransactionStatus + 1)] = item.SavedReason;
                }
                dt.Rows.Add(dr);

            }

            var memoryStream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.None);
                worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                worksheet.DefaultRowHeight = 18;
                worksheet.DefaultColWidth = 20;
                return excelPackage.GetAsByteArray();
            }
        }



        [CustomAuthorizationAttribute(UserClaims.Reports.SaveReports)]
        public ActionResult SentTransactionExportToPdf(SentTransactionReportVM transactionReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                transactionReportVM.IsPrint = true;
                GetResult<List<SentTransactionReportResultDTO>> getResult = GetSentTransactionReportResult(transactionReportVM, page);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                SentTransactionReportResult transactionReportResult = new SentTransactionReportResult();
                transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
                var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
                transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
                transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                    transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                    if (value != null)
                    {
                        transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                    }
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                var grid = (AjaxGrid<SentTransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), 1, getResult.RowsCount.Value,
                page.HasValue, pageSize);
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                transactionReportResult.TransactionGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintSentTransactionReport", transactionReportResult);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "SentTransactionReport.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Reports.SaveReports)]
        public ActionResult TaskReportExportToPdf(TaskReportVM taskReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                taskReportVM.IsPrint = true;
                GetResult<List<TaskReportResultDTO>> getResult = GetTasksReportResult(taskReportVM, page);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TaskReportResult transactionReportResult = new TaskReportResult();
                transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
                var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(taskReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(taskReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
                transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
                transactionReportResult.TransactionBasicResultVM.TotalCount = taskReportVM.IsPrint.HasValue && taskReportVM.IsPrint.Value ? taskReportVM.TotalCount : getResult.RowsCount.Value;

                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    var value = lookupVMs.FirstOrDefault(a => a.Id == taskReportVM.TransactionCategory);
                    transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                    if (value != null)
                    {
                        transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                    }
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                var grid = (AjaxGrid<TaskGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), 1, getResult.RowsCount.Value,
                page.HasValue, pageSize);
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                transactionReportResult.TransactionGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = taskReportVM.ColumnsToGrid;

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintTaskReport", transactionReportResult);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "TaskReport.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }


        [CustomAuthorizationAttribute(UserClaims.Reports.SaveReports)]
        public ActionResult FollowupReportExportToPdf(FollowupReportVM transactionReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                transactionReportVM.IsPrint = true;
                GetResult<List<FollowupReportResultDTO>> getResult = GetFollowupReportResult(transactionReportVM, page);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                FollowupReportResult transactionReportResult = new FollowupReportResult();

                transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
                var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
                transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;

                transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                    transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                    if (value != null)
                    {
                        transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                    }
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                var grid = (AjaxGrid<FollowupGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), 1, getResult.RowsCount.Value,
                page.HasValue, pageSize);
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                transactionReportResult.TransactionGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintFollowupReport", transactionReportResult);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "FollowupReport.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [ValidateAntiForgeryToken]
        public ActionResult ConvertToPdf(string transactionReportInfo, bool group)
        {
            try
            {
                List<DeliveryReportDTO> deliveryReportDTOs = new List<DeliveryReportDTO>();

                bool IsNew = false;

                if (group)
                {
                    GetResult<DeliveryReportDTO> deliveryReportDTOResult =
                       HttpClientWrapper<GetResult<DeliveryReportDTO>>.GetItemRequest(string.Format("api/Transaction/PrintTransactionsDeliveryReport?strTransactionReportInfos={0}&cultureName={1}&userId={2}&perTransaction={3}", transactionReportInfo, SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, true)).Result;

                    deliveryReportDTOs.Add(deliveryReportDTOResult.Result);
                }
                else
                {
                    GetResult<List<DeliveryReportDTO>> deliveryReportDTOResult =
                       HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReportById?strTransactionReportInfos={0}&cultureName={1}&perTransaction={2}&IsNew={3}", transactionReportInfo, SessionInfo.CultureShortName, true, IsNew)).Result;

                    deliveryReportDTOs = deliveryReportDTOResult.Result;

                }

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionReportInfoDTO> transactionReportInfoDTOs = javaScriptSerializer.Deserialize<List<TransactionReportInfoDTO>>(transactionReportInfo) as List<TransactionReportInfoDTO>;

                //GetResult<List<DeliveryReportDTO>> deliveryReportDTOResult =
                //   HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReportById?strTransactionReportInfos={0}&cultureName={1}&perTransaction={2}&IsNew={3}", transactionReportInfo, SessionInfo.CultureShortName, true, IsNew)).Result;
                //deliveryReportDTOs = deliveryReportDTOResult.Result;

                List<DeliveryReportVM> deliveryReportVMs = DeliveryReportMapper.Map(deliveryReportDTOs);
                int count = 0;

                GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
     HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}", SessionInfo.CultureShortName, transactionReportInfoDTOs.FirstOrDefault().TransactionId, SessionInfo.OrgUnitId)).Result;
                if (transactionBarcodesDTOs == null)
                {
                    string message = DbRes.TValidation("User.PrinttingBarcodes.TransactionIsDeleted");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);
                if (transactionBarcodesDTOs.StatusCode != StatusCode.Ok)
                {
                    string message = DbRes.TResource("User.Barcodes.NotCreateInAdmin");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                foreach (BarcodeVM barcodeVM in transactionBarcodesVM.BarcodeVMs)
                {
                    if (barcodeVM.Type == BarcodePrintType.Transaction)
                    {

                        FillBarcodeDesign(transactionBarcodesVM.TransactionBarcodeHtmlDesign, barcodeVM, transactionBarcodesVM, transactionBarcodesVM.TransactionDesignWidth, transactionBarcodesVM.TransactionDesignHeight);
                    }
                    else if (barcodeVM.Type == BarcodePrintType.Copy)
                    {
                        FillBarcodeDesign(transactionBarcodesVM.TransactionBarcodeHtmlDesign, barcodeVM, transactionBarcodesVM, transactionBarcodesVM.TransactionDesignWidth, transactionBarcodesVM.TransactionDesignHeight);
                    }
                    else
                    {
                        AttachmentBarcodeVM attachmentBarcodeVM = transactionBarcodesVM.AttachmentBarcodes.Where(t => t.Id == barcodeVM.ReferenceId).ToList().FirstOrDefault();
                        FillAttachmentDesgin(transactionBarcodesVM.TransactionAttachmentHtml, barcodeVM, transactionBarcodesVM, attachmentBarcodeVM, transactionBarcodesVM.TransactionDesignWidth, transactionBarcodesVM.TransactionDesignHeight);
                    }
                }


                string orgUnitName = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                deliveryReportVMs.ForEach(d =>
                {
                    d.transactionBarcodesVM = new TransactionBarcodesVM();
                    d.OrgUnitName = orgUnitName;

                    d.transactionBarcodesVM.AttachmentBarcodes = transactionBarcodesVM.AttachmentBarcodes;
                    d.transactionBarcodesVM.CompanyName = transactionBarcodesVM.CompanyName;
                    d.transactionBarcodesVM.Date = transactionBarcodesVM.Date;
                    d.transactionBarcodesVM.DateH = transactionBarcodesVM.DateH;
                    d.transactionBarcodesVM.Entity = transactionBarcodesVM.Entity;
                    d.transactionBarcodesVM.TransactionAttachmentHtml = transactionBarcodesVM.TransactionAttachmentHtml;
                    d.transactionBarcodesVM.TransactionBarcodeHtmlDesign = transactionBarcodesVM.TransactionBarcodeHtmlDesign;
                    d.transactionBarcodesVM.TransactionDate = transactionBarcodesVM.TransactionDate;
                    d.transactionBarcodesVM.TransactionDateH = transactionBarcodesVM.TransactionDateH;
                    d.transactionBarcodesVM.VisitTicketHtmlDesign = transactionBarcodesVM.VisitTicketHtmlDesign;
                    d.transactionBarcodesVM.TransactionCategory = transactionBarcodesVM.TransactionCategory;
                    d.transactionBarcodesVM.TransactionType = transactionBarcodesVM.TransactionType;
                    d.transactionBarcodesVM.TransactionNumber = transactionBarcodesVM.TransactionNumber;
                    d.transactionBarcodesVM.TransactionDesignWidth = transactionBarcodesVM.TransactionDesignWidth;
                    d.transactionBarcodesVM.TransactionDesignHeight = transactionBarcodesVM.TransactionDesignHeight;
                    d.transactionBarcodesVM.BarcodeVMs = transactionBarcodesVM.BarcodeVMs;

                    d.DeliveryReportTransactions.ForEach(t =>
                    {
                        t.FromEntity = orgUnitName;
                        t.TransactionNumberString = ArabicDigitConverter.ConvertToArabic(t.TransactionNumber.ToString());
                        t.DateH = t.DateH.ToString();
                    });

                    d.Transactions = d.DeliveryReportTransactions;

                    d.DateH = /*ArabicDigitConverter.ConvertToArabic(*/d.DateH/*)*/;
                    d.ReportNumber = ArabicDigitConverter.ConvertToArabic(d.ReportNumber);

                    count++;

                });
                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "DeliveryReport", deliveryReportVMs);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "TransactionReport.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void FillBarcodeDesign(string HtmlDesign, BarcodeVM barcodeVM, TransactionBarcodesVM transactionBarcodesVM, int width, int heigth)
        {
            try
            {
                string barcode2D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, (heigth / 8), ((width / 3) * 2));
                string barcode3D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.QR_CODE, heigth, (width / 7));
                if (SessionInfo.CultureShortName == Constants.Languages.English)
                {
                    HtmlDesign = HtmlDesign.Replace("direction", Constants.LeftDirection);
                }

                HtmlDesign = HtmlDesign.Replace("{1}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{2}", "<img style='max-width:100%;max-height:100%;width:" + ((width / 3) * 2).ToString() + ";height:" + (heigth / 8).ToString() + "' src='" + barcode2D + "' />");
                HtmlDesign = HtmlDesign.Replace("{3}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{4}", "<img style='max-width:100%;max-height:100%;width:" + (width / 7).ToString() + ";height:" + (width / 7).ToString() + "' src='" + barcode3D + "' />");

                HtmlDesign = HtmlDesign.Replace("{9}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{10}", ArabicDigitConverter.ConvertToArabic(transactionBarcodesVM.TransactionNumber.ToString()));
                HtmlDesign = HtmlDesign.Replace("{11}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{5}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Department") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{6}", transactionBarcodesVM.Entity);
                if (transactionBarcodesVM.TransactionCategory == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) || transactionBarcodesVM.TransactionCategory == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
                {
                    HtmlDesign = HtmlDesign.Replace("{13}", string.Empty);
                    HtmlDesign = HtmlDesign.Replace("{12}", transactionBarcodesVM.TransactionType);
                }

                HtmlDesign = HtmlDesign.Replace("{13}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DirectedDepartment") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{14}", transactionBarcodesVM.Entity);
                HtmlDesign = HtmlDesign.Replace("{7}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{8}", ArabicDigitConverter.ConvertToArabic(transactionBarcodesVM.TransactionDateH));
                HtmlDesign = HtmlDesign.Replace("{15}", transactionBarcodesVM.TransactionType);
                HtmlDesign = HtmlDesign.Replace("{16}", string.Empty);


                string attachmentValue = string.Empty;


                foreach (AttachmentBarcodeVM attachmentBarcodeVM in transactionBarcodesVM.AttachmentBarcodes)
                {
                    attachmentValue = attachmentValue + string.Format("{0} {1} {2}", attachmentBarcodeVM.Count.ToString(), attachmentBarcodeVM.Name, ",");
                }

                attachmentValue = attachmentValue.TrimEnd(',');
                HtmlDesign = HtmlDesign.Replace("{12}", attachmentValue);
                barcodeVM.Content = ConvertHtmlToImageBytes(HtmlDesign, width, heigth);
                barcodeVM.Templete = HtmlDesign;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void FillAttachmentDesgin(string htmlDesign, BarcodeVM barcodeVM, TransactionBarcodesVM transactionBarcodesVM, AttachmentBarcodeVM attachmentBarcodeVM, int width, int heigth)
        {
            string barcode2D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, (heigth / 8), ((width / 3) * 2));
            string barcode3D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.QR_CODE, heigth, (width / 7));
            if (SessionInfo.CultureShortName == Constants.Languages.English)
            {
                htmlDesign = htmlDesign.Replace("direction", Constants.LeftDirection);
            }
            htmlDesign = htmlDesign.Replace("{attachmentOrgunit}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Orgunit") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentCount}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Count") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentName}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentName") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentDate}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentDate") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentOrgunitValue}", transactionBarcodesVM.Entity);
            htmlDesign = htmlDesign.Replace("{attachmentCountValue}", ArabicDigitConverter.ConvertToArabic(attachmentBarcodeVM.Count.ToString()));
            htmlDesign = htmlDesign.Replace("{attachmentNameValue}", attachmentBarcodeVM.Name);
            htmlDesign = htmlDesign.Replace("{attachmentDateValue}", ArabicDigitConverter.ConvertToArabic(transactionBarcodesVM.DateH));
            htmlDesign = htmlDesign.Replace("{attachment2DImageValue}", "<img style='max-width:100%;max-height:100%;width:" + ((width / 3) * 2).ToString() + ";height:" + (heigth / 8).ToString() + "' src='" + barcode2D + "' />");
            htmlDesign = htmlDesign.Replace("{attachment2DImage}", "");
            htmlDesign = htmlDesign.Replace("{attachment3DImageValue}", "<div style='width:150px;height:100px'> <img style='max-width:100%;max-height:100%;width:" + (width / 7).ToString() + ";height:" + (width / 7).ToString() + "' src='" + barcode3D + "' /> </div>");
            htmlDesign = htmlDesign.Replace("{attachment3DImage}", "");

            barcodeVM.Content = ConvertHtmlToImageBytes(htmlDesign, width, heigth);
            barcodeVM.Templete = htmlDesign;
        }
        public byte[] ConvertHtmlToImageBytes(string htmlString, int width, int height)
        {
            try
            {
                string header = "<head><meta charset='utf-8'></head>";


                var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();

                htmlToImageConv.Width = width;

                htmlToImageConv.Height = height;

                return htmlToImageConv.GenerateImage(header + htmlString, NReco.ImageGenerator.ImageFormat.Png);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [CustomAuthorizationAttribute(UserClaims.Reports.UserPerformanceReports)]
        public ActionResult PerformanceMeasurementReport()
        {
            ViewData["ReportType"] = GetLookupItemsByType(LookupCategory.ReportType);
            ViewData["RepresentationType"] = GetLookupItemsByType(LookupCategory.RepresentationType);
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;

                GetResult<OrgUnitDTO> orgUnitDTO =
                   HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTO.Result.ParentId = -1;
                orgUnitDTO.Result.HasChilds = true;
                newList.Add(orgUnitDTO.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList));
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;

            }

            PerformanceMeasurementReportVM model = new PerformanceMeasurementReportVM
            {
                OrgUnitId = SessionInfo.OrgUnitId,
                Level = level,
                EmployeeVM = new EmployeeVM { EmployeeId = level == 1 ? SessionInfo.CurrentUser.Id : 0 }
            };
            return View("~/Areas/User/Views/Reports/_PerformanceMeasurementReportPartial.cshtml", model);
        }
        [HttpPost]
        public ActionResult PerformanceSearch(PerformanceMeasurementReportVM performanceMeasurementReportVM, int? page)
        {
            performanceMeasurementReportVM.RepresentationTypeId = 472;
            string message = string.Empty;

            performanceMeasurementReportVM.To = performanceMeasurementReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);
            GetResult<List<PerformanceMeasurementReportResultDTO>> getResult = GetPerformanceMeasurementResult(performanceMeasurementReportVM, page);
            if (getResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            TransactionReportResult transactionReportResult = new TransactionReportResult();
            transactionReportResult.RepresentationReportType = (RepresentationReportType)performanceMeasurementReportVM.RepresentationTypeId.LookupInternalID(LookupCategory.RepresentationType, SessionInfo.CultureShortName);
            transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
            transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
            var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
            transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(performanceMeasurementReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
            transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(performanceMeasurementReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }
            transactionReportResult.TransactionBasicResultVM.TotalCount = performanceMeasurementReportVM.IsPrint.HasValue && performanceMeasurementReportVM.IsPrint.Value ? performanceMeasurementReportVM.TotalCount : getResult.RowsCount.Value;
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            //int GridSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            int GridSize = 1000;
            //int PageSize = (RepresentationReportType)performanceMeasurementReportVM.RepresentationTypeId.LookupInternalID(LookupCategory.RepresentationType, SessionInfo.CultureShortName) == RepresentationReportType.Barchart ? 1 : GridSize;
            int PageSize = 1000;

            List<PerformanceMeasurementGridResultVM> PerformanceMeasurementGridResultVMList = TransactionReportMapper.Map(getResult.Result);

            var grid = (AjaxGrid<PerformanceMeasurementGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(PerformanceMeasurementGridResultVMList, page ?? 1, getResult.RowsCount.Value,
            page.HasValue, PageSize);

            transactionReportResult.PerformanceMeasurementGridResultVMs = grid;
            ViewData["ColumnsToGrid"] = performanceMeasurementReportVM.ColumnsToGrid;
            ViewData["PageSize"] = PageSize;
            ViewData["IsPrint"] = performanceMeasurementReportVM.IsPrint ?? false;
            ViewData["ShowReportUser"] = (TransactionReportType)performanceMeasurementReportVM.ReportTypeId.LookupInternalID(LookupCategory.ReportType, SessionInfo.CultureShortName) == TransactionReportType.PerformanceMeasurementStaff;

            ViewData["ReportType"] = performanceMeasurementReportVM.ReportTypeId.LookupInternalID(LookupCategory.ReportType, SessionInfo.CultureShortName);
            if (page.HasValue)
            {
                string gridPartialViewName = "~/Areas/User/Views/Reports/_PerformanceMeasurementGridResultPartial.cshtml";
                grid = (AjaxGrid<PerformanceMeasurementGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), page ?? 1, getResult.RowsCount.Value,
                         page.HasValue, PageSize);
                if ((RepresentationReportType)performanceMeasurementReportVM.RepresentationTypeId.LookupInternalID(LookupCategory.RepresentationType, SessionInfo.CultureShortName) == RepresentationReportType.Barchart)
                {
                    gridPartialViewName = "~/Areas/User/Views/Reports/_PerformanceMeasurementBarchartGridResultPartial.cshtml";
                }
                return Json(new { Html = grid.ToJson(gridPartialViewName, this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (performanceMeasurementReportVM.IsPrint.HasValue && performanceMeasurementReportVM.IsPrint.Value)
            {
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintPerformanceMeasurementReport", transactionReportResult)
                }, JsonRequestBehavior.AllowGet);
            }
            if (performanceMeasurementReportVM.ummalqura != null)
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(performanceMeasurementReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(performanceMeasurementReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
            }
            else
            {
                transactionReportResult.TransactionBasicResultVM.DateFrom = performanceMeasurementReportVM.From.ToShortDateString();
                transactionReportResult.TransactionBasicResultVM.DateTo = performanceMeasurementReportVM.To.ToShortDateString();
            }
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PerformanceMeasurementReportResultPartial", transactionReportResult),
                UserHasTransactions = true,
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PerformanceMeasurementExportToPdf(PerformanceMeasurementReportVM performanceMeasurementReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                performanceMeasurementReportVM.IsPrint = true;
                GetResult<List<PerformanceMeasurementReportResultDTO>> getResult = GetPerformanceMeasurementResult(performanceMeasurementReportVM, page);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TransactionReportResult transactionReportResult = new TransactionReportResult();
                transactionReportResult.RepresentationReportType = (RepresentationReportType)performanceMeasurementReportVM.RepresentationTypeId.LookupInternalID(LookupCategory.RepresentationType, SessionInfo.CultureShortName);
                transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
                var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(performanceMeasurementReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(performanceMeasurementReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
                transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;
                transactionReportResult.TransactionBasicResultVM.TenantName = SessionInfo.CurrentUser.TenantName;
                if (SessionInfo.CurrentUser.TenantLogo != null)
                {
                    ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
                }
                transactionReportResult.TransactionBasicResultVM.TotalCount = performanceMeasurementReportVM.IsPrint.HasValue && performanceMeasurementReportVM.IsPrint.Value ? performanceMeasurementReportVM.TotalCount : getResult.RowsCount.Value;


                var grid = (AjaxGrid<PerformanceMeasurementGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), 1, getResult.RowsCount.Value,
                page.HasValue, UIHelper.PageSize);

                transactionReportResult.PerformanceMeasurementGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = performanceMeasurementReportVM.ColumnsToGrid;
                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintPerformanceMeasurementReport", transactionReportResult);

                //return Json(new { Html, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "PerformanceMeasurement.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #region Private Methods
        [HttpGet]
        public virtual ActionResult Download(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] pdfContent = TempData[fileGuid] as byte[];
                if (pdfContent == null)
                {
                    return null;
                }
                return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
        private string GetTransactionCategoriesForFilters()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    if (lookupVMs.Count == 6)
                    {
                        lookupVMs.RemoveAt(5);
                        lookupVMs.RemoveAt(4);
                    }
                    dataSource.Add(UIHelper.GetDefaultSelect());

                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = transactionType.Id.ToString(),
                            Label = transactionType.Text
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

        private string GeTaskStatusForFilters()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> taskStatusList = LookupsHelper.GetLookupItems(LookupCategory.TaskStatus, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = taskStatusList.Result;
                if (lookupVMs != null)
                {

                    dataSource.Add(UIHelper.GetDefaultSelect());

                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = transactionType.Id.ToString(),
                            Label = transactionType.Text
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

        private string GeFollowupkStatusForFilters()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource() { Value = "-1", Label = "الكل" });
                dataSource.Add(new AutoCompleteDataSource() { Value = ((int)FollowupStatus.New).ToString(), Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.New") });
                dataSource.Add(new AutoCompleteDataSource() { Value = ((int)FollowupStatus.UnderFollowup).ToString(), Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.UnderProcessing") });
                dataSource.Add(new AutoCompleteDataSource() { Value = ((int)FollowupStatus.Completed).ToString(), Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Completed") });
                dataSource.Add(new AutoCompleteDataSource() { Value = ((int)FollowupStatus.Delayed).ToString(), Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Delayed") });
                dataSource.Add(new AutoCompleteDataSource() { Value = ((int)FollowupStatus.Cancled).ToString(), Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpStatus.Cancled") });

                return JsonConvert.SerializeObject(dataSource);
            }

            catch (Exception)
            {
                throw;
            }
        }
        private string GetTransactionCategoriesWithSentForFilters()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    //if (lookupVMs.Count == 6)
                    //{
                    //    lookupVMs.RemoveAt(5);
                    //    lookupVMs.RemoveAt(4);
                    //}
                    dataSource.Add(UIHelper.GetDefaultSelect());

                    foreach (LookupVM transactionType in lookupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = transactionType.Id.ToString(),
                            Label = transactionType.Text
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
        public string GetLookupItemsByType(LookupCategory lookupCategory)
        {
            try
            {
                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
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

        private GetResult<List<TransactionReportResultDTO>> GetTransactionReportResult(TransactionReportVM transactionReportVM, int? page, int level = 4)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();
            //Basic
            transactionReportVM.To = transactionReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);
            searchCriteriaTransactionReportDTO.Number = transactionReportVM.Number;
            searchCriteriaTransactionReportDTO.TransactionCategory = transactionReportVM.TransactionCategory;
            searchCriteriaTransactionReportDTO.Subject = transactionReportVM.Subject;
            searchCriteriaTransactionReportDTO.From = transactionReportVM.From;
            searchCriteriaTransactionReportDTO.To = transactionReportVM.To;
            //Common
            searchCriteriaTransactionReportDTO.TransactionTypeId = transactionReportVM.CommonVM.TransactionTypeId;
            searchCriteriaTransactionReportDTO.IsAppointment = transactionReportVM.CommonVM.IsAppointment;
            searchCriteriaTransactionReportDTO.AppointmentDate = transactionReportVM.CommonVM.AppointmentDate;
            searchCriteriaTransactionReportDTO.PriorityLevelId = transactionReportVM.CommonVM.PriorityLevelId;
            searchCriteriaTransactionReportDTO.ConfidentialityLevelId = transactionReportVM.CommonVM.ConfidentialityLevelId;
            searchCriteriaTransactionReportDTO.LetterTypeId = transactionReportVM.CommonVM.LetterTypeId;
            searchCriteriaTransactionReportDTO.Remarks = transactionReportVM.CommonVM.Remarks;
            searchCriteriaTransactionReportDTO.DeliveryMethodId = transactionReportVM.CommonVM.ReceiveId;
            searchCriteriaTransactionReportDTO.TransactionStatusId = transactionReportVM.CommonVM.TransactionStatusId;
            //Names
            searchCriteriaTransactionReportDTO.FullName = transactionReportVM.NamesVM.FullName;
            searchCriteriaTransactionReportDTO.CivilID = transactionReportVM.NamesVM.CivilID;
            searchCriteriaTransactionReportDTO.MobileNumber = transactionReportVM.NamesVM.MobileNumber;
            //Assignment
            searchCriteriaTransactionReportDTO.FromOrgUnitId = transactionReportVM.SearchAssignmentVM.FromOrgUnitId;
            searchCriteriaTransactionReportDTO.ToOrgUnitId = transactionReportVM.SearchAssignmentVM.ToOrgUnitId;
            searchCriteriaTransactionReportDTO.FromEmployeeId = transactionReportVM.SearchAssignmentVM.FromEmployeeId;
            searchCriteriaTransactionReportDTO.ToEmployeeId = transactionReportVM.SearchAssignmentVM.ToEmployeeId;
            //Additional Fields
            searchCriteriaTransactionReportDTO.IsForIndividual = false;
            switch ((TransactionCategory)transactionReportVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
            {
                case TransactionCategory.Inbound:
                    if ((EntitiesType)transactionReportVM.AdditionalFieldsInboundVM.EntitiesTypeId.LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName) == EntitiesType.Entities)
                    {
                        searchCriteriaTransactionReportDTO.IsForIndividual = false;
                        searchCriteriaTransactionReportDTO.InboundDocumentNumber = transactionReportVM.AdditionalFieldsInboundVM.InboundDocumentNumber;
                        searchCriteriaTransactionReportDTO.InboundDateH = transactionReportVM.AdditionalFieldsInboundVM.InboundDateH;
                        searchCriteriaTransactionReportDTO.DestinationId = transactionReportVM.AdditionalFieldsInboundVM.ExternalPartiesId;
                    }
                    else if ((EntitiesType)transactionReportVM.AdditionalFieldsInboundVM.EntitiesTypeId.LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName) == EntitiesType.Individual)
                    {
                        searchCriteriaTransactionReportDTO.IsForIndividual = true;
                    }
                    break;
                case TransactionCategory.ExternalOutbound:
                case TransactionCategory.DraftOutbound:
                    searchCriteriaTransactionReportDTO.InboundDateH = transactionReportVM.AdditionalFieldsOutboundVM.OutboundDateH;
                    searchCriteriaTransactionReportDTO.DestinationId = transactionReportVM.AdditionalFieldsOutboundVM.ExternalPartiesId;
                    break;
            }
            //Grid Configration

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = transactionReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = transactionReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = SessionInfo.OrgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<TransactionReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<TransactionReportResultDTO>>>
                               .PostRequest("api/Report/TransactionReportSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<TransactionReportResultDTO>> GetSecretaryTransactionReportResult(TransactionReportVM transactionReportVM, int? page, int level = 4, int orgUnitId = 0)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();
            //Basic
            transactionReportVM.To = transactionReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);
            searchCriteriaTransactionReportDTO.Number = transactionReportVM.Number;
            searchCriteriaTransactionReportDTO.TransactionCategory = transactionReportVM.TransactionCategory;
            searchCriteriaTransactionReportDTO.Subject = transactionReportVM.Subject;
            searchCriteriaTransactionReportDTO.From = transactionReportVM.From;
            searchCriteriaTransactionReportDTO.To = transactionReportVM.To;
            //Common
            searchCriteriaTransactionReportDTO.TransactionTypeId = transactionReportVM.CommonVM.TransactionTypeId;
            searchCriteriaTransactionReportDTO.IsAppointment = transactionReportVM.CommonVM.IsAppointment;
            searchCriteriaTransactionReportDTO.AppointmentDate = transactionReportVM.CommonVM.AppointmentDate;
            searchCriteriaTransactionReportDTO.PriorityLevelId = transactionReportVM.CommonVM.PriorityLevelId;
            searchCriteriaTransactionReportDTO.ConfidentialityLevelId = transactionReportVM.CommonVM.ConfidentialityLevelId;
            searchCriteriaTransactionReportDTO.LetterTypeId = transactionReportVM.CommonVM.LetterTypeId;
            searchCriteriaTransactionReportDTO.Remarks = transactionReportVM.CommonVM.Remarks;
            searchCriteriaTransactionReportDTO.DeliveryMethodId = transactionReportVM.CommonVM.ReceiveId;
            searchCriteriaTransactionReportDTO.TransactionStatusId = transactionReportVM.CommonVM.TransactionStatusId;
            //Names
            searchCriteriaTransactionReportDTO.FullName = transactionReportVM.NamesVM.FullName;
            searchCriteriaTransactionReportDTO.CivilID = transactionReportVM.NamesVM.CivilID;
            searchCriteriaTransactionReportDTO.MobileNumber = transactionReportVM.NamesVM.MobileNumber;
            //Assignment
            searchCriteriaTransactionReportDTO.FromOrgUnitId = transactionReportVM.SearchAssignmentVM.FromOrgUnitId;
            searchCriteriaTransactionReportDTO.ToOrgUnitId = transactionReportVM.SearchAssignmentVM.ToOrgUnitId;
            searchCriteriaTransactionReportDTO.FromEmployeeId = transactionReportVM.SearchAssignmentVM.FromEmployeeId;
            searchCriteriaTransactionReportDTO.ToEmployeeId = transactionReportVM.SearchAssignmentVM.ToEmployeeId;
            //Additional Fields
            searchCriteriaTransactionReportDTO.IsForIndividual = false;
            switch ((TransactionCategory)transactionReportVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
            {
                case TransactionCategory.Inbound:
                    if ((EntitiesType)transactionReportVM.AdditionalFieldsInboundVM.EntitiesTypeId.LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName) == EntitiesType.Entities)
                    {
                        searchCriteriaTransactionReportDTO.IsForIndividual = false;
                        searchCriteriaTransactionReportDTO.InboundDocumentNumber = transactionReportVM.AdditionalFieldsInboundVM.InboundDocumentNumber;
                        searchCriteriaTransactionReportDTO.InboundDateH = transactionReportVM.AdditionalFieldsInboundVM.InboundDateH;
                        searchCriteriaTransactionReportDTO.DestinationId = transactionReportVM.AdditionalFieldsInboundVM.ExternalPartiesId;
                    }
                    else if ((EntitiesType)transactionReportVM.AdditionalFieldsInboundVM.EntitiesTypeId.LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName) == EntitiesType.Individual)
                    {
                        searchCriteriaTransactionReportDTO.IsForIndividual = true;
                    }
                    break;
                case TransactionCategory.ExternalOutbound:
                case TransactionCategory.DraftOutbound:
                    searchCriteriaTransactionReportDTO.InboundDateH = transactionReportVM.AdditionalFieldsOutboundVM.OutboundDateH;
                    searchCriteriaTransactionReportDTO.DestinationId = transactionReportVM.AdditionalFieldsOutboundVM.ExternalPartiesId;
                    break;
            }
            //Grid Configration

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = transactionReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = transactionReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = orgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<TransactionReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<TransactionReportResultDTO>>>
                               .PostRequest("api/Report/SecretaryTransactionReportSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<SentTransactionReportResultDTO>> GetSentTransactionReportResult(SentTransactionReportVM transactionReportVM, int? page, int level = 4)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();

            transactionReportVM.To = transactionReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);

            searchCriteriaTransactionReportDTO.TransactionCategory = transactionReportVM.TransactionCategory;
            searchCriteriaTransactionReportDTO.From = transactionReportVM.From;
            searchCriteriaTransactionReportDTO.To = transactionReportVM.To;
            searchCriteriaTransactionReportDTO.FromOrgUnitId = transactionReportVM.FromOrgUnitId.HasValue ? transactionReportVM.FromOrgUnitId.Value : 0;
            searchCriteriaTransactionReportDTO.ToOrgUnitId = transactionReportVM.ToOrgUnitId.HasValue ? transactionReportVM.ToOrgUnitId.Value : 0;


            //Grid Configration

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = transactionReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = transactionReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = SessionInfo.OrgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<SentTransactionReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<SentTransactionReportResultDTO>>>
                               .PostRequest("api/Report/SentTransactionReportSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<SentTransactionReportResultDTO>> GetSentTransactionReporStatustResult(SentTransactionReportVM transactionReportVM, int? page, int level = 4)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();

            transactionReportVM.To = transactionReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);

            searchCriteriaTransactionReportDTO.TransactionCategory = transactionReportVM.TransactionCategory;
            searchCriteriaTransactionReportDTO.From = transactionReportVM.From;
            searchCriteriaTransactionReportDTO.To = transactionReportVM.To;
            searchCriteriaTransactionReportDTO.FromOrgUnitId = transactionReportVM.FromOrgUnitId.HasValue ? transactionReportVM.FromOrgUnitId.Value : 0;
            searchCriteriaTransactionReportDTO.ToOrgUnitId = transactionReportVM.ToOrgUnitId.HasValue ? transactionReportVM.ToOrgUnitId.Value : 0;


            //Grid Configration

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = transactionReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = transactionReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = SessionInfo.OrgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<SentTransactionReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<SentTransactionReportResultDTO>>>
                               .PostRequest("api/Report/SentTransactionReportStatusSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<FollowupReportResultDTO>> GetFollowupReportResult(FollowupReportVM followupReportVM, int? page, int level = 4)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();
            //Basic
            followupReportVM.To = followupReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);

            searchCriteriaTransactionReportDTO.TransactionCategory = followupReportVM.TransactionCategory;
            searchCriteriaTransactionReportDTO.From = followupReportVM.From;
            searchCriteriaTransactionReportDTO.To = followupReportVM.To;
            searchCriteriaTransactionReportDTO.TransactionStatusId = followupReportVM.FollowUpStatusId;


            //Grid Configration

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = followupReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = followupReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = SessionInfo.OrgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<FollowupReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<FollowupReportResultDTO>>>
                               .PostRequest("api/Report/FollowupReportSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<TaskReportResultDTO>> GetTasksReportResult(TaskReportVM transactionReportVM, int? page, int level = 4)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();
            transactionReportVM.To = transactionReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59);

            searchCriteriaTransactionReportDTO.TransactionCategory = transactionReportVM.TransactionCategory;
            searchCriteriaTransactionReportDTO.From = transactionReportVM.From;
            searchCriteriaTransactionReportDTO.To = transactionReportVM.To;

            searchCriteriaTransactionReportDTO.TransactionStatusId = transactionReportVM.CommonVM.TransactionStatusId;

            //Grid Configration

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = transactionReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = transactionReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = SessionInfo.OrgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<TaskReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<TaskReportResultDTO>>>
                               .PostRequest("api/Report/TasksReportSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<TransactionDeliveryReportDTO>> GetTransactionReportResult(TransactionDeliveryReportVM transactionReportVM, int? page, int level = 4)
        {
            #region Search Criteria
            var searchCriteriaTransactionReportDTO = new SearchCriteriaTransactionReportDTO();
            //Basic
            

            searchCriteriaTransactionReportDTO.Number = Convert.ToInt32(transactionReportVM.TransactionNumber);
            searchCriteriaTransactionReportDTO.TransactionCategory = Convert.ToInt32(transactionReportVM.TransactionCategoryName);
            searchCriteriaTransactionReportDTO.Subject = transactionReportVM.Subject;
            //Common 
            searchCriteriaTransactionReportDTO.From = transactionReportVM.Date; 
            searchCriteriaTransactionReportDTO.To = transactionReportVM.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
            searchCriteriaTransactionReportDTO.TransactionTypeId = Convert.ToInt32(transactionReportVM.TransactionCategoryId);
            searchCriteriaTransactionReportDTO.PriorityLevelId = Convert.ToInt32(transactionReportVM.Priority);
            searchCriteriaTransactionReportDTO.ConfidentialityLevelId = Convert.ToInt32(transactionReportVM.Confidentiality);
            //Additional Fields
            searchCriteriaTransactionReportDTO.IsForIndividual = false;
            switch ((TransactionCategory)Convert.ToInt32(transactionReportVM.TransactionCategoryName).LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
            {
                case TransactionCategory.Inbound:
                    if ((EntitiesType)transactionReportVM.AdditionalFieldsInboundVM.EntitiesTypeId.LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName) == EntitiesType.Entities)
                    {
                        searchCriteriaTransactionReportDTO.IsForIndividual = false;
                        searchCriteriaTransactionReportDTO.InboundDocumentNumber = transactionReportVM.AdditionalFieldsInboundVM.InboundDocumentNumber;
                        searchCriteriaTransactionReportDTO.InboundDateH = transactionReportVM.AdditionalFieldsInboundVM.InboundDateH;
                        searchCriteriaTransactionReportDTO.DestinationId = transactionReportVM.AdditionalFieldsInboundVM.ExternalPartiesId;
                    }
                    else if ((EntitiesType)transactionReportVM.AdditionalFieldsInboundVM.EntitiesTypeId.LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName).LookupInternalID(LookupCategory.Entity, SessionInfo.CultureShortName) == EntitiesType.Individual)
                    {
                        searchCriteriaTransactionReportDTO.IsForIndividual = true;
                    }
                    break;
                case TransactionCategory.ExternalOutbound:
                case TransactionCategory.DraftOutbound:
                    searchCriteriaTransactionReportDTO.InboundDateH = transactionReportVM.AdditionalFieldsOutboundVM.OutboundDateH;
                    searchCriteriaTransactionReportDTO.DestinationId = transactionReportVM.AdditionalFieldsOutboundVM.ExternalPartiesId;
                    break;
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            //Grid Configration
            searchCriteriaTransactionReportDTO.PageIndex = page ?? 1;
            searchCriteriaTransactionReportDTO.PageSize = pageSize;
            searchCriteriaTransactionReportDTO.CultureName = SessionInfo.CultureShortName;
            //Report Configration
            searchCriteriaTransactionReportDTO.IsPrint = transactionReportVM.IsPrint;
            searchCriteriaTransactionReportDTO.TotalCount = transactionReportVM.TotalCount;
            searchCriteriaTransactionReportDTO.EntityId = SessionInfo.OrgUnitId;
            searchCriteriaTransactionReportDTO.UserId = SessionInfo.CurrentUser.Id;
            searchCriteriaTransactionReportDTO.Level = level;
            #endregion

            GetResult<List<TransactionDeliveryReportDTO>> getResult = HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>
                               .PostRequest("api/Report/TransactionReportSearch", searchCriteriaTransactionReportDTO).Result;

            return getResult;
        }

        private GetResult<List<PerformanceMeasurementReportResultDTO>> GetPerformanceMeasurementResult(PerformanceMeasurementReportVM performanceMeasurementReportVM, int? page)
        {
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int GridSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
            #region Search Criteria
            int PageSize = (RepresentationReportType)performanceMeasurementReportVM.RepresentationTypeId.LookupInternalID(LookupCategory.RepresentationType, SessionInfo.CultureShortName) == RepresentationReportType.Barchart ? 1 : GridSize;
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsAllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.ReportsParentDepartment))
            {
                level = 2;
            }

            var searchCriteriaPerformanceMeasurementDTO = new SearchCriteriaPerformanceMeasurementDTO
            {
                //Basic
                ReportType = performanceMeasurementReportVM.ReportTypeId.LookupInternalID(LookupCategory.ReportType, SessionInfo.CultureShortName),
                From = performanceMeasurementReportVM.From,
                To = performanceMeasurementReportVM.To.AddHours(23).AddMinutes(59).AddSeconds(59),
                OrgUnitId = performanceMeasurementReportVM.OrgUnitId,
                Level = level,
                //Common
                LetterTypeId = performanceMeasurementReportVM.CommonVM.LetterTypeId,
                IsAppointment = performanceMeasurementReportVM.CommonVM.IsAppointment,
                AppointmentDate = performanceMeasurementReportVM.CommonVM.AppointmentDate,
                PriorityLevelId = performanceMeasurementReportVM.CommonVM.PriorityLevelId,
                ConfidentialityLevelId = performanceMeasurementReportVM.CommonVM.ConfidentialityLevelId,
                TransactionTypeId = performanceMeasurementReportVM.CommonVM.TransactionTypeId,
                Remarks = performanceMeasurementReportVM.CommonVM.Remarks,
                DeliveryMethodId = performanceMeasurementReportVM.CommonVM.ReceiveId,
                //Employee
                EmployeeId = level == 1 ? SessionInfo.CurrentUser.Id : performanceMeasurementReportVM.EmployeeVM.EmployeeId,

                //Grid Configration
                PageIndex = page ?? 1,
                PageSize = PageSize,
                CultureName = SessionInfo.CultureShortName,
                //Report Configration
                IsPrint = performanceMeasurementReportVM.IsPrint,
                TotalCount = performanceMeasurementReportVM.TotalCount
            };
            #endregion

            GetResult<List<PerformanceMeasurementReportResultDTO>> getResult = HttpClientWrapper<GetResult<List<PerformanceMeasurementReportResultDTO>>>
                               .PostRequest("api/Report/PerformanceMeasurementReportSearch", searchCriteriaPerformanceMeasurementDTO).Result;

            return getResult;
        }
        #endregion

        #endregion
        public void AddTransactionDeliveryReport()
        {

        }

        public List<ReporterVM> GetReporterVMs()
        {
            var reporterDTOs = HttpClientWrapper<GetResult<List<ReporterDTO>>>.GetItemRequest(string.Format("api/Common/GetReporter?cultureName={0}&orgUnitId={1}",
                SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
            var result = ReporterMapper.Map(reporterDTOs.Result);
            return result;
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
        private GetResult<List<UserGroupDTO>> GetUsersGroupsResult(string GroupId = null)
        {
            GetResult<List<UserGroupDTO>> userGroupDTOList = HttpClientWrapper<GetResult<List<UserGroupDTO>>>
                               .GetItemRequest(string.Format("api/Admin/GetUsersWithGroups?GroupId={0}", GroupId)).Result;
            return userGroupDTOList;
        }

        private GetResult<List<UserProfileDTO>> GetUsersResult()
        {
            GetResult<List<UserProfileDTO>> usersDTOList = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                               .GetItemRequest(string.Format("api/Admin/GetUsers")).Result;
            return usersDTOList;
        }
        public ActionResult UsersReportExportToPdf()
        {
            try
            {
                int? page = 1;
                Admin.Models.UserProfileVM usersVM = new Admin.Models.UserProfileVM();
                string message = string.Empty;

                GetResult<List<UserProfileDTO>> getResult = GetUsersResult();
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                List<Admin.Models.UserProfileVM> usersVMList = Admin.Mappers.UserProfileMapper.Map(getResult.Result);

                var grid = (AjaxGrid<Admin.Models.UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(usersVMList, 1, getResult.RowsCount.Value,
                page.HasValue, UIHelper.PageSize);

                TransactionReportResult transactionReportResult = new TransactionReportResult();

                transactionReportResult.UsersDTOtGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = null;
                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UsersReport", transactionReportResult);

                //return Json(new { Html, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                //Convert Html to Pdf    
                // System.IO.File.WriteAllText(@"C:\Builds\a.htm", Html);
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "Users Report" + handle + ".pdf";
                //System.IO.File.WriteAllBytes(@"C:\Builds\" + fileResult.FileDownloadName, pdf);
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult UsersWithGroupsExportToPdf()
        {
            try
            {
                int? page = 1;
                LookupModels.UserGroupVM userGroupVM = new LookupModels.UserGroupVM();
                string message = string.Empty;
                userGroupVM.IsPrint = true;

                GetResult<List<UserGroupDTO>> getResult = GetUsersGroupsResult();
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                List<LookupModels.UserGroupVM> userGroupVMList = UserGroupMapper.Map(getResult.Result);

                var grid = (AjaxGrid<LookupModels.UserGroupVM>)new AjaxGridFactory().CreateAjaxGrid(userGroupVMList, 1, getResult.RowsCount.Value,
                page.HasValue, UIHelper.PageSize);

                TransactionReportResult transactionReportResult = new TransactionReportResult();

                transactionReportResult.UserGroupDTOtGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = null;
                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "UsersWithGroupsReport", transactionReportResult);

                //return Json(new { Html, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                //Convert Html to Pdf    
                // System.IO.File.WriteAllText(@"C:\Builds\a.htm", Html);
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "Users with roles" + handle + ".pdf";
                //System.IO.File.WriteAllBytes(@"C:\Builds\" + fileResult.FileDownloadName, pdf);
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public ActionResult ExportUsersWithGroupsReportToExcel()
        {
            try
            {
                LookupModels.UserGroupVM userGroupVM = new LookupModels.UserGroupVM();
                string message = string.Empty;
                userGroupVM.IsPrint = true;

                GetResult<List<UserGroupDTO>> getResult = GetUsersGroupsResult();
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                
                var transactions = getResult.Result;
                string handle = Guid.NewGuid().ToString();
                var excel = ConvertUsersWithGroupsReportToExcel(transactions);

                TempData[handle] = excel;

                FileResult fileResult = new FileContentResult(excel, "aapplication/ms-excel");
                fileResult.FileDownloadName = "TransactionReport.xls";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static byte[] ConvertUsersWithGroupsReportToExcel(List<UserGroupDTO> list)
        {
            DataTable dt = new DataTable();
            List<UsersWithGroupsGridColumn> collsIndexList = new List<UsersWithGroupsGridColumn>();

           
                dt.Columns.Add(DbRes.TResource("اسم المستخدم"));
                collsIndexList.Add(UsersWithGroupsGridColumn.Name);
         
                dt.Columns.Add(DbRes.TResource("الوحدة"));
                collsIndexList.Add(UsersWithGroupsGridColumn.OrgUnitName);
         
                dt.Columns.Add(DbRes.TResource("المدير"));
                collsIndexList.Add(UsersWithGroupsGridColumn.AdminUserName);
          
                dt.Columns.Add(DbRes.TResource("الدور"));
                collsIndexList.Add(UsersWithGroupsGridColumn.GroupName);
           

            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
      
                    dr[collsIndexList.IndexOf(UsersWithGroupsGridColumn.Name)] = item.Name;

                    dr[collsIndexList.IndexOf(UsersWithGroupsGridColumn.OrgUnitName)] = item.OrgUnitNames.FirstOrDefault();
               
                    dr[collsIndexList.IndexOf(UsersWithGroupsGridColumn.AdminUserName)] = item.AdminUserName;

                    dr[collsIndexList.IndexOf(UsersWithGroupsGridColumn.GroupName)] = item.GroupName;
                
                dt.Rows.Add(dr);

            }

            var memoryStream = new MemoryStream();
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
                worksheet.Cells["A1"].LoadFromDataTable(dt, true, TableStyles.None);
                worksheet.Cells["A1:AN1"].Style.Font.Bold = true;
                worksheet.DefaultRowHeight = 18;
                worksheet.DefaultColWidth = 20;
                return excelPackage.GetAsByteArray();
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Reports.SaveReports)]
        public ActionResult ExportSecretaryToPdf(TransactionReportVM transactionReportVM, int? page)
        {
            try
            {
                string message = string.Empty;
                transactionReportVM.IsPrint = true;
                GetResult<List<TransactionReportResultDTO>> getResult = GetSecretaryTransactionReportResult(transactionReportVM, page, orgUnitId: transactionReportVM.OrgUnitId);
                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                foreach (TransactionReportResultDTO transactionReportResultDTO in getResult.Result)
                {
                    if (int.Parse(transactionReportResultDTO.DelayedDaysCount) > 0)
                    {
                        transactionReportResultDTO.TransactionStatusText = DbRes.TResource("User.Transaction.Report.Late");
                    }
                    else
                    {
                        transactionReportResultDTO.DelayedDaysCount = "---";
                    }
                }

                TransactionReportResult transactionReportResult = new TransactionReportResult();
                transactionReportResult.TransactionBasicResultVM.Number = transactionReportVM.Number;
                transactionReportResult.TransactionBasicResultVM.CreateOn = DateTime.Now.AddDays(-1);
                var DTFormat = new CultureInfo(SessionInfo.CultureShortName, false).DateTimeFormat;
                transactionReportResult.TransactionBasicResultVM.DateFrom = $"{DateHelper.DateCalendar(transactionReportVM.From.AddDays(-1), SessionInfo.CultureShortName)} ";
                transactionReportResult.TransactionBasicResultVM.DateTo = $"{DateHelper.DateCalendar(transactionReportVM.To.AddDays(-1), SessionInfo.CultureShortName)}";
                transactionReportResult.TransactionBasicResultVM.CreatedBy = SessionInfo.CurrentUser.Name;

                transactionReportResult.TransactionBasicResultVM.TotalCount = transactionReportVM.IsPrint.HasValue && transactionReportVM.IsPrint.Value ? transactionReportVM.TotalCount : getResult.RowsCount.Value;

                GetResult<IList<LookupVM>> transactionCategories = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = transactionCategories.Result;
                if (lookupVMs != null)
                {
                    var value = lookupVMs.FirstOrDefault(a => a.Id == transactionReportVM.TransactionCategory);
                    transactionReportResult.TransactionBasicResultVM.TransactionType = DbRes.TResource("User.TransactionType.All");
                    if (value != null)
                    {
                        transactionReportResult.TransactionBasicResultVM.TransactionType = value.Text;
                    }
                }
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int pageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                var grid = (AjaxGrid<TransactionGridResultVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionReportMapper.Map(getResult.Result), 1, getResult.RowsCount.Value,
                page.HasValue, pageSize);
                foreach (TransactionReportResultDTO transactionReportResultDTO in getResult.Result)
                {
                    transactionReportResultDTO.DelayedDaysCount = Int32.Parse((DateTime.Now.Date - transactionReportResultDTO.Date.Date).Days.ToString()).ToString();

                }
                transactionReportResult.TransactionPrintResultVMs = TransactionReportMapper.Map(getResult.Result);
                transactionReportResult.TransactionGridResultVMs = grid;
                ViewData["ColumnsToGrid"] = transactionReportVM.ColumnsToGrid;

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "PrintSecretaryTransactionReport", transactionReportResult);
                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "TransactionReport.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}