using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Inbound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal;
using MCS.UI.Areas.User.Mappers.UserPreferences;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Notifications;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Common;
using MCS.UI.Helpers;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AssignmentPaperMapper = MCS.UI.Areas.User.Mappers.OrgUnit.AssignmentPaperMapper;
using OrgUnitMapper = MCS.UI.Areas.User.Mappers.OrgUnit.OrgUnitMapper;


namespace MCS.UI.Areas.User.Controllers
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    public class VipOutboundInternalController : TransactionController
    {





        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditInternalOutbound, UserClaims.Outbound.EditorInternalOutbound)]
        [CustomAction]
        public ActionResult Edit(string id, string defaultTabId)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                var editOutboundInternalDTO = HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest($"api/Vip/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (editOutboundInternalDTO.StatusCode != StatusCode.Ok)
                {


                    if (editOutboundInternalDTO.StatusCode.ToString().Contains("Permission"))
                    {
                        string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editOutboundInternalDTO.StatusCode.ToString());
                        return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                    }
                    else if (editOutboundInternalDTO.StatusCode == StatusCode.TransactionNotFound)
                    {
                        string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editOutboundInternalDTO.StatusCode.ToString());
                        TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                        return RedirectToAction("DashboardHome", "Shared");
                    }
                    else
                    {
                        throw new Exception(ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editOutboundInternalDTO.StatusCode.ToString()));
                    }
                }
                SetTransactionAssignmentToViewed(trxId);
                VIPEditOutboundInternalVM editOutboundInternalVM = OutboundInternalMapper.VIPMap(editOutboundInternalDTO.Result);

                Initialize(editOutboundInternalVM.SavedTransactionAssignment);
                InitializerAssignmentPaperData(trxId);

                ViewData["transactionId"] = editOutboundInternalVM.Id;
                List<TransactionArchiveVM> transactionArchiveVMs = FillTransactionArchiveVMs(editOutboundInternalVM);
                List<TransactionArchiveVM> transactionArchiveIncVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == false).ToList();
                editOutboundInternalVM.Archives = transactionArchiveIncVMs;
                ViewData["ConfidentialityId"] = editOutboundInternalVM.OutboundInternalBasicInfoEdit.ConfidentialityLevelId;
                ViewData["ArchiveListData"] = editOutboundInternalVM?.Archives != null ? editOutboundInternalVM.Archives.ToList() : new List<TransactionArchiveVM>();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();

                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = editOutboundInternalDTO.Result.FromUser.LocalName == editOutboundInternalDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;


                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                LogTransactionAction(AuditingActionCode.UpadteTransaction, editOutboundInternalVM.Id);

                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == editOutboundInternalVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Inbound.EditInbound));


                #region Add value to key Field

                for (int i = 0; i < editOutboundInternalVM.Archives.Count; i++)
                {
                    editOutboundInternalVM.Archives[i].Key = i + 1;
                }
                #endregion


                RemoveAllAttachemntsPhysically();

                editOutboundInternalVM.Id = trxId;
                ViewData["LinkTransactions"] = editOutboundInternalVM.Links.ToList();

                Session["TransactionId"] = trxId;

                return View(editOutboundInternalVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditInternalOutbound, UserClaims.Outbound.EditorInternalOutbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult Send(VipOutboundInternalUpdateVM outboundInternalVM, string explanationTxt, string selectedConfidentiality)
        {
            try
            {
                string message = string.Empty;
                int? TransactionAssignmentExplanationId = 0;
                outboundInternalVM.AssignmentVMs.Where(x => x.ActionId == 0).ForEach(x => x.ActionId = outboundInternalVM.AssignmentVMs.Where(a => a.ActionId > 0).FirstOrDefault().ActionId);
                byte[] data = DocumentViewerHelper.GetPDFFile(outboundInternalVM.hdnMainDocTokenId);
                outboundInternalVM.DocumentVM = new DocumentVM
                {
                    Content = data,
                    Size = data.Length,
                    MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,
                    FromUserId = SessionInfo.CurrentUser.Id,
                    FromEntityId = SessionInfo.OrgUnitId
                };
                #region 
                if (!string.IsNullOrWhiteSpace(explanationTxt))
                {
                    outboundInternalVM.ExplanationForAssignmentPaper = explanationTxt;
                }
                var EditedOutboundInternal = OutboundInternalMapper.VIPEditMap(outboundInternalVM);
                if (EditedOutboundInternal.PublicFollowUps != null && outboundInternalVM.PublicFollowUps.IsValid())
                {

                    PostResult postResultFodept =
                   HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/getFollowUpDepartment?EntityId={0}", SessionInfo.OrgUnitId), null).Result;

                    if (postResultFodept.Id.HasValue)
                    {
                        EditedOutboundInternal.PublicFollowUps.FollowUpEntityId = (int)postResultFodept.Id;

                    }
                    else
                    {

                        message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpOrgUnitDoesNotExist");
                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    }
                }


                if (outboundInternalVM.AssignmentVMs.Where(ta => ta.IsAssigned == true).Count() <= 0)
                {
                    message = "يرجى ادخال احالة";
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                }
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}",
                    outboundInternalVM.InboundId.ToString().Trim(',')),
                    TransactionAssignmentMapper.VipMap(outboundInternalVM.AssignmentVMs.Where(ta => ta.IsAssigned == true).ToList(), outboundInternalVM.ExplanationForAssignmentPaper, ""))
                    .Result;
                bool hasPermission = SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.AssignTransactionToUnauthorize);

                if (postResult.StatusCode != StatusCode.Ok && !outboundInternalVM.IsConfirmed)
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




                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Vip/SaveOutboundInternal"), EditedOutboundInternal).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }




                if (!string.IsNullOrWhiteSpace(explanationTxt))
                {
                    byte[] expdata = Encoding.Unicode.GetBytes(explanationTxt.Trim());
                    ExplanationVM explanationVM = new ExplanationVM()
                    {
                        Description = explanationTxt,
                        ConfidentialityId = int.Parse(selectedConfidentiality),
                        FromUserId = SessionInfo.CurrentUser.Id,
                        EditorType = EditorType.Text,
                        DocumentVM = new Areas.User.Models.Shared.DocumentVM()
                        {
                            MimeType = System.Net.Mime.MediaTypeNames.Text.Plain,
                            Content = expdata,
                            Size = expdata.Length,
                            FromEntityId = SessionInfo.OrgUnitId,
                            FromUserId = SessionInfo.CurrentUser.Id
                        }
                    };

                    PostResult postExplanationResult = HttpClientWrapper<PostResult>
                        .PostRequest(string.Format("api/Transaction/AddTransactionExplanation?transactionId={0}", EditedOutboundInternal.Id),
                            ExplanationMapper.Map(explanationVM)).Result;

                    if (postExplanationResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postExplanationResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                    TransactionAssignmentExplanationId = postExplanationResult.Id;
                }
                PutResult UpdateTransactionAssignmentHistory = HttpClientWrapper<PutResult>
                         .PutRequest(string.Format("api/Transaction/UpdateTransactionAssignmentHistory?transactionId={0}&ExplanationId={1}",
                           EditedOutboundInternal.Id,
                           TransactionAssignmentExplanationId),
                           null).Result;

                GetResult<VipBasicTransactionInfoDto> trayDetailsDTO =
              HttpClientWrapper<GetResult<VipBasicTransactionInfoDto>>.GetItemRequest(string.Format("api/Vip/GetNextTransactionId?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}&OrderBy={5}",
              SessionInfo.OrgUnitId, (int)TrayType.MyTransactions, 1, 1, SessionInfo.CultureShortName, "Number")).Result;
                string controller = "";
                int? nextId = (int?)null;
                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                if (trayDetailsDTO.Result != null && trayDetailsDTO.Result.Id > 0)
                {
                    int inboundType = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                    int internalOutboundType = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                    nextId = trayDetailsDTO.Result.Id;
                    if ((int)trayDetailsDTO.Result.TransactionCategory == inboundType)
                    {
                        controller = "VipInbound";
                    }
                    else if ((int)trayDetailsDTO.Result.TransactionCategory == internalOutboundType)
                    {
                        controller = "VipInternalOutbound";
                    }

                }

                Session["DocumentData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                Session["DocoNutexplanations"] = null;
                #endregion


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundInternal.UpdateSucceeded");
                return Json(new
                {
                    NextId = nextId,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt"),
                    EncryptedId = AESEncrytDecry.Base64Encode(outboundInternalVM.Id.ToString()),
                    MessageText = message,
                    MessageType = MessageType.Information,
                    Controller = controller

                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult OutboundInternalAssignItBack(VipOutboundInternalUpdateVM inboundEditVM)
        {
            string message = string.Empty;
            MessageType messageType = MessageType.Information;
            PostResult postResult = null;
            try
            {
                postResult = HttpClientWrapper<PostResult>
                                                 .PostRequest($"api/MobileApi/AssignItBackVip?TransId={inboundEditVM.InboundId}&Notes={inboundEditVM.ExplanationForAssignmentPaper}&userId={SessionInfo.CurrentUser.Id}&entityId={SessionInfo.OrgUnitId}", SessionInfo.CultureShortName)
                                                 .Result;


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }

                GetResult<VipBasicTransactionInfoDto> trayDetailsDTO =
           HttpClientWrapper<GetResult<VipBasicTransactionInfoDto>>.GetItemRequest(string.Format("api/Vip/GetNextTransactionId?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}&OrderBy={5}",
           SessionInfo.OrgUnitId, (int)TrayType.MyTransactions, 1, 1, SessionInfo.CultureShortName, "Number")).Result;

                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                string controller = "VipInbound";
                int? nextId = (int?)null;
                if (trayDetailsDTO.Result != null && trayDetailsDTO.Result.Id > 0)
                {
                    nextId = trayDetailsDTO.Result.Id;
                    switch (trayDetailsDTO.Result.TransactionCategory)
                    {
                        case TransactionCategory.Inbound:
                            controller = "VipInbound";
                            break;
                        case TransactionCategory.InternalOutbound:
                            controller = "VipOutboundInternal";
                            break;
                    }
                }

                Session["DocumentData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                Session["DocoNutexplanations"] = null;


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundInternal.UpdateSucceeded");
                return Json(new
                {
                    NextId = nextId,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt"),
                    MessageText = message,
                    MessageType = MessageType.Information,
                    Controller = controller

                }, JsonRequestBehavior.AllowGet);



                return Json(new { MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        private void Initialize(string savedTransactionAssignment)
        {
            try
            {
                Session["IsEditMode"] = true;
                ViewData["IsEditMode"] = true;
                ViewData["DeliveryMethod"] = GetDelivery(false);
                GetResult<UserPreferenceDTO> userPreferenceResult =
               HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}&orgUnitId={2}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                UserPreferenceVM userPreferenceVMS = UserPreferenceMapper.Map(userPreferenceResult.Result);
                if (userPreferenceVMS != null)
                {
                    ViewData["FollowUpOrgId"] = userPreferenceVMS.FollowUpOrgId;
                    ViewData["FollowUpUserId"] = userPreferenceVMS.FollowUpUserId;
                    ViewData["FollowUpOrgUnitUsersData"] = userPreferenceVMS.FollowUpOrgId.HasValue ? GetUsersByOrgUnitId(userPreferenceVMS.FollowUpOrgId.Value) : null;
                    ViewData["FollowUpProccess"] = GetFollowUpProccess(TransactionCategory.InternalOutbound);
                    ViewData["FollowupPeriod"] = FollowupPeriod(TransactionCategory.InternalOutbound);
                }
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities(TransactionCategory.InternalOutbound);
                ViewData["PrivecyLevelsData"] = TransactionHelper.GetPrivecyLevels(TransactionCategory.InternalOutbound);
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["ControllerName"] = "VipInternalOutbound";
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActionsVIP();
                var transactionAssignmentVMs = GetAssignmentPaper_VIP();
                List<VIPTransactionAssignmentVM> assignmentSaved = null;
                if (!string.IsNullOrWhiteSpace(savedTransactionAssignment))
                {
                    assignmentSaved = JsonConvert.DeserializeObject<List<VIPTransactionAssignmentVM>>(savedTransactionAssignment);
                    assignmentSaved.ForEach(x => x.DeliveryMethodId = 236);
                }

                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(savedTransactionAssignment) ?
assignmentSaved : transactionAssignmentVMs;
                //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;
                IAjaxGrid gridAssignmentPaper = (AjaxGrid<VIPTransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;
                ViewData["FollowupOrgUnitData"] = orgUnitDTOs.Result.Where(x => x.FollowupDepartment.HasValue && x.FollowupDepartment.Value > 0).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        private List<TransactionArchiveVM> FillTransactionArchiveVMs(VIPEditOutboundInternalVM editInboundVM)
        {
            Session["DocoNutDocument"] = null;
            List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();
            if (editInboundVM.DocumentVM != null)
            {
                string documentId = Guid.NewGuid().ToString();
                var transactionArchiveVM = new TransactionArchiveVM
                {
                    Id = documentId,
                    IsMainDocument = true,
                    DocumentId = editInboundVM.DocumentVM.Id,
                    EncryptDocumentId = AESEncrytDecry.Base64Encode(editInboundVM.DocumentVM.Id.ToString()),
                    AttachmentTypeId = -1,
                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text,
                    IsNew = true,
                    Number = int.Parse(editInboundVM.DocumentVM.Number)
                };
                transactionArchiveVMs.Add(transactionArchiveVM);


                Session["DocoNutDocument"] = editInboundVM.DocumentVM.Content;
            }

            if (editInboundVM.Attachments != null && editInboundVM.Attachments.Count > 0)
            {


                foreach (TransactionAttachmentVM item in editInboundVM.Attachments)
                {
                    TransactionArchiveVM Archive = new TransactionArchiveVM
                    {
                        Number = item.Number,
                        AttachmentTypeId = item.TypeId,
                        ArcivingTypeName = item.TypeName,
                        Archivable = item.Archivable,
                        AttachmentName = item.AttachmentName,
                        IsEnableAction = item.IsEnableAction,
                        JFile = item.Archivable ? TransactionAttachmentMapper.GetArchivingFileDate(item) : string.Empty,
                        UserId = item.UserId,
                        ReadOnly = !(item.UserId == SessionInfo.CurrentUser.Id),

                    };

                    if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                    {
                        Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                        Archive.Id = item.Id.ToString();//Guid.NewGuid().ToString();
                        Archive.DocumentId = item.DocumentVM.Id;
                        Archive.AttachmentTypeId = item.TypeId;
                        Archive.ArcivingTypeName = item.TypeName;
                        Archive.IsNew = true;
                        Archive.IsDeleted = item.DocumentVM.IsDeleted;
                        Archive.AttachmentSource = item.AttachmentSource;
                        Archive.FileName = item.DocumentVM.Name;
                        Archive.FromEntityId = item.DocumentVM.FromEntityId;
                        Archive.FromUserId = item.DocumentVM.FromUserId;

                    }
                    transactionArchiveVMs.Add(Archive);
                }
            }
            return transactionArchiveVMs;
        }
    }
}