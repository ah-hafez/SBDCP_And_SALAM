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
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Framework.Persistence;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User;
using MCS.UI.Areas.User.Controllers;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Action;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Permission;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Inbound;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.External;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Common;
using Newtonsoft.Json.Linq;
using MCS.UI.Areas.User.Mappers.UserPreferences;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.Common.Utility;
using DotnetDaddy.DocumentConfig;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Drawing;
using Font = System.Drawing.Font;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Helpers;
using System.Net.Http;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using TXTextControl;
using System.Threading;
using DocumentFormat.OpenXml.Drawing.Charts;
using MvcSiteMapProvider.Linq;
using OpenMcdf;
using DocumentFormat.OpenXml.Office2010.Excel;
using System.Configuration;
using MCS.Framework.Logging;
using DocumentFormat.OpenXml.Office2010.Word;
using Org.BouncyCastle.Bcpg.OpenPgp;
using ZXing;
using MCS.DoconutMVC.Helpers;
using Spire.Pdf.Lists;
using System.Globalization;
using MCS.Domain;

namespace MCS.UI
{
    public class TransactionController : BaseController
    {

        public string TempStorgepath = string.Empty;
        public static string StartKey = "Transaction";
        public static string EndKey = "GAMI";

        public static char Sperator = '_';


        [HttpGet]
        public ActionResult InboundOpen()
        {
            //TODO: Doconut
            Session["DocoNutDocument"] = null;
            try
            {
                ViewData["TransactionTypesData"] = GetTransactionTypes(TransactionCategory.Inbound);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                TransactionOpenVM transactionOpenVM = new TransactionOpenVM();
                transactionOpenVM.TransactionCategory = TransactionCategory.Inbound;
                return View("~/Areas/User/Views/Transaction/TransactionOpen.cshtml", transactionOpenVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        public ActionResult OutboundOpen()
        {
            try
            {
                ViewData["TransactionTypesData"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                TransactionOpenVM transactionOpenVM = new TransactionOpenVM();
                transactionOpenVM.TransactionCategory = TransactionCategory.ExternalOutbound;
                return View("~/Areas/User/Views/Transaction/TransactionOpen.cshtml", transactionOpenVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetTransaction(TransactionOpenVM transactionOpenVM)
        {
            try
            {
                string message = string.Empty;
                MessageType messageType = MessageType.Information;
                LookupVM yearLookup = LookupsHelper.GetLookupItem(transactionOpenVM.Year, SessionInfo.CultureShortName).Result;
                switch (transactionOpenVM.TransactionCategory)
                {
                    case TransactionCategory.Inbound:
                        GetResult<EditInboundDTO> inboundEditDTO =
                        HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?cultureName={0}&userId={1}&transactionNumber={2}&transactionCategory={3}&year={4}&sourceId={5}&orgUnitId={6}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, transactionOpenVM.TransactionNumber, transactionOpenVM.TransactionCategory, Convert.ToInt32(yearLookup.Text), transactionOpenVM.TransactionTypeId, transactionOpenVM.OrgUnitId)).Result;
                        if (inboundEditDTO.StatusCode != StatusCode.Ok)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, inboundEditDTO.StatusCode.ToString());
                            messageType = MessageType.Error;
                        }
                        TempData["TransactionData"] = EditInboundMapper.Map(inboundEditDTO.Result);
                        break;
                    case TransactionCategory.ExternalOutbound:
                        GetResult<EditOutboundExternalDTO> outboundExternalEditDTO =
                        HttpClientWrapper<GetResult<EditOutboundExternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?cultureName={0}&userId={1}&transactionNumber={2}&transactionCategory={3}&year={4}&sourceId={5}&orgUnitId={6}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, transactionOpenVM.TransactionNumber, transactionOpenVM.TransactionCategory, Convert.ToInt32(yearLookup.Text), transactionOpenVM.TransactionTypeId, transactionOpenVM.OrgUnitId)).Result;
                        if (outboundExternalEditDTO.StatusCode != StatusCode.Ok)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundExternalEditDTO.StatusCode.ToString());
                            messageType = MessageType.Error;
                            break;
                        }
                        TempData["TransactionData"] = OutboundExternalMapper.Map(outboundExternalEditDTO.Result);
                        break;
                    case TransactionCategory.InternalOutbound:
                        GetResult<EditOutboundInternalDTO> outboundInternalEditDTO =
                        HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?cultureName={0}&userId={1}&transactionNumber={2}&transactionCategory={3}&year={4}&sourceId={5}&orgUnitId={6}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, transactionOpenVM.TransactionNumber, transactionOpenVM.TransactionCategory, Convert.ToInt32(yearLookup.Text), transactionOpenVM.TransactionTypeId, transactionOpenVM.OrgUnitId)).Result;
                        if (outboundInternalEditDTO.StatusCode != StatusCode.Ok)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalEditDTO.StatusCode.ToString());
                            messageType = MessageType.Error;
                        }
                        TempData["TransactionData"] = OutboundInternalMapper.Map(outboundInternalEditDTO.Result);
                        break;
                    case TransactionCategory.DraftOutbound:
                        GetResult<EditOutboundDraftDTO> outboundDraftEditDTO =
                        HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?cultureName={0}&userId={1}&transactionNumber={2}&transactionCategory={3}&year={4}&sourceId={5}&orgUnitId={6}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, transactionOpenVM.TransactionNumber, transactionOpenVM.TransactionCategory, Convert.ToInt32(yearLookup.Text), transactionOpenVM.TransactionTypeId, transactionOpenVM.OrgUnitId)).Result;
                        if (outboundDraftEditDTO.StatusCode != StatusCode.Ok)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundDraftEditDTO.StatusCode.ToString());
                            messageType = MessageType.Error;
                        }
                        TempData["TransactionData"] = OutboundDraftMapper.Map(outboundDraftEditDTO.Result);
                        break;
                }

                return Json(new { MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public string GetTransactionTypes(string transactionType)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<TransactionTypeDTO>> transactionTypeDTOs =
                    HttpClientWrapper<GetResult<List<TransactionTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionType={0}", transactionType)).Result;
                if (transactionTypeDTOs.Result != null)
                {
                    foreach (TransactionTypeVM transactionTypeVM in TransactionTypeMapper.Map(transactionTypeDTOs.Result))
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
        public virtual void InitializeExternalParties()
        {
            try
            {
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                var externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                ViewData["ExternalPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual void Initialize(TransactionCategory transactionCategory)
        {
            try
            {
                TempData["ControllerName"] = null;
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
                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;
                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;
                IAjaxGrid gridCopies = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionCopyVM>(), 1, 0, true);
                ViewData["CopiesData"] = gridCopies;
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["TempDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);


                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
                {
                    GetResult<List<OrgUnitDTO>> AssignorgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["AssignDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(AssignorgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllChildsModules))
                {
                    GetResult<OrgUnitDTO> AssignorgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    AssignorgUnitDTOs.Result.ParentId = -1;
                    AssignorgUnitDTOs.Result.HasChilds = true;
                    newList.Add(AssignorgUnitDTOs.Result);
                    ViewData["AssignDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.ParentDepartment))
                {
                    GetResult<OrgUnitDTO> AssignorgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    AssignorgUnitDTOs.Result.ParentId = -1;
                    AssignorgUnitDTOs.Result.HasChilds = true;
                    newList.Add(AssignorgUnitDTOs.Result);
                    ViewData["AssignDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else
                {
                    GetResult<OrgUnitDTO> AssignorgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    AssignorgUnitDTOs.Result.ParentId = -1;
                    AssignorgUnitDTOs.Result.HasChilds = false;
                    newList.Add(AssignorgUnitDTOs.Result);
                    ViewData["AssignDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                ViewData["DepartmentsData"] = ViewData["TempDepartmentsData"] as TreeViewModel;

                //ViewData["FollowUpDepartmentsData"] = ViewData["TempDepartmentsData"] as TreeViewModel;
                ViewData["FollowUpProccess"] = GetFollowUpProccess(transactionCategory);
                ViewData["FollowupPeriod"] = FollowupPeriod(transactionCategory);
                ViewData["DepartmentsDataCopies"] = ViewData["TempDepartmentsData"] as TreeViewModel;
                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionExternalCopyVM>(), 1, 0, true);
                ViewData["ExternalCopiesData"] = gridExternalCopies;
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                ViewData["ExternalCopiesPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;



                //var yesserRegisterd = ""; ExternalPartyMapper.Map(externalPartyDTOs.Result);
                //string data = "";
                //foreach (var item in yesserRegisterd)
                //{
                //    if (item.YasserRegistered)
                //    {
                //        data = data == string.Empty ? item.Id.ToString() : (data + "," + item.Id.ToString());
                //    }
                //}
                //ViewData["isYesserRegisterd"] = data;
                //ViewData["SignedByOrgUnitDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //InitializeExternalParties();
                ViewData["LinkTypeData"] = GetLinkTypes(transactionCategory);
                //ViewData["ExternalCopiesPartiesData"] = ViewData["ExternalPartiesData"] as TreeViewModel;
                ViewData["PrioritiesData"] = GetPriorities(transactionCategory);
                ViewData["AttachmentsTypeData"] = GetAttachmentTypes(transactionCategory);
                ViewData["TransactionTypesData"] = GetTransactionTypes(transactionCategory);
                ViewData["LetterTypeData"] = GetLetterTypes(transactionCategory);
                ViewData["ActionData"] = GetActions();
                ViewData["AssignmentGroupData"] = GetUserAssignmentGroups();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["HasAssignmentPaper"] = CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = CheckOrgUnitIsAllowedToCreateGroup();
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(new List<TransactionAttachmentVM>());
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(new List<TransactionNameVM>());
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(new List<TransactionLinkVM>());
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(new List<TransactionCopyVM>());
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(new List<TransactionExternalCopyVM>());
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(new List<AutoCompleteDataSource>());
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["SettingCity"] = GetCitySetting();
                ViewData["MainDocumentId"] = null;
                ViewData["DocumentId"] = null;
                ViewData["TransactionCategoryId"] = (int)transactionCategory;
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AllExternalActionsData"] = ViewData["AllActionsData"];
                ViewData["ExternalPartyAscDesc"] = TransactionHelper.GetByExternalPartyAscDesc();
                ViewData["SourceTypeAscDesc"] = TransactionHelper.GetBySourceTypeAscDesc();
                //ViewData["SubjectClassifications"] = GetSubjectClassification();

                //ViewData["SubjectClassifications"] = GetSubjectClassification();
                //GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.GeneralSettings.ConfidentialityAcknowledgment)).Result;
                //ViewData["ConfidentialityAcknowledgment"] = SettingMapper.Map(SettingValue.Result).Value.ToString();
                ViewData["ConfidentialityAcknowledgment"] = GetConfidentialityAcknowledgments(transactionCategory);
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();
                if (!string.IsNullOrEmpty(ViewData["ActionData"].ToString()))
                {
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["ActionData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }
                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;
                if (transactionCategory == TransactionCategory.Inbound || transactionCategory == TransactionCategory.InternalOutbound)
                {
                    ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    ViewData["ReceiveMethod"] = GetDeliveryMethod(false);
                }
                ViewData["SessionArchiveMainDocumentKey"] = Guid.NewGuid().ToString();
                ViewData["SessionArchiveDocumentKey"] = Guid.NewGuid().ToString();

                ViewData["Reporters"] = GetReporters();

                Session["BarcodeImgByte"] = null;
                Session["DocumentData"] = null;
                Session["DocViewerData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                Session["DocoNutexplanations"] = null;


                ViewData["DistributionLists"] = GetDistributionLists();
                ViewData["TransactionPaths"] = GetTransactionPaths();

                GetResult<UserPreferenceDTO> userPreferenceResult =
                HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}&orgUnitId={2}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                UserPreferenceVM userPreferenceVMS = UserPreferenceMapper.Map(userPreferenceResult.Result);
                if (userPreferenceVMS != null)
                {

                    List<OrgStructureInfoVM> orgStructureInfoVMs = GetOrgStructuresLight();
                    OrgStructureInfoVM orgStructureInfoVM = orgStructureInfoVMs.FirstOrDefault(o => o.Key == SessionInfo.OrgUnitId);


                    ViewData["FollowUpOrgId"] = orgStructureInfoVM.FollowupDepartmentId.HasValue ? orgStructureInfoVM.FollowupDepartmentId.Value : userPreferenceVMS.FollowUpOrgId;
                    //ViewData["FollowUpOrgId"] = userPreferenceVMS.FollowUpOrgId;

                    ViewData["FollowUpUserId"] = userPreferenceVMS.FollowUpUserId;
                    ViewData["FollowUpOrgUnitUsersData"] = userPreferenceVMS.FollowUpOrgId.HasValue ? GetUsersByOrgUnitId(userPreferenceVMS.FollowUpOrgId.Value) : null;
                }

                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["PrivecyLevelsData"] = TransactionHelper.GetPrivecyLevels(transactionCategory);


            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<OrgStructureInfoVM> GetOrgStructuresLight()
        {
            string url = $"api/Admin/GetOrgUnitsLight?cultureName={SessionInfo.CultureShortName}";
            var orgStructureInfoDTO = HttpClientWrapper<GetResult<List<OrgStructureInfoDTO>>>.GetItemRequest(url).Result;
            List<OrgStructureInfoVM> result = Map(orgStructureInfoDTO.Result, SessionInfo.CultureShortName);

            return result;
        }

        private static List<OrgStructureInfoVM> Map(IList<OrgStructureInfoDTO> organizationStructureInfoDTOs, string cultureName)
        {
            if (organizationStructureInfoDTOs == null || !organizationStructureInfoDTOs.Any())
            {
                return new List<OrgStructureInfoVM>();
            }
            List<OrgStructureInfoVM> organizationStructureInfoVMs = organizationStructureInfoDTOs
                .Select(b => new OrgStructureInfoVM
                {
                    AssignmentPaper = AssignmentPaperMapper.Map(b.AssignmentPaper),
                    BarCode = b.BarCode,
                    // BarcodeDesigners = BarcodeDesignerMapper.Map(b.BarcodeDesigners),
                    // Counter = CounterMapper.Map(b.Counter, cultureName),
                    IdentifierId = b.IdentifierId,
                    IsActive = b.IsActive,
                    IsDeleted = b.IsDeleted,
                    IsExternal = b.IsExternal,
                    LinkUnitsKeys = b.LinkUnitsKeys,
                    IsNew = b.IsNew,
                    ManagerId = b.ManagerId,
                    Names = LocalizationMapper.Map(b.Names),
                    StructureAsJson = b.StructureAsJson,
                    TransactionsProcessingPeriod = b.TransactionsProcessingPeriod,
                    // Users = OrgUnitMapper.Map(b.Users),
                    IsVirtualUnit = b.IsVirtualUnit,
                    Key = b.Key,
                    Name = b.Name,
                    Number = b.Number,
                    ParentId = b.ParentId,
                    HasChilds = b.HasChilds,
                    //Lineage = b.Lineage,
                    //ExternalId = b.ExternalId,
                    //IoDepartment = b.IoDepartment,
                    //IsExecutive = b.IsExecutive,
                    FollowupDepartmentId = b.FollowupDepartmentId
                }).ToList();
            return organizationStructureInfoVMs;
        }

        private object GetSubjectClassification()
        {
            try
            {
                GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                        HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (subjectClassificationDTOs.Result != null)
                {
                    foreach (SubjectClassificationDTO subjectClassificationDTO in subjectClassificationDTOs.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = subjectClassificationDTO.Id.ToString(),
                            Label = subjectClassificationDTO.LocalName
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
        public virtual void EditInitialize(TransactionCategory transactionCategory)
        {
            try
            {
                TempData["ControllerName"] = null;
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
                IAjaxGrid gridAssignmentIndividual = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                ViewData["AssignmentIndividualGridData"] = gridAssignmentIndividual;
                //IAjaxGrid gridAssignmentGroup = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionAssignmentVM>(), 1, 0, true);
                //ViewData["AssignmentGroupGridData"] = gridAssignmentGroup;
                IAjaxGrid gridCopies = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionCopyVM>(), 1, 0, true);
                ViewData["CopiesData"] = gridCopies;
                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionExternalCopyVM>(), 1, 0, true);
                ViewData["ExternalCopiesData"] = gridExternalCopies;
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;
                ViewData["ExternalCopiesPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;
                var yesserRegisterd = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                string data = "";
                foreach (var item in yesserRegisterd)
                {
                    if (item.YasserRegistered)
                    {
                        data = data == string.Empty ? item.Id.ToString() : (data + "," + item.Id.ToString());
                    }
                }
                ViewData["isYesserRegisterd"] = data;
                InitializeExternalParties();
                ViewData["LinkTypeData"] = GetLinkTypes(transactionCategory);
                ViewData["PrioritiesData"] = GetPriorities(transactionCategory);
                ViewData["AttachmentsTypeData"] = GetAttachmentTypes(transactionCategory);
                ViewData["TransactionTypesData"] = GetTransactionTypes(transactionCategory);
                ViewData["LetterTypeData"] = GetLetterTypes(transactionCategory);
                ViewData["ActionData"] = GetActions();
                ViewData["AssignmentGroupData"] = GetUserAssignmentGroups();
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["HasAssignmentPaper"] = CheckOrgUnitHasAssignmentPaper();
                ViewData["IsAllowedToCreateGroup"] = CheckOrgUnitIsAllowedToCreateGroup();
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(new List<TransactionAttachmentVM>());
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(new List<TransactionNameVM>());
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(new List<TransactionLinkVM>());
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(new List<TransactionCopyVM>());
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(new List<TransactionExternalCopyVM>());
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(new List<TransactionArchiveVM>());
                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(new List<AutoCompleteDataSource>());
                ViewData["SettingCity"] = GetCitySetting();

                ViewData["MainDocumentId"] = null;
                ViewData["DocumentId"] = null;
                ViewData["TransactionCategoryId"] = (int)transactionCategory;
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AllExternalActionsData"] = ViewData["AllActionsData"];
                ViewData["ExternalPartyAscDesc"] = TransactionHelper.GetByExternalPartyAscDesc();
                ViewData["SourceTypeAscDesc"] = TransactionHelper.GetBySourceTypeAscDesc();
                List<AutoCompleteDataSource> autoCompleteDataSources = new List<AutoCompleteDataSource>();
                if (!string.IsNullOrEmpty(ViewData["ActionData"].ToString()))
                {
                    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                    autoCompleteDataSources.AddRange(javaScriptSerializer.Deserialize(ViewData["ActionData"].ToString(), typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>);
                }
                ViewData["HasActions"] = autoCompleteDataSources.Count > 0;
                if (transactionCategory == TransactionCategory.Inbound || transactionCategory == TransactionCategory.InternalOutbound)
                {
                    ViewData["DeliveryMethod"] = GetDeliveryMethod(true);
                    ViewData["ReceiveMethod"] = GetDeliveryMethod(false);
                }
                ViewData["SessionArchiveMainDocumentKey"] = Guid.NewGuid().ToString();
                ViewData["SessionArchiveDocumentKey"] = Guid.NewGuid().ToString();
                ViewData["Reporters"] = GetReporters();
                Session["BarcodeImgByte"] = null;
                Session["DocumentData"] = null;
                Session["DocViewerData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                Session["DocoNutexplanations"] = null;
                ViewData["DistributionLists"] = GetDistributionLists();
                ViewData["TransactionPaths"] = GetTransactionPaths();
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult AddAttachment([Bind(Prefix = "TransactionAttachment")] TransactionAttachmentVM TransactionAttachmentVM,
            List<TransactionAttachmentVM> Attachments)
        {
            string message = string.Empty;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                string archiveData = string.Empty;

                List<TransactionAttachmentVM> attachmentVMs = new List<TransactionAttachmentVM>();
                Attachments = Attachments ?? new List<TransactionAttachmentVM>();
                if (!Attachments.Any(a => a.TypeName == TransactionAttachmentVM.TypeName))
                {
                    TransactionAttachmentVM.Key = Attachments.Count + 1;
                    attachmentVMs.Add(TransactionAttachmentVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.Attachment.AttachmentAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                if (Attachments != null)
                {
                    if (TransactionAttachmentVM.Archivable)
                    {
                        dataSource.Add(new AutoCompleteDataSource() { Value = TransactionAttachmentVM.TypeId.ToString(), Label = TransactionAttachmentVM.TypeName });
                    }
                    foreach (var item in Attachments)
                    {
                        if (item.Archivable)
                        {
                            dataSource.Add(new AutoCompleteDataSource() { Value = item.TypeId.ToString(), Label = item.TypeName });
                        }
                    }
                }

                archiveData = JsonConvert.SerializeObject(dataSource);
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AttachmentsGridPartial", (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(attachmentVMs, 1, attachmentVMs.Count, true)),
                    hdnArchive = archiveData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult AssignItBack(int TransId, string Notes)
        {
            string message = string.Empty;
            MessageType messageType = MessageType.Information;
            PostResult postResult = null;
            try
            {
                postResult = HttpClientWrapper<PostResult>
                                                 .PostRequest($"api/MobileApi/AssignItBack?TransId={TransId}&Notes={Notes}&userId={SessionInfo.CurrentUser.Id}&entityId={SessionInfo.OrgUnitId}", SessionInfo.CultureShortName)
                                                 .Result;


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult AssignTransactionBack(string TransId, string Notes)
        {
            int trxId = int.Parse(StringCipher.DecryptStringAES(TransId.Replace(" ", "+")));
            string message = string.Empty;
            MessageType messageType = MessageType.Information;
            PostResult postResult = null;
            try
            {
                postResult = HttpClientWrapper<PostResult>
                                                 .PostRequest($"api/MobileApi/AssignItBack?TransId={trxId}&Notes={Notes}&userId={SessionInfo.CurrentUser.Id}&entityId={SessionInfo.OrgUnitId}", SessionInfo.CultureShortName)
                                                 .Result;


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteAttachments(string ids, string hdnAttachments, string ArchiveAttachmentsData, string hdnArchiveData)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                List<TransactionAttachmentVM> attachments = new List<TransactionAttachmentVM>();
                List<TransactionArchiveVM> archiveDataGrid = new List<TransactionArchiveVM>();
                string autoCompleteArchiveData = ArchiveAttachmentsData;
                archiveDataGrid = javaScriptSerializer.Deserialize(hdnArchiveData, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;
                if (!string.IsNullOrEmpty(hdnAttachments))
                {
                    attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                    dataSource = javaScriptSerializer.Deserialize(ArchiveAttachmentsData, typeof(List<AutoCompleteDataSource>)) as List<AutoCompleteDataSource>;
                }
                attachments.Remove(attachments.Where(a => a.TypeId == Convert.ToInt32(ids)).FirstOrDefault());
                dataSource.Remove(dataSource.Where(d => d.Value == ids).FirstOrDefault());
                TransactionArchiveVM transactionArchiveVM = archiveDataGrid.Where(a => a.AttachmentTypeId != null)
                     .Where(a => a.AttachmentTypeId.Value == Convert.ToInt32(ids)).FirstOrDefault();
                if (transactionArchiveVM != null)
                {
                    archiveDataGrid.Remove(transactionArchiveVM);
                }
                string data = JsonConvert.SerializeObject(attachments);
                autoCompleteArchiveData = JsonConvert.SerializeObject(dataSource);
                string archiveData = JsonConvert.SerializeObject(archiveDataGrid);
                IAjaxGrid gridArchive = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(archiveDataGrid.Where(a => !a.IsDeleted).ToList(), 1, archiveDataGrid.Count, true);
                IAjaxGrid grid = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(attachments, 1, attachments.Count, true);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AttachmentsGridPartial", grid), hdnValue = data, hdnArchive = autoCompleteArchiveData, hdnArchiveData = archiveData, HtmlArchive = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ArchivingGridPartial", gridArchive) }, JsonRequestBehavior.AllowGet);
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

                //SohaibZ: allow save without document
                byte[] data = DocumentViewerHelper.GetPDFFile(param, false);
                if (data.Length <= 12397)
                {
                    _hasArchive = false;
                }

                return Json(new { hasArchive = _hasArchive.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult InboundNavigateTransactions(int CurrentId, bool IsNext)
        {
            try
            {
                Dictionary<int, int> Ids = Session["InboundNextPreviousIds"] != null ? Session["InboundNextPreviousIds"] as Dictionary<int, int> : new Dictionary<int, int>();
                int id = 0;
                int TransactionCategoryId = 0;
                int index = Ids.Keys != null ? Ids.Keys.ToList().IndexOf(CurrentId) : 0;
                string message = string.Empty;
                if (IsNext)
                {
                    if (index < Ids.Count - 1)
                    {
                        id = Ids.ElementAt(index + 1).Key;
                        TransactionCategoryId = Ids.ElementAt(index + 1).Value;
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Inbound.NextNavegation");
                        return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        id = Ids.ElementAt(index - 1).Key;
                        TransactionCategoryId = Ids.ElementAt(index - 1).Value;
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Inbound.PrevNavegation");
                        return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Id = id, TransactionCategoryId = TransactionCategoryId, Message = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        [HttpGet]
        public ActionResult OutBoundNavigateTransactions(int CurrentId, bool IsNext)
        {
            try
            {
                Dictionary<int, int> Ids = Session["OutBoundNextPreviousIds"] != null ? Session["OutBoundNextPreviousIds"] as Dictionary<int, int> : new Dictionary<int, int>();
                int id = 0;
                int? TransactionCategoryId = 0;
                int index = Ids.Keys != null ? Ids.Keys.ToList().IndexOf(CurrentId) : 0;
                string message = string.Empty;
                if (IsNext)
                {
                    if (index < Ids.Count - 1)
                    {
                        id = Ids.ElementAt(index + 1).Key;
                        TransactionCategoryId = Ids.ElementAt(index + 1).Value;
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Inbound.NextNavegation");
                        return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        id = Ids.ElementAt(index - 1).Key;
                        TransactionCategoryId = Ids.ElementAt(index - 1).Value;
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Inbound.PrevNavegation");
                        return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Id = id, TransactionCategoryId = TransactionCategoryId, Message = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


        [HttpGet]
        public ActionResult CopiesNavigateTransactions(int CurrentId, bool IsNext, int TransactionCategoryId)
        {
            try
            {

                Dictionary<int, int?> Ids = new Dictionary<int, int?>();
                if (TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    Ids = Session["InboundCopiesIds"] != null ? Session["InboundCopiesIds"] as Dictionary<int, int?> : new Dictionary<int, int?>();
                }
                else if (TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    Ids = Session["OutboundCopiesIds"] != null ? Session["OutboundCopiesIds"] as Dictionary<int, int?> : new Dictionary<int, int?>();
                }
                else if (TransactionCategoryId == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    Ids = Session["InternalCopiesIds"] != null ? Session["InternalCopiesIds"] as Dictionary<int, int?> : new Dictionary<int, int?>();
                }
                else
                {
                    Ids = Session["SpecialCopiesIds"] != null ? Session["SpecialCopiesIds"] as Dictionary<int, int?> : new Dictionary<int, int?>();
                }
                int id = 0;
                int? transactionTypeId = 0;
                int index = Ids.Keys != null ? Ids.Keys.ToList().IndexOf(CurrentId) : 0;
                string message = string.Empty;
                if (IsNext)
                {
                    if (index < Ids.Count - 1)
                    {
                        id = Ids.ElementAt(index + 1).Key;
                        transactionTypeId = Ids.ElementAt(index + 1).Value;
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Inbound.NextNavegation");
                        return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        id = Ids.ElementAt(index - 1).Key;
                        transactionTypeId = Ids.ElementAt(index - 1).Value;
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Inbound.PrevNavegation");
                        return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { Id = id, Type = transactionTypeId, Message = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }



        }
        [HttpPost]
        public ActionResult AddArchiveMainDocument(TransactionArchiveVM transactionArchiveVM, string hdnArchivigMainDocumentdata, string param, string hdnTransactionNumber, string hdnMainDocumentSessionKey)
        {
            try
            {
                string message = string.Empty;
                transactionArchiveVM.Id = Guid.NewGuid().ToString();
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, byte[]> documentData = new Dictionary<string, byte[]>();

                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigMainDocumentdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;

                byte[] data = DocumentViewerHelper.GetPDFFile(param);

                if (data.Length <= 12397)
                {
                    message = DbRes.TValidation("User.Inbound.MainDocument");
                    return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                if (documentData == null)
                {
                    documentData = new Dictionary<string, byte[]>();
                }
                bool checkDocument = true;
                bool newDocument = false;

                transactionArchiveVMs.ForEach(t =>
                {
                    if (!t.IsDeleted)
                    {
                        checkDocument = false;
                    }
                    if (t.IsDeleted)
                    {
                        newDocument = true;
                    }
                });

                if (newDocument)
                {
                    transactionArchiveVMs.Remove(transactionArchiveVMs.Where(a => a.IsMainDocument).FirstOrDefault());
                    documentData.Add(transactionArchiveVM.Id, data);
                    transactionArchiveVM.IsMainDocument = true;
                    transactionArchiveVM.AttachmentTypeId = -1;
                    transactionArchiveVM.ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text;
                    transactionArchiveVMs.Add(transactionArchiveVM);
                }
                else if (checkDocument)
                {
                    documentData.Add(transactionArchiveVM.Id, data);
                    transactionArchiveVM.IsMainDocument = true;
                    transactionArchiveVM.IsNew = true;
                    transactionArchiveVM.AttachmentTypeId = -1;
                    transactionArchiveVM.ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text;
                    transactionArchiveVMs.Add(transactionArchiveVM);
                }
                else
                {
                    message = DbRes.TResource("User.Transaction.Archive.AddMainDocErrorMsg");
                    return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                Session["DocumentData"] = documentData;
                Session[hdnMainDocumentSessionKey] = null;

                string dataGrid = JsonConvert.SerializeObject(transactionArchiveVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsDeleted == false).ToList(), 1, transactionArchiveVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ArchivingGridPartialMainDocument", grid), hdnValue = dataGrid, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddArchive([Bind(Prefix = "TransactionArchive")] TransactionArchiveVM transactionArchiveVM,
            List<TransactionArchiveVM> Archives, string param, string hdnTransactionNumber, string hdnSessionKey, string file)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                var TransactionArchiveVMList = new List<TransactionArchiveVM>();
                Archives = Archives ?? new List<TransactionArchiveVM>();
                transactionArchiveVM.Id = Guid.NewGuid().ToString();
                transactionArchiveVM.Key = Archives.Count + 1;
                Dictionary<string, byte[]> documentData = new Dictionary<string, byte[]>();
                documentData = Session["DocumentData"] as Dictionary<string, byte[]> ?? new Dictionary<string, byte[]>();
                string FilePrefix;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + transactionArchiveVM.AttachmentTypeId;
                }
                else
                {
                    FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + transactionArchiveVM.AttachmentTypeId;
                }

                if (file != string.Empty && transactionArchiveVM.AttachmentSource == (int)AttachmentSource.Uploaded)
                {
                    string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                    file = StringUtility.ValidateFileNames(file);
                    byte[] fileContent = System.IO.File.ReadAllBytes(StringUtility.ValidateFileNames($"{path}{file}"));
                    documentData.Add(transactionArchiveVM.Id, fileContent);
                    Session["DocumentData"] = documentData;
                }
                else
                {
                    if (documentData.Keys.Contains(transactionArchiveVM.Id))
                    {
                        byte[] data = DocumentViewerHelper.GetPDFFile(param);
                        if (transactionArchiveVM.Archivable)
                        {
                            documentData.Add(transactionArchiveVM.Id, data);
                            Session["DocumentData"] = documentData;
                        }
                    }
                    else
                    {
                        documentData.Add(transactionArchiveVM.Id, null);
                        Session["DocumentData"] = documentData;
                    }

                    //if (data.Length <= 12397 && transactionArchiveVM.Archivable)
                    //{
                    //    message = DbRes.TValidation("User.Inbound.IncludedItemDocument");
                    //    return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    //}


                }

                if (transactionArchiveVM.TransactionAttachmentType == TransactionAttachmentType.Attachment && transactionArchiveVM.AttachmentTypeId == null)
                {
                    message = DbRes.TResource("User.Transaction.Archive.EnterAttachmentTypeErrorMsg");
                    return Json(new { Message = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                transactionArchiveVM.DocumentId = 0;
                transactionArchiveVM.ReadOnly = false;
                transactionArchiveVM.IsNew = true;
                transactionArchiveVM.AttachmentTypeId = transactionArchiveVM.AttachmentTypeId;
                transactionArchiveVM.ArcivingTypeName = transactionArchiveVM.ArcivingTypeName;
                transactionArchiveVM.JFile = JsonConvert.SerializeObject(new { Id = 0, Name = file, IsDeleted = 0 });
                TransactionArchiveVMList.Add(transactionArchiveVM);

                Session[hdnSessionKey] = null;

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ArchivingGridPartial",
                    (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(TransactionArchiveVMList.Where(t => t.IsDeleted == false && !t.IsMainDocument).ToList(), 1, TransactionArchiveVMList.Count, true)),
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult DeleteArchiveMainDocument(string ids, string hdnArchivigMainDocumentdata, string hdnArchiveMainDocumentId, string hdnMainDocumentSessionKey)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigMainDocumentdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;

                transactionArchiveVMs.Where(t => t.Id == ids).FirstOrDefault().IsDeleted = true;

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                if (documentData != null)
                {
                    if (documentData.Keys.Contains(ids))
                    {
                        documentData.Remove(ids);
                    }
                }

                Session[hdnMainDocumentSessionKey] = null;

                Session["DocumentData"] = documentData;

                string data = JsonConvert.SerializeObject(transactionArchiveVMs);

                IAjaxGrid grid = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsDeleted == false).ToList(), 1, transactionArchiveVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ArchivingGridPartialMainDocument", grid), hdnValue = data, removeViewer = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public void DeleteArchive(string hdnSessionKey)
        {
            try
            {
                var documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                if (documentData != null)
                {
                    if (documentData.Keys.Contains(hdnSessionKey))
                    {
                        documentData.Remove(hdnSessionKey);
                    }
                    Session["DocumentData"] = documentData;
                }
                Session[hdnSessionKey] = null;
                Session["DocoNutIncDocument"] = null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public string ViewArchiveMainDocument(string id, string hdnArchivigMainDocumentdata, string hdnMainDocumentSessionKey)
        {
            try
            {
                Session["DocoNutDocument"] = null;

                id = StringUtility.ValidateId(id);
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(StringUtility.ValidateGridData(hdnArchivigMainDocumentdata), typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;
                if (documentData == null)
                {
                    documentData = new Dictionary<string, byte[]>();
                }
                if (documentData.Keys.Contains(id))
                {
                    Session[hdnMainDocumentSessionKey] = documentData.Where(k => k.Key == id).FirstOrDefault().Value;
                    Session["DocoNutDocument"] = documentData.Where(k => k.Key == id).FirstOrDefault().Value;
                }
                else
                {
                    int documentId = transactionArchiveVMs.Where(t => t.Id == id).FirstOrDefault().DocumentId;

                    GetResult<DocumentDTO> document =
                     HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?documentId={0}&cultureName={1}", documentId, SessionInfo.CultureShortName)).Result;
                    Session[hdnMainDocumentSessionKey] = DocumentMapper.Map(document.Result).Content;

                    documentData.Add(id, DocumentMapper.Map(document.Result).Content);
                    Session["DocumentData"] = documentData;
                    Session["DocoNutDocument"] = document.Result.Content;
                }

                return id;
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult ViewArchive(string key, int documentId)
        {
            try
            {

                Session["DocoNutIncDocument"] = null;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, byte[]> documentData = new Dictionary<string, byte[]>();
                documentData = Session["DocumentData"] as Dictionary<string, byte[]> ?? new Dictionary<string, byte[]>();
                if (documentData.Keys.Contains(key) && documentData[key] != null)
                {
                    Session["DocoNutIncDocument"] = documentData[key];
                }
                else if (documentId > 0)
                {
                    GetResult<DocumentDTO> document =
               HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?documentId={0}&cultureName={1}", documentId, SessionInfo.CultureShortName)).Result;

                    if (document.StatusCode != StatusCode.Ok)
                    {
                        string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, document.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    Session[key] = DocumentMapper.Map(document.Result).Content;
                    if (documentData.ContainsKey(key))
                    {
                        documentData.Remove(key);
                    }
                    documentData.Add(key, DocumentMapper.Map(document.Result).Content);
                    Session["DocumentData"] = documentData;
                    Session["DocoNutIncDocument"] = document.Result.Content;
                }
                else if (documentData.Keys.Contains(key))
                {
                    Session["DocoNutIncDocument"] = documentData[key];
                }
                else
                {


                    Session[key] = null;
                    Session["DocoNutIncDocument"] = null;
                }


                return Json(new { MessageType = MessageType.Information, key }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult VIPViewArchive(string key, int documentId)
        {
            try
            {

                Session["DocoNutIncDocument"] = null;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, byte[]> documentData = new Dictionary<string, byte[]>();
                documentData = Session["DocumentData"] as Dictionary<string, byte[]> ?? new Dictionary<string, byte[]>();
                if (documentId > 0 && !documentData.ContainsKey(key))
                {
                    GetResult<DocumentDTO> document =
               HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Document/GetDocumentById?documentId={0}&cultureName={1}", documentId, SessionInfo.CultureShortName)).Result;

                    if (document.StatusCode != StatusCode.Ok)
                    {
                        string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, document.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    Session[key] = DocumentMapper.Map(document.Result).Content;
                    if (documentData.ContainsKey(key))
                    {
                        documentData.Remove(key);
                    }
                    documentData.Add(key, DocumentMapper.Map(document.Result).Content);
                    Session["DocumentData"] = documentData;
                    Session["DocoNutIncDocument"] = document.Result.Content;
                }
                else if (documentData.Keys.Contains(key))
                {
                    Session["DocoNutIncDocument"] = documentData[key];
                }
                else
                {


                    Session[key] = null;
                    Session["DocoNutIncDocument"] = null;
                }


                return Json(new { MessageType = MessageType.Information, key }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        public void UpdateArchiveMainDocument(string hdnArchiveMainDocumentId, string param, string hdnMainDocumentSessionKey)
        {
            try
            {

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                byte[] data = DocumentViewerHelper.GetPDFFile(param);

                if (Session["DocumentData"] != null)
                {
                    Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                    if (documentData.Keys.Contains(hdnArchiveMainDocumentId))
                    {

                        documentData[hdnArchiveMainDocumentId] = data;
                        Session["DocumentData"] = documentData;
                        Session[hdnMainDocumentSessionKey] = null;
                        Session["DocoNutDocument"] = null;
                    }
                }
                Session["DocoNutDocument"] = null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateArchive([Bind(Prefix = "TransactionArchive")] TransactionArchiveVM transactionArchiveVM,
            List<TransactionArchiveVM> Archives, string hdnArchiveId, string param, int key, string file, AttachmentSource ArchiveingType)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                Dictionary<string, byte[]> documentData = new Dictionary<string, byte[]>();
                documentData = Session["DocumentData"] as Dictionary<string, byte[]> ?? new Dictionary<string, byte[]>();
                string FilePrefix;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + transactionArchiveVM.AttachmentTypeId + "_";
                }
                else
                {
                    FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + transactionArchiveVM.AttachmentTypeId + "_";
                }

                if (file != string.Empty && transactionArchiveVM.AttachmentSource == (int)AttachmentSource.Uploaded)
                {
                    string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                    file = StringUtility.ValidateFileNames(file);
                    if (System.IO.File.Exists(path + file))
                    {

                        string fileExtenstion = GetMimeType(path + file);

                        byte[] fileContent = System.IO.File.ReadAllBytes(StringUtility.ValidateFileNames($"{path}{file}"));


                        documentData[transactionArchiveVM.Id] = fileContent;


                        Session["DocumentData"] = documentData;

                        Session[transactionArchiveVM.Id] = fileContent;
                        Session[transactionArchiveVM.Id + "MimeType"] = fileExtenstion;

                    }


                }
                else
                {

                    byte[] data = DocumentViewerHelper.GetPDFFile(param);

                    documentData[hdnArchiveId] = data;
                    Session["DocumentData"] = documentData;
                    Session[hdnArchiveId] = null;
                    Session["DocoNutIncDocument"] = null;



                }




                List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();
                transactionArchiveVMs.Add(transactionArchiveVM);

                return Json(new
                {
                    MessageType = MessageType.Information,
                    Key = key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_ArchivingGridPartial.cshtml", (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs, 1, transactionArchiveVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public void CancelUpdate(string hdnSessionKey)
        {
            try
            {
                Session[hdnSessionKey] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult EditCopies()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditExternalCopies()
        {
            return View();
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Names.Add)]
        public ActionResult AddName([Bind(Prefix = "TransactionName")] TransactionNameVM TransactionNameVM, List<TransactionNameVM> Names)
        {
            string message = string.Empty;
            try
            {
                List<TransactionNameVM> nameVMs = new List<TransactionNameVM>();
                Names = Names ?? new List<TransactionNameVM>();
                if (!Names.Any(copy => copy.CivilID == TransactionNameVM.CivilID))
                {
                    TransactionNameVM.Key = Names.Count + 1;
                    TransactionNameVM.MobileNumber = TransactionNameVM.Phone;
                    nameVMs.Add(TransactionNameVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.Name.CivilIdDefinedErrorMsg");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesGridPartial", (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(nameVMs, 1, nameVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Names.Edit)]
        public ActionResult EditName([Bind(Prefix = "TransactionName")] TransactionNameVM TransactionNameVM, List<TransactionNameVM> Names)
        {
            string message = string.Empty;
            try
            {
                List<TransactionNameVM> TransactionNameVMs = new List<TransactionNameVM>();
                if (!Names.Any(n => n.CivilID == TransactionNameVM.CivilID && n.Key != TransactionNameVM.Key))
                {
                    TransactionNameVMs.Add(TransactionNameVM);
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.Name.CivilIdDefinedErrorMsg");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = TransactionNameVM.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesGridPartial", (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionNameVMs, 1, TransactionNameVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
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
                List<TransactionNameVM> nameVMs = new List<TransactionNameVM>();
                if (!string.IsNullOrEmpty(param))
                {
                    object objects = javaScriptSerializer.Deserialize(param, typeof(object[]));
                    List<object> list = ((object[])objects).ToList();
                    objects = list.ToArray<object>();
                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionNameVM)
                        {
                            nameVMs.Add(o as TransactionNameVM);
                        }
                        else
                        {
                            TransactionNameVM nameDTO =
                                javaScriptSerializer.Deserialize<TransactionNameVM>(javaScriptSerializer.Serialize(o));
                            nameVMs.Add(nameDTO);
                        }
                    });
                }
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(nameVMs, page.HasValue ? page.Value : 1, nameVMs.Count, page.HasValue);
                return Json(new { Html = grid.ToJson("_NamesGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Names.Delete)]
        public ActionResult DeleteNames(string ids, string hdnNames)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionNameVM> nameVMs = new List<TransactionNameVM>();
                if (!string.IsNullOrEmpty(hdnNames))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnNames, typeof(object[]));
                    List<object> list = ((object[])objects).ToList();
                    objects = list.ToArray<object>();
                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionNameVM)
                        {
                            nameVMs.Add(o as TransactionNameVM);
                        }
                        else
                        {
                            TransactionNameVM NameVM =
                                javaScriptSerializer.Deserialize<TransactionNameVM>(javaScriptSerializer.Serialize(o));
                            nameVMs.Add(NameVM);
                        }
                    });
                }
                List<string> civilIds = ids.Split(',').ToList();
                civilIds.ForEach(id =>
                {
                    TransactionNameVM remove = nameVMs.Where(n => n.CivilID == id).FirstOrDefault();
                    nameVMs.Remove(remove);
                });
                string data = JsonConvert.SerializeObject(nameVMs);
                IAjaxGrid grid = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(nameVMs, 1, nameVMs.Count, true);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Links.Add)]
        public ActionResult AddLink([Bind(Prefix = "TransactionLink")] TransactionLinkVM TransactionLinkVM,
            List<TransactionLinkVM> Links, string transactionId)
        {
            try
            {
                bool? isOutboundInternal = (bool?)TempData["IsOutboundInternal"];

                if (isOutboundInternal.HasValue && isOutboundInternal.Value)
                {
                    TransactionLinkVM.TransactionCategory = 256;
                    TransactionLinkVM.TransactionCategoryName = "معاملة داخلية";
                }

                string message = string.Empty;
                List<TransactionLinkVM> linkVMs = new List<TransactionLinkVM>();
                Links = Links ?? new List<TransactionLinkVM>();
                int nLinkTypeId = LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                if (TransactionLinkVM.LinkTypeId == LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName))
                {
                    switch (TransactionLinkVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                    {
                        case (int)TransactionCategory.Inbound:
                            {
                                if (TransactionLinkVM.WithDocumentNumber.HasValue && TransactionLinkVM.WithDocumentNumber.Value)
                                    nLinkTypeId = LinkingType.WithInboundDocumentNumber.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                                else
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
                    switch (TransactionLinkVM.TransactionCategory.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                    {
                        case (int)TransactionCategory.Inbound:
                            {
                                if (TransactionLinkVM.WithDocumentNumber.HasValue && TransactionLinkVM.WithDocumentNumber.Value)
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
                    TransactionLinkVM.OrgUnitId = -1;
                }
                else
                {
                    TransactionLinkVM.OrgUnitId = SessionInfo.OrgUnitId;
                }
                var transaction =
                HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest(
                    string.Format("api/Transaction/GetTransactionIdByLinkType?sourceNumber={0}&orgUnitId={1}&yearId={2}&linkTypeId={3}&cultureName={4}&yearSearch={5}",
                    TransactionLinkVM.TransactionNumber, TransactionLinkVM.OrgUnitId, TransactionLinkVM.Year, nLinkTypeId, SessionInfo.CultureShortName, TransactionLinkVM.YearSearch)).Result;

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
                    TransactionLinkVM.Key = Links.Count + 1;
                    TransactionLinkVM.DateH = transaction.Result.HijriDate;
                    TransactionLinkVM.Date = transaction.Result.Date.ToShortDateString();
                    TransactionLinkVM.TransactionType = transaction.Result.TransactionsTypes;
                    TransactionLinkVM.TransactionId = transaction.Result.Id;
                    TransactionLinkVM.Subject = transaction.Result.Subject;


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
                        TransactionLinkVM.Subject = "* * * *";
                        message = DbRes.TResource("PermissionAssignTo");
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }


                    TransactionLinkVM.YearDesc = transaction.Result.Year;
                    linkVMs.Add(TransactionLinkVM);
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinksGridPartial", (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(linkVMs, 1, linkVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Links.Edit)]

        [CustomAuthorizationAttribute(UserClaims.Links.Edit)]
        public ActionResult EditLink([Bind(Prefix = "TransactionLink")] TransactionLinkVM transactionLink, List<TransactionLinkVM> Links, string transactionId)
        {
            string message = string.Empty;
            try
            {
                Links = Links ?? new List<TransactionLinkVM>();
                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();
                int nLinkTypeId = LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName);
                if (transactionLink.LinkTypeId == LinkingType.WithReplyInbound.LookupIdentity(LookupCategory.LinkingType, SessionInfo.CultureShortName))
                {
                    switch (transactionLink.TransactionCategory.LookupInternalID(LookupCategory.LinkingType, SessionInfo.CultureShortName))
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
                    switch (transactionLink.TransactionCategory.LookupInternalID(LookupCategory.LinkingType, SessionInfo.CultureShortName))
                    {
                        case (int)TransactionCategory.Inbound:
                            {
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
                if ((!Links.Any(l => l.TransactionNumber == transactionLink.TransactionNumber && l.TransactionCategory == transactionLink.TransactionCategory && l.Year == transactionLink.Year)))
                {
                    var transaction =
                    HttpClientWrapper<GetResult<TransactionDetailsDTO>>.GetItemRequest(
                        string.Format("api/Transaction/GetTransactionIdByLinkType?sourceNumber={0}&orgUnitId={1}&yearId={2}&linkTypeId={3}&cultureName={4}",
                        transactionLink.TransactionNumber, transactionLink.OrgUnitId, transactionLink.Year, nLinkTypeId, SessionInfo.CultureShortName)).Result;

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
                        transactionLink.DateH = transaction.Result.HijriDate;
                        transactionLink.Date = transaction.Result.Date.ToShortDateString();
                        transactionLink.TransactionType = transaction.Result.TransactionsTypes;
                        transactionLink.TransactionId = transaction.Result.Id;
                        transactionLink.Subject = transaction.Result.Subject;
                        transactionLink.YearDesc = transaction.Result.Year;
                        transactionLinkVMs.Add(transactionLink);
                    }
                }
                else
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.TransactionDoubleLinked.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = transactionLink.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinksGridPartial", (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs, 1, transactionLinkVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        public ActionResult UpdateGridLinks(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();
                if (!string.IsNullOrEmpty(param))
                {
                    object objects = javaScriptSerializer.Deserialize(param, typeof(object[]));
                    List<object> list = ((object[])objects).ToList();
                    objects = list.ToArray<object>();
                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionNameVM)
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
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs, page.HasValue ? page.Value : 1, transactionLinkVMs.Count, page.HasValue);
                return Json(new { Html = grid.ToJson("_LinksGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Links.Delete)]
        public ActionResult DeleteLinks(string ids, string hdnLinks)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionLinkVM> transactionLinkVMs = new List<TransactionLinkVM>();
                if (!string.IsNullOrEmpty(hdnLinks))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnLinks, typeof(object[]));
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
                IAjaxGrid grid = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs, 1, transactionLinkVMs.Count, true);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_LinksGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public string GetUsersByOrgUnitIdInbound(int? id, bool addSelectOption = false)
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
                    foreach (UserProfileVM userProfileVM in UserProfileMapper.Map(userProfileDTOs.Result))
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
                    foreach (UserProfileVM userProfileVM in UserProfileMapper.Map(userProfileDTOs.Result))
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
        public string GetCitySetting()
        {
            try
            {
                GetResult<List<CityDTO>> getResult =
                    HttpClientWrapper<GetResult<List<CityDTO>>>.GetItemRequest(string.Format("api/Admin/GetCities?PageIndex=1&PageSize={0}&CultureName={1}", -1, SessionInfo.CultureShortName)).Result;

                List<Areas.Admin.Models.Lookups.CityVM> cityVMs = Areas.Admin.Mappers.CityMapper.Map(getResult.Result);

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (getResult.Result != null)
                {
                    foreach (Areas.Admin.Models.Lookups.CityVM cityVM in cityVMs)
                    {
                        AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                        {
                            Value = cityVM.Id.ToString(),
                            Label = cityVM.Description.FirstOrDefault(a => a.CultureName == SessionInfo.CultureShortName).Text,
                            Parameters = new object[1]
                        };
                        autoCompleteDataSource.Parameters[0] = cityVM.CityId;
                        dataSource.Add(autoCompleteDataSource);
                    }
                }

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetOrgUnitsManagers()
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitsManagers?cultureName={0}", SessionInfo.CultureShortName)).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (userProfileDTOs.Result != null)
                {
                    foreach (UserProfileVM userProfileVM in UserProfileMapper.Map(userProfileDTOs.Result))
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
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintEncryptionCode)]
        public ActionResult Print()
        {
            try
            {
                PrintVM printVM = new PrintVM();
                printVM.BarCode = true;
                printVM.DelevaryReport = true;
                printVM.Ticket = true;
                return PartialView("~/Areas/User/Views/Shared/_PrintPartial.cshtml", printVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.PrintEncryptionCode)]
        public ActionResult Print(bool printBarCodeOriginally, bool printBarCodeCopies, bool printBarCodeAttachments, bool printDelevaryReport, bool printTicket)
        {
            try
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetPriorities(TransactionCategory transactionCategory)
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
        protected List<PriorityVM> GetPrioritiesList(TransactionCategory transactionCategory)
        {
            try
            {
                GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(transactionCategory);

                return priorityVMs.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public string GetDeliveryMethod(bool isYesseRregistered)
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
        public string GetDeliveryMethodForYesser(bool isYesseRregistered)
        {
            try
            {
                int[] yesserRegistered = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] notYesserRegistered = { DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

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
        public string GetDelivery(bool? isPaper = null)
        {
            try
            {
                int[] ContainPaper = { DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] elctronic = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isPaper.HasValue)
                    {
                        if (isPaper.Value == true)
                        {
                            lookups.Result = lookups.Result.Where(a => ContainPaper.Contains(a.Id)).ToList();
                        }
                        else
                        {
                            lookups.Result = lookups.Result.Where(a => elctronic.Contains(a.Id)).ToList();
                        }

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
        protected string GetAttachmentTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<AttachmentTypeDTO>> attachmentTypeDTOs =
                    HttpClientWrapper<GetResult<List<AttachmentTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetAttachmentTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (attachmentTypeDTOs.Result != null)
                {
                    foreach (AttachmentTypeVM attachmentTypeVM in AttachmentTypeMapper.Map(attachmentTypeDTOs.Result))
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


        protected string GetConfidentialityAcknowledgments(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<ConfidentialityAcknowledgmentsDTO>> confidentialityAcknowledgmentsDTOs =
                    HttpClientWrapper<GetResult<List<ConfidentialityAcknowledgmentsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetConfidentialityAcknowledgments?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (confidentialityAcknowledgmentsDTOs.Result != null)
                {
                    foreach (ConfidentialityAcknowledgmentsVM confidentialityAcknowledgmentsVM in ConfidentialityAcknowledgmentsMapper.Map(confidentialityAcknowledgmentsDTOs.Result))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = confidentialityAcknowledgmentsVM.Id.ToString(),
                            Label = confidentialityAcknowledgmentsVM.LocalName,
                            Parameters = new object[] { confidentialityAcknowledgmentsVM.IsMandatary }
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
        protected string GetTransactionTypes(TransactionCategory transactionCategory)
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
        protected string GetLinkTypes(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<LinkDTO>> linkDTOs =
                    HttpClientWrapper<GetResult<List<LinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLinkTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (linkDTOs.Result != null)
                {
                    foreach (LinkVM linkVM in LinkMapper.Map(linkDTOs.Result))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = linkVM.Id.ToString(),
                            Label = linkVM.LocalName
                        });
                        //Set first value as defualt value
                        ViewData["SelectLinkTypeId"] = dataSource[0].Value;
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);
                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (permissionDTOs.Result != null)
                {
                    foreach (PermissionVM permissionVM in PermissionMapper.Map(permissionDTOs.Result))
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
        [HttpPost]
        //[ValidateAntiForgeryToken()]
        public ActionResult GetTransactionName(string civilId)
        {
            try
            {
                GetResult<TransactionNameDTO> transactionNameDTO =
                     HttpClientWrapper<GetResult<TransactionNameDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionName?cultureName={0}&civilID={1}", SessionInfo.CultureShortName, civilId)).Result;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                if (transactionNameDTO.Result != null)
                {
                    TransactionNameVM transactionNameVM = TransactionNameMapper.Map(transactionNameDTO.Result);
                    return Json(transactionNameVM);
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
                    foreach (ManagerVM manager in ManagerMapper.Map(managerDTOs.Result))
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
        public TreeViewModel BulidSubjectClassificationsTree(List<SubjectClassificationVM> subjectClassificationVMs)
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
        public TreeViewModel BulidSuggestedTopicsTree(List<SuggestedTopicVM> suggestedTopicVMs)
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
        #region Assignmnets
        protected string GetAssignmentGroups(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<LetterTypeDTO>> letterTypeDTOs =
                    HttpClientWrapper<GetResult<List<LetterTypeDTO>>>.GetItemRequest(string.Format("api/Transaction/GetLetterTypes?cultureName=" + SessionInfo.CultureShortName + "&transactionCategory={0}", transactionCategory)).Result;
                if (letterTypeDTOs.Result != null)
                {
                    foreach (LetterTypeVM letterTypeVM in LetterTypeMapper.Map(letterTypeDTOs.Result))
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
            if (actionDTOs.Result != null)
            {
                foreach (ActionVM actionVM in ActionMapper.Map(actionDTOs.Result))
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
            if (assignmentGroupDTOs.Result != null)
            {
                foreach (AssignmentGroupVM assignmentGroupVM in AssignmentGroupMapper.Map(assignmentGroupDTOs.Result))
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
        public ActionResult AssignmentGroupAdd(string txtId, string divContainer)
        {
            try
            {
                ViewData["divContainer"] = divContainer;
                ViewData["txtId"] = txtId;
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                IAjaxGrid grid = (AjaxGrid<AssignmentGroupDetailVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentGroupDetailVM>(), 1, 0, true);
                ViewData["AssignmentGroupDetailData"] = grid;
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
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
        public ActionResult AddAssignmentIndividual([Bind(Prefix = "EditorAssignment")] TransactionAssignmentVM transactionAssignmentVM, string hdnAssignmentIndividualData)
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
        [HttpPost]
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
                //       ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), transactionAssignmentVM.ToOrgUnitId);
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
                transactionAssignmentVMs[hdnIndexIndividual] = transactionAssignmentVM;
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
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, page.HasValue ? page.Value : 1, transactionAssignmentVMs.Count, true);
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
                    if (assignmentGroupDTO.Result != null)
                    {
                        foreach (AssignmentGroupDetailVM assignmentGroupDetailVM in AssignmentGroupMapper.Map(assignmentGroupDTO.Result).GroupDetails)
                        {
                            TransactionAssignmentVM groupDetails = new TransactionAssignmentVM()
                            {
                                Id = assignmentGroupDetailVM.Id,
                                GroupId = assignmentGroupDTO.Result.Id,
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
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(transactionAssignmentVMs, page.HasValue ? page.Value : 1, transactionAssignmentVMs.Count, true);
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
        public ActionResult SendAssignments(string hdnAssignmentIndividualData, string hdnDetailAssignmentGroupData, string hdnTransactionId)
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
                //TODO:Get orgUnitId from session
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

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostTransactionAssignments?cultureName={0}&transactionId={1}", SessionInfo.CultureShortName, hdnTransactionId), transactionAssignmentVMs).Result;

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
                return Json(new { MessageText = message, MessageType = MessageType.Information, url = url, PrintDeliveryReport = printDeliveryReport, OneDeliveryReport = oneDeliveryReport, TransactionReportInfo = javaScriptSerializer.Serialize(postResult.Result) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Assignments.Assign)]
        [ValidateAntiForgeryToken()]
        public ActionResult SendAssignment([Bind(Prefix = "EditorAssignment")] TransactionAssignmentVM transactionAssignmentVM, string hdnTransactionId, bool isConfirmed)
        {
            try
            {
                if (transactionAssignmentVM.ToUserId == -1)
                {
                    transactionAssignmentVM.ToUserId = null;
                }
                string message = string.Empty;
                transactionAssignmentVM.DeliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                transactionAssignmentVM.FromOrgUnitId = SessionInfo.OrgUnitId;
                transactionAssignmentVM.FromUserName = SessionInfo.CurrentUser.Name;
                transactionAssignmentVM.IsAssigned = false;
                List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();

                transactionAssignmentVMs.Add(transactionAssignmentVM);
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

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


                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransactions?sTransactionsIds={0}", hdnTransactionId.Trim(',')), TransactionAssignmentMapper.Map(transactionAssignmentVMs)).Result;
                PutResult UpdateDelivary = HttpClientWrapper<PutResult>
                                           .PutRequest(string.Format("api/Transaction/UpdateTransactionsDelivary?transactionIds={0}&DeliveryMethodId={1}", hdnTransactionId.Trim(','), transactionAssignmentVM.DeliveryMethodId), null).Result;


                if (!string.IsNullOrEmpty(transactionAssignmentVM.Remarks))
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
              hdnTransactionId), ExplanationMapper.Map(explanationVM)).Result;

                    if (postExpinationResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postExpinationResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                }
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");

                bool printDeliveryReport = false;
                bool oneDeliveryReport = false;


                return Json(new { MessageText = message, MessageType = MessageType.Information, url = url, PrintDeliveryReport = printDeliveryReport, OneDeliveryReport = oneDeliveryReport, TransactionReportInfo = javaScriptSerializer.Serialize(postResult.Result) }, JsonRequestBehavior.AllowGet);
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
        #region ExternalParties
        [HttpPost]
        public ActionResult AddNewExternalParty(string treeName, string onAddPartyfuntion, int transactionId = 0)
        {
            try
            {
                ViewData["TreeName"] = treeName;
                ViewData["TransactionId"] = transactionId;
                ViewData["OnAddPartyfuntion"] = onAddPartyfuntion;
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                string numberStartWithCustomizeValue = "9999"; // هذه القيمه تخص الإدارات التي تمت اضافتها من خلال انشاء معاملة وارد وليس لها ادارة رئيسية 
                GetResult<string> maxNumber =
                   HttpClientWrapper<GetResult<string>>.GetItemRequest(String.Format("api/Common/GetLastNumberByCustomizeValue?numberStartWithCustomizeValue={0}", numberStartWithCustomizeValue)).Result;
                List<ExternalPartyListTypeVM> PartyTypes = GetExternalPartyListTypeLookups();
                List<ManagerVM> managers = new List<ManagerVM>();
                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(managers, 1, managers.Count, false);
                ViewData["GridData"] = grid;
                ExternalPartyViewModel externalPartyViewModel = new ExternalPartyViewModel();
                externalPartyViewModel.AddExternalParty.Types = PartyTypes;
                externalPartyViewModel.EditExternalParty.Types = PartyTypes;
                externalPartyViewModel.AddExternalParty.PartyNumber = maxNumber.Result;
                externalPartyViewModel.Tree = GetAllTreeData();
                string htmlView = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/Index.cshtml", externalPartyViewModel);
                return Json(new { View = htmlView }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult AddExternalParty(ExternalPartyAddVM externalPartyAddVM)
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
                if (externalPartyAddVM.ParentId != null)
                {
                    if (externalPartyAddVM.ParentId.Value == 0)
                    {
                        externalPartyAddVM.ParentId = null;
                    }
                }



                externalPartyAddVM.Name[0].Text = externalPartyAddVM.NameAr;
                if (!externalPartyAddVM.NameEn.IsNullOrEmpty())
                {
                    externalPartyAddVM.Name[1].Text = externalPartyAddVM.NameEn;
                }
                else
                {
                    externalPartyAddVM.Name[1].Text = "NA";
                }



                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PostParty", externalPartyAddVM).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.AddSucceeded");
                //TreeViewModel tree = GetAllTreeData(postResult.Id, null);
                //TreeNode nodeData = tree.Nodes.Where(n => n.Key == postResult.Id).FirstOrDefault().Value;

                return Json(new
                {
                    Html =
                    UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/_ExternalPartiesTreePartial.cshtml", GetAllTreeData()),
                    MessageText = message,
                    MessageType = MessageType.Information,
                    ParentId = externalPartyAddVM.ParentId,
                    Name = externalPartyAddVM.Name.FirstOrDefault(a => a.CultureName == SessionInfo.CultureShortName).Text,
                    Id = postResult.Id
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetExternalPartyChilds(int? id)
        {
            try
            {
                return PartialView("_ExternalPartyChildTreeViewPartial", GetTreeData(id));

            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetRoot()
        {
            try
            {
                return PartialView("_ExternalPartyRootTreeViewPartial", GetTreeData());
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string GetSearch(int? id)
        {
            StringBuilder result = new StringBuilder();
            string filter = string.Empty;

            result.Append("CultureName=").Append(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);

            string columnName = "ParentId";

            result.Append("&Filters[").Append(0).Append("].ColumnName=")
                  .Append(columnName).Append("&Filters[").Append(0)
                  .Append("].Type=").Append(FilterType.Equals).Append("&Filters[")
                  .Append(0).Append("].Value=").Append(id);

            return result.ToString();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditExternalParty(ExternalPartyEditVM externalPartyEditVM)
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
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Common/PutParty?cultureName=" + SessionInfo.CultureShortName, externalPartyEditVM).Result;
                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                //render tree 
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.UpdateSucceeded");
                TreeViewModel tree = GetTreeData(externalPartyEditVM.Id);
                string name = externalPartyEditVM.Name.Where(l => l.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/_ExternalPartiesTreePartial.cshtml", tree), MessageText = message, MessageType = MessageType.Information, Name = name, partyID = externalPartyEditVM.Id, }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private List<ExternalPartyVM> exteralPartyList = new List<ExternalPartyVM>();
        [HttpGet]
        public ActionResult GetPartyById(int id)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                List<ExternalPartyListTypeVM> PartyTypes = GetExternalPartyListTypeLookups();
                GetResult<ExternalPartyEditDTO> partyEditDTO =
                   HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", id)).Result;

                ExternalPartyEditVM partyEditVM = ExternalPartyMapper.Map(partyEditDTO.Result);

                partyEditVM.Types = MergeExternalPartyListTypeLookups(ExternalPartyMapper.Map(partyEditDTO.Result).Types);

                GetResult<List<ManagerDTO>> managerDTOs =
                     HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", id, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result), 1, managerDTOs.RowsCount.Value, false);
                ManagersManagementViewModel model = new ManagersManagementViewModel();
                model.AddManager.PartyId = id;
                ViewData["GridData"] = grid;
                return Json(new
                {
                    EditHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/_ExternalPartiesEditPartial.cshtml", partyEditVM),
                    ManagersHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/ManagersManagement.cshtml", model),
                }, JsonRequestBehavior.AllowGet
                );

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult CheckPartyNumber(string Number, int partyId = -1)
        {
            try
            {
                GetResult<bool> partyEditDTO =
                   HttpClientWrapper<GetResult<bool>>.GetItemRequest(String.Format("api/Common/CheckPartyNumber?Number={0}&partyId={1}", Number, partyId)).Result;

                return Json(new { Exists = partyEditDTO.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddManager(ManagerAddVM managerAddVM)
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
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PostExternalPartyManager", managerAddVM).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<ManagerDTO>> managerDTOs =
                     HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", managerAddVM.PartyId, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                if (managerDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result), 1, managerDTOs.RowsCount.Value, false);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.AddSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/_ManagersManagementGridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditManager(ManagerEditVM managerEditVM)
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
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/PutExternalPartyManager", ManagerMapper.Map(managerEditVM)).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<ManagerDTO>> managerDTOs =
                 HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&PageIndex=1&PageSize={1}&cultureName={2}", ManagerMapper.Map(managerEditVM).PartyId, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                if (managerDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                IAjaxGrid grid = (AjaxGrid<ManagerVM>)new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result), 1, managerDTOs.RowsCount.Value, false);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.UpdateSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/_ManagersManagementGridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetManager(string id)
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
                int externalPartyManagerId = 0;
                if (!string.IsNullOrEmpty(id))
                {
                    externalPartyManagerId = Convert.ToInt32(id);
                }
                GetResult<ManagerEditDTO> managerEditDTO =
                   HttpClientWrapper<GetResult<ManagerEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalPartyManagerById?externalPartyManagerId={0}", externalPartyManagerId)).Result;
                if (managerEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, managerEditDTO.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.UpdateSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/ExternalParties/_ManagersManagementEditPartial.cshtml", ManagerMapper.Map(managerEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateManagersGrid(int? page, string param)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                string parameters = GridHelper.GetGridParameters();
                GetResult<List<ManagerDTO>> managerDTOs =
                   HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalPartyManagers?PartyId={0}&{1}&cultureName={2}", param, parameters, SessionInfo.CultureShortName)).Result;
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(ManagerMapper.Map(managerDTOs.Result), page.HasValue ? page.Value : 1, managerDTOs.RowsCount.Value, page.HasValue);
                return Json(new { Html = grid.ToJson("~/Areas/User/Views/ExternalParties/_ManagersManagementGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GenerateExternalPartyNumber(int? id)
        {
            try
            {
                List<ExternalPartyDTO> parties = new List<ExternalPartyDTO>();
                string result = string.Empty;
                string generatedNumber = string.Empty;
                string parentNumber = string.Empty;

                result = GetSearch(id);

                parties = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalPartiesByParentId?parentId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result.Result;

                GetResult<ExternalPartyEditDTO> parentPartyDTO = null;
                if (id.HasValue)
                    parentPartyDTO = HttpClientWrapper<GetResult<ExternalPartyEditDTO>>.GetItemRequest(String.Format("api/Common/GetExternalParty?id={0}", id)).Result;

                int digitNumber = (parties != null && parties.Count > 0) ? parties.Count.ToString().Length : 0;
                parentNumber = (parentPartyDTO != null && parentPartyDTO.Result != null) ? parentPartyDTO.Result.PartyNumber : string.Empty;
                string partyNumber = (parties != null && parties.Count > 0) ? (parties.Count + 1).ToString() : "1";

                generatedNumber = string.Format("{0}0{1}", parentNumber, partyNumber);

                return new JsonResult { Data = generatedNumber, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private List<ExternalPartyListTypeVM> GetExternalPartyListTypeLookups()
        {
            GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.PartyType, SessionInfo.CultureShortName);
            List<ExternalPartyListTypeVM> partyTypeListVMs = new List<ExternalPartyListTypeVM>();
            foreach (LookupVM lookupVM in lookups.Result)
            {
                partyTypeListVMs.Add(new ExternalPartyListTypeVM()
                {
                    Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                    Text = lookupVM.Text
                });
            }
            return partyTypeListVMs;
        }
        private List<ExternalPartyListTypeVM> MergeExternalPartyListTypeLookups(List<ExternalPartyListTypeVM> partyTypeListVMs)
        {
            List<ExternalPartyListTypeVM> localizePartyTypeListVMs = GetExternalPartyListTypeLookups();
            foreach (ExternalPartyListTypeVM PartyTypeListVM in partyTypeListVMs)
            {
                if (localizePartyTypeListVMs.Where(l => l.Id == PartyTypeListVM.Id &&
                    PartyTypeListVM.IsSelected == true).SingleOrDefault() != null)
                {
                    localizePartyTypeListVMs.Where(l => l.Id == PartyTypeListVM.Id &&
                        PartyTypeListVM.IsSelected == true).SingleOrDefault().IsSelected = true;
                }
            }
            return localizePartyTypeListVMs;
        }
        private TreeViewModel GetTreeData(int? id = null, int? parentId = null)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            List<ExternalPartyDTO> parties =
                HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                .GetItemRequest(String.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}",
                            SessionInfo.CultureShortName, parentId)).Result.Result;
            exteralPartyList = ExternalPartyMapper.Map(parties);
            if (parties != null && parties.Count != 0)
            {
                nodes = parties.Select(p => new TreeNode()
                {
                    Id = p.Id,
                    ParentId = p.ParentId.HasValue ? p.ParentId.Value : 0,
                    Name = p.LocalName,
                    HasChilds = p.HasChilds,
                }).ToList();
            }
            if (id != null && id != 0)
            {
                TreeNode node = nodes.Where(n => n.Id == id).FirstOrDefault();
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
            var tree = new TreeViewModel();
            tree.RootNode = new TreeNode { Id = 0, Name = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.ExternalParty"), Mode = tree.Mode };
            BuildTree(tree, nodes);
            return tree;
        }
        private TreeViewModel GetAllTreeData(int? id = null, int? parentId = null)
        {
            List<TreeNode> nodes = new List<TreeNode>();
            List<ExternalPartyDTO> parties =
                HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetAllExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, parentId)).Result.Result;
            exteralPartyList = ExternalPartyMapper.Map(parties);
            if (parties != null && parties.Count != 0)
            {
                nodes = parties.Select(p => new TreeNode()
                {
                    Id = p.Id,
                    ParentId = p.ParentId.HasValue ? p.ParentId.Value : 0,
                    Name = p.LocalName,
                    HasChilds = p.HasChilds,
                }).ToList();
            }
            if (id != null && id != 0)
            {
                TreeNode node = nodes.Where(n => n.Id == id).FirstOrDefault();
                if (node != null)
                {
                    node.IsSelected = true;
                }
            }
            var tree = new TreeViewModel();
            tree.RootNode = new TreeNode { Id = 0, Name = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.ExternalParty.ExternalParty"), Mode = tree.Mode };
            BuildTree(tree, nodes);
            return tree;
        }
        private void BuildTree(TreeViewModel tree, List<TreeNode> nodes)
        {
            TreeNode parent;
            tree.Nodes = nodes.Select(t => new TreeNode { Id = t.Id, IsSelected = t.IsSelected, ParentId = t.ParentId, Name = t.Name, Mode = tree.Mode, HasChilds = t.HasChilds })
                  .ToDictionary(t => t.Id);
            tree.Nodes.Add(tree.RootNode.Id, tree.RootNode);
            foreach (var node in tree.Nodes.Values)
            {
                if (tree.Nodes.TryGetValue(node.ParentId, out parent) && node.Id != node.ParentId)
                {
                    node.Parent = parent;
                    parent.Childs.Add(node);
                }
            }
        }
        #endregion ExternalParties
        #region LogTransactionAction
        [HttpPost]
        public ActionResult LogTransactionAction(AuditingActionCode auditingActionCode, int transactionId)
        {
            try
            {
                string message = string.Empty;
                PostResult postResult =
                 HttpClientWrapper<PostResult>.PostRequest(String.Format("api/Transaction/LogTransactionAction?auditingActionCode={0}&transactionId={1}", auditingActionCode, transactionId), null).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion LogTransactionAction
        public byte[] GetBarcodeImage(int transactionId, bool ignoreLogging = false)
        {
            GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
        HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}&ignoreLogging={3}", SessionInfo.CultureShortName, transactionId, SessionInfo.OrgUnitId, ignoreLogging)).Result;
            byte[] barcodeImg = null;
            if (transactionBarcodesDTOs.StatusCode != StatusCode.Ok)
            {
                return barcodeImg;
            }
            BarcodeVM barcode = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result).BarcodeVMs.Where(b => b.Type == BarcodePrintType.Transaction).FirstOrDefault();
            TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);
            if (barcode != null)
            {
                SharedController sharedController = new SharedController();
                sharedController.FillBarcodeDesign(transactionBarcodesVM.TransactionBarcodeHtmlDesign, barcode, transactionBarcodesVM, transactionBarcodesVM.TransactionDesignWidth, transactionBarcodesVM.TransactionDesignHeight);
                barcodeImg = barcode.Content;
            }
            return barcodeImg;
        }

        public byte[] GetBarcodeImageCustom(int transactionId, int width, int height, bool ignoreLogging = false)
        {
            GetResult<TransactionBarcodesDTO> transactionBarcodesDTOs =
        HttpClientWrapper<GetResult<TransactionBarcodesDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBarcodes?cultureName={0}&transactionId={1}&orgUnitId={2}&ignoreLogging={3}", SessionInfo.CultureShortName, transactionId, SessionInfo.OrgUnitId, ignoreLogging)).Result;
            byte[] barcodeImg = null;
            if (transactionBarcodesDTOs.StatusCode != StatusCode.Ok)
            {
                return barcodeImg;
            }
            BarcodeVM barcode = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result).BarcodeVMs.Where(b => b.Type == BarcodePrintType.Transaction).FirstOrDefault();
            TransactionBarcodesVM transactionBarcodesVM = TransactionBarcodesMapper.Map(transactionBarcodesDTOs.Result);
            if (barcode != null)
            {
                SharedController sharedController = new SharedController();
                sharedController.FillBarcodeDesign(transactionBarcodesVM.TransactionBarcodeHtmlDesign, barcode, transactionBarcodesVM, width, height);
                barcodeImg = barcode.Content;
            }
            return barcodeImg;
        }
        #region Copies
        [HttpGet]
        public string GetUnitUsers(int id)
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                var userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
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
        private readonly int count = 0;
        [HttpGet]
        public ActionResult GetOrgUnitUsers(int id)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                string message = string.Empty;
                TransactionCopyVM copyVMs = new TransactionCopyVM();
                OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(id, SessionInfo.CultureShortName);
                int orgUnitNomber = 0;
                orgUnitNomber = Convert.ToInt32(orgUnitDTO.Number);
                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    orgUnitNumber = orgUnitNomber,
                }, JsonRequestBehavior.AllowGet);
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

                return Json(new { Html = grid.ToJson("_CopiesGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesInternal.Add)]
        public ActionResult AddCopy([Bind(Prefix = "TransactionCopy")] TransactionCopyVM TransactionCopyVM, List<TransactionCopyVM> Copies)
        {
            try
            {
                string message = string.Empty;
                List<int> result = !string.IsNullOrWhiteSpace(TransactionCopyVM.OrgSelectedList) ? TransactionCopyVM.OrgSelectedList.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();

                ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();

                Copies = Copies ?? new List<TransactionCopyVM>();
                int copiesCount = Copies.Count;
                List<TransactionCopyVM> CopieVm = new List<TransactionCopyVM>();
                bool isHasNew = false;
                isHasNew = result.Any(x => !Copies.Any(copy => copy.OrgUnitId == x && (copy.UserId == -1 || copy.UserId == null)));
                if (!isHasNew)
                {
                    message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                foreach (var orgUnitId in result)
                {

                    TransactionCopyVM newTransactionCopyVM = new TransactionCopyVM
                    {
                        ActionId = 1,
                        ActionName = TransactionCopyVM.ActionName,
                        ActionTypeId = TransactionCopyVM.ActionTypeId,
                        OrgUnitId = orgUnitId,
                        FromUserId = -1,
                        UserId = -1
                    };

                    if (!Copies.Any(copy => copy.OrgUnitId == newTransactionCopyVM.OrgUnitId && copy.UserId == newTransactionCopyVM.UserId))
                    {

                        newTransactionCopyVM.Status = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, SessionInfo.CultureShortName);
                        newTransactionCopyVM.Id = 0;
                        newTransactionCopyVM.Key = copiesCount + 1;
                        if (newTransactionCopyVM.OrgUnitId > 0)
                        {
                            OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(newTransactionCopyVM.OrgUnitId, SessionInfo.CultureShortName);
                            newTransactionCopyVM.OrgUnitName = orgUnitDTO.Name;
                            newTransactionCopyVM.UserList = GetUsersByOrgUnitId(newTransactionCopyVM.OrgUnitId, true);

                        }
                        CopieVm.Add(newTransactionCopyVM);
                        copiesCount++;
                    }



                }

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopiesGridPartial", (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(CopieVm, 1, CopieVm.Count, true))
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
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopiesGridPartial", (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesInternal.Delete)]
        public ActionResult DeleteCopies(string ids, string hdnCopies)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TransactionCopyVM> copyVMs = new List<TransactionCopyVM>();

                if (!string.IsNullOrEmpty(hdnCopies))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnCopies, typeof(object[]));

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

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopiesGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
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
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopiesGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region ExternalCopies
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesExternal.Add)]
        public ActionResult AddExternalCopy([Bind(Prefix = "TransactionExtCopy")] TransactionExternalCopyVM ExternalCopyVM, List<TransactionExternalCopyVM> ExternalCopies)
        {
            try
            {
                string message = string.Empty;
                List<int> result = !string.IsNullOrWhiteSpace(ExternalCopyVM.ExternalOrgSelectedList) ? ExternalCopyVM.ExternalOrgSelectedList.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();
                List<TransactionExternalCopyVM> externalCopyVMs = new List<TransactionExternalCopyVM>();
                ExternalCopies = ExternalCopies ?? new List<TransactionExternalCopyVM>();
                int copiesCount = ExternalCopies.Count;
                bool isHasNew = false;
                isHasNew = result.Any(x => !ExternalCopies.Any(copy => copy.OrgUnitId == x && (copy.UserId == -1 || copy.UserId == null)));
                if (!isHasNew)
                {
                    message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }


                foreach (var orgUnitId in result)
                {

                    TransactionExternalCopyVM newTransactionCopyVM = new TransactionExternalCopyVM
                    {
                        ActionId = 1,
                        ActionName = ExternalCopyVM.ActionName,
                        ActionTypeId = ExternalCopyVM.ActionTypeId,
                        OrgUnitId = orgUnitId,
                        FromUserId = -1,
                        UserId = null,
                        Id = 0
                    };

                    if (!ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopyVM.OrgUnitId && (copy.UserId == ExternalCopyVM.UserId || (copy.UserId == null || copy.UserId == -1))))
                    {
                        if ((ExternalCopyVM.UserId != null && !ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopyVM.OrgUnitId && copy.UserId == null)) ||
                           (ExternalCopyVM.UserId == null && !ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopyVM.OrgUnitId && copy.UserId != null)))
                        {

                            newTransactionCopyVM.Id = 0;
                            newTransactionCopyVM.Key = copiesCount + 1;
                            if (newTransactionCopyVM.OrgUnitId > 0)
                            {
                                var orgUnitDTO = OrgHelper.GetExternalParty(newTransactionCopyVM.OrgUnitId);
                                newTransactionCopyVM.OrgUnitName = orgUnitDTO.Name.Where(x => x.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;


                            }
                            externalCopyVMs.Add(newTransactionCopyVM);
                            copiesCount++;
                        }

                    }

                    if (ExternalCopyVM.OrgUnitId > 0)
                    {
                        ExternalPartyDTO orgUnitDTO = OrgHelper.GetExternalParty(ExternalCopyVM.OrgUnitId);
                        ExternalCopyVM.OrgUnitName = orgUnitDTO.Name.Where(s => s.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;
                    }

                }
                IAjaxGrid grid = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(externalCopyVMs, 1, externalCopyVMs.Count, true);
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalCopiesGridPartial", (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(externalCopyVMs, 1, externalCopyVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }


        }
        [HttpPost]
        public ActionResult RemoveAttachemntPhysically(string attachmentsToDelete, int entityId)
        {
            string[] AttachmentName = attachmentsToDelete.TrimEnd(',').Split(',');
            string entity;
            if (entityId > 0)
            {
                entity = $"{StringUtility.ValidateId(entityId.ToString())}_";
            }
            else
            {
                entity = string.Empty;
            }

            string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath);
            foreach (var item in AttachmentName)
            {
                string FilePrefix;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + entity;
                }
                else
                {
                    FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + entity;
                }
                if (System.IO.File.Exists(Path.Combine(path, FilePrefix + StringUtility.ValidateFileNames(item))))
                {
                    System.IO.File.Delete(Path.Combine(path, FilePrefix + StringUtility.ValidateFileNames(item)));
                }
            }
            return Json(new
            {
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RemoveAllAttachemntsPhysically()
        {
            string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath);
            string FilePrefix;
            if (SystemConfigurations.MultiTenantEnabled)
            {
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_";
            }

            DirectoryInfo hdDirectoryInWhichToSearch = new DirectoryInfo(path);
            FileInfo[] filesInDir = hdDirectoryInWhichToSearch.GetFiles(FilePrefix + "*");

            foreach (FileInfo foundFile in filesInDir)
            {
                string fullName = foundFile.FullName;
                System.IO.File.Delete(Path.Combine(path, fullName));
            }

            //if (System.IO.File.Exists(Path.Combine(path, FilePrefix + StringUtility.ValidateFileNames(item))))
            //{
            //    System.IO.File.Delete(Path.Combine(path, FilePrefix + StringUtility.ValidateFileNames(item)));
            //}
            return Json(new
            {
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult DeleteExternalCopy(string OrgUnit)
        {
            string path = SystemConfigurations.ExternalCopiesAttachmentPath;
            var filteredByFilename = Directory.GetFiles(path).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith($"{StringUtility.ValidateId(OrgUnit)} _"));
            foreach (var item in filteredByFilename)
            {
                if (System.IO.File.Exists(path + item))
                {
                    System.IO.File.Delete(path + item);
                }
            }
            return Json(new
            {
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UploadExternalCopyAttachments(int EntityId)
        {
            string addedFilesJson = string.Empty;
            List<object> list = new List<object>();
            bool isValid = true;
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
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + EntityId + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + EntityId + "_";
            }
            int totalCount = Directory.GetFiles(SystemConfigurations.ExternalCopiesAttachmentPath).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith(FilePrefix)).Count();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (!IsValidMimeType(MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName)))
                {
                    isValid = false;
                    break;
                }
                file.SaveAs(SystemConfigurations.ExternalCopiesAttachmentPath + FilePrefix + file.FileName);
                list.Add(new { Id = totalCount++, Name = file.FileName, IsDeleted = 0 });
                //addedFilesJson += JsonConvert.SerializeObject(new { Id = 0, Name = file.FileName, IsDeleted = 0 });
            }

            addedFilesJson = JsonConvert.SerializeObject(list);
            if (isValid == false)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    MessageText = DbRes.TResource("Task.File.MimeType")
                });
            }

            return Json(new
            {
                AddedFilesJson = addedFilesJson,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadMainDocAttachments(int AttachmentTypeId)
        {
            string addedFilesJson = string.Empty;
            bool isValid = true;
            List<object> list = new List<object>();
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
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + AttachmentTypeId + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + AttachmentTypeId + "_";
            }
            int totalCount = Directory.GetFiles(SystemConfigurations.ExternalCopiesAttachmentPath).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith(FilePrefix)).Count();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (!IsValidMimeType(MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName)))
                {
                    isValid = false;
                    break;
                }
                file.SaveAs(SystemConfigurations.ExternalCopiesAttachmentPath + FilePrefix + file.FileName);
                list.Add(new { Id = totalCount++, Name = file.FileName, IsDeleted = 0, ExtensionFile = MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName) });
                // addedFilesJson += JsonConvert.SerializeObject(new { Id = totalCount++, AttachmentName = file.FileName, IsDeleted = 0 });
            }
            addedFilesJson = JsonConvert.SerializeObject(list);

            if (isValid == false)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    MessageText = DbRes.TResource("Task.File.MimeType")
                });
            }

            return Json(new
            {
                AddedFilesJson = addedFilesJson,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult UploadExplanationAttachments()
        {
            string addedFilesJson = string.Empty;
            bool isValid = true;
            List<object> list = new List<object>();
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
                FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
            }
            else
            {
                FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
            }
            int totalCount = Directory.GetFiles(SystemConfigurations.ExternalCopiesAttachmentPath).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith(FilePrefix)).Count();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                file = Request.Files[i];

                if (!IsValidMimeType(MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName)))
                {
                    isValid = false;
                    break;
                }
                file.SaveAs(SystemConfigurations.ExternalCopiesAttachmentPath + FilePrefix + file.FileName);
                list.Add(new { Id = totalCount++, Name = file.FileName, IsDeleted = 0, ExtensionFile = MimeMapping.GetMimeMapping(SystemConfigurations.ExternalCopiesAttachmentPath + file.FileName) });
                // addedFilesJson += JsonConvert.SerializeObject(new { Id = totalCount++, AttachmentName = file.FileName, IsDeleted = 0 });
            }
            addedFilesJson = JsonConvert.SerializeObject(list);

            if (isValid == false)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    MessageText = DbRes.TResource("Task.File.MimeType")
                });
            }

            return Json(new
            {
                AddedFilesJson = addedFilesJson,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RetriveExternalCopiesAttechment(string OrgUnitId, string id)
        {
            try
            {
                string path = SystemConfigurations.ExternalCopiesAttachmentPath;
                TransactionExternalCopyVM transactionExternalCopy = new TransactionExternalCopyVM();
                var filteredByFilename = Directory.GetFiles(path).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith($"{StringUtility.ValidateId(OrgUnitId)}_"));
                return Json(new
                {
                    filesNames = string.Join(",", filteredByFilename.ToArray()),
                    MessageType = MessageType.Information,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesExternal.Edit)]
        public ActionResult EditExternalCopy([Bind(Prefix = "TransactionExtCopy")] TransactionExternalCopyVM ExternalCopy, List<TransactionExternalCopyVM> ExternalCopies)
        {
            string message = string.Empty;
            try
            {
                List<TransactionExternalCopyVM> copyVMs = new List<TransactionExternalCopyVM>();
                if (!ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopy.OrgUnitId && copy.UserId == ExternalCopy.UserId && copy.Key != ExternalCopy.Key))
                {
                    if ((ExternalCopy.UserId != null && !ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopy.OrgUnitId && copy.UserId == null)) ||
                    (ExternalCopy.UserId == null && !ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopy.OrgUnitId && copy.UserId != null)))
                    {
                        copyVMs.Add(ExternalCopy);
                    }
                    else if (ExternalCopy.UserId != null && ExternalCopies.Any(copy => copy.OrgUnitId == ExternalCopy.OrgUnitId && copy.UserId == null))
                    {
                        copyVMs.Add(ExternalCopy);
                    }
                    else
                    {
                        int count = 0;
                        foreach (var item in ExternalCopies)
                        {
                            if (item.OrgUnitId == ExternalCopy.OrgUnitId)
                            {
                                count++;

                            }
                        }
                        if (count == 1)
                        {
                            copyVMs.Add(ExternalCopy);
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
                if (ExternalCopy.OrgUnitId > 0)
                {
                    ExternalPartyDTO orgUnitDTO = OrgHelper.GetExternalParty(ExternalCopy.OrgUnitId);
                    ExternalCopy.OrgUnitName = orgUnitDTO.Name.Where(s => s.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = ExternalCopy.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalCopiesGridPartial", (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(copyVMs, 1, copyVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult UpdateGridExternalCopies(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionExternalCopyVM> ExternalCopyVMs = new List<TransactionExternalCopyVM>();
                if (!string.IsNullOrEmpty(param))
                {
                    object objects = javaScriptSerializer.Deserialize(param, typeof(object[]));
                    List<object> list = ((object[])objects).ToList();
                    objects = list.ToArray<object>();
                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionNameVM)
                        {
                            ExternalCopyVMs.Add(o as TransactionExternalCopyVM);
                        }
                        else
                        {
                            TransactionExternalCopyVM ExternalCopyVM =
                                javaScriptSerializer.Deserialize<TransactionExternalCopyVM>(javaScriptSerializer.Serialize(o));
                            ExternalCopyVMs.Add(ExternalCopyVM);
                        }
                    });
                }
                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(ExternalCopyVMs, page.HasValue ? page.Value : 1, ExternalCopyVMs.Count, true);
                return Json(new { Html = grid.ToJson("_ExternalCopiesGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesExternal.DisplayLink)]
        public ActionResult GetExternalCopy(string ids, string hdnExternalCopiesGrid)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionExternalCopyVM> ExternalCopyVMs = new List<TransactionExternalCopyVM>();
                if (!string.IsNullOrEmpty(hdnExternalCopiesGrid))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnExternalCopiesGrid, typeof(object[]));
                    List<object> list = ((object[])objects).ToList();
                    objects = list.ToArray<object>();
                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionExternalCopyVM)
                        {
                            ExternalCopyVMs.Add(o as TransactionExternalCopyVM);
                        }
                        else
                        {
                            TransactionExternalCopyVM ExternalCopyVM =
                                javaScriptSerializer.Deserialize<TransactionExternalCopyVM>(javaScriptSerializer.Serialize(o));
                            ExternalCopyVMs.Add(ExternalCopyVM);
                        }
                    });
                }
                List<int> copyIds = ids.Split(',').Select(int.Parse).ToList();
                copyIds.ForEach(id =>
                {
                    TransactionExternalCopyVM edit = ExternalCopyVMs.Where(n => n.Key == id).FirstOrDefault();
                });
                string data = JsonConvert.SerializeObject(ExternalCopyVMs);
                IAjaxGrid grid = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(ExternalCopyVMs, 1, ExternalCopyVMs.Count, true);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalCopiesGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.CopiesExternal.Delete)]
        public ActionResult DeleteExternalCopies(string ids, string hdnExternalCopies, string AttachmentName)
        {
            try
            {
                AttachmentName = StringUtility.ValidateFileNames(AttachmentName);
                string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath);
                if (System.IO.File.Exists(path + AttachmentName))
                {
                    System.IO.File.Delete(path + AttachmentName);
                }
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionExternalCopyVM> ExternalCopyVMs = new List<TransactionExternalCopyVM>();

                if (!string.IsNullOrEmpty(hdnExternalCopies))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(object[]));

                    List<object> list = ((object[])objects).ToList();
                    objects = list.ToArray<object>();
                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is TransactionExternalCopyVM)
                        {
                            ExternalCopyVMs.Add(o as TransactionExternalCopyVM);
                        }
                        else
                        {
                            TransactionExternalCopyVM ExternalCopyVM = javaScriptSerializer.Deserialize<TransactionExternalCopyVM>(javaScriptSerializer.Serialize(o));
                            ExternalCopyVMs.Add(ExternalCopyVM);
                        }
                    });
                }
                List<int> copyIds = ids.Split(',').Select(int.Parse).ToList();
                copyIds.ForEach(id =>
                {
                    TransactionExternalCopyVM remove = ExternalCopyVMs.Where(n => n.Key == id).FirstOrDefault();
                    ExternalCopyVMs.Remove(remove);
                });
                string data = JsonConvert.SerializeObject(ExternalCopyVMs);
                IAjaxGrid grid = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(ExternalCopyVMs, 1, ExternalCopyVMs.Count, true);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalCopiesGridPartial", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Names.Delete)]
        public ActionResult DeleteTransactionNames(string ids, string hdnNames)
        {
            try
            {
                return DeleteNames(ids, hdnNames);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SendAssignmentPaper(List<TransactionAssignmentVM> TransactionAssignments,
            string TransactionId,
            string explanationTxt,
            string ConfedentialityId,
            int deliveryMethodId,
            int? reporterId,
            bool AddFollowUp,
            string FolloupDateTo,
            string FolloupDateH,
            int FollowUpProccess,
            int FollowUpPeriod,
            bool isConfirmed,
            string action = null

            )
        {
            string message = string.Empty;
            int? TransactionAssignmentExplanationId = 0;
            var _action = !string.IsNullOrEmpty(action) ? int.Parse(action) : 0;
            int? AssignEntityId = TransactionAssignments.Where(ta => ta.IsAssigned == true).FirstOrDefault()?.ToOrgUnitId;
            if (explanationTxt != "" && explanationTxt != null)
            {
                TransactionAssignments.ForEach(exp => exp.GeneralExplanation = explanationTxt);
            }
            if (!AssignEntityId.HasValue)
            {

                message = DbRes.TValidation("User.UserDelegation.OrgUnitRequired");
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

            }

            bool hasNormalAction = TransactionAssignments.FirstOrDefault(ta => ta.IsAssigned == true).ActionId != 0;

            deliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
            foreach (var item in TransactionAssignments)
            {
                if (item.IsAssigned)
                {
                    if (item.ActionId == 0 && _action == 0)
                    {
                        message = "اختر الادارة المراد الاحالة لها";
                        break;
                    }
                }
            }
            if (!hasNormalAction)
            {
                TransactionAssignments.Where(ta => ta.IsAssigned == true).FirstOrDefault().ActionId = int.Parse(action);
            }

            if (hasNormalAction && _action != 0)
            {
                message = "اختر ألأجراء";
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
                assignmentPaperBeneficiaryVM.ChkConstant = item.IsCopy;

                assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiaryVM);
            }

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs);

            //PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", TransactionId.Trim(',')), TransactionAssignmentMapper.Map(TransactionAssignments.Where(ta => ta.IsAssigned == true).ToList())).Result;
            //bool hasPermission = SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.AssignTransactionToUnauthorize);

            //if (postResult.StatusCode != StatusCode.Ok && !isConfirmed)
            //{
            //    if (postResult.StatusCode == StatusCode.NotSupported)
            //    {
            //        string Statuskey = hasPermission ? StatusCode.WarningNoPermissionToReceiveTransaction.ToString() : StatusCode.ErrorNoPermissionToReceiveTransaction.ToString();
            //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, Statuskey);
            //        return Json(new { MessageText = message, MessageType = (hasPermission ? MessageType.Warning : MessageType.Error), isNeedConfimed = hasPermission }, JsonRequestBehavior.AllowGet);
            //    }

            //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
            //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            //}


            //PostResult postResultassignmentPaper = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateAssignmentPaper?userId=" + SessionInfo.CurrentUser.Id, assignmentPaperDTO).Result;

            PostResult postResult = HttpClientWrapper<PostResult>
                .PostRequest(string.Format("api/Transaction/PostAssignTransaction?transactionId={0}", TransactionId),
                        TransactionAssignmentMapper.Map(TransactionAssignments.Where(ta => ta.IsAssigned == true).ToList()))
                        .Result;


            PutResult UpdateDelivary = HttpClientWrapper<PutResult>
                              .PutRequest(string.Format("api/Transaction/UpdateTransactionDelivary?transactionId={0}&DeliveryMethodId={1}",
                              TransactionId, deliveryMethodId), null).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            #region followup
            if (AddFollowUp)
            {
                TransactionFollowUpVM followVM = new TransactionFollowUpVM();
                followVM.FollowUpStatusId = (int)FollowupStatus.New;
                followVM.CreationDate = DateTime.Now;
                followVM.Active = true;
                followVM.CreatingUserId = SessionInfo.CurrentUser.Id;
                followVM.CreatingEntityId = SessionInfo.OrgUnitId;
                followVM.FollowUpProccessId = FollowUpProccess;
                followVM.FollowUpTypeId = 2;
                followVM.TransactionId = Convert.ToInt32(TransactionId);
                PostResult postResultFodept =
                       HttpClientWrapper<PostResult>
                       .PostRequest(string.Format("api/Transaction/getFollowUpDepartment?EntityId={0}", SessionInfo.OrgUnitId), null).Result;
                if (postResultFodept.Id.HasValue)
                {
                    followVM.FollowUpEntityId = (int)postResultFodept.Id;
                }
                else
                {

                    message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpOrgUnitDoesNotExist");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                if (FollowUpPeriod == -1)
                    followVM.FollowUpExpireDate = Convert.ToDateTime(FolloupDateTo);
                else
                    followVM.FollowUpExpireDate = DateTime.Now.AddDays(Convert.ToInt32(FollowUpPeriod));

                PostResult followuppostResult =
                        HttpClientWrapper<PostResult>.
                        PostRequest("api/Transaction/TransactionFollowUpAdd?cultureName=" +
                                            SessionInfo.CultureShortName,
                                            TransactionFollowUpMapper.Map(followVM))
                                            .Result;
                if (followuppostResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followuppostResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (followuppostResult.Id.HasValue && followuppostResult.Id > 0)
                {
                    FollowUpAuditTrailVM followUpAuditTrail = new FollowUpAuditTrailVM();
                    followUpAuditTrail.FollowupId = (int)followuppostResult.Id;
                    followUpAuditTrail.ProccessDate = DateTime.Now;
                    followUpAuditTrail.ProccessId = (int)FollowupAuditProcess.AddPublicFollowup;
                    followUpAuditTrail.ProccessDescription = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPublicFollowUp");
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
            }
            #endregion

            #region Copy
            if (TransactionAssignments.Any(x => x.IsCopy))
            {
                List<TransactionCopyDTO> transactionCopyDTOs = TransactionAssignments
                    .Where(ta => ta.IsCopy == true)
                    .Select(tc => new TransactionCopyDTO
                    {
                        ActionId = tc.ActionId == 0 ? _action : tc.ActionId,
                        UserId = tc.ToUserId,
                        OrgUnitId = tc.ToOrgUnitId,
                        IsSent = 1,
                        FromUserId = SessionInfo.CurrentUser.Id,
                        FromOrgUnitId = SessionInfo.OrgUnitId,
                        IsBcc = tc.IsBcc
                    }).ToList();

                PostResult postCopiesResult = HttpClientWrapper<PostResult>
                    .PostRequest(string.Format("api/Transaction/AddAssignmentCopies?transactionId={0}", TransactionId),
                                transactionCopyDTOs).Result;

                if (postCopiesResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postCopiesResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
            }


            #endregion



            if (explanationTxt != "" && explanationTxt != null)
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

                PostResult postExplanationResult = HttpClientWrapper<PostResult>
                    .PostRequest(string.Format("api/Transaction/AddTransactionExplanation?transactionId={0}", TransactionId),
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
                                TransactionId,
                                TransactionAssignmentExplanationId),
                                null).Result;



            string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

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
        public ActionResult AssignmentPaperSettings(List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs)
        {
            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0 && x.BeneficiaryOrgUnitId.HasValue).ToList());
            int groupId = assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0).FirstOrDefault().GroupId;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateGroupAssignmentPaper?groupId=" + groupId, assignmentPaperDTO.Beneficiaries).Result;
            string message = string.Empty;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }
        public ActionResult SaveAssignmentPaper(List<TransactionAssignmentVM> TransactionAssignments, int TransactionId, string explanationTxt, string ConfedentialityId)
        {



            if (explanationTxt != "" && explanationTxt != null)
            {
                TransactionAssignments.ForEach(exp => exp.Remarks = explanationTxt);
            }
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
                assignmentPaperBeneficiaryVM.DefaultActionId = item.ActionId;
                assignmentPaperBeneficiaryVM.GroupId = item.GroupId;
                assignmentPaperBeneficiaryVM.ChkConstant = item.IsCopy;

                assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiaryVM);
            }

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();


            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper
                .Map(assignmentPaperBeneficiaryVMs
                    .Where(x => x.GroupId > 0 && x.BeneficiaryOrgUnitId.HasValue)
                    .ToList());
            var saveList = TransactionAssignments
                    .Where(x => x.GroupId > 0 && x.ToOrgUnitId > 0)
                    .ToList();
            SaveAssignmentPaperDTO saveAssignmentPaperDTO = new SaveAssignmentPaperDTO
            {

                TransactionId = TransactionId,
                AssignmentList = JsonConvert.SerializeObject(saveList),

            };

            //int groupId = assignmentPaperBeneficiaryVMs
            //    .Where(x => x.GroupId > 0)
            //    .FirstOrDefault().GroupId;
            PostResult postResult = HttpClientWrapper<PostResult>
                .PostRequest("api/Transaction/UpdateAssignmentSelectedoption",
                            saveAssignmentPaperDTO).Result;




            //PostResult postResult = HttpClientWrapper<PostResult>
            //    .PostRequest("api/UserProfile/UpdateGroupAssignmentPaper",
            //                assignmentPaperDTO.Beneficiaries).Result;


            string message = string.Empty;


            string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SaveSucceeded");

            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                returnUrl = url
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult SaveAssignmentPaperVip(List<TransactionAssignmentVM> AssignmentVMs, int TransactionId, string explanationTxt)
        {

            if (explanationTxt != "" && explanationTxt != null)
            {
                AssignmentVMs.ForEach(exp => exp.Remarks = explanationTxt);
            }


            List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs = new List<AssignmentPaperBeneficiaryVM>();
            foreach (var item in AssignmentVMs)
            {
                AssignmentPaperBeneficiaryVM assignmentPaperBeneficiaryVM = new AssignmentPaperBeneficiaryVM();
                assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId = item.ToOrgUnitId;
                assignmentPaperBeneficiaryVM.Id = item.Id;
                //assignmentPaperBeneficiaryVM.Key = item.Key;
                assignmentPaperBeneficiaryVM.OrgUnitName = item.ToOrgUnitName;
                assignmentPaperBeneficiaryVM.UserId = item.ToUserId;
                assignmentPaperBeneficiaryVM.UserName = item.ToUserName;
                assignmentPaperBeneficiaryVM.UserImageId = item.UserImageId;
                assignmentPaperBeneficiaryVM.DefaultActionId = item.ActionId;
                assignmentPaperBeneficiaryVM.GroupId = item.GroupId;
                assignmentPaperBeneficiaryVM.ChkConstant = item.IsCopy;

                assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiaryVM);
            }

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();


            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper
                .Map(assignmentPaperBeneficiaryVMs
                    .Where(x => x.GroupId > 0 && x.BeneficiaryOrgUnitId.HasValue)
                    .ToList());

            var saveList = AssignmentVMs
                    .Where(x => x.GroupId > 0 && x.ToOrgUnitId > 0)
                    .ToList();
            SaveAssignmentPaperDTO saveAssignmentPaperDTO = new SaveAssignmentPaperDTO
            {

                TransactionId = TransactionId,
                AssignmentList = JsonConvert.SerializeObject(saveList),

            };

            //int groupId = assignmentPaperBeneficiaryVMs
            //    .Where(x => x.GroupId > 0)
            //    .FirstOrDefault().GroupId;

            PostResult postResult = HttpClientWrapper<PostResult>
               .PostRequest("api/Transaction/UpdateAssignmentSelectedoption",
                           saveAssignmentPaperDTO).Result;
            //PostResult postResult = HttpClientWrapper<PostResult>
            //    .PostRequest("api/UserProfile/UpdateGroupAssignmentPaper",
            //                assignmentPaperDTO.Beneficiaries).Result;


            string message = string.Empty;


            string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SaveSucceeded");

            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                returnUrl = url
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult PdfAssignmentPaper(List<TransactionAssignmentVM> TransactionAssignments,
            int transactionId,
            string MainDocToken,
            string explanation,
            string Action = null,
            string Confidentiality = null,
            int ExplanationPriority = 0,
            bool Generalization = false


            )
        {


            List<TransactionAssignmentVM> transactionAssignmentVMs = GetPrintAssignmentPaper(TransactionAssignments);

            GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                .GetItemRequest(string
                            .Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                                transactionId, SessionInfo.CultureShortName)).Result;




            var toOrgunit = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ToOrgUnitName;
            var actionId = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ActionId ?? 0;



            //GetResult<OrgUnitDTO> orgUnitDTOs =
            //    HttpClientWrapper<GetResult<OrgUnitDTO>>
            //    .GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}",
            //                    SessionInfo.CultureShortName,
            //                    SessionInfo.OrgUnitId)).Result;


            //var parentorgunit = orgUnitDTOs.Result.Name;
            string Logo = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/GAMI_Logo_Color-01.svg";

            ViewData["LogoFiles"] = Logo;

            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }



            TransactionAssignmentPrintVM transactionAssignmentPrintVM = new TransactionAssignmentPrintVM
            {
                TransactionAssignmentVM = transactionAssignmentVMs,
                ConfedentialityId = transactionDetailsDTOResult.Result.ConfidentialityId,

                PriorityLevelId = transactionDetailsDTOResult.Result
                                .PriorityId,

                Number = transactionDetailsDTOResult.Result.Number.ToString(),
                PriorityLevel = transactionDetailsDTOResult.Result.Priority,
                Subject = transactionDetailsDTOResult.Result.Subject,
                FromOrgUnit = SessionInfo.OrgUnitInfo.Name,
                ToOrgUnit = toOrgunit,
                DateTimeNowG = transactionDetailsDTOResult.Result.Date.ToString(),
                ReminderDate = transactionDetailsDTOResult.Result.ReminderDate,
                //ParentOrgUnit = parentorgunit,
                InboundDateH = transactionDetailsDTOResult.Result.InboundDateH,
                DateTimeNowH = DateTime.Now.ToString() + " " + DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now),
                ActionId = string.IsNullOrEmpty(Action) ? actionId : int.Parse(Action),
                InboundNumber = transactionDetailsDTOResult.Result.InboundNumber,
                TransactionDateH = transactionDetailsDTOResult.Result.HijriDate,
                TransactionId = transactionDetailsDTOResult.Result.Id,
                Explanation = explanation,
                ExplanationPriority = ExplanationPriority,
                Generalization = Generalization
            };
            DateTime inboundDateFormat = DateTime.Now;
            DateTime transactionDateHFormat = DateTime.Now;
            if (transactionAssignmentPrintVM.InboundDateH != null && transactionAssignmentPrintVM.InboundDateH != "")
            {
                inboundDateFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.InboundDateH);
                transactionAssignmentPrintVM.InboundDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(inboundDateFormat);
            }
            if (transactionAssignmentPrintVM.TransactionDateH != null && transactionAssignmentPrintVM.TransactionDateH != "")
            {
                transactionDateHFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.TransactionDateH);
                transactionAssignmentPrintVM.TransactionDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionDateHFormat);
            }

            ViewData["AssignmentPaperData"] = transactionAssignmentPrintVM;
            Session["DocoNutDocument"] = null;
            ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();
            IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

            var actionVMs = GetAllActionsValues();

            actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

            ViewData["AllActionsData"] = actionVMs;


            var Html = UIHelper
                .RenderRazorViewToHtml(
                        ControllerContext,
                        "~/Areas/User/Views/Editor/AssignmentPaper/_PrintAddedAssignment.cshtml",
                        transactionAssignmentPrintVM);

            //Convert Html to Pdf    
            string handle = Guid.NewGuid().ToString();
            var pdf = PdfHelper.ConvertHtml2PDF_2(Html);
            byte[] data = DocumentViewerHelper.GetPDFFile(MainDocToken);



            List<byte[]> contactPdf = new List<byte[]>();


            contactPdf.Add(pdf);
            if (data != null)
            {
                contactPdf.Add(data);

            }




            byte[] mergedDocument = DoconutHelper.concatAndAddContent(contactPdf, "");
            Session["DocoNutDocument"] = mergedDocument;


            return Json(new { FileGuid = handle, FileName = "", MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }
     public ActionResult PrintAssignmentPaper(List<TransactionAssignmentVM> TransactionAssignments,
        int transactionId,
        string explanation,
        string Action = null,
        string Confidentiality = null,
        int ExplanationPriority = 0,
        bool Generalization = false


    )
        {


            List<TransactionAssignmentVM> transactionAssignmentVMs = GetPrintAssignmentPaper(TransactionAssignments);

            GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                .GetItemRequest(string
                            .Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                                transactionId, SessionInfo.CultureShortName)).Result;




            var toOrgunit = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ToOrgUnitName;
            var actionId = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ActionId ?? 0;



            //GetResult<OrgUnitDTO> orgUnitDTOs =
            //    HttpClientWrapper<GetResult<OrgUnitDTO>>
            //    .GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}",
            //                    SessionInfo.CultureShortName,
            //                    SessionInfo.OrgUnitId)).Result;


            //var parentorgunit = orgUnitDTOs.Result.Name;
            string Logo = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/GAMI_Logo_Color-001.svg";

            ViewData["LogoFiles"] = Logo;

            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }



            TransactionAssignmentPrintVM transactionAssignmentPrintVM = new TransactionAssignmentPrintVM
            {
                TransactionAssignmentVM = transactionAssignmentVMs,
                ConfedentialityId = transactionDetailsDTOResult.Result.ConfidentialityId,

                PriorityLevelId = transactionDetailsDTOResult.Result
                                .PriorityId,

                Number = transactionDetailsDTOResult.Result.Number.ToString(),
                PriorityLevel = transactionDetailsDTOResult.Result.Priority,
                Subject = transactionDetailsDTOResult.Result.Subject,
                FromOrgUnit = SessionInfo.OrgUnitInfo.Name,
                ToOrgUnit = toOrgunit,
                DateTimeNowG = transactionDetailsDTOResult.Result.Date.ToString(),
                ReminderDate = transactionDetailsDTOResult.Result.ReminderDate,
                //ParentOrgUnit = parentorgunit,
                InboundDateH = transactionDetailsDTOResult.Result.InboundDateH,
                DateTimeNowH = DateTime.Now.ToString() + " " + DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now),
                ActionId = string.IsNullOrEmpty(Action) ? actionId : int.Parse(Action),
                InboundNumber = transactionDetailsDTOResult.Result.InboundNumber,
                TransactionDateH = transactionDetailsDTOResult.Result.HijriDate,
                TransactionId = transactionDetailsDTOResult.Result.Id,
                Explanation = explanation,
                ExplanationPriority = ExplanationPriority,
                Generalization = Generalization
            };
            DateTime inboundDateFormat = DateTime.Now;
            DateTime transactionDateHFormat = DateTime.Now;
            if (transactionAssignmentPrintVM.InboundDateH != null && transactionAssignmentPrintVM.InboundDateH != "")
            {
                inboundDateFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.InboundDateH);
                transactionAssignmentPrintVM.InboundDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(inboundDateFormat);
            }
            if (transactionAssignmentPrintVM.TransactionDateH != null && transactionAssignmentPrintVM.TransactionDateH != "")
            {
                transactionDateHFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.TransactionDateH);
                transactionAssignmentPrintVM.TransactionDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionDateHFormat);
            }

            ViewData["AssignmentPaperData"] = transactionAssignmentPrintVM;
            ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();
            IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

            var actionVMs = GetAllActionsValues();

            actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

            ViewData["AllActionsData"] = actionVMs;
            return PartialView("~/Areas/User/Views/Editor/AssignmentPaper/_PrintAddedAssignment.cshtml");

        }
        public ActionResult PrintAssignmentPaperVIP(List<TransactionAssignmentVM> AssignmentVMs,
     int transactionId,
     string explanation,
     string Action = null,
     string Confidentiality = null,
     int ExplanationPriority = 0,
     bool Generalization = false


 )
        {


            List<TransactionAssignmentVM> transactionAssignmentVMs = GetPrintAssignmentPaper(AssignmentVMs);

            GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                .GetItemRequest(string
                            .Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                                transactionId, SessionInfo.CultureShortName)).Result;




            var toOrgunit = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ToOrgUnitName;
            var actionId = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ActionId ?? 0;



            //GetResult<OrgUnitDTO> orgUnitDTOs =
            //    HttpClientWrapper<GetResult<OrgUnitDTO>>
            //    .GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}",
            //                    SessionInfo.CultureShortName,
            //                    SessionInfo.OrgUnitId)).Result;


            //var parentorgunit = orgUnitDTOs.Result.Name;
            string Logo = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/GAMI_Logo_Color-001.svg";

            ViewData["LogoFiles"] = Logo;

            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }



            TransactionAssignmentPrintVM transactionAssignmentPrintVM = new TransactionAssignmentPrintVM
            {
                TransactionAssignmentVM = transactionAssignmentVMs,
                ConfedentialityId = transactionDetailsDTOResult.Result.ConfidentialityId,

                PriorityLevelId = transactionDetailsDTOResult.Result
                                .PriorityId,

                Number = transactionDetailsDTOResult.Result.Number.ToString(),
                PriorityLevel = transactionDetailsDTOResult.Result.Priority,
                Subject = transactionDetailsDTOResult.Result.Subject,
                FromOrgUnit = SessionInfo.OrgUnitInfo.Name,
                ToOrgUnit = toOrgunit,
                DateTimeNowG = transactionDetailsDTOResult.Result.Date.ToString(),
                ReminderDate = transactionDetailsDTOResult.Result.ReminderDate,
                //ParentOrgUnit = parentorgunit,
                InboundDateH = transactionDetailsDTOResult.Result.InboundDateH,
                DateTimeNowH = DateTime.Now.ToString() + " " + DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now),
                ActionId = string.IsNullOrEmpty(Action) ? actionId : int.Parse(Action),
                InboundNumber = transactionDetailsDTOResult.Result.InboundNumber,
                TransactionDateH = transactionDetailsDTOResult.Result.HijriDate,
                TransactionId = transactionDetailsDTOResult.Result.Id,
                Explanation = explanation,
                ExplanationPriority = ExplanationPriority,
                Generalization = Generalization
            };
            DateTime inboundDateFormat = DateTime.Now;
            DateTime transactionDateHFormat = DateTime.Now;
            if (transactionAssignmentPrintVM.InboundDateH != null && transactionAssignmentPrintVM.InboundDateH != "")
            {
                inboundDateFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.InboundDateH);
                transactionAssignmentPrintVM.InboundDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(inboundDateFormat);
            }
            if (transactionAssignmentPrintVM.TransactionDateH != null && transactionAssignmentPrintVM.TransactionDateH != "")
            {
                transactionDateHFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.TransactionDateH);
                transactionAssignmentPrintVM.TransactionDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionDateHFormat);
            }

            ViewData["AssignmentPaperData"] = transactionAssignmentPrintVM;
            ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();
            IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

            var actionVMs = GetAllActionsValues();

            actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

            ViewData["AllActionsData"] = actionVMs;
            return PartialView("~/Areas/User/Views/Editor/AssignmentPaper/_PrintAddedAssignment.cshtml");

        }
        public ActionResult VIPPdfAssignmentPaper(List<TransactionAssignmentVM> AssignmentVMs,
          int transactionId,
          string MainDocToken,
          string explanation,
          string Action = null,
          string Confidentiality = null,
          int ExplanationPriority = 0,
          bool Generalization = false


          )
        {


            List<TransactionAssignmentVM> transactionAssignmentVMs = GetPrintAssignmentPaper(AssignmentVMs);

            GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                .GetItemRequest(string
                            .Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                                transactionId, SessionInfo.CultureShortName)).Result;




            var toOrgunit = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ToOrgUnitName;
            var actionId = transactionAssignmentVMs.Where(x => x.IsAssigned)?.FirstOrDefault()?.ActionId ?? 0;



            GetResult<OrgUnitDTO> orgUnitDTOs =
                HttpClientWrapper<GetResult<OrgUnitDTO>>
                .GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}",
                                SessionInfo.CultureShortName,
                                SessionInfo.OrgUnitId)).Result;


            var parentorgunit = orgUnitDTOs.Result.Name;

            string Logo = MCS.UI.UrlHelper.GetBaseUri() + "/Content/User/lib/images/GAMI_Logo_Color-01.svg";

            ViewData["LogoFiles"] = Logo;

            if (SessionInfo.CurrentUser.TenantLogo != null)
            {
                ViewData["LogoFile"] = Convert.ToBase64String(SessionInfo.CurrentUser.TenantLogo);
            }


            TransactionAssignmentPrintVM transactionAssignmentPrintVM = new TransactionAssignmentPrintVM
            {
                TransactionAssignmentVM = transactionAssignmentVMs,
                ConfedentialityId = string.IsNullOrEmpty(Confidentiality) ? transactionDetailsDTOResult.Result
                                .ConfidentialityId : int.Parse(Confidentiality),

                PriorityLevelId = transactionDetailsDTOResult.Result
                                .PriorityId,

                Number = transactionDetailsDTOResult.Result.Number.ToString(),
                PriorityLevel = transactionDetailsDTOResult.Result.Priority,
                Subject = transactionDetailsDTOResult.Result.Subject,
                FromOrgUnit = SessionInfo.OrgUnitInfo.Name,
                ToOrgUnit = toOrgunit,
                DateTimeNowG = transactionDetailsDTOResult.Result.Date.ToString(),
                ReminderDate = transactionDetailsDTOResult.Result.ReminderDate,
                ParentOrgUnit = parentorgunit,
                InboundDateH = transactionDetailsDTOResult.Result.InboundDateH,
                DateTimeNowH = DateTime.Now.ToString() + " " + DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now),
                ActionId = string.IsNullOrEmpty(Action) ? actionId : int.Parse(Action),
                InboundNumber = transactionDetailsDTOResult.Result.InboundNumber,
                TransactionDateH = transactionDetailsDTOResult.Result.HijriDate,
                TransactionId = transactionDetailsDTOResult.Result.Id,
                Explanation = explanation,
                ExplanationPriority = ExplanationPriority,

            };
            DateTime inboundDateFormat = DateTime.Now;
            DateTime transactionDateHFormat = DateTime.Now;
            if (transactionAssignmentPrintVM.InboundDateH != null && transactionAssignmentPrintVM.InboundDateH != "")
            {
                inboundDateFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.InboundDateH);
                transactionAssignmentPrintVM.InboundDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(inboundDateFormat);
            }
            if (transactionAssignmentPrintVM.TransactionDateH != null && transactionAssignmentPrintVM.TransactionDateH != "")
            {
                transactionDateHFormat = DateTimeUtility.HijriToGreg(transactionAssignmentPrintVM.TransactionDateH);
                transactionAssignmentPrintVM.TransactionDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionDateHFormat);
            }

            ViewData["AssignmentPaperData"] = transactionAssignmentPrintVM;
            Session["DocoNutDocument"] = null;
            ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();
            IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

            var actionVMs = GetAllActionsValues();

            actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

            ViewData["AllActionsData"] = actionVMs;

            var Html = UIHelper
                .RenderRazorViewToHtml(
                        ControllerContext,
                        "~/Areas/User/Views/Editor/AssignmentPaper/_PrintAddedAssignment.cshtml",
                        transactionAssignmentPrintVM);

            //Convert Html to Pdf    
            string handle = Guid.NewGuid().ToString();
            var pdf = PdfHelper.ConvertHtml2PDF_2(Html);
            byte[] data = DocumentViewerHelper.GetPDFFile(MainDocToken);



            List<byte[]> contactPdf = new List<byte[]>();


            contactPdf.Add(pdf);
            if (data != null)
            {
                contactPdf.Add(data);

            }




            byte[] mergedDocument = DoconutHelper.concatAndAddContent(contactPdf, "");
            Session["DocoNutDocument"] = mergedDocument;


            return Json(new { FileGuid = handle, FileName = "", MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }

        protected void InitializerAssignmentPaperData(int transactionId)
        {
            GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                        .GetItemRequest(string.Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                        transactionId,
                        SessionInfo.CultureShortName)).Result;

            GetResult<OrgUnitDTO> _orgUnitDTOs =
                HttpClientWrapper<GetResult<OrgUnitDTO>>
                .GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}",
                                SessionInfo.CultureShortName,
                                SessionInfo.OrgUnitId)).Result;



            var parentorgunit = _orgUnitDTOs?.Result?.Name;


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

                ParentOrgUnit = parentorgunit,
                InboundDateH = transactionDetailsDTOResult.Result.InboundDateH,

                DateTimeNowH = DateTime.Now.ToString() + " " + DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now),
                InboundNumber = transactionDetailsDTOResult.Result.InboundNumber,
                TransactionDateH = transactionDetailsDTOResult.Result.HijriDate,
                TransactionId = transactionDetailsDTOResult.Result.Id,
                ToOrgUnit = transactionDetailsDTOResult.Result.FromOrgUnit,
                LetterTypeId = transactionDetailsDTOResult.Result.LetterTypeId,
                LetterType = transactionDetailsDTOResult.Result.LetterType,
            };
            DateTime inboundDateFormat = DateTime.Now;
            DateTime transactionDateHFormat = DateTime.Now;
            if (transactionAssignmentBasicData.InboundDateH != null && transactionAssignmentBasicData.InboundDateH != "")
            {
                inboundDateFormat = DateTimeUtility.HijriToGreg(transactionAssignmentBasicData.InboundDateH);
                transactionAssignmentBasicData.InboundDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(inboundDateFormat);
            }
            if (transactionAssignmentBasicData.TransactionDateH != null && transactionAssignmentBasicData.TransactionDateH != "")
            {
                transactionDateHFormat = DateTimeUtility.HijriToGreg(transactionAssignmentBasicData.TransactionDateH);
                transactionAssignmentBasicData.TransactionDateH = DateTimeUtility.ConvertToUmAlQuraCalendarFullFormat(transactionDateHFormat);
            }
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
        [HttpGet]
        public ActionResult DownloadExternalTransactions(string transId)
        {
            try
            {
                DocumentVM documentVM = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(transId.Replace(" ", "+")));
                GetResult<TransactionPrintDTO> transactionPrintDTO = HttpClientWrapper<GetResult<TransactionPrintDTO>>.GetItemRequest(string.Format("api/Transaction/GetAllTransactionDocuments?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                documentVM = DocumentMapper.Map(transactionPrintDTO.Result.DocumentDTO);
                byte[] pdfContent = documentVM.Content;

                return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Pdf, documentVM.Name);



            }
            catch (Exception ex)
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
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/AssignmentPaper/_AddedAssignmentEntities.cshtml", transactionAssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult SaveDeliveryNumber(int id, int transactionCategoryId)
        {
            try
            {
                var transactionEditVM = new TransactionEditVM()
                {
                    Id = id,
                    TransactionCategory = (TransactionCategory)transactionCategoryId
                };

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SaveDeliveryNumber", transactionEditVM)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveDeliveryNumber(TransactionEditVM transactionEditVM)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransactionDeliveryNumber", TransactionLightMapper.Map(transactionEditVM)).Result;

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
        [HttpPost]
        public ActionResult UpdateTransactionStatus(int transactionId, int statusId, int type)
        {
            try
            {
                string message = string.Empty;
                //int transactionId,int statusId, int type
                PutResult postResult = HttpClientWrapper<PutResult>
                    .PutRequest(string.Format("api/Transaction/UpdateTransactionStatus?transactionId={0}&statusId={1}&type={2}", transactionId, statusId, type), null).Result;

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
        public ActionResult VerifyTransactionNumberOrBarcode(int id, int transactionCategoryId)
        {
            try
            {
                var transactionLightVM = new TransactionLightVM()
                {
                    Id = id,
                    TransactionCategory = (TransactionCategory)transactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)
                };

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_VerifyTransactionNumberOrBarcode", transactionLightVM)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VerifyTransactionNumberOrBarcode(TransactionLightVM transactionLightVM)
        {
            try
            {
                try
                {
                    string message = string.Empty;
                    if (string.IsNullOrEmpty(transactionLightVM.Number) && string.IsNullOrEmpty(transactionLightVM.Barcode))
                    {
                        return Json(new
                        {
                            MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.EnterAtLeastOneField"),
                            MessageType = MessageType.Error,
                            IsMatch = false
                        }, JsonRequestBehavior.AllowGet);
                    }

                    transactionLightVM.UserId = SessionInfo.CurrentUser.Id;
                    transactionLightVM.EntityId = SessionInfo.OrgUnitId;

                    var postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/VerifyTransactionNumberOrBarcode"),
                        TransactionLightMapper.Map(transactionLightVM)).Result;

                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    if (Convert.ToBoolean(postResult.Result) == false)
                    {
                        return Json(new
                        {
                            MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.ThereIsNoValueForInput"),
                            MessageType = MessageType.Error,
                            IsMatch = false
                        }, JsonRequestBehavior.AllowGet);
                    }

                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.AddSucceeded");
                    return Json(new
                    {
                        MessageText = message,
                        MessageType = MessageType.Information,
                        IsMatch = postResult.Result
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetDeliveryReportByTransactionId(int id, int type)
        {
            string message = string.Empty;

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest($"api/Transaction/GetDeliveryReportByTransactionIds?transactionId={id}&type={type}", null).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                ReportsIds = JsonConvert.SerializeObject(postResult.Result)
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDeliveryReportByTransactionIds(string transactionIds)
        {
            string message = string.Empty;
            GetResult<List<TransactionDeliveryReportDTO>> postResult =
            HttpClientWrapper<GetResult<List<TransactionDeliveryReportDTO>>>.PostRequest($"api/Transaction/GetDeliveryReportByTransIds",
                transactionIds.Split(',').Select(a => int.Parse(a)).ToList()).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                TransactionDeliveryReports = postResult.Result
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetContentByFormId(int id)
        {
            try
            {
                string html = string.Empty;

                GetResult<DocumentDTO> formContentDTO =
                HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetContentByFormId?formId={0}", id)).Result;

                if (formContentDTO.Result != null && formContentDTO.Result.Content != null)
                {
                    // html = FormMapper.Map(formContentDTO.Result).Content;
                    html = Encoding.Default.GetString(formContentDTO.Result?.Content);
                }

                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetByteContentByFormId(int id, string fileId)
        {
            try
            {
                Byte[] fileContent = null;
                String fileName = string.Empty;
                GetResult<DocumentDTO> formContentDTO =
                HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetContentByFormId?formId={0}", id)).Result;

                if (formContentDTO.Result != null && formContentDTO.Result.Content != null)
                {
                    // html = FormMapper.Map(formContentDTO.Result).Content;
                    fileContent = formContentDTO.Result?.Content;

                    fileName = fileId;
                    ViewData["OfficeOnlineFileGuid"] = fileName;
                    DocumentViewerHelper.WriteOfficeFile(fileContent, fileName);
                }

                return fileName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetDistributionLists()
        {
            GetResult<List<DistributionListDTO>> DistributionListDTO =
               HttpClientWrapper<GetResult<List<DistributionListDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionList?userId={0}&orgUnitId={1}&cultureName={2}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            if (DistributionListDTO.Result != null)
            {
                foreach (DistributionListVM distributionList in DistributionListMapper.Map(DistributionListDTO.Result))
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = distributionList.Id.ToString(),
                        Label = distributionList.Name.Where(s => s.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text
                    });
                }
            }
            return JsonConvert.SerializeObject(dataSource);
        }

        public string GetTransactionPaths(int? pathId = null)
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            if (pathId.HasValue)
            {
                GetResult<TransactionPathDTO> transactionPathDTO =
                 HttpClientWrapper<GetResult<TransactionPathDTO>>.GetItemRequest(String.Format("api/UserProfile/GetTransactionPathById?pathId={0}", pathId)).Result;

                if (transactionPathDTO.Result != null)
                {
                    TransactionPathVM transactionPath = TransactionPathMapper.Map(transactionPathDTO.Result);
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = transactionPath.Id.ToString(),
                        Label = transactionPath.Name
                    });
                }
            }
            else
            {
                GetResult<List<TransactionPathDTO>> transactionPathsResult =
                 HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetTransactionPathForTransaction?userId={0}&orgUnitId={1}", null, SessionInfo.OrgUnitId)).Result;

                if (transactionPathsResult.Result != null)
                {
                    foreach (TransactionPathDTO transactionPath in transactionPathsResult.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = transactionPath.Id.ToString(),
                            Label = transactionPath.Name
                        });
                    }
                }
            }
            return JsonConvert.SerializeObject(dataSource);
        }

        #region Tasks
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult AddedTasks(int transactionId)
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

                List<TaskAddVM> gridData = GetTransactionTasks(transactionId);

                IAjaxGrid Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);


                TaskAddVM taskAddVM = new TaskAddVM();

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_AddTaskPartial.cshtml", taskAddVM), GridHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", Grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<TaskAddVM> GetTransactionTasks(int transactionId)
        {
            GetResult<List<TaskAddDTO>> taskDTOs =
               HttpClientWrapper<GetResult<List<TaskAddDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTasks?transactionId={0}&PageIndex={1}&pageSize={2}&cultureName={3}", transactionId, 1, UIHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<TaskAddVM> taskAddVMs = TaskAddMapper.Map(taskDTOs.Result);
            if (taskAddVMs == null)
            {
                taskAddVMs = new List<TaskAddVM>();
            }
            int key = 0;
            foreach (var Task in taskAddVMs)
            {
                Task.Key = key++;
            }

            return taskAddVMs;
        }
        public static List<ReceivedTaskVM> GetTransactionTasksReply(int transactionId)
        {
            GetResult<List<ReceivedTaskDTO>> taskDTOs =
               HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTasksReply?transactionId={0}&PageIndex={1}&pageSize={2}&cultureName={3}", transactionId, 1, UIHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<ReceivedTaskVM> receivedTaskVMs = ReceivedTaskMapper.Map(taskDTOs.Result);
            if (receivedTaskVMs == null)
            {
                receivedTaskVMs = new List<ReceivedTaskVM>();
            }
            int key = 0;
            foreach (var Task in receivedTaskVMs)
            {
                Task.Key = key++;
            }

            return receivedTaskVMs;
        }
        #endregion


        #region FollowUp


        protected string GetFollowUpProccess(TransactionCategory transactionCategory)
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

        protected string FollowupPeriod(TransactionCategory transactionCategory)
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                dataSource.Add(new AutoCompleteDataSource() { Value = "99", Label = "اختر" });
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
                        return PartialView("~/Areas/User/Views/Transaction/_FollowUpGridPartial.cshtml", detailsList);

                    }
                    else
                    {

                        message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpProcessNeeded");
                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {

                    //message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpAlreadyAdded");
                    //return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);



                    message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpAlreadyExist");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<TransactionFollowUpVM> GetTransactionFollowUps(int transactionId)
        {
            GetResult<List<TransactionFollowUpDTO>> dtoAPI =
               HttpClientWrapper<GetResult<List<TransactionFollowUpDTO>>>.GetItemRequest(string.Format("api/Transaction/TransactionFollowUpSelectByTransId?transId={0}&cultureName={1}", transactionId, SessionInfo.CultureShortName)).Result;

            List<TransactionFollowUpVM> transactionCoordinationVMs = TransactionFollowUpMapper.Map(dtoAPI.Result);

            return transactionCoordinationVMs;

        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.FollowUps.DeleteFollowUp)]
        public ActionResult FollowUpUpdateIsDeleted(int id)
        {
            try
            {
                string message = string.Empty;
                PutResult putResult =
                    HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/FollowUpUpdateIsDeleted?Id={0}", id), null).Result;
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

        public ActionResult FollowUpDetailsLoad(int transactionId)
        {
            List<TransactionFollowUpVM> list = GetTransactionFollowUps(transactionId);

            var detailsList = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(list, 1, list.Count, false);
            return PartialView("~/Areas/User/Views/Transaction/_FollowUpGridPartial.cshtml", detailsList);


        }
        #endregion



        [HttpPost]
        public ActionResult AddProcessPeriodTransaction(int transactionId, int processPeriod)
        {
            try
            {
                string message = string.Empty;


                PostResult putResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/AddProcessPeriodTransaction?transId=" + transactionId + "&processPeriod=" + processPeriod, null).Result;
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




        public ActionResult SendAssignmentByTransaction(string hdnTransactionId, string DirectedToOrgUnitId, string DirectedToId, string DeliveryMethodId, bool isConfirmed)
        {
            try
            {
                string message = string.Empty;
                int? hdnDirectedToId = null;

                int DefualtAction = Convert.ToInt32(ConfigurationManager.AppSettings["DefualtAssignmentAction"] ?? "1");
                if (DirectedToId != "-1" && DirectedToId != "")
                {
                    hdnDirectedToId = int.Parse(DirectedToId);
                }
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TransactionAssignmentVM> TransactionAssignments = new List<TransactionAssignmentVM>();
                TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM()
                {
                    FromOrgUnitId = SessionInfo.OrgUnitId,
                    ToUserId = hdnDirectedToId,
                    FromUserId = SessionInfo.CurrentUser.Id,
                    ToOrgUnitId = int.Parse(DirectedToOrgUnitId),
                    DeliveryMethodId = int.Parse(DeliveryMethodId),
                    ActionId = DefualtAction
                };
                TransactionAssignments.Add(transactionAssignmentVM);
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
                    assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiaryVM);
                }
                AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
                assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs);
                //PostResult postResultassignmentPaper = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateAssignmentPaper?userId=" + SessionInfo.CurrentUser.Id, assignmentPaperDTO).Result;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/CheckUserHasPermission?sTransactionsIds={0}", hdnTransactionId.Trim(',')), TransactionAssignmentMapper.Map(TransactionAssignments)).Result;
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



                postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostAssignTransaction?transactionId={0}", hdnTransactionId), TransactionAssignmentMapper.Map(TransactionAssignments.Where(ta => ta.IsAssigned == true).ToList())).Result;
                PutResult UpdateDelivary = HttpClientWrapper<PutResult>
                                  .PutRequest(string.Format("api/Transaction/UpdateTransactionDelivary?transactionId={0}&DeliveryMethodId={1}", hdnTransactionId, DeliveryMethodId), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                string url = UrlHelper.GetBaseUri() + "/User/File/MyTransactions";

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");

                bool printDeliveryReport = false;
                bool oneDeliveryReport = false;


                return Json(new { MessageText = message, MessageType = MessageType.Information, url = url, PrintDeliveryReport = printDeliveryReport, OneDeliveryReport = oneDeliveryReport, TransactionReportInfo = javaScriptSerializer.Serialize(postResult.Result) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string request = "hdnTransactionId=" + hdnTransactionId + ", DirectedToOrgUnitId=" + DirectedToOrgUnitId + " , DirectedToId=" + DirectedToId + ", DeliveryMethodId=" + DeliveryMethodId + "   ";
                Logger.WriteExceptionWithMessage(ex, request);
                throw;
            }
        }
        [HttpPost]
        public ActionResult TransactionDirectReply(int transactionId, string remarks)
        {
            {
                try
                {
                    string message = string.Empty;

                    //PutResult putResult = HttpClientWrapper<PutResult>
                    //   .PutRequest(string.Format("api/Transaction/TransactionDirectReply?transactionId={0}&remarks={1}", transactionId, remarks), null).Result;

                    GetResult<bool> getResult = HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Transaction/TransactionDirectReply?transactionId={0}&remarks={1}&userId={2}", transactionId, remarks, SessionInfo.CurrentUser.Id)).Result;


                    if (getResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    return Json(new { MessageText = message, MessageType = MessageType.Information, result = getResult.Result }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }



        [HttpGet]
        public ActionResult GetMainDocumentByTransactionId(string transactionId)
        {
            string message = string.Empty;
            MessageType messageType = MessageType.Information;
            try
            {
                GetResult<DocumentDTO> documentDTOResult =
                        HttpClientWrapper<GetResult<DocumentDTO>>.GetItemRequest(string.Format("api/Transaction/GetMainDocumentByTransactionId?transactionId={0}", transactionId)).Result;


                if (documentDTOResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, documentDTOResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { MessageText = message, MessageType = messageType, DocumentDTOResult = documentDTOResult.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string GetVIPPriorities(TransactionCategory transactionCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                GetResult<List<PriorityVM>> priorityVMResult = LookupsHelper.GetPriorities(transactionCategory);

                List<PriorityVM> priorityVMList = priorityVMResult.Result.Where(p => p.Id != 11).ToList();

                //
                if (priorityVMList != null)
                {
                    foreach (PriorityVM priorityVM in priorityVMList)
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

        [HttpPost]
        public ActionResult AssignDraftOutboundBack(int TransId, string Notes)
        {
            string message = string.Empty;
            MessageType messageType = MessageType.Information;
            PostResult postResult = null;
            try
            {
                postResult = HttpClientWrapper<PostResult>
                                                 .PostRequest($"api/MobileApi/AssignDraftOutboundBack?TransId={TransId}&Notes={Notes}&userId={SessionInfo.CurrentUser.Id}&entityId={SessionInfo.OrgUnitId}", SessionInfo.CultureShortName)
                                                 .Result;


                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

                }

                return Json(new { MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string GetAllUsers()
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Transaction/GetAllUsers")).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (userProfileDTOs.Result != null)
                {
                    foreach (UserProfileVM userProfileVM in UserProfileMapper.Map(userProfileDTOs.Result))
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

        [HttpGet]
        public ActionResult GetLetterTypeById(int letterTypeId)
        {
            string message = string.Empty;

            GetResult<LetterTypeDTO> letterTypeDTO = HttpClientWrapper<GetResult<LetterTypeDTO>>.GetItemRequest(String.Format("api/Transaction/GetLetterTypeById?letterTypeId={0}&cultureName={1}", letterTypeId, SessionInfo.CultureShortName)).Result;
            if (letterTypeDTO.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, letterTypeDTO.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                Result = JsonConvert.SerializeObject(letterTypeDTO.Result),
                WithExtraField = letterTypeDTO.Result.WithExtraField
            }, JsonRequestBehavior.AllowGet);
        }





        private void LogData(string data)
        {
            string filePath = @"C:\CustomLog\";
            StringBuilder sb = new StringBuilder();

            sb.Append(data);
            // flush every 20 seconds as you do it
            System.IO.File.AppendAllText(filePath + "log.txt", sb.ToString());
            sb.Clear();
        }

        [HttpPost]
        public string GetByFormIdForWordAddIn(int? documentId, string content)
        {
            try
            {
                MCS.UI.Areas.User.Models.Shared.UserVM user = (MCS.UI.Areas.User.Models.Shared.UserVM)SessionInfo.GetObjectFromSession(Constants.LoggedInUserKey);
                byte[] contentByte = null;
                string extension = ".docx";
                if (documentId.HasValue && documentId.Value > 0)
                {
                    GetResult<byte[]> fileContent =
             HttpClientWrapper<GetResult<byte[]>>.GetItemRequest(string.Format("api/WordAddIn/GetFormById?formId={0}&transactionId={1}&userName={2}", documentId, 0, user.UserName)).Result;
                    contentByte = fileContent.Result;
                }
                else
                {
                    contentByte = Convert.FromBase64String(content);
                    extension = ".doc";
                }

                string message = "";
                string FileName = StartKey + Sperator + user.UserName.ToLower() + Sperator + EndKey + extension;
                WordAddinDocumentDTO wordAddInTemp = new WordAddinDocumentDTO { content = contentByte, FileName = FileName };
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest($"api/WordAddIn/SaveTempDocument", wordAddInTemp).Result;
                return FileName;
            }
            catch (Exception ex)
            {

                throw;
            }
        }





        public string ApprovedWordAddIn(bool isApproved)
        {
            try
            {

                MCS.UI.Areas.User.Models.Shared.UserVM user = (MCS.UI.Areas.User.Models.Shared.UserVM)SessionInfo.GetObjectFromSession(Constants.LoggedInUserKey);

                GetResult<string> fileName =
                HttpClientWrapper<GetResult<string>>.GetItemRequest(string.Format("api/WordAddIn/ApprovedWordAddIn?isApproved={0}&userName={1}", isApproved, user.UserName)).Result;


                GetResult<WordAddinDocumentDTO> wordAddinDocumentDTO =
               HttpClientWrapper<GetResult<WordAddinDocumentDTO>>.GetItemRequest(string.Format("api/WordAddIn/GetDocumentSessionData?userName=" + user.UserName)).Result;


                wordAddinDocumentDTO.Result.userName = user.UserName;
                if (wordAddinDocumentDTO.Result != null)
                {

                    wordAddinDocumentDTO.Result.IsApproved = isApproved;

                    var docToPdf = OfficeOnlineHelper.ConvertDocToPDF(wordAddinDocumentDTO.Result.content).Result;

                    wordAddinDocumentDTO.Result.contentAsPDF = docToPdf;

                    TempStorgepath = System.Configuration.ConfigurationManager.AppSettings["WordAddInStoragePath"];

                    string bodyContent = UnicodeEncoding.UTF8.GetString(wordAddinDocumentDTO.Result.content);

                    string FileName = StartKey + Sperator + user.UserName.ToLower() + Sperator + EndKey;

                    System.IO.File.WriteAllBytes(TempStorgepath + FileName + ".doc", wordAddinDocumentDTO.Result.content);

                    wordAddinDocumentDTO.Result.FileName = FileName + ".doc";

                }

                PostResult postResultCall = HttpClientWrapper<PostResult>.PostRequest("api/WordAddIn/PostDocumentStringObject", wordAddinDocumentDTO.Result).Result;


                return fileName.Result;
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpGet]
        public ActionResult UpdateTransactionSubject(int transactionId, string subject)
        {
            try
            {

                EditSubjectTransactionVM transactionVM = new EditSubjectTransactionVM
                {
                    Id = transactionId,
                    Subject = subject
                };
                return View("~/Areas/User/Views/Transaction/_UpdateSubjectPartialView.cshtml", transactionVM);

                return Json(new { message = MessageType.Error });

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        public ActionResult UpdateTransactionSubject(EditSubjectTransactionVM transactionVM)
        {
            string message = string.Empty;
            try
            {


                PostResult postResult =
           HttpClientWrapper<PostResult>.PostRequest("api/Transaction/UpdateTransactionSubject?cultureName=" + SessionInfo.CultureShortName, TransactionMapper.Map(transactionVM)).Result;

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
        public ActionResult AddConfidentialityAcknowledgment(string transactionId)
        {
            string message = string.Empty;

            PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/AddConfidentialityAcknowledgment?TransactionId={0}&UserId={1}&OrgUnitId={2}&CreatedDate={3}", transactionId, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, DateTime.Now), new { }).Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = putResult.Id.ToString() }, JsonRequestBehavior.AllowGet);
        }





        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VIPAddTemporaryEntity([Bind(Prefix = "AssignmentVM")] VIPTransactionAssignmentVM AssignmentVM, List<VIPTransactionAssignmentVM> TransactionAssignments)
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
                AssignmentVM.DeliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (AssignmentVM.ToUserId != null)
                {
                    GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", AssignmentVM.ToUserId)).Result;


                }
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                AssignmentVM.Key = TransactionAssignments != null ? TransactionAssignments.Count + 1 : 1;
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Vip/AssignmentPaper/_AddedAssignmentEntities.cshtml", AssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }




        protected List<TransactionAttachmentVM> FillTransactionAttachment(List<TransactionArchiveVM> archives, Dictionary<string, byte[]> documentData)
        {
            List<TransactionAttachmentVM> transactionAttachmentVMs = new List<TransactionAttachmentVM>();

            if (archives == null || archives.Count == 0)
                return transactionAttachmentVMs;

            DocumentDTO docDTO = new DocumentDTO();
            foreach (var item in archives)
            {

                int currentAttachmentId = 0;
                int.TryParse(item.Id, out currentAttachmentId);
                TransactionAttachmentVM transactionAttachmentVM = new TransactionAttachmentVM();
                transactionAttachmentVM.Id = currentAttachmentId;
                transactionAttachmentVM.Number = item.Number;
                transactionAttachmentVM.TypeId = item.AttachmentTypeId ?? 0;
                transactionAttachmentVM.TypeName = item.ArcivingTypeName;
                transactionAttachmentVM.Archivable = item.Archivable;
                transactionAttachmentVM.AttachmentName = item.AttachmentName;
                transactionAttachmentVM.IsEnableAction = item.IsEnableAction;
                transactionAttachmentVM.AttachmentSource = item.AttachmentSource;

                byte[] valueInDictonary;
                string mimType = !string.IsNullOrWhiteSpace(item.MimeType) ? item.MimeType : System.Net.Mime.MediaTypeNames.Application.Pdf;

                if (Session[item.Id] != null)
                {

                    valueInDictonary = (byte[])Session[item.Id];

                    if (Session[item.Id + "MimeType"] != null)
                    {
                        mimType = Session[item.Id + "MimeType"].ToString();
                    }

                }


                if (item.Archivable)
                {
                    if (documentData != null && !string.IsNullOrEmpty(item.Id))
                    {
                        documentData.TryGetValue(item.Id, out valueInDictonary);
                    }
                    else
                    {
                        valueInDictonary = null;
                    }
                    if (valueInDictonary == null && item.DocumentId > 0)
                    {
                        docDTO = HttpClientWrapper<GetResult<DocumentDTO>>
                        .GetItemRequest(string.Format("api/Document/GetDocumentById?documentId={0}&cultureName={1}", item.DocumentId, SessionInfo.CultureShortName)).Result.Result;
                    }
                }
                else
                {
                    valueInDictonary = null;
                }
                if (!item.IsMainDocument && item.IsNew && item.Archivable)
                {
                    transactionAttachmentVM.DocumentVM = new DocumentVM();
                    if (valueInDictonary != null)
                    {
                        transactionAttachmentVM.DocumentVM.Content = valueInDictonary;
                        transactionAttachmentVM.DocumentVM.Size = valueInDictonary.Length;
                    }
                    else
                    {
                        transactionAttachmentVM.DocumentVM.Id = docDTO.Id;
                        transactionAttachmentVM.DocumentVM.Content = docDTO.Content; // handle case if docDTO not retrived
                        transactionAttachmentVM.DocumentVM.Size = docDTO.Size;
                        mimType = docDTO.MimeType;

                    }
                    transactionAttachmentVM.DocumentVM.Name = item.FileName;
                    transactionAttachmentVM.DocumentVM.MimeType = mimType;
                    transactionAttachmentVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    transactionAttachmentVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                }
                else if (!item.IsMainDocument && !item.IsDeleted && !item.IsNew && item.Archivable && documentData != null && item.Id != null && documentData[item.Id] != null)
                {



                    transactionAttachmentVM.DocumentVM = new DocumentVM
                    {
                        IsDeleted = false,
                        Content = documentData[item.Id],
                        Size = documentData[item.Id].Length,
                        Name = item.FileName,
                        MimeType = mimType,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id,
                        EncryptedId = item.EncryptDocumentId,
                        DocumentId = item.DocumentId,
                        Id = item.DocumentId
                    };
                }
                else if (!item.IsMainDocument && item.IsDeleted && item.Archivable)
                {
                    transactionAttachmentVM.DocumentVM = new DocumentVM
                    {
                        IsDeleted = true
                    };
                }
                else
                {
                    if (documentData != null && item.Id != null && documentData[item.Id] != null)
                    {
                        transactionAttachmentVM.DocumentVM = new DocumentVM
                        {
                            IsDeleted = false,
                            Content = documentData[item.Id],
                            Size = documentData[item.Id].Length,
                            Name = item.FileName,
                            MimeType = mimType,
                            FromEntityId = SessionInfo.OrgUnitId,
                            FromUserId = SessionInfo.CurrentUser.Id,
                            EncryptedId = item.EncryptDocumentId,
                            DocumentId = item.DocumentId,
                            Id = item.DocumentId

                        };
                    }

                }
                transactionAttachmentVMs.Add(transactionAttachmentVM);
            }

            return transactionAttachmentVMs;
        }





        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Links.DisplayLink)]
        public ActionResult GetLinkTransactions(string transactionId)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                GetResult<List<TransactionLinkDTO>> transactionLinkDTOs =
                 HttpClientWrapper<GetResult<List<TransactionLinkDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionLinks?transactionId={0}&cultureName={1}", trxId, SessionInfo.CultureShortName)).Result;

                List<TransactionLinkVM> transactionLinkVMs = TransactionLinkMapper.Map(transactionLinkDTOs.Result);
                if (transactionLinkVMs == null)
                {
                    transactionLinkVMs = new List<TransactionLinkVM>();
                }

                var detailsList = (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory().CreateAjaxGrid(transactionLinkVMs, 1, transactionLinkVMs.Count, false);
                return PartialView("~/Areas/User/Views/Shared/_ViewLinksGridPartial.cshtml", detailsList);
                return Json(new
                {
                    MessageType = MessageType.Information,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/_ViewLinksGridPartial.cshtml", (AjaxGrid<TransactionLinkVM>)new AjaxGridFactory()
                .CreateAjaxGrid(transactionLinkVMs, 1, transactionLinkVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult UpdateSession(string isEditMode)
        {
            try
            {
                bool isEdit = bool.Parse(StringCipher.DecryptStringAES(isEditMode.Replace(" ", "+")));
                Session["IsEditMode"] = isEdit;
                return Json(new { MessageType = MessageType.Information, });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    MessageText = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.InternalServerError.ToString()),
                    MessageType = MessageType.Error,

                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public FileResult DownLoadWordAddInInstallation()
        {
            try
            {

                TempStorgepath = System.Configuration.ConfigurationManager.AppSettings["WordAddInInstallationFileName"];
                //string rootpath = Server.MapPath("~/");
                //rootpath = rootpath + TempStorgepath;
                byte[] fileBytes = System.IO.File.ReadAllBytes(TempStorgepath);
                string fileName = "WordAddInInstallation.zip";
                return File(fileBytes, "application/zip", fileName);


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [HttpGet]
        public ActionResult GetExplanationByDocumentId(int id, string hdnExplanationDocumentSessionKey)
        {
            try
            {
                GetResult<ExplanationDTO> explanationDTO =
                HttpClientWrapper<GetResult<ExplanationDTO>>.GetItemRequest(string.Format("api/Transaction/GetExplanationByDocumentId?cultureName={0}&DocumentId={1}", SessionInfo.CultureShortName, id)).Result;

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
                    return Json(new { Type = explanationVM.EditorType, Content = Encoding.Unicode.GetString(explanationVM.DocumentVM.Content), ConfidentialityId = explanationVM.ConfidentialityId, Date = DateTimeUtility.ConvertToUmAlQuraCalendarWithTime(explanationVM.Date), FromUser = explanationVM.FromUser }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Type = explanationVM.EditorType, Content = Encoding.Unicode.GetString(explanationVM.DocumentVM.Content), ConfidentialityId = explanationVM.ConfidentialityId, Date = DateTimeUtility.ConvertToUmAlQuraCalendarWithTime(explanationVM.Date), FromUser = explanationVM.FromUser }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }



        protected List<VIPTransactionAssignmentVM> GetAssignmentPaper_VIP()
        {
            GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
             .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
            var deliveryMethodElectronic = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
            List<VIPTransactionAssignmentVM> transactionAssignmentVMs = new List<VIPTransactionAssignmentVM>();
            if (AssignmentPaperDTOs.Result != null && AssignmentPaperDTOs.Result.Beneficiaries != null)
            {
                transactionAssignmentVMs = AssignmentPaperDTOs.Result.Beneficiaries.Select(a =>
                {
                    VIPTransactionAssignmentVM transactionAssignmentVM = new VIPTransactionAssignmentVM();
                    transactionAssignmentVM.ToOrgUnitId = a.BeneficiaryOrgUnitId;
                    transactionAssignmentVM.ToOrgUnitName = a.OrgUnitName;
                    transactionAssignmentVM.ToUserId = a.UserId;
                    transactionAssignmentVM.ToUserName = a.UserName == null ? "استقبال الادارة" : a.UserName;
                    transactionAssignmentVM.GroupName = a.GroupName;
                    transactionAssignmentVM.GroupOrderNo = a.GroupOrderNo;
                    transactionAssignmentVM.GroupId = a.GroupId;
                    transactionAssignmentVM.ChkConstant = a.ChkConstant;
                    transactionAssignmentVM.DeliveryMethodId = deliveryMethodElectronic;
                    transactionAssignmentVM.OrderNo = a.OrderNo;
                    transactionAssignmentVM.ActionId = a.DefaultActionId;
                    transactionAssignmentVM.SpecialExplanation = a.SpecialExplanation;
                    transactionAssignmentVM.IsAssigned = false;

                    return transactionAssignmentVM;
                }).OrderBy(x => x.OrderNo).ToList();
            }
            return transactionAssignmentVMs;
        }

        protected List<TransactionAssignmentVM> GetAssignmentPaper()
        {
            GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
                .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}",
                                    SessionInfo.CurrentUser.Id,
                                    SessionInfo.CultureShortName)).Result;

            List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
            if (AssignmentPaperDTOs.Result != null && AssignmentPaperDTOs.Result.Beneficiaries != null)
            {
                transactionAssignmentVMs = AssignmentPaperDTOs.Result.Beneficiaries.Select(aa =>
                {
                    TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();
                    transactionAssignmentVM.ToOrgUnitId = aa.BeneficiaryOrgUnitId;
                    transactionAssignmentVM.ToOrgUnitName = aa.OrgUnitName;
                    transactionAssignmentVM.GroupName = aa.GroupName;
                    transactionAssignmentVM.ToUserId = aa.UserId;
                    transactionAssignmentVM.ToUserName = aa.UserName == null ? "استقبال الادارة" : aa.UserName;
                    transactionAssignmentVM.ChkConstant = aa.ChkConstant;
                    transactionAssignmentVM.OrderNo = aa.OrderNo;
                    transactionAssignmentVM.GroupOrderNo = aa.GroupOrderNo;
                    transactionAssignmentVM.ActionId = aa.DefaultActionId;
                    transactionAssignmentVM.GroupId = aa.GroupId;
                    transactionAssignmentVM.GroupName = aa.GroupName == null ? null : aa.GroupName;
                    transactionAssignmentVM.GroupOrderNo = aa.GroupOrderNo;
                    transactionAssignmentVM.Id = aa.Id;
                    transactionAssignmentVM.ChkConstant = aa.ChkConstant;
                    transactionAssignmentVM.IsAssigned = false;

                    //transactionAssignmentVM.SpecialExplanation = aa.
                    return transactionAssignmentVM;
                }).OrderBy(x => x.OrderNo).ToList();
            }

            return transactionAssignmentVMs;
        }

        protected List<TransactionAssignmentVM> GetPrintAssignmentPaper(List<TransactionAssignmentVM> transactionAssignmentVMs)
        {
            //GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
            //    .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

            //List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
            //if (AssignmentPaperDTOs.Result != null && AssignmentPaperDTOs.Result.Beneficiaries != null)
            //{
            //}
            transactionAssignmentVMs = transactionAssignmentVMs.Select(aa =>
            {
                TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();
                transactionAssignmentVM.ToOrgUnitId = aa.ToOrgUnitId;
                transactionAssignmentVM.ToOrgUnitName = aa.ToOrgUnitName;
                transactionAssignmentVM.GroupName = aa.GroupName;
                transactionAssignmentVM.ToUserId = aa.ToUserId;
                transactionAssignmentVM.ToUserName = aa.ToUserName == null ? "استقبال الادارة" : aa.ToUserName;
                transactionAssignmentVM.ChkConstant = aa.ChkConstant;
                transactionAssignmentVM.OrderNo = aa.OrderNo;
                transactionAssignmentVM.GroupOrderNo = aa.GroupOrderNo;
                transactionAssignmentVM.ActionId = aa.ActionId;
                transactionAssignmentVM.GroupId = aa.GroupId;
                transactionAssignmentVM.GroupName = aa.GroupName == null ? null : aa.GroupName;
                transactionAssignmentVM.GroupOrderNo = aa.GroupOrderNo;
                transactionAssignmentVM.Id = aa.Id;
                transactionAssignmentVM.IsBcc = aa.IsBcc;
                transactionAssignmentVM.IsOpr = aa.IsOpr;
                transactionAssignmentVM.IsAssigned = aa.IsAssigned;
                transactionAssignmentVM.IsCopy = aa.IsCopy;




                return transactionAssignmentVM;
            }).OrderBy(x => x.OrderNo).ToList();

            return transactionAssignmentVMs;
        }

        private string GetMimeType(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }
        private bool IsGuid(string value)
        {
            Guid x;
            return Guid.TryParse(value, out x);
        }
        [HttpPost]
        public ActionResult AddMultiExternal([Bind(Prefix = "MultiExternalOutbound")] MultiExternalOutboundVM MultiExternalOutbound, List<MultiExternalOutboundVM> MultipleExternalOutbound)
        {
            try
            {
                string message = string.Empty;
                List<int> result = !string.IsNullOrWhiteSpace(MultiExternalOutbound.ExternalOrgSelectedList) ? MultiExternalOutbound.ExternalOrgSelectedList.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                List<MultiExternalOutboundVM> MultiExternalOutboundVMs = new List<MultiExternalOutboundVM>();
                MultipleExternalOutbound = MultipleExternalOutbound ?? new List<MultiExternalOutboundVM>();
                int partiesCount = MultipleExternalOutbound.Count;
                bool isHasNew = false;
                isHasNew = result.Any(x => !MultipleExternalOutbound.Any(c => c.OrgUnitId == x));
                if (!isHasNew)
                {
                    message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }


                foreach (var orgUnitId in result)
                {

                    MultiExternalOutboundVM newMultiExternalOutbound = new MultiExternalOutboundVM
                    {
                        OrgUnitId = orgUnitId,
                        Id = 0
                    };

                    if (!MultipleExternalOutbound.Any(x => x.OrgUnitId == MultiExternalOutbound.OrgUnitId))
                    {
                        if ((!MultipleExternalOutbound.Any(x => x.OrgUnitId == MultiExternalOutbound.OrgUnitId)))
                        {

                            newMultiExternalOutbound.Id = 0;
                            newMultiExternalOutbound.Key = partiesCount + 1;
                            if (newMultiExternalOutbound.OrgUnitId > 0)
                            {
                                var orgUnitDTO = OrgHelper.GetExternalParty(newMultiExternalOutbound.OrgUnitId);
                                newMultiExternalOutbound.OrgUnitName = orgUnitDTO.Name.Where(x => x.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;


                            }
                            MultiExternalOutboundVMs.Add(newMultiExternalOutbound);
                            partiesCount++;
                        }

                    }

                    if (MultiExternalOutbound.OrgUnitId > 0)
                    {
                        ExternalPartyDTO orgUnitDTO = OrgHelper.GetExternalParty(MultiExternalOutbound.OrgUnitId);
                        MultiExternalOutbound.OrgUnitName = orgUnitDTO.Name.Where(s => s.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;
                    }

                }
                IAjaxGrid grid = (AjaxGrid<MultiExternalOutboundVM>)new AjaxGridFactory().CreateAjaxGrid(MultiExternalOutboundVMs, 1, MultiExternalOutboundVMs.Count, true);
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_MultiExternalGridPartial", (AjaxGrid<MultiExternalOutboundVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(MultiExternalOutboundVMs, 1, MultiExternalOutboundVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }


        }
        [HttpPost]
        public ActionResult AddMultiInternal([Bind(Prefix = "MultiInternalOutbound")] MultiInternalOutboundVM MultiInternalOutbound, List<MultiInternalOutboundVM> MultipleInternalOutbound)
        {
            try
            {
                string message = string.Empty;
                List<int> result = !string.IsNullOrWhiteSpace(MultiInternalOutbound.InternalOrgSelectedList) ? MultiInternalOutbound.InternalOrgSelectedList.Split(',').Select(x => int.Parse(x)).ToList() : new List<int>();
                List<MultiInternalOutboundVM> MultiInternalOutboundVMs = new List<MultiInternalOutboundVM>();
                MultipleInternalOutbound = MultipleInternalOutbound ?? new List<MultiInternalOutboundVM>();
                int partiesCount = MultipleInternalOutbound.Count;
                bool isHasNew = false;
                isHasNew = result.Any(x => !MultipleInternalOutbound.Any(c => c.OrgUnitId == x));
                if (!isHasNew)
                {
                    message = DbRes.TValidation("User.Transaction.Copy.OrganizationUnitAlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }


                foreach (var orgUnitId in result)
                {

                    MultiInternalOutboundVM newMultiInternalOutbound = new MultiInternalOutboundVM
                    {
                        OrgUnitId = orgUnitId,
                        Id = 0
                    };

                    if (!MultipleInternalOutbound.Any(x => x.OrgUnitId == MultiInternalOutbound.OrgUnitId))
                    {
                        if ((!MultipleInternalOutbound.Any(x => x.OrgUnitId == MultiInternalOutbound.OrgUnitId)))
                        {

                            newMultiInternalOutbound.Id = 0;
                            newMultiInternalOutbound.Key = partiesCount + 1;
                            if (newMultiInternalOutbound.OrgUnitId > 0)
                            {
                                var orgUnitDTO = OrgHelper.GetOrgUnit(newMultiInternalOutbound.OrgUnitId, SessionInfo.CultureShortName);
                                newMultiInternalOutbound.OrgUnitName = orgUnitDTO.Name;


                            }
                            MultiInternalOutboundVMs.Add(newMultiInternalOutbound);
                            partiesCount++;
                        }

                    }

                    if (MultiInternalOutbound.OrgUnitId > 0)
                    {
                        var orgUnitDTO = OrgHelper.GetOrgUnit(MultiInternalOutbound.OrgUnitId, SessionInfo.CultureShortName);
                        MultiInternalOutbound.OrgUnitName = orgUnitDTO.Name;
                    }

                }
                IAjaxGrid grid = (AjaxGrid<MultiInternalOutboundVM>)new AjaxGridFactory().CreateAjaxGrid(MultiInternalOutboundVMs, 1, MultiInternalOutboundVMs.Count, true);
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_MultiInternalGridPartial", (AjaxGrid<MultiInternalOutboundVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(MultiInternalOutboundVMs, 1, MultiInternalOutboundVMs.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }


        }
        [HttpPost]
        public ActionResult DeleteExternal(string OrgUnit)
        {
            string path = SystemConfigurations.ExternalCopiesAttachmentPath;
            var filteredByFilename = Directory.GetFiles(path).Select(o => Path.GetFileName(o)).Where(o => o.StartsWith($"{StringUtility.ValidateId(OrgUnit)} _"));
            foreach (var item in filteredByFilename)
            {
                if (System.IO.File.Exists(path + item))
                {
                    System.IO.File.Delete(path + item);
                }
            }
            return Json(new
            {
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }

        public List<TransactionLinkVM> GetTransactionLinks(int transactionId)
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

        #region Transaction Encryption
        [HttpPost]
        public ActionResult ShowVerifyCodeBox(int transactionId, int TransactionCategoryId, string Mode)
        {
            try
            {
                string message = string.Empty;
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.OpenEncryptTransaction))
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                VerificationCodeVM verificationCodeVM = new VerificationCodeVM()
                {

                    TransactionId = transactionId,
                    TransactionCategoryId = TransactionCategoryId,
                    Mode = Mode,
                    CodeExpirationDuration = Convert.ToInt32(SystemConfigurations.CodeExpirationDuration)
                };
                return Json(new
                {
                    Count = 1,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/VerificationCode.cshtml", verificationCodeVM),
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult SendTransactionEncryptionCode(int transactionId, int TransactionCategoryId)
        {
            try
            {
                string message = string.Empty;
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.OpenEncryptTransaction))
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                var HashedCode = HasheCodeGenerater.GenerateHashCode();
                DateTime CodeExpireDate = DateTime.Now.AddSeconds(Convert.ToInt32(SystemConfigurations.CodeExpirationDuration));
                // set hashed code to user sesstion 
                CodeDetials codeDetials = new CodeDetials()
                {
                    TransactionID = transactionId,
                    HashedCode = HashedCode,
                    CodeExpireDate = CodeExpireDate
                };

                SessionInfo.SetObjectInSession(codeDetials, Constants.GeneralSettings.TransactionHashedCode);
                TransactionEncryptionCodeDTO transactionEncryptionCodeDTO = new TransactionEncryptionCodeDTO()
                {

                    TransactionId = transactionId,
                    Code = HashedCode,
                    UserId = SessionInfo.CurrentUser.Id,
                    OrgUnitId = SessionInfo.OrgUnitId,
                    EncryptionChannel = EncryptionChannel.Email,
                    CreatedBy = SessionInfo.CurrentUser.Id,
                    CreatedOn = DateTime.Now,
                    CodeExpireDate = CodeExpireDate

                };



                PostResult postResult =
           HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/SendTransactionEncryptionCode?cultureName={0}", SessionInfo.CultureShortName), transactionEncryptionCodeDTO).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                VerificationCodeVM verificationCodeVM = new VerificationCodeVM()
                {

                    TransactionId = transactionId,
                    TransactionCategoryId = TransactionCategoryId
                };
                return Json(new
                {
                    Count = 1,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/File/VerificationCode.cshtml", verificationCodeVM),
                }, JsonRequestBehavior.AllowGet); ;


            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult VerifyCode(int transactionId, string UserVerifyCode)
        {
            try
            {
                string message = string.Empty;
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.OpenEncryptTransaction))
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                CodeDetials codeDetials = SessionInfo.GetObjectFromSession(Constants.GeneralSettings.TransactionHashedCode) as CodeDetials;

                if (codeDetials.TransactionID == transactionId
                    && codeDetials.HashedCode.ToLower() == UserVerifyCode.ToLower()
                    && codeDetials.CodeExpireDate >= DateTime.Now)
                {
                    return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = transactionId.ToString() }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.VerificationCode.CodeIsExpiredOrInvalid");
                    return Json(new { MessageText = message, MessageType = MessageType.Error, TransactionId = transactionId.ToString() }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult ReSendTransactionEncryptionCode(int transactionId, int TransactionCategoryId)
        {
            try
            {
                string message = string.Empty;
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.OpenEncryptTransaction))
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, "");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                var HashedCode = HasheCodeGenerater.GenerateHashCode();
                DateTime CodeExpireDate = DateTime.Now.AddMinutes(5);
                // set hashed code to user sesstion 
                CodeDetials codeDetials = new CodeDetials()
                {
                    TransactionID = transactionId,
                    HashedCode = HashedCode,
                    CodeExpireDate = CodeExpireDate
                };

                SessionInfo.SetObjectInSession(codeDetials, Constants.GeneralSettings.TransactionHashedCode);
                TransactionEncryptionCodeDTO transactionEncryptionCodeDTO = new TransactionEncryptionCodeDTO()
                {

                    TransactionId = transactionId,
                    Code = HashedCode,
                    UserId = SessionInfo.CurrentUser.Id,
                    OrgUnitId = SessionInfo.OrgUnitId,
                    EncryptionChannel = EncryptionChannel.Email,
                    CreatedBy = SessionInfo.CurrentUser.Id,
                    CreatedOn = DateTime.Now,
                    CodeExpireDate = CodeExpireDate

                };


                PostResult postResult =
           HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/SendTransactionEncryptionCode?cultureName={0}", SessionInfo.CultureShortName), transactionEncryptionCodeDTO).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = DbRes.TResource("User.Transaction.VerificationCode.SendCodeSuccessfully");
                return Json(new { MessageText = message, MessageType = MessageType.Information, TransactionId = transactionId.ToString() }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region Function
        protected void SetTransactionAssignmentToViewed(int transactionId)
        {
            string message = string.Empty;
            PutResult putResult =
                HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Transaction/SetTransactionAssignmentToViewedByTransactionId?transactionId={0}", transactionId), null).Result;
            if (putResult.StatusCode != StatusCode.Ok)
            {
                throw new Exception(putResult.StatusCode.ToString());
            }
        }
        #endregion
    }
}
