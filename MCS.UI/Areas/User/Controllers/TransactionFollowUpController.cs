using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using DotnetDaddy.DocumentConfig;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Controls;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Framework.Persistence;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Mappers.UserPreferences;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Common;
using MCS.UI.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using A = DocumentFormat.OpenXml.Drawing;
using CustomAjaxGrid = MCS.GridMvc.Ajax.GridExtensions;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
namespace MCS.UI.Areas.User.Controllers
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]

    public class TransactionFollowUpController : TransactionController
    {
        TransactionCategory transactionCategory = TransactionCategory.Inbound;
        public ActionResult FollowUp(string transId, string FollowuptrayId)
        {
            try
            {
                FollowUpCertificateVM followUpCertificateVM = GetFollowUpCertificateByTransactionId(Convert.ToInt32(transId), Convert.ToInt32(FollowuptrayId));

                return View("~/Areas/User/Views/TransactionFollowUp/FollowUp.cshtml", followUpCertificateVM);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public FollowUpCertificateVM GetFollowUpCertificateByTransactionId(int id, int FollowuptrayId)
        {
            try
            {
                int followupstatus = (int)FollowupStatus.All;
                switch (FollowuptrayId)
                {

                    case (int)TrayType.FollowUpCanceld:
                        {
                            followupstatus = (int)FollowupStatus.Cancled;
                            break;
                        }
                    case (int)TrayType.FollowUpComplete:
                        {
                            followupstatus = (int)FollowupStatus.Completed;
                            break;
                        }
                }
                string message = string.Empty;
                //FollowUpDetailsByFollowUpId
                GetResult<TransactionFollowUpDTO> getResult =
                      HttpClientWrapper<GetResult<TransactionFollowUpDTO>>.GetItemRequest(string.Format("api/Transaction/FollowUpDetailsByTransId?transId={0}&FollowUpStatusId={1}&UserId={2}&OrgUnitId={3}&cultureName={4}", id, followupstatus, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                if (getResult.StatusCode == StatusCode.Ok && getResult.Result != null)
                {
                    FollowUpCertificateVM followUpCertificateVM = TransactionFollowUpMapper.MapToFollowUpCertificate(getResult.Result);



                    if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.New || followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.UnLockFollowup ||
                        (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.Delayed && followUpCertificateVM.FollowUpReceiveDate == null))
                    {
                        followUpCertificateVM.ReadOnly = true;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsFinalCompleted = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                    }
                    else if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.EnsureComplition)
                    {
                        followUpCertificateVM.IsCompleted = true;
                        followUpCertificateVM.ReadOnly = false;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                        followUpCertificateVM.IsFinalCompleted = true;

                    }
                    else if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.Cancled)
                    {
                        followUpCertificateVM.ReadOnly = false;
                        followUpCertificateVM.IsCanceld = true;
                        followUpCertificateVM.IsFinalCompleted = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                    }
                    else if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.UnderFollowup || followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.Completed ||
                        (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.Delayed && followUpCertificateVM.FollowUpReceiveDate != null))
                    {
                        followUpCertificateVM.ReadOnly = false;
                        followUpCertificateVM.IsUnderFollowup = true;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsFinalCompleted = false;
                    }
                    else
                    {
                        followUpCertificateVM.ReadOnly = true;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsFinalCompleted = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                    }



                    //followUpCertificateVM.FollowUps = listDetails;

                    List<TransactionDetailsVM> transactionDetailsVMs = new List<TransactionDetailsVM>();
                    TransactionLinkVM transactionLinkVM = new TransactionLinkVM();
                    followUpCertificateVM.transactionLinkVM = transactionLinkVM;
                    followUpCertificateVM.transactionLinkVM.TransactionLinkSearch = transactionDetailsVMs;
                    followUpCertificateVM.transactionLinkVM.Links = GetTransactionLinks(followUpCertificateVM.TransactionId);

                    initialize(transactionCategory, followUpCertificateVM);
                    buildtree(followUpCertificateVM.FollowUpEntityId);
                    GetTransactionInfo(followUpCertificateVM.TransactionId);

                    return followUpCertificateVM;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public FollowUpCertificateVM GetFollowUpCertificateByFollowUpId(int id)
        {
            try
            {
                string message = string.Empty;
                //FollowUpDetailsByFollowUpId
                GetResult<TransactionFollowUpDTO> getResult =
                      HttpClientWrapper<GetResult<TransactionFollowUpDTO>>.GetItemRequest(string.Format("api/Transaction/FollowUpDetailsByFollowUpId?FollowUpId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;
                if (getResult.StatusCode == StatusCode.Ok && getResult.Result != null)
                {
                    FollowUpCertificateVM followUpCertificateVM = TransactionFollowUpMapper.MapToFollowUpCertificate(getResult.Result);


                    if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.New || followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.UnLockFollowup)
                    {
                        followUpCertificateVM.ReadOnly = true;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsFinalCompleted = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                    }
                    else if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.EnsureComplition)
                    {
                        followUpCertificateVM.IsCompleted = true;
                        followUpCertificateVM.ReadOnly = false;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                        followUpCertificateVM.IsFinalCompleted = true;

                    }
                    else if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.Cancled)
                    {
                        followUpCertificateVM.ReadOnly = false;
                        followUpCertificateVM.IsCanceld = true;
                        followUpCertificateVM.IsFinalCompleted = false;
                        followUpCertificateVM.IsUnderFollowup = false;
                    }
                    else if (followUpCertificateVM.FollowUpStatusId == (int)FollowupStatus.UnderFollowup)
                    {
                        followUpCertificateVM.ReadOnly = false;
                        followUpCertificateVM.IsUnderFollowup = true;
                        followUpCertificateVM.IsCanceld = false;
                        followUpCertificateVM.IsFinalCompleted = false;
                    }
                    //List<FollowUpAuditTrailVM> list = GetFollowUpAuditTrails(followUpCertificateVM.FollowUpId);
                    //followUpCertificateVM.FollowUpAuditTrails = list;

                    //List<TransactionFollowUpVM> listDetails = GetTransactionFollowUps(followUpCertificateVM.TransactionId);
                    //followUpCertificateVM.FollowUps = listDetails;

                    List<TransactionDetailsVM> transactionDetailsVMs = new List<TransactionDetailsVM>();
                    TransactionLinkVM transactionLinkVM = new TransactionLinkVM();
                    followUpCertificateVM.transactionLinkVM = transactionLinkVM;
                    followUpCertificateVM.transactionLinkVM.TransactionLinkSearch = transactionDetailsVMs;
                    followUpCertificateVM.transactionLinkVM.Links = GetTransactionLinks(followUpCertificateVM.TransactionId);
                    initialize(transactionCategory, followUpCertificateVM);
                    buildtree(followUpCertificateVM.FollowUpEntityId);
                    GetTransactionInfo(followUpCertificateVM.TransactionId);


                    return followUpCertificateVM;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.FollowUps.AddFollowUp)]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveFollowUp([Bind(Prefix = "FollowUpTab")] FollowUpCertificateVM followUpCertificateVM)
        {
            string message = string.Empty;
            try
            {

                if ((followUpCertificateVM.FollowUpUserId == SessionInfo.CurrentUser.Id) || (!followUpCertificateVM.FollowUpUserId.HasValue && followUpCertificateVM.FollowUpEntityId == SessionInfo.OrgUnitId))
                {
                    int FollowupStatu = (int)FollowupStatus.UnderFollowup;

                    if (followUpCertificateVM.IsCompleted)
                        FollowupStatu = (int)FollowupStatus.Completed;
                    else if (followUpCertificateVM.FollowUpExpireDate < DateTime.Now)
                        FollowupStatu = (int)FollowupStatus.Delayed;
                    else
                        FollowupStatu = (int)FollowupStatus.UnderFollowup;


                    PostResult postResult =
                     HttpClientWrapper<PostResult>.PostRequest("api/Transaction/TransactionFollowUpUpdate?cultureName=" + SessionInfo.CultureShortName, TransactionFollowUpMapper.MapFollowUpCertificateToDTO(followUpCertificateVM)).Result;

                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    FollowUpChangeStatus(followUpCertificateVM.FollowUpId, FollowupStatu, true);


                    if (!followUpCertificateVM.IsCompleted)

                    {
                        followUpCertificateVM.FollowUpProgressId = followUpCertificateVM.FollowUpProgressId.HasValue ? followUpCertificateVM.FollowUpProgressId : 0;
                        if (followUpCertificateVM.FollowUpProgressId > 0)
                            AddFollowupUditTrial(followUpCertificateVM.FollowUpId, (int)FollowupAuditProcess.UnderProcessingFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.FollowUpPercentageOfCompletion") + " " + followUpCertificateVM.FollowUpProgressId + "%", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);
                    }
                    else
                    {
                        AddFollowupUditTrial(followUpCertificateVM.FollowUpId, (int)FollowupAuditProcess.CompletionFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.CompletionFollowup"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);
                    }
                }

                initialize(transactionCategory, followUpCertificateVM);
                buildtree(followUpCertificateVM.FollowUpEntityId);

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.FollowUps.AddFollowUp)]
        [ValidateAntiForgeryToken()]
        public ActionResult SendFollowUp([Bind(Prefix = "FollowUpTab")] FollowUpCertificateVM followUpCertificateVM)
        {
            string message = string.Empty;
            try
            {

                if ((followUpCertificateVM.FollowUpUserId != SessionInfo.CurrentUser.Id) || (followUpCertificateVM.FollowUpUserId.HasValue && followUpCertificateVM.FollowUpEntityId != SessionInfo.OrgUnitId))
                {

                    int followUpParentId = followUpCertificateVM.FollowUpId;
                    int followUpChildId = AddFollowUpSecoundLevel(followUpCertificateVM);

                    followUpCertificateVM.FollowUpStatusId = (int)FollowupStatus.UnderFollowupSecondLevel;
                    followUpCertificateVM.HasChild = true;
                    followUpCertificateVM.FollowUpId = followUpParentId;
                    followUpCertificateVM.ParentId = null;
                    PostResult postResult =
                HttpClientWrapper<PostResult>.PostRequest("api/Transaction/TransactionFollowUpUpdate?cultureName=" + SessionInfo.CultureShortName, TransactionFollowUpMapper.MapFollowUpCertificateToDTO(followUpCertificateVM)).Result;

                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    FollowUpChangeStatus(followUpCertificateVM.FollowUpId, (int)FollowupStatus.UnderFollowupSecondLevel, true);
                    AddFollowupUditTrial(followUpChildId, (int)FollowupAuditProcess.AddSecondaryFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddSecondaryFollowup"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);
                    AddFollowupUditTrial(followUpCertificateVM.FollowUpId, (int)FollowupAuditProcess.AddAssignFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddAssignFollowup") + " " + followUpCertificateVM.FollowUpUserName, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);

                }

                initialize(transactionCategory, followUpCertificateVM);
                buildtree(followUpCertificateVM.FollowUpEntityId);

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int AddFollowUpSecoundLevel(FollowUpCertificateVM followVM)
        {
            string message = string.Empty;
            try
            {

                followVM.ParentId = followVM.FollowUpId;
                followVM.HasChild = false;
                followVM.FollowUpId = 0;
                followVM.CreatingEntityId = SessionInfo.OrgUnitId;
                followVM.FollowUpProccessId = 0;
                followVM.CreatingUserId = SessionInfo.CurrentUser.Id;
                followVM.FollowUpStatusId = (int)FollowupStatus.New;
                followVM.CreationDate = DateTime.Now;
                followVM.Active = true;
                followVM.FollowUpTypeId = (int)FollowupType.Secondary;

                PostResult postResult =
              HttpClientWrapper<PostResult>.PostRequest("api/Transaction/TransactionFollowUpAdd?cultureName=" + SessionInfo.CultureShortName, TransactionFollowUpMapper.MapFollowUpCertificateToDTO(followVM)).Result;

                if (postResult.Id.HasValue && postResult.Id > 0)
                {

                    return postResult.Id.Value;
                }
                else
                {
                    return 0;
                }



            }
            catch (Exception)
            {
                throw;
            }

        }
        public ActionResult AddFollowUpLink([Bind(Prefix = "FollowUpTab")] FollowUpCertificateVM followUpCertificateVM, List<TransactionLinkVM> Links, string transactionId)
        {
            try
            {

                bool? isOutboundInternal = (bool?)TempData["IsOutboundInternal"];

                if (isOutboundInternal.HasValue && isOutboundInternal.Value)
                {
                    followUpCertificateVM.transactionLinkVM.TransactionCategory = 256;
                    followUpCertificateVM.transactionLinkVM.TransactionCategoryName = "معاملة داخلية";
                }

                string message = string.Empty;
                List<TransactionLinkVM> linkVMs = new List<TransactionLinkVM>();
                Links = Links ?? new List<TransactionLinkVM>();
                List<TransactionLinkVM> transactionLinks = new List<TransactionLinkVM>();
                int nLinkTypeId = LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                if (followUpCertificateVM.transactionLinkVM.LinkTypeId == LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName))
                {
                    switch (followUpCertificateVM.transactionLinkVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                    {
                        case (int)TransactionCategory.Inbound:
                            {
                                nLinkTypeId = LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                            }
                            break;
                        case (int)TransactionCategory.InternalOutbound:
                            {
                                nLinkTypeId = LinkingType.WithReplyOutboundInternal.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                            }
                            break;
                        case (int)TransactionCategory.ExternalOutbound:
                            {
                                nLinkTypeId = LinkingType.WithReplyOutbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                            }
                            break;
                    }
                }
                else
                {
                    switch (followUpCertificateVM.transactionLinkVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                    {
                        case (int)TransactionCategory.Inbound:
                            {
                                if (followUpCertificateVM.transactionLinkVM.WithDocumentNumber.HasValue && followUpCertificateVM.transactionLinkVM.WithDocumentNumber.Value)
                                    nLinkTypeId = LinkingType.WithInboundDocumentNumber.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                                else
                                    nLinkTypeId = LinkingType.WithReferenceInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);


                            }
                            break;
                        case (int)TransactionCategory.InternalOutbound:
                            {
                                nLinkTypeId = LinkingType.WithReferenceOutboundInternal.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                            }
                            break;
                        case (int)TransactionCategory.ExternalOutbound:
                            {
                                nLinkTypeId = LinkingType.WithReferenceOutbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                            }
                            break;
                    }
                }

                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Links.GeneralLink))
                {
                    followUpCertificateVM.transactionLinkVM.OrgUnitId = -1;
                }
                else
                {
                    followUpCertificateVM.transactionLinkVM.OrgUnitId = SessionInfo.OrgUnitId;
                }

                var transaction =
                HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest(
                    string.Format("api/Transaction/GetTransactionIdByLinkType?sourceNumber={0}&orgUnitId={1}&yearId={2}&linkTypeId={3}&cultureName={4}&yearSearch={5}",
                    followUpCertificateVM.transactionLinkVM.TransactionNumber, followUpCertificateVM.transactionLinkVM.OrgUnitId, followUpCertificateVM.transactionLinkVM.Year, nLinkTypeId, SessionInfo.CultureShortName, null)).Result;

                if (transaction.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transaction.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else if (transactionId == transaction.Result.Id.ToString())
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionCycleLinked.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else if (Links.ToList().Where(l => l.TransactionId == transaction.Result.Id).FirstOrDefault() != null)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionDoubleLinked.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    followUpCertificateVM.transactionLinkVM.Key = Links.Count + 1;
                    followUpCertificateVM.transactionLinkVM.DateH = transaction.Result.HijriDate;
                    followUpCertificateVM.transactionLinkVM.Date = transaction.Result.Date.ToShortDateString();
                    followUpCertificateVM.transactionLinkVM.TransactionType = transaction.Result.TransactionsTypes;
                    followUpCertificateVM.transactionLinkVM.TransactionId = transaction.Result.Id;
                    followUpCertificateVM.transactionLinkVM.Subject = transaction.Result.Subject;


                    bool isPermition = false;
                    switch (transaction.Result.ConfidentialityId)
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
                        followUpCertificateVM.transactionLinkVM.Subject = "* * * *";
                        message = DbRes.TResource("PermissionAssignTo");
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }


                    followUpCertificateVM.transactionLinkVM.Year = transaction.Result.Year;
                    followUpCertificateVM.transactionLinkVM.ToTransactionId = followUpCertificateVM.transactionLinkVM.TransactionId;
                    followUpCertificateVM.transactionLinkVM.TransactionId = 0;
                    followUpCertificateVM.transactionLinkVM.TypeId = followUpCertificateVM.transactionLinkVM.LinkTypeId;
                    linkVMs.Add(followUpCertificateVM.transactionLinkVM);

                    PostResult postResult =
                       HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/FollowUpAddTransactionLinks?transactionId={0}", transactionId), linkVMs).Result;
                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                initialize(transactionCategory, followUpCertificateVM);
                buildtree(followUpCertificateVM.FollowUpEntityId);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Links.AddSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult UnLockFollowUp(int FollowUpId, int TransactionId)
        {
            string message = string.Empty;

            FollowUpChangeStatus(FollowUpId, (int)FollowupStatus.UnLockFollowup, true);

            AddFollowupUditTrial(FollowUpId, (int)FollowupAuditProcess.UnderProcessingFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.FollowUpStopWork"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);


        }
        [HttpGet]
        public ActionResult LockFollowUp(int FollowUpId, int TransactionId)
        {
            string message = string.Empty;

            FollowUpChangeStatus(FollowUpId, (int)FollowupStatus.UnderFollowup, true);
            FollowUpUpdateReceive(FollowUpId);
            AddFollowupUditTrial(FollowUpId, (int)FollowupAuditProcess.LockFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.FollowUpStartWork"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);


            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult DeactivateFollowUp(int FollowUpId, int TransactionId)
        {
            string message = string.Empty;

            FollowUpChangeStatus(FollowUpId, (int)FollowupStatus.Cancled, false);
            AddFollowupUditTrial(FollowUpId, (int)FollowupAuditProcess.CancelFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.FollowUpCanceled"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);

            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ComplitFollowUp(int FollowUpId, int TransactionId)
        {
            string message = string.Empty;
            FollowUpChangeStatus(FollowUpId, (int)FollowupStatus.EnsureComplition, false);

            AddFollowupUditTrial(FollowUpId, (int)FollowupAuditProcess.InsureCompletionFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.CompletionFollowup"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);



            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult EscalateFollowUp(int FollowUpId, int TransactionId)
        {
            string message = string.Empty;


            PostResult postResultFodept =
                        HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/GetChildFollowUpUserId?FollowUpId={0}", FollowUpId), null).Result;

            if (postResultFodept.Id.HasValue)
            {
                PostResult postResult =
              HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/EscalateFollowUp?FollowUpId={0}&TransactionId={1}&FollowUpUserID={2}&cultureName={3}", FollowUpId, TransactionId, postResultFodept.Id, SessionInfo.CultureShortName), null).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                AddFollowupUditTrial(FollowUpId, (int)FollowupAuditProcess.ScaltFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.ScaltFollowup"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            else
            {

                message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpNotRecivedYet");
                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult SendFollowUpReminder(int FollowUpId, int TransactionId, int FollowUoUserId)
        {
            string message = string.Empty;

            PostResult postResultFodept =
                        HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/GetChildFollowUpUserId?FollowUpId={0}", FollowUpId), null).Result;

            if (postResultFodept.Id.HasValue)
            {
                PostResult postResult =
              HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/SendFollowUpReminder?FollowUpId={0}&TransactionId={1}&FollowUoUserId={2}&cultureName={3}", FollowUpId, TransactionId, postResultFodept.Id, SessionInfo.CultureShortName), null).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                AddFollowupUditTrial(FollowUpId, (int)FollowupAuditProcess.ReminderFollowup, ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.SendFollowUpReminder"), SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId);

                message = DbRes.TValidation("User.Transaction.FollowUp.TheReminderHasBeenSentSuccessfully");
                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpOrgUnitDoesNotExist");
                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        private void AddFollowupUditTrial(int FollowUpId, int ProccessId, string ProccessDescription, int UserId, int EntityId)
        {

            FollowUpAuditTrailVM followUpAuditTrail = new FollowUpAuditTrailVM();
            followUpAuditTrail.FollowupId = FollowUpId;
            followUpAuditTrail.ProccessDate = DateTime.Now;
            followUpAuditTrail.ProccessId = ProccessId;
            followUpAuditTrail.ProccessDescription = ProccessDescription;

            followUpAuditTrail.UserId = UserId;
            followUpAuditTrail.EntityId = EntityId;
            PostResult postResultAudit =
            HttpClientWrapper<PostResult>.PostRequest("api/Transaction/AddFollowupUditTrial?cultureName=" + SessionInfo.CultureShortName, FollowUpAuditTrailMapper.Map(followUpAuditTrail)).Result;

        }
        private void FollowUpChangeStatus(int FollowUpId, int FollowupStatusId, bool Active)
        {
            string message = string.Empty;
            PutResult putResult =
                HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/FollowUpChangeStatus?Id={0}&FollowupStatus={1}&IsActive={2}", FollowUpId, FollowupStatusId, Active), null).Result;
            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                // return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

        }
        private void FollowUpUpdateEscalatedStatus(int FollowUpId, bool IsEscalated)
        {
            string message = string.Empty;
            PutResult putResult =
                HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/FollowUpUpdateEscalatedStatus?Id={0}&IsActive={1}", FollowUpId, IsEscalated), null).Result;
            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                // return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

        }
        private void FollowUpUpdateReminderStatus(int FollowUpId, bool IsReminder)
        {
            string message = string.Empty;
            PutResult putResult =
                HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/FollowUpUpdateReminderStatus?Id={0}&IsActive={1}", FollowUpId, IsReminder), null).Result;
            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                // return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult FollowUpLoadTransactionLinkGrid(int transactionId)
        {
            List<TransactionLinkVM> list = GetTransactionLinks(transactionId);

            if (list.Count == 0)
                list = new List<TransactionLinkVM>();

            var detailsList = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(list, 1, list.Count, false);
            return PartialView("~/Areas/User/Views/TransactionFollowUp/_FollowUpLinksGridPartial.cshtml", detailsList);


        }
        private void FollowUpUpdateReceive(int FollowUpId)
        {
            string message = string.Empty;
            PutResult putResult =
                HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/FollowUpUpdateReceive?Id={0}&UserId={1}", FollowUpId, SessionInfo.CurrentUser.Id), null).Result;
            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                // return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public ActionResult FollowUpLoadGrid(int transactionId)
        {


            List<TransactionFollowUpVM> list = GetTransactionFollowUps(transactionId);

            if (list.Count == 0)
                list = new List<TransactionFollowUpVM>();

            var detailsList = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(list, 1, list.Count, false);
            return PartialView("~/Areas/User/Views/TransactionFollowUp/_FollowUpGridPartial.cshtml", detailsList);


        }
        public ActionResult FollowUpLoadAuditTrailGrid(int FollowUpId)
        {
            List<FollowUpAuditTrailVM> list = GetFollowUpAuditTrails(FollowUpId);

            if (list.Count == 0)
                list = new List<FollowUpAuditTrailVM>();

            var detailsList = (AjaxGrid<FollowUpAuditTrailVM>)new AjaxGridFactory().CreateAjaxGrid(list, 1, list.Count, false);
            return PartialView("~/Areas/User/Views/TransactionFollowUp/_FollowUpAuditTrail.cshtml", detailsList);


        }
        List<TransactionLinkVM> GetTransactionLinks(int transactionId)
        {


            GetResult<List<TransactionLinkDTO>> transactionLinkDTOs =
                HttpClientWrapper<GetResult<List<TransactionLinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionLinks?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

            List<TransactionLinkVM> transactionLinkVMs = TransactionLinkMapper.Map(transactionLinkDTOs.Result);
            if (transactionLinkVMs == null)
            {
                transactionLinkVMs = new List<TransactionLinkVM>();
            }

            return transactionLinkVMs;
        }
        public string GetUnitUsers(int id)
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (userProfileDTOs.Result != null)
                {
                    foreach (UserProfileVM userProfileVM in userProfileVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName,
                            Parameters = new object[] { userProfileVM.UserImageId }
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
        public void buildtree(int FollowUpEntityId)
        {

            GetResult<List<OrgUnitDTO>> orgUnits =
            HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnits.Result);
            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);


            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(FollowUpEntityId, true);


            GetResult<UserPreferenceDTO> userPreferenceResult =
                   HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}&orgUnitId={2}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

            UserPreferenceVM userPreferenceVMS = UserPreferenceMapper.Map(userPreferenceResult.Result);
            if (userPreferenceVMS != null)
            {
                ViewData["FollowUpOrgId"] = userPreferenceVMS.FollowUpOrgId;
                ViewData["FollowUpUserId"] = userPreferenceVMS.FollowUpUserId;
            }

        }
        public static List<TransactionFollowUpVM> GetTransactionFollowUps(int TransactionId)
        {
            GetResult<List<TransactionFollowUpDTO>> dtoAPI =
               HttpClientWrapper<GetResult<List<TransactionFollowUpDTO>>>.GetItemRequest(string.Format("api/Transaction/TransactionFollowUpSelectByTransId?transId={0}&cultureName={1}", TransactionId, SessionInfo.CultureShortName)).Result;

            List<TransactionFollowUpVM> transactionFollowUpVMs = TransactionFollowUpMapper.Map(dtoAPI.Result);

            return transactionFollowUpVMs;

        }
        public static List<FollowUpAuditTrailVM> GetFollowUpAuditTrails(int FollowUpId)
        {
            GetResult<List<FollowUpAuditTrailDTO>> dtoAPI =
               HttpClientWrapper<GetResult<List<FollowUpAuditTrailDTO>>>.GetItemRequest(string.Format("api/Transaction/GetListFollowupUditTrial?id={0}&cultureName={1}", FollowUpId, SessionInfo.CultureShortName)).Result;

            List<FollowUpAuditTrailVM> followUpAuditTrailVMs = FollowUpAuditTrailMapper.Map(dtoAPI.Result);

            return followUpAuditTrailVMs;

        }
        void GetTransactionInfo(int id)
        {
            EditorViewModel editorViewModels = new EditorViewModel();
            var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                  .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

            editorViewModels.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
            ViewData["ConfidentialityName"] = editorViewModels.TransactionBasicInfoVM.ConfidentialityName;
            ViewData["PriorityLevel"] = editorViewModels.TransactionBasicInfoVM.PriorityName;
            ViewData["Subject"] = editorViewModels.TransactionBasicInfoVM.Subject;
            ViewData["TransactionsNumber"] = editorViewModels.TransactionBasicInfoVM.Number;
        }
        string GetFollowUpProccess(TransactionCategory transactionCategory)
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<FollowUpLookUpDTO>> followUpProccessDTOs =
                    HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Transaction/GetFollowUpProccess?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (followUpProccessDTOs.Result != null)
                {
                    foreach (FollowUpLookUpsVM ProccessVm in FollowUpLookUpsMapper.Map(followUpProccessDTOs.Result))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = ProccessVm.Id.ToString(),
                            Label = ProccessVm.LocalName
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
        string GetFollowUpPrioritytype(TransactionCategory transactionCategory)
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<FollowUpLookUpDTO>> followUpPrioritytypeDTOs =
                    HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Transaction/GetFollowUpPrioritytype?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (followUpPrioritytypeDTOs.Result != null)
                {
                    foreach (FollowUpLookUpsVM ProccessVm in FollowUpLookUpsMapper.Map(followUpPrioritytypeDTOs.Result))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = ProccessVm.Id.ToString(),
                            Label = ProccessVm.LocalName
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
        string GetFollowUpSource(TransactionCategory transactionCategory)
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<FollowUpLookUpDTO>> followUpSourceDTOs =
                    HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Transaction/GetFollowUpSource?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (followUpSourceDTOs.Result != null)
                {
                    foreach (FollowUpLookUpsVM ProccessVm in FollowUpLookUpsMapper.Map(followUpSourceDTOs.Result))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = ProccessVm.Id.ToString(),
                            Label = ProccessVm.LocalName
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
        string GetFollowUpMethod(TransactionCategory transactionCategory)
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<FollowUpLookUpDTO>> followUpMethodDTOs =
                    HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Transaction/GetFollowUpMethod?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (followUpMethodDTOs.Result != null)
                {
                    foreach (FollowUpLookUpsVM ProccessVm in FollowUpLookUpsMapper.Map(followUpMethodDTOs.Result))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = ProccessVm.Id.ToString(),
                            Label = ProccessVm.LocalName
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
        void initialize(TransactionCategory transactionCategory, FollowUpCertificateVM followUpCertificateVM)
        {

            ViewData["TransactionId"] = followUpCertificateVM.TransactionId;
            ViewData["FollowUpProccess"] = GetFollowUpProccess(transactionCategory);
            ViewData["FollowUpPrioritytype"] = GetFollowUpPrioritytype(transactionCategory);
            ViewData["FollowUpSource"] = GetFollowUpSource(transactionCategory);
            ViewData["FollowUpMethod"] = GetFollowUpMethod(transactionCategory);
            TempData["ControllerName"] = "TransactionFollowUp";
            ViewData["LinkTypeData"] = GetLinkTypes(transactionCategory);
            //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(followUpCertificateVM.transactionLinkVM.Links);

            List<FollowUpAuditTrailVM> AuditTraillist = GetFollowUpAuditTrails(followUpCertificateVM.FollowUpId);
            CustomAjaxGrid.IAjaxGrid AuditTrailgrid = (CustomAjaxGrid.AjaxGrid<FollowUpAuditTrailVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(AuditTraillist, 1, AuditTraillist.Count, false, GridHelper.PageSize);

            ViewData["AuditTrailGridData"] = AuditTrailgrid;
            //followUpCertificateVM.FollowUpAuditTrails = list;



            List<TransactionFollowUpVM> listDetails = GetTransactionFollowUps(followUpCertificateVM.TransactionId);

            CustomAjaxGrid.IAjaxGrid Detailsgrid = (CustomAjaxGrid.AjaxGrid<TransactionFollowUpVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(listDetails, 1, listDetails.Count, false, GridHelper.PageSize);

            ViewData["DetailsGridData"] = Detailsgrid;


            List<TransactionLinkVM> listtransactionLinks = GetTransactionLinks(followUpCertificateVM.TransactionId);

            CustomAjaxGrid.IAjaxGrid transactionLinkgrid = (CustomAjaxGrid.AjaxGrid<TransactionLinkVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(listtransactionLinks, 1, listtransactionLinks.Count, false, GridHelper.PageSize);

            ViewData["transactionLinkData"] = transactionLinkgrid;

        }



    }
}