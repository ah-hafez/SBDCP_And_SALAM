using MobileApi.Domain;
using MobileApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Entities;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.DTO;
using MCS.Service.Mappers;
using TransactionCategory = MCS.Common.TransactionCategory;
using YESSERDomain = MCS.Domain;
using YESSERMobileDomain = MCS.Domain.MobileSearchCriteria;
using MCS.DataAccess;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Configuration;
using YESSER.NCS.MCS.Service.Helpers;
using MCS.DTO.MobileApi;

namespace MCS.Service.Controllers
{
    [CustomAuthentication]
    public class MobileApiController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetUserInfo(string userName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserData> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.UserProfile userProfile = mobileApiBL.GetUserInfo(userName, Language);

                    UserData userData = new UserData()
                    {
                        PersonId = userProfile.Id,
                        FullName = userProfile.LocalName,
                        LoginName = userProfile.UserName,
                        AllowMobile = userProfile.AllowMobile,
                        DefaultEntityName = userProfile.OrgUnits[0].LocalName
                    };



                    getResult = GetResult<UserData>.Create(statusCode, userData, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserData>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserData>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserMobile(int? userId, string userName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserMobile> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.UserMobile domainUserMobile = mobileApiBL.GetUserMobile(userId, userName, Language);

                    UserMobile userMobile = new UserMobile()
                    {
                        UserId = domainUserMobile.UserId,
                        Token = domainUserMobile.Token,
                        DeviceToken = domainUserMobile.DeviceToken,
                        ActivationRequestCode = domainUserMobile.ActivationRequestCode,
                        ActivataionCode = domainUserMobile.ActivataionCode,
                        DeactivationRequestCode = domainUserMobile.DeactivationRequestCode,
                        SignedCert = domainUserMobile.SignedCert,
                        CA = domainUserMobile.CA,
                        CACRL = domainUserMobile.CACRL,
                        IsUpdated = domainUserMobile.IsUpdated,
                        UpdateFlags = domainUserMobile.UpdateFlags,
                        LastLoginDate = domainUserMobile.LastLoginDate,
                        LoginName = domainUserMobile.LoginName,
                        EntityId = domainUserMobile.EntityId,
                        AllowMobile = domainUserMobile.AllowMobile,
                        Settings = domainUserMobile.Settings,
                        UserMobileClass = domainUserMobile.UserMobileClassId.HasValue ? (UserMobileClass)domainUserMobile.UserMobileClassId.Value.LookupInternalID(LookupCategory.UserMobileClass, Language) : UserMobileClass.NormalUser
                    };

                    getResult = GetResult<UserMobile>.Create(statusCode, userMobile, 1);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserMobile>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserMobile>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage SetDefaultEntity(int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                //Common.TransactionContext.transactionContextScopeOption.ForceCreateNew
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.SetDefaultEntity(userId, entityId);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateUserMobile(UserMobile userMobileDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                //Common.TransactionContext.transactionContextScopeOption.ForceCreateNew
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.UserMobile userMobileDomain = new YESSERDomain.UserMobile()
                    {
                        UserId = userMobileDTO.UserId,
                        Token = userMobileDTO.Token,
                        LastLoginDate = userMobileDTO.LastLoginDate
                    };

                    userMobileBL.UpdateUserMobile(userMobileDomain, Language);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage UserMobileUpdateTransactionStatus(int transId, int statusId, int userId, int orgUnitId, string reason)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.UserMobileUpdateTransactionStatus(transId, statusId, userId, orgUnitId, reason);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage UserMobileDeletedTransaction(int transId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.UserMobileDeletedTransaction(transId);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserSignature(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SignatureData> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.UserPreference userPreference = mobileApiBL.GetUserSignature(userId, Language);

                    SignatureData signatureData = new SignatureData()
                    {
                        FreeText = userPreference.FreeText,
                        Password = userPreference.SignaturePasswordText,
                        HasPassword = userPreference.SignaturePassword,
                        Signature = userPreference.Signature
                    };

                    getResult = GetResult<SignatureData>.Create(statusCode, signatureData, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SignatureData>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SignatureData>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUserSignature(SignatureData signatureData, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                //Common.TransactionContext.transactionContextScopeOption.ForceCreateNew
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.UserPreference userPreference = new YESSERDomain.UserPreference()
                    {
                        Signature = signatureData.Signature,
                        SignaturePasswordText = signatureData.Password,
                        FreeText = signatureData.FreeText
                    };

                    userMobileBL.AddUserSignature(userPreference, userId, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage UserMobileGetOrgHierarchy(int? parentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserMobileOrgUnitDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    IList<YESSERDomain.OrgUnit> orgUnit = mobileApiBL.UserMobileGetOrgHierarchy(parentId, Language);

                    List<UserMobileOrgUnitDTO> userMobileOrgUnitDTOs = UserMobileMapper.Map(orgUnit, Language);

                    getResult = GetResult<List<UserMobileOrgUnitDTO>>.Create(statusCode, userMobileOrgUnitDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserMobileOrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserMobileOrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage UserMobileGetOrgHierarchyAC(string searchQuery)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserMobileOrgUnitDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    IList<YESSERDomain.OrgUnit> orgUnit = mobileApiBL.UserMobileGetOrgHierarchyAC(searchQuery, Language);

                    List<UserMobileOrgUnitDTO> userMobileOrgUnitDTOs = UserMobileMapper.Map(orgUnit, Language);

                    getResult = GetResult<List<UserMobileOrgUnitDTO>>.Create(statusCode, userMobileOrgUnitDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserMobileOrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserMobileOrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDocumentById(int documentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.DocumentInfo document = mobileApiBL.GetDocumentById(documentId, Language);

                    DocData docData = DocRepository.DocRepository.Load(documentId.ToString(), new DocumentLocation());
                    document.Document.Content = document.MimeType != System.Net.Mime.MediaTypeNames.Application.Pdf ? System.Text.Encoding.UTF8.GetBytes(System.Text.Encoding.Unicode.GetString(docData.Data))
                        : docData.Data;

                    DocumentDTO documentDTO = DocumentMapper.Map(document);

                    getResult = GetResult<DocumentDTO>.Create(statusCode, documentDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserAuthorization(int userId, string userName, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserAuthorization> getResult = null;

            try
            {

                UserAuthorization userAuthorization = new UserAuthorization()
                {
                    TransCategories = GetTransactionCategories(),
                    RowStatus = GetRowStatus(),
                    TrayIDs = GetTrayIds(),
                    ArchivingTypes = GetArchivingTypes(),
                    AttachmentMethods = GetAttachmentMethods(),
                    TransactionConfidentialities = GetConfidentialities((int)PermissionGroupName.TransactiosConfidentiality),
                    AttachConfidentialities = GetAttachConfidentialities((int)PermissionGroupName.ExplanationsConfidentiality),
                    Permissions = GetUserMobilePermissions(userId),
                    TransactionSources = GetTransactionSources(),
                    Processes = GetAllActions(),
                    TransactionPriorities = GetPriorities(),
                    TransactionTypes = GetLetterType(),
                    IncludedItemTypes = GetAttachmentType(),
                    AttachmentTypes = GetLookupAttachementType(),
                    Trays = GetTrays(userId, entityId),
                    WithAppointmentIDs = new WithAppointmentID(),
                    AssignmentPaperProcesses = GetAssignmentPapers(userId),
                    PermissionNames = GetPermissionNames(),
                    TransactionPartyDirection = GetTransPartyDirection(),
                    Entities = GetAllEntity(userId),

                };

                getResult = GetResult<UserAuthorization>.Create(statusCode, userAuthorization, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserAuthorization>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserAuthorization>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserTrays(int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<Tray>> getResult = null;

            try
            {
                getResult = GetResult<List<Tray>>.Create(statusCode, GetTrays(userId, entityId), null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<Tray>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<Tray>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        [HttpPost]
        public HttpResponseMessage CreateTransaction(TransData transData, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDetailsDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transData.TransCategory.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                    YESSERDomain.Transaction transaction = new YESSERDomain.Transaction()
                    {
                        Id = transData.TransId,
                        Number = transData.TransNo != string.Empty ? long.Parse(transData.TransNo) : 0,
                        TransactionCategoryId = transData.TransCategory,
                        TransactionTypeId = transData.TransSource,
                        ConfidentialityId = transData.ConfidId,
                        PriorityId = transData.PriorityId,
                        LetterTypeId = transData.TypeId,
                        RemindDate = transData.PriorityDate,
                        RemindDateH = transData.PriorityDateHJ,
                        Date = transData.TransDate,
                        DateH = transData.TransDateHJ,
                        StatusId = transData.Status,
                        Year = transData.Year,
                        Remarks = transData.Remarks,
                        Subject = transData.Subject,
                        ToUserId = transData.InitialAssignToPersonId != 0 ? transData.InitialAssignToPersonId : transData.UserId,
                        ExternalPartyId = transData.ExternalPartyId,
                        UserId = transData.UserId,
                        OrgUnitId = transData.EntityId,
                        EntityId = transData.ConcernedEntityId != 0 ? transData.ConcernedEntityId : transData.EntityId,
                        DocumentNumber = transData.ExtTransNo,
                        DeliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty)
                    };

                    List<ArchiveRecord> mainArchives = transData.archiveRecords.Where(m => m.type == enArchivingType.TransSourceID).ToList();
                    List<ArchiveRecord> includedItemArchives = transData.archiveRecords.Where(m => m.type == enArchivingType.IncludedItem).ToList();
                    //List<ArchiveRecord> explanationArchives = (List<ArchiveRecord>)transData.archiveRecords.Where(m => m.type == enArchivingType.Explaination);

                    //Incuded Items
                    transaction.Attachments = new List<YESSERDomain.Attachment>();
                    if (transData.IncludedItems.Count > 0)
                    {
                        foreach (var includedItem in transData.IncludedItems)
                        {
                            transaction.Attachments.Add(new YESSERDomain.Attachment()
                            {
                                Id = includedItem.RecordId,
                                TypeId = includedItem.ItemId,
                                Count = includedItem.ItemCount,
                                Description = includedItem.Remarks,
                                CreatedOn = DateTime.Now
                            });
                        }
                    }

                    ////Explanation
                    transaction.Explanations = new List<YESSERDomain.Explanation>();
                    transaction.ExternalCopies = new List<YESSERDomain.TransactionExternalCopy>();

                    //MainArchive
                    if (mainArchives.Any())
                    {
                        foreach (var mainArchive in mainArchives)
                        {
                            transaction.MainDocument = new YESSERDomain.DocumentInfo()
                            {
                                Id = 0,
                                Name = mainArchive.fileName,
                                MimeType = mainArchive.method == enEditorAttachMethod.ScanAttach ? System.Net.Mime.MediaTypeNames.Application.Pdf : System.Net.Mime.MediaTypeNames.Text.Plain,
                                Size = mainArchive.DocData == null ? 0 : mainArchive.DocData.Length,
                                FromEntityId = transData.EntityId,
                                FromUserId = transData.UserId,
                                Document = new YESSERDomain.Document()
                                {
                                    Content = null,
                                    Id = 0
                                },
                                CreatedOn = DateTime.Now,
                                CreatedBy = mainArchive.UserID
                            };
                        }
                    }

                    transaction.Copies = new List<YESSERDomain.TransactionCopy>();

                    transaction.Links = new List<YESSERDomain.TransactionLink>();

                    TransactionDetails transactionDetails = transactionBL.Save(transaction);

                    if (mainArchives.Count > 0)
                    {
                        DocData docData = new DocData()
                        {
                            Data = mainArchives[0].DocData,
                            DocName = transaction.MainDocument.Name,
                            DocID = transaction.MainDocument.Id.ToString(),
                            PersonId = transaction.MainDocument.CreatedBy,
                            MimeContent = transaction.MainDocument.MimeType,
                            EntityId = transaction.OrgUnitId,
                            DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                            User_ID = transaction.MainDocument.CreatedBy.ToString(),
                            TransactionId = transaction.Id
                        };
                        DocRepository.DocRepository.Save(docData, new DocumentLocation());
                    }

                    if (includedItemArchives.Count > 0)
                    {
                        for (int index = 0; index < includedItemArchives.Count; index++)
                        {
                            DocData docData = new DocData()
                            {
                                Data = includedItemArchives[index].DocData,
                                DocName = transaction.Attachments[index].DocumentInfo.Name,
                                DocID = transaction.Attachments[index].DocumentInfo.Id.ToString(),
                                PersonId = transaction.Attachments[index].DocumentInfo.CreatedBy,
                                MimeContent = transaction.Attachments[index].DocumentInfo.MimeType,
                                EntityId = transaction.OrgUnitId,
                                DataSize = Convert.ToInt32(transaction.Attachments[index].DocumentInfo.Size),
                                User_ID = transaction.Attachments[index].DocumentInfo.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }
                    }

                    TransactionDetailsDTO transactionDetailsDTO = UserMobileMapper.MapTransactionDetails(transactionDetails);
                    getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransaction(int transId, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransData> getResult = null;

            try
            {
                IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                YESSERDomain.Transaction transaction = userMobileBL.GetTransaction(transId, Language);
                IEditorBL editorBL = new EditorBL();
                IList<MCS.Domain.TransactionLink> transactionLinks = editorBL.GetTransactionLinks(transaction.Id, "ar");
                //IList<YESSERDomain.Explanation> explanations = userMobileBL.GetTransactionExplanations(transId, userId, Language);
                List<YESSERDomain.TransactionAssignmentHistory> transactionAssignmentHistories = userMobileBL.GetTransactionAssignmentHistory(transId, Language);
                YESSERDomain.UserProfile userProfile = userMobileBL.GetUserById(userId);
                DateTime processFinishDate = transaction.RemindDate != null ? transaction.RemindDate.Value : userProfile.TransactionProcessingPeriod > 0 ? transaction.Date.AddDays(userProfile.TransactionProcessingPeriod) : transaction.Date;
                //MainData
                TransData transData = new TransData()
                {
                    TransId = transaction.Id,
                    TransNo = transaction.Number.ToString(),
                    TransCategory = transaction.TransactionCategoryId,
                    TransSource = transaction.TransactionTypeId.Value,
                    TransSourceDesc = transaction.TransactionType.Text,
                    ConfidId = transaction.ConfidentialityId,
                    ConfidDesc = transaction.Confidentiality.LocalName,
                    PriorityId = transaction.PriorityId,
                    PriorityDesc = transaction.Priority.Text,
                    TypeId = transaction.LetterTypeId.Value,
                    TypeDesc = transaction.LetterType.Text,
                    PriorityDate = transaction.RemindDate,
                    PriorityDateHJ = transaction.RemindDateH,
                    FormattedPriorityDate = transaction.RemindDateH,
                    TransDate = transaction.Date,
                    TransDateHJ = transaction.DateH,
                    FormattedTransDate = transaction.DateH,
                    Status = transaction.StatusId,
                    StatusDesc = transaction.Status.Text,
                    Year = transaction.Year,
                    Remarks = string.IsNullOrEmpty(transaction.Remarks) ? "" : transaction.Remarks,
                    Subject = transaction.Subject,
                    IsInternalOutbound = transaction.TransactionCategoryId == TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) ? true : false,
                    OutboundDraft = transaction.TransactionCategoryId == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) ? true : false,
                    InitialAssignToPersonId = transaction.ToUserId != null ? transaction.ToUserId.Value : 0,
                    InitialAssignToPersonName = transaction.ToUser?.LocalName,
                    MainParty = transaction.EntityId != null ? transaction.EntityId.Value : -1,
                    MainPartyDesc = transaction.EntityId != null ? IoC.Resolve<IOrgUnitBL>().GetOrgUnitName(o => o.Id == transaction.EntityId, Language) : string.Empty,
                    UserId = transaction.UserId,
                    CreatorUserName = transaction.User.LocalName,
                    EntityId = transaction.OrgUnitId,
                    CreatingEntityName = transaction.OrgUnit.LocalName,
                    ConcernedEntityId = transaction.Assignments[0].ToEntity.Id,
                    ConcernedEntityDesc = transaction.Assignments[0].ToEntity.LocalName,
                    ExtTransNo = transaction.DocumentNumber,
                    ProcessFinishDate = processFinishDate,
                    IsEditable = transaction.Assignments[0].ToUser != null && Convert.ToInt32(transaction.Assignments[0].ToUser.Id) == userId && transaction.Status.Id != (int)TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty),
                    ProcessFinishDateHJ = DateTimeUtility.ConvertToUmAlQuraCalendar(processFinishDate),
                    BarcodeRand = new Random().Next().ToString(),
                    TrayId = transaction.Assignments != null && transaction.Assignments.Count > 0 ? transaction.Assignments.FirstOrDefault().Tray.Id : (int)TrayType.MyTransactions,
                    ExternalPartyId = transaction.ExternalPartyId,
                    CivilID = transaction.Name != null ? transaction.Name.CivilID : string.Empty,
                    IsAssign =   transactionAssignmentHistories.Count > 1 ? true : false,
                };

                //Trans Parties
                transData.TransCopies = new List<TransPartiy>();
                if (transaction.Copies?.Count > 0)
                {
                    foreach (var copy in transaction.Copies)
                    {
                        transData.TransCopies.Add(new TransPartiy()
                        {
                            TransPartyId = copy.Id,
                            PartyID = copy.Entity.Id,
                            PersonID = copy.User != null ? copy.User.Id : -1,
                            ProcessId = copy.Action.Id,
                            ProcessDesc = copy.Action.LocalName,
                            EntityName = copy.Entity.LocalName,
                            PersonName = copy.User != null && copy.User.LocalizationIdentifier != null ? copy.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text : string.Empty,
                            SendDateHJ = copy.DateH,
                            FromEntityName = string.Empty,
                            FromPersonName = string.Empty
                        });
                    }
                }
                transData.TransLinks = new List<TransLink>();
                if (transaction.Links?.Count > 0)
                {
                    foreach (var Link in transaction.Links)
                    {
                        transData.TransLinks.Add(new TransLink()
                        {
                            Id = Link.Id,
                            ToTransactionId = Link.ToTransactionId,
                            TransactionId = Link.TransactionId
                        });
                    }
                }
                transData.Names = new List<TransactionName>();
                if (transaction.Names?.Count > 0)
                {
                    foreach (var name in transaction.Names)
                    {
                        transData.Names.Add(new TransactionName()
                        {
                            TransactionId = name.TransactionId,
                            NameId = name.NameId,
                            Name = new Name()
                            {
                                CivilID = name.Name.CivilID,
                                NationalityId = name.Name.NationalityId,
                                FirstName = name.Name.FirstName,
                                MobileNumber = name.Name.MobileNumber,
                                Phone = name.Name.Phone,
                                Gender = name.Name.Gender,
                                Id = name.Name.Id,
                                CreatedOn = name.Name.CreatedOn,
                                CreatedBy = name.Name.CreatedBy,
                                ModefiedOn = name.Name.ModefiedOn,

                                ModefiedBy = name.Name.ModefiedBy,

                            }

                        });
                    }
                }

                //Included Items Link Transaction
                transData.IncludedItems = new List<IncludedItem>();
                foreach (var transLink in transactionLinks)
                {
                    if (transLink.ToTransaction.Attachments?.Count > 0)
                    {
                        foreach (var attachment in transLink.ToTransaction.Attachments)
                        {
                            transData.IncludedItems.Add(new IncludedItem()
                            {
                                RecordId = attachment.Id,
                                ItemId = attachment.TypeId,
                                ItemCount = attachment.Count,
                                Remarks = attachment.Description ?? string.Empty,
                                Desc = attachment.Type.Text,
                                RowStatus = (int)DataRowStatus.UnChanged,
                                ItemDate = (DateTimeUtility.ConvertToUmAlQuraCalendar(attachment.CreatedOn))
                            });
                        }
                    }
                }
                //  transData.IncludedItems = new List<IncludedItem>();
                if (transaction.Attachments?.Count > 0)
                {
                    foreach (var attachment in transaction.Attachments)
                    {
                        transData.IncludedItems.Add(new IncludedItem()
                        {
                            RecordId = attachment.Id,
                            ItemId = attachment.TypeId,
                            ItemCount = attachment.Count,
                            Remarks = attachment.Description ?? string.Empty,
                            Desc = attachment.Type.Text,
                            RowStatus = (int)DataRowStatus.UnChanged,
                            ItemDate = (DateTimeUtility.ConvertToUmAlQuraCalendar(attachment.CreatedOn))
                        });
                    }
                }

                //Assignment Track
                transData.AssignTrack = new TransAssignTrack()
                {

                    ElcEntity = transaction.Assignments[0].ToEntity.LocalName,
                    ElcUser = transaction.Assignments[0].ToUser != null ? transaction.Assignments[0].ToUser.LocalName : string.Empty,
                    ElcDate = transaction.Assignments[0].DateH,
                    PhysicalEntity = transaction.Assignments[0].PhysicalEntity != null ? transaction.Assignments[0].PhysicalEntity.LocalName : string.Empty,
                    PhysicalUser = transaction.Assignments[0].PhysicalUser != null ? transaction.Assignments[0].PhysicalUser.LocalName : string.Empty,
                    PhysicalDate = transaction.Assignments[0].DateH,
                    ElcEntityId = transaction.Assignments[0].ToEntity.Id,
                    ElcUserId = transaction.Assignments[0].ToUser != null ? transaction.Assignments[0].ToUser.Id : 0
                };

                transData.AssignTrack.Assignments = new List<AssignTrackEntity>();
                if (transactionAssignmentHistories.Count > 0)
                {
                    foreach (var t in transactionAssignmentHistories)
                    {
                        transData.AssignTrack.Assignments.Add(new AssignTrackEntity()
                        {
                            Date = t.DateH,
                            FromEntity = t.FromEntity.LocalName,
                            FromPerson = t.FromUser.LocalName,
                            ToEntity = t.ToEntity.LocalName,
                            ToPerson = t.ToUser != null ? t.ToUser.LocalName : string.Empty,
                            ProcessName = t.Action != null ? t.Action.LocalName : string.Empty,
                            Remarks = t.Description ?? string.Empty
                        });
                    }
                }

                //Archiving
                DocData docData = transaction.MainDocumentId != null && transaction.MainDocumentId > 0 ? DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation()) : null;
                if (docData != null)
                {
                    transData.archiveRecords = new List<ArchiveRecord>
                {
                    //MainDocument
                    new ArchiveRecord()
                    {
                     attachRecoredId =docData!=null&&  docData.Data!=null && docData.Data.Length > 0 ?  transaction.MainDocument.Id: -1,
                     docId =docData!=null&& docData.Data!=null &&  docData.Data.Length > 0 ?  transaction.MainDocument.Id: -1,
                     transId = transaction.Id,
                     type = enArchivingType.TransSourceID,
                     securtyLevel = transaction.ConfidentialityId,
                     securityLevelDesc = transaction.Confidentiality.LocalName,
                     PrivilegeName = transaction.Confidentiality.Code,
                     date = transaction.MainDocument!=null ? DateTimeUtility.ConvertToUmAlQuraCalendar(transaction.MainDocument.CreatedOn): "",
                     method =
                     transaction.MainDocument?.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf ? enEditorAttachMethod.ScanAttach :
                     transaction.MainDocument?.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Octet ?  enEditorAttachMethod.ScanAttach : enEditorAttachMethod.HtmlAttach,
                     title = "أصل المعاملة", //ToDo
                     user =transaction.MainDocument==null ? "" :  userProfile.Id== transaction.MainDocument.CreatedBy.Value ?
                     userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text
                     :   userMobileBL.GetUserById(transaction.MainDocument.CreatedBy.Value)?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text, //ToDo
                     UserID =transaction.MainDocument!=null ?  transaction.MainDocument.CreatedBy.Value : 0,
                     fileName =  "أصل المعاملة",
                     MimeContent = transaction.MainDocument?.MimeType,
                     IncludedItemId = -1,
                     LastModifiedOn = transaction.MainDocument.ModefiedOn ?? transaction.MainDocument.CreatedOn

                    }
                };
                }
                else
                {
                    transData.archiveRecords = new List<ArchiveRecord>();
                }

                //Attachements
                if (transaction.Attachments.Count > 0)
                {
                    foreach (var attachment in transaction.Attachments)
                    {
                        if (attachment.DocumentInfo != null)
                        {
                            transData.archiveRecords.Add(new ArchiveRecord()
                            {
                                attachRecoredId = attachment.Id,
                                docId = attachment.DocumentInfo.Id,
                                transId = attachment.TransactionId,
                                type = enArchivingType.IncludedItem,
                                securtyLevel = transaction.ConfidentialityId,
                                securityLevelDesc = transaction.Confidentiality.LocalName,
                                date = DateTimeUtility.ConvertToUmAlQuraCalendar(attachment.DocumentInfo.CreatedOn),
                                method = attachment.DocumentInfo.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf ? enEditorAttachMethod.ScanAttach : enEditorAttachMethod.HtmlAttach,
                                title = "المرفقات", //ToDo
                                user = userProfile.Id == attachment.DocumentInfo.CreatedBy.Value ? userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text
                                : userMobileBL.GetUserById(attachment.DocumentInfo.CreatedBy.Value)?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text, //ToDo
                                UserID = attachment.DocumentInfo.CreatedBy.Value,
                                fileName = transaction.Attachments.Where(t => t.Id == attachment.Id).FirstOrDefault().Type.Text,
                                MimeContent = attachment.DocumentInfo.MimeType,
                                IncludedItemId = attachment.TypeId
                            });
                        }
                    }
                }
                //Explanations
                IList<YESSERDomain.Explanation> textExplanations = transaction.Explanations.Where(t => t.ExplanationEditorType == (int)EditorType.Text).ToList();
                if (textExplanations.Count > 0)
                {
                    foreach (var textExplanation in textExplanations)
                    {
                        transData.archiveRecords.Add(new ArchiveRecord()
                        {
                            attachRecoredId = textExplanation.Id,
                            docId = textExplanation.Document.Id,
                            transId = textExplanation.TransactionId,
                            type = enArchivingType.Explaination,
                            securtyLevel = textExplanation.Permission.Id,
                            securityLevelDesc = textExplanation.Permission != null ? textExplanation.Permission.LocalName : string.Empty,
                            date = DateTimeUtility.ConvertToUmAlQuraCalendar(textExplanation.Document.CreatedOn),
                            method = enEditorAttachMethod.TextAttach,
                            title = "الشروحات", //ToDo
                            user = userProfile.Id == textExplanation.Document.CreatedBy.Value ? userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text
                            : userMobileBL.GetUserById(textExplanation.Document.CreatedBy.Value)?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text, //ToDo
                            UserID = textExplanation.Document.CreatedBy.Value,
                            fileName = textExplanation.Document.Name ?? string.Empty,
                            MimeContent = System.Net.Mime.MediaTypeNames.Text.Plain,
                            IncludedItemId = -1,
                            PrivilegeName = textExplanation.Permission.Code
                        });
                    }
                }

                IList<YESSERDomain.Explanation> pdfExplanations = transaction.Explanations.Where(t => t.ExplanationEditorType == (int)EditorType.Scanning).ToList();
                if (textExplanations.Count > 0)
                {
                    foreach (var pdfExplanation in pdfExplanations)
                    {
                        transData.archiveRecords.Add(new ArchiveRecord()
                        {
                            attachRecoredId = pdfExplanation.Id,
                            docId = pdfExplanation.Document.Id,
                            transId = pdfExplanation.TransactionId,
                            type = enArchivingType.Explaination,
                            securtyLevel = pdfExplanation.Permission.Id,
                            securityLevelDesc = pdfExplanation.Permission != null ? pdfExplanation.Permission.LocalName : string.Empty,
                            date = DateTimeUtility.ConvertToUmAlQuraCalendar(pdfExplanation.Document.CreatedOn),
                            method = enEditorAttachMethod.ScanAttach,
                            title = "الشروحات", //ToDo
                            user = userProfile.Id == pdfExplanation.Document.CreatedBy.Value ? userProfile.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text
                            : userMobileBL.GetUserById(pdfExplanation.Document.CreatedBy.Value)?.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text, //ToDo
                            UserID = pdfExplanation.Document.CreatedBy.Value,
                            fileName = pdfExplanation.Document.Name ?? string.Empty,
                            MimeContent = System.Net.Mime.MediaTypeNames.Application.Pdf,
                            IncludedItemId = -1,
                            PrivilegeName = pdfExplanation.Permission.Code
                        });
                    }
                }

                //transData.BarcodeData = GetTransactionBarcodes(transId, transaction.Assignments[0].ToEntity.Id, "ar");

                //predefined assigness
                transData.predefinedAssignees = new List<PredefinedAssignee>();

                getResult = GetResult<TransData>.Create(statusCode, transData, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransData>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransData>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        [HttpPost]
        public HttpResponseMessage UpdateTransaction(TransData transData, int userId, int EntityId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IEditorBL editorBL = new EditorBL();
                    ILookupBL lookupBL = new LookupBL();
                    IOrgUnitBL orgunitBL = new OrgUnitBL();
                    ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transData.TransCategory.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();
                    YESSERDomain.Transaction getTransOld = userMobileBL.GetTransaction(transData.TransId, Language);
                    YESSERDomain.Transaction transaction = new YESSERDomain.Transaction()
                    {
                        Id = transData.TransId,
                        Number = long.Parse(transData.TransNo),
                        TransactionCategoryId = transData.TransCategory,
                        TransactionTypeId = transData.TransSource,
                        ConfidentialityId = transData.ConfidId,
                        PriorityId = transData.PriorityId,
                        LetterTypeId = transData.TypeId,
                        RemindDate = transData.PriorityDate,
                        RemindDateH = transData.PriorityDateHJ,
                        Date = transData.TransDate,
                        DateH = transData.TransDateHJ,
                        StatusId = transData.Status,
                        Year = transData.Year,
                        Remarks = transData.Remarks,
                        Subject = transData.Subject,
                        ToUserId = transData.InitialAssignToPersonId != 0 ? transData.InitialAssignToPersonId : (int?)null,
                        UserId = transData.UserId,
                        OrgUnitId = getTransOld.Assignments.FirstOrDefault().ToEntity.Id,
                        DocumentNumber = transData.ExtTransNo,
                        DeliveryMethodId = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty),
                        EntityId = transData.ConcernedEntityId,
                        ProcessPeriodTransaction = 10,
                        ExternalPartyId = getTransOld.ExternalPartyId,
                        InboundDateH = getTransOld.InboundDateH,


                    };

                    List<ArchiveRecord> mainArchives = transData.archiveRecords.Where(m => m.type == enArchivingType.TransSourceID && (m.DocData != null || m.RowStatus == (int)DataRowStatus.Deleted)).ToList();
                    List<ArchiveRecord> includedItemArchives = transData.archiveRecords.Where(m => m.type == enArchivingType.IncludedItem && (m.DocData != null || m.RowStatus == (int)DataRowStatus.Deleted)).ToList();
                    List<ArchiveRecord> explanationArchives = transData.archiveRecords.Where(m => m.type == enArchivingType.Explaination && (m.DocData != null || m.RowStatus == (int)DataRowStatus.Deleted)).ToList();

                    //Incuded Items
                    transaction.Attachments = new List<YESSERDomain.Attachment>();
                    var includeItem = getTransOld.Attachments.ToList();
                    if (transData.IncludedItems.Count > 0)
                    {
                        foreach (var includedItem in includeItem)
                        {
                            transaction.Attachments.Add(new YESSERDomain.Attachment()
                            {
                                Id = includedItem.Id,
                                TypeId = includedItem.TypeId,
                                Count = includedItem.Count,
                                Description = includedItem.Description,
                                CreatedOn = DateTime.Now,
                                CreatedBy = userId,

                            });
                        }
                    }

                    transaction.ExternalCopies = new List<YESSERDomain.TransactionExternalCopy>();

                    //Attachements
                    if (includedItemArchives.Any())
                    {
                        YESSERDomain.Attachment attachment;
                        YESSERDomain.Document document = null;
                        YESSERDomain.DocumentInfo documentInfo;
                        foreach (var archiveRecord in includedItemArchives)
                        {
                            switch (archiveRecord.RowStatus)
                            {
                                case (int)DataRowStatus.Added:
                                case (int)DataRowStatus.Modified:
                                    attachment = transaction.Attachments.Where(a => a.Id == archiveRecord.attachRecoredId).FirstOrDefault();
                                    if (attachment == null)
                                    {
                                        attachment = new YESSERDomain.Attachment { TypeId = (int)archiveRecord.type };
                                    }
                                    if (archiveRecord.DocData != null && archiveRecord.DocData.Count() > 0)
                                    {
                                        document = new YESSERDomain.Document()
                                        {
                                            Content = archiveRecord.DocData
                                        };
                                    }
                                    documentInfo = new YESSERDomain.DocumentInfo()
                                    {
                                        MimeType = archiveRecord.MimeContent,
                                        Name = archiveRecord.fileName,
                                        Size = archiveRecord.DocData.Length,
                                        Document = document,
                                        IsDeleted = false,
                                        FromEntityId = transaction.OrgUnitId,
                                        FromUserId = transaction.UserId
                                    };
                                    attachment.DocumentInfo = documentInfo;
                                    if (transaction.Attachments.Where(a => a.Id == archiveRecord.attachRecoredId).FirstOrDefault() == null)
                                    {
                                        transaction.Attachments.Add(attachment);
                                    }

                                    break;
                                case (int)DataRowStatus.Deleted:
                                    transactionBL.DeleteDocument(archiveRecord.docId);

                                    break;

                                default:
                                    break;
                            }

                        }
                    }

                    //MainArchive
                    if (mainArchives.Any())
                    {
                        foreach (var mainArchive in mainArchives)
                        {
                            transaction.MainDocument = new YESSERDomain.DocumentInfo()
                            {
                                Id = Convert.ToInt32(mainArchive.attachRecoredId),
                                Name = mainArchive.fileName,
                                MimeType = mainArchive.method == enEditorAttachMethod.ScanAttach ? System.Net.Mime.MediaTypeNames.Application.Pdf : System.Net.Mime.MediaTypeNames.Text.Plain,
                                Size = mainArchive.DocData == null ? 0 : mainArchive.DocData.Length,
                                FromEntityId = transData.EntityId,
                                FromUserId = transData.UserId,
                                Document = new YESSERDomain.Document()
                                {
                                    Content = null
                                },
                                ModefiedBy = userId,
                                ModefiedOn = DateTime.Now
                            };
                        }
                    }

                    transaction.Copies = new List<YESSERDomain.TransactionCopy>();
                    var itemsAddedOrUpdated = transData.TransCopies.Where(r => r.RowStatus != (int)DataRowStatus.Deleted);
                    if (transData.TransCopies.Count > 0)
                    {
                        foreach (var transCopies in itemsAddedOrUpdated)
                        {
                            transaction.Copies.Add(new YESSERDomain.TransactionCopy()
                            {
                                Id = transCopies.TransPartyId,
                                EntityId = transCopies.PartyID,
                                UserId = transCopies.PersonID,
                                ActionId = transCopies.ProcessId,
                                DateH = transCopies.SendDateHJ,
                                IsSent = 1,
                                SentDate = DateTime.Now,
                                FromEntityId = transData.EntityId,
                                FromUserId = transData.UserId,
                                CreatedOn = DateTime.Now,
                                Date = DateTime.Now,
                                CreatedBy = transData.UserId,
                                GeneralExplanation = transData.AssignEntity != null ? transData.AssignEntity.Remarks : null
                            });
                        }
                    }

                    transaction.Links = new List<YESSERDomain.TransactionLink>();
                    var userTray = TrayBaseBL.GetUserTrays(userId, transData.EntityId, null);



                    if (transData.IsSigned && transData.TransCategory == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, Language))
                    {


                        int DefualtAction = Convert.ToInt32(ConfigurationManager.AppSettings["DefualtSignAction"] ?? "1");


                        int transCategoryLookupId;
                        bool isInternalOutboundDraft = !transaction.ExternalPartyId.HasValue;
                        int? ioDepartment = orgunitBL.getIoDepartment(LoggedInOrgUnitId);
                        int? GeneralIoDepartment = Convert.ToInt32(ConfigurationManager.AppSettings["GeneralIoDepartment"] ?? null); //orgunitBL.getGeneralIoDepartment();
                        transaction.IsDraft = false;
                        transaction.OldWordDocumntId = transaction.MainDocumentId;
                        transaction.MainDocumentId = 0;
                        transaction.IsElcOutBound = true;
                        if (!transaction.IsPresentationDraft)
                            transaction.OutBoundDraftNumber = transaction.Number;

                        if (!isInternalOutboundDraft)
                        {
                            transCategoryLookupId = Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, Language);
                            transaction.TransactionCategoryId = transCategoryLookupId;
                            transaction.StatusId = Common.TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, Language);

                            transaction.PrintedDeliveryReport = true;
                            if (!transaction.IsPresentationDraft)
                                (transactionBL as OutboundExternalBL).GetNewExternalOutboundNumber(transaction);
                        }
                        else
                        {
                            transCategoryLookupId = Common.TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, Language);
                            transaction.TransactionCategoryId = transCategoryLookupId;
                            transaction.StatusId = Common.TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, Language);

                            transactionBL = TransactionBL.Create(TransactionCategory.InternalOutbound);

                            if (!transaction.IsPresentationDraft)
                                (transactionBL as OutboundInternalBL).GetNewInternalOutboundNumber(transaction);
                        }

                        transaction.TransactionCategory = lookupBL.GetLookupItem(transCategoryLookupId);


                        transaction.IsPresentationDraft = false;

                        //check if 
                        if (isInternalOutboundDraft)
                        {
                            if (orgunitBL.ReceiveElcOutBoundWithAcknowled(Convert.ToInt32(transaction.EntityId)))
                                transaction.NeedAcknowled = true;

                        }
                        foreach (YESSERDomain.TransactionCopy transactionCopy in transaction.Copies)
                        {

                            transactionCopy.IsSent = transaction.NeedAcknowled ? 0 : 1;

                        }


                        transactionBL.Update(transaction);

                        if (!isInternalOutboundDraft && ioDepartment.HasValue)
                        {
                            YESSERDomain.TransactionAssignment transAssign = new YESSERDomain.TransactionAssignment
                            {
                                FromUserId = userId,
                                FromEntityId = LoggedInOrgUnitId,
                                ToEntityId = ioDepartment.Value,
                                ActionId = DefualtAction,
                                DeliveryMethodId = Common.DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, Language),
                                TrayId = (int)TrayType.MyTransactions
                            };

                            IList<YESSERDomain.TransactionAssignment> transactionAssignments = new List<YESSERDomain.TransactionAssignment>();
                            transactionAssignments.Add(transAssign);

                            editorBL.AssignTransaction(transaction.Id, transactionAssignments, Language);
                        }
                        else if (isInternalOutboundDraft)
                        {
                            YESSERDomain.TransactionAssignment transAssign = new YESSERDomain.TransactionAssignment
                            {
                                FromUserId = userId,
                                FromEntityId = LoggedInOrgUnitId,
                                //ToUserId = draftTransaction.ExternalPartyManager.Id,
                                ToEntityId = transaction.Entity.Id,
                                ActionId = DefualtAction,
                                DeliveryMethodId = Common.DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, Language),
                                TrayId = (int)TrayType.OrgUnit
                            };

                            IList<YESSERDomain.TransactionAssignment> transactionAssignments = new List<YESSERDomain.TransactionAssignment>();
                            transactionAssignments.Add(transAssign);

                            editorBL.AssignTransaction(transaction.Id, transactionAssignments, Language);
                        }

                        if (ioDepartment.HasValue && ioDepartment.Value > 0)
                        {

                            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO();
                            transactionElcOutBoundDTO.TransactionId = transaction.Id;
                            transactionElcOutBoundDTO.EntityId = ioDepartment.Value;
                            transactionElcOutBoundDTO.Ishidden = false;
                            transactionElcOutBoundDTO.CreatedOn = DateTime.Now;
                            transactionElcOutBoundDTO.CreatedBy = userId;

                            TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));

                        }
                        else if (GeneralIoDepartment.HasValue && GeneralIoDepartment.Value > 0)
                        {

                            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO();
                            transactionElcOutBoundDTO.TransactionId = transaction.Id;
                            transactionElcOutBoundDTO.EntityId = GeneralIoDepartment.Value;
                            transactionElcOutBoundDTO.Ishidden = false;
                            transactionElcOutBoundDTO.CreatedOn = DateTime.Now;
                            transactionElcOutBoundDTO.CreatedBy = userId;

                            TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));
                        }
                        TransactionBL.TransactionElcOutBoundUpdate(userId, LoggedInOrgUnitId, false, transaction.Id);


                    }
                    else if (transData.TrayId == (int)TrayType.Manager || (transData.AssignEntity != null && transData.AssignEntity.EntityId > 0
                        && transaction.ToUserId != transData.AssignEntity.PersonId))
                    {
                        ITrayBL trayBL = TrayBaseBL.Create(TrayType.Manager);
                        int? assignId = null;
                        if (transData.AssignEntity.PersonId > 0)
                        {
                            assignId = transData.AssignEntity.PersonId;
                        }
                        int electronicDelivery = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                        ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                        IList<YESSERDomain.TransactionAssignment> transactionAssignments = transactionAssignmentBL.GetTransactionAssignments(transData.TransId, null);
                        transactionAssignments.ToList().ForEach(x =>
                        {
                            x.FromEntityId = EntityId;
                            x.FromUserId = userId;
                            x.ToEntityId = transData.AssignEntity.EntityId;
                            x.ToUserId = assignId;
                            x.ActionId = transData.AssignEntity.ProcessId;
                            x.DeliveryMethodId = electronicDelivery;
                            x.Description = transData.AssignEntity.Remarks;

                        });
                        trayBL.ManagerAssign(transData.TransId, transactionAssignments.FirstOrDefault().Id, transactionAssignments, transData.AssignEntity.EntityId, cultureName);

                    }
                    else
                    {
                        transactionBL.Update(transaction);
                    }


                    if (mainArchives.Count > 0)
                    {
                        DocData docData = new DocData()
                        {
                            Data = mainArchives[0].DocData,
                            DocName = transaction.MainDocument.Name,
                            DocID = transaction.MainDocument.Id.ToString(),
                            PersonId = transaction.MainDocument.CreatedBy,
                            MimeContent = transaction.MainDocument.MimeType,
                            EntityId = transaction.OrgUnitId,
                            DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                            User_ID = transaction.MainDocument.CreatedBy.ToString(),
                            TransactionId = transaction.Id
                        };
                        DocRepository.DocRepository.Save(docData, new DocumentLocation());
                    }


                    if (transaction.Attachments != null && transaction.Attachments.Any())
                    {
                        byte[] AttachmentDocumentContent = null;
                        foreach (YESSERDomain.Attachment attachment in transaction.Attachments)
                        {
                            if (attachment.DocumentInfo != null)
                            {
                                if (attachment.DocumentInfo.Document != null)
                                {
                                    AttachmentDocumentContent = attachment.DocumentInfo.Document.Content;
                                    attachment.DocumentInfo.Document.Content = null;

                                    DocData docData = new DocData()
                                    {
                                        Data = AttachmentDocumentContent,
                                        DocName = attachment.DocumentInfo.Name,
                                        DocID = attachment.DocumentInfo.Id.ToString(),
                                        PersonId = attachment.DocumentInfo.Document.CreatedBy,
                                        MimeContent = attachment.DocumentInfo.MimeType,
                                        EntityId = transaction.EntityId,
                                        DataSize = Convert.ToInt32(attachment.DocumentInfo.Size),
                                        User_ID = attachment.DocumentInfo.Document.CreatedBy.ToString(),
                                        TransactionId = transaction.Id
                                    };
                                    DocRepository.DocRepository.Save(docData, new DocumentLocation());
                                }
                            }
                        }
                    }

                    //Explanation
                    transaction.Explanations = new List<YESSERDomain.Explanation>();

                    if (explanationArchives.Any())
                    {
                        foreach (var explanationArchive in explanationArchives)
                        {

                            YESSERDomain.Explanation explanation = new YESSERDomain.Explanation()
                            {
                                Id = Convert.ToInt32(explanationArchive.attachRecoredId),
                                PermissionId = explanationArchive.securtyLevel,
                                Document = new YESSERDomain.DocumentInfo
                                {
                                    Name = explanationArchive.fileName,
                                    MimeType = explanationArchive.method == enEditorAttachMethod.ScanAttach ? System.Net.Mime.MediaTypeNames.Application.Pdf : System.Net.Mime.MediaTypeNames.Text.Plain,
                                    Size = explanationArchive.DocData.Length,
                                    Document = new YESSERDomain.Document()
                                    {
                                        Content = explanationArchive.method == enEditorAttachMethod.ScanAttach ? explanationArchive.DocData
                                        : System.Text.Encoding.Unicode.GetBytes(System.Text.Encoding.UTF8.GetString(explanationArchive.DocData))
                                    },
                                    FromEntityId = transaction.OrgUnitId,
                                    FromUserId = transaction.UserId
                                },
                                CreatedOn = DateTime.Now,
                                CreatedBy = explanationArchive.UserID,
                                Date = !string.IsNullOrWhiteSpace(explanationArchive.date) ? Convert.ToDateTime(explanationArchive.date) : DateTime.Now,
                                ExplanationEditorType = (int)explanationArchive.method == (int)enEditorAttachMethod.TextAttach ? (int)EditorType.Text : (int)EditorType.Scanning
                            };
                            if (explanationArchive.RowStatus == (int)DataRowStatus.Added)
                            {
                                explanation.Id = 0;
                                editorBL.AddTransactionExplanation(explanationArchive.transId, explanation, Language);
                            }
                            else if (explanationArchive.RowStatus == (int)DataRowStatus.Modified)
                            {
                                editorBL.UpdateExplanation(explanation);
                            }
                            else if (explanationArchive.RowStatus == (int)DataRowStatus.Deleted)
                            {
                                editorBL.DeleteExplanation(explanation.Id);
                            }
                        }
                    }

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostAssignTransaction(int transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs, string followUp = "false")
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                        IList<Domain.TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                        transactionAssignments.ToList().ForEach(a => a.TransactionId = transactionId);

                        editorBL.AssignTransaction(transactionId, transactionAssignments, Language);

                        Domain.Transaction transaction = TransactionBL.GetTransactionById(transactionId);


                        putResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpPut]
        public HttpResponseMessage RejectTransaction(int transactionId, int orgUnitId, string remarks, int userId,string cultureName,object extraParams)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL(); 

                    transactionAssignmentBL.RejectTransactionMobile(transactionId, orgUnitId, (int)TrayType.MyTransactions, remarks, cultureName, userId);

                    putResult = PutResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage AssignTransaction(TransData transData)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {

                    List<YESSERDomain.TransactionAssignment> transactionAssignments = new List<YESSERDomain.TransactionAssignment> {
            new YESSERDomain.TransactionAssignment
            {
                ActionId=transData.AssignEntity.ProcessId,
                ToUserId=transData.AssignEntity.PersonId,
                ToEntityId=transData.AssignEntity.EntityId,
                Description=transData.AssignEntity.Remarks,
                DeliveryMethodId=(int)DeliveryMethodType.Electronic,
                TransactionId=transData.TransId,
                FromEntityId=transData.AssignTrack.ElcEntityId,
                FromUserId=transData.AssignTrack.ElcUserId,
                TransactionAssignmentProcessPeriod=DateTime.Now.AddDays(5)

            },


            };
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();
                    userMobileBL.AssignTransaction(transData.TransId, transactionAssignments, Language);
                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }



            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage AssignItBack(int TransId, string Notes, int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.AssignItBack(TransId, userId, entityId, (int)TrayType.MyTransactions, Notes, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }
        public HttpResponseMessage AssignItBackVip(int TransId, string Notes, int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.AssignItBackVip(TransId, userId, entityId, (int)TrayType.MyTransactions, Notes, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage UserMobileGetExternalOrgHierarchy(int? parentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserMobileExternalPartyDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    IList<YESSERDomain.ExternalParty> externalParties = mobileApiBL.UserMobileGetExternalParties(parentId, Language);

                    List<UserMobileExternalPartyDTO> userMobileExternalPartyDTOs = UserMobileMapper.ExternalPartiesMap(externalParties, Language);

                    getResult = GetResult<List<UserMobileExternalPartyDTO>>.Create(statusCode, userMobileExternalPartyDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserMobileExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserMobileExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage UserMobileGetExternalOrgHierarchyAC(string searchQuery)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserMobileExternalPartyDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    IList<YESSERDomain.ExternalParty> externalParties = mobileApiBL.UserMobileGetExternalPartiesAC(searchQuery, Language);

                    List<UserMobileExternalPartyDTO> userMobileExternalPartyDTOs = UserMobileMapper.ExternalPartiesMap(externalParties, Language);

                    getResult = GetResult<List<UserMobileExternalPartyDTO>>.Create(statusCode, userMobileExternalPartyDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserMobileExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserMobileExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage SpecializeTransaction(int TransId, int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.SpecializeTransaction(TransId, userId, entityId, (int)TrayType.OrgUnit, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTrayTransactions(int userId, int entityId, int trayId, bool isAscending)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<Transaction>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    List<int> filteredTrayIds = new List<int> { 1, 99, 100 };
                    IList<YESSERDomain.Transaction> transactions = new List<YESSERDomain.Transaction>();

                    TransactionDateType transactionDateType = TransactionDateType.Any;
                    TrayType trayType = (TrayType)Enum.ToObject(typeof(TrayType), trayId);

                    if (filteredTrayIds.Contains(trayId))
                    {
                        foreach (int trayIdItem in filteredTrayIds)
                        {
                            transactionDateType = TransactionDateType.Any;
                            trayType = (TrayType)Enum.ToObject(typeof(TrayType), trayId);

                            switch (trayIdItem)
                            {
                                case 99:
                                    transactionDateType = TransactionDateType.HasDate;
                                    trayId = (int)TrayType.MyTransactions;
                                    break;
                                case 100:
                                    transactionDateType = TransactionDateType.Late;
                                    trayId = (int)TrayType.MyTransactions;
                                    break;
                                default:
                                    break;
                            }

                            IList<YESSERDomain.Transaction> transactionList = userMobileBL.UserMobileGetTrayTransactions(userId, entityId, trayType, transactionDateType, new YESSERMobileDomain.FilterCriteria(), Language, isAscending).ToList();
                            foreach (YESSERDomain.Transaction transactionItem in transactionList)
                            {
                                bool addedTransaction = transactions.Select(s => s.Id).Contains(transactionItem.Id);
                                if (!addedTransaction)
                                {
                                    transactionItem.SourceTray = (int)transactionDateType > 1 ? transactionDateType : TransactionDateType.Any;
                                    transactionItem.IsAppointment = transactionItem.IsDelayed = false;

                                    switch (transactionItem.SourceTray)
                                    {
                                        case TransactionDateType.Any:
                                            break;
                                        case TransactionDateType.Today:
                                            break;
                                        case TransactionDateType.HasDate:
                                            transactionItem.IsAppointment = true;
                                            break;
                                        case TransactionDateType.Late:
                                            transactionItem.IsDelayed = true;
                                            break;
                                        default:
                                            break;
                                    }
                                    transactions.Add(transactionItem);

                                }
                                else if (addedTransaction && (int)transactionDateType > 1)
                                {
                                    transactions.First(s => s.Id == transactionItem.Id).SourceTray = transactionDateType;
                                    transactionItem.IsAppointment = transactionItem.IsDelayed = false;

                                    switch (transactionItem.SourceTray)
                                    {
                                        case TransactionDateType.Any:
                                            break;
                                        case TransactionDateType.Today:
                                            break;
                                        case TransactionDateType.HasDate:
                                            transactionItem.IsAppointment = true;
                                            break;
                                        case TransactionDateType.Late:
                                            transactionItem.IsDelayed = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        transactions = userMobileBL.UserMobileGetTrayTransactions(userId, entityId, trayType, transactionDateType, new YESSERMobileDomain.FilterCriteria(), Language, isAscending);
                    }


                    List<Transaction> userMobileTransactions = UserMobileMapper.TransactionsMap(transactions, trayId, Language).Distinct().ToList();

                    getResult = GetResult<List<Transaction>>.Create(statusCode, userMobileTransactions, null);


                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<Transaction>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<Transaction>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage AssignmentTrack(int transId, int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransAssignTrack> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    YESSERDomain.TransactionAssignment transactionAssignment = userMobileBL.GetTransactionAssignment(transId, Language);
                    List<YESSERDomain.TransactionAssignmentHistory> transactionAssignmentHistories = userMobileBL.GetTransactionAssignmentHistory(transId, Language);

                    TransAssignTrack transAssignTrack = new TransAssignTrack()
                    {
                        ElcUserId = transactionAssignment.ToUserId ?? -1,
                        ElcEntityId = transactionAssignment.ToEntityId,
                        ElcUser = transactionAssignment.ToUser?.LocalName,
                        ElcEntity = transactionAssignment.ToEntity.LocalName,
                        PhysicalUser = transactionAssignment.PhysicalUser.LocalName,
                        PhysicalEntity = transactionAssignment.PhysicalEntity.LocalName,
                        ElcDate = transactionAssignment.DateH + " " + transactionAssignment.Date.ToShortTimeString(),
                        PhysicalDate = transactionAssignment.DateH + " " + transactionAssignment.Date.ToShortTimeString(),
                        Assignments = UserMobileMapper.TransactionAssignmentHistoryMap(transactionAssignmentHistories)
                    };

                    getResult = GetResult<TransAssignTrack>.Create(statusCode, transAssignTrack, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransAssignTrack>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransAssignTrack>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserAccompleshmentsReport(int userId, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserAccomplishmentReportInfo> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();
                    YESSERDomain.UserAccompleshmentsReportResult userAccompleshmentsReportResult = userMobileBL.GetUserAccompleshmentsReport(userId, entityId);

                    UserAccomplishmentReportInfo userAccomplishmentReportInfo = new UserAccomplishmentReportInfo()
                    {
                        TransactionCount = userAccompleshmentsReportResult.TRANSACTIONS,
                        DelayedCount = userAccompleshmentsReportResult.DELAYED,
                        WithAppointmentCount = userAccompleshmentsReportResult.WITH_APPOITMENT,
                        TransPartiesCount = userAccompleshmentsReportResult.TRANS_PARTIES
                    };

                    getResult = GetResult<UserAccomplishmentReportInfo>.Create(statusCode, userAccomplishmentReportInfo, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserAccomplishmentReportInfo>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserAccomplishmentReportInfo>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetEntitiesAccompleshmentsReport(int entityId, int periodCount, int selectedPeriod)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<EntityAccomplishmentReportInfoResult>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();
                    List<YESSERDomain.EntitiesAccompleshmentsReportResult> entitiesAccompleshmentsReportResults = userMobileBL.GetEntitiesAccompleshmentsReport(entityId, periodCount, selectedPeriod);

                    List<EntityAccomplishmentReportInfoResult> entityAccomplishmentReportInfoResults = entitiesAccompleshmentsReportResults.Select(e => new EntityAccomplishmentReportInfoResult
                    {
                        FROM_DATE = e.FROM_DATE,
                        TO_DATE = e.TO_DATE,
                        TRANSACTIONS = e.TRANSACTIONS,
                        DELAYED = e.DELAYED,
                        WITH_APPOITMENT = e.WITH_APPOITMENT,
                        TRANS_PARTIES = e.TRANS_PARTIES
                    }).ToList();

                    getResult = GetResult<List<EntityAccomplishmentReportInfoResult>>.Create(statusCode, entityAccomplishmentReportInfoResults, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<EntityAccomplishmentReportInfoResult>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<EntityAccomplishmentReportInfoResult>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage FilterTrayTransactions(int userId, int entityId, int trayId, FilterCriteria filterCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<Transaction>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    YESSERMobileDomain.FilterCriteria filter = new YESSERMobileDomain.FilterCriteria()
                    {
                        TransNo = filterCriteria.TransNo,
                        Subject = filterCriteria.Subject,
                        FromAssignDate = filterCriteria.FromAssignDate,
                        ToAssignDate = filterCriteria.ToAssignDate,
                        TransSource = filterCriteria.TransSource
                    };


                    List<int> filteredTrayIds = new List<int> { 1, 99, 100 };
                    IList<YESSERDomain.Transaction> transactions = new List<YESSERDomain.Transaction>();

                    TransactionDateType transactionDateType = TransactionDateType.Any;
                    TrayType trayType = (TrayType)Enum.ToObject(typeof(TrayType), trayId);

                    if (filteredTrayIds.Contains(trayId))
                    {
                        foreach (int trayIdItem in filteredTrayIds)
                        {
                            transactionDateType = TransactionDateType.Any;
                            trayType = (TrayType)Enum.ToObject(typeof(TrayType), trayId);
                            switch (trayIdItem)
                            {
                                case 99:
                                    transactionDateType = TransactionDateType.HasDate;
                                    trayId = (int)TrayType.MyTransactions;
                                    break;
                                case 100:
                                    transactionDateType = TransactionDateType.Late;
                                    trayId = (int)TrayType.MyTransactions;
                                    break;
                                default:
                                    break;
                            }

                            List<YESSERDomain.Transaction> transactionList = userMobileBL.UserMobileGetTrayTransactions(userId, entityId, trayType, transactionDateType, filter, Language);
                            foreach (YESSERDomain.Transaction transactionItem in transactionList)
                            {
                                bool addedTransaction = transactions.Select(s => s.Id).Contains(transactionItem.Id);
                                if (!addedTransaction)
                                {
                                    transactionItem.SourceTray = (int)transactionDateType > 1 ? transactionDateType : TransactionDateType.Any;

                                    transactionItem.IsAppointment = transactionItem.IsDelayed = false;

                                    switch (transactionItem.SourceTray)
                                    {
                                        case TransactionDateType.Any:
                                            break;
                                        case TransactionDateType.Today:
                                            break;
                                        case TransactionDateType.HasDate:
                                            transactionItem.IsAppointment = true;
                                            break;
                                        case TransactionDateType.Late:
                                            transactionItem.IsDelayed = true;
                                            break;
                                        default:
                                            break;
                                    }

                                    transactions.Add(transactionItem);
                                }
                                else if (addedTransaction && (int)transactionDateType > 1)
                                {
                                    transactions.First(s => s.Id == transactionItem.Id).SourceTray = transactionDateType;

                                    transactionItem.IsAppointment = transactionItem.IsDelayed = false;

                                    switch (transactionItem.SourceTray)
                                    {
                                        case TransactionDateType.Any:
                                            break;
                                        case TransactionDateType.Today:
                                            break;
                                        case TransactionDateType.HasDate:
                                            transactionItem.IsAppointment = true;
                                            break;
                                        case TransactionDateType.Late:
                                            transactionItem.IsDelayed = true;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        transactions = userMobileBL.UserMobileGetTrayTransactions(userId, entityId, trayType, transactionDateType, filter, Language);
                    }


                    List<Transaction> userMobileTransactions = UserMobileMapper.TransactionsMap(transactions, trayId, Language);

                    getResult = GetResult<List<Transaction>>.Create(statusCode, userMobileTransactions, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                GetResult<List<Transaction>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                GetResult<List<Transaction>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage MobileSearch(SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SearchTransactionDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    YESSERMobileDomain.SearchCriteria search = new YESSERMobileDomain.SearchCriteria()
                    {
                        TransNo = searchCriteria.TransNo,
                        Subject = searchCriteria.Subject,
                        EntityId = searchCriteria.EntityId,
                        TransCategory = searchCriteria.TransCategory,
                        TransSource = searchCriteria.TransSource,
                        CreationDateFrom = searchCriteria.CreationDateFrom,
                        CreationDateTo = searchCriteria.CreationDateTo
                    };

                    List<YESSERDomain.MobileSearchResult> mobileSearchResults = userMobileBL.UserMobileSearchTransaction(search, Language);

                    List<SearchTransactionDTO> userMobileSearch = UserMobileMapper.SearchTransactionsMap(mobileSearchResults, Language);

                    getResult = GetResult<List<SearchTransactionDTO>>.Create(statusCode, userMobileSearch, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                GetResult<List<SearchTransactionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                GetResult<List<SearchTransactionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserPrivileges(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<string>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                    IList<MCS.Domain.Permission> userPermisionResult = mobileApiBL.GetUserPrivileges(userId, CurrentUserIdentity, Language);

                    List<string> userPrivileges = UserMobileMapper.UserPrivilegesMap(userPermisionResult, Language);

                    getResult = GetResult<List<string>>.Create(statusCode, userPrivileges, null);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<string>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<string>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #region Private Methods
        private List<TransactionProcess> GetAllActions()
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                List<YESSERDomain.Action> actions = mobileApiBL.GetAllActions(Language);

                return UserMobileMapper.GetAllActions(actions);
            }
        }



        private List<UserEntity> GetAllEntity(int userid)
        {


            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                var orgunits = mobileApiBL.GetAllEntities(Language, userid);

                return UserMobileMapper.UserEntityMap(orgunits, Language);

            }
        }
        private List<MobileApi.Domain.TransactionCategory> GetTransactionCategories()
        {
            return new List<MobileApi.Domain.TransactionCategory>() {
                        new MobileApi.Domain.TransactionCategory() { CategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), Id= TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty) , Text= TransactionCategory.Inbound.ToString() },
                        new MobileApi.Domain.TransactionCategory() { CategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), Id= TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty) , Text= TransactionCategory.ExternalOutbound.ToString() },
                        new MobileApi.Domain.TransactionCategory() { CategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), Id= TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty) , Text= TransactionCategory.InternalOutbound.ToString() },
                        new MobileApi.Domain.TransactionCategory() { CategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty), Id= TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty) , Text= TransactionCategory.DraftOutbound.ToString() }
                    };
        }
        private List<RowStatus> GetRowStatus()
        {
            return new List<RowStatus>() {
                new RowStatus() { CategoryId = (int)DataRowStatus.Added, Id= (int)DataRowStatus.Added , Text= DataRowStatus.Added.ToString() },
                new RowStatus() { CategoryId = (int)DataRowStatus.ChildModified, Id= (int)DataRowStatus.ChildModified , Text= DataRowStatus.ChildModified.ToString() },
                new RowStatus() { CategoryId = (int)DataRowStatus.Current, Id= (int)DataRowStatus.Current , Text= DataRowStatus.Current.ToString() },
                new RowStatus() { CategoryId = (int)DataRowStatus.Deleted, Id= (int)DataRowStatus.Deleted , Text= DataRowStatus.Deleted.ToString() },
                new RowStatus() { CategoryId = (int)DataRowStatus.Modified, Id= (int)DataRowStatus.Modified , Text= DataRowStatus.Modified.ToString() },
                new RowStatus() { CategoryId = (int)DataRowStatus.UnChanged, Id= (int)DataRowStatus.UnChanged , Text= DataRowStatus.UnChanged.ToString() },
            };
        }
        private TrayID GetTrayIds()
        {
            return new TrayID();
        }
        private ArchivingType GetArchivingTypes()
        {
            return new ArchivingType();
        }
        private List<AttachmentMethod> GetAttachmentMethods()
        {
            return new List<AttachmentMethod>() {
                new AttachmentMethod() { Id= (int)EditorType.TextEditor , Text= EditorType.TextEditor.ToString() },
                new AttachmentMethod() { Id= (int)EditorType.Scanning , Text= EditorType.Scanning.ToString() },
                new AttachmentMethod() { Id= (int)EditorType.Text , Text= EditorType.Text.ToString() },
                new AttachmentMethod() { Id= (int)EditorType.File , Text= EditorType.File.ToString() }
            };
        }
        private List<Confidentiality> GetConfidentialities(int groupId)
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                IList<YESSERDomain.Permission> permissions = mobileApiBL.GetPermisions(Language, groupId);

                return UserMobileMapper.ConfidentialityMap(permissions);
            }
        }
        private List<AttachConfidentiality> GetAttachConfidentialities(int groupId)
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                IList<YESSERDomain.Permission> permissions = mobileApiBL.GetPermisions(Language, groupId);

                return UserMobileMapper.AttachConfidentialityMap(permissions);
            }
        }
        private List<Permission> GetUserMobilePermissions(int userId)
        {
            //DataTable dtUser = GatewayService.GetUserIPad(userId.ToString(), userName, null, null, null, null, null, null);
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();

                List<Permission> permissions = new List<Permission>()
                {
                    new Permission()
                    {
                        Code = UserClaims.Outbound.CreateInternalOutbound,
                        Value = mobileApiBL.CheckIfUserHasPermission(userId, UserClaims.Outbound.CreateInternalOutbound)
                    },
                    new Permission()
                    {

                        Code = UserClaims.Outbound.CreateExternalOutbound,
                        Value = mobileApiBL.CheckIfUserHasPermission(userId, UserClaims.Outbound.CreateExternalOutbound)
                    },
                     new Permission()
                    {

                        Code = UserClaims.Inbound.CreateInbound,
                        Value = mobileApiBL.CheckIfUserHasPermission(userId, UserClaims.Inbound.CreateInbound)
                    },
                      new Permission()
                    {

                        Code = UserClaims.Outbound.CreateOutboundDraft,
                        Value = mobileApiBL.CheckIfUserHasPermission(userId, UserClaims.Outbound.CreateOutboundDraft)
                    }

                };
                return permissions;
            }
        }
        private List<TransactionSource> GetTransactionSources()
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                List<YESSERDomain.TransactionType> transactionTypes = mobileApiBL.GetTransactionSources(TransactionCategories.Inbound, Language);
                List<TransactionSource> transactionSources = UserMobileMapper.TransactionSourceMap(transactionTypes, TransactionCategories.Inbound);
                transactionTypes = mobileApiBL.GetTransactionSources(TransactionCategories.Outbound, Language);
                transactionSources.AddRange(UserMobileMapper.TransactionSourceMap(transactionTypes, TransactionCategories.Outbound));

                transactionTypes = mobileApiBL.GetTransactionSources(TransactionCategories.DraftOutbound, Language);
                transactionSources.AddRange(UserMobileMapper.TransactionSourceMap(transactionTypes, TransactionCategories.DraftOutbound));
                transactionTypes = mobileApiBL.GetTransactionSources(TransactionCategories.InternalOutbound, Language);
                transactionSources.AddRange(UserMobileMapper.TransactionSourceMap(transactionTypes, TransactionCategories.InternalOutbound));

                return transactionSources;
            }
        }
        private List<Priority> GetPriorities()
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                List<YESSERDomain.Priority> priorities = mobileApiBL.GetPriorities(TransactionCategories.Inbound, Language);
                List<Priority> userMobilePriorities = UserMobileMapper.PriorityMap(priorities, TransactionCategories.Inbound);
                priorities = mobileApiBL.GetPriorities(TransactionCategories.Outbound, Language);
                userMobilePriorities.AddRange(UserMobileMapper.PriorityMap(priorities, TransactionCategories.Outbound));
                //priorities = mobileApiBL.GetPriorities(TransactionCategories.Outbound, Language);
                userMobilePriorities.AddRange(UserMobileMapper.PriorityMap(priorities, TransactionCategories.DraftOutbound));
                priorities = mobileApiBL.GetPriorities(TransactionCategories.InternalOutbound, Language);
                userMobilePriorities.AddRange(UserMobileMapper.PriorityMap(priorities, TransactionCategories.InternalOutbound));
                return userMobilePriorities;
            }
        }
        private List<TransactionType> GetLetterType()
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                List<YESSERDomain.LetterType> letterTypes = mobileApiBL.GetLetterType(TransactionCategories.Inbound, Language);
                List<TransactionType> userMobileLetterTypes = UserMobileMapper.LetterTypeMap(letterTypes, TransactionCategories.Inbound);
                letterTypes = mobileApiBL.GetLetterType(TransactionCategories.Outbound, Language);
                userMobileLetterTypes.AddRange(UserMobileMapper.LetterTypeMap(letterTypes, TransactionCategories.Outbound));

                letterTypes = mobileApiBL.GetLetterType(TransactionCategories.InternalOutbound, Language);
                userMobileLetterTypes.AddRange(UserMobileMapper.LetterTypeMap(letterTypes, TransactionCategories.InternalOutbound));

                letterTypes = mobileApiBL.GetLetterType(TransactionCategories.DraftOutbound, Language);
                userMobileLetterTypes.AddRange(UserMobileMapper.LetterTypeMap(letterTypes, TransactionCategories.DraftOutbound));

                return userMobileLetterTypes;
            }
        }
        private List<IncludedItemType> GetAttachmentType()
        {
            using (var transactionContextScope = context.CreateReadOnly())
            {
                IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
                List<YESSERDomain.AttachmentType> attachmentTypes = mobileApiBL.GetAttachementsType(TransactionCategories.Inbound, Language);
                List<IncludedItemType> userMobileIncludedItemTypes = UserMobileMapper.AttachementsTypeMap(attachmentTypes, TransactionCategories.Inbound);
                attachmentTypes = mobileApiBL.GetAttachementsType(TransactionCategories.Outbound, Language);
                userMobileIncludedItemTypes.AddRange(UserMobileMapper.AttachementsTypeMap(attachmentTypes, TransactionCategories.Outbound));

                attachmentTypes = mobileApiBL.GetAttachementsType(TransactionCategories.DraftOutbound, Language);
                userMobileIncludedItemTypes.AddRange(UserMobileMapper.AttachementsTypeMap(attachmentTypes, TransactionCategories.DraftOutbound));

                attachmentTypes = mobileApiBL.GetAttachementsType(TransactionCategories.InternalOutbound, Language);
                userMobileIncludedItemTypes.AddRange(UserMobileMapper.AttachementsTypeMap(attachmentTypes, TransactionCategories.InternalOutbound));
                return userMobileIncludedItemTypes;
            }
        }
        private List<AttachmentType> GetLookupAttachementType()
        {
            var attchmentList = new List<AttachmentType>()
            {

            };

            GetTransactionCategories().ForEach(x =>
            {
                attchmentList.Add(new AttachmentType() { Id = (int)enArchivingType.TransSourceID, Text = "اصل المعاملة", CategoryId = x.CategoryId });
                attchmentList.Add(new AttachmentType() { Id = (int)enArchivingType.IncludedItem, Text = "المرفقات", CategoryId = x.CategoryId });
            });



            return attchmentList;


            //using (var transactionContextScope = context.CreateReadOnly())
            //{
            //    IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
            //    List<YESSERDomain.Lookup> attachmentTypes = mobileApiBL.GetLookupItems(LookupCategory.TransactionAttachmentType, Language);
            //    return UserMobileMapper.LookupAttachementsTypeMap(attachmentTypes);
            //}
        }
        private List<Tray> GetTrays(int userId, int entityId)
        {
            IUserMobileBL mobileApiBL = IoC.Resolve<IUserMobileBL>();
            List<TrayDetailsInfo> trays = mobileApiBL.GetUserTrays(userId, entityId, Language);
            return UserMobileMapper.UserTraysMap(trays);
        }
        private List<TransactionPartyDirection> GetTransPartyDirection()
        {
            return new List<TransactionPartyDirection>() { };
        }
        private PermissionName GetPermissionNames()
        {
            return new PermissionName();
        }

        [HttpGet]
        public HttpResponseMessage GetAssignmentPaper(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransAssignPaper>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();
                    //int Id = Convert.ToInt32(transactionId);

                    //YESSERDomain.AssignmentPaper transAssignPaper = userMobileBL.UserMobileGetAssignmentPaperByTransactionId(Id);
                    YESSERDomain.AssignmentPaper userAssignPaper = userMobileBL.UserMobileGetAssignmentPaperByUserId(userId, "ar");
                    List<TransAssignPaper> userMobileAssignPaper = new List<TransAssignPaper>();

                    //if (transAssignPaper != null)
                    //{
                    //    foreach (YESSERDomain.AssignmentPaperBeneficiary assignmentPaperBeneficiary in transAssignPaper.AssignmentPaperBeneficiaries)
                    //    {
                    //        TransAssignPaper assignPaper = new TransAssignPaper()
                    //        {
                    //            ToOrgUnitId = assignmentPaperBeneficiary.OrgUnitId,
                    //            ToOrgUnitName = assignmentPaperBeneficiary.OrgUnit.LocalName,
                    //            ToUserId = assignmentPaperBeneficiary.UserId.HasValue ? assignmentPaperBeneficiary.UserId.Value : 0,
                    //            ToUserName = assignmentPaperBeneficiary.User.LocalName
                    //        };
                    //        userMobileAssignPaper.Add(assignPaper);
                    //    }
                    //}

                    if (userAssignPaper != null)
                    {
                        foreach (YESSERDomain.AssignmentPaperBeneficiary assignmentPaperBeneficiary in userAssignPaper.AssignmentPaperBeneficiaries)
                        {
                            TransAssignPaper assignPaper = new TransAssignPaper()
                            {
                                PartyID = assignmentPaperBeneficiary.OrgUnitId,
                                EntityName = assignmentPaperBeneficiary.OrgUnit.LocalName,
                                PersonID = assignmentPaperBeneficiary.UserId.HasValue ? assignmentPaperBeneficiary.UserId.Value : 0,
                                PersonName = assignmentPaperBeneficiary.UserId.HasValue ? assignmentPaperBeneficiary.User.LocalName : string.Empty
                            };
                            userMobileAssignPaper.Add(assignPaper);
                        }
                    }

                    getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, userMobileAssignPaper, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        public List<int> GetAssignmentPapers(int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransAssignPaper>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {


                    // YESSERDomain.AssignmentPaper transAssignPaper = userMobileBL.UserMobileGetAssignmentPaperByTransactionId(Id);
                    IActionBL processBL = IoC.Resolve<IActionBL>();
                    IList<MCS.Domain.Action> actions = processBL.GetAllAction(Language).ToList();
                    List<ActionDTO> actionsDTO = ActionMapper.Map(actions);


                    return actionsDTO.Select(x => x.Id).ToList();
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, null, null);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, null, null);

                return null;
            }
        }
        #endregion
        [HttpPut]
        public HttpResponseMessage SetCopyAsViewed(int transId, int? toUserId, int toOrgUnit)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransAssignPaper>> getResult = null;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {

                    ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                    transactionAssignmentBL.SetCopyAsViewed(transId, toUserId, toOrgUnit);

                    transactionContextScope.Commit();
                    postResult = PostResult.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, null, null);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransAssignPaper>>.Create(statusCode, null, null);

                return null;
            }
        }

        [HttpPost]
        public HttpResponseMessage AddAssignmentCopies(int transactionId, int userId, int EntityId, List<TransPartiy> TransPartiys)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted)) 
                    {
                    int CopyStatus = TransCopyStatus.NotViewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);

                    List<YESSERDomain.TransactionCopy> transactionCopies = new List<YESSERDomain.TransactionCopy>();

                    if (TransPartiys?.Count > 0)
                    {
                        foreach (var copy in TransPartiys)
                        {
                            transactionCopies.Add(new YESSERDomain.TransactionCopy()
                            {
                                ActionId = copy.ProcessId,
                                FromEntityId = EntityId,
                                FromUserId = userId,
                                IsBcc = false,
                                IsOpr = false,
                                IsSent = 1,
                                Date = DateTime.Now,
                                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                                EntityId = copy.PartyID.Value,
                                UserId = copy.PersonID == -1 ? null : copy.PersonID,
                                SentDate = DateTime.Now,
                                Status = CopyStatus
                            });
                        }
                    }

                    IUserMobileBL uerMobileBL = IoC.Resolve<IUserMobileBL>();
                    ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                    if (transactionCopies != null && transactionCopies.Count > 0)
                    {
                        uerMobileBL.AddAssignmentCopies(transactionId, transactionCopies);

                        transactionAssignmentBL.SetCopyAsViewed(transactionId, userId, EntityId);
                    }


                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);

                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);
                return null;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);
                return null;
            }
        }



        [HttpPost]
        public HttpResponseMessage AddTransactionDocument(DocumentDTO documentDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    if (ModelState.IsValid)
                    {
                        IDocumentBL documentVIPBL = new DocumentBL();

                        byte[] content = documentDTO.Content;
                        // transactionDocument.Document.Content = null;

                        DocData docData = new DocData()
                        {
                            Data = content,
                            DocName = documentDTO.Name,
                            DocID = documentDTO.Id.ToString(),
                            // PersonId = transactionDocument.Document.CreatedBy,
                            MimeContent = documentDTO.MimeType,
                            EntityId = documentDTO.FromEntityId,
                            DataSize = Convert.ToInt32(documentDTO.Size)
                            // TransactionId = transactionId
                            // User_ID = transactionDocument.Document.CreatedBy.ToString(),
                        };
                        DocRepository.DocRepository.Save(docData, new DocumentLocation());

                        putResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetSettingValue(string Key)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SettingDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    ISettingBL settingBL = new SettingBL();
                    SettingDTO settingDTO = null;
                    List<MCS.Domain.Setting> settings = settingBL.GetSettingByKey(Key);
                    MCS.Domain.Setting setting = settings.Find(a => a.Key == Key);
                    if (setting != null)
                    {
                        settingDTO = SettingMapper.Map(setting);
                    }

                    getResult = GetResult<SettingDTO>.Create(statusCode, settingDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SettingDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SettingDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public List<PermissionDTO> GetPermissionsByGroupId(PermissionGroupName permissionGroupName, string cultureName, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    IList<MCS.Domain.Permission> permissions =
                        permissionBL.GetUserPermissionsByGroupIdMobile(permissionGroupName, cultureName, userId);

                    List<PermissionDTO> permissionsDTOs = PermissionMapper.Map(permissions);

                    getResult = GetResult<List<PermissionDTO>>.Create(statusCode, permissionsDTOs, rowsCount);

                    return permissionsDTOs;
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return null;
            }
        }
        public List<TransactionTypeDTO> GetTransactionType(TransactionCategory transactionCategory, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();
                    TransactionCategories sourceTransactionType = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<Domain.TransactionType> transactionTypes = transactionTypeBL.GetTransactionTypesByUserId(sourceTransactionType, cultureName);

                    List<TransactionTypeDTO> transactionTypesDTOs = TransactionTypeMapper.Map(transactionTypes, cultureName);

                    getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, transactionTypesDTOs, null);

                    return transactionTypesDTOs;
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return null;
            }
        }
        public Domain.Setting GetSetting(string Key)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SettingDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    ISettingBL settingBL = new SettingBL();
                    SettingDTO settingDTO = null;
                    List<Domain.Setting> settings = settingBL.GetSettingByKey(Key);
                    Domain.Setting setting = settings.Find(a => a.Key == Key);
                    if (setting != null)
                    {
                        settingDTO = SettingMapper.Map(setting);
                    }

                    getResult = GetResult<SettingDTO>.Create(statusCode, settingDTO, null);

                    return setting;
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SettingDTO>.Create(statusCode, null, null);

                return null;
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SettingDTO>.Create(statusCode, null, null);

                return null;
            }
        }

        public TransactionBarcodesDTO GetTransactionBarcodes(int transactionId, int orgUnitId, string cultureName)
        {


            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Domain.Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                    if (transaction == null)
                    {
                        return null;
                    }
                    else
                    {
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));
                        TransactionBarcodesInfo transactionBarcodes = transactionBL.GetTransactionBarcodes(transactionId, orgUnitId, cultureName);
                        TransactionBarcodesDTO transactionBarcodesDTO = TransactionBarcodesMapper.Map(transactionBarcodes);

                        LogAction(AuditingActionCode.ViewBarcodes, transaction.Id);
                        return transactionBarcodesDTO;
                    }
                }
            }
            catch (BusinessException ex)
            {

                return null;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
     
        public static byte[] AddDateAndDectionNumber(byte[] data, string DateAndDectionNumber)
        {
            //create pdfreader object to read sorce pdf
            PdfReader pdfReader = new PdfReader(data);
            //create stream of filestream or memorystream etc. to create output file
            using (MemoryStream msOutput = new MemoryStream())
            {
                //create pdfstamper object which is used to add addtional content to source pdf file
                PdfStamper pdfStamper = new PdfStamper(pdfReader, msOutput);

                BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\ARIAL.TTF", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                Font f = new Font(bf, 18);
                PdfLayer layer = new PdfLayer("WatermarkLayer", pdfStamper.Writer);

                //iterate through all pages in source pdf

                //Rectangle class in iText represent geomatric representation... in this case, rectanle object would contain page geomatry
                Rectangle rect = pdfReader.GetPageSizeWithRotation(1);
                //pdfcontentbyte object contains graphics and text content of page returned by pdfstamper
                PdfContentByte cb = pdfStamper.GetOverContent(1);
                cb.SetFontAndSize(BaseFont.CreateFont(
                                BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED), 6);
                cb.BeginText();
                string text = "Some random blablablabla...";
                // put the alignment and coordinates here
                ColumnText.ShowTextAligned(cb, Element.ALIGN_CENTER, new Phrase(DateAndDectionNumber, f), Convert.ToInt64(0.5 * rect.Width), Convert.ToInt64(0.75 * rect.Height), 0, PdfWriter.RUN_DIRECTION_RTL, 1);

                // cb.ShowTextAligned(1, text, 520, 640, 0);
                cb.EndText();



                pdfStamper.Close();

                return msOutput.ToArray();

            }
        }
        public string ReplaceString(string Replace)
        {
            if (Replace == null || Replace == "null")
            {
                return "";
            }
            Replace = Replace.Replace('1', '١');
            Replace = Replace.Replace('0', '٠');
            Replace = Replace.Replace('2', '٢');
            Replace = Replace.Replace('3', '٣');
            Replace = Replace.Replace('4', '٤');
            Replace = Replace.Replace('5', '٥');
            Replace = Replace.Replace('6', '٦');
            Replace = Replace.Replace('7', '٧');
            Replace = Replace.Replace('8', '٨');
            Replace = Replace.Replace('9', '٩');
            return Replace;
        }
    }
}