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
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.Inbound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.External;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound.Internal;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Notifications;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Inbound;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Internal;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Common;
using MCS.UI.Helpers;
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.UserPreferences;
using DotnetDaddy.DocumentConfig;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Drawing;
using Font = System.Drawing.Font;
using MCS.DoconutMVC.Helpers;
using MCS.UI.Areas.User.Models.Actions;
using MCS.UI.Areas.User.Mappers.Action;

namespace MCS.UI.Areas.User.Controllers
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    //[CustomAuthorizationAttribute(UserClaims.Outbound.DisplayOutbound)]
    public class OutboundInternalController : TransactionController
    {


        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateInternalOutbound)]

        public ActionResult Add(bool IsMulti = false)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();
                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                AddOutboundInternalVM outboundInternalAddVM = new AddOutboundInternalVM();
                outboundInternalAddVM.OutboundInternalBasicInfoAdd.IsMulti = IsMulti;
                outboundInternalAddVM.EditorTypeId = (int)EditorType.Scanning;
                outboundInternalAddVM.DocumentVM = new DocumentVM();
                Initialize(TransactionCategory.InternalOutbound);
                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.ReadOnly = false;
                editorViewModel.EditorType = EditorType.Scanning;
                editorViewModel.Content = string.Empty;
                editorViewModel.IsShowWordAddIn = true;
                ViewData["EditorViewModel"] = editorViewModel;
                ViewData["hdnInternalArray"] = JsonConvert.SerializeObject(new List<TransactionCopyVM>());

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
                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);
                //ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId, true);
                outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId = SessionInfo.CurrentUser.Id;
                outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                outboundInternalAddVM.OutboundInternalBasicInfoAdd.IsAcknowledged = false;
                return View(outboundInternalAddVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateInternalOutbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOutboundInternal(AddOutboundInternalVM outboundInternalAddVM, TextEditorViewModel editorViewModel, string hdnMainDocToken)
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
                string message = string.Empty;

                outboundInternalAddVM.OrgUnitId = outboundInternalAddVM.OutboundInternalBasicInfoAdd.OriginatorOrgUnitId > 0 ? outboundInternalAddVM.OutboundInternalBasicInfoAdd.OriginatorOrgUnitId : SessionInfo.OrgUnitId;

                if(outboundInternalAddVM.MultiInternalOutbound != null)
                {
                    if (outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId == -1 && (string.IsNullOrEmpty(outboundInternalAddVM.MultiInternalOutbound.InternalOrgSelectedList)))
                    {
                        outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId = null;
                    }
                }
                else
                {
                    if (outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId == -1 )
                    {
                        outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId = null;
                    }
                }


                outboundInternalAddVM.DocumentVM = new DocumentVM();
                //Main Document

                //if (outboundInternalAddVM.IsSigned && editorViewModel.EditorType == EditorType.TextEditor)

                if (editorViewModel.EditorType == EditorType.TextEditor)
                {

                    byte[] data = DocumentViewerHelper.GetOfficeFile(editorViewModel.OfficeFileId);


                    outboundInternalAddVM.EditorTypeId = (int)EditorType.TextEditor;
                    outboundInternalAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    outboundInternalAddVM.DocumentVM.Content = data;
                    outboundInternalAddVM.DocumentVM.Size = data.Length;
                    outboundInternalAddVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundInternalAddVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;

                    DocumentViewerHelper.DeleteOfficeFile(editorViewModel.OfficeFileId);


                }
                else
                {
                    outboundInternalAddVM.EditorTypeId = (int)EditorType.Scanning;
                    byte[] data = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);
                    outboundInternalAddVM.DocumentVM = new DocumentVM();
                    outboundInternalAddVM.DocumentVM.Content = data;
                    outboundInternalAddVM.DocumentVM.Size = data.Length;
                    outboundInternalAddVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundInternalAddVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundInternalAddVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;

                }


                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                outboundInternalAddVM.Attachments = new List<TransactionAttachmentVM>();
                outboundInternalAddVM.Attachments = FillTransactionAttachment(outboundInternalAddVM.Archives, documentData);//fill attachments

                //if (doDocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;cumentData != null && documentData.Count != 0)
                //{
                //    outboundInternalAddVM.Archives.ForEach(t =>
                //    {
                //        if (!t.IsMainDocument && t.IsNew && t.Archivable)
                //        {
                //            outboundInternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM = new DocumentVM();
                //            outboundInternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Content = documentData[t.Id];
                //            outboundInternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Size = documentData[t.Id].Length;
                //            outboundInternalAddVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                //        }
                //    });
                //}
                var prefix = string.Empty;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    prefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
                }
                else
                {
                    prefix = "_" + SessionInfo.CurrentUser.Id + "_";
                }
                if (outboundInternalAddVM.ExternalCopies != null && outboundInternalAddVM.ExternalCopies.Any())
                {
                    foreach (TransactionExternalCopyVM transactionExternalCopy in outboundInternalAddVM.ExternalCopies)
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
                                    FromUserId = SessionInfo.CurrentUser.Id,
                                    FromEntityId = SessionInfo.OrgUnitId
                                }
                            });

                            f.Delete();
                        }
                        outboundInternalAddVM.ExternalCopies.ForEach(a => a.externalPartyAttachmentVMs = externalPartyAttachmentVMs);

                    }
                }
                if (outboundInternalAddVM.Id != 0)
                {
                    switch (outboundInternalAddVM.OutboundInternalBasicInfoAdd.CopyTypeId.Value.LookupInternalID(LookupCategory.TransactionStatus, SessionInfo.CultureShortName))
                    {
                        case (int)TransactionCategory.DraftOutbound:
                            {
                                GetResult<EditOutboundDraftDTO> draftOutboundDTO =
                HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={outboundInternalAddVM.Id}&cultureName={SessionInfo.CultureShortName}").Result;

                                outboundInternalAddVM.Links.Add(new TransactionLinkVM()
                                {
                                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                    LinkTypeId = (int)LinkType.ByOutboundNumber,
                                    OrgUnitId = draftOutboundDTO.Result.OrgUnitId,
                                    TransactionId = draftOutboundDTO.Result.Id,
                                    TransactionNumber = draftOutboundDTO.Result.OutboundDraftBasicInfo.DraftNumber.Value.ToString(),
                                    TransactionCategory = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    Year = draftOutboundDTO.Result.RecordDate.Year
                                });

                                break;
                            }
                        case (int)TransactionCategory.Inbound:
                            {
                                GetResult<EditInboundDTO> inboundDTO =
                HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={outboundInternalAddVM.Id}&cultureName={SessionInfo.CultureShortName}").Result;

                                outboundInternalAddVM.Links.Add(new TransactionLinkVM()
                                {
                                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                    LinkTypeId = (int)LinkType.ByOutboundNumber,
                                    OrgUnitId = inboundDTO.Result.OrgUnitId,
                                    TransactionId = inboundDTO.Result.Id,
                                    TransactionNumber = inboundDTO.Result.InboundBasicInfoEdit.InboundNumber.ToString(),
                                    TransactionCategory = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    Year = inboundDTO.Result.RecordDate.Year
                                });

                                break;
                            }
                        case (int)TransactionCategory.InternalOutbound:
                            {
                                GetResult<EditOutboundInternalDTO> internalOutboundDTO =
                HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={outboundInternalAddVM.Id}&cultureName={SessionInfo.CultureShortName}").Result;

                                outboundInternalAddVM.Links.Add(new TransactionLinkVM()
                                {
                                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                    LinkTypeId = (int)LinkType.ByOutboundNumber,
                                    OrgUnitId = internalOutboundDTO.Result.OrgUnitId,
                                    TransactionId = internalOutboundDTO.Result.Id,
                                    TransactionNumber = internalOutboundDTO.Result.OutboundInternalBasicInfoEdit.Number.ToString(),
                                    TransactionCategory = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    Year = internalOutboundDTO.Result.RecordDate.Year
                                });

                                break;
                            }
                        case (int)TransactionCategory.ExternalOutbound:
                            {
                                GetResult<EditOutboundExternalDTO> externalOutboundDTO =
                HttpClientWrapper<GetResult<EditOutboundExternalDTO>>.GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={outboundInternalAddVM.Id}&cultureName={SessionInfo.CultureShortName}").Result;

                                outboundInternalAddVM.Links.Add(new TransactionLinkVM()
                                {
                                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                    LinkTypeId = (int)LinkType.ByOutboundNumber,
                                    OrgUnitId = externalOutboundDTO.Result.OrgUnitId,
                                    TransactionId = externalOutboundDTO.Result.Id,
                                    TransactionNumber = externalOutboundDTO.Result.OutboundExternalBasicInfo.OutboundNumber.ToString(),
                                    TransactionCategory = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    Year = externalOutboundDTO.Result.RecordDate.Year
                                });

                                break;
                            }
                    }

                    outboundInternalAddVM.Id = 0;

                }

                DistributionListVM distributionList = new DistributionListVM();

                if (outboundInternalAddVM.OutboundInternalBasicInfoAdd.DistrubutionListId != null)
                {
                    GetResult<DistributionListDTO> distributionListDTO =
                      HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, outboundInternalAddVM.OutboundInternalBasicInfoAdd.DistrubutionListId.Value)).Result;

                    distributionList = DistributionListMapper.Map(distributionListDTO.Result);

                }
                List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                if (distributionList.DistributionListDetails != null)
                {
                    foreach (var item in distributionList.DistributionListDetails)
                    {
                        if (!outboundInternalAddVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                        {
                            if ((item.UserId != 0 && !outboundInternalAddVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                               (item.UserId == 0 && !outboundInternalAddVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
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

                    outboundInternalAddVM.Copies.AddRange(Copies);
                }
                //outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId = SessionInfo.CurrentUser.Id;
                //outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                var addOutboundInternalDTO = OutboundInternalMapper.Map(outboundInternalAddVM);
             
                    PostObjectResult<TransactionDetailsDTO> postResult = null;
                if (outboundInternalAddVM.MultiInternalOutbound != null)
                {
                    List<string> externalparties = new List<string>(outboundInternalAddVM.MultiInternalOutbound.InternalOrgSelectedList.Split(','));
                    //int MainInternalPartyId = Convert.ToInt32(externalparties.FirstOrDefault());
                    //addOutboundInternalDTO.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = Convert.ToInt32(MainInternalPartyId);
                    //externalparties.Remove(MainInternalPartyId.ToString());
                    //PostObjectResult<TransactionDetailsDTO> mainpostResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutboundInternalDTO).Result;
                    //if (mainpostResult.StatusCode != StatusCode.Ok)
                    //{
                    //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, mainpostResult.StatusCode.ToString());
                    //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    //}
                    foreach (var externalpartie in externalparties)
                    {
                        addOutboundInternalDTO.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = Convert.ToInt32(externalpartie);

                        postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>
                   .PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutboundInternalDTO).Result;


                    }


                }
                else
                {
                    postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>
                  .PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, addOutboundInternalDTO).Result;

                }



                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundInternalAddVM.OutboundInternalBasicInfoAdd.IsAcknowledged)
                {
                    PostResult postConfidentialityAcknowledgmentResult =
                        HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddConfidentialityAcknowledgment?TransactionId={0}&UserId={1}&OrgUnitId={2}", postResult.Result.Id, SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId), null).Result;

                }

                if (addOutboundInternalDTO.Links != null && addOutboundInternalDTO.Links.Count > 0)
                {
                    foreach (TransactionLinkDTO link in addOutboundInternalDTO.Links)
                    {
                        GetResult<TransactionDetailsDTO> transactionDetailsDTOResult = HttpClientWrapper<GetResult<TransactionDetailsDTO>>
                       .GetItemRequest(string.Format("api/Transaction/GetTransactionDetailsByTransactionId?transactionId={0}&cultureName={1}",
                       link.TransactionId,
                       SessionInfo.CultureShortName)).Result;
                        var User = transactionDetailsDTOResult.Result.ToUserId;
                        if(User == null)
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
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundInternal.SaveSucceeded");
                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    InternalNumber = postResult.Result.Number,
                    Id = postResult.Result.Id,
                    Date = postResult.Result.HijriDate,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt"),
                    IsPopularization = outboundInternalAddVM.OutboundInternalBasicInfoAdd.GroupId.HasValue,
                    EncryptedId = AESEncrytDecry.Base64Encode(postResult.Result.Id.ToString())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateInternalOutbound)]
        public ActionResult AddPrevious(string transactionId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int? trxId = null;
                if (!string.IsNullOrWhiteSpace(transactionId))
                    trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();

                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);

                Initialize(TransactionCategory.InternalOutbound);

                // ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                string message = string.Empty;
                string apiUrl = "api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}";
                if (trxId != null)
                {
                    apiUrl = "api/Transaction/GetPreviousTransactionByID?transactionsId=" + trxId + "&cultureName={0}&transactionCategory={1}&orgUnitId={2}";
                }
                GetResult<AddOutboundInternalDTO> outboundInternalAddDTO =
                  HttpClientWrapper<GetResult<AddOutboundInternalDTO>>.GetItemRequest(String.Format(apiUrl, SessionInfo.CultureShortName, TransactionCategory.InternalOutbound, SessionInfo.OrgUnitId)).Result;

                outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.DirectedToId = SessionInfo.CurrentUser.Id;
                outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = SessionInfo.OrgUnitId;

                if (outboundInternalAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalAddDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundInternalAddDTO.Result == null)
                {
                    message = DbRes.TResource("User.OutboundInternal.NoPreviousDataInfoMsg");

                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }

                //                GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //                if (subjectClassificationDTOs.Result != null && outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SubjectClassifications != null)
                //                {
                //                    outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SubjectClassifications.ForEach(s =>
                //                    {
                //                        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //                        {
                //                            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //                        }
                //                    });
                //                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                   HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(OutboundInternalMapper.Map(outboundInternalAddDTO.Result).OutboundInternalBasicInfoAdd.DirectedToOrgUnitId);

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), OutboundInternalMapper.Map(outboundInternalAddDTO.Result).OutboundInternalBasicInfoAdd.DirectedToOrgUnitId);

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.EditorType = EditorType.Scanning;
                editorViewModel.Content = string.Empty;
                editorViewModel.ReadOnly = false;
                ViewData["EditorViewModel"] = editorViewModel;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

                ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundInternalBasicInfoAdd";

                return View("~/Areas/User/Views/OutboundInternal/Add.cshtml", OutboundInternalMapper.Map(outboundInternalAddDTO.Result));

            }
            catch (Exception)
            {
                throw;
            }
        }




        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateInternalOutboundFromCopy)]
        [CustomAction]
        public ActionResult CreateCopyOutboundInternal(string transactionId, int transactionType)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                AddOutboundInternalVM outboundInternalAddVM = new AddOutboundInternalVM();

                switch (transactionType.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    case (int)TransactionCategory.InternalOutbound:
                        {
                            GetResult<EditOutboundInternalDTO> outboundInternalEditDTO = HttpClientWrapper<GetResult<EditOutboundInternalDTO>>
                                .GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={trxId}&cultureName={SessionInfo.CultureShortName}").Result;
                            EditOutboundInternalVM editOutboundInternalVM = OutboundInternalMapper.Map(outboundInternalEditDTO.Result);
                            Initialize(TransactionCategory.InternalOutbound);

                            TextEditorViewModel editorViewModel = new TextEditorViewModel();
                            editorViewModel.ReadOnly = false;
                            var transactionArchiveVMs = new List<TransactionArchiveVM>();
                            if (editOutboundInternalVM.DocumentVM != null)
                            {

                                string documentId = Guid.NewGuid().ToString();
                                if (string.IsNullOrEmpty(editOutboundInternalVM.DocumentVM.MimeType) || editOutboundInternalVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                                {
                                    editOutboundInternalVM.EditorType = EditorType.Scanning;
                                }
                                else
                                {
                                    editOutboundInternalVM.EditorType = EditorType.TextEditor;
                                }
                                ViewData["hdnDocumentId"] = editOutboundInternalVM.DocumentVM.Id;
                                if (!editOutboundInternalVM.IsSigned && editOutboundInternalVM.EditorType == EditorType.TextEditor)
                                {
                                    editorViewModel.EditorType = EditorType.TextEditor;
                                    editorViewModel.Content = editOutboundInternalVM.DocumentVM != null && editOutboundInternalVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(editOutboundInternalVM.DocumentVM.Content) : null;
                                    editorViewModel.IsSigned = editOutboundInternalVM.IsSigned;
                                }
                                else
                                {
                                    editorViewModel.EditorType = EditorType.Scanning;
                                    editorViewModel.IsSigned = editOutboundInternalVM.IsSigned;
                                    string sessionKey = Guid.NewGuid().ToString();
                                    ViewData[sessionKey] = sessionKey;
                                    Session["DocoNutDocument"] = editOutboundInternalVM.DocumentVM.Content;
                                }
                                ViewData["EditorViewModel"] = editorViewModel;
                                transactionArchiveVMs.Add(new TransactionArchiveVM
                                {
                                    EncryptDocumentId = AESEncrytDecry.Base64Encode(editOutboundInternalVM.DocumentVM.Id.ToString()),
                                    Id = documentId,
                                    IsMainDocument = true,
                                    DocumentId = editOutboundInternalVM.DocumentVM.Id,
                                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                                });
                            }

                            outboundInternalAddVM = new AddOutboundInternalVM()
                            {
                                IsSigned = editOutboundInternalVM.IsSigned,
                                OrgUnitId = editOutboundInternalVM.OrgUnitId,
                                UserId = editOutboundInternalVM.OrgUnitId,
                                Id = editOutboundInternalVM.Id,
                                Attachments = editOutboundInternalVM.Attachments,
                                DocumentVM = editOutboundInternalVM.DocumentVM,
                                Archives = editOutboundInternalVM.Archives,

                                OutboundInternalBasicInfoAdd = new AddOutboundInternalBasicInfoVM()
                                {
                                    ConfidentialityLevelId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.ConfidentialityLevelId,
                                    DeliveryMethod = editOutboundInternalVM.OutboundInternalBasicInfoEdit.DeliveryMethod,
                                    DeliveryMethodId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.DeliveryMethodId,
                                    DirectedToId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.DirectedToId,
                                    DirectedToOrgUnitId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId,
                                    GroupId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.GroupId,
                                    Hour = editOutboundInternalVM.OutboundInternalBasicInfoEdit.Hour,
                                    Minute = editOutboundInternalVM.OutboundInternalBasicInfoEdit.Minute,
                                    PriorityLevelId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.PriorityLevelId,
                                    Remarks = editOutboundInternalVM.OutboundInternalBasicInfoEdit.Remarks,
                                    RemindDate = editOutboundInternalVM.OutboundInternalBasicInfoEdit.RemindDate,
                                    RemindDateH = editOutboundInternalVM.OutboundInternalBasicInfoEdit.RemindDateH,
                                    TransactionTypeId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.TransactionTypeId,
                                    Subject = editOutboundInternalVM.OutboundInternalBasicInfoEdit.Subject,
                                    SubjectClassifications = editOutboundInternalVM.OutboundInternalBasicInfoEdit.SubjectClassifications,
                                    SuggestedTopicId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.SuggestedTopicId,
                                    LetterTypeId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.LetterTypeId,
                                    CopyTypeId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    ReporterId = editOutboundInternalVM.OutboundInternalBasicInfoEdit.ReporterId,
                                    LetterNumber = editOutboundInternalVM.OutboundInternalBasicInfoEdit.LetterNumber
                                }
                            };
                        }
                        break;
                    case (int)TransactionCategory.Inbound:
                        {
                            GetResult<EditInboundDTO> inboundEditDTO =
                           HttpClientWrapper<GetResult<EditInboundDTO>>.GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={trxId}&cultureName={SessionInfo.CultureShortName}").Result;

                            EditInboundVM editInboundVM = EditInboundMapper.Map(inboundEditDTO.Result);
                            Initialize(TransactionCategory.InternalOutbound);
                            TextEditorViewModel editorViewModel = new TextEditorViewModel();
                            editorViewModel.ReadOnly = false;
                            var transactionArchiveVMs = new List<TransactionArchiveVM>();
                            if (editInboundVM.DocumentVM != null)
                            {
                                string documentId = Guid.NewGuid().ToString();
                                if (string.IsNullOrEmpty(editInboundVM.DocumentVM.MimeType) || editInboundVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                                {
                                    editInboundVM.EditorType = EditorType.Scanning;
                                }
                                else
                                {
                                    editInboundVM.EditorType = EditorType.TextEditor;
                                }
                                ViewData["hdnDocumentId"] = editInboundVM.DocumentVM.Id;
                                if (!editInboundVM.IsSigned && editInboundVM.EditorType == EditorType.TextEditor)
                                {
                                    editorViewModel.EditorType = EditorType.TextEditor;
                                    editorViewModel.Content = editInboundVM.DocumentVM != null && editInboundVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(editInboundVM.DocumentVM.Content) : null;
                                    editorViewModel.IsSigned = editInboundVM.IsSigned;
                                }
                                else
                                {
                                    editorViewModel.EditorType = EditorType.Scanning;
                                    editorViewModel.IsSigned = editInboundVM.IsSigned;
                                    string sessionKey = Guid.NewGuid().ToString();
                                    ViewData[sessionKey] = sessionKey;
                                    Session["DocoNutDocument"] = editInboundVM.DocumentVM.Content;
                                }
                                ViewData["EditorViewModel"] = editorViewModel;
                                transactionArchiveVMs.Add(new TransactionArchiveVM
                                {
                                    EncryptDocumentId = AESEncrytDecry.Base64Encode(editInboundVM.DocumentVM.Id.ToString()),
                                    Id = documentId,
                                    IsMainDocument = true,
                                    DocumentId = editInboundVM.DocumentVM.Id,
                                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                                });
                            }

                            outboundInternalAddVM = new AddOutboundInternalVM()
                            {
                                IsSigned = editInboundVM.IsSigned,
                                OrgUnitId = editInboundVM.OrgUnitId,
                                UserId = editInboundVM.OrgUnitId,
                                Id = editInboundVM.Id,
                                Archives = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(editInboundVM.Archives, 1, editInboundVM.Archives.Count, true),
                                Attachments = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(editInboundVM.Attachments, 1, editInboundVM.Attachments.Count, true),
                                DocumentVM = editInboundVM.DocumentVM,
                                OutboundInternalBasicInfoAdd = new AddOutboundInternalBasicInfoVM()
                                {
                                    ConfidentialityLevelId = editInboundVM.InboundBasicInfoEdit.ConfidentialityLevelId,
                                    DeliveryMethod = editInboundVM.InboundBasicInfoEdit.DeliveryMethod,
                                    DeliveryMethodId = editInboundVM.InboundBasicInfoEdit.DeliveryMethodId,
                                    DirectedToId = editInboundVM.InboundBasicInfoEdit.DirectedToId,
                                    DirectedToOrgUnitId = editInboundVM.InboundBasicInfoEdit.DirectedToOrgUnitId,
                                    Hour = editInboundVM.InboundBasicInfoEdit.Hour,
                                    Minute = editInboundVM.InboundBasicInfoEdit.Minute,
                                    PriorityLevelId = editInboundVM.InboundBasicInfoEdit.PriorityLevelId,
                                    Remarks = editInboundVM.InboundBasicInfoEdit.Remarks,
                                    RemindDate = editInboundVM.InboundBasicInfoEdit.RemindDate,
                                    RemindDateH = editInboundVM.InboundBasicInfoEdit.RemindDateH,
                                    TransactionTypeId = editInboundVM.InboundBasicInfoEdit.TransactionTypeId,
                                    Subject = editInboundVM.InboundBasicInfoEdit.Subject,
                                    SubjectClassifications = editInboundVM.InboundBasicInfoEdit.SubjectClassifications,
                                    SuggestedTopicId = editInboundVM.InboundBasicInfoEdit.SuggestedTopicId,
                                    LetterTypeId = editInboundVM.InboundBasicInfoEdit.LetterTypeId,
                                    CopyTypeId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    ReporterId = editInboundVM.InboundBasicInfoEdit.ReporterId,
                                    LetterNumber = editInboundVM.InboundBasicInfoEdit.LetterNumber
                                }
                            };
                        }
                        break;
                    case (int)TransactionCategory.ExternalOutbound:
                        {
                            GetResult<EditOutboundExternalDTO> outboundExternalEditDTO =
                HttpClientWrapper<GetResult<EditOutboundExternalDTO>>.GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={trxId}&cultureName={SessionInfo.CultureShortName}").Result;

                            EditOutboundExternalVM editOutboundExternalVM = OutboundExternalMapper.Map(outboundExternalEditDTO.Result);
                            Initialize(TransactionCategory.InternalOutbound);
                            TextEditorViewModel editorViewModel = new TextEditorViewModel();
                            editorViewModel.ReadOnly = false;
                            var transactionArchiveVMs = new List<TransactionArchiveVM>();
                            if (editOutboundExternalVM.DocumentVM != null)
                            {
                                string documentId = Guid.NewGuid().ToString();
                                if (string.IsNullOrEmpty(editOutboundExternalVM.DocumentVM.MimeType) || editOutboundExternalVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                                {
                                    editOutboundExternalVM.EditorType = EditorType.Scanning;
                                }
                                else
                                {
                                    editOutboundExternalVM.EditorType = EditorType.TextEditor;
                                }
                                ViewData["hdnDocumentId"] = editOutboundExternalVM.DocumentVM.Id;
                                if (!editOutboundExternalVM.IsSigned && editOutboundExternalVM.EditorType == EditorType.TextEditor)
                                {
                                    editorViewModel.EditorType = EditorType.TextEditor;
                                    editorViewModel.Content = editOutboundExternalVM.DocumentVM != null && editOutboundExternalVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(editOutboundExternalVM.DocumentVM.Content) : null;
                                    editorViewModel.IsSigned = editOutboundExternalVM.IsSigned;
                                }
                                else
                                {
                                    editorViewModel.EditorType = EditorType.Scanning;
                                    editorViewModel.IsSigned = editOutboundExternalVM.IsSigned;
                                    string sessionKey = Guid.NewGuid().ToString();
                                    ViewData[sessionKey] = sessionKey;
                                    Session["DocoNutDocument"] = editOutboundExternalVM.DocumentVM.Content;
                                }
                                ViewData["EditorViewModel"] = editorViewModel;
                                transactionArchiveVMs.Add(new TransactionArchiveVM
                                {
                                    EncryptDocumentId = AESEncrytDecry.Base64Encode(editOutboundExternalVM.DocumentVM.Id.ToString()),
                                    Id = documentId,
                                    IsMainDocument = true,
                                    DocumentId = editOutboundExternalVM.DocumentVM.Id,
                                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                                });
                            }

                            outboundInternalAddVM = new AddOutboundInternalVM()
                            {
                                IsSigned = editOutboundExternalVM.IsSigned,
                                OrgUnitId = editOutboundExternalVM.OrgUnitId,
                                UserId = editOutboundExternalVM.OrgUnitId,
                                Id = editOutboundExternalVM.Id,
                                Attachments = editOutboundExternalVM.Attachments,
                                DocumentVM = editOutboundExternalVM.DocumentVM,
                                Archives = editOutboundExternalVM.Archives,
                                OutboundInternalBasicInfoAdd = new AddOutboundInternalBasicInfoVM()
                                {
                                    ConfidentialityLevelId = editOutboundExternalVM.OutboundExternalBasicInfo.ConfidentialityLevelId,
                                    DeliveryMethod = editOutboundExternalVM.OutboundExternalBasicInfo.DeliveryMethod,
                                    DeliveryMethodId = editOutboundExternalVM.OutboundExternalBasicInfo.DeliveryMethodId,
                                    Hour = editOutboundExternalVM.OutboundExternalBasicInfo.Hour,
                                    Minute = editOutboundExternalVM.OutboundExternalBasicInfo.Minute,
                                    PriorityLevelId = editOutboundExternalVM.OutboundExternalBasicInfo.PriorityLevelId,
                                    Remarks = editOutboundExternalVM.OutboundExternalBasicInfo.Remarks,
                                    RemindDate = editOutboundExternalVM.OutboundExternalBasicInfo.RemindDate,
                                    RemindDateH = editOutboundExternalVM.OutboundExternalBasicInfo.RemindDateH,
                                    TransactionTypeId = editOutboundExternalVM.OutboundExternalBasicInfo.TransactionTypeId,
                                    Subject = editOutboundExternalVM.OutboundExternalBasicInfo.Subject,
                                    SubjectClassifications = editOutboundExternalVM.OutboundExternalBasicInfo.SubjectClassifications,
                                    SuggestedTopicId = editOutboundExternalVM.OutboundExternalBasicInfo.SuggestedTopicId,
                                    LetterTypeId = editOutboundExternalVM.OutboundExternalBasicInfo.LetterTypeId,
                                    CopyTypeId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    ReporterId = editOutboundExternalVM.OutboundExternalBasicInfo.ReporterId,
                                    LetterNumber = editOutboundExternalVM.OutboundExternalBasicInfo.LetterNumber
                                }
                            };
                        }
                        break;
                    case (int)TransactionCategory.DraftOutbound:
                        {
                            GetResult<EditOutboundDraftDTO> outboundDraftEditDTO = HttpClientWrapper<GetResult<EditOutboundDraftDTO>>
                                .GetItemRequest($"api/Transaction/GetTransactionByCopy?transactionId={trxId}&cultureName={SessionInfo.CultureShortName}").Result;

                            EditOutboundDraftVM editOutboundDraftVM = OutboundDraftMapper.Map(outboundDraftEditDTO.Result);
                            Initialize(TransactionCategory.InternalOutbound);
                            TextEditorViewModel editorViewModel = new TextEditorViewModel();
                            editorViewModel.ReadOnly = false;
                            var transactionArchiveVMs = new List<TransactionArchiveVM>();
                            if (editOutboundDraftVM.DocumentVM != null)
                            {
                                string documentId = Guid.NewGuid().ToString();
                                if (string.IsNullOrEmpty(editOutboundDraftVM.DocumentVM.MimeType) || editOutboundDraftVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                                {
                                    editOutboundDraftVM.EditorType = EditorType.Scanning;
                                }
                                else
                                {
                                    editOutboundDraftVM.EditorType = EditorType.TextEditor;
                                }
                                ViewData["hdnDocumentId"] = editOutboundDraftVM.DocumentVM.Id;
                                if (!editOutboundDraftVM.IsSigned && editOutboundDraftVM.EditorType == EditorType.TextEditor)
                                {
                                    editorViewModel.EditorType = EditorType.TextEditor;
                                    editorViewModel.Content = editOutboundDraftVM.DocumentVM != null && editOutboundDraftVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(editOutboundDraftVM.DocumentVM.Content) : null;
                                    editorViewModel.IsSigned = editOutboundDraftVM.IsSigned;
                                }
                                else
                                {
                                    editorViewModel.EditorType = EditorType.Scanning;
                                    editorViewModel.IsSigned = editOutboundDraftVM.IsSigned;
                                    string sessionKey = Guid.NewGuid().ToString();
                                    ViewData[sessionKey] = sessionKey;
                                    Session["DocoNutDocument"] = editOutboundDraftVM.DocumentVM.Content;
                                }
                                ViewData["EditorViewModel"] = editorViewModel;
                                transactionArchiveVMs.Add(new TransactionArchiveVM
                                {
                                    Id = documentId,
                                    EncryptDocumentId = AESEncrytDecry.Base64Encode(editOutboundDraftVM.DocumentVM.Id.ToString()),
                                    IsMainDocument = true,
                                    DocumentId = editOutboundDraftVM.DocumentVM.Id,
                                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                                });
                            }

                            outboundInternalAddVM = new AddOutboundInternalVM()
                            {
                                IsSigned = editOutboundDraftVM.IsSigned,
                                OrgUnitId = editOutboundDraftVM.OrgUnitId,
                                UserId = editOutboundDraftVM.OrgUnitId,
                                Id = editOutboundDraftVM.Id,
                                Archives = editOutboundDraftVM.Archives,
                                Attachments = editOutboundDraftVM.Attachments,
                                DocumentVM = editOutboundDraftVM.DocumentVM,
                                OutboundInternalBasicInfoAdd = new AddOutboundInternalBasicInfoVM()
                                {
                                    ConfidentialityLevelId = editOutboundDraftVM.OutboundDraftBasicInfo.ConfidentialityLevelId,
                                    Hour = editOutboundDraftVM.OutboundDraftBasicInfo.Hour,
                                    Minute = editOutboundDraftVM.OutboundDraftBasicInfo.Minute,
                                    PriorityLevelId = editOutboundDraftVM.OutboundDraftBasicInfo.PriorityLevelId,
                                    RemindDate = editOutboundDraftVM.OutboundDraftBasicInfo.RemindDate,
                                    RemindDateH = editOutboundDraftVM.OutboundDraftBasicInfo.RemindDateH,
                                    TransactionTypeId = editOutboundDraftVM.OutboundDraftBasicInfo.TransactionTypeId,
                                    Subject = editOutboundDraftVM.OutboundDraftBasicInfo.Subject,
                                    SubjectClassifications = editOutboundDraftVM.OutboundDraftBasicInfo.SubjectClassifications,
                                    SuggestedTopicId = editOutboundDraftVM.OutboundDraftBasicInfo.SuggestedTopicId,
                                    LetterTypeId = editOutboundDraftVM.OutboundDraftBasicInfo.LetterTypeId,
                                    CopyTypeId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName),
                                    ReporterId = editOutboundDraftVM.OutboundDraftBasicInfo.ReporterId,
                                    LetterNumber = editOutboundDraftVM.OutboundDraftBasicInfo.LetterNumber
                                }
                            };
                        }
                        break;
                }


                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.
                //    GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (subjectClassificationDTOs.Result != null && outboundInternalAddVM.OutboundInternalBasicInfoAdd.SubjectClassifications != null)
                //{
                //    outboundInternalAddVM.OutboundInternalBasicInfoAdd.SubjectClassifications.ForEach(s =>
                //    {
                //        if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //        {
                //            subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //        }
                //    });
                //}

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs = HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>
                //    .GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //if (suggestedTopicDTOs.Result != null && outboundInternalAddVM.OutboundInternalBasicInfoAdd.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalAddVM.OutboundInternalBasicInfoAdd.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalAddVM.OutboundInternalBasicInfoAdd.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                //ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["CreateFromCopy"] = true;
                outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToId = SessionInfo.CurrentUser.Id;
                outboundInternalAddVM.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                return View("~/Areas/User/Views/OutboundInternal/Add.cshtml", outboundInternalAddVM);
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
            List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();

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
        }

        [CustomAuthorizationAttribute(UserClaims.Outbound.EditInternalOutbound, UserClaims.Outbound.EditorInternalOutbound)]
        [CustomAction]
        public ActionResult Edit(string id, string defaultTabId)
        {

            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));



                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();


                var outboundInternalEditDTO = HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (outboundInternalEditDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalEditDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                SetTransactionAssignmentToViewed(trxId);
                // write outboundInternalEditDTO.DocumentDTO.Content to file(name = guid.ToString);
                // Session[abc] = guid

                var outboundInternalEditVM = OutboundInternalMapper.Map(outboundInternalEditDTO.Result);

                //remove Blind Carbon Copy from list 
                //outboundInternalEditVM.Copies.RemoveAll(x => x.IsBcc == true);


                ViewData["InternalCopiesData"] =
                    outboundInternalEditVM?.Copies != null ? outboundInternalEditVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    outboundInternalEditVM?.ExternalCopies != null ? outboundInternalEditVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();
                //outboundInternalEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundInternalEditVM.HijriRecordDate);
                Initialize(TransactionCategory.InternalOutbound);

                var transactionArchiveVMs = new List<TransactionArchiveVM>();

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.ReadOnly = false;
                IList<LookupVM> yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (outboundInternalEditVM.Links != null && outboundInternalEditVM.Links.Count > 0)
                {
                    List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                    foreach (TransactionLinkVM item in outboundInternalEditVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                if (outboundInternalEditVM.DocumentVM != null && outboundInternalEditVM.DocumentVM.Size > 0)
                {
                    string documentId = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(outboundInternalEditVM.DocumentVM.MimeType) || outboundInternalEditVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                    {
                        outboundInternalEditVM.EditorType = EditorType.Scanning;
                    }
                    else
                    {
                        outboundInternalEditVM.EditorType = EditorType.TextEditor;




                    }
                    ViewData["hdnDocumentId"] = outboundInternalEditVM.DocumentVM.Id;
                    if (!outboundInternalEditVM.IsSigned && outboundInternalEditVM.EditorType == EditorType.TextEditor)
                    {

                        editorViewModel.EditorType = EditorType.TextEditor;
                        // editorViewModel.Content = outboundInternalEditVM.DocumentVM != null && outboundInternalEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundInternalEditVM.DocumentVM.Content) : null;

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

                        editorViewModel.IsSigned = outboundInternalEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;
                        editorViewModel.IsSigned = outboundInternalEditVM.IsSigned;
                        string sessionKey = Guid.NewGuid().ToString();
                        ViewData[sessionKey] = sessionKey;
                        Session["DocoNutDocument"] = outboundInternalEditVM.DocumentVM.Content;
                    }
                    ViewData["EditorViewModel"] = editorViewModel;
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundInternalEditVM.DocumentVM.Id.ToString()),
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = outboundInternalEditVM.DocumentVM.Id,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text,
                    });
                }
                else
                {
                    editorViewModel.ReadOnly = false;
                    editorViewModel.IsShowWordAddIn = true;
                    editorViewModel.EditorType = EditorType.Scanning;
                    editorViewModel.Content = string.Empty;
                    ViewData["EditorViewModel"] = editorViewModel;
                    ViewData["hdnDocumentId"] = -1;
                }
                if (outboundInternalEditVM.Attachments != null)
                {
                    foreach (TransactionAttachmentVM item in outboundInternalEditVM.Attachments)
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
                            Id = item.Id.ToString(),
                            UserId = item.UserId,
                            ReadOnly = !(item.UserId == SessionInfo.CurrentUser.Id),
                            AttachmentSource = item.AttachmentSource,
                            IsNew = true
                        };

                        if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                        {
                            //Archive.Id = Guid.NewGuid().ToString();
                            Archive.DocumentId = item.DocumentVM.Id;
                            Archive.EncryptDocumentId = AESEncrytDecry.Base64Encode(item.DocumentVM.Id.ToString());
                            Archive.IsDeleted = item.DocumentVM.IsDeleted;
                            Archive.FileName = item.DocumentVM.Name;
                            Archive.FromUserId = item.DocumentVM.FromUserId;
                            Archive.FromEntityId = item.DocumentVM.FromEntityId;
                        }
                        transactionArchiveVMs.Add(Archive);
                    }
                }

                outboundInternalEditVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();
                if (outboundInternalEditVM.Copies != null && outboundInternalEditVM.Copies.Count > 0)
                    outboundInternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));

                ViewData["transactionId"] = outboundInternalEditVM.Id;

                //IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();
                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(outboundInternalEditVM.SavedTransactionAssignment) ?
      JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(outboundInternalEditVM.SavedTransactionAssignment) : transactionAssignmentVMs;

                //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

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



                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId, true);
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Links);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                //ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();

                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = outboundInternalEditDTO.Result.FromUser.LocalName == outboundInternalEditDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;

                InitializerAssignmentPaperData(outboundInternalEditVM.Id);




                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ArchiveListData"] =
                    outboundInternalEditVM?.Archives != null ? outboundInternalEditVM.Archives.ToList() : new List<TransactionArchiveVM>();
                //var subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

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

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));
                if (outboundInternalEditVM.OutboundInternalBasicInfoEdit.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }
                //var suggestedTopicDTOs = HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //if (suggestedTopicDTOs.Result != null && outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                Session["BarcodeImgByte"] = GetBarcodeImage(outboundInternalEditVM.Id, true);
                LogTransactionAction(AuditingActionCode.UpadteTransaction, outboundInternalEditVM.Id);
                ViewData["WithBarcode"] = true;
                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == outboundInternalEditVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Outbound.EditInternalOutbound));

                #region Add value to key Field

                for (int i = 0; i < outboundInternalEditVM.Attachments.Count; i++)
                {
                    outboundInternalEditVM.Attachments[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Archives.Count; i++)
                {
                    outboundInternalEditVM.Archives[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Copies.Count; i++)
                {
                    outboundInternalEditVM.Copies[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Names.Count; i++)
                {
                    outboundInternalEditVM.Names[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Links.Count; i++)
                {
                    outboundInternalEditVM.Links[i].Key = i + 1;
                }

                #endregion


                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                taskAddVM.SentToOrgUnitId = SessionInfo.OrgUnitId;
                ViewData["Tasks"] = taskAddVM;
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);

                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);

                //outboundInternalEditVM.HijriRecordDate = StringUtility.
                RemoveAllAttachemntsPhysically();

                outboundInternalEditVM.defaultTabId = defaultTabId;
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
                if (outboundInternalEditVM.Copies != null && outboundInternalEditVM.Copies.Count > 0)
                    outboundInternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/OutboundInternal/Edit.cshtml", outboundInternalEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Editor(string id, string defaultTabId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();

                var outboundInternalEditDTO = HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (outboundInternalEditDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalEditDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }

                var outboundInternalEditVM = OutboundInternalMapper.Map(outboundInternalEditDTO.Result);
                //outboundInternalEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundInternalEditVM.HijriRecordDate);

                //outboundInternalEditVM.Copies.RemoveAll(x => x.IsBcc == true);

                SetTransactionAssignmentToViewed(trxId);


                Initialize(TransactionCategory.InternalOutbound);
                InitializerAssignmentPaperData(trxId);

                var transactionArchiveVMs = new List<TransactionArchiveVM>();

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.ReadOnly = true;
                IList<LookupVM> yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                if (outboundInternalEditVM.Links != null && outboundInternalEditVM.Links.Count > 0)
                {
                    List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                    foreach (TransactionLinkVM item in outboundInternalEditVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                if (outboundInternalEditVM.DocumentVM != null && outboundInternalEditVM.DocumentVM.Size > 0)
                {
                    string documentId = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(outboundInternalEditVM.DocumentVM.MimeType) || outboundInternalEditVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                    {
                        outboundInternalEditVM.EditorType = EditorType.Scanning;
                    }
                    else
                    {
                        outboundInternalEditVM.EditorType = EditorType.TextEditor;



                    }
                    ViewData["hdnDocumentId"] = outboundInternalEditVM.DocumentVM.Id;
                    if (!outboundInternalEditVM.IsSigned && outboundInternalEditVM.EditorType == EditorType.TextEditor)
                    {
                        editorViewModel.EditorType = EditorType.TextEditor;
                        // editorViewModel.Content = outboundInternalEditVM.DocumentVM != null && outboundInternalEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundInternalEditVM.DocumentVM.Content) : null;

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

                        editorViewModel.IsSigned = outboundInternalEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;
                        editorViewModel.IsSigned = outboundInternalEditVM.IsSigned;
                        string sessionKey = Guid.NewGuid().ToString();
                        ViewData[sessionKey] = sessionKey;
                        Session["DocoNutDocument"] = outboundInternalEditVM.DocumentVM.Content;
                    }
                    ViewData["EditorViewModel"] = editorViewModel;
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundInternalEditVM.DocumentVM.Id.ToString()),
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = outboundInternalEditVM.DocumentVM.Id,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                    });
                }
                else
                {
                    editorViewModel.ReadOnly = true;
                    editorViewModel.EditorType = EditorType.Scanning;
                    editorViewModel.Content = string.Empty;
                    ViewData["EditorViewModel"] = editorViewModel;
                    ViewData["hdnDocumentId"] = -1;
                }
                if (outboundInternalEditVM.Attachments != null)
                {
                    foreach (TransactionAttachmentVM item in outboundInternalEditVM.Attachments)
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
                            Id = item.Id.ToString(),
                            UserId = item.UserId,
                            ReadOnly = !(item.UserId == SessionInfo.CurrentUser.Id),
                        };

                        if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                        {
                            //Archive.Id = Guid.NewGuid().ToString();
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

                outboundInternalEditVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();


                ViewData["transactionId"] = outboundInternalEditVM.Id;

                //IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();
                ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(outboundInternalEditVM.SavedTransactionAssignment) ?
        JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(outboundInternalEditVM.SavedTransactionAssignment) : transactionAssignmentVMs;
                //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

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


                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId, true);
                // ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Links);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                //ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();


                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = outboundInternalEditDTO.Result.FromUser.LocalName == outboundInternalEditDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;

                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ArchiveListData"] =
                    outboundInternalEditVM?.Archives != null ? outboundInternalEditVM.Archives.ToList() : new List<TransactionArchiveVM>();
                //var subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

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

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));
                if (outboundInternalEditVM.OutboundInternalBasicInfoEdit.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }
                //var suggestedTopicDTOs = HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //if (suggestedTopicDTOs.Result != null && outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                Session["BarcodeImgByte"] = GetBarcodeImage(outboundInternalEditVM.Id, true);
                LogTransactionAction(AuditingActionCode.OpenEditor, outboundInternalEditVM.Id);
                ViewData["WithBarcode"] = true;
                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == outboundInternalEditVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Outbound.EditInternalOutbound));

                #region Add value to key Field

                for (int i = 0; i < outboundInternalEditVM.Attachments.Count; i++)
                {
                    outboundInternalEditVM.Attachments[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Archives.Count; i++)
                {
                    outboundInternalEditVM.Archives[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Copies.Count; i++)
                {
                    outboundInternalEditVM.Copies[i].Key = i + 1;

                }

                for (int i = 0; i < outboundInternalEditVM.Names.Count; i++)
                {
                    outboundInternalEditVM.Names[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Links.Count; i++)
                {
                    outboundInternalEditVM.Links[i].Key = i + 1;
                }

                #endregion


                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                taskAddVM.SentToOrgUnitId = SessionInfo.OrgUnitId;
                ViewData["Tasks"] = taskAddVM;
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);

                //outboundInternalEditVM.HijriRecordDate = StringUtility.
                RemoveAllAttachemntsPhysically();

                outboundInternalEditVM.defaultTabId = defaultTabId;

                ViewData["TransactionId"] = trxId;
                ViewData["ConfidentialityName"] = outboundInternalEditVM.OutboundInternalBasicInfoEdit.ConfidentialityLevelText;

                ViewData["InternalCopiesData"] =
                    outboundInternalEditVM?.Copies != null ? outboundInternalEditVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    outboundInternalEditVM?.ExternalCopies != null ? outboundInternalEditVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();
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
                if (outboundInternalEditVM.Copies != null && outboundInternalEditVM.Copies.Count > 0)
                    outboundInternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId));
                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);
                if (outboundInternalEditVM.Copies != null && outboundInternalEditVM.Copies.Count > 0)
                    outboundInternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/OutboundInternal/Editor.cshtml", outboundInternalEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult NotificationEditor(string id, string defaultTabId)
        {
            try
            {
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                int trxId = int.Parse(StringCipher.Decrypt(id.Replace(" ", "+")));
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();
                //ViewData["AllActionsData"] = TransactionHelper.GetAllActionsDDL();

                var outboundInternalEditDTO = HttpClientWrapper<GetResult<EditOutboundInternalDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}&orgUnitId={SessionInfo.OrgUnitId}&cultureName={SessionInfo.CultureShortName}").Result;
                if (outboundInternalEditDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalEditDTO.StatusCode.ToString());
                    TempData["InfoMessage"] = new NotificationInformationVM { Message = message, MessageType = MessageType.Warning };
                    return RedirectToAction("DashboardHome", "Shared");
                }
                SetTransactionAssignmentToViewed(trxId);

                var outboundInternalEditVM = OutboundInternalMapper.Map(outboundInternalEditDTO.Result);
                //outboundInternalEditVM.HijriRecordDate = StringUtility.ValidateDate(outboundInternalEditVM.HijriRecordDate);

                //outboundInternalEditVM.Copies.RemoveAll(x => x.IsBcc == true);




                Initialize(TransactionCategory.InternalOutbound);

                var transactionArchiveVMs = new List<TransactionArchiveVM>();

                TextEditorViewModel editorViewModel = new TextEditorViewModel();
                editorViewModel.ReadOnly = true;
                IList<LookupVM> yearlookups = LookupsHelper.GetLookupItems(LookupCategory.Year, SessionInfo.CultureShortName).Result;
                var actionVMs = GetAllActionsValues();
                IList<string> _actionsId = SystemConfigurations.AssignmentPaperActionsIds.Split(',');

                actionVMs = actionVMs.Where(a => _actionsId.Contains(a.Id.ToString())).ToList();

                ViewData["AllActionsData2"] = actionVMs;
                ViewData["ExplanationConfidentiality"] = TransactionHelper.GetExplanationConfidentialityLevelList();
                if (outboundInternalEditVM.Links != null && outboundInternalEditVM.Links.Count > 0)
                {
                    List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                    foreach (TransactionLinkVM item in outboundInternalEditVM.Links)
                    {
                        item.YearDesc = item.Year;
                        item.Year = yearlookups.Where(lo => lo.Text == item.Year.ToString()).FirstOrDefault().Id;
                    }
                }
                if (outboundInternalEditVM.DocumentVM != null && outboundInternalEditVM.DocumentVM.Size > 0)
                {
                    string documentId = Guid.NewGuid().ToString();
                    if (string.IsNullOrEmpty(outboundInternalEditVM.DocumentVM.MimeType) || outboundInternalEditVM.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                    {
                        outboundInternalEditVM.EditorType = EditorType.Scanning;
                    }
                    else
                    {
                        outboundInternalEditVM.EditorType = EditorType.TextEditor;



                    }
                    ViewData["hdnDocumentId"] = outboundInternalEditVM.DocumentVM.Id;
                    if (!outboundInternalEditVM.IsSigned && outboundInternalEditVM.EditorType == EditorType.TextEditor)
                    {
                        editorViewModel.EditorType = EditorType.TextEditor;
                        // editorViewModel.Content = outboundInternalEditVM.DocumentVM != null && outboundInternalEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundInternalEditVM.DocumentVM.Content) : null;

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

                        editorViewModel.IsSigned = outboundInternalEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;
                        editorViewModel.IsSigned = outboundInternalEditVM.IsSigned;
                        string sessionKey = Guid.NewGuid().ToString();
                        ViewData[sessionKey] = sessionKey;
                        Session["DocoNutDocument"] = outboundInternalEditVM.DocumentVM.Content;
                    }
                    ViewData["EditorViewModel"] = editorViewModel;
                    transactionArchiveVMs.Add(new TransactionArchiveVM
                    {
                        EncryptDocumentId = AESEncrytDecry.Base64Encode(outboundInternalEditVM.DocumentVM.Id.ToString()),
                        Id = documentId,
                        IsMainDocument = true,
                        DocumentId = outboundInternalEditVM.DocumentVM.Id,
                        ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName), SessionInfo.CultureShortName).Result.Text
                    });
                }
                else
                {
                    editorViewModel.ReadOnly = true;
                    editorViewModel.EditorType = EditorType.Scanning;
                    editorViewModel.Content = string.Empty;
                    ViewData["EditorViewModel"] = editorViewModel;
                    ViewData["hdnDocumentId"] = -1;
                }
                if (outboundInternalEditVM.Attachments != null)
                {
                    foreach (TransactionAttachmentVM item in outboundInternalEditVM.Attachments)
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
                            Id = item.Id.ToString(),
                            UserId = item.UserId,
                            ReadOnly = !(item.UserId == SessionInfo.CurrentUser.Id),
                        };

                        if (item.DocumentVM != null && item.DocumentVM.Size > 0)
                        {
                            //Archive.Id = Guid.NewGuid().ToString();
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

                outboundInternalEditVM.Archives = transactionArchiveVMs.Where(ta => ta.IsMainDocument == false).ToList();


                ViewData["transactionId"] = outboundInternalEditVM.Id;

                //IAjaxGrid gridArchivingMainDocument = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(transactionArchiveVMs.Where(t => t.IsMainDocument == true).ToList(), 1, 0, true);
                //ViewData["ArchivingMainDocumentData"] = gridArchivingMainDocument;

                List<TransactionAssignmentVM> transactionAssignmentVMs = GetAssignmentPaper();
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


                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId, true);
                // ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Attachments);
                //ViewData["hdnNameArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Names);
                //ViewData["hdnLinkArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Links);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundInternalEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                //ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                ViewData["ExplanationDocumentSessionKey"] = Guid.NewGuid();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                var expData = TransactionHelper.GetTransactionExplanations(trxId).ToList();



                var currentOrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                var IsAssigned = outboundInternalEditDTO.Result.FromUser.LocalName == outboundInternalEditDTO.Result.ToUser.LocalName;
                ViewData["IsAssigned"] = IsAssigned;

                expData.ForEach(x => { x.Key = Guid.NewGuid().ToString("N"); });
                ViewData["ExplanationsData"] = expData;
                ViewData["FormDocumentList"] = TransactionHelper.GetOrgUnitForms();
                ViewData["ArchiveListData"] =
                    outboundInternalEditVM?.Archives != null ? outboundInternalEditVM.Archives.ToList() : new List<TransactionArchiveVM>();
                //var subjectClassificationDTOs = HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

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

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));
                if (outboundInternalEditVM.OutboundInternalBasicInfoEdit.DeliveryMethodId == DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName))
                {
                    ViewData["DeliveryMethod"] = GetDelivery(false);
                }
                //var suggestedTopicDTOs = HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                //if (suggestedTopicDTOs.Result != null && outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalEditVM.OutboundInternalBasicInfoEdit.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                Session["BarcodeImgByte"] = GetBarcodeImage(outboundInternalEditVM.Id, true);
                LogTransactionAction(AuditingActionCode.OpenEditor, outboundInternalEditVM.Id);
                ViewData["WithBarcode"] = true;
                ViewData["isTransactionCreator"] = (SessionInfo.CurrentUser.Id == outboundInternalEditVM.UserId || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Outbound.EditInternalOutbound));

                #region Add value to key Field

                for (int i = 0; i < outboundInternalEditVM.Attachments.Count; i++)
                {
                    outboundInternalEditVM.Attachments[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Archives.Count; i++)
                {
                    outboundInternalEditVM.Archives[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Copies.Count; i++)
                {
                    outboundInternalEditVM.Copies[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Names.Count; i++)
                {
                    outboundInternalEditVM.Names[i].Key = i + 1;
                }

                for (int i = 0; i < outboundInternalEditVM.Links.Count; i++)
                {
                    outboundInternalEditVM.Links[i].Key = i + 1;
                }

                #endregion


                TaskAddVM taskAddVM = new TaskAddVM();

                List<TaskAddVM> gridData = GetTransactionTasks(trxId);

                AjaxGrid<TaskAddVM> Grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(gridData, 1, gridData.Count(), false);

                taskAddVM.TasksGrid = Grid;
                taskAddVM.SentToOrgUnitId = SessionInfo.OrgUnitId;
                ViewData["Tasks"] = taskAddVM;
                ViewData["GridData"] = JsonConvert.SerializeObject(gridData);
                ViewData["TasksUsersData"] = GetUsersByOrgUnitId(taskAddVM.SentToOrgUnitId);
                List<ReceivedTaskVM> receivedTaskVMs = GetTransactionTasksReply(trxId);

                ViewData["ReplyGridData"] = (AjaxGrid<ReceivedTaskVM>)new AjaxGridFactory().CreateAjaxGrid(receivedTaskVMs, 1, receivedTaskVMs.Count(), false);

                //outboundInternalEditVM.HijriRecordDate = StringUtility.
                RemoveAllAttachemntsPhysically();

                outboundInternalEditVM.defaultTabId = defaultTabId;
                ViewData["TransactionId"] = trxId;

                ViewData["ConfidentialityName"] = outboundInternalEditVM.OutboundInternalBasicInfoEdit.ConfidentialityLevelText;
                ViewData["InternalCopiesData"] =
                    outboundInternalEditVM?.Copies != null ? outboundInternalEditVM.Copies.ToList() : new List<TransactionCopyVM>();
                ViewData["ExternalCopiesListData"] =
                    outboundInternalEditVM?.ExternalCopies != null ? outboundInternalEditVM.ExternalCopies.ToList() : new List<TransactionExternalCopyVM>();
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
                if (outboundInternalEditVM.Copies != null && outboundInternalEditVM.Copies.Count > 0)
                    outboundInternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId));
                ViewData["ExternalPartysData"] = JsonConvert.SerializeObject(partiesdataSource);
                if (outboundInternalEditVM.Copies != null && outboundInternalEditVM.Copies.Count > 0)
                    outboundInternalEditVM.Copies.ForEach(x => x.UserList = GetUsersByOrgUnitId(x.OrgUnitId, true));
                Session["TransactionId"] = trxId;
                return View("~/Areas/User/Views/OutboundInternal/Editor.cshtml", outboundInternalEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Outbound.EditInternalOutbound, UserClaims.Outbound.EditorInternalOutbound)]
        [ValidateAntiForgeryToken()]
        // [ValidateInput(false)]
        public ActionResult EditOutboundInternal(EditOutboundInternalVM outboundInternalEditVM, TextEditorViewModel editorViewModel, string hdnMainDocToken, string hdnDocumentId)
        {
            try
            {
                string message = string.Empty;
                outboundInternalEditVM.OrgUnitId = SessionInfo.OrgUnitId;

                if (outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToId == -1)
                {
                    outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToId = null;
                }

                #region Main Document
                outboundInternalEditVM.DocumentVM = new DocumentVM();
                outboundInternalEditVM.DocumentVM.Id = Convert.ToInt32(hdnDocumentId);

                byte[] datass = DocumentViewerHelper.GetPDFFile(hdnMainDocToken);

                if (datass != null)
                {
                    datass = DoconutHelper.RemoveWatermark(datass);
                    outboundInternalEditVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    outboundInternalEditVM.DocumentVM.Content = datass;
                    outboundInternalEditVM.DocumentVM.Size = datass != null ? datass.Length : 0;
                    outboundInternalEditVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    outboundInternalEditVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                }



                #endregion

                #region Transaction External Copy
                var prefix = string.Empty;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    prefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_";
                }
                else
                {
                    prefix = "_" + SessionInfo.CurrentUser.Id + "_";
                }
                if (outboundInternalEditVM.ExternalCopies != null && outboundInternalEditVM.ExternalCopies.Any())
                {
                    foreach (TransactionExternalCopyVM transactionExternalCopy in outboundInternalEditVM.ExternalCopies)
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
                                    FromUserId = SessionInfo.CurrentUser.Id,
                                }
                            });

                            f.Delete();
                        }
                        transactionExternalCopy.externalPartyAttachmentVMs = externalPartyAttachmentVMs;
                    }
                }
                #endregion

                #region Archives
                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                outboundInternalEditVM.Attachments = FillTransactionAttachment(outboundInternalEditVM.Archives, documentData);//fill attachments
                #endregion

                DistributionListVM distributionList = new DistributionListVM();

                if (outboundInternalEditVM.OutboundInternalBasicInfoEdit.DistrubutionListId != null)
                {
                    GetResult<DistributionListDTO> distributionListDTO =
                             HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, outboundInternalEditVM.OutboundInternalBasicInfoEdit.DistrubutionListId.Value)).Result;

                    distributionList = DistributionListMapper.Map(distributionListDTO.Result);
                }

                List<TransactionCopyVM> Copies = new List<TransactionCopyVM>();

                if (distributionList.DistributionListDetails != null)
                {
                    foreach (var item in distributionList.DistributionListDetails)
                    {
                        if (!outboundInternalEditVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == item.UserId))
                        {
                            if ((item.UserId != 0 && !outboundInternalEditVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId == null)) ||
                               (item.UserId == 0 && !outboundInternalEditVM.Copies.Any(copy => copy.OrgUnitId == item.OrgUnitId && copy.UserId != null)))
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

                    outboundInternalEditVM.Copies.AddRange(Copies);
                }

                //outboundInternalEditVM.OutboundInternalBasicInfoEdit.SourceId = Convert.ToInt32(ConfigurationManager.AppSettings["DefaultInternalOutboundSourceType"]);
                //outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToId = SessionInfo.CurrentUser.Id;
                //outboundInternalEditVM.OutboundInternalBasicInfoEdit.DirectedToOrgUnitId = SessionInfo.OrgUnitId;
                EditOutboundInternalDTO editOutboundInternalDTO = OutboundInternalMapper.Map(outboundInternalEditVM);
                if (editOutboundInternalDTO == null)
                {
                    editOutboundInternalDTO = new EditOutboundInternalDTO();

                }




                #region Update transaction Outbound Internal 
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransaction?cultureName=" + SessionInfo.CultureShortName, editOutboundInternalDTO).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;
                if (editOutboundInternalDTO.Links != null && editOutboundInternalDTO.Links.Count > 0)
                {
                    foreach (TransactionLinkDTO link in editOutboundInternalDTO.Links)
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


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundInternal.UpdateSucceeded");
                return Json(new
                {

                    InternalNumber = outboundInternalEditVM.OutboundInternalBasicInfoEdit.Number,
                    Id = outboundInternalEditVM.Id,
                    Date = outboundInternalEditVM.HijriRecordDate,
                    currTime = DateTime.Now.ToString("HH:mm:ss tt"),
                    EncryptedId = AESEncrytDecry.Base64Encode(outboundInternalEditVM.Id.ToString()),
                    MessageText = message,
                    MessageType = MessageType.Information,
                    IsPopularization = outboundInternalEditVM.OutboundInternalBasicInfoEdit.GroupId.HasValue
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public ActionResult GetPreviousOutboundInternal()
        {
            try
            {
                ViewData["OfficeOnlineFileGuid"] = Guid.NewGuid();

                string message = string.Empty;

                Initialize(TransactionCategory.InternalOutbound);
                ViewData["HijriDate"] = string.Empty;
                ViewData["HijriDateTitle"] = DbRes.TResource("User.OutboundInternal.RecordDate");
                ViewData["TransactionNumber"] = string.Empty;
                ViewData["TransactionNumberTitle"] = DbRes.TResource("User.OutboundInternal.Number");

                GetResult<AddOutboundInternalDTO> outboundInternalAddDTO =
                  HttpClientWrapper<GetResult<AddOutboundInternalDTO>>.GetItemRequest(String.Format("api/Transaction/GetPreviousTransaction?cultureName={0}&transactionCategory={1}&orgUnitId={2}", SessionInfo.CultureShortName, TransactionCategory.InternalOutbound, SessionInfo.OrgUnitId)).Result;

                if (outboundInternalAddDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, outboundInternalAddDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (outboundInternalAddDTO.Result == null)
                {
                    message = DbRes.TResource("User.OutboundInternal.NoPreviousDataInfoMsg");
                    return Json(new { UserHasTransactions = false, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }

                outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.DirectedToId = SessionInfo.CurrentUser.Id;
                outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.DirectedToOrgUnitId = SessionInfo.OrgUnitId;

                //        GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //        if (subjectClassificationDTOs.Result != null && outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SubjectClassifications != null)
                //        {
                //            outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SubjectClassifications.ForEach(s =>
                //            {
                //                if (subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault() != null)
                //                {
                //                    subjectClassificationDTOs.Result.Where(sc => sc.Id == s).FirstOrDefault().IsSelected = true;

                //                }
                //            });
                //        }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                   HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(OutboundInternalMapper.Map(outboundInternalAddDTO.Result).OutboundInternalBasicInfoAdd.DirectedToOrgUnitId);

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), OutboundInternalMapper.Map(outboundInternalAddDTO.Result).OutboundInternalBasicInfoAdd.DirectedToOrgUnitId);

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundInternalAddDTO.Result.OutboundInternalBasicInfoAdd.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}

                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));

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

                ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundInternalBasicInfoAdd";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_BasicInfoAddPartial", OutboundInternalMapper.Map(outboundInternalAddDTO.Result).OutboundInternalBasicInfoAdd), UserHasTransactions = true, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        //[CustomAuthorizationAttribute(UserClaims.Outbound.AddAttachments)]
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
        //[CustomAuthorizationAttribute(UserClaims.Outbound.DeleteAttachments)]
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
        public override void InitializeExternalParties()
        {
            ViewData["ExternalPartiesData"] = null;
        }

        public ActionResult SentImgSign(int id, int type)
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
        public ActionResult GetDeliveryReportByTransactionIdaa(string id, int type)
        {
            int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
            string message = string.Empty;

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest($"api/Transaction/GetDeliveryReportByTransactionIds?transactionId={trxId}&type={type}", null).Result;
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

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VipAddTemporaryEntity([Bind(Prefix = "TransactionAssignmentVM")] TransactionAssignmentVM transactionAssignmentVM, List<TransactionAssignmentVM> TransactionAssignments)
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