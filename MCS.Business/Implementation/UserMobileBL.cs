using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.MobileSearchCriteria;

namespace MCS.Business
{
    public class UserMobileBL : BaseBL, IUserMobileBL
    {

        public UserProfile GetUserInfo(string userName, string cultureName)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();

                return userManagementRepository.GetUserInfo(userName, cultureName);
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

        public UserProfile GetUserById(int userId)
        {
            try
            {
                IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();

                return userManagementRepository.GetUserById(userId);
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

        public UserMobile GetUserMobile(int? userId, string userName, string cultureName)
        {
            try
            {
                IUserMobileRepository mobileApiRepository = IoC.Resolve<IUserMobileRepository>();

                return mobileApiRepository.GetUserMobile(userId, userName, cultureName);
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

        public void UpdateUserMobile(UserMobile userMobile, string cultureName)
        {
            try
            {
                IUserMobileRepository userMobileRepository = IoC.Resolve<IUserMobileRepository>();
                userMobileRepository.UpdateUserMobile(userMobile, cultureName);
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

        public void SetDefaultEntity(int userId, int defaultEntityId)
        {
            try
            {
                IUserMobileRepository userMobileRepository = IoC.Resolve<IUserMobileRepository>();
                userMobileRepository.SetDefaultEntity(userId, defaultEntityId);
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



        public void UserMobileUpdateTransactionStatus(int transId, int statusId, int userId, int orgUnitId, string reason)
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                TransactionAssignment transactionAssignment = null;

                transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                        ts.ToUserId == userId & ts.ToEntityId == orgUnitId &
                        ts.TransactionId == transId);

                if (transactionAssignment == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                transactionAssignment.TrayId = statusId == (int)TransactionStatus.TempSave ? (int)TrayType.Saved : (int)TrayType.MyTransactions;
                transactionAssignment.Date = DateTime.Now;
                transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                transactionAssignment.Description = reason;
                transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);
                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);
                transactionRepository.UserMobileUpdateTransactionStatus(transId, ((TransactionStatus)statusId).LookupIdentity(LookupCategory.TransactionStatus, string.Empty), reason);
                transactionRepository.FollowUpUpdateIsDeleted(transId, -1);

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
        public void UserMobileDeletedTransaction(int transId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

                transactionRepository.DeletedTransaction(transId);
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

        public IList<OrgUnit> UserMobileGetOrgHierarchy(int? parentId, string cultureName)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                return orgUnitRepository.UserMobileGetOrgHierarchy(parentId, cultureName);
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

        public IList<OrgUnit> UserMobileGetOrgHierarchyAC(string searchQuery, string cultureName)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<IOrgUnitRepository>();

                return orgUnitRepository.UserMobileGetOrgHierarchyAC(searchQuery, cultureName, 10);
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

        public void AddUserSignature(UserPreference userPreference, int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                userPreferenceRepository.AddUserSignature(userPreference, userId, cultureName);
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

        public UserPreference GetUserSignature(int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                return userPreferenceRepository.GetUserSignature(userId, cultureName);
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

        public DocumentInfo GetDocumentById(int documentId, string cultureName)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();
                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }
                IDocumentRepository documentRepository = IoC.Resolve<IDocumentRepository>();
                return documentRepository.GetDocumentById(documentId, userWeight);
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

        #region Authenticated Items
        public List<Permission> GetPermisions(string cultureName, int groupId)
        {
            try
            {
                IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
                return permissionRepository.GetUserPermissionsByGroupId(groupId, User.Id, cultureName).ToList();
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

        public List<OrgUnit> GetAllEntities(string cultureName, int userId)
        {
            try
            {
                IOrgUnitRepository orgUnitRepository = IoC.Resolve<OrgUnitRepository>();
                return orgUnitRepository.GetOrgUnits(null, cultureName, userId).ToList();
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

        public bool CheckIfUserHasPermission(int userId, string permissionName)
        {

            IPermissionRepository permissionRepository = IoC.Resolve<IPermissionRepository>();
            int permissionId = permissionRepository.GetPermissionByCode(permissionName).Id;
            IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();
            return userManagementRepository.CheckIfUserHasPermission(userId, permissionId);
        }
        public List<Domain.TransactionType> GetTransactionSources(TransactionCategories transactionCategories, string cultureName)
        {
            ITransactionTypeRepository transactionSourceTypeRepository = IoC.Resolve<TransactionTypeRepository>();
            return transactionSourceTypeRepository.UserMobileGetTransactionTypes(transactionCategories, cultureName).ToList();
        }

        public List<Domain.Action> GetAllActions(string cultureName)
        {
            IActionRepository oActionRepository = IoC.Resolve<IActionRepository>();
            return oActionRepository.GetAllActions(cultureName).ToList();
        }

        public List<Priority> GetPriorities(TransactionCategories transactionCategories, string language)
        {
            IPriorityRepository priorityRepository = IoC.Resolve<IPriorityRepository>();
            IList<Priority> priorities = priorityRepository.GetPriorities(language).ToList();

            var result = (from p in priorities where (p.TransactionCategories & transactionCategories) != 0 select p);

            return result.ToList();
        }

        public List<LetterType> GetLetterType(TransactionCategories transactionCategories, string language)
        {
            ILetterTypeRepository letterTypeRepository = IoC.Resolve<ILetterTypeRepository>();
            IList<LetterType> letterTypes = letterTypeRepository.GetLetterTypes(language).ToList();

            var result = (from p in letterTypes where (p.TransactionCategories & transactionCategories) != 0 select p);

            return result.ToList();
        }
        public List<AttachmentType> GetAttachementsType(TransactionCategories transactionCategories, string language)
        {
            IAttachmentTypeRepository attachmentTypeRepository = IoC.Resolve<IAttachmentTypeRepository>();
            IList<AttachmentType> attachmentTypes = attachmentTypeRepository.GetAttachmentTypes(language).ToList();

            var result = (from p in attachmentTypes where (p.TransactionCategories & transactionCategories) != 0 select p);

            return result.ToList();
        }
        public List<Lookup> GetLookupItems(LookupCategory lookupCategory, string language)
        {
            ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();

            return lookupRepository.GetLookupItems((int)lookupCategory, language).ToList();
        }
        #endregion

        public Transaction CreateTransaction(Transaction transaction, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                transactionRepository.AddTransaction(transaction);

                return transactionRepository.GetTransactionById(transaction.Id);
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

        public Transaction GetTransaction(int transId, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.GetUserMobileTransaction(transId, cultureName);
        }

        public List<TrayDetailsInfo> GetUserTrays(int userId, int OrgUnitId, string cultureName)
        {
            try
            {
                IList<TrayDetailsInfo> traysDetails = new List<TrayDetailsInfo>();
                IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
                ILookupRepository lookupRepository = IoC.Resolve<ILookupRepository>();

                UserCategory userCategory = userManagementBL.GetUserCategoryByUserId(userId);

                if (userCategory != null)
                {
                    ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();

                    IList<Tray> trays = userManagementBL.GetUserCategoryTrays(userCategory.Id, cultureName);
                    TrayDetailsInfo trayDetails;

                    foreach (Tray tray in trays)
                    {
                        if (tray.Id != (int)TrayType.Tasks && tray.Id != (int)TrayType.OrgUnit)
                        {


                            trayDetails = new TrayDetailsInfo()
                            {
                                Id = tray.Id,
                                Name = tray.LocalName,
                            };

                            trayDetails.AllTransactionCount =
                                transactionAssignmentBL.GetTransactionAssignmentCount(userId, tray.Id, OrgUnitId, TransactionDateType.Any);

                            if (tray.Id == (int)TrayType.Copies || tray.Id == (int)TrayType.CopiesOutbound)
                            {
                                trayDetails.TodayTransactionCount =
                                    trayDetails.TodayTransactionCount +
                                    TransactionBL.GetTransactionCopiesCount(userId, OrgUnitId, DateTime.Now);

                                trayDetails.AllTransactionCount =
                                    trayDetails.AllTransactionCount +
                                    TransactionBL.GetTransactionCopiesCount(userId, OrgUnitId, null);
                            }

                            traysDetails.Add(trayDetails);
                        }
                    }

                    trayDetails = new TrayDetailsInfo()
                    {
                        Id = 99,
                        Name = lookupRepository.GetLookupItem(TransactionDateType.HasDate.LookupIdentity(LookupCategory.TransactionDateType, cultureName), cultureName).Text,
                        AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.HasDate)
                    };

                    traysDetails.Add(trayDetails);

                    trayDetails = new TrayDetailsInfo()
                    {
                        Id = 100,
                        Name = lookupRepository.GetLookupItem(TransactionDateType.Late.LookupIdentity(LookupCategory.TransactionDateType, cultureName), cultureName).Text,
                        AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.Late)
                    };

                    traysDetails.Add(trayDetails);
                }


                return traysDetails.ToList();
            }
            catch (BusinessException ex)
            {
                throw ex;
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

        public void AssignItBack(int transactionId, int userId, int OrgUnitId, int trayId, string remarks, string cultureName)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

            TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment
       (
       ts =>
           ts.TransactionId == transactionId &
           ts.FromEntityId == OrgUnitId &
           (ts.FromUserId == userId)
         );
            if (transactionAssignment != null)
            {
                if (transactionAssignment.FromUserId == transactionAssignment.ToUserId)
                {
                    throw new BusinessException(StatusCode.CantReturnToSelf);
                }
            }




            transactionAssignment.ToUserId = transactionAssignment.FromUserId;
            transactionAssignment.ToEntityId = transactionAssignment.FromEntityId;
            transactionAssignment.FromUserId = userId;
            transactionAssignment.FromEntityId = OrgUnitId;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
            transactionAssignment.Description = remarks;
            transactionAssignment.CurrentPathStep = transactionAssignment.CurrentPathStep.HasValue ? transactionAssignment.CurrentPathStep - 1 : transactionAssignment.CurrentPathStep;
            transactionAssignment.Viewed = false;

            switch (transactionAssignment.Transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, cultureName))
            {
                case (int)TransactionCategory.Inbound:
                case (int)TransactionCategory.InternalOutbound:
                    {
                        transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                        break;
                    }
                case (int)TransactionCategory.DraftOutbound:
                    {
                        transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                        break;
                    }
            }

            transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);

            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
            int transactionAssignmentHistoryId = transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            var oldTransactionDeliveryReport = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transactionAssignment.TransactionId).LastOrDefault();
            if (oldTransactionDeliveryReport != null)
            {
                oldTransactionDeliveryReport.TransactionAssignmentHistoryId = transactionAssignmentHistoryId;
                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(oldTransactionDeliveryReport);
            }

        }

        public void AssignItBackVip(int transactionId, int userId, int OrgUnitId, int trayId, string remarks, string cultureName)
        {
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

            TransactionAssignment transactionAssignment = transactionAssignmentRepository.GetTransactionAssignment
       (
       ts =>
           ts.TransactionId == transactionId &
           ts.ToEntityId == OrgUnitId &
           (ts.ToUserId == userId)
         );
            if (transactionAssignment != null)
            {
                if (transactionAssignment.FromUserId == transactionAssignment.ToUserId)
                {
                    throw new BusinessException(StatusCode.CantReturnToSelf);
                }
            }




            transactionAssignment.ToUserId = null;
            transactionAssignment.ToEntityId = transactionAssignment.FromEntityId;
            transactionAssignment.FromUserId = userId;
            transactionAssignment.FromEntityId = OrgUnitId;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
            transactionAssignment.Description = remarks;
            transactionAssignment.CurrentPathStep = transactionAssignment.CurrentPathStep.HasValue ? transactionAssignment.CurrentPathStep - 1 : transactionAssignment.CurrentPathStep;
            transactionAssignment.Viewed = false;

            switch (transactionAssignment.Transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, cultureName))
            {
                case (int)TransactionCategory.Inbound:
                case (int)TransactionCategory.InternalOutbound:
                    {
                        transactionAssignment.TrayId = (int)TrayType.OrgUnit;
                        break;
                    }
                case (int)TransactionCategory.DraftOutbound:
                    {
                        transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
                        break;
                    }
            }

            transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);

            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();
            int transactionAssignmentHistoryId = transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            var oldTransactionDeliveryReport = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transactionAssignment.TransactionId).LastOrDefault();
            if (oldTransactionDeliveryReport != null)
            {
                oldTransactionDeliveryReport.TransactionAssignmentHistoryId = transactionAssignmentHistoryId;
                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(oldTransactionDeliveryReport);
            }

        }

        public IList<ExternalParty> UserMobileGetExternalParties(int? parentId, string cultureName)
        {
            try
            {
                IExternalPartyRepository externalPartyRepository = IoC.Resolve<IExternalPartyRepository>();

                return externalPartyRepository.UserMobileGetExternalParties(parentId, cultureName);
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

        public IList<ExternalParty> UserMobileGetExternalPartiesAC(string searchQuery, string cultureName)
        {
            try
            {
                IExternalPartyRepository externalPartyRepository = IoC.Resolve<IExternalPartyRepository>();

                return externalPartyRepository.UserMobileGetExternalPartiesAC(searchQuery, cultureName, 10);
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

        public void SpecializeTransaction(int transactionId, int userId, int OrgUnitId, int trayId, string cultureName)
        {

            ITransactionAssignmentRepository transactionAssignmentRepository =
                   IoC.Resolve<ITransactionAssignmentRepository>();

            TransactionAssignment transactionAssignment = transactionAssignmentRepository
                                                          .GetTransactionAssignment(ts => ts.ToUserId == null
                                                                                    & ts.TransactionId == transactionId
                                                                                    & ts.ToEntityId == OrgUnitId
                                                                                    & ts.TrayId == (int)TrayType.OrgUnit);

            if (transactionAssignment == null)
            {
                throw new BusinessException(StatusCode.TransactionNotFound);
            }

            transactionAssignment.ToUserId = userId;
            transactionAssignment.ToEntityId = OrgUnitId;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(transactionAssignment.Date);
            if (transactionAssignment.Transaction.TransactionCategory.Id == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
            {
                transactionAssignment.TrayId = (int)TrayType.DraftOutbound;
            }
            else
            {
                transactionAssignment.TrayId = (int)TrayType.MyTransactions;
            }

            transactionAssignmentRepository.UpdateTransactionAssignment(transactionAssignment);
            TransactionAssignment sentTransactionAssignment = transactionAssignmentRepository.GetTransactionAssignment(ts =>
                            ts.TransactionId == transactionAssignment.TransactionId &
                            ts.FromEntityId == transactionAssignment.FromEntityId &
                            ts.FromUserId == transactionAssignment.FromUserId &
                            ts.TrayId == (int)TrayType.SentTransactions);


            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = IoC.Resolve<ITransactionAssignmentHistoryBL>();

            IList<TransactionAssignmentHistory> transactionAssignmentHistories = transactionAssignmentHistoryBL.GetTransactionAssignmentHistoryByTransactionId(transactionAssignment.TransactionId);

            if (transactionAssignmentHistories.Count > 1)
            {
                ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                transactionBL.SetTransactionCopiesSent(transactionAssignment.TransactionId);
            }

            if (sentTransactionAssignment != null)
            {
                transactionAssignmentRepository.DeleteTransactionAssignment(sentTransactionAssignment.Id);
            }

            transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);


            //ITransactionAssignmentBL transactionAssignmentBL = IoC.Resolve<ITransactionAssignmentBL>();
            //transactionAssignmentBL.Assign(transactionId, OrgUnitId, cultureName);
        }

        public List<Transaction> UserMobileGetTrayTransactions(int userId, int OrgUnitId, TrayType trayType, TransactionDateType transactionDate, FilterCriteria filterCriteria, string cultureName, bool isAscending = false)
        {
            return TransactionBL.UserMobileGetUserTransactionsTray(userId, OrgUnitId, trayType, transactionDate, filterCriteria, cultureName, isAscending);
        }

        public List<MobileSearchResult> UserMobileSearchTransaction(SearchCriteria searchCriteria, string cultureName)
        {
            try
            {
                IMobileWrapper mobileWrapper = IoC.Resolve<IMobileWrapper>();

                return mobileWrapper.MobileSearch(searchCriteria, cultureName);
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

        public TransactionAssignment GetTransactionAssignment(int transId, string cultureName)
        {
            IPermissionBL permissionBL = new PermissionBL();
            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

            int? userWeight = null;

            if (permissions != null)
            {
                userWeight = permissions.Max(s => s.Weight);
            }
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();
            return transactionAssignmentRepository.GetTransactionAssignment(transId, cultureName, userWeight);
        }

        public List<TransactionAssignmentHistory> GetTransactionAssignmentHistory(int transId, string cultureName)
        {
            IPermissionBL permissionBL = new PermissionBL();
            IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
            int? userWeight = null;

            if (permissions != null)
            {
                userWeight = permissions.Max(s => s.Weight);
            }
            ITransactionAssignmentHistoryRepository transactionAssignmentHistoryRepository = IoC.Resolve<ITransactionAssignmentHistoryRepository>();
            return transactionAssignmentHistoryRepository.GetTransactionAssignmentHistory(transId, cultureName, userWeight).ToList();
        }

        public UserAccompleshmentsReportResult GetUserAccompleshmentsReport(int userId, int entityId)
        {
            IMobileWrapper mobileWrapper = IoC.Resolve<IMobileWrapper>();
            return mobileWrapper.GetUserAccompleshmentsReport(userId, entityId);
        }
        public List<EntitiesAccompleshmentsReportResult> GetEntitiesAccompleshmentsReport(int entityId, int periodCount, int selectedPeriod)
        {
            IMobileWrapper mobileWrapper = IoC.Resolve<IMobileWrapper>();
            return mobileWrapper.GetEntitiesAccompleshmentsReport(entityId, periodCount, selectedPeriod);
        }

        public List<Explanation> GetTransactionExplanations(int transId, int userId, string cultureName)
        {
            IExplanationRepository explanationRepository = IoC.Resolve<IExplanationRepository>();
            return explanationRepository.GetExplanationsByTransactionId(transId, userId, cultureName).ToList();
        }

        public Transaction CreateTransaction(Transaction transaction)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();

            int transId = transactionRepository.AddTransaction(transaction);

            return transactionRepository.GetTransactionById(transId);
        }

        public IList<Permission> GetUserPrivileges(int userId, string currentUserIdentity, string cultureName)
        {
            IUserManagementRepository userManagementRepository = IoC.Resolve<IUserManagementRepository>();

            return userManagementRepository.GetUserPrivileges(userId, currentUserIdentity, cultureName);
        }

        public AssignmentPaper UserMobileGetAssignmentPaperByUserId(int userId, string cultureName)
        {
            try
            {
                IUserPreferenceRepository userPreferenceRepository = IoC.Resolve<IUserPreferenceRepository>();
                AssignmentPaper assignmentPaper = userPreferenceRepository.GetAssignmentPaperByUserId(userId, cultureName);
                if (assignmentPaper != null)
                {
                    return assignmentPaper;
                }
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

        public void AssignTransaction(int transactionId, IList<TransactionAssignment> transactionAssignments, string cultureName)
        {
            try
            {
                INotificationBL notificationBL = IoC.Resolve<INotificationBL>();
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
                transactionAssignmentBL.AssignTransaction(transactions, transactionAssignments, cultureName);

                foreach (var trans in transactions)
                {
                    notificationBL.SendAssignmentNotification(trans, transactionAssignments, cultureName);
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

    }
}