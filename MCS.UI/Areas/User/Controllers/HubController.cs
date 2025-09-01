using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO.HubTransaction;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Hub;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Common;
using sharedDocumentVM = MCS.UI.Areas.User.Models.Shared;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Controllers
{
    public class HubController : BaseController
    {
        #region Outbound
        //[CustomAuthorizationAttribute(UserClaims.Hub.ContactHub)]
        public string SendOutbound(int transactionId)
        {
            PostResult postResult = HttpClientWrapper<PostResult>.
                PostRequest(
                string.Format(
                    "api/Hub/SendOutbound?transactionId={0}&culture={1}",
                    transactionId, SessionInfo.CultureShortName),
                null).Result;

            return postResult.Result.ToString();
        }
        //[CustomAuthorizationAttribute(UserClaims.Hub.ContactHub)]
        public string ResendOutbound(int transactionId)
        {
            PostResult postResult = HttpClientWrapper<PostResult>.
                PostRequest(
                string.Format(
                    "api/Hub/ResendOutbound?transactionId={0}&tenantId={1}&culture={2}",
                    transactionId, -1, SessionInfo.CultureShortName),
                null).Result;

            return postResult.Result.ToString();
        }
        //[CustomAuthorizationAttribute(UserClaims.Hub.ContactHub)]
        public string SendStatusInquiry(int transactionId)
        {
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(
                string.Format(
                    "api/Hub/SendStatusInquiry?transactionId={0}&culture={1}",
                    transactionId, SessionInfo.CultureShortName),
                null
                ).Result;

            return postResult.Result.ToString();
        }
        //[CustomAuthorizationAttribute(UserClaims.Hub.ContactHub)]
        public string SendReject(string transactionNumber, int orgUnitId, string rejectionReason)
        {
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(
                string.Format(
                    "api/Hub/SendReject?transactionNumber={0}&orgUnitId={1}&rejectionReason={2}",
                    transactionNumber, orgUnitId, rejectionReason),
                null
                ).Result;

            return postResult.Result.ToString();
        }
        #endregion

        #region Inbound
        [CustomAuthorizationAttribute(UserClaims.Files.YESSER)]
        public ActionResult GetAll(int? typeId)
        {
            try
            {
                var TypeID = typeId ?? (int)OutboundClassification.Original;
                GetResult<List<HubTransactionDTO>> hubTransactionDTOList =
                         HttpClientWrapper<GetResult<List<HubTransactionDTO>>>
                         .GetItemRequest(
                             string.Format(
                                 "api/Hub/GetOriginalHubTransactions?culture={0}&TypeId={1}",
                                 SessionInfo.CultureShortName, TypeID)).Result;

                TrayDetailsVM trayDetailsVM = new TrayDetailsVM();
                trayDetailsVM.Id = (int)TrayType.YESSER;

                List<TransactionTrayInfoVM> transactionTrayInfos = hubTransactionDTOList.Result.Select(hubTransactionDTO =>
                {
                    TransactionTrayInfoVM transactionTrayInfoVM = new TransactionTrayInfoVM
                    {
                        TransactionDetailsInfoVM = new TransactionDetailsInfoVM
                        {
                            Id = hubTransactionDTO.Id,
                            Subject = hubTransactionDTO.Subject,
                            ConfidentialityId = hubTransactionDTO.ConfidentialityLevelId,
                            ExternalPartyName = hubTransactionDTO.ExternalPartyName,
                            PriorityName = hubTransactionDTO.PriorityText,
                            Date = hubTransactionDTO.RecordDate,
                            DateH = hubTransactionDTO.HijriRecordDate,
                            ConfidentialityName = hubTransactionDTO.ConfidentialityName,
                            TransactionType = hubTransactionDTO.TransactionType,
                            SourceTypeName = hubTransactionDTO.SourceTypeName,
                            DocumentNumber = hubTransactionDTO.TransactionNumber,
                            DestinationId = hubTransactionDTO.DestinationId,
                            OrgUnitId = hubTransactionDTO.OrgUnitId,
                            DeliveryType = hubTransactionDTO.DeliveryType,
                            DeliveryTypeName = hubTransactionDTO.DeliveryTypeName
                        }
                    };
                    return transactionTrayInfoVM;
                }).ToList();

                trayDetailsVM.TransactionTrayInfoVMs =
                    (AjaxGrid<TransactionTrayInfoVM>)new AjaxGridFactory().CreateAjaxGrid(transactionTrayInfos, 1, transactionTrayInfos.Count, false, 100);

                if (typeId != null)
                {
                    if (typeId == (int)OutboundClassification.Original)
                    {
                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_YesserTransactionsPartial.cshtml", trayDetailsVM.TransactionTrayInfoVMs) }, JsonRequestBehavior.AllowGet);
                    }
                    else if (typeId == (int)OutboundClassification.Copy)
                    {
                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_YesserCopiesPartial.cshtml", trayDetailsVM.TransactionTrayInfoVMs) }, JsonRequestBehavior.AllowGet);
                    }
                }
                return View("~/Areas/User/Views/File/OrgUnitIndex.cshtml", trayDetailsVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[CustomAuthorizationAttribute(UserClaims.Hub.ContactHub)]
        public string CreateInbound(string transactionNumber, int orgUnitId)
        {
            var internalOrgUnitId = SessionInfo.OrgUnitId;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(
                string.Format(
                    "api/Hub/CreateInbound?transactionNumber={0}&orgUnitId={1}&userId={2}&internalOrgUnitId={3}",
                    transactionNumber, orgUnitId, SessionInfo.CurrentUser.Id, internalOrgUnitId),
                null
                ).Result;

            return postResult.Result.ToString();
        }
        public string CreateOutboundInternal(string transactionNumber, int orgUnitId)
        {
            var internalOrgUnitId = SessionInfo.OrgUnitId;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(
                string.Format(
                    "api/Hub/CreateOutboundInternal?transactionNumber={0}&tenantId={1}&orgUnitId={2}&userId={3}&internalOrgUnitId={4}",
                    transactionNumber, -1, orgUnitId, SessionInfo.CurrentUser.Id, internalOrgUnitId),
                null
                ).Result;

            return postResult.Result.ToString();
        }
        [HttpGet]
        public ActionResult ViewTransaction(int TransactionId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                GetResult<HubTransactionDTO> hubTransactionDTO =
                         HttpClientWrapper<GetResult<HubTransactionDTO>>
                         .GetItemRequest(
                             string.Format(
                                 "api/Hub/GetHubTransactionById?transactionId={0}&cultureName={1}", TransactionId,
                                 SessionInfo.CultureShortName)).Result;

                var hubTransactionResult = hubTransactionDTO.Result;

                HubTransactionVM hubTransactionVM = new HubTransactionVM()
                {
                    Id = hubTransactionResult.Id,
                    ConfidentialityLevelId = hubTransactionResult.ConfidentialityLevelId,
                    ConfidentialityName = hubTransactionResult.ConfidentialityName,
                    DestinationId = hubTransactionResult.DestinationId,
                    HijriRecordDate = hubTransactionResult.HijriRecordDate,
                    PriorityLevelId = hubTransactionResult.PriorityLevelId,
                    PriorityText = hubTransactionResult.PriorityText,
                    OrgUnitId = hubTransactionResult.OrgUnitId,
                    RecordDate = hubTransactionResult.RecordDate,
                    Subject = hubTransactionResult.Subject,
                    Remarks = hubTransactionResult.Remarks,
                    RQUID = hubTransactionResult.RQUID,
                    TransactionNumber = hubTransactionResult.TransactionNumber,
                    ExternalPartyName = hubTransactionResult.ExternalPartyName,
                    TransactionType = hubTransactionResult.TransactionType,
                    SourceTypeName = hubTransactionResult.SourceTypeName,
                    Status = hubTransactionResult.Status,
                    DeliveryType = hubTransactionResult.DeliveryType,
                    DeliveryTypeName = hubTransactionResult.DeliveryTypeName,
                    HubAttachments = hubTransactionResult.HubAttachments.Select(ta =>
                    {
                        HubAttachmentVM hubAttachmentVM = new HubAttachmentVM
                        {
                            Id = ta.Id,
                            Count = ta.Count,
                            Description = ta.Description,
                            ExternalAttachementId = ta.ExternalAttachementId,
                            TypeId = ta.TypeId,
                            DocumentInfo = new DocumentInfoVM
                            {
                                Document = new DocumentVM
                                {
                                    Id = ta.DocumentInfo.Document.Id,
                                    Content = ta.DocumentInfo.Document.Content
                                },
                                ECMId = ta.DocumentInfo.ECMId,
                                IsDeleted = ta.DocumentInfo.IsDeleted,
                                MimeType = ta.DocumentInfo.MimeType,
                                Name = ta.DocumentInfo.Name,
                                Size = ta.DocumentInfo.Size
                            }
                        };
                        return hubAttachmentVM;
                    }).ToList(),
                    HubRelatedPersons = hubTransactionResult.HubRelatedPersons.Select(hrp =>
                    {
                        HubRelatedPersonVM hubRelatedPersonDTO = new HubRelatedPersonVM
                        {
                            Id = hrp.Id,
                            Address = hrp.Address,
                            Email = hrp.Email,
                            Name = hrp.Name,
                            NationalId = hrp.NationalId
                        };
                        return hubRelatedPersonDTO;
                    }).ToList(),
                    MainDocument = hubTransactionResult.MainDocument != null ? new DocumentInfoVM
                    {
                        Name = hubTransactionResult.MainDocument.Name,
                        ECMId = hubTransactionResult.MainDocument.ECMId,
                        IsDeleted = hubTransactionResult.MainDocument.IsDeleted,
                        Size = hubTransactionResult.MainDocument.Size,
                        MimeType = hubTransactionResult.MainDocument.MimeType,
                        Document = hubTransactionResult.MainDocument.Document != null ? new DocumentVM
                        {
                            Id = hubTransactionResult.MainDocument.Document.Id,
                            Content = hubTransactionResult.MainDocument.Document.Content
                        } : null
                    } : new DocumentInfoVM()
                };

                List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();
                DocumentVM documentVM = hubTransactionVM.MainDocument.Document;
                if (documentVM != null)
                {
                    string documentId = Guid.NewGuid().ToString();
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = documentVM.Id,
                        AttachmentTypeId = -1,
                        ArcivingTypeName = LookupsHelper.GetLookupItem((int)TransactionAttachmentType.Main,
                        SessionInfo.CultureShortName).Result.Text
                    });
                    Session["DocoNutDocument"] = documentVM.Content;
                    Session["DocoNutDocumentId"] = TransactionId;
                    ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                    ViewData["SessionArchiveMainDocumentKey"] = Guid.NewGuid().ToString();
                    ViewData["MainDocumentId"] = documentVM.Id;

                }

                List<TransactionNameVM> transactionNameVM = hubTransactionResult.HubRelatedPersons.Select(hrp =>
                {
                    TransactionNameVM transactionName = new TransactionNameVM
                    {
                        Id = hrp.Id,
                        Address = hrp.Address,
                        Email = hrp.Email,
                        FirstName = hrp.Name,
                        CivilID = hrp.NationalId
                    };
                    return transactionName;
                }).ToList();

                List<TransactionAttachmentVM> transactionAttachmentVM = hubTransactionResult.HubAttachments.Select(ta =>
                {
                    TransactionAttachmentVM TransactionAttachment = new TransactionAttachmentVM
                    {
                        Id = ta.Id,
                        Number = ta.Count,
                        TypeId = ta.TypeId,
                        AttachmentName = ta.DocumentInfo.Name,
                        DocumentVM = new sharedDocumentVM.DocumentVM()
                        {
                            Id = ta.DocumentInfo.Document.Id,
                            Content = ta.DocumentInfo.Document.Content,
                            IsDeleted = ta.DocumentInfo.IsDeleted,
                            MimeType = ta.DocumentInfo.MimeType,
                            Name = ta.DocumentInfo.Name,
                            Size = ta.DocumentInfo.Size
                        }
                    };
                    return TransactionAttachment;
                }).ToList();


                List<TransactionArchiveVM> Archives = new List<TransactionArchiveVM>();
                if (hubTransactionVM.HubAttachments != null && hubTransactionVM.HubAttachments.Count > 0)
                {
                    foreach (var transactionAttachment in hubTransactionVM.HubAttachments)
                    {
                        if (transactionAttachment.DocumentInfo.Document != null && transactionAttachment.DocumentInfo.Document.Size > 0)
                        {
                            Archives.Add(new TransactionArchiveVM
                            {
                                Id = Guid.NewGuid().ToString(),
                                DocumentId = transactionAttachment.DocumentInfo.Document.Id,
                                AttachmentTypeId = transactionAttachment.TypeId,
                                IsMainDocument = false,
                                IsNew = true,
                                IsDeleted = transactionAttachment.DocumentInfo.Document.IsDeleted
                            });
                        }
                    }

                }
                ViewData["Names"] = transactionNameVM;
                ViewData["Attachments"] = transactionAttachmentVM;
                ViewData["Archives"] = Archives;
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(Archives);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(transactionAttachmentVM);
                ViewData["SessionArchiveDocumentKey"] = Guid.NewGuid().ToString();
                return View("~/Areas/User/Views/Hub/_ShowHubTransaction.cshtml", hubTransactionVM);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult MarkCopyAsSeen(int transactionId)
        {
            try
            {
                PostResult isDeleted = HttpClientWrapper<PostResult>.PostRequest(
               string.Format("api/Hub/MarkCopyAsSeen?transactionId={0}", transactionId), null).Result;

                if (isDeleted.Result.ToString() == "Success")
                {
                    return Json(new { message = "Success" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { message = "Fail" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        [HttpGet]
        public ActionResult ShowDocumentViewer(int transactionId, int documentId, string documentSessionKey)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                GetResult<HubTransactionDTO> hubTransactionDTO =
                        HttpClientWrapper<GetResult<HubTransactionDTO>>
                        .GetItemRequest(
                            string.Format(
                                "api/Hub/GetHubTransactionById?transactionId={0}&cultureName={1}", transactionId,
                                SessionInfo.CultureShortName)).Result;

                HubTransactionDTO hubTransactionResult = hubTransactionDTO.Result;

                DocumentVM documentVM;

                if (hubTransactionResult.MainDocument.Document.Id == documentId)
                {
                    documentVM = hubTransactionResult.MainDocument != null ? new DocumentVM
                    {
                        Name = hubTransactionResult.MainDocument.Name,
                        IsDeleted = hubTransactionResult.MainDocument.IsDeleted,
                        Size = hubTransactionResult.MainDocument.Size,
                        MimeType = hubTransactionResult.MainDocument.MimeType,
                        Content = hubTransactionResult.MainDocument.Document != null ? hubTransactionResult.MainDocument.Document.Content : null
                    } : new DocumentVM();
                }
                else
                {
                    documentVM = hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault() != null ? new DocumentVM
                    {
                        Name = hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault().DocumentInfo.Name,
                        IsDeleted = hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault().DocumentInfo.IsDeleted,
                        Size = hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault().DocumentInfo.Size,
                        MimeType = hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault().DocumentInfo.MimeType,
                        Content = hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault().DocumentInfo.Document != null ? hubTransactionResult.HubAttachments.Where(t => t.DocumentInfo.Document.Id == documentId).FirstOrDefault().DocumentInfo.Document.Content : null
                    } : new DocumentVM();
                }

                ViewData["DocumentSessionKey"] = documentSessionKey;

                Session["DocoNutDocument"] = documentVM.Content;

                return View("~/Areas/User/Views/Shared/TransactionCertificate/_DocumentViewerPartial.cshtml");
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
