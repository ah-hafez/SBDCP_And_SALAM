using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Framework.Localization.SupportClasses;
using System.Web;

namespace MCS.Business
{
    public class EditorBL : BaseBL, IEditorBL
    {
        public void AddTransactionLinks(int transactionId, IList<TransactionLink> Links)
        {
            try
            {
                Transaction transaction = TransactionBL.GetTransactionById(transactionId);
                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

                transactionBL.AddTransactionLinks(transactionId, Links);
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

        public void UpdateTransactionDocument(int transactionId, DocumentInfo documentInfo)
        {
            try
            {
                TransactionBL.UpdateTransactionDocument(transactionId, documentInfo);
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


        public TransactionBasicInfo GetTransactionBasicInfo(int transactionId, string cultureName)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}

                Transaction transaction =
                    TransactionBL.GetTransactionBasicInfoById(transactionId, cultureName);

                if (transaction == null)
                { //Nasser Removed if type==outboundexternal
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                TransactionBasicInfo transactionBasicinfo = new TransactionBasicInfo()
                {
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Number = transaction.Number,
                    Remarks = transaction.Remarks,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    Subject = transaction.Subject,
                    DocumentNumber = transaction.DocumentNumber,
                    TransactionCategoryId = transaction.TransactionCategory.Id,
                    ConfidentialityName = (transaction.Confidentiality != null) ? transaction.Confidentiality.LocalName : null,
                    ConfidentialityId = (transaction.Confidentiality != null) ? transaction.Confidentiality.Id : -1,
                    ExternalPartyName = (transaction.ExternalParty != null) ? transaction.ExternalParty.LocalName : null,
                    ExternalPartyId = (transaction.ExternalParty != null) ? transaction.ExternalParty.Id : -1,
                    ExternalPartyManagerName = (transaction.ExternalPartyManager != null) ? transaction.ExternalPartyManager.LocalName : null,
                    ExternalPartyManagerId = (transaction.ExternalPartyManager != null) ? transaction.ExternalPartyManager.Id : -1,
                    LetterTypeName = (transaction.LetterType != null) ? transaction.LetterType.Text : null,
                    LetterTypeId = (transaction.LetterType != null) ? transaction.LetterType.Id : -1,
                    PriorityName = (transaction.Priority != null) ? transaction.Priority.Text : null,
                    PriorityId = (transaction.Priority != null) ? transaction.Priority.Id : -1,
                    SignedByUserName = (transaction.SignedByUser != null) ? transaction.SignedByUser.LocalName : null,
                    SignedByUserId = (transaction.SignedByUser != null) ? transaction.SignedByUser.Id : -1,
                    TransactionTypeName = (transaction.TransactionType != null) ? transaction.TransactionType.Text : null,
                    TransactionTypeId = (transaction.TransactionType != null) ? transaction.TransactionType.Id : -1,
                    ToEntityName = (transaction.Entity != null) ? transaction.Entity.LocalName : null,
                    ToUserName = (transaction.ToUser != null) ? transaction.ToUser.LocalName : null,
                    OutboundDraftId = transaction.OutboundDraftId,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    SuggestedTopicId = (transaction.SuggestedTopic != null) ? transaction.SuggestedTopic.Id : -1,
                    DeliveryMethodId = transaction.DeliveryMethod != null ? transaction.DeliveryMethod.Id : -1,
                    DeliveryMethod = transaction.DeliveryMethod != null ? transaction.DeliveryMethod.Text : string.Empty,
                    PostCode = transaction.PostCode,
                    POBox = transaction.POBox,
                    YearH = transaction.YearH,
                    Year = transaction.Year,
                    Links = transaction.Links,
                    Attachments = transaction.Attachments
                };

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    transactionBasicinfo.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s => transactionBasicinfo.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                return transactionBasicinfo;
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

        public void UpdateTransactionBasicInfo(int transactionId, TransactionBasicInfo transactionBasicInfo)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}

                if (transactionBasicInfo.TransactionCategoryId != TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    throw new BusinessException(StatusCode.ConnotUpdateTransactionBasicInfo);
                }

                Transaction transaction = new Transaction()
                {
                    Id = transactionId,
                    ExternalPartyId = transactionBasicInfo.ExternalPartyId,
                    ExternalPartyManagerId = transactionBasicInfo.ExternalPartyManagerId,
                    ConfidentialityId = transactionBasicInfo.ConfidentialityId,
                    PriorityId = transactionBasicInfo.PriorityId,
                    TransactionTypeId = transactionBasicInfo.TransactionTypeId,
                    SignedByUserId = transactionBasicInfo.SignedByUserId,
                    Remarks = transactionBasicInfo.Remarks,
                    Subject = transactionBasicInfo.Subject,
                    RemindDate = transactionBasicInfo.RemindDate,
                    RemindDateH = transactionBasicInfo.RemindDateH,
                    LetterTypeId = transactionBasicInfo.LetterTypeId,
                    SuggestedTopicId = transactionBasicInfo.SuggestedTopicId,
                    DeliveryMethodId = transactionBasicInfo.DeliveryMethodId,
                    POBox = transactionBasicInfo.POBox,
                    PostCode = transactionBasicInfo.PostCode,
                    LetterNumber = transactionBasicInfo.LetterNumber
                };

                if (transactionBasicInfo.SubjectClassifications != null
                    && transactionBasicInfo.SubjectClassifications.Count > 0)
                {
                    transaction.SubjectClassifications = new List<TransactionSubjectClassification>();
                    transactionBasicInfo.SubjectClassifications.ForEach(s =>
                        transaction.SubjectClassifications
                        .Add(new TransactionSubjectClassification { SubjectClassificationId = s }
                        ));
                }

                TransactionBL.UpdateTransactionBasicInfo(transaction);
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

        public IList<TransactionLink> GetTransactionLinks(int transactionId, string cultureName)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}

                return TransactionBL.GetTransactionLinksById(transactionId, cultureName);
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

        public Transaction GetTransaction(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}

                Transaction transaction = TransactionBL.GetTransaction(transactionId, User.Id, OrgUnitId, cultureName);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                //ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                IUserManagementBL userManagementBL = new UserManagementBL();
                //transactionAssignmentBL.SetTransactionAssignmentToViewedByTransactionId(transactionId);
                if ((DeliveryMethodType)transaction.DeliveryMethodId.LookupInternalID(LookupCategory.DeliveryMethod, cultureName) == DeliveryMethodType.ElectronicPaper &&
                    transaction.CreatedBy.Value != User.Id)
                {
                    NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(transaction.CreatedBy.Value, cultureName);


                    if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ReceiveReport))
                    {
                        var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(transaction.CreatedBy.Value) };
                        SendReceiveTransactionNotification(transaction, NotificationSource.ReceiveReport, NotificationTemplateType.ReceiveReportWeb,
                            NotificationTemplateType.ReceiveReportEmail, NotificationEmailSubject.ReceiveReportEmail, NotificationWebSubject.ReceiveReport,
                            notificationUsers, cultureName);
                    }
                }
                return transaction;
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

