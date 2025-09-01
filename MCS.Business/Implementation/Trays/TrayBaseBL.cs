using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Framework.Security;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.Framework.Web;

namespace MCS.Business
{
    public abstract class TrayBaseBL : BaseBL, ITrayBL
    {
        public static ITrayBL Create(TrayType trayType)
        {
            switch (trayType)
            {
                case TrayType.Copies:
                case TrayType.SavedCopies:
                case TrayType.SpecialCopies:
                case TrayType.InternalInboundCopies:
                    return IoC.Container.Resolve<ICopiesTrayBL>();
                case TrayType.CopiesOutbound:
                    return IoC.Container.Resolve<ICopiesOutboundTrayBL>();
                case TrayType.DraftOutbound:
                case TrayType.DeletedDraftOutbound:
                    return IoC.Container.Resolve<IDraftOutboundTrayBL>();
                case TrayType.Manager:
                    return IoC.Container.Resolve<IManagerTrayBL>();
                case TrayType.OrgUnit:
                    return IoC.Container.Resolve<IOrgUnitTrayBL>();
                case TrayType.Saved:
                    return IoC.Container.Resolve<ISavedTrayBL>();
                case TrayType.SentTransactions:
                    return IoC.Container.Resolve<ISentTransactionsTrayBL>();
                case TrayType.MyTransactions:
                    return IoC.Container.Resolve<IMyTransactionsTrayBL>();
                case TrayType.YESSER:
                    return IoC.Container.Resolve<IYesserTrayBL>();
                case TrayType.OutboundExternal:
                    return IoC.Container.Resolve<IOutboundExternalTrayBL>();
                case TrayType.ElcOutBound:
                    return IoC.Container.Resolve<IElcOutboundTrayBL>();
                case TrayType.FollowUp:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.FollowUpComplete:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.FollowUpUnderProcess:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.FollowUpCanceld:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.FollowUpEscalation:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.FollowUpReminder:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.FollowUpLate:
                    return IoC.Container.Resolve<IFollowUpTrayBL>();
                case TrayType.ReservedExternalOutbound:
                    return IoC.Container.Resolve<IReservationExternalOutboundTrayBL>();
                case TrayType.Reservation:
                    return IoC.Container.Resolve<IReservationTrayBL>();
                case TrayType.Withdrawal:
                    return IoC.Container.Resolve<IWithdrawalTrayBL>();
            }

            return null;
        }

        public abstract TrayType TrayType { get; }

        public abstract string TrayPermission { get; }

