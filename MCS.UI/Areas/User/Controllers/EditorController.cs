using Newtonsoft.Json;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.File;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Common;
using ActionMapper = MCS.UI.Areas.User.Mappers.Action.ActionMapper;
using AssignmentPaperMapper = MCS.UI.Areas.User.Mappers.OrgUnit.AssignmentPaperMapper;
using AttachmentTypeMapper = MCS.UI.Areas.User.Mappers.Lookups.AttachmentTypeMapper;
using FormMapper = MCS.UI.Areas.User.Mappers.Lookups.FormMapper;
using LetterTypeMapper = MCS.UI.Areas.User.Mappers.Lookups.LetterTypeMapper;
using LinkMapper = MCS.UI.Areas.User.Mappers.Lookups.LinkMapper;
using OrgUnitMapper = MCS.UI.Areas.User.Mappers.OrgUnit.OrgUnitMapper;
using PermissionMapper = MCS.UI.Areas.User.Mappers.Permission.PermissionMapper;
using SubjectClassificationMapper = MCS.UI.Areas.User.Mappers.Lookups.SubjectClassificationMapper;
using SuggestedTopicMapper = MCS.UI.Areas.User.Mappers.Lookups.SuggestedTopicMapper;
using UserProfileMapper = MCS.UI.Areas.User.Mappers.UserProfile.UserProfileMapper;
using CustomGridMvc = MCS.GridMvc.Ajax.GridExtensions;
using DocumentMapper = MCS.UI.Areas.User.Mappers.Shared.DocumentMapper;
using ZXing;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using System.Security.Policy;
using DotnetDaddy.DocumentViewer.License;
using MCS.Framework.MultiTenants;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace MCS.UI.Areas.User
{
    public class EditorController : BaseController
    {
        [HttpGet]
        //[CustomAuthorizationAttribute(UserClaims.Editor.ViewEditor)]
        [CustomAction]
        public ActionResult Index(string transactionId, string transactionCopyId, int transactionCategoryId, int trayId, bool IsVip = false)
        {
            try
            {

                Session["IsEditMode"] = false;
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                int trxCopyId = 0;
                if (!string.IsNullOrEmpty(transactionCopyId))
                    trxCopyId = int.Parse(StringCipher.DecryptStringAES(transactionCopyId.Replace(" ", "+")));
                switch ((TransactionCategory)transactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    case TransactionCategory.Inbound:
                        if (trayId == (int)TrayType.Copies || trayId == (int)TrayType.SpecialCopies || trayId == (int)TrayType.CopiesOutbound || trayId == (int)TrayType.Manager || trayId == (int)TrayType.OrgUnit || trayId == (int)TrayType.SentTransactions)
                        {
                            return InitializeInboundRead(trxId, trxCopyId);
                        }
                        else
                        {
                            return InitializeInboundReadWrite(trxId, trxCopyId);
                        }
                    case TransactionCategory.InternalOutbound:
                    case TransactionCategory.ExternalOutbound:
                        if (trayId == (int)TrayType.Copies || trayId == (int)TrayType.SpecialCopies || trayId == (int)TrayType.CopiesOutbound || trayId == (int)TrayType.Manager || trayId == (int)TrayType.OrgUnit || trayId == (int)TrayType.SentTransactions)
                        {
                            return InitializeOutboundInternalRead(trxId, trxCopyId);
                        }
                        else
                        {
                            return InitializeOutboundInternalReadWrite(trxId, trxCopyId);
                        }
                    case TransactionCategory.DraftOutbound:
                        if (trayId == (int)TrayType.Copies || trayId == (int)TrayType.SpecialCopies || trayId == (int)TrayType.CopiesOutbound || trayId == (int)TrayType.Manager || trayId == (int)TrayType.OrgUnit || trayId == (int)TrayType.SentTransactions)
                        {
                            return InitializeOutboundDraftRead(trxId, trxCopyId);
                        }
                        else
                        {
                            return InitializeOutboundDraftReadWrite(trxId, trxCopyId);
                        }
                }

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveMainDocument(int hdnTransactionId, DocumentVM documentVM, string param)
        {
            try
            {
                string message = string.Empty;

                byte[] data = DocumentViewerHelper.GetPDFFile(param);

                documentVM.Content = data;
                documentVM.Size = data.Length;
                documentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;

                PutResult postResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/UpdateMainDocument?transactionId={0}", hdnTransactionId), DocumentMapper.Map(documentVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Archiving.AddSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }


            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateDraftBasicInfo(int hdnTransactionId, [Bind(Prefix = "EditorBasicInfo")] TransactionBasicInfoVM transactionBasicInfoVM)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/PutTransactionBasicInfo?transactionId={0}", hdnTransactionId), TransactionBasicInfoMapper.Map(transactionBasicInfoVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundDraft.UpdateSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public ActionResult GetUserDelegationsById(int? UserId)
        {
            try
            {
                if (UserId.HasValue)
                {
                    GetResult<List<UserDelegationDTO>> UserDelegationDTO = HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserDelegationsById?UserId={0}&cultureName={1}", UserId, SessionInfo.CultureShortName)).Result;
                    return Json(new { result = UserDelegationDTO.Result }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
                // return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_TransactionPrintAll.cshtml", documentVM), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Initialization
        public ActionResult InitializeInboundRead(int transactionId, int trxCopyId)
        {
            try
            {
                EditorViewModel editorViewModel = new EditorViewModel();
                ViewData["TransactionId"] = transactionId;
                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", transactionId, SessionInfo.CultureShortName, trxCopyId)).Result;

                editorViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                editorViewModel.DocumentVM = TransactionHelper.GetMainDocument(transactionId);

                string editorMainDocumentSessionKey = Guid.NewGuid().ToString();
                ViewData["SessionArchiveDocumentKey"] = "DocoNutDocument";
                ViewData["TransactionNumber"] = editorViewModel.TransactionBasicInfoVM.Number;

                GetResult<List<TransactionCopyDTO>> transactionCopyDTOs =
                       HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                editorViewModel.TransactionCopyVM.Copies = TransactionCopyMapper.Map(transactionCopyDTOs.Result);
                editorViewModel.TransactionBasicInfoVM.TransactionId = transactionId;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                GetAssignmentCopis(editorViewModel.TransactionBasicInfoVM.DeliveryMethodId);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                InitializerAssignmentPaperData(transactionId);

                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(TransactionCopyMapper.Map(transactionCopyDTOs.Result));
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);

                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = FollowUpProccess();
                ViewData["FollowupPeriod"] = FollowupPeriod();
                editorViewModel.Archives = GetListArchives(editorViewModel.TransactionBasicInfoVM.Attachments);
                EditorInitializer(editorViewModel);
                DoconutInitializer(transactionId);

                return View("~/Areas/User/Views/Editor/EditorInbound/Read/Index.cshtml", editorViewModel);

            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<TransactionArchiveVM> GetListArchives(List<TransactionAttachmentVM> attachmentVMs)
        {

            List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();
            if (attachmentVMs != null && attachmentVMs.Count > 0)
            {

                foreach (TransactionAttachmentVM item in attachmentVMs)
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
                        AttachmentSource = item.AttachmentSource,
                        IsNew = true,
                        Id = item.Id.ToString()//Guid.NewGuid().ToString();
                    };



                    if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                    {
                        Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                        Archive.DocumentId = item.DocumentVM.Id;
                        Archive.IsDeleted = item.DocumentVM.IsDeleted;
                        Archive.FileName = item.DocumentVM.Name;
                        Archive.FromEntityId = item.DocumentVM.FromEntityId;
                        Archive.FromUserId = item.DocumentVM.FromUserId;
                    }
                    transactionArchiveVMs.Add(Archive);
                }
            }
            return transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();

        }
        public ActionResult InitializeInboundReadWrite(int transactionId, int trxCopyId)
        {
            try
            {
                EditorViewModel editorViewModel = new EditorViewModel();

                ViewData["TransactionId"] = transactionId;

                GetResult<TransactionBasicInfoDTO> transactionBasicInfoDTO =
                        HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", transactionId, SessionInfo.CultureShortName, trxCopyId)).Result;

                editorViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                editorViewModel.DocumentVM = TransactionHelper.GetMainDocument(transactionId);

                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                ViewData["EditorMainDocumentSessionKey"] = "DocoNutDocument";

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, false);

                ViewData["TasksGridData"] = grid;
                ViewData["CurrentTransactionTasksGrid"] = GetCurrentTransactionTasks(transactionId);

                GetResult<List<TransactionLinkDTO>> transactionLinkDTOs =
                    HttpClientWrapper<GetResult<List<TransactionLinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionLinks?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                List<TransactionLinkVM> transactionLinkVMs = TransactionLinkMapper.Map(transactionLinkDTOs.Result);
                if (transactionLinkVMs == null)
                {
                    transactionLinkVMs = new List<TransactionLinkVM>();
                }

                IAjaxGrid gridLinks = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs.ToList(), 1, 0, false);

                ViewData["LinksData"] = gridLinks;
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(TransactionLinkMapper.Map(transactionLinkDTOs.Result));

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                TransactionCategory TransactionCategory = (TransactionCategory)TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result).TransactionCategoryId.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName);

                ViewData["LinkTypeData"] = TransactionHelper.GetLinkTypes(TransactionCategory);

                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                    HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitBeneficiaries?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result), 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

                ViewData["AssignmentPaperData"] = JsonConvert.SerializeObject(transactionAssignmentDTOs.Result);

                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

                AssignmentPaperDTO assignmentPaperDTO = AssignmentPaperMapper.Map(TransactionHelper.GetAssignmentPaperByOrgUnitId());

                if (assignmentPaperDTO != null)
                {
                    editorViewModel.AssignmentPaperVM = AssignmentPaperMapper.Map(assignmentPaperDTO);

                    IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions, 1, AssignmentPaperMapper.Map(assignmentPaperDTO).Actions.Count, true);

                    ViewData["ActionGridData"] = actionGridData;
                    ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions);

                    IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Beneficiaries, 1, assignmentPaperDTO.Beneficiaries.Count, true);

                    ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                    ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(assignmentPaperDTO.Beneficiaries);
                }
                else
                {
                    editorViewModel.AssignmentPaperVM = new AssignmentPaperVM();

                    IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperActionVM>(), 1, 0, true);
                    ViewData["ActionGridData"] = actionGridData;
                    ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(new List<AssignmentPaperActionVM>());

                    IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperBeneficiaryVM>(), 1, 0, true);
                    ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                    ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(new List<AssignmentPaperBeneficiaryVM>());
                }

                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AssignmentGroupData"] = TransactionHelper.GetUserAssignmentGroups();
                ViewData["HasAssignmentPaper"] = TransactionHelper.CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = TransactionHelper.CheckOrgUnitIsAllowedToCreateGroup();
                ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

                if (!string.IsNullOrEmpty(ViewData["AllActionsData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["AllActionsData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }
                if (!string.IsNullOrEmpty(ViewData["AllExternalActionsData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["AllExternalActionsData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }

                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;
                InitializerAssignmentPaperData(transactionId);

                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = FollowUpProccess();
                ViewData["FollowupPeriod"] = FollowupPeriod();
                ViewData["ControllerName"] = "Editor";
                editorViewModel.Archives = GetListArchives(editorViewModel.TransactionBasicInfoVM.Attachments);
                EditorInitializer(editorViewModel);
                editorViewModel.TransactionBasicInfoVM.TransactionId = transactionId;
                return View("~/Areas/User/Views/Editor/EditorInbound/ReadWrite/Index.cshtml", editorViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult InitializeOutboundDraftRead(int transactionId, int trxCopyId)
        {
            try
            {
                EditorViewModel editorViewModel = new EditorViewModel();
                ViewData["TransactionId"] = transactionId;

                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", transactionId, SessionInfo.CultureShortName, trxCopyId)).Result;

                editorViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                editorViewModel.DocumentVM = TransactionHelper.GetMainDocument(transactionId);


                GetResult<List<TransactionCopyDTO>> transactionCopyDTOs =
                       HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                editorViewModel.TransactionCopyVM.Copies = TransactionCopyMapper.Map(transactionCopyDTOs.Result);

                IAjaxGrid grid = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionCopyMapper.Map(transactionCopyDTOs.Result), 1, transactionCopyDTOs.Result.Count, true);

                ViewData["CopiesData"] = grid;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                InitializerAssignmentPaperData(transactionId);

                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(TransactionCopyMapper.Map(transactionCopyDTOs.Result));
                ViewData["EditorMainDocumentSessionKey"] = "DocoNutDocument";
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = FollowUpProccess();
                ViewData["FollowupPeriod"] = FollowupPeriod();
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                editorViewModel.Archives = GetListArchives(editorViewModel.TransactionBasicInfoVM.Attachments);
                EditorInitializer(editorViewModel);
                DoconutInitializer(transactionId);
                editorViewModel.TransactionBasicInfoVM.TransactionId = transactionId;
                return View("~/Areas/User/Views/Editor/EditorOutboundDraft/Read/Index.cshtml", editorViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult InitializeOutboundDraftReadWrite(int transactionId, int trxCopyId)
        {
            try
            {
                EditorViewModel editorViewModel = new EditorViewModel();
                ViewData["TransactionId"] = transactionId;

                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", transactionId, SessionInfo.CultureShortName, trxCopyId)).Result;

                editorViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                editorViewModel.DocumentVM = TransactionHelper.GetMainDocument(transactionId);

                ViewData["EditorMainDocumentSessionKey"] = "DocoNutDocument";
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();


                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, false);
                ViewData["TasksGridData"] = grid;

                ViewData["CurrentTransactionTasksGrid"] = GetCurrentTransactionTasks(transactionId);

                TransactionCategory transactionCategory = (TransactionCategory)TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result).TransactionCategoryId.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName);

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                    HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitBeneficiaries?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result), 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

                ViewData["AssignmentPaperData"] = JsonConvert.SerializeObject(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result));

                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

                AssignmentPaperDTO assignmentPaperDTO = AssignmentPaperMapper.Map(TransactionHelper.GetAssignmentPaperByOrgUnitId());

                if (assignmentPaperDTO != null)
                {
                    editorViewModel.AssignmentPaperVM = AssignmentPaperMapper.Map(assignmentPaperDTO);

                    IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions, 1, AssignmentPaperMapper.Map(assignmentPaperDTO).Actions.Count, true);
                    ViewData["ActionGridData"] = actionGridData;
                    ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions);

                    IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Beneficiaries, 1, assignmentPaperDTO.Beneficiaries.Count, true);
                    ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                    ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(AssignmentPaperMapper.Map(assignmentPaperDTO).Beneficiaries);

                }
                else
                {
                    editorViewModel.AssignmentPaperVM = new AssignmentPaperVM();

                    IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperActionVM>(), 1, 0, true);
                    ViewData["ActionGridData"] = actionGridData;
                    ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(new List<AssignmentPaperActionVM>());

                    IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperBeneficiaryVM>(), 1, 0, true);
                    ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                    ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(new List<AssignmentPaperBeneficiaryVM>());
                }

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                    HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AssignmentGroupData"] = TransactionHelper.GetUserAssignmentGroups();
                ViewData["HasAssignmentPaper"] = TransactionHelper.CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = TransactionHelper.CheckOrgUnitIsAllowedToCreateGroup();
                ViewData["LetterTypeData"] = TransactionHelper.GetLetterTypes(transactionCategory);
                ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = FollowUpProccess();
                ViewData["FollowupPeriod"] = FollowupPeriod();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

                if (!string.IsNullOrEmpty(ViewData["AllActionsData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["AllActionsData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }
                if (!string.IsNullOrEmpty(ViewData["AllExternalActionsData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["AllExternalActionsData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }
                InitializerAssignmentPaperData(transactionId);

                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;

                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(TransactionCategory.DraftOutbound);
                ViewData["PrioritiesData"] = TransactionHelper.GetPriorities(TransactionCategory.DraftOutbound);
                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), editorViewModel.TransactionBasicInfoVM.ExternalPartyId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(editorViewModel.TransactionBasicInfoVM.ExternalPartyId);

                ViewData["ControllerName"] = "Editor";
                //ViewData["OrgUnitsManagers"] = TransactionHelper.GetOrgUnitsManagers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //   HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (subjectClassificationDTOs.Result != null && editorViewModel.TransactionBasicInfoVM.SubjectClassifications != null)
                //{
                //    editorViewModel.TransactionBasicInfoVM.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}
                //ViewData["SubjectClassificationsData"] = TransactionHelper.BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && editorViewModel.TransactionBasicInfoVM.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == editorViewModel.TransactionBasicInfoVM.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == editorViewModel.TransactionBasicInfoVM.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = TransactionHelper.BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                editorViewModel.Archives = GetListArchives(editorViewModel.TransactionBasicInfoVM.Attachments);
                EditorInitializer(editorViewModel);
                editorViewModel.TransactionBasicInfoVM.TransactionId = transactionId;
                return View("~/Areas/User/Views/Editor/EditorOutboundDraft/ReadWrite/Index.cshtml", editorViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult InitializeOutboundInternalRead(int transactionId, int trxCopyId)
        {
            try
            {
                EditorViewModel editorViewModel = new EditorViewModel();
                ViewData["TransactionId"] = transactionId;

                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", transactionId, SessionInfo.CultureShortName, trxCopyId)).Result;
                editorViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                editorViewModel.DocumentVM = TransactionHelper.GetMainDocument(transactionId);

                var transactionCopyDTOs = HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;
                editorViewModel.TransactionCopyVM.Copies = TransactionCopyMapper.Map(transactionCopyDTOs.Result);

                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                InitializerAssignmentPaperData(transactionId);
                GetAssignmentCopis(editorViewModel.TransactionBasicInfoVM.DeliveryMethodId);

                //
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(TransactionCopyMapper.Map(transactionCopyDTOs.Result));

                ViewData["EditorMainDocumentSessionKey"] = "DocoNutDocument";
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = FollowUpProccess();
                ViewData["FollowupPeriod"] = FollowupPeriod();
                editorViewModel.Archives = GetListArchives(editorViewModel.TransactionBasicInfoVM.Attachments);
                EditorInitializer(editorViewModel);
                DoconutInitializer(transactionId);

                editorViewModel.TransactionBasicInfoVM.TransactionId = transactionId;
                return View("~/Areas/User/Views/Editor/EditorInbound/Read/Index.cshtml", editorViewModel);

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void InitializerAssignmentPaperData(int transactionId)
        {
            GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                        .GetItemRequest(string.Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                        transactionId,
                        SessionInfo.CultureShortName)).Result;

            //GetResult<OrgUnitDTO> _orgUnitDTOs =
            //    HttpClientWrapper<GetResult<OrgUnitDTO>>
            //    .GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}",
            //                    SessionInfo.CultureShortName,
            //                    SessionInfo.OrgUnitId)).Result;



            //var parentorgunit = _orgUnitDTOs.Result.Name;


            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }


            BasicTransactionAssignmentNewVM transactionAssignmentBasicData = new BasicTransactionAssignmentNewVM
            {
                ConfedentialityId = transactionDetailsDTOResult.Result
                    .ConfidentialityId,

                PriorityLevelId = transactionDetailsDTOResult.Result
                    .PriorityId,

                Number = transactionDetailsDTOResult.Result.Number.ToString(),
                PriorityLevel = transactionDetailsDTOResult.Result.Priority,
                Subject = transactionDetailsDTOResult.Result.Subject,
                FromOrgUnit = SessionInfo.OrgUnitInfo.Name,
                DateTimeNowG = transactionDetailsDTOResult.Result.Date.ToString(),
                ReminderDate = transactionDetailsDTOResult.Result.ReminderDate,

                //ParentOrgUnit = parentorgunit,
                InboundDateH = transactionDetailsDTOResult.Result.InboundDateH,

                DateTimeNowH = DateTime.Now.ToString() + " " + DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now),
                InboundNumber = transactionDetailsDTOResult.Result.InboundNumber,
                TransactionDateH = transactionDetailsDTOResult.Result.HijriDate,
                TransactionId = transactionDetailsDTOResult.Result.Id,

            };

            ViewData["BasicTransactionAssignmentData"] = transactionAssignmentBasicData;
            var actionVMs = GetAllActionsValues();
            IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

            actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

            ViewData["AllActionsData2"] = actionVMs;

            ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();


        }
        private List<ActionVM> GetAllActionsValues()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);


            return processVMs;
        }
        public static string FollowUpProccess()
        {

            try
            {
                TransactionCategory transactionCategory = TransactionCategory.All;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<FollowUpLookUpDTO>> followUpProccessDTOs =
                    HttpClientWrapper<GetResult<List<FollowUpLookUpDTO>>>.GetItemRequest(string.Format("api/Transaction/GetFollowUpProccess?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (followUpProccessDTOs.Result != null)
                {
                    foreach (FollowUpLookUpsVM ProccessVm in Mappers.Lookups.FollowUpLookUpsMapper.Map(followUpProccessDTOs.Result))
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

        public static string FollowupPeriod()
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                dataSource.Add(new AutoCompleteDataSource() { Value = "1", Label = "يوم" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "2", Label = "يومين" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "3", Label = "ثلاثة أيام" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "7", Label = "أسبوع" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "14", Label = "أسبوعين" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "30", Label = "شهر" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "60", Label = "شهرين" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "-1", Label = "أخرى" });


                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetAssignmentCopis(int delivridMethod)
        {
            // 
            List<TransactionAssignmentVM> TransactionAssignmentVM = new List<TransactionAssignmentVM>();


            GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
           .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
            List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
            if (AssignmentPaperDTOs.Result != null && AssignmentPaperDTOs.Result.Beneficiaries != null)
            {
                transactionAssignmentVMs = AssignmentPaperDTOs.Result.Beneficiaries.Select(a =>
                {
                    TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();
                    transactionAssignmentVM.ToOrgUnitId = a.BeneficiaryOrgUnitId;
                    transactionAssignmentVM.ToOrgUnitName = a.OrgUnitName;
                    transactionAssignmentVM.ToUserId = a.UserId;
                    transactionAssignmentVM.ToUserName = a.UserName == null ? "استقبال الادارة" : a.UserName;
                    transactionAssignmentVM.GroupName = a.GroupName;
                    transactionAssignmentVM.GroupId = a.GroupId;
                    transactionAssignmentVM.ChkConstant = a.ChkConstant;
                    transactionAssignmentVM.GroupOrderNo = a.GroupOrderNo;

                    return transactionAssignmentVM;
                }).ToList();
            }

            ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

            //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
            //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

            AssignmentPaperDTO assignmentPaperDTO = AssignmentPaperMapper.Map(TransactionHelper.GetAssignmentPaperByOrgUnitId());

            if (assignmentPaperDTO != null)
            {
                IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions, 1, AssignmentPaperMapper.Map(assignmentPaperDTO).Actions.Count, true);
                ViewData["ActionGridData"] = actionGridData;
                ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions);

                IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Beneficiaries, 1, assignmentPaperDTO.Beneficiaries.Count, true);
                ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(AssignmentPaperMapper.Map(assignmentPaperDTO).Beneficiaries);
            }
            else
            {
                IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperActionVM>(), 1, 0, true);
                ViewData["ActionGridData"] = actionGridData;
                ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(new List<AssignmentPaperActionVM>());

                IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperBeneficiaryVM>(), 1, 0, true);
                ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(new List<AssignmentPaperBeneficiaryVM>());
            }

            ViewData["DeliveryMethod"] = GetDelivery(false);
        }
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
        public ActionResult InitializeOutboundInternalReadWrite(int transactionId, int trxCopyId)
        {
            try
            {
                EditorViewModel editorViewModel = new EditorViewModel();
                ViewData["TransactionId"] = transactionId;

                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", transactionId, SessionInfo.CultureShortName, trxCopyId)).Result;
                editorViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                editorViewModel.DocumentVM = TransactionHelper.GetMainDocument(transactionId);

                ViewData["EditorMainDocumentSessionKey"] = "DocoNutDocument";
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, true);
                ViewData["TasksGridData"] = grid;

                ViewData["CurrentTransactionTasksGrid"] = GetCurrentTransactionTasks(transactionId);

                TransactionCategory transactionCategory = (TransactionCategory)transactionBasicInfoDTO.Result.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName);

                GetResult<List<TransactionLinkDTO>> transactionLinkDTOs =
                    HttpClientWrapper<GetResult<List<TransactionLinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionLinks?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                IAjaxGrid gridLinks = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionLinkMapper.Map(transactionLinkDTOs.Result), 1, 0, true);
                ViewData["LinksData"] = gridLinks;
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(TransactionLinkMapper.Map(transactionLinkDTOs.Result));

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                ViewData["LinkTypeData"] = TransactionHelper.GetLinkTypes(transactionCategory);

                GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                    HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitBeneficiaries?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result), 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

                ViewData["AssignmentPaperData"] = JsonConvert.SerializeObject(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result));

                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

                AssignmentPaperDTO assignmentPaperDTO = AssignmentPaperMapper.Map(TransactionHelper.GetAssignmentPaperByOrgUnitId());

                if (assignmentPaperDTO != null)
                {
                    editorViewModel.AssignmentPaperVM = AssignmentPaperMapper.Map(assignmentPaperDTO);

                    IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions, 1, AssignmentPaperMapper.Map(assignmentPaperDTO).Actions.Count, true);
                    ViewData["ActionGridData"] = actionGridData;
                    ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(AssignmentPaperMapper.Map(assignmentPaperDTO).Actions);

                    IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperMapper.Map(assignmentPaperDTO).Beneficiaries, 1, assignmentPaperDTO.Beneficiaries.Count, true);
                    ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                    ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(assignmentPaperDTO.Beneficiaries);

                }
                else
                {
                    editorViewModel.AssignmentPaperVM = new AssignmentPaperVM();

                    IAjaxGrid actionGridData = (AjaxGrid<AssignmentPaperActionVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperActionVM>(), 1, 0, false);
                    ViewData["ActionGridData"] = actionGridData;
                    ViewData["AssignmentPeperActions"] = JsonConvert.SerializeObject(new List<AssignmentPaperActionVM>());

                    IAjaxGrid beneficiariesGridData = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperBeneficiaryVM>(), 1, 0, false);
                    ViewData["BeneficiariesGridData"] = beneficiariesGridData;
                    ViewData["AssignmentPeperBeneficiaries"] = JsonConvert.SerializeObject(new List<AssignmentPaperBeneficiaryVM>());
                }

                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AssignmentGroupData"] = TransactionHelper.GetUserAssignmentGroups();
                ViewData["HasAssignmentPaper"] = TransactionHelper.CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = TransactionHelper.CheckOrgUnitIsAllowedToCreateGroup();
                ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = FollowUpProccess();
                ViewData["FollowupPeriod"] = FollowupPeriod();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();
                InitializerAssignmentPaperData(transactionId);

                if (!string.IsNullOrEmpty(ViewData["AllActionsData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["AllActionsData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }
                if (!string.IsNullOrEmpty(ViewData["AllExternalActionsData"].ToString()))
                {
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["AllExternalActionsData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }

                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;

                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["ExplanationsData"] = TransactionHelper.GetTransactionExplanations(transactionId);
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ControllerName"] = "Editor";

                editorViewModel.TransactionBasicInfoVM.TransactionId = transactionId;
                EditorInitializer(editorViewModel);
                return View("~/Areas/User/Views/Editor/EditorOutboundInternal/ReadWrite/Index.cshtml", editorViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void EditorInitializer(EditorViewModel editorViewModel)
        {
            Session["DocoNutDocument"] = null;
            TextEditorViewModel textEditorViewModel = new TextEditorViewModel();
            if (editorViewModel.DocumentVM != null && editorViewModel.DocumentVM.Size > 0)
            {
                string documentId = Guid.NewGuid().ToString();
                ViewData["hdnDocumentId"] = editorViewModel.DocumentVM.Id;
                if (string.IsNullOrEmpty(editorViewModel.DocumentVM.MimeType) || editorViewModel.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                {
                    editorViewModel.EditorType = editorViewModel.EditorType = EditorType.Scanning;
                    string sessionKey = Guid.NewGuid().ToString();
                    ViewData[sessionKey] = sessionKey;
                    Session["DocoNutDocument"] = editorViewModel.DocumentVM.Content;
                }
                else
                {
                    textEditorViewModel.EditorType = editorViewModel.EditorType = EditorType.TextEditor;
                    textEditorViewModel.IsSigned = false;
                    textEditorViewModel.IsScanning = false;
                    textEditorViewModel.Content = editorViewModel.DocumentVM != null && editorViewModel.DocumentVM.Content != null ? Encoding.UTF8.GetString(editorViewModel.DocumentVM.Content) : null;
                    ViewData["EditorViewModel"] = textEditorViewModel;
                }
            }
        }
        [HttpGet]
        public ActionResult GetTransactionExplanationsForPrint(int transactionId)
        {
            try
            {
                GetResult<List<ExplanationDTO>> explanationDTOs =
                    HttpClientWrapper<GetResult<List<ExplanationDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionExplanations?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                GetResult<TransactionPrintDTO> transactionPrintDTO =
                    HttpClientWrapper<GetResult<TransactionPrintDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetAllTransactionDocuments?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                //List<ExplanationVM> explanationVMs = new List<ExplanationVM>();
                //explanationVMs = ExplanationMapper.Map(explanationDTOs.Result);

                //List<TransactionAssignmentVM> transactionAssignmentVMs = TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result);
                List<ExplanationVM> explanationVMs = ExplanationMapper.Map(explanationDTOs.Result);
                CustomGridMvc.IAjaxGrid Explanations = (CustomGridMvc.AjaxGrid<ExplanationVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(explanationVMs, 1, explanationVMs.Count(), false);

                ViewData["ExplanationsDataObject"] = explanationVMs;

                ViewData["ExplanationsData"] = Explanations;

                ViewData["TransactionId"] = transactionId;

                GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
                HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}&isElectronic={3}", SessionInfo.CultureShortName, transactionId, SessionInfo.OrgUnitId, true)).Result;
                TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);

                foreach (BarcodeVM barcodeVM in transactionBarcodesVM.BarcodeVMs)
                {
                    string barcode2D = Barcode.DrawBarcode(barcodeVM.Value, BarcodeFormat.CODE_128, 45, 160);
                    ViewData["barcode2D"] = barcode2D;
                }



                return View("~/Areas/User/Views/Editor/Explanations/_ExplanationPrintAll.cshtml");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DoconutInitializer(int transactionId)
        {
            List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();
            DocumentVM documentVM = TransactionHelper.GetMainDocument(transactionId);
            if (documentVM != null)
            {
                string documentId = Guid.NewGuid().ToString();
                transactionArchiveVMs.Add(new TransactionArchiveVM
                {
                    Id = documentId,
                    EncryptDocumentId = AESEncrytDecry.Base64Encode(documentVM.Id.ToString()),
                    IsMainDocument = true,
                    DocumentId = documentVM.Id,
                    AttachmentTypeId = -1,
                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                    SessionInfo.CultureShortName).Result.Text
                });
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["SessionArchiveMainDocumentKey"] = Guid.NewGuid().ToString();
                ViewData["MainDocumentId"] = documentVM.Id;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridTasks(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<TaskAddDTO>> taskDTOs =
                    HttpClientWrapper<GetResult<List<TaskAddDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTasks?transactionId={0}&{1}&cultureName={2}", param, parameters, SessionInfo.CultureShortName)).Result;


                var grid = new AjaxGridFactory().CreateAjaxGrid(TaskAddMapper.Map(taskDTOs.Result), page.HasValue ? page.Value : 1, taskDTOs.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Editor/TaskManagement/_CurrentTasksGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static AjaxGrid<TaskAddVM> GetCurrentTransactionTasks(int transactionId)
        {
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            GetResult<List<TaskAddDTO>> taskDTOs =
               HttpClientWrapper<GetResult<List<TaskAddDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTasks?transactionId={0}&PageIndex={1}&pageSize={2}&cultureName={3}", transactionId, 1, settingVM.Value, SessionInfo.CultureShortName)).Result;

            List<TaskAddVM> taskAddVMs = TaskAddMapper.Map(taskDTOs.Result);
            if (taskAddVMs == null)
            {
                taskAddVMs = new List<TaskAddVM>();
            }
            return (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddVMs, 1, (int)taskDTOs.RowsCount, false);
        }

        #endregion Initialization

        #region Explanation

        [HttpGet]
        public string GetContentByFormId(int id)
        {
            try
            {
                string html = string.Empty;

                GetResult<FormContentDTO> formContentDTO =
                HttpClientWrapper<GetResult<FormContentDTO>>.GetItemRequest(string.Format("api/Transaction/GetContentByFormId?formId={0}", id)).Result;

                if (formContentDTO.Result != null && formContentDTO.Result.Content != null)
                {
                    // html = FormContentMapper.Map(formContentDTO.Result).Content;
                }

                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.ExpalanationsEditor.DisplayLink)]
        public ActionResult GetExplanationById(int id, string hdnExplanationDocumentSessionKey)
        {
            try
            {
                GetResult<ExplanationDTO> explanationDTO =
                HttpClientWrapper<GetResult<ExplanationDTO>>.GetItemRequest(string.Format("api/Transaction/GetExplanationById?cultureName={0}&explanationId={1}", SessionInfo.CultureShortName, id)).Result;

                ExplanationVM explanationVM = ExplanationMapper.Map(explanationDTO.Result);

                if (explanationDTO.Result.EditorType == EditorType.TextEditor)
                {
                    ViewData["TextEditorContent"] = System.Text.Encoding.UTF8.GetString(explanationVM.DocumentVM.Content);

                    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Explanations/_TextEditorPartial.cshtml", null), Type = explanationVM.EditorType, ConfidentialityId = explanationVM.ConfidentialityId }, JsonRequestBehavior.AllowGet);

                }
                else if (explanationDTO.Result.EditorType == EditorType.Scanning)
                {
                    Session["DocoNutexplanations"] = explanationVM.DocumentVM.Content;
                    return Json(new { Type = explanationDTO.Result.EditorType, ConfidentialityId = explanationVM.ConfidentialityId, Date = explanationVM.Date.ToString() }, JsonRequestBehavior.AllowGet);
                }
                else if (explanationDTO.Result.EditorType == EditorType.Text)
                {
                    return Json(new { Type = explanationVM.EditorType, Content = Encoding.Unicode.GetString(explanationVM.DocumentVM.Content), ConfidentialityId = explanationVM.ConfidentialityId, Date = explanationVM.Date.ToString() }, JsonRequestBehavior.AllowGet);
                }
                else if (explanationDTO.Result.EditorType == EditorType.File)
                {

                    return Json(new { Type = explanationVM.EditorType, FileId = explanationVM.DocumentVM.Id, FileName = explanationVM.DocumentVM.Name ?? "File", ConfidentialityId = explanationVM.ConfidentialityId, Date = explanationVM.Date.ToString() }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    return Json(new { Type = explanationVM.EditorType, Content = Encoding.Unicode.GetString(explanationVM.DocumentVM.Content), ConfidentialityId = explanationVM.ConfidentialityId, Date = explanationVM.Date.ToString() }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.ExpalanationsEditor.Add)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddExplanation(int hdnTransactionId, [Bind(Prefix = "EditorExplanations")] ExplanationVM explanationVM, string hdnTextEditorContent, string param, HttpPostedFileBase UploadExplanationFile)
        {


            try
            {

                string message = string.Empty;

                switch (explanationVM.EditorType)
                {
                    case EditorType.TextEditor:
                        explanationVM.DocumentVM = new DocumentVM
                        {
                            MimeType = System.Net.Mime.MediaTypeNames.Application.Octet,
                            Content = Encoding.UTF8.GetBytes(hdnTextEditorContent),
                            Size = Encoding.UTF8.GetBytes(hdnTextEditorContent).Length,
                            FromEntityId = SessionInfo.OrgUnitId,
                            FromUserId = SessionInfo.CurrentUser.Id
                        };
                        break;
                    case EditorType.Scanning:
                        {
                            byte[] data = DocumentViewerHelper.GetPDFFile(param);

                            explanationVM.DocumentVM = new DocumentVM
                            {
                                MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,
                                Content = data,
                                Size = data.Length,
                                FromEntityId = SessionInfo.OrgUnitId,
                                FromUserId = SessionInfo.CurrentUser.Id
                            };
                            break;
                        }

                    case EditorType.File:
                        {

                            if (string.IsNullOrWhiteSpace(explanationVM.FileName))
                            {

                                message = DbRes.TValidation("Task.File.MineType");
                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string FilePrefix;
                            if (SystemConfigurations.MultiTenantEnabled)
                            {
                                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile;
                            }
                            else
                            {
                                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
                            }


                            string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                            var fullPath = StringUtility.ValidateFileNames($"{path}{explanationVM.FileName}");
                            byte[] fileContent = System.IO.File.ReadAllBytes(fullPath);
                            string fileExtenstion = GetAttchementMimeType(explanationVM.FileName);

                            explanationVM.DocumentVM = new DocumentVM
                            {
                                MimeType = fileExtenstion,
                                Content = fileContent,
                                Size = fileContent.Length,
                                FromEntityId = SessionInfo.OrgUnitId,
                                FromUserId = SessionInfo.CurrentUser.Id
                            };
                            break;
                        }

                    case EditorType.Text:
                        {
                            byte[] data = Encoding.Unicode.GetBytes(explanationVM.Description.Trim());

                            explanationVM.DocumentVM = new DocumentVM
                            {
                                MimeType = System.Net.Mime.MediaTypeNames.Text.Plain,
                                Content = data,
                                Size = data.Length,
                                FromEntityId = SessionInfo.OrgUnitId,
                                FromUserId = SessionInfo.CurrentUser.Id
                            };
                            break;
                        }

                    case 0:
                        message = DbRes.TValidation("User.Transaction.Explanations.EditorExplanation");

                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    default:
                        if (UploadExplanationFile != null)
                        {
                            if (UploadExplanationFile.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(UploadExplanationFile.FileName);
                                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                                UploadExplanationFile.SaveAs(path);
                            }
                        }

                        break;
                }


                PostResult postResult = 
                    HttpClientWrapper<PostResult>
                    .PostRequest(string.Format("api/Transaction/AddTransactionExplanation?transactionId={0}",
                    hdnTransactionId),
                    ExplanationMapper.Map(explanationVM))
                    .Result;



                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Explanations.AddSucceeded");

                UserVM userVM = SessionInfo.GetObjectFromSession(Constants.LoggedInUserKey) as UserVM;
                Session["DocoNutexplanations"] = null;

                return Json(new { MessageText = message, MessageType = MessageType.Information, Id = postResult.Id, Name = userVM.LoclizationName.Where(l => l.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.ExpalanationsEditor.Edit)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditExplanation([Bind(Prefix = "EditorExplanations")] ExplanationVM explanationVM, string hdnTextEditorContent, string param)
        {
            try
            {
                string message = string.Empty;

                if (explanationVM.EditorType == EditorType.TextEditor)
                {
                    explanationVM.DocumentVM = new DocumentVM
                    {
                        MimeType = System.Net.Mime.MediaTypeNames.Application.Octet,
                        Content = Encoding.UTF8.GetBytes(hdnTextEditorContent),
                        Size = Encoding.UTF8.GetBytes(hdnTextEditorContent).Length,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id
                    };
                }
                else if (explanationVM.EditorType == EditorType.Scanning)
                {
                    byte[] data = DocumentViewerHelper.GetPDFFile(param);

                    explanationVM.DocumentVM = new DocumentVM
                    {
                        MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,
                        Content = data,
                        Size = data.Length,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id,
                    };
                }
                else if (explanationVM.EditorType == EditorType.Text)
                {
                    byte[] data = Encoding.Unicode.GetBytes(explanationVM.Description.Trim());

                    explanationVM.DocumentVM = new DocumentVM
                    {
                        MimeType = System.Net.Mime.MediaTypeNames.Text.Plain,
                        Content = data,
                        Size = data.Length,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id
                    };
                }
                else if (explanationVM.EditorType == EditorType.File && !string.IsNullOrWhiteSpace(explanationVM.FileName))
                {


                    string FilePrefix;
                    if (SystemConfigurations.MultiTenantEnabled)
                    {
                        FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile;
                    }
                    else
                    {
                        FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
                    }


                    string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                    var fullPath = StringUtility.ValidateFileNames($"{path}{explanationVM.FileName}");
                    byte[] fileContent = System.IO.File.ReadAllBytes(fullPath);
                    string fileExtenstion = GetAttchementMimeType(explanationVM.FileName);

                    explanationVM.DocumentVM = new DocumentVM
                    {
                        MimeType = fileExtenstion,
                        Content = fileContent,
                        Size = fileContent.Length,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id
                    };

                }

                PostResult postResult =
                HttpClientWrapper<PostResult>.PostRequest("api/Transaction/UpdateExplanation", ExplanationMapper.Map(explanationVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Explanations.UpdateSucceeded");

                UserDTO userDTO = SessionInfo.GetObjectFromSession(Constants.LoggedInUserKey) as UserDTO;

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }


            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.ExpalanationsEditor.Delete)]
        public ActionResult DeleteExplanationById(int id)
        {
            try
            {
                PostResult postResult =
                HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/DeleteExplanation?explanationId={0}", id), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                Session["DocoNutexplanations"] = null;
                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.FollowUps.AddFollowUp)]
        public ActionResult AddFollowUp([Bind(Prefix = "FollowUpTab")] TransactionFollowUpVM followVM, List<TransactionFollowUpVM> FollowUps)
        {
            string message = string.Empty;
            try
            {
                PostResult postResultCheck =
                                   HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckIfFollowUpAdded?TransactionId={0}", followVM.TransactionId), null).Result;


                if (!Convert.ToBoolean(postResultCheck.Result))
                {
                    if (followVM.FollowUpProccessId != 0)
                    {
                        followVM.FollowUpStatusId = (int)FollowupStatus.New;
                        followVM.CreationDate = DateTime.Now;
                        followVM.Active = true;
                        followVM.CreatingUserId = SessionInfo.CurrentUser.Id;
                        followVM.CreatingEntityId = SessionInfo.OrgUnitId;
                        followVM.IsCopy = true;

                        if (followVM.FollowUpTypeId == 2)
                        {
                            followVM.FollowUpUserId = null;
                            PostResult postResultFodept =
                            HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/getFollowUpDepartment?EntityId={0}", SessionInfo.OrgUnitId), null).Result;

                            if (postResultFodept.Id.HasValue)
                            {
                                followVM.FollowUpEntityId = (int)postResultFodept.Id;
                            }
                            else
                            {

                                message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpOrgUnitDoesNotExist");
                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (followVM.ProccessPeriod == -1)
                            followVM.FollowUpExpireDate = (DateTime)followVM.DateTo;
                        else
                            followVM.FollowUpExpireDate = DateTime.Now.AddDays(Convert.ToInt32(followVM.ProccessPeriod));



                        PostResult postResult =
                       HttpClientWrapper<PostResult>.PostRequest("api/Transaction/TransactionFollowUpAdd?cultureName=" + SessionInfo.CultureShortName, TransactionFollowUpMapper.Map(followVM)).Result;


                        if (postResult.StatusCode != StatusCode.Ok)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                        }
                        if (postResult.Id.HasValue && postResult.Id > 0)
                        {
                            FollowUpAuditTrailVM followUpAuditTrail = new FollowUpAuditTrailVM();
                            followUpAuditTrail.FollowupId = (int)postResult.Id;
                            followUpAuditTrail.ProccessDate = DateTime.Now;
                            followUpAuditTrail.ProccessId = followVM.FollowUpTypeId == 1 ? (int)FollowupAuditProcess.AddPrivetFollowup : (int)FollowupAuditProcess.AddPublicFollowup;
                            followUpAuditTrail.ProccessDescription = followVM.FollowUpTypeId == 1 ? ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPrivetFollowUp") : ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPublicFollowUp");
                            followUpAuditTrail.UserId = SessionInfo.CurrentUser.Id;
                            followUpAuditTrail.EntityId = SessionInfo.OrgUnitId;
                            PostResult postResultAudit =
                            HttpClientWrapper<PostResult>.PostRequest("api/Transaction/AddFollowupUditTrial?cultureName=" + SessionInfo.CultureShortName, FollowUpAuditTrailMapper.Map(followUpAuditTrail)).Result;

                        }
                        else
                        {
                            message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpAlreadyAdded");
                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                        }

                        List<TransactionFollowUpVM> list = GetTransactionFollowUps(followVM.TransactionId);

                        var detailsList = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(list, 1, list.Count, false);
                        return PartialView("~/Areas/User/Views/Editor/FollowUp/_FollowUpGridPartial.cshtml", detailsList);
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpProcessNeeded");
                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpAlreadyExist");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult FollowUpDetailsLoad(int transactionId)
        {
            List<TransactionFollowUpVM> list = GetTransactionFollowUps(transactionId);

            var detailsList = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(list, 1, list.Count, false);
            return PartialView("~/Areas/User/Views/Editor/FollowUp/_FollowUpGridPartial.cshtml", detailsList);


        }
        public static List<TransactionFollowUpVM> GetTransactionFollowUps(int transactionId)
        {
            GetResult<List<TransactionFollowUpDTO>> dtoAPI =
               HttpClientWrapper<GetResult<List<TransactionFollowUpDTO>>>.GetItemRequest(string.Format("api/Transaction/TransactionFollowUpSelectByTransId?transId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

            List<TransactionFollowUpVM> transactionCoordinationVMs = TransactionFollowUpMapper.Map(dtoAPI.Result);

            return transactionCoordinationVMs;

        }

        [HttpPost]
        public ActionResult CheckIfHasArchive(string param)
        {
            //string message = string.Empty;

            //byte[] data = DocumentViewerHelper.GetPDFFile(param);

            //if (data.Length <= 12397)
            //{
            //    message = DbRes.TValidation("User.Inbound.MainDocument");
            //    return Json(new { data = "empty" }, JsonRequestBehavior.AllowGet);
            //}
            return Json(new { data = "NotEmpty" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public void RefreshDocumentViewer(string hdnExplanationDocumentSessionKey)
        {
            Session[hdnExplanationDocumentSessionKey] = null;
            Session["DocoNutexplanations"] = null;
        }

        #endregion Explanation

        #region Links

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Links.Add)]
        public ActionResult AddEditorLink([Bind(Prefix = "EditorLinks")] TransactionLinkVM transactionLinkVM, string hdnEditorLinks, string transactionId)
        {

            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();
                transactionLinkVMs = javaScriptSerializer.Deserialize(hdnEditorLinks, typeof(List<TransactionLinkVM>)) as List<TransactionLinkVM>;
                int nLinkTypeId = LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                if (transactionLinkVM.LinkTypeId == LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName))

                {
                    nLinkTypeId = transactionLinkVM.TransactionCategory == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) ? LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName) : LinkingType.WithReplyOutbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                }
                else
                {
                    nLinkTypeId = transactionLinkVM.TransactionCategory == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) ? LinkingType.WithReferenceInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName) : LinkingType.WithReferenceOutbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                }

                GetResult<TransactionDetailsDTO> transaction =
                HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionIdByLinkType?sourceNumber={0}&orgUnitId={1}&yearId={2}&linkTypeId={3}&cultureName={4}", transactionLinkVM.TransactionNumber, transactionLinkVM.OrgUnitId, transactionLinkVM.Year, nLinkTypeId, SessionInfo.CultureShortName)).Result;


                if (transaction.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transaction.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else if (transactionId == transaction.Result.ToString())
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionCycleLinked.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else if (transactionLinkVMs.ToList().Where(l => l.TransactionId == transaction.Result.Id).FirstOrDefault() != null)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionDoubleLinked.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    transactionLinkVM.DateH = transaction.Result.HijriDate;
                    transactionLinkVM.Date = transaction.Result.Date.ToShortDateString();
                    transactionLinkVM.TransactionType = transaction.Result.TransactionsTypes;
                    transactionLinkVM.TransactionId = transaction.Result.Id;

                    transactionLinkVMs.Add(transactionLinkVM);
                }


                string data = JsonConvert.SerializeObject(transactionLinkVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs, 1, transactionLinkVMs.Count, false);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Links/_LinksGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Links.Delete)]
        public ActionResult DeleteEditorLinks(string ids, string hdnEditorLinks)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();

                if (!string.IsNullOrEmpty(hdnEditorLinks))
                {

                    object objects = javaScriptSerializer.Deserialize(hdnEditorLinks, typeof(object[]));

                    List<object> list = ((object[])objects).ToList();

                    objects = list.ToArray<object>();

                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionLinkVM)
                        {
                            transactionLinkVMs.Add(o as TransactionLinkVM);
                        }
                        else
                        {
                            TransactionLinkVM transactionLinkVM =
                                javaScriptSerializer.Deserialize<TransactionLinkVM>(javaScriptSerializer.Serialize(o));

                            transactionLinkVMs.Add(transactionLinkVM);
                        }

                    });
                }



                List<int> linkIds = ids.Split(',').Select(int.Parse).ToList();

                linkIds.ForEach(id =>
                {
                    TransactionLinkVM remove = transactionLinkVMs.Where(n => n.TransactionId == id).FirstOrDefault();
                    transactionLinkVMs.Remove(remove);
                });

                string data = JsonConvert.SerializeObject(transactionLinkVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs, 1, transactionLinkVMs.Count, false);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Links/_LinksGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Links.Add)]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveTransactionLinks(int hdnTransactionId, string hdnEditorLinks)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();

                if (!string.IsNullOrEmpty(hdnEditorLinks))
                {
                    transactionLinkVMs.AddRange(javaScriptSerializer.Deserialize(hdnEditorLinks, typeof(List<TransactionLinkVM>)) as List<TransactionLinkVM>);
                }

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddTransactionLinks?transactionId={0}", hdnTransactionId), TransactionLinkMapper.Map(transactionLinkVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Links.AddSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }


            catch (Exception)
            {

                throw;
            }
        }

        #endregion Links

        #region AssignmentPaper
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult AddAssignmentPaper([Bind(Prefix = "EditorAssignmentPaper")] TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentPaperData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentPaperData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentPaperData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                bool checkDetail = true;

                transactionAssignmentVMs.ForEach(a =>
                {
                    if (a.ToOrgUnitId == transactionAssignmentVM.ToOrgUnitId && a.ToUserId == transactionAssignmentVM.ToUserId)
                    {
                        checkDetail = false;
                    }
                });

                if (checkDetail)
                {
                    transactionAssignmentVMs.Add(transactionAssignmentVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");

                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                ViewData["ControllerName"] = "Editor";

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AssignmentPaperGridPartial.cshtml", grid),
                    hdnValue = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }





        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult GetAssignmentPaper(int id, string hdnAssignmentPaperData)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();

                if (!string.IsNullOrEmpty(hdnAssignmentPaperData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentPaperData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                transactionAssignmentVM = transactionAssignmentVMs[id];

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), transactionAssignmentVM.ToOrgUnitId);
                ViewData["ToUserAssignment"] = GetUsersByOrgUnitId(transactionAssignmentVM.ToOrgUnitId, true);
                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["ControllerName"] = "Editor";
                ViewData.TemplateInfo.HtmlFieldPrefix = "EditorAssignmentPaper";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AssignmentPaperBodyPartial.cshtml", transactionAssignmentVM), Index = id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult EditAssignmentPaper(int hdnIndexAssignmentPaper, [Bind(Prefix = "EditorAssignmentPaper")] TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentPaperData)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentPaperData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentPaperData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                bool checkDetail = true;

                transactionAssignmentVMs.ForEach(a =>
                {
                    if (a.ToOrgUnitId == transactionAssignmentVM.ToOrgUnitId && a.ToUserId == transactionAssignmentVM.ToUserId
                        && transactionAssignmentVMs.IndexOf(a) != hdnIndexAssignmentPaper)
                    {
                        checkDetail = false;
                    }
                });

                if (checkDetail)
                {
                    transactionAssignmentVMs[hdnIndexAssignmentPaper] = transactionAssignmentVM;
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");

                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                ViewData["ControllerName"] = "Editor";

                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AssignmentPaperGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridAssignmentPaper(int? page, string param)
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

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Editor/AssignmentPaper/_AssignmentPaperGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult DeleteAssignmentPapers(string ids, string hdnAssignmentPaperData)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentPaperData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentPaperData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                List<int> index = ids.Split(',').Select(int.Parse).ToList();

                List<TransactionAssignmentVM> deletedData = new List<TransactionAssignmentVM>();

                index.ForEach(i =>
                {
                    deletedData.Add(transactionAssignmentVMs[i]);
                });

                deletedData.ForEach(d => transactionAssignmentVMs.Remove(d));

                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.DeleteSucceeded");

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AssignmentPaperGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTemporaryEntity([Bind(Prefix = "EditorAssignmentPaper")] TransactionAssignmentVM transactionAssignmentVM, List<TransactionAssignmentVM> TransactionAssignments)
        {
            string message = string.Empty;
            bool already = false;
            if (TransactionAssignments != null)
            {
                foreach (var item in TransactionAssignments)
                {
                    if (transactionAssignmentVM.ToOrgUnitId == item.ToOrgUnitId && transactionAssignmentVM.ToUserId == item.ToUserId)
                    {
                        already = true;
                    }
                }
            }
            if (!already)
            {
                GetResult<OrgUnitDTO> orgUnitDTO =
              HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", transactionAssignmentVM.ToOrgUnitId, SessionInfo.CultureShortName)).Result;

                if (orgUnitDTO.Result != null)
                {
                    transactionAssignmentVM.ToOrgUnitName = orgUnitDTO.Result.Name;
                }
                if (transactionAssignmentVM.ToUserId != null)
                {
                    GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", transactionAssignmentVM.ToUserId)).Result;

                    if (userProfileEditDTO.Result != null)
                    {
                        transactionAssignmentVM.UserImageId = userProfileEditDTO.Result.UserImageId;

                    }
                }
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                transactionAssignmentVM.Key = TransactionAssignments != null ? TransactionAssignments.Count + 1 : 1;
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AddedAssignmentEntitiesCopisOnly.cshtml", transactionAssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTemporaryEntityNew([Bind(Prefix = "EditorAssignmentPaper")] TransactionAssignmentVM transactionAssignmentVM, List<TransactionAssignmentVM> TransactionAssignments)
        {
            string message = string.Empty;
            bool already = false;
            if (TransactionAssignments != null)
            {
                foreach (var item in TransactionAssignments)
                {
                    if (transactionAssignmentVM.ToOrgUnitId == item.ToOrgUnitId && transactionAssignmentVM.ToUserId == item.ToUserId)
                    {
                        already = true;
                    }
                }
            }
            if (!already)
            {
                GetResult<OrgUnitDTO> orgUnitDTO =
              HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", transactionAssignmentVM.ToOrgUnitId, SessionInfo.CultureShortName)).Result;

                if (orgUnitDTO.Result != null)
                {
                    transactionAssignmentVM.ToOrgUnitName = orgUnitDTO.Result.Name;
                }
                if (transactionAssignmentVM.ToUserId != null)
                {
                    GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", transactionAssignmentVM.ToUserId)).Result;

                    if (userProfileEditDTO.Result != null)
                    {
                        transactionAssignmentVM.UserImageId = userProfileEditDTO.Result.UserImageId;

                    }
                }
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                transactionAssignmentVM.Key = TransactionAssignments != null ? TransactionAssignments.Count + 1 : 1;
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AddedAssignmentEntitiesCopisOnlyNew.cshtml", transactionAssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTemporaryEntityNewVip([Bind(Prefix = "AssignmentVM")] TransactionAssignmentVM AssignmentVM, List<TransactionAssignmentVM> AssignmentVMs)
        {
            string message = string.Empty;
            bool already = false;
            if (AssignmentVMs != null)
            {
                foreach (var item in AssignmentVMs)
                {
                    if (AssignmentVM.ToOrgUnitId == item.ToOrgUnitId && AssignmentVM.ToUserId == item.ToUserId)
                    {
                        already = true;
                    }
                }
            }
            if (!already)
            {
                GetResult<OrgUnitDTO> orgUnitDTO =
              HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", AssignmentVM.ToOrgUnitId, SessionInfo.CultureShortName)).Result;

                if (orgUnitDTO.Result != null)
                {
                    AssignmentVM.ToOrgUnitName = orgUnitDTO.Result.Name;
                }
                if (AssignmentVM.ToUserId != null)
                {
                    GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", AssignmentVM.ToUserId)).Result;

                    if (userProfileEditDTO.Result != null)
                    {
                        AssignmentVM.UserImageId = userProfileEditDTO.Result.UserImageId;

                    }
                }
                AssignmentVM.DeliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                AssignmentVM.Key = AssignmentVMs != null ? AssignmentVMs.Count + 1 : 1;
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Vip/AssignmentPaper/_AddedAssignmentEntitiesNew2.cshtml", AssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AddTemporaryEntityNewVipCopies([Bind(Prefix = "AssignmentVM")] TransactionAssignmentVM AssignmentVM, List<TransactionAssignmentVM> TransactionAssignments)
        {
            string message = string.Empty;
            bool already = false;
            if (TransactionAssignments != null)
            {
                foreach (var item in TransactionAssignments)
                {
                    if (AssignmentVM.ToOrgUnitId == item.ToOrgUnitId && AssignmentVM.ToUserId == item.ToUserId)
                    {
                        already = true;
                    }
                }
            }
            if (!already)
            {
                GetResult<OrgUnitDTO> orgUnitDTO =
              HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", AssignmentVM.ToOrgUnitId, SessionInfo.CultureShortName)).Result;

                if (orgUnitDTO.Result != null)
                {
                    AssignmentVM.ToOrgUnitName = orgUnitDTO.Result.Name;
                }
                if (AssignmentVM.ToUserId != null)
                {
                    GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", AssignmentVM.ToUserId)).Result;

                    if (userProfileEditDTO.Result != null)
                    {
                        AssignmentVM.UserImageId = userProfileEditDTO.Result.UserImageId;

                    }
                }
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                AssignmentVM.Key = TransactionAssignments != null ? TransactionAssignments.Count + 1 : 1;
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Vip/AssignmentPaper/_AddedAssignmentEntitiesNew.cshtml", AssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SendACopisssignmentPaper(List<TransactionAssignmentVM> TransactionAssignments, string TransactionId, string explanationTxt, string ConfedentialityId, int deliveryMethodId, int? reporterId)
        {
            string message = string.Empty;
            int? TransactionAssignmentExplanationId = 0;



            foreach (var item in TransactionAssignments)
            {
                if (item.IsAssigned || item.IsCopy || item.IsOpr)
                {
                    if (item.ActionId == 0)
                    {
                        message = DbRes.TValidation("User.Transaction.AssignmentPaper.ActionsValidate");

                        break;
                    }
                }
            }
            if (message != string.Empty)
            {
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            TransactionAssignments.ForEach(t =>
            {
                t.FromOrgUnitId = SessionInfo.OrgUnitId;
                if (t.ToUserId == -1)
                {
                    t.ToUserId = null;
                }
            });

            TransactionAssignments.Where(ta => ta.IsAssigned == true).FirstOrDefault().DeliveryMethodId = deliveryMethodId;
            TransactionAssignments.Where(ta => ta.IsAssigned == true).FirstOrDefault().ReporterId = reporterId;
            List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs = new List<AssignmentPaperBeneficiaryVM>();
            foreach (var item in TransactionAssignments)
            {
                AssignmentPaperBeneficiaryVM assignmentPaperBeneficiaryVM = new AssignmentPaperBeneficiaryVM();
                assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId = item.ToOrgUnitId;
                assignmentPaperBeneficiaryVM.Id = item.Id;
                //assignmentPaperBeneficiaryVM.Key = item.Key;
                assignmentPaperBeneficiaryVM.OrgUnitName = item.ToOrgUnitName;

                assignmentPaperBeneficiaryVM.UserId = item.ToUserId;
                assignmentPaperBeneficiaryVM.UserName = item.ToUserName;
                assignmentPaperBeneficiaryVM.UserImageId = item.UserImageId;
                assignmentPaperBeneficiaryVM.GroupId = item.GroupId;
                assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiaryVM);
            }

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs);

            int? OprEntityId = TransactionAssignments.Where(ta => ta.IsOpr == true).FirstOrDefault()?.ToOrgUnitId;


            List<TransactionCopyDTO> transactionCopyDTOs = TransactionAssignments.Where(ta => ta.IsCopy == true || ta.IsOpr == true).Select(tc => new TransactionCopyDTO
            {
                ActionId = tc.ActionId,
                UserId = tc.ToUserId,
                OrgUnitId = tc.ToOrgUnitId,
                IsSent = 1,
                FromUserId = SessionInfo.CurrentUser.Id,
                FromOrgUnitId = SessionInfo.OrgUnitId,
                SpecialExplanation = tc.SpecialExplanation,
                GeneralExplanation = explanationTxt,
                IsBcc = tc.IsBcc,
                IsOpr = tc.IsOpr,
                OprEntityId = OprEntityId
            }).ToList();

            PostResult postCopiesResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddAssignmentCopies?transactionId={0}", TransactionId), transactionCopyDTOs).Result;

            if (postCopiesResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postCopiesResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            if (explanationTxt != string.Empty)
            {
                byte[] data = Encoding.Unicode.GetBytes(explanationTxt.Trim());
                ExplanationVM explanationVM = new ExplanationVM()
                {
                    Description = explanationTxt,
                    ConfidentialityId = int.Parse(ConfedentialityId),
                    FromUserId = SessionInfo.CurrentUser.Id,
                    EditorType = EditorType.Text,
                    DocumentVM = new Areas.User.Models.Shared.DocumentVM()
                    {
                        MimeType = System.Net.Mime.MediaTypeNames.Text.Plain,
                        Content = data,
                        Size = data.Length,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id
                    }
                };

                PostResult postExplanationResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddTransactionExplanation?transactionId={0}", TransactionId), ExplanationMapper.Map(explanationVM)).Result;

                if (postExplanationResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postExplanationResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                TransactionAssignmentExplanationId = postExplanationResult.Id;
            }
            if (TransactionAssignmentExplanationId.HasValue && TransactionAssignmentExplanationId.Value > 0)
            {
                PutResult UpdateTransactionAssignmentHistory = HttpClientWrapper<PutResult>
                  .PutRequest(string.Format("api/Transaction/UpdateTransactionAssignmentHistory?transactionId={0}&ExplanationId={1}", TransactionId, TransactionAssignmentExplanationId), null).Result;

            }



            string url = UrlHelper.GetBaseUri() + "/User/File/Copies";

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");

            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                returnUrl = url
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        [ValidateAntiForgeryToken()]
        public ActionResult SendAssignmentPaper(string hdnAssignmentPaperData, string hdnTransactionId, int trayId, string pageSize, int? dateType, bool isConfirmed)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                if (!string.IsNullOrEmpty(hdnAssignmentPaperData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(hdnAssignmentPaperData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                transactionAssignmentVMs.ForEach(t => t.FromOrgUnitId = SessionInfo.OrgUnitId);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", hdnTransactionId.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
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


                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransaction?transactionId={0}", hdnTransactionId), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string url = MCS.UI.UrlHelper.GetBaseUri() + "/User/Home/Index";

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");

                string parameters = GetListTransactionParameters();

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
                ViewData["PageSize"] = settingVM.Value;
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
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion AssignmentPaper

        #region Assignmnets

        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult EditorAssignmentGroupAdd()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentGroupDetailVM>(), 1, 0, false);
                ViewData["AssignmentGroupDetailData"] = grid;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["ControllerName"] = "Editor";

                return PartialView("~/Areas/User/Views/Editor/Assignments/_AssignmentCreateGroupPartial.cshtml");
            }


            catch (Exception)
            {

                throw;
            }
        }


        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Assignments.Assign)]
        [ValidateAntiForgeryToken()]
        public ActionResult AssignTrans(string hdnTransactionId, int type, string chkboxFollowUp)
        {
            var editInboundDTO = HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={hdnTransactionId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;

            string message = string.Empty;
            object reportsIds = null;
            List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

            transactionAssignmentVMs.Add(new TransactionAssignmentVM()
            {
                ToUserId = editInboundDTO.Result.OutboundInternalBasicInfoEdit.DirectedToId,
                TrayId = editInboundDTO.Result.OutboundInternalBasicInfoEdit.DirectedToId.HasValue ? (int)TrayType.MyTransactions : (int)TrayType.OrgUnit,
                FromOrgUnitId = SessionInfo.OrgUnitId,
                ToOrgUnitId = Convert.ToInt32(editInboundDTO.Result.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId),
                DeliveryMethodId = Convert.ToInt32(editInboundDTO.Result.OutboundInternalBasicInfoEdit.DeliveryMethodId),
                ActionId = ActionType.SendMainTransaction.LookupIdentity(LookupCategory.ActionType, SessionInfo.CultureShortName),
                ReporterId = editInboundDTO.Result.OutboundInternalBasicInfoEdit.ReporterId
            });

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransaction?transactionId={0}&cultureName={1}&followUp={2}", hdnTransactionId, SessionInfo.CultureShortName, chkboxFollowUp), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            var result = transactionAssignmentVMs.Where(a => a.DeliveryMethodId != DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName)).ToList();
            if (result != null && result.Count > 0)
            {
                var resultDeliveryReport = HttpClientWrapper<PostResult>.PostRequest($"api/Transaction/GetDeliveryReportByTransactionIds?transactionId={hdnTransactionId}&type={type}", null).Result;
                if (resultDeliveryReport.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, resultDeliveryReport.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                reportsIds = resultDeliveryReport.Result;


            }

            string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";
            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                ReportsIds = reportsIds != null ? JsonConvert.SerializeObject(reportsIds) : "",
            }, JsonRequestBehavior.AllowGet);

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Archiving.AddSucceeded");




        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddGroupByEditor(AssignmentGroupVM assignmentGroupVM, string hdnAssignmentDetails, string hdnAssignmentGroups)
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
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult AddEditorAssignmentDetail(AssignmentGroupDetailVM assignmentGroupDetailVM, string hdnAssignmentDetails)
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

                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(assignmentGroupDetailVMs, 1, assignmentGroupDetailVMs.Count, false);

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentGroupDetailsGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridEditorAssignmentDetails(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<AssignmentGroupDetailDTO> assignmentGroupDetailDTOs = new List<AssignmentGroupDetailDTO>();

                if (!string.IsNullOrEmpty(param))
                {
                    assignmentGroupDetailDTOs.AddRange(javaScriptSerializer.Deserialize(param, typeof(List<AssignmentGroupDetailDTO>)) as List<AssignmentGroupDetailDTO>);
                }

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(assignmentGroupDetailDTOs, page.HasValue ? page.Value : 1, assignmentGroupDetailDTOs.Count, page.HasValue);

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Editor/Assignments/_AssignmentGroupDetailsGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult DeleteEditorAssignmentDetails(string ids, string hdnAssignmentDetails)
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

                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(assignmentGroupDetailVMs, 1, assignmentGroupDetailVMs.Count, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.AssignmentDetail.DeleteSucceeded");

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentGroupDetailsGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult AddEditorAssignmentIndividual([Bind(Prefix = "EditorAssignment")] TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
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
                    if (a.ToOrgUnitId == transactionAssignmentVM.ToOrgUnitId && a.ToUserId == transactionAssignmentVM.ToUserId)
                    {
                        checkDetail = false;
                    }
                });
                if (checkDetail)
                {
                    transactionAssignmentVMs.Add(transactionAssignmentVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");

                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }


                string data = JsonConvert.SerializeObject(transactionAssignmentVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                ViewData["ControllerName"] = "Editor";

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentIndividualGridPartial.cshtml", grid),
                    hdnValue = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
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
                ViewData["ControllerName"] = "Editor";

                //GetResult<List<DeliveryReportDTO>> deliveryReportDTOs =
                //   HttpClientWrapper<GetResult<List<DeliveryReportDTO>>>.GetItemRequest(string.Format("api/Transaction/PrintDeliveryReport?transactionId={0}&cultureName={1}&perTransaction={2}", transactionId, SessionInfo.CultureShortName, isPerTransaction)).Result;

                //List<DeliveryReportVM> deliveryReportVMs = DeliveryReportMapper.Map(deliveryReportDTOs.Result);

                //TempData["DeliveryReport"] = deliveryReportVMs;
                //ViewData.TemplateInfo.HtmlFieldPrefix = "EditorAssignment";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentIndividualPartial.cshtml", transactionAssignmentVM), Index = id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult EditEditorAssignmentIndividual(int hdnIndexIndividual, [Bind(Prefix = "EditorAssignment")] TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
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

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                ViewData["ControllerName"] = "Editor";

                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentIndividualGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridEditorAssignmentIndividual(int? page, string param)
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

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Editor/Assignments/_AssignmentIndividualGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
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

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.DeleteSucceeded");

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentIndividualGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult AddEditorAssignmentGroup([Bind(Prefix = "EditorAssignment")] TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentGroupData, string hdnDetailAssignmentGroupData)
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
                    if (assignmentGroupDTO.Result != null)
                    {
                        foreach (AssignmentGroupDetailDTO assignmentGroupDetailDTO in assignmentGroupDTO.Result.GroupDetails)
                        {
                            TransactionAssignmentVM groupDetails = new TransactionAssignmentVM()
                            {
                                Id = assignmentGroupDetailDTO.Id,
                                GroupId = assignmentGroupDTO.Result.Id,
                                ToOrgUnitId = assignmentGroupDetailDTO.OrgUnitId,
                                ToOrgUnitName = assignmentGroupDetailDTO.OrgUnitName,
                                ToUserId = assignmentGroupDetailDTO.UserProfileId,
                                ToUserName = assignmentGroupDetailDTO.UserProfileName,
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

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                ViewData["ControllerName"] = "Editor";


                return Json(new { MessageType = MessageType.Information, MessageText = message, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentGroupGridPartial.cshtml", grid), hdnValue = data, hdnDetailData = detailData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridEditorAssignmentGroup(int? page, string param)
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

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Editor/Assignments/_AssignmentGroupGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        public ActionResult DeleteEditorAssignmentGroups(string ids, string hdnAssignmentGroupData, string hdnDetailAssignmentGroupData)
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

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, transactionAssignmentVMs.Count, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.DeleteSucceeded");

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentGroupGridPartial.cshtml", grid), hdnValue = data, hdnDetailData = detailData, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Editor.Assignments)]
        [ValidateAntiForgeryToken()]
        public ActionResult SendAssignmentsByEditor(string hdnAssignmentIndividualData, string hdnDetailAssignmentGroupData, string hdnTransactionId, int trayId, string pageSize, int? dateType, bool isConfirmed)
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

                transactionAssignmentVMs.ForEach(t => t.FromOrgUnitId = SessionInfo.OrgUnitId);

                transactionAssignmentVMs.RemoveAll(t => t.IsAssigned == false);

                for (int i = 0; i < transactionAssignmentVMs.Count(); i++)
                {
                    int count = 0;

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
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", hdnTransactionId.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
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


                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransaction?transactionId={0}", hdnTransactionId), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;

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

                string parameters = GetListTransactionParameters();

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
                ViewData["PageSize"] = settingVM.Value;
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
                    TransactionReportInfo = javaScriptSerializer.Serialize(postResult.Result),
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
        public ActionResult EditorAssignmentGroupDetailsEdit(int groupId, string groupName, string groupData)
        {
            try
            {
                ViewData["GroupName"] = groupName;
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();

                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                if (!string.IsNullOrEmpty(groupData))
                {
                    transactionAssignmentVMs.AddRange(javaScriptSerializer.Deserialize(groupData, typeof(List<TransactionAssignmentVM>)) as List<TransactionAssignmentVM>);
                }

                IAjaxGrid grid = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs.Where(t => t.GroupId == groupId).ToList(), 1, 0, false);
                ViewData["AssignmentGroupGrid"] = grid;

                ViewData["ControllerName"] = "Editor";

                return Json(new { View = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Assignments/_AssignmentGroupDetailEditPartial.cshtml", transactionAssignmentVMs.Where(t => t.GroupId == groupId).ToList()) }, JsonRequestBehavior.AllowGet);

            }


            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateEditorGroupDetails(List<TransactionAssignmentVM> transactionAssignmentVMs, string hdnGroupDataEdit, string hdnGroupEdit)
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




        public string GetListTransactionParameters()
        {
            try
            {
                StringBuilder result = new StringBuilder();

                string filter = Request.Form["filter"];
                string sortColumnName = Request.Form["gridColumn"];
                string dir = Request.Form["dir"];
                string pageIndex = Request.Form["page"];
                string searchColumn = Request.Form["searchColumn"];
                string fromDate = Request.Form["fromDate"];
                string toDate = Request.Form["toDate"];


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
                    result.Append("&FromDate=").Append(Convert.ToDateTime(fromDate).ToUniversalTime());
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    result.Append("&ToDate=").Append(Convert.ToDateTime(toDate).ToUniversalTime());
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

                result.Append("&PageSize=").Append(GridHelper.PageSize);

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
            catch (Exception)
            {
                throw;
            }
        }

        private string GetPartialPathByTrayId(int trayId)
        {
            switch ((TrayType)trayId)
            {
                case TrayType.MyTransactions:
                    return "~/Areas/User/Views/File/_MyTransactionsPartial.cshtml";
                case TrayType.DraftOutbound:
                    return "~/Areas/User/Views/File/_DraftOutboundPartial.cshtml";
                case TrayType.SentTransactions:
                    return "~/Areas/User/Views/File/_SentTransactionsPartial.cshtml";
                case TrayType.Saved:
                    return "~/Areas/User/Views/File/_SavedPartial.cshtml";
                case TrayType.OrgUnit:
                    return "~/Areas/User/Views/File/_OrgunitPartial.cshtml";
                case TrayType.Manager:
                    return "~/Areas/User/Views/File/_ManagerPartial.cshtml";
                case TrayType.Copies:
                    return "~/Areas/User/Views/File/_CopiesPartial.cshtml";
                case TrayType.YESSER:
                    return "~/Areas/User/Views/File/_YESSERPartial.cshtml";
            }
            return null;
        }

        #endregion Assignmnets

        #region AddTask
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult AddTask()
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

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, false);

                TaskAddVM taskAddVM = new TaskAddVM();

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_AddTaskPartial.cshtml", taskAddVM), GridHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult AddTask([Bind(Prefix = "TaskAdd")] TaskAddVM taskAddVM, List<TaskAddVM> TasksGrid, int TransactionIdForTask)
        {
            try
            {
                string message = string.Empty;
                bool checkDetail = true;
                int key = 0;
                if (TasksGrid != null)
                {
                    TasksGrid.ForEach(t =>
                    {
                        t.Key = key++;
                        if (t.SentToOrgUnitId == taskAddVM.SentToOrgUnitId && t.SentToUserId == taskAddVM.SentToUserId)
                        {
                            checkDetail = false;
                        }
                    });
                }
                else
                {
                    TasksGrid = new List<TaskAddVM>();
                }
                if (checkDetail)
                {
                    taskAddVM.Key = key + 1;
                    TasksGrid.Add(taskAddVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitUserAlreadyAdded");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                
                bool saveResult = SaveTasks(TasksGrid, TransactionIdForTask);

                if (!saveResult)
                {
                    message = DbRes.TResource("User.Task.TaskAdd.TaskAddedFail");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = DbRes.TResource("Admin.LetterType.AddSucceeded");
                List<TaskAddVM> gridData = TransactionController.GetTransactionTasks(TransactionIdForTask);

                string data = JsonConvert.SerializeObject(gridData);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), true);


                ViewData["ControllerName"] = "Editor";

                return Json(new
                {
                    Result = data,
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", Grid),
                    hdnValue = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Edit)]
        public ActionResult EditTask([Bind(Prefix = "TaskAdd")] TaskAddVM taskAddVM, List<TaskAddVM> TasksGrid, int TransactionIdForTask)
        {
            string message = string.Empty;
            string data = string.Empty;
            try
            {
                if (!TasksGrid.Any(copy => copy.SentToOrgUnitId == taskAddVM.SentToOrgUnitId &&
                    copy.SentToUserId == taskAddVM.SentToUserId && copy.Key != taskAddVM.Key))
                {
                    TaskAddVM taskAdd = new TaskAddVM
                    {
                        Id = taskAddVM.Id,
                        SentToOrgUnitId = taskAddVM.SentToOrgUnitId,
                        SentToOrgUnitName = taskAddVM.SentToOrgUnitName,
                        SentToUserId = taskAddVM.SentToUserId,
                        SentToUserName = taskAddVM.SentToUserName,
                        DeliveryDate = taskAddVM.DeliveryDate,
                        DeliveryDateH = taskAddVM.DeliveryDateH,
                        TaskDescription = taskAddVM.TaskDescription,
                        Key = taskAddVM.Key,
                        StatusId = TaskStatus.Sent.LookupIdentity(LookupCategory.TaskStatus, string.Empty)
                    };

                    TasksGrid.Remove(TasksGrid.FirstOrDefault(t => t.Key == taskAddVM.Key));
                    TasksGrid.Insert(taskAddVM.Key, taskAdd);

                    data = JsonConvert.SerializeObject(TasksGrid);

                    bool saveResult = SaveTasks(TasksGrid, TransactionIdForTask);

                    if (!saveResult)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, DbRes.TResource("User.Task.TaskAdd.TaskAddedFail"));
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }


                List<TaskAddVM> gridData = TransactionController.GetTransactionTasks(TransactionIdForTask);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                return Json(new
                {
                    Result = data,
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = taskAddVM.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, @"~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", Grid)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public bool SaveTasks(List<TaskAddVM> TasksGrid, int TransactionId)
        {

            List<TaskAddVM> Tasks = TasksGrid ?? new List<TaskAddVM>();

            if (Tasks.Count() > 0)
            {
                TransactionTaskVM transactionTaskVM = new TransactionTaskVM
                {
                    TransactionId = TransactionId,
                    TaskVMs = Tasks
                };

                foreach (TaskAddVM taskAddVM in Tasks)
                {
                    taskAddVM.ReceivedFromOrgUnitId = SessionInfo.OrgUnitId;
                    TaskWorkflowVM taskWorkflowVM = new TaskWorkflowVM();
                    taskWorkflowVM.ToOrgUnitId = taskAddVM.SentToOrgUnitId;
                    taskWorkflowVM.FromOrgUnitId = taskAddVM.SentToOrgUnitId;
                    taskAddVM.TaskWorkflows.Add(taskWorkflowVM);
                }

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostTransactionTasks?cultureName={0}", SessionInfo.CultureShortName), TransactionTaskMapper.Map(transactionTaskVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return false;
                }
            }

            return true;
        }


        [HttpPost]
        public ActionResult AssignTask(SentTaskVM sentTaskVM)
        {
            string message = string.Empty;
            GetResult<SentTaskDTO> sentTaskDTO =
                  HttpClientWrapper<GetResult<SentTaskDTO>>.GetItemRequest(string.Format("api/Transaction/GetSentTask?taskId={0}&cultureName={1}", sentTaskVM.Id, SessionInfo.CultureShortName)).Result;


            List<TaskAddVM> Tasks = new List<TaskAddVM>();
            TaskAddVM taskAdd = new TaskAddVM();
            taskAdd.Id = sentTaskVM.Id.Value;
            taskAdd.SentToOrgUnitId = sentTaskVM.ToOrgUnitId.Value;
            taskAdd.SentToUserId = sentTaskVM.ToUserId;
            taskAdd.DeliveryDate = sentTaskDTO.Result.DeliveryDate.ToString();
            taskAdd.DeliveryDateH = sentTaskDTO.Result.DeliveryDateH.ToString();
            taskAdd.TaskDescription = sentTaskDTO.Result.TaskDescription;
            taskAdd.Notes = sentTaskDTO.Result.Notes;
            //taskAdd.StatusName= sentTaskDTO.Result.StatusName;
            taskAdd.Status = sentTaskDTO.Result.Status.ToString();


            Tasks.Add(taskAdd);

            if (Tasks.Count() > 0)
            {
                TransactionTaskVM transactionTaskVM = new TransactionTaskVM
                {
                    TransactionId = Convert.ToInt32(sentTaskDTO.Result.TransactionNumber),
                    TaskVMs = Tasks,
                };

                //foreach (TaskAddVM taskAddVM in Tasks)
                //{
                //    taskAddVM.ReceivedFromOrgUnitId = SessionInfo.OrgUnitId;
                //    TaskWorkflowVM taskWorkflowVM = new TaskWorkflowVM();
                //    taskWorkflowVM.ToOrgUnitId = taskAddVM.SentToOrgUnitId;
                //    taskWorkflowVM.FromOrgUnitId = taskAddVM.SentToOrgUnitId;
                //    taskAddVM.TaskWorkflows.Add(taskWorkflowVM);
                //}

                PostResult postResult = HttpClientWrapper<PostResult>
                    .PostRequest(string.Format("api/Transaction/UpdateTransactionTasks?cultureName={0}", SessionInfo.CultureShortName),
                    TransactionTaskMapper.Map(transactionTaskVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return RedirectToAction("Tasks", "File");
                }
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            //return RedirectToAction("Tasks", "File");
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Delete)]
        public ActionResult DeleteTask(int TaskId, int TransactionId)
        {

            string message = string.Empty;
            if (TaskId > 0)
            {
                int DeletedTaskId = TaskId;

                List<int> DeletedTasksIds = new List<int>();
                DeletedTasksIds.Add(DeletedTaskId);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/DeleteTransactionTasks"), DeletedTasksIds).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<TaskAddVM> Tasks = TransactionController.GetTransactionTasks(TransactionId);
                IAjaxGrid gridTask = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(Tasks, 1, Tasks.Count(), false);

                return Json(new { Result = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", gridTask), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Reminder)]
        public ActionResult RemindTask(int taskId)
        {
            string message = string.Empty;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostTaskReminder?taskId={0}", taskId), null).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        [ValidateAntiForgeryToken()]
        public ActionResult PostTasks(int hdnTransactionId, string hdnTaskArray)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TaskAddVM> taskAddVMs = new List<TaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddVMs.AddRange(javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<TaskAddVM>)) as List<TaskAddVM>);
                }

                TransactionTaskVM transactionTaskVM = new TransactionTaskVM();
                transactionTaskVM.TransactionId = hdnTransactionId;
                transactionTaskVM.TaskVMs = taskAddVMs;

                foreach (TaskAddVM taskAddVM in taskAddVMs)
                {
                    taskAddVM.ReceivedFromOrgUnitId = SessionInfo.OrgUnitId;
                    TaskWorkflowVM taskWorkflowVM = new TaskWorkflowVM();
                    taskWorkflowVM.ToOrgUnitId = taskAddVM.SentToOrgUnitId;
                    taskWorkflowVM.FromOrgUnitId = taskAddVM.SentToOrgUnitId;
                    taskAddVM.TaskWorkflows.Add(taskWorkflowVM);
                }

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostTransactionTasks?cultureName={0}", SessionInfo.CultureShortName), TransactionTaskMapper.Map(transactionTaskVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, DbRes.TResource("User.Task.TaskAdd.TaskAddedFail"));

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, DbRes.TResource("User.Task.TaskAdd.TaskAddedSuccess"));

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, false);

                GetResult<List<TaskAddDTO>> taskDTOs =
              HttpClientWrapper<GetResult<List<TaskAddDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTasks?transactionId={0}&cultureName={1}", hdnTransactionId, SessionInfo.CultureShortName)).Result;

                var currentTasksGrid = GetCurrentTransactionTasks(hdnTransactionId);

                return Json(new { HtmlCurrentTasks = currentTasksGrid.ToJson("~/Areas/User/Views/Editor/TaskManagement/_CurrentTasksGridPartial.cshtml", this), GridHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Delete)]
        public ActionResult DeleteTasks(string ids, string hdnTaskArray)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TaskAddVM> taskAddVM = new List<TaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddVM = javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<TaskAddVM>)) as List<TaskAddVM>;
                }

                int index = Convert.ToInt32(ids);

                taskAddVM.RemoveAt(index);

                string data = JsonConvert.SerializeObject(taskAddVM);

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddVM, 1, taskAddVM.Count, false);

                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ResendTask(int TaskId, string Reason, int TransactionId, int ExpectedDays)
        {


            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/ResendTask?TaskId={0}&ResendReason={1}&ExpectedDays={2}", TaskId, Reason, ExpectedDays), null).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                return Json(new { MessageType = MessageType.Error });
            }

            List<TaskAddVM> gridData = TransactionController.GetTransactionTasks(TransactionId);

            AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

            return Json(new { MessageType = MessageType.Information, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", Grid) }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult TaskWorkflow(int taskIndex, string OrgSettings, List<OrgStructureInfoVM> OrgStructure, TaskAddVM taskAddVM)
        {
            try
            {
                GetResult<OrgUnitStructureDesignDTO> orgUnitStructureDesignDTO =
                       HttpClientWrapper<GetResult<OrgUnitStructureDesignDTO>>.GetItemRequest(string.Format("api/Admin/GetOrgUnitStructure?cultureName=" + SessionInfo.CultureShortName)).Result;

                orgUnitStructureDesignDTO.Result.OrgUnits.ForEach(o =>
                {

                    o.Users = new List<OrgUnitUserDTO>();
                    o.AssignmentPaper = null;
                });


                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                OrgUnitStructureDesignVM organizationUnitStructureDesignVMs = OrgUnitStructureDesignMapper.Map(orgUnitStructureDesignDTO.Result);
                List<OrgStructureInfoVM> OrgStructureInfoVMsss = new List<OrgStructureInfoVM>();
                foreach (OrgStructureInfoVM orgStructureInfoVM in organizationUnitStructureDesignVMs.OrgUnits)
                {
                    orgStructureInfoVM.LinkUnitsKeys = new List<int>();

                    List<TaskWorkflowVM> taskWorkflowVMs = taskAddVM.TaskWorkflows.Where(w => w.FromOrgUnitId == orgStructureInfoVM.Key).ToList();

                    foreach (TaskWorkflowVM taskWorkflowVM in taskWorkflowVMs)
                    {
                        orgStructureInfoVM.LinkUnitsKeys.Add(taskWorkflowVM.ToOrgUnitId);
                    }

                    if (orgUnitDTOs.Result.Find(jj => jj.Id == orgStructureInfoVM.Key) != null)
                    {
                        OrgStructureInfoVMsss.Add(orgStructureInfoVM);
                    }

                }

                ViewData["DepartmentsStructure"] = OrgStructureInfoVMsss;

                if (OrgStructure != null)
                {
                    foreach (OrgStructureInfoVM orgStructureInfoVM in OrgStructure)
                    {
                        if (orgStructureInfoVM.LinkUnitsKeys == null)
                        {
                            orgStructureInfoVM.LinkUnitsKeys = new List<int>();
                        }
                    }

                    ViewData["DepartmentsStructure"] = OrgStructure;
                }

                if (orgUnitStructureDesignDTO.Result.Settings != string.Empty)
                {
                    ViewData["SettingsStructure"] = OrgUnitStructureDesignMapper.Map(orgUnitStructureDesignDTO.Result).Settings;
                }
                else
                {
                    ViewData["SettingsStructure"] = JsonConvert.SerializeObject(new List<object>());
                }

                if (taskAddVM.OrgSettings != null)
                {
                    ViewData["SettingsStructure"] = OrgSettings;
                }

                Dictionary<string, string> listOfActions = new Dictionary<string, string>()
                {
                {DbRes.TResource("User.Task.SelectTaskUser.SelectUnitUser"), "SelectUser"}
                };

                ViewData["ListOfActions"] = listOfActions;
                ViewData["TaskIndex"] = taskIndex;
                ViewData["ControllerName"] = "Editor";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TaskWorkflowGialogPartial.cshtml", null) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SelectUser(int orgUnitKey)
        {
            try
            {
                ViewData["OrgUnitKey"] = orgUnitKey;
                ViewData["ListOfUsers"] = GetUsersByOrgUnitId(orgUnitKey);
                ViewData["ControllerName"] = "Editor";


                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_SelectUserGialogPartial.cshtml", null) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion AddTask

        #region OutboundDraft

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateOutboundDraft)]
        public ActionResult AddDraft()
        {
            try
            {
                TempData["ControllerName"] = "OutboundDraft";
                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                GetResult<List<FormDTO>> formDocumentDTOs =
                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

                if (formDocumentDTOs.Result != null)
                {
                    List<FormVM> formvm = FormMapper.Map(formDocumentDTOs.Result);
                    foreach (FormVM formDocumentVM in formvm)
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formDocumentVM.Id.ToString(),
                            Label = formDocumentVM.LocalName
                        });
                    }
                }

                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);

                AddOutboundDraftVM outboundDraftAddVM = new AddOutboundDraftVM();
                TextEditorViewModel editorViewModel = new TextEditorViewModel();

                Initialize(outboundDraftAddVM.Type);

                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();

                IAjaxGrid grid = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, false);

                ViewData["CopiesData"] = grid;

                List<TransactionExternalCopyVM> ExternalcopyVMs = new List<TransactionExternalCopyVM>();
                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(ExternalcopyVMs, 1, ExternalcopyVMs.Count, false);
                ViewData["ExternalCopiesData"] = gridExternalCopies;

                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(new List<TransactionCopyVM>());
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(new List<TransactionExternalCopyVM>());
                editorViewModel.EditorType = EditorType.TextEditor;
                editorViewModel.Content = string.Empty;

                ViewData["EditorViewModel"] = editorViewModel;

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                //ViewData["OrgUnitsManagers"] = TransactionHelper.GetOrgUnitsManagers();

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                ViewData["ExternalCopiesPartiesData"] = ExternalPartyMapper.Map(externalPartyDTOs.Result) != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/OutboundDraft/AddDraft.cshtml", outboundDraftAddVM) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateOutboundDraft)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOutboundDraft(string hdnInboundId, AddOutboundDraftVM outboundDraftAddVM, string hdnExternalCopies, TextEditorViewModel editorViewModel, string hdnAttachments, string hdnCopies, string hdnArchivigdata)
        {
            try
            {
                outboundDraftAddVM.OrgUnitId = SessionInfo.OrgUnitId;

                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                outboundDraftAddVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                outboundDraftAddVM.Copies = javaScriptSerializer.Deserialize(hdnCopies, typeof(List<TransactionCopyVM>)) as List<TransactionCopyVM>;
                outboundDraftAddVM.ExternalCopies = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(List<TransactionExternalCopyVM>)) as List<TransactionExternalCopyVM>;

                if (outboundDraftAddVM.IsSigned && editorViewModel.EditorType == EditorType.TextEditor)
                {
                    outboundDraftAddVM.EditorType = EditorType.Scanning;
                    outboundDraftAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;

                    PdfDocument doc = new PdfDocument();
                    PdfHtmlLayoutFormat pdfHtmlLayoutFormat = new PdfHtmlLayoutFormat();
                    PdfPageSettings pdfPageSettings = new PdfPageSettings();
                    MemoryStream stream = new MemoryStream();

                    string html = ((string[])editorViewModel.Content)[0].ToString();

                    Thread thread = new Thread(() =>
                    {
                        doc.LoadFromHTML(HttpUtility.HtmlDecode(html), false, pdfPageSettings, pdfHtmlLayoutFormat);

                        doc.SaveToStream(stream);

                        doc.Close();
                    });

                    thread.SetApartmentState(ApartmentState.STA);

                    thread.Start();

                    thread.Join();

                    while (thread.IsAlive)
                    {

                    }

                    outboundDraftAddVM.DocumentVM.Content = stream.ToArray();
                    outboundDraftAddVM.DocumentVM.Size = stream.ToArray().Length;
                }
                else
                {
                    if (editorViewModel.EditorType == EditorType.TextEditor)
                    {
                        outboundDraftAddVM.EditorType = EditorType.TextEditor;
                        outboundDraftAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        outboundDraftAddVM.DocumentVM.Content = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]);
                        outboundDraftAddVM.DocumentVM.Size = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]).Length;
                    }
                    else
                    {
                        var content = DocumentViewerHelper.GetPDFFile(((string[])(editorViewModel.Content))[0]);
                        outboundDraftAddVM.EditorType = EditorType.Scanning;
                        outboundDraftAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        outboundDraftAddVM.DocumentVM.Content = content;
                        outboundDraftAddVM.DocumentVM.Size = content.Length;

                    }
                }
                List<TransactionArchiveDTO> transactionArchiveDTOs = javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveDTO>)) as List<TransactionArchiveDTO>;

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                transactionArchiveDTOs.ForEach(t =>
                {
                    if (!t.IsMainDocument && t.IsNew)
                    {
                        outboundDraftAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM = new DocumentVM();
                        outboundDraftAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Content = documentData[t.Id];
                        outboundDraftAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Size = documentData[t.Id].Length;
                        outboundDraftAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    }
                });

                PostObjectResult<TransactionDetailsDTO> postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransactionDraft?transactionId=" + hdnInboundId, OutboundDraftMapper.Map(outboundDraftAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundDraft.AddSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information, OutboundDraftNumber = TransactionDetailsMapper.Map(postResult.Result).Number, Id = TransactionDetailsMapper.Map(postResult.Result).Id, Date = TransactionDetailsMapper.Map(postResult.Result).HijriDate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        public ActionResult EditDraft(int id, int transactionCategoryId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                string message = string.Empty;

                GetResult<EditOutboundDraftDTO> result =
                HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?transactionid={0}&orgUnitId={1}&cultureName={2}", id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


                if (result.Result == null)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, result.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                EditOutboundDraftVM outboundDraftEditVM = OutboundDraftMapper.Map(result.Result);
                //outboundDraftEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundDraftEditVM.HijriRecordDate);

                ViewData["TransactionCategory"] = transactionCategoryId;

                TempData["ControllerName"] = "OutboundDraft";

                TextEditorViewModel editorViewModel = new TextEditorViewModel();

                GetResult<List<FormDTO>> formDocumentDTOs =
                                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

                if (formDocumentDTOs.Result != null)
                {
                    foreach (FormVM formDocumentVM in FormMapper.Map(formDocumentDTOs.Result))
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formDocumentVM.Id.ToString(),
                            Label = formDocumentVM.LocalName
                        });
                    }
                }

                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);

                Initialize(TransactionCategory.DraftOutbound);

                IAjaxGrid gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Attachments, 1, 0, false);
                ViewData["AttachmentData"] = gridAttachment;

                List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();

                if (outboundDraftEditVM.DocumentVM != null)
                {
                    if (outboundDraftEditVM.EditorType == EditorType.TextEditor)
                    {
                        editorViewModel.EditorType = EditorType.TextEditor;
                        editorViewModel.Content = System.Text.Encoding.UTF8.GetString(outboundDraftEditVM.DocumentVM.Content);
                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;
                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;

                        ViewData["SessionMainDocumentKey"] = "DocoNutDocument";
                        Session["DocoNutDocument"] = outboundDraftEditVM.DocumentVM.Content;
                    }

                }

                ViewData["EditorViewModel"] = editorViewModel;

                if (outboundDraftEditVM.Attachments != null)
                {
                    //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                    foreach (TransactionAttachmentVM transactionAttachmentVM in outboundDraftEditVM.Attachments)
                    {
                        if (transactionAttachmentVM.DocumentVM != null)
                        {
                            transactionArchiveVMs.Add(new TransactionArchiveVM
                            {
                                Id = Guid.NewGuid().ToString(),
                                DocumentId = transactionAttachmentVM.DocumentVM.Id,
                                AttachmentTypeId = transactionAttachmentVM.TypeId,
                                ArcivingTypeName = transactionAttachmentVM.TypeName,
                                IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted,
                                EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                            });
                        }
                        //if (transactionAttachmentVM.Archivable)
                        //{
                        //    dataSource.Add(new AutoCompleteDataSource { Label = transactionAttachmentVM.TypeName, Value = transactionAttachmentVM.TypeId.ToString(), Parameters = new object[] { transactionAttachmentVM.Archivable } });
                        //}
                    }

                    //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                }

                IAjaxGrid gridCopies = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Copies, 1, outboundDraftEditVM.Copies.Count, false);
                ViewData["CopiesData"] = gridCopies;

                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.ExternalCopies, 1, outboundDraftEditVM.ExternalCopies.Count, false);
                ViewData["ExternalCopiesData"] = gridExternalCopies;


                IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs, 1, 0, false);
                ViewData["ArchivingData"] = gridArchiving;

                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Attachments);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(result.Result.OutboundDraftBasicInfo.DestinationId);

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
           HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                ViewData["ExternalCopiesPartiesData"] = ExternalPartyMapper.Map(externalPartyDTOs.Result) != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                         HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));


                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                //ViewData["OrgUnitsManagers"] = TransactionHelper.GetOrgUnitsManagers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //         HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (subjectClassificationDTOs.Result != null && outboundDraftEditVM.OutboundDraftBasicInfo.SubjectClassifications != null)
                //{
                //    outboundDraftEditVM.OutboundDraftBasicInfo.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}

                //ViewData["SubjectClassificationsData"] = TransactionHelper.BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = TransactionHelper.BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/OutboundDraft/EditDraft.cshtml", outboundDraftEditVM) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditOutboundDraft(EditOutboundDraftVM outboundDraftEditVM, TextEditorViewModel editorViewModel, string hdnAttachments, string hdnCopies, string hdnExternalCopies, string hdnArchivigdata)
        {
            try
            {
                outboundDraftEditVM.OrgUnitId = SessionInfo.OrgUnitId;

                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                outboundDraftEditVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                outboundDraftEditVM.Copies = javaScriptSerializer.Deserialize(hdnCopies, typeof(List<TransactionCopyVM>)) as List<TransactionCopyVM>;
                outboundDraftEditVM.ExternalCopies = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(List<TransactionExternalCopyVM>)) as List<TransactionExternalCopyVM>;

                if (outboundDraftEditVM.IsSigned && editorViewModel.EditorType == EditorType.TextEditor)
                {
                    outboundDraftEditVM.EditorType = EditorType.Scanning;
                    outboundDraftEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;

                    PdfDocument doc = new PdfDocument();
                    PdfHtmlLayoutFormat pdfHtmlLayoutFormat = new PdfHtmlLayoutFormat();
                    PdfPageSettings pdfPageSettings = new PdfPageSettings();
                    MemoryStream stream = new MemoryStream();

                    string html = ((string[])editorViewModel.Content)[0].ToString();

                    Thread thread = new Thread(() =>
                    {
                        doc.LoadFromHTML(HttpUtility.HtmlDecode(html), false, pdfPageSettings, pdfHtmlLayoutFormat);

                        doc.SaveToStream(stream);

                        doc.Close();
                    });

                    thread.SetApartmentState(ApartmentState.STA);

                    thread.Start();

                    thread.Join();

                    while (thread.IsAlive)
                    {

                    }

                    outboundDraftEditVM.DocumentVM.Content = stream.ToArray();
                    outboundDraftEditVM.DocumentVM.Size = stream.ToArray().Length;
                }
                else
                {
                    if (editorViewModel.EditorType == EditorType.TextEditor)
                    {
                        outboundDraftEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        outboundDraftEditVM.DocumentVM.Content = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]);
                    }
                    else
                    {
                        outboundDraftEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        outboundDraftEditVM.DocumentVM.Content = DocumentViewerHelper.GetPDFFile(((string[])(editorViewModel.Content))[0]);
                    }
                }
                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                transactionArchiveVMs.ForEach(t =>
                {
                    if (!t.IsMainDocument && t.IsDeleted)
                    {
                        outboundDraftEditVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && a.DocumentVM.Id == t.DocumentId)
                            {
                                a.DocumentVM.IsDeleted = true;
                            }
                        });
                    }
                    if (!t.IsMainDocument && t.IsNew)
                    {
                        TransactionAttachmentVM transactionAttachmentVM = outboundDraftEditVM.Attachments.Where(s => s.TypeId == t.AttachmentTypeId).SingleOrDefault();

                        transactionAttachmentVM.DocumentVM = new DocumentVM();
                        transactionAttachmentVM.DocumentVM.IsDeleted = false;
                        transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                        transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                        transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    }
                    if (!t.IsMainDocument && !t.IsDeleted && !t.IsNew)
                    {
                        outboundDraftEditVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && documentData != null)
                            {
                                if (a.DocumentVM.Id == t.DocumentId && documentData.Keys.Contains(t.Id))
                                {
                                    TransactionAttachmentVM transactionAttachmentVM = outboundDraftEditVM.Attachments.Where(s => s.DocumentVM != null)
                                        .Where(s => s.DocumentVM.Id == t.DocumentId).FirstOrDefault();

                                    transactionAttachmentVM.DocumentVM.IsDeleted = false;
                                    transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                                    transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                                    transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                                }
                            }
                        });
                    }
                });

                Session["DocumentData"] = null;

                PostResult putResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransaction", OutboundDraftMapper.Map(outboundDraftEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TempData["TransactionData"] = outboundDraftEditVM;
                TempData.Keep("TransactionData");

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundDraft.UpdateSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditOutboundDraftForInbound(EditOutboundDraftVM outboundDraftEditVM, string hdnExternalCopies, TextEditorViewModel editorViewModel, string hdnAttachments, string hdnCopies, string hdnArchivigdata)
        {
            try
            {
                outboundDraftEditVM.OrgUnitId = SessionInfo.OrgUnitId;

                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                outboundDraftEditVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                outboundDraftEditVM.Copies = javaScriptSerializer.Deserialize(hdnCopies, typeof(List<TransactionCopyVM>)) as List<TransactionCopyVM>;
                outboundDraftEditVM.ExternalCopies = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(List<TransactionExternalCopyVM>)) as List<TransactionExternalCopyVM>;


                if (editorViewModel.EditorType == EditorType.TextEditor)
                {
                    outboundDraftEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    outboundDraftEditVM.DocumentVM.Content = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]);
                }
                else
                {
                    outboundDraftEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundDraftEditVM.DocumentVM.Content = DocumentViewerHelper.GetPDFFile(((string[])(editorViewModel.Content))[0]);
                }

                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                transactionArchiveVMs.ForEach(t =>
                {
                    if (!t.IsMainDocument && t.IsDeleted)
                    {
                        outboundDraftEditVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && a.DocumentVM.Id == t.DocumentId)
                            {
                                a.DocumentVM.IsDeleted = true;
                            }
                        });
                    }
                    if (!t.IsMainDocument && t.IsNew)
                    {
                        TransactionAttachmentVM transactionAttachmentVM = outboundDraftEditVM.Attachments.Where(s => s.TypeId == t.AttachmentTypeId).SingleOrDefault();

                        transactionAttachmentVM.DocumentVM = new DocumentVM();
                        transactionAttachmentVM.DocumentVM.IsDeleted = false;
                        transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                        transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                        transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    }
                    if (!t.IsMainDocument && !t.IsDeleted && !t.IsNew)
                    {
                        outboundDraftEditVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && documentData != null)
                            {
                                if (a.DocumentVM.Id == t.DocumentId && documentData.Keys.Contains(t.Id))
                                {
                                    TransactionAttachmentVM transactionAttachmentVM = outboundDraftEditVM.Attachments.Where(s => s.DocumentVM != null)
                                        .Where(s => s.DocumentVM.Id == t.DocumentId).FirstOrDefault();

                                    transactionAttachmentVM.DocumentVM.IsDeleted = false;
                                    transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                                    transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                                    transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                                }
                            }
                        });
                    }
                });

                Session["DocumentData"] = null;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Transaction/PutTransaction", OutboundDraftMapper.Map(outboundDraftEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TempData["TransactionData"] = outboundDraftEditVM;
                TempData.Keep("TransactionData");

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundDraft.UpdateSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetUnitUsers(int id, bool addSelectOption = false)
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

        #endregion OutboundDraft

        #region OutboundInternal

        [HttpGet]
        public ActionResult EditInternal(int id)
        {
            try
            {

                string message = string.Empty;

                GetResult<EditOutboundInternalDTO> result =
                HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?transactionid={0}&orgUnitId={1}&cultureName={2}", id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


                if (result.Result == null || result.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, result.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                EditOutboundInternalVM outboundInternalEditVM = OutboundInternalMapper.Map(result.Result);
                // outboundInternalEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundInternalEditVM.HijriRecordDate);

                TempData["ControllerName"] = "OutboundInternal";

                Initialize(TransactionCategory.InternalOutbound);

                IAjaxGrid gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundInternalEditVM.Attachments, 1, 0, false);
                ViewData["AttachmentData"] = gridAttachment;

                List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();

                if (outboundInternalEditVM.DocumentVM != null)
                {
                    string documentId = Guid.NewGuid().ToString();
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        Id = documentId,
                        IsMainDocument = true,
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundInternalEditVM.DocumentVM.Id.ToString()),
                        DocumentId = outboundInternalEditVM.DocumentVM.Id,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                        SessionInfo.CultureShortName).Result.Text
                    });

                }

                if (outboundInternalEditVM.Attachments != null)
                {
                    //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                    foreach (TransactionAttachmentVM transactionAttachmentVM in outboundInternalEditVM.Attachments)
                    {
                        if (transactionAttachmentVM.DocumentVM != null)
                        {
                            transactionArchiveVMs.Add(new TransactionArchiveVM
                            {
                                Id = Guid.NewGuid().ToString(),
                                EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                                DocumentId = transactionAttachmentVM.DocumentVM.Id,
                                AttachmentTypeId = transactionAttachmentVM.TypeId,
                                ArcivingTypeName = transactionAttachmentVM.TypeName,
                                IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted
                            });
                        }
                        //if (transactionAttachmentVM.Archivable)
                        //{
                        //    dataSource.Add(new AutoCompleteDataSource { Label = transactionAttachmentVM.TypeName, Value = transactionAttachmentVM.TypeId.ToString(), Parameters = new object[] { transactionAttachmentVM.Archivable } });
                        //}
                    }

                    //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                }


                IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs, 1, 0, false);
                ViewData["ArchivingData"] = gridArchiving;

                IAjaxGrid gridNames = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(outboundInternalEditVM.Names, 1, outboundInternalEditVM.Names.Count, false);
                ViewData["NamesData"] = gridNames;

                IAjaxGrid gridLinks = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(outboundInternalEditVM.Links, 1, outboundInternalEditVM.Links.Count, false);
                ViewData["LinksData"] = gridLinks;

                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Links);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //    HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (subjectClassificationDTOs.Result != null && outboundInternalEditVM.OutboundInternalBasicInfoEdit.SubjectClassifications != null)
                //{
                //    outboundInternalEditVM.OutboundInternalBasicInfoEdit.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}

                //ViewData["SubjectClassificationsData"] = TransactionHelper.BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = TransactionHelper.BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/OutboundInternal/EditInternal.cshtml", outboundInternalEditVM) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditOutboundInternal(EditOutboundInternalVM outboundInternalEditVM, string hdnAttachments, string hdnNames, string hdnLinks, string hdnArchivigdata)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                outboundInternalEditVM.OrgUnitId = SessionInfo.OrgUnitId;

                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;

                outboundInternalEditVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                outboundInternalEditVM.Names = javaScriptSerializer.Deserialize(hdnNames, typeof(List<TransactionNameVM>)) as List<TransactionNameVM>;
                outboundInternalEditVM.Links = javaScriptSerializer.Deserialize(hdnLinks, typeof(List<TransactionLinkVM>)) as List<TransactionLinkVM>;

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                transactionArchiveVMs.ForEach(t =>
                {
                    if (t.IsMainDocument && !t.IsDeleted)
                    {
                        outboundInternalEditVM.DocumentVM = new DocumentVM();
                        if (documentData != null)
                        {
                            if (documentData.Keys.Contains(t.Id))
                            {
                                outboundInternalEditVM.DocumentVM.Content = documentData[t.Id];
                                outboundInternalEditVM.DocumentVM.Size = documentData[t.Id].Length;
                                outboundInternalEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                            }
                        }
                    }
                    if (t.IsMainDocument && t.IsDeleted)
                    {
                        outboundInternalEditVM.DocumentVM = new DocumentVM();
                        outboundInternalEditVM.DocumentVM.Id = t.DocumentId;
                        outboundInternalEditVM.DocumentVM.IsDeleted = true;
                    }
                    if (!t.IsMainDocument && t.IsDeleted)
                    {
                        outboundInternalEditVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && a.DocumentVM.Id == t.DocumentId)
                            {
                                a.DocumentVM.IsDeleted = true;
                            }
                        });
                    }
                    if (!t.IsMainDocument && t.IsNew)
                    {
                        TransactionAttachmentVM transactionAttachmentVM = outboundInternalEditVM.Attachments.Where(s => s.TypeId == t.AttachmentTypeId).SingleOrDefault();

                        transactionAttachmentVM.DocumentVM = new DocumentVM();
                        transactionAttachmentVM.DocumentVM.IsDeleted = false;
                        transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                        transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                        transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    }
                    if (!t.IsMainDocument && !t.IsDeleted && !t.IsNew)
                    {
                        outboundInternalEditVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && documentData != null)
                            {
                                if (a.DocumentVM.Id == t.DocumentId && documentData.Keys.Contains(t.Id))
                                {
                                    TransactionAttachmentVM transactionAttachmentVM = outboundInternalEditVM.Attachments.Where(s => s.DocumentVM != null)
                                        .Where(s => s.DocumentVM.Id == t.DocumentId).FirstOrDefault();

                                    transactionAttachmentVM.DocumentVM.IsDeleted = false;
                                    transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                                    transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                                    transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                                }
                            }
                        });
                    }
                });

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Transaction/PutTransaction?cultureName=" + SessionInfo.CultureShortName, OutboundInternalMapper.Map(outboundInternalEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TempData["TransactionData"] = outboundInternalEditVM;
                TempData.Keep("TransactionData");

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundInternal.UpdateSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information, IsPopularization = outboundInternalEditVM.OutboundInternalBasicInfoEdit.GroupId.HasValue }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion OutboundInternal

        #region Shared
        [HttpGet]
        public ActionResult GetExternalPartiesbyLetterId(string LetterId, int selectedParty = -1)
        {
            try
            {
                List<ExternalPartyVM> externalPartyVMs = new List<ExternalPartyVM>();
                if (LetterId != "")
                {
                    GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                   HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                    externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                }

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(externalPartyVMs, selectedParty);

                string data = "";
                foreach (var item in externalPartyVMs)
                {
                    if (item.YasserRegistered)
                    {
                        data = data == string.Empty ? item.Id.ToString() : (data + "," + item.Id.ToString());
                    }
                }

                ViewData["isYesserRegisterd"] = data;

                return PartialView("~/Areas/User/Views/Editor/EditorOutboundDraft/ReadWrite/_ExternalPartiesPartial.cshtml", new EditorOutboundDraftExternalPartiesVM() { ExternalPartyId = selectedParty != -1 ? selectedParty : 1 });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Initialize(TransactionCategory transactionCategory)
        {
            IAjaxGrid grid = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAttachmentVM>(), 1, 0, false);
            ViewData["AttachmentData"] = grid;

            IAjaxGrid gridNames = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionNameVM>(), 1, 0, false);
            ViewData["NamesData"] = gridNames;

            IAjaxGrid gridLinks = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionLinkVM>(), 1, 0, false);
            ViewData["LinksData"] = gridLinks;

            IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, false);
            ViewData["ArchivingData"] = gridArchiving;

            ViewData["TransactionCategory"] = (int)transactionCategory;

            GetResult<List<TransactionAssignmentDTO>> transactionAssignmentDTOs =
                   HttpClientWrapper<GetResult<List<TransactionAssignmentDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitBeneficiaries?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result), 1, 0, false);
            ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

            IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, false);
            ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

            ViewData["AssignmentPaperData"] = JsonConvert.SerializeObject(TransactionAssignmentMapper.Map(transactionAssignmentDTOs.Result));

            //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, false);
            //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;

            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


            ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

            ViewData["ExternalPartiesData"] = new TreeViewModel();

            //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
            //        HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //ViewData["SubjectClassificationsData"] = TransactionHelper.BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

            //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
            //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //ViewData["SuggestedTopicsData"] = TransactionHelper.BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

            ViewData["LinkTypeData"] = TransactionHelper.GetLinkTypes(transactionCategory);
            ViewData["PrioritiesData"] = TransactionHelper.GetPriorities(transactionCategory);
            ViewData["AttachmentsTypeData"] = TransactionHelper.GetAttachmentTypes(transactionCategory);
            ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
            ViewData["LetterTypeData"] = TransactionHelper.GetLetterTypes(transactionCategory);
            ViewData["ActionData"] = TransactionHelper.GetAllActions();
            ViewData["AssignmentGroupData"] = TransactionHelper.GetUserAssignmentGroups();
            ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
            ViewData["HasAssignmentPaper"] = TransactionHelper.CheckOrgUnitHasAssignmentPaper();
            ViewData["IsAllowedToCreateGroup"] = TransactionHelper.CheckOrgUnitIsAllowedToCreateGroup();

            //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(new List<TransactionAttachmentVM>());
            //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(new List<TransactionNameVM>());
            //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(new List<TransactionLinkVM>());
            //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
            //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(new List<AutoCompleteDataSource>());
            ViewData["OrgUnitsUsersData"] = null;
            ViewData["DocumentId"] = null;

            ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
            ViewData["AllActionsData"] = TransactionHelper.GetAllActions();

            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();

            if (!string.IsNullOrEmpty(ViewData["ActionData"].ToString()))
            {
                autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["ActionData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
            }

            ViewData["HasActions"] = autoCompleteDataSources.Count > 0;

            Session["DocumentData"] = null;
            ViewData["SessionArchiveDocumentKey"] = Guid.NewGuid().ToString();
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
                    List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
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

        [HttpPost]
        public ActionResult GetTransactionName(string civilId)
        {
            try
            {
                GetResult<TransactionNameDTO> transactionNameDTO =
                     HttpClientWrapper<GetResult<TransactionNameDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionName?cultureName={0}&civilID={1}", SessionInfo.CultureShortName, civilId)).Result;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                if (transactionNameDTO.Result != null)
                {
                    string json = javaScriptSerializer.Serialize(TransactionNameMapper.Map(transactionNameDTO.Result));
                    return Json(TransactionNameMapper.Map(transactionNameDTO.Result));
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
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


                if (managerDTOs.Result != null)
                {
                    List<ManagerVM> ManagerVMs = ManagerMapper.Map(managerDTOs.Result);
                    foreach (ManagerVM managerVM in ManagerVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = managerVM.Id.ToString(),
                            Label = managerVM.LocalName
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
        public ActionResult HideTransactionAssignment(int id, int transactionId)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/HideTransactionAssignment?assignmentId={0}", id), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/HideAssignment?transactionId={0}", transactionId), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult HideTransactionAssignments(string transactionIds, string assignmentIds)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/HideTransactionAssignments?assignmentIds={0}", assignmentIds), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/HideAssignments?transactionIds={0}", transactionIds), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Shared

        #region Copies


        private readonly int count = 0;

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesInternal.Add)]
        public ActionResult AddCopy([Bind(Prefix = "TransactionCopy")] TransactionCopyVM TransactionCopyVM, List<TransactionCopyVM> Copies)
        {
            try
            {
                string message = string.Empty;
                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();
                Copies = Copies ?? new List<TransactionCopyVM>();
                if (!Copies.Any(copy => copy.OrgUnitId == TransactionCopyVM.OrgUnitId && copy.UserId == TransactionCopyVM.UserId))
                {
                    if ((TransactionCopyVM.UserId != null && !Copies.Any(copy => copy.OrgUnitId == TransactionCopyVM.OrgUnitId && copy.UserId == null)) ||
                        (TransactionCopyVM.UserId == null && !Copies.Any(copy => copy.OrgUnitId == TransactionCopyVM.OrgUnitId && copy.UserId != null)))
                    {
                        TransactionCopyVM.Status = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, SessionInfo.CultureShortName);
                        TransactionCopyVM.Id = 0;
                        TransactionCopyVM.Key = Copies.Count + 1;
                        copyVMs.Add(TransactionCopyVM);
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (TransactionCopyVM.UserId == null)
                    {
                        //Copy for entity was added before
                        message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                    }
                    else
                    {
                        //Copy for user in entity was added before
                        message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitUserAlreadyAdded");
                    }

                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                if (TransactionCopyVM.OrgUnitId > 0)
                {
                    OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(TransactionCopyVM.OrgUnitId, SessionInfo.CultureShortName);
                    TransactionCopyVM.OrgUnitName = orgUnitDTO.Name;
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Copies/_CopiesGridPartial.cshtml",
                           (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesInternal.Edit)]
        public ActionResult EditCopy([Bind(Prefix = "TransactionCopy")] TransactionCopyVM copyVM, List<TransactionCopyVM> Copies)
        {
            try
            {
                string message = string.Empty;
                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();
                if (!Copies.Any(copy => copy.OrgUnitId == copyVM.OrgUnitId && copy.UserId == copyVM.UserId && copy.Key != copyVM.Key))
                {
                    if ((copyVM.UserId != null && !Copies.Any(copy => copy.OrgUnitId == copyVM.OrgUnitId && copy.UserId == null)) ||
                    (copyVM.UserId == null && !Copies.Any(copy => copy.OrgUnitId == copyVM.OrgUnitId && copy.UserId != null)))
                    {
                        copyVMs.Add(copyVM);
                    }
                    else if (copyVM.UserId != null && Copies.Any(copy => copy.OrgUnitId == copyVM.OrgUnitId && copy.UserId == null))
                    {
                        copyVMs.Add(copyVM);
                    }
                    else
                    {
                        int count = 0;
                        foreach (var item in Copies)
                        {
                            if (item.OrgUnitId == copyVM.OrgUnitId)
                            {
                                count++;

                            }
                        }
                        if (count == 1)
                        {
                            copyVMs.Add(copyVM);
                        }
                        else
                        {
                            message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                if (copyVM.OrgUnitId > 0)
                {
                    OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(copyVM.OrgUnitId, SessionInfo.CultureShortName);
                    copyVM.OrgUnitName = orgUnitDTO.Name;
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = copyVM.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Copies/_CopiesGridPartial.cshtml",
                    (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true))
                }
                , JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridCopies(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();

                if (!string.IsNullOrEmpty(param))
                {
                    object objects = javaScriptSerializer.Deserialize(param, typeof(object[]));

                    List<object> list = ((object[])objects).ToList();

                    objects = list.ToArray<object>();

                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionNameVM)
                        {
                            copyVMs.Add(o as TransactionCopyVM);
                        }
                        else
                        {
                            TransactionCopyVM copyVM =
                                javaScriptSerializer.Deserialize<TransactionCopyVM>(javaScriptSerializer.Serialize(o));

                            copyVMs.Add(copyVM);
                        }
                    });
                }

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(copyVMs, page.HasValue ? page.Value : 1, copyVMs.Count, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Editor/Copies/_CopiesGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesInternal.Delete)]
        public ActionResult DeleteCopies(string ids, string hdnCopiesGrid)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();

                if (!string.IsNullOrEmpty(hdnCopiesGrid))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnCopiesGrid, typeof(object[]));

                    List<object> list = ((object[])objects).ToList();

                    objects = list.ToArray<object>();

                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionCopyVM)
                        {
                            copyVMs.Add(o as TransactionCopyVM);
                        }
                        else
                        {
                            TransactionCopyVM copyVM =
                                javaScriptSerializer.Deserialize<TransactionCopyVM>(javaScriptSerializer.Serialize(o));

                            copyVMs.Add(copyVM);
                        }
                    });
                }

                List<int> copyIds = ids.Split(',').Select(int.Parse).ToList();

                copyIds.ForEach(id =>
                {
                    TransactionCopyVM remove = copyVMs.Where(n => n.Key == id).FirstOrDefault();
                    copyVMs.Remove(remove);
                });

                string data = JsonConvert.SerializeObject(copyVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Copies/_CopiesGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetCopy(string ids, string hdnCopiesGrid)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();

                if (!string.IsNullOrEmpty(hdnCopiesGrid))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnCopiesGrid, typeof(object[]));

                    List<object> list = ((object[])objects).ToList();

                    objects = list.ToArray<object>();

                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionCopyVM)
                        {
                            copyVMs.Add(o as TransactionCopyVM);
                        }
                        else
                        {
                            TransactionCopyVM copyVM =
                                javaScriptSerializer.Deserialize<TransactionCopyVM>(javaScriptSerializer.Serialize(o));

                            copyVMs.Add(copyVM);
                        }
                    });
                }

                List<int> copyIds = ids.Split(',').Select(int.Parse).ToList();

                copyIds.ForEach(id =>
                {
                    TransactionCopyVM edit = copyVMs.Where(n => n.Key == id).FirstOrDefault();

                });

                string data = JsonConvert.SerializeObject(copyVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/Copies/_CopiesGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Inbound.EditInbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTransactionCopy([Bind(Prefix = "TransactionCopy")] TransactionCopyVM copyVM, List<TransactionCopyVM> Copies, string hdntransactionId)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionCopyVM> transactionCopyVMs = null;
                if (Copies != null)
                {
                    transactionCopyVMs = Copies;
                }

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddTransactionCopies?transactionId={0}", hdntransactionId), TransactionCopyMapper.Map(transactionCopyVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Inbound.SaveSucceeded");

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
        #endregion
    }

    internal class TransactionHelper
    {

        public static string GetLinkTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<LinkDTO>> linkDTOs =
                    HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLinkTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;

                if (linkDTOs.Result != null)
                {
                    List<LinkVM> linkVMs = LinkMapper.Map(linkDTOs.Result);
                    foreach (LinkVM linkVM in linkVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = linkVM.Id.ToString(),
                            Label = linkVM.LocalName
                        });
                        //Set first value as defualt value
                    }
                }


                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetPriorities(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(transactionCategory);
                if (priorityVMs.Result != null)
                {

                    foreach (PriorityVM priorityVM in priorityVMs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = priorityVM.Id.ToString(),
                            Label = priorityVM.LocalName,
                            Parameters = new object[] { priorityVM.HasDate }
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

        public static string GetAttachmentTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs =
                    HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetAttachmentTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;

                if (attachmentTypeDTOs.Result != null)
                {
                    List<AttachmentTypeVM> attachmentTypeVMs = AttachmentTypeMapper.Map(attachmentTypeDTOs.Result);
                    foreach (AttachmentTypeVM attachmentTypeVM in attachmentTypeVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = attachmentTypeVM.Id.ToString(),
                            Label = attachmentTypeVM.LocalName,
                            Parameters = new object[] { attachmentTypeVM.Archivable }
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
        public static string GetLookupItemsForAutoComplete(LookupCategory lookupCategory)
        {
            try
            {
                GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (lookupVMs.Result != null)
                {

                    foreach (LookupVM lookupVM in lookupVMs.Result)
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


        public static string GetTransactionTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<TransactionTypeVM>> transactionTypeVMs = LookupsHelper.GetTransactionTypes(transactionCategory);

                if (transactionTypeVMs.Result != null)
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

        public static string GetSaveReason(LookupCategory lookupCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetActiveLookupItemswithoutCached(lookupCategory, "ar");

                if (lookupVMs.Result != null)
                {

                    foreach (LookupVM lookupVM in lookupVMs.Result)
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

        public static string GetTransactionTypesForSearch(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<TransactionTypeVM>> transactionTypeVMs = LookupsHelper.GetTransactionTypes(transactionCategory);

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

        public static string GetTransactionConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (permissionDTOs.Result != null)
                {
                    List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
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
        public static string GetTransactionConfidentialityForSearch()
        {
            try
            {
                GetResult<List<PermissionDTO>> permissionDTOs = CacheHelper.Get(CachedObjectsKey.LetterTypes, SessionInfo.CultureShortName) as GetResult<List<PermissionDTO>>;

                if (permissionDTOs == null)
                {
                    var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);
                    permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                    CacheHelper.Insert(CachedObjectsKey.ConfidentialityPermissions, permissionDTOs, SessionInfo.CultureShortName);
                }

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (permissionDTOs.Result != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());

                    List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
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

        public static string GetExplanationConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.ExplanationsConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (permissionDTOs.Result != null)
                {
                    List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
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
        public static List<PermissionVM> GetExplanationConfidentialityLevelList()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.ExplanationsConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;
                List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);


                return permissionVMs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetLetterTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<LetterTypeVM>> letterTypeVMs = LookupsHelper.GetLetterTypes(transactionCategory);

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

        public static string GetPrivecyLevels(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<SpecificLevelVM>> specificLevelVMs = LookupsHelper.GetSpecificLevels(transactionCategory);

                if (specificLevelVMs.Result != null)
                {
                    dataSource.Add(UIHelper.GetDefaultSelect());
                    foreach (SpecificLevelVM specificLevelVM in specificLevelVMs.Result)
                    {
                        switch (specificLevelVM.Id)
                        {
                            case (int)PrivacyOfTransactions.Private:
                                if (SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.Private"))
                                {
                                    dataSource.Add(new AutoCompleteDataSource()
                                    {
                                        Value = specificLevelVM.Id.ToString(),
                                        Label = specificLevelVM.LocalName
                                    });
                                }
                                break;
                            case (int)PrivacyOfTransactions.Limited:
                                if (SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.Limited"))
                                {
                                    dataSource.Add(new AutoCompleteDataSource()
                                    {
                                        Value = specificLevelVM.Id.ToString(),
                                        Label = specificLevelVM.LocalName
                                    });
                                }
                                break;
                            case (int)PrivacyOfTransactions.OpenByHand:
                                if (SessionInfo.CurrentUser.Claims.Contains("Privacy.Transactions.OpenByHand"))
                                {
                                    dataSource.Add(new AutoCompleteDataSource()
                                    {
                                        Value = specificLevelVM.Id.ToString(),
                                        Label = specificLevelVM.LocalName
                                    });
                                }
                                break;
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

        public static string GetNumberAcsDesc()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "0",
                    Label = DbRes.TResource("User.Transaction.Number.SortDesc"),
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "1",
                    Label = DbRes.TResource("User.Transaction.Number.SortAsc"),
                });
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetByDateAscDesc()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "1",
                    Label = DbRes.TResource("User.Transaction.Date.SortAsc"),
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "0",
                    Label = DbRes.TResource("User.Transaction.Date.SortDesc"),
                });
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetBySourceTypeAscDesc()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "1",
                    Label = DbRes.TResource("User.Transaction.SourceType.SortAsc"),
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "0",
                    Label = DbRes.TResource("User.Transaction.SourceType.SortDesc"),
                });
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetByPriorityAscDesc()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "1",
                    Label = DbRes.TResource("User.Transaction.Priority.SortAsc"),
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "0",
                    Label = DbRes.TResource("User.Transaction.Priority.SortDesc"),
                });
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetByConfidentialityAscDesc()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "1",
                    Label = DbRes.TResource("User.Transaction.Confidentiality.SortAsc"),
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "0",
                    Label = DbRes.TResource("User.Transaction.Confidentiality.SortDesc"),
                });
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string GetByExternalPartyAscDesc()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "1",
                    Label = DbRes.TResource("User.Transaction.ExternalParty.SortAsc"),
                });
                dataSource.Add(new AutoCompleteDataSource()
                {
                    Value = "0",
                    Label = DbRes.TResource("User.Transaction.ExternalParty.SortDesc"),
                });
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool CheckOrgUnitHasAssignmentPaper()
        {
            GetResult<bool> getResult =
               HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Transaction/CheckOrgUnitHasAssignmentPaper?orgUnitId={0}", SessionInfo.OrgUnitId)).Result;

            return getResult.Result;

        }

        public static bool CheckOrgUnitIsAllowedToCreateGroup()
        {
            GetResult<bool> getResult =
               HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Transaction/CheckOrgUnitIsAllowedToCreateGroup?orgUnitId={0}", SessionInfo.OrgUnitId)).Result;

            return getResult.Result;

        }

        public static DocumentVM GetMainDocument(int transactionId)
        {
            try
            {
                GetResult<DocumentDTO> documentDTO = HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetMainDocument?transactionId={0}", transactionId)).Result;

                return DocumentMapper.Map(documentDTO.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetOrgUnitForms()
        {
            GetResult<List<FormDTO>> formDocumentDTOs =
               HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

            if (formDocumentDTOs.Result != null)
            {
                List<FormVM> formVMs = FormMapper.Map(formDocumentDTOs.Result);
                foreach (FormVM formDocumentVM in formVMs)
                {
                    formDocumentDataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = formDocumentVM.Id.ToString(),
                        Label = formDocumentVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(formDocumentDataSource);
        }

        public static List<ExplanationVM> GetTransactionExplanations_New(int transactionId)
        {
            try
            {
                GetResult<List<ExplanationDTO>> explanationDTOs = HttpClientWrapper<GetResult<List<ExplanationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionExplanations_New?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                if (explanationDTOs.Result != null)
                {
                    explanationDTOs.Result.ForEach(e =>
                    {
                        e.CanBeDeleted = (e.CanBeDeleted || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ExpalanationsEditor.Delete) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ExpalanationsEditor.Edit)) && e.FromUserId == SessionInfo.CurrentUser.Id;
                    });
                }

                return ExplanationMapper.Map(explanationDTOs.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<ExplanationVM> GetTransactionExplanations(int transactionId)
        {
            try
            {
                GetResult<List<ExplanationDTO>> explanationDTOs = HttpClientWrapper<GetResult<List<ExplanationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionExplanations_New?transactionId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

                //if (explanationDTOs.Result != null)
                //{
                //    explanationDTOs.Result.ForEach(e =>
                //    {
                //        e.CanBeDeleted = (e.CanBeDeleted || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ExpalanationsEditor.Delete) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.ExpalanationsEditor.Edit)) && e.FromUserId == SessionInfo.CurrentUser.Id;
                //    });
                //}

                return ExplanationMapper.Map(explanationDTOs.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }





        public static string GetAssignmentGroups(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                    HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLetterTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;

                if (letterTypeDTOs.Result != null)
                {
                    List<LetterTypeVM> letterTypeVMs = LetterTypeMapper.Map(letterTypeDTOs.Result);
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

        public static string GetOrgUnitActions()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitActions?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

            if (actionDTOs.Result != null)
            {
                List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);
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

        public static string GetAllActions()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            if (actionDTOs.Result != null)
            {
                List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);
                foreach (ActionVM processVM in processVMs)
                {
                    AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                    {
                        Value = processVM.Id.ToString(),
                        Label = processVM.LocalName,
                        Parameters = new object[1]
                    };

                    autoCompleteDataSource.Parameters[0] = processVM.TypeId;

                    dataSource.Add(autoCompleteDataSource);
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }
        public static string GetAllActionsDDL()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            if (actionDTOs.Result != null)
            {
                List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);
                foreach (ActionVM processVM in processVMs)
                {
                    AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                    {
                        Value = processVM.Id.ToString(),
                        Label = processVM.LocalName,
                        Parameters = new object[1]
                    };

                    autoCompleteDataSource.Parameters[0] = processVM.TypeId;

                    dataSource.Add(autoCompleteDataSource);
                }
                dataSource = dataSource.Prepend(new AutoCompleteDataSource()
                {
                    Value = (-1).ToString(),
                    Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Editor.Select")
                }).ToList();
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        public static string GetUserAssignmentGroups()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            GetResult<List<AssignmentGroupDTO>> assignmentGroupDTOs =
                    HttpClientWrapper<GetResult<List<AssignmentGroupDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserAssignmentGroups?cultureName={0}&userId={1}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id)).Result;

            if (assignmentGroupDTOs.Result != null)
            {
                List<AssignmentGroupVM> assignmentGroupVMs = AssignmentGroupMapper.Map(assignmentGroupDTOs.Result);
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

        public static AssignmentPaperVM GetAssignmentPaperByOrgUnitId()
        {
            try
            {
                GetResult<AssignmentPaperDTO> assignmentPaperDTO = HttpClientWrapper<GetResult<AssignmentPaperDTO>>.GetItemRequest(string.Format("api/Transaction/GetAssignmentPaperByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                return AssignmentPaperMapper.Map(assignmentPaperDTO.Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string GetOrgUnitsManagers()
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitsManagers?cultureName={0}", SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (userProfileDTOs.Result != null)
                {
                    List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                    foreach (UserProfileVM userProfileVM in userProfileVMs)
                    {
                        if (userProfileVM != null)
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
        public static TreeViewModel BulidSubjectClassificationsTree(List<SubjectClassificationVM> subjectClassificationVMs)
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

        private static TreeNode AddSubjectClassificationsChilds(List<SubjectClassificationVM> subjectClassificationVMs, SubjectClassificationVM subjectClassificationVM)
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

        public static TreeViewModel BulidSuggestedTopicsTree(List<SuggestedTopicVM> suggestedTopicVMs)
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

        private static TreeNode AddSubjectClassificationsChilds(List<SuggestedTopicVM> suggestedTopicVMs, SuggestedTopicVM suggestedTopicVM)
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

        public static string GetAllActionsVIP()
        {
            IList<string> VipActionsId = SystemConfigurations.VipAssignmentPaperActions.Split(',');
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            if (actionDTOs.Result != null)
            {
                List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);

                List<ActionVM> actionVMs = new List<ActionVM>();

                if (processVMs != null && processVMs.Count > 0)
                {
                    actionVMs = processVMs.Where(a => VipActionsId.Contains(a.Id.ToString())).ToList();
                    //actionVMs = processVMs.Where(a => a.SortNo.HasValue).OrderBy(a => a.SortNo).ToList(); 
                    // actionVMs.AddRange(processVMs.Where(a => !a.SortNo.HasValue).ToList());
                }


                foreach (ActionVM processVM in actionVMs)
                {
                    AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                    {
                        Value = processVM.Id.ToString(),
                        Label = processVM.LocalName,
                        Parameters = new object[1]
                    };

                    autoCompleteDataSource.Parameters[0] = processVM.TypeId;

                    dataSource.Add(autoCompleteDataSource);
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        public ActionResult HideTransactionAssignment(int id)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/HideTransactionAssignment?assignmentId={0}", id), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    //return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetClassifications()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<ClassificationDto>> classificationDtos = LookupsHelper.GetClassificationTypes();

                if (classificationDtos.Result != null)
                {
                    foreach (ClassificationDto classificationDto in classificationDtos.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = classificationDto.Id.ToString(),
                            Label = classificationDto.Name,
                 
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


    }
}