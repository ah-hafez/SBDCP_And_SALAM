using Newtonsoft.Json;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.External;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Notifications;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Common;
using MCS.UI.Helpers;
using DotnetDaddy.DocumentConfig;
using MCS.UI.Areas.User.Mappers.UserPreferences;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using System.Configuration;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.Word;
using Microsoft.Ajax.Utilities;
using MobileApi.Domain;
using TransactionCategory = MCS.Common.TransactionCategory;
using Microsoft.AspNet.SignalR.Hubs;
using ZXing;
using MCS.Domain;
using Newtonsoft.Json.Linq;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Mappers.Action;
using MCS.UI.Areas.User.Mappers.Transaction.Inbound;

namespace MCS.UI.Areas.User
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    //[CustomAuthorizationAttribute(UserClaims.Outbound.DisplayOutbound)]
    public class OutboundExternalController : TransactionController
    {
        string yesserData = "";
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateExternalOutbound, UserClaims.Outbound.CreateOutboundDraft, UserClaims.Outbound.CreateDecisionDraft)]
        [CustomAction]
        public ActionResult Add(string baseTransactionId, bool isOutboundInternal = false, bool isDraft = false, bool isInternal = false, bool IsPresentationDraft = false, bool IsDecisionDraft = false, bool IsMultiExternal = false)
        {
            try
            {
                string trxId = !string.IsNullOrWhiteSpace(baseTransactionId) ? StringCipher.DecryptStringAES(baseTransactionId.Replace(" ", "+")) : "";

                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (IsDecisionDraft && !SessionInfo.CurrentUser.Claims.Contains(UserClaims.Outbound.CreateDecisionDraft))
                {
                    throw new UnauthorizedAccessException();
                }
                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                GetResult<List<FormDTO>> formDocumentDTOs =
                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

                if (formDocumentDTOs.Result != null)
                {
                    foreach (FormVM formVM in FormMapper.Map(formDocumentDTOs.Result))
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formVM.Id.ToString(),
                            Label = formVM.LocalName
                        });
                    }
                }

                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);

                AddOutboundExternalVM outboundExternalAddVM = new AddOutboundExternalVM();

                outboundExternalAddVM.EditorTypeId = (int)EditorType.TextEditor;
                outboundExternalAddVM.DocumentVM = new DocumentVM();
                Initialize(outboundExternalAddVM.Type);
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                var externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                ViewData["ExternalPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;
                if (trxId != null && trxId != string.Empty)
                {
                    TempData["IsOutboundInternal"] = isOutboundInternal;
                    var transactionBasicInfoResult = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>.GetItemRequest($"api/Transaction/GetTransactionBasicInfo?transactionId={trxId}&cultureName={SessionInfo.CultureShortName}").Result;
                    var yearLookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName);
                    int yearId = yearLookups.Result.Where(y => y.Text == transactionBasicInfoResult.Result.YearH.ToString()).FirstOrDefault().Id;
                    //var transaction = HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionIdByLinkType0?sourceNumber={0}&yearId={1}&linkTypeId={2}&cultureName={3}", transactionBasicInfoResult.Result.Number.ToString(),  yearId, LinkingType.WithReferenceInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName), SessionInfo.CultureShortName.ToString())).Result;
                    var transaction = HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;

                    string linkTypeDesc = "اجابة";
                    //GetResult<List<LinkDTO>> linkDTOs = HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLinkTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", TransactionCategory.Inbound)).Result;

                    //linkTypeDesc = linkDTOs.Result.Where(x => x.Id == (int)LinkType.ByInboundNumber).FirstOrDefault().LocalName;
                    List<TransactionLinkVM> linkBaseInbound = new List<TransactionLinkVM>
                    {
                    };
                    foreach (TransactionLinkDTO links in transaction.Result.Links)
                    {
                        linkBaseInbound = new List<TransactionLinkVM>
                    {
                        new TransactionLinkVM()
                        {
                            DateH = links.DateH,
                            Id = 0,
                            LinkTypeId = (int)LinkType.ByInboundNumber,
                            LinkTypeName = linkTypeDesc,
                            Subject = links.Subject,
                            TransactionCategory = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                            TransactionCategoryName = LookupsHelper.GetLookupItem(TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text,
                            TransactionId = transaction.Result.Id,
                            TransactionNumber =links.TransactionNumber,
                            YearDesc = links.Year,
                            TransactionType =links.TransactionType,
                            Year =links.Year,
                        }
                    };
                    }

                    GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest(
                    string.Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}", trxId)).Result;



                    var transactionLinkVM = new TransactionLinkVM
                    {
                        Key = transaction.Result.Links.Count + 1,
                        DateH = transactionDetailsDTOResult.Result.HijriDate,
                        Date = transactionDetailsDTOResult.Result.Date.ToShortDateString(),
                        TransactionType = transactionDetailsDTOResult.Result.TransactionsTypes,
                        TransactionId = transactionDetailsDTOResult.Result.Id,
                        Subject = transactionDetailsDTOResult.Result.Subject,
                        YearDesc = transactionDetailsDTOResult.Result.Year,
                        TransactionNumber = transactionDetailsDTOResult.Result.Number.ToString(),
                        LinkTypeId = 1,
                        LinkTypeName = "إشارة",
                        OrgUnitId = SessionInfo.OrgUnitId,
                        TypeId = 1
                    };

                    if (isOutboundInternal)
                    {
                        transactionLinkVM.TransactionCategory = 256;
                        transactionLinkVM.TransactionCategoryName = "معاملة داخلية";
                    }
                    else
                    {
                        transactionLinkVM.TransactionCategory = 254;
                        transactionLinkVM.TransactionCategoryName = "وارد خارجي";
                    }



                    linkBaseInbound.Add(transactionLinkVM);

                    outboundExternalAddVM.Links = linkBaseInbound;
                    outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft = true;
                    outboundExternalAddVM.OutboundExternalBasicInfo.DestinationId = transactionBasicInfoResult.Result.ExternalPartyId;
                    outboundExternalAddVM.OutboundExternalBasicInfo.Subject = transactionBasicInfoResult.Result.Subject;
                    outboundExternalAddVM.OutboundExternalBasicInfo.PriorityLevelId = transactionBasicInfoResult.Result.PriorityId;
                    outboundExternalAddVM.OutboundExternalBasicInfo.ConfidentialityLevelId = transactionBasicInfoResult.Result.ConfidentialityId;
                    outboundExternalAddVM.OutboundExternalBasicInfo.BaseTransactionNumber = transactionBasicInfoResult.Result.Number;

                    ViewData["DraftId"] = trxId;
                }
                else
                {
                    outboundExternalAddVM.Links = new List<TransactionLinkVM>();
                }

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.EditorType = EditorType.TextEditor;
                editorViewModel.Content = string.Empty;
                editorViewModel.ReadOnly = false;
                editorViewModel.IsShowWordAddIn = true;
                editorViewModel.IsScanning = false;
                ViewData["EditorViewModel"] = editorViewModel;
                //ViewData["OrgUnitsManagers"] = GetAllUsers();
                ViewData["DeliveryMethod"] = GetDeliveryMethod(true);

                List<ExternalPartyDTO> parties =
            HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result.Result;

                IList<AutoCompleteDataSource> partiesdataSource = new List<AutoCompleteDataSource>();
                if (parties != null)
                {
                    partiesdataSource.Add(UIHelper.GetDefaultSelect());

                    foreach (ExternalPartyDTO item in parties)
                    {
                        partiesdataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.LocalName
                        });
                    }
                }
                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);

                outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft = outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft || isDraft;
                outboundExternalAddVM.OutboundExternalBasicInfo.IsPresentationDraft = outboundExternalAddVM.OutboundExternalBasicInfo.IsPresentationDraft || IsPresentationDraft;
                outboundExternalAddVM.OutboundExternalBasicInfo.isOutboundInternalDraft = isInternal;
                outboundExternalAddVM.OutboundExternalBasicInfo.ParentTransactionId = !string.IsNullOrWhiteSpace(trxId) ? int.Parse(trxId) : (int?)null;
                outboundExternalAddVM.OutboundExternalBasicInfo.IsDecisionDraft = IsDecisionDraft;
                outboundExternalAddVM.OutboundExternalBasicInfo.IsMultiExternal = IsMultiExternal;
                return View(outboundExternalAddVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateExternalOutbound)]
        public ActionResult AddPrevious()
        {
            try
            {
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();

                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                Initialize(TransactionCategory.ExternalOutbound);

                //ViewData["OrgUnitsManagers"] = GetAllUsers();


                string message = string.Empty;

                GetResult<AddOutboundExternalDTO> outboundExternalAddDTO =
                  HttpClientWrapper<GetResult<AddOutboundExternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}", SessionInfo.CultureShortName, TransactionCategory.ExternalOutbound, SessionInfo.OrgUnitId)).Result;

                if (outboundExternalAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternalAddDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundExternalAddDTO.Result == null)
                {

                    message = DbRes.TResource("User.OutboundExternal.NoPreviousDataInfoMsg");

                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.PreparationEntityId);

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                   HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                // ViewData["OrgUnitsManagers"] = GetAllUsers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //  HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (subjectClassificationDTOs.Result != null && outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SubjectClassifications != null)
                //{
                //    outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                ViewData["DeliveryMethod"] = GetDeliveryMethod(true);

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.EditorType = EditorType.Scanning;
                editorViewModel.Content = string.Empty;
                editorViewModel.ReadOnly = false;
                ViewData["EditorViewModel"] = editorViewModel;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

                ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundExternalBasicInfo";

                return View("~/Areas/User/Views/OutboundExternal/Add.cshtml", OutboundExternalMapper.Map(outboundExternalAddDTO.Result));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateOutboundDraft)]
        public ActionResult AddPreviousDraft()
        {
            try
            {
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();

                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                Initialize(TransactionCategory.DraftOutbound);

                //ViewData["OrgUnitsManagers"] = GetAllUsers();


                string message = string.Empty;

                GetResult<AddOutboundExternalDTO> outboundExternalAddDTO =
                  HttpClientWrapper<GetResult<AddOutboundExternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}", SessionInfo.CultureShortName, TransactionCategory.DraftOutbound, SessionInfo.OrgUnitId)).Result;

                if (outboundExternalAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternalAddDTO.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundExternalAddDTO.Result == null)
                {
                    message = DbRes.TResource("User.OutboundExternal.NoPreviousDataInfoMsg");
                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.PreparationEntityId);

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                   HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                //ViewData["OrgUnitsManagers"] = GetAllUsers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //  HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (subjectClassificationDTOs.Result != null && outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SubjectClassifications != null)
                //{
                //    outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}
                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));



                outboundExternalAddDTO.Result.OutboundExternalBasicInfo.IsDraft = true;


                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.EditorType = EditorType.Scanning;
                editorViewModel.Content = string.Empty;
                editorViewModel.ReadOnly = false;
                ViewData["EditorViewModel"] = editorViewModel;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

                ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundExternalBasicInfo";

                return View("~/Areas/User/Views/OutboundExternal/Add.cshtml", OutboundExternalMapper.Map(outboundExternalAddDTO.Result));

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult NewOutbound(string transactionId)
        {
            try
            {
                ViewData["DraftId"] = transactionId;
                TempData["ControllerName"] = "OutboundExternal";
                AddOutboundExternalVM outboundExternalAddVM = new AddOutboundExternalVM();
                InitializeOutboundExternal();
                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();
                IAjaxGrid grid = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true);
                ViewData["CopiesData"] = grid;
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(new List<TransactionCopyVM>());
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(new List<TransactionExternalCopyVM>());
                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(new List<AutoCompleteDataSource>());

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/_OutboundExternalPartial.cshtml", outboundExternalAddVM) }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet]
        [CustomAction]
        public ActionResult CreateOutbound(string transactionId, int trayId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                var outboundExternalAddDTO = HttpClientWrapper<GetResult<AddOutboundExternalDTO>>
                    .GetItemRequest(string.Format("api/Transaction/PrepareOutboundCreation?cultureName={0}&orgUnitId={1}&transactionId={2}&trayId={3}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId, transactionId, trayId)).Result;

                AddOutboundExternalVM addOutboundExternalVM = OutboundExternalMapper.Map(outboundExternalAddDTO.Result);
                ViewData["DraftId"] = transactionId;

                TempData["ControllerName"] = "OutboundExternal";
                ViewData["DeliveryMethod"] = GetDelivery(true);
                addOutboundExternalVM.OutboundExternalBasicInfo.IsFromDraft = Convert.ToInt32(transactionId);

                if (outboundExternalAddDTO.StatusCode != StatusCode.Ok || addOutboundExternalVM == null)
                {
                    return NewOutbound(transactionId);
                }
                else
                {
                    InitializeOutboundExternal();
                    if (addOutboundExternalVM.Attachments == null)
                    {
                        addOutboundExternalVM.Attachments = new List<TransactionAttachmentVM>();
                    }
                    List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();

                    if (addOutboundExternalVM.DocumentVM != null)
                    {
                        string documentId = Guid.NewGuid().ToString();

                        Dictionary<string, byte[]> documentData = new Dictionary<string, byte[]>();
                        TextEditorViewModel editorViewModel = new TextEditorViewModel();
                        editorViewModel.ReadOnly = false;
                        ViewData["hdnDocumentId"] = addOutboundExternalVM.DocumentVM.Id;
                        if (addOutboundExternalVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                        {
                            addOutboundExternalVM.EditorTypeId = (int)EditorType.Scanning;
                        }
                        else
                        {
                            addOutboundExternalVM.EditorTypeId = (int)EditorType.TextEditor;
                        }
                        if (addOutboundExternalVM.EditorTypeId.HasValue && (EditorType)addOutboundExternalVM.EditorTypeId.Value == EditorType.TextEditor)
                        {
                            editorViewModel.EditorType = EditorType.TextEditor;
                            editorViewModel.Content = addOutboundExternalVM.DocumentVM != null && addOutboundExternalVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(addOutboundExternalVM.DocumentVM.Content) : null;
                            editorViewModel.IsSigned = addOutboundExternalVM.IsSigned;
                        }
                        else
                        {
                            if (addOutboundExternalVM.DocumentVM.Content != null &&
                             addOutboundExternalVM.DocumentVM.Content.Length > 0)
                            {
                                documentData.Add(documentId, addOutboundExternalVM.DocumentVM.Content);
                                Session["DocumentData"] = documentData;
                                Session["DocoNutDocument"] = addOutboundExternalVM.DocumentVM.Content;
                            }
                            editorViewModel.EditorType = EditorType.Scanning; editorViewModel.EditorType = EditorType.Scanning;
                        }


                        editorViewModel.IsSigned = addOutboundExternalVM.IsSigned;
                        string sessionKey = Guid.NewGuid().ToString();
                        ViewData[sessionKey] = sessionKey;
                        ViewData["EditorViewModel"] = editorViewModel;
                        ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                        ViewData["DistributionLists"] = GetDistributionLists();
                        transactionArchiveVMs.Add(new TransactionArchiveVM
                        {
                            Id = documentId,
                            EncryptDocumentId = AESEncrytDecry.Base64Encode(addOutboundExternalVM.DocumentVM.Id.ToString()),
                            IsMainDocument = true,
                            DocumentId = addOutboundExternalVM.DocumentVM.Id,
                            AttachmentTypeId = -1,
                            ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                            SessionInfo.CultureShortName).Result.Text
                        });
                    }

                    if (addOutboundExternalVM.Attachments != null)
                    {
                        //var dataSource = new List<AutoCompleteDataSource>();
                        foreach (TransactionAttachmentVM transactionAttachmentVM in addOutboundExternalVM.Attachments)
                        {
                            if (transactionAttachmentVM.DocumentVM != null && transactionAttachmentVM.DocumentVM.Size > 0)
                            {
                                transactionArchiveVMs.Add(new TransactionArchiveVM
                                {
                                    EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                                    Id = Guid.NewGuid().ToString(),
                                    DocumentId = transactionAttachmentVM.DocumentVM.Id,
                                    AttachmentTypeId = transactionAttachmentVM.TypeId,
                                    ArcivingTypeName = transactionAttachmentVM.TypeName,
                                    IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted,
                                    IsNew = true
                                });
                            }
                            //if (transactionAttachmentVM.Archivable)
                            //{
                            //    dataSource.Add(new AutoCompleteDataSource
                            //    {
                            //        Label = transactionAttachmentVM.TypeName,
                            //        Value = transactionAttachmentVM.TypeId.ToString(),
                            //        Parameters = new object[] { transactionAttachmentVM.Archivable }
                            //    });
                            //}
                        }
                        //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                    }


                    if (addOutboundExternalVM.Names == null)
                    {
                        addOutboundExternalVM.Names = new List<TransactionNameVM>();
                    }
                    if (addOutboundExternalVM.Links == null)
                    {
                        addOutboundExternalVM.Links = new List<TransactionLinkVM>();
                    }
                    if (addOutboundExternalVM.Copies == null)
                    {
                        addOutboundExternalVM.Copies = new List<TransactionCopyVM>();
                    }

                    //var externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                    //    .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                    //ViewData["ExternalCopiesPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;

                    ViewData["ExternalCopiesPartiesData"] = ViewData["ExternalPartiesData"];

                    ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(addOutboundExternalVM.OutboundExternalBasicInfo.DestinationId.Value);
                    //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(new List<TransactionExternalCopyVM>());

                    IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                    //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                    //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(addOutboundExternalVM.Attachments);
                    //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(addOutboundExternalVM.Names);
                    //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(addOutboundExternalVM.Links);
                    //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(addOutboundExternalVM.Copies);
                    //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(new List<TransactionExternalCopyVM>());
                    //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(TransactionArchiveMapper.Map(transactionArchiveVMs));
                    ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(TransactionArchiveMapper.Map(transactionArchiveVMs));
                    ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                    ViewData["AllExternalActionsData"] = ViewData["AllActionsData"];
                    ViewData["TransactionPaths"] = GetTransactionPaths();
                    //var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    //    .GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                    ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.PreparationEntityId);

                    //ViewData["OrgUnitsManagers"] = GetAllUsers(); //TransactionHelper.GetOrgUnitsManagers();
                    //List<ExternalPartyVM> externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                    //ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(externalPartyVMs, addOutboundExternalVM.OutboundExternalBasicInfo.DestinationId);

                    ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(addOutboundExternalVM.OutboundExternalBasicInfo.DestinationId.Value);
                    //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                    //                 HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                    //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                }
                ViewData["transactionId"] = addOutboundExternalVM.Id;

                return View("~/Areas/User/Views/File/_OutboundExternalPartial.cshtml", addOutboundExternalVM);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateExternalOutbound, UserClaims.Outbound.CreateOutboundDraft)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOutboundExternal(AddOutboundExternalVM outboundExternalAddVM, TextEditorViewModel editorViewModel, string hdnMainDocToken)
        {
            try
            {
                if (ModelState.IsValid)
                {

                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);

                }
                bool isYesserRej = false;
                string message = string.Empty;
                string messageForYesser = string.Empty;
                outboundExternalAddVM.OrgUnitId = SessionInfo.OrgUnitId;

                if ((!outboundExternalAddVM.OutboundExternalBasicInfo.DestinationId.HasValue &&
                    !outboundExternalAddVM.OutboundExternalBasicInfo.ExternalPartyId.HasValue) && (string.IsNullOrEmpty(outboundExternalAddVM.MultiExternalOutbound.ExternalOrgSelectedList)))
                {

                    message = DbRes.TValidation("User.OutBoundExternal.DestinationShouldBeFilled");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                }

                byte[] data = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);
                outboundExternalAddVM.DocumentVM = new DocumentVM();
                outboundExternalAddVM.OldDocumentVM = new DocumentVM();
                if (data != null)
                {
                    outboundExternalAddVM.DocumentVM.Content = data;
                    outboundExternalAddVM.DocumentVM.Size = data.Length;
                    outboundExternalAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundExternalAddVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundExternalAddVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                }


                if (!string.IsNullOrWhiteSpace(editorViewModel.DocumentBase64String))
                {

                    var oldData = Convert.FromBase64String(editorViewModel.DocumentBase64String);
                    outboundExternalAddVM.OldDocumentVM.Content = oldData;
                    outboundExternalAddVM.OldDocumentVM.Size = oldData.Length;
                    outboundExternalAddVM.OldDocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    outboundExternalAddVM.OldDocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundExternalAddVM.OldDocumentVM.FromUserId = SessionInfo.CurrentUser.Id;


                }
                else
                {

                    outboundExternalAddVM.OldDocumentVM.Content = data;
                    outboundExternalAddVM.OldDocumentVM.Size = data.Length;
                    outboundExternalAddVM.OldDocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundExternalAddVM.OldDocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundExternalAddVM.OldDocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                }


                var documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                outboundExternalAddVM.Attachments = new List<TransactionAttachmentVM>();
                outboundExternalAddVM.Attachments = FillTransactionAttachment(outboundExternalAddVM.Archives, documentData);//fill attachments

                var prefix = string.Empty;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    prefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
                }
                else
                {
                    prefix = "_" + SessionInfo.CurrentUser.Id + "_";
                }
                if (outboundExternalAddVM.ExternalCopies != null && outboundExternalAddVM.ExternalCopies.Any())
                {
                    foreach (TransactionExternalCopyVM transactionExternalCopy in outboundExternalAddVM.ExternalCopies)
                    {
                        string path = SystemConfigurations.ExternalCopiesAttachmentPath;
                        var filteredByFilename = Directory
                                              .GetFiles(path)
                                              .Select(o => Path.GetFileName(o))
                                              .Where(o => o.StartsWith(prefix + transactionExternalCopy.OrgUnitId.ToString() + "_"));
                        List<ExternalPartyAttachmentVM> externalPartyAttachmentVMs = new List<ExternalPartyAttachmentVM>();

                        foreach (var item in filteredByFilename)
                        {
                            byte[] fileContent = System.IO.File.ReadAllBytes(path + item);
                            string mimeType = MimeMapping.GetMimeMapping(path + item);
                            FileInfo f = new FileInfo(path + item);
                            long size = f.Length;
                            string name = item.Substring(item.LastIndexOf('_') + 1);


                            externalPartyAttachmentVMs.Add(new ExternalPartyAttachmentVM()
                            {
                                Name = name,
                                PartyId = transactionExternalCopy.OrgUnitId,
                                DocumentVM = new DocumentVM
                                {
                                    Name = name,
                                    MimeType = mimeType,
                                    Content = fileContent,
                                    Size = size,
                                    FromEntityId = SessionInfo.OrgUnitId,
                                    FromUserId = SessionInfo.CurrentUser.Id
                                }
                            });

                            f.Delete();
                        }
                        transactionExternalCopy.externalPartyAttachmentVMs = externalPartyAttachmentVMs;
                    }
                }
                DistributionListVM distributionList = new DistributionListVM();

                if (outboundExternalAddVM.OutboundExternalBasicInfo.DistrubutionListId != null)
                {

                    GetResult<DistributionListDTO> distributionListDTO =
               HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, outboundExternalAddVM.OutboundExternalBasicInfo.DistrubutionListId.Value)).Result;

                    distributionList = DistributionListMapper.Map(distributionListDTO.Result);

                }

                List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                PostObjectResult<TransactionDetailsDTO> postResult = null;
                AddOutboundExternalDTO addOutbound = OutboundExternalMapper.Map(outboundExternalAddVM);

                if (outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft || outboundExternalAddVM.OutboundExternalBasicInfo.IsPresentationDraft || outboundExternalAddVM.OutboundExternalBasicInfo.IsDecisionDraft)
                {
                    AddOutboundDraftDTO addOutboundDraftDTO = new AddOutboundDraftDTO();
                    addOutboundDraftDTO.Attachments = addOutbound.Attachments;
                    addOutboundDraftDTO.Copies = addOutbound.Copies;
                    addOutboundDraftDTO.ExternalCopies = addOutbound.ExternalCopies;
                    addOutboundDraftDTO.Names = addOutbound.Names;
                    addOutboundDraftDTO.Links = addOutbound.Links;
                    addOutboundDraftDTO.OrgUnitId = SessionInfo.OrgUnitId;
                    addOutboundDraftDTO.DocumentDTO = addOutbound.DocumentDTO;
                    addOutboundDraftDTO.OldDocumentDTO = addOutbound.OldDocumentDTO;
                    addOutboundDraftDTO.EditorType = addOutbound.EditorTypeId != null ? (EditorType)addOutbound.EditorTypeId : EditorType.TextEditor;
                    addOutboundDraftDTO.StatusId = TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    addOutboundDraftDTO.OutboundDraftBasicInfo.SubjectClassifications = addOutbound.OutboundExternalBasicInfo.SubjectClassifications;
                    addOutboundDraftDTO.OutboundDraftBasicInfo = new AddOutboundDraftBasicInfoDTO()
                    {
                        TransactionTypeId = addOutbound.OutboundExternalBasicInfo.TransactionTypeId,
                        ConfidentialityLevelId = addOutbound.OutboundExternalBasicInfo.ConfidentialityLevelId,
                        DestinationId = addOutbound.OutboundExternalBasicInfo.DestinationId,
                        ExternalPartyId = addOutbound.OutboundExternalBasicInfo.ExternalPartyId,
                        DirectedToId = addOutbound.OutboundExternalBasicInfo.DirectedToId,
                        DraftNumber = addOutbound.OutboundExternalBasicInfo.DirectedToId,
                        Hour = addOutbound.OutboundExternalBasicInfo.Hour,
                        Minute = addOutbound.OutboundExternalBasicInfo.Minute,
                        PriorityLevelId = addOutbound.OutboundExternalBasicInfo.PriorityLevelId,
                        RemindDate = addOutbound.OutboundExternalBasicInfo.RemindDate,
                        RemindDateH = addOutbound.OutboundExternalBasicInfo.RemindDateH,
                        SignedById = addOutbound.OutboundExternalBasicInfo.SignedById,
                        Subject = addOutbound.OutboundExternalBasicInfo.Subject,
                        SubjectClassifications = addOutbound.OutboundExternalBasicInfo.SubjectClassifications,
                        SuggestedTopicId = addOutbound.OutboundExternalBasicInfo.SuggestedTopicId,
                        LetterTypeId = addOutbound.OutboundExternalBasicInfo.LetterTypeId,
                        DeliveryMethodId = addOutbound.OutboundExternalBasicInfo.DeliveryMethodId,
                        IsDraft = addOutbound.OutboundExternalBasicInfo.IsDraft,
                        POBox = addOutbound.OutboundExternalBasicInfo.POBox,
                        PostCode = addOutbound.OutboundExternalBasicInfo.PostCode,
                        ReporterId = addOutbound.OutboundExternalBasicInfo.ReporterId,
                        TransactionPathId = addOutbound.OutboundExternalBasicInfo.TransactionPathId,
                        SubjectClassificationsId = addOutbound.OutboundExternalBasicInfo.SubjectClassificationsId,
                        PreparationEntityId = addOutbound.OutboundExternalBasicInfo.PreparationEntityId,
                        isOutboundInternalDraft = addOutbound.OutboundExternalBasicInfo.isOutboundInternalDraft,
                        LetterNumber = addOutbound.OutboundExternalBasicInfo.LetterNumber,
                        IsPresentationDraft = addOutbound.OutboundExternalBasicInfo.IsPresentationDraft,
                        PresentationDraftNumber = addOutbound.OutboundExternalBasicInfo.PresentationDraftNumber,
                        IsElcOutBound = addOutbound.OutboundExternalBasicInfo.IsElcOutBound,
                        NeedAcknowled = addOutbound.OutboundExternalBasicInfo.NeedAcknowled,
                        OutBoundDraftNumber = addOutbound.OutboundExternalBasicInfo.OutBoundDraftNumber,
                        IsDecisionDraft = addOutbound.OutboundExternalBasicInfo.IsDecisionDraft,
                        Summary = addOutbound.OutboundExternalBasicInfo.Summary,
                        Encrypted = addOutbound.OutboundExternalBasicInfo.Encrypted,



                    };

                    if (distributionList.DistributionListDetails != null)
                    {
                        foreach (var item in distributionList.DistributionListDetails)
                        {
                            if (!addOutboundDraftDTO.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                            {
                                if ((item.UserId != 0 && !addOutboundDraftDTO.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                                   (item.UserId == 0 && !addOutboundDraftDTO.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
                                {
                                    TransactionCopyVM copy = new TransactionCopyVM
                                    {
                                        ActionId = (int)CopiesActions.ToView,
                                        UserId = item.UserId,
                                        OrgUnitId = item.OrgUnitId
                                    };
                                    Copies.Add(copy);
                                }
                            }
                        }

                        addOutboundDraftDTO.Copies.AddRange(TransactionCopyMapper.Map(Copies));
                    }


                    postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutboundDraftDTO).Result;
                }
                else
                {
                    InitializeExternalParties();
                    addOutbound.StatusId = TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                    if (distributionList.DistributionListDetails != null)
                    {
                        foreach (var item in distributionList.DistributionListDetails)
                        {
                            if (!addOutbound.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                            {
                                if ((item.UserId != 0 && !addOutbound.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                                   (item.UserId == 0 && !addOutbound.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
                                {
                                    TransactionCopyVM copy = new TransactionCopyVM
                                    {
                                        ActionId = (int)CopiesActions.ToView,
                                        UserId = item.UserId,
                                        OrgUnitId = item.OrgUnitId
                                    };
                                    Copies.Add(copy);
                                }
                            }
                        }

                        addOutbound.Copies.AddRange(TransactionCopyMapper.Map(Copies));
                    }

                    if (outboundExternalAddVM.MultiExternalOutbound != null)
                    {
                        List<string> externalparties = new List<string>(outboundExternalAddVM.MultiExternalOutbound.ExternalOrgSelectedList.Split(','));
                        //int MainExternalPartyId = Convert.ToInt32(externalparties.FirstOrDefault());
                        //addOutbound.OutboundExternalBasicInfo.ExternalPartyId = Convert.ToInt32(MainExternalPartyId);
                        //externalparties.Remove(MainExternalPartyId.ToString());
                        //PostObjectResult<TransactionDetailsDTO> mainpostResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutbound).Result;

                        foreach (var externalpartie in externalparties)
                        {
                            addOutbound.OutboundExternalBasicInfo.ExternalPartyId = Convert.ToInt32(externalpartie);
                            postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutbound).Result;

                        }


                    }

                    else
                    {
                        postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutbound).Result;

                    }


                }


                if (outboundExternalAddVM.OutboundExternalBasicInfo.IsAcknowledged)
                {
                    PostResult postConfidentialityAcknowledgmentResult =
                        HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddConfidentialityAcknowledgment?TransactionId={0}&UserId={1}&OrgUnitId={2}", postResult.Result.Id, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId), null).Result;

                }


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundExternalAddVM.OutboundExternalBasicInfo.ParentTransactionId != null && outboundExternalAddVM.OutboundExternalBasicInfo.ParentTransactionId > 0)
                {
                    message = string.Empty;
                    string remarks = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.CreatedDraftReason");
                    PutResult putResult =
                        HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/MoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}",
                        outboundExternalAddVM.OutboundExternalBasicInfo.ParentTransactionId.Value, SessionInfo.OrgUnitId, (int)TrayActionType.Save, null, (int)TrayType.MyTransactions, remarks, SessionInfo.CurrentUser.Id), null).Result;

                    //if (putResult.StatusCode != StatusCode.Ok)
                    //{
                    //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    //}
                }
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                //if (outboundExternalAddVM.OutboundExternalBasicInfo.IsFromDraft != null && outboundExternalAddVM.OutboundExternalBasicInfo.IsFromDraft != 0)
                //{
                //    var putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/MoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}", outboundExternalAddVM.OutboundExternalBasicInfo.IsFromDraft, SessionInfo.OrgUnitId, (int)TrayActionType.DeleteDraft, null, (int)TrayType.DraftOutbound, null, SessionInfo.CurrentUser.Id), null).Result;
                //}

                if (addOutbound.Links != null && addOutbound.Links.Count > 0)
                {
                    foreach (TransactionLinkDTO link in addOutbound.Links)
                    {
                        GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                        .GetItemRequest(string.Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                        link.TransactionId,
                        SessionInfo.CultureShortName)).Result;
                        var User = transactionDetailsDTOResult.Result.ToUserId;
                        if (User == null)
                        {
                            User = SessionInfo.CurrentUser.Id;
                        }
                        message = string.Empty;
                        string remarks = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Link");
                        PutResult putResult =
                            HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/LinkedMoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}",
                            link.TransactionId, SessionInfo.OrgUnitId, (int)TrayActionType.Save, null, (int)TrayType.MyTransactions, remarks, User), null).Result;

                    }
                }
                var gregorianDate = postResult.Result.Date.ToString("MM/dd/yyyy");
                messageForYesser = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundExternal.AddSucceeded");
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundExternal.AddSucceededNotYesserReg");


                return Json(new
                {
                    MessageText = message,
                    MessageYesserText = messageForYesser,
                    MessageType = MessageType.Information,
                    OutboundExternalNumber = postResult.Result.Number,
                    postResult.Result.Id,
                    Date = postResult.Result.HijriDate,
                    UmmalquraDate = postResult.Result.HijriDate,
                    GregorianDate = gregorianDate,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt"),
                    EncryptedId = AESEncrytDecry.Base64Encode(postResult.Result.Id.ToString()),
                    EncryptedDraft = AESEncrytDecry.Base64Encode(outboundExternalAddVM.OutboundExternalBasicInfo.IsDraft.ToString()),
                    EncryptedPresentationDraft = AESEncrytDecry.Base64Encode(outboundExternalAddVM.OutboundExternalBasicInfo.IsPresentationDraft.ToString()),
                    EncryptedIsYesserRegisterd = AESEncrytDecry.Base64Encode(true.ToString()),
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound, UserClaims.Outbound.EditDraft)]
        [CustomAction]
        public ActionResult Edit(string id, bool IsFromDraft, bool isHubEditable)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();
                EditOutboundExternalDTO outboundExternalDTO = new EditOutboundExternalDTO();
                var outboundExternallEditDTO = HttpClientWrapper<GetResult<object>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (outboundExternallEditDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternallEditDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                string serilizeObject = JsonConvert.SerializeObject(outboundExternallEditDTO.Result);
                TransactionCategory transactionCategory = TransactionCategory.ExternalOutbound;
                SetTransactionAssignmentToViewed(trxId);
                if (IsFromDraft)
                {
                    var outboundDraftEditDTO = JsonConvert.DeserializeObject<EditOutboundDraftDTO>(serilizeObject);
                    //if (outboundDraftEditDTO.StatusCode == StatusCode.TransactionNotFound)
                    //{
                    //    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundDraftEditDTO.StatusCode.ToString());
                    //    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    //    return RedirectToAction("DashboardHome", "Shared");
                    //}
                    outboundExternalDTO = new EditOutboundExternalDTO()
                    {
                        Attachments = outboundDraftEditDTO.Attachments,
                        Copies = outboundDraftEditDTO.Copies,
                        DocumentDTO = outboundDraftEditDTO.DocumentDTO,
                        OldDocumentDTO = outboundDraftEditDTO.OldDocumentDTO,
                        ExternalCopies = outboundDraftEditDTO.ExternalCopies,
                        HijriRecordDate = outboundDraftEditDTO.HijriRecordDate,
                        Id = outboundDraftEditDTO.Id,
                        IsSigned = outboundDraftEditDTO.IsSigned,
                        Links = outboundDraftEditDTO.Links,
                        ModifiedByUserId = outboundDraftEditDTO.ModifiedByUserId,
                        Names = outboundDraftEditDTO.Names,
                        OrgUnitId = outboundDraftEditDTO.OrgUnitId,
                        StatusId = outboundDraftEditDTO.StatusId,
                        RecordDate = outboundDraftEditDTO.RecordDate,
                        FollowUp = outboundDraftEditDTO.FollowUps,
                        UserId = outboundDraftEditDTO.UserId,
                        FromUser = outboundDraftEditDTO.FromUser,
                        ToUser = outboundDraftEditDTO.ToUser,
                        SavedTransactionAssignment = outboundDraftEditDTO.SavedTransactionAssignment,
                        OutboundExternalBasicInfo = new EditOutboundExternalBasicInfoDTO()
                        {
                            ConfidentialityLevelId = outboundDraftEditDTO.OutboundDraftBasicInfo.ConfidentialityLevelId,
                            DeliveryMethodId = outboundDraftEditDTO.OutboundDraftBasicInfo.DeliveryMethodId,
                            DestinationId = outboundDraftEditDTO.OutboundDraftBasicInfo.DestinationId,
                            ExternalPartyId = outboundDraftEditDTO.OutboundDraftBasicInfo.ExternalPartyId.HasValue ? outboundDraftEditDTO.OutboundDraftBasicInfo.ExternalPartyId.Value : 0,
                            DirectedToId = outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId,
                            Hour = outboundDraftEditDTO.OutboundDraftBasicInfo.Hour,
                            IsDraft = true,
                            Minute = outboundDraftEditDTO.OutboundDraftBasicInfo.Minute,
                            OutboundNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.DraftNumber.HasValue ? outboundDraftEditDTO.OutboundDraftBasicInfo.DraftNumber.Value : 0,
                            PriorityLevelId = outboundDraftEditDTO.OutboundDraftBasicInfo.PriorityLevelId,
                            RemindDate = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDate,
                            RemindDateH = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDateH,
                            SignedById = outboundDraftEditDTO.OutboundDraftBasicInfo.SignedById,
                            TransactionTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionTypeId,
                            Subject = outboundDraftEditDTO.OutboundDraftBasicInfo.Subject,
                            SubjectClassifications = outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications,
                            SuggestedTopicId = outboundDraftEditDTO.OutboundDraftBasicInfo.SuggestedTopicId,
                            LetterTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterTypeId,
                            PostCode = outboundDraftEditDTO.OutboundDraftBasicInfo.PostCode,
                            POBox = outboundDraftEditDTO.OutboundDraftBasicInfo.POBox,
                            ReporterId = outboundDraftEditDTO.OutboundDraftBasicInfo.ReporterId,
                            TransactionPathId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionPathId,
                            PreparationEntityId = outboundDraftEditDTO.OutboundDraftBasicInfo.PreparationEntityId,
                            Remarks = outboundDraftEditDTO.OutboundDraftBasicInfo.Remarks,
                            LetterNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterNumber,
                            IsPresentationDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsPresentationDraft,
                            PresentationDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.PresentationDraftNumber,
                            IsElcOutBound = outboundDraftEditDTO.OutboundDraftBasicInfo.IsElcOutBound,
                            NeedAcknowled = outboundDraftEditDTO.OutboundDraftBasicInfo.NeedAcknowled,
                            OutBoundDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.OutBoundDraftNumber,
                            IsDecisionDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsDecisionDraft,
                            Summary = outboundDraftEditDTO.OutboundDraftBasicInfo.Summary,

                        }
                    };

                    // handling internal outbound draft
                    outboundExternalDTO.OutboundExternalBasicInfo.isOutboundInternalDraft = (outboundExternalDTO.OutboundExternalBasicInfo.ExternalPartyId == 0);

                    if (!outboundExternalDTO.OutboundExternalBasicInfo.isOutboundInternalDraft)
                    {
                        outboundExternalDTO.OutboundExternalBasicInfo.DestinationId = outboundExternalDTO.OutboundExternalBasicInfo.PreparationEntityId;
                    }

                    transactionCategory = TransactionCategory.DraftOutbound;
                }
                else
                {
                    outboundExternalDTO = JsonConvert.DeserializeObject<EditOutboundExternalDTO>(serilizeObject);
                }

                EditOutboundExternalVM outboundExternalEditVM = OutboundExternalMapper.Map(outboundExternalDTO);

                IList<LookupVM> yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (outboundExternalEditVM.Links != null && outboundExternalEditVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM item in outboundExternalEditVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                Initialize(TransactionCategory.ExternalOutbound);

                //ViewData["OrgUnitsManagers"] = GetAllUsers();


                var gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundExternalEditVM.Attachments, 1, 0, true);
                ViewData["AttachmentData"] = gridAttachment;

                var transactionArchiveVMs = new List<TransactionArchiveVM>();

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                if (IsFromDraft)
                {
                    editorViewModel.IsShowWordAddIn = true;
                }
                ViewData["EditorViewModel"] = editorViewModel;

                if (outboundExternalEditVM.DocumentVM != null)
                {
                    if (outboundExternalEditVM?.OldDocumentVM?.Content != null && outboundExternalEditVM?.OldDocumentVM.MimeType == System.Net.Mime.MediaTypeNames.Application.Octet)
                        editorViewModel.DocumentBase64String = Convert.ToBase64String(outboundExternalEditVM.OldDocumentVM.Content);
                    string documentId = Guid.NewGuid().ToString();
                    if (IsFromDraft)
                    {

                        outboundExternalEditVM.EditorType = EditorType.TextEditor;

                    }
                    else
                    {
                        outboundExternalEditVM.EditorType = EditorType.Scanning;

                    }

                    ViewData["hdnDocumentId"] = outboundExternalEditVM.DocumentVM.Id;
                    if (outboundExternalEditVM?.OldDocumentVM?.DocumentId != null)
                    {
                        editorViewModel.OldDocumentId = outboundExternalEditVM.OldDocumentVM.DocumentId;
                    }
                    if (outboundExternalEditVM.EditorType == EditorType.TextEditor)
                    {
                        if (!outboundExternalEditVM.IsSigned)
                        {
                            editorViewModel.ReadOnly = false;
                        }
                        else
                        {
                            editorViewModel.ReadOnly = true;
                        }



                        string signatureText = string.Format("{0} {1}",
                        SessionInfo.GetObjectFromSession("UserName"),
                        SessionInfo.GetObjectFromSession("UserEmailAddress"));

                        Image signature = ImageHelper.DrawText(signatureText, new Font("Arial", 50), System.Drawing.Color.Black, System.Drawing.Color.Transparent);
                        signature = ImageHelper.SetImageOpacity(signature, (float)0.1);
                        using (MemoryStream m = new MemoryStream())
                        {
                            signature.Save(m, ImageFormat.Png);
                            byte[] imageBytes = m.ToArray();
                            string base64String = Convert.ToBase64String(imageBytes);
                            Session["textEditorSignature"] = base64String;
                        }


                        editorViewModel.IsSigned = outboundExternalEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;
                        editorViewModel.IsSigned = outboundExternalEditVM.IsSigned;
                        string sessionKey = Guid.NewGuid().ToString();
                        ViewData[sessionKey] = sessionKey;

                    }
                    if (outboundExternalEditVM?.DocumentVM?.Content != null)
                        Session["DocoNutDocument"] = outboundExternalEditVM.DocumentVM.Content;

                    ViewData["EditorViewModel"] = editorViewModel;
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = outboundExternalEditVM.DocumentVM.Id,
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundExternalEditVM.DocumentVM.Id.ToString()),
                        AttachmentTypeId = -1,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                        SessionInfo.CultureShortName).Result.Text
                    });
                }

                if (outboundExternalEditVM.Attachments != null)
                {
                    foreach (TransactionAttachmentVM item in outboundExternalEditVM.Attachments)
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
                            Id = item.Id.ToString()
                        };

                        if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                        {

                            Archive.DocumentId = item.DocumentVM.Id;
                            Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                            Archive.IsDeleted = item.DocumentVM.IsDeleted;
                            Archive.AttachmentSource = item.AttachmentSource;
                            Archive.FileName = item.DocumentVM.Name;
                            Archive.FromUserId = item.DocumentVM.FromUserId;
                            Archive.FromEntityId = item.DocumentVM.FromEntityId;

                        }
                        transactionArchiveVMs.Add(Archive);
                    }
                }
                if (outboundExternalEditVM.Copies != null && outboundExternalEditVM.Copies.Count > 0)
                    outboundExternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                outboundExternalEditVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();
                ViewData["TransactionCategory"] = (int)transactionCategory;
                ViewData["TransactionId"] = outboundExternalEditVM.Id;

                IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                // ViewData["ExternalCopiesPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId) : null;
                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                //List<ExternalPartyVM> externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                //ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(externalPartyVMs, outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Links);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                // ViewData["AllExternalActionsData"] = ViewData["AllActionsData"];
                GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                //ViewData["OrgUnitsManagers"] = GetAllUsers();
                ViewData["InternalCopiesData"] =
                outboundExternalEditVM?.Copies != null ? outboundExternalEditVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    outboundExternalEditVM?.ExternalCopies != null ? outboundExternalEditVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();

                InitializerAssignmentPaperData(outboundExternalEditVM.Id);

                GetResult<TransactionPathDetailsDTO> TransactionPathDetails = HttpClientWrapper<GetResult<TransactionPathDetailsDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionPathNextStep?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                if (TransactionPathDetails.Result != null && TransactionPathDetails.Result.Id > 0)
                {
                    TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM()
                    {
                        ActionId = TransactionPathDetails.Result.ActionId,
                        ToOrgUnitId = TransactionPathDetails.Result.OrgUnitId,
                        ToUserId = TransactionPathDetails.Result.UserId,
                        ActionName = TransactionPathDetails.Result.ActionName,
                        ToOrgUnitName = TransactionPathDetails.Result.OrgUnitName,
                        ToUserName = TransactionPathDetails.Result.UserName,
                    };

                    outboundExternalEditVM.TransactionAssignmentVM = transactionAssignmentVM;

                }

                if (outboundExternalEditVM.OutboundExternalBasicInfo.TransactionPathId.HasValue)
                {
                    if (outboundExternalEditVM.UserId != SessionInfo.CurrentUser.Id)
                    {
                        ViewData["TransactionPaths"] = GetTransactionPaths(outboundExternalEditVM.OutboundExternalBasicInfo.TransactionPathId);
                    }
                }
                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();

                IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(outboundExternalEditVM.SavedTransactionAssignment) ?
    JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(outboundExternalEditVM.SavedTransactionAssignment) : transactionAssignmentVMs;


                //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

                string editorMainDocumentSessionKey = Guid.NewGuid().ToString();
                // ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
                ViewData["EditorMainDocumentSessionKey"] = editorMainDocumentSessionKey;
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();

                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = outboundExternalDTO.FromUser.LocalName == outboundExternalDTO.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;



                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(a);
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                Session["DocoNutexplanations"] = null;
                Session["BarcodeImgByte"] = GetBarcodeImage(outboundExternalEditVM.Id, true);
                ViewData["ArchiveListData"] =
                    outboundExternalEditVM?.Archives != null ? outboundExternalEditVM.Archives.ToList() : new List<TransactionArchiveVM>();
                LogTransactionAction(AuditingActionCode.UpadteTransaction, outboundExternalEditVM.Id);
                ViewData["WithBarcode"] = true;
                if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && (transactionCategory == TransactionCategory.DraftOutbound))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }
                if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == (int)DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && (transactionCategory == TransactionCategory.DraftOutbound))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(true);
                }
                if (transactionCategory == TransactionCategory.ExternalOutbound)
                {

                    if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(false);
                    }
                    else if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) ||
                        outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    }
                    else
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    }
                }
                #region Add value to key Field

                if (outboundExternalEditVM.Attachments != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Attachments.Count; i++)
                    {
                        outboundExternalEditVM.Attachments[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Archives != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Archives.Count; i++)
                    {
                        outboundExternalEditVM.Archives[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Copies != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Copies.Count; i++)
                    {
                        outboundExternalEditVM.Copies[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Names != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Names.Count; i++)
                    {
                        outboundExternalEditVM.Names[i].Key = i + 1;
                    }
                }
                if (outboundExternalEditVM.Links != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Links.Count; i++)
                    {
                        outboundExternalEditVM.Links[i].Key = i + 1;
                    }
                }
                #endregion


                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);
                RemoveAllAttachemntsPhysically();
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/OutboundExternal/Edit.cshtml", outboundExternalEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound, UserClaims.Outbound.EditDraft)]
        [CustomAction]
        public ActionResult Editor(string id, bool IsFromDraft, bool isHubEditable)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();
                // ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();
                var outboundExternallEditDTO = HttpClientWrapper<GetResult<object>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (outboundExternallEditDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternallEditDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                EditOutboundExternalDTO outboundExternalDTO = new EditOutboundExternalDTO();
                string serilizeObject = JsonConvert.SerializeObject(outboundExternallEditDTO.Result);
                TransactionCategory transactionCategory = TransactionCategory.ExternalOutbound;
                SetTransactionAssignmentToViewed(trxId);
                if (IsFromDraft)
                {
                    var outboundDraftEditDTO = JsonConvert.DeserializeObject<EditOutboundDraftDTO>(serilizeObject);

                    outboundExternalDTO = new EditOutboundExternalDTO()
                    {
                        Attachments = outboundDraftEditDTO.Attachments,
                        Copies = outboundDraftEditDTO.Copies,
                        DocumentDTO = outboundDraftEditDTO.DocumentDTO,
                        ExternalCopies = outboundDraftEditDTO.ExternalCopies,
                        HijriRecordDate = outboundDraftEditDTO.HijriRecordDate,
                        Id = outboundDraftEditDTO.Id,
                        IsSigned = outboundDraftEditDTO.IsSigned,
                        Links = outboundDraftEditDTO.Links,
                        ModifiedByUserId = outboundDraftEditDTO.ModifiedByUserId,
                        Names = outboundDraftEditDTO.Names,
                        OrgUnitId = outboundDraftEditDTO.OrgUnitId,
                        StatusId = outboundDraftEditDTO.StatusId,
                        RecordDate = outboundDraftEditDTO.RecordDate,
                        FollowUp = outboundDraftEditDTO.FollowUps,
                        UserId = outboundDraftEditDTO.UserId,
                        FromUser = outboundDraftEditDTO.FromUser,
                        ToUser = outboundDraftEditDTO.ToUser,
                        SavedTransactionAssignment = outboundDraftEditDTO.SavedTransactionAssignment,
                        OutboundExternalBasicInfo = new EditOutboundExternalBasicInfoDTO()
                        {
                            ConfidentialityLevelId = outboundDraftEditDTO.OutboundDraftBasicInfo.ConfidentialityLevelId,
                            DeliveryMethodId = outboundDraftEditDTO.OutboundDraftBasicInfo.DeliveryMethodId,
                            DestinationId = outboundDraftEditDTO.OutboundDraftBasicInfo.DestinationId,
                            DirectedToId = outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId,
                            Hour = outboundDraftEditDTO.OutboundDraftBasicInfo.Hour,
                            IsDraft = true,
                            Minute = outboundDraftEditDTO.OutboundDraftBasicInfo.Minute,
                            OutboundNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.DraftNumber.HasValue ? outboundDraftEditDTO.OutboundDraftBasicInfo.DraftNumber.Value : 0,
                            PriorityLevelId = outboundDraftEditDTO.OutboundDraftBasicInfo.PriorityLevelId,
                            RemindDate = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDate,
                            RemindDateH = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDateH,
                            SignedById = outboundDraftEditDTO.OutboundDraftBasicInfo.SignedById,
                            TransactionTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionTypeId,
                            Subject = outboundDraftEditDTO.OutboundDraftBasicInfo.Subject,
                            SubjectClassifications = outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications,
                            SuggestedTopicId = outboundDraftEditDTO.OutboundDraftBasicInfo.SuggestedTopicId,
                            LetterTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterTypeId,
                            PostCode = outboundDraftEditDTO.OutboundDraftBasicInfo.PostCode,
                            POBox = outboundDraftEditDTO.OutboundDraftBasicInfo.POBox,
                            ReporterId = outboundDraftEditDTO.OutboundDraftBasicInfo.ReporterId,
                            TransactionPathId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionPathId,
                            LetterNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterNumber,
                            IsPresentationDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsPresentationDraft,
                            PresentationDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.PresentationDraftNumber,
                            IsElcOutBound = outboundDraftEditDTO.OutboundDraftBasicInfo.IsElcOutBound,
                            NeedAcknowled = outboundDraftEditDTO.OutboundDraftBasicInfo.NeedAcknowled,
                            OutBoundDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.OutBoundDraftNumber
                        }
                    };
                    transactionCategory = TransactionCategory.DraftOutbound;

                }
                else
                {

                    outboundExternalDTO = JsonConvert.DeserializeObject<EditOutboundExternalDTO>(serilizeObject);
                }

                EditOutboundExternalVM outboundExternalEditVM = OutboundExternalMapper.Map(outboundExternalDTO);
                // outboundExternalEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundExternalEditVM.HijriRecordDate);

                IList<LookupVM> yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (outboundExternalEditVM.Links != null && outboundExternalEditVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM item in outboundExternalEditVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                Initialize(TransactionCategory.ExternalOutbound);

                //ViewData["OrgUnitsManagers"] = GetAllUsers();


                var gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundExternalEditVM.Attachments, 1, 0, true);
                ViewData["AttachmentData"] = gridAttachment;

                var transactionArchiveVMs = new List<TransactionArchiveVM>();

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.ReadOnly = true;
                ViewData["EditorViewModel"] = editorViewModel;
                if (outboundExternalEditVM.DocumentVM != null)
                {
                    if (outboundExternalEditVM?.DocumentVM?.Content != null)
                        editorViewModel.DocumentBase64String = Convert.ToBase64String(outboundExternalEditVM.OldDocumentVM.Content);


                    string documentId = Guid.NewGuid().ToString();
                    outboundExternalEditVM.EditorType = EditorType.Scanning;

                    ViewData["hdnDocumentId"] = outboundExternalEditVM.DocumentVM.Id;
                    if (outboundExternalEditVM?.OldDocumentVM?.Id != null)
                        editorViewModel.OldDocumentId = outboundExternalEditVM.OldDocumentVM.Id;

                    editorViewModel.IsSigned = outboundExternalEditVM.IsSigned;

                    string sessionKey = Guid.NewGuid().ToString();

                    ViewData[sessionKey] = sessionKey;
                    Session["DocoNutDocument"] = outboundExternalEditVM.DocumentVM.Content;

                    ViewData["EditorViewModel"] = editorViewModel;
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = outboundExternalEditVM.DocumentVM.Id,
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundExternalEditVM.DocumentVM.Id.ToString()),
                        AttachmentTypeId = -1,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                        SessionInfo.CultureShortName).Result.Text
                    });
                }

                if (outboundExternalEditVM.Attachments != null)
                {
                    foreach (TransactionAttachmentVM item in outboundExternalEditVM.Attachments)
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
                            Archive.Id = item.Id.ToString();//Guid.NewGuid().ToString();
                            Archive.DocumentId = item.DocumentVM.Id;
                            Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                            Archive.AttachmentTypeId = item.TypeId;
                            Archive.ArcivingTypeName = item.TypeName;
                            Archive.IsNew = true;
                            Archive.IsDeleted = item.DocumentVM.IsDeleted;
                            Archive.AttachmentSource = item.AttachmentSource;
                            Archive.FileName = item.DocumentVM.Name;
                            Archive.FromUserId = item.DocumentVM.FromUserId;
                            Archive.FromEntityId = item.DocumentVM.FromEntityId;

                        }
                        transactionArchiveVMs.Add(Archive);
                    }
                }

                outboundExternalEditVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();
                ViewData["TransactionCategory"] = (int)transactionCategory;
                ViewData["TransactionId"] = outboundExternalEditVM.Id;

                IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;



                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Links);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);

                GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);


                var subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;



                GetResult<TransactionPathDetailsDTO> TransactionPathDetails = HttpClientWrapper<GetResult<TransactionPathDetailsDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionPathNextStep?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                if (TransactionPathDetails.Result != null && TransactionPathDetails.Result.Id > 0)
                {
                    TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM()
                    {
                        ActionId = TransactionPathDetails.Result.ActionId,
                        ToOrgUnitId = TransactionPathDetails.Result.OrgUnitId,
                        ToUserId = TransactionPathDetails.Result.UserId,
                        ActionName = TransactionPathDetails.Result.ActionName,
                        ToOrgUnitName = TransactionPathDetails.Result.OrgUnitName,
                        ToUserName = TransactionPathDetails.Result.UserName,
                    };

                    outboundExternalEditVM.TransactionAssignmentVM = transactionAssignmentVM;

                }

                if (outboundExternalEditVM.OutboundExternalBasicInfo.TransactionPathId.HasValue)
                {
                    if (outboundExternalEditVM.UserId != SessionInfo.CurrentUser.Id)
                    {
                        ViewData["TransactionPaths"] = GetTransactionPaths(outboundExternalEditVM.OutboundExternalBasicInfo.TransactionPathId);
                    }
                }

                GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
                  .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();

                IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(outboundExternalEditVM.SavedTransactionAssignment) ?
  JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(outboundExternalEditVM.SavedTransactionAssignment) : transactionAssignmentVMs;
                //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

                string editorMainDocumentSessionKey = Guid.NewGuid().ToString();
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
                ViewData["EditorMainDocumentSessionKey"] = editorMainDocumentSessionKey;
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();
                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(a);
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                Session["DocoNutexplanations"] = null;
                Session["BarcodeImgByte"] = GetBarcodeImage(outboundExternalEditVM.Id, true);
                LogTransactionAction(AuditingActionCode.OpenEditor, outboundExternalEditVM.Id);
                ViewData["WithBarcode"] = true;
                if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && (transactionCategory == TransactionCategory.DraftOutbound))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }
                if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == (int)DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && (transactionCategory == TransactionCategory.DraftOutbound))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(true);
                }
                if (transactionCategory == TransactionCategory.ExternalOutbound)
                {

                    if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(false);
                    }
                    else if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) ||
                        outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    }
                    else
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    }
                }
                #region Add value to key Field

                if (outboundExternalEditVM.Attachments != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Attachments.Count; i++)
                    {
                        outboundExternalEditVM.Attachments[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Archives != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Archives.Count; i++)
                    {
                        outboundExternalEditVM.Archives[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Copies != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Copies.Count; i++)
                    {
                        outboundExternalEditVM.Copies[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Names != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Names.Count; i++)
                    {
                        outboundExternalEditVM.Names[i].Key = i + 1;
                    }
                }
                if (outboundExternalEditVM.Links != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Links.Count; i++)
                    {
                        outboundExternalEditVM.Links[i].Key = i + 1;
                    }
                }
                #endregion

                InitializerAssignmentPaperData(outboundExternalEditVM.Id);

                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;

                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);
                RemoveAllAttachemntsPhysically();
                ViewData["TransactionId"] = trxId;
                ViewData["ConfidentialityName"] = outboundExternalEditVM.OutboundExternalBasicInfo.ConfidentialityLevelText;
                if (outboundExternalEditVM.Copies != null && outboundExternalEditVM.Copies.Count > 0)
                    outboundExternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/OutboundExternal/Editor.cshtml", outboundExternalEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound, UserClaims.Outbound.EditDraft)]
        [CustomAction]
        public ActionResult NotificationEditor(string id, bool IsFromDraft, bool isHubEditable)
        {
            try
            {
                int trxId = int.Parse(StringCipher.Decrypt(id.Replace(" ", "+")));

                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();
                // ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();
                var outboundExternallEditDTO = HttpClientWrapper<GetResult<object>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (outboundExternallEditDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternallEditDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                EditOutboundExternalDTO outboundExternalDTO = new EditOutboundExternalDTO();
                string serilizeObject = JsonConvert.SerializeObject(outboundExternallEditDTO.Result);
                TransactionCategory transactionCategory = TransactionCategory.ExternalOutbound;
                SetTransactionAssignmentToViewed(trxId);
                if (IsFromDraft)
                {
                    var outboundDraftEditDTO = JsonConvert.DeserializeObject<EditOutboundDraftDTO>(serilizeObject);
                    outboundExternalDTO = new EditOutboundExternalDTO()
                    {
                        Attachments = outboundDraftEditDTO.Attachments,
                        Copies = outboundDraftEditDTO.Copies,
                        DocumentDTO = outboundDraftEditDTO.DocumentDTO,
                        ExternalCopies = outboundDraftEditDTO.ExternalCopies,
                        HijriRecordDate = outboundDraftEditDTO.HijriRecordDate,
                        Id = outboundDraftEditDTO.Id,
                        IsSigned = outboundDraftEditDTO.IsSigned,
                        Links = outboundDraftEditDTO.Links,
                        ModifiedByUserId = outboundDraftEditDTO.ModifiedByUserId,
                        Names = outboundDraftEditDTO.Names,
                        OrgUnitId = outboundDraftEditDTO.OrgUnitId,
                        StatusId = outboundDraftEditDTO.StatusId,
                        RecordDate = outboundDraftEditDTO.RecordDate,
                        FollowUp = outboundDraftEditDTO.FollowUps,
                        UserId = outboundDraftEditDTO.UserId,
                        FromUser = outboundDraftEditDTO.FromUser,
                        ToUser = outboundDraftEditDTO.ToUser,
                        OutboundExternalBasicInfo = new EditOutboundExternalBasicInfoDTO()
                        {
                            ConfidentialityLevelId = outboundDraftEditDTO.OutboundDraftBasicInfo.ConfidentialityLevelId,
                            DeliveryMethodId = outboundDraftEditDTO.OutboundDraftBasicInfo.DeliveryMethodId,
                            DestinationId = outboundDraftEditDTO.OutboundDraftBasicInfo.DestinationId,
                            DirectedToId = outboundDraftEditDTO.OutboundDraftBasicInfo.DirectedToId,
                            Hour = outboundDraftEditDTO.OutboundDraftBasicInfo.Hour,
                            IsDraft = true,
                            Minute = outboundDraftEditDTO.OutboundDraftBasicInfo.Minute,
                            OutboundNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.DraftNumber.HasValue ? outboundDraftEditDTO.OutboundDraftBasicInfo.DraftNumber.Value : 0,
                            PriorityLevelId = outboundDraftEditDTO.OutboundDraftBasicInfo.PriorityLevelId,
                            RemindDate = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDate,
                            RemindDateH = outboundDraftEditDTO.OutboundDraftBasicInfo.RemindDateH,
                            SignedById = outboundDraftEditDTO.OutboundDraftBasicInfo.SignedById,
                            TransactionTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionTypeId,
                            Subject = outboundDraftEditDTO.OutboundDraftBasicInfo.Subject,
                            SubjectClassifications = outboundDraftEditDTO.OutboundDraftBasicInfo.SubjectClassifications,
                            SuggestedTopicId = outboundDraftEditDTO.OutboundDraftBasicInfo.SuggestedTopicId,
                            LetterTypeId = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterTypeId,
                            PostCode = outboundDraftEditDTO.OutboundDraftBasicInfo.PostCode,
                            POBox = outboundDraftEditDTO.OutboundDraftBasicInfo.POBox,
                            ReporterId = outboundDraftEditDTO.OutboundDraftBasicInfo.ReporterId,
                            TransactionPathId = outboundDraftEditDTO.OutboundDraftBasicInfo.TransactionPathId,
                            LetterNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.LetterNumber,
                            IsPresentationDraft = outboundDraftEditDTO.OutboundDraftBasicInfo.IsPresentationDraft,
                            PresentationDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.PresentationDraftNumber,
                            IsElcOutBound = outboundDraftEditDTO.OutboundDraftBasicInfo.IsElcOutBound,
                            NeedAcknowled = outboundDraftEditDTO.OutboundDraftBasicInfo.NeedAcknowled,
                            OutBoundDraftNumber = outboundDraftEditDTO.OutboundDraftBasicInfo.OutBoundDraftNumber
                        }
                    };
                    transactionCategory = TransactionCategory.DraftOutbound;

                }
                else
                {
                    outboundExternalDTO = JsonConvert.DeserializeObject<EditOutboundExternalDTO>(serilizeObject);
                }

                EditOutboundExternalVM outboundExternalEditVM = OutboundExternalMapper.Map(outboundExternalDTO);
                // outboundExternalEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundExternalEditVM.HijriRecordDate);

                IList<LookupVM> yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (outboundExternalEditVM.Links != null && outboundExternalEditVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM item in outboundExternalEditVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                Initialize(TransactionCategory.ExternalOutbound);

                //ViewData["OrgUnitsManagers"] = GetAllUsers();


                var gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundExternalEditVM.Attachments, 1, 0, true);
                ViewData["AttachmentData"] = gridAttachment;
                var actionVMs = GetAllActionsValues();
                IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

                actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

                ViewData["AllActionsData2"] = actionVMs;
                ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();
                var transactionArchiveVMs = new List<TransactionArchiveVM>();

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.ReadOnly = true;
                ViewData["EditorViewModel"] = editorViewModel;
                if (outboundExternalEditVM.DocumentVM != null)
                {
                    string documentId = Guid.NewGuid().ToString();
                    if (!string.IsNullOrWhiteSpace(outboundExternalEditVM.DocumentVM.MimeType))
                    {
                        if (outboundExternalEditVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                        {
                            outboundExternalEditVM.EditorType = EditorType.Scanning;
                        }
                        else
                        {
                            outboundExternalEditVM.EditorType = EditorType.TextEditor;

                        }
                    }
                    else
                    {
                        outboundExternalEditVM.EditorType = EditorType.Scanning;

                    }

                    ViewData["hdnDocumentId"] = outboundExternalEditVM.DocumentVM.Id;
                    if (outboundExternalEditVM.EditorType == EditorType.TextEditor)
                    {

                        editorViewModel.ReadOnly = true;

                        editorViewModel.EditorType = EditorType.TextEditor;
                        // editorViewModel.Content = outboundExternalEditVM.DocumentVM != null && outboundExternalEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundExternalEditVM.DocumentVM.Content) : null;

                        string signatureText = string.Format("{0} {1}",
                        SessionInfo.GetObjectFromSession("UserName"),
                        SessionInfo.GetObjectFromSession("UserEmailAddress"));

                        Image signature = ImageHelper.DrawText(signatureText, new Font("Arial", 50), System.Drawing.Color.Black, System.Drawing.Color.Transparent);
                        signature = ImageHelper.SetImageOpacity(signature, (float)0.1);
                        using (MemoryStream m = new MemoryStream())
                        {
                            signature.Save(m, ImageFormat.Png);
                            byte[] imageBytes = m.ToArray();
                            string base64String = Convert.ToBase64String(imageBytes);
                            Session["textEditorSignature"] = base64String;
                        }

                        editorViewModel.IsSigned = outboundExternalEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;

                        editorViewModel.IsSigned = outboundExternalEditVM.IsSigned;

                        string sessionKey = Guid.NewGuid().ToString();

                        ViewData[sessionKey] = sessionKey;

                        Session["DocoNutDocument"] = outboundExternalEditVM.DocumentVM.Content;
                    }
                    ViewData["EditorViewModel"] = editorViewModel;
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = outboundExternalEditVM.DocumentVM.Id,
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundExternalEditVM.DocumentVM.Id.ToString()),
                        AttachmentTypeId = -1,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                        SessionInfo.CultureShortName).Result.Text
                    });
                }

                if (outboundExternalEditVM.Attachments != null)
                {
                    foreach (TransactionAttachmentVM item in outboundExternalEditVM.Attachments)
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
                            Archive.Id = item.Id.ToString();//Guid.NewGuid().ToString();
                            Archive.DocumentId = item.DocumentVM.Id;
                            Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                            Archive.AttachmentTypeId = item.TypeId;
                            Archive.ArcivingTypeName = item.TypeName;
                            Archive.IsNew = true;
                            Archive.IsDeleted = item.DocumentVM.IsDeleted;
                            Archive.AttachmentSource = item.AttachmentSource;
                            Archive.FileName = item.DocumentVM.Name;
                            Archive.FromUserId = item.DocumentVM.FromUserId;
                            Archive.FromEntityId = item.DocumentVM.FromEntityId;

                        }
                        transactionArchiveVMs.Add(Archive);
                    }
                }

                outboundExternalEditVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();
                ViewData["TransactionCategory"] = (int)transactionCategory;
                ViewData["TransactionId"] = outboundExternalEditVM.Id;

                IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Links);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundExternalEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["AllExternalActionsData"] = ViewData["AllActionsData"];
                GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalEditVM.OutboundExternalBasicInfo.DestinationId);
                //ViewData["OrgUnitsManagers"] = GetAllUsers();

                var subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


                GetResult<TransactionPathDetailsDTO> TransactionPathDetails = HttpClientWrapper<GetResult<TransactionPathDetailsDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionPathNextStep?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                if (TransactionPathDetails.Result != null && TransactionPathDetails.Result.Id > 0)
                {
                    TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM()
                    {
                        ActionId = TransactionPathDetails.Result.ActionId,
                        ToOrgUnitId = TransactionPathDetails.Result.OrgUnitId,
                        ToUserId = TransactionPathDetails.Result.UserId,
                        ActionName = TransactionPathDetails.Result.ActionName,
                        ToOrgUnitName = TransactionPathDetails.Result.OrgUnitName,
                        ToUserName = TransactionPathDetails.Result.UserName,
                    };

                    outboundExternalEditVM.TransactionAssignmentVM = transactionAssignmentVM;

                }

                if (outboundExternalEditVM.OutboundExternalBasicInfo.TransactionPathId.HasValue)
                {
                    if (outboundExternalEditVM.UserId != SessionInfo.CurrentUser.Id)
                    {
                        ViewData["TransactionPaths"] = GetTransactionPaths(outboundExternalEditVM.OutboundExternalBasicInfo.TransactionPathId);
                    }
                }

                GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
                  .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();

                IAjaxGrid gridAssignmentPaper = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, 1, 0, true);
                ViewData["AssignmentPaperGridData"] = gridAssignmentPaper;

                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;

                ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

                string editorMainDocumentSessionKey = Guid.NewGuid().ToString();
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
                ViewData["EditorMainDocumentSessionKey"] = editorMainDocumentSessionKey;
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid().ToString();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();
                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(a);
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                Session["DocoNutexplanations"] = null;
                Session["BarcodeImgByte"] = GetBarcodeImage(outboundExternalEditVM.Id, true);
                LogTransactionAction(AuditingActionCode.OpenEditor, outboundExternalEditVM.Id);
                ViewData["WithBarcode"] = true;
                if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && (transactionCategory == TransactionCategory.DraftOutbound))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }
                if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == (int)DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) && (transactionCategory == TransactionCategory.DraftOutbound))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(true);
                }
                if (transactionCategory == TransactionCategory.ExternalOutbound)
                {

                    if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(false);
                    }
                    else if (outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) ||
                        outboundExternalEditVM.OutboundExternalBasicInfo.DeliveryMethodId == DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    }
                    else
                    {
                        ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    }
                }
                #region Add value to key Field

                if (outboundExternalEditVM.Attachments != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Attachments.Count; i++)
                    {
                        outboundExternalEditVM.Attachments[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Archives != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Archives.Count; i++)
                    {
                        outboundExternalEditVM.Archives[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Copies != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Copies.Count; i++)
                    {
                        outboundExternalEditVM.Copies[i].Key = i + 1;
                    }
                }

                if (outboundExternalEditVM.Names != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Names.Count; i++)
                    {
                        outboundExternalEditVM.Names[i].Key = i + 1;
                    }
                }
                if (outboundExternalEditVM.Links != null)
                {
                    for (int i = 0; i < outboundExternalEditVM.Links.Count; i++)
                    {
                        outboundExternalEditVM.Links[i].Key = i + 1;
                    }
                }
                #endregion


                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;

                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);
                RemoveAllAttachemntsPhysically();
                ViewData["TransactionId"] = trxId;

                ViewData["ConfidentialityName"] = outboundExternalEditVM.OutboundExternalBasicInfo.ConfidentialityLevelText;
                if (outboundExternalEditVM.Copies != null && outboundExternalEditVM.Copies.Count > 0)
                    outboundExternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/OutboundExternal/Editor.cshtml", outboundExternalEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound, UserClaims.Outbound.EditDraft)]
        public ActionResult Edit(EditOutboundExternalVM outboundExternalEditVM, TextEditorViewModel editorViewModel, string hdnMainDocToken, string hdnDocumentId)
        {
            try
            {


                ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();
                string message = string.Empty;
                string oldWordlDocument = null;
                AjaxGrid<TransactionArchiveVM> gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, true);
                AjaxGrid<TransactionExternalCopyVM> gridExternalCopy = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionExternalCopyVM>(), 1, 0, true);
                AjaxGrid<TransactionAttachmentVM> gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAttachmentVM>(), 1, 0, true);
                AjaxGrid<TransactionCopyVM> gridCopy = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionCopyVM>(), 1, 0, true);

                outboundExternalEditVM.OrgUnitId = SessionInfo.OrgUnitId;
                outboundExternalEditVM.IsSigned = false;
                if (outboundExternalEditVM.OutboundExternalBasicInfo.PreparationEntityId == null)
                {
                    outboundExternalEditVM.OutboundExternalBasicInfo.PreparationEntityId = SessionInfo.OrgUnitId;
                }

                //Main Document 
                byte[] data = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);
                byte[] wordData = null;
                if (!string.IsNullOrWhiteSpace(editorViewModel.DocumentBase64String))
                    wordData = Convert.FromBase64String(editorViewModel.DocumentBase64String);

                outboundExternalEditVM.DocumentVM = new DocumentVM();
                outboundExternalEditVM.OldDocumentVM = new DocumentVM();


                if (data != null)
                {
                    outboundExternalEditVM.DocumentVM.Content = data;
                    outboundExternalEditVM.DocumentVM.Size = data.Length;
                    outboundExternalEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundExternalEditVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    outboundExternalEditVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundExternalEditVM.DocumentVM.Id = Convert.ToInt32(hdnDocumentId);
                }



                if (wordData != null && wordData.Length > 0)
                {


                    outboundExternalEditVM.OldDocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    outboundExternalEditVM.OldDocumentVM.Content = wordData;
                    outboundExternalEditVM.OldDocumentVM.Size = wordData.Length;
                    outboundExternalEditVM.OldDocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundExternalEditVM.OldDocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    outboundExternalEditVM.OldDocumentVM.Id = editorViewModel.OldDocumentId;

                }
                else if (outboundExternalEditVM.OutboundExternalBasicInfo.IsDraft || outboundExternalEditVM.OutboundExternalBasicInfo.IsDecisionDraft)
                {
                    outboundExternalEditVM.OldDocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundExternalEditVM.OldDocumentVM.Content = data;
                    outboundExternalEditVM.OldDocumentVM.Size = data.Length;
                    outboundExternalEditVM.OldDocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundExternalEditVM.OldDocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    outboundExternalEditVM.OldDocumentVM.Id = editorViewModel.OldDocumentId;
                }




                var prefix = string.Empty;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    prefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
                }
                else
                {
                    prefix = "_" + SessionInfo.CurrentUser.Id + "_";
                }
                if (outboundExternalEditVM.ExternalCopies != null && outboundExternalEditVM.ExternalCopies.Any())
                {
                    foreach (TransactionExternalCopyVM transactionExternalCopy in outboundExternalEditVM.ExternalCopies)
                    {
                        string path = SystemConfigurations.ExternalCopiesAttachmentPath;
                        var filteredByFilename = Directory
                        .GetFiles(path)
                        .Select(o => Path.GetFileName(o))
                        .Where(o => o.StartsWith(prefix + transactionExternalCopy.OrgUnitId.ToString() + "_"));
                        List<ExternalPartyAttachmentVM> externalPartyAttachmentVMs = new List<ExternalPartyAttachmentVM>();

                        foreach (var item in filteredByFilename)
                        {
                            byte[] fileContent = System.IO.File.ReadAllBytes(path + item);
                            string mimeType = MimeMapping.GetMimeMapping(path + item);
                            FileInfo f = new FileInfo(path + item);
                            long size = f.Length;
                            string name = item.Substring(item.LastIndexOf('_') + 1);


                            externalPartyAttachmentVMs.Add(new ExternalPartyAttachmentVM()
                            {
                                Name = name,
                                PartyId = transactionExternalCopy.OrgUnitId,
                                DocumentVM = new DocumentVM
                                {
                                    Name = name,
                                    MimeType = mimeType,
                                    Content = fileContent,
                                    Size = size,
                                    FromEntityId = SessionInfo.OrgUnitId,
                                    FromUserId = SessionInfo.CurrentUser.Id
                                }
                            });

                            f.Delete();
                        }
                        transactionExternalCopy.externalPartyAttachmentVMs = externalPartyAttachmentVMs;

                    }
                }

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                DocumentDTO docDTO = new DocumentDTO();
                outboundExternalEditVM.Attachments = FillTransactionAttachment(outboundExternalEditVM.Archives, documentData);//fill attachments


                Session["DocumentData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                TempData["TransactionData"] = outboundExternalEditVM;
                TempData.Keep("TransactionData");

                PostResult postResult = null;
                EditOutboundExternalDTO addOutbound = OutboundExternalMapper.Map(outboundExternalEditVM);

                var transactionCategory = TransactionCategory.ExternalOutbound;
                if (outboundExternalEditVM.OutboundExternalBasicInfo.IsDraft || outboundExternalEditVM.OutboundExternalBasicInfo.IsDecisionDraft)
                {
                    #region Update
                    var addOutboundDraftDTO = new EditOutboundDraftDTO
                    {
                        Attachments = addOutbound.Attachments,
                        Copies = addOutbound.Copies,
                        ExternalCopies = addOutbound.ExternalCopies,
                        DocumentDTO = addOutbound.DocumentDTO,
                        OldDocumentDTO = addOutbound.OldDocumentDTO,
                        Names = addOutbound.Names,
                        OrgUnitId = SessionInfo.OrgUnitId,
                        Id = addOutbound.Id,
                        EditorType = addOutbound.EditorType,
                        Links = addOutbound.Links,
                        UserId = addOutbound.UserId,
                        HijriRecordDate = addOutbound.HijriRecordDate,
                        IsSigned = addOutbound.IsSigned,
                        StatusId = addOutbound.StatusId,
                        RecordDate = addOutbound.RecordDate,
                        FollowUps = addOutbound.FollowUp,
                        OutboundDraftBasicInfo = new EditOutboundDraftBasicInfoDTO()
                        {
                            TransactionTypeId = addOutbound.OutboundExternalBasicInfo.TransactionTypeId,
                            ConfidentialityLevelId = addOutbound.OutboundExternalBasicInfo.ConfidentialityLevelId,
                            ExternalPartyId = addOutbound.OutboundExternalBasicInfo.ExternalPartyId > 0 ? addOutbound.OutboundExternalBasicInfo.ExternalPartyId : (int?)null,
                            DestinationId = addOutbound.OutboundExternalBasicInfo.DestinationId,
                            DirectedToId = addOutbound.OutboundExternalBasicInfo.DirectedToId,
                            DraftNumber = addOutbound.OutboundExternalBasicInfo.DirectedToId,
                            Hour = addOutbound.OutboundExternalBasicInfo.Hour,
                            Minute = addOutbound.OutboundExternalBasicInfo.Minute,
                            PriorityLevelId = addOutbound.OutboundExternalBasicInfo.PriorityLevelId,
                            RemindDate = addOutbound.OutboundExternalBasicInfo.RemindDate,
                            RemindDateH = addOutbound.OutboundExternalBasicInfo.RemindDateH,
                            SignedById = addOutbound.OutboundExternalBasicInfo.SignedById,
                            Subject = addOutbound.OutboundExternalBasicInfo.Subject,
                            SubjectClassifications = addOutbound.OutboundExternalBasicInfo.SubjectClassifications,
                            SuggestedTopicId = addOutbound.OutboundExternalBasicInfo.SuggestedTopicId,
                            LetterTypeId = addOutbound.OutboundExternalBasicInfo.LetterTypeId,
                            DeliveryMethodId = addOutbound.OutboundExternalBasicInfo.DeliveryMethodId,
                            IsDraft = true,
                            POBox = addOutbound.OutboundExternalBasicInfo.POBox,
                            PostCode = addOutbound.OutboundExternalBasicInfo.PostCode,
                            ReporterId = addOutbound.OutboundExternalBasicInfo.ReporterId,
                            TransactionPathId = addOutbound.OutboundExternalBasicInfo.TransactionPathId,
                            PreparationEntityId = addOutbound.OutboundExternalBasicInfo.PreparationEntityId,
                            isOutboundInternalDraft = addOutbound.OutboundExternalBasicInfo.isOutboundInternalDraft,
                            Remarks = addOutbound.OutboundExternalBasicInfo.Remarks,
                            LetterNumber = addOutbound.OutboundExternalBasicInfo.LetterNumber,
                            IsDecisionDraft = addOutbound.OutboundExternalBasicInfo.IsDecisionDraft,
                            Summary = addOutbound.OutboundExternalBasicInfo.Summary,

                        }
                    };

                    DistributionListVM distributionList = new DistributionListVM();

                    if (addOutboundDraftDTO.OutboundDraftBasicInfo.DistrubutionListId != null)
                    {
                        GetResult<DistributionListDTO> distributionListDTO =
                                 HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, addOutboundDraftDTO.OutboundDraftBasicInfo.DistrubutionListId.Value)).Result;

                        distributionList = DistributionListMapper.Map(distributionListDTO.Result);
                    }

                    List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                    if (distributionList.DistributionListDetails != null)
                    {
                        foreach (var item in distributionList.DistributionListDetails)
                        {
                            if (!addOutboundDraftDTO.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                            {
                                if ((item.UserId != 0 && !addOutboundDraftDTO.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                                   (item.UserId == 0 && !addOutboundDraftDTO.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
                                {
                                    TransactionCopyVM copy = new TransactionCopyVM
                                    {
                                        ActionId = (int)CopiesActions.ToView,
                                        UserId = item.UserId,
                                        OrgUnitId = item.OrgUnitId,

                                    };
                                    Copies.Add(copy);
                                }
                            }
                        }

                        addOutboundDraftDTO.Copies.AddRange(TransactionCopyMapper.Map(Copies));
                    }

                    postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransaction?cultureName=" + SessionInfo.CultureShortName, addOutboundDraftDTO).Result;

                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                    #endregion


                }
                else
                {

                    #region Update
                    DistributionListVM distributionList = new DistributionListVM();

                    if (addOutbound.OutboundExternalBasicInfo.DistrubutionListId != null)
                    {
                        GetResult<DistributionListDTO> distributionListDTO =
                                 HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, addOutbound.OutboundExternalBasicInfo.DistrubutionListId.Value)).Result;

                        distributionList = DistributionListMapper.Map(distributionListDTO.Result);
                    }

                    List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                    if (distributionList.DistributionListDetails != null)
                    {
                        foreach (var item in distributionList.DistributionListDetails)
                        {
                            if (!addOutbound.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                            {
                                if ((item.UserId != 0 && !addOutbound.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                                   (item.UserId == 0 && !addOutbound.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
                                {
                                    TransactionCopyVM copy = new TransactionCopyVM
                                    {
                                        ActionId = (int)CopiesActions.ToView,
                                        UserId = item.UserId,
                                        OrgUnitId = item.OrgUnitId
                                    };
                                    Copies.Add(copy);
                                }
                            }
                        }

                        addOutbound.Copies.AddRange(TransactionCopyMapper.Map(Copies));
                    }

                    postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransaction?cultureName=" + SessionInfo.CultureShortName, addOutbound).Result;
                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    var outboundDTO = HttpClientWrapper<GetResult<EditOutboundDraftDTO>>
                       .GetItemRequest($"api/Transaction/GetTransaction?transactionId={outboundExternalEditVM.Id}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;

                    var refreshedTransactionExternalCopies = TransactionExternalCopyMapper.Map(outboundDTO.Result.ExternalCopies);

                    var attachmentResult = TransactionAttachmentMapper.Map(outboundDTO.Result.Attachments) ?? new List<TransactionAttachmentVM>();

                    List<TransactionArchiveVM> TransactionArchives = new List<TransactionArchiveVM>();
                    foreach (TransactionAttachmentVM item in attachmentResult)
                    {

                        TransactionArchiveVM Archive = new TransactionArchiveVM
                        {
                            Number = item.Number,
                            AttachmentTypeId = item.TypeId,
                            ArcivingTypeName = item.TypeName,
                            Archivable = item.Archivable,
                            AttachmentName = item.AttachmentName,
                            IsEnableAction = item.IsEnableAction,
                            UserId = item.UserId,
                            ReadOnly = !(item.UserId == SessionInfo.CurrentUser.Id),
                            AttachmentSource = item.AttachmentSource,
                            JFile = item.Archivable ? TransactionAttachmentMapper.GetArchivingFileDate(item) : string.Empty,
                        };

                        if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                        {
                            Archive.Id = item.Id.ToString();//Guid.NewGuid().ToString();
                            Archive.DocumentId = item.DocumentVM.Id;
                            Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                            Archive.AttachmentTypeId = item.TypeId;
                            Archive.ArcivingTypeName = item.TypeName;
                            Archive.IsNew = true;
                            Archive.IsDeleted = item.DocumentVM.IsDeleted;
                            Archive.FromEntityId = item.DocumentVM.FromEntityId;
                            Archive.FromUserId = item.DocumentVM.FromUserId;
                            Archive.FileName = item.DocumentVM.Name;
                        }
                        TransactionArchives.Add(Archive);
                    }

                    var transactionArchiveIncVMs = TransactionArchives.Where(t => t.IsMainDocument == false).ToList();
                    int archiveCount = transactionArchiveIncVMs.Count;
                    for (int i = 0; i < archiveCount; i++)
                    {
                        transactionArchiveIncVMs[i].Key = i + 1;
                    }
                    gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveIncVMs, 1, transactionArchiveIncVMs.Count, true);



                    gridExternalCopy = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(refreshedTransactionExternalCopies, 1, refreshedTransactionExternalCopies.Count, true);
                    var refreshedTransactionCopies = TransactionCopyMapper.Map(outboundDTO.Result.Copies);
                    if (refreshedTransactionCopies != null && refreshedTransactionCopies.Count > 0)
                        refreshedTransactionCopies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                    gridCopy = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(refreshedTransactionCopies, 1, refreshedTransactionCopies.Count, true);
                    #endregion

                }

                ViewData["TransactionCategory"] = (int)transactionCategory;
                if (addOutbound.Links != null && addOutbound.Links.Count > 0)
                {
                    foreach (TransactionLinkDTO link in addOutbound.Links)
                    {
                        GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                       .GetItemRequest(string.Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                       link.TransactionId,
                       SessionInfo.CultureShortName)).Result;
                        var User = transactionDetailsDTOResult.Result.ToUserId;
                        if (User == null)
                        {
                            User = SessionInfo.CurrentUser.Id;
                        }
                        message = string.Empty;
                        string remarks = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Link");
                        PutResult putResult =
                            HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/LinkedMoveTransaction?transactionId={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}",
                            link.TransactionId, SessionInfo.OrgUnitId, (int)TrayActionType.Save, null, (int)TrayType.MyTransactions, remarks, User), null).Result;

                    }
               
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundExternal.UpdateSucceeded");
                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    Date = outboundExternalEditVM.HijriRecordDate,
                    OutboundExternalNumber = outboundExternalEditVM.OutboundExternalBasicInfo.OutboundNumber,
                    IsDraft = (outboundExternalEditVM.OutboundExternalBasicInfo.IsDraft || outboundExternalEditVM.OutboundExternalBasicInfo.IsDecisionDraft)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        public ActionResult AddTransactionAttachment(TransactionAttachmentVM attachmentVM, string hdnAttachments, string ArchiveAttachmentsData)
        {
            try
            {
                return null;// AddAttachment(attachmentVM, hdnAttachments, ArchiveAttachmentsData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteTransactionAttachments(string ids, string hdnAttachments, string ArchiveAttachmentsData, string hdnArchiveData)
        {
            try
            {
                return DeleteAttachments(ids, hdnAttachments, ArchiveAttachmentsData, hdnArchiveData);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Names.Add)]
        public ActionResult AddTransactionName(TransactionNameVM nameVM, string hdnNames)
        {
            try
            {
                return null;// AddName(nameVM, hdnNames);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult GetPreviousOutboundExternal(TransactionCategory TransactionTypeToGetPrevious)
        {
            try
            {
                string message = string.Empty;
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();

                Initialize(TransactionCategory.ExternalOutbound);

                ViewData["HijriDate"] = string.Empty;
                ViewData["HijriDateTitle"] = DbRes.TResource("User.OutboundExternal.BasicInfo.RecordDate");
                ViewData["TransactionNumber"] = string.Empty;
                ViewData["TransactionNumberTitle"] = DbRes.TResource("User.OutboundExternal.BasicInfo.OutboundNumber");

                GetResult<AddOutboundExternalDTO> outboundExternalAddDTO =
                  HttpClientWrapper<GetResult<AddOutboundExternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}", SessionInfo.CultureShortName, TransactionTypeToGetPrevious, SessionInfo.OrgUnitId)).Result;

                if (outboundExternalAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternalAddDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundExternalAddDTO.Result == null)
                {
                    if (TransactionTypeToGetPrevious == TransactionCategory.ExternalOutbound)
                    {
                        message = DbRes.TResource("User.OutboundExternal.NoPreviousDataInfoMsg");
                    }
                    else
                    {
                        message = DbRes.TResource("User.OutboundDraft.NoPreviousDataInfoMsg");
                    }
                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.PreparationEntityId);

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                   HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);

                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagersInExternalCopies"] = GetManagersByPartyId(outboundExternalAddDTO.Result.OutboundExternalBasicInfo.DestinationId);
                //ViewData["OrgUnitsManagers"] = GetAllUsers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //  HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //if (subjectClassificationDTOs.Result != null && outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SubjectClassifications != null)
                //{
                //    outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}
                ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));
                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs = HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>
                //    .GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundExternalAddDTO.Result.OutboundExternalBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundExternalBasicInfo";

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.EditorType = EditorType.Scanning;
                editorViewModel.Content = string.Empty;
                editorViewModel.ReadOnly = false;
                ViewData["EditorViewModel"] = editorViewModel;
                GetResult<List<FormDTO>> formDocumentDTOs = HttpClientWrapper<GetResult<List<FormDTO>>>
                    .GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();
                if (formDocumentDTOs.Result != null)
                {
                    foreach (FormVM formVM in FormMapper.Map(formDocumentDTOs.Result))
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formVM.Id.ToString(),
                            Label = formVM.LocalName
                        });
                    }
                }

                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);

                return Json(new
                {
                    Html =
                    UIHelper.RenderRazorViewToHtml(ControllerContext, "_BasicInfoAddPartial", OutboundExternalMapper.Map(outboundExternalAddDTO.Result).OutboundExternalBasicInfo),
                    UserHasTransactions = true,
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.AddExternalParty.AddExternalPartyForOutbound)]
        public ActionResult AddNewExternalPartyForTransaction(string treeName, string onAddPartyfuntion, int transactionId = 0)
        {
            try
            {
                return AddNewExternalParty(treeName, onAddPartyfuntion, transactionId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.AddExternalParty.AddExternalPartyForOutbound)]
        public ActionResult AddTransactionExternalParty(ExternalPartyAddVM externalPartyAddVM)
        {
            try
            {
                return AddExternalParty(externalPartyAddVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetExternalParties(int selectedParty)
        {
            try
            {
                List<ExternalPartyVM> externalPartyVMs = new List<ExternalPartyVM>();

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
               HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);

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

                return PartialView("~/Areas/User/Views/OutboundExternal/_ExternalPartiesPartial.cshtml", new OutboundExternalPartiesVM() { DestinationId = selectedParty != -1 ? selectedParty : 1 });
            }
            catch (Exception)
            {
                throw;
            }
        }
        public override void InitializeExternalParties()
        {
            try
            {
                //GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                //  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                //var externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                //ViewData["ExternalPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;

                yesserData = "";
                //foreach (var item in externalPartyVMs)
                //{
                //    if (item.YasserRegistered)
                //    {
                //        yesserData = yesserData == string.Empty ? item.Id.ToString() : (yesserData + "," + item.Id.ToString());
                //    }
                //}

                //ViewData["isYesserRegisterd"] = yesserData;
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

            //IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, true);
            //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

            ViewData["TransactionCategory"] = (int)transactionCategory;

            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);

            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs);
            ViewData["DepartmentsDataCopies"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

            GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

            ViewData["ExternalPartiesData"] = (externalPartyDTOs.Result != null) ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;

            //var yesserRegisterd = ExternalPartyMapper.Map(externalPartyDTOs.Result);
            string data = "";

            ViewData["isYesserRegisterd"] = data;
            ViewData["SettingCity"] = GetCitySetting();

            ViewData["LinkTypeData"] = GetLinkTypes(transactionCategory);
            ViewData["PrioritiesData"] = TransactionHelper.GetPriorities(transactionCategory);
            ViewData["PrivecyLevelsData"] = TransactionHelper.GetPrivecyLevels(transactionCategory);
            ViewData["AttachmentsTypeData"] = TransactionHelper.GetAttachmentTypes(transactionCategory);
            ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(transactionCategory);
            ViewData["LetterTypeData"] = TransactionHelper.GetLetterTypes(transactionCategory);
            ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
            ViewData["PrivecyLevelData"] = TransactionHelper.GetPrivecyLevels(transactionCategory);

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
            ViewData["Reporters"] = GetReporters();

            //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
            //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

            //    GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
            //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            // ViewData["SubjectClassifications"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

        }

        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public ActionResult GetDeliveryReportByTransactionId(int transactionId, int type)
        {
            string message = string.Empty;
            var postResult = HttpClientWrapper<PostResult>
                .PostRequest($"api/Transaction/GetDeliveryReportByTransactionIds?transactionId={transactionId}&type={type}", null).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                ReportsIds = JsonConvert.SerializeObject(postResult.Result),
                JsonRequestBehavior.AllowGet
            });
        }
        public ActionResult GetDeliveryReportByTransactionIdV2(int transactionId, int type)
        {
            string message = string.Empty;
            var postResult = HttpClientWrapper<PostResult>
                .PostRequest($"api/Transaction/GetDeliveryReportByTransactionAllIds?transactionId={transactionId}&type={type}", null).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                ReportsIds = JsonConvert.SerializeObject(postResult.Result),
                JsonRequestBehavior.AllowGet
            });
        }
        [HttpPost]
        public ActionResult ConvertDraftToOutbound(int transactionId, string document, bool isDecisionDraft)
        {

            string message = string.Empty;


            //int oldWordDocument = mainDocumentDTO.Result.Id;

            PutResult putResultConvertDraftToOutbound = HttpClientWrapper<PutResult>
                                                     .PutRequest(string.Format("api/Transaction/ConvertDraftToOutbound?draftTransactionId={0}", transactionId), new { })
                                                     .Result;

            if (putResultConvertDraftToOutbound.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResultConvertDraftToOutbound.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            GetResult<DocumentDTO> mainDocumentDTO =
                HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetMainDocument?transactionId={0}", transactionId)).Result;


            byte[] content = null;
            if (!isDecisionDraft)
            {
                var barcode = GetBarcodeByte(transactionId);
                content = addImageToPDF(mainDocumentDTO.Result.Content, barcode, Convert.ToInt32(ConfigurationManager.AppSettings["BarcodePxMaxWidth"].ToString()), Convert.ToInt32(ConfigurationManager.AppSettings["BarcodePxMaxHeight"].ToString()));

            }

            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/UpdateMainDocument_New?transactionId={0}", transactionId), content).Result;


            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = putResult.Id.ToString() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult UpdatePresentationDraftNumber(string transactionId)
        {
            //int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
            string message = string.Empty;

            PutResult putResult = HttpClientWrapper<PutResult>
                                                      .PutRequest(string.Format("api/Transaction/UpdatePresentationDraftNumber?draftTransactionId={0}", transactionId), new { })
                                                      .Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }


            return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = putResult.Id.ToString() }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TransactionElcOutBoundAdd(TransactionElcOutBoundVm transactionElcOutBoundVm)
        {
            try
            {

                string message = string.Empty;
                PostResult postResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/TransactionElcOutBoundAdd"), TransactionElcOutBoundMapper.Map(transactionElcOutBoundVm)).Result;

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
        [HttpPost]
        public ActionResult AcknowledgeElcOutBound(int transactionId)
        {
            try
            {

                string message = string.Empty;
                PostResult postResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AcknowledgeElcOutBound?userId={0}&orgUnitId={1}&ishidden={2}&transactionId={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, true, transactionId), null).Result;

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
        private List<ActionVM> GetAllActionsValues()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);


            return processVMs;
        }



    }

}
