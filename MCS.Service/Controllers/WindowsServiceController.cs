using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Notifications;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Common.Utility;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Notifications;
using MCS.DTO.Tenants;
using MCS.Service.Filters;
using MCS.Service.Helpers.TenantsAPI;

namespace MCS.Service.Controllers
{
    [WindowsServiceAuthorization]
    public class WindowsServiceController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage CheckEndTasks()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                    transactionTaskBL.CheckEndTasks(Language);

                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }
        [HttpGet]
        public HttpResponseMessage SendToUserReminderBeforeTaskEnded()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                ISettingBL settingBL = new SettingBL();
                List<Setting> settings = settingBL.GetSettingByKey(Constants.GeneralSettings.NotifyEmployeeBeforeTaskExpiry);
                Setting setting = settings.Find(a => a.Key == Constants.GeneralSettings.NotifyEmployeeBeforeTaskExpiry);
                var TaskProcessingPeriod = Convert.ToInt32(setting.Value);
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();
                    transactionTaskBL.SendToUserReminderBeforeTaskEnded(TaskProcessingPeriod != 0 ? TaskProcessingPeriod : SystemConfigurations.TaskProcessingPeriod, SystemConfigurations.TaskReminderCount, Language);
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage TenantNotifyByEmail()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                ITenantBL TenantBL = IoC.Resolve<ITenantBL>();
                ISettingBL settingBL = new SettingBL();
                List<Setting> settings = settingBL.GetSettingByKey(Constants.EmailSettings.EmailService);
                Setting setting = settings.Find(a => a.Key == Constants.EmailSettings.EmailService);
                var EmailSettingValue = setting.Value;
                var failedNotifactions = TenantBL.GetFailedNotifactions(SystemConfigurations.FaliureSendSupportAttemptsLimts, NotificationType.Email);
                var notifcationDetailsDto = new List<TenantNotifcationDetailsDto>();
                foreach (var item in failedNotifactions)
                {

                    var tenantNotifcationDetailsDto = new TenantNotifcationDetailsDto();
                    tenantNotifcationDetailsDto.Body = item.Body;
                    tenantNotifcationDetailsDto.Id = item.Id;
                    tenantNotifcationDetailsDto.FailureCount = item.FailureCount;
                    tenantNotifcationDetailsDto.IsSent = item.IsSent;
                    tenantNotifcationDetailsDto.Subject = item.Subject;
                    tenantNotifcationDetailsDto.TypeId = item.TypeId;
                    tenantNotifcationDetailsDto.Email = item.Email;
                    tenantNotifcationDetailsDto.tenantNotificationAttachment = new List<AttachmentDTO>();
                    foreach (var Attachment in item.Attachments.ToList())
                    {
                        var attachmentDTO = new AttachmentDTO();
                        attachmentDTO.FileName = Attachment.FileName;
                        attachmentDTO.ContentType = Attachment.ContentType;
                        attachmentDTO.ContentLength = Attachment.ContentLength;
                        attachmentDTO.Binary = Attachment.Binary;
                        tenantNotifcationDetailsDto.tenantNotificationAttachment.Add(attachmentDTO);
                    }

                    notifcationDetailsDto.Add(tenantNotifcationDetailsDto);
                }

                foreach (var item in notifcationDetailsDto)
                {

                    EmailMessage emailMessage = new EmailMessage();
                    emailMessage.Subject = item.Subject;
                    emailMessage.Body = item.Body;
                    emailMessage.To = item.Email;
                    IList<System.Net.Mail.Attachment> mailAttachments = null;

                    if (item.tenantNotificationAttachment != null && item.tenantNotificationAttachment.Count > 0)
                    {
                        mailAttachments = new List<System.Net.Mail.Attachment>();

                        foreach (var notificationAttachment in item.tenantNotificationAttachment)
                        {
                            byte[] file = new byte[notificationAttachment.ContentLength];
                            file = notificationAttachment.Binary;

                            System.Net.Mail.Attachment mailAttachment =
                                new System.Net.Mail.Attachment(new MemoryStream(file), notificationAttachment.FileName);

                            mailAttachments.Add(mailAttachment);
                        }
                        emailMessage.Attachments = mailAttachments;
                    }
                    if (EmailSettingValue == "true")
                    {
                        var result = EmailUtility.Send(emailMessage);
                        if (result == true)
                        {
                            item.IsSent = true;
                        }
                        else
                        {
                            item.FailureCount += 1;
                        }
                    }
                    else
                    {
                        item.FailureCount += 1;
                    }

                }

                var tenantNotifcationDetails = new List<TenantNotificationDetail>();

                foreach (var item in notifcationDetailsDto)
                {
                    tenantNotifcationDetails.Add(new TenantNotificationDetail() { Id = item.Id, FailureCount = item.FailureCount, IsSent = item.IsSent });
                }
                if (tenantNotifcationDetails.Count > 0)
                {
                    TenantBL.UpdateNotifactionDetails(tenantNotifcationDetails);
                }

                getResult = GetResult<int>.Create(statusCode, notifcationDetailsDto.Count, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage NotifyByEmail()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            try
            {
                var notificationBL = IoC.Resolve<INotificationBL>();
                ISettingBL settingBL = new SettingBL();
                List<Setting> settings = settingBL.GetSettingByKey(Constants.EmailSettings.EmailService);
                Setting setting = settings.Find(a => a.Key == Constants.EmailSettings.EmailService);
                var EmailSettingValue = setting.Value;

                var failedNotifactions = notificationBL.GetFailedNotifactions(SystemConfigurations.FaliureSendSupportAttemptsLimts, NotificationType.Email);
                var notifcationDetailsDto = new List<NotifcationDetailsDTO>();
                foreach (var item in failedNotifactions)
                {
                    //TenantNotifcationDetailsDto
                    var notifcationDetailsDTO = new NotifcationDetailsDTO();
                    notifcationDetailsDTO.Body = item.Body;
                    notifcationDetailsDTO.Id = item.Id;
                    notifcationDetailsDTO.FailureCount = item.FailureCount;
                    notifcationDetailsDTO.IsSent = item.IsSent;
                    notifcationDetailsDTO.Subject = item.Subject;
                    notifcationDetailsDTO.TypeId = item.NotificationType.Id;
                    notifcationDetailsDTO.Email = item.Email;
                    notifcationDetailsDTO.NotificationAttachment = new List<AttachmentDTO>();
                    foreach (var Attachment in item.Attachments.ToList())
                    {
                        var attachmentDTO = new AttachmentDTO();
                        attachmentDTO.FileName = Attachment.FileName;
                        attachmentDTO.ContentType = Attachment.ContentType;
                        attachmentDTO.ContentLength = Attachment.ContentLength;
                        attachmentDTO.Binary = Attachment.Binary;
                        notifcationDetailsDTO.NotificationAttachment.Add(attachmentDTO);
                    }
                    notifcationDetailsDto.Add(notifcationDetailsDTO);
                }

                foreach (var item in notifcationDetailsDto)
                {
                    EmailMessage emailMessage = new EmailMessage();
                    emailMessage.Subject = item.Subject;
                    emailMessage.Body = item.Body;
                    emailMessage.To = item.Email;
                    IList<System.Net.Mail.Attachment> mailAttachments = null;

                    if (item.NotificationAttachment != null && item.NotificationAttachment.Count > 0)
                    {
                        mailAttachments = new List<System.Net.Mail.Attachment>();
                        foreach (var notificationAttachment in item.NotificationAttachment)
                        {
                            byte[] file = new byte[notificationAttachment.ContentLength];
                            file = notificationAttachment.Binary;
                            System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(new MemoryStream(file), notificationAttachment.FileName);
                            mailAttachments.Add(mailAttachment);
                        }
                        emailMessage.Attachments = mailAttachments;
                    }
                    if (EmailSettingValue == "true")
                    {
                        var result = EmailUtility.Send(emailMessage);
                        if (result == true)
                        {
                            item.IsSent = true;
                        }
                        else
                        {
                            item.FailureCount += 1;
                        }
                    }
                    else
                    {
                        item.FailureCount += 1;
                    }

                }

                var notifcationDetails = new List<NotificationDetail>();

                foreach (var item in notifcationDetailsDto)
                {
                    notifcationDetails.Add(new NotificationDetail() { Id = item.Id, FailureCount = item.FailureCount, IsSent = item.IsSent });
                }
                if (notifcationDetails.Count > 0)
                {
                    notificationBL.UpdateNotifactionDetails(notifcationDetails);
                }

                getResult = GetResult<int>.Create(statusCode, notifcationDetailsDto.Count, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage MigrateDocuments(int pageSize)
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();
                    var documents = documentBL.GetAllDocuments(pageSize);

                    if (documents != null && documents.Count > 0)
                    {
                        foreach (var currentDocument in documents)
                        {
                            DocData docData = new DocData()
                            {
                                Data = currentDocument.Document.Content,
                                DocName = currentDocument.Name,
                                DocID = currentDocument.Id.ToString(),
                                PersonId = currentDocument.CreatedBy,
                                MimeContent = currentDocument.MimeType,
                                EntityId = currentDocument.FromEntityId,
                                DataSize = Convert.ToInt32(currentDocument.Size),
                                User_ID = currentDocument.CreatedBy.ToString(),
                                ECMID = currentDocument.ECMId,
                                TransactionDate = currentDocument.CreatedOn,
                                TransactionDateHijri= DateTimeUtility.ConvertToUmAlQuraCalendar( currentDocument.CreatedOn),
                                TransactionId = currentDocument.TransactionId ?? -1

                            };
                            //DocRepository.DocRepository.Save(docData, new DocumentLocation());

                            DocRepository.Provider.ECM.ECMDocRepositoryProvider repositoryProvider = new DocRepository.Provider.ECM.ECMDocRepositoryProvider(null);
                            repositoryProvider.Save(docData, new DocumentLocation());

                            documentBL.ClearMigratedDocumentBinary(currentDocument.Id);
                        }    
                    }
                    //documents = documentBL.GetAllDocuments(pageSize);

                    getResult = GetResult<int>.Create(statusCode, documents !=  null? documents.Count : 0, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage SendLateTransactionReminderToSender()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {                                               
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();
                    fileBL.SendLateTransactionReminderToSender(Language);
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage AddUserSync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.AddUserSync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage DeleteUserSync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.DeleteUserSync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage MoveUserSync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.MoveUserSync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage DelegationUserSync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.DelegationUserSync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage AddEntitySync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.AddEntitySync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage MoveEntitySync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.MoveEntitySync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage UpdateEntityNameSync()
        {
            if (SystemConfigurations.MultiTenantEnabled)
            {
                string token = Request.Headers.Authorization.Parameter;
                HttpContext.Current.Request.Headers.Add(Constants.TenantId, Request.Headers.GetValues("Tenant_Id").First());
                HttpContext.Current.Request.Headers.Add(Constants.TenantDatabaseName, Request.Headers.GetValues("__TenantDatabaseName").First());
                PostObjectResult<ApplicationUserDTO> postResult = AuthorizationApiHelper<PostObjectResult<ApplicationUserDTO>>
                    .GetItemRequest("api/authorization/validateUser", token).Result;
                if (postResult.StatusCode != Common.StatusCode.CodeOK)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }
            }

            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IERPIntegrationBL integrationBL = new ERPIntegrationBL();
                    integrationBL.UpdateEntityNameSync();
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage SendLateTransactionWithNotifyLetterTypes()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();
                    fileBL.SendLateTransactionWithNotifyLetterTypes(Language);
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        public HttpResponseMessage SendNearlyLateTransaction()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<int> getResult = null;
            int rowsCount = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IFileBL fileBL = new FileBL();
                    fileBL.SendNearlyLateTransaction(Language);
                    getResult = GetResult<int>.Create(statusCode, 1, rowsCount);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<int>.Create(statusCode, 0, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}
