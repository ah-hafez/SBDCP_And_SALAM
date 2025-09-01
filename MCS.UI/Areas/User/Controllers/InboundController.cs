using DocumentFormat.OpenXml.Drawing.Charts;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Action;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Inbound;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Notifications;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Common;
using MCS.UI.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static MCS.Common.UserClaims;


namespace MCS.UI.Areas.User
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    //[CustomAuthorizationAttribute(UserClaims.Inbound.DisplayInbound)]
    public class InboundController : TransactionController
    {
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Inbound.CreateInbound)]
        public ActionResult Add()
        {
            try
            {

                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                AddInboundVM inboundAddVM = new AddInboundVM();
                Initialize(inboundAddVM.Type);
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
                var externalPartyVMs = ExternalPartyMapper.Map(parties);
                ViewData["ExternalPartiesData"] = parties != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;
                ViewData["InboundDocumentNumber"] = DateTime.Now.ToString("M/d/yyyy");
                ViewData["DeliveryMethod"] = GetDelivery();

                // ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);
                //ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitIdInbound(SessionInfo.OrgUnitId, true);
                inboundAddVM.InboundBasicInfo.DirectedToId = SessionInfo.CurrentUser.Id;
                inboundAddVM.InboundBasicInfo.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                inboundAddVM.InboundBasicInfo.ConfidentialityList = GetConfidentialityLevel();
                inboundAddVM.InboundBasicInfo.InboundDateH = DateTimeUtility.ConvertToUmAlQuraCalendar_NewFormat(DateTime.Now);
                Session["IsEditMode"] = true;
                ViewData["IsEditMode"] = true;
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();


                return View(inboundAddVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.Inbound.CreateInbound)]
        public ActionResult AddInbound(AddInboundVM inboundAddVM, string hdnMainDocToken)
        {
            try
            {
                string message = string.Empty;
                if (inboundAddVM.InboundBasicInfo.IsForIndividual == true)
                {
                    inboundAddVM.InboundBasicInfo.DestinationId = null;
                }

                if (inboundAddVM.InboundBasicInfo.DirectedToId == -1)
                {
                    inboundAddVM.InboundBasicInfo.DirectedToId = null;
                }


                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.GeneralPermissions.AdministrationCreated) && inboundAddVM.InboundBasicInfo.OriginatorOrgUnitId > 0)
                {
                    inboundAddVM.OrgUnitId = inboundAddVM.InboundBasicInfo.OriginatorOrgUnitId;
                }
                else
                {
                    inboundAddVM.OrgUnitId = SessionInfo.OrgUnitId;
                }
                //Main Document
                byte[] data = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);

                inboundAddVM.DocumentVM = new DocumentVM
                {
                    Content = data,
                    Size = data.Length,
                    MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf,
                    FromUserId = SessionInfo.CurrentUser.Id,
                    FromEntityId = SessionInfo.OrgUnitId
                };


                //if (documentData != null && documentData.Count != 0)
                //{
                //    inboundAddVM.Archives.ForEach(t =>
                //    {
                //        if (!t.IsMainDocument && t.IsNew)
                //        {
                //            inboundAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM = new DocumentVM();
                //            inboundAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Content = documentData[t.Id];
                //            inboundAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Size = documentData[t.Id].Length;
                //            inboundAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                //        }
                //    });
                //}

                ViewData["SettingCity"] = GetCitySetting();
                var documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                inboundAddVM.Attachments = new List<TransactionAttachmentVM>();
                inboundAddVM.Attachments = FillTransactionAttachment(inboundAddVM.Archives, documentData);//fill attachments

                var prefix = string.Empty;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    prefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
                }
                else
                {
                    prefix = "_" + SessionInfo.CurrentUser.Id + "_";
                }
                if (inboundAddVM.ExternalCopies != null && inboundAddVM.ExternalCopies.Any())
                {
                    foreach (TransactionExternalCopyVM transactionExternalCopy in inboundAddVM.ExternalCopies)
                    {
                        string path = SystemConfigurations.ExternalCopiesAttachmentPath;
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
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
                                    MimeType = mimeType,
                                    Content = fileContent,
                                    Size = size,
                                    Name = name,
                                    FromUserId = SessionInfo.CurrentUser.Id,
                                    FromEntityId = SessionInfo.OrgUnitId
                                }
                            });

                            f.Delete();
                        }
                        transactionExternalCopy.externalPartyAttachmentVMs = externalPartyAttachmentVMs;
                    }
                }

                Session["DocumentData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;

                DistributionListVM distributionList = new DistributionListVM();

                if (inboundAddVM.InboundBasicInfo.DistrubutionListId != null)
                {
                    GetResult<DistributionListDTO> distributionListDTO =
                             HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, inboundAddVM.InboundBasicInfo.DistrubutionListId.Value)).Result;

                    distributionList = DistributionListMapper.Map(distributionListDTO.Result);
                }

                List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                if (distributionList.DistributionListDetails != null)
                {
                    foreach (var item in distributionList.DistributionListDetails)
                    {
                        if (!inboundAddVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                        {
                            if ((item.UserId != 0 && !inboundAddVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                               (item.UserId == 0 && !inboundAddVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
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

                    inboundAddVM.Copies.AddRange(Copies);
                }
                inboundAddVM.InboundBasicInfo.DirectedToId = SessionInfo.CurrentUser.Id;
                //inboundAddVM.InboundBasicInfo.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                AddInboundDTO addInbound = AddInboundMapper.Map(inboundAddVM);


                var postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addInbound).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                TempData["TransactionId"] = TransactionDetailsMapper.Map(TransactionDetailsMapper.Map(postResult.Result)).Id;
                var id = TempData["TransactionId"];
                //PostResult postResultFodept =
                //           HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/getFollowUpDepartment?EntityId={0}", SessionInfo.OrgUnitId), null).Result;


                //if (postResultFodept.Id.HasValue)
                //{
                //    int DefualtProcess = Convert.ToInt32(ConfigurationManager.AppSettings["DefualtFollowUpProcess"] ?? "1");

                //    TransactionFollowUpVM followVM = new TransactionFollowUpVM();
                //    followVM.FollowUpStatusId = (int)FollowupStatus.New;
                //    followVM.CreationDate = DateTime.Now;
                //    followVM.Active = true;
                //    followVM.CreatingUserId = SessionInfo.CurrentUser.Id;
                //    followVM.CreatingEntityId = SessionInfo.OrgUnitId;
                //    followVM.FollowUpProccessId = DefualtProcess;
                //    followVM.FollowUpTypeId = (int)FollowupType.Public;
                //    followVM.TransactionId = Convert.ToInt32(id);
                //    followVM.FollowUpEntityId = (int)postResultFodept.Id;
                //    followVM.FollowUpExpireDate = DateTime.Now.AddDays(15);


                //    PostResult followuppostResult =
                //   HttpClientWrapper<PostResult>.PostRequest("api/Transaction/TransactionFollowUpAdd?cultureName=" + SessionInfo.CultureShortName, TransactionFollowUpMapper.Map(followVM)).Result;


                //    if (followuppostResult.StatusCode != StatusCode.Ok)
                //    {
                //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, followuppostResult.StatusCode.ToString());

                //        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //    }
                //    if (followuppostResult.Id.HasValue && followuppostResult.Id > 0)
                //    {
                //        FollowUpAuditTrailVM followUpAuditTrail = new FollowUpAuditTrailVM();
                //        followUpAuditTrail.FollowupId = (int)followuppostResult.Id;
                //        followUpAuditTrail.ProccessDate = DateTime.Now;
                //        followUpAuditTrail.ProccessId = (int)FollowupAuditProcess.AddPublicFollowup;
                //        followUpAuditTrail.ProccessDescription = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPublicFollowUp");
                //        followUpAuditTrail.UserId = SessionInfo.CurrentUser.Id;
                //        followUpAuditTrail.EntityId = SessionInfo.OrgUnitId;
                //        PostResult postResultAudit =
                //        HttpClientWrapper<PostResult>.PostRequest("api/Transaction/AddFollowupUditTrial?cultureName=" + SessionInfo.CultureShortName, FollowUpAuditTrailMapper.Map(followUpAuditTrail)).Result;

                //    }
                //}

                if (inboundAddVM.InboundBasicInfo.IsAcknowledged)
                {
                    PostResult postConfidentialityAcknowledgmentResult =
                        HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddConfidentialityAcknowledgment?TransactionId={0}&UserId={1}&OrgUnitId={2}", postResult.Result.Id, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId), null).Result;

                }
                if (addInbound.Links != null && addInbound.Links.Count > 0)
                {
                    foreach (TransactionLinkDTO link in addInbound.Links)
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

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Inbound.SaveSucceeded");

                if (addInbound.Names.Count != null && addInbound.Names.Count != 0)
                {
                    SmsHelper.SendSmsTransactionNumber(postResult.Result.Number, addInbound.Names[0].MobileNumber);
                }


                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    InboundNumber = postResult.Result.Number,
                    Id = id,
                    EncryptedId = AESEncrytDecry.Base64Encode(id.ToString()),
                    Date = (postResult.Result).HijriDate,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
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

        [CustomAuthorizationAttribute(UserClaims.Inbound.CreateInbound)]
        public ActionResult AddPrevious(string transactionId)
        {
            try
            {
                int? trxId = null;
                if (!string.IsNullOrWhiteSpace(transactionId))
                    trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                //ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                // Request.QueryString
                string message = string.Empty;


                string apiUrl = "api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}";
                if (trxId != null)
                {
                    apiUrl = "api/Transaction/GetPreviousTransactionByID?transactionsId=" + trxId + "&cultureName={0}&transactionCategory={1}&orgUnitId={2}";
                }

                GetResult<AddInboundDTO> inboundAddDTO =
                    HttpClientWrapper<GetResult<AddInboundDTO>>.GetItemRequest(String.Format(apiUrl, SessionInfo.CultureShortName, TransactionCategory.Inbound, SessionInfo.OrgUnitId)).Result;

                if (inboundAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, inboundAddDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (inboundAddDTO.Result == null)
                {
                    message = DbRes.TResource("User.Inbound.NoPreviousDataInfoMsg");

                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }

                Initialize(TransactionCategory.Inbound);

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo.DirectedToOrgUnitId);

                if (AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo.DestinationId.HasValue)
                {
                    int destinationId = AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo.DestinationId.Value;
                    ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(destinationId);
                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo.DirectedToOrgUnitId);


                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                if (AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo.DestinationId.HasValue)
                {
                    int destinationId = AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo.DestinationId.Value;
                    ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), destinationId);
                }

                //        GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //        if (subjectClassificationDTOs.Result != null && inboundAddDTO.Result.InboundBasicInfo.SubjectClassifications != null)
                //        {
                //            inboundAddDTO.Result.InboundBasicInfo.SubjectClassifications.ForEach(s =>
                //            {
                //                if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //                {
                //                    subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //                }
                //            });
                //        }

                //        ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && inboundAddDTO.Result.InboundBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == inboundAddDTO.Result.InboundBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == inboundAddDTO.Result.InboundBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

                ViewData.TemplateInfo.HtmlFieldPrefix = "InboundBasicInfo";

                return View("~/Areas/User/Views/Inbound/Add.cshtml", AddInboundMapper.Map(inboundAddDTO.Result));

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorization(UserClaims.Inbound.EditInbound, UserClaims.Inbound.Editor)]
        [CustomAction]
        public ActionResult Edit(string id, string defaultTabId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));

                var editInboundDTO = HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;

                if (editInboundDTO.StatusCode.ToString().Contains("Permission"))
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                else if (editInboundDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                else if (editInboundDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString()));
                }
                SetTransactionAssignmentToViewed(trxId);
                var editInboundVM = EditInboundMapper.Map(editInboundDTO.Result);

                //
                //remove Blind Carbon Copy from list 

                ViewData["InternalCopiesData"] =
                    editInboundVM?.Copies != null ? editInboundVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    editInboundVM?.ExternalCopies != null ? editInboundVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();




                IList<LookupVM> Yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (editInboundVM.Links != null && editInboundVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM item in editInboundVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = Yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                Initialize(TransactionCategory.Inbound);

                ViewData["OrgUnitsUsersData"] = editInboundVM.InboundBasicInfoEdit.DirectedToOrgUnitId > 0 ? GetUsersByOrgUnitIdInbound(editInboundVM.InboundBasicInfoEdit.DirectedToOrgUnitId, true) :
                GetUsersByOrgUnitIdInbound(SessionInfo.OrgUnitId, true);
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(TransactionCategory.Inbound);
                if (editInboundVM.InboundBasicInfoEdit.DestinationId.HasValue)
                {
                    int destiantionId = editInboundVM.InboundBasicInfoEdit.DestinationId.Value;
                    ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(destiantionId);
                }
                ViewData["transactionId"] = editInboundVM.Id;

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

                editInboundVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();


                List<TransactionArchiveVM> transactionArchiveIncVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == false).ToList();
                List<TransactionArchiveVM> transactionArchiveMainVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList();
                editInboundVM.Archives = transactionArchiveIncVMs;

                IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveMainVMs, 1, transactionArchiveMainVMs.Count, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                IAjaxGrid gridTasks = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, true);
                ViewData["TasksGridData"] = gridTasks;

                ViewData["CurrentTransactionTasksGrid"] = GetCurrentTransactionTasks(trxId);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(editInboundVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(editInboundVM.ExternalCopies);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(editInboundVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(editInboundVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(editInboundVM.Links);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveIncVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveMainVMs);
                ViewData["ArchiveListData"] =
                    editInboundVM?.Archives != null ? editInboundVM.Archives.ToList() : new List<TransactionArchiveVM>();
                if (editInboundVM.InboundBasicInfoEdit.DestinationId.HasValue)
                {
                    int destinationId = editInboundVM.InboundBasicInfoEdit.DestinationId.Value;
                }


                Session["BarcodeImgByte"] = GetBarcodeImage(editInboundVM.Id, true);
                LogTransactionAction(AuditingActionCode.UpadteTransaction, editInboundVM.Id);
                ViewData["WithBarcode"] = true;
                ViewData["ControllerName"] = "Inbound";
                //ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();



                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = editInboundDTO.Result.FromUser.LocalName == editInboundDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;



                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();


                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                       .GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}",
                                SessionInfo.OrgUnitId,
                                SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);

                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                ViewData["UsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);




                ViewData["ControllerName"] = "Inbound";


                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                taskAddVM.SentToOrgUnitId = SessionInfo.OrgUnitId;
                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["hdnTaskArray"] = JsonConvert.SerializeObject(gridData);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);



                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == editInboundVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Inbound.EditInbound));


                #region Add value to key Field

                for (int i = 0; i < editInboundVM.Attachments.Count; i++)
                {
                    editInboundVM.Attachments[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Archives.Count; i++)
                {
                    editInboundVM.Archives[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Copies.Count; i++)
                {
                    editInboundVM.Copies[i].Key = i + 1;
                }
                for (int i = 0; i < editInboundVM.ExternalCopies.Count; i++)
                {
                    editInboundVM.ExternalCopies[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Names.Count; i++)
                {
                    editInboundVM.Names[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Links.Count; i++)
                {
                    editInboundVM.Links[i].Key = i + 1;
                }

                #endregion


                RemoveAllAttachemntsPhysically();

                InitializerAssignmentPaperData(editInboundVM.Id);







                editInboundVM.defaultTabId = defaultTabId;
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
                var externalPartyVMs = ExternalPartyMapper.Map(parties);
                ViewData["ExternalPartiesData"] = parties != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();



                if (editInboundVM.Copies != null && editInboundVM.Copies.Count > 0)
                    editInboundVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));

                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(editInboundVM.SavedTransactionAssignment) ?
            JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(editInboundVM.SavedTransactionAssignment) : transactionAssignmentVMs;


                Session["IsEditMode"] = true;
                ViewData["IsEditMode"] = true;
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/Inbound/Edit.cshtml", editInboundVM);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public ActionResult Editor(string id, string defaultTabId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                var editInboundDTO = HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();
                if (editInboundDTO.StatusCode.ToString().Contains("Permission"))
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                else if (editInboundDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                else if (editInboundDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString()));
                }
                SetTransactionAssignmentToViewed(trxId);
                var editInboundVM = EditInboundMapper.Map(editInboundDTO.Result);
                // editInboundVM.HijriRecordDate = StringUtility.ValidateDate(editInboundVM.HijriRecordDate);

                //remove Blind Carbon Copy from list 
                //editInboundVM.Copies.RemoveAll(x => x.IsBcc == true);

                IList<LookupVM> Yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (editInboundVM.Links != null && editInboundVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM item in editInboundVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = Yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                Initialize(TransactionCategory.Inbound);
                //InitializeOutboundDraftReadWrite(id);

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId, true);
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(TransactionCategory.Inbound);
                if (editInboundVM.InboundBasicInfoEdit.DestinationId.HasValue)
                {
                    int destiantionId = editInboundVM.InboundBasicInfoEdit.DestinationId.Value;
                    ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(destiantionId);
                }
                ViewData["transactionId"] = editInboundVM.Id;

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
                        ReadOnly = true
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

                editInboundVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();


                List<TransactionArchiveVM> transactionArchiveIncVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == false).ToList();
                List<TransactionArchiveVM> transactionArchiveMainVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList();
                editInboundVM.Archives = transactionArchiveIncVMs;

                IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveMainVMs, 1, transactionArchiveMainVMs.Count, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                IAjaxGrid gridTasks = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, true);
                ViewData["TasksGridData"] = gridTasks;

                ViewData["CurrentTransactionTasksGrid"] = GetCurrentTransactionTasks(trxId);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(editInboundVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(editInboundVM.ExternalCopies);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(editInboundVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(editInboundVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(editInboundVM.Links);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveIncVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveMainVMs);
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["ActionData"] = GetActions();
                ViewData["ArchiveListData"] =
                   editInboundVM?.Archives != null ? editInboundVM.Archives.ToList() : new List<TransactionArchiveVM>();
                if (editInboundVM.InboundBasicInfoEdit.DestinationId.HasValue)
                {
                    int destinationId = editInboundVM.InboundBasicInfoEdit.DestinationId.Value;
                }


                if (editInboundVM.InboundBasicInfoEdit.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }

                Session["BarcodeImgByte"] = GetBarcodeImage(editInboundVM.Id, true);
                LogTransactionAction(AuditingActionCode.OpenEditor, editInboundVM.Id);
                ViewData["WithBarcode"] = true;
                ViewData["ControllerName"] = "Inbound";
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();

                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = editInboundDTO.Result.FromUser.LocalName == editInboundDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;


                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData; ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                editInboundVM.InboundBasicInfoEdit.DirectedToId = SessionInfo.CurrentUser.Id;
                editInboundVM.InboundBasicInfoEdit.DirectedToOrgUnitId = SessionInfo.OrgUnitId;



                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);

                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                ViewData["UsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);



                //  ViewData["transactionId"] = 472;
                ViewData["IsEditMode"] = false;
                ViewData["ControllerName"] = "Editor";

                //IAjaxGrid grid = GetTransactionTasks(id);

                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                taskAddVM.SentToOrgUnitId = SessionInfo.OrgUnitId;
                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["hdnTaskArray"] = JsonConvert.SerializeObject(gridData);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);
                /*gridData.Where(g => g.StatusId == (int)TaskStatus.Complete).ToList();*/

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);

                // return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_AddTaskPartial.cshtml", taskAddVM), GridHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid) }, JsonRequestBehavior.AllowGet);


                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == editInboundVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Inbound.EditInbound));


                #region Add value to key Field

                for (int i = 0; i < editInboundVM.Attachments.Count; i++)
                {
                    editInboundVM.Attachments[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Archives.Count; i++)
                {
                    editInboundVM.Archives[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Copies.Count; i++)
                {
                    editInboundVM.Copies[i].Key = i + 1;
                }
                for (int i = 0; i < editInboundVM.ExternalCopies.Count; i++)
                {
                    editInboundVM.ExternalCopies[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Names.Count; i++)
                {
                    editInboundVM.Names[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Links.Count; i++)
                {
                    editInboundVM.Links[i].Key = i + 1;
                }

                #endregion


                RemoveAllAttachemntsPhysically();

                editInboundVM.defaultTabId = defaultTabId;
                ViewData["TransactionId"] = trxId;
                ViewData["ConfidentialityName"] = editInboundVM.InboundBasicInfoEdit.ConfidentialityLevelText;
                ViewData["InternalCopiesData"] =
                   editInboundVM?.Copies != null ? editInboundVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    editInboundVM?.ExternalCopies != null ? editInboundVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();
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

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();

                editInboundVM.TransactionAssignmentVM.ToFollowUp = false;
                if (editInboundVM.Copies != null && editInboundVM.Copies.Count > 0)
                    editInboundVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));

                InitializerAssignmentPaperData(editInboundVM.Id);

                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(editInboundVM.SavedTransactionAssignment) ?
          JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(editInboundVM.SavedTransactionAssignment) : transactionAssignmentVMs;
                //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);
                Session["TransactionId"] = trxId;
                Session["IsEditMode"] = false;
                ViewData["IsEditMode"] = true;
                return View("~/Areas/User/Views/Inbound/Editor.cshtml", editInboundVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult NotificationEditor(string id, string defaultTabId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.Decrypt(id.Replace(" ", "+")));
                var editInboundDTO = HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();
                if (editInboundDTO.StatusCode.ToString().Contains("Permission"))
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                    return RedirectToAction("Unauthorized", "Error", new { area = "User", controller = "Error", action = "Unauthorized" });
                }
                else if (editInboundDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                else if (editInboundDTO.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editInboundDTO.StatusCode.ToString()));
                }
                SetTransactionAssignmentToViewed(trxId);
                var editInboundVM = EditInboundMapper.Map(editInboundDTO.Result);
                // editInboundVM.HijriRecordDate = StringUtility.ValidateDate(editInboundVM.HijriRecordDate);

                //remove Blind Carbon Copy from list 
                //editInboundVM.Copies.RemoveAll(x => x.IsBcc == true);

                IList<LookupVM> Yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (editInboundVM.Links != null && editInboundVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM item in editInboundVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = Yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                Initialize(TransactionCategory.Inbound);
                //InitializeOutboundDraftReadWrite(id);

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId, true);
                ViewData["TransactionTypesData"] = TransactionHelper.GetTransactionTypes(TransactionCategory.Inbound);
                if (editInboundVM.InboundBasicInfoEdit.DestinationId.HasValue)
                {
                    int destiantionId = editInboundVM.InboundBasicInfoEdit.DestinationId.Value;
                    ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(destiantionId);
                }
                ViewData["transactionId"] = editInboundVM.Id;
                var actionVMs = GetAllActionsValues();
                IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

                actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

                ViewData["AllActionsData2"] = actionVMs;
                ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();

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
                        ReadOnly = true
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

                editInboundVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();


                List<TransactionArchiveVM> transactionArchiveIncVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == false).ToList();
                List<TransactionArchiveVM> transactionArchiveMainVMs = transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList();
                editInboundVM.Archives = transactionArchiveIncVMs;

                IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveMainVMs, 1, transactionArchiveMainVMs.Count, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                IAjaxGrid gridTasks = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TaskAddVM>(), 1, 0, true);
                ViewData["TasksGridData"] = gridTasks;

                ViewData["CurrentTransactionTasksGrid"] = GetCurrentTransactionTasks(trxId);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(editInboundVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(editInboundVM.ExternalCopies);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(editInboundVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(editInboundVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(editInboundVM.Links);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveIncVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveMainVMs);
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["ActionData"] = GetActions();
                ViewData["ArchiveListData"] =
                   editInboundVM?.Archives != null ? editInboundVM.Archives.ToList() : new List<TransactionArchiveVM>();
                if (editInboundVM.InboundBasicInfoEdit.DestinationId.HasValue)
                {
                    int destinationId = editInboundVM.InboundBasicInfoEdit.DestinationId.Value;
                }


                if (editInboundVM.InboundBasicInfoEdit.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }

                Session["BarcodeImgByte"] = GetBarcodeImage(editInboundVM.Id, true);
                LogTransactionAction(AuditingActionCode.OpenEditor, editInboundVM.Id);
                ViewData["WithBarcode"] = true;
                ViewData["ControllerName"] = "Inbound";
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();

                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = editInboundDTO.Result.FromUser.LocalName == editInboundDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;


                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData; ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                editInboundVM.InboundBasicInfoEdit.DirectedToId = SessionInfo.CurrentUser.Id;
                editInboundVM.InboundBasicInfoEdit.DirectedToOrgUnitId = SessionInfo.OrgUnitId;



                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);

                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                ViewData["UsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);



                //  ViewData["transactionId"] = 472;
                ViewData["IsEditMode"] = false;
                ViewData["ControllerName"] = "Editor";

                //IAjaxGrid grid = GetTransactionTasks(id);

                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                taskAddVM.SentToOrgUnitId = SessionInfo.OrgUnitId;
                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["hdnTaskArray"] = JsonConvert.SerializeObject(gridData);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);
                /*gridData.Where(g => g.StatusId == (int)TaskStatus.Complete).ToList();*/

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);

                // return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_AddTaskPartial.cshtml", taskAddVM), GridHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid) }, JsonRequestBehavior.AllowGet);


                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == editInboundVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Inbound.EditInbound));


                #region Add value to key Field

                for (int i = 0; i < editInboundVM.Attachments.Count; i++)
                {
                    editInboundVM.Attachments[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Archives.Count; i++)
                {
                    editInboundVM.Archives[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Copies.Count; i++)
                {
                    editInboundVM.Copies[i].Key = i + 1;
                }
                for (int i = 0; i < editInboundVM.ExternalCopies.Count; i++)
                {
                    editInboundVM.ExternalCopies[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Names.Count; i++)
                {
                    editInboundVM.Names[i].Key = i + 1;
                }

                for (int i = 0; i < editInboundVM.Links.Count; i++)
                {
                    editInboundVM.Links[i].Key = i + 1;
                }

                #endregion


                RemoveAllAttachemntsPhysically();

                editInboundVM.defaultTabId = defaultTabId;
                ViewData["TransactionId"] = trxId;
                ViewData["ConfidentialityName"] = editInboundVM.InboundBasicInfoEdit.ConfidentialityLevelText;
                ViewData["InternalCopiesData"] =
                   editInboundVM?.Copies != null ? editInboundVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    editInboundVM?.ExternalCopies != null ? editInboundVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();
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

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();

                editInboundVM.TransactionAssignmentVM.ToFollowUp = false;
                if (editInboundVM.Copies != null && editInboundVM.Copies.Count > 0)
                    editInboundVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);
                Session["TransactionId"] = trxId;

                Session["IsEditMode"] = false;
                ViewData["IsEditMode"] = true;
                return View("~/Areas/User/Views/Inbound/Editor.cshtml", editInboundVM);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Inbound.EditInbound, UserClaims.Inbound.Editor)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditInbound(EditInboundVM inboundEditVM, string hdnMainDocToken)
        {
            try
            {
                string message = string.Empty;
                inboundEditVM.OrgUnitId = SessionInfo.OrgUnitId;
                inboundEditVM.UserId = SessionInfo.CurrentUser.Id;

                if (inboundEditVM.InboundBasicInfoEdit.DirectedToId == -1)
                {
                    inboundEditVM.InboundBasicInfoEdit.DirectedToId = null;
                }

                //Main Document
                byte[] data = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);
                inboundEditVM.DocumentVM = new DocumentVM();
                if (data != null)
                {
                    inboundEditVM.DocumentVM.Content = data;
                    inboundEditVM.DocumentVM.Size = data.Length;
                    inboundEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    inboundEditVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    inboundEditVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
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
                if (inboundEditVM.ExternalCopies != null && inboundEditVM.ExternalCopies.Any())
                {
                    foreach (TransactionExternalCopyVM transactionExternalCopy in inboundEditVM.ExternalCopies)
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

                        foreach (var item in inboundEditVM.ExternalCopies)
                        {
                            if (item.attachmentNames != "" && item.attachmentNames != null)
                            {
                                var deletedAttachments = JsonConvert.DeserializeObject<List<ExternalPartyAttachmentVM>>(item.attachmentNames).Where(ex => ex.IsDeleted == true).ToList();
                                if (item.externalPartyAttachmentVMs == null)
                                {
                                    item.externalPartyAttachmentVMs = new List<ExternalPartyAttachmentVM>();
                                }
                                item.externalPartyAttachmentVMs.AddRange(deletedAttachments);
                            }
                        }
                    }
                }
                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                DocumentDTO docDTO = new DocumentDTO();


                inboundEditVM.Attachments = FillTransactionAttachment(inboundEditVM.Archives, documentData);

                var archiveList = inboundEditVM.Archives.Where(t => t.IsMainDocument == false).ToList();

                Session["DocumentData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                Session["DocoNutexplanations"] = null;
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["SettingCity"] = GetCitySetting();

                DistributionListVM distributionList = new DistributionListVM();

                if (inboundEditVM.InboundBasicInfoEdit.DistrubutionListId != null)
                {
                    GetResult<DistributionListDTO> distributionListDTO =
                             HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, inboundEditVM.InboundBasicInfoEdit.DistrubutionListId.Value)).Result;

                    distributionList = DistributionListMapper.Map(distributionListDTO.Result);
                }

                List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                if (distributionList.DistributionListDetails != null)
                {
                    foreach (var item in distributionList.DistributionListDetails)
                    {
                        if (!inboundEditVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                        {
                            if ((item.UserId != 0 && !inboundEditVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                               (item.UserId == 0 && !inboundEditVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
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

                    inboundEditVM.Copies.AddRange(Copies);
                }

                //inboundEditVM.InboundBasicInfoEdit.DirectedToId = SessionInfo.CurrentUser.Id;
                //inboundEditVM.InboundBasicInfoEdit.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                #region Update transaction inbound 
                var EditedInbound = EditInboundMapper.Map(inboundEditVM);


                

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransaction?cultureName=" + SessionInfo.CultureShortName, EditedInbound).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (EditedInbound.Links != null && EditedInbound.Links.Count > 0)
                {

                    foreach (TransactionLinkDTO link in EditedInbound.Links)
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
                #endregion



                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Inbound.UpdateSucceeded");
                return Json(new
                {
                    InboundNumber = inboundEditVM.InboundBasicInfoEdit.InboundNumber,
                    Id = inboundEditVM.Id,
                    Date = inboundEditVM.HijriRecordDate,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt"),
                    EncryptedId = AESEncrytDecry.Base64Encode(inboundEditVM.Id.ToString()),
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
        public ActionResult LogicallyRemoveAttachemnt(List<ExternalPartyAttachmentVM> externalPartyAttachmentVMs)
        {
            return Json(new { });
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Inbound.CreateInbound)]
        public ActionResult GetPreviousInbound(bool hdnIsForIndividual)
        {
            try
            {
                string message = string.Empty;

                ViewData["HijriDate"] = string.Empty;
                ViewData["HijriDateTitle"] = DbRes.TResource("User.Inbound.BasicInfo.RecordDate");
                ViewData["TransactionNumber"] = string.Empty;
                ViewData["TransactionNumberTitle"] = DbRes.TResource("User.Inbound.BasicInfo.InboundNumber");

                GetResult<AddInboundDTO> inboundAddDTO =
                  HttpClientWrapper<GetResult<AddInboundDTO>>.GetItemRequest(String.Format("api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}&IsForIndividual={3}", SessionInfo.CultureShortName, TransactionCategory.Inbound, SessionInfo.OrgUnitId, hdnIsForIndividual)).Result;

                if (inboundAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, inboundAddDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (inboundAddDTO.Result == null)
                {
                    message = DbRes.TResource("User.Inbound.NoPreviousDataInfoMsg");

                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }

                AddInboundVM addInboundVM = AddInboundMapper.Map(inboundAddDTO.Result);

                Initialize(TransactionCategory.Inbound);

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(addInboundVM.InboundBasicInfo.DirectedToOrgUnitId);

                if (addInboundVM.InboundBasicInfo.DestinationId.HasValue)
                {
                    int destinationId = addInboundVM.InboundBasicInfo.DestinationId.Value;
                    ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(destinationId);
                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), addInboundVM.InboundBasicInfo.DirectedToOrgUnitId);


                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                 HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                if (addInboundVM.InboundBasicInfo.DestinationId.HasValue)
                {
                    int destinationId = addInboundVM.InboundBasicInfo.DestinationId.Value;
                    ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), destinationId);
                }

                //        GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //        if (subjectClassificationDTOs.Result != null && addInboundVM.InboundBasicInfo.SubjectClassifications != null)
                //        {
                //            addInboundVM.InboundBasicInfo.SubjectClassifications.ForEach(s =>
                //            {
                //                if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //                {
                //                    subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //                }
                //            });
                //        }

                //        ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && addInboundVM.InboundBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == addInboundVM.InboundBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == addInboundVM.InboundBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

                ViewData.TemplateInfo.HtmlFieldPrefix = "InboundBasicInfo";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_BasicInfoAddPartial", AddInboundMapper.Map(inboundAddDTO.Result).InboundBasicInfo), UserHasTransactions = true, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Inbound.CreateInbound)]
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
        [CustomAuthorizationAttribute(UserClaims.Inbound.CreateInbound)]
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
                return null;//AddName(nameVM, hdnNames);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.AddExternalParty.AddExternalPartyForInbound)]
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
        [CustomAuthorizationAttribute(UserClaims.AddExternalParty.AddExternalPartyForInbound)]
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

                var externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                    .GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(externalPartyVMs, selectedParty);

                return PartialView("~/Areas/User/Views/Inbound/_ExternalPartiesPartial.cshtml", new InboundExternalPartiesVM() { DestinationId = selectedParty != -1 ? selectedParty : 1 });
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

        public override void InitializeExternalParties()
        {
            GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

            ViewData["ExternalPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result)) : null;
        }

        private static AjaxGrid<TaskAddVM> GetCurrentTransactionTasks(int transactionId)
        {
            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InMyTransactionTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            string pageSize = settingVM.Value;
            GetResult<List<TaskAddDTO>> taskDTOs =
               HttpClientWrapper<GetResult<List<TaskAddDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionTasks?transactionId={0}&PageIndex={1}&pageSize={2}&cultureName={3}", transactionId, 1, settingVM.Value, SessionInfo.CultureShortName)).Result;

            List<TaskAddVM> taskAddVMs = TaskAddMapper.Map(taskDTOs.Result);
            if (taskAddVMs == null)
            {
                taskAddVMs = new List<TaskAddVM>();
            }
            return (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddVMs, 1, (int)taskDTOs.RowsCount, true);
        }

        [HttpGet]
        public string AutoComplete_SearchUsersByOrgUnit(int? orgUnitId, string term)
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/SearchUsersByOrgUnitId?cultureName={0}&orgUnitId={1}&term={2}", SessionInfo.CultureShortName, orgUnitId, term)).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
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
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string AutoComplete_SearchUsersByUserOrgUnitOnly(int? orgUnitId, string term)
        {
            try
            {
                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.ModulesLevel.AllModules))
                {
                    orgUnitId = SessionInfo.OrgUnitId;
                }
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/SearchUsersByOrgUnitId?cultureName={0}&orgUnitId={1}&term={2}", SessionInfo.CultureShortName, orgUnitId, term)).Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
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
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetAllowedUserAssignment(int FromUserId)
        {
            try
            {
                string message = string.Empty;
                bool isUserAllowAssignment = false;
                int selectedUserAllowedAssignmentCount = 0;
                GetResult<List<AllowedAssignmentDTO>> selectedUserAllowedAssignmentDTOs =
                HttpClientWrapper<GetResult<List<AllowedAssignmentDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAllowedAssignment?UserId={0}&cultureName={1}", FromUserId, SessionInfo.CultureShortName)).Result;
                selectedUserAllowedAssignmentCount = selectedUserAllowedAssignmentDTOs.Result != null ? selectedUserAllowedAssignmentDTOs.Result.Count : 0;
                GetResult<AllowedAssignmentDTO> AllowedUser = HttpClientWrapper<GetResult<AllowedAssignmentDTO>>.GetItemRequest(string.Format("api/UserProfile/GetAllowedUserAssignment?ToUserId={0}&FromUserId={1}", SessionInfo.CurrentUser.Id, FromUserId)).Result;
                if (AllowedUser.Result != null)
                {
                    isUserAllowAssignment = true;

                }
                AllowedUser.Result = new AllowedAssignmentDTO(); ;

                message = "لا يمكنك الأحالة للموظف الذي تم اختياره";

                return Json(new { result = AllowedUser.Result, MessageText = message, isUserAllowAssignment = isUserAllowAssignment, selectedUserAllowedAssignmentCount = selectedUserAllowedAssignmentCount }, JsonRequestBehavior.AllowGet);

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
