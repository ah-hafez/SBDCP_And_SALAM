using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Transactions;
using MCS.Framework;
using MCS.Framework.Notifications;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public class TenantBL : BaseBL, ITenantBL
    {
        public int AddTenant(Tenant tenant, string cultureName)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

                Tenant tenantOld = tenantManagementRepository.GetTenantByHostName(tenant.HostName);

                if (tenantOld != null)
                {
                    throw new BusinessException(StatusCode.TenantAlreadyExist);
                }

                int tenantId;
                tenant.DatabaseName = tenant.HostName;
                tenant.IsActive = true;
                using (var oTransactionScope = new TransactionScope())
                {
                    //add the tenant
                    tenantId = tenantManagementRepository.AddTenant(tenant);
                    //get cultures
                    foreach (TenantLocalization tenantLocalization in tenant.DelegatedName.Localizations)
                    {
                        tenantLocalization.Culture = tenantManagementRepository.GetTenantCulture(tenantLocalization.CultureId);
                    }
                    //Create the tenant database using migration
                    oTransactionScope.Complete();
                }
                if (tenant.Id > 0)
                {
                    MigrationManager.GenerateConnectionString(tenant.DatabaseName);
                    //Send notification to the user 
                    SendTanentResetPasswordEmail(tenant.Id, cultureName);
                }

                return tenantId;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int AddEditUserTenant(UserTenant userTenant)
        {
            try
            {
                int tenantId;
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

                var tenantUserName = tenantManagementRepository.IsExistTenantUserName(userTenant.UserName, userTenant.Id);
                if (tenantUserName)
                {
                    throw new BusinessException(StatusCode.UserTenantAlreadyExist);
                }

                //add user tenant
                tenantId = tenantManagementRepository.AddEditUserTenant(userTenant);

                return tenantId;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void SendTanentResetPasswordEmail(int id, string cultureName)
        {
            ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

            Tenant tenant = tenantManagementRepository.GetTenantById(id);

            IList<TenantNotificationDetail> tenantNotificationDetails = new List<TenantNotificationDetail>();

            TenantNotificationDetail tenantNotificationDetail = BuildNotificationDetail(TenantNotificationType.Email,
                TenantNotificationTemplateType.TenantAdminEmail, TenantNotificationEmailSubject.NewUser, cultureName);

            tenantNotificationDetails.Add(tenantNotificationDetail);

            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("{UserName}", tenant.DelegatedName.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text);

            string tenantHostPath = SystemConfigurations.MultiTenantResetPage;

            tenantHostPath = tenantHostPath.Replace("{HostName}", tenant.HostName);

            keyValues.Add("{Url}", tenantHostPath);

            SendNotification(TenantNotificationSource.ResetPassword, tenantNotificationDetails, tenant.LocalDelegatedName, tenant.DelegatedEmail, keyValues);
        }

        private void SendNotification(TenantNotificationSource notificationSource, IList<TenantNotificationDetail> tenantNotificationDetails, string hostName, string email, Dictionary<string, string> labels)
        {
            INotificationBL notificationBL = IoC.Resolve<INotificationBL>();

            TenantNotification tenantNotification = new TenantNotification()
            {
                SourceId = (int)notificationSource,
                Date = DateTime.Now,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                Details = tenantNotificationDetails,
                DelegatedEmail = hostName
            };

            SaveTenantNotification(tenantNotification, labels);

            foreach (TenantNotificationDetail tenantNotificationDetail in tenantNotification.Details)
            {
                SendEmail(tenantNotificationDetail.Subject, tenantNotificationDetail.Body, email,
                            tenantNotificationDetail.Attachments);
            }
        }

        public void SaveTenantNotification(TenantNotification tenantNotification, Dictionary<string, string> labels)
        {
            try
            {
                foreach (TenantNotificationDetail tenantNotificationDetail in tenantNotification.Details)
                {
                    string body = (labels != null) ? FormatNotificationLabels(tenantNotificationDetail.Body, labels) :
                        tenantNotificationDetail.Body;

                    tenantNotificationDetail.Body = body;
                }

                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

                tenantManagementRepository.AddTenantNotification(tenantNotification);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateTenant(Tenant tenant)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

                tenantManagementRepository.UpdateTenant(tenant);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Tenant GetTenantById(int tenantId)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetTenantById(tenantId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public Tenant GetTenantByYesserCode(string yesserCode)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetTenantByYesserCode(yesserCode);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<TenantNotificationDetail> GetFailedNotifactions(int failureCount, NotificationType notificationType)
        {

            ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

            return tenantManagementRepository.GetFailedNotifactions(failureCount, notificationType);
        }

        public void ActivateTenant(int tenantId, bool isActive)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                Tenant tenant = tenantManagementRepository.GetTenantById(tenantId);

                if (tenant != null)
                {
                    tenant.IsActive = isActive;
                    tenantManagementRepository.UpdateTenant(tenant);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Tenant GetTenantByHostName(string hostName, bool validate = true)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                Tenant tenant = tenantManagementRepository.GetTenantByHostName(hostName);

                if (validate)
                {
                    Validate(tenant);
                }
                return tenant;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Tenant GetTenantByUserName(string userName, string cultureName, bool validate = true)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                Tenant tenant = tenantManagementRepository.GetTenantByUserName(userName, cultureName);

                if (validate)
                {
                    Validate(tenant);
                }

                return tenant;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public Tenant GetTenantById(int tenantId, bool validate)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                Tenant tenant = tenantManagementRepository.GetTenantById(tenantId);

                if (validate)
                {
                    Validate(tenant);
                }
                return tenant;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Tenant> GetTenants(System.Linq.Expressions.Expression<Func<Tenant, bool>> @where, string cultureName)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetTenants(@where, cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Tenant> GetTenants(SearchCriteria searchCriteria, string cultureName, out int rowsCount)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                IList<Tenant> tenants = tenantManagementRepository.GetTenants(searchCriteria, cultureName, out rowsCount);

                return tenants;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<Tenant> GetAllTenants(string cultureName)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetAllTenants(cultureName);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public IList<UserTenant> GetAllUserTenants()
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetAllUserTenants();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void DeleteTenants(IList<int> Ids)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                Tenant tenant;
                tenant = tenantManagementRepository.GetTenantById(Ids[0]);
                if (tenant.UserTenants.Count == 0)
                {
                    tenant.IsDeleted = true;
                    tenantManagementRepository.UpdateTenant(tenant);
                }
                else
                {
                    throw new BusinessException(StatusCode.ConnectWithUserTenant);
                }
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void DeleteUserTenant(int id)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                tenantManagementRepository.DeleteUserTenant(id);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public IList<TenantCulture> GetTenantCultures()
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetTenantCultures();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void Validate(Tenant tenant)
        {
            try
            {
                //validate if the Tenant is active or not
                if (!tenant.IsActive)
                {
                    throw new BusinessException(StatusCode.TenantNotActive);
                }

                //if (DateTime.Now > tenant.ToDate)
                //{
                //    throw new BusinessException(StatusCode.SystemExpired);
                //}
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private UserProfile MapUserProfile(Tenant tenant)
        {
            if (tenant != null)
            {
                IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                UserProfile userProfile = new UserProfile()
                {
                    //LocalizationIdentifier = tenant.DelegatedName,
                    Email = tenant.DelegatedEmail,
                    PhoneNumber = tenant.DelegatedMobile,
                    UserName = tenant.DelegatedUserName,
                    TitleId = Title.Mr.LookupIdentity(LookupCategory.Title, string.Empty),
                    IsActive = true,
                    TransactionProcessingPeriod = 0
                };

                userProfile.Permissions = new List<UserPermission>();
                userProfile.Permissions.Add(new UserPermission() { Permission = permissionBL.GetPermissionByCode(UserClaims.Admin.Administrator) });

                return userProfile;
            }

            return null;
        }

        private TenantNotificationDetail BuildNotificationDetail(NotificationType notificationType,
            NotificationTemplateType notificationTemplateType,
            NotificationEmailSubject? notificationEmailSubject, string cultureName,
            string body = null)
        {
            INotificationBL notificationBL = new NotificationBL();
            ILookupBL lookupBL = new LookupBL();

            NotificationDetail notificationDetail = new NotificationDetail()
            {
                NotificationType = lookupBL.GetLookupItem((int)notificationType) 
            };

            if (body == null)
            {
                notificationDetail.NotificationTemplateType = lookupBL.GetLookupItem((int)notificationTemplateType);

                LookupLocalization templateText = notificationDetail.NotificationTemplateType.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                notificationDetail.Body = (templateText != null) ? templateText.Text : string.Empty;
            }
            else
            {
                notificationDetail.Body = body;
            }
            TenantNotificationDetail tenantNotificationDetail = new TenantNotificationDetail() { TypeId = (int)notificationType };
            tenantNotificationDetail.Body = notificationDetail.Body;

            if (notificationEmailSubject.HasValue)
            {               
                Lookup subject = lookupBL.GetLookupItem((int)notificationEmailSubject);
                LookupLocalization subjectLocalization = subject.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();
                if (subjectLocalization != null)
                {
                    notificationDetail.Subject = subjectLocalization.Text;
                    tenantNotificationDetail.Subject = notificationDetail.Subject;
                }
            }

            return tenantNotificationDetail;
        }


        private TenantNotificationDetail BuildNotificationDetail(TenantNotificationType notificationType,
            TenantNotificationTemplateType? notificationTemplateType,
            TenantNotificationEmailSubject? notificationEmailSubject, string cultureName,
            string body = null)
        {
            ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();

            TenantNotificationDetail notificationDetail = new TenantNotificationDetail()
            {
                TypeId = (int)notificationType
            };

            if (body == null && notificationTemplateType.HasValue)
            {
                notificationDetail.Template = tenantManagementRepository.GetNotificationTemplate(notificationTemplateType.Value);

                TenantLookupLocalization templateText = notificationDetail.Template.Type.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                notificationDetail.Body = (templateText != null) ? templateText.Text : string.Empty;
            }
            else
            {
                notificationDetail.Body = body;
            }

            if (notificationEmailSubject != null && notificationType == TenantNotificationType.Email)
            {

                TenantLookup subject = tenantManagementRepository.GetLookupItem((int)notificationEmailSubject);

                TenantLookupLocalization subjectLocalization =
                       subject.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                if (subjectLocalization != null)
                {
                    notificationDetail.Subject = subjectLocalization.Text;
                }
            }

            return notificationDetail;
        }

        private void SendNotification(NotificationSource notificationSource, IList<TenantNotificationDetail> tenantNotificationDetails, string hostName, string email, Dictionary<string, string> labels)
        {
            INotificationBL notificationBL = IoC.Resolve<INotificationBL>();

            TenantNotification tenantNotification = new TenantNotification()
            {
                SourceId = (int)notificationSource,
                Date = DateTime.Now,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                Details = tenantNotificationDetails,
                DelegatedEmail = email
            };

            SaveTenantNotification(tenantNotification, labels);

            foreach (TenantNotificationDetail tenantNotificationDetail in tenantNotification.Details)
            {
                SendEmail(tenantNotificationDetail.Subject, tenantNotificationDetail.Body, email,
                            tenantNotificationDetail.Attachments);
            }
        }

        private void SendEmail(string subject, string body, string email, IList<TenantNotificationAttachment> tenantNotificationAttachment)
        {
            IEmailNotificationService emailNotificationService = IoC.Resolve<IEmailNotificationService>();
            EmailMessage emailMessage = new EmailMessage();

            emailMessage.Subject = subject;
            emailMessage.Body = body;
            emailMessage.To = email;

            IList<System.Net.Mail.Attachment> mailAttachments = null;

            if (tenantNotificationAttachment != null && tenantNotificationAttachment.Count > 0)
            {
                mailAttachments = new List<System.Net.Mail.Attachment>();

                foreach (TenantNotificationAttachment notificationAttachment in tenantNotificationAttachment)
                {
                    System.Net.Mail.Attachment mailAttachment =
                        new System.Net.Mail.Attachment(new MemoryStream(notificationAttachment.Binary), notificationAttachment.FileName);

                    mailAttachments.Add(mailAttachment);
                }
            }

            emailNotificationService.Send(emailMessage);
        }

        private string FormatNotificationLabels(string notificationBody, Dictionary<string, string> labels)
        {
            foreach (KeyValuePair<string, string> label in labels)
            {
                notificationBody = notificationBody.Replace(label.Key, label.Value);
            }

            return notificationBody;
        }

        private void CopyTenantService(string sourceDir, string targetDir)
        {
            Directory.CreateDirectory(targetDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                File.Copy(file, Path.Combine(targetDir, Path.GetFileName(file)));
            }

            foreach (var directory in Directory.GetDirectories(sourceDir))
            {
                CopyTenantService(directory, Path.Combine(targetDir, Path.GetFileName(directory)));
            }
        }

        private void InstallTenantService(string serviceName, string displayName, string fileName)
        {
            ServiceInstaller.InstallAndStart(serviceName, displayName, fileName);
        }

        private void SetTenantConfiguration(string filePath, string tenantName)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(filePath);

            string solrUrlValue = config.AppSettings.Settings["SolrUrlTenant"].Value;

            string ConnectionString = config.AppSettings.Settings["TenantConnectionString"].Value;

            config.ConnectionStrings.ConnectionStrings["eMorasalat"].ConnectionString = ConnectionString.Replace("{DbName}", tenantName);

            solrUrlValue = solrUrlValue.Replace("{CoreName}", tenantName);

            config.AppSettings.Settings["SolrUrl"].Value = solrUrlValue;

            config.Save(ConfigurationSaveMode.Minimal);
        }

        private void StartTenantService(string serviceName)
        {
            ServiceInstaller.StartService(serviceName);
        }

        private void StopTenantService(string serviceName)
        {
            ServiceInstaller.StopService(serviceName);
        }

        private void UninstallTenantService(string serviceName)
        {
            ServiceInstaller.Uninstall(serviceName);
        }

        private string getPathFileParent(string pathFile)
        {
            return new DirectoryInfo(pathFile).Parent.FullName;
        }

        private void DeleteTanentFolder(string tenantName)
        {
            DirectoryInfo dir = new DirectoryInfo(SystemConfigurations.SolrServiceTenantsPath + tenantName);

            foreach (FileInfo fi in dir.GetFiles())
            {
                fi.Delete();
            }

            foreach (DirectoryInfo di in dir.GetDirectories())
            {
                DeleteTanentFolder(di.FullName);
                di.Delete();
            }

            dir.Delete();
        }

        #region Tenant Lookup
        public TenantLookup GetLookupItem(int lookupId)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetLookupItem(lookupId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        #endregion Tenant Lookup

        public void PrepareTanentNotification(NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationEmailSubject? notificationEmailSubject, string Email,
            string cultureName, IList<TenantNotificationAttachment> attachments,
            Dictionary<string, string> labels)
        {
            IList<TenantNotificationDetail> tenantNotificationDetails = new List<TenantNotificationDetail>();

            TenantNotificationDetail tenantNotificationDetail = BuildNotificationDetail(NotificationType.Email, notificationTemplateType, notificationEmailSubject, cultureName);

            tenantNotificationDetail.Attachments = attachments;
            tenantNotificationDetail.Email = Email;

            tenantNotificationDetails.Add(tenantNotificationDetail);

            INotificationBL notificationBL = IoC.Resolve<INotificationBL>();

            TenantNotification tenantNotification = new TenantNotification()
            {
                SourceId = notificationSource.LookupIdentity(LookupCategory.NotificationSource, cultureName),
                Date = DateTime.Now,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                Details = tenantNotificationDetails
            };

            SaveNotification(tenantNotification, labels);
        }

        private void SaveNotification(TenantNotification tenantNotification, Dictionary<string, string> labels)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                foreach (TenantNotificationDetail tenantNotificationDetail in tenantNotification.Details)
                {
                    if (labels != null)
                    {
                        tenantNotificationDetail.Body = FormatNotificationLabels(tenantNotificationDetail.Body, labels);
                        if (!string.IsNullOrEmpty(tenantNotificationDetail.Subject))
                        {
                            tenantNotificationDetail.Subject = FormatNotificationLabels(tenantNotificationDetail.Subject, labels);
                        }
                    }
                }
                tenantManagementRepository.AddTenantNotification(tenantNotification);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void SendSupportEmail(SupportDTO supportDTO, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                IList<NotificationDetail> notificationDetails = new List<NotificationDetail>();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{Description}"] = supportDTO.Description;
                keyValues["{SupportType}"] = lookupBL.GetLookupItem(int.Parse(supportDTO.SupportType)).Localizations.Where(l=>l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                keyValues["{Category}"] = lookupBL.GetLookupItem(int.Parse(supportDTO.Category)).Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;

                notificationDetails.Add(NotificationsManager.BuildNotificationDetail(NotificationType.Email,
                    NotificationTemplateType.SupportEmail, null, NotificationEmailSubject.ReceiveReportEmail, cultureName));


                //Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    var tenantNoticationAttachments = new List<TenantNotificationAttachment>();
                    foreach (var item in supportDTO.NotificationAttachmentDTOS.ToList())
                    {
                        var tenantNoticationAttachment = new TenantNotificationAttachment
                        {
                            Binary = item.Binary,
                            ContentLength = item.ContentLength,
                            ContentType = item.ContentType,
                            FileName = item.FileName
                        };
                        tenantNoticationAttachments.Add(tenantNoticationAttachment);
                    }
                    tenantBL.PrepareTanentNotification(NotificationSource.ReceiveSupport, NotificationTemplateType.SupportEmail,
                        null, supportDTO.ToEmail, cultureName, tenantNoticationAttachments, keyValues);
                }
                else
                {
                    var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(User.Id) };
                    notificationUsersEmail.FirstOrDefault().User.Email = supportDTO.ToEmail;

                    var noticationAttachments = new List<NotificationAttachment>();
                    foreach (var item in supportDTO.NotificationAttachmentDTOS.ToList())
                    {
                        var noticationAttachment = new NotificationAttachment
                        {
                            Binary = item.Binary,
                            ContentLength = item.ContentLength,
                            ContentType = item.ContentType,
                            FileName = item.FileName,
                            CreatedOn = DateTime.Now,
                            CreatedBy = User.Id
                        };
                        noticationAttachments.Add(noticationAttachment);
                    }
                    //System Notification  Email
                    NotificationsManager.EmailNotification(NotificationSource.ReceiveSupport, NotificationTemplateType.SupportEmail, null,
                        notificationUsersEmail, cultureName, noticationAttachments, keyValues);
                }
            }
        }

        public void SaveNotification(TenantNotification tenantNotification)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                tenantManagementRepository.AddTenantNotification(tenantNotification);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateNotifactionDetails(IList<TenantNotificationDetail> tenantNotificationDetail)
        {
            ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
            tenantManagementRepository.UpdateNotifactionDetails(tenantNotificationDetail);
        }

        public UserTenant GetUserTenantById(int id)
        {
            try
            {
                ITenantManagementRepository tenantManagementRepository = IoC.Resolve<ITenantManagementRepository>();
                return tenantManagementRepository.GetUserTenantById(id);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