        public static IList<Tray> GetAllTrays(string cultureName)
        {
            try
            {
                IList<Tray> trays = CacheHelper.Get(CachedObjectsKey.Trays, cultureName) as List<Tray>;

                if (trays == null)
                {
                    ITrayRepository trayRepository = IoC.Resolve<ITrayRepository>();
                    trays = trayRepository.GetAllTrays(cultureName);
                    CacheHelper.Insert(CachedObjectsKey.Trays, trays, cultureName);
                }

                return trays;
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

        public virtual void UpdateTray(Tray tray)
        {
            try
            {
                ITrayRepository trayRepository = IoC.Resolve<ITrayRepository>();
                trayRepository.UpdateTray(tray);
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

        public static Tray GetTrayById(int trayId, string cultureName = "ar")
        {
            try
            {
                IList<Tray> trays = CacheHelper.Get(CachedObjectsKey.Trays, cultureName) as List<Tray>;

                if (trays == null)
                {
                    trays = GetAllTrays(cultureName);
                    CacheHelper.Insert(CachedObjectsKey.Trays, trays, cultureName);
                }

                return trays.Where(t => t.Id == trayId).FirstOrDefault();
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

        public static void UpdateTrays(IList<Tray> trays)
        {
            try
            {
                ITrayRepository trayRepository = IoC.Resolve<ITrayRepository>();
                trayRepository.UpdateTrays(trays);
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

        public static IList<Tray> GetTrays(SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                ITrayRepository trayRepository = IoC.Resolve<ITrayRepository>();
                IList<Tray> trays = trayRepository.GetTrays(searchCriteria, out rowsCount);
                return trays;
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

        public virtual TrayDetailsInfo GetWithdrawalData(int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                Tray tray = GetTrayById((int)TrayType, searchCriteria.CultureName);

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo();
                //{
                //    Id = tray.Id,
                //    Name = tray.LocalName,
                //    TransactionTraysInfo = new List<TransactionTrayInfo>()
                //};

                // trayDetailsInfo.TodayTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any);

                trayDetailsInfo.TransactionTraysInfo = GetWithdrawalTransactions(transId, orgunitId, transactionTypeId, year, searchCriteria, out rowsCount);

                trayDetailsInfo.AllTransactionCount = rowsCount;

                return trayDetailsInfo;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message));
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
        public virtual TrayDetailsInfo GetTrayDetailsInfo(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                CheckTrayAuthorization();

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                Tray tray = GetTrayById((int)TrayType, searchCriteria.CultureName);

                TrayDetailsInfo trayDetailsInfo = new TrayDetailsInfo()
                {
                    Id = tray.Id,
                    Name = tray.LocalName,
                    TransactionTraysInfo = new List<TransactionTrayInfo>()
                };

                // trayDetailsInfo.TodayTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(User.Id, tray.Id, OrgUnitId, TransactionDateType.Any);

                trayDetailsInfo.TransactionTraysInfo = GetUserTransactionsByTray(TrayType, OrgUnitId, searchCriteria, TransactionDateType.Any, out rowsCount);

                trayDetailsInfo.AllTransactionCount = rowsCount;

                return trayDetailsInfo;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message));
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

       

        public virtual Transaction GetNextTransaction(int OrgUnitId, SearchCriteriaCustom searchCriteria)
        {
            try
            {
                CheckTrayAuthorization();
                var transaction = GetNextTransactionsByTray(TrayType, OrgUnitId, searchCriteria);

                return transaction;
            }
            catch (BusinessException ex)
            {
                throw new BusinessException((StatusCode)Enum.Parse(typeof(StatusCode), ex.Message));
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

        public virtual TransactionAssignment GetTransactionAssignmentLightByOrgUnitIdAndTransactionId(int OrgUnitId, int transactionId)
        {
            try
            {
                CheckTrayAuthorization();
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                var result = transactionAssignmentBL.GetTransactionAssignmentLight(User.Id, (int)TrayType.OrgUnit, OrgUnitId, transactionId);
                return result;
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

        protected void CheckTrayAuthorization()
        {
            if (!UserContext.LoggedInUser.HasClaim(TrayPermission))
            {
                ThrowTrayAuthorizationException();
            }
        }

        public virtual IList<TransactionTrayInfo> GetUserTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, out int rowsCount)
        {
            try
            {
                return GetTransactionsInfoByTray(c => c.ToUserId == User.Id, OrgUnitId, searchCriteria, transactionDate, trayType, out rowsCount);
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



        public virtual Transaction GetNextTransactionsByTray(TrayType trayType, int OrgUnitId, SearchCriteriaCustom searchCriteria)
        {
            try
            {
                return TransactionBL.GetNextTransactionsTray(User.Id, OrgUnitId, trayType == TrayType.DeletedDraftOutbound ? trayType : TrayType, searchCriteria);
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

        public virtual IList<TransactionTrayInfo> GetWithdrawalTransactions(int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                return GetWithdrawalTransactionsInfo(c => c.ToUserId == User.Id, transId, orgunitId, transactionTypeId, year, searchCriteria, out rowsCount);
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

        public static IList<TrayDetailsInfo> GetUserTrays(int userId, int OrgUnitId, string cultureName)
        {
            try
            {
                IList<TrayDetailsInfo> traysDetails = new List<TrayDetailsInfo>();

                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

                IList<Tray> trays = GetAllTrays(cultureName);

                foreach (Tray tray in trays)
                {
                    TrayDetailsInfo trayDetails = new TrayDetailsInfo()
                    {
                        Id = tray.Id,
                        Name = tray.LocalName,
                    };
                    switch (tray.Id)
                    {
                        case (int)TrayType.Copies:
                        case (int)TrayType.CopiesOutbound:
                            trayDetails.TodayTransactionCount =
                          trayDetails.TodayTransactionCount +
                          TransactionBL.GetTransactionCopiesCount(userId, OrgUnitId, DateTime.Now);

                            trayDetails.AllTransactionCount =
                                trayDetails.AllTransactionCount +
                                TransactionBL.GetTransactionCopiesCount(userId, OrgUnitId, null);
                            break;

                        case (int)TrayType.SentTransactions:
                            trayDetails.AllTransactionCount =
                       transactionAssignmentBL.GetTransactionAssignmentHistoryCount(userId, tray.Id, OrgUnitId, TransactionDateType.Any);
                            break;

                        default:
                            trayDetails.AllTransactionCount =
                           transactionAssignmentBL.GetTransactionAssignmentCount(userId, tray.Id, OrgUnitId, TransactionDateType.Any);
                            break;

                    }


                    traysDetails.Add(trayDetails);


                }

                // Late transactions count
                TrayDetailsInfo lateTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.Late
                };
                lateTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.Late);
                traysDetails.Add(lateTrayDetails);

                // Has date transactions count
                TrayDetailsInfo hasDateTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.HasDate
                };
                hasDateTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.HasDate);
                traysDetails.Add(hasDateTrayDetails);

                // Today transactions count
                TrayDetailsInfo TodayDateTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.Today
                };
                TodayDateTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.Today);
                traysDetails.Add(TodayDateTrayDetails);

                TrayDetailsInfo DecisionsTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.Decisions
                };
                DecisionsTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.Decisions);
                traysDetails.Add(DecisionsTrayDetails);


                // Decisions transactions count
                TrayDetailsInfo CircularsTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.Circulars
                };
                // Decisions transactions count
                TrayDetailsInfo SublimeMatterTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.SublimeMatter
                };
                CircularsTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.Circulars);
                traysDetails.Add(CircularsTrayDetails);
                SublimeMatterTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TransactionDateType.SublimeMatter);
                traysDetails.Add(SublimeMatterTrayDetails);


                #region My transcation tray actions filter
                // Necessary transactions count
                TrayDetailsInfo NecessaryTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayProcedureFilter.Necessary
                };
                NecessaryTrayDetails.AllTransactionCount = transactionAssignmentBL.GetMyTransactionTrayCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TrayProcedureFilter.Necessary, TransactionDateType.Any);
                traysDetails.Add(NecessaryTrayDetails);

                // Opinion transactions count
                TrayDetailsInfo OpinionTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayProcedureFilter.Opinion
                };
                OpinionTrayDetails.AllTransactionCount = transactionAssignmentBL.GetMyTransactionTrayCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TrayProcedureFilter.Opinion, TransactionDateType.Any);
                traysDetails.Add(OpinionTrayDetails);

                // Follow transactions count
                TrayDetailsInfo FollowTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayProcedureFilter.Follow
                };
                FollowTrayDetails.AllTransactionCount = transactionAssignmentBL.GetMyTransactionTrayCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TrayProcedureFilter.Follow, TransactionDateType.Any);
                traysDetails.Add(FollowTrayDetails);

