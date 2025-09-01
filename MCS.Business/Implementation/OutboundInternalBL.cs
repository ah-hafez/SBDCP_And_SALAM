using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;


namespace MCS.Business
{
    public class OutboundInternalBL : TransactionBL, IOutboundInternalBL
    {
        protected override void Validate(Transaction transaction)
        {
        }

        protected override void PreSave(Transaction transaction)
        {
            transaction.Links.ToList().ForEach(l =>
            {
                int count = transaction.Links.ToList().Where(tl => tl.ToTransactionId == l.ToTransactionId).ToList().Count;
                if (count > 1)
                    throw new BusinessException(StatusCode.TransactionDoubleLinked);
            });

            if (!User.HasClaim(UserClaims.Outbound.CreateInternalOutbound))
            {
                throw new BusinessException(StatusCode.PermissionOutbound);
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
            CheckTransactionSourceTypePermission(transaction);

            IncrementTransactionCounter(transaction);

            base.PreSave(transaction);
        }

        protected override void PostSave(Transaction transaction, byte[] content = null)
        {
            base.PostSave(transaction);

            if (transaction.GroupId.HasValue)
            {
                HandlePopulariazationTransaction(transaction);
            }
        }

        protected override void PreUpdate(Transaction transaction)
        {
            if (transaction.Links.Where(tl => tl.ToTransactionId == transaction.Id).FirstOrDefault() != null)
                throw new BusinessException(StatusCode.TransactionCycleLinked);

            transaction.Links.ToList().ForEach(l =>
            {
                int count = transaction.Links.ToList().Where(tl => tl.ToTransactionId == l.ToTransactionId).ToList().Count;

                if (count > 1)
                    throw new BusinessException(StatusCode.TransactionDoubleLinked);
            });

            if (!User.HasClaim(UserClaims.Outbound.EditOutbound) && !User.HasClaim(UserClaims.Outbound.EditorInternalOutbound))
            {
                throw new BusinessException(StatusCode.PermissionOutboundEditOutbound);
            }

            CheckTransactionConfidentiality(transaction.ConfidentialityId, transaction.Id, User.Id);
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
                    notificationSource = NotificationSource.ElectronicCopies;
                    notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                    notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                    notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                    notificationWebSubject = NotificationWebSubject.ElectronicCopies;


                    IUserManagementBL userManagementBL = new UserManagementBL();
                    var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
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
                                var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                                IUserManagementBL userManagementBL = new UserManagementBL();
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
                            IUserManagementBL userManagementBL = new UserManagementBL();
                            notificationSource = NotificationSource.ElectronicCopies;
                            notificationTemplateType = NotificationTemplateType.ElectronicCopiesWeb;
                            notificationEmailTemplateType = NotificationTemplateType.ElectronicCopiesEmail;
                            notificationEmailSubject = NotificationEmailSubject.ElectronicCopiesEmail;
                            notificationWebSubject = NotificationWebSubject.ElectronicCopies;
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

            base.PreUpdate(transaction);
        }

        protected override void PostUpdate(Transaction transaction)
        {
            base.PostUpdate(transaction);

            if (SystemConfigurations.IsSolrIndexingEnabled)
            {
                if (transaction.Priority == null)
                {
                    IPriorityBL priorityBL = new PriorityBL();

                    transaction.Priority = priorityBL.GetPriorityById(transaction.PriorityId);
                }

                if (transaction.Confidentiality == null)
                {
                    IPermissionBL permissionBL = new PermissionBL();

                    transaction.Confidentiality = permissionBL.GetPermissionById(transaction.ConfidentialityId);
                }

                if (transaction.ExternalParty == null && transaction.ExternalPartyId.HasValue)
                {
                    IExternalPartyBL externalPartyBL = new ExternalPartyBL();

                    transaction.ExternalParty = externalPartyBL.GetExternalPartyById(transaction.ExternalPartyId.Value);

                    if (transaction.ExternalPartyManager == null && transaction.ExternalPartyManagerId.HasValue)
                    {
                        transaction.ExternalPartyManager = externalPartyBL.GetExternalPartyManagerById(transaction.ExternalPartyManagerId.Value);
                    }
                }

                if (transaction.TransactionType == null && transaction.TransactionTypeId.HasValue)
                {
                    ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();

                    transaction.TransactionType = transactionTypeBL.GetTransactionSourceTypeById(transaction.TransactionTypeId.Value);
                }

                if (transaction.Status == null)
                {
                    ILookupBL lookupBL = new LookupBL();

                    transaction.Status = lookupBL.GetLookupItem(transaction.StatusId);
                }

                if (transaction.SignedByUser == null && transaction.SignedByUserId.HasValue)
                {
                    IUserManagementBL userManagementBL = new UserManagementBL();

                    transaction.SignedByUser = userManagementBL.GetUserById(transaction.SignedByUserId.Value);
                }

                if (transaction.TransactionCategory == null)
                {
                    ILookupBL lookupBL = new LookupBL();

                    transaction.TransactionCategory = lookupBL.GetLookupItem(transaction.TransactionCategoryId);
                }
            }

            if (transaction.GroupId.HasValue)
            {
                HandlePopulariazationTransaction(transaction);
            }
        }

        public void GetNewInternalOutboundNumber(Transaction transaction)
        {
            IncrementTransactionCounter(transaction);
        }

        public override string GetDestinationName(Transaction transaction, string cultureName)
        {
            try
            {
                string destinationName = string.Empty;

                Localization localization = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

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
            get { return TransactionCategory.InternalOutbound; }
        }

        protected override Barcode AddTransactionCopiesBarcode(Transaction transaction)
        {
            return base.AddTransactionCopiesBarcode(transaction);
        }

        private void HandlePopulariazationTransaction(Transaction transaction)
        {
            ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
            List<TransactionAssignment> transactionAssignments = new List<TransactionAssignment>();
            List<Transaction> transactions = new List<Transaction>();

            transactions.Add(transaction);

            AssignmentGroup assignmentGroup = userManagementBL.GetAssignmentGroupById(transaction.GroupId.Value);

            if (assignmentGroup == null)
                throw new BusinessException(StatusCode.AssignmentGroupNotFound);

            IList<Domain.Action> processes = OrgUnitBL.GetOrgUnitActions(transaction.OrgUnitId);

            if (processes == null)
                throw new BusinessException(StatusCode.ActionNotFound);

            int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
            Domain.Action process = processes.Where(a => a.Type.Id == SendCopyToView).FirstOrDefault();

            if (process == null)
                throw new BusinessException(StatusCode.ActionSendCopyToViewNotFound);

            assignmentGroup.AssignmentGroupDetails.ToList().ForEach(a =>
            {
                TransactionAssignment transactionAssignment = new TransactionAssignment();

                transactionAssignment.FromEntityId = transaction.OrgUnitId;
                transactionAssignment.ToEntityId = a.OrgUnit.Id;
                transactionAssignment.IsPopulariazation = true;
                transactionAssignment.ActionId = process.Id;

                if (a.UserProfile != null)
                {
                    transactionAssignment.ToUserId = a.UserProfile.Id;
                }

                transactionAssignments.Add(transactionAssignment);
            });

            transactionAssignmentBL.AssignTransaction(transactions, transactionAssignments);

            foreach (var trans in transactions)
            {
                notificationBL.SendAssignmentNotification(trans, transactionAssignments);
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