        public Transaction GetTransaction_VIP(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {


                Transaction transaction = TransactionBL.GetTransaction_VIP(transactionId, User.Id, OrgUnitId, cultureName);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                //ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                //transactionAssignmentBL.SetTransactionAssignmentToViewedByTransactionId(transactionId);
                if ((DeliveryMethodType)transaction.DeliveryMethodId.LookupInternalID(LookupCategory.DeliveryMethod, cultureName) == DeliveryMethodType.ElectronicPaper &&
                    transaction.CreatedBy.Value != User.Id)
                {
                    NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(transaction.CreatedBy.Value, cultureName);

                    if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ReceiveReport))
                    {
                        var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(transaction.CreatedBy.Value) };
                        SendReceiveTransactionNotification(transaction, NotificationSource.ReceiveReport, NotificationTemplateType.ReceiveReportWeb,
                            NotificationTemplateType.ReceiveReportEmail, NotificationEmailSubject.ReceiveReportEmail, NotificationWebSubject.ReceiveReport,
                            notificationUsers, cultureName);
                    }
                }
                return transaction;
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



        public Transaction GetTransactionLight(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}
                Transaction transaction = TransactionBL.GetTransactionLight(transactionId, User.Id, OrgUnitId, cultureName);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }
                return transaction;
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

        public Transaction GetByTransactionNumber(int transactionNumber)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}

                Transaction transaction = TransactionBL.GetByTransactionNumber(transactionNumber);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                if (transaction.TransactionCategory.Id == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                return transaction;
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

        public DocumentInfo GetMainDocumentByTransactionId(int transactionId)
        {
            try
            {
                //if (!User.HasClaim(UserClaims.Editor.ViewTransactions))
                //{
                //    throw new BusinessException(StatusCode.PermissionEditorViewTransactions);
                //}

                DocumentInfo documentInfo = TransactionBL.GetMainDocumentByTransactionId(transactionId);

                return documentInfo;
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

        public DocumentInfo GetOldMainDocumentByTransactionId(int transactionId)
        {
            try
            {

                DocumentInfo documentInfo = TransactionBL.GetOldMainDocumentByTransactionId(transactionId);

                return documentInfo;
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

        public TransactionDetails AddTransactionDraft(int transactionId, Transaction transactionDraft)
        {
            try
            {
                {
                    if (!User.HasClaim(UserClaims.Outbound.CreateOutboundDraft))
                    {
                        throw new BusinessException(StatusCode.PermissionEditorDraft);
                    }

                    Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                    if (transaction == null)
                    {
                        throw new BusinessException(StatusCode.TransactionNotFound);
                    }

                    if (transaction.OutboundDraftId > 0)
                    {
                        throw new BusinessException(StatusCode.TransactionDraftAlreadyCreated);
                    }

                    TransactionDetails transactionDetails = null;
                    ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.DraftOutbound);

                    transactionDetails = transactionBL.Save(transactionDraft);

                    transaction.OutboundDraftId = transactionDetails.Id;

                    transactionBL.Update(transaction);

                    return transactionDetails;
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

        public void UpdateTransaction(Transaction transaction)
        {
            try
            {
                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId);

                transactionBL.Update(transaction);
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

        public void AddTasks(int transactionId, List<Task> tasks, string cultureName)
        {
            try
            {
                ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();

                transactionTaskBL.AddTasks(transactionId, tasks, cultureName);
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

        public void AssignTransaction(int transactionId, IList<TransactionAssignment> transactionAssignments, string culturName = "")
        {
            try
            {
                INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
                if (!User.HasClaim(UserClaims.Assignments.Assign))
                {
                    throw new BusinessException(StatusCode.PermissionEditorAssignments);
                }

                Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                foreach (TransactionAssignment transactionAssignment in transactionAssignments)
                {
                    transactionAssignment.Transaction = transaction;
                }

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                IList<Transaction> transactions = new List<Transaction>();

                transactions.Add(transaction);

                transactionAssignmentBL.AssignTransaction(transactions, transactionAssignments, culturName);
                foreach (var trans in transactions)
                {
                    notificationBL.SendAssignmentNotification(trans, transactionAssignments, culturName);
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

        public bool CheckTransactionForAssigne(List<int> transactionIds, IList<TransactionAssignment> transactionAssignments)
        {
            try
            {

                if (!User.HasClaim(UserClaims.Assignments.Assign))
                {
                    throw new BusinessException(StatusCode.PermissionEditorAssignments);
                }

                return TransactionBL.CheckUserHasPermission(transactionIds, transactionAssignments.FirstOrDefault().ToUserId);

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


        public void AssignTransactionWithdrawal(int transactionId, IList<TransactionAssignment> transactionAssignments, string culturName = "")
        {
            try
            {

                if (!User.HasClaim(UserClaims.Assignments.Assign))
                {
                    throw new BusinessException(StatusCode.PermissionEditorAssignments);
                }

                Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                foreach (TransactionAssignment transactionAssignment in transactionAssignments)
                {
                    transaction.EntityId = transactionAssignment.ToEntityId;
                    transaction.ToUserId = transactionAssignment.ToUserId;
                    transactionAssignment.Transaction = transaction;
                }

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                IList<Transaction> transactions = new List<Transaction>();

                transactions.Add(transaction);

                transactionAssignmentBL.AssignTransactionWithdrawal(transactions, transactionAssignments, culturName);
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

        public IList<TransactionCopy> GetTransactionCopiesByTransactionId(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();


                return transactionRepository.GetTransactionCopiesByTransactionId(transactionId, User.Id, cultureName);

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
        public void AddAssignmentCopies(int transactionId, IList<TransactionCopy> Copies)
        {
            try
            {
                foreach (TransactionCopy transactionCopy in Copies)
                {
                    transactionCopy.Date = DateTime.Now;
                    transactionCopy.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                }

                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

                transactionRepository.UpdateAssignmentPaperCopies(transactionId, Copies);

                foreach (TransactionCopy transactionCopy in Copies)
                {
                    ITransactionEntityDetailsRepository transactionEntityDetailsRepository = IoC.Resolve<ITransactionEntityDetailsRepository>();
                    transactionEntityDetailsRepository.AddTransactionEntityDetails(new TransactionEntityDetails() { TransactionId = transactionId, EntityId = transactionCopy.EntityId.Value });
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
        public void AddTransactionCopies(int transactionId, IList<TransactionCopy> Copies)
        {
            try
            {


                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

                transactionRepository.UpdateTransactionCopies(transactionId, Copies);

                foreach (TransactionCopy transactionCopy in Copies)
                {
                    ITransactionEntityDetailsRepository transactionEntityDetailsRepository = IoC.Resolve<ITransactionEntityDetailsRepository>();
                    transactionEntityDetailsRepository.AddTransactionEntityDetails(new TransactionEntityDetails() { TransactionId = transactionId, EntityId = transactionCopy.EntityId.Value });
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

        public void AddEntityDetails(int transactionId, IList<TransactionCopy> Copies)
        {
            try
            {

                foreach (TransactionCopy transactionCopy in Copies)
                {
                    ITransactionEntityDetailsRepository transactionEntityDetailsRepository = IoC.Resolve<ITransactionEntityDetailsRepository>();
                    transactionEntityDetailsRepository.AddTransactionEntityDetails(new TransactionEntityDetails() { TransactionId = transactionId, EntityId = transactionCopy.EntityId.Value });
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

        public int AddTransactionExplanation(int transactionId, Explanation explanation, string cultureName)
        {
            try
            {
                if (!User.HasClaim(UserClaims.ExpalanationsEditor.Add))
                {
                    throw new BusinessException(StatusCode.PermissionEditorExplanations);
                }

                Transaction transaction = TransactionBL.GetTransactionById(transactionId);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                if (explanation.PermissionId <= 0)
                {
                    throw new BusinessException(StatusCode.ExplanationConfidentialityRequired);
                }

                explanation.FromUserId = User.Id;
                explanation.Date = DateTime.Now;
                explanation.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();
                int explanationId = explanationRepository.AddExplanation(transaction, explanation);

                if (explanation.isCopies)
                {

                    IUserManagementBL userManagementBL = new UserManagementBL();
                    var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(transaction.CreatedBy.Value) };
                    NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(User.Id, cultureName);

                    if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Explanation))
                    {
                        SendExplanationNotification(transaction, NotificationSource.AddExplanation, NotificationTemplateType.AddExplanationWeb,
                            NotificationTemplateType.AddExplanationEmail, NotificationEmailSubject.AddExplanationEmail, NotificationWebSubject.AddExplanation,
                            notificationUsers, cultureName);
                    }
                }
                return explanationId;
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

        public void UpdateExplanation(Explanation explanation)
        {
            try
            {
                if (!User.HasClaim(UserClaims.ExpalanationsEditor.Edit))
                {
                    throw new BusinessException(StatusCode.PermissionEditorExplanations);
                }

                if (explanation.PermissionId <= 0)
                {
                    throw new BusinessException(StatusCode.ExplanationConfidentialityRequired);
                }

                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();

                explanationRepository.UpdateExplanation(explanation);
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

        public void DeleteExplanation(int explanationId)
        {
            try
            {
                if (!User.HasClaim(UserClaims.ExpalanationsEditor.Delete))
                {
                    throw new BusinessException(StatusCode.PermissionEditorExplanations);
                }

                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();

                explanationRepository.DeleteExplanation(explanationId);
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

        public IList<Explanation> GetExplanationsByTransactionId(int transactionId, string cultureName)
        {
            try
            {
                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                TransactionAssignment lastTransactionAssignment = transactionAssignmentBL.GetLastTransactionAssignments(transactionId, cultureName);

                IList<Explanation> explanations = explanationRepository.GetExplanationsByTransactionId(transactionId, User.Id, cultureName);

                explanations.ToList().ForEach(e => e.CanBeDeleted = (lastTransactionAssignment != null) ? lastTransactionAssignment.Date < e.Date : false);

                return explanations;
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

        public IList<Explanation> GetExplanationsByTransactionId_New(int transactionId, string cultureName)
        {
            try
            {
                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();
                //ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                //TransactionAssignment lastTransactionAssignment = transactionAssignmentBL.GetLastTransactionAssignments(transactionId, cultureName);

                IList<Explanation> explanations = explanationRepository.GetExplanationsByTransactionIdWithoutContent(transactionId, User.Id, cultureName);
                if (explanations.Count > 0)
                {
                    int MaxExpRowNumber = explanations.Max(e => e.RowNumber);

                    explanations.ToList().ForEach(e => e.CanBeDeleted = (e.FromUser.Id == User.Id && e.RowNumber == MaxExpRowNumber) ? true : false);
                }
                return explanations;
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
        public IList<Explanation> GetExplanationsCertifByTransactionId(int transactionId, string cultureName)
        {
            try
            {
                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                IList<Explanation> explanations = explanationRepository.GetExplanationsByTransactionId(transactionId, User.Id, cultureName);

                explanations.ToList().ForEach(e => e.CanBeDeleted = false);

                return explanations;
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
        public Explanation GetExplanationById(int explanationId, string cultureName)
        {
            try
            {
                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();

                return explanationRepository.GetExplanationById(explanationId, cultureName);
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
        public Explanation GetExplanationByDocumentId(int DocumentId, string cultureName)
        {
            try
            {
                IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();

                return explanationRepository.GetExplanationByDocumentId(DocumentId, cultureName);
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

        public Attachment GetAttachmentById(int attachmentId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

                return null;
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

        public Transaction GetInboundTransaction(int transactionId, string cultureName)
        {
            try
            {
                Transaction transaction = TransactionBL.GetUserTransactionById(transactionId, cultureName);

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                return transaction;
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

        public void UpdateAssignmentPaper(AssignmentPaper assignmentPaper)
        {
            try
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();

                OrgUnitBL.UpdateAssignmentPaper(assignmentPaper);
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

        public AssignmentPaper GetAssignmentPaperByOrgUnitId(int OrgUnitId, string cultureName)
        {
            try
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();

                return OrgUnitBL.GetAssignmentPaperByOrgUnitId(OrgUnitId, cultureName);
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

        public int GetExplanationCount(int permissionId)
        {
            IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();

            return explanationRepository.GetExplanations(e => e.PermissionId == permissionId).Count;
        }

        public TransactionBasicInfo GetTransactionBasicInfoByNumber(int transactionNumber, int year, int transactionType, string cultureName)
        {
            try
            {

                Transaction TransactionForId = TransactionBL.GetByTransactionNumber(transactionNumber, year, transactionType);

                if (TransactionForId == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                Transaction transaction = TransactionBL.GetTransactionBasicInfoById(TransactionForId.Id, year, cultureName);

                TransactionBasicInfo transactionBasicinfo = new TransactionBasicInfo()
                {
                    Id = transaction.Id,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    Number = transaction.Number,
                    Remarks = transaction.Remarks,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH,
                    Subject = transaction.Subject,
                    DocumentNumber = transaction.DocumentNumber,
                    TransactionCategoryId = transaction.TransactionCategory.Id,
                    ConfidentialityName = (transaction.Confidentiality != null) ? transaction.Confidentiality.LocalName : null,
                    ConfidentialityId = (transaction.Confidentiality != null) ? transaction.Confidentiality.Id : -1,
                    ExternalPartyName = (transaction.ExternalParty != null) ? transaction.ExternalParty.LocalName : null,
                    ExternalPartyId = (transaction.ExternalParty != null) ? transaction.ExternalParty.Id : -1,
                    ExternalPartyManagerName = (transaction.ExternalPartyManager != null) ? transaction.ExternalPartyManager.LocalName : null,
                    ExternalPartyManagerId = (transaction.ExternalPartyManager != null) ? transaction.ExternalPartyManager.Id : -1,
                    LetterTypeName = (transaction.LetterType != null) ? transaction.LetterType.Text : null,
                    LetterTypeId = (transaction.LetterType != null) ? transaction.LetterType.Id : -1,
                    PriorityName = (transaction.Priority != null) ? transaction.Priority.Text : null,
                    PriorityId = (transaction.Priority != null) ? transaction.Priority.Id : -1,
                    SignedByUserName = (transaction.SignedByUser != null) ? transaction.SignedByUser.LocalName : null,
                    SignedByUserId = (transaction.SignedByUser != null) ? transaction.SignedByUser.Id : -1,
                    TransactionTypeName = (transaction.TransactionType != null) ? transaction.TransactionType.Text : null,
                    TransactionTypeId = (transaction.TransactionType != null) ? transaction.TransactionType.Id : -1,
                    ToEntityName = (transaction.Entity != null) ? transaction.Entity.LocalName : null,
                    ToUserName = (transaction.ToUser != null) ? transaction.ToUser.LocalName : null,
                    OutboundDraftId = transaction.OutboundDraftId,
                    IsSigned = transaction.IsSigned,
                    OutboundDraftEditorType = transaction.OutboundDraftEditorType,
                    SuggestedTopicId = (transaction.SuggestedTopic != null) ? transaction.SuggestedTopic.Id : -1,
                    DeliveryMethodId = transaction.DeliveryMethod != null ? transaction.DeliveryMethod.Id : -1,
                    DeliveryMethod = transaction.DeliveryMethod != null ? transaction.DeliveryMethod.Text : string.Empty,
                    PostCode = transaction.PostCode,
                    POBox = transaction.POBox,
                    StatusName = transaction.Status != null ? transaction.Status.Text : string.Empty
                };

                if (transaction.SubjectClassifications != null && transaction.SubjectClassifications.Count > 0)
                {
                    transactionBasicinfo.SubjectClassifications = new List<int>();

                    transaction.SubjectClassifications.ToList().ForEach(s => transactionBasicinfo.SubjectClassifications.Add(s.SubjectClassification.Id));
                }

                return transactionBasicinfo;
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

        //AddExplanationWeb
        public void SendExplanationNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
            IList<NotificationUser> notificationUsers, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{TransactionId}"] = transaction.Id.ToString();
                keyValues["{UserName}"] = User.UserName;

                //Notification Web
                NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                //Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType, notificationEmailSubject,
                        notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
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

        //ReceiveReportWeb
        private void SendReceiveTransactionNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
            IList<NotificationUser> notificationUsers, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{PriorityId}"] = transaction.Priority.Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.LocalName;
                keyValues["{UserName}"] = User.UserName;
                keyValues["{OrgName}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transaction.OrgUnitId, cultureName);

                //Notification Web
                NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                //Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType, notificationEmailSubject,
                        notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
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

        public bool TransactionDirectReply(int transactionId, string remarks, int userId)
        {
            ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
            var assignment = transactionAssignmentBL.TransactionDirectReply(transactionId, remarks, userId);
            transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(assignment);
            return true;
        }
    }
}
