using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.Search.TransactionCertificate;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Mappers;
using CustomGridMvc = MCS.GridMvc.Ajax.GridExtensions;
using framework = MCS.Framework.AuditTrail;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.IO;
using DotnetDaddy.DocumentViewer.License;
using MCS.UI.Helpers;
using MCS.UI.Common;

namespace MCS.UI.Areas.User.Controllers
{
    public class TransactionLogController : BaseController
    {

        // GET: User/TransactionLog
        [HttpGet]
        public ActionResult Index(string transactionId, TransactionCategory transactionCategory)
        {
            try
            {
                // List<AuditVM> auditVMs = new List<AuditVM>();
                // List<TransactionLogDetailInfoVM> transactionLogInfoVMs = new List<TransactionLogDetailInfoVM>();
                Session["IsEditMode"] = false;
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
                Session["TransactionId"] = trxId;
                int itemsCount = 0;
                switch (transactionCategory)
                {
                    case TransactionCategory.Inbound:
                        GetResult<InboundCertificateDTO> inboundCertificateDTO =
                               HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                        InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                        inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                        //  auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, AuditEntityName.MainTransaction, out itemsCount); // just for testing 
                        //  auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);
                        //  transactionLogInfoVMs = GetTransactionLogInfo(transactionId);

                        //  CustomGridMvc.IAjaxGrid inboundAuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false, UIHelper.PageSize);
                        //  ViewData["TransactionAudits"] = inboundAuditGrid;
                        ViewData["AuditType"] = BuildAuditTypeDataSource();
                        ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
                        //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                        // CustomGridMvc.IAjaxGrid inboundTransactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                        //    ViewData["TransactionLogs"] = inboundTransactionLogInfoGrids;
                        // return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundCertificatePartial.cshtml", inboundCertificateVM);
                        return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundTransactionLogPartial.cshtml", inboundCertificateVM);

                    case TransactionCategory.InternalOutbound:
                        GetResult<InboundCertificateDTO> InternalOutboundCertificateDTO =
                               HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                        InboundCertificateVM InternalOutboundCertificateVM = InboundCertificateMapper.Map(InternalOutboundCertificateDTO.Result);
                        InternalOutboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                        // auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, AuditEntityName.MainTransaction, out itemsCount); // just for testing 
                        // auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);
                        //transactionLogInfoVMs = GetTransactionLogInfo(transactionId);
                        ViewData["AuditType"] = BuildAuditTypeDataSource();
                        ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
                        //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                        //  CustomGridMvc.IAjaxGrid InternalOutboundAuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                        //   ViewData["TransactionAudits"] = InternalOutboundAuditGrid;
                        //   CustomGridMvc.IAjaxGrid InternalOutboundTransactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                        //   ViewData["TransactionLogs"] = InternalOutboundTransactionLogInfoGrids;

                        return View("~/Areas/User/Views/Shared/TransactionCertificate/_InternalOutboundTransactionLogPartial.cshtml", InternalOutboundCertificateVM);


                    case TransactionCategory.ExternalOutbound:
                    case TransactionCategory.DraftOutbound:

                        GetResult<OutboundCertificateDTO> outboundExternalEditDTO =
                           HttpClientWrapper<GetResult<OutboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetOutboundBasicInfo?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                        OutboundCertificateVM outboundCertificateVM = OutboundCertificateMapper.Map(outboundExternalEditDTO.Result);
                        outboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                        // auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, AuditEntityName.MainTransaction, out itemsCount); // just for testing 
                        //auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);
                        //transactionLogInfoVMs = GetTransactionLogInfo(transactionId);

                        //  CustomGridMvc.IAjaxGrid outboundAuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                        // ViewData["TransactionAudits"] = outboundAuditGrid;
                        ViewData["AuditType"] = BuildAuditTypeDataSource();
                        ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
                        //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                        // CustomGridMvc.IAjaxGrid outboundTransactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                        // ViewData["TransactionLogs"] = outboundTransactionLogInfoGrids;

                        return View("~/Areas/User/Views/Shared/TransactionCertificate/_OutboundTransactionLogPartial.cshtml", outboundCertificateVM);

                    default:
                        return Json(new { message = MessageType.Error });
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult GetInboundBasicInfo(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                        HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);



                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                //  List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, AuditEntityName.MainTransaction, out itemsCount); // just for testing 
                // auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);
                //  List<TransactionLogDetailInfoVM> transactionLogInfoVMs = GetTransactionLogInfo(transactionId);

                // CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                // ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                // CustomGridMvc.IAjaxGrid transactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                // ViewData["TransactionLogs"] = transactionLogInfoGrids;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_InboundCertificateBasicInfoPartial.cshtml", inboundCertificateVM);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult GetInternalOutboundBasicInfo(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                //    List<AuditVM> auditVMs = new List<AuditVM>();
                //     List<TransactionLogDetailInfoVM> transactionLogInfoVMs = new List<TransactionLogDetailInfoVM>();

                GetResult<InboundCertificateDTO> InternalOutboundCertificateDTO =
                               HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                InboundCertificateVM InternalOutboundCertificateVM = InboundCertificateMapper.Map(InternalOutboundCertificateDTO.Result);
                InternalOutboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                // auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, AuditEntityName.MainTransaction, out itemsCount); // just for testing 
                //  auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);
                //transactionLogInfoVMs = GetTransactionLogInfo(transactionId);

                // CustomGridMvc.IAjaxGrid InternalOutboundAuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                // ViewData["TransactionAudits"] = InternalOutboundAuditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                // CustomGridMvc.IAjaxGrid InternalOutboundTransactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                // ViewData["TransactionLogs"] = InternalOutboundTransactionLogInfoGrids;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_InternalOutboundCertificateBasicInfoPartial.cshtml", InternalOutboundCertificateVM);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        public ActionResult GetOutboundBasicInfo(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                List<AuditVM> auditVMs = new List<AuditVM>();
                List<TransactionLogDetailInfoVM> transactionLogInfoVMs = new List<TransactionLogDetailInfoVM>();

                GetResult<OutboundCertificateDTO> outboundExternalEditDTO =
                      HttpClientWrapper<GetResult<OutboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetOutboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                OutboundCertificateVM outboundCertificateVM = OutboundCertificateMapper.Map(outboundExternalEditDTO.Result);
                outboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                // auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, AuditEntityName.MainTransaction, out itemsCount); // just for testing 
                // auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);
                // transactionLogInfoVMs = GetTransactionLogInfo(transactionId);

                //  CustomGridMvc.IAjaxGrid outboundAuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                // ViewData["TransactionAudits"] = outboundAuditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                //      CustomGridMvc.IAjaxGrid outboundTransactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                // ViewData["TransactionLogs"] = outboundTransactionLogInfoGrids;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_OutboundCertificateBasicInfoPartial.cshtml", outboundCertificateVM);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult GetTransactionAssignmentHistories(int transactionId, int Ascending = 0)
        {
            try
            {

                GetResult<TransactionAssignmentDTO> transactionAssignmentResult =
                      HttpClientWrapper<GetResult<TransactionAssignmentDTO>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignment?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                TransactionAssignmentVM transactionAssignmentVM = TransactionAssignmentMapper.Map(transactionAssignmentResult.Result);
                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                      HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignmentHistories?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result);
                for (int i = 0; i < transactionAssignmentVMs.Count; i++)
                {
                    transactionAssignmentVMs[i].DateH = transactionAssignmentVMs[i].DateH.Replace("PM", SessionInfo.CultureShortName == "ar" ? "م" : "PM").Replace("AM", SessionInfo.CultureShortName == "ar" ? "ص" : "AM");
                    transactionAssignmentVMs[i].Sequence = i + 1;


                    TimeSpan timeSpan = new TimeSpan();
                    DateTime dateTime1 = transactionAssignmentVMs[i].Date;
                    DateTime dateTime2 = DateTime.Now;

                    if (i + 1 < transactionAssignmentVMs.Count)
                    {
                        dateTime2 = transactionAssignmentVMs[i + 1].Date;
                    }

                    timeSpan = dateTime2 - dateTime1;

                    if (timeSpan.Days <= 0 && timeSpan.Hours < 7)
                    {
                        transactionAssignmentVMs[i].Duration = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
                    }
                    else if (timeSpan.Days <= 0 && (timeSpan.Hours >= 7 && timeSpan.Hours < 24))
                    {
                        transactionAssignmentVMs[i].Duration = "1" + " يوم/ايام ";
                    }
                    else if (timeSpan.Days > 0)
                    {
                        int daysCount = 0;

                        while (dateTime1 < dateTime2)
                        {
                            if (dateTime1.DayOfWeek != DayOfWeek.Friday && dateTime1.DayOfWeek != DayOfWeek.Saturday)
                            {
                                daysCount++;
                            }

                            dateTime1 = dateTime1.AddDays(1);
                        }

                        transactionAssignmentVMs[i].Duration = daysCount.ToString() + " يوم/ايام ";
                    }
                }
                CustomGridMvc.IAjaxGrid Assignments;
                if (Ascending == 0)
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.ToList(), 1, transactionAssignmentVMs.Count(), false);
                }
                else
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.OrderByDescending(x => x.Date).ToList(), 1, transactionAssignmentVMs.Count(), false);
                }


