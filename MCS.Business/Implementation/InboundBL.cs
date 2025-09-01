using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class InboundBL : TransactionBL, IInboundBL
    {
        protected override void Validate(Transaction transaction)
        {
            if (transaction.DocumentNumber == null)
            {
                return;
            }

            int? transactionId = GetTransactionId(transaction.DocumentNumber, transaction.ExternalPartyId ?? 0, transaction.TransactionTypeId ?? 0);

            if (transactionId != null && transaction.Id != transactionId)
            {
                throw new BusinessException(StatusCode.DocumentNumberAlreadyExist);
            }
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
                    throw new BusinessException(StatusCode.TransactionDoubleLinked);
            });

            if (!User.HasClaim(UserClaims.Inbound.EditInbound) && !User.HasClaim(UserClaims.Inbound.Editor))
            {
                throw new BusinessException(StatusCode.PermissionEditInbound);
            }
            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);
            if (User.HasClaim(UserClaims.Names.Add))
            {
                UpdateTransactionNames(transaction);
            }

            if (transaction.Attachments != null && transaction.Attachments.Count > 0)
            {
                UpdateTransactionAttachments(transaction);
            }

            if (transaction.Copies != null && User.HasClaim(UserClaims.CopiesInternal.Delete))
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

                NotificationSource notificationSource = NotificationSource.None;
                NotificationTemplateType notificationTemplateType = NotificationTemplateType.None;
                NotificationTemplateType notificationEmailTemplateType = NotificationTemplateType.None;
                NotificationEmailSubject notificationEmailSubject = NotificationEmailSubject.None;
                NotificationWebSubject notificationWebSubject = NotificationWebSubject.None;
                IUserManagementBL userManagementBL = new UserManagementBL();
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
                    var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);

                    List<int> userIds = new List<int>();

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

                            userIds.Add(copy.UserId.Value);
                        }
                    }
                }

                //There are copies added before
                else if (transactionBeforeUpdate != null && transactionCopiesBeforeUpdate != null && transactionCopiesBeforeUpdate.Count > 0)
                {
                    var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);

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
                            List<int> userIds = new List<int>();
                            notificationSource = NotificationSource.ElectronicCopies;
                            notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                            notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                            notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                            notificationWebSubject = NotificationWebSubject.ElectronicCopies;

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
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                transactionRepository.UpdateTransactionExternalCopies(transaction.Id, transaction.ExternalCopies);
            }

            CheckTransactionSourceTypePermission(transaction);
        }

        protected override void PreSave(Transaction transaction)
        {
            transaction.Links.ToList().ForEach(l =>
            {
                int count = transaction.Links.ToList().Where(tl => tl.ToTransactionId == l.ToTransactionId).ToList().Count;
                if (count > 1)
                    throw new BusinessException(StatusCode.TransactionDoubleLinked);
            });

            if (!User.HasClaim(UserClaims.Inbound.CreateInbound))
            {
                throw new BusinessException(StatusCode.PermissionCreateInbound);
            }

            if (transaction.Names != null && transaction.Names.Count > 0 && !User.HasClaim(UserClaims.Names.Add))
            {
                throw new BusinessException(StatusCode.PermissionInboundAddNames);
            }

            //if (transaction.Attachments != null && transaction.Attachments.Count > 0 && !User.HasClaim(UserClaims.Inbound.AddAttachments))
            //{
            //    throw new BusinessException(StatusCode.PermissionInboundAddAttachments);
            //}

            CheckTransactionSourceTypePermission(transaction);

            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);

            IncrementTransactionCounter(transaction);

            base.PreSave(transaction);
        }

        public override string GetDestinationName(Transaction transaction, string cultureName)
        {
            try
            {
                string destinationName = string.Empty;

                Localization localization = transaction.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

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

                Localization localization = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

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

        protected override Barcode AddTransactionCopiesBarcode(Transaction transaction)
        {
            return base.AddTransactionCopiesBarcode(transaction);
        }

        public override TransactionCategory TransactionCategory
        {
            get { return TransactionCategory.Inbound; }
        }

        private int? GetTransactionId(string documentNumber, int ExternalPartyId, int transactionTypeId)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.GetTransactionId(t => t.DocumentNumber == documentNumber && t.ExternalParty.Id == ExternalPartyId && t.TransactionType.Id == transactionTypeId);
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
