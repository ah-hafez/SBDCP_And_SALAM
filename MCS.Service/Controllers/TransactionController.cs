using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using MCS.DTO.ExternalParties;
using MCS.DTO.Transaction;
using MCS.Service.Mappers;
using System.Text;
using System.Web;
using YESSER.NCS.MCS.Service.Helpers;
using HashMechanism;
using static MCS.Service.Controllers.TransactionController.CertificationClient;
using RestSharp;
using System.Configuration;
using MCS.DataAccess;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class TransactionController : ApiBaseController
    {

        [HttpGet]
        public HttpResponseMessage GetDocumentAttributes()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<DocumentAttribute>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IDocumentAttributeBL documentAttributeBL = new DocumentAttributeBL();

                    List<DocumentAttribute> documentAttributes = documentAttributeBL.GetDocumentAttributes();

                    getResult = GetResult<List<DocumentAttribute>>.Create(statusCode, documentAttributes, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<DocumentAttribute>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<DocumentAttribute>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #region Attributes

        #endregion Attributes

        #region Notifications

        [HttpGet]
        public HttpResponseMessage GetNotifications([FromUri] SearchCriteria searchCriteria, bool isRead)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<NotificationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    INotificationBL notificationBL = new NotificationBL();
                    int rowsCount;
                    IList<Notification> notifications = notificationBL.GetNotifications(searchCriteria, isRead, Language, out rowsCount);

                    List<NotificationDTO> notificationDTO = NotificationMapper.Map(notifications, Language);

                    getResult = GetResult<List<NotificationDTO>>.Create(statusCode, notificationDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<NotificationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<NotificationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpDelete]
        public HttpResponseMessage DeleteNotifications(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> formIds = ids.Split(',').Select(int.Parse).ToList();

                    INotificationBL notificationBL = new NotificationBL();

                    notificationBL.DeleteNotifications(formIds);

                    deleteResult = DeleteResult.Create(statusCode);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage MarkAsReadNotification(string ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IList<int> formIds = ids.Split(',').Select(int.Parse).ToList();
                    INotificationBL notificationBL = new NotificationBL();
                    notificationBL.MarkAsReadNotification(formIds);
                    transactionContextScope.Commit();
                    postResult = PostResult.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        #endregion  Notifications
        #region Transactions
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
        [HttpPost]
        public HttpResponseMessage AssignItBackWithTray(int TransId, string Notes, int userId, int entityId, int trayId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IUserMobileBL userMobileBL = IoC.Resolve<IUserMobileBL>();

                    userMobileBL.AssignItBack(TransId, userId, entityId, trayId, Notes, Language);

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
        public HttpResponseMessage PostTransaction(TransactionDTO transactionDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<TransactionDetailsDTO> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(transactionDTO.TransactionCategory);
                        byte[] mainDocumentContent = null;
                        byte[] mainPDFDocumentContent = null;
                        User LoggedInUser = (User)UserContext.LoggedInUser;
                        IOrgUnitBL orgunitBL = new OrgUnitBL();

                        Transaction transaction = TransactionMapper.Map(transactionDTO);


                        bool SendSpecialCopy = orgunitBL.CheckIfOrgunitSendSpecialCopy(transaction.OrgUnitId);

                        if (transactionDTO.TransactionCategory == TransactionCategory.ExternalOutbound)
                        {
                            foreach (TransactionCopy transactionCopy in transaction.Copies)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 1;
                            }
                        }
                        else
                        {
                            foreach (TransactionCopy transactionCopy in transaction.Copies)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 0;
                            }
                        }


                        if (transaction.MainDocument.Document != null && transaction.MainDocument.Document.Content != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            transaction.MainDocument.Document.Content = null;
                        }
                        if (transaction?.OldWordDocumnt?.Document?.Content != null)
                        {
                            mainPDFDocumentContent = transaction.OldWordDocumnt.Document.Content;
                            transaction.OldWordDocumnt.Document.Content = null;
                        }



                        //DocData mainDocumentData = null;
                        //if (transactionDTO.TransactionCategory == TransactionCategory.ExternalOutbound)
                        //{
                        //    mainDocumentData = DocRepository.DocRepository.Load(transactionDTO.DocumentDTO.Id.ToString(), new DocumentLocation());
                        //}

                        TransactionDetails transactionDetails = transactionBL.Save(transaction, transactionDTO.DocumentDTO.Content);
                        if (transaction.ExternalCopies != null && transaction.ExternalCopies.Any())
                        {
                            foreach (TransactionExternalCopy transactionExternalCopy in transaction.ExternalCopies)
                            {
                                if (transactionExternalCopy.ExternalPartyAttachment != null && transactionExternalCopy.ExternalPartyAttachment.Any())
                                {
                                    byte[] AttachmentDocumentContent = null;

                                    foreach (ExternalPartyAttachment attachment in transactionExternalCopy.ExternalPartyAttachment)
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

                        }


                        if (transaction.IsDraft || transaction.IsPresentationDraft || transaction.IsDecisionDraft)
                        {
                            User user = (User)UserContext.LoggedInUser;
                            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO();
                            transactionElcOutBoundDTO.TransactionId = transaction.Id;
                            transactionElcOutBoundDTO.EntityId = transaction.OrgUnitId;
                            transactionElcOutBoundDTO.UserId = user.Id;
                            transactionElcOutBoundDTO.Ishidden = false;
                            transactionElcOutBoundDTO.CreatedOn = DateTime.Now;
                            transactionElcOutBoundDTO.CreatedBy = user.Id;

                            TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));

                        }


                        if (transaction.Attachments != null && transaction.Attachments.Any())
                        {
                            byte[] AttachmentDocumentContent = null;
                            foreach (Attachment attachment in transaction.Attachments)
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

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transaction.MainDocument.Name,
                                DocID = transaction.MainDocument.Id.ToString(),
                                PersonId = transaction.MainDocument.CreatedBy,
                                MimeContent = transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        if (transaction.OldWordDocumnt != null && transaction.OldWordDocumnt.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainPDFDocumentContent,
                                DocName = transaction.OldWordDocumnt.Name,
                                DocID = transaction.OldWordDocumnt.Id.ToString(),
                                PersonId = transaction.OldWordDocumnt.CreatedBy,
                                MimeContent = transaction.OldWordDocumnt.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.OldWordDocumnt.Size),
                                User_ID = transaction.OldWordDocumnt.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }




                        //if (transaction.Links != null)
                        //{
                        //    IMyTransactionsTrayBL myTransactionTrayBL = new MyTransactionsTrayBL();
                        //    foreach (TransactionLink link in transaction.Links)
                        //    {
                        //        myTransactionTrayBL.Save(link.ToTransactionId, -1, "", cultureName, true);
                        //    }
                        //}
                        //transactionBL.Update(transaction);
                        TransactionDetailsDTO transactionDetailsDTO = TransactionDetailsMapper.Map(transactionDetails);
                        postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO);
                        LogAction(AuditingActionCode.CreateTransaction, transactionDetails.Id);
                        transactionContextScope.Commit();

                        if (transactionDTO.TransactionCategory == TransactionCategory.Inbound)
                        {
                            foreach (var name in transactionDTO.Names)
                            {
                                if (name.SendSMS)
                                {
                                    SendSMS(name.MobileNumber, transaction.Number);
                                }
                            }
                        }

                        return Request.CreateResponse(HttpStatusCode.Created, postObjectResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage PostMultiTransaction(TransactionDTO transactionDTO, string cultureName, int MainNumber)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<TransactionDetailsDTO> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(transactionDTO.TransactionCategory);
                        byte[] mainDocumentContent = null;
                        byte[] mainPDFDocumentContent = null;
                        User LoggedInUser = (User)UserContext.LoggedInUser;
                        IOrgUnitBL orgunitBL = new OrgUnitBL();
                        bool SendSpecialCopy = false;


                        Transaction transaction = TransactionMapper.Map(transactionDTO);
                        transaction.Number = MainNumber;
                        if (orgunitBL.CheckIfOrgunitSendSpecialCopy(transaction.OrgUnitId))
                            SendSpecialCopy = true;

                        if (transactionDTO.TransactionCategory == TransactionCategory.ExternalOutbound)
                        {
                            foreach (TransactionCopy transactionCopy in transaction.Copies)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 1;
                            }
                        }
                        else
                        {
                            foreach (TransactionCopy transactionCopy in transaction.Copies)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 0;
                            }
                        }


                        if (transaction.MainDocument.Document != null && transaction.MainDocument.Document.Content != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            transaction.MainDocument.Document.Content = null;
                        }
                        if (transaction?.OldWordDocumnt?.Document?.Content != null)
                        {
                            mainPDFDocumentContent = transaction.OldWordDocumnt.Document.Content;
                            transaction.OldWordDocumnt.Document.Content = null;
                        }



                        //DocData mainDocumentData = null;
                        //if (transactionDTO.TransactionCategory == TransactionCategory.ExternalOutbound)
                        //{
                        //    mainDocumentData = DocRepository.DocRepository.Load(transactionDTO.DocumentDTO.Id.ToString(), new DocumentLocation());
                        //}

                        TransactionDetails transactionDetails = transactionBL.Save(transaction, transactionDTO.DocumentDTO.Content);
                        if (transaction.ExternalCopies != null && transaction.ExternalCopies.Any())
                        {
                            foreach (TransactionExternalCopy transactionExternalCopy in transaction.ExternalCopies)
                            {
                                if (transactionExternalCopy.ExternalPartyAttachment != null && transactionExternalCopy.ExternalPartyAttachment.Any())
                                {
                                    byte[] AttachmentDocumentContent = null;

                                    foreach (ExternalPartyAttachment attachment in transactionExternalCopy.ExternalPartyAttachment)
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

                        }


                        if (transaction.IsDraft || transaction.IsPresentationDraft || transaction.IsDecisionDraft)
                        {
                            User user = (User)UserContext.LoggedInUser;
                            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO();
                            transactionElcOutBoundDTO.TransactionId = transaction.Id;
                            transactionElcOutBoundDTO.EntityId = transaction.OrgUnitId;
                            transactionElcOutBoundDTO.UserId = user.Id;
                            transactionElcOutBoundDTO.Ishidden = false;
                            transactionElcOutBoundDTO.CreatedOn = DateTime.Now;
                            transactionElcOutBoundDTO.CreatedBy = user.Id;

                            TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));

                        }


                        if (transaction.Attachments != null && transaction.Attachments.Any())
                        {
                            byte[] AttachmentDocumentContent = null;
                            foreach (Attachment attachment in transaction.Attachments)
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

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transaction.MainDocument.Name,
                                DocID = transaction.MainDocument.Id.ToString(),
                                PersonId = transaction.MainDocument.CreatedBy,
                                MimeContent = transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        if (transaction.OldWordDocumnt != null && transaction.OldWordDocumnt.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainPDFDocumentContent,
                                DocName = transaction.OldWordDocumnt.Name,
                                DocID = transaction.OldWordDocumnt.Id.ToString(),
                                PersonId = transaction.OldWordDocumnt.CreatedBy,
                                MimeContent = transaction.OldWordDocumnt.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.OldWordDocumnt.Size),
                                User_ID = transaction.OldWordDocumnt.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }




                        //if (transaction.Links != null)
                        //{
                        //    IMyTransactionsTrayBL myTransactionTrayBL = new MyTransactionsTrayBL();
                        //    foreach (TransactionLink link in transaction.Links)
                        //    {
                        //        myTransactionTrayBL.Save(link.ToTransactionId, -1, "", cultureName, true);
                        //    }
                        //}
                        //transactionBL.Update(transaction);
                        TransactionDetailsDTO transactionDetailsDTO = TransactionDetailsMapper.Map(transactionDetails);
                        postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO);
                        LogAction(AuditingActionCode.CreateTransaction, transactionDetails.Id);
                        transactionContextScope.Commit();

                        if (transactionDTO.TransactionCategory == TransactionCategory.Inbound)
                        {
                            foreach (var name in transactionDTO.Names)
                            {
                                if (name.SendSMS)
                                {
                                    SendSMS(name.MobileNumber, transaction.Number);
                                }
                            }
                        }

                        return Request.CreateResponse(HttpStatusCode.Created, postObjectResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
        }


        private void SendSMS(string mobileNumber, long transactionNumber)
        {
            try
            {
                string responseMessage = string.Empty;
                HttpClient client = new HttpClient();

                string smsUrl = string.Format("https://api1.yamamah.com/SendSMSV2?Username=cc4-MCS&Password=NewMCS&Tagname=CCHI&RecepientNumber={0}&Message={1}&SendDateTime=0&EnableDR=true&SentMessageID=True", mobileNumber, $"تم انشاء المعاملة رقم {transactionNumber}");
                HttpResponseMessage response = client.GetAsync(smsUrl).Result;

                responseMessage = response.Content.ReadAsStringAsync().Result;

                if (!response.IsSuccessStatusCode || responseMessage != "1") // 1 is success
                {
                    if (string.IsNullOrWhiteSpace(responseMessage))
                    {
                        responseMessage = "Can't read http client sms resopnse!";
                    }
                    else
                    {
                        responseMessage = "Status Code From API : " + responseMessage;
                    }

                    Exception smsException = new Exception(responseMessage);
                    ExceptionHelper.HandleException(smsException);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
        }
        [HttpPut]
        public HttpResponseMessage ConvertTransactionToDraft(int TransactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        User user = (User)UserContext.LoggedInUser;

                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.ExternalOutbound);

                        ILookupBL lookupBL = new LookupBL();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();
                        IDocumentBL documentBL = new DocumentBL();
                        Transaction draftTransaction = transactionBL.GetTransaction(t => t.Id == TransactionId);

                        int transCategoryLookupId;

                        if (!draftTransaction.IsPresentationDraft)
                            draftTransaction.Number = draftTransaction.OutBoundDraftNumber.HasValue ? draftTransaction.OutBoundDraftNumber.Value : draftTransaction.Number;

                        draftTransaction.IsDraft = true;
                        draftTransaction.IsSigned = false;
                        draftTransaction.IsPresentationDraft = false;
                        draftTransaction.IsElcOutBound = false;
                        draftTransaction.NeedAcknowled = false;
                        transCategoryLookupId = Common.TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, Language);
                        draftTransaction.TransactionCategoryId = transCategoryLookupId;
                        draftTransaction.StatusId = Common.TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, Language);
                        draftTransaction.TransactionCategory = lookupBL.GetLookupItem(transCategoryLookupId);

                        //int oldMainDocumentId = draftTransaction.OldWordDocumntId.Value;
                        //int mainDocumentId = draftTransaction.MainDocumentId.Value;
                        //if (draftTransaction.OldWordDocumntId.HasValue)
                        //{

                        //    documentBL.UpdateDocumentContentByTransaction(draftTransaction.Id, null);
                        //    draftTransaction.MainDocumentId = oldMainDocumentId;
                        //    draftTransaction.OldWordDocumntId = mainDocumentId;
                        //}

                        transactionBL.UpdateCanceledOutBound(draftTransaction);

                        TransactionBL.TransactionElcOutBoundUpdate(user.Id, LoggedInOrgUnitId, true, TransactionId);

                        putResult = PutResult.Create(statusCode, draftTransaction.Id);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        [HttpPut]
        public HttpResponseMessage ConvertDraftToOutbound(int draftTransactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            int DefualtAction = Convert.ToInt32(ConfigurationManager.AppSettings["DefualtSignAction"] ?? "1");
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        User user = (User)UserContext.LoggedInUser;

                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.ExternalOutbound);
                        IEditorBL editorBL = new EditorBL();
                        ILookupBL lookupBL = new LookupBL();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();

                        Transaction draftTransaction = transactionBL.GetTransaction(t => t.Id == draftTransactionId);

                        // handling internal outbound draft
                        int transCategoryLookupId;
                        bool isInternalOutboundDraft = !(draftTransaction?.ExternalPartyId != null && draftTransaction?.ExternalPartyId.Value > 0);
                        int? ioDepartment = orgunitBL.getIoDepartment(draftTransaction.EntityId ?? 0);
                        int? GeneralIoDepartment = orgunitBL.getGeneralIoDepartment();
                        draftTransaction.IsDraft = false;
                        draftTransaction.IsElcOutBound = true;
                        if (!draftTransaction.IsPresentationDraft)
                            draftTransaction.OutBoundDraftNumber = draftTransaction.Number;

                        if (!isInternalOutboundDraft)
                        {
                            transCategoryLookupId = Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, Language);
                            draftTransaction.TransactionCategoryId = transCategoryLookupId;
                            draftTransaction.StatusId = Common.TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, Language);

                            draftTransaction.PrintedDeliveryReport = true;
                            if (!draftTransaction.IsPresentationDraft)
                                (transactionBL as OutboundExternalBL).GetNewExternalOutboundNumber(draftTransaction);
                        }
                        else
                        {
                            transCategoryLookupId = Common.TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, Language);
                            draftTransaction.TransactionCategoryId = transCategoryLookupId;
                            draftTransaction.StatusId = Common.TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, Language);

                            transactionBL = TransactionBL.Create(TransactionCategory.InternalOutbound);

                            if (!draftTransaction.IsPresentationDraft)
                                (transactionBL as OutboundInternalBL).GetNewInternalOutboundNumber(draftTransaction);
                        }

                        draftTransaction.TransactionCategory = lookupBL.GetLookupItem(transCategoryLookupId);


                        draftTransaction.IsPresentationDraft = false;

                        //check if 
                        if (isInternalOutboundDraft)
                        {
                            if (orgunitBL.ReceiveElcOutBoundWithAcknowled(Convert.ToInt32(draftTransaction.EntityId)))
                                draftTransaction.NeedAcknowled = true;

                        }
                        foreach (TransactionCopy transactionCopy in draftTransaction.Copies)
                        {

                            transactionCopy.IsSent = draftTransaction.NeedAcknowled ? 0 : 1;

                        }


                        transactionBL.Update(draftTransaction);

                        if (!isInternalOutboundDraft && ioDepartment.HasValue)
                        {
                            TransactionAssignment transAssign = new TransactionAssignment
                            {
                                FromUserId = user.Id,
                                FromEntityId = LoggedInOrgUnitId,
                                ToEntityId = ioDepartment.Value,
                                ActionId = DefualtAction,
                                DeliveryMethodId = Common.DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, Language),
                                TrayId = (int)TrayType.MyTransactions
                            };

                            IList<TransactionAssignment> transactionAssignments = new List<TransactionAssignment>();
                            transactionAssignments.Add(transAssign);

                            editorBL.AssignTransaction(draftTransaction.Id, transactionAssignments, Language);
                        }
                        else if (isInternalOutboundDraft)
                        {
                            TransactionAssignment transAssign = new TransactionAssignment
                            {
                                FromUserId = user.Id,
                                FromEntityId = LoggedInOrgUnitId,
                                //ToUserId = draftTransaction.ExternalPartyManager.Id,
                                ToEntityId = draftTransaction.Entity.Id,
                                ActionId = DefualtAction,
                                DeliveryMethodId = Common.DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, Language),
                                TrayId = (int)TrayType.OrgUnit
                            };

                            IList<TransactionAssignment> transactionAssignments = new List<TransactionAssignment>();
                            transactionAssignments.Add(transAssign);

                            editorBL.AssignTransaction(draftTransaction.Id, transactionAssignments, Language);
                        }

                        if (ioDepartment.HasValue)
                        {

                            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO();
                            transactionElcOutBoundDTO.TransactionId = draftTransactionId;
                            transactionElcOutBoundDTO.EntityId = ioDepartment.Value;
                            transactionElcOutBoundDTO.Ishidden = false;
                            transactionElcOutBoundDTO.CreatedOn = DateTime.Now;
                            transactionElcOutBoundDTO.CreatedBy = user.Id;

                            TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));

                        }
                        else if (GeneralIoDepartment.HasValue)
                        {

                            TransactionElcOutBoundDTO transactionElcOutBoundDTO = new TransactionElcOutBoundDTO();
                            transactionElcOutBoundDTO.TransactionId = draftTransactionId;
                            transactionElcOutBoundDTO.EntityId = GeneralIoDepartment.Value;
                            transactionElcOutBoundDTO.Ishidden = false;
                            transactionElcOutBoundDTO.CreatedOn = DateTime.Now;
                            transactionElcOutBoundDTO.CreatedBy = user.Id;

                            TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));


                        }



                        TransactionBL.TransactionElcOutBoundUpdate(user.Id, LoggedInOrgUnitId, false, draftTransactionId);
                        putResult = PutResult.Create(statusCode, draftTransaction.Id);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        [HttpPut]
        public HttpResponseMessage UpdatePresentationDraftNumber(int draftTransactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        User user = (User)UserContext.LoggedInUser;

                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.DraftOutbound);

                        ILookupBL lookupBL = new LookupBL();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();

                        Transaction draftTransaction = transactionBL.GetTransaction(t => t.Id == draftTransactionId);
                        draftTransaction.PresentationDraftNumber = draftTransaction.Number;
                        transactionBL = TransactionBL.Create(TransactionCategory.DraftOutbound);
                        (transactionBL as OutboundDraftBL).UpdatePresentationDraftNumber(draftTransaction);
                        transactionBL.Update(draftTransaction);

                        putResult = PutResult.Create(statusCode, draftTransaction.Id);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        public HttpResponseMessage PutTransaction(string cultureName, TransactionDTO transactionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(transactionDTO.TransactionCategory);
                        IBarcodeBL barcodeBL = IoC.Resolve<IBarcodeBL>();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();
                        bool SendSpecialCopy = false;



                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();


                        Transaction transaction = TransactionMapper.Map(transactionDTO);
                        if (orgunitBL.CheckIfOrgunitSendSpecialCopy(transaction.OrgUnitId))
                            SendSpecialCopy = true;


                        byte[] mainDocumentContent = null;
                        byte[] oldMainDocumentContent = null;

                        string mimeType = "";
                        string oldMimeType = "";

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            mimeType = transaction.MainDocument.MimeType;
                            transaction.MainDocument.Document.Content = null;
                        }
                        if (transaction.OldWordDocumnt != null && transaction.OldWordDocumnt.Document != null)
                        {
                            oldMainDocumentContent = transaction.OldWordDocumnt.Document.Content;
                            oldMimeType = transaction.OldWordDocumnt.MimeType;
                            transaction.OldWordDocumnt.Document.Content = null;
                        }

                        //Approved Word 
                        IList<TransactionAssignmentHistory> transactionHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistoryByTransactionId(transaction.Id);


                        foreach (TransactionCopy transactionCopy in transaction.Copies)
                        {
                            if (transactionHistories.Count == 0)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 0;
                                transactionCopy.SentDate = null;
                            }
                            else if (transaction.TransactionCategoryId != Common.TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 1;
                                transactionCopy.SentDate = DateTime.Now;
                            }
                        }

                        transactionBL.Update(transaction);

                        if (transaction.Attachments != null && transaction.Attachments.Any())
                        {
                            byte[] AttachmentDocumentContent = null;
                            foreach (Attachment attachment in transaction.Attachments)
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

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocumentInfo transMainDocument = TransactionBL.GetMainDocumentByTransactionId(transaction.Id);
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transMainDocument.Name,//transaction.MainDocument.Name,
                                DocID = transMainDocument.Id.ToString(),//transaction.MainDocument.Id.ToString(),
                                PersonId = transMainDocument.CreatedBy,//transaction.MainDocument.CreatedBy,

                                MimeContent = !string.IsNullOrWhiteSpace(mimeType) ? mimeType : transMainDocument.MimeType,//transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transMainDocument.CreatedBy.ToString(),//transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };

                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        if (transaction.OldWordDocumnt != null && transaction.OldWordDocumnt.Document != null)
                        {
                            DocumentInfo transMainDocument = TransactionBL.GetOldMainDocumentByTransactionId(transaction.Id);
                            DocData docData = new DocData()
                            {
                                Data = oldMainDocumentContent,
                                DocName = transMainDocument.Name,//transaction.MainDocument.Name,
                                DocID = transMainDocument.Id.ToString(),//transaction.MainDocument.Id.ToString(),
                                PersonId = transMainDocument.CreatedBy,//transaction.MainDocument.CreatedBy,

                                MimeContent = !string.IsNullOrWhiteSpace(oldMimeType) ? oldMimeType : transMainDocument.MimeType,//transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.OldWordDocumnt.Size),
                                User_ID = transMainDocument.CreatedBy.ToString(),//transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };

                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }




                        LogAction(AuditingActionCode.UpadteTransaction, transaction.Id);
                        putResult = PutResult.Create(statusCode);
                        transactionContextScope.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    putResult = PutResult.Create(statusCode);
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
        public HttpResponseMessage VIPPutTransaction(string cultureName, TransactionDTO transactionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(transactionDTO.TransactionCategory);
                        IBarcodeBL barcodeBL = IoC.Resolve<IBarcodeBL>();
                        Transaction transaction = TransactionBL.GetTransactionByIdAsNoTacking(transactionDTO.Id);
                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();
                        bool SendSpecialCopy = false;

                        if (orgunitBL.CheckIfOrgunitSendSpecialCopy(transactionDTO.OrgUnitId))
                            SendSpecialCopy = true;


                        if (transactionDTO.GetType().ToString().Contains("EditOutboundInternalDTO"))
                        {
                            //transaction.PriorityId = ((EditOutboundInternalDTO)transactionDTO).OutboundInternalBasicInfoEdit.PriorityLevelId;
                            transaction.ConfidentialityId = ((EditOutboundInternalDTO)transactionDTO).OutboundInternalBasicInfoEdit.ConfidentialityLevelId;

                            EditOutboundInternalBasicInfoDTO basicInfo = ((EditOutboundInternalDTO)transactionDTO).OutboundInternalBasicInfoEdit;
                            DateTime? RemindDate = basicInfo.RemindDate;
                            if (RemindDate.HasValue)
                            {
                                TimeSpan ts = new TimeSpan(basicInfo.Hour.HasValue ? basicInfo.Hour.Value : 00, basicInfo.Minute.HasValue ? basicInfo.Minute.Value : 00, 00);

                                // transaction.RemindDate.Value = 0;
                                DateTime newDate = basicInfo.RemindDate.Value;
                                newDate = new DateTime(basicInfo.RemindDate.Value.Year, basicInfo.RemindDate.Value.Month, basicInfo.RemindDate.Value.Day, 0, 0, 0);

                                transaction.RemindDate = newDate + ts;
                                transaction.RemindDateH = basicInfo.RemindDateH;
                            }
                            else
                            {
                                transaction.RemindDate = null;
                                transaction.RemindDateH = null;
                            }
                        }
                        else if (transactionDTO.GetType().ToString().Contains("EditInboundDTO"))
                        {
                            transaction.PriorityId = ((EditInboundDTO)transactionDTO).InboundBasicInfoEdit.PriorityLevelId;
                            transaction.ConfidentialityId = ((EditInboundDTO)transactionDTO).InboundBasicInfoEdit.ConfidentialityLevelId;

                            EditInboundBasicInfoDTO basicInfo = ((EditInboundDTO)transactionDTO).InboundBasicInfoEdit;
                            DateTime? RemindDate = basicInfo.RemindDate;

                            if (RemindDate.HasValue)
                            {
                                TimeSpan ts = new TimeSpan(basicInfo.Hour.HasValue ? basicInfo.Hour.Value : 00, basicInfo.Minute.HasValue ? basicInfo.Minute.Value : 00, 00);

                                // transaction.RemindDate.Value = 0;
                                DateTime newDate = basicInfo.RemindDate.Value;
                                newDate = new DateTime(basicInfo.RemindDate.Value.Year, basicInfo.RemindDate.Value.Month, basicInfo.RemindDate.Value.Day, 0, 0, 0);

                                transaction.RemindDate = newDate + ts;
                                transaction.RemindDateH = basicInfo.RemindDateH;
                            }
                            else
                            {
                                transaction.RemindDate = null;
                                transaction.RemindDateH = null;
                            }

                        }
                        else if (transactionDTO.GetType().ToString().Contains("EditOutboundDraftDTO"))
                        {
                            transaction.PriorityId = ((EditOutboundDraftDTO)transactionDTO).OutboundDraftBasicInfo.PriorityLevelId;
                            transaction.ConfidentialityId = ((EditOutboundDraftDTO)transactionDTO).OutboundDraftBasicInfo.ConfidentialityLevelId;
                        }

                        transaction.Attachments = TransactionAttachmentMapper.Map(transactionDTO.Attachments);
                        transaction.MainDocument = DocumentMapper.Map(transactionDTO.DocumentDTO);



                        //transaction.Copies = TransactionCopyMapper.Map(outboundTrx.Copies);
                        //transaction.ExternalCopies = TransactionExternalCopyMapper.Map(outboundTrx.ExternalCopies);


                        byte[] mainDocumentContent = null;
                        string mimeType = "";
                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            mimeType = transaction.MainDocument.MimeType;
                            transaction.MainDocument.Document.Content = null;
                        }


                        IList<TransactionAssignmentHistory> transactionHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistoryByTransactionId(transaction.Id);


                        foreach (TransactionCopy transactionCopy in transaction.Copies)
                        {
                            if (transactionHistories.Count == 0)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 0;
                                transactionCopy.SentDate = null;
                            }
                            else if (transaction.TransactionCategoryId != Common.TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 1;
                                transactionCopy.SentDate = DateTime.Now;
                            }
                        }

                        transactionBL.Update(transaction);

                        if (transaction.Attachments != null && transaction.Attachments.Any())
                        {
                            byte[] AttachmentDocumentContent = null;
                            foreach (Attachment attachment in transaction.Attachments)
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

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocumentInfo transMainDocument = TransactionBL.GetMainDocumentByTransactionId(transaction.Id);
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transMainDocument.Name,//transaction.MainDocument.Name,
                                DocID = transMainDocument.Id.ToString(),//transaction.MainDocument.Id.ToString(),
                                PersonId = transMainDocument.CreatedBy,//transaction.MainDocument.CreatedBy,

                                MimeContent = !string.IsNullOrWhiteSpace(mimeType) ? mimeType : transMainDocument.MimeType,//transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transMainDocument.CreatedBy.ToString(),//transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };

                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }
                        //if (transaction.Links != null)
                        //{
                        //    IMyTransactionsTrayBL myTransactionTrayBL = new MyTransactionsTrayBL();
                        //    foreach (TransactionLink link in transaction.Links)
                        //    {
                        //        myTransactionTrayBL.Save(link.ToTransactionId, -1, "", cultureName, true);
                        //    }
                        //}

                        LogAction(AuditingActionCode.UpadteTransaction, transaction.Id);
                        putResult = PutResult.Create(statusCode);
                        transactionContextScope.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    putResult = PutResult.Create(statusCode);
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
        [HttpGet]
        public HttpResponseMessage PrintTransactionTicket(TransactionTicketInfoDTO transactionTicketInfoDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionTicketDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Transaction transaction = TransactionBL.GetTransactionById(transactionTicketInfoDTO.TransactionId);
                    ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));
                    TransactionTicket transactionTicket = transactionBL.PrintTransactionTicket(transaction);
                    TransactionTicketDTO transactionTicketDTO = TransactionTicketMapper.Map(transactionTicket);
                    getResult = GetResult<TransactionTicketDTO>.Create(statusCode, transactionTicketDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionTicketDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionTicketDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetTransactionBarcodes(int transactionId, int orgUnitId, 
            string cultureName, bool ignoreLogging = false)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionBarcodesDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                    if (transaction == null)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                    else
                    {
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));
                        TransactionBarcodesInfo transactionBarcodes = transactionBL
                            .GetTransactionBarcodes(transactionId, orgUnitId, cultureName);
                        TransactionBarcodesDTO transactionBarcodesDTO = TransactionBarcodesMapper.Map(transactionBarcodes);
                        getResult = GetResult<TransactionBarcodesDTO>.Create(statusCode, transactionBarcodesDTO, null);
                        if (!ignoreLogging)
                        {
                            LogAction(AuditingActionCode.ViewBarcodes, transaction.Id);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionBarcodesDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionBarcodesDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage PrintTransactionsDeliveryReport(string strTransactionReportInfos, string cultureName, int userId, bool perTransaction = true)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DeliveryReportDTO> getResult = null;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            List<TransactionReportInfoDTO> transactionReportInfoDTOs = javaScriptSerializer.Deserialize<List<TransactionReportInfoDTO>>(strTransactionReportInfos) as List<TransactionReportInfoDTO>;
            List<TransactionReportInfo> transactionReportInfos = DeliveryReportMapper.Map(transactionReportInfoDTOs);

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    DeliveryReportInfoDTO deliveryReportInfoDTO = TransactionBL.PrintTransactionsDeliveryReport(transactionReportInfos, cultureName, userId, perTransaction);

                    DeliveryReportDTO deliveryReportDTO = DeliveryReportMapper.Map(deliveryReportInfoDTO);

                    getResult = GetResult<DeliveryReportDTO>.Create(statusCode, deliveryReportDTO, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<DeliveryReportDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<DeliveryReportDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage PrintDeliveryReportById(string strTransactionReportInfos, string cultureName, bool perTransaction = true, bool IsNew = false)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<DeliveryReportDTO>> getResult = null;
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            List<TransactionReportInfoDTO> transactionReportInfoDTOs = javaScriptSerializer.Deserialize<List<TransactionReportInfoDTO>>(strTransactionReportInfos) as List<TransactionReportInfoDTO>;

            List<TransactionReportInfo> transactionReportInfos = DeliveryReportMapper.Map(transactionReportInfoDTOs);

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    List<DeliveryReportDTO> deliveryReportDTOs = new List<DeliveryReportDTO>();
                    if (transactionReportInfos != null)
                    {
                        foreach (TransactionReportInfo transactionReportInfo in transactionReportInfos)
                        {
                            Transaction transaction = TransactionBL.GetTransactionById(transactionReportInfo.TransactionId);

                            if (transaction != null)
                            {
                                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));
                                List<Attachment> attachments = transaction.Attachments.ToList();
                                List<string> attachmentTotal = new List<string>();

                                attachments.ForEach(t =>
                                {
                                    attachmentTotal.Add(t.Count + " " + t.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text);
                                });

                                List<DeliveryReportDTO> transactionDeliveryReportDTOs = DeliveryReportMapper.Map(transactionBL.DeliveryReport(transaction, cultureName, transactionReportInfo.ReportsIds, perTransaction, IsNew));
                                if (transactionDeliveryReportDTOs != null && transactionDeliveryReportDTOs.Count > 0)
                                {
                                    foreach (DeliveryReportDTO transactionDeliveryReportDTO in transactionDeliveryReportDTOs)
                                    {
                                        transactionDeliveryReportDTO.DeliveryReportTransactions.ForEach(t => { t.AttachmentTotal = string.Join("+", (object[])attachmentTotal.ToArray()); });
                                        deliveryReportDTOs.Add(transactionDeliveryReportDTO);
                                    }
                                }

                            }
                        }
                    }

                    if (transactionReportInfos != null && transactionReportInfos.Count > 0)
                    {
                        var rejectReportId = transactionReportInfos.FirstOrDefault().RejectReportId;
                        if (rejectReportId.HasValue)
                        {
                            IReporterBL reporterBL = IoC.Resolve<IReporterBL>();
                            if (rejectReportId > 0)
                            {
                                var reporter = reporterBL.GetReporterById(rejectReportId.Value, cultureName);
                                deliveryReportDTOs.FirstOrDefault().DeliveryReportTransactions.FirstOrDefault().Receiver = reporter.Text;
                            }
                            else
                            {
                                deliveryReportDTOs.FirstOrDefault().DeliveryReportTransactions.FirstOrDefault().Receiver = string.Empty;
                            }
                        }
                    }

                    getResult = GetResult<List<DeliveryReportDTO>>.Create(statusCode, deliveryReportDTOs, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<DeliveryReportDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<DeliveryReportDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public class TransactionsSignedDeliveryReport
        {
            public int Number { get; set; }
            public DateTime? Date { get; set; }
        }
        [HttpGet]
        public HttpResponseMessage SearchDeliveryReportByNumber(string DateH, string cultureName)
        {
            //JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            DateTime DateG = DateTime.ParseExact(DateH, "dd/MM/yyyy", null);
            //TransactionsSignedDeliveryReport objTransactionsSignedDeliveryReport = javaScriptSerializer.Deserialize(strDeliveryReportObj, typeof(TransactionsSignedDeliveryReport)) as TransactionsSignedDeliveryReport;

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDeliveryReportDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

                    IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByNumber(DateG, cultureName);

                    List<TransactionDeliveryReportDTO> TransactionDeliveryReportDTOs = TransactionDeliveryReportMapper.Map(transactionDeliveryReports, Language);

                    getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, TransactionDeliveryReportDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage SearchDeliveryReportByNumberAndYear(int? NumberTran, string year, string numberD, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDeliveryReportDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    DateTime? DateG = null;
                    if (!string.IsNullOrWhiteSpace(year))
                    {
                        DateG = DateTime.ParseExact(year, "dd/MM/yyyy", null);
                    }
                    //int yearh = DateTimeUtility.GetHijriYear(DateG);
                    ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
                    //Transaction transactionForId = TransactionBL.GetByTransactionNumberTransaction(NumberTran, yearh);
                    //if (transactionForId == null)
                    //{
                    //    getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(Common.StatusCode.TransactionNotFound, null, null);

                    //    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    //}
                    IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByNumber(DateG, NumberTran, numberD, cultureName);

                    if (transactionDeliveryReports.Count == 0)
                    {
                        getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(Common.StatusCode.TransactionNotFound, null, null);

                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }


                    List<TransactionDeliveryReportDTO> TransactionDeliveryReportDTOs = TransactionDeliveryReportMapper.Map(transactionDeliveryReports, cultureName);

                    getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, TransactionDeliveryReportDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSignedDeliveryReport(string date, int? orgunitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SignedDeliveryReportDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    ISignedDeliveryReportBL signedDeliveryReportBL = new SignedDeliveryReportBL();

                    IList<SignedDeliveryReport> signedDeliveryReport = signedDeliveryReportBL.GetSignedDeliveryReport(date, orgunitId);
                    signedDeliveryReport = signedDeliveryReport.OrderByDescending(o => o.DocumentId)
                        .ThenByDescending(o => o.CreatedOn)
                        .ToList()
                        .GroupBy(g => g.TransactionDeliveryReportId)
                        .Select(s => s.FirstOrDefault())
                        .ToList();

                    List<SignedDeliveryReportDTO> SignedDeliveryReportDTOs = SignedDeliveryReportMapper.Map(signedDeliveryReport);

                    getResult = GetResult<List<SignedDeliveryReportDTO>>.Create(statusCode, SignedDeliveryReportDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SignedDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SignedDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetDeliveryReportDocument(string documentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        DocData docData = DocRepository.DocRepository.Load(documentId, new DocumentLocation());

                        DocumentDTO documentDTO = new DocumentDTO()
                        {
                            Content = docData.Data
                        };

                        getResult = GetResult<DocumentDTO>.Create(statusCode, documentDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

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
        [HttpPost]
        public HttpResponseMessage UploadSignedDeliveryReport(DocumentDTO documentDTO, string DeliveryReportNumber, int userId, string cultureName, string DateH)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

                        DocumentInfo documentInfo = DocumentMapper.Map(documentDTO);

                        int documentId = 0;

                        byte[] content = documentInfo.Document.Content;

                        documentInfo.Document.Content = null;
                        DateTime date = DateTimeUtility.ConvertToDate(DateH);
                        documentId = transactionDeliveryReportBL.UpdateDeliveryReportsDocumentByDate(documentInfo, DateH, DeliveryReportNumber);

                        if (documentDTO != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = content,
                                DocID = documentId.ToString(),
                                MimeContent = documentDTO.MimeType,
                                DataSize = Convert.ToInt32(documentDTO.Size),
                                PersonId = userId,
                                User_ID = userId.ToString()
                            };

                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage UploadSignedDeliveryReportByReportNumber(DocumentDTO documentDTO, string DeliveryReportNumber, int userId, string cultureName, string DateH)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

                        DocumentInfo documentInfo = DocumentMapper.Map(documentDTO);

                        int documentId = 0;

                        byte[] content = documentInfo.Document.Content;

                        documentInfo.Document.Content = null;
                        DateTime date = DateTimeUtility.ConvertToDate(DateH);

                        IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByNumber(DeliveryReportNumber);
                        foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
                        {
                            documentId = transactionDeliveryReportBL.UpdateDeliveryReportsDocumentByDeliveryReportId(documentInfo, DateH, transactionDeliveryReport.Id);

                            if (documentDTO != null)
                            {
                                DocData docData = new DocData()
                                {
                                    Data = content,
                                    DocID = documentId.ToString(),
                                    MimeContent = documentDTO.MimeType,
                                    DataSize = Convert.ToInt32(documentDTO.Size),
                                    PersonId = userId,
                                    User_ID = userId.ToString()
                                };

                                DocRepository.DocRepository.Save(docData, new DocumentLocation());
                                documentInfo.Id = documentId;
                                Attachment attachment = new Attachment
                                {
                                    AttachmentSource = 0,
                                    Count = 1,
                                    Description = "TransactionDelviryReport",
                                    TransactionId = transactionDeliveryReport.TransactionId,
                                    DocumentInfo = documentInfo,
                                    TypeId = 84,
                                };
                                TransactionBL.AddDeliveryReportToAttachment(attachment);
                                DocData docAttachData = new DocData()
                                {
                                    Data = content,
                                    DocID = attachment.DocumentInfo.Id.ToString(),
                                    MimeContent = documentDTO.MimeType,
                                    DataSize = Convert.ToInt32(documentDTO.Size),
                                    PersonId = userId,
                                    User_ID = userId.ToString(),
                                };
                                DocRepository.DocRepository.Save(docAttachData, new DocumentLocation());

                            }
                        }
                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage PrintDeliveryReport(int transactionId, string cultureName, bool perTransaction = true)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<DeliveryReportDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    List<DeliveryReportDTO> deliveryReportDTOs = new List<DeliveryReportDTO>();

                    Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                    if (transaction != null)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));
                        IList<DeliveryReportInfoDTO> deliveryReportInfos = transactionBL.DeliveryReport(transaction, cultureName, perTransaction);
                        List<DeliveryReportDTO> transactionDeliveryReportDTOs = DeliveryReportMapper.Map(deliveryReportInfos);
                        foreach (DeliveryReportDTO transactionDeliveryReportDTO in transactionDeliveryReportDTOs)
                        {
                            deliveryReportDTOs.Add(transactionDeliveryReportDTO);
                        }
                    }

                    LogAction(AuditingActionCode.PrintDeliveryReport, transaction.Id);
                    getResult = GetResult<List<DeliveryReportDTO>>.Create(statusCode, deliveryReportDTOs, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<DeliveryReportDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<DeliveryReportDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionCertificateById(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResultExtraData<object> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    User user = (User)UserContext.LoggedInUser;
                    Transaction transaction = TransactionBL.GetTransactionById(transactionId, cultureName);
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);

                    if (transaction.TransactionCategory.Id == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                    {
                        InboundCertificateDTO inboundCertificateDTO = TransactionCertificateMapper.MapInbound(transactionBL.GetTransactionCertificate(transaction.Id, cultureName), cultureName);
                        getResult = GetResultExtraData<object>.Create(statusCode, inboundCertificateDTO, Common.TransactionCategory.Inbound, null);
                    }
                    else
                    {
                        OutboundCertificateDTO outboundCertificateDTO = TransactionCertificateMapper.MapOutbound(transactionBL.GetTransactionCertificate(transaction.Id, cultureName), cultureName);
                        getResult = GetResultExtraData<object>.Create(statusCode, outboundCertificateDTO, Common.TransactionCategory.InternalOutbound, null);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResultExtraData<object>.Create(statusCode, null, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResultExtraData<object>.Create(statusCode, null, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetTransactionCertificateByReference(string referenceCode, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResultExtraData<object> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    User user = (User)UserContext.LoggedInUser;
                    Transaction transaction = TransactionBL.GetTransactionByReference(referenceCode, user.Id, orgUnitId, cultureName);
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);

                    if (transaction.TransactionCategory.Id == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                    {
                        InboundCertificateDTO inboundCertificateDTO = TransactionCertificateMapper.MapInbound(transactionBL.GetTransactionCertificate(transaction.Id, cultureName), cultureName);
                        getResult = GetResultExtraData<object>.Create(statusCode, inboundCertificateDTO, TransactionCategory.Inbound, null);
                    }
                    else
                    {
                        OutboundCertificateDTO outboundCertificateDTO = TransactionCertificateMapper.MapOutbound(transactionBL.GetTransactionCertificate(transaction.Id, cultureName), cultureName);
                        getResult = GetResultExtraData<object>.Create(statusCode, outboundCertificateDTO, TransactionCategory.InternalOutbound, null);
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResultExtraData<object>.Create(statusCode, null, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResultExtraData<object>.Create(statusCode, null, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
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
                    List<Setting> settings = settingBL.GetSettingByKey(Key);
                    Setting setting = settings.Find(a => a.Key == Key);
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


        [HttpGet]
        public HttpResponseMessage GetInboundCertificate(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<InboundCertificateDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    InboundCertificateDTO inboundCertificateDTO = TransactionCertificateMapper.MapInbound(transactionBL.GetTransactionCertificate(transactionId, cultureName), cultureName);
                    getResult = GetResult<InboundCertificateDTO>.Create(statusCode, inboundCertificateDTO, null);
                    LogAction(AuditingActionCode.ViewCertificate, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<InboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<InboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOutboundCertificate(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OutboundCertificateDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.ExternalOutbound);
                    OutboundCertificateDTO outboundCertificateDTO = TransactionCertificateMapper.MapOutbound(transactionBL.GetTransactionCertificate(transactionId, cultureName), cultureName);
                    getResult = GetResult<OutboundCertificateDTO>.Create(statusCode, outboundCertificateDTO, null);
                    LogAction(AuditingActionCode.ViewCertificate, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<OutboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<OutboundCertificateDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage SearchDeliveryReport(string strSearchCriteria)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

            SearchCriteria searchCriteria = javaScriptSerializer.Deserialize(strSearchCriteria, typeof(SearchCriteria)) as SearchCriteria;

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDeliveryReportDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

                    IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetDeliveryReport(searchCriteria, out rowsCount);

                    List<TransactionDeliveryReportDTO> TransactionDeliveryReportDTOs = TransactionDeliveryReportMapper.Map(transactionDeliveryReports, Language);

                    getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, TransactionDeliveryReportDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionVisitTicket(int transactionId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionVisitTicketDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                    ITransactionBL transactionBL = TransactionBL.Create((Common.TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, ""));

                    TransactionVisitTicketInfo transactionVisitTicket = transactionBL.GetVisitTicket(transaction, orgUnitId, cultureName);

                    TransactionVisitTicketDTO transactionVisitTicketDTO = TransactionBarcodesMapper.Map(transactionVisitTicket);

                    // LogAction(AuditingActionCode.ViewVisitTicket, transaction.Id);

                    getResult = GetResult<TransactionVisitTicketDTO>.Create(statusCode, transactionVisitTicketDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionVisitTicketDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionVisitTicketDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpGet]
        public HttpResponseMessage GetTransactionName(string civilID, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionNameDTO> getResult = null;
            TransactionNameDTO transactionNameDTO = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    INameBL nameBL = new NameBL();

                    Name transactionName = nameBL.GetNameByCivilId(civilID);

                    if (transactionName != null)
                    {
                        transactionNameDTO = TransactionNameMapper.Map(transactionName);
                    }
                    else
                    {
                        transactionNameDTO = new TransactionNameDTO();
                        transactionNameDTO.CivilID = civilID;
                        statusCode = Common.StatusCode.NameNotFound;
                    }

                    getResult = GetResult<TransactionNameDTO>.Create(statusCode, transactionNameDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionNameDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionNameDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransaction(int userId, int transactionNumber, TransactionCategory transactionCategory, int year, int sourceId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(transactionCategory);

                    Transaction transaction = transactionBL.GetTransaction(userId, transactionNumber, transactionCategory, year,
                        sourceId, orgUnitId, cultureName);

                    if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                    {
                        if (transaction.MainDocument.Document.Content == null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation());
                            transaction.MainDocument.Document.Content = docData.Data;
                        }
                    }

                    foreach (Attachment attachment in transaction.Attachments)
                    {
                        if (attachment.DocumentInfo.Document != null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(attachment.DocumentInfo.Id.ToString(), new DocumentLocation());
                            attachment.DocumentInfo.Document.Content = docData.Data;
                        }
                    }

                    TransactionDTO transactionDTO = TransactionMapper.Map(transaction);

                    LogAction(AuditingActionCode.ViewBasicInformation, transaction.Id);

                    getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]

        public HttpResponseMessage GetTransactionsByNationalId(string nationalId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDetailsDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);

                    List<Transaction> transactionList = transactionBL.GetTransactionsByNationalId(nationalId);

                    List<TransactionDetailsDTO> transactionDetailsDTO = TransactionMapper.MapTransaction(transactionList);

                    getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, transactionDetailsDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        public HttpResponseMessage GetTransactionByNumberAndYear(int year, int transactionNumber)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDetailsDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);

                    Transaction transaction = transactionBL.GetTransactionByNumberAndYear(year, transactionNumber);





                    TransactionDetailsDTO transactionDetailsDTO = TransactionMapper.MapTransaction(transaction);



                    getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionIdByLinkType(int linkTypeId, string sourceNumber, int orgUnitId, int yearId, string cultureName, int? yearSearch)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDetailsDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                    LinkingType linkingType = (LinkingType)linkTypeId.LookupInternalID(LookupCategory.LinkingType, cultureName);

                    int year = -1;
                    if (yearSearch == null || yearSearch == 0)
                    {
                        Lookup lookup = lookupBL.GetLookupItem(yearId, cultureName);
                        if (lookup != null)
                        {
                            year = int.Parse(lookup.Text);

                        }
                    }
                    else
                    {
                        year = (int)yearSearch;
                    }



                    int? transactionId = TransactionBL.GetTransactionIdByLinkType(sourceNumber, year, orgUnitId, linkingType, cultureName);
                    if (transactionId == null)
                    {
                        statusCode = Common.StatusCode.TransactionNotFound;
                        getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, new TransactionDetailsDTO(), null);

                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                    else
                    {
                        int transactionCount = TransactionBL.GetTransactionByIdAndOrgUnit(transactionId.Value, orgUnitId);
                        if (transactionCount > 0)
                        {
                            Transaction transaction = TransactionBL.GetTransactionById(transactionId.Value);

                            string SourceTypeName = (transaction.TransactionType != null) ? transaction.TransactionType.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText() : null;
                            getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, new TransactionDetailsDTO() { Id = transaction.Id, Number = transaction.Number, Date = transaction.Date, HijriDate = transaction.DateH, TransactionsTypes = SourceTypeName, Subject = transaction.Subject, Year = year, ConfidentialityId = transaction.ConfidentialityId }, null);

                        }
                        else
                        {
                            statusCode = Common.StatusCode.TransactionNotFound;
                            getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, new TransactionDetailsDTO(), null);

                            return Request.CreateResponse(HttpStatusCode.OK, getResult);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        //GetPreviousTransactionByID
        [HttpGet]
        public HttpResponseMessage GetPreviousTransactionByID(Common.TransactionCategory transactionCategory, int transactionsId, int orgUnitId, string cultureName, bool IsForIndividual = false)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(transactionCategory);
                    TransactionDTO transactionDTO = null;

                    Transaction previousTransaction = transactionBL.GetPreviousTransactionByID(transactionsId, orgUnitId, cultureName, IsForIndividual);

                    if (previousTransaction != null)
                    {
                        if (previousTransaction.MainDocument != null && previousTransaction.MainDocument.Document != null)
                        {
                            if (previousTransaction.MainDocument.Document.Content == null)
                            {
                                DocData docData = DocRepository.DocRepository.Load(previousTransaction.MainDocument.Id.ToString(), new DocumentLocation());
                                previousTransaction.MainDocument.Document.Content = docData.Data;
                            }
                        }

                        transactionDTO = TransactionMapper.MapGetPrevious(previousTransaction);
                    }

                    getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPreviousTransaction(Common.TransactionCategory transactionCategory, int orgUnitId, string cultureName, bool IsForIndividual = false)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(transactionCategory);
                    TransactionDTO transactionDTO = null;

                    Transaction previousTransaction = transactionBL.GetPreviousTransaction(orgUnitId, cultureName, IsForIndividual);

                    if (previousTransaction != null)
                    {
                        if (previousTransaction.MainDocument != null && previousTransaction.MainDocument.Document != null)
                        {
                            if (previousTransaction.MainDocument.Document.Content == null)
                            {
                                DocData docData = DocRepository.DocRepository.Load(previousTransaction.MainDocument.Id.ToString(), new DocumentLocation());
                                previousTransaction.MainDocument.Document.Content = docData.Data;
                            }
                        }

                        transactionDTO = TransactionMapper.MapGetPrevious(previousTransaction);
                    }

                    getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  Transactions

        #region TransactionAssignment

        [HttpPost]
        public HttpResponseMessage PostTransactionAssignments([FromUri] List<int> transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
                        ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                        IBarcodeBL barcodeBL = IoC.Resolve<IBarcodeBL>();
                        List<Transaction> transactions = new List<Transaction>();
                        foreach (var item in transactionId)
                        {
                            transactions.Add(TransactionBL.GetTransactionById(item));
                        }

                        IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                        transactionAssignmentBL.AssignTransaction(transactions, transactionAssignments, cultureName);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        foreach (var transaction in transactions)
                        {
                            notificationBL.SendAssignmentNotification(transaction, transactionAssignments, cultureName);
                        }
                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage PostTransactionAssignment(TransactionAssignmentDTO transactionAssignmentDTO, string transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                        IBarcodeBL barcodeBL = IoC.Resolve<IBarcodeBL>();
                        INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
                        List<Transaction> transactions = new List<Transaction>();

                        transactions.Add(TransactionBL.GetTransactionById(Convert.ToInt32(transactionId)));

                        TransactionAssignment transactionAssignment = TransactionAssignmentMapper.Map(transactionAssignmentDTO);

                        List<TransactionAssignment> transactionAssignments = new List<TransactionAssignment>();
                        transactionAssignments.Add(transactionAssignment);

                        transactionAssignmentBL.AssignTransaction(transactions, transactionAssignments, cultureName);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        foreach (var transaction in transactions)
                        {
                            notificationBL.SendAssignmentNotification(transaction, transactionAssignments, cultureName);
                        }

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage SetTransactionAssignmentToViewedByTransactionId(int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                    transactionAssignmentBL.SetTransactionAssignmentToViewedByTransactionId(transactionId);
                    postResult = PostResult.Create(statusCode, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage HideTransactionAssignment(int assignmentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();

                        transactionAssignmentHistoryBL.HideTransactionAssignment(assignmentId);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage HideAssignment(int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();

                        transactionAssignmentHistoryBL.HideTransaction(transactionId);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage HideTransactionAssignments(string assignmentIds)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();

                        transactionAssignmentHistoryBL.HideTransactionAssignments(assignmentIds);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage HideAssignments(string transactionIds)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();

                        transactionAssignmentHistoryBL.HideTransactions(transactionIds);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage TransactionElcOutBoundAdd(TransactionElcOutBoundDTO transactionElcOutBoundDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    TransactionBL.TransactionElcOutBoundAdd(TransactionElcOutBoundMapper.Map(transactionElcOutBoundDTO));
                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage AddConfidentialityAcknowledgment(int TransactionId, int UserId, int OrgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {

                    TransactionBL.AddConfidentialityAcknowledgment(TransactionId, UserId, OrgUnitId, DateTime.Now);
                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage AcknowledgeElcOutBound(int userId, int orgUnitId, bool ishidden, int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    LogAction(AuditingActionCode.AcknowledgeElcOutBound, transactionId);
                    TransactionBL.AcknowledgeElcOutBound(userId, orgUnitId, ishidden, transactionId);
                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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


        #endregion  TransactionAssignment

        #region TransactionTasks

        [HttpPost]
        public HttpResponseMessage UpdateTransactionTasks(TransactionTaskDTO transactionTaskDTOs, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                        List<Task> tasks = TransactionTaskMapper.Map(transactionTaskDTOs);
                        // AddTask(task);
                        foreach (var Task in tasks)
                        {
                            transactionTaskBL.UpdateTask(Task, Language);
                        }

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage DeleteTransactionTasks(IList<int> ids)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                        transactionTaskBL.DeleteTasks(ids, Language);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage PostTransactionTasks(TransactionTaskDTO transactionTaskDTOs, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();// (new ResolverOverride[] { new ParameterOverride("userId", 1116) });

                        List<Task> tasks = TransactionTaskMapper.Map(transactionTaskDTOs);

                        transactionTaskBL.AddTasks(transactionTaskDTOs.TransactionId, tasks, cultureName);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage PutRejectTransactionTask(TaskActionDTO taskActionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();// (new ResolverOverride[] { new ParameterOverride("userId", 1116) });

                        Task Task = TaskRejectMapper.Map(taskActionDTO);

                        transactionTaskBL.RejectTask(Task);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        public HttpResponseMessage PutCompleteTransactionTask(TaskActionDTO taskActionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                        Task Task = TaskRejectMapper.Map(taskActionDTO);

                        transactionTaskBL.CompleteTask(Task, Language);

                        putResult = PutResult.Create(statusCode);

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

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

        //[HttpPost]
        //public HttpResponseMessage PostSubTransactionTask(TransactionSubTaskDTO transactionSubTaskDTO, string cultureName)
        //{
        //    StatusCode statusCode = Common.StatusCode.Ok;
        //    PostResult postResult = null;

        //    try
        //    {
        //        using (var transactionContextScope = context.Create())
        //        {
        //            if (ModelState.IsValid)
        //            {
        //                ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

        //                IList<Task> tasks = TransactionTaskMapper.Map(transactionSubTaskDTO);

        //                transactionTaskBL.AddSubTask(transactionSubTaskDTO.TransactionId, tasks, cultureName);

        //                postResult = PostResult.Create(statusCode, null);

        //                transactionContextScope.Commit();

        //                return Request.CreateResponse(HttpStatusCode.Created, postResult);
        //            }

        //            statusCode = Common.StatusCode.ModelNotValid;

        //            postResult = PostResult.Create(statusCode, null);

        //            return Request.CreateResponse(HttpStatusCode.OK, postResult);
        //        }
        //    }
        //    catch (BusinessException ex)
        //    {
        //        statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

        //        postResult = PostResult.Create(statusCode, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, postResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.HandleException(ex);

        //        statusCode = Common.StatusCode.GeneralError;

        //        postResult = PostResult.Create(statusCode, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, postResult);
        //    }
        //}

        [HttpGet]
        public HttpResponseMessage GetTasksCount(int transactonId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int?> getResult = null;
            int tasksCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    tasksCount = transactionTaskBL.GetTaskCount(transactonId);

                    getResult = GetResult<int?>.Create(statusCode, tasksCount, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int?>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int?>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTask(int taskId, int transactonId, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TaskLightDTO> getResult = null;
            TaskLightDTO taskLightDTO = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();
                    var task = transactionTaskBL.GetTask(taskId, transactonId, orgUnitId);
                    taskLightDTO = TaskMapper.Map(task);
                    getResult = GetResult<TaskLightDTO>.Create(statusCode, taskLightDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TaskLightDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TaskLightDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetReceivedTasksByFilter(int pageIndex, int pageSize, int orgUnitId, string cultureName, [FromUri] SearchCriteriaCustom searchCriteria, int ReceivedTasksTypeId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReceivedTaskDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Task> tasks = transactionTaskBL.GetReceivedTasks(pageIndex, pageSize, orgUnitId, cultureName, searchCriteria, ReceivedTasksTypeId, out rowsCount);

                    List<ReceivedTaskDTO> taskDTOs = TaskMapper.MapReceivedTask(tasks, cultureName);

                    getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, taskDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetReceivedTasks(int pageIndex, int pageSize, int orgUnitId, string cultureName, int ReceivedTasksTypeId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReceivedTaskDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Task> tasks = transactionTaskBL.GetReceivedTasks(pageIndex, pageSize, orgUnitId, cultureName, ReceivedTasksTypeId, out rowsCount);

                    List<ReceivedTaskDTO> taskDTOs = TaskMapper.MapReceivedTask(tasks, cultureName);

                    getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, taskDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetUserTasksStatus(int userId, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TaskStatusDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Task> tasks = transactionTaskBL.GetTasks(t => t.ToOrgUnitId == orgUnitId && t.ToUserId == userId);

                    List<TaskStatusDTO> taskStatusDTOs = TaskMapper.MapTasksStatus(tasks);

                    getResult = GetResult<List<TaskStatusDTO>>.Create(statusCode, taskStatusDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TaskStatusDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TaskStatusDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetReceivedTask(int taskId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ReceivedTaskDTO> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    Task task = transactionTaskBL.GetTaskById(taskId);

                    ReceivedTaskDTO taskDTO = TaskMapper.MapReceivedTask(task, cultureName);

                    getResult = GetResult<ReceivedTaskDTO>.Create(statusCode, taskDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ReceivedTaskDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ReceivedTaskDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSentTask(int taskId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SentTaskDTO> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    Task task = transactionTaskBL.GetTaskById(taskId);

                    SentTaskDTO sentTaskDTO = TaskMapper.MapSentTask(task, cultureName);

                    getResult = GetResult<SentTaskDTO>.Create(statusCode, sentTaskDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SentTaskDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SentTaskDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSentTasks(int pageIndex, int pageSize, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SentTaskDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Task> tasks = transactionTaskBL.GetSentTasks(pageIndex, pageSize, orgUnitId, cultureName, out rowsCount);

                    List<SentTaskDTO> sentTaskDTOs = TaskMapper.MapSentTask(tasks, cultureName);

                    getResult = GetResult<List<SentTaskDTO>>.Create(statusCode, sentTaskDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SentTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SentTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostTaskReminder(int taskId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    transactionTaskBL.SendTaskReminder(taskId, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        //[HttpGet]
        //public HttpResponseMessage GetTaskSequenceOrgUnits(int taskId, int orgUnitId, string cultureName)
        //{
        //    StatusCode statusCode = Common.StatusCode.Ok;
        //    GetResult<List<OrgUnitDTO>> getResult = null;
        //    int rowsCount = 0;

        //    try
        //    {
        //        using (var transactionContextScope = context.CreateReadOnly())
        //        {
        //            ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

        //            IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

        //            IList<OrgUnit> orgUnits = transactionTaskBL.GetTaskSequenceOrgUnits(taskId, orgUnitId, cultureName);

        //            List<OrgUnitDTO> OrgUnitDTOs = OrgUnitMapper.Map(orgUnits, cultureName);

        //            getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTOs, rowsCount);

        //            return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //        }
        //    }
        //    catch (BusinessException ex)
        //    {
        //        statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

        //        getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.HandleException(ex);

        //        statusCode = Common.StatusCode.GeneralError;

        //        getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage GetTaskSequenceUsers(int taskId, int fromOrgUnitId, int toOrgUnitId, string cultureName)
        //{
        //    StatusCode statusCode = Common.StatusCode.Ok;
        //    GetResult<List<UserProfileDTO>> getResult = null;
        //    int rowsCount = 0;

        //    try
        //    {
        //        using (var transactionContextScope = context.CreateReadOnly())
        //        {
        //            ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

        //            IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

        //            IList<UserProfile> userProfiles = transactionTaskBL.GetTaskSequenceUsers(taskId, fromOrgUnitId, toOrgUnitId, cultureName);

        //            List<UserProfileDTO> userProfileDTOs = UserProfileMapper.Map(userProfiles);

        //            getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, userProfileDTOs, rowsCount);

        //            return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //        }
        //    }
        //    catch (BusinessException ex)
        //    {
        //        statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

        //        getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.HandleException(ex);

        //        statusCode = Common.StatusCode.GeneralError;

        //        getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //}

        [HttpPut]
        public HttpResponseMessage ExtendTaskDate(int taskId, string dateTime)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                        DateTime newDateTime = DateTime.ParseExact(dateTime, "dd/MM/yyyy", null);

                        transactionTaskBL.ExtendTaskDate(taskId, newDateTime);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetTaskAttachments(int TaskId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TaskAttachmentsDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<TasksAttachments> tasksAttachments = transactionTaskBL.GetTaskAttachments(TaskId);

                    List<TaskAttachmentsDTO> taskAttachmentsDTOs = TaskMapper.Map(tasksAttachments);

                    getResult = GetResult<List<TaskAttachmentsDTO>>.Create(statusCode, taskAttachmentsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TaskAttachmentsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TaskAttachmentsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTaskAttachmentById(int DocumentInfoId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    DocumentInfo documentInfo = transactionTaskBL.GetDocumentInfoById(DocumentInfoId);

                    DocumentDTO documentDTO = DocumentMapper.Map(documentInfo);

                    getResult = GetResult<DocumentDTO>.Create(statusCode, documentDTO, rowsCount);

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
        public HttpResponseMessage GetTransactionTasks(int transactionId, [FromUri] SearchCriteria searchCriteria, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TaskAddDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Task> tasks = transactionTaskBL.GetTransactionTasks(transactionId, searchCriteria, cultureName, out rowsCount);

                    List<TaskAddDTO> taskDTOs = TaskMapper.MapTasks(tasks, cultureName);

                    getResult = GetResult<List<TaskAddDTO>>.Create(statusCode, taskDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TaskAddDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TaskAddDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionTasksReply(int transactionId, [FromUri] SearchCriteria searchCriteria, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReceivedTaskDTO>> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    IList<Task> tasks = transactionTaskBL.GetTransactionTasksReply(transactionId, searchCriteria, cultureName, out rowsCount);

                    List<ReceivedTaskDTO> taskDTOs = TaskMapper.MapTasksReply(tasks, cultureName);

                    getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, taskDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReceivedTaskDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage AcceptRejectTask(int TaskId, int taskAcceptanceStatus, string RejectionReason)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    transactionTaskBL.AcceptTask(TaskId, taskAcceptanceStatus, RejectionReason, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage ResendTask(int TaskId, string ResendReason, int ExpectedDays)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    transactionTaskBL.ResendTask(TaskId, ResendReason, ExpectedDays, Language);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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




        #endregion  TransactionTasks

        #region File

        [HttpGet]
        public HttpResponseMessage GetUserTrays(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TrayDetailsDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    IList<TrayDetailsInfo> traysDetails = fileBL.GetUserTrays(orgUnitId, cultureName);

                    List<TrayDetailsDTO> trayDetailsDTOs = TrayDetailsMapper.Map(traysDetails);

                    getResult = GetResult<List<TrayDetailsDTO>>.Create(statusCode, trayDetailsDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TrayDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TrayDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTrayDetailsInfo(TrayType trayType, int orgUnitId, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TrayDetailsDTO> getResult = null;
            int rowsCount = 0;
            if (searchCriteria.OrderBy == null || searchCriteria.OrderBy == "")
            {
                searchCriteria.OrderBy = "Id";
            }

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();


                    TrayDetailsInfo trayDetailsInfo = fileBL.GetTrayDetailsInfo(trayType, orgUnitId, searchCriteria, out rowsCount);

                    TrayDetailsDTO trayDetailsInfoDTO = TrayDetailsMapper.Map(trayDetailsInfo);

                    getResult = GetResult<TrayDetailsDTO>.Create(statusCode, trayDetailsInfoDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }





        [HttpGet]
        public HttpResponseMessage GetWithdrawalData(int? transId, int? orgunitId, int? transactionTypeId, int? year, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TrayDetailsDTO> getResult = null;
            int rowsCount = 0;
            if (searchCriteria.OrderBy == null || searchCriteria.OrderBy == "")
            {
                searchCriteria.OrderBy = "Id";
            }

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    TrayDetailsInfo trayDetailsInfo = fileBL.GetWithdrawalData(transId, orgunitId, transactionTypeId, year, searchCriteria, out rowsCount);

                    TrayDetailsDTO trayDetailsInfoDTO = TrayDetailsMapper.Map(trayDetailsInfo);

                    getResult = GetResult<TrayDetailsDTO>.Create(statusCode, trayDetailsInfoDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetTransactionAssignmentLight(int orgUnitId, int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionAssignmentDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    var transactionAssignmentInfo = fileBL.GetTransactionAssignmentLight(orgUnitId, transactionId);

                    var transactionAssignmentDTO = TransactionAssignmentMapper.Map(transactionAssignmentInfo);

                    getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, transactionAssignmentDTO, 0);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetSelectedTransactions(string transactionsIds, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TrayDetailsDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    List<int> ids = transactionsIds.Split(',').Select(int.Parse).ToList();

                    TrayDetailsInfo trayDetailsInfo = fileBL.GetSelectedTransactions(ids, CultureName);

                    TrayDetailsDTO trayDetailsInfoDTO = TrayDetailsMapper.Map(trayDetailsInfo);

                    getResult = GetResult<TrayDetailsDTO>.Create(statusCode, trayDetailsInfoDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetPopulariazations(int orgUnitId, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TrayDetailsDTO> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITrayBL trayBL = TrayBaseBL.Create(TrayType.Copies);

                    TrayDetailsInfo trayDetailsInfo = trayBL.GetPopulariazations(orgUnitId, searchCriteria, out rowsCount);

                    TrayDetailsDTO trayDetailsInfoDTO = TrayDetailsMapper.Map(trayDetailsInfo);

                    getResult = GetResult<TrayDetailsDTO>.Create(statusCode, trayDetailsInfoDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TrayDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetUserTransactionsTray(TrayType trayType, int orgUnitId, TransactionDateType transactionDate, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionTrayInfoDTO>> getResult = null;
            if (searchCriteria.OrderBy == null || searchCriteria.OrderBy == "")
            {
                searchCriteria.OrderBy = "Id";
            }

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    IList<TransactionTrayInfo> transactions = fileBL.GetAllUserTransactionsByTray(trayType, orgUnitId, transactionDate, searchCriteria, out rowsCount);

                    List<TransactionTrayInfoDTO> userTransactionsTrayDTOs = TrayDetailsMapper.Map(transactions);

                    getResult = GetResult<List<TransactionTrayInfoDTO>>.Create(statusCode, userTransactionsTrayDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionTrayInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionTrayInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #region FollowUp
        [HttpGet]
        public HttpResponseMessage GetFollowUpProccess(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpProccessBL ProccesBl = IoC.Resolve<IFollowUpProccessBL>();

                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<FollowUpProccess> followupProccess = ProccesBl.GetFollowUpProccess(transactionCategories, cultureName);

                    List<FollowUpLookUpDTO> ProccessDTOs = FollowUpProccessMapper.Map(followupProccess, cultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, ProccessDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        public HttpResponseMessage GetFollowUpPrioritytype(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpPriorityTypeBL PriorityTypeBl = IoC.Resolve<IFollowUpPriorityTypeBL>();

                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<FollowUpPriorityType> followupPriorityType = PriorityTypeBl.GetFollowUpPrioritytypes(transactionCategories, cultureName);

                    List<FollowUpLookUpDTO> PriorityTypeDTOs = FollowUpLookUpsMapper.Map(followupPriorityType, cultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, PriorityTypeDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetFollowUpSource(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpSourceBL SourceBl = IoC.Resolve<IFollowUpSourceBL>();

                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<FollowUpSource> followupSource = SourceBl.GetFollowUpSources(transactionCategories, cultureName);

                    List<FollowUpLookUpDTO> SourcesDTOs = FollowUpSourceMapper.Map(followupSource, cultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, SourcesDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetFollowUpMethod(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FollowUpLookUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFollowUpMethodBL MethodBl = IoC.Resolve<IFollowUpMethodBL>();

                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<FollowUpMethod> followupMethod = MethodBl.GetFollowUpMethods(transactionCategories, cultureName);

                    List<FollowUpLookUpDTO> MethodsDTOs = FollowUpMethodMapper.Map(followupMethod, cultureName);

                    getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, MethodsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FollowUpLookUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage FollowUpDetailsAdd(int transactionId, int orgUnitId, int userId, string note)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    TransactionBL.FollowUpDetailsAdd(transactionId, orgUnitId, userId, note);
                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage AddFollowupUditTrial(FollowUpAuditTrailDTO addFollowupUditTrialDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    TransactionBL.AddFollowupUditTrial(FollowUpAuditTrailMapper.Map(addFollowupUditTrialDTO));
                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetListFollowupUditTrial(int id, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IList<DTO.FollowUpAuditTrailDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IList<FollowUpAuditTrail> list = TransactionBL.GetListFollowupUditTrial(id, cultureName);

                    getResult = GetResult<IList<DTO.FollowUpAuditTrailDTO>>.Create(statusCode, FollowUpAuditTrailMapper.Map(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IList<DTO.FollowUpAuditTrailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IList<DTO.FollowUpAuditTrailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage FollowUpDetailsByTransId(int transId, int FollowUpStatusId, int UserId, int OrgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionFollowUpDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    TransactionFollowUp list = TransactionBL.FollowUpDetailsByTransId(transId, FollowUpStatusId, UserId, OrgUnitId, cultureName);

                    getResult = GetResult<TransactionFollowUpDTO>.Create(statusCode, TransactionFollowUpMapper.Map(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionFollowUpDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionFollowUpDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage FollowUpDetailsByFollowUpId(int FollowUpId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionFollowUpDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    TransactionFollowUp list = TransactionBL.FollowUpDetailsByFollowUpId(FollowUpId, cultureName);

                    getResult = GetResult<TransactionFollowUpDTO>.Create(statusCode, TransactionFollowUpMapper.Map(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionFollowUpDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionFollowUpDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage FollowUpDetailsById(int id, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IList<DTO.FollowUpDetailsDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IList<FollowUpDetails> list = TransactionBL.FollowUpDetailsById(id, cultureName);

                    getResult = GetResult<IList<DTO.FollowUpDetailsDTO>>.Create(statusCode, TransactionFollowUpMapper.MapToFollowUpDetailsDTO(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IList<DTO.FollowUpDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IList<DTO.FollowUpDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage TransactionFollowUpSelectByTransId(int transId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IList<TransactionFollowUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IList<TransactionFollowUp> list = TransactionBL.TransactionFollowUpSelectByTransId(transId, cultureName);

                    getResult = GetResult<IList<TransactionFollowUpDTO>>.Create(statusCode, TransactionFollowUpMapper.Map(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IList<TransactionFollowUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IList<TransactionFollowUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage TransactionFollowUpSelectByFollowUpId(int transId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IList<TransactionFollowUpDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IList<TransactionFollowUp> list = TransactionBL.TransactionFollowUpSelectByTransId(transId, cultureName);

                    getResult = GetResult<IList<TransactionFollowUpDTO>>.Create(statusCode, TransactionFollowUpMapper.Map(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IList<TransactionFollowUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IList<TransactionFollowUpDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetFollowUpAuditTrail(int FollowUpId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IList<FollowUpAuditTrailDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IList<FollowUpAuditTrail> list = TransactionBL.GetFollowUpAuditTrail(FollowUpId, cultureName);

                    getResult = GetResult<IList<FollowUpAuditTrailDTO>>.Create(statusCode, FollowUpAuditTrailMapper.Map(list), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IList<FollowUpAuditTrailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IList<FollowUpAuditTrailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage TransactionFollowUpAdd(TransactionFollowUpDTO oTransactionFollowUpDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            int followupId;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    followupId = transactionBL.TransactionFollowUpAdd(TransactionFollowUpMapper.Map(oTransactionFollowUpDTO), cultureName);

                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, followupId);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        public HttpResponseMessage TransactionFollowUpUpdate(TransactionFollowUpDTO oTransactionFollowUpDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;


            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    transactionBL.TransactionFollowUpUpdate(TransactionFollowUpMapper.Map(oTransactionFollowUpDTO), cultureName);

                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage SendFollowUpReminder(int FollowUpId, int TransactionId, int FollowUoUserId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            int followupId;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    transactionBL.SendFollowUpReminder(FollowUpId, TransactionId, FollowUoUserId, cultureName);

                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage EscalateFollowUp(int FollowUpId, int TransactionId, int FollowUpUserID, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            int followupId;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    transactionBL.EscalateFollowUp(FollowUpId, TransactionId, FollowUpUserID, cultureName);

                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetChildFollowUpUserId(int FollowUpId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            int? ChildFollowUpUser;
            try
            {

                {
                    using (var transactionContextScope = context.Create())
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                        ChildFollowUpUser = transactionBL.GetChildFollowUpUserId(FollowUpId);

                        transactionContextScope.Commit();
                    }

                    postResult = PostResult.Create(statusCode, ChildFollowUpUser);


                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage CheckIfFollowUpAdded(int TransactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    bool userGroup = transactionBL.CheckIfFollowUpAdd(TransactionId);

                    getResult = GetResult<bool>.Create(statusCode, userGroup, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpPost]
        public HttpResponseMessage getFollowUpDepartment(int EntityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            int? FoDepartment;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IOrgUnitBL orgunitBL = new OrgUnitBL();
                    FoDepartment = orgunitBL.getFollowUpDepartment(EntityId);

                    transactionContextScope.Commit();
                }

                postResult = PostResult.Create(statusCode, FoDepartment);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage FollowUpUpdateIsDeleted(int transactionId, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);
                    transactionBL.FollowUpUpdateIsDeleted(transactionId, userId, Language);
                }
                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage MultiFollowUpUpdateIsDeleted(List<int> transactionIds, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {

                ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                using (var transactionContextScope = context.Create())
                {
                    transactionIds.ForEach(t =>
                    transactionBL.FollowUpUpdateIsDeleted(t, userId, Language)
                    );
                }
                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage UpdateTransactionLinks(int transactionId, IList<TransactionLink> Links)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.UpdateTransactionLinks(transactionId, Links);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage FollowUpAddTransactionLinks(int transactionId, IList<TransactionLink> Links)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.FollowUpAddTransactionLinks(transactionId, Links);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage UpdateTransactionDeleteByTransId(long transactionId, bool isDeleted)
        {

            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                        transactionBL.UpdateTransactionDeleteByTransId(transactionId, isDeleted);
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

        [HttpPost]
        public HttpResponseMessage DeleteDraftTransaction(long transactionId, bool isDeleted)
        {

            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                        transactionBL.DeleteDraftTransaction(transactionId, isDeleted);
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
        public HttpResponseMessage FollowUpChangeStatus(int Id, int FollowupStatus, bool IsActive)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.FollowUpChangeStatus(Id, FollowupStatus, IsActive);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage FollowUpUpdateEscalatedStatus(int Id, bool IsEscalated)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.FollowUpUpdateEscalatedStatus(Id, IsEscalated);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage FollowUpUpdateReminderStatus(int Id, bool IsReminder)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.FollowUpUpdateReminderStatus(Id, IsReminder);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage FollowUpUpdateReceive(int Id, int UserId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.FollowUpUpdateReceive(Id, UserId);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage FollowUpUpdateIsDeleted(int Id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                    transactionBL.FollowUpUpdateIsDeleted(Id, Language);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        #endregion

        [HttpPut]
        public HttpResponseMessage MoveTransaction(int transactionId, int orgUnitId, int trayActionTypeId, int trayId, int? assigmentId, string remarks, int userId, object extraParams)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IFileBL fileBL = new FileBL();
                    IList<TransactionAssignment> transactionAssignments = null;
                    if (extraParams != null)
                    {
                        List<TransactionAssignmentDTO> transactionAssignmentsDTO = null;
                        transactionAssignmentsDTO =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<List<TransactionAssignmentDTO>>(extraParams.ToString());
                        transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentsDTO);
                    }
                    fileBL.MoveTransaction(transactionId, orgUnitId, trayId, (TrayActionType)trayActionTypeId, assigmentId, remarks, userId, Language, transactionAssignments);

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

        [HttpPut]
        public HttpResponseMessage LinkedMoveTransaction(int transactionId, int orgUnitId, int trayActionTypeId, int trayId, int? assigmentId, string remarks, int userId, object extraParams)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IFileBL fileBL = new FileBL();
                    IList<TransactionAssignment> transactionAssignments = null;
                    if (extraParams != null)
                    {
                        List<TransactionAssignmentDTO> transactionAssignmentsDTO = null;
                        transactionAssignmentsDTO =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<List<TransactionAssignmentDTO>>(extraParams.ToString());
                        transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentsDTO);
                    }
                    fileBL.LinkedMoveTransaction(transactionId, orgUnitId, trayId, (TrayActionType)trayActionTypeId, assigmentId, remarks, userId, Language, transactionAssignments);

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
        public HttpResponseMessage MoveTransactionsList(string transactionsIds, int orgUnitId, int trayActionTypeId, int trayId, int? assigmentId, string remarks, int userId, object extraParams)
        {
            List<int> IdsList = transactionsIds.Split(',').Select(int.Parse).ToList();
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IFileBL fileBL = new FileBL();
                    IList<TransactionAssignment> transactionAssignments = null;
                    if (extraParams != null)
                    {
                        List<TransactionAssignmentDTO> transactionAssignmentsDTO = null;
                        transactionAssignmentsDTO =
                            Newtonsoft.Json.JsonConvert.DeserializeObject<List<TransactionAssignmentDTO>>(extraParams.ToString());
                        transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentsDTO);
                    }
                    foreach (var item in IdsList)
                    {
                        fileBL.MoveTransaction(item, orgUnitId, trayId, (TrayActionType)trayActionTypeId, assigmentId, remarks, userId, Language, transactionAssignments);
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
        public HttpResponseMessage CreateOutboundExternal(int transactionId, int trayId, TransactionDTO transactionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<TransactionDetailsDTO> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFileBL fileBL = new FileBL();

                        TransactionDetails transactionDetails = null;

                        TransactionDetailsDTO transactionDetailsDTO = null;

                        Transaction transaction = TransactionMapper.Map(transactionDTO);

                        byte[] mainDocumentContent = null;
                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            transaction.MainDocument.Document.Content = null;
                        }

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transaction.MainDocument.Name,
                                DocID = transaction.MainDocument.Id.ToString(),
                                PersonId = transaction.CreatedBy,
                                MimeContent = transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transaction.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        transactionDetails = fileBL.CreateOutboundExternal(transactionId, trayId, transaction);

                        transactionDetailsDTO = TransactionDetailsMapper.Map(transactionDetails);

                        postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postObjectResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }

        }

        [HttpGet]
        public HttpResponseMessage PrepareOutboundCreation(int transactionId, int orgUnitId, int trayId, string cultureName) //NotSure
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IFileBL fileBL = new FileBL();

                        Transaction transaction = fileBL.PrepareOutboundCreation(transactionId, orgUnitId, trayId, cultureName);

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            transaction.MainDocument.Document.Content = DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation()).Data;
                        }

                        TransactionDTO transactionDTO = TransactionMapper.Map(transaction);

                        getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOutBoundAddressInfo(int TransactionId, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionAddressDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Transaction transaction = TransactionBL.GetTransactionById(TransactionId);
                    TransactionAddressDTO transactionAddressDTO = TransactionAddressMapper.Map(transaction, CultureName);
                    getResult = GetResult<TransactionAddressDTO>.Create(statusCode, transactionAddressDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionAddressDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionAddressDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        #endregion  File

        #region Priority

        [HttpGet]
        public HttpResponseMessage GetPriorities(TransactionCategory transactionCategory, string cultureName, int OrgUnitId, int UserId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PriorityDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IPriorityBL priorityBL = IoC.Resolve<IPriorityBL>();
                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);
                    IList<Priority> priorities = priorityBL.GetPriorities(transactionCategories, cultureName, OrgUnitId, UserId).ToList();
                    List<PriorityDTO> prioritiesDTO = PriorityMapper.Map(priorities, cultureName);

                    getResult = GetResult<List<PriorityDTO>>.Create(statusCode, prioritiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PriorityDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PriorityDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  Priority

        #region TransactionType

        [HttpGet]
        public HttpResponseMessage GetTransactionTypes(TransactionCategory transactionCategory, string cultureName)
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

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionSourceTypes(TransactionCategory transactionCategory, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();
                    TransactionCategories sourceTransactionType = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<Domain.TransactionType> transactionTypes = transactionTypeBL.GetTransactionSourceTypes(sourceTransactionType, cultureName);

                    List<TransactionTypeDTO> transactionTypesDTOs = TransactionTypeMapper.Map(transactionTypes, cultureName);

                    getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, transactionTypesDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion Transaction

        #region Link

        [HttpGet]
        public HttpResponseMessage GetLinkTypes(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LinkDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILinkBL linkBL = IoC.Resolve<ILinkBL>();

                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);

                    IList<Link> links = linkBL.GetLinks(transactionCategories, cultureName);

                    List<LinkDTO> linksDTOs = LinkMapper.Map(links, cultureName);

                    getResult = GetResult<List<LinkDTO>>.Create(statusCode, linksDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LinkDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LinkDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion Link

        #region Form
        [HttpGet]
        public HttpResponseMessage GetContentByFormId(int formId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFormBL formBL = IoC.Resolve<IFormBL>();

                    DocumentInfo formContent = formBL.GetContentByFormId(formId);

                    DocumentDTO formContentDTO = DocumentMapper.MapWithContent(formContent);

                    getResult = GetResult<DocumentDTO>.Create(statusCode, formContentDTO, null);

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
        #endregion  FormSearchDeliveryReportByNumber

        #region LetterType

        [HttpGet]
        public HttpResponseMessage GetLetterTypes(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LetterTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();
                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);
                    IList<LetterType> letterTypes = letterTypeBL.GetLetterTypes(transactionCategories, cultureName).ToList();
                    List<LetterTypeDTO> letterTypesDTO = LetterTypeMapper.Map(letterTypes, cultureName);
                    getResult = GetResult<List<LetterTypeDTO>>.Create(statusCode, letterTypesDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LetterTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LetterTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  LetterType

        #region SpecificLevel

        [HttpGet]
        public HttpResponseMessage GetSpecificLevels(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SpecificLevelDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISpecificLevelBL specificLevelBL = IoC.Resolve<ISpecificLevelBL>();
                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);
                    IList<SpecificLevel> specificLevels = specificLevelBL.GetSpecificLevels(transactionCategories, cultureName).ToList();
                    List<SpecificLevelDTO> specificLevelDTOs = SpecificLevelMapper.Map(specificLevels, cultureName);
                    getResult = GetResult<List<SpecificLevelDTO>>.Create(statusCode, specificLevelDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SpecificLevelDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SpecificLevelDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  SpecificLevel

        #region AttachmentType

        [HttpGet]
        public HttpResponseMessage GetAttachmentTypes(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AttachmentTypeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IAttachmentTypeBL attachmentTypeBL = IoC.Resolve<IAttachmentTypeBL>();
                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);
                    IList<AttachmentType> attachmentTypes = attachmentTypeBL.GetAttachmentTypes(transactionCategories, cultureName).ToList();
                    List<AttachmentTypeDTO> attachmentTypesDTOs = AttachmentTypeMapper.Map(attachmentTypes, cultureName);

                    getResult = GetResult<List<AttachmentTypeDTO>>.Create(statusCode, attachmentTypesDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AttachmentTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AttachmentTypeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        #endregion  AttachmentType

        [HttpGet]
        public HttpResponseMessage GetConfidentialityAcknowledgments(TransactionCategory transactionCategory, string cultureName)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ConfidentialityAcknowledgmentsDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IConfidentialityAcknowledgmentsBL confidentialityAcknowledgmentsBL = IoC.Resolve<IConfidentialityAcknowledgmentsBL>();
                    TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory(transactionCategory);
                    IList<ConfidentialityAcknowledgment> confidentialityAcknowledgments = confidentialityAcknowledgmentsBL.GetConfidentialityAcknowledgments(transactionCategories, cultureName).ToList();
                    List<ConfidentialityAcknowledgmentsDTO> confidentialityAcknowledgmentsDTOs = ConfidentialityAcknowledgmentsMapper.Map(confidentialityAcknowledgments, cultureName);

                    getResult = GetResult<List<ConfidentialityAcknowledgmentsDTO>>.Create(statusCode, confidentialityAcknowledgmentsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ConfidentialityAcknowledgmentsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ConfidentialityAcknowledgmentsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        #region OrgUnit

        [HttpGet]
        public HttpResponseMessage CheckOrgUnitHasAssignmentPaper(int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    bool hasAssignmentPaper = orgUnitBL.CheckOrgUnitHasAssignmentPaper(orgUnitId);

                    getResult = GetResult<bool>.Create(statusCode, hasAssignmentPaper, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage CheckOrgUnitIsAllowedToCreateGroup(int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    bool isAllowed = orgUnitBL.CheckOrgUnitIsAllowedToCreateGroup(orgUnitId);

                    getResult = GetResult<bool>.Create(statusCode, isAllowed, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitLinks(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitLinks(orgUnitId, cultureName);

                    List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetAllOrgUnitsId(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<int>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    List<int> orgUnits = orgUnitBL.GetAllOrgUnitsId(cultureName);



                    getResult = GetResult<List<int>>.Create(statusCode, orgUnits, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<int>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<int>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetOrgUnitsManagers(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<UserProfile> managers = orgUnitBL.GetOrgUnitsManagers(cultureName);

                    List<UserProfileDTO> userProfileDTOs = UserProfileMapper.Map(managers);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, userProfileDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitActions(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ActionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<Domain.Action> processes = orgUnitBL.GetOrgUnitActions(orgUnitId, cultureName);

                    List<ActionDTO> actionDTOs = ActionMapper.Map(processes);

                    getResult = GetResult<List<ActionDTO>>.Create(statusCode, actionDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ActionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ActionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitBeneficiaries(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionAssignmentDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<AssignmentPaperBeneficiary> assignmentPaperBeneficiaries = orgUnitBL.GetOrgUnitBeneficiaries(orgUnitId, cultureName);

                    List<TransactionAssignmentDTO> transactionAssignmentDTOs = TransactionAssignmentMapper.Map(assignmentPaperBeneficiaries);

                    getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, transactionAssignmentDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionAssignmentDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        private void AddLinksNodes(List<OrgUnit> orgUnits, OrgUnit orgUnit)
        {
            if (orgUnits != null && orgUnit != null)
            {
                foreach (OrgUnitLink orgUnitLink in orgUnit.Links)
                {
                    if (!orgUnits.Contains(orgUnitLink.ToEntity))
                    {
                        if (!orgUnitLink.ToEntity.IsVirtualUnit || orgUnitLink.ToEntity.Parent == null)
                        {
                            orgUnits.Add(orgUnitLink.ToEntity);
                        }

                        AddParentNode(orgUnits, orgUnitLink.ToEntity);
                    }
                }
            }
        }

        private void AddParentNode(List<OrgUnit> orgUnits, OrgUnit orgUnit)
        {
            if (!orgUnits.Contains(orgUnit))
            {
                if (!orgUnit.IsVirtualUnit || orgUnit.Parent == null)
                {
                    orgUnits.Add(orgUnit);
                }
            }

            if (orgUnit.Parent != null)
            {
                AddParentNode(orgUnits, orgUnit.Parent);
            }
        }

        #endregion  OrgUnit

        #region LogTransactionAction

        [HttpPost]
        public HttpResponseMessage LogTransactionAction(AuditingActionCode auditingActionCode, int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        LogAction(auditingActionCode, transactionId);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        #endregion LogTransactionAction

        #region Editor

        [HttpGet]
        public HttpResponseMessage GetInboundTransaction(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        Transaction transaction = editorBL.GetInboundTransaction(transactionId, cultureName);

                        TransactionDTO transactionDTO = TransactionMapper.Map(transaction);

                        getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostTransactionDraft(int transactionId, TransactionDTO transactionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostObjectResult<TransactionDetailsDTO> postObjectResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        TransactionDetails transactionDetails = null;

                        TransactionDetailsDTO transactionDetailsDTO = null;

                        Transaction transaction = TransactionMapper.Map(transactionDTO);

                        byte[] mainDocumentContent = null;
                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            transaction.MainDocument.Document.Content = null;

                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transaction.MainDocument.Name,
                                DocID = transaction.MainDocument.Id.ToString(),
                                PersonId = transaction.CreatedBy,
                                MimeContent = transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transaction.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        transactionDetails = editorBL.AddTransactionDraft(transactionId, transaction);

                        transactionDetailsDTO = TransactionDetailsMapper.Map(transactionDetails);

                        postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postObjectResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postObjectResult = PostObjectResult<TransactionDetailsDTO>.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postObjectResult);
            }

        }

        [HttpPost]
        public HttpResponseMessage PutTransaction(TransactionDTO transactionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            string cultureName = string.Empty;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(transactionDTO.TransactionCategory);

                        IEditorBL editorBL = new EditorBL();

                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();

                        User LoggedInUser = (User)UserContext.LoggedInUser;


                        Transaction transaction = TransactionMapper.Map(transactionDTO);

                        byte[] mainDocumentContent = null;
                        string mimeType = "";


                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            mimeType = transaction.MainDocument.MimeType;
                            transaction.MainDocument.Document.Content = null;
                        }

                        IList<TransactionAssignmentHistory> transactionHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistoryByTransactionId(transaction.Id);


                        foreach (TransactionCopy transactionCopy in transaction.Copies)
                        {
                            if (transactionHistories.Count == 0)
                            {
                                transactionCopy.IsSent = 0;
                                transactionCopy.SentDate = null;
                            }
                            else if (transaction.TransactionCategoryId != Common.TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                            {
                                transactionCopy.IsSent = 1;
                                transactionCopy.SentDate = DateTime.Now;
                            }
                        }
                        editorBL.UpdateTransaction(transaction);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        public HttpResponseMessage PutTransactionBasicInfo(int transactionId, TransactionBasicInfoDTO transactionBasicInfoDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        TransactionBasicInfo transactionBasicInfo = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO);

                        editorBL.UpdateTransactionBasicInfo(transactionId, transactionBasicInfo);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        public HttpResponseMessage PostAssignTransaction(int transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs, string followUp = "false")
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                        IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                        transactionAssignments.ToList().ForEach(a => a.TransactionId = transactionId);

                        editorBL.AssignTransaction(transactionId, transactionAssignments, Language);

                        Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                        if (followUp.ToLower() == bool.TrueString.ToLower())
                        {
                            TransactionFollowUpDTO oTransactionFollowUpDTO = new TransactionFollowUpDTO
                            {
                                TransactionId = transaction.Id,
                                CreatedBy = transaction.CreatedBy.Value,
                                CreatingUserId = transaction.CreatedBy.Value,
                                CreatingEntityId = transaction.OrgUnitId,
                                DateTo = null,
                                DateToH = null
                            };
                            transactionBL.TransactionFollowUpAdd(TransactionFollowUpMapper.Map(oTransactionFollowUpDTO), Language);
                        }

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


        [HttpPost]
        public HttpResponseMessage VIPPostAssignTransaction(int transactionId, List<VIPTransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                        IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                        transactionAssignments.ToList().ForEach(a => a.TransactionId = transactionId);

                        editorBL.AssignTransaction(transactionId, transactionAssignments, Language);

                        Transaction transaction = TransactionBL.GetTransactionById(transactionId);


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



        [HttpPost]
        public HttpResponseMessage PostAssignTransactionWithdrawal(int transactionId, List<TransactionAssignmentDTO> transactionAssignmentDTOs, string followUp = "false")
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                        IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                        transactionAssignments.ToList().ForEach(a => a.TransactionId = transactionId);

                        editorBL.AssignTransactionWithdrawal(transactionId, transactionAssignments, Language);

                        //Transaction transaction = TransactionBL.GetTransactionById(transactionId);

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


        [HttpPost]
        public HttpResponseMessage GetDeliveryReportByTransactionAllIds(int transactionId, int type)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)type.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                        var result = transactionBL.GetTransactionDeliveryReportByTransactionId(transactionId,true);

                        putResult = PostResult.Create(statusCode, result);

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

        [HttpPost]
        public HttpResponseMessage GetDeliveryReportByTransIds(List<int> transactionIds)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDeliveryReportDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);

                    var result = transactionBL.GetTransactionDeliveryReportByTransactionIds(transactionIds);

                    getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, TransactionDeliveryReportMapper.MapLight(result), null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDeliveryReportDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostAssignTransactions(string sTransactionsIds, List<TransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;
            List<int> transactionsIds = sTransactionsIds.Split(',').Select(int.Parse).ToList();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();


                        //IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                        transactionsIds.ForEach(x =>
                        {
                            IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);

                            transactionAssignments.ToList().ForEach(a => a.TransactionId = x);
                            editorBL.AssignTransaction(x, transactionAssignments, Language);
                        });

                        putResult = PutResult.Create(statusCode);
                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

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
        public HttpResponseMessage CheckUserHasPermission(string sTransactionsIds, List<TransactionAssignmentDTO> transactionAssignmentDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            List<int> transactionsIds = sTransactionsIds.Split(',').Select(int.Parse).ToList();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        IList<TransactionAssignment> transactionAssignments = TransactionAssignmentMapper.Map(transactionAssignmentDTOs);
                        bool isValid = editorBL.CheckTransactionForAssigne(transactionsIds, transactionAssignments);

                        if (!isValid)
                        {
                            statusCode = Common.StatusCode.NotSupported;
                        }
                        postResult = PostResult.Create(statusCode, null);


                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage AddAssignmentCopies(int transactionId, List<TransactionCopyDTO> transactionCopyDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        List<TransactionCopy> transactionCopies = TransactionCopyMapper.Map(transactionCopyDTOs);

                        IEditorBL editorBL = new EditorBL();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();
                        bool SendSpecialCopy = false;
                        int? OrgUnit = transactionCopies.FirstOrDefault()?.FromEntityId;
                        if (OrgUnit.HasValue && orgunitBL.CheckIfOrgunitSendSpecialCopy(OrgUnit.Value))
                            SendSpecialCopy = true;
                        if (transactionCopies != null && transactionCopies.Count > 0)
                        {
                            foreach (TransactionCopy transactionCopy in transactionCopies)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 1;
                                transactionCopy.SentDate = DateTime.Now;
                            }
                            editorBL.AddAssignmentCopies(transactionId, transactionCopies);
                        }


                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage AddTransactionCopies(int transactionId, List<TransactionCopyDTO> transactionCopyDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        List<TransactionCopy> transactionCopies = TransactionCopyMapper.Map(transactionCopyDTOs);

                        IEditorBL editorBL = new EditorBL();
                        IOrgUnitBL orgunitBL = new OrgUnitBL();
                        bool SendSpecialCopy = false;
                        int? OrgUnit = transactionCopies.FirstOrDefault()?.FromEntityId;
                        if (orgunitBL.CheckIfOrgunitSendSpecialCopy(OrgUnit.Value))
                            SendSpecialCopy = true;

                        foreach (TransactionCopy transactionCopy in transactionCopies)
                        {
                            if (transactionCopy.Id == 0)
                            {
                                transactionCopy.SpecialCopy = SendSpecialCopy;
                                transactionCopy.IsSent = 1;
                                transactionCopy.SentDate = DateTime.Now;
                                transactionCopy.Date = DateTime.Now;
                                transactionCopy.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                            }


                        }
                        editorBL.AddTransactionCopies(transactionId, transactionCopies);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetTransactionCopiesByTransactionId(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionCopyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        IList<TransactionCopy> transactionCopies = editorBL.GetTransactionCopiesByTransactionId(transactionId, cultureName);

                        List<TransactionCopyDTO> transactionDTOs = TransactionCopyMapper.Map(transactionCopies);

                        getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, transactionDTOs, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionCopyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddTransactionExplanation(int transactionId, ExplanationDTO explanationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        Explanation explanation = ExplanationMapper.Map(explanationDTO);

                        IEditorBL editorBL = new EditorBL();

                        if (explanation.ExplanationEditorType != (int)EditorType.Text && explanation.Document != null)
                        {
                            byte[] documentContent = null;

                            if (explanation.Document.Document.Content != null)
                            {
                                documentContent = explanation.Document.Document.Content;
                                explanation.Document.Document.Content = null;
                            }

                            int id = editorBL.AddTransactionExplanation(transactionId, explanation, Language);

                            if (explanation.Document != null && explanation.Document.Document != null)
                            {
                                DocData docData = new DocData()
                                {
                                    Data = documentContent,
                                    DocName = explanation.Document.Name,
                                    DocID = explanation.Document.Id.ToString(),
                                    PersonId = explanation.Document.CreatedBy,
                                    MimeContent = explanation.Document.MimeType,
                                    DataSize = Convert.ToInt32(explanation.Document.Size),
                                    User_ID = explanation.Document.CreatedBy.ToString(),
                                    TransactionId = transactionId
                                };

                                DocRepository.DocRepository.Save(docData, new DocumentLocation());
                            }

                            postResult = PostResult.Create(statusCode, id);

                            transactionContextScope.Commit();

                            return Request.CreateResponse(HttpStatusCode.Created, postResult);
                        }
                        else
                        {
                            int id = editorBL.AddTransactionExplanation(transactionId, explanation, Language);

                            postResult = PostResult.Create(statusCode, id);

                            transactionContextScope.Commit();

                            return Request.CreateResponse(HttpStatusCode.Created, postResult);
                        }

                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage UpdateExplanation(ExplanationDTO explanationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        Explanation explanation = ExplanationMapper.Map(explanationDTO);
                        if (explanation.ExplanationEditorType != (int)EditorType.Text)
                        {
                            byte[] documentContent = null;

                            if (explanation.Document.Document.Content != null)
                            {
                                documentContent = explanation.Document.Document.Content;
                                explanation.Document.Document.Content = null;
                            }

                            IEditorBL editorBL = new EditorBL();

                            editorBL.UpdateExplanation(explanation);

                            if (explanation.Document != null && explanation.Document.Document != null)
                            {
                                DocData docData = new DocData()
                                {
                                    Data = documentContent,
                                    DocName = explanation.Document.Name,
                                    DocID = explanation.Document.Id.ToString(),
                                    PersonId = explanation.Document.CreatedBy,
                                    MimeContent = explanation.Document.MimeType,
                                    DataSize = Convert.ToInt32(explanation.Document.Size),
                                    User_ID = explanation.Document.CreatedBy.ToString(),
                                    TransactionId = explanation.TransactionId
                                };

                                DocRepository.DocRepository.Save(docData, new DocumentLocation());
                            }


                            putResult = PostResult.Create(statusCode, null);

                            transactionContextScope.Commit();

                            return Request.CreateResponse(HttpStatusCode.Created, putResult);
                        }
                        else
                        {
                            IEditorBL editorBL = new EditorBL();

                            editorBL.UpdateExplanation(explanation);

                            putResult = PostResult.Create(statusCode, null);

                            transactionContextScope.Commit();

                            return Request.CreateResponse(HttpStatusCode.Created, putResult);
                        }

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
        public HttpResponseMessage GetTransactionExplanations(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExplanationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        IList<Explanation> explanations = editorBL.GetExplanationsByTransactionId(transactionId, cultureName);

                        List<ExplanationDTO> explanationDTOs = ExplanationMapper.Map(explanations);

                        if (explanationDTOs != null)
                        {
                            foreach (ExplanationDTO explanation in explanationDTOs)
                            {
                                if (explanation.EditorType != EditorType.Text)
                                {
                                    if (explanation.DocumentDTO != null)
                                    {
                                        DocData docData = DocRepository.DocRepository.Load(explanation.DocumentDTO.Id.ToString(), new DocumentLocation());
                                        explanation.DocumentDTO.Content = docData.Data;
                                    }
                                }
                            }
                        }


                        getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, explanationDTOs, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionExplanations_New(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExplanationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        IList<Explanation> explanations = editorBL.GetExplanationsByTransactionId_New(transactionId, cultureName);

                        List<ExplanationDTO> explanationDTOs = ExplanationMapper.Map(explanations);



                        getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, explanationDTOs, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExplanationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExplanationById(int explanationId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ExplanationDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        Explanation explanation = editorBL.GetExplanationById(explanationId, cultureName);

                        ExplanationDTO explanationDTO = ExplanationMapper.Map(explanation);

                        if (explanation.ExplanationEditorType != (int)EditorType.Text)
                        {
                            explanationDTO.DocumentDTO.Content = DocRepository.DocRepository.Load(explanationDTO.DocumentDTO.Id.ToString(), new DocumentLocation()).Data;
                        }

                        getResult = GetResult<ExplanationDTO>.Create(statusCode, explanationDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<ExplanationDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ExplanationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ExplanationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetExplanationByDocumentId(string cultureName, int DocumentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ExplanationDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        Explanation explanation = editorBL.GetExplanationByDocumentId(DocumentId, cultureName);

                        ExplanationDTO explanationDTO = ExplanationMapper.Map(explanation);

                        if (explanation.ExplanationEditorType != (int)EditorType.Text)
                        {
                            explanationDTO.DocumentDTO.Content = DocRepository.DocRepository.Load(explanationDTO.DocumentDTO.Id.ToString(), new DocumentLocation()).Data;
                        }

                        getResult = GetResult<ExplanationDTO>.Create(statusCode, explanationDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<ExplanationDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ExplanationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ExplanationDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAttachmentById(int attachmentId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionAttachmentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        Attachment attachment = editorBL.GetAttachmentById(attachmentId, cultureName);

                        TransactionAttachmentDTO transactionAttachmentDTO = TransactionAttachmentMapper.Map(attachment);

                        getResult = GetResult<TransactionAttachmentDTO>.Create(statusCode, transactionAttachmentDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<TransactionAttachmentDTO>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionAttachmentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionAttachmentDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DeleteExplanation(int explanationId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IEditorBL editorBL = new EditorBL();

                    editorBL.DeleteExplanation(explanationId);

                    postResult = PostResult.Create(statusCode, null);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetMainDocument(int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        DocumentInfo documentInfo = editorBL.GetMainDocumentByTransactionId(transactionId);

                        if (documentInfo != null && documentInfo.Document != null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(documentInfo.Id.ToString(), new DocumentLocation());
                            documentInfo.Document.Content = docData.Data;
                        }

                        DocumentDTO documentDTO = DocumentMapper.MapWithContent(documentInfo);

                        getResult = GetResult<DocumentDTO>.Create(statusCode, documentDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    getResult = GetResult<DocumentDTO>.Create(statusCode, null, null);

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

        [HttpPut]
        public HttpResponseMessage UpdateMainDocument(int transactionId, DocumentDTO documentDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        DocumentInfo documentInfo = DocumentMapper.Map(documentDTO);

                        IEditorBL editorBL = new EditorBL();

                        byte[] mainDocumentContent = null;
                        if (documentInfo != null && documentInfo.Document != null)
                        {
                            mainDocumentContent = documentInfo.Document.Content;

                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = documentInfo.Name,
                                DocID = documentInfo.Document.Id.ToString(),
                                PersonId = documentInfo.CreatedBy,
                                MimeContent = documentInfo.MimeType,
                                DataSize = Convert.ToInt32(documentInfo.Size),
                                User_ID = documentInfo.CreatedBy.ToString(),
                                TransactionId = transactionId,

                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        editorBL.UpdateTransactionDocument(transactionId, documentInfo);

                        postResult = PostResult.Create(statusCode, transactionId);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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

        [HttpPut]
        public HttpResponseMessage UpdateMainDocument_New(int transactionId, byte[] content)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {


                        IDocumentBL documentBL = new DocumentBL();

                        documentBL.UpdateDocumentContentByTransaction(transactionId, content);

                        postResult = PostResult.Create(statusCode, transactionId);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage AddTransactionLinks(int transactionId, List<TransactionLinkDTO> transactionLinkDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();

                        List<TransactionLink> transactionLinks = TransactionLinkMapper.Map(transactionLinkDTOs, TransactionCategory.DraftOutbound);

                        editorBL.AddTransactionLinks(transactionId, transactionLinks);

                        postResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage UpdateTransactionSubject(EditSubjectTransactionDTO editSubjectTransactionDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {

                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                        transactionBL.UpdateTransactionSubject(editSubjectTransactionDTO);
                        postResult = PostResult.Create(statusCode, null);
                        transactionContextScope.Commit();
                        return Request.CreateResponse(HttpStatusCode.OK, postResult);
                    }
                    statusCode = Common.StatusCode.ModelNotValid;
                    postResult = PostResult.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetTransactionBasicInfo(int transactionId, string cultureName, int? transactionCopyId = null)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionBasicInfoDTO> getResult = null;

            try
            {

                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        TransactionBasicInfo transactionBasicinfo = editorBL.GetTransactionBasicInfo(transactionId, cultureName);
                        TransactionBasicInfoDTO transactionBasicinfoDTO = TransactionBasicInfoMapper.Map(transactionBasicinfo);
                        if (transactionCopyId.HasValue && transactionCopyId.Value > 0)
                        {
                            SetViewdTransactionCopy(transactionCopyId.Value);
                        }

                        LogAction(AuditingActionCode.ViewTransactionCopies, transactionId);
                        getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, transactionBasicinfoDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetExternalCopyAttachment(int transactionId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyAttachmentDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {

                        Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                        ITransactionBL editorBL = TransactionBL.Create((Common.TransactionCategory)transaction.TransactionCategory.Id);
                        IList<ExternalPartyAttachment> externalPartyAttachments = editorBL.GetExternalPartiesAttach(transactionId, orgUnitId, cultureName);
                        List<ExternalPartyAttachmentDTO> transactionDTO = ExternalPartyAttachmentMapper.Map(externalPartyAttachments);
                        getResult = GetResult<List<ExternalPartyAttachmentDTO>>.Create(statusCode, transactionDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<ExternalPartyAttachmentDTO>>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyAttachmentDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyAttachmentDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransaction(int transactionId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = IoC.Resolve<IEditorBL>();
                        Transaction transaction = editorBL.GetTransaction(transactionId, orgUnitId, cultureName);

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            if (transaction.MainDocument.Document.Content == null)
                            {
                                DocData docData = DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation());
                                transaction.MainDocument.Document.Content = docData.Data;
                            }
                        }

                        if (transaction?.OldWordDocumnt?.Document != null && transaction?.OldWordDocumnt?.Document?.Content == null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(transaction.OldWordDocumnt.Id.ToString(), new DocumentLocation());
                            transaction.OldWordDocumnt.Document.Content = docData.Data;
                        }


                        TransactionDTO transactionDTO = TransactionMapper.Map(transaction);


                        LogAction(AuditingActionCode.ViewTransaction, transactionId);
                        getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionLight(int transactionId, int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = IoC.Resolve<IEditorBL>();
                        Transaction transaction = editorBL.GetTransactionLight(transactionId, orgUnitId, cultureName);
                        TransactionDTO transactionDTO = TransactionMapper.MapLight(transaction);
                        getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);
                        LogAction(AuditingActionCode.ViewTransaction, transactionId);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }
                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionAuditing(int userId, int orgUnitId, int transactionId, string EntityName, string cultureName, AuditFor auditFor, [FromUri] SearchCriteriaCustom SearchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AuditDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        int itemsCount = 0;
                        List<MainAudit> audits = TransactionBL.GetAuditByEntityName(userId, orgUnitId, transactionId, EntityName, cultureName, auditFor, false, out itemsCount, SearchCriteria);
                        List<AuditDTO> auditDTO = AuditMapper.Map(audits);

                        getResult = GetResult<List<AuditDTO>>.Create(statusCode, auditDTO, itemsCount);
                        LogAction(AuditingActionCode.ViewTransaction, transactionId);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<AuditDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AuditDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AuditDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionAuditingForPrint(int userId, int orgUnitId, int transactionId, string EntityName, string cultureName, AuditFor auditFor, [FromUri] SearchCriteriaCustom SearchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AuditDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        int itemsCount = 0;
                        List<MainAudit> audits = TransactionBL.GetAuditByEntityName(userId, orgUnitId, transactionId, EntityName, cultureName, auditFor, true, out itemsCount, SearchCriteria);
                        List<AuditDTO> auditDTO = AuditMapper.Map(audits);
                        foreach (var item in auditDTO)
                        {
                            item.AuditDetails = AuditMapper.Map(TransactionBL.GetEntityAuditing(auditFor, item.Id, "", cultureName));
                        }

                        getResult = GetResult<List<AuditDTO>>.Create(statusCode, auditDTO, itemsCount);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<AuditDTO>>.Create(statusCode, null, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AuditDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AuditDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionLogInfo(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionLogInfoDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                        List<TransactionLogInfo> TransactionLogInfos = transactionLoggingBL.GetTransactionLogInfo(transactionId, cultureName).ToList();
                        List<TransactionLogInfoDTO> transactionLogInfoDTOs = TransactionLogInfoMapper.Map(TransactionLogInfos);
                        getResult = GetResult<List<TransactionLogInfoDTO>>.Create(statusCode, transactionLogInfoDTOs, null);
                        LogAction(AuditingActionCode.ViewTransaction, transactionId);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<TransactionLogInfoDTO>>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionLogInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionLogInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionLogDetailsInfo(int transactionId, string cultureName, bool IsForPrint, [FromUri] SearchCriteriaCustom searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionLogDetailInfoDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                        List<TransactionLogDetailInfo> transactionLogDetailInfos = transactionLoggingBL.GetTransactionLogDetailsInfo(transactionId, cultureName, IsForPrint, searchCriteria, out int itemsCount).ToList();
                        List<TransactionLogDetailInfoDTO> transactionLogDetailInfoDTOs = TransactionLogInfoMapper.Map(transactionLogDetailInfos);
                        getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, transactionLogDetailInfoDTOs, itemsCount);
                        LogAction(AuditingActionCode.ViewTransaction, transactionId);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionLogDetailsInfo(int transactionId, int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionLogDetailInfoDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                        List<TransactionLogDetailInfo> transactionLogDetailInfos = transactionLoggingBL.GetTransactionLogDetailsInfo(transactionId, userId, cultureName).ToList();
                        List<TransactionLogDetailInfoDTO> transactionLogDetailInfoDTOs = TransactionLogInfoMapper.Map(transactionLogDetailInfos);
                        LogAction(AuditingActionCode.ViewTransaction, transactionId);
                        getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, transactionLogDetailInfoDTOs, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionLogDetailInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetEntityAuditing(AuditFor auditFor, int auditId, string PropName, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AuditDetailDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        List<AuditDetails> audits = TransactionBL.GetEntityAuditing(auditFor, auditId, PropName, cultureName);
                        List<AuditDetailDTO> auditDetailDTOs = AuditMapper.Map(audits);

                        getResult = GetResult<List<AuditDetailDTO>>.Create(statusCode, auditDetailDTOs, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }
                }

                statusCode = Common.StatusCode.ModelNotValid;
                getResult = GetResult<List<AuditDetailDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AuditDetailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AuditDetailDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionByCopy(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        Transaction transaction = TransactionBL.GetTransactionById(transactionId, cultureName);

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            if (transaction.MainDocument.Document.Content == null)
                            {
                                DocData docData = DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation());
                                transaction.MainDocument.Document.Content = docData.Data;
                            }
                        }

                        TransactionDTO transactionDTO = TransactionMapper.Map(transaction);
                        getResult = GetResult<TransactionDTO>.Create(statusCode, transactionDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetTransactionLinks(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionLinkDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        IList<TransactionLink> transactionLinks = editorBL.GetTransactionLinks(transactionId, cultureName);
                        List<TransactionLinkDTO> transactionLinkDTOs = TransactionLinkMapper.Map(transactionLinks);


                        foreach (var transactionLinkDTO in transactionLinkDTOs)
                        {
                            TransactionBasicInfo transactionBasicInfo = editorBL.GetTransactionBasicInfo(transactionLinkDTO.TransactionId, cultureName);
                            transactionLinkDTO.Subject = transactionBasicInfo.Subject;
                            transactionLinkDTO.DateH = transactionBasicInfo.DateH;
                            transactionLinkDTO.TransactionType = transactionBasicInfo.TransactionTypeName;
                            transactionLinkDTO.TransactionCategory = transactionBasicInfo.TransactionCategoryId;
                        }
                        getResult = GetResult<List<TransactionLinkDTO>>.Create(statusCode, transactionLinkDTOs, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<List<TransactionLinkDTO>>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionLinkDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionLinkDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage UpdateAssignmentPaper(AssignmentPaperDTO assignmentPaperDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        AssignmentPaper assignmentPaper = AssignmentPaperMapper.Map(assignmentPaperDTO);
                        editorBL.UpdateAssignmentPaper(assignmentPaper);
                        putResult = PutResult.Create(statusCode);
                        transactionContextScope.Commit();
                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    putResult = PutResult.Create(statusCode);
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

        [HttpGet]
        public HttpResponseMessage GetAssignmentPaperByOrgUnitId(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<AssignmentPaperDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        AssignmentPaper assignmentPaper = editorBL.GetAssignmentPaperByOrgUnitId(orgUnitId, cultureName);
                        AssignmentPaperDTO assignmentPaperDTO = AssignmentPaperMapper.Map(assignmentPaper);
                        getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, assignmentPaperDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<AssignmentPaperDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PutTransactionDeliveryNumber(TransactionEditDTO transactionEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(transactionEditDTO.TransactionCategory);

                        var transaction = TransactionMapper.Map(transactionEditDTO);

                        transactionBL.SaveTransactionDeliveryNumber(transaction);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.Created, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    putResult = PutResult.Create(statusCode);
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

        [HttpPut]
        public HttpResponseMessage UpdateTransactionStatus(int transactionId, int statusId, int type)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)type.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));
                        transactionBL.UpdateTransactionStatus(transactionId, statusId);
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
        public HttpResponseMessage UpdateTransactionDelivary(int transactionId, int DeliveryMethodId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));
                        transactionBL.UpdateTransactionDelivary(transactionId, DeliveryMethodId);

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
        public HttpResponseMessage UpdateTransactionAssignmentHistory(int transactionId, int ExplanationId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                        transactionAssignmentHistoryBL.UpdateTransactionAssignmentHistory(transactionId, ExplanationId);

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
        public HttpResponseMessage UpdateTransactionsDelivary(string transactionIds, int DeliveryMethodId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;
            List<int> transactionsId = transactionIds.Split(',').Select(int.Parse).ToList();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        transactionsId.ForEach(x =>
                        {
                            Transaction transaction = TransactionBL.GetTransactionById(x);
                            ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));
                            transactionBL.UpdateTransactionDelivary(x, DeliveryMethodId);
                        });


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

        [HttpPost]
        public HttpResponseMessage VerifyTransactionNumberOrBarcode(TransactionLightDTO transactionLightDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> putResult = null;
            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    ITransactionBL transactionBL = TransactionBL.Create(transactionLightDTO.TransactionCategory);
                    var result = transactionBL.IsMatchNumberOrBarcode(transactionLightDTO.Id, transactionLightDTO.Number, transactionLightDTO.Barcode, transactionLightDTO.UserId, transactionLightDTO.EntityId);
                    putResult = GetResult<bool>.Create(statusCode, result, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                putResult = GetResult<bool>.Create(statusCode, false, null);
                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                putResult = GetResult<bool>.Create(statusCode, false, null);
                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionBasicInfoByNumber(int transactionNumber, int year, int transactionType, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionBasicInfoDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        IEditorBL editorBL = new EditorBL();
                        ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                        TransactionBasicInfo transactionBasicinfo = editorBL.GetTransactionBasicInfoByNumber(transactionNumber, year, transactionType, cultureName);

                        TransactionBasicInfoDTO transactionBasicinfoDTO = TransactionBasicInfoMapper.Map(transactionBasicinfo);

                        if (transactionBasicinfo != null)
                        {
                            int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, cultureName);
                            IList<TransactionAssignment> transactionAssignments = transactionAssignmentBL.GetTransactionAssignments(a => a.TransactionId == transactionBasicinfo.Id && a.Action.Type.Id != SendCopyToView, cultureName);

                            if (transactionAssignments.Any())
                            {
                                transactionBasicinfoDTO.CurrentOrgUnit = transactionAssignments.FirstOrDefault().ToEntity != null ? transactionAssignments.FirstOrDefault().ToEntity.LocalName : string.Empty;
                            }
                        }

                        getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, transactionBasicinfoDTO, null);
                        return Request.CreateResponse(HttpStatusCode.Created, getResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, null, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionBasicInfoDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        #endregion Editor

        #region SubjectClassification
        [HttpGet]
        public HttpResponseMessage GetSubjectClassificationsByOrgUnitId(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SubjectClassificationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISubjectClassificationBL subjectClassificationBL = IoC.Resolve<ISubjectClassificationBL>();
                    IList<SubjectClassification> subjectClassifications = subjectClassificationBL.GetSubjectClassificationByOrgUnitId(orgUnitId, cultureName);
                    List<SubjectClassificationDTO> subjectClassificationDTOs = SubjectClassificationMapper.Map(subjectClassifications);
                    getResult = GetResult<List<SubjectClassificationDTO>>.Create(statusCode, subjectClassificationDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<SubjectClassificationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<SubjectClassificationDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        #endregion SubjectClassification

        #region SuggestedTopic
        [HttpGet]
        public HttpResponseMessage GetSuggestedTopicsByOrgUnitId(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SuggestedTopicDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISuggestedTopicBL subjectClassificationBL = IoC.Resolve<ISuggestedTopicBL>();
                    IList<SuggestedTopic> suggestedTopics = subjectClassificationBL.GetSuggestedTopicsByOrgUnitId(orgUnitId, cultureName);
                    List<SuggestedTopicDTO> suggestedTopicDTOs = SuggestedTopicMapper.Map(suggestedTopics);
                    getResult = GetResult<List<SuggestedTopicDTO>>.Create(statusCode, suggestedTopicDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<SuggestedTopicDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<SuggestedTopicDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion SuggestedTopic

        [HttpGet]
        public HttpResponseMessage LastTransactionAssignments(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionAssignmentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                    TransactionAssignment transactionAssignment = transactionAssignmentBL.GetLastTransactionAssignments(transactionId, cultureName);
                    TransactionAssignmentDTO transactionAssignmentDTO = TransactionAssignmentMapper.Map(transactionAssignment);
                    getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, transactionAssignmentDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionAssignmentDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #region Reservation
        [HttpPost]
        public HttpResponseMessage PostTransactionReservation(TransactionReservationDTO transactionReservationDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transactionReservationDTO.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                        TransactionReservation transactionReservation = TransactionReservationMapper.Map(transactionReservationDTO);

                        transactionBL.SaveTransactionReservation(transactionReservation);
                        transactionContextScope.Commit();
                        postResult = PostResult.Create(statusCode, null);
                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    postResult = PostResult.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        public HttpResponseMessage GetTransactionReservations(int? orgUnitId, int? userId, [FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<TransactionReservationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    List<TransactionReservation> transactionReservations = TransactionBL.GetTransactionReservations(orgUnitId, userId, searchCriteria, out rowsCount);

                    List<TransactionReservationDTO> transactionReservationDTOs = TransactionReservationMapper.Map(transactionReservations, Language);

                    getResult = GetResult<List<TransactionReservationDTO>>.Create(statusCode, transactionReservationDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionReservationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionReservationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetReservedTransaction(int reservationId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;

            GetResult<List<TransactionReservedDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    List<Transaction> transactionReservations = TransactionBL.GetReservedTransaction(reservationId);

                    List<TransactionReservedDTO> transactionReservationDTOs = TransactionReservationMapper.Map(transactionReservations, Language);

                    getResult = GetResult<List<TransactionReservedDTO>>.Create(statusCode, transactionReservationDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.Created, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionReservedDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionReservedDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        #endregion

        [HttpGet]
        public HttpResponseMessage GetTransactionPathNextStep(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionPathDetailsDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                        TransactionPathDetails transactionPathDetails = transactionAssignmentBL.GetTransactionPathNextStep(transactionId, cultureName);
                        TransactionPathDetailsDTO transactionPathDetailsDTO = TransactionPathMapper.Map(transactionPathDetails);

                        getResult = GetResult<TransactionPathDetailsDTO>.Create(statusCode, transactionPathDetailsDTO, null);

                        return Request.CreateResponse(HttpStatusCode.Created, getResult);

                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionPathDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionPathDetailsDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionsByExternalPartyId(int externalPartyId, int orgUnitId)
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionDetailsDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();

                    List<Transaction> transactions = fileBL.GetTransactionsByExternalPartyId(externalPartyId, orgUnitId);

                    List<TransactionDetailsDTO> userTransactionsTrayDTOs = TransactionDetailsMapper.Map(transactions);

                    getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, userTransactionsTrayDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionDetailsDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        #region SystemDefaultValues

        [HttpGet]
        public HttpResponseMessage GetSystemDefaultValues()
        {
            int rowsCount = 0;
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SystemDefaultValuesDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISystemDefaultValuesBL systemDefaultValuesBL = IoC.Resolve<ISystemDefaultValuesBL>();
                    IList<SystemDefaultValues> systemDefaultValues = systemDefaultValuesBL.GetSystemDefaultValue().ToList();
                    List<SystemDefaultValuesDTO> systemDefaultValuesDTO = SystemDefaultValuesMapper.Map(systemDefaultValues);
                    getResult = GetResult<List<SystemDefaultValuesDTO>>.Create(statusCode, systemDefaultValuesDTO, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SystemDefaultValuesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SystemDefaultValuesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllTransactionDocuments(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionPrintDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    //Main Document
                    IEditorBL editorBL = new EditorBL();

                    DocumentInfo documentInfo = editorBL.GetMainDocumentByTransactionId(transactionId);

                    if (documentInfo != null && documentInfo.Document != null)
                    {
                        //if (documentInfo.Document.Content == null)
                        //{
                        DocData docData = DocRepository.DocRepository.Load(documentInfo.Id.ToString(), new DocumentLocation());
                        documentInfo.Document.Content = docData.Data;
                        //}
                    }

                    //Attachments

                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    List<Attachment> attachments = transactionLoggingBL.GetTransactionAttachments(transactionId, cultureName).ToList();


                    //Explanations

                    IList<Explanation> explanations = editorBL.GetExplanationsByTransactionId(transactionId, cultureName);

                    TransactionPrintDTO transactionPrintDTO = TransactionMapper.MapTransactionPrint(documentInfo, attachments, explanations);


                    getResult = GetResult<TransactionPrintDTO>.Create(statusCode, transactionPrintDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<TransactionPrintDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<TransactionPrintDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage DigitalSigning(int paperId)
        {
            bool errorHappend = false;
            string reason = string.Empty;
            try
            {
                DocData draftVersion = null;
                DocData docData = null;
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    DocumentInfo documentInfo = null;
                    IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
                    //if (!string.IsNullOrWhiteSpace(isMainDocument))
                    IEditorBL editorBL = new EditorBL();

                    string documentName = new Random().Next().ToString() + ".pdf";

                    documentInfo = documentBL.GetDocumentById(paperId);
                    docData = DocRepository.DocRepository.Load(paperId.ToString(), new DocumentLocation());

                    //System.IO.File.WriteAllBytes("c:\\Output1.pdf", documentInfo.Document.Content);



                    draftVersion = new DocData()
                    {
                        Data = docData.Data,
                        DocName = documentName,
                        DocID = docData.DocID,
                        // PersonId = docData.PersonId,
                        MimeContent = documentInfo.MimeType,
                        EntityId = docData.EntityId,
                        DataSize = docData.DataSize,
                        User_ID = docData.User_ID
                    };
                    if (/*documentInfo.IsDigitallySigned == false*/ true)
                    {
                        if (documentInfo.MimeType != "application/pdf")
                        {
                            string content = Encoding.UTF8.GetString(draftVersion.Data);
                            content = HttpUtility.HtmlDecode(content);
                            draftVersion.Data = PdfHelper.ConvertHtml2PDF(content);
                            draftVersion.MimeContent = "application/pdf";
                        }

                        User user = (User)UserContext.LoggedInUser;

                        IUserManagementBL userManagementBL = new UserManagementBL();
                        UserProfile signingUser = userManagementBL.GetUserById(user.Id);
                        // Variables
                        string Alias = "cer123";
                        string PrivateKeyPassword = "cer123";

                        string SignBy = signingUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == Language).FirstOrDefault().Text;
                        string Surname = signingUser.UserName;
                        string Reason = "I approve this document";
                        string Location = "KSA";
                        string ContactInfo = signingUser.Email;

                        string RequestIdentifier = "26594878-1001-0000-0000-" + DateTime.Now.ToString("yyyyMMddHHmmss");//"26594878-0000-0000-0000-000000000000"
                        string AdssURL = "https://stage-signservice.stcs.com.sa";
                        string ClientID = "MOJ_STAGE_OID";
                        string SigningProfile = "adss:signing:profile:009";
                        int RequestMode = 3;//http
                        string fieldName = "signature1";
                        int sigSiz = 80000;



                        string MutualAuthCertSerial = "xxxx";
                        int x1 = 300;
                        int y1 = 130;
                        int x2 = 570;
                        int y2 = 40;
                        int onPage = 1;

                        HashMechanism.Signature sig = null;
                        HashMechanism.SignatureAppearance appearance = null;
                        byte[] originalBytes = null;

                        HashMechanism.ExtractResponse signatureHash = null;

                        originalBytes = draftVersion.Data;
                        //originalBytes = File.ReadAllBytes("c:\\file.pdf");

                        UserPreferenceInfo userPreferenceInfo = userManagementBL.GetUserPreferenceByUserId(user.Id, Language);

                        //byte[] backImage = File.ReadAllBytes("c:\\HandSig.png"); //or null
                        byte[] backImage = userPreferenceInfo.Signature;
                        if (userPreferenceInfo.Signature == null)
                        {
                            errorHappend = true;
                            reason = "المستخدم ليس لديه توقيع";
                        }
                        else
                        {
                            HashMechanism.ExtractHash hashEx = new HashMechanism.ExtractHash();
                            appearance = new HashMechanism.SignatureAppearance(true, new rectangle(x1, y1, x2, y2), onPage, backImage);
                            sig = new HashMechanism.Signature(SignBy, Surname, Reason, Location, ContactInfo, fieldName, false, appearance);

                            signatureHash = hashEx.AddEmptySigAndCalculateHash(originalBytes, sig, sigSiz);

                            //----------------------------------------------------------------------------------------------------------------
                            // 2 - EmbedHash

                            string FileBytes = signatureHash.ReturnString;

                            CertificationClient client = new CertificationClient();
                            var result = SignHash(new SignHashRequest
                            {
                                AdssURL = AdssURL,
                                ClientID = ClientID,
                                SignBy = SignBy,
                                FileBytes = FileBytes,
                                SigningProfile = SigningProfile,
                                RequestMode = RequestMode,
                                MutualAuthentication = new MutualAuthentication
                                {
                                    EnableMutualAuth = false,
                                    MutualAuthCertSerial = MutualAuthCertSerial
                                },
                                RequestIdentifier = RequestIdentifier,
                                Alias = Alias,
                                PrivateKeyPassword = PrivateKeyPassword

                            });


                            if (result != null)
                            {
                                if (result is SignHashResponse)
                                {
                                    var response = result as SignHashResponse;
                                    byte[] dataSignedFileBytes = Convert.FromBase64String(response.SignedFileBytes);// _dataResponse["SignedFileBytes"];
                                    HashMechanism.EmbedSignature emHash = new HashMechanism.EmbedSignature();

                                    var y = emHash.EmbedHash(signatureHash.ReturnBytes, dataSignedFileBytes, sig);

                                    //before LTV PDF

                                    if (y.SignedBytes != null)
                                    {
                                        draftVersion.Data = y.SignedBytes;
                                        //using (var transactionContextScope = context.Create())

                                        //string content = Encoding.UTF8.GetString(draftVersion.Data);
                                        //content = HttpUtility.HtmlDecode(content);
                                        //draftVersion.Data = PdfHelper.ConvertHtml2PDF(content);
                                        //draftVersion.MimeContent = "application/pdf";
                                        //document.EditorType = EditorType.Scanning;

                                        // System.IO.File.WriteAllBytes("c:\\Output2.pdf", draftVersion.Data);
                                        draftVersion.MimeContent = "application/pdf";
                                        string documentId = DocRepository.DocRepository.Save(draftVersion, new DocumentLocation(), IsDigitallySigned: true);

                                        //if (discussion != null)
                                        //{
                                        //    discussion.EditorType = (int)EditorType.Scanning;
                                        //    discussion.PermissionId = discussion.Permission.Id;
                                        //    editorBL.UpdateDiscussion(discussion);

                                        //}

                                        // lbl_ErrVisibleSignPdfHash.Text = response.Message;
                                    }

                                    else
                                    {
                                        // lbl_ErrVisibleSignPdfHash.Text = y.getMessage();
                                    }


                                    // 3 - ADDLTV

                                    // string HashAlgorithm = "SHA256";
                                    // var pades = emHash.AddLtv(y.SignedBytes, sigSiz, HashAlgorithm, "https://stage-signservice.stcs.com.sa/adss/tsa", null, null, sig);
                                    // if (pades.PadesSignedBytes != null)
                                    // System.IO.File.WriteAllBytes("c:\\OutputTSA.pdf", pades.PadesSignedBytes);



                                }
                                else
                                {
                                    // lbl_ErrVisibleSignPdfHash.Text = result.ToString();
                                }
                            }
                            else
                            {
                                errorHappend = true;
                                reason = "حدث خطأ";
                            }
                            // Hash PDF
                            //----------------------------------------------------------------------------------------------------------------
                            // 1 - AddEmptySigAndCalculateHash
                        }
                    }
                    else
                    {
                        errorHappend = true;
                        reason = "الملف مصادق من قبل";
                    }
                    GetResult<dynamic> getResult = GetResult<dynamic>.Create(Common.StatusCode.Ok, new { errorHappend, Reason = reason }, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { errorHappend = true, Reason = "Error" });
                //throw ex;
                // lbl_ErrVisibleSignPdfHash.Text = ex.Message;
            }
        }

        [HttpGet]
        public HttpResponseMessage TransactionDirectReply(int transactionId, string remarks, int userId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            bool result = false;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IEditorBL editorBL = new EditorBL();
                    result = editorBL.TransactionDirectReply(transactionId, remarks, userId);
                    transactionContextScope.Commit();

                    getResult = GetResult<bool>.Create(statusCode, result, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, result, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, result, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetTransactionDetailsByTransactionId(int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDetailsDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                    TransactionDetailsDTO transactionDetailsDTO = TransactionDetailsMapper.Map(transaction);
                    getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetTransactionDetailsByTransactionId(int transactionId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<TransactionDetailsDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    Transaction transaction = TransactionBL.GetTransactionById(transactionId, cultureName);
                    TransactionDetailsDTO transactionDetailsDTO = TransactionDetailsMapper.Map(transaction);
                    getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, transactionDetailsDTO, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<TransactionDetailsDTO>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetMainDocumentByTransactionId(string transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEditorBL editorBL = new EditorBL();
                    DocumentInfo documentInfo = editorBL.GetMainDocumentByTransactionId(int.Parse(transactionId));

                    DocumentDTO documentDTO = DocumentMapper.Map(documentInfo);

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
        public HttpResponseMessage GetOldMainDocumentByTransactionId(string transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<DocumentDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IEditorBL editorBL = new EditorBL();
                    DocumentInfo documentInfo = editorBL.GetOldMainDocumentByTransactionId(int.Parse(transactionId));

                    if (documentInfo != null && documentInfo.Document != null)
                    {
                        //if (documentInfo.Document.Content == null)
                        //{
                        DocData docData = DocRepository.DocRepository.Load(documentInfo.Id.ToString(), new DocumentLocation());
                        documentInfo.Document.Content = docData.Data;
                        //}
                    }


                    DocumentDTO documentDTO = DocumentMapper.Map(documentInfo);

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


        public class CertificationClient
        {


            public class SignHashRequest
            {
                public string AdssURL { get; set; }
                public string ClientID { get; set; }
                public string Alias { get; set; }
                public string PrivateKeyPassword { get; set; }
                public string FileBytes { get; set; }
                public string SigningProfile { get; set; }
                public int RequestMode { get; set; }
                public string SignBy { get; set; }

                public MutualAuthentication MutualAuthentication { get; set; }
                public string RequestIdentifier { get; set; }
            }

            public class MutualAuthentication
            {
                public bool EnableMutualAuth { get; set; }
                public string MutualAuthCertSerial { get; set; }
            }

            public class SignHashResponse
            {
                public string SignedFileBytes { get; set; }
                public string Message { get; set; }
                public string UserFriendlyMessage { get; set; }
                public int Result { get; set; }
                public string Identifier { get; set; }
            }

            public class EmbedSignature
            {
                public EmbedSignature()
                {

                }

                public EmbedResponse AddLtv(byte[] signedDocument, int digestSize, string algorithm, string tsaURL, string tsaUser, string tsaPass, Signature sig)
                {
                    return null;
                }
                public EmbedResponse EmbedHash(byte[] beforeHashedBytes, byte[] signedBytes, Signature signature)
                {
                    return null;
                }
            }

            public class GenerateTokenResponse
            {
                public string status { get; set; }
                public string access_token { get; set; }

            }

            #endregion

        }
        public class GenerateTokenClient
        {
            public string GetToken()
            {
                var client = new RestClient("https://api-test.moj.gov.local/v1/authorize/access-token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                // user name :password convert to basestring64 
                client.Authenticator = new RestSharp.Authenticators.HttpBasicAuthenticator("jMvdzYd5w4fe2H0tG9mOSLpO7Yi3Avcy", "O3u1vn2FjKYUeyNi");
                //request.AddHeader("Cookie", "MOJi=1073848842.47873.0000");

                request.AddParameter("grant_type", "client_credentials");
                // exec
                var response = client.Execute<GenerateTokenResponse>(request);
                if (response.StatusCode == 0)
                {
                    return "error";
                }
                return response.Data.access_token;
            }
        }
        public object SignHash(SignHashRequest modelRequest)
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(delegate { return true; });
            GenerateTokenClient tokenClient = new GenerateTokenClient();
            string token = tokenClient.GetToken();

            var client = new RestClient("https://qaapi.emsigner.com/api/InitiateAndSign");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");

            request.AddHeader("Authorization", $"Bearer {token}");

            request.AddJsonBody(modelRequest);

            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<SignHashResponse>(response.Content);
            }
            else
            {
                return null;
            }

        }

        [HttpGet]
        public HttpResponseMessage GetAllUsers()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserProfileDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<UserProfile> managers = orgUnitBL.GetAllUsers();

                    List<UserProfileDTO> userProfileDTOs = UserProfileMapper.Map(managers);

                    getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, userProfileDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserProfileDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage AddArchivesLibrary(ArchivesLibraryDTO archivesLibraryDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {


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
        public HttpResponseMessage GetLetterTypeById(int letterTypeId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<LetterTypeDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILetterTypeBL letterTypeBL = IoC.Resolve<ILetterTypeBL>();
                    LetterTypeDTO letterTypeDTO = LetterTypeMapper.MapLetterType(letterTypeBL.GetLetterTypeById(letterTypeId), cultureName);

                    getResult = GetResult<LetterTypeDTO>.Create(statusCode, letterTypeDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);


                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<LetterTypeDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage ResetWordAddInSession()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {

                User user = (User)UserContext.LoggedInUser;

                CustomHttpApplication.WordAddInListSession.Remove(user.UserName.ToLower());

                getResult = GetResult<bool>.Create(statusCode, true, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<bool>.Create(statusCode, false, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<bool>.Create(statusCode, false, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage LogPrintDocument(int transactionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionAttachmentDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionLoggingBL transactionLoggingBL = IoC.Resolve<TransactionLoggingBL>();
                    getResult = GetResult<List<TransactionAttachmentDTO>>.Create(statusCode, null, null);
                    LogAction(AuditingActionCode.PrintWithoutWatermark, transactionId);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<TransactionAttachmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<TransactionAttachmentDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        #region Transaction Encryption
        [HttpPost]
        public HttpResponseMessage SendTransactionEncryptionCode(string cultureName, TransactionEncryptionCodeDTO transactionEncryptionCodeDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            //The defualt sending channel is Email 
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        TransactionBL.AddTransactionEncryptionCode(TransactionEncryptionCodeMapper.Map(transactionEncryptionCodeDTO));

                        if (transactionEncryptionCodeDTO.EncryptionChannel == EncryptionChannel.Email)
                        {
                            TransactionEncryptionBL.SendHashCodeByEmail(transactionEncryptionCodeDTO.TransactionId, transactionEncryptionCodeDTO.Code, transactionEncryptionCodeDTO.UserId, cultureName);
                        }
                        else
                        {
                            TransactionEncryptionBL.SendHashCodeBySMS(transactionEncryptionCodeDTO.TransactionId, "0557138682", transactionEncryptionCodeDTO.Code, transactionEncryptionCodeDTO.UserId, cultureName);
                        }
                    }
                }

                statusCode = Common.StatusCode.Ok;
                postResult = PostResult.Create(statusCode, null);
                return Request.CreateResponse(HttpStatusCode.OK, postResult);

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
        public HttpResponseMessage VerifyCode(int transactionId, string UserVerifyCode)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReleaseNotesDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        User user = (User)UserContext.LoggedInUser;
                        var notesList = TransactionBL.ReleaaseNotesUsersSelect(user.Id);
                        var dtoListObj = ReleaseNotesMapper.Map(notesList);
                        getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, dtoListObj, 0);
                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        #endregion

        #region Release Notes
        [HttpGet]
        public HttpResponseMessage ReleaaseNotesUsersSelect()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReleaseNotesDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    {
                        User user = (User)UserContext.LoggedInUser;
                        var notesList = TransactionBL.ReleaaseNotesUsersSelect(user.Id);
                        var dtoListObj = ReleaseNotesMapper.Map(notesList);
                        getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, dtoListObj, 0);
                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReleaseNotesDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage ReleaaseNotesUsersAdd()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        User user = (User)UserContext.LoggedInUser;
                        TransactionBL.ReleaaseNotesUsersAdd(user.Id);
                        transactionContextScope.Commit();
                        postResult = PostResult.Create(statusCode, null);
                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;
                    postResult = PostResult.Create(statusCode, null);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
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
        #endregion


        #region
        [HttpPost]
        public HttpResponseMessage UpdateAssignmentSelectedoption(SaveAssignmentPaperDTO saveAssignmentPaperDTO)
        {

            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ITransactionBL transactionBL = TransactionBL.Create(Common.TransactionCategory.Inbound);

                    transactionBL.UpdateAssignmentSelectedoption(saveAssignmentPaperDTO.TransactionId, saveAssignmentPaperDTO.AssignmentList);

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
        #endregion
    }

}

