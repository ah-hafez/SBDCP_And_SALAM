using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.File;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Search.TransactionCertificate;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Support;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Common;
using MCS.UI.Mappers;
using ZXing;
using CustomGridMvc = MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Models.OrgUnit;
using SelectPdf;
using Spire.Pdf.HtmlConverter;
using Spire.Pdf;
using System.IO;
using System.Drawing;
using System.Web;
using MCS.UI.Helpers;
using UserDocuments = MCS.UI.Areas.User.Mappers.Shared;
using System.Drawing.Imaging;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DotnetDaddy.DocumentViewer;
using DotnetDaddy.DocumentConfig;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.DTO.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using DocumentFormat.OpenXml.Wordprocessing;
using static MCS.Common.UserClaims;
using System.Drawing.Drawing2D;
using System.Configuration;
using DocumentFormat.OpenXml.Office2010.Excel;
using MCS.DoconutMVC.Helpers;
using MCS.DTO;
using MCS.UI.Helpers.Extensions;
using DocumentFormat.OpenXml.Drawing.Charts;
using System.Web.UI.WebControls;

namespace MCS.UI.Areas.User.Controllers
{
    public class SharedController : BaseController
    {
        #region Audit&Log
        [HttpGet]
        public List<AuditVM> GetTransactionAuditing(int transactionId, AuditFor auditFor, string EntityName)
        {
            GetResult<List<AuditDTO>> AuditDTOs =
                                   HttpClientWrapper<GetResult<List<AuditDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionAuditing?userId={0}&orgUnitId={1}&transactionId={2}&EntityName={3}&cultureName={4}&auditFor={5}",
                                      SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, transactionId, EntityName, SessionInfo.CultureShortName, auditFor)).Result;

            List<AuditVM> auditVMs = AuditMapper.Map(AuditDTOs.Result);

