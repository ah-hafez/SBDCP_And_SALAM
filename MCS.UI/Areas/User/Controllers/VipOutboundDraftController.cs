using MCS.Common;
using MCS.Common.ApiControllerResults;
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
using MCS.UI.Areas.User.Mappers.Transaction.OutBound;
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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using AssignmentPaperMapper = MCS.UI.Areas.User.Mappers.OrgUnit.AssignmentPaperMapper;
using OrgUnitMapper = MCS.UI.Areas.User.Mappers.OrgUnit.OrgUnitMapper;


namespace MCS.UI.Areas.User.Controllers
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    public class VipOutboundDraftController : TransactionController
    {





        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditDraft, UserClaims.Outbound.CreateOutboundDraftPresentation)]
        [CustomAction]
        public ActionResult Edit(string id)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                var editOutboundInternalDTO = HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest($"api/Vip/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
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
                VIPEditOutboundDraftVM editOutboundDraftVM = OutboundDraftMapper.VIPMap(editOutboundInternalDTO.Result);

                VIPTextEditorViewModel editorViewModel = new VIPTextEditorViewModel();

                InitializerAssignmentPaperData(trxId);

                string documentId = Guid.NewGuid().ToString();

                editOutboundDraftVM.EditorType = EditorType.TextEditor;

                if (editOutboundDraftVM?.OldDocumentVM?.Content != null)
                {
                    editorViewModel.OldDocumentBase64String = Convert.ToBase64String(editOutboundDraftVM.OldDocumentVM.Content);
                    editorViewModel.OldDocumentId = editOutboundDraftVM.OldDocumentVM.Id;
                }

                editorViewModel.IsShowWordAddIn = true;


                ViewData["hdnDocumentId"] = editOutboundDraftVM?.DocumentVM?.Id ?? 0;
                ViewData["EditorViewModel"] = editorViewModel;


                Initialize();
                ViewData["transactionId"] = editOutboundDraftVM.Id;
                List<TransactionArchiveVM> transactionArchiveVMs = FillTransactionArchiveVMs(editOutboundDraftVM);
                List<TransactionArchiveVM> transactionArchiveIncVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == false).ToList();
                editOutboundDraftVM.Archives = transactionArchiveIncVMs;
                ViewData["ConfidentialityId"] = editOutboundDraftVM.OutboundDraftBasicInfo.ConfidentialityLevelId;
                ViewData["ArchiveListData"] = editOutboundDraftVM?.Archives != null ? editOutboundDraftVM.Archives.ToList() : new List<TransactionArchiveVM>();

                LogTransactionAction(AuditingActionCode.UpadteTransaction, editOutboundDraftVM.Id);

                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == editOutboundDraftVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Inbound.EditInbound));
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();

                #region Add value to key Field

                for (int i = 0; i < editOutboundDraftVM.Archives.Count; i++)
                {
                    editOutboundDraftVM.Archives[i].Key = i + 1;
                }
                #endregion


                RemoveAllAttachemntsPhysically();

                editOutboundDraftVM.Id = trxId;
                ViewData["LinkTransactions"] = editOutboundDraftVM.Links.ToList();

                Session["TransactionId"] = trxId;

                return View(editOutboundDraftVM);
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditDraft, UserClaims.Outbound.CreateOutboundDraftPresentation)]
        [ValidateAntiForgeryToken()]
        public ActionResult Send(VipOutboundDraftUpdateVM outboundDraftUpdateVM, string explanationTxt, string selectedConfidentiality)
        {
            try
            {
                byte[] data = DocumentViewerHelper.GetPDFFile(outboundDraftUpdateVM.hdnMainDocToken);
                string message = string.Empty;
                int? TransactionAssignmentExplanationId = 0;

                #region 
                outboundDraftUpdateVM.DocumentBase64String = Convert.ToBase64String(data);
                if (!string.IsNullOrWhiteSpace(explanationTxt))
                {
                    outboundDraftUpdateVM.ExplanationForAssignmentPaper = explanationTxt;
                }
                var EditedOutboundInternal = OutboundDraftMapper.VIPEditMap(outboundDraftUpdateVM);
                if (outboundDraftUpdateVM.PublicFollowUps != null && outboundDraftUpdateVM.PublicFollowUps.IsValid())
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


                if (outboundDraftUpdateVM.AssignmentVMs.Where(ta => ta.IsAssigned == true).Count() <= 0)
                {
                    message = "يرجى ادخال احالة";
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                }

                PostResult postResult = HttpClientWrapper<PostResult>
                    .PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", outboundDraftUpdateVM.OutboundDraftId.ToString().Trim(',')), TransactionAssignmentMapper.VipMap(outboundDraftUpdateVM.AssignmentVMs.Where(ta => ta.IsAssigned == true).ToList(), outboundDraftUpdateVM.ExplanationForAssignmentPaper, "")).Result;
                bool hasPermission = SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.AssignTransactionToUnauthorize);

                if (postResult.StatusCode != StatusCode.Ok && !outboundDraftUpdateVM.IsConfirmed)
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

                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Vip/SaveOutboundDraft"), EditedOutboundInternal).Result;


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


                if (outboundDraftUpdateVM.IsSigned)
                {
                    ConvertDraftToOutbound(outboundDraftUpdateVM.OutboundDraftId, data, false);
                }

                GetResult<VipBasicTransactionInfoDto> trayDetailsDTO =
              HttpClientWrapper<GetResult<VipBasicTransactionInfoDto>>.GetItemRequest(string.Format("api/Vip/GetNextTransactionId?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}&OrderBy={5}",
              SessionInfo.OrgUnitId, (int)TrayType.DraftOutbound, 1, 1, SessionInfo.CultureShortName, "Number")).Result;

                string controller = "VipOutboundDraft";
                int? nextId = (int?)null;
                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTO.StatusCode.ToString());


                }
                else if (trayDetailsDTO.Result != null && trayDetailsDTO.Result.Id > 0)
                {
                    nextId = trayDetailsDTO.Result.Id;
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
                    EncryptedId = AESEncrytDecry.Base64Encode(outboundDraftUpdateVM.Id.ToString()),
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
        public ActionResult DraftAssignItBack(VipOutboundDraftUpdateVM inboundEditVM)
        {
            string message = string.Empty;
            MessageType messageType = MessageType.Information;
            PostResult postResult = null;
            try
            {
                postResult = HttpClientWrapper<PostResult>
                                                 .PostRequest($"api/Transaction/AssignItBackWithTray?TransId={inboundEditVM.OutboundDraftId}&Notes={inboundEditVM.ExplanationForAssignmentPaper}&userId={SessionInfo.CurrentUser.Id}&entityId={SessionInfo.OrgUnitId}&trayId={(int)TrayType.DraftOutbound}", SessionInfo.CultureShortName)
                                                 .Result;


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }



                int? nextId = (int?)null;
                GetResult<VipBasicTransactionInfoDto> trayDetailsDTO =
       HttpClientWrapper<GetResult<VipBasicTransactionInfoDto>>.GetItemRequest(string.Format("api/Vip/GetNextTransactionId?orgUnitId={0}&trayType={1}&PageIndex={2}&PageSize={3}&CultureName={4}&OrderBy={5}",
       SessionInfo.OrgUnitId, (int)TrayType.DraftOutbound, 1, 1, SessionInfo.CultureShortName, "Number")).Result;
                if (trayDetailsDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayDetailsDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else if (trayDetailsDTO.Result != null && trayDetailsDTO.Result.Id > 0)
                {
                    nextId = trayDetailsDTO.Result.Id;

                }

                string controller = "VipOutboundDraft";
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



        private void Initialize()
        {
            try
            {

                ViewData["DeliveryMethod"] = GetDelivery(false);
                GetResult<UserPreferenceDTO> userPreferenceResult =
               HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}&orgUnitId={2}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                UserPreferenceVM userPreferenceVMS = UserPreferenceMapper.Map(userPreferenceResult.Result);
                if (userPreferenceVMS != null)
                {
                    ViewData["FollowUpOrgId"] = userPreferenceVMS.FollowUpOrgId;
                    ViewData["FollowUpUserId"] = userPreferenceVMS.FollowUpUserId;
                    ViewData["FollowUpOrgUnitUsersData"] = userPreferenceVMS.FollowUpOrgId.HasValue ? GetUsersByOrgUnitId(userPreferenceVMS.FollowUpOrgId.Value) : null;
                    ViewData["FollowUpProccess"] = GetFollowUpProccess(TransactionCategory.DraftOutbound);
                    ViewData["FollowupPeriod"] = FollowupPeriod(TransactionCategory.DraftOutbound);
                }
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities(TransactionCategory.DraftOutbound);
                ViewData["PrivecyLevelsData"] = TransactionHelper.GetPrivecyLevels(TransactionCategory.DraftOutbound);
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["ControllerName"] = "VipDraftOutbound";
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActionsVIP();
                var transactionAssignmentVMs = GetAssignmentPaper_VIP();
                ViewData["AssignmentPaperData"] = transactionAssignmentVMs;
                IAjaxGrid gridAssignmentPaper = (AjaxGrid<VIPTransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;
                ViewData["FollowupOrgUnitData"] = orgUnitDTOs.Result.Where(x => x.FollowupDepartment.HasValue && x.FollowupDepartment.Value > 0).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<TransactionArchiveVM> FillTransactionArchiveVMs(VIPEditOutboundDraftVM editInboundVM)
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


                Session["DocoNutDocument"] = editInboundVM?.DocumentVM?.Content;
            }
            ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
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

        private void ConvertDraftToOutbound(int transactionId, byte[] data, bool isDecisionDraft)
        {
            //int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));

            string message = string.Empty;

            //int oldWordDocument = mainDocumentDTO.Result.Id;

            PutResult putResultConvertDraftToOutbound = HttpClientWrapper<PutResult>
                                                     .PutRequest(string.Format("api/Transaction/ConvertDraftToOutbound?draftTransactionId={0}", transactionId), new { })
                                                     .Result;

            if (putResultConvertDraftToOutbound.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResultConvertDraftToOutbound.StatusCode.ToString());
                throw new Exception(message);
            }


            byte[] content = null;
            if (!isDecisionDraft)
            {
                var barcode = GetBarcodeByte(transactionId);
                content = addImageToPDF(data, barcode, Convert.ToInt32(ConfigurationManager.AppSettings["BarcodePxMaxWidth"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["BarcodePxMaxHeight"].ToString()));

            }

            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/UpdateMainDocument_New?transactionId={0}", transactionId), content).Result;


            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                throw new Exception(message);
            }




        }





    }
}