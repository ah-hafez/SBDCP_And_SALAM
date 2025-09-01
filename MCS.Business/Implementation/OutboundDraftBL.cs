using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
namespace MCS.Business
{
    public class OutboundDraftBL : TransactionBL, IOutboundDraftBL
    {
        protected override void Validate(Transaction transaction)
        {
            string TodayDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            Transaction existTransaction = GetTransaction(t => t.TransactionCategoryId == transaction.TransactionCategoryId && t.ExternalPartyId == transaction.ExternalPartyId && t.Subject == transaction.Subject && t.DateH == TodayDateH);
            if (existTransaction != null && existTransaction.Id != transaction.Id)
            {
                throw new BusinessException(StatusCode.DraftAlreadyExist);
            }
        }
        protected override void PostAssignTransaction(IList<TransactionAssignment> transactionAssignments)
        {
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            foreach (TransactionAssignment transactionAssignment in transactionAssignments)
            {
                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);
            }
        }
        protected override void MoveTransaction(Transaction transaction)
        {
            MoveTransactionToTray(transaction);
        }
        protected override void PreSave(Transaction transaction)
        {
            if (!User.HasClaim(UserClaims.Outbound.CreateOutboundDraft))
            {
                throw new BusinessException(StatusCode.PermissionOutboundOutboundDraft);
            }

            if (transaction.Names != null && transaction.Names.Count > 0 && !User.HasClaim(UserClaims.Names.Add))
            {
                throw new BusinessException(StatusCode.PermissionOutboundAddNames);
            }

            //if (transaction.Attachments != null && !User.HasClaim(UserClaims.Outbound.AddAttachments))
            //{
            //    throw new BusinessException(StatusCode.PermissionOutboundAddAttachments);
            //}

            if (transaction.Copies != null && transaction.Copies.Count > 0 && !User.HasClaim(UserClaims.CopiesInternal.Add))
            {
                throw new BusinessException(StatusCode.PermissionOutboundAddCopies);
            }
            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);