                // Reviews transactions count
                TrayDetailsInfo ReviewsTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayProcedureFilter.Reviews
                };
                ReviewsTrayDetails.AllTransactionCount = transactionAssignmentBL.GetMyTransactionTrayCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TrayProcedureFilter.Reviews, TransactionDateType.Any);
                traysDetails.Add(ReviewsTrayDetails);

                // OthersAll transactions count
                TrayDetailsInfo OthersAllTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayProcedureFilter.OthersAll
                };
                OthersAllTrayDetails.AllTransactionCount = transactionAssignmentBL.GetMyTransactionTrayCount(userId, (int)TrayType.MyTransactions, OrgUnitId, TrayProcedureFilter.OthersAll, TransactionDateType.Any);
                traysDetails.Add(OthersAllTrayDetails);

                #endregion

                TrayDetailsInfo outboundExternalTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.OutboundExternal
                };
                outboundExternalTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.OutboundExternal, OrgUnitId);
                traysDetails.Add(outboundExternalTrayDetails);

                TrayDetailsInfo ElcoutboundTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.ElcOutBound
                };
                ElcoutboundTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.ElcOutBound, OrgUnitId);
                traysDetails.Add(ElcoutboundTrayDetails);

                TrayDetailsInfo DeletedDraftOutboundTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.DeletedDraftOutbound
                };
                DeletedDraftOutboundTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.DeletedDraftOutbound, OrgUnitId);
                traysDetails.Add(DeletedDraftOutboundTrayDetails);

                TrayDetailsInfo followUpUnderProcessTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.FollowUpUnderProcess
                };
                followUpUnderProcessTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.FollowUpUnderProcess, OrgUnitId);
                traysDetails.Add(followUpUnderProcessTrayDetails);

                TrayDetailsInfo followUpCanceldTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.FollowUpCanceld
                };
                followUpCanceldTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.FollowUpCanceld, OrgUnitId);
                traysDetails.Add(followUpCanceldTrayDetails);


                TrayDetailsInfo followUpCompleteTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.FollowUpComplete
                };
                followUpCompleteTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.FollowUpComplete, OrgUnitId);
                traysDetails.Add(followUpCompleteTrayDetails);

                TrayDetailsInfo followUpLateTrayDetails = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.FollowUpLate
                };
                followUpLateTrayDetails.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.FollowUpLate, OrgUnitId);
                traysDetails.Add(followUpLateTrayDetails);

                TrayDetailsInfo FollowUpEscalation = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.FollowUpEscalation
                };
                FollowUpEscalation.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.FollowUpEscalation, OrgUnitId);
                traysDetails.Add(FollowUpEscalation);

                TrayDetailsInfo followUpNotification = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.FollowUpReminder
                };
                followUpNotification.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.FollowUpReminder, OrgUnitId);
                traysDetails.Add(followUpNotification);


                TrayDetailsInfo SavedCopies = new TrayDetailsInfo()
                {
                    Id = (int)TrayType.SavedCopies
                };
                SavedCopies.AllTransactionCount = transactionAssignmentBL.GetTransactionAssignmentCount(userId, (int)TrayType.SavedCopies, OrgUnitId);
                traysDetails.Add(SavedCopies);

                return traysDetails;
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

        public void ApplyTrayAction(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId)
        {
            try
            {
                Transaction transaction = transaction = TransactionBL.GetTransactionById(transactionId); ;

                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                TransactionAssignment transactionAssignment = null;
                List<TransactionLink> transactionLinks = new List<TransactionLink>();
                List<TransactionCopy> transactionCopies = new List<TransactionCopy>();
                switch (trayActionType)
                {
                    case TrayActionType.Save:
                        transaction.StatusId = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                        transactionAssignment = transaction.Assignments.Where(a => a.ToEntity.Id == OrgUnitId && a.ToUser.Id == User.Id).FirstOrDefault();

                        if (transactionAssignment == null)
                        {
                            throw new BusinessException(StatusCode.TransactionNotFound);
                        }

                        transactionAssignment.TrayId = (int)TrayType.Saved;
                        transactionAssignment.Date = DateTime.Now;
                        transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                        transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);

                        transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                        transactionBL.Update(transaction);
                        break;

                    case TrayActionType.Assign:
                        transactionAssignmentBL.Assign(transaction.Id, OrgUnitId);
                        break;

                    case TrayActionType.Revert:
                        transactionAssignmentBL.RevertAssignByTransaction(transaction.Id, OrgUnitId, trayId);
                        break;

                    case TrayActionType.DeleteDraft:

                        Transaction transactionLinkedToDraft = transactionRepository.GetTransaction(t => t.OutboundDraftId == transaction.Id);

                        if (transactionLinkedToDraft != null)
                        {
                            transactionLinkedToDraft.IsDeleted = true;
                            transactionLinkedToDraft.OutboundDraftId = null;
                            transactionRepository.Update(transactionLinkedToDraft);
                        }

                        TransactionBL.Delete(transaction);

                        break;

                    case TrayActionType.SaveRevert:
                        transaction.StatusId = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                        transactionAssignment = transaction.Assignments.Where(a => a.ToEntity.Id == OrgUnitId && a.ToUser.Id == User.Id).FirstOrDefault();

                        if (transactionAssignment == null)
                        {
                            throw new BusinessException(StatusCode.TransactionNotFound);
                        }

                        transactionAssignment.TrayId = (int)TrayType.MyTransactions;
                        transactionAssignment.Date = DateTime.Now;
                        transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                        transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);

                        transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                        transactionBL.Update(transaction);
                        break;

                    case TrayActionType.CreateOutbound:
                        transaction.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);

                        Transaction transactionLinkedToOutboundDraft = transactionRepository.GetTransaction(t => t.OutboundDraftId == transaction.Id);

                        if (transactionLinkedToOutboundDraft != null)
                        {
                            List<TransactionLink> Links = new List<TransactionLink>();

                            transactionLinkedToOutboundDraft.OutboundDraftId = null;
                            transactionLinkedToOutboundDraft.StatusId = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

                            Links.AddRange(transactionLinkedToOutboundDraft.Links);
                            Links.Add(new TransactionLink { ToTransactionId = transaction.Id, TransactionId = transactionLinkedToOutboundDraft.Id, TypeId = (int)LinkType.ByOutboundNumber });

                            transactionRepository.UpdateTransactionLinks(transactionLinkedToOutboundDraft.Id, Links);

                            transactionRepository.Update(transactionLinkedToOutboundDraft);

                            transactionLinks.Add(new TransactionLink { ToTransactionId = transactionLinkedToOutboundDraft.Id, TransactionId = transaction.Id, TypeId = (int)LinkType.ByInboundNumber });
                        }

                        transactionCopies.Add(new TransactionCopy
                        {
                            Date = DateTime.Now,
                            DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                            EntityId = transaction.OrgUnitId,
                            UserId = transaction.UserId,
                            TransactionId = transaction.Id
                        });

                        if (transaction.Links != null && transaction.Links.Count > 0)
                        {
                            transactionLinks.AddRange(transaction.Links);
                        }
                        if (transaction.Copies != null && transaction.Copies.Count > 0)
                        {
                            transactionCopies.AddRange(transaction.Copies);
                        }

                        transactionRepository.UpdateTransactionCopies(transaction.Id, transactionCopies);
                        transactionRepository.UpdateTransactionLinks(transaction.Id, transactionLinks);
                        transactionRepository.Update(transaction);

                        break;

                    case TrayActionType.Viewed:
                        TransactionCopy transactionCopy = transaction.Copies.Where(tc => tc.TransactionId == transaction.Id && tc.UserId == User.Id && tc.EntityId == OrgUnitId && tc.IsSent == 1).FirstOrDefault();

                        if (transactionCopy != null)
                        {
                            transactionRepository.SetTransactionCopyToViewed(transactionCopy);
                        }

                        TransactionAssignment transAssignment = transaction.Assignments
                            .Where(a => a.TransactionId == transaction.Id && a.ToUserId == User.Id && a.ToEntityId == OrgUnitId
                                && (a.TrayId == (int)TrayType.Copies | a.TrayId == (int)TrayType.CopiesOutbound)).FirstOrDefault();

                        if (transAssignment != null)
                        {
                            transactionAssignmentBL.SetTransactionAssignmentToViewed(transAssignment);
                        }

                        break;

                    case TrayActionType.ManagerRevert:
                        TransactionAssignment transactionAssignmentTemp = transactionAssignmentBL.GetTransactionAssignmentById(assignmentId.Value);
                        ITransactionTaskBL transactionTaskBL = new TransactionTaskBL();
                        transactionTaskBL.MoveUserTasks(assignmentId.Value, transactionAssignmentTemp.ToUser.Id);
                        transactionAssignmentBL.RevertAssignById(assignmentId.Value, OrgUnitId);

                        break;

                    case TrayActionType.Complete:
                        transaction.StatusId = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                        transactionAssignment = transaction.Assignments.OrderByDescending(a => a.Id).FirstOrDefault();

                        if (transactionAssignment == null)
                        {
                            throw new BusinessException(StatusCode.TransactionNotFound);
                        }

                        transactionAssignment.TrayId = (int)TrayType.Saved;
                        transactionAssignment.Date = DateTime.Now;
                        transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                        transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);

                        transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);

                        transactionBL.Update(transaction);

                        break;
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

        public virtual void Save(int transactionId, int OrgUnitId, string remarks, string cultureName, bool SaveWithComplete = false) { }
        public virtual void LinkedSave(int transactionId, int OrgUnitId, string remarks,int userId, string cultureName, bool SaveWithComplete = false) { }

        public virtual void Assign(int transactionId, int OrgUnitId, string cultureName) { }
        public virtual void RevertAssignTransaction(int transactionId, int OrgUnitId, int trayId) { }
        public virtual void DeleteDraft(int transactionId) { }
        public virtual void SaveRevert(int transactionId, int OrgUnitId) { }
        public virtual void Viewed(int transactionId, int OrgUnitId, int userId, string cultureName = "") { }
        public virtual void DeleteCopy(int transactionId, int OrgUnitId, int userId, string cultureName = "") {
            try
            {
                TransactionCopy transactionCopy = TransactionBL.GetCopyTransactionByID(transactionId);
                Transaction transaction = TransactionBL.GetTransactionById(transactionCopy.TransactionId);
                ITransactionBL transactionBL = TransactionBL.Create((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));


                if (transaction.Copies != null)
                {
                    transactionBL.SetTransactionCopyToDelete(transactionCopy);

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
        public virtual void SetTransactionCopyToUndo(int transactionId, int OrgUnitId, int userId, string cultureName = "") { }
        public virtual void ManagerRevert(int assignmentId, int OrgUnitId) { }
        public virtual void ManagerSave(int transactionId, int OrgUnitId, int trayId, TrayActionType trayActionType, int? assignmentId) { }
        public virtual void ManagerAssign(int transactionId, int assignmentId, IList<TransactionAssignment> transactionAssignments, int OrgUnit, string cultureName = "") { }
        public virtual Transaction PrepareOutboundCreation(int transactionId, int OrgUnitId, string cultureName)
        { return null; }
        public virtual TransactionDetails CreateOutboundExternal(int transactionId, Transaction transactionExternal)
        { return null; }
        public virtual TrayDetailsInfo GetPopulariazations(int OrgUnitId, SearchCriteriaCustom searchCriteria, out int rowCount)
        { rowCount = 0; return null; }

        private void ThrowTrayAuthorizationException()
        {
            switch (TrayType)
            {
                case TrayType.MyTransactions:
                    throw new BusinessException(StatusCode.MyTransactionNotAuthorized);
                case TrayType.DraftOutbound:
                    throw new BusinessException(StatusCode.DraftOutboundNotAuthorized);
                case TrayType.SentTransactions:
                    throw new BusinessException(StatusCode.SentTransactionsNotAuthorized);
                case TrayType.OrgUnit:
                    throw new BusinessException(StatusCode.OrgUnitNotAuthorized);
            }
        }

        protected List<TransactionTrayInfo> GetTransactionsInfoByTray(Func<TransactionAssignment, bool> where, int OrgUnitId, SearchCriteriaCustom searchCriteria, Common.TransactionDateType transactionDate, TrayType trayType, out int rowsCount)
        {
            IList<Transaction> transactions = null;
            IList<TransactionAssignment> transactionAssignments = null;
            List<TransactionTrayInfo> transactionTrayInfos = new List<TransactionTrayInfo>();

            ICollaborationBL collaborationBL = new CollaborationBL();

            transactions = TransactionBL.GetUserTransactionsTray(User.Id, OrgUnitId, trayType == TrayType.DeletedDraftOutbound ? trayType : TrayType, transactionDate, searchCriteria, out rowsCount);

            if (transactions != null)
            {

                foreach (var transaction in transactions)
                {
                    transactionAssignments = transaction.Assignments.Where(where).ToList();

                    TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo
                    {
                        TransactionAssignmentInfos = new List<TransactionAssignmentInfo>()
                    };

                    foreach (var transactionAssignment in transactionAssignments)
                    {
                        TransactionAssignmentInfo transactionAssignmentInfo = TransactionAssignmentBL.MapTransactionAssignment(transactionAssignment, searchCriteria.CultureName);

                        transactionAssignmentInfo.HasCollaboration = collaborationBL.HasCollaboration(transactionAssignment.FromUserId, transaction.Id);

                        transactionTrayInfo.TransactionAssignmentInfos.Add(transactionAssignmentInfo);
                    }

                    transactionTrayInfo.transactionDetailsInfo = TransactionBL.MapTransaction(transaction, searchCriteria.CultureName);

                    if (transaction.RemindDate.HasValue)
                    {
                        if (transaction.RemindDate.Value.Date < DateTime.Now.Date)
                        {
                            transactionTrayInfo.transactionDetailsInfo.IsLate = true;
                        }
                    }
                    else
                    {
                        TransactionAssignment transactionAssignment = transactionAssignments.Where(a => a.ToEntityId == OrgUnitId && a.ToUserId.HasValue && a.ToUserId.Value == User.Id).FirstOrDefault();
                        if (transactionAssignment != null)
                        {
                            int processingPeriod = 0;
                            if (transactionAssignment.ToUserId.HasValue)
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();
                                UserProfile userProfile = userManagementBL.GetUserById(transactionAssignment.ToUserId.Value);
                                processingPeriod = userProfile.TransactionProcessingPeriod;
                            }

                            DateTime date = DateTime.Now.Date;

                            if (processingPeriod > 0)
                            {
                                date = DateTime.Now.Date.AddDays(-processingPeriod);
                            }
                            if (transactionAssignment.Date.Date > date)
                            {
                                transactionTrayInfo.transactionDetailsInfo.IsLate = true;
                            }
                        }
                    }
                    transactionTrayInfos.Add(transactionTrayInfo);
                }
            }

            return transactionTrayInfos;
        }

        protected List<TransactionTrayInfo> GetWithdrawalTransactionsInfo(Func<TransactionAssignment, bool> where, int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            IList<Transaction> transactions = null;
            IList<TransactionAssignment> transactionAssignments = null;
            List<TransactionTrayInfo> transactionTrayInfos = new List<TransactionTrayInfo>();

            ICollaborationBL collaborationBL = new CollaborationBL();

            transactions = TransactionBL.GetWithdrawalTransactions(transId, orgunitId, transactionTypeId, year, searchCriteria, User.Id, out rowsCount);

            if (transactions != null)
            {

                foreach (var transaction in transactions)
                {
                    transactionAssignments = transaction.Assignments.Where(where).ToList();

                    TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo
                    {
                        TransactionAssignmentInfos = new List<TransactionAssignmentInfo>()
                    };

                    foreach (var transactionAssignment in transactionAssignments)
                    {
                        TransactionAssignmentInfo transactionAssignmentInfo = TransactionAssignmentBL.MapTransactionAssignment(transactionAssignment, searchCriteria.CultureName);

                        transactionAssignmentInfo.HasCollaboration = collaborationBL.HasCollaboration(transactionAssignment.FromUserId, transaction.Id);

                        transactionTrayInfo.TransactionAssignmentInfos.Add(transactionAssignmentInfo);
                    }

                    transactionTrayInfo.transactionDetailsInfo = TransactionBL.MapTransaction(transaction, searchCriteria.CultureName);

                    if (transaction.RemindDate.HasValue)
                    {
                        if (transaction.RemindDate.Value.Date < DateTime.Now.Date)
                        {
                            transactionTrayInfo.transactionDetailsInfo.IsLate = true;
                        }
                    }
                    else
                    {
                        TransactionAssignment transactionAssignment = transactionAssignments.Where(a => a.ToUserId.HasValue && a.ToUserId.Value == User.Id).FirstOrDefault();
                        if (transactionAssignment != null)
                        {
                            int processingPeriod = 0;
                            if (transactionAssignment.ToUserId.HasValue)
                            {
                                IUserManagementBL userManagementBL = new UserManagementBL();
                                UserProfile userProfile = userManagementBL.GetUserById(transactionAssignment.ToUserId.Value);
                                processingPeriod = userProfile.TransactionProcessingPeriod;
                            }

                            DateTime date = DateTime.Now.Date;

                            if (processingPeriod > 0)
                            {
                                date = DateTime.Now.Date.AddDays(-processingPeriod);
                            }
                            if (transactionAssignment.Date.Date > date)
                            {
                                transactionTrayInfo.transactionDetailsInfo.IsLate = true;
                            }
                        }
                    }
                    transactionTrayInfos.Add(transactionTrayInfo);
                }
            }

            return transactionTrayInfos;
        }

        public virtual void RevertReject(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName = "")
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                transactionAssignmentBL.RevertReject(transactionId, OrgUnitId, trayId, remarks, cultureName);
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

        public virtual void RevertRejectToCreator(int transactionId, int OrgUnitId, int trayId, string remarks, string cultureName = "")
        {
            try
            {
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                transactionAssignmentBL.RevertRejectToCreator(transactionId, OrgUnitId, trayId, remarks, cultureName);
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

        public virtual void FollowUpAddNote(int transactionId, int orgUnitId, int userId, string note)
        {
            try
            {
                //ITransactionBL trans = new TransactionBL();
                TransactionBL.FollowUpDetailsAdd(transactionId, orgUnitId, userId, note);
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