            return auditVMs;
        }

        [HttpGet]
        public List<TransactionLogInfoVM> GetTransactionLogInfo(int transactionId)
        {
            GetResult<List<TransactionLogInfoDTO>> TransactionLogInfos =
                                   HttpClientWrapper<GetResult<List<TransactionLogInfoDTO>>>.GetItemRequest(string.Format(
                                       "api/Transaction/GetTransactionLogInfo?transactionId={0}&cultureName={1}",
                                      transactionId, SessionInfo.CultureShortName)).Result;

            List<TransactionLogInfoVM> transactionLogInfoVMs = TransactionLogInfoMapper.Map(TransactionLogInfos.Result);

            return transactionLogInfoVMs;
        }
        [HttpGet]
        public ActionResult ConvertLanguage()
        {
            CultureInfo cultureInfo;
            HttpCookie cookieTemp;
            var arCulture = ConfigurationManager.AppSettings["DefaultArabicCulture"].ToString();
            var enCulture = ConfigurationManager.AppSettings["DefaultEnglishCulture"].ToString();
            if (SessionInfo.CultureShortName == "en")
            {
                cultureInfo = new CultureInfo(arCulture);
                cookieTemp = cultureInfo.SetCookieCulture(arCulture);
            }
            else
            {
                cultureInfo = new CultureInfo(enCulture);
                cookieTemp = cultureInfo.SetCookieCulture(enCulture);
            }

            Response.Cookies.Add(cookieTemp);
            System.Globalization.CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            return RedirectToAction("MyTransactions", "File");
        }
        #endregion

        #region Certificate

        [HttpGet]
        public ActionResult InboundCertificate(string transactionId)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                    HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetInboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, trxId)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                //foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                //{
                //    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                //}

                //ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();
                string message = string.Empty;
                bool isPermition = false;
                switch (inboundCertificateVM.ConfidentialityId)
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
                    message = DbRes.TResource("PermissionAssignTo");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                CustomGridMvc.IAjaxGrid names = (CustomGridMvc.AjaxGrid<TransactionNameVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names, 1, inboundCertificateDTO.Result.Names.Count(), false);
                ViewData["NamesData"] = names;

                int current = 1;
                foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                {
                    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                    transactionAssignmentVM.Sequence = current;
                    current++;
                }
                CustomGridMvc.IAjaxGrid assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments, 1, inboundCertificateDTO.Result.Assignments.Count(), false);
                ViewData["AssignmentsData"] = assignments;

                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Copies, 1, inboundCertificateDTO.Result.Copies.Count(), false);
                ViewData["CopiesData"] = copies;

                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.ExternalCopies, 1, inboundCertificateDTO.Result.ExternalCopies.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;

                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                List<AuditVM> auditVMs = GetTransactionAuditing(trxId, AuditFor.MainDataAuditDetails, SessionInfo.CultureShortName); // just for testing 
                List<TransactionLogInfoVM> transactionLogInfoVMs = GetTransactionLogInfo(trxId);

                CustomGridMvc.IAjaxGrid auditGrid = (CustomGridMvc.AjaxGrid<AuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(auditVMs, 1, auditVMs.Count(), false);
                ViewData["TransactionAudits"] = auditGrid;

                CustomGridMvc.IAjaxGrid transactionLogInfoGrids = (CustomGridMvc.AjaxGrid<TransactionLogInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogInfoVMs, 1, transactionLogInfoVMs.Count(), false);
                ViewData["TransactionLogs"] = transactionLogInfoGrids;
                ViewData["TransactionId"] = trxId;
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundTransactionLogPartial.cshtml", inboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult OutboundInternalCertificate(string transactionId)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                    HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetInboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, trxId)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                //IAjaxGrid names = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names.AsQueryable(), 1, false, inboundCertificateDTO.Result.Names.Count(), true);

                //foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                //{
                //    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                //}

                //ViewData["NamesData"] = names;

                //IAjaxGrid assignments = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments.AsQueryable(), 1, false, inboundCertificateDTO.Result.Assignments.Count(), true);

                //ViewData["AssignmentsData"] = assignments;

                //ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();


                IAjaxGrid names = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names.AsQueryable(), 1, false, inboundCertificateDTO.Result.Names.Count(), false);
                ViewData["NamesData"] = names;

                int current = 1;
                foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                {
                    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                    transactionAssignmentVM.Sequence = current;
                    current++;
                }
                CustomGridMvc.IAjaxGrid assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments, 1, inboundCertificateDTO.Result.Assignments.Count(), false);
                ViewData["AssignmentsData"] = assignments;

                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Copies, 1, inboundCertificateDTO.Result.Copies.Count(), false);
                ViewData["CopiesData"] = copies;

                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.ExternalCopies, 1, inboundCertificateDTO.Result.ExternalCopies.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;

                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                List<AuditVM> auditVMs = GetTransactionAuditing(trxId, AuditFor.MainDataAuditDetails, SessionInfo.CultureShortName); // just for testing 
                List<TransactionLogInfoVM> transactionLogInfoVMs = GetTransactionLogInfo(trxId);

                ViewData["TransactionAudits"] = auditVMs;
                ViewData["TransactionLogs"] = transactionLogInfoVMs;
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundTransactionLogPartial.cshtml", inboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult OutboundCertificate(string transactionId)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                GetResult<OutboundCertificateDTO> outboundCertificateDTO =
                    HttpClientWrapper<GetResult<OutboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetOutboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, Convert.ToInt32(trxId))).Result;

                OutboundCertificateVM outboundCertificateVM = OutboundCertificateMapper.Map(outboundCertificateDTO.Result);
                outboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                //CustomGridMvc.IAjaxGrid names = (CustomGridMvc.AjaxGrid<TransactionNameVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.Names, 1, outboundCertificateVM.Names.Count(), true);
                //ViewData["NamesData"] = names;
                //ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                CustomGridMvc.IAjaxGrid assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.Assignments, 1, outboundCertificateVM.Assignments.Count(), false);
                ViewData["AssignmentsData"] = assignments;

                CustomGridMvc.IAjaxGrid names = (CustomGridMvc.AjaxGrid<TransactionNameVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.Names, 1, outboundCertificateVM.Names.Count(), false);
                ViewData["NamesData"] = names;

                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.Copies, 1, outboundCertificateDTO.Result.Copies.Count(), false);
                ViewData["CopiesData"] = copies;

                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.ExternalCopies, 1, outboundCertificateDTO.Result.ExternalCopies.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;

                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                List<AuditVM> auditVMs = GetTransactionAuditing(Convert.ToInt32(trxId), AuditFor.MainDataAuditDetails, SessionInfo.CultureShortName); // just for testing 
                List<TransactionLogInfoVM> transactionLogInfoVMs = GetTransactionLogInfo(Convert.ToInt32(trxId));

                ViewData["TransactionAudits"] = auditVMs;
                ViewData["TransactionLogs"] = transactionLogInfoVMs;
                Session["TransactionId"] = Convert.ToInt32(trxId);
                return View("~/Areas/User/Views/Shared/TransactionCertificate/_OutboundCertificatePartial.cshtml", outboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult SavedCertificate(string transactionId)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                GetResult<InboundCertificateDTO> inboundCertificateDTO =
                    HttpClientWrapper<GetResult<InboundCertificateDTO>>.GetItemRequest(string.Format("api/Transaction/GetInboundCertificate?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, trxId)).Result;

                InboundCertificateVM inboundCertificateVM = InboundCertificateMapper.Map(inboundCertificateDTO.Result);
                inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                //IAjaxGrid names = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names.AsQueryable(), 1, false, inboundCertificateDTO.Result.Names.Count(), true);

                //foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                //{
                //    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                //}

                //ViewData["NamesData"] = names;

                //IAjaxGrid assignments = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments.AsQueryable(), 1, false, inboundCertificateDTO.Result.Assignments.Count(), true);

                //ViewData["AssignmentsData"] = assignments;

                //ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();


                IAjaxGrid names = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names.AsQueryable(), 1, false, inboundCertificateDTO.Result.Names.Count(), false);
                ViewData["NamesData"] = names;

                int current = 1;
                foreach (TransactionAssignmentVM transactionAssignmentVM in inboundCertificateVM.Assignments)
                {
                    transactionAssignmentVM.DateH = transactionAssignmentVM.DateH + " " + transactionAssignmentVM.Date.ToShortTimeString();
                    transactionAssignmentVM.Sequence = current;
                    current++;
                }
                CustomGridMvc.IAjaxGrid assignments = (CustomGridMvc.AjaxGrid<TransactionAssignmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments, 1, inboundCertificateDTO.Result.Assignments.Count(), false);
                ViewData["AssignmentsData"] = assignments;

                CustomGridMvc.IAjaxGrid copies = (CustomGridMvc.AjaxGrid<TransactionCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Copies, 1, inboundCertificateDTO.Result.Copies.Count(), false);
                ViewData["CopiesData"] = copies;

                CustomGridMvc.IAjaxGrid externalCopies = (CustomGridMvc.AjaxGrid<TransactionExternalCopyVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.ExternalCopies, 1, inboundCertificateDTO.Result.ExternalCopies.Count(), false);
                ViewData["ExternalCopiesData"] = externalCopies;

                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                // List<AuditVM> auditVMs = GetTransactionAuditing(transactionId, AuditFor.MainDataAuditDetails, SessionInfo.CultureShortName); // just for testing 
                List<TransactionLogInfoVM> transactionLogInfoVMs = GetTransactionLogInfo(trxId);

                //     ViewData["TransactionAudits"] = auditVMs;
                ViewData["TransactionLogs"] = transactionLogInfoVMs;
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundTransactionLogPartial.cshtml", inboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridNames(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionNameVM> transactionNames = javaScriptSerializer.Deserialize<List<TransactionNameVM>>(param);

                CustomGridMvc.IAjaxGrid grid = new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionNames, page.HasValue ? page.Value : 1, transactionNames.Count(), page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Shared/TransactionCertificate/_NameGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridAssignments(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignments = javaScriptSerializer.Deserialize<List<TransactionAssignmentVM>>(param);

                CustomGridMvc.IAjaxGrid grid = new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAssignments, page.HasValue ? page.Value : 1, transactionAssignments.Count(), page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Shared/TransactionCertificate/_AssignmentGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ShowDocumentViewer(string documentId, string documentSessionKey, bool Mode = true)
        {
            try
            {
                Session["IsEditMode"] = false;


                var documentData = Session["DocumentData"] as Dictionary<string, byte[]> ?? new Dictionary<string, byte[]>();
                string key = StringCipher.DecryptStringAES(documentId.Replace(" ", "+"));
                DocumentVM documentVM = new DocumentVM();
                if (documentData != null && documentData.Keys.Any(x => x == documentSessionKey))
                {
                    documentVM.Content = documentData[documentSessionKey];
                    documentVM.Id = 0;
                    documentVM.Key = documentSessionKey;
                    documentVM.MimeType = Session[documentSessionKey + "MimeType"] != null ? Session[documentSessionKey + "MimeType"].ToString() : System.Net.Mime.MediaTypeNames.Application.Pdf;
                }
                else
                {
                    int documentIdClear = int.Parse(StringCipher.DecryptStringAES(documentId.Replace(" ", "+")));
                    GetResult<DocumentDTO> documentDTO =
                        HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, documentIdClear)).Result;
                    documentVM = DocumentMapper.Map(documentDTO.Result);
                    documentVM.Key = documentVM.Id.ToString();

                }




                if (documentVM.MimeType == "application/octet-stream")
                {
                    documentVM.Content = ConvertWordToPDF(Convert.ToBase64String(documentVM.Content));
                }

                ViewData["DocumentSessionKey"] = documentSessionKey;
                documentVM.Mode = Mode;



                return View("~/Areas/User/Views/Shared/TransactionCertificate/_DocumentViewerPartial.cshtml", documentVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public FileResult DocumentDownload(string documentId)
        {

            var documentData = Session["DocumentData"] as Dictionary<string, byte[]> ?? new Dictionary<string, byte[]>();
            string key = StringCipher.DecryptStringAES(documentId.Replace(" ", "+"));
            bool isValid = false;
            int documentIdClear = 0;
            int.TryParse(key, out documentIdClear);
            DocumentVM documentVM = new DocumentVM();
            if (documentData.Keys.Any(x => x == key) && documentIdClear == 0)
            {
                documentVM.Content = documentData[key];
                documentVM.MimeType = Session[key + "MimeType"].ToString();
                documentVM.Name = "NewFile." + GetMimeType(Session[key + "MimeType"].ToString());
            }
            else
            {

                GetResult<DocumentDTO> documentDTO =
                        HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, documentIdClear)).Result;
                if (documentDTO.Result != null)
                {
                    documentVM.Content = documentDTO.Result.Content;
                    documentVM.MimeType = documentDTO.Result.MimeType;
                    documentVM.Name = documentDTO.Result.Name;
                }
                else
                    return null;


            }

            return File(documentVM.Content, documentVM.MimeType, documentVM.Name);
        }



        [HttpGet]
        public ActionResult ShowAttatchmentViewer(int documentId, string documentSessionKey, bool Mode = true)
        {
            try
            {
                Session["IsEditMode"] = false;
                GetResult<DocumentDTO> documentDTO =
                    HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, documentId)).Result;

                DocumentVM documentVM = DocumentMapper.Map(documentDTO.Result);

                ViewData["DocumentSessionKey"] = documentSessionKey;
                documentVM.Mode = Mode;



                return View("~/Areas/User/Views/Shared/TransactionCertificate/_AttachmentViewerPartial.cshtml", documentVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static void ConvertFile()
        {

        }
        [HttpGet]
        public ActionResult ShowHUBDocumentViewer(int documentId, string documentSessionKey)
        {
            try
            {
                Session["IsEditMode"] = false;
                GetResult<DocumentDTO> documentDTO =
                    HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetHUBDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, documentId)).Result;

                DocumentVM documentVM = DocumentMapper.Map(documentDTO.Result);
                ViewData["DocumentSessionKey"] = documentSessionKey;

                return View("~/Areas/User/Views/Shared/TransactionCertificate/_DocumentViewerPartial.cshtml", documentVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Certificate

        #region PrintBarcode

        [HttpGet]
        public ActionResult GetBarcodeImage()
        {
            byte[] barcodeImg = null;
            if (Session["BarcodeImgByte"] != null)
            {
                barcodeImg = Session["BarcodeImgByte"] as byte[];
                return File(barcodeImg, "image/png");
            }
            return new EmptyResult();
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



        public void FillBarcodeDesign(string HtmlDesign, BarcodeVM barcodeVM, 
            TransactionBarcodesVM transactionBarcodesVM, int width, int heigth)
        {
            try
            {
                string barcode2D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, 20, 80);
                string barcode3D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.QR_CODE, 20, 20);
                string Logo = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/GAMI_Logo_Color-01.svg";
                string background = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/MODbackground.png";
                string TransactionNumberWithSymbol = transactionBarcodesVM.OrgUnitSymbol.ToString() + transactionBarcodesVM.TransactionNumber.ToString();
                //string TransactionNumberWithSymbol = SessionInfo.CultureShortName == "ar" ? transactionBarcodesVM.TransactionNumber.ToString() : transactionBarcodesVM.TransactionNumber.ToString();

                if (SessionInfo.CultureShortName == Constants.Languages.English)
                {
                    HtmlDesign = HtmlDesign.Replace("direction", Constants.LeftDirection);
                }

                //HtmlDesign = HtmlDesign.Replace("{1}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{1}", barcode2D);
                HtmlDesign = HtmlDesign.Replace("{15}", background);
                //HtmlDesign = HtmlDesign.Replace("{3}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{2}", barcode3D);
                HtmlDesign = HtmlDesign.Replace("{3}", Logo);
                if (transactionBarcodesVM.TransactionCategory == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    HtmlDesign = HtmlDesign.Replace("{4}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InboundNumber") + " : ");
                    HtmlDesign = HtmlDesign.Replace("{5}", TransactionNumberWithSymbol);
                }
                else if (transactionBarcodesVM.TransactionCategory == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    HtmlDesign = HtmlDesign.Replace("{4}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.OutboundNumber") + " : ");
                    HtmlDesign = HtmlDesign.Replace("{5}", TransactionNumberWithSymbol);
                }
                else if (transactionBarcodesVM.TransactionCategory == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    HtmlDesign = HtmlDesign.Replace("{4}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InternalTransactionNumber") + " : ");
                    HtmlDesign = HtmlDesign.Replace("{5}", TransactionNumberWithSymbol);
                }
                else
                {
                    HtmlDesign = HtmlDesign.Replace("{4}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber") + " : ");
                    HtmlDesign = HtmlDesign.Replace("{5}", TransactionNumberWithSymbol);
                }

                HtmlDesign = HtmlDesign.Replace("{4}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.OrgUnitInfo.Number") + " : ");
                HtmlDesign = HtmlDesign.Replace("{5}", TransactionNumberWithSymbol);

                HtmlDesign = HtmlDesign.Replace("{11}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments") + " : ");
                //HtmlDesign = HtmlDesign.Replace("{7}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Department") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{7}", SessionInfo.CultureShortName == "ar" ? transactionBarcodesVM.Entity :
                    transactionBarcodesVM.Entity);
                //if (transactionBarcodesVM.TransactionCategory == (int)TransactionCategory.Inbound || transactionBarcodesVM.TransactionCategory == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                //{
                //    HtmlDesign = HtmlDesign.Replace("{13}", string.Empty);
                //    HtmlDesign = HtmlDesign.Replace("{14}", transactionBarcodesVM.TransactionType);

                //    //HtmlDesign = HtmlDesign.Replace("{15}", "");
                //    HtmlDesign = HtmlDesign.Replace("{16}", SessionInfo.CurrentUser.TenantName);
                //}
                if (!string.IsNullOrEmpty(barcodeVM.EntityName))
                {
                    HtmlDesign = HtmlDesign.Replace("{13}",
                        "الجهه المرسل لها :  ");
                    HtmlDesign = HtmlDesign.Replace("{14}", barcodeVM.EntityName);

                }
                else
                {
                    HtmlDesign = HtmlDesign.Replace("{13}",
                        "");
                    HtmlDesign = HtmlDesign.Replace("{14}", "");

                }
                HtmlDesign = HtmlDesign.Replace("{8}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{9}", DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionBarcodesVM.TransactionDate) + " - " + (transactionBarcodesVM.TransactionDate.ToString("yyyy/MM/dd")));


                //HtmlDesign = HtmlDesign.Replace("{11}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{6}", string.Empty);
                // HtmlDesign = HtmlDesign.Replace("{10}", string.Join("/", transactionBarcodesVM.TransactionDate.ToShortDateString().Split('/').Reverse().ToList()));                  //HtmlDesign = HtmlDesign.Replace("{16}", string.Empty);
                //HtmlDesign = HtmlDesign.Replace("{17}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName") + " :  ");
                //HtmlDesign = HtmlDesign.Replace("{18}", SessionInfo.CurrentUser.TenantName);


                string attachmentValue = "لا يوجد";

                if (transactionBarcodesVM.AttachmentBarcodes.Count != 0)
                {
                    attachmentValue = string.Empty;
                }
                var attachmentCount = 0;
                foreach (AttachmentBarcodeVM attachmentBarcodeVM in transactionBarcodesVM.AttachmentBarcodes)
                {

                    attachmentValue = attachmentValue + string.Format("{0} {1} {2}", SessionInfo.CultureShortName == "ar" ? attachmentBarcodeVM.Count.ToString() :

                      attachmentBarcodeVM.Count.ToString(), attachmentBarcodeVM.Name, ",");
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


        public void FillBarcodeDesign(string HtmlDesign, BarcodeVM barcodeVM, TransactionVisitTicketVM transactionBarcodesVM, int width, int heigth)
        {
            try
            {
                string barcode2D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, (heigth / 8), ((width / 3) * 2));
                string Logo = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/GAMI_Logo_Color-01.svg";

                if (SessionInfo.CultureShortName == Constants.Languages.English)
                {
                    HtmlDesign = HtmlDesign.Replace("direction", Constants.LeftDirection);
                }
                HtmlDesign = HtmlDesign.Replace("{1}", string.Empty);
                HtmlDesign = HtmlDesign.Replace("{2}", "<img style='max-width:100%;max-height:100%;width:" + ((width / 3) * 2).ToString() + ";height:" + (heigth / 8).ToString() + "' src='" + barcode2D + "' />");
                HtmlDesign = HtmlDesign.Replace("{3}", Logo);
                string barcode3D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.QR_CODE, 150, 150, true);
                HtmlDesign = HtmlDesign.Replace("{4}", "<div style='max-width:100%;max-height:100%'><img style='width:50px;height:50px;' src='" + barcode3D + "' /> </div>");



                HtmlDesign = HtmlDesign.Replace("{9}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{10}", transactionBarcodesVM.TransactionNumber.ToString());



                HtmlDesign = HtmlDesign.Replace("{11}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{5}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Department") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{6}", transactionBarcodesVM.Entity);//CompanyName
                HtmlDesign = HtmlDesign.Replace("{13}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DirectedDepartment") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{14}", transactionBarcodesVM.Entity);
                HtmlDesign = HtmlDesign.Replace("{7}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{8}", transactionBarcodesVM.TransactionDateH);

                HtmlDesign = HtmlDesign.Replace("{15}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DocumentNumber") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{16}", transactionBarcodesVM.InboundNumber);
                HtmlDesign = HtmlDesign.Replace("{17}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InboundDestination") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{18}", transactionBarcodesVM.InboundDestination);
                HtmlDesign = HtmlDesign.Replace("{19}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.ToEntity") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{20}", transactionBarcodesVM.ToEntityName);
                HtmlDesign = HtmlDesign.Replace("{21}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Name") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{22}", "");
                HtmlDesign = HtmlDesign.Replace("{23}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Signature") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{24}", transactionBarcodesVM.Subject);
                HtmlDesign = HtmlDesign.Replace("{25}", "");//ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Inquery") + " :  ");
                HtmlDesign = HtmlDesign.Replace("{26}", SystemConfigurations.VisitTicketFooter);



                string attachmentValue = string.Empty;


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
            string background = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/MODbackground.png";


            string TransactionNumberWithSymbol = SessionInfo.CultureShortName == "ar" ? transactionBarcodesVM.TransactionNumber.ToString() : transactionBarcodesVM.TransactionNumber.ToString();


            if (SessionInfo.CultureShortName == Constants.Languages.English)
            {
                htmlDesign = htmlDesign.Replace("direction", Constants.LeftDirection);
            }
            htmlDesign = htmlDesign.Replace("{15}", background);

            if (transactionBarcodesVM.TransactionCategory == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
            {
                htmlDesign = htmlDesign.Replace("{transactionNumber}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InboundNumber") + " :  ");
                htmlDesign = htmlDesign.Replace("{transactionNumberValue}", TransactionNumberWithSymbol);

            }
            else if (transactionBarcodesVM.TransactionCategory == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
            {
                htmlDesign = htmlDesign.Replace("{transactionNumber}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.OutboundNumber") + " :  ");
                htmlDesign = htmlDesign.Replace("{transactionNumberValue}", TransactionNumberWithSymbol);


            }
            else if (transactionBarcodesVM.TransactionCategory == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
            {

                htmlDesign = htmlDesign.Replace("{transactionNumber}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InternalTransactionNumber") + " :  ");
                htmlDesign = htmlDesign.Replace("{transactionNumberValue}", TransactionNumberWithSymbol);

            }
            else
            {
                htmlDesign = htmlDesign.Replace("{transactionNumber}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber") + " :  ");
                htmlDesign = htmlDesign.Replace("{transactionNumberValue}", TransactionNumberWithSymbol);


            }



            htmlDesign = htmlDesign.Replace("{attachmentOrgunit}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Orgunit") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentCount}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Count") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentName}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentDate}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentDate") + " :  ");
            htmlDesign = htmlDesign.Replace("{attachmentOrgunitValue}", transactionBarcodesVM.Entity);
            htmlDesign = htmlDesign.Replace("{attachmentCountValue}", attachmentBarcodeVM.Count.ToString());
            htmlDesign = htmlDesign.Replace("{attachmentNameValue}", attachmentBarcodeVM.Name);
            htmlDesign = htmlDesign.Replace("{attachmentDateValue}", DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionBarcodesVM.TransactionDate));
            //htmlDesign = htmlDesign.Replace("{attachment2DImageValue}", "<img style='height: 20px; max-width: 150px;margin-bottom: 7px;' src='" + barcode2D + "' />");
            htmlDesign = htmlDesign.Replace("{attachment2DImageValue}", barcode2D);
            htmlDesign = htmlDesign.Replace("{attachment2DImage}", "");
            //htmlDesign = htmlDesign.Replace("{attachment3DImageValue}", "<div style='width:150px;height:100px'> <img style='max-width:100%;max-height:100%;width:" + 50 + "px;height:" + 50 + "px;' src='" + barcode3D + "' /> </div>");
            //htmlDesign = htmlDesign.Replace("{attachment3DImage}", "");

            barcodeVM.Content = ConvertHtmlToImageBytes(htmlDesign, width, heigth);
            barcodeVM.Templete = htmlDesign;
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintEncryptionCode)]
        public ActionResult PrinttingBarcodes(int transactionId)
        {
            try
            {

                GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
              HttpClientWrapper<GetResult<TransactionBarcodesDTO>>
              .GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}", SessionInfo.CultureShortName, transactionId, SessionInfo.OrgUnitId)).Result;
                if (transactionBarcodesDTOs == null)
                {
                    string message = DbRes.TValidation("User.PrinttingBarcodes.TransactionIsDeleted");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);
                if (transactionBarcodesDTOs.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionBarcodesDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                foreach (BarcodeVM barcodeVM in transactionBarcodesVM.BarcodeVMs)
                {
                    if (barcodeVM.Type == BarcodePrintType.Transaction)
                    {

                        FillBarcodeDesign(transactionBarcodesVM.TransactionBarcodeHtmlDesign, barcodeVM, 
                            transactionBarcodesVM, transactionBarcodesVM.TransactionDesignWidth,
                            transactionBarcodesVM.TransactionDesignHeight);
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


                ViewData["transId"] = transactionId;
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PrintBarcodePartial.cshtml", transactionBarcodesVM),
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion PrintBarcode

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintReviewTicket)]
        public ActionResult PrintTicket(int transactionId)
        {
            try
            {
                string message = string.Empty;

                GetResult<TransactionVisitTicketDTO> transactionVisitTicketDTO =
              HttpClientWrapper<GetResult<TransactionVisitTicketDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionVisitTicket?cultureName={0}&transactionId={1}&orgUnitId={2}", SessionInfo.CultureShortName, transactionId, SessionInfo.OrgUnitId)).Result;

                TransactionVisitTicketVM transactionVisitTicketVM = TransactionVisitTicketMapper.Map(transactionVisitTicketDTO.Result);
                if (transactionVisitTicketDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionVisitTicketDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                FillBarcodeDesign(transactionVisitTicketVM.VisitTicketHtmlDesign, transactionVisitTicketVM.barcodeVM, transactionVisitTicketVM, transactionVisitTicketVM.TicketDesignWidth, transactionVisitTicketVM.TicketDesignHeight);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PrintTicketPartial.cshtml", transactionVisitTicketVM.barcodeVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintReviewTicket)]
        public ActionResult PrintAddress(string DirectedTo, string DocumentType, string TransactionDate, string DirectedToOrgUnit, string TransactionNo)
        {
            {
                try
                {
                    TransactionAddressVM data = new TransactionAddressVM { DirectedTo = DirectedTo, DirectedToOrgUnit = DirectedToOrgUnit, DocumentType = DocumentType, TransactionDate = TransactionDate.ToString(), Transactionnumber = TransactionNo };
                    string message = string.Empty;
                    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PrintingAddressPartial.cshtml", data), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }


        #region File
        [HttpGet]
        public ActionResult GetMenuFile()
        {
            string message = string.Empty;

            GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<TrayDetailsVM> trayDetailsVMs = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
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
                trayDetailsVMs.ForEach(t =>
                {
                    t.IsExcluded = userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault() != null ?
                        !userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault().IsSelected : false;
                });
            }

            TempData["TrayDetails"] = trayDetailsVMs;

            return Json(new
            {
                FileMenuHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_UserFileMenuPartial.cshtml", null),
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetOrgUnitMenuFile()
        {
            string message = string.Empty;

            GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
                HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<TrayDetailsVM> trayDetailsVMs = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
            if (trayDetailsDTOs.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTOs.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            TempData["TrayDetails"] = trayDetailsVMs;

            return Json(new
            {
                FileMenuHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_UserFileMenuPartial.cshtml", null),
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        [CustomAction]
        public ActionResult GetOutBoundAddressInfo(int TransactionId)
        {
            try
            {
                GetResult<TransactionAddressDTO> getResult = HttpClientWrapper<GetResult<TransactionAddressDTO>>.GetItemRequest(string.Format("api/Transaction/GetOutBoundAddressInfo?TransactionId={0}&CultureName={1}", TransactionId, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new { MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                TransactionAddressVM transactionAddressVM = TransactionAddressMapper.Map(getResult.Result);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PrintingAddressPartial.cshtml", transactionAddressVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion File

        #region Reporters
        [HttpGet]
        public ActionResult AddReporter()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                var reporterVM = new ReporterVM();
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ReporterAddPartial", reporterVM)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddReporter(ReporterVM reporterVM)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                reporterVM.ToEntityId = SessionInfo.OrgUnitId;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PostReporter", ReporterMapper.Map(reporterVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.AddSucceeded");
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Dashboards.Dashboard)]
        public ActionResult DashboardHome()
        {
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.DashboardAllModules))
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
              HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.DashboardAllChildsModules))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                          HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = true;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Reports.DashboardParentDepartment))
            {
                GetResult<OrgUnitDTO> orgUnitDTOs =
                          HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
                newList.Add(orgUnitDTOs.Result);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);

                level = 2;
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

            ViewBag.Level = level;

            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId, true);

            GetResult<List<CounterDetailDTO>> counterDetailDTOs =
                      HttpClientWrapper<GetResult<List<CounterDetailDTO>>>.GetItemRequest(string.Format("api/Dashboard/GetCounterDetails?cultureName={0}&Id={1}", SessionInfo.CultureShortName, 1)).Result;

            var counterVM = CounterDetailMapper.Map_New(counterDetailDTOs.Result);
            if (counterVM != null)
            {
                ViewData["TransactionCounter"] = counterVM;
            }

            DateTime formattedFromDate;
            DateTime formattedToDate;

            string sHijriYear = DateTimeUtility.GetHijriYear(DateTime.Now).ToString();
            formattedFromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            formattedToDate = DateTime.Now;


            DashboardFilterCriteria dashboardFilterCriteria = new DashboardFilterCriteria()
            {
                level = level,
                userId = SessionInfo.CurrentUser.Id,
                entityId = SessionInfo.OrgUnitId,
                toDate = formattedToDate.ToString(),
                fromDate = formattedFromDate.ToString()
            };

            GetResult<DashboardHomeReportDTO> dashboardHomeDTO =
                HttpClientWrapper<GetResult<DashboardHomeReportDTO>>.PostRequest("api/Dashboard/GetDashboardHomeReport", dashboardFilterCriteria).Result;


            DashboardHomeVM dashboardHomeVM = DashboardHomeMapper.Map(dashboardHomeDTO.Result);
            dashboardHomeVM.OrgUnitId = SessionInfo.OrgUnitId;
            dashboardHomeVM.FromDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(formattedFromDate);
            dashboardHomeVM.ToDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(formattedToDate);

            dashboardHomeVM.DirectedToId = SessionInfo.CurrentUser.Id;
            dashboardHomeVM.DirectedToOrgUnitId = SessionInfo.OrgUnitId;

            int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
            int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
            int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
            //int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);

            if (dashboardHomeVM.DashboardReportBottomList != null && dashboardHomeVM.DashboardReportBottomList.Any())
            {
                dashboardHomeVM.transactionTypesReport = new List<TransactionTypesReport>();
                dashboardHomeVM.transactionConfidentialityReports = new List<TransactionConfidentialityReport>();

                var totalYears = dashboardHomeVM.DashboardReportBottomList.Select(x => x.YEAR).Distinct().ToList();
                foreach (var year in totalYears)
                {
                    var totalInbound = 0;
                    var totalInternal = 0;
                    var totalExternal = 0;

                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == Inbound && x.ReportType == 0 && x.YEAR == year))
                    {
                        totalInbound = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == Inbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == InternalOutbound && x.ReportType == 0 && x.YEAR == year))
                    {
                        totalInternal = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == InternalOutbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == ExternalOutbound && x.ReportType == 0 && x.YEAR == year))
                    {
                        totalExternal = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == ExternalOutbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
                    }


                    dashboardHomeVM.transactionTypesReport.Add(new TransactionTypesReport
                    {
                        year = year.ToString(),
                        external = totalExternal.ToString(),
                        inbound = totalInbound.ToString(),
                        internalv = totalExternal.ToString()
                    });


                    var TotalNormal_27 = 0;
                    var TotalSecret_28 = 0;
                    var TotalVerySecret_29 = 0;
                    var TotalByHand_121 = 0;

                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 27 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalNormal_27 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 27 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 28 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalSecret_28 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 28 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 29 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalVerySecret_29 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 29 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 121 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalByHand_121 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 121 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }

                    dashboardHomeVM.transactionConfidentialityReports.Add(new TransactionConfidentialityReport
                    {
                        year = year.ToString(),
                        byHand = TotalByHand_121.ToString(),
                        normal = TotalNormal_27.ToString(),
                        secret = TotalSecret_28.ToString(),
                        verysecret = TotalVerySecret_29.ToString()
                    });

                }

            }


            GetResult<SettingDTO> Setting = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.DateAndNumbersSettings.DateType)).Result;
            var Language = SettingMapper.Map(Setting.Result);
            var adminDate = GetDateLookups(LookupCategory.DateType);
            if (Language.Value == adminDate.LastOrDefault().Value)
            {
                dashboardHomeVM.DateFormateSetting = "Gregorian";
            }
            else
            {
                dashboardHomeVM.DateFormateSetting = "Ummalqura";
            }
            //return Json(new
            //{
            //    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_DashboardHomeCards", dashboardHomeVM)
            //}, JsonRequestBehavior.AllowGet);

            return View("~/Areas/User/Views/Shared/DashboardHome.cshtml", dashboardHomeVM);
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
                if (userProfileDTOs.Result != null)
                {
                    foreach (MCS.UI.Areas.User.Models.UserManagement.UserProfileVM userProfileVM in MCS.UI.Areas.User.Mappers.UserManagement.UserProfileMapper.Map(userProfileDTOs.Result))
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
                        Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Select")
                    }).ToList();
                }

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Dashboards.Dashboard)]
        public ActionResult DashboardSearch(DashboardFilterCriteria dashboardFilterCriteria)
        {

            GetResult<List<CounterDetailDTO>> counterDetailDTOs =
                      HttpClientWrapper<GetResult<List<CounterDetailDTO>>>.GetItemRequest(string.Format("api/Dashboard/GetCounterDetails?cultureName={0}&Id={1}", SessionInfo.CultureShortName, 1)).Result;

            var counterVM = CounterDetailMapper.Map_New(counterDetailDTOs.Result);
            if (counterVM != null)
            {
                ViewData["TransactionCounter"] = counterVM;
            }
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
            {
                level = 2;
            }

            ViewBag.Level = level;
            string formattedFromDate;
            string formattedToDate;

            string sHijriYear = DateTimeUtility.GetHijriYear(DateTime.Now).ToString();
            formattedFromDate = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            formattedToDate = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            dashboardFilterCriteria.fromDate = DateTimeUtility.HijriToGreg(dashboardFilterCriteria.fromDate).ToString();
            dashboardFilterCriteria.toDate = DateTimeUtility.HijriToGreg(dashboardFilterCriteria.toDate).ToString();
            dashboardFilterCriteria.level = level;
            GetResult<DashboardHomeReportDTO> dashboardHomeDTO =
                HttpClientWrapper<GetResult<DashboardHomeReportDTO>>.PostRequest("api/Dashboard/GetDashboardHomeReport", dashboardFilterCriteria).Result;


            DashboardHomeVM dashboardHomeVM = DashboardHomeMapper.Map(dashboardHomeDTO.Result);

            int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
            int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
            int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
            //int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);

            if (dashboardHomeVM.DashboardReportBottomList != null && dashboardHomeVM.DashboardReportBottomList.Any())
            {
                dashboardHomeVM.transactionTypesReport = new List<TransactionTypesReport>();
                dashboardHomeVM.transactionConfidentialityReports = new List<TransactionConfidentialityReport>();

                var totalYears = dashboardHomeVM.DashboardReportBottomList.Select(x => x.YEAR).Distinct().ToList();
                foreach (var year in totalYears)
                {
                    var totalInbound = 0;
                    var totalInternal = 0;
                    var totalExternal = 0;

                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == Inbound && x.ReportType == 0 && x.YEAR == year))
                    {
                        totalInbound = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == Inbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == InternalOutbound && x.ReportType == 0 && x.YEAR == year))
                    {
                        totalInternal = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == InternalOutbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == ExternalOutbound && x.ReportType == 0 && x.YEAR == year))
                    {
                        totalExternal = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == ExternalOutbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
                    }


                    dashboardHomeVM.transactionTypesReport.Add(new TransactionTypesReport
                    {
                        year = year.ToString(),
                        external = totalExternal.ToString(),
                        inbound = totalInbound.ToString(),
                        internalv = totalExternal.ToString()
                    });


                    var TotalNormal_27 = 0;
                    var TotalSecret_28 = 0;
                    var TotalVerySecret_29 = 0;
                    var TotalByHand_121 = 0;

                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 27 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalNormal_27 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 27 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 28 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalSecret_28 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 28 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 29 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalVerySecret_29 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 29 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }
                    if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 121 && x.ReportType == 1 && x.YEAR == year))
                    {
                        TotalByHand_121 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 121 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
                    }

                    dashboardHomeVM.transactionConfidentialityReports.Add(new TransactionConfidentialityReport
                    {
                        year = year.ToString(),
                        byHand = TotalByHand_121.ToString(),
                        normal = TotalNormal_27.ToString(),
                        secret = TotalSecret_28.ToString(),
                        verysecret = TotalVerySecret_29.ToString()
                    });

                }

            }


            GetResult<SettingDTO> Setting = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.DateAndNumbersSettings.DateType)).Result;
            var Language = SettingMapper.Map(Setting.Result);
            var adminDate = GetDateLookups(LookupCategory.DateType);
            if (Language.Value == adminDate.LastOrDefault().Value)
            {
                dashboardHomeVM.DateFormateSetting = "Gregorian";
            }
            else
            {
                dashboardHomeVM.DateFormateSetting = "Ummalqura";
            }
            return Json(new
            {
                MessageType = MessageType.Information,
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_DashboardHomeCards.cshtml", dashboardHomeVM)
            }, JsonRequestBehavior.AllowGet);

            //return RedirectToAction("DashboardHome");
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken()]
        //[CustomAuthorizationAttribute(UserClaims.Dashboards.Dashboard)]
        //public ActionResult DashboardSearch(string FromDate, string ToDate, int? DirectedToOrgUnitId, int? DirectedToId)
        //{
        //    int level = 1;
        //    if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
        //    {
        //        level = 4;
        //    }
        //    else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
        //    {
        //        level = 3;
        //    }
        //    else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
        //    {
        //        level = 2;
        //    }

        //    ViewBag.Level = level;
        //    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
        //        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

        //    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
        //    ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId, true);


        //    DateTime? formattedFromDate = null;
        //    DateTime? formattedToDate = null;

        //    if (string.IsNullOrEmpty(FromDate) == false)
        //    {
        //        string[] fromDateArr = FromDate.Split('/');

        //        if (fromDateArr[0].Length == 1)
        //            fromDateArr[0] = "0" + fromDateArr[0];

        //        if (fromDateArr[1].Length == 1)
        //            fromDateArr[1] = "0" + fromDateArr[1];

        //        FromDate = string.Join("/", fromDateArr);

        //        formattedFromDate = DateTimeUtility.ConvertToDate(FromDate.Trim());
        //    }

        //    if (string.IsNullOrEmpty(ToDate) == false)
        //    {
        //        string[] toDateArr = ToDate.Split('/');

        //        if (toDateArr[0].Length == 1)
        //            toDateArr[0] = "0" + toDateArr[0];

        //        if (toDateArr[1].Length == 1)
        //            toDateArr[1] = "0" + toDateArr[1];

        //        ToDate = string.Join("/", toDateArr);

        //        formattedToDate = DateTimeUtility.ConvertToDate(ToDate.Trim());
        //    }
        //    if (!DirectedToOrgUnitId.HasValue)
        //        DirectedToOrgUnitId = 0;

        //       DashboardFilterCriteria dashboardFilterCriteria = new DashboardFilterCriteria()
        //    {
        //        level = level,
        //        //userId = (DirectedToId.HasValue == false || level == 1 || level == 2) ? SessionInfo.CurrentUser.Id : DirectedToId.Value,
        //        entityId = (level == 1 || level == 2 || DirectedToOrgUnitId.Value <= 0) ? SessionInfo.OrgUnitId : DirectedToOrgUnitId.Value,
        //        toDate = formattedToDate.HasValue ? formattedToDate.ToString() : "",
        //        fromDate = formattedFromDate.HasValue ? formattedFromDate.ToString() : "",
        //    };

        //    if (level == 1 || level == 2)
        //    {
        //        dashboardFilterCriteria.userId = SessionInfo.CurrentUser.Id;
        //    }
        //    else if (DirectedToId.HasValue == false)
        //    {
        //        dashboardFilterCriteria.userId = 0;
        //    }
        //    else
        //    {
        //        dashboardFilterCriteria.userId = DirectedToId.Value;
        //    }

        //    GetResult<DashboardHomeReportDTO> dashboardHomeDTO =
        //        HttpClientWrapper<GetResult<DashboardHomeReportDTO>>.PostRequest("api/Dashboard/GetDashboardHomeReport", dashboardFilterCriteria).Result;


        //    DashboardHomeVM dashboardHomeVM = DashboardHomeMapper.Map(dashboardHomeDTO.Result);
        //    dashboardHomeVM.OrgUnitId = dashboardFilterCriteria.entityId;
        //    dashboardHomeVM.DirectedToOrgUnitId = dashboardFilterCriteria.entityId;
        //    dashboardHomeVM.DirectedToId = dashboardFilterCriteria.userId;
        //    dashboardHomeVM.FromDate = formattedFromDate;
        //    dashboardHomeVM.ToDate = formattedToDate;


        //    int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
        //    int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
        //    int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);

        //    if (dashboardHomeVM.DashboardReportBottomList != null && dashboardHomeVM.DashboardReportBottomList.Any())
        //    {
        //        dashboardHomeVM.transactionTypesReport = new List<TransactionTypesReport>();
        //        dashboardHomeVM.transactionConfidentialityReports = new List<TransactionConfidentialityReport>();

        //        var totalYears = dashboardHomeVM.DashboardReportBottomList.Select(x => x.YEAR).Distinct().ToList();
        //        foreach (var year in totalYears)
        //        {
        //            var totalInbound = 0;
        //            var totalInternal = 0;
        //            var totalExternal = 0;

        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == Inbound && x.ReportType == 0 && x.YEAR == year))
        //            {
        //                totalInbound = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == Inbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }
        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == InternalOutbound && x.ReportType == 0 && x.YEAR == year))
        //            {
        //                totalInternal = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == InternalOutbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }
        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == ExternalOutbound && x.ReportType == 0 && x.YEAR == year))
        //            {
        //                totalExternal = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == ExternalOutbound && x.ReportType == 0 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }


        //            dashboardHomeVM.transactionTypesReport.Add(new TransactionTypesReport
        //            {
        //                year = year.ToString(),
        //                external = totalExternal.ToString(),
        //                inbound = totalInbound.ToString(),
        //                internalv = totalInternal.ToString()
        //            });


        //            var TotalNormal_27 = 0;
        //            var TotalSecret_28 = 0;
        //            var TotalVerySecret_29 = 0;
        //            var TotalByHand_121 = 0;

        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 27 && x.ReportType == 1 && x.YEAR == year))
        //            {
        //                TotalNormal_27 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 27 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }
        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 28 && x.ReportType == 1 && x.YEAR == year))
        //            {
        //                TotalSecret_28 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 28 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }
        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 29 && x.ReportType == 1 && x.YEAR == year))
        //            {
        //                TotalVerySecret_29 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 29 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }
        //            if (dashboardHomeVM.DashboardReportBottomList.Any(x => x.TypeId == 121 && x.ReportType == 1 && x.YEAR == year))
        //            {
        //                TotalByHand_121 = dashboardHomeVM.DashboardReportBottomList.Where(x => x.TypeId == 121 && x.ReportType == 1 && x.YEAR == year).Sum(x => x.TotalCount);
        //            }

        //            dashboardHomeVM.transactionConfidentialityReports.Add(new TransactionConfidentialityReport
        //            {
        //                year = year.ToString(),
        //                byHand = TotalByHand_121.ToString(),
        //                normal = TotalNormal_27.ToString(),
        //                secret = TotalSecret_28.ToString(),
        //                verysecret = TotalVerySecret_29.ToString()
        //            });

        //        }

        //    }

        //    //GetResult<SettingDTO> Setting = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.DateAndNumbersSettings.DateType)).Result;
        //    //var Language = SettingMapper.Map(Setting.Result);
        //    //var adminDate = GetDateLookups(LookupCategory.DateType);
        //    dashboardHomeVM.DateFormateSetting = "Gregorian";

        //    return View("~/Areas/User/Views/Shared/DashboardHome.cshtml", dashboardHomeVM);
        //}
        public ActionResult DashboardDetails(string FromDate, string ToDate, int ItemId, int? page, int DepartmentId = -1, bool? isPrinting = false)

        {

            int rowCount = UIHelper.PageSize;
            if (isPrinting.HasValue && isPrinting.Value)
            {
                rowCount = 100000;
            }
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
            {
                level = 2;
            }

            int PageIndex = page.HasValue ? page.Value - 1 : 0;

            DateTime formattedFromDate;
            DateTime formattedToDate;
            if (FromDate == null)
            {
                string sHijriYear = DateTimeUtility.GetHijriYear(DateTime.Now).ToString();
                formattedFromDate = DateTime.Now;
            }
            else
            {
                formattedFromDate = Convert.ToDateTime(FromDate);
            }

            if (ToDate == null)
            {
                formattedToDate = DateTime.Now;
            }
            else
            {
                formattedToDate = Convert.ToDateTime(ToDate);
            }

            DashboardFilterCriteria dashboardFilterCriteria = new DashboardFilterCriteria()
            {
                level = level,
                userId = SessionInfo.CurrentUser.Id,
                entityId = (level == 1 || level == 2) ? SessionInfo.OrgUnitId : DepartmentId,
                toDate = formattedToDate.ToString(),
                fromDate = formattedFromDate.ToString(),
                itemId = ItemId,
                cultureId = SessionInfo.CultureShortName,
                pageIndex = PageIndex,
                pageSize = rowCount
            };

            GetResult<List<TransactionDetailsDTO>> transactionDTOList =
                HttpClientWrapper<GetResult<List<TransactionDetailsDTO>>>.PostRequest("api/Dashboard/GetDashboardDetails", dashboardFilterCriteria).Result;

            List<TransactionDetailsVM> TransactionDetailsVMList = TransactionDetailsMapper.Map(transactionDTOList.Result);

            foreach (var item in TransactionDetailsVMList)
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
            }

            var transactionDetails = (CustomGridMvc.AjaxGrid<TransactionDetailsVM>)new CustomGridMvc.AjaxGridFactory()
                .CreateAjaxGrid(TransactionDetailsVMList, 1, transactionDTOList.RowsCount.Value, page.HasValue, rowCount);

            if (page.HasValue)
            {
                return Json(new { Html = transactionDetails.ToJson("~/Areas/User/Views/Shared/_DashboardDetailsGrid.cshtml", this), transactionDetails.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (isPrinting.HasValue && isPrinting.Value)
            {

                var httpValue = SystemConfigurations.EnableSSL ? "https://" : "http://";
                var url = string.Format("{0}{1}", httpValue, HttpContext.Request.Url.Host);

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/DashboardDetails.cshtml", transactionDetails);
                Html = Html.Replace("href=\"", "href=\"" + url);
                Html = Html.Replace("<button class=\"btn btn-st1 btn-lg\" onclick=\"ShowPrintDialog()\">طباعة</button>", "");
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "Dashboard Details.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("~/Areas/User/Views/Shared/DashboardDetails.cshtml", transactionDetails);
            }
        }
        public ActionResult DashboardDetailsPerformanceMeasurement(string From, string To, int? page, int DepartmentId = -1, bool? isPrinting = false)
        {

            int rowCount = UIHelper.PageSize;
            if (isPrinting.HasValue && isPrinting.Value)
            {
                rowCount = 100000;
            }
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
            {
                level = 2;
            }

            int PageIndex = page.HasValue ? page.Value - 1 : 0;

            DateTime formattedFromDate;
            DateTime formattedToDate;
            if (From == null)
            {
                string sHijriYear = DateTimeUtility.GetHijriYear(DateTime.Now).ToString();
                formattedFromDate = DateTime.Now;
            }
            else
            {
                formattedFromDate = DateTimeUtility.ConvertToDate(From.Trim());
            }

            if (To == null)
            {
                formattedToDate = DateTime.Now;
            }
            else
            {
                formattedToDate = DateTimeUtility.ConvertToDate(To.Trim());
            }

            DashboardFilterCriteria dashboardFilterCriteria = new DashboardFilterCriteria()
            {
                level = level,
                userId = SessionInfo.CurrentUser.Id,
                entityId = (level == 1 || level == 2) ? SessionInfo.OrgUnitId : DepartmentId,
                toDate = formattedToDate.ToString(),
                fromDate = formattedFromDate.ToString(),
                itemId = 8,
                cultureId = SessionInfo.CultureShortName,
                pageIndex = PageIndex,
                pageSize = rowCount
            };

            GetResult<List<TransactionDetailsDTO>> transactionDTOList =
                HttpClientWrapper<GetResult<List<TransactionDetailsDTO>>>.PostRequest("api/Dashboard/GetDashboardDetails", dashboardFilterCriteria).Result;

            List<TransactionDetailsVM> TransactionDetailsVMList = TransactionDetailsMapper.Map(transactionDTOList.Result);

            var transactionDetails = (CustomGridMvc.AjaxGrid<TransactionDetailsVM>)new CustomGridMvc.AjaxGridFactory()
                .CreateAjaxGrid(TransactionDetailsVMList, 1, transactionDTOList.RowsCount.Value, page.HasValue, rowCount);

            if (page.HasValue)
            {
                return Json(new { Html = transactionDetails.ToJson("~/Areas/User/Views/Shared/_PerformanceMeasurementDetailsGrid.cshtml", this), transactionDetails.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (isPrinting.HasValue && isPrinting.Value)
            {

                var httpValue = SystemConfigurations.EnableSSL ? "https://" : "http://";
                var url = string.Format("{0}{1}", httpValue, HttpContext.Request.Url.Host);

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PerformanceMeasurementDetails.cshtml", transactionDetails);
                Html = Html.Replace("href=\"", "href=\"" + url);
                Html = Html.Replace("<button class=\"btn btn-st1 btn-lg\" onclick=\"ShowPrintDialog()\">طباعة</button>", "");
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "Dashboard Details.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("~/Areas/User/Views/Shared/_PerformanceMeasurementDetails.cshtml", transactionDetails);
            }
        }

        #endregion

        #region Support
        [HttpGet]
        public ActionResult Support()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            IList<AutoCompleteDataSource> SubProblem = new List<AutoCompleteDataSource>();

            IList<AutoCompleteDataSource> SubInquery = new List<AutoCompleteDataSource>();

            var supportTypes = LookupsHelper.GetLookupItems(LookupCategory.SupportType, SessionInfo.CultureShortName).Result.ToList();

            foreach (var supportType in supportTypes)
            {
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = supportType.Id.ToString(),
                    Label = supportType.Text
                });
            }

            var errorTypes = LookupsHelper.GetLookupItems(LookupCategory.ProblemSupportType, SessionInfo.CultureShortName).Result.ToList();

            foreach (var errorType in errorTypes)
            {
                SubProblem.Add(new AutoCompleteDataSource()
                {
                    Value = errorType.Id.ToString(),
                    Label = errorType.Text
                });
            }

            //
            var SubInquerys = LookupsHelper.GetLookupItems(LookupCategory.InquirySupportType, SessionInfo.CultureShortName).Result.ToList();

            foreach (var errorType in SubInquerys)
            {
                SubInquery.Add(new AutoCompleteDataSource()
                {
                    Value = errorType.Id.ToString(),
                    Label = errorType.Text
                });
            }

            ViewData["MainDDL"] = JsonConvert.SerializeObject(dataSource);
            ViewData["SubProblem"] = JsonConvert.SerializeObject(SubProblem);
            ViewData["SubInquery"] = JsonConvert.SerializeObject(SubInquery);



            return PartialView("~/Areas/User/Views/Shared/_SupportPatial.cshtml");
        }

        [HttpPost]
        public ActionResult ReceiveSupport(SupportVM request)
        {
            string message = string.Empty;
            GetResult<SettingDTO> SystemConfigurationsSupportEmail =
                               HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.GeneralSettings.SupportEmail)).Result;

            var settingVM = SettingMapper.Map(SystemConfigurationsSupportEmail.Result);
            var supportDTO = new SupportDTO
            {
                Subject = request.Subject,
                Category = request.Category,
                Description = request.Description,
                SupportType = request.SupportType,
                ToEmail = settingVM.Value
            };

            supportDTO.Description = SessionInfo.CurrentUser.Email + "/" + SessionInfo.CurrentUser.Name + "/" + SessionInfo.CurrentUser.UserName + " " + supportDTO.Description;

            IList<NotificationAttachmentDTO> mailAttachments = null;

            if (request.Files.Count > 0 & request.Files[0] != null)
            {
                //if (request.Attachments[0] == null)
                //{
                //    return Json(new
                //    {
                //        MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Support.UploadAttachment"),
                //        MessageType = MessageType.Error
                //    }, JsonRequestBehavior.AllowGet);
                //}

                mailAttachments = new List<NotificationAttachmentDTO>();
                foreach (var notificationAttachment in request.Files)
                {
                    byte[] file = new byte[notificationAttachment.ContentLength];
                    notificationAttachment.InputStream.Read(file, 0, file.Length);
                    var notificationAttachmentDTO = new NotificationAttachmentDTO()
                    {
                        Binary = file,
                        ContentLength = notificationAttachment.ContentLength,
                        ContentType = notificationAttachment.ContentType,
                        FileName = notificationAttachment.FileName
                    };

                    mailAttachments.Add(notificationAttachmentDTO);
                }
                supportDTO.NotificationAttachmentDTOS = mailAttachments.ToList();
            }


            var postResult = HttpClientWrapper<GetResult<object>>.PostRequest(string.Format("api/Common/SaveNotification"), supportDTO).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Inbound.SaveSucceeded");
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult HelpInfo(string ControllerName, string ActionName)
        {
            HelpInfoVM oHelpInfoVM = new HelpInfoVM();

            if (ControllerName.Equals("File"))
            {
                if (ActionName.Equals("Saved"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Saved_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Saved_Body").ToString();
                }
                else if (ActionName.Equals("Manager"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Manager_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Manager_Body").ToString();
                }
                else if (ActionName.Equals("MyTransactions"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_MyTransaction_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_MyTransaction_Body").ToString();
                }
                else if (ActionName.Equals("SentTransactions"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_SentTransactions_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_SentTransactions_Body").ToString();
                }
                else if (ActionName.Equals("Copies"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Copies_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Copies_Body").ToString();
                }
                else if (ActionName.Equals("DraftOutbound"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_DraftOutbound_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_DraftOutbound_Body").ToString();
                }
                else if (ActionName.Equals("OrgUnit"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_OrgUnit_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_OrgUnit_Body").ToString();
                }
                else if (ActionName.Equals("Tasks"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Tasks_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Tasks_Body").ToString();
                }
                else if (ActionName.Equals("FollowUp"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_FollowUp_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_FollowUp_Body").ToString();
                }
                else if (ActionName.Equals("Reservation"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Reservation_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "File_Reservation_Body").ToString();
                }
            }
            else if (ControllerName.Equals("OutboundExternal"))
            {
                if (ActionName.Equals("Add") || ActionName.Equals("Edit"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundExternal_Add_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundExternal_Add_Body").ToString();
                }
                else if (ActionName.Equals("Edit"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundExternal_Edit_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundExternal_Edit_Body").ToString();
                }
                else if (ActionName.Equals("CreateOutbound"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundExternal_CreateOutbound_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundExternal_CreateOutbound_Body").ToString();
                }
            }
            else if (ControllerName.Equals("Inbound"))
            {
                if (ActionName.Equals("Add"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Inbound_Add_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Inbound_Add_Body").ToString();
                }
                else if (ActionName.Equals("Edit"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Inbound_Edit_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Inbound_Edit_Body").ToString();
                }
            }
            else if (ControllerName.Equals("OutboundInternal"))
            {
                if (ActionName.Equals("Add"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundInternal_Add_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundInternal_Add_Body").ToString();
                }
                else if (ActionName.Equals("Edit"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundInternal_Edit_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundInternal_Edit_Body").ToString();
                }
                else if (ActionName.Equals("CreateCopyOutboundInternal"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundInternal_CreateCopyOutboundInternal_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "OutboundInternal_CreateCopyOutboundInternal_Body").ToString();
                }

            }
            else if (ControllerName.Equals("Hub"))
            {
                if (ActionName.Equals("GetAll"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Hub_GetAll_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Hub_GetAll_Body").ToString();
                }
            }
            else if (ControllerName.Equals("Search"))
            {
                if (ActionName.Equals("Index"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Search_Index_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Search_Index_Body").ToString();
                }
            }
            else if (ControllerName.Equals("Reports"))
            {
                if (ActionName.Equals("TransactionsDeliveryReport"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Reports_TransactionsDeliveryReport_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Reports_TransactionsDeliveryReport_Body").ToString();
                }
                else if (ActionName.Equals("TransactionReport"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Reports_TransactionReport_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Reports_TransactionReport_Body").ToString();
                }
                else if (ActionName.Equals("PerformanceMeasurementReport"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Reports_PerformanceMeasurementReport_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Reports_PerformanceMeasurementReport_Body").ToString();
                }
            }
            else if (ControllerName.Equals("Shared"))
            {
                if (ActionName.Equals("Support"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Shared_Support_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Shared_Support_Body").ToString();
                }
                else if (ActionName.Equals("DashboardHome"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Shared_DashboardHome_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Shared_DashboardHome_Body").ToString();
                }
            }
            else if (ControllerName.Equals("UserPreferences"))
            {
                if (ActionName.Equals("Index"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_Index_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_Index_Body").ToString();
                }
                else if (ActionName.Equals("AssignmentPaperSettings"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_AssignmentPaperSettings_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_AssignmentPaperSettings_Body").ToString();
                }
                else if (ActionName.Equals("DistributionList"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_DistributionList_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_DistributionList_Body").ToString();
                }
                else if (ActionName.Equals("UserDelegations"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_UserDelegations_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_UserDelegations_Body").ToString();
                }
                else if (ActionName.Equals("TransactionPath"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_TransactionPath_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "UserPreferences_TransactionPath_Body").ToString();
                }
            }
            else if (ControllerName.Equals("Home"))
            {
                if (ActionName.Equals("AllNotification"))
                {
                    oHelpInfoVM.Title = HttpContext.GetGlobalResourceObject("ResourceHelp", "Home_AllNotification_Title").ToString();
                    oHelpInfoVM.Body = HttpContext.GetGlobalResourceObject("ResourceHelp", "Home_AllNotification_Body").ToString();
                }
            }

            return PartialView("~/Areas/User/Views/Shared/HelpInfo.cshtml", oHelpInfoVM);
        }
        #endregion

        [HttpGet]
        public ActionResult GetUserImage(int UserImageId)
        {
            GetResult<DocumentDTO> documentDTO =
                           HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(String.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, UserImageId)).Result;

            var documentVM = DocumentMapper.Map(documentDTO.Result);

            byte[] userImage = documentVM.Content;

            if (userImage != null)
                return new FileContentResult(userImage, "image/jpeg");

            return null;
        }

        [HttpGet]
        public ActionResult GetExplanationById(int id)
        {
            try
            {
                GetResult<ExplanationDTO> explanationDTO = HttpClientWrapper<GetResult<ExplanationDTO>>.GetItemRequest(string.Format("api/Transaction/GetExplanationById?cultureName={0}&explanationId={1}", SessionInfo.CultureShortName, id)).Result;
                byte[] content = ExplanationMapper.Map(explanationDTO.Result).DocumentVM.Content;

                if (content != null)
                {
                    switch (explanationDTO.Result.EditorType)
                    {
                        case EditorType.TextEditor:
                            {
                                return new FileContentResult(PdfHelper.ConvertHtml2PDFExp(System.Text.Encoding.Default.GetString(content)), "application/pdf");
                            }
                        case EditorType.Text:
                            {
                                return new FileContentResult(PdfHelper.ConvertHtml2PDFExp(System.Text.Encoding.Unicode.GetString(content)), "application/pdf");

                            }
                        case EditorType.Scanning:
                            {
                                return new FileContentResult(content, "application/pdf");
                            }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult PrintAllExplanations(int transactionId)
        {
            ViewData["TransactionId"] = transactionId;
            //return new FileContentResult(mergedDocument, "application/pdf");
            return View("~/Areas/User/Views/Editor/Explanations/_ExplanationPrintAllDialog.cshtml");
        }

        [HttpGet]
        public ActionResult GetExplanationPDFResult(int id)
        {
            try
            {

                GetResult<List<ExplanationDTO>> explanationDTOs =
                    HttpClientWrapper<GetResult<List<ExplanationDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionExplanations?transactionId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                GetResult<TransactionPrintDTO> transactionPrintDTO =
                    HttpClientWrapper<GetResult<TransactionPrintDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetAllTransactionDocuments?transactionId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                List<ExplanationVM> explanationVMs = ExplanationMapper.Map(explanationDTOs.Result);
                CustomGridMvc.IAjaxGrid Explanations = (CustomGridMvc.AjaxGrid<ExplanationVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(explanationVMs, 1, explanationVMs.Count(), false);

                ViewData["ExplanationsDataObject"] = explanationVMs;

                ViewData["ExplanationsData"] = Explanations;

                List<byte[]> contactPdf = new List<byte[]>();


                GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
                    HttpClientWrapper<GetResult<TransactionBarcodesDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}&isElectronic={3}", SessionInfo.CultureShortName, id, SessionInfo.OrgUnitId, true)).Result;

                TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);

                foreach (BarcodeVM barcodeVM in transactionBarcodesVM.BarcodeVMs)
                {
                    string barcode2D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, 45, 160);
                    ViewData["barcode2D"] = barcode2D;
                }

                foreach (var item in explanationVMs)
                {
                    var _html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Explanations/_ExplanationPrintData.cshtml", item);
                    var pdf = PdfHelper.ConvertHtml2PDF(_html);
                    contactPdf.Add(pdf);



                    GetResult<ExplanationDTO> explanationDTO = HttpClientWrapper<GetResult<ExplanationDTO>>
                        .GetItemRequest(string.Format("api/Transaction/GetExplanationById?cultureName={0}&explanationId={1}", SessionInfo.CultureShortName, item.Id)).Result;
                    byte[] content = ExplanationMapper.Map(explanationDTO.Result).DocumentVM.Content;




                    if (content != null)
                    {


                        switch (explanationDTO.Result.EditorType)
                        {
                            case EditorType.TextEditor:
                                {
                                    contactPdf.Add(PdfHelper.ConvertHtml2PDF(System.Text.Encoding.Default.GetString(content)));
                                    break;
                                }
                            case EditorType.Text:
                                {
                                    contactPdf.Add(PdfHelper.ConvertHtml2PDF(System.Text.Encoding.Unicode.GetString(content)));
                                    break;
                                }
                            case EditorType.Scanning:
                                {
                                    contactPdf.Add(content);
                                    break;
                                }

                        }
                    }
                }
                byte[] mergedDocument = PdfHelper.ConcatenateAndAddContent(contactPdf);
                return new FileContentResult(mergedDocument, "application/pdf");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public byte[] GetExplanationByteById(int id)
        {
            try
            {
                GetResult<ExplanationDTO> explanationDTO = HttpClientWrapper<GetResult<ExplanationDTO>>.GetItemRequest(string.Format("api/Transaction/GetExplanationById?cultureName={0}&explanationId={1}", SessionInfo.CultureShortName, id)).Result;
                byte[] content = ExplanationMapper.Map(explanationDTO.Result).DocumentVM.Content;

                if (content != null)
                {
                    switch (explanationDTO.Result.EditorType)
                    {
                        case EditorType.TextEditor:
                            {
                                return PdfHelper.ConvertHtml2PDF(System.Text.Encoding.Default.GetString(content));

                            }
                        case EditorType.Text:
                            {
                                return PdfHelper.ConvertHtml2PDF(System.Text.Encoding.Unicode.GetString(content));



                            }
                        case EditorType.Scanning:
                            {
                                return content;
                            }
                    }
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpGet]
        public ActionResult GetAttachmentById(int id)
        {
            try
            {
                GetResult<DocumentDTO> documentDTO =
                            HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(String.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, id)).Result;
                var documentVM = UserDocuments.DocumentMapper.Map(documentDTO.Result);

                return new FileContentResult(documentVM.Content, "application/pdf");
            }
            catch (Exception)
            {
                throw;
            }

        }


        public byte[] GetAttachmentByteById(int id)
        {
            try
            {
                GetResult<DocumentDTO> documentDTO =
                            HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(String.Format("api/Document/GetDocumentById?cultureName={0}&documentId={1}", SessionInfo.CultureShortName, id)).Result;
                var documentVM = UserDocuments.DocumentMapper.Map(documentDTO.Result);

                return documentVM.Content;
            }
            catch (Exception)
            {
                throw;
            }

        }



        [HttpGet]
        public ActionResult GetGetMainDocument(byte[] content)
        {
            try
            {

                if (content != null)
                {
                    return new FileContentResult(content, "application/pdf");
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult Systems()
        {
            return View("~/Areas/User/Views/Shared/Systems.cshtml");
        }






        public ActionResult RulesAndRegulations()
        {

            //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(new List<TransactionAttachmentVM>());
            //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(new List<TransactionNameVM>());
            //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(new List<TransactionLinkVM>());
            //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
            //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(new List<AutoCompleteDataSource>());
            ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
            ViewData["OrgUnitsUsersData"] = null;
            ViewData["DocumentId"] = null;

            Session["DocumentData"] = null;
            return View("~/Areas/User/Views/Shared/_RulesAndRegulations.cshtml");
        }
        [HttpPost]
        public ActionResult AddRulesAndRegulations(RulesAndRegulations rulesAndRegulations)
        {
            return View("~/Areas/User/Views/Shared/_RulesAndRegulations.cshtml");
        }



        //  [ValidateAntiForgeryToken()]
        public ActionResult ArchivesLibrary()
        {
            ViewData["LetterTypeData"] = GetLetterTypes(TransactionCategory.InternalOutbound);
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
            return View("~/Areas/User/Views/Shared/_ArchivesLibrary.cshtml");
        }
        [HttpPost]
        public ActionResult AddArchivesLibrary(ArchivesLibrary archivesLibrary, string hdnMainDocToken)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/AddArchivesLibrary", ArchivesLibraryMapper.Map(archivesLibrary)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.AddSucceeded");
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #region Org Internal Hierarchy tree actions

        [HttpGet]
        public ActionResult GetInternalPartyChildren(OrgHierarchyTreeViewModel treeVM)
        {
            try
            {
                OrgHierarchyTreeViewModel treeViewModel = new OrgHierarchyTreeViewModel();
                if (TempData.Peek("isAssignmentView") != null && bool.Parse(TempData.Peek("isAssignmentView").ToString()) && !SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
                {

                    treeVM.UserId = SessionInfo.CurrentUser.Id;
                    List<OrgUnitDTO> orgUnitsVM = new List<OrgUnitDTO>();

                    var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                        .GetItemRequest(string.Format("api/Common/GetOrgUnits?cultureName={0}&parentId={1}&UserId={2}&orgUnitTreeMode={3}", SessionInfo.CultureShortName, treeVM.SelectedNode, treeVM.UserId, treeVM.OrgUnitTreeMode)).Result;

                    treeViewModel = new OrgHierarchyTreeViewModel()
                    {
                        GetChildrenActionURL = treeVM.GetChildrenActionURL,
                        GetChildrenActionParameters = treeVM.GetChildrenActionParameters,
                        CallBackFunction = treeVM.CallBackFunction,
                        TreeId = treeVM.TreeId,
                        OrgUnitTreeMode = treeVM.OrgUnitTreeMode,
                        Nodes = orgUnitDTOs.Result.Select(x => new OrgHierarchyTreeNodeViewModel()
                        {
                            DepartmentNumber = x.Number,
                            IsSelected = x.IsSelected,
                            IsSelectable = x.IsVirtualUnit ? false : true,
                            Name = x.Name,
                            Id = x.Id,
                            HasChilds = x.HasChilds && !treeVM.UserId.HasValue,
                            IsYesserRegistered = false,
                            ParentId = treeVM.SelectedNode
                        }).Where(o => o.Id == SessionInfo.OrgUnitId).ToList()
                    };

                }

                else
                {
                    List<OrgUnitDTO> orgUnitsVM = new List<OrgUnitDTO>();

                    var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                        .GetItemRequest(string.Format("api/Common/GetOrgUnits?cultureName={0}&parentId={1}&UserId={2}&orgUnitTreeMode={3}", SessionInfo.CultureShortName, treeVM.SelectedNode, treeVM.UserId, treeVM.OrgUnitTreeMode)).Result;

                    treeViewModel = new OrgHierarchyTreeViewModel()
                    {
                        GetChildrenActionURL = treeVM.GetChildrenActionURL,
                        GetChildrenActionParameters = treeVM.GetChildrenActionParameters,
                        CallBackFunction = treeVM.CallBackFunction,
                        TreeId = treeVM.TreeId,
                        OrgUnitTreeMode = treeVM.OrgUnitTreeMode,
                        Nodes = orgUnitDTOs.Result.Select(x => new OrgHierarchyTreeNodeViewModel()
                        {
                            DepartmentNumber = x.Number,
                            IsSelected = x.IsSelected,
                            IsSelectable = x.IsVirtualUnit ? false : true,
                            Name = x.Name,
                            Id = x.Id,
                            HasChilds = x.HasChilds && !treeVM.UserId.HasValue,
                            IsYesserRegistered = false,
                            ParentId = treeVM.SelectedNode
                        }).ToList()
                    };
                }


                if (treeVM.SelectedNode.HasValue)
                {
                    return PartialView("~/Areas/User/Views/Shared/EditorTemplates/OrgHierarchyModalItem.cshtml", treeViewModel);
                }
                else
                {
                    return PartialView("~/Areas/User/Views/Shared/EditorTemplates/OrgHierarchyModal.cshtml", treeViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetInternalPartyInfoById(string partyId)
        {
            GetResult<OrgUnitDTO> orgUnitDTO =
                   HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, partyId)).Result;
            OrgUnitVM orgUnitVM = OrgUnitMapper.Map(orgUnitDTO.Result);

            return new JsonResult() { Data = new OrgHierarchyTreeNodeViewModel() { Id = orgUnitVM.Id, DepartmentNumber = orgUnitVM.Number, Name = orgUnitVM.Name, IsSelectable = orgUnitVM.IsVirtualUnit ? false : true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetInternalPartyInfoByNumber(string partyNumber)
        {
            GetResult<OrgUnitDTO> orgUnitDTO =
                   HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetInternalPartyInfoByNumber?partyNumber={0}&cultureName={1}", partyNumber, SessionInfo.CultureShortName)).Result;
            OrgUnitVM orgUnitVM = OrgUnitMapper.Map(orgUnitDTO.Result);
            return new JsonResult() { Data = new OrgHierarchyTreeNodeViewModel() { Id = orgUnitVM.Id, DepartmentNumber = orgUnitVM.Number, Name = orgUnitVM.Name, IsSelectable = orgUnitVM.IsVirtualUnit ? false : true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        #endregion

        #region Org External Hierarchy tree actions

        [HttpGet]
        public ActionResult GetExternalPartyInfoById(string partyId)
        {


            GetResult<ExternalPartyEditDTO> partyEditDTO =
                   HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", partyId)).Result;

            ExternalPartyEditVM externalPartyListTypeVM = ExternalPartyMapper.Map(partyEditDTO.Result);

            return new JsonResult() { Data = new OrgHierarchyTreeNodeViewModel() { Id = externalPartyListTypeVM.Id, DepartmentNumber = externalPartyListTypeVM.PartyNumber, Name = externalPartyListTypeVM.Name[0].Text, Email = externalPartyListTypeVM.Email, IsYesserRegistered = externalPartyListTypeVM.IsYesserRegistered, IsSelectable = externalPartyListTypeVM.IsVirtual ? false : true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetExternalPartyInfoByNumber(string partyNumber)
        {
            GetResult<ExternalPartyEditDTO> partyEditDTO =
                   HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalPartyInfoByNumber?partyNumber={0}", partyNumber)).Result;

            ExternalPartyEditVM externalPartyListTypeVM = ExternalPartyMapper.Map(partyEditDTO.Result);

            return new JsonResult() { Data = new OrgHierarchyTreeNodeViewModel() { Id = externalPartyListTypeVM.Id, DepartmentNumber = externalPartyListTypeVM.PartyNumber, Name = externalPartyListTypeVM.Name[0].Text, Email = externalPartyListTypeVM.Email, IsYesserRegistered = externalPartyListTypeVM.IsYesserRegistered, IsSelectable = externalPartyListTypeVM.IsVirtual ? false : true }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult GetExternalPartyChildren(OrgHierarchyTreeViewModel treeVM)
        {
            try
            {
                List<ExternalPartyVM> externalPartyVMs = new List<ExternalPartyVM>();

                var externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                    .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, treeVM.SelectedNode)).Result;

                externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);

                OrgHierarchyTreeViewModel treeViewModel = new OrgHierarchyTreeViewModel()
                {
                    GetChildrenActionURL = treeVM.GetChildrenActionURL,
                    GetChildrenActionParameters = treeVM.GetChildrenActionParameters,
                    CallBackFunction = treeVM.CallBackFunction,
                    TreeId = treeVM.TreeId,
                    Nodes = externalPartyVMs.Select(x => new OrgHierarchyTreeNodeViewModel()
                    {
                        DepartmentNumber = x.Number,
                        IsSelected = x.IsSelected,
                        IsSelectable = x.IsVirtual ? false : true,
                        Name = x.LocalName,
                        Id = x.Id,
                        HasChilds = x.HasChilds,
                        IsYesserRegistered = x.YasserRegistered,
                        Email = x.Email,
                        ParentId = treeVM.SelectedNode
                    }).ToList()
                };

                if (treeVM.SelectedNode.HasValue)
                {
                    return PartialView("~/Areas/User/Views/Shared/EditorTemplates/OrgHierarchyModalItem.cshtml", treeViewModel);
                }
                else
                {
                    return PartialView("~/Areas/User/Views/Shared/EditorTemplates/OrgHierarchyModal.cshtml", treeViewModel);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult PrintTransaction(int transactionId, string transactionNumber)
        {
            try
            {
                ViewData["Number"] = transactionNumber;
                GetResult<TransactionPrintDTO> transactionPrintDTO = HttpClientWrapper<GetResult<TransactionPrintDTO>>.GetItemRequest(string.Format("api/Transaction/GetAllTransactionDocuments?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                DocumentVM documentVM = DocumentMapper.Map(transactionPrintDTO.Result.DocumentDTO);
                ViewData["MainDocumentDataObject"] = documentVM;

                List<TransactionAttachmentVM> transactionAttachmentVMs = TransactionAttachmentMapper.Map(transactionPrintDTO.Result.Attachments);
                //CustomGridMvc.IAjaxGrid Attachments = (CustomGridMvc.AjaxGrid<TransactionAttachmentVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionAttachmentVMs, 1, transactionAttachmentVMs.Count(), false);
                ViewData["TransactionAttachmentDataObject"] = transactionAttachmentVMs;

                List<ExplanationVM> explanationVMs = ExplanationMapper.Map(transactionPrintDTO.Result.Explanations);
                //CustomGridMvc.IAjaxGrid Explanations = (CustomGridMvc.AjaxGrid<ExplanationVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(explanationVMs, 1, explanationVMs.Count(), false);
                ViewData["ExplanationsDataObject"] = explanationVMs;



                PrintAllVM printAllVM = new PrintAllVM();


                Spire.Pdf.PdfDocument pdfDocument = new Spire.Pdf.PdfDocument();
                pdfDocument.LoadFromBytes(documentVM.Content);

                System.Drawing.Image[] MainDocumentImages = SaveAsImage(pdfDocument);
                string[] mainDocumentContent = new string[MainDocumentImages.Count()];
                for (int i = 0; i < MainDocumentImages.Count(); i++)
                {
                    mainDocumentContent[i] = ImageToBase64(MainDocumentImages[i]);
                }

                List<PrintAttachVM> attachmentDocumentContentList = new List<PrintAttachVM>();
                foreach (TransactionAttachmentVM item in transactionAttachmentVMs)
                {

                    if (string.IsNullOrWhiteSpace(item.DocumentVM.Name) || item.DocumentVM.Name.Contains(".png"))
                    {
                        string[] attachmentDocumentContent = new string[1];
                        attachmentDocumentContent[0] = Convert.ToBase64String(GetAttachmentByteById(item.DocumentVM.Id));
                        attachmentDocumentContentList.Add(new PrintAttachVM() { AttachmentDocumentImages = attachmentDocumentContent, transactionAttachmentVM = item });
                    }
                    else
                    {

                        Spire.Pdf.PdfDocument attachmentDocument = new Spire.Pdf.PdfDocument();
                        attachmentDocument.LoadFromBytes(GetAttachmentByteById(item.DocumentVM.Id));

                        System.Drawing.Image[] AttachmentDocumentImages = SaveAsImage(attachmentDocument);
                        string[] attachmentDocumentContent = new string[AttachmentDocumentImages.Count()];
                        for (int i = 0; i < AttachmentDocumentImages.Count(); i++)
                        {
                            attachmentDocumentContent[i] = ImageToBase64(AttachmentDocumentImages[i]);
                        }
                        attachmentDocumentContentList.Add(new PrintAttachVM() { AttachmentDocumentImages = attachmentDocumentContent, transactionAttachmentVM = item });

                    }

                }


                List<PrintExplanationVM> explanationDocumentContentList = new List<PrintExplanationVM>();
                foreach (ExplanationVM item in explanationVMs)
                {
                    Spire.Pdf.PdfDocument explanationDocument = new Spire.Pdf.PdfDocument();
                    explanationDocument.LoadFromBytes(GetExplanationByteById(item.Id));

                    System.Drawing.Image[] ExplanationDocumentImages = SaveAsImage(explanationDocument);
                    string[] explanationDocumentContent = new string[ExplanationDocumentImages.Count()];
                    for (int i = 0; i < ExplanationDocumentImages.Count(); i++)
                    {
                        explanationDocumentContent[i] = ImageToBase64(ExplanationDocumentImages[i]);
                    }
                    explanationDocumentContentList.Add(new PrintExplanationVM() { ExplanationDocumentImages = explanationDocumentContent, explanationVM = item });
                }


                printAllVM.MainDocumentImages = mainDocumentContent;
                printAllVM.AttachmentDocumentImages = attachmentDocumentContentList;
                printAllVM.ExplanationDocumentImages = explanationDocumentContentList;


                return View("~/Areas/User/Views/Transaction/_TransactionPrintAll.cshtml", printAllVM);

                // return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_TransactionPrintAll.cshtml", documentVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult PrintDraftOutboundTransaction(int transactionId, string transactionNumber, string hdnMainDocToken, bool isDecisionDraft)
        {
            try
            {
                ViewData["Number"] = transactionNumber;
                GetResult<TransactionPrintDTO> transactionPrintDTO = HttpClientWrapper<GetResult<TransactionPrintDTO>>.GetItemRequest(string.Format("api/Transaction/GetAllTransactionDocuments?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                DocumentVM documentVM = DocumentMapper.Map(transactionPrintDTO.Result.DocumentDTO);
                ViewData["MainDocumentDataObject"] = documentVM;


                PrintAllVM printAllVM = new PrintAllVM();

                var content = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);
                if (!isDecisionDraft)
                {
                    var barcode = GetBarcodeByte(transactionId);
                    content = addImageToPDF(content, barcode, Convert.ToInt32(ConfigurationManager.AppSettings["BarcodePxMaxWidth"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["BarcodePxMaxHeight"].ToString()));

                }

                DocumentVM documentVMzs = new DocumentVM
                {
                    Content = content,
                    Mode = true,
                    MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,


                };

                return View("~/Areas/User/Views/Shared/TransactionCertificate/_DocumentViewerPartial.cshtml", documentVMzs);



                //return View("~/Areas/User/Views/Transaction/_TransactionPrintAll.cshtml", printAllVM);

                // return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_TransactionPrintAll.cshtml", documentVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetLoggedInUserDelegations()
        {
            try
            {
                if (SessionInfo.CurrentUser == null)
                {
                    return null;
                }

                GetResult<List<UserDelegationDTO>> UserDelegationDTO = HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetLoggedInUserDelegations?UserId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
                return Json(new { result = UserDelegationDTO.Result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult DownloadUserManual()
        {
            try
            {


                //string filepath = AppDomain.CurrentDomain.BaseDirectory + "IAU User Manual.pdf";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["UserManual"].ToString();
                // byte[] filedata = System.IO.File.ReadAllBytes(filepath);

                byte[] pdfContent = System.IO.File.ReadAllBytes(filepath);

                if (pdfContent == null)
                {
                    return null;
                }

                return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Pdf, ConfigurationManager.AppSettings["UserManual"].ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DownloadVideoUserManual()
        {
            try
            {
                //string filepath = AppDomain.CurrentDomain.BaseDirectory + "IAU Video.zip";
                string filepath = AppDomain.CurrentDomain.BaseDirectory + ConfigurationManager.AppSettings["UserVideo"].ToString();
                // byte[] filedata = System.IO.File.ReadAllBytes(filepath);

                byte[] pdfContent = System.IO.File.ReadAllBytes(filepath);

                if (pdfContent == null)
                {
                    return null;
                }

                return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Zip, ConfigurationManager.AppSettings["UserVideo"].ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        public ActionResult DownloaAdminManual()
        {
            try
            {
                string filepath = AppDomain.CurrentDomain.BaseDirectory + "MOD Admin UM.pdf";
                // byte[] filedata = System.IO.File.ReadAllBytes(filepath);

                byte[] pdfContent = System.IO.File.ReadAllBytes(filepath);

                if (pdfContent == null)
                {
                    return null;
                }

                return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Pdf, "MOD Admin UM.pdf");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult OpenWacom()
        {
            try
            {
                //string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Wacom/Wacom.html";
                //byte[] filedata = System.IO.File.ReadAllBytes(filepath);

                //byte[] pdfContent = System.IO.File.ReadAllBytes(filepath);

                //if (pdfContent == null)
                //{
                //    return null;
                //}

                //return new FilePathResult("~/Wacom/Wacom.html", "text/html");

                string filepath = AppDomain.CurrentDomain.BaseDirectory + "/Wacom/Wacom.html";
                string wacomHtml = System.IO.File.ReadAllText(filepath);
                wacomHtml = wacomHtml.Replace("BASEPATH", AppDomain.CurrentDomain.BaseDirectory + "Wacom").Replace("\\", "/");
                byte[] filedata = Encoding.ASCII.GetBytes(wacomHtml);

                // byte[] pdfContent = System.IO.File.ReadAllBytes(filepath);

                if (filedata == null)
                {
                    return null;
                }

                return View("~/Areas/User/Views/Shared/Wacom.cshtml", model: wacomHtml);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult LateTransactionsDetails(string From, string To, int? page, int DepartmentId = -1, bool? isPrinting = false)
        {
            ViewData["LateTransactionsDetails"] = true;
            int rowCount = UIHelper.PageSize;
            if (isPrinting.HasValue && isPrinting.Value)
            {
                rowCount = 100000;
            }
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
            {
                level = 2;
            }

            int PageIndex = page.HasValue ? page.Value - 1 : 0;

            DateTime formattedFromDate;
            DateTime formattedToDate;
            if (From == null)
            {
                string sHijriYear = DateTimeUtility.GetHijriYear(DateTime.Now).ToString();
                formattedFromDate = DateTime.Now;
            }
            else
            {
                formattedFromDate = DateTimeUtility.ConvertToDate(From.Trim());
            }

            if (To == null)
            {
                formattedToDate = DateTime.Now;
            }
            else
            {
                formattedToDate = DateTimeUtility.ConvertToDate(To.Trim());
            }

            DashboardFilterCriteria dashboardFilterCriteria = new DashboardFilterCriteria()
            {
                level = level,
                userId = SessionInfo.CurrentUser.Id,
                entityId = (level == 1 || level == 2) ? SessionInfo.OrgUnitId : DepartmentId,
                toDate = formattedToDate.ToString(),
                fromDate = formattedFromDate.ToString(),
                itemId = 8,
                cultureId = SessionInfo.CultureShortName,
                pageIndex = PageIndex,
                pageSize = rowCount
            };

            GetResult<List<TransactionDetailsDTO>> transactionDTOList =
                HttpClientWrapper<GetResult<List<TransactionDetailsDTO>>>.PostRequest("api/Dashboard/LateTransactionsDetails", dashboardFilterCriteria).Result;

            List<TransactionDetailsVM> TransactionDetailsVMList = TransactionDetailsMapper.Map(transactionDTOList.Result);

            var transactionDetails = (CustomGridMvc.AjaxGrid<TransactionDetailsVM>)new CustomGridMvc.AjaxGridFactory()
                .CreateAjaxGrid(TransactionDetailsVMList, 1, transactionDTOList.RowsCount.Value, page.HasValue, rowCount);

            if (page.HasValue)
            {
                return Json(new { Html = transactionDetails.ToJson("~/Areas/User/Views/Shared/_PerformanceMeasurementLateDetailsGrid.cshtml", this), transactionDetails.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (isPrinting.HasValue && isPrinting.Value)
            {

                var httpValue = SystemConfigurations.EnableSSL ? "https://" : "http://";
                var url = string.Format("{0}{1}", httpValue, HttpContext.Request.Url.Host);

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PerformanceMeasurementDetails.cshtml", transactionDetails);
                Html = Html.Replace("href=\"", "href=\"" + url);
                Html = Html.Replace("<button class=\"btn btn-st1 btn-lg\" onclick=\"ShowPrintDialog()\">طباعة</button>", "");
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "Dashboard Details.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("~/Areas/User/Views/Shared/_PerformanceMeasurementLateDetails.cshtml", transactionDetails);
            }
        }

        public ActionResult InProgressTransactionsDetails(string From, string To, int? page, int DepartmentId = -1, bool? isPrinting = false)
        {
            ViewData["InProgressTransactionsDetails"] = true;
            int rowCount = UIHelper.PageSize;
            if (isPrinting.HasValue && isPrinting.Value)
            {
                rowCount = 100000;
            }
            int level = 1;
            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
            {
                level = 4;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
            {
                level = 3;
            }
            else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
            {
                level = 2;
            }

            int PageIndex = page.HasValue ? page.Value - 1 : 0;

            DateTime formattedFromDate;
            DateTime formattedToDate;
            if (From == null)
            {
                string sHijriYear = DateTimeUtility.GetHijriYear(DateTime.Now).ToString();
                formattedFromDate = DateTime.Now;
            }
            else
            {
                formattedFromDate = DateTimeUtility.ConvertToDate(From.Trim());
            }

            if (To == null)
            {
                formattedToDate = DateTime.Now;
            }
            else
            {
                formattedToDate = DateTimeUtility.ConvertToDate(To.Trim());
            }

            DashboardFilterCriteria dashboardFilterCriteria = new DashboardFilterCriteria()
            {
                level = level,
                userId = SessionInfo.CurrentUser.Id,
                entityId = (level == 1 || level == 2) ? SessionInfo.OrgUnitId : DepartmentId,
                toDate = formattedToDate.ToString(),
                fromDate = formattedFromDate.ToString(),
                itemId = 8,
                cultureId = SessionInfo.CultureShortName,
                pageIndex = PageIndex,
                pageSize = rowCount
            };

            GetResult<List<TransactionDetailsDTO>> transactionDTOList =
                HttpClientWrapper<GetResult<List<TransactionDetailsDTO>>>.PostRequest("api/Dashboard/InProgressTransactionsDetails", dashboardFilterCriteria).Result;

            List<TransactionDetailsVM> TransactionDetailsVMList = TransactionDetailsMapper.Map(transactionDTOList.Result);

            var transactionDetails = (CustomGridMvc.AjaxGrid<TransactionDetailsVM>)new CustomGridMvc.AjaxGridFactory()
                .CreateAjaxGrid(TransactionDetailsVMList, 1, transactionDTOList.RowsCount.Value, page.HasValue, rowCount);

            if (page.HasValue)
            {
                return Json(new { Html = transactionDetails.ToJson("~/Areas/User/Views/Shared/_PerformanceMeasurementInProgressDetailsGrid.cshtml", this), transactionDetails.HasItems }, JsonRequestBehavior.AllowGet);
            }

            if (isPrinting.HasValue && isPrinting.Value)
            {

                var httpValue = SystemConfigurations.EnableSSL ? "https://" : "http://";
                var url = string.Format("{0}{1}", httpValue, HttpContext.Request.Url.Host);

                var Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_PerformanceMeasurementDetails.cshtml", transactionDetails);
                Html = Html.Replace("href=\"", "href=\"" + url);
                Html = Html.Replace("<button class=\"btn btn-st1 btn-lg\" onclick=\"ShowPrintDialog()\">طباعة</button>", "");
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(Html);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "Dashboard Details.pdf";
                return Json(new { FileGuid = handle, FileName = fileResult.FileDownloadName, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View("~/Areas/User/Views/Shared/_PerformanceMeasurementInProgressDetails.cshtml", transactionDetails);
            }
        }

        #endregion
        protected string GetLetterTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<LetterTypeVM>> letterTypeVMs = LookupsHelper.GetLetterTypes(transactionCategory);
                if (transactionCategory == TransactionCategory.ExternalOutbound)
                {
                    // Remove رقم الوثيقة 
                    letterTypeVMs.Result = letterTypeVMs.Result.Where(x => x.Id != 52).ToList();
                }

                if (letterTypeVMs.Result != null)
                {
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
        public ActionResult DeliveryStatment()
        {
            return View("~/Areas/User/Views/Shared/_DeliveryStatments.cshtml");

        }

        public ActionResult PrintTransactions(string transId, bool withRelated, bool hidePrint = false)
        {
            try
            {
                Session["IsEditMode"] = false;
                int trxId = int.Parse(StringCipher.DecryptStringAES(transId.Replace(" ", "+")));
                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();
                if (withRelated)
                {
                    GetResult<List<TransactionLinkDTO>> transactionLinkDTOs =
                  HttpClientWrapper<GetResult<List<TransactionLinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionLinks?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                    transactionLinkVMs = TransactionLinkMapper.Map(transactionLinkDTOs.Result);
                    if (transactionLinkVMs == null)
                    {
                        transactionLinkVMs = new List<TransactionLinkVM>();
                    }


                }
                else
                {
                    transactionLinkVMs = new List<TransactionLinkVM>();
                }

                List<DocumentVM> documentVMs = new List<DocumentVM>();
                DocumentVM DocumentVMVariable = PrintMainTransaction(trxId);
                if (hidePrint)
                {
                    DocumentVMVariable.HidePrint = true;
                }

                if (DocumentVMVariable != null)
                {
                    documentVMs.Add(DocumentVMVariable);
                }

                foreach (var item in transactionLinkVMs)
                {
                    DocumentVMVariable = PrintMainTransaction(item.TransactionId);

                    if (DocumentVMVariable != null)
                    {
                        if (hidePrint)
                        {
                            DocumentVMVariable.HidePrint = true;
                        }
                        documentVMs.Add(DocumentVMVariable);
                    }

                }
                var content = DoconutHelper.concatAndAddContent(documentVMs.Select(x => x.Content).ToList(), Session["WatermarkText"].ToString());

                DocumentVM documentVM = new DocumentVM
                {
                    Content = content,
                    Mode = !withRelated,
                    MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,


                };


                return View("~/Areas/User/Views/Shared/TransactionCertificate/_DocumentViewerPartial.cshtml", documentVM);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult PrintExternalTransactions(string transId, bool hidePrint = false)
        {
            try
            {
                Session["IsEditMode"] = false;
                int trxId = int.Parse(StringCipher.DecryptStringAES(transId.Replace(" ", "+")));
                Session["TransactionId"] = trxId;
                List<DocumentVM> documentVMs = new List<DocumentVM>();
                DocumentVM DocumentVMVariable = PrintMainTransaction(trxId);
                if (hidePrint)
                {
                    DocumentVMVariable.HidePrint = true;
                }

                if (DocumentVMVariable != null)
                {
                    documentVMs.Add(DocumentVMVariable);
                }

                var content = DoconutHelper.concatAndAddContent(documentVMs.Select(x => x.Content).ToList(), "");

                DocumentVM documentVM = new DocumentVM
                {
                    Content = content,
                    MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,
                };


                return View("~/Areas/User/Views/Shared/TransactionCertificate/_DocumentExternalViewerPartial.cshtml", documentVM);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public DocumentVM PrintMainTransaction(int transactionId)
        {
            try
            {

                DocumentVM documentVM = null;

                GetResult<TransactionPrintDTO> transactionPrintDTO = HttpClientWrapper<GetResult<TransactionPrintDTO>>.GetItemRequest(string.Format("api/Transaction/GetAllTransactionDocuments?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                documentVM = DocumentMapper.Map(transactionPrintDTO.Result.DocumentDTO);

                Spire.Pdf.PdfDocument pdfDocument = new Spire.Pdf.PdfDocument();
                pdfDocument.LoadFromBytes(documentVM.Content);

                System.Drawing.Image[] MainDocumentImages = SaveAsImage(pdfDocument);

                string[] mainDocumentContent = new string[MainDocumentImages.Count()];
                for (int i = 0; i < MainDocumentImages.Count(); i++)
                {
                    mainDocumentContent[i] = ImageToBase64(MainDocumentImages[i]);
                }

                for (int i = 0; i < MainDocumentImages.Count(); i++)
                {
                    mainDocumentContent[i] = ImageToBase64(MainDocumentImages[i]);
                }


                documentVM.MainDocumentImages = mainDocumentContent;

                return documentVM;

            }
            catch (Exception ex)
            {
                // throw;

                return null;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteTransaction(long transId, bool isDeleted)
        {
            try
            {
                string message = string.Empty;
                PostResult putResult = HttpClientWrapper<PostResult>
                                                     .PostRequest(string.Format("api/Transaction/UpdateTransactionDeleteByTransId?transactionId={0}&isDeleted={1}", transId, isDeleted), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = putResult.Id.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteDraftTransaction(long transId, bool isDeleted)
        {
            try
            {
                string message = string.Empty;
                PostResult putResult = HttpClientWrapper<PostResult>
                                                     .PostRequest(string.Format("api/Transaction/DeleteDraftTransaction?transactionId={0}&isDeleted={1}", transId, isDeleted), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = putResult.Id.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult LogPrintWithoutWatermark(int transactionId)
        {
            try
            {
                string message = string.Empty;
                PostResult putResult = HttpClientWrapper<PostResult>
                                                     .PostRequest(string.Format("api/Transaction/LogPrintDocument?transactionId={0}", transactionId), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = putResult.Id.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }






    }




}