            if (transaction.TransactionPathId.HasValue)
                CheckTransactionConfidentialityForPath(transaction.ConfidentialityId, transaction.TransactionPathId.Value);
            IncrementTransactionCounter(transaction);
        }
        protected override void PostSave(Transaction transaction, byte[] content = null)
        {
            MoveTransaction(transaction);
            int transactionHistoryId = AddTransactionHistory(transaction);
            AddTransactionEntityDetails(transaction);
            AddTransactionCopiesBarcode(transaction);
            int orgUnitId = transaction.OrgUnit.Id;

            SaveDeliveryReport(transactionHistoryId, transaction, orgUnitId);

            //Notification => Web
            if (transaction.Copies != null && transaction.Copies.Count > 0)
            {
                List<int> userIds = new List<int>();
                foreach (var copy in transaction.Copies)
                {
                    if (copy.UserId.HasValue)
                    {
                        userIds.Add(copy.UserId.Value);
                    }
                    else if (copy.EntityId.HasValue)
                    {
                        OrgUnitBL orgUnitBL = new OrgUnitBL();
                        var userProfiles = orgUnitBL.GetUsersByParentId(copy.EntityId.Value, "ar");
                        foreach (var user in userProfiles)
                        {
                            userIds.Add(user.Id);
                        }
                    }
                }
                if (userIds.Count > 0)
                {
                    IUserManagementBL userManagementBL = new UserManagementBL();
                    var userPreferenceInfos = userManagementBL.GetUserPreferenceByUserIds(userIds);
                    var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                    foreach (var item in userPreferenceInfos)
                    {
                        if (item.NotificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                        {
                            IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserProfile.Id) };
                            SendTransactionNotification(transactionForNotification, NotificationSource.ElectronicCopies, NotificationTemplateType.ElectronicCopiesWeb,
                                NotificationTemplateType.ElectronicCopiesEmail, NotificationEmailSubject.ElectronicCopiesEmail, NotificationWebSubject.ElectronicCopies,
                                notificationUsers, "ar");
                        }
                    }
                }
            }
        }
        protected override void PreUpdate(Transaction transaction)
        {
            if (!User.HasClaim(UserClaims.Outbound.EditOutbound))
            {
                throw new BusinessException(StatusCode.PermissionOutboundEditOutbound);
            }
            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);
            if (transaction.TransactionPathId.HasValue)
                CheckTransactionConfidentialityForPath(transaction.ConfidentialityId, transaction.TransactionPathId.Value);
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            if (User.HasClaim(UserClaims.Names.Delete))
            {
                transactionRepository.UpdateTransactionNames(transaction.Id, transaction.Names);
            }

            if (transaction.Copies != null && User.HasClaim(UserClaims.CopiesInternal.Delete))
            {
                NotificationSource notificationSource = NotificationSource.None;
                NotificationTemplateType notificationTemplateType = NotificationTemplateType.None;
                NotificationTemplateType notificationEmailTemplateType = NotificationTemplateType.None;
                NotificationEmailSubject notificationEmailSubject = NotificationEmailSubject.None;
                NotificationWebSubject notificationWebSubject = NotificationWebSubject.None;
                var transactionBeforeUpdate = GetTransactionById(transaction.Id);
                var transactionCopiesBeforeUpdate = transactionBeforeUpdate.Copies;
                //There is no copies added before
                if (transactionBeforeUpdate != null && (transactionCopiesBeforeUpdate == null || transactionCopiesBeforeUpdate.Count == 0))
                {
                    notificationSource = NotificationSource.ElectronicCopies;
                    notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                    notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                    notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                    notificationWebSubject = NotificationWebSubject.ElectronicCopies;
                    List<int> userIds = new List<int>();
                    foreach (var copy in transaction.Copies)
                    {
                        if (copy.UserId.HasValue)
                        {
                            userIds.Add(copy.UserId.Value);
                        }
                        else if (copy.EntityId.HasValue)
                        {
                            OrgUnitBL orgUnitBL = new OrgUnitBL();
                            var userProfiles = orgUnitBL.GetUsersByParentId(copy.EntityId.Value, "ar");
                            foreach (var user in userProfiles)
                            {
                                userIds.Add(user.Id);
                            }
                        }
                    }
                    if (userIds.Count > 0)
                    {
                        IUserManagementBL userManagementBL = new UserManagementBL();
                        var userPreferenceInfos = userManagementBL.GetUserPreferenceByUserIds(userIds);
                        var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                        foreach (var item in userPreferenceInfos)
                        {
                            if (item.NotificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                            {
                                IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserProfile.Id) };
                                SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                    notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                    notificationUsers, "ar");
                            }
                        }
                    }
                }
                //There are copies added before
                else if (transactionBeforeUpdate != null && transactionCopiesBeforeUpdate != null && transactionCopiesBeforeUpdate.Count > 0)
                {
                    foreach (var copy in transaction.Copies)
                    {
                        var originalTransactionCopy = transactionCopiesBeforeUpdate
                                                                .Where(c => c.Id == copy.Id && c.Id != 0)
                                                                .SingleOrDefault();
                        //Updated Item
                        if (originalTransactionCopy != null)
                        {
                            if (originalTransactionCopy.UserId != copy.UserId || originalTransactionCopy.EntityId != copy.EntityId || originalTransactionCopy.ActionId != copy.ActionId)
                            {
                                List<int> userIds = new List<int>();
                                notificationSource = NotificationSource.EditElectronicCopies;
                                notificationTemplateType = NotificationTemplateType.EditElectronicCopiesWeb;
                                notificationEmailTemplateType = NotificationTemplateType.EditElectronicCopiesEmail;
                                notificationEmailSubject = NotificationEmailSubject.EditElectronicCopiesEmail;
                                notificationWebSubject = NotificationWebSubject.EditElectronicCopies;
                                if (copy.UserId.HasValue)
                                {
                                    userIds.Add(copy.UserId.Value);
                                }
                                else if (copy.EntityId.HasValue)
                                {
                                    OrgUnitBL orgUnitBL = new OrgUnitBL();
                                    var userProfiles = orgUnitBL.GetUsersByParentId(copy.EntityId.Value, "ar");
                                    foreach (var user in userProfiles)
                                    {
                                        userIds.Add(user.Id);
                                    }
                                }
                                if (userIds.Count > 0)
                                {
                                    IUserManagementBL userManagementBL = new UserManagementBL();
                                    var userPreferenceInfos = userManagementBL.GetUserPreferenceByUserIds(userIds);
                                    var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                                    foreach (var item in userPreferenceInfos)
                                    {
                                        if (item.NotificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                                        {
                                            IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserProfile.Id) };
                                            SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                                notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                                notificationUsers, "ar");
                                        }
                                    }
                                }
                            }
                        }
                        //Added item
                        else
                        {
                            List<int> userIds = new List<int>();
                            notificationSource = NotificationSource.ElectronicCopies;
                            notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                            notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                            notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                            notificationWebSubject = NotificationWebSubject.ElectronicCopies;
                            if (copy.UserId.HasValue)
                            {
                                userIds.Add(copy.UserId.Value);
                            }
                            else if (copy.EntityId.HasValue)
                            {
                                OrgUnitBL orgUnitBL = new OrgUnitBL();
                                var userProfiles = orgUnitBL.GetUsersByParentId(copy.EntityId.Value, "ar");
                                foreach (var user in userProfiles)
                                {
                                    userIds.Add(user.Id);
                                }
                            }
                            if (userIds.Count > 0)
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();
                                var userPreferenceInfos = userManagementBL.GetUserPreferenceByUserIds(userIds);
                                var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                                foreach (var item in userPreferenceInfos)
                                {
                                    if (item.NotificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                                    {
                                        IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserProfile.Id) };
                                        SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                            notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                            notificationUsers, "ar");
                                    }
                                }
                            }
                        }
                    }
                    foreach (var originalTransactionCopy in transactionCopiesBeforeUpdate.Where(c => c.Id != 0).ToList())
                    {
                        if (!transaction.Copies.Any(c => c.Id == originalTransactionCopy.Id))
                        {
                            List<int> userIds = new List<int>();
                            notificationSource = NotificationSource.DeleteElectronicCopies;
                            notificationTemplateType = NotificationTemplateType.DeleteElectronicCopiesWeb;
                            notificationEmailTemplateType = NotificationTemplateType.DeleteElectronicCopiesEmail;
                            notificationEmailSubject = NotificationEmailSubject.DeleteElectronicCopiesEmail;
                            notificationWebSubject = NotificationWebSubject.DeleteElectronicCopies;
                            if (originalTransactionCopy.UserId.HasValue)
                            {
                                userIds.Add(originalTransactionCopy.UserId.Value);
                            }
                            else if (originalTransactionCopy.EntityId.HasValue)
                            {
                                OrgUnitBL orgUnitBL = new OrgUnitBL();
                                var userProfiles = orgUnitBL.GetUsersByParentId(originalTransactionCopy.EntityId.Value, "ar");
                                foreach (var user in userProfiles)
                                {
                                    userIds.Add(user.Id);
                                }
                            }
                            if (userIds.Count > 0)
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();
                                var userPreferenceInfos = userManagementBL.GetUserPreferenceByUserIds(userIds);
                                var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                                foreach (var item in userPreferenceInfos)
                                {
                                    if (item.NotificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                                    {
                                        IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserProfile.Id) };
                                        SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                            notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                            notificationUsers, "ar");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (transactionBeforeUpdate != null && transactionCopiesBeforeUpdate != null && transactionCopiesBeforeUpdate.Count > 0 && (transaction.Copies == null || transaction.Copies.Count == 0))
                {
                    List<int> userIds = new List<int>();
                    foreach (var copy in transaction.Copies)
                    {
                        notificationSource = NotificationSource.DeleteElectronicCopies;
                        notificationTemplateType = NotificationTemplateType.DeleteElectronicCopiesWeb;
                        notificationEmailTemplateType = NotificationTemplateType.DeleteElectronicCopiesEmail;
                        notificationEmailSubject = NotificationEmailSubject.DeleteElectronicCopiesEmail;
                        notificationWebSubject = NotificationWebSubject.DeleteElectronicCopies;
                        if (copy.UserId.HasValue)
                        {
                            userIds.Add(copy.UserId.Value);
                        }
                        else if (copy.EntityId.HasValue)
                        {
                            OrgUnitBL orgUnitBL = new OrgUnitBL();
                            var userProfiles = orgUnitBL.GetUsersByParentId(copy.EntityId.Value, "ar");
                            foreach (var user in userProfiles)
                            {
                                userIds.Add(user.Id);
                            }
                        }
                        if (userIds.Count > 0)
                        {
                            IUserManagementBL userManagementBL = new UserManagementBL();
                            var userPreferenceInfos = userManagementBL.GetUserPreferenceByUserIds(userIds);
                            var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                            foreach (var item in userPreferenceInfos)
                            {
                                if (item.NotificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                                {
                                    IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(item.UserProfile.Id) };
                                    SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                        notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                        notificationUsers, "ar");
                                }
                            }
                        }
                    }
                }
                transactionRepository.UpdateTransactionCopies(transaction.Id, transaction.Copies);
            }

            if (transaction.ExternalCopies != null && User.HasClaim(UserClaims.CopiesExternal.Delete))
            {
                transactionRepository.UpdateTransactionExternalCopies(transaction.Id, transaction.ExternalCopies);
            }

            if (transaction.Attachments != null /*&& User.HasClaim(UserClaims.Outbound.DeleteAttachments)*/)
            {
                UpdateTransactionAttachments(transaction);
            }
        }
        public void CreateExternalOutbound(Transaction transaction, UserProfile userProfile, OutboundDraftStatus outboundDraftStatus)
        {
            try
            {
                using (System.Transactions.TransactionScope oTransactionScope = new System.Transactions.TransactionScope())
                {
                    ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                    ILookupBL lookupBL = new LookupBL();
                    transaction.TransactionCategory = lookupBL.GetLookupItem(TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty));
                    transactionRepository.UpdateTransaction(transaction);
                    MoveTransactionToTray(transaction);
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
        public override string GetSourceName(Transaction transaction, string cultureName)
        {
            try
            {
                return string.Empty;
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
        public override string GetDestinationName(Transaction transaction, string cultureName)
        {
            try
            {
                return string.Empty;
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
        protected override void IncrementTransactionCounter(Transaction transaction)
        {
            //transaction.Number = OutboundDraftCounter.Instance.Next();
            TransactionCategories transactionCategories = TransactionCategories.DraftOutbound;
            if (transaction.IsPresentationDraft)
            {
                if (transaction.ExternalPartyId.HasValue)
                {
                    transactionCategories = TransactionCategories.Outbound;
                    transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);
                }
                else if (transaction.EntityId.HasValue && !transaction.ExternalPartyId.HasValue)
                {
                    transactionCategories = TransactionCategories.InternalOutbound;
                    transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);
                }
            }
            else
            {
                transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);
            } 
        }
        public  void UpdatePresentationDraftNumber(Transaction transaction)
        {
            //transaction.Number = OutboundDraftCounter.Instance.Next();
            TransactionCategories transactionCategories = TransactionCategories.DraftOutbound;
            if (transaction.IsPresentationDraft)
            {
                if (transaction.ExternalPartyId.HasValue)
                {
                    transactionCategories = TransactionCategories.Outbound;
                    transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);
                }
                else if (transaction.EntityId.HasValue && !transaction.ExternalPartyId.HasValue)
                {
                    transactionCategories = TransactionCategories.InternalOutbound;
                    transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);
                }
            }
            else
            {
                transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);
            }
        }
        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.DraftOutbound; }
        }
        private void SaveDeliveryReport(int transactionHistoryId, Transaction transaction, int orgUnitId = 0)
        {

            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id, false, true);

            int? reporterId = transaction.ReporterId;
            int transactionId = transaction.Id;
            if (reporterId.HasValue && reporterId.Value == 0)
            {
                reporterId = null;
            }
            transactionDeliveryReportBL.AddTransactionDeliveryReport(new TransactionDeliveryReport()
            {
                Date = DateTime.Now,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                TransactionHistoryId = transactionHistoryId,
                UserId = User.Id,
                TransactionId = transactionId,
                ReporterId = reporterId,
                OrgunitId = orgUnitId
            });




            if (transaction.Copies.Count > 0)
            {
                foreach (TransactionCopy copy in transaction.Copies)
                {
                    int? CopyId = transactionDeliveryReports != null && transactionDeliveryReports.Count > 0 ?
                              transactionDeliveryReports.Where(x => x.TransactionCopyId.HasValue && x.TransactionCopyId.Value == copy.Id).FirstOrDefault()?.Id : null;

                    if (CopyId == null)
                    {
                        transactionDeliveryReportBL.AddTransactionDeliveryReport(new TransactionDeliveryReport()
                        {
                            Date = DateTime.Now,
                            DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                            TransactionHistoryId = transactionHistoryId,
                            UserId = User.Id,
                            TransactionId = transactionId,
                            ReporterId = reporterId,
                            OrgunitId = orgUnitId,
                            TransactionCopyId = copy.Id
                        });
                    }
                }
            }

            //add external copy
            if (transaction.ExternalCopies != null && transaction.ExternalCopies.Count > 0)
            {
                foreach (TransactionExternalCopy externalCopy in transaction.ExternalCopies)
                {
                    int? externalCopyId = transactionDeliveryReports != null && transactionDeliveryReports.Count > 0 ?
                           transactionDeliveryReports.Where(x => x.TransactionExternalCopyId.HasValue && x.TransactionExternalCopyId.Value == externalCopy.Id).FirstOrDefault()?.Id
                           : null;

                    if (externalCopyId == null)
                    {
                        transactionDeliveryReportBL.AddTransactionDeliveryReport(new TransactionDeliveryReport()
                        {
                            Date = DateTime.Now,
                            DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                            TransactionHistoryId = transactionHistoryId,
                            UserId = User.Id,
                            TransactionId = transactionId,
                            ReporterId = reporterId,
                            OrgunitId = orgUnitId,
                            TransactionExternalCopyId = externalCopy.Id
                        });
                    }
                }
            }
        }
        private void SendTransactionNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
   NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
               IList<NotificationUser> notificationUsers, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransTypeId}"] = transaction.TransactionCategoryId.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{sender}"] = User.UserName;
                keyValues["{Date}"] = transaction.DateH;
                keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{TransactionId}"] = transaction.Id.ToString();
                keyValues["{UserName}"] = User.UserName;
                keyValues["{OrgName}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transaction.OrgUnitId, cultureName);

                //System Notification Web
                NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);

                //System Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                }
                else
                {
                    var notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                    //System Notification  Email
                    NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsersEmail, cultureName, null, keyValues);
                }
            }
        }
    }
}