                EditorViewModel editorViewModels = new EditorViewModel();
                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                      .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                editorViewModels.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                ViewData["ConfidentialityName"] = editorViewModels.TransactionBasicInfoVM.ConfidentialityName;
                ViewData["PriorityLevel"] = editorViewModels.TransactionBasicInfoVM.PriorityName;
                ViewData["Subject"] = editorViewModels.TransactionBasicInfoVM.Subject;
                ViewData["TransactionsNumber"] = editorViewModels.TransactionBasicInfoVM.Number;



                ViewData["AssignmentsData"] = Assignments;
                ViewData["TransactionId"] = transactionId;
                Session["TransactionId"] = transactionId;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateAssignmentsPartial.cshtml", transactionAssignmentVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private TransactionAssignmentVM GetAssignment(int transactionId, int Ascending = 0)
        {
            try
            {

                GetResult<TransactionAssignmentDTO> transactionAssignmentResult =
                      HttpClientWrapper<GetResult<TransactionAssignmentDTO>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignment?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                TransactionAssignmentVM transactionAssignmentVM = TransactionAssignmentMapper.Map(transactionAssignmentResult.Result);
                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                      HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignmentHistories?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result);
                for (int i = 0; i < transactionAssignmentVMs.Count; i++)
                {
                    //transactionAssignmentVMs[i].DateH = transactionAssignmentVMs[i].DateH + "   " + transactionAssignmentVMs[i].Date.ToShortTimeString();
                    transactionAssignmentVMs[i].DateH = transactionAssignmentVMs[i].DateH.Replace("PM", "م").Replace("AM", "ص");
                    transactionAssignmentVMs[i].Sequence = i + 1;


                    TimeSpan timeSpan = new TimeSpan();
                    DateTime dateTime1 = transactionAssignmentVMs[i].Date;
                    DateTime dateTime2 = DateTime.Now;

                    if (i + 1 < transactionAssignmentVMs.Count)
                    {
                        dateTime2 = transactionAssignmentVMs[i + 1].Date;
                    }

                    timeSpan = dateTime2 - dateTime1;

                    if (timeSpan.Days <= 0 && timeSpan.Hours < 7)
                    {
                        transactionAssignmentVMs[i].Duration = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
                    }
                    else if (timeSpan.Days <= 0 && (timeSpan.Hours >= 7 && timeSpan.Hours < 24))
                    {
                        transactionAssignmentVMs[i].Duration = "1" + " يوم/ايام ";
                    }
                    else if (timeSpan.Days > 0)
                    {
                        int daysCount = 0;

                        while (dateTime1 < dateTime2)
                        {
                            if (dateTime1.DayOfWeek != DayOfWeek.Friday && dateTime1.DayOfWeek != DayOfWeek.Saturday)
                            {
                                daysCount++;
                            }

                            dateTime1 = dateTime1.AddDays(1);
                        }

                        transactionAssignmentVMs[i].Duration = daysCount.ToString() + " يوم/ايام ";
                    }
                }
                CustomGridMvc.IAjaxGrid Assignments;
                if (Ascending == 0)
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.ToList(), 1, transactionAssignmentVMs.Count(), false);
                }
                else
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.OrderByDescending(x => x.Date).ToList(), 1, transactionAssignmentVMs.Count(), false);
                }
                ViewData["AssignmentsData"] = Assignments;
                ViewData["TransactionId"] = transactionId;
                Session["TransactionId"] = transactionId;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

                return transactionAssignmentVM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetTransactionVipAssignmentHistoriesData(string transactionId, int Ascending = 0)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                GetResult<TransactionAssignmentDTO> transactionAssignmentResult =
                      HttpClientWrapper<GetResult<TransactionAssignmentDTO>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignment?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                TransactionAssignmentVM transactionAssignmentVM = TransactionAssignmentMapper.Map(transactionAssignmentResult.Result);

                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                      HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignmentHistories?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result);
                for (int i = 0; i < transactionAssignmentVMs.Count; i++)
                {
                    transactionAssignmentVMs[i].DateH = transactionAssignmentVMs[i].DateH + "   " + transactionAssignmentVMs[i].Date.ToShortTimeString();
                    transactionAssignmentVMs[i].Sequence = i + 1;


                    TimeSpan timeSpan = new TimeSpan();
                    DateTime dateTime1 = transactionAssignmentVMs[i].Date;
                    DateTime dateTime2 = DateTime.Now;

                    if (i + 1 < transactionAssignmentVMs.Count)
                    {
                        dateTime2 = transactionAssignmentVMs[i + 1].Date;
                    }

                    timeSpan = dateTime2 - dateTime1;

                    if (timeSpan.Days <= 0 && timeSpan.Hours < 7)
                    {
                        transactionAssignmentVMs[i].Duration = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
                    }
                    else if (timeSpan.Days <= 0 && (timeSpan.Hours >= 7 && timeSpan.Hours < 24))
                    {
                        transactionAssignmentVMs[i].Duration = "1" + " يوم/ايام ";
                    }
                    else if (timeSpan.Days > 0)
                    {
                        int daysCount = 0;

                        while (dateTime1 < dateTime2)
                        {
                            if (dateTime1.DayOfWeek != DayOfWeek.Friday && dateTime1.DayOfWeek != DayOfWeek.Saturday)
                            {
                                daysCount++;
                            }

                            dateTime1 = dateTime1.AddDays(1);
                        }

                        transactionAssignmentVMs[i].Duration = daysCount.ToString() + " يوم/ايام ";
                    }
                }
                CustomGridMvc.IAjaxGrid Assignments;
                if (Ascending == 0)
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.ToList(), 1, transactionAssignmentVMs.Count(), false);

                }
                else
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.OrderByDescending(x => x.Date).ToList(), 1, transactionAssignmentVMs.Count(), false);
                }
                ViewData["AssignmentsData"] = Assignments;
                ViewData["TransactionId"] = trxId;
                Session["TransactionId"] = trxId;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/_AssignmentGridPartial.cshtml", ViewData["AssignmentsData"]);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult GetTransactionAssignmentHistoriesData(int transactionId, int Ascending = 0)
        {
            try
            {
                GetResult<TransactionAssignmentDTO> transactionAssignmentResult =
                      HttpClientWrapper<GetResult<TransactionAssignmentDTO>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignment?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                TransactionAssignmentVM transactionAssignmentVM = TransactionAssignmentMapper.Map(transactionAssignmentResult.Result);

                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                      HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignmentHistories?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result);
                for (int i = 0; i < transactionAssignmentVMs.Count; i++)
                {
                    transactionAssignmentVMs[i].DateH = transactionAssignmentVMs[i].DateH + "   " + transactionAssignmentVMs[i].Date.ToShortTimeString();
                    transactionAssignmentVMs[i].Sequence = i + 1;


                    TimeSpan timeSpan = new TimeSpan();
                    DateTime dateTime1 = transactionAssignmentVMs[i].Date;
                    DateTime dateTime2 = DateTime.Now;

                    if (i + 1 < transactionAssignmentVMs.Count)
                    {
                        dateTime2 = transactionAssignmentVMs[i + 1].Date;
                    }

                    timeSpan = dateTime2 - dateTime1;

                    if (timeSpan.Days <= 0 && timeSpan.Hours < 7)
                    {
                        transactionAssignmentVMs[i].Duration = timeSpan.Hours + ":" + timeSpan.Minutes + ":" + timeSpan.Seconds;
                    }
                    else if (timeSpan.Days <= 0 && (timeSpan.Hours >= 7 && timeSpan.Hours < 24))
                    {
                        transactionAssignmentVMs[i].Duration = "1" + " يوم/ايام ";
                    }
                    else if (timeSpan.Days > 0)
                    {
                        int daysCount = 0;

                        while (dateTime1 < dateTime2)
                        {
                            if (dateTime1.DayOfWeek != DayOfWeek.Friday && dateTime1.DayOfWeek != DayOfWeek.Saturday)
                            {
                                daysCount++;
                            }

                            dateTime1 = dateTime1.AddDays(1);
                        }

                        transactionAssignmentVMs[i].Duration = daysCount.ToString() + " يوم/ايام ";
                    }
                }
                CustomGridMvc.IAjaxGrid Assignments;
                if (Ascending == 0)
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.ToList(), 1, transactionAssignmentVMs.Count(), false);

                }
                else
                {
                    Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.OrderByDescending(x => x.Date).ToList(), 1, transactionAssignmentVMs.Count(), false);
                }
                ViewData["AssignmentsData"] = Assignments;
                ViewData["TransactionId"] = transactionId;
                Session["TransactionId"] = transactionId;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/_AssignmentGridPartial.cshtml", ViewData["AssignmentsData"]);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult GetTransactionAssignmentHistoryWithContent(int transactionId)
        {
            try
            {
                GetResult<TransactionAssignmentDTO> transactionAssignmentResult =
                      HttpClientWrapper<GetResult<TransactionAssignmentDTO>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignment?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                TransactionAssignmentVM transactionAssignmentVM = TransactionAssignmentMapper.Map(transactionAssignmentResult.Result);

                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                      HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignmentHistoryWithContent?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result);

                CustomGridMvc.IAjaxGrid Assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count(), false);
                ViewData["AssignmentsData"] = Assignments;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                ViewData["TransactionId"] = transactionId;
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/_AssignmentTransactionPrint.cshtml", transactionAssignmentVM);

            }
            catch (Exception)
            {

                throw;
            }
        }



        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.TransactionCertificate.CopiesInternal)]
        public ActionResult GetTransactionCopiesByTransactionId(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<List<TransactionCopyDTO>> transactionCopyDTOs =
                    HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionCopyVM> transactionCopyVMs = TransactionCopyMapper.Map(transactionCopyDTOs.Result);


                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionCopyVMs, 1, transactionCopyVMs.Count(), false);
                ViewData["CopiesData"] = copies;

                //CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionExternalCopyVMs, 1, transactionExternalCopyVMs.Count(), false);
                //ViewData["ExternalCopiesData"] = externalCopies;

                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.Copies, AuditEntityName.TransactionCopy, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.Copies);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogCopies);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateCopiesPartial.cshtml");

            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult GetElectronicreceipt(int transactionId, int Ascending = 0)
        {
            try
            {

                int itemsCount = 0;
                GetResult<List<TransactionExternalCopyDTO>> transactionExternalCopyDTOs =
                  HttpClientWrapper<GetResult<List<TransactionExternalCopyDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionExternalCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                List<TransactionExternalCopyVM> transactionExternalCopyVMs = TransactionExternalCopyMapper.Map(transactionExternalCopyDTOs.Result);
                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionExternalCopyVMs, 1, transactionExternalCopyVMs.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;
                GetResult<List<TransactionCopyDTO>> transactionCopyDTOs =
                    HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                List<TransactionCopyVM> transactionCopyVMs = TransactionCopyMapper.Map(transactionCopyDTOs.Result);

                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionCopyVMs, 1, transactionCopyVMs.Count(), false);
                ViewData["CopiesData"] = copies;
                Session["TransactionId"] = transactionId;
                ViewData["TransactionAssignmentHistories"] = GetAssignment(transactionId, Ascending);
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                        HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                ViewData["ConfidentialityName"] = inboundCertificateVM.ConfidentialityLevel;
                ViewData["PriorityLevel"] = inboundCertificateVM.PriorityLevel;
                ViewData["Subject"] = inboundCertificateVM.Subject;
                ViewData["TransactionsNumber"] = inboundCertificateVM.InboundNumber;

                //string HTMLDesign = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_PrintElectronicreceipt.cshtml", null);
                //Session["DocoNutDocument"] = PdfHelper.ConvertHtml2PDF(HTMLDesign);
                return View("~/Areas/User/Views/Shared/TransactionCertificate/_PrintElectronicreceipt.cshtml");

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult PrintElectronicreceipt(int transactionId, int Ascending = 0)
        {
            try
            {

                int itemsCount = 0;
                GetResult<List<TransactionExternalCopyDTO>> transactionExternalCopyDTOs =
                  HttpClientWrapper<GetResult<List<TransactionExternalCopyDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionExternalCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                List<TransactionExternalCopyVM> transactionExternalCopyVMs = TransactionExternalCopyMapper.Map(transactionExternalCopyDTOs.Result);
                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionExternalCopyVMs, 1, transactionExternalCopyVMs.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;
                GetResult<List<TransactionCopyDTO>> transactionCopyDTOs =
                    HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                List<TransactionCopyVM> transactionCopyVMs = TransactionCopyMapper.Map(transactionCopyDTOs.Result);
                transactionCopyVMs.RemoveAll(x => x.IsBcc == true);
                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionCopyVMs, 1, transactionCopyVMs.Count(), false);
                ViewData["CopiesData"] = copies;
                Session["TransactionId"] = transactionId;
                ViewData["TransactionAssignmentHistories"] = GetAssignment(transactionId, Ascending);
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                        HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                ViewData["ConfidentialityName"] = inboundCertificateVM.ConfidentialityLevel;
                ViewData["PriorityLevel"] = inboundCertificateVM.PriorityLevel;
                ViewData["Subject"] = inboundCertificateVM.Subject;
                ViewData["TransactionsNumber"] = inboundCertificateVM.InboundNumber;
                return View("~/Areas/User/Views/Shared/TransactionCertificate/_PrintElectronicreceiptDocument.cshtml");

            }
            catch (Exception)
            {

                throw;
            }
        }


        public byte[] ConvertHtmlToImageBytes(string htmlString, int width, int height)
        {
            try
            {

                var htmlToImageConv = new NReco.ImageGenerator.HtmlToImageConverter();

                htmlToImageConv.Width = width;
                htmlToImageConv.Height = height;
                htmlToImageConv.ProcessPriority = System.Diagnostics.ProcessPriorityClass.High;
                return htmlToImageConv.GenerateImage(htmlString, NReco.ImageGenerator.ImageFormat.Png);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.TransactionCertificate.CopiesExternal)]
        public ActionResult GetTransactionExternalCopiesByTransactionId(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<List<TransactionExternalCopyDTO>> transactionExternalCopyDTOs =
                  HttpClientWrapper<GetResult<List<TransactionExternalCopyDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionExternalCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionExternalCopyVM> transactionExternalCopyVMs = TransactionExternalCopyMapper.Map(transactionExternalCopyDTOs.Result);
                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionExternalCopyVMs, 1, transactionExternalCopyVMs.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;

                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.ExternalCopies, AuditEntityName.TransactionExternalCopy, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.ExternalCopies);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogExternalCopies);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateExternalCopiesPartial.cshtml");

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.TransactionCertificate.Explanations)]
        public ActionResult GetExplanationsByTransactionId(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<List<ExplanationDTO>> transactionCopyDTOs =
                 HttpClientWrapper<GetResult<List<ExplanationDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetExplanationsByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                var explanations = ExplanationMapper.Map(transactionCopyDTOs.Result);
                CustomGridMvc.IAjaxGrid explanationsGrid = (CustomGridMvc.AjaxGrid<ExplanationVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(explanations, 1, explanations.Count(), false);


                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                ViewData["TransactionId"] = transactionId;

                List<AuditVM> auditVMs = new List<AuditVM>();//GetTransactionAuditing(transactionId, AuditFor.ExplanationsAuditDetails, AuditEntityName.Explanation, out itemsCount);
               // auditVMs.ForEach(a => a.AuditFor = AuditFor.ExplanationsAuditDetails);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogExplanations);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                // var explanations = TransactionHelper.GetTransactionExplanations(transactionId);
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_ExplanationsCretificatePartial.cshtml", explanationsGrid);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult GetTransactionAssignment(int transactionId)
        {
            try
            {
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                       HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(string.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                GetResult<List<TransactionAssignmentDTO>> TransactionAssignmentDTOs =
                HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/TransactionLog/GetTransactionAssignment?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(TransactionAssignmentDTOs.Result);


                inboundCertificateVM.Assignments = transactionAssignmentVMs;
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateAssignmentsPartial.cshtml", inboundCertificateVM);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetTransactionNames(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<List<TransactionNameDTO>> TransactionNameDTOs =
               HttpClientWrapper<GetResult<List<TransactionNameDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionNames?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionNameVM> transactionNameVMs = TransactionNameMapper.Map(TransactionNameDTOs.Result);

                CustomGridMvc.IAjaxGrid names = (CustomGridMvc.AjaxGrid<TransactionNameVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionNameVMs, 1, transactionNameVMs.Count(), false);

                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.NamesAuditDetails, AuditEntityName.TransactionName, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.NamesAuditDetails);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogNames);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateNamesPartial.cshtml", names);

            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        public ActionResult GetTransactionLinks(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<List<TransactionCertificateLinkDTO>> TransactionCertificateLinks =
               HttpClientWrapper<GetResult<List<TransactionCertificateLinkDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionLinksForCertificate?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionCertificateLinkVM> transactionCertificateLinkVMs = TransactionCertificateLinkMapper.Map(TransactionCertificateLinks.Result);

                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.Links, AuditEntityName.Link, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.Links);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogAttachments);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateLinksPartial.cshtml", transactionCertificateLinkVMs);


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        [HttpGet]
        public ActionResult GetTransactionAttachments(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                       HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(String.Format("api/TransactionLog/GetInboundBasicInfo?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                GetResult<List<TransactionAttachmentDTO>> transactionCopyDTOs =
               HttpClientWrapper<GetResult<List<TransactionAttachmentDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetTransactionAttachments?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionAttachmentVM> transactionAttachmentVMs = TransactionAttachmentMapper.Map(transactionCopyDTOs.Result);

                inboundCertificateVM.Attachments = transactionAttachmentVMs;

                GetResult<List<ExplanationDTO>> transactionExplanationDTOs =
                HttpClientWrapper<GetResult<List<ExplanationDTO>>>.GetItemRequest(String.Format("api/TransactionLog/GetExplanationsByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                var explanations = ExplanationMapper.Map(transactionExplanationDTOs.Result);
                CustomGridMvc.IAjaxGrid explanationsGrid = (CustomGridMvc.AjaxGrid<ExplanationVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(explanations, 1, explanations.Count(), false);
                ViewData["ExplanationsData"] = explanationsGrid;
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();

                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.AttachmentsAuditDetails, AuditEntityName.Attachment, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.AttachmentsAuditDetails);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["AttachmentsAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogAttachments);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateArchivingPartial.cshtml", inboundCertificateVM);

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        public ActionResult GetTransactionTasks(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = TransactionController.GetTransactionTasks(transactionId);

                CustomGridMvc.AjaxGrid<TaskAddVM> Grid = (CustomGridMvc.AjaxGrid<TaskAddVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;

                ViewData["Tasks"] = taskAddVM;
                ViewData["hdnTaskArray"] = JsonConvert.SerializeObject(gridData);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = TransactionController.GetTransactionTasksReply(transactionId);

                taskAddVM.TasksReplyGrid = (CustomGridMvc.AjaxGrid<ReceivedTaskVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);
                ViewData["TransactionId"] = transactionId;
                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.Tasks, AuditEntityName.Task, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.Tasks);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogTasks);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateTasksPartial.cshtml", taskAddVM);

            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        public ActionResult GetTransactionFollowUp(int transactionId)
        {
            try
            {
                int itemsCount = 0;
                GetResult<IList<TransactionFollowUpDTO>> transactionFollowUpResult =
                              HttpClientWrapper<GetResult<IList<TransactionFollowUpDTO>>>.GetItemRequest(String.Format("api/Transaction/TransactionFollowUpSelectByTransId?transId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionFollowUpVM> transactionFollowUpVMs = TransactionFollowUpMapper.Map(transactionFollowUpResult.Result);

                CustomGridMvc.IAjaxGrid transactionFollowUpGrid = (CustomGridMvc.AjaxGrid<TransactionFollowUpVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionFollowUpVMs, 1, transactionFollowUpVMs.Count(), false);

                ViewData["TransactionId"] = transactionId;
                List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.FollowUp, AuditEntityName.FollowUp, out itemsCount);
                auditVMs.ForEach(a => a.AuditFor = AuditFor.FollowUp);
                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);
                ViewData["TransactionAudits"] = auditGrid;
                ViewData["AuditType"] = BuildAuditTypeDataSource();
                ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogFollowUps);
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                Session["TransactionId"] = transactionId;
                return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/CertificatePartials/_CertificateFollowUpPartial.cshtml", transactionFollowUpGrid);

            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Audit&Log
        [HttpGet]
        public List<AuditVM> GetTransactionAuditing(int transactionId, AuditFor auditFor, string EntityName, out int itemsCount)
        {
            string parameters = GetListTransactionParameters(null);
            GetResult<List<AuditDTO>> AuditDTOs =
                                   HttpClientWrapper<GetResult<List<AuditDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionAuditing?{0}&userId={1}&orgUnitId={2}&transactionId={3}&EntityName={4}&cultureName={5}&auditFor={6}",
                                     parameters, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, transactionId, EntityName, SessionInfo.CultureShortName, auditFor)).Result;

            List<AuditVM> auditVMs = AuditMapper.Map(AuditDTOs.Result);
            itemsCount = AuditDTOs.RowsCount ?? 0;

            GetResult<SettingDTO> SettingValue = null;
            SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;

            var settingVM = SettingMapper.Map(SettingValue.Result);

            int GridSize = Convert.ToInt32(settingVM.Value);
            ViewData["PaginationData"] = new Pagination { Page = 1, PageSize = GridSize, TotalCount = itemsCount };
            ViewData["TransactionId"] = transactionId;
            ViewData["auditFor"] = (int)auditFor;
            ViewData["EntityName"] = EntityName;
            return auditVMs;
        }
        [HttpPost]
        public ActionResult UpdateTransactionAuditingGrid(int transactionId, AuditFor auditFor, string EntityName, string sortType, int? searchData, int? page)
        {
            string parameters = GetListTransactionParameters(page ?? 1);
            parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
            GetResult<List<AuditDTO>> AuditDTOs =
                                   HttpClientWrapper<GetResult<List<AuditDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionAuditing?{0}&userId={1}&orgUnitId={2}&transactionId={3}&EntityName={4}&cultureName={5}&auditFor={6}",
                                      parameters, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, transactionId, EntityName, SessionInfo.CultureShortName, auditFor)).Result;
            int itemsCount = AuditDTOs.RowsCount ?? 0;

            List<AuditVM> auditVMs = AuditMapper.Map(AuditDTOs.Result);
            auditVMs.ForEach(a => a.AuditFor = auditFor);
            foreach (var item in auditVMs)
            {
                item.OperationTypeName = DbRes.TResource("Enum.OperationType." + item.OperationType);
            }
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int GridSize = Convert.ToInt32(settingVM.Value);
            ViewData["TransactionId"] = transactionId;
            ViewData["auditFor"] = (int)auditFor;
            ViewData["EntityName"] = EntityName;
            CustomGridMvc.IAjaxGrid AuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false, UIHelper.PageSize);

            return Json(new { PaginationData = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogPaginationPartial.cshtml", new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount }), Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_AuditingTablePartial.cshtml", AuditGrid) });
        }
        [HttpPost]
        public ActionResult TransactionAuditingGridEventHandler(int transactionId, AuditFor auditFor, string EntityName, string sortType, int? searchData, int? page)
        {
            string parameters = GetListTransactionParameters(page ?? 1);
            parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
            GetResult<List<AuditDTO>> AuditDTOs =
                                   HttpClientWrapper<GetResult<List<AuditDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionAuditing?{0}&userId={1}&orgUnitId={2}&transactionId={3}&EntityName={4}&cultureName={5}&auditFor={6}",
                                      parameters, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, transactionId, EntityName, SessionInfo.CultureShortName, auditFor)).Result;
            int itemsCount = AuditDTOs.RowsCount ?? 0;
            List<AuditVM> auditVMs = AuditMapper.Map(AuditDTOs.Result);
            auditVMs.ForEach(a => a.AuditFor = auditFor);
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int GridSize = Convert.ToInt32(settingVM.Value);
            ViewData["TransactionId"] = transactionId;
            ViewData["auditFor"] = (int)auditFor;
            ViewData["EntityName"] = EntityName;
            CustomGridMvc.IAjaxGrid AuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, true);

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_AuditingTablePartial.cshtml", AuditGrid) });
        }
        [HttpPost]
        public ActionResult PrintTransactionAuditingGrid(int transactionId, AuditFor auditFor, string EntityName)
        {
            string parameters = GetListTransactionParameters(null);
            GetResult<List<AuditDTO>> AuditDTOs =
                                   HttpClientWrapper<GetResult<List<AuditDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionAuditingForPrint?{0}&userId={1}&orgUnitId={2}&transactionId={3}&EntityName={4}&cultureName={5}&auditFor={6}",
                                      parameters, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, transactionId, EntityName, SessionInfo.CultureShortName, auditFor)).Result;
            int itemsCount = AuditDTOs.RowsCount ?? 0;
            if (AuditDTOs.Result == null)
            {
                AuditDTOs.Result = new List<AuditDTO>();
            }
            List<AuditVM> auditVMs = AuditMapper.Map(AuditDTOs.Result);

            CustomGridMvc.IAjaxGrid AuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false);

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_PrintAuditingTable.cshtml", AuditGrid) });
        }
        public List<TransactionLogDetailInfoVM> GetTransactionLogInfo(int transactionId, bool IsForPrint, string parameters, out int itemsCount)
        {
            GetResult<List<TransactionLogDetailInfoDTO>> TransactionLogInfos =
                                   HttpClientWrapper<GetResult<List<TransactionLogDetailInfoDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionLogDetailsInfo?{0}&transactionId={1}&cultureName={2}&IsForPrint={3}",
                                     parameters, transactionId, SessionInfo.CultureShortName, IsForPrint)).Result;

            itemsCount = TransactionLogInfos.RowsCount ?? 0;
            List<TransactionLogDetailInfoVM> transactionLogInfoVMs = TransactionLogInfoMapper.Map(TransactionLogInfos.Result);
            // commented until the SP finished
            return transactionLogInfoVMs;
        }

        [HttpGet]
        public ActionResult GetTransactionLogDetailsInfo(int transactionId, int userId)
        {
            GetResult<List<TransactionLogDetailInfoDTO>> TransactionLogDetailsInfos =
                                   HttpClientWrapper<GetResult<List<TransactionLogDetailInfoDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionLogDetailsInfo?transactionId={0}&userId={1}&cultureName={2}",
                                      transactionId, userId, SessionInfo.CultureShortName)).Result;

            List<TransactionLogDetailInfoVM> transactionLogDetailsInfoVMs = TransactionLogInfoMapper.Map(TransactionLogDetailsInfos.Result);
            // commented until the SP finished
            var grid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailsInfoVMs, 1, transactionLogDetailsInfoVMs.Count(), false);
            return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogDetailInfoPartial.cshtml", grid);

        }
        [HttpGet]
        public ActionResult GetEntityAuditing(int auditId, int auditFor, string propName)
        {
            propName = propName == "" ? "empty" : propName;
            GetResult<List<AuditDetailDTO>> AuditDetailsDTO =
                                   HttpClientWrapper<GetResult<List<AuditDetailDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetEntityAuditing?auditFor={0}&auditId={1}&PropName={2}&cultureName={3}",
                                      (AuditFor)auditFor, auditId, propName, SessionInfo.CultureShortName)).Result;

            List<AuditDetailVM> AuditDetails = AuditMapper.Map(AuditDetailsDTO.Result);
            foreach (var item in AuditDetails)
            {
                item.PropertyName = DbRes.TResource("User.Audit.TransactionLog." + item.PropertyName);
            }
            //var grid = (CustomGridMvc.AjaxGrid<AuditDetailVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(AuditDetails, 1, AuditDetails.Count(), false);
            return PartialView("~/Areas/User/Views/Shared/TransactionCertificate/_EntityAuditGridPartial.cshtml", AuditDetails);
        }
        [HttpGet]
        public ActionResult GetTransactionAuditingGrid(int transactionId, AuditFor auditFor, string EntityName)
        {
            List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, auditFor, EntityName, out int itemsCount);

            foreach (var item in auditVMs)
            {
                item.OperationTypeName = DbRes.TResource("Enum.OperationType." + item.OperationType);
            }

            auditVMs.ForEach(a => a.AuditFor = AuditFor.MainDataAuditDetails);

            CustomGridMvc.IAjaxGrid AuditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, itemsCount, false, UIHelper.PageSize);

            ViewData["PaginationData"] = new Pagination { Page = 1, PageSize = UIHelper.PageSize, TotalCount = itemsCount };
            ViewData["TransactionId"] = transactionId;
            ViewData["auditFor"] = (int)auditFor;
            ViewData["EntityName"] = EntityName;
            ViewData["PropNames"] = GetPropsNames(LookupCategory.TransactionLogBasicInfo);
            //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
            ViewData["AuditType"] = BuildAuditTypeDataSource();
            ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionAuditGridPartial.cshtml", AuditGrid) }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetTransactionLogGrid(int transactionId)
        {
            string parameters = GetListTransactionParameters(null);
            List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, false, parameters, out int itemsCount);

            CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, false, UIHelper.PageSize);

            ViewData["PaginationData"] = new Pagination { Page = 1, PageSize = UIHelper.PageSize, TotalCount = itemsCount };
            ViewData["TransactionId"] = transactionId;
            //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
            ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
            ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogGridPartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult TransactionLogGridEventHandler(int transactionId, string sortType, int? searchData, int? page)
        {
            string parameters = GetListTransactionParameters(page ?? 1);
            parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
            List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, false, parameters, out int itemsCount);

            CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, true, UIHelper.PageSize);

            int GridSize = UIHelper.PageSize;
            ViewData["PaginationData"] = new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount };
            ViewData["TransactionId"] = transactionId;
            //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
            ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
            ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

            return Json(new { PaginationData = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogPaginationPartial.cshtml", new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount }), Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_LoggingTablePartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult UpdateTransactionLogGrid(int transactionId, string sortType, int? searchData, int? page)
        {
            string parameters = GetListTransactionParameters(page ?? 1);
            parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
            List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, false, parameters, out int itemsCount);

            CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, false, UIHelper.PageSize);

            int GridSize = UIHelper.PageSize;
            ViewData["PaginationData"] = new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount };
            ViewData["TransactionId"] = transactionId;
            //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
            ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
            ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

            return Json(new { PaginationData = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogPaginationPartial.cshtml", new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount }), Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_LoggingTablePartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult PrintTransactionLoggingGrid(int transactionId, string sortType, int? searchData)
        {
            int page = Convert.ToInt32(Request.Form["page"]);
            string parameters = GetListTransactionParameters(null);
            parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
            List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, true, parameters, out int itemsCount);

            CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, false);

            //int GridSize = UIHelper.PageSize;
            //ViewData["PaginationData"] = new Pagination { Page = page, PageSize = GridSize, TotalCount = itemsCount };
            //ViewData["TransactionId"] = transactionId;
            //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
            //ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
            //ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

            //return Json(new { PaginationData = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogPaginationPartial.cshtml", new Pagination { Page = page, PageSize = GridSize, TotalCount = itemsCount }), Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_LoggingTablePartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_PrintLogTable.cshtml", LogGrid) });

        }
        private string GetPropsNames(LookupCategory lookupCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                List<LookupVM> lookupVMs = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName).Result.ToList();

                if (lookupVMs != null)
                {
                    foreach (LookupVM lookupVM in lookupVMs)
                    {
                        var ResourceKey = lookupVM.Text;
                        var Label = DbRes.TResource("User.Audit.TransactionLog." + ResourceKey);
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = ResourceKey,
                            Label = Label
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
        private string GetLookupsAudingActionCodes(LookupCategory lookupCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                List<LookupVM> lookupVMs = LookupsHelper.GetLookupItems(LookupCategory.AuditingActionCode, SessionInfo.CultureShortName).Result.ToList();

                if (lookupVMs != null)
                {
                    foreach (LookupVM lookupVM in lookupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = lookupVM.Id.ToString(),
                            Label = lookupVM.Text
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
        private string BuildAuditTypeDataSource()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = ((int)framework.OperationType.Insert).ToString(),
                    Label = DbRes.TResource("User.TransactionCertificate.Insert")
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = ((int)framework.OperationType.Update).ToString(),
                    Label = DbRes.TResource("User.TransactionCertificate.Update")
                });

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetListTransactionParameters(int? pageValue)
        {
            StringBuilder result = new StringBuilder();
            string filter = Request.Form["filter"];
            string sortColumnName = Request.Form["gridColumn"];
            string dir = Request.Form["dir"];
            string pageIndex = pageValue.HasValue ? pageValue.Value.ToString() : Request.Form["page"];
            string searchColumn = Request.Form["searchColumn"];
            string fromDate = Request.Form["fromDate"];
            string toDate = Request.Form["toDate"];
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            string pageSize = settingVM.Value;
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
                result.Append("&PageSize=").Append(UIHelper.PageSize);
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
        #endregion
        public List<AuditDetailVM> GetAuditDetails(int auditId, int auditFor)
        {

            GetResult<List<AuditDetailDTO>> AuditDetailsDTO =
                                   HttpClientWrapper<GetResult<List<AuditDetailDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetEntityAuditing?auditFor={0}&auditId={1}&PropName={2}&cultureName={3}",
                                      (AuditFor)auditFor, auditId, "empty", SessionInfo.CultureShortName)).Result;

            List<AuditDetailVM> AuditDetails = AuditMapper.Map(AuditDetailsDTO.Result);
            foreach (var item in AuditDetails)
            {
                item.PropertyName = DbRes.TResource("User.Audit.TransactionLog." + item.PropertyName);
            }
            return AuditDetails;
        }
    }
}