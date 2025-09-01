using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Business
{
    public class OutboundExternalBL : TransactionBL, IOutboundExternalBL
    {
        protected override void Validate(Transaction transaction)
        {
            string TodayDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            Transaction existTransaction = GetTransaction(t => t.TransactionCategoryId == transaction.TransactionCategoryId && t.ExternalPartyId == transaction.ExternalPartyId && t.Subject == transaction.Subject && t.DateH == TodayDateH);

            if (existTransaction != null && existTransaction.Id != transaction.Id)
            {
                throw new BusinessException(StatusCode.TransactionAlreadyExist);
            }
        }

        protected override void PreSave(Transaction transaction)
        {
            transaction.Links.ToList().ForEach(l =>
            {
                int count = transaction.Links.ToList().Where(tl => tl.ToTransactionId == l.ToTransactionId).ToList().Count;
                if (count > 1)
                {
                    throw new BusinessException(StatusCode.TransactionDoubleLinked);
                }
            });

            if (!User.HasClaim(UserClaims.Outbound.CreateExternalOutbound))
            {
                throw new BusinessException(StatusCode.PermissionOutbound);
            }

            if (transaction.Names != null && transaction.Names.Count > 0 && !User.HasClaim(UserClaims.Names.Add))
            {
                throw new BusinessException(StatusCode.PermissionOutboundAddNames);
            }

            //if (transaction.Attachments != null /*&& !User.HasClaim(UserClaims.Outbound.AddAttachments)*/)
            //{
            //    throw new BusinessException(StatusCode.PermissionOutboundAddAttachments);
            //}

            if (transaction.Copies != null && transaction.Copies.Count > 0 && !User.HasClaim(UserClaims.CopiesInternal.Add))
            {
                throw new BusinessException(StatusCode.PermissionOutboundAddCopies);
            }

            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);
            CheckTransactionSourceTypePermission(transaction);

            if (transaction.Number == 0)
                IncrementTransactionCounter(transaction);

            base.PreSave(transaction);
        }

        protected override void PostSave(Transaction transaction, byte[] content = null)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            MoveTransaction(transaction);
            int transactionHistoryId = AddTransactionHistory(transaction);
            AddTransactionEntityDetails(transaction);

            //SendExternalPartiyEmail(transaction, content);

            AddTransactionCopiesBarcode(transaction);
            int orgUnitId = transaction.OrgUnit.Id;

            SaveDeliveryReport(transactionHistoryId, transaction, orgUnitId);

            //Notification => Web
            if (transaction.Copies != null && transaction.Copies.Count > 0)
            {
                var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                foreach (var copy in transaction.Copies)
                {
                    if (copy.UserId.HasValue)
                    {
                        NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(copy.UserId.Value);
                        if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                        {
                            IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(copy.UserId.Value) };
                            //Notification => Email
                            SendTransactionNotification(transactionForNotification, NotificationSource.ElectronicCopies, NotificationTemplateType.ElectronicCopiesWeb,
                                NotificationTemplateType.ElectronicCopiesEmail, NotificationEmailSubject.ElectronicCopiesEmail, NotificationWebSubject.ElectronicCopies,
                                notificationUsers, "ar");
                        }

                    }
                }
            }
        }
        protected override void PostUpdate(Transaction transaction)
        {
            AddTransactionHistory(transaction);
            AddTransactionEntityDetails(transaction);
            UpdateTransactionDeliveryReportCopies(transaction.Id, transaction.ReporterId);
        }
        protected override void PreUpdate(Transaction transaction)
        {
            if (transaction.Links.Where(tl => tl.ToTransactionId == transaction.Id).FirstOrDefault() != null)
            {
                throw new BusinessException(StatusCode.TransactionCycleLinked);
            }

            transaction.Links.ToList().ForEach(l =>
            {
                int count = transaction.Links.ToList().Where(tl => tl.ToTransactionId == l.ToTransactionId).ToList().Count;

                if (count > 1)
                {
                    throw new BusinessException(StatusCode.TransactionDoubleLinked);
                }
            });

            if (!User.HasClaim(UserClaims.Outbound.EditOutbound))
            {
                throw new BusinessException(StatusCode.PermissionOutboundEditOutbound);
            }
            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);
            //CheckDeliveryReportPrinted(transaction);

            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            if (User.HasClaim(UserClaims.Names.Delete))
            {
                UpdateTransactionNames(transaction);
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
                    var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                    notificationSource = NotificationSource.ElectronicCopies;
                    notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                    notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                    notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                    notificationWebSubject = NotificationWebSubject.ElectronicCopies;

                    IUserManagementBL userManagementBL = new UserManagementBL();

                    foreach (var copy in transaction.Copies)
                    {
                        if (copy.UserId.HasValue)
                        {
                            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(copy.UserId.Value);
                            if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                            {
                                IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(copy.UserId.Value) };
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

                                notificationSource = NotificationSource.EditElectronicCopies;
                                notificationTemplateType = NotificationTemplateType.EditElectronicCopiesWeb;
                                notificationEmailTemplateType = NotificationTemplateType.EditElectronicCopiesEmail;
                                notificationEmailSubject = NotificationEmailSubject.EditElectronicCopiesEmail;
                                notificationWebSubject = NotificationWebSubject.EditElectronicCopies;
                                IUserManagementBL userManagementBL = new UserManagementBL();
                                var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                                if (copy.UserId.HasValue)
                                {
                                    NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(copy.UserId.Value);

                                    if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                                    {
                                        IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(copy.UserId.Value) };
                                        SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                            notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                            notificationUsers, "ar");
                                    }
                                }
                            }
                        }
                        //Added item
                        else
                        {

                            notificationSource = NotificationSource.ElectronicCopies;
                            notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                            notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                            notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                            notificationWebSubject = NotificationWebSubject.ElectronicCopies;
                            IUserManagementBL userManagementBL = new UserManagementBL();
                            var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                            if (copy.UserId.HasValue)
                            {
                                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(copy.UserId.Value);

                                if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                                {
                                    IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(copy.UserId.Value) };
                                    SendTransactionNotification(transactionForNotification, notificationSource, notificationTemplateType,
                                        notificationEmailTemplateType, notificationEmailSubject, notificationWebSubject,
                                        notificationUsers, "ar");
                                }
                            }
                        }
                    }

                }
                transactionRepository.UpdateTransactionCopies(transaction.Id, transaction.Copies);
                transactionRepository.UpdateProcessPeriodTransaction(transaction.Id, transaction.ProcessPeriodTransaction);
            }

            if (transaction.ExternalCopies != null && User.HasClaim(UserClaims.CopiesExternal.Delete))
            {
                transactionRepository.UpdateTransactionExternalCopies(transaction.Id, transaction.ExternalCopies);
            }

            if (transaction.Attachments != null /*&& User.HasClaim(UserClaims.Outbound.DeleteAttachments)*/)
            {
                UpdateTransactionAttachments(transaction);
            }

            CheckTransactionSourceTypePermission(transaction);
        }

        public override string GetDestinationName(Transaction transaction, string cultureName)
        {
            try
            {
                string destinationName = string.Empty;

                Localization localization = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                if (localization != null)
                {
                    destinationName = localization.Text;
                }

                return destinationName;
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
                string sourceName = string.Empty;

                Localization localization = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                if (localization != null)
                {
                    sourceName = localization.Text;
                }

                return sourceName;
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

        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.ExternalOutbound; }
        }

        protected override IList<DeliveryReportInfoDTO> OnDeliveryReport(Transaction transaction, string cultureName, bool perTransaction = true)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();

            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

            ITransactionBL transactionBL = Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

            DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
            {
                TransactionNumber = transaction.Number,
                AttachmentCount = transaction.Attachments.Count,
                ToEntity = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                DateH = transaction.DateH,
                TransactionCategory = transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
            };

            deliveryReportTransactions.Add(deliveryReportTransaction);

            DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
            {
                ReportNumber = DeliveryReportCounter.GetInstance().Next().ToString(),
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                UserName = userManagementBL.GetUserName(User.Id, cultureName),
                DeliveryReportTransactions = deliveryReportTransactions
            };

            deliveryReports.Add(deliveryReport);

            return deliveryReports;
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
        private void UpdateTransactionDeliveryReportCopies(int transactionId, int? reporterId)
        {

            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            if (reporterId.HasValue && reporterId.Value == 0)
            {
                reporterId = null;
            }
            transactionDeliveryReportBL.UpdateTransactionDeliveryReportCopies(transactionId, reporterId);
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

        public void GetNewExternalOutboundNumber(Transaction transaction)
        {
            IncrementTransactionCounter(transaction);
        }
    }
}
