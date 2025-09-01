using Newtonsoft.Json;
using Spire.Pdf;
using Spire.Pdf.HtmlConverter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.Transaction.OutBound;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Common;


namespace MCS.UI.Areas.User.Controllers
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    //[CustomAuthorizationAttribute(UserClaims.Outbound.DisplayOutbound)]
    public class OutboundDraftController : TransactionController
    {
        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateOutboundDraft)]
        public ActionResult Add()
        {
            try
            {

                //List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

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

                AddOutboundDraftVM outboundDraftAddVM = new AddOutboundDraftVM();
                TextEditorViewModel editorViewModel = new TextEditorViewModel();

                Initialize(outboundDraftAddVM.Type);

                editorViewModel.EditorType = EditorType.TextEditor;
                editorViewModel.Content = string.Empty;
                editorViewModel.IsShowWordAddIn = true;
                ViewData["EditorViewModel"] = editorViewModel;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();

                List<ExternalPartyDTO> parties =
                    HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>
                    .GetItemRequest(String.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", 
                        SessionInfo.CultureShortName, null)).Result
                        .Result;

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
                return View(outboundDraftAddVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.CreateOutboundDraft)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddOutboundDraft(AddOutboundDraftVM addOutboundDraftVM, TextEditorViewModel editorViewModel, string hdnAttachments, string hdnCopies, string hdnExternalCopies, string hdnArchivigdata)
        {
            try
            {
                addOutboundDraftVM.OrgUnitId = SessionInfo.OrgUnitId;

                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                addOutboundDraftVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                addOutboundDraftVM.Copies = javaScriptSerializer.Deserialize(hdnCopies, typeof(List<TransactionCopyVM>)) as List<TransactionCopyVM>;
                addOutboundDraftVM.ExternalCopies = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(List<TransactionExternalCopyVM>)) as List<TransactionExternalCopyVM>;

                if (addOutboundDraftVM.IsSigned && editorViewModel.EditorType == EditorType.TextEditor)
                {
                    addOutboundDraftVM.EditorType = EditorType.Scanning;
                    addOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    addOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    addOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;

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

                    addOutboundDraftVM.DocumentVM.Content = stream.ToArray();
                    addOutboundDraftVM.DocumentVM.Size = stream.ToArray().Length;
                }
                else
                {

                    if (editorViewModel.EditorType == EditorType.TextEditor)
                    {

                        addOutboundDraftVM.EditorType = EditorType.TextEditor;
                        addOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        addOutboundDraftVM.DocumentVM.Content = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]);
                        addOutboundDraftVM.DocumentVM.Size = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]).Length;
                        addOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                        addOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    }
                    else
                    {
                        var content = DocumentViewerHelper.GetPDFFile(((string[])(editorViewModel.Content))[0]);
                        addOutboundDraftVM.EditorType = EditorType.Scanning;
                        addOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        addOutboundDraftVM.DocumentVM.Content = content;
                        addOutboundDraftVM.DocumentVM.Size = content.Length;
                        addOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                        addOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    }
                }

                List<TransactionArchiveVM> transactionArchiveVM = javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;
                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;
                transactionArchiveVM.ForEach(t =>
                {
                    if (!t.IsMainDocument && t.IsNew)
                    {
                        addOutboundDraftVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM = new DocumentVM();
                        addOutboundDraftVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Content = documentData[t.Id];
                        addOutboundDraftVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.Size = documentData[t.Id].Length;
                        addOutboundDraftVM.Attachments.Where(a => a.TypeId == t.AttachmentTypeId).SingleOrDefault().DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    }
                });

                PostObjectResult<TransactionDetailsDTO> postResult = HttpClientWrapper<PostObjectResult<TransactionDetailsDTO>>.PostRequest("api/Transaction/PostTransaction?cultureName=" + SessionInfo.CultureShortName, OutboundDraftMapper.Map(addOutboundDraftVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (addOutboundDraftVM.Links != null && addOutboundDraftVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM link in addOutboundDraftVM.Links)
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

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundDraft.AddSucceeded");

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                    OutboundDraftNumber = postResult.Result.Number,
                    Id = postResult.Result.Id,
                    Date = postResult.Result.HijriDate,
                    EncryptedId = AESEncrytDecry.Base64Encode(postResult.Result.Id.ToString())
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        public ActionResult Edit(string id)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));

                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;

                GetResult<EditOutboundDraftDTO> outboundDraftEditDTO =
                        HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}").Result;

                EditOutboundDraftVM outboundDraftEditVM = OutboundDraftMapper.Map(outboundDraftEditDTO.Result);

                TempData["ControllerName"] = null;

                TextEditorViewModel editorViewModel = new TextEditorViewModel();

                GetResult<List<FormDTO>> formDocumentDTOs =
                                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

                var formDocumentVMs = FormMapper.Map(formDocumentDTOs.Result);

                if (formDocumentVMs != null)
                {
                    foreach (FormVM formVMs in formDocumentVMs)
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formVMs.Id.ToString(),
                            Label = formVMs.LocalName
                        });
                    }
                }
                ViewData["transactionId"] = outboundDraftEditVM.Id;
                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);
                Initialize(TransactionCategory.DraftOutbound);
                IAjaxGrid gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Attachments, 1, 0, true);
                ViewData["AttachmentData"] = gridAttachment;
                IAjaxGrid gridCopies = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Copies, 1, outboundDraftEditVM.Copies.Count, true);
                ViewData["CopiesData"] = gridCopies;

                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.ExternalCopies, 1, outboundDraftEditVM.ExternalCopies.Count, true);
                ViewData["ExternalCopiesData"] = gridExternalCopies;
                List<TransactionArchiveDTO> transactionArchiveDTOs = new List<TransactionArchiveDTO>();

                if (outboundDraftEditVM.DocumentVM != null)
                {
                    if (!outboundDraftEditVM.IsSigned && outboundDraftEditVM.EditorType == EditorType.TextEditor)
                    {
                        editorViewModel.EditorType = EditorType.TextEditor;
                        editorViewModel.Content = outboundDraftEditVM.DocumentVM != null && outboundDraftEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundDraftEditVM.DocumentVM.Content) : null;
                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;

                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;

                        string sessionKey = Guid.NewGuid().ToString();

                        ViewData["SessionMainDocumentKey"] = sessionKey;

                        Session[sessionKey] = outboundDraftEditVM.DocumentVM.Content;
                    }

                }
                editorViewModel.IsShowWordAddIn = true;
                ViewData["EditorViewModel"] = editorViewModel;

                //var transactionArchiveVMs = TransactionArchiveMapper.Map(transactionArchiveDTOs);

                //if (transactionArchiveVMs != null)
                //{
                //    List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //    foreach (TransactionAttachmentVM transactionAttachmentVM in outboundDraftEditVM.Attachments)
                //    {
                //        if (transactionAttachmentVM.DocumentVM != null)
                //        {
                //            transactionArchiveVMs.Add(new TransactionArchiveVM
                //            {
                //                Id = Guid.NewGuid().ToString(),
                //                EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                //                DocumentId = transactionAttachmentVM.DocumentVM.Id,
                //                AttachmentTypeId = transactionAttachmentVM.TypeId,
                //                ArcivingTypeName = transactionAttachmentVM.TypeName,
                //                IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted,
                //                FromUserId = transactionAttachmentVM.DocumentVM.FromUserId,
                //                FromEntityId = transactionAttachmentVM.DocumentVM.FromEntityId
                //            });
                //        }
                //        if (transactionAttachmentVM.Archivable)
                //        {
                //            dataSource.Add(new AutoCompleteDataSource { Label = transactionAttachmentVM.TypeName, Value = transactionAttachmentVM.TypeId.ToString(), Parameters = new object[] { transactionAttachmentVM.Archivable } });
                //        }
                //    }

                //    ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                //}
                IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionArchiveMapper.Map(transactionArchiveDTOs), 1, 0, true);
                ViewData["ArchivingData"] = gridArchiving;

                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Attachments);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                         HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //        HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

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
                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
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
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                return View(outboundDraftEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        public ActionResult VIPEdit(string id)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;

                GetResult<EditOutboundDraftDTO> outboundDraftEditDTO =
                        HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}").Result;

                EditOutboundDraftVM outboundDraftEditVM = OutboundDraftMapper.Map(outboundDraftEditDTO.Result);

                TempData["ControllerName"] = null;

                TextEditorViewModel editorViewModel = new TextEditorViewModel();

                GetResult<List<FormDTO>> formDocumentDTOs =
                                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

                var formDocumentVMs = FormMapper.Map(formDocumentDTOs.Result);

                if (formDocumentVMs != null)
                {
                    foreach (FormVM formVMs in formDocumentVMs)
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formVMs.Id.ToString(),
                            Label = formVMs.LocalName
                        });
                    }
                }
                ViewData["transactionId"] = outboundDraftEditVM.Id;
                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);
                Initialize(TransactionCategory.DraftOutbound);
                IAjaxGrid gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Attachments, 1, 0, true);
                ViewData["AttachmentData"] = gridAttachment;
                IAjaxGrid gridCopies = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Copies, 1, outboundDraftEditVM.Copies.Count, true);
                ViewData["CopiesData"] = gridCopies;

                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.ExternalCopies, 1, outboundDraftEditVM.ExternalCopies.Count, true);
                ViewData["ExternalCopiesData"] = gridExternalCopies;
                List<TransactionArchiveDTO> transactionArchiveDTOs = new List<TransactionArchiveDTO>();

                if (outboundDraftEditVM.DocumentVM != null)
                {
                    if (!outboundDraftEditVM.IsSigned && outboundDraftEditVM.EditorType == EditorType.TextEditor)
                    {
                        editorViewModel.EditorType = EditorType.TextEditor;
                        editorViewModel.Content = outboundDraftEditVM.DocumentVM != null && outboundDraftEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundDraftEditVM.DocumentVM.Content) : null;
                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;

                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;

                        string sessionKey = Guid.NewGuid().ToString();

                        ViewData["SessionMainDocumentKey"] = sessionKey;

                        Session[sessionKey] = outboundDraftEditVM.DocumentVM.Content;
                    }

                }
                ViewData["EditorViewModel"] = editorViewModel;

                //var transactionArchiveVMs = TransactionArchiveMapper.Map(transactionArchiveDTOs);

                //if (transactionArchiveVMs != null)
                //{
                //    List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //    foreach (TransactionAttachmentVM transactionAttachmentVM in outboundDraftEditVM.Attachments)
                //    {
                //        if (transactionAttachmentVM.DocumentVM != null)
                //        {
                //            transactionArchiveVMs.Add(new TransactionArchiveVM
                //            {
                //                Id = Guid.NewGuid().ToString(),
                //                EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                //                DocumentId = transactionAttachmentVM.DocumentVM.Id,
                //                AttachmentTypeId = transactionAttachmentVM.TypeId,
                //                ArcivingTypeName = transactionAttachmentVM.TypeName,
                //                IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted,
                //                FromUserId = transactionAttachmentVM.DocumentVM.FromUserId,
                //                FromEntityId = transactionAttachmentVM.DocumentVM.FromEntityId
                //            });
                //        }
                //        if (transactionAttachmentVM.Archivable)
                //        {
                //            dataSource.Add(new AutoCompleteDataSource { Label = transactionAttachmentVM.TypeName, Value = transactionAttachmentVM.TypeId.ToString(), Parameters = new object[] { transactionAttachmentVM.Archivable } });
                //        }
                //    }

                //    ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                //}
                IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionArchiveMapper.Map(transactionArchiveDTOs), 1, 0, true);
                ViewData["ArchivingData"] = gridArchiving;

                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Attachments);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                         HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //        HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

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
                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
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
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                return View(outboundDraftEditVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult Editor(string id)
        {
            try
            {
                int trxId = int.Parse(StringCipher.DecryptStringAES(id.Replace(" ", "+")));
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;

                GetResult<EditOutboundDraftDTO> outboundDraftEditDTO =
                        HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest($"api/Transaction/GetTransaction?transactionId={trxId}").Result;

                EditOutboundDraftVM outboundDraftEditVM = OutboundDraftMapper.Map(outboundDraftEditDTO.Result);

                TempData["ControllerName"] = null;

                TextEditorViewModel editorViewModel = new TextEditorViewModel();

                GetResult<List<FormDTO>> formDocumentDTOs =
                                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Lookups/GetOrgUnitForms?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                IList<AutoCompleteDataSource> formDocumentDataSource = new List<AutoCompleteDataSource>();

                var formDocumentVMs = FormMapper.Map(formDocumentDTOs.Result);

                if (formDocumentVMs != null)
                {
                    foreach (FormVM formVMs in formDocumentVMs)
                    {
                        formDocumentDataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = formVMs.Id.ToString(),
                            Label = formVMs.LocalName
                        });
                    }
                }
                ViewData["transactionId"] = outboundDraftEditVM.Id;
                ViewData["FormDocumentList"] = JsonConvert.SerializeObject(formDocumentDataSource);
                Initialize(TransactionCategory.DraftOutbound);
                IAjaxGrid gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Attachments, 1, 0, true);
                ViewData["AttachmentData"] = gridAttachment;
                IAjaxGrid gridCopies = (AjaxGrid<TransactionCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.Copies, 1, outboundDraftEditVM.Copies.Count, true);
                ViewData["CopiesData"] = gridCopies;

                IAjaxGrid gridExternalCopies = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(outboundDraftEditVM.ExternalCopies, 1, outboundDraftEditVM.ExternalCopies.Count, true);
                ViewData["ExternalCopiesData"] = gridExternalCopies;
                List<TransactionArchiveDTO> transactionArchiveDTOs = new List<TransactionArchiveDTO>();

                if (outboundDraftEditVM.DocumentVM != null)
                {
                    if (!outboundDraftEditVM.IsSigned && outboundDraftEditVM.EditorType == EditorType.TextEditor)
                    {
                        editorViewModel.EditorType = EditorType.TextEditor;
                        editorViewModel.Content = outboundDraftEditVM.DocumentVM != null && outboundDraftEditVM.DocumentVM.Content != null ? System.Text.Encoding.UTF8.GetString(outboundDraftEditVM.DocumentVM.Content) : null;
                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;
                    }
                    else
                    {
                        editorViewModel.EditorType = EditorType.Scanning;

                        editorViewModel.IsSigned = outboundDraftEditVM.IsSigned;

                        string sessionKey = Guid.NewGuid().ToString();

                        ViewData["SessionMainDocumentKey"] = sessionKey;

                        Session[sessionKey] = outboundDraftEditVM.DocumentVM.Content;
                    }

                }
                ViewData["EditorViewModel"] = editorViewModel;

                //var transactionArchiveVMs = TransactionArchiveMapper.Map(transactionArchiveDTOs);

                //if (transactionArchiveVMs != null)
                //{
                //    List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //    foreach (TransactionAttachmentVM transactionAttachmentVM in outboundDraftEditVM.Attachments)
                //    {
                //        if (transactionAttachmentVM.DocumentVM != null)
                //        {
                //            transactionArchiveVMs.Add(new TransactionArchiveVM
                //            {
                //                Id = Guid.NewGuid().ToString(),
                //                EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                //                DocumentId = transactionAttachmentVM.DocumentVM.Id,
                //                AttachmentTypeId = transactionAttachmentVM.TypeId,
                //                ArcivingTypeName = transactionAttachmentVM.TypeName,
                //                IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted,
                //                FromUserId = transactionAttachmentVM.DocumentVM.FromUserId,
                //                FromEntityId = transactionAttachmentVM.DocumentVM.FromEntityId
                //            });
                //        }
                //        if (transactionAttachmentVM.Archivable)
                //        {
                //            dataSource.Add(new AutoCompleteDataSource { Label = transactionAttachmentVM.TypeName, Value = transactionAttachmentVM.TypeId.ToString(), Parameters = new object[] { transactionAttachmentVM.Archivable } });
                //        }
                //    }

                //    ViewData["ArchivingAttachmentsData"] = JsonConvert.SerializeObject(dataSource);
                //}
                IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionArchiveMapper.Map(transactionArchiveDTOs), 1, 0, true);
                ViewData["ArchivingData"] = gridArchiving;

                //ViewData["hdnAttachmentArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Attachments);
                //ViewData["hdnCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.Copies);
                //ViewData["hdnExternalCopyArray"] = JsonConvert.SerializeObject(outboundDraftEditVM.ExternalCopies);
                //ViewData["hdnArchivingArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["AllExternalActionsData"] = TransactionHelper.GetAllActions();
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                         HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs =
                  HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                ViewData["ExternalPartiesData"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result), outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);
                ViewData["ExternalPartiesManagers"] = GetManagersByPartyId(outboundDraftEditVM.OutboundDraftBasicInfo.DestinationId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["OrgUnitsManagers"] = GetOrgUnitsManagers();

                //GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //        HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

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
                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                //GetResult<List<SuggestedTopicDTO>> suggestedTopicDTOs =
                //        HttpClientWrapper<GetResult<List<SuggestedTopicDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSuggestedTopicsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //if (suggestedTopicDTOs.Result != null && outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.HasValue)
                //{
                //    if (suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault() != null)
                //    {
                //        suggestedTopicDTOs.Result.Where(s => s.Id == outboundDraftEditVM.OutboundDraftBasicInfo.SuggestedTopicId.Value).FirstOrDefault().IsSelected = true;
                //    }
                //}
                //ViewData["SuggestedTopicsData"] = BulidSuggestedTopicsTree(SuggestedTopicMapper.Map(suggestedTopicDTOs.Result));
                ViewData["TransactionId"] = trxId;
                ViewData["ConfidentialityName"] = outboundDraftEditVM.OutboundDraftBasicInfo.ConfidentialityLevelText;
           
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
                return View("~/Areas/User/Views/OutboundExternal/Editor.cshtml", outboundDraftEditVM);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Outbound.EditOutbound)]
        [ValidateAntiForgeryToken()]
        public ActionResult EditOutboundDraft(EditOutboundDraftVM editOutboundDraftVM, string hdnExternalCopies, TextEditorViewModel editorViewModel, string hdnAttachments, string hdnCopies, string hdnArchivigdata)
        {
            try
            {
                editOutboundDraftVM.OrgUnitId = SessionInfo.OrgUnitId;

                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                editOutboundDraftVM.Attachments = javaScriptSerializer.Deserialize(hdnAttachments, typeof(List<TransactionAttachmentVM>)) as List<TransactionAttachmentVM>;
                editOutboundDraftVM.Copies = javaScriptSerializer.Deserialize(hdnCopies, typeof(List<TransactionCopyVM>)) as List<TransactionCopyVM>;
                editOutboundDraftVM.ExternalCopies = javaScriptSerializer.Deserialize(hdnExternalCopies, typeof(List<TransactionExternalCopyVM>)) as List<TransactionExternalCopyVM>;

                if (editOutboundDraftVM.IsSigned && editorViewModel.EditorType == EditorType.TextEditor)
                {
                    editOutboundDraftVM.EditorType = EditorType.Scanning;
                    editOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                    editOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                    editOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;

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

                    editOutboundDraftVM.DocumentVM.Content = stream.ToArray();
                    editOutboundDraftVM.DocumentVM.Size = stream.ToArray().Length;
                }
                else
                {
                    if (editorViewModel.EditorType == EditorType.TextEditor)
                    {
                        editOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        editOutboundDraftVM.DocumentVM.Content = System.Text.Encoding.UTF8.GetBytes(((string[])(editorViewModel.Content))[0]);
                        editOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                        editOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    }
                    else
                    {
                        editOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        editOutboundDraftVM.DocumentVM.Content = DocumentViewerHelper.GetPDFFile(((string[])(editorViewModel.Content))[0]);
                        editOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;
                        editOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                    }
                }

                List<TransactionArchiveVM> transactionArchiveVMs = javaScriptSerializer.Deserialize(hdnArchivigdata, typeof(List<TransactionArchiveVM>)) as List<TransactionArchiveVM>;

                Dictionary<string, byte[]> documentData = Session["DocumentData"] as Dictionary<string, byte[]>;

                transactionArchiveVMs.ForEach(t =>
                {
                    if (!t.IsMainDocument && t.IsDeleted)
                    {
                        editOutboundDraftVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && a.DocumentVM.Id == t.DocumentId)
                            {
                                a.DocumentVM.IsDeleted = true;
                            }
                        });
                    }
                    if (!t.IsMainDocument && t.IsNew)
                    {
                        TransactionAttachmentVM transactionAttachmentVM = editOutboundDraftVM.Attachments.Where(w => w.TypeId == t.AttachmentTypeId).SingleOrDefault();

                        editOutboundDraftVM.DocumentVM = new DocumentVM();
                        editOutboundDraftVM.DocumentVM.IsDeleted = false;
                        editOutboundDraftVM.DocumentVM.Content = documentData[t.Id];
                        editOutboundDraftVM.DocumentVM.Size = documentData[t.Id].Length;
                        editOutboundDraftVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                        editOutboundDraftVM.DocumentVM.FromUserId = SessionInfo.CurrentUser.Id;
                        editOutboundDraftVM.DocumentVM.FromEntityId = SessionInfo.OrgUnitId;

                    }
                    if (!t.IsMainDocument && !t.IsDeleted && !t.IsNew)
                    {
                        editOutboundDraftVM.Attachments.ForEach(a =>
                        {
                            if (a.DocumentVM != null && documentData != null)
                            {
                                if (a.DocumentVM.Id == t.DocumentId && documentData.Keys.Contains(t.Id))
                                {
                                    TransactionAttachmentVM transactionAttachmentVM = editOutboundDraftVM.Attachments.Where(b => b.DocumentVM != null)
                                        .Where(x => x.DocumentVM.Id == t.DocumentId).FirstOrDefault();

                                    transactionAttachmentVM.DocumentVM.IsDeleted = false;
                                    transactionAttachmentVM.DocumentVM.Content = documentData[t.Id];
                                    transactionAttachmentVM.DocumentVM.Size = documentData[t.Id].Length;
                                    transactionAttachmentVM.DocumentVM.MimeType = System.Net.Mime.MediaTypeNames.Application.Pdf;
                                    editOutboundDraftVM.DocumentVM.FromUserId = a.DocumentVM.FromUserId;
                                    editOutboundDraftVM.DocumentVM.FromEntityId = a.DocumentVM.FromEntityId;
                                }
                            }
                        });
                    }
                });

                Session["DocumentData"] = null;
                Session["DocoNutDocument"] = null;
                Session["DocoNutIncDocument"] = null;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PutTransaction?cultureName=" + SessionInfo.CultureShortName, OutboundDraftMapper.Map(editOutboundDraftVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TempData["TransactionData"] = OutboundDraftMapper.Map(editOutboundDraftVM);
                TempData.Keep("TransactionData");

                GetResult<EditOutboundDraftDTO> updatedEditOutboundDraftDTO =
                        HttpClientWrapper<GetResult<EditOutboundDraftDTO>>.GetItemRequest(String.Format("api/Transaction/GetTransaction?cultureName={0}&userId={1}&transactionNumber={2}&TransactionType={3}&year={4}&sourceId={5}&orgUnitId={6}", SessionInfo.CultureShortName, SessionInfo.CurrentUser.Id, editOutboundDraftVM.OutboundDraftBasicInfo.DraftNumber, TransactionCategory.DraftOutbound, DateTimeUtility.GetHijriYear(editOutboundDraftVM.RecordDate), editOutboundDraftVM.OutboundDraftBasicInfo.TransactionTypeId, editOutboundDraftVM.OrgUnitId)).Result;

                var s = OutboundDraftMapper.Map(updatedEditOutboundDraftDTO.Result);

                List<TransactionArchiveVM> updatedTransactionArchiveVMs = new List<TransactionArchiveVM>();

                if (s.Attachments != null)
                {
                    foreach (TransactionAttachmentVM transactionAttachmentVM in s.Attachments)
                    {
                        if (transactionAttachmentVM.DocumentVM != null)
                        {
                            updatedTransactionArchiveVMs.Add(new TransactionArchiveVM
                            {
                                Id = Guid.NewGuid().ToString(),
                                EncryptDocumentId = AESEncrytDecry.Base64Encode(transactionAttachmentVM.DocumentVM.Id.ToString()),
                                DocumentId = transactionAttachmentVM.DocumentVM.Id,
                                AttachmentTypeId = transactionAttachmentVM.TypeId,
                                ArcivingTypeName = transactionAttachmentVM.TypeName,
                                IsDeleted = transactionAttachmentVM.DocumentVM.IsDeleted
                            });
                        }
                    }
                }
                List<TransactionArchiveDTO> updatedTransactionArchiveDTOs = new List<TransactionArchiveDTO>();
                IAjaxGrid gridArchiving = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(TransactionArchiveMapper.Map(updatedTransactionArchiveDTOs), 1, 0, true);

                IAjaxGrid gridAttachment = (AjaxGrid<TransactionAttachmentVM>)new AjaxGridFactory().CreateAjaxGrid(OutboundDraftMapper.Map(updatedEditOutboundDraftDTO.Result).Attachments, 1, 0, true);

                TempData["TransactionData"] = OutboundDraftMapper.Map(updatedEditOutboundDraftDTO.Result);
                TempData.Keep("TransactionData");
                var gridExternalCopy = (AjaxGrid<TransactionExternalCopyVM>)new AjaxGridFactory().CreateAjaxGrid(s.ExternalCopies, 1, s.ExternalCopies.Count, true);
                if (editOutboundDraftVM.Links != null && editOutboundDraftVM.Links.Count > 0)
                {
                    foreach (TransactionLinkVM link in editOutboundDraftVM.Links)
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
                    ExternalCopiesHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_ExternalCopiesGridPartial.cshtml", gridExternalCopy),
                    ArchivingHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_ArchivingGridPartial.cshtml", gridArchiving),
                    AttachmentsHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_AttachmentsGridPartial.cshtml", gridAttachment),
                    MessageText = message,
                    MessageType = MessageType.Information,
                    UpdatedTransactionArchive = JsonConvert.SerializeObject(TransactionArchiveMapper.Map(updatedTransactionArchiveDTOs)),
                    UpdatedTransactionAttachments = JsonConvert.SerializeObject(OutboundDraftMapper.Map(updatedEditOutboundDraftDTO.Result).Attachments)

                }, JsonRequestBehavior.AllowGet);

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
                    // html = FormMapper.Map(formContentDTO.Result).Content;
                }

                return html;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private TreeNode AddExternalPartyChilds(List<ExternalPartyVM> externalPartyVMs, ExternalPartyVM externalPartyVM, int selectedOrgUnitId)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = externalPartyVM.Number,
                IsSelected = externalPartyVM.IsSelected,
                Selectable = true,
                Name = externalPartyVM.LocalName,
                Id = externalPartyVM.Id
            };

            if (externalPartyVM.Id == selectedOrgUnitId)
            {
                treeNode.IsSelected = true;
            }

            externalPartyVMs.Where(o => o.ParentId == externalPartyVM.Id).ToList().ForEach(p =>
            {
                treeNode.Childs.Add(AddExternalPartyChilds(externalPartyVMs, p, selectedOrgUnitId));
            });

            return treeNode;
        }

        public override void InitializeExternalParties()
        {

            ViewData["ExternalPartiesData"] = new TreeViewModel();
        }

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

                return PartialView("~/Areas/User/Views/OutboundDraft/_ExternalPartiesPartial.cshtml", new OutboundDraftExternalPartiesVM() { DestinationId = selectedParty != -1 ? selectedParty : 1 });
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}