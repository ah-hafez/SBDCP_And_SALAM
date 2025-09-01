using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MCS.Framework;
using MCS.Framework.ObjectExtensions;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using YESSERMobileDomain = MCS.Domain.MobileSearchCriteria;
using System.Data.Entity;
using static MCS.Common.UserClaims;
using MCS.DTO.Transaction;
using System.Runtime.CompilerServices;
using MCS.Business.Implementation;

namespace MCS.Business
{
    public abstract class TransactionBL : BaseBL, ITransactionBL
    {
        public static ITransactionBL Create(Common.TransactionCategory transactionType)
        {
            switch (transactionType)
            {
                case Common.TransactionCategory.Inbound:
                    return IoC.Container.Resolve<IInboundBL>();
                case Common.TransactionCategory.ExternalOutbound:
                    return IoC.Container.Resolve<IOutboundExternalBL>();
                case Common.TransactionCategory.InternalOutbound:
                    return IoC.Container.Resolve<IOutboundInternalBL>();
                case Common.TransactionCategory.DraftOutbound:
                    return IoC.Container.Resolve<IOutboundDraftBL>();
            }

            return null;
        }

        public virtual IList<ExternalPartyAttachment> GetExternalPartiesAttach(int TransactionId, int externalPartyId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                IList<ExternalPartyAttachment> externalParty = transactionRepository.GetExternalPartiesAttach(TransactionId, externalPartyId, cultureName);

                return externalParty;
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

        public abstract TransactionCategory TransactionCategory { get; }

        public abstract string GetSourceName(Transaction transaction, string cultureName);
        public virtual Transaction GetPreviousTransactionByID(int transactionsId, int OrgUnitId, string cultureName, bool IsForIndividual)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                Transaction previousTransaction = transactionRepository.GetPreviousTransactionByID(transactionsId, OrgUnitId, TransactionCategory, cultureName, IsForIndividual);
                return previousTransaction;
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

        public static TransactionCopy GetCopyTransactionByID(int id)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                TransactionCopy previousTransaction = transactionRepository.GetTransactionCopyById(id);
                return previousTransaction;
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
        public virtual Transaction GetPreviousTransaction(int OrgUnitId, string cultureName, bool IsForIndividual)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                Transaction previousTransaction = transactionRepository.GetPreviousTransaction(User.Id, OrgUnitId, TransactionCategory, cultureName, IsForIndividual);
                return previousTransaction;
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

        public void Update(Transaction transaction)
        {
            try
            {
                Validate(transaction);

                PreUpdate(transaction);

                OnUpdate(transaction);

                PostUpdate(transaction);

                //do indexing
                //DoTransactionIndex(transaction.Id);
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

        public Transaction UpdateVipInbound(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary)
        {
            try
            {
                var transaction = OnInboundUpdate(transactionFollowUps, transactionCopies, transactionId, ConfidentialityId, documentContent, summary);

                PostUpdate(transaction);

                //do indexing
                //DoTransactionIndex(transaction.Id);
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
        public Transaction UpdateVipOutboundInternal(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary)
        {
            try
            {
                var transaction = OnOutboundInternalUpdate(transactionFollowUps, transactionCopies, transactionId, ConfidentialityId, documentContent, summary);

                PostUpdate(transaction);

                //do indexing
                //DoTransactionIndex(transaction.Id);
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
        public Transaction UpdateVipOutboundDraft(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, string mainDocumentContent
            , string pdfMainDocumentContent, bool isSigned)
        {
            try
            {
                var transaction = OnOutboundDraftUpdate(transactionFollowUps, transactionCopies, transactionId, ConfidentialityId, mainDocumentContent, pdfMainDocumentContent, isSigned);

                PostUpdate(transaction);

                //do indexing
                //DoTransactionIndex(transaction.Id);
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

        public void UpdateCanceledOutBound(Transaction transaction)
        {
            try
            {
                Validate(transaction);

                PreUpdate(transaction);

                OnCanceledOutBoundUpdate(transaction);

                PostUpdate(transaction);

                //do indexing
                //DoTransactionIndex(transaction.Id);
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
        public static void Delete(Transaction transaction)
        {
            try
            {
                int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                ILookupBL lookupBL = new LookupBL();
                transaction.IsDeleted = true;
                transaction.Status = lookupBL.GetLookupItem(Outbound);
                transaction.StatusId = Outbound;

                transactionRepository.UpdateTransaction(transaction);
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

        public void SetTransactionCopyToViewed(TransactionCopy transactionCopy)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.SetTransactionCopyToViewed(transactionCopy);
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

        public void SetTransactionCopyToDelete(TransactionCopy transactionCopy)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.SetTransactionCopyToDelete(transactionCopy);
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
        public void SetTransactionCopyToUndo(TransactionCopy transactionCopy)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.SetTransactionCopyToUndo(transactionCopy);
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

        public virtual IList<DeliveryReportInfoDTO> DeliveryReport(Transaction transaction, string cultureName, List<int> reportIds, bool perTransaction = true, bool IsNew = false)
        {
            try
            {
                IList<DeliveryReportInfoDTO> deliveryReport = new List<DeliveryReportInfoDTO>();

                PreDeliveryReport();

                deliveryReport = OnDeliveryReport(transaction, cultureName, reportIds, perTransaction, IsNew);

                PostDeliveryReport(transaction);

                return deliveryReport;
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
        protected virtual IList<DeliveryReportInfoDTO> OnDeliveryReport(Transaction transaction, string cultureName, List<int> reportIds, bool perTransaction = true, bool IsNew = false)
        {
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();

            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            if (transactionDeliveryReports.Count > 1 && transactionDeliveryReports[0].Number != transactionDeliveryReports[1].Number)
            {
                perTransaction = false;
            }

            if (IsNew)
            {
                deliveryReports = NewDeliveryReportNumber(transaction, reportIds, cultureName, IsNew);
            }
            else if (perTransaction)
            {
                deliveryReports = DeliveryReportNumberPerTransaction(transaction, reportIds, cultureName);
            }
            else
            {
                deliveryReports = DeliveryReportNumberPerAssignment(transaction, reportIds, cultureName);
            }

            foreach (var deliveryReport in deliveryReports)
            {
                deliveryReport.ConfidentialityName = transaction.Confidentiality.Name.Localizations.FirstOrDefault(t => t.Culture.ShortName == cultureName).Text;
                deliveryReport.TransactionTypeName = transaction.TransactionType.LocalizationIdentifier.Localizations.FirstOrDefault(t => t.Culture.ShortName == cultureName).Text;
            }

            return deliveryReports;
        }
        private IList<DeliveryReportInfoDTO> DeliveryReportNumberPerTransaction(Transaction transaction, List<int> reportIds, string cultureName)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

            List<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id).ToList();
            List<TransactionDeliveryReport> transactionExCopiesDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id, true).ToList();
            transactionDeliveryReports.AddRange(transactionExCopiesDeliveryReports);

            if (reportIds != null)
            {
                transactionDeliveryReports = transactionDeliveryReports.Where(r => reportIds.Contains(r.Id)).ToList();
            }

            string deliveryReportNumber = string.Empty;
            string deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            DateTime deliveryReportDateTime = DateTime.Now;

            int deliveryReportsCount = 0;

            if (transactionDeliveryReports != null && transactionDeliveryReports.Count > 0)
            {
                if (!string.IsNullOrEmpty(transactionDeliveryReports.FirstOrDefault().Number) && (deliveryReportsCount = transactionDeliveryReportBL.GetTransactionDeliveryReportByNumber(transactionDeliveryReports.FirstOrDefault().Number).Count()) == 1)
                {
                    deliveryReportNumber = transactionDeliveryReports.FirstOrDefault().Number;
                    deliveryReportDateTime = transactionDeliveryReports.FirstOrDefault().Date;
                    deliveryReportDateTimeH = transactionDeliveryReports.FirstOrDefault().DateH;
                }
                else
                {
                    deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
                }
            }

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {
                DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                {
                    TransactionNumber = transaction.Number,
                    AttachmentCount = transactionDeliveryReport.Transaction.Attachments.Count(),
                    DateH = transaction.DateH,
                    Subject = transaction.Subject
                };

                if (transaction.TransactionCategoryId == Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                {
                    deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionExternalCopyId.HasValue ? (
                        transactionDeliveryReport.TransactionExternalCopy != null ?
                        transactionDeliveryReport.TransactionExternalCopy.Entity != null ?
                        transactionDeliveryReport.TransactionExternalCopy.Entity.Name != null ?
                        transactionDeliveryReport.TransactionExternalCopy.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty : string.Empty)
                        : (transaction.ExternalParty != null ?
                           transaction.ExternalParty.Name != null ?
                           transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text :
                           string.Empty :
                           string.Empty);
                }
                else
                {
                    if (transactionDeliveryReport.TransactionCopyId.HasValue)
                    {
                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionCopyId.HasValue && transactionDeliveryReport.TransactionCopy != null && transactionDeliveryReport.TransactionCopy.Entity != null ? transactionDeliveryReport.TransactionCopy.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty;
                        deliveryReportTransaction.ToEntity = deliveryReportTransaction.ToEntity;
                    }
                    else if (transactionDeliveryReport.TransactionExternalCopyId.HasValue)
                    {

                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionExternalCopyId.HasValue ? (
                      transactionDeliveryReport.TransactionExternalCopy != null ?
                      transactionDeliveryReport.TransactionExternalCopy.Entity != null ?
                      transactionDeliveryReport.TransactionExternalCopy.Entity.Name != null ?
                      transactionDeliveryReport.TransactionExternalCopy.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty : string.Empty)
                      : string.Empty;
                        deliveryReportTransaction.ToEntity = deliveryReportTransaction.ToEntity;
                    }
                    else
                    {
                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty;


                    }

                    if (transaction.TransactionCategoryId == Common.TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                    {
                        deliveryReportTransaction.ExternalParty = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                    }
                }

                if (transactionDeliveryReport.Reporter != null)
                {
                    deliveryReportTransaction.Receiver = transactionDeliveryReport.Reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                }

                deliveryReportTransaction.TransactionCategory = transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                deliveryReportTransaction.TransactionCategoryId = transaction.TransactionCategoryId;

                if (transactionDeliveryReport.TransactionExternalCopyId.HasValue || transactionDeliveryReport.TransactionCopyId.HasValue)
                    deliveryReportTransaction.IsCopy = true;
                else
                    deliveryReportTransaction.IsCopy = false;


                deliveryReportTransactions.Add(deliveryReportTransaction);

                transactionDeliveryReport.Number = deliveryReportNumber;
                transactionDeliveryReport.Date = deliveryReportDateTime;
                transactionDeliveryReport.DateH = deliveryReportDateTimeH;
                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);
            }

            //foreach (var item in transactionExCopiesDeliveryReports)
            //{
            //    if (item.Number == null)
            //    {
            //        item.Number = DeliveryReportCounter.GetInstance().Next().ToString();
            //        item.Date = deliveryReportDateTime;
            //        item.DateH = deliveryReportDateTimeH;
            //        transactionDeliveryReportBL.UpdateTransactionDeliveryReport(item);
            //    }
            //}

            DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
            {
                ReportNumber = deliveryReportNumber,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                UserName = userManagementBL.GetUserName(User.Id, cultureName),
                DeliveryReportTransactions = deliveryReportTransactions
            };

            deliveryReports.Add(deliveryReport);

            return deliveryReports;
        }
        private IList<DeliveryReportInfoDTO> NewDeliveryReportNumber(Transaction transaction, List<int> reportIds, string cultureName, bool IsNew = false)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            if (reportIds != null)
            {
                transactionDeliveryReports = transactionDeliveryReports.Where(r => reportIds.Contains(r.Id)).ToList();
            }

            string deliveryReportNumber = deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
            string deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            DateTime deliveryReportDateTime = DateTime.Now;

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {
                DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                {
                    TransactionNumber = transaction.Number,
                    AttachmentCount = transactionDeliveryReport.TransactionHistory.AttchmentCount,
                    DateH = transaction.DateH
                };

                if (transaction.TransactionCategoryId == Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                {
                    deliveryReportTransaction.ToEntity = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                }
                else
                {
                    deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty;

                    if (transaction.TransactionCategoryId == Common.TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                    {
                        deliveryReportTransaction.ExternalParty = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                    }
                }
                if (transactionDeliveryReport.Reporter != null)
                {
                    deliveryReportTransaction.Receiver = transactionDeliveryReport.Reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                }

                deliveryReportTransactions.Add(deliveryReportTransaction);

                transactionDeliveryReport.Number = deliveryReportNumber;
                transactionDeliveryReport.Date = deliveryReportDateTime;
                transactionDeliveryReport.DateH = deliveryReportDateTimeH;


                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);
            }

            DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
            {
                ReportNumber = deliveryReportNumber,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                UserName = userManagementBL.GetUserName(User.Id, cultureName),
                DeliveryReportTransactions = deliveryReportTransactions
            };

            deliveryReports.Add(deliveryReport);

            return deliveryReports;
        }

        private IList<DeliveryReportInfoDTO> DeliveryReportNumberPerAssignment(Transaction transaction, List<int> reportIds, string cultureName)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions;

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            string deliveryReportNumber = string.Empty;
            string deliveryReportDateTimeH;
            DateTime deliveryReportDateTime;

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {
                if (string.IsNullOrEmpty(transactionDeliveryReport.Number) == false)
                {
                    deliveryReportDateTime = transactionDeliveryReports.FirstOrDefault().Date;
                    deliveryReportDateTimeH = transactionDeliveryReports.FirstOrDefault().DateH;

                    ITransactionBL transactionBL = TransactionBL.Create((Common.TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));

                    DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                    {
                        TransactionNumber = transaction.Number,
                        AttachmentCount = transaction.Attachments.Count,
                        DateH = transaction.DateH
                    };

                    if (transaction.TransactionCategoryId == Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                    {
                        deliveryReportTransaction.ToEntity = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                    }
                    else
                    {
                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty;

                        if (transaction.TransactionCategoryId == Common.TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                        {
                            deliveryReportTransaction.ExternalParty = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                        }
                    }

                    deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();
                    if (transactionDeliveryReport.Reporter != null)
                    {
                        deliveryReportTransaction.Receiver = transactionDeliveryReport.Reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                    }

                    deliveryReportTransactions.Add(deliveryReportTransaction);

                    transactionDeliveryReport.Number = transactionDeliveryReport.Number;
                    transactionDeliveryReport.Date = deliveryReportDateTime;
                    transactionDeliveryReport.DateH = deliveryReportDateTimeH;

                    transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);

                    DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
                    {
                        ReportNumber = transactionDeliveryReport.Number,
                        DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                        RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                        UserName = userManagementBL.GetUserName(User.Id, cultureName),
                        DeliveryReportTransactions = deliveryReportTransactions
                    };

                    if (reportIds != null)
                    {
                        if (reportIds.Contains(transactionDeliveryReport.Id))
                        {
                            deliveryReports.Add(deliveryReport);
                        }
                    }
                }
                else
                {
                    deliveryReportDateTime = DateTime.Now;
                    deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                    ITransactionBL transactionBL = Create((Common.TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));

                    DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                    {
                        TransactionNumber = transaction.Number,
                        AttachmentCount = transaction.Attachments.Count,
                        ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty
                    };
                    if (transaction.TransactionCategoryId == Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                    {
                        deliveryReportTransaction.ToEntity = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                    }
                    else
                    {
                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty;

                        if (transaction.TransactionCategoryId == Common.TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                        {
                            deliveryReportTransaction.ExternalParty = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                        }
                    }

                    if (transactionDeliveryReport.Reporter != null)
                    {
                        deliveryReportTransaction.Receiver = transactionDeliveryReport.Reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                    }
                    deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

                    deliveryReportTransactions.Add(deliveryReportTransaction);

                    transactionDeliveryReport.Number = deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
                    transactionDeliveryReport.Date = deliveryReportDateTime;
                    transactionDeliveryReport.DateH = deliveryReportDateTimeH;

                    transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);

                    DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
                    {
                        ReportNumber = deliveryReportNumber,
                        DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                        RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                        UserName = userManagementBL.GetUserName(User.Id, cultureName),
                        DeliveryReportTransactions = deliveryReportTransactions
                    };

                    deliveryReports.Add(deliveryReport);
                }
            }

            return deliveryReports;
        }
        private IList<DeliveryReportInfoDTO> NewDeliveryReportNumberPerAssignment(Transaction transaction, List<int> reportIds, string cultureName, bool IsNew = false)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions;

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            string deliveryReportNumber = string.Empty;
            string deliveryReportDateTimeH;
            DateTime deliveryReportDateTime;

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {


                deliveryReportDateTime = DateTime.Now;
                deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                ITransactionBL transactionBL = Create((Common.TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));

                DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                {
                    TransactionNumber = transaction.Number,
                    AttachmentCount = transaction.Attachments.Count,
                    ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty
                };

                if (transactionDeliveryReport.Reporter != null)
                {
                    deliveryReportTransaction.Receiver = transactionDeliveryReport.Reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                }
                deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

                deliveryReportTransactions.Add(deliveryReportTransaction);

                transactionDeliveryReport.Number = deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
                transactionDeliveryReport.Date = deliveryReportDateTime;
                transactionDeliveryReport.DateH = deliveryReportDateTimeH;

                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);

                DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
                {
                    ReportNumber = deliveryReportNumber,
                    DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                    RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                    UserName = userManagementBL.GetUserName(User.Id, cultureName),
                    DeliveryReportTransactions = deliveryReportTransactions
                };

                deliveryReports.Add(deliveryReport);

            }

            return deliveryReports;
        }

        public void SetTransactionExternalCopyToViewed(TransactionExternalCopy transactionExternalCopy)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.SetTransactionExternalCopyToViewed(transactionExternalCopy);
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

        public static void UpdateTransactionBasicInfo(Transaction transaction)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                bool isSigned = transactionRepository.CheckIfTransactionSigned(transaction.Id);
                if (isSigned)
                {
                    transaction.IsSigned = isSigned;
                }
                transactionRepository.UpdateTransaction(transaction, false);
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
        public void SetTransactionCopiesSent(int transactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.SetTransactionCopiesSent(transactionId);
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

        public static Transaction StaticGetTransaction(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransaction(@where);
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
        public Transaction GetTransaction(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransaction(@where);
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

        public static Transaction GetTransactionByReference(string transactionCode, int userId, int OrgUnitId, string cultureName)
        {
            try
            {
                if (string.IsNullOrEmpty(transactionCode))
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                Transaction transaction = null;
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                if (transactionCode.All(char.IsDigit))
                {

                    transaction = transactionRepository.GetTransaction(t => t.Assignments.Any(a => a.ToUserId == userId &
                    a.ToEntityId == OrgUnitId & a.Transaction.Number != 0 && a.Transaction.Number.ToString() == transactionCode &
                    a.Transaction.TransactionCategoryId == Inbound & a.Transaction.Year == DateTime.Now.Year), userId, cultureName);

                    if (transaction != null)
                    {
                        return transaction;
                    }
                }
                else
                {
                    transaction = transactionRepository.GetTransaction(t => t.Assignments.Any(a => a.ToUserId == userId & a.ToEntityId == OrgUnitId &
                    a.Transaction.Subject == transactionCode & a.Transaction.TransactionCategoryId == Inbound &
                    a.Transaction.Year == DateTime.Now.Year), userId, cultureName);

                    if (transaction != null)
                    {
                        return transaction;
                    }
                }
                IBarcodeBL barcodeBL = new BarcodeBL();

                Barcode barcode = barcodeBL.GetBarcode(b => b.Value == transactionCode).FirstOrDefault();

                if (barcode == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }


                transaction = transactionRepository.GetTransaction(t => t.Id == barcode.ReferenceId & t.Assignments.Any(a => a.ToUserId == userId & a.ToEntityId == OrgUnitId), userId, cultureName);

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

        public static Transaction GetTransactionByDraftNumber(int draftId)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            Transaction transaction = transactionRepository.GetTransaction(t => t.OutboundDraftId == draftId);

            return transaction;
        }

        public abstract string GetDestinationName(Transaction transaction, string cultureName);

        public static Transaction GetTransactionById(int transactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionById(transactionId);
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
        public static Transaction GetTransactionById(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionById(transactionId, cultureName);
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
        public static bool CheckUserHasPermission(List<int> transactionId, int? userId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.CheckUserHasPermission(transactionId, userId);
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

        public static void AddTransactionSpecialAuthorize(int transactionId, int userId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.AddTransactionSpecialAuthorize(transactionId, userId);
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

        public static bool CheckIfHasSpecialAuthorize(int transactionId, int userId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.HasSpecialAuthorize(transactionId, userId);
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
        public static int GetTransactionByIdAndOrgUnit(int transactionId, int OrgUnitId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionByIdAndOrgUnit(transactionId, OrgUnitId);
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
        public static Transaction GetTransactionByIdAsNoTacking(int transactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionByIdAsNotacking(transactionId);
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
        public Transaction GetTransactionByIdForNotification(int transactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionByIdForNotification(transactionId);
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
        public static Transaction GetTransactionById(int transactionId, string cultureName, bool isNotification = false)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransaction(t => t.Id == transactionId, UserContext.LoggedInUser.Id, cultureName, isNotification);
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

        public static Transaction GetUserTransactionById(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransaction(t => t.Id == transactionId & t.Assignments.Any(a => a.ToUserId == UserContext.LoggedInUser.Id), UserContext.LoggedInUser.Id, cultureName);
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

        public static Transaction GetTransactionBasicInfoById(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactionBasicInfo(transactionId, cultureName);
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

        public static IList<TransactionLink> GetTransactionLinksById(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactionLinks(transactionId, cultureName);
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

        public static IList<Transaction> GetTransactions(int OrgUnitId, int year)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactions(OrgUnitId, year);
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

        public static IList<Transaction> GetTransactions(Expression<Func<Transaction, bool>> @where)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactions(@where);
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

        public virtual Transaction GetTransaction(int userId, int transactionNumber, Common.TransactionCategory transactionCategory, int year, int sourceTypeId, int OrgUnitId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int transactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, cultureName);
                Transaction transaction = transactionRepository.GetTransaction(t =>
                     t.Number == transactionNumber &
                     t.TransactionCategoryId == (int)transactionCategoryId &
                     (t.Year == year | t.YearH == year) &
                     t.Assignments.Any(a => a.ToUserId == userId & a.ToEntityId == OrgUnitId) &
                     t.TransactionTypeId == sourceTypeId &
                     t.StatusId == InProcess, userId,
                     cultureName
                     );

                if (transaction == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                if (transactionCategory == Common.TransactionCategory.ExternalOutbound && transaction.PrintedDeliveryReport == true)
                {
                    throw new BusinessException(StatusCode.UpdateNotAllow);
                }

                CheckTransactionConfidentiality(transaction.Confidentiality.Code);

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

        public List<Transaction> GetTransactionsByNationalId(string nationalId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();



                List<Transaction> transactionList = transactionRepository.GetTransactionsByNationalId((t =>
                     t.Names.Any(n => n.Name.CivilID == nationalId) &&

                      t.TransactionCategoryId == 254), "ar").ToList();

                if (transactionList == null)
                {
                    throw new BusinessException(StatusCode.TransactionNotFound);
                }

                return transactionList;
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
        public virtual Transaction GetTransactionByNumberAndYear(int year, int transactionNumber)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();


                Transaction transaction = transactionRepository.GetTransactionByNumberAndYear(t =>
                     t.Number == transactionNumber &
                      //t.TransactionCategoryId == TransactionCategorieColor.Inbound &
                      t.TransactionCategoryId == 254 &
                     (t.Year == year | t.YearH == year),
                     "ar"
                     );

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
        public static Transaction GetTransaction(int transactionId, int userId, int OrgUnitId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransaction(t =>
                    t.Id == transactionId &
                    t.Assignments.Any(a => a.ToUserId == userId & a.ToEntityId == OrgUnitId || a.FromUserId == userId & a.FromEntityId == OrgUnitId || a.CreatedBy == userId), userId,
                    cultureName
                    );
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
        public static Transaction GetTransaction_VIP(int transactionId, int userId, int OrgUnitId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransaction_VIP(t =>
                    t.Id == transactionId &
                    t.Assignments.Any(a => a.ToUserId == userId & a.ToEntityId == OrgUnitId),
                    cultureName
                    );
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

        public static Transaction GetTransactionLight(int transactionId, int userId, int OrgUnitId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                var result = transactionRepository.GetTransactionLight(t => t.Id == transactionId &
                t.Assignments.Any(a => a.ToUserId == userId & a.ToEntityId == OrgUnitId), cultureName);
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

        public static Transaction GetByTransactionNumber(int transactionNumber)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransaction(t => t.Number == transactionNumber);
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
        public static Transaction GetByTransactionNumberTransaction(int transactionNumber, int year)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransaction(t => t.Number == transactionNumber && t.YearH == year);
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
        public static int GetTransactionCopiesCount(int userId, int OrgUnitId, DateTime? dateTime)
        {
            try
            {
                int Delete = TransCopyStatus.Delete.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                int Viewed = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                if (permissions != null && permissions.Count > 0)
                {
                    int? userWeigth = permissions.Max(s => s.Weight);

                    ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                    Expression<Func<TransactionCopy, bool>> where = null;

                    where = tc => tc.EntityId == OrgUnitId &
                            tc.UserId == userId &
                            tc.IsSent == 1 & tc.Status != Delete &
                            !tc.Transaction.IsDeleted & tc.Status != Viewed;

                    if (dateTime.HasValue)
                    {
                        where = ExpressionUtility.AndAlso(where, ts =>
                            ts.Date.Year == DateTime.Now.Year &
                            ts.Date.Day == DateTime.Now.Day &
                            ts.Date.Month == DateTime.Now.Month);
                    }

                    return transactionRepository.GetTransactionCopiesCount(where);
                }
                return 0;
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
        public static void TransactionElcOutBoundAdd(TransactionElcOutBound transactionElcOutBound)
        {

            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.TransactionElcOutBoundAdd(transactionElcOutBound);
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


        public static void AddConfidentialityAcknowledgment(int TransactionId, int UserId, int OrgUnitId, DateTime CreatedDate)
        {

            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.AddConfidentialityAcknowledgment(TransactionId, UserId, OrgUnitId, CreatedDate);
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
        public static void TransactionElcOutBoundUpdate(int userId, int orgUnitId, bool ishidden, int transactionId)
        {

            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.TransactionElcOutBoundUpdate(userId, orgUnitId, ishidden, transactionId);
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

        public static void AcknowledgeElcOutBound(int userId, int orgUnitId, bool ishidden, int transactionId)
        {

            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.AcknowledgeElcOutBound(userId, orgUnitId, ishidden, transactionId);
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


        public static IList<TransactionCopy> GetTransactionCopies(Expression<Func<TransactionCopy, bool>> where, TrayType trayType, SearchCriteriaCustom searchCriteria, TransactionDateType transactionDate, int userId, out int rowsCount)
        {
            try
            {
                rowsCount = 0;
                IPermissionBL permissionBL = new PermissionBL();
                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                if (permissions != null && permissions.Count > 0)
                {
                    int? userWeight = permissions.Max(s => s.Weight);
                    ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                    if (transactionDate == TransactionDateType.Today)
                    {
                        where = ExpressionUtility.AndAlso(where, tc =>
                           tc.Date.Year == DateTime.Now.Year &
                           tc.Date.Day == DateTime.Now.Day &
                           tc.Date.Month == DateTime.Now.Month);
                    }
                    return transactionRepository.GetTransactionCopies(where, trayType, searchCriteria, out rowsCount, userWeight, userId);
                }
                return new List<TransactionCopy>();
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

        public static int? GetTransactionIdByLinkType(string sourceNumber, int year, int OrgUnitId, LinkingType linkingType, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                int number = 0;

                switch ((int)linkingType)//.LookupIdentity(LookupCategory.LinkingType, cultureName))
                {
                    case (int)LinkingType.WithReplyInbound:
                    case (int)LinkingType.WithReferenceInbound:
                    case (int)LinkingType.WithInboundDocumentNumber:
                        bool isNumber ;
                        isNumber = int.TryParse(sourceNumber,  out number);
                        int Inbound = Common.TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName);
                        if (LinkingType.WithInboundDocumentNumber == linkingType)
                        {
                            return transactionRepository.GetTransactionId(t => t.DocumentNumber == sourceNumber & (t.Year == year | t.YearH == year) & t.TransactionCategory.Id == Inbound);
                        }
                        return transactionRepository.GetTransactionId(t => t.Number == number & (t.Year == year | t.YearH == year) & t.TransactionCategory.Id == Inbound);
                    case (int)LinkingType.WithReplyOutboundInternal:
                    case (int)LinkingType.WithReferenceOutboundInternal:
                        number = Convert.ToInt32(sourceNumber);
                        int InternalOutbound = Common.TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName);
                        return transactionRepository.GetTransactionId(t => t.Number == number & (t.Year == year | t.YearH == year) & t.TransactionCategory.Id == InternalOutbound);
                    case (int)LinkingType.WithReplyOutbound:
                    case (int)LinkingType.WithReferenceOutbound:
                        number = Convert.ToInt32(sourceNumber);
                        int ExternalOutbound = Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName);
                        return transactionRepository.GetTransactionId(t => t.Number == number & (t.Year == year | t.YearH == year) & t.TransactionCategory.Id == ExternalOutbound);
                }

                return number;
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

        protected virtual void PreDeliveryReport()
        {
            if (!User.HasClaim(UserClaims.GeneralPermissions.PrintDeliveryData))
            {
                throw new BusinessException(StatusCode.PermissionPrintDeliveryData);
            }
        }
        public static DeliveryReportInfoDTO PrintTransactionsDeliveryReport(IList<TransactionReportInfo> transactionReportInfos, string cultureName, int userId, bool perTransaction = true)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

            List<int> arr = new List<int>();
            transactionReportInfos.ToList().ForEach(tr => arr.Add(tr.ReportsIds[0]));
            List<string> ReportsNumbers = transactionDeliveryReportBL.GetDeliveryReport(arr).ToList().Select(r => r.Number).ToList();

            string firstItem = ReportsNumbers[0];
            bool allEqual = ReportsNumbers.Skip(1)
              .All(s => string.Equals(firstItem, s, StringComparison.InvariantCultureIgnoreCase)) && !string.IsNullOrWhiteSpace(firstItem);

            string deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
            string deliveryReportDateTimeH;
            DateTime deliveryReportDateTime;

            DeliveryReportInfoDTO deliveryReportInfoDTO = new DeliveryReportInfoDTO()
            {
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                DeliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>(),
                ReportNumber = allEqual ? ReportsNumbers[0] : deliveryReportNumber,
                RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                UserName = userManagementBL.GetUserName(userId, cultureName),
            };

            foreach (TransactionReportInfo transactionReportInfo in transactionReportInfos)
            {
                List<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transactionReportInfo.TransactionId).ToList();
                List<TransactionDeliveryReport> transactionCopiesDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transactionReportInfo.TransactionId, true).ToList();
                transactionDeliveryReports.AddRange(transactionCopiesDeliveryReports);

                Transaction transaction = GetTransactionById(transactionReportInfo.TransactionId);
                List<Attachment> attachments = transaction.Attachments.ToList();
                List<string> attachmentTotal = new List<string>();

                attachments.ForEach(t =>
                {
                    attachmentTotal.Add(t.Count + " " + t.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text);
                });

                transactionDeliveryReports = transactionDeliveryReports.Where(r => transactionReportInfo.ReportsIds.Contains(r.Id)).ToList();


                foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
                {
                    DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                    {
                        TransactionNumber = transaction.Number,
                        AttachmentCount = transactionDeliveryReport.Transaction.Attachments.Count(),
                        AttachmentTotal = string.Join("+", (object[])attachmentTotal.ToArray()),
                        DateH = transaction.DateH,
                        Subject = transaction.Subject
                    };


                    if (transactionDeliveryReport.TransactionCopyId.HasValue)
                    {
                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionCopyId.HasValue && transactionDeliveryReport.TransactionCopy != null && transactionDeliveryReport.TransactionCopy.Entity != null ? transactionDeliveryReport.TransactionCopy.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty;
                        deliveryReportTransaction.ToEntity = deliveryReportTransaction.ToEntity;
                    }
                    else if (transactionDeliveryReport.TransactionExternalCopyId.HasValue)
                    {

                        deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionExternalCopyId.HasValue ? (
                        transactionDeliveryReport.TransactionExternalCopy != null ?
                        transactionDeliveryReport.TransactionExternalCopy.Entity != null ?
                        transactionDeliveryReport.TransactionExternalCopy.Entity.Name != null ?
                        transactionDeliveryReport.TransactionExternalCopy.Entity.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty : string.Empty)
                        : string.Empty;
                        deliveryReportTransaction.ToEntity = deliveryReportTransaction.ToEntity;
                    }
                    else
                    {
                        if (transaction.TransactionCategoryId == Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, cultureName))
                        {
                            deliveryReportTransaction.ToEntity = transaction.ExternalParty != null ? transaction.ExternalParty.Name != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty;
                        }
                        else
                        {
                            deliveryReportTransaction.ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty;
                        }

                    }



                    if (transactionDeliveryReport.Reporter != null)
                    {
                        deliveryReportTransaction.Receiver = transactionDeliveryReport.Reporter.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                    }

                    if (transactionDeliveryReports.FirstOrDefault().Number != null)
                    {
                        deliveryReportDateTime = transactionDeliveryReports.FirstOrDefault().Date;
                        deliveryReportDateTimeH = transactionDeliveryReports.FirstOrDefault().DateH;
                    }
                    else
                    {
                        deliveryReportDateTime = DateTime.Now;
                        deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
                    }


                    if (transactionDeliveryReport.TransactionExternalCopyId != null || transactionDeliveryReport.TransactionCopyId != null)
                        deliveryReportTransaction.IsCopy = true;
                    else
                        deliveryReportTransaction.IsCopy = false;


                    deliveryReportTransaction.TransactionCategoryId = transaction.TransactionCategoryId;
                    deliveryReportTransaction.TransactionCategory = transaction.TransactionCategory.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;

                    if (transaction.ExternalParty != null)
                    {
                        deliveryReportTransaction.ExternalParty = transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                    }

                    deliveryReportInfoDTO.DeliveryReportTransactions.Add(deliveryReportTransaction);

                    transactionDeliveryReport.Number = allEqual ? ReportsNumbers[0] : deliveryReportNumber;
                    transactionDeliveryReport.Date = deliveryReportDateTime;
                    transactionDeliveryReport.DateH = deliveryReportDateTimeH;


                    transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);
                }

                //foreach (var item in transactionExCopiesDeliveryReports)
                //{
                //    item.Number = DeliveryReportCounter.GetInstance().Next().ToString();
                //    transactionDeliveryReportBL.UpdateTransactionDeliveryReport(item);
                //}
            }
            return deliveryReportInfoDTO;
        }
        private IList<DeliveryReportInfoDTO> DeliveryReportNumberPerTransaction(Transaction transaction, string cultureName)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            string deliveryReportNumber = string.Empty;
            string deliveryReportDateTimeH;
            DateTime deliveryReportDateTime;
            int deliveryReportsCount = 0;

            if (transactionDeliveryReports.FirstOrDefault().Number != null)
            {
                deliveryReportsCount = transactionDeliveryReportBL.GetTransactionDeliveryReportByNumber(transactionDeliveryReports.FirstOrDefault().Number).Count();
            }
            if (transactionDeliveryReports.FirstOrDefault().Number != null && deliveryReportsCount == 1)
            {

                deliveryReportNumber = transactionDeliveryReports.FirstOrDefault().Number;
                deliveryReportDateTime = transactionDeliveryReports.FirstOrDefault().Date;
                deliveryReportDateTimeH = transactionDeliveryReports.FirstOrDefault().DateH;
            }
            else
            {
                deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
                deliveryReportDateTime = DateTime.Now;
                deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            }

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {
                DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                {
                    TransactionNumber = transaction.Number,
                    AttachmentCount = transactionDeliveryReport.TransactionHistory.AttchmentCount,
                    ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty
                };

                deliveryReportTransactions.Add(deliveryReportTransaction);

                transactionDeliveryReport.Number = deliveryReportNumber;
                transactionDeliveryReport.Date = deliveryReportDateTime;
                transactionDeliveryReport.DateH = deliveryReportDateTimeH;

                transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);
            }

            DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
            {
                ReportNumber = deliveryReportNumber,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                UserName = userManagementBL.GetUserName(User.Id, cultureName),
                DeliveryReportTransactions = deliveryReportTransactions
            };

            deliveryReports.Add(deliveryReport);

            return deliveryReports;
        }

        private IList<DeliveryReportInfoDTO> DeliveryReportNumberPerAssignment(Transaction transaction, string cultureName)
        {
            IUserManagementBL userManagementBL = new UserManagementBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
            IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions;

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            string deliveryReportNumber = string.Empty;
            string deliveryReportDateTimeH;
            DateTime deliveryReportDateTime;

            if (transactionDeliveryReports.FirstOrDefault().Number != null)
            {
                deliveryReportDateTime = transactionDeliveryReports.FirstOrDefault().Date;
                deliveryReportDateTimeH = transactionDeliveryReports.FirstOrDefault().DateH;

                foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
                {
                    ITransactionBL transactionBL = Create((Common.TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));

                    DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                    {
                        TransactionNumber = transaction.Number,
                        AttachmentCount = transaction.Attachments.Count,
                        ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.Destination != null ? transactionDeliveryReport.TransactionHistory.Destination.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty : string.Empty
                    };

                    deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

                    deliveryReportTransactions.Add(deliveryReportTransaction);

                    transactionDeliveryReport.Number = transactionDeliveryReport.Number;
                    transactionDeliveryReport.Date = deliveryReportDateTime;
                    transactionDeliveryReport.DateH = deliveryReportDateTimeH;
                    transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);

                    DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
                    {
                        ReportNumber = transactionDeliveryReport.Number,
                        DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                        RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                        UserName = userManagementBL.GetUserName(User.Id, cultureName),
                        DeliveryReportTransactions = deliveryReportTransactions
                    };

                    deliveryReports.Add(deliveryReport);
                }

                return deliveryReports;

            }
            else
            {
                deliveryReportDateTime = DateTime.Now;
                deliveryReportDateTimeH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);

                foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
                {
                    ITransactionBL transactionBL = Create((Common.TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));

                    DeliveryReportTransactionInfoDTO deliveryReportTransaction = new DeliveryReportTransactionInfoDTO()
                    {
                        TransactionNumber = transaction.Number,
                        AttachmentCount = transaction.Attachments.Count,
                        ToEntity = transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text
                    };

                    deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();

                    deliveryReportTransactions.Add(deliveryReportTransaction);

                    transactionDeliveryReport.Number = deliveryReportNumber = DeliveryReportCounter.GetInstance().Next().ToString();
                    transactionDeliveryReport.Date = deliveryReportDateTime;
                    transactionDeliveryReport.DateH = deliveryReportDateTimeH;

                    transactionDeliveryReportBL.UpdateTransactionDeliveryReport(transactionDeliveryReport);

                    DeliveryReportInfoDTO deliveryReport = new DeliveryReportInfoDTO()
                    {
                        ReportNumber = deliveryReportNumber,
                        DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                        RootOrgUnitName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName),
                        UserName = userManagementBL.GetUserName(User.Id, cultureName),
                        DeliveryReportTransactions = deliveryReportTransactions
                    };
                    deliveryReports.Add(deliveryReport);
                }
            }
            return deliveryReports;
        }

        protected virtual IList<DeliveryReportInfoDTO> OnDeliveryReport(Transaction transaction, string cultureName, bool perTransaction = true)
        {
            IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();

            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();

            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transaction.Id);

            if (transactionDeliveryReports.Count > 1 && transactionDeliveryReports[0].Number != transactionDeliveryReports[1].Number)
            {
                perTransaction = false;
            }

            if (perTransaction)
            {
                deliveryReports = DeliveryReportNumberPerTransaction(transaction, cultureName);
            }
            else
            {
                deliveryReports = DeliveryReportNumberPerAssignment(transaction, cultureName);
            }

            return deliveryReports;
        }
        public List<int> GetTransactionDeliveryReportByTransactionId(int transcationId)
        {
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transcationId);
            //IList<TransactionDeliveryReport> transactionDeliveryReportsCopies = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transcationId);
            return transactionDeliveryReports.Select(dr => dr.Id).ToList();
        }
        public List<int> GetTransactionDeliveryReportByTransactionId(int transcationId, bool? all = false)
        {
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL
                .GetTransactionDeliveryReportByTransactionId(transcationId,false, all);
            //IList<TransactionDeliveryReport> transactionDeliveryReportsCopies = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionId(transcationId);
            return transactionDeliveryReports.Select(dr => dr.Id).ToList();
        }
        public IList<TransactionDeliveryReport> GetTransactionDeliveryReportByTransactionIds(List<int> transcationIds)
        {
            ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
            IList<TransactionDeliveryReport> transactionDeliveryReports = transactionDeliveryReportBL.GetTransactionDeliveryReportByTransactionIds(transcationIds);
            var result = transactionDeliveryReports.GroupBy(a => a.TransactionId).Select(a => new TransactionDeliveryReport
            {
                TransactionId = a.Key,
                Id = a.LastOrDefault().Id
            }).ToList();
            return result;
        }

        protected virtual void PostDeliveryReport(Transaction transaction)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();

            transaction.PrintedDeliveryReport = true;
            transactionRepository.UpdateTransaction(transaction);
            transactionHistoryBL.AddTransactionHistory(transaction);
        }

        public virtual IList<DeliveryReportInfoDTO> ReprintDeliveryReport(int deliveryReportId, string cultureName)
        {
            try
            {
                IList<DeliveryReportInfoDTO> deliveryReport = new List<DeliveryReportInfoDTO>();
                ITransactionDeliveryReportBL transactionDeliveryReportBL = new TransactionDeliveryReportBL();
                ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                IList<DeliveryReportInfoDTO> deliveryReports = new List<DeliveryReportInfoDTO>();
                IList<DeliveryReportTransactionInfoDTO> deliveryReportTransactions = new List<DeliveryReportTransactionInfoDTO>();
                return deliveryReport;
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

        public virtual IList<DeliveryReportInfoDTO> DeliveryReport(Transaction transaction, string cultureName, bool perTransaction = true)
        {
            try
            {
                IList<DeliveryReportInfoDTO> deliveryReport = new List<DeliveryReportInfoDTO>();
                PreDeliveryReport();
                deliveryReport = OnDeliveryReport(transaction, cultureName, perTransaction);
                PostDeliveryReport(transaction);
                return deliveryReport;
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

        public Transaction ReverAssignTransaction(Transaction transaction, UserProfile userProfile)
        {
            try
            {
                PreRevertAssignTransaction(transaction, userProfile);
                OnRevertAssignTransaction(transaction, userProfile);
                PostRevertAssignTransaction(transaction, userProfile);
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

        public TransactionBarcodesInfo GetTransactionBarcodes(int transactionId, int OrgUnitId, string cultureName)
        {
            try
            {
                TransactionBarcodesInfo transactionBarcodes;

                PreGetTransactionBarcodes(transactionId);

                transactionBarcodes = OnGetTransactionBarcodes(transactionId, OrgUnitId, cultureName);

                PostGetTransactionBarcodes(transactionId);

                return transactionBarcodes;
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

        public IList<Barcode> GetTransactionCopiesBarcodes(int transactionId)
        {
            try
            {
                IList<Barcode> barcodes;

                PreGetTransactionCopiesBarcodes(transactionId);

                barcodes = OnGetTransactionCopiesBarcodes(transactionId);

                PostGetTransactionCopiesBarcodes(transactionId);

                return barcodes;
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

        protected virtual void PreGetTransactionCopiesBarcodes(int transactionId)
        {

        }

        protected virtual IList<Barcode> OnGetTransactionCopiesBarcodes(int transactionId)
        {
            IBarcodeBL barcodeBL = new BarcodeBL();

            return barcodeBL.GetBarcode(bc => bc.ReferenceType.Id == BarcodeReferenceType.Copy.LookupIdentity(LookupCategory.BarcodeReferenceType, string.Empty) & bc.ReferenceId == transactionId);
        }

        protected virtual void PostGetTransactionCopiesBarcodes(int transactionId)
        {

        }

        public TransactionTicket PrintTransactionTicket(Transaction transaction)
        {
            try
            {
                TransactionTicket transactionTicket = null;
                PrePrintTransactionTicket(transaction);
                transactionTicket = OnPrintTransactionTicket(transaction);
                PostPrintTicket(transaction);
                return transactionTicket;
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
        public static List<MainAudit> GetAuditByEntityName(int userId, int orgUnitId, int transactionId, string EntityName, string culture, AuditFor auditFor, bool IsForPrint, out int itemsCount, SearchCriteriaCustom searchCriteria = null)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.GetAuditByEntityName(userId, orgUnitId, transactionId, EntityName, culture, auditFor, IsForPrint, out itemsCount, searchCriteria);
        }
        public static List<AuditDetails> GetEntityAuditing(AuditFor auditFor, int auditId, string PropName, string culture)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.GetEntityAuditing(auditFor, auditId, PropName, culture);
        }

        protected abstract void Validate(Transaction transaction);

        protected virtual Barcode AddTransactionBarcode(Transaction transaction)
        {
            IBarcodeBL barcodeBL = new BarcodeBL();
            Barcode transactionBarcode = new Barcode();
            string barcode = BarecodeNumberGenerator.Generate(transaction.Id, (Common.TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

            transactionBarcode.Value = barcode;
            transactionBarcode.ReferenceId = transaction.Id;
            transactionBarcode.ReferenceTypeId = BarcodeReferenceType.MainTransaction.LookupIdentity(LookupCategory.BarcodeReferenceType, string.Empty);
            barcodeBL.AddBarcode(transactionBarcode);
            return transactionBarcode;
        }

        protected virtual Barcode AddTransactionCopiesBarcode(Transaction transaction)
        {
            IBarcodeBL barcodeBL = new BarcodeBL();
            ILookupBL lookupBL = new LookupBL();

            string copiesValue = string.Empty;
            Barcode copyBarcode = null;

            if (transaction.Copies != null)
            {
                string barcode = string.Empty;

                foreach (TransactionCopy transactionCopy in transaction.Copies)
                {
                    copyBarcode = new Barcode();
                    barcode = BarecodeNumberGenerator.Generate(transactionCopy.Id, (Common.TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));
                    copyBarcode.Value = barcode;
                    copyBarcode.ReferenceId = transactionCopy.Id;
                    copyBarcode.ReferenceTypeId = (int)BarcodeReferenceType.Copy.LookupIdentity(LookupCategory.BarcodeReferenceType, string.Empty);
                    barcodeBL.AddBarcode(copyBarcode);
                }
            }
            return copyBarcode;
        }
        protected virtual void SendNotification(Transaction transaction)
        {
            if (transaction.ToUserId.HasValue && transaction.ToUserId.Value > 0 && transaction.RemindDate.HasValue)
            {
                List<NotificationAttachment> attachments = new List<NotificationAttachment>();
                attachments = Map(transaction.Attachments);
                if (transaction.MainDocument.Document != null && transaction.MainDocument.Document.Content != null)
                {
                    attachments.Add(MapMainDocument(transaction.MainDocument));
                }


                var notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(transaction.ToUserId.Value) };
                SendTransactionNotification(transaction, NotificationSource.CreateTransactionWithRemindDate,
                    NotificationTemplateType.CreateTransactionWithRemindDate,
                    NotificationTemplateType.CreateTransactionWithRemindDate,
                    NotificationEmailSubject.CreateTransactionWithRemindDate,
                    NotificationWebSubject.CreateTransactionWithRemindDate,
                    notificationUsers, "ar", attachments);

            }
        }

        protected virtual int AddTransactionHistory(Transaction transaction)
        {
            ITransactionHistoryBL transactionHistoryBL = new TransactionHistoryBL();

            return transactionHistoryBL.AddTransactionHistory(transaction);
        }

        protected virtual void MoveTransaction(Transaction transaction)
        {

            MoveTransactionToTray(transaction);

        }

        protected virtual void MoveTransactionToTray(Transaction transaction)
        {
            //add new transaction assignment for the transaction created, in order to move the transaction to 
            //my transactions tray

            ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
            IUserManagementBL userManagementBL = new UserManagementBL();
            TransactionAssignment transactionAssignment = new TransactionAssignment();

            transactionAssignment.TrayId = !transaction.IsDraft ? (int)TrayType.MyTransactions : (int)TrayType.DraftOutbound;
            transactionAssignment.ToUserId = transaction.UserId;
            transactionAssignment.ToEntityId = transaction.OrgUnitId;
            transactionAssignment.FromUserId = transaction.UserId;
            transactionAssignment.FromEntityId = transaction.OrgUnitId;
            transactionAssignment.PhysicalEntityId = transaction.OrgUnitId;
            transactionAssignment.PhysicalUserId = transaction.UserId;
            transactionAssignment.TransactionId = transaction.Id;
            transactionAssignment.Date = DateTime.Now;
            transactionAssignment.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            transactionAssignment.PhysicalDate = DateTime.Now;
            transactionAssignment.PhysicalDateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            transactionAssignment.DeliveryMethodId = transaction.DeliveryMethodId;
            transactionAssignment.TransactionPathId = transaction.TransactionPathId;
            transactionAssignment.Viewed = true;
            if (transaction.Priority == null)
            {
                IPriorityBL priorityBL = new PriorityBL();

                transactionAssignment.TransactionAssignmentProcessPeriod = DateTime.Now.AddDays(priorityBL.GetPriorityById(transaction.PriorityId).ProcessPeriod);
            }

            var userTransactionProcessingPeriod = userManagementBL.GetUserById(transaction.UserId).TransactionProcessingPeriod;
            transactionAssignment.DueDate = transaction.RemindDate ?? DateTime.Now.AddDays(userTransactionProcessingPeriod);

            transactionAssignmentBL.AddTransactionAssignment(transactionAssignment);
            transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);
        }

        protected void CheckDeliveryReportPrinted(Transaction transaction)
        {
            Transaction transactionOld = GetTransactionById(transaction.Id);

            if (transactionOld.PrintedDeliveryReport)
            {
                throw new BusinessException(StatusCode.UpdateNotAllow);
            }
        }

        protected virtual void IncrementTransactionCounter(Transaction transaction)
        {
            TransactionCategories transactionCategories = EnumMapper.GetTransactionCategory((TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty));

            transaction.Number = TransactionCounter.Instance.Next(transaction.OrgUnitId, transactionCategories, transaction.TransactionTypeId);

        }

        protected virtual void UpdateTransactionLinks(Transaction transaction)
        {
            if (transaction.Links != null && User.HasClaim(UserClaims.Links.Delete))
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionLinks(transaction.Id, transaction.Links);
            }
        }
        public void UpdateTransactionLinks(int transactionId, IList<TransactionLink> Links)
        {
            if (Links != null)
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionLinks(transactionId, Links);
            }
        }
        public void FollowUpAddTransactionLinks(int transactionId, IList<TransactionLink> Links)
        {
            if (Links != null)
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpAddTransactionLinks(transactionId, Links);
            }
        }

        protected virtual void UpdateTransactionContactDate(Transaction transaction)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            transactionRepository.UpdateTransactionContactDate(transaction.Id, transaction.ContactDateH);
        }
        protected virtual void UpdateTransactionFollowUps(Transaction transaction)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            if (User.HasClaim(UserClaims.Links.Delete))
            {
                transactionRepository.UpdateTransactionFollowUps(transaction.Id, transaction.FollowUp);
            }
        }
        protected virtual void PreUpdate(Transaction transaction)
        {
            try
            {
                transaction.Links.ToList().ForEach(l =>
                {
                    int count = transaction.Links.ToList().Where(tl => tl.ToTransactionId == l.ToTransactionId).ToList().Count;

                    if (count > 1)
                    {
                        throw new BusinessException(StatusCode.TransactionDoubleLinked);
                    }
                });
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
        protected virtual void OnCanceledOutBoundUpdate(Transaction transaction)
        {

            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();


            UpdateTransactionContactDate(transaction);

            bool updateDocument = false;
            bool isDocumentDeleted = false;
            int documentId = -1;

            //if (transaction.MainDocument != null && transaction.MainDocument.IsDeleted)
            //{
            //    documentId = transaction.MainDocument.Id;
            //    transaction.MainDocument = null;
            //    updateDocument = true;
            //    isDocumentDeleted = true;
            //}

            transactionRepository.UpdateTransaction(transaction, updateDocument, false);


            ////yousef todo
            //if (updateDocument == true)
            //{
            //    transactionRepository.UpdateMainDcument(transaction.MainDocument, transaction.Id);
            //}



            //check if Solr indexing is enabled, if yes, then, do index the transation
            //if (SystemConfigurations.IsSolrIndexingEnabled)
            //{
            //    //do transaction indexing

            //    //get the barcode, to be maintained in the transaction
            //    IBarcodeBL barcodeBL = new BarcodeBL();

            //    Barcode barcode = barcodeBL.GetBarcode(b => b.ReferenceId == transaction.Id).FirstOrDefault();
            //    string barcodeValue = (barcode != null) ? barcode.Value : string.Empty;

            //    //get transaction from the database as we need to map the localization in Solr
            //    Transaction transactionToBeIndexed = GetTransactionById(transaction.Id);

            //    //LogTransactionIndexing(transactionToBeIndexed, barcodeValue, false);
            //}
        }

        protected virtual void OnUpdate(Transaction transaction)
        {
            ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();

            TransactionAssignment transactionAssignment = transactionAssignmentBL.GetTransactionAssignment(User.Id, transaction.Id);

            if (transactionAssignment != null &&
                transactionAssignment.Action != null &&
                transactionAssignment.Action.Id == ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty))
            {
                throw new BusinessException(StatusCode.UpdateNotAllowDueToActionAsCopy);
            }

            if (transaction.RemindDate.HasValue)
            {
                transactionAssignment.DueDate = transaction.RemindDate.Value;
            }
            else if (!transaction.RemindDate.HasValue && transactionAssignment.Transaction.RemindDate.HasValue)
            {
                IUserManagementBL userManagementBL = new UserManagementBL();
                transactionAssignment.DueDate = transactionAssignment.Date.AddDays(userManagementBL.GetUserPreferenceByUserId(transactionAssignment.ToUserId.Value).UserProfile.TransactionProcessingPeriod);
            }
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            bool isSigned = transactionRepository.CheckIfTransactionSigned(transaction.Id);

            if (isSigned)
            {
                transaction.IsSigned = isSigned;
            }

            UpdateTransactionLinks(transaction);
            UpdateTransactionContactDate(transaction);

            transactionAssignment.TransactionPathId = transaction.TransactionPathId;
            transactionAssignmentBL.UpdateTransactionAssignment(transactionAssignment);

            bool updateDocument = false;
            bool isDocumentDeleted = false;
            int documentId = -1;

            if (transaction.MainDocument != null && transaction.MainDocument.IsDeleted)
            {
                documentId = transaction.MainDocument.Id;
                transaction.MainDocument = null;
                updateDocument = true;
                isDocumentDeleted = true;
            }

            bool isReserved = transaction.StatusId == TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

            //TODO:MH - IsMultiOwnerShip
            if (!TransactionAssignmentBL.IsMultiOwnerShip(transaction.Id))
            {
                transactionRepository.UpdateTransaction(transaction, updateDocument, isReserved);

                if (isReserved)
                {
                    int statusId = transaction.StatusId;
                    if ((Common.TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty) == Common.TransactionCategory.ExternalOutbound)
                    {
                        statusId = TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty); ;
                    }
                    else
                    {
                        statusId = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty); ;
                    }

                    UpdateTransactionStatus(transaction.Id, statusId);
                }
            }

            if (updateDocument == true)
            {
                transactionRepository.UpdateMainDcument(transaction.MainDocument, transaction.Id);
            }

            if (isDocumentDeleted)
            {
                IDocumentBL documentBL = new DocumentBL();

                documentBL.DeleteDocument(documentId);
            }
            if (transaction.Attachments == null || transaction.Attachments.Count == 0)
            {
                transactionRepository.CleanAttachment(transaction.Id);
            }
            //check if Solr indexing is enabled, if yes, then, do index the transation
            if (SystemConfigurations.IsSolrIndexingEnabled)
            {
                //do transaction indexing

                //get the barcode, to be maintained in the transaction
                IBarcodeBL barcodeBL = new BarcodeBL();

                Barcode barcode = barcodeBL.GetBarcode(b => b.ReferenceId == transaction.Id).FirstOrDefault();
                string barcodeValue = (barcode != null) ? barcode.Value : string.Empty;

                //get transaction from the database as we need to map the localization in Solr
                Transaction transactionToBeIndexed = GetTransactionById(transaction.Id);

                //LogTransactionIndexing(transactionToBeIndexed, barcodeValue, false);
            }
        }

        protected Transaction OnInboundUpdate(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.UpdateVipInbound(transactionFollowUps, transactionCopies, transactionId, ConfidentialityId, documentContent, summary);
        }
        protected Transaction OnOutboundInternalUpdate(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, byte[] documentContent, string summary)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.UpdateVipOutboundInternal(transactionFollowUps, transactionCopies, transactionId, ConfidentialityId, documentContent, summary);
        }
        protected Transaction OnOutboundDraftUpdate(List<TransactionFollowUp> transactionFollowUps, List<TransactionCopy> transactionCopies, int transactionId, int? ConfidentialityId, string mainDocumentContent, string pdfMainDocumentContent, bool isSigned)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.UpdateVipOutboundDraft(transactionFollowUps, transactionCopies, transactionId, ConfidentialityId, mainDocumentContent, pdfMainDocumentContent, isSigned);
        }
        protected virtual void PostUpdate(Transaction transaction)
        {
            AddTransactionHistory(transaction);
            AddTransactionEntityDetails(transaction);
        }

        public TransactionDetails Save(Transaction transaction, byte[] content = null)
        {
            try
            {
                TransactionDetails transactionDetails = null;

                Validate(transaction);

                PreSave(transaction);

                transactionDetails = OnSave(transaction);

                PostSave(transaction, content);

                //do indexing
                //DoTransactionIndex(transaction.Id);
                return transactionDetails;
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

        protected virtual void PreSave(Transaction transaction)
        {
            if (transaction.Links != null && transaction.Links.Count > 0)
            {
                if (!User.HasClaim(UserClaims.Links.Add))
                {
                    throw new BusinessException(StatusCode.PermissionLinkAddLink);
                }
            }

            //check if transaction name already exist

            if (transaction.Names != null && transaction.Names.Count > 0)
            {
                INameBL nameBL = new NameBL();

                foreach (TransactionName transactionName in transaction.Names)
                {
                    //check if name is exist
                    if (transactionName.Name.Id > 0)
                    {
                        nameBL.UpdateName(transactionName.Name);
                    }
                    else
                    {
                        nameBL.AddName(transactionName.Name);
                    }

                    transactionName.NameId = transactionName.Name.Id;
                    transactionName.Name = null;
                }
            }
        }

        protected virtual TransactionDetails OnSave(Transaction transaction, bool isReservation = false)
        {
            TransactionDetails result = new TransactionDetails();
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            transaction.Date = DateTime.Now;
            transaction.Year = DateTime.Now.Year;
            transaction.DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now);
            transaction.YearH = DateTimeUtility.GetHijriYear(DateTime.Now);
            transaction.ProcessPeriodTransaction = transaction.ProcessPeriodTransaction == null ? 0 : transaction.ProcessPeriodTransaction;
            if (!isReservation)
            {
                transaction.UserId = User.Id;
            }
            if ((Common.TransactionCategory)transaction.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, string.Empty) == Common.TransactionCategory.ExternalOutbound)
            {
                if (transaction.StatusId != TransactionStatus.NotSent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty))
                {
                    transaction.StatusId = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                }
            }
            else
            {
                //set the status of the transaction to InAction
                transaction.StatusId = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
            }

            if (isReservation)
            {
                transaction.StatusId = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
            }

            if (transaction.TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty) || transaction.TransactionCategoryId == (int)Common.TransactionCategory.InternalOutbound)
            {
                transaction.DeliveryMethodId = DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
            }

            transactionRepository.AddTransaction(transaction);

            Barcode transactionBarcode = AddTransactionBarcode(transaction);

            //check if Solr indexing is enabled, if yes, then, do index the transation
            if (SystemConfigurations.IsSolrIndexingEnabled)
            {
                //do transaction indexing

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
                    ITransactionTypeBL transactionSourceTypeBL = new TransactionTypeBL();

                    transaction.TransactionType = transactionSourceTypeBL.GetTransactionSourceTypeById(transaction.TransactionTypeId.Value);
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

            result.Date = transaction.CreatedOn;
            result.Number = transaction.Number;
            result.Id = transaction.Id;
            result.DateH = transaction.DateH;
            result.Status = transaction.StatusId;
            result.Barcode = transactionBarcode.Value;

            return result;
        }


        public static List<NotificationAttachment> Map(IList<Attachment> attachments)
        {
            if (attachments == null || !attachments.Any())
            {
                return new List<NotificationAttachment>();
            }
            List<NotificationAttachment> notificationAttachment = attachments
                .Select(attachment => new NotificationAttachment()
                {
                    Id = attachment.Id,
                    Binary = attachment.DocumentInfo != null ? attachment.DocumentInfo?.Document?.Content : null,
                    ContentLength = attachment.DocumentInfo != null ? Convert.ToInt32(attachment.DocumentInfo.Size) : 0,
                    FileName = attachment.Description,
                    ContentType = attachment.DocumentInfo != null ? attachment.DocumentInfo.MimeType : null,
                }).ToList();
            return notificationAttachment;
        }

        public static NotificationAttachment MapMainDocument(DocumentInfo documentInfo)
        {
            if (documentInfo != null)
            {
                NotificationAttachment notificationAttachment = new NotificationAttachment();
                notificationAttachment.Id = documentInfo.Id;
                notificationAttachment.Binary = documentInfo.Document.Content;
                notificationAttachment.ContentLength = Convert.ToInt32(documentInfo.Size);
                notificationAttachment.FileName = documentInfo.Name;
                notificationAttachment.ContentType = documentInfo.MimeType;
                return notificationAttachment;
            }
            return null;
        }

        protected virtual void PostSave(Transaction transaction, byte[] content = null)
        {
            MoveTransaction(transaction);
            AddTransactionHistory(transaction);
            AddTransactionEntityDetails(transaction);
            AddTransactionCopiesBarcode(transaction);
            SendNotification(transaction);
        }

        private void SendCopiesNotification(Transaction transaction)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            List<NotificationAttachment> attachments = new List<NotificationAttachment>();
            attachments = Map(transaction.Attachments);
            if (transaction.MainDocument.Document != null && transaction.MainDocument.Document.Content != null)
            {
                attachments.Add(MapMainDocument(transaction.MainDocument));
            }
            //Notification => Web
            if (transaction.Copies != null && transaction.Copies.Count > 0)
            {
                IUserManagementBL userManagementBL = new UserManagementBL();
                var transactionForNotification = GetTransactionByIdForNotification(transaction.Id);
                foreach (var copy in transaction.Copies)
                {
                    if (copy.UserId.HasValue)
                    {
                        if (copy.SendEmail == true)
                        {
                            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(copy.UserId.Value);
                            if (notificationSubscriptions.HasFlag(NotificationSubscriptions.ElectronicCopies))
                            {
                                IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(copy.UserId.Value) };
                                SendTransactionNotification(transactionForNotification, NotificationSource.ElectronicCopies, NotificationTemplateType.ElectronicCopiesWeb,
                                    NotificationTemplateType.ElectronicCopiesEmail, NotificationEmailSubject.ElectronicCopiesEmail, NotificationWebSubject.ElectronicCopies,
                                    notificationUsers, "ar", attachments);
                            }
                            transactionRepository.UpdateCopy(copy.Id);
                        }
                    }
                }
            }
        }
        private void SendTransactionNotification(Transaction transaction, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
          NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
                      IList<NotificationUser> notificationUsers, string cultureName, IList<NotificationAttachment> attachments, bool externalParty = false)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransTypeId}"] = transaction.TransactionCategoryId.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory != null ? transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text : "";
                keyValues["{sender}"] = User.UserName;
                keyValues["{Date}"] = transaction.DateH;
                keyValues["{PriorityId}"] = transaction.Priority.LocalizationIdentifier.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.Name.Localizations.FirstOrDefault(l => l.Culture.ShortName == cultureName).Text;
                keyValues["{TransactionId}"] = transaction.Id.ToString();
                keyValues["{UserName}"] = User.UserName;
                keyValues["{OrgName}"] = OrgUnitBL.GetOrgUnitName(o => o.Id == transaction.OrgUnitId, cultureName);
                keyValues["{RemindDate}"] = transaction.RemindDateH;

                if (!externalParty)
                {
                    //System Notification Web
                    NotificationsManager.SystemNotification(notificationSource, notificationTemplateType, notificationWebSubject, notificationUsers, cultureName, keyValues);
                }

                //System Notification Email
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    TenantBL tenantBL = new TenantBL();
                    tenantBL.PrepareTanentNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsers.FirstOrDefault().User.Email, cultureName, null, keyValues);
                }
                else
                {
                    List<NotificationUser> notificationUsersEmail = new List<NotificationUser>();
                    if (externalParty)
                    {
                        notificationUsersEmail = notificationUsers.ToList();
                    }
                    else
                    {
                        notificationUsersEmail = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(notificationUsers.FirstOrDefault().User.Id) };
                    }
                    //System Notification  Email
                    NotificationsManager.EmailNotification(notificationSource, notificationEmailTemplateType,
                        notificationEmailSubject, notificationUsersEmail, cultureName, attachments, keyValues);
                }
            }
        }
        protected void AddTransactionEntityDetails(Transaction transaction)
        {
            ITransactionEntityDetailsRepository transactionEntityDetailsRepository = IoC.Resolve<ITransactionEntityDetailsRepository>();
            TransactionEntityDetails transactionEntityDetails = new TransactionEntityDetails
            {
                TransactionId = transaction.Id,
                EntityId = transaction.OrgUnitId
            };
            transactionEntityDetailsRepository.AddTransactionEntityDetails(transactionEntityDetails);

            if (transaction.Copies.Count > 0)
            {
                foreach (TransactionCopy transactionCopy in transaction.Copies)
                {
                    transactionEntityDetails = new TransactionEntityDetails
                    {
                        TransactionId = transaction.Id,
                        EntityId = transactionCopy.EntityId.Value
                    };
                    transactionEntityDetailsRepository.AddTransactionEntityDetails(transactionEntityDetails);
                }
                SendCopiesNotification(transaction);
            }
        }
        protected void CheckTransactionConfidentiality(int confidentialityId, int transactionId, int userId)
        {
            IPermissionBL permissionBL = new PermissionBL();

            Permission confidentiality = permissionBL.GetPermissionById(confidentialityId);

            if (!User.HasClaim(confidentiality.Code))
            {
                if (!CheckIfHasSpecialAuthorize(transactionId, userId))
                    throw new BusinessException(StatusCode.TransactionSourceTypeConfidentialityRequired);
            }
        }

        protected void CheckTransactionConfidentialityForPath(int confidentialityId, int pathId)
        {
            IPermissionBL permissionBL = new PermissionBL();
            ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<ITransactionAssignmentRepository>();

            Permission confidentiality = permissionBL.GetPermissionById(confidentialityId);
            int userPermissionsCount = permissionBL.GetTransactionPathUsersPermissions(pathId, confidentiality.Id).Count;
            int transactionPathDetailsCount = transactionAssignmentRepository.GetTransactionPathCount(pathId, true);

            if (userPermissionsCount != transactionPathDetailsCount)
            {
                throw new BusinessException(StatusCode.TransactionPathConfidentialityRequired);
            }
        }

        protected virtual void PostAssignTransaction(IList<TransactionAssignment> transactionAssignments)
        {
            ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();

            foreach (TransactionAssignment transactionAssignment in transactionAssignments)
            {
                transactionAssignmentHistoryBL.AddTransactionAssignmentHistory(transactionAssignment);
            }
        }

        protected virtual void PreReadTransaction(Transaction transaction, UserProfile userProfile)
        {

        }

        protected virtual void PostReadTransaction(Transaction transaction, UserProfile userProfile)
        {

        }

        protected virtual void PreRevertAssignTransaction(Transaction transaction, UserProfile userProfile)
        {

        }

        protected virtual void OnRevertAssignTransaction(Transaction transaction, UserProfile userProfile)
        {

        }

        protected virtual void PostRevertAssignTransaction(Transaction transaction, UserProfile userProfile)
        {

        }

        protected virtual void PreGetTransactionBarcodes(int transactionId)
        {

        }

        protected virtual TransactionBarcodesInfo OnGetTransactionBarcodes(int transactionId, int OrgUnitId, string cultureName)
        {
            IBarcodeBL barcodeBL = new BarcodeBL();
            ILookupBL lookupBL = new LookupBL();
            ISettingBL settingBL = new SettingBL();
            IOrgUnitBL OrgUnitBL = new OrgUnitBL();
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            //Transaction transaction = GetTransactionById(transactionId);
            Transaction transaction = transactionRepository.GetTransaction(t => t.Id == transactionId, cultureName);

            BarcodeDesignType barcodeDesignType = 
                GetBarcodeDesignType((Common.TransactionCategory)transaction.TransactionCategory
                .Id.LookupInternalID(LookupCategory.TransactionCategory, cultureName));

            BarcodeDesign transactionBarcodeDesign = barcodeBL
                .GetBarcodeDesign(barcodeDesignType, OrgUnitId);

            if (transactionBarcodeDesign == null)
            {
                throw new BusinessException(StatusCode.BarcodeDesignNotFound);
            }

            string OrgUnitRootName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName);
            string OrgUnitSymbol = OrgUnitBL.GetOrgUnitSymbol(OrgUnitId);

            TransactionBarcodesInfo transactionBarcodes = new TransactionBarcodesInfo();

            transactionBarcodes.TransactionDesignHeight = transactionBarcodeDesign.Height;
            transactionBarcodes.TransactionDesignWidth = transactionBarcodeDesign.Width;
            transactionBarcodes.TransactionBarcodeHtmlDesign = transactionBarcodeDesign.Html;
            transactionBarcodes.TransactionDate = transaction.Date;
            transactionBarcodes.TransactionDateH = transaction.DateH;
            transactionBarcodes.TransactionNumber = transaction.Number;
            transactionBarcodes.OrgUnitSymbol = OrgUnitSymbol;
            //transactionBarcodes.FromEntity = this.GetSourceName(transaction, cultureName);
            //transactionBarcodes.ToEntity = this.GetDestinationName(transaction, cultureName);
            transactionBarcodes.Date = transaction.Date;
            transactionBarcodes.DateH = transaction.DateH;
            transactionBarcodes.CompanyName = OrgUnitRootName;
            transactionBarcodes.TransactionType = transaction.TransactionType.Abbreviation?.Localizations
                .Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
            transactionBarcodes.TransactionCategory = transaction.TransactionCategoryId;
            transactionBarcodes.TransactionAttachmentHtmlDesign = transactionBarcodeDesign.AttachmentHtml;
            transactionBarcodes.Entity = transaction.OrgUnit.LocalName;
            if (transaction.ExternalParty != null && transaction.ExternalParty.Name != null)
            {
                transactionBarcodes.OutboundDestination = transaction.ExternalParty.Name.Localizations
                    .Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
            }
            transactionBarcodes.Barcodes = new List<Barcode>();
            int value = BarcodeReferenceType.MainTransaction.LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName);
            Barcode barcode = barcodeBL.GetBarcode(b => b.ReferenceId == transaction.Id 
            & b.ReferenceType.Id == value).FirstOrDefault();

            if (barcode == null)
            {
                barcode = AddTransactionBarcode(transaction);
            }

            Lookup mainReferenceType = lookupBL.GetLookupItem(BarcodeReferenceType.MainTransaction.LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName));


            if (barcode != null && mainReferenceType != null)
            {
                barcode.ReferenceType = mainReferenceType;

                transactionBarcodes.TicketBarcode = barcode;

                transactionBarcodes.Barcodes.Add(barcode);
                transactionBarcodes.CustomBarcodes
                                        .Add(new BarcodeInfo
                                        {
                                            Value = barcode.Value,
                                            ReferenceType = barcode.ReferenceType,
                                        });
            }

            foreach (TransactionCopy transactionCopy in transaction.Copies)
            {
                int referenceTypeId = BarcodeReferenceType.Copy
                    .LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName);
                Barcode barcodeCopy = barcodeBL
                    .GetBarcode(b => b.ReferenceId == transactionCopy.Id && b.ReferenceTypeId == referenceTypeId).FirstOrDefault();
                Lookup referenceType = lookupBL
                    .GetLookupItem(BarcodeReferenceType.Copy.LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName));

                if (barcodeCopy != null && referenceType != null)
                {
                    transactionBarcodes.CustomBarcodes
                        .Add(new BarcodeInfo
                        { 
                            Value = barcodeCopy.Value, 
                            ReferenceType = referenceType,
                            EntityName = transactionCopy.Entity.LocalName

                        });
                }


            }

            foreach (TransactionExternalCopy transactionCopy in transaction.ExternalCopies)
            {
                int referenceTypeId = BarcodeReferenceType.Copy.LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName);
                Barcode barcodeCopy = barcodeBL
                    .GetBarcode(b => b.ReferenceId == transactionCopy.Id && b.ReferenceTypeId == referenceTypeId).FirstOrDefault();
                Lookup referenceType = lookupBL.GetLookupItem(BarcodeReferenceType.Copy.LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName));

                if (barcodeCopy != null && referenceType != null)
                {
                    transactionBarcodes.CustomBarcodes
                        .Add(new BarcodeInfo
                        {
                            Value = barcodeCopy.Value,
                            ReferenceType = referenceType,
                            EntityName = transactionCopy.Entity.LocalName

                        });
                }


            }
            transactionBarcodes.AttachmentBarcodes = new List<AttachmentBarcode>();

            foreach (Attachment attachment in transaction.Attachments)
            {
                Localization nameLocalization  = new Localization();
                Barcode attachmentBarcode = new Barcode();

                attachmentBarcode.Value = BarecodeNumberGenerator.GenerateForAttachment(OrgUnitId, transaction.Number,
                    attachment.Id, transaction.Year);

                attachmentBarcode.ReferenceId = attachment.Id;

                string name = string.Empty;
                //if (attachment.Type.LocalizationIdentifier != null)
                //{

                //    nameLocalization = attachment.Type.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();
                //}

                //if (nameLocalization != null)
                //{
                //}
                    name = attachment.Type.Text;

                transactionBarcodes.AttachmentBarcodes.Add(new AttachmentBarcode { Id = attachment.Id, Name = name, Count = attachment.Count });

                transactionBarcodes.Barcodes.Add(attachmentBarcode);
            }

            return transactionBarcodes;
        }

        protected virtual void PostGetTransactionBarcodes(int transactionId)
        {

        }

        protected virtual void PrePrintTransactionAttachmentBarcode(Transaction transaction)
        {
        }

        protected virtual void PostPrintTransactionAttachmentBarcode(Transaction transaction)
        {
        }

        protected virtual void PrePrintTransactionCopiesBarcode(Transaction transaction)
        {
        }

        protected virtual void PostPrintTransactionCopiesBarcode(Transaction transaction)
        {
        }

        protected virtual void PrePrintTransactionTicket(Transaction transaction)
        {

        }

        protected virtual TransactionTicket OnPrintTransactionTicket(Transaction transaction)
        {
            IBarcodeBL barcodeBL = new BarcodeBL();

            TransactionTicket transactionTicket = new TransactionTicket()
            {
                barcode = barcodeBL.GetBarcode(bc => bc.ReferenceType.Id == transaction.TransactionCategory.Id & bc.ReferenceId == transaction.Id).FirstOrDefault(),
                SequenceNumber = transaction.Id,
                Number = transaction.Number,
                Date = DateTime.Now
            };

            return transactionTicket;
        }

        protected virtual void PostPrintTicket(Transaction transaction)
        {
        }

        protected virtual void UpdateTransactionAttachments(Transaction transaction)
        {
            IDocumentBL documentBL = new DocumentBL();

            foreach (Attachment attachment in transaction.Attachments)
            {
                if (attachment.DocumentInfo != null && attachment.DocumentInfo.IsDeleted)
                {
                    int documentId = attachment.DocumentInfo.Id;

                    attachment.DocumentInfo = null;
                }

                if (attachment.Id > 0 && attachment.DocumentInfo != null && attachment.DocumentInfo.Document == null)
                {
                    DocumentInfo copiedDocumentInfo = documentBL.GetDocumentById(attachment.DocumentInfo.Id);
                    if (copiedDocumentInfo != null)
                    {
                        DocumentInfo documentInfo = copiedDocumentInfo.ShallowCopy();
                        attachment.DocumentInfo = documentInfo;
                    }

                }
            }

            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            transactionRepository.UpdateTransactionAttachments(transaction.Id, transaction.Attachments);
        }

        protected virtual void UpdateTransactionNames(Transaction transaction)
        {
            //IList<TransactionName> transactionNames = new List<TransactionName>();

            //if (transaction.Names != null && transaction.Names.Count > 0)
            //{
            //    INameBL nameBL = new NameBL();

            //    foreach (TransactionName transactionName in transaction.Names)
            //    {
            //        //check if name is exist
            //        if (transactionName.Name.Id > 0)
            //        {
            //            nameBL.UpdateName(transactionName.Name);

            //            transactionName.NameId = transactionName.Name.Id;
            //            transactionName.Name = null;
            //        }
            //        else
            //        {
            //            nameBL.AddName(transactionName.Name);

            //            transactionName.NameId = transactionName.Name.Id;
            //            transactionName.Name = null;
            //        }

            //        transactionNames.Add(transactionName);
            //    }

            //    transaction.Names.ToList().ForEach(n =>
            //           transaction.Names.Remove(n)
            //           );

            //    transaction.Names = transactionNames;
            //}

            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

            transactionRepository.UpdateTransactionNames(transaction.Id, transaction.Names);
        }

        public static List<Transaction> GetTransactionsByExternalPartyId(int externalPartyId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return null; //transactionRepository.GetTransactionsByExternalPartyId(externalPartyId);
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

        public static IList<Transaction> GetTransactionsByOrgUnitId(int OrgUnitId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactions(t => t.OrgUnitId == OrgUnitId ||
                    //t.SignedByOrgUnitId == OrgUnitId ||
                    t.EntityId == OrgUnitId);
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
        public virtual TransactionCertificateInfo GetTransactionCertificate(int transactionId, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                ITransactionAssignmentHistoryBL transactionAssignmentHistoryBL = new TransactionAssignmentHistoryBL();
                ITransactionAssignmentBL transactionAssignmentBL = new TransactionAssignmentBL();
                ITransactionLoggingBL transactionLoggingBL = new TransactionLoggingBL();
                IEditorBL editorBL = new EditorBL();
                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }

                TransactionCertificateInfo transactionCertificate = transactionRepository.GetTransactionCertificate(transactionId, cultureName, userWeight);

                transactionCertificate.AssignmentsHistory = transactionAssignmentHistoryBL.GetTransactionAssignmentHistories(transactionId, cultureName);

                transactionCertificate.Copies = transactionRepository.GetTransactionCopiesByTransactionId(transactionId, User.Id, cultureName);

                transactionCertificate.ExternalCopies = transactionRepository.GetTransactionExternalCopiesByTransactionId(transactionId, cultureName);

                transactionCertificate.TransactionLog = transactionLoggingBL.GetTransactionLogInfo(transactionId, cultureName);

                transactionCertificate.Explanations = editorBL.GetExplanationsByTransactionId(transactionId, cultureName);
                int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, cultureName);
                IList<TransactionAssignment> transactionAssignments = transactionAssignmentBL.GetTransactionAssignments(a => a.TransactionId == transactionId && a.Action.Type.Id != SendCopyToView, cultureName);

                bool isMultiOwnership = (transactionAssignments.Count > 1);

                transactionCertificate.IsMultiOwnership = isMultiOwnership;

                //if (isMultiOwnership)
                //{
                transactionCertificate.CurrentAssignment = transactionAssignments.FirstOrDefault();

                //}

                return transactionCertificate;
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

        public virtual void AddTransactionLinks(int transactionId, IList<TransactionLink> transactionLinks)
        {
            try
            {
                if (!User.HasClaim(UserClaims.Links.Add))
                {
                    throw new BusinessException(StatusCode.PermissionLinkAddLink);
                }

                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                transactionRepository.UpdateTransactionLinks(transactionId, transactionLinks);
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

        private BarcodeDesignType GetBarcodeDesignType(Common.TransactionCategory transactionCategory)
        {
            switch (transactionCategory)
            {
                case Common.TransactionCategory.Inbound:
                    return BarcodeDesignType.Inbound;
                case Common.TransactionCategory.DraftOutbound:
                case Common.TransactionCategory.ExternalOutbound:
                    return BarcodeDesignType.Outbound;
                case Common.TransactionCategory.InternalOutbound:
                    return BarcodeDesignType.OutboundInternal;
            }

            return BarcodeDesignType.None;
        }

        public static DocumentInfo GetMainDocumentByTransactionId(int transactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                DocumentInfo documentInfo = transactionRepository.GetMainDocumentByTransactionId(transactionId);

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


        public static DocumentInfo GetOldMainDocumentByTransactionId(int transactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                DocumentInfo documentInfo = transactionRepository.GetOldMainDocumentByTransactionId(transactionId);

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

        public static IList<TransactionCountReportInfo> GetDashboardData(string cultureName)
        {
            try
            {
                IList<TransactionCountReportInfo> transactionTypeReportInfos = new List<TransactionCountReportInfo>();

                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                IList<Transaction> transactions = transactionRepository.GetTransactions(t => t.Status.Id == (TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty)));

                foreach (Transaction transaction in transactions)
                {
                    transactionTypeReportInfos.Add(new TransactionCountReportInfo
                    {
                        TypeId = transaction.TransactionCategory.Id,
                        Date = transaction.Date,
                        UserCategoryId = (transaction.User.Category != null) ? transaction.User.Category.Id : -1
                    });
                }

                return transactionTypeReportInfos;
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

        public static void UpdateTransactionDocument(int transactionId, DocumentInfo documentInfo)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();

                Transaction transaction = GetTransactionById(transactionId);

                if (documentInfo.Id > 0)
                {

                    transaction.MainDocument = documentInfo;
                    transactionRepository.UpdateTransaction(transaction);
                }
                else
                {
                    documentInfo.TransactionId = transactionId;
                    int documentId = documentRepository.AddDocument(documentInfo);

                    transaction.MainDocumentId = documentId;

                    transactionRepository.UpdateTransaction(transaction);
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

        public static void AddDeliveryReportToAttachment(Attachment attachment)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.AddDeliveryReportToAttachment(attachment);
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

        public static TransactionDetailsInfo MapTransaction(Transaction transaction, string cultureName)
        {
            try
            {
                ITransactionTaskBL transactiontask = IoC.Resolve<TransactionTaskBL>();
                TransactionDetailsInfo transactionDetailsInfo = new TransactionDetailsInfo
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
                    TransactionCategoryId = (transaction.TransactionCategory != null) ? transaction.TransactionCategory.Id : 0,
                    ConfidentialityName = (transaction.Confidentiality != null) ? transaction.Confidentiality.LocalName : null,
                    ConfidentialityId = (transaction.Confidentiality != null) ? transaction.Confidentiality.Id : -1,
                    ExternalPartyName = (transaction.ExternalParty != null) ? transaction?.ExternalParty?.Name?.Localizations.FirstOrDefault()?.LocalizationIdentifier?.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault()?.Text ?? transaction.ExternalParty.LocalName : null,
                    ExternalPartyId = (transaction.ExternalParty != null) ? transaction.ExternalParty.Id : -1,
                    ExternalPartyManagerName = (transaction.ExternalPartyManager != null) ? transaction.ExternalPartyManager.LocalName : null,
                    ExternalPartyManagerId = (transaction.ExternalPartyManager != null) ? transaction.ExternalPartyManager.Id : -1,
                    LetterTypeName = (transaction.LetterType != null) ? transaction.LetterType.Text : null,
                    PriorityName = (transaction.Priority != null) ? transaction.Priority.Text : null,
                    PriorityId = (transaction.Priority != null) ? transaction.Priority.Id : -1,
                    //transactionDetailsInfo.//SignedByOrgUnitName = (transaction.SignedByOrgUnit != null) ? transaction.SignedByOrgUnit.LocalName : null;
                    //transactionDetailsInfo.//SignedByOrgUnitId = (transaction.SignedByOrgUnit != null) ? transaction.SignedByOrgUnit.Id : -1;
                    SignedByUserName = (transaction.SignedByUser != null) ? transaction.SignedByUser.LocalName : null,
                    SignedByUserId = (transaction.SignedByUser != null) ? transaction.SignedByUser.Id : -1,
                    TransactionTypeName = (transaction.TransactionType != null) ? transaction.TransactionType.Text : null,
                    TransactionTypeId = (transaction.TransactionType != null) ? transaction.TransactionType.Id : -1,
                    TransactionTypeColorId = (transaction.TransactionType != null) ? transaction.TransactionType.Color.Id : -1,
                    ToEntityName = (transaction.Entity != null) ? transaction.Entity.LocalName : string.Empty,
                    ToUserName = (transaction.ToUser != null) ? transaction.ToUser.LocalName : string.Empty,
                    TransactionCategory = (transaction.TransactionCategory != null) ? transaction.TransactionCategory.Text : string.Empty,
                    EntityName = (transaction.OrgUnit != null) ? transaction.OrgUnit.LocalName : string.Empty,
                    User = (transaction.User != null) ? transaction.User.LocalName : string.Empty,
                    Status = (transaction.Status != null) ? transaction.Status.Text : string.Empty,
                    UserId = (transaction.User != null) ? transaction.User.Id : -1,
                    ToUserId = (transaction.ToUser != null) ? transaction.ToUser.Id : -1,
                    IsLate = false,
                    StatusId = transaction.StatusId,
                    RejectionReason = transaction.RejectionReason,
                    HasPermission = transaction.HasPermission,
                    SavedReason = transaction.SavedReason,
                    DeliveryMethodId = (transaction.Assignments != null) ? transaction.Assignments[0].DeliveryMethodId : DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, cultureName),
                    TransactionPathId = transaction.Assignments != null ? transaction.Assignments.FirstOrDefault() != null ? transaction.Assignments.FirstOrDefault().TransactionPathId : null : null,
                    IsIndividual = transaction.IsForIndividual,
                    DeliveryMethodName = (transaction.Assignments != null) && transaction.Assignments[0].DeliveryMethod != null ? transaction.Assignments[0].DeliveryMethod.Text : string.Empty,
                    FollowupDateH = (transaction.FollowUp != null && transaction.FollowUp.Count > 0) ? transaction.FollowUp.Where(f => f.FollowUpUserId == UserContext.LoggedInUser.Id).Select(r => r.DateToH).FirstOrDefault() : string.Empty,
                    FollowupDate = (transaction.FollowUp != null && transaction.FollowUp.Count > 0) ? transaction.FollowUp.Where(f => f.FollowUpUserId == UserContext.LoggedInUser.Id).Select(r => r.DateTo).FirstOrDefault() : null,
                    HasLinks = transaction.HasLinks,
                    YesserRegistered = (transaction.ExternalParty != null) ? transaction.ExternalParty.YasserRegistered : false,
                    PrivecyName = (transaction.Privecy != null) ? transaction.Privecy.Text : null,
                    PrivecyId = (transaction.Privecy != null) ? transaction.Privecy.Id : -1,
                    isDeleted = transaction.IsDeleted,
                    IsPresentationDraft = transaction.IsPresentationDraft,
                    IsElcOutBound = transaction.IsElcOutBound,
                    NeedAcknowled = transaction.NeedAcknowled,
                    IsImportant = transaction.IsImportant,
                    HasTask = transactiontask.GetTaskCount(transaction.Id) > 0 ? true : false,
                    Encrypted = transaction.Encrypted

                };

                int count = 0;

                if (transaction.Attachments != null && transaction.Attachments.Any())
                {
                    foreach (Attachment attachment in transaction.Attachments)
                    {
                        count = count + attachment.Count;
                    }
                }

                transactionDetailsInfo.AttachmentCount = count;

                return transactionDetailsInfo;
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

        public static IList<TransactionTrayInfo> MapTransaction(IList<Transaction> transactions, string culturName)
        {
            try
            {
                IList<TransactionTrayInfo> transactionTrayInfos = new List<TransactionTrayInfo>();

                if (transactions != null)
                {
                    foreach (Transaction transaction in transactions)
                    {
                        TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo();

                        transactionTrayInfo.transactionDetailsInfo = MapTransaction(transaction, culturName);

                        transactionTrayInfos.Add(transactionTrayInfo);
                    }
                }

                return transactionTrayInfos;
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

        public static TransactionDetailsInfo MapTransactionCopy(TransactionCopy transaction, string cultureName)
        {
            try
            {
                TransactionDetailsInfo transactionDetailsInfo = new TransactionDetailsInfo
                {
                    Id = transaction.Transaction.Id,
                    Date = transaction.Transaction.Date,
                    DateH = transaction.Transaction.DateH,
                    Number = transaction.Transaction.Number,
                    Remarks = transaction.Transaction.Remarks,
                    RemindDate = transaction.Transaction.RemindDate,
                    RemindDateH = transaction.Transaction.RemindDateH,
                    Subject = transaction.Transaction.Subject,
                    DocumentNumber = transaction.Transaction.DocumentNumber,
                    TransactionCategoryId = (transaction.Transaction.TransactionCategory != null) ? transaction.Transaction.TransactionCategory.Id : 0,
                    ConfidentialityName = (transaction.Transaction.Confidentiality != null) ? transaction.Transaction.Confidentiality.LocalName : null,
                    ConfidentialityId = (transaction.Transaction.Confidentiality != null) ? transaction.Transaction.Confidentiality.Id : -1,
                    ExternalPartyName = (transaction.Transaction.ExternalParty != null) ? transaction.Transaction.ExternalParty.LocalName : null,
                    ExternalPartyId = (transaction.Transaction.ExternalParty != null) ? transaction.Transaction.ExternalParty.Id : -1,
                    ExternalPartyManagerName = (transaction.Transaction.ExternalPartyManager != null) ? transaction.Transaction.ExternalPartyManager.LocalName : null,
                    ExternalPartyManagerId = (transaction.Transaction.ExternalPartyManager != null) ? transaction.Transaction.ExternalPartyManager.Id : -1,
                    LetterTypeName = (transaction.Transaction.LetterType != null) ? transaction.Transaction.LetterType.Text : null,
                    PriorityName = (transaction.Transaction.Priority != null) ? transaction.Transaction.Priority.Text : null,
                    PriorityId = (transaction.Transaction.Priority != null) ? transaction.Transaction.Priority.Id : -1,
                    //transactionDetailsInfo.//SignedByOrgUnitName = (transaction.SignedByOrgUnit != null) ? transaction.SignedByOrgUnit.LocalName : null;
                    //transactionDetailsInfo.//SignedByOrgUnitId = (transaction.SignedByOrgUnit != null) ? transaction.SignedByOrgUnit.Id : -1;
                    SignedByUserName = (transaction.Transaction.SignedByUser != null) ? transaction.Transaction.SignedByUser.LocalName : null,
                    SignedByUserId = (transaction.Transaction.SignedByUser != null) ? transaction.Transaction.SignedByUser.Id : -1,
                    TransactionTypeName = (transaction.Transaction.TransactionType != null) ? transaction.Transaction.TransactionType.Text : null,
                    TransactionTypeId = (transaction.Transaction.TransactionType != null) ? transaction.Transaction.TransactionType.Id : -1,
                    TransactionTypeColorId = (transaction.Transaction.TransactionType != null) ? transaction.Transaction.TransactionType.Color.Id : -1,
                    ToEntityName = (transaction.Transaction.Entity != null) ? transaction.Transaction.Entity.LocalName : string.Empty,
                    ToUserName = (transaction.Transaction.ToUser != null) ? transaction.Transaction.ToUser.LocalName : string.Empty,
                    TransactionCategory = (transaction.Transaction.TransactionCategory != null) ? transaction.Transaction.TransactionCategory.Text : string.Empty,
                    EntityName = (transaction.Transaction.OrgUnit != null) ? transaction.Transaction.OrgUnit.LocalName : string.Empty,
                    User = (transaction.Transaction.User != null) ? transaction.Transaction.User.LocalName : string.Empty,
                    Status = (transaction.Transaction.Status != null) ? transaction.Transaction.Status.Text : string.Empty,
                    UserId = (transaction.Transaction.User != null) ? transaction.Transaction.User.Id : -1,
                    IsLate = false,
                    StatusId = transaction.Transaction.StatusId,
                    RejectionReason = transaction.Transaction.RejectionReason,
                    HasPermission = transaction.Transaction.HasPermission,
                    SavedReason = transaction.Transaction.SavedReason,
                    DeliveryMethodId = (transaction.Transaction.Assignments != null) ? transaction.Transaction.Assignments[0].DeliveryMethodId : DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, cultureName),
                    TransactionPathId = transaction.Transaction.Assignments != null ? transaction.Transaction.Assignments.FirstOrDefault() != null ? transaction.Transaction.Assignments.FirstOrDefault().TransactionPathId : null : null,
                    IsIndividual = transaction.Transaction.IsForIndividual,
                    DeliveryMethodName = (transaction.Transaction.Assignments != null) && transaction.Transaction.Assignments[0].DeliveryMethod != null ? transaction.Transaction.Assignments[0].DeliveryMethod.Text : string.Empty,
                    FollowupDateH = (transaction.Transaction.FollowUp != null && transaction.Transaction.FollowUp.Count > 0) ? transaction.Transaction.FollowUp.Where(f => f.FollowUpUserId == UserContext.LoggedInUser.Id).Select(r => r.DateToH).FirstOrDefault() : string.Empty,
                    FollowupDate = (transaction.Transaction.FollowUp != null && transaction.Transaction.FollowUp.Count > 0) ? transaction.Transaction.FollowUp.Where(f => f.FollowUpUserId == UserContext.LoggedInUser.Id).Select(r => r.DateTo).FirstOrDefault() : null,
                    HasLinks = transaction.Transaction.HasLinks,
                    CopyStatus = transaction.Transaction.StatusId,
                    IsOpr = transaction.IsOpr,
                    OprEntityId = transaction.OprEntityId,
                    SpecialCopy = transaction.SpecialCopy,
                    OprEntityName = transaction.OprEntity?.LocalName,
                    IsBcc = transaction.IsBcc,
                    TransactionCopyId = transaction.Id,
                };

                int count = 0;

                if (transaction.Transaction.Attachments != null && transaction.Transaction.Attachments.Any())
                {
                    foreach (Attachment attachment in transaction.Transaction.Attachments)
                    {
                        count = count + attachment.Count;
                    }
                }

                transactionDetailsInfo.AttachmentCount = count;

                return transactionDetailsInfo;
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

        public static IList<TransactionTrayInfo> MapTransactionCopy(IList<TransactionCopy> transactions, string culturName)
        {
            try
            {
                IList<TransactionTrayInfo> transactionTrayInfos = new List<TransactionTrayInfo>();

                if (transactions != null)
                {
                    foreach (TransactionCopy transaction in transactions)
                    {
                        TransactionTrayInfo transactionTrayInfo = new TransactionTrayInfo();

                        transactionTrayInfo.transactionDetailsInfo = MapTransactionCopy(transaction, culturName);

                        transactionTrayInfos.Add(transactionTrayInfo);
                    }
                }

                return transactionTrayInfos;
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

        public TransactionVisitTicketInfo GetVisitTicket(Transaction transaction, int OrgUnitId, string cultureName)
        {
            try
            {
                TransactionVisitTicketInfo transactionVisitTicket = null;
                PreGetVisitTicket(transaction);
                transactionVisitTicket = OnGetVisitTicket(transaction, OrgUnitId, cultureName);
                PostGetVisitTicket(transaction);
                return transactionVisitTicket;

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

        public static IList<Transaction> GetUserTransactionsTray(int userId, int OrgUnitId, TrayType trayType, TransactionDateType transactionDate, SearchCriteriaCustom searchCriteria, out int rowsCount)
        {
            try
            {
                bool isManager = false;
                if (trayType == TrayType.Saved)
                {
                    IOrgUnitBL orgUnitBL = new OrgUnitBL();
                    OrgUnit orgUnit = orgUnitBL.GetOrgUnitById(OrgUnitId);
                    if (orgUnit.ManagerId == userId)
                    {
                        isManager = true;
                    }
                }

                rowsCount = 0;
                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                searchCriteria.UserId = userId;

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }

                Expression<Func<TransactionAssignment, bool>> where = null;

                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int deletedStatus = TransactionStatus.Deleted.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int SendCopyToView = (int)ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Completed = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int electronicDelivery = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                int electronicPaperDelivery = DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);


                switch (trayType)
                {
                    case TrayType.MyTransactions:
                        where = (s =>
                     s.ToUserId == userId &
                     s.TrayId == (int)trayType &
                     s.ToEntityId == OrgUnitId &
                     (s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected | s.Transaction.Status.Id == MultiOwnership) &
                     s.Transaction.TransactionCategoryId != ExternalOutbound
                     );
                        break;
                    case TrayType.DraftOutbound:
                        where = (s =>
                     s.ToUserId == userId &
                     s.ToEntityId == OrgUnitId &&
                     s.Transaction.StatusId != Reserved &&
                     (((s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected) & s.Transaction.TransactionCategoryId == DraftOutbound & s.TrayId == (int)trayType)
                     | (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft))
                        /*| (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & s.Transaction.DeliveryMethodId == electronicPaperDelivery
                           & !s.Transaction.PrintedDeliveryReport & s.Transaction.StatusId != Outbound & s.Transaction.StatusId != Sent & s.Transaction.StatusId != Reserved)*/
                        )
                     );
                        break;
                    case TrayType.DeletedDraftOutbound:
                        where = (s =>
                            s.ToUserId == userId &
                            s.ToEntityId == OrgUnitId &
                            (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft) & s.Transaction.StatusId == deletedStatus);
                        break;
                    case TrayType.SentTransactions:
                        where = (s =>
                                 s.FromUserId == userId &
                                 s.FromEntityId == OrgUnitId &
                                 s.TrayId == (int)trayType &
                                (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership) &
                                 s.Transaction.TransactionCategoryId != ExternalOutbound
                                 );
                        break;
                    case TrayType.Saved:
                        if (isManager)
                        {
                            where = (s =>
                             s.TrayId == (int)trayType &
                             //s.ToUserId == userId &
                             s.ToEntityId == OrgUnitId &
                             (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed
                             || s.Transaction.StatusId == 3281 || s.Transaction.StatusId == 3279 || s.Transaction.StatusId == 3273 || s.Transaction.StatusId == 3272) &
                             s.Transaction.TransactionCategoryId != ExternalOutbound
                             );
                        }
                        else
                        {
                            where = (s =>
                             s.TrayId == (int)trayType &
                             //s.ToUserId == userId &
                             s.ToEntityId == OrgUnitId &
                             (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed) &
                             s.Transaction.TransactionCategoryId != ExternalOutbound
                             );
                        }
                        break;
                    case TrayType.OrgUnit:
                        where = (s =>
                                   s.ToUser == null &
                                   s.TrayId == (int)trayType &
                                   s.ToEntityId == OrgUnitId &
                                   (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership) &
                                   s.Transaction.TransactionCategoryId != ExternalOutbound & !s.Transaction.NeedAcknowled
                                   );
                        break;


                    case TrayType.Manager:
                        where = (s =>
                                  (s.TrayId == (int)TrayType.MyTransactions) &
                                   s.ToEntityId == OrgUnitId &
                                   s.ToUserId != userId &
                                  (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership)
                                   );
                        break;

                    case TrayType.Copies:
                        where = (s =>
                                  (s.ToUserId == userId | s.ToUserId == null) &
                                  s.TrayId == (int)trayType &
                                  s.ToEntityId == OrgUnitId &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                 s.Transaction.TransactionCategoryId == Inbound
                                  );
                        break;
                    case TrayType.InternalInboundCopies:
                        where = (s =>
                                  (s.ToUserId == userId | s.ToUserId == null) &
                                  s.TrayId == (int)trayType &
                                  s.ToEntityId == OrgUnitId &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                  s.Transaction.TransactionCategoryId == InternalOutbound
                                  );
                        break;
                    case TrayType.CopiesOutbound:
                        where = (s =>
                                 (s.ToUserId == userId | s.ToUserId == null) &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                  s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound |
                                 s.Transaction.TransactionCategoryId == DraftOutbound)
                        );
                        break;
                    case TrayType.SpecialCopies:
                        where = (s =>
                                    (s.ToUserId == userId | s.ToUserId == null) &
                                    s.TrayId == (int)trayType &
                                    s.ToEntityId == OrgUnitId &
                                    s.Action.Type.Id == SendCopyToView &
                                    s.Viewed != true
                                    );
                        break;
                    case TrayType.SavedCopies:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetSavedCopiesIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.YESSER:
                        where = (s =>
                                s.ToUserId == userId
                        );
                        break;

                    case TrayType.OutboundExternal:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id) &&
                                     s.Transaction.StatusId != Reserved &&
                                     (s.Transaction.TransactionCategoryId == ExternalOutbound &
                                        (s.DeliveryMethodId == electronicDelivery || (s.DeliveryMethodId == electronicPaperDelivery))) &
                                     !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft


                           );
                        }
                        break;
                    case TrayType.ElcOutBound:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id) &
                                    !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft &
                                    s.Transaction.TransactionCategoryId == InternalOutbound);
                        }
                        break;
                    case TrayType.FollowUp:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowUpTransactionsIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpUnderProcess:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowProcessIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpComplete:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowCompleteIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpLate:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowLateIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpCanceld:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowDeleteIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpReminder:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowReminderIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpEscalation:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowUpEscalationIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.Reservation:
                        where = (s =>
                                 s.ToUserId == userId &
                                 //s.TrayId == (int)trayType &
                                 s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == Inbound) &
                                 (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership));
                        break;
                    case TrayType.ReservedExternalOutbound:
                        where = (s =>
                                 s.ToUserId == userId &
                                 //s.TrayId == (int)trayType &
                                 s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound) &
                                 (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership) &
                                 !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft);
                        break;
                }

                switch (transactionDate)
                {
                    case TransactionDateType.Today:
                        {
                            if (trayType == TrayType.SentTransactions)
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                                     ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                                     ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                                     ts.ModefiedOn.Value.Month == DateTime.Now.Month);
                            }
                            else
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                               ts.Date.Year == DateTime.Now.Year &
                               ts.Date.Day == DateTime.Now.Day &
                               ts.Date.Month == DateTime.Now.Month);
                            }

                        }
                        break;
                    case TransactionDateType.Late:
                        {

                            IUserManagementBL userManagementBL = new UserManagementBL();

                            UserProfile userProfile = userManagementBL.GetUserById(userId);

                            DateTime date = DateTime.Now.Date;

                            if (userProfile.TransactionProcessingPeriod > 0)
                            {
                                date = DateTime.Now.AddDays(-userProfile.TransactionProcessingPeriod);
                            }
                            where = ExpressionUtility.AndAlso(where, ts =>
                            ts.Transaction.RemindDate < date ||
                          (ts.TransactionAssignmentProcessPeriod == null && (
                            ts.Date < date)) ||
                            (ts.TransactionAssignmentProcessPeriod != null && (
                            ts.TransactionAssignmentProcessPeriod < date))

                           );
                            // where = ExpressionUtility.AndAlso(where, ts =>
                            // ts.TransactionAssignmentProcessPeriod == null && ( ts.Transaction.RemindDate < DateTime.Now |
                            // ts.Date < date)
                            //);
                        }
                        break;
                    case TransactionDateType.HasDate:
                        {
                            where = ExpressionUtility.AndAlso(where, ts =>
                         ts.Transaction.RemindDate != null
                         );
                        }
                        break;
                    case TransactionDateType.Decisions:
                        {
                            where = ExpressionUtility.AndAlso(where, ts =>
                         ts.Transaction.TransactionTypeId == 4

                         );
                        }
                        break;

                    case TransactionDateType.Circulars:
                        {
                            where = ExpressionUtility.AndAlso(where, ts =>
                         ts.Transaction.LetterTypeId == 3

                         );
                        }
                        break;
                    case TransactionDateType.SublimeMatter:
                        {
                            where = ExpressionUtility.AndAlso(where, ts =>
                         ts.Transaction.LetterTypeId == 58

                         );
                        }
                        break;
                }

                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetUserTransactionsTray(where, userWeight, searchCriteria, userId, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public static IList<Transaction> GetTransactionByUsername(BaseSearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {

                rowsCount = 0;


                Expression<Func<TransactionAssignment, bool>> where = null;
                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);

                where = (s =>
                   s.ToUserId == searchCriteria.UserId &
                   s.TrayId == (int)TrayType.MyTransactions &
                   (s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected | s.Transaction.Status.Id == MultiOwnership)
                   && !s.Viewed
                   );

                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetTransactionByUsername(where, searchCriteria, out rowsCount);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public static Transaction GetNextTransactionsTray(int userId, int OrgUnitId, TrayType trayType, SearchCriteriaCustom searchCriteria)
        {
            try
            {
                bool isManager = false;
                if (trayType == TrayType.Saved)
                {
                    IOrgUnitBL orgUnitBL = new OrgUnitBL();
                    OrgUnit orgUnit = orgUnitBL.GetOrgUnitById(OrgUnitId);
                    if (orgUnit.ManagerId == userId)
                    {
                        isManager = true;
                    }
                }

                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                searchCriteria.UserId = userId;

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }

                Expression<Func<TransactionAssignment, bool>> where = null;

                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int deletedStatus = TransactionStatus.Deleted.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int SendCopyToView = (int)ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Completed = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int electronicDelivery = DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);
                int electronicPaperDelivery = DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, string.Empty);


                switch (trayType)
                {
                    case TrayType.MyTransactions:
                        where = (s =>
                     s.ToUserId == userId &
                     s.TrayId == (int)trayType &
                     s.ToEntityId == OrgUnitId &
                     (s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected | s.Transaction.Status.Id == MultiOwnership) &
                     s.Transaction.TransactionCategoryId != ExternalOutbound
                     );
                        break;
                    case TrayType.DraftOutbound:
                        where = (s =>
                     s.ToUserId == userId &
                     s.ToEntityId == OrgUnitId &&
                     s.Transaction.StatusId != Reserved &&
                     (((s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected) & s.Transaction.TransactionCategoryId == DraftOutbound & s.TrayId == (int)trayType)
                     | (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft))
                        /*| (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & s.Transaction.DeliveryMethodId == electronicPaperDelivery
                           & !s.Transaction.PrintedDeliveryReport & s.Transaction.StatusId != Outbound & s.Transaction.StatusId != Sent & s.Transaction.StatusId != Reserved)*/
                        )
                     );
                        break;
                    case TrayType.DeletedDraftOutbound:
                        where = (s =>
                            s.ToUserId == userId &
                            s.ToEntityId == OrgUnitId &
                            (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft) & s.Transaction.StatusId == deletedStatus);
                        break;
                    case TrayType.SentTransactions:
                        where = (s =>
                                 s.FromUserId == userId &
                                 s.FromEntityId == OrgUnitId &
                                 s.TrayId == (int)trayType &
                                (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership) &
                                 s.Transaction.TransactionCategoryId != ExternalOutbound
                                 );
                        break;
                    case TrayType.Saved:
                        if (isManager)
                        {
                            where = (s =>
                             s.TrayId == (int)trayType &
                             //s.ToUserId == userId &
                             s.ToEntityId == OrgUnitId &
                             (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed
                             || s.Transaction.StatusId == 3281 || s.Transaction.StatusId == 3279 || s.Transaction.StatusId == 3273 || s.Transaction.StatusId == 3272) &
                             s.Transaction.TransactionCategoryId != ExternalOutbound
                             );
                        }
                        else
                        {
                            where = (s =>
                             s.TrayId == (int)trayType &
                             //s.ToUserId == userId &
                             s.ToEntityId == OrgUnitId &
                             (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed) &
                             s.Transaction.TransactionCategoryId != ExternalOutbound
                             );
                        }
                        break;
                    case TrayType.OrgUnit:
                        where = (s =>
                                   s.ToUser == null &
                                   s.TrayId == (int)trayType &
                                   s.ToEntityId == OrgUnitId &
                                   (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership) &
                                   s.Transaction.TransactionCategoryId != ExternalOutbound & !s.Transaction.NeedAcknowled
                                   );
                        break;


                    case TrayType.Manager:
                        where = (s =>
                                  (s.TrayId == (int)TrayType.MyTransactions) &
                                   s.ToEntityId == OrgUnitId &
                                   s.ToUserId != userId &
                                  (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership)
                                   );
                        break;

                    case TrayType.Copies:
                        where = (s =>
                                  (s.ToUserId == userId | s.ToUserId == null) &
                                  s.TrayId == (int)trayType &
                                  s.ToEntityId == OrgUnitId &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                 s.Transaction.TransactionCategoryId == Inbound
                                  );
                        break;
                    case TrayType.InternalInboundCopies:
                        where = (s =>
                                  (s.ToUserId == userId | s.ToUserId == null) &
                                  s.TrayId == (int)trayType &
                                  s.ToEntityId == OrgUnitId &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                  s.Transaction.TransactionCategoryId == InternalOutbound
                                  );
                        break;
                    case TrayType.SavedCopies:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetSavedCopiesIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.YESSER:
                        where = (s =>
                                s.ToUserId == userId
                        );
                        break;

                    case TrayType.OutboundExternal:
                        where = (s =>
                                 s.FromEntityId == OrgUnitId &&
                                 //s.FromUserId == userId &
                                 s.Transaction.StatusId != Reserved &&
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound &
                                    (s.DeliveryMethodId == electronicDelivery || (s.DeliveryMethodId == electronicPaperDelivery))) &
                                 !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft
                       );
                        break;
                    case TrayType.ElcOutBound:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id) &
                                    !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft &
                                    s.Transaction.TransactionCategoryId == InternalOutbound);
                        }
                        break;
                    case TrayType.CopiesOutbound:
                        where = (s =>
                                 (s.ToUserId == userId | s.ToUserId == null) &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                  s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound |
                                 s.Transaction.TransactionCategoryId == DraftOutbound)
                        );
                        break;

                    case TrayType.FollowUp:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowUpTransactionsIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpUnderProcess:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowProcessIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpComplete:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowCompleteIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpLate:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowLateIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpCanceld:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowDeleteIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpReminder:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowReminderIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpEscalation:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowUpEscalationIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.Reservation:
                        where = (s =>
                                 s.ToUserId == userId &
                                 //s.TrayId == (int)trayType &
                                 s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == Inbound) &
                                 (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership));
                        break;
                    case TrayType.ReservedExternalOutbound:
                        where = (s =>
                                 s.ToUserId == userId &
                                 //s.TrayId == (int)trayType &
                                 s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound) &
                                 (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership) &
                                 !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft);
                        break;
                }


                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetNextTransactionsTray(where, searchCriteria);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public static IList<Transaction> GetWithdrawalTransactions(int? transId, int? orgunitId, int? transactionTypeId, int? year, SearchCriteriaCustom searchCriteria, int userId, out int rowsCount)
        {
            try
            {
                rowsCount = 0;
                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                //searchCriteria.UserId = userId;

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }

                Expression<Func<TransactionAssignment, bool>> where = null;

                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Saved = TransactionStatus.Saved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int SendCopyToView = (int)ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Completed = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);


                where = (s =>
                     (s.Transaction.Status.Id != Saved));

                if (transId.HasValue)
                {
                    where = ExpressionUtility.AndAlso(where, s =>
                       (s.Transaction.Number == transId)
                       );
                }

                if (transactionTypeId.HasValue && transactionTypeId != -1)
                {
                    where = ExpressionUtility.AndAlso(where, s =>
                      s.Transaction.TransactionCategoryId == transactionTypeId
                      );
                }
                if (year.HasValue)
                {
                    where = ExpressionUtility.AndAlso(where, s =>
                      (s.Transaction.YearH == year)
                      );
                }

                if (orgunitId.HasValue)
                {
                    where = ExpressionUtility.AndAlso(where, s =>
                        (s.ToEntityId == orgunitId)
                    );
                }

                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();
                return transactionAssignmentRepository.GetUserTransactionsTray(where, userWeight, searchCriteria, userId, out rowsCount);
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


        public static void UpdateStatus(long transactionNumber, int statusId, string rejectionReason = null)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionStatusByTransNo(transactionNumber, statusId, rejectionReason);
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

        public static void UpdateStatusById(long transactionId, int statusId, string rejectionReason = null)
        {
            try
            {
                //ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                //transactionRepository.UpdateTransactionStatusByTransId(transactionId, statusId, rejectionReason);
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

        protected virtual void PreGetVisitTicket(Transaction transaction)
        {

        }

        protected virtual TransactionVisitTicketInfo OnGetVisitTicket(Transaction transaction, int OrgUnitId, string cultureName)
        {
            try
            {
                int value = BarcodeReferenceType.MainTransaction.LookupIdentity(LookupCategory.BarcodeReferenceType, cultureName);
                IBarcodeBL barcodeBL = new BarcodeBL();
                ILookupBL lookupBL = new LookupBL();
                ISettingBL settingBL = new SettingBL();
                IOrgUnitBL OrgUnitBL = new OrgUnitBL();
                BarcodeDesign visitTicketDesign = barcodeBL.GetBarcodeDesign(BarcodeDesignType.VisitTicket, OrgUnitId);

                if (visitTicketDesign == null)
                {
                    throw new BusinessException(StatusCode.VisitTicketNotAvailable);
                }

                TransactionVisitTicketInfo transactionVisitTicket = new TransactionVisitTicketInfo();

                transactionVisitTicket.TicketDesignHeight = visitTicketDesign.Height;
                transactionVisitTicket.TicketDesignWidth = visitTicketDesign.Width;
                transactionVisitTicket.VisitTicketHtmlDesign = visitTicketDesign.Html;
                transactionVisitTicket.TransactionDate = transaction.Date;
                transactionVisitTicket.TransactionDateH = transaction.DateH;
                transactionVisitTicket.TransactionNumber = transaction.Number;
                transactionVisitTicket.Entity = transaction.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionVisitTicket.Date = transaction.Date;
                transactionVisitTicket.DateH = transaction.DateH;
                transactionVisitTicket.CompanyName = OrgUnitBL.GetOrgUnitName(o => o.Parent == null, cultureName);
                transactionVisitTicket.InboundNumber = transaction.DocumentNumber;
                transactionVisitTicket.InboundDestination = transaction.ExternalParty != null ? transaction.ExternalParty.Name.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : string.Empty;
                transactionVisitTicket.ToEntityName = transaction.Assignments[0].ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text;
                transactionVisitTicket.Subject = transaction.Subject;

                Barcode barcode = new Barcode
                {
                    ReferenceType = lookupBL.GetLookupItem(value),
                    Value = barcodeBL.GetBarcode(b => b.ReferenceId == transaction.Id & b.ReferenceType.Id == value).FirstOrDefault().Value
                };
                transactionVisitTicket.barcode = barcode;

                return transactionVisitTicket;
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

        protected virtual void PostGetVisitTicket(Transaction transaction)
        {

        }

        private void CheckTransactionConfidentiality(string confidentialityCode)
        {
            List<UserClaim> userTransactionClaims =
                User.Claims.Where(c => c.Name.StartsWith(UserClaims.ConfidentialityOfTransactions.Prefix)).ToList();

            UserClaim userClaim = userTransactionClaims.Where(c => c.Name == confidentialityCode).FirstOrDefault();

            if (userClaim == null)
            {
                throw new BusinessException(StatusCode.TransactionConfidentialityRequired);
            }
        }

        protected void CheckTransactionSourceTypePermission(Transaction transaction)
        {
            ITransactionTypeBL transactionTypeBL = IoC.Resolve<ITransactionTypeBL>();

            Domain.TransactionType transactionType = transactionTypeBL.GetTransactionSourceTypeById(transaction.TransactionTypeId.Value);

            if (!User.HasClaim(transactionType.Permission.Code))
            {
                throw new BusinessException(StatusCode.TransactionSourceTypeConfidentialityRequired);
            }
        }

        //private TransactionInfo LogTransactionIndexing(Transaction transaction,
        //    string barcode, bool isAddOperation)
        //{
        //    //map the transaction info to be indexed into solr
        //    TransactionInfo transactionInfo = SolrTransactionInfoMapper.Map(transaction, barcode);

        //    ITransactionIndexLogBL transactionIndexBL = new TransactionIndexLogBL();

        //    //prepare the TransactionIndex model to be saved in the database, in order to check 
        //    //if the transaction is indexed successfully or not using a custom service
        //    TransactionIndexLog transactionIndexLog = SolrTransactionInfoMapper.Map(transactionInfo);

        //    //add the transaction index to the database
        //    if (isAddOperation)
        //    {
        //        transactionIndexBL.AddIndex(transactionIndexLog);
        //    }
        //    else
        //    {
        //        transactionIndexBL.UpdateIndex(transactionIndexLog);
        //    }

        //    return transactionInfo;
        //}

        //private void DoTransactionIndex(int transactionId)
        //{
        //    //check if Solr indexing is enabled, if yes, then, do index the transation
        //    if (SystemConfigurations.IsSolrIndexingEnabled)
        //    {
        //        //get the transactionIndexLog
        //        ITransactionIndexLogBL transactionIndexLogBL = new TransactionIndexLogBL();

        //        TransactionIndexLog transactionIndexLog =
        //            transactionIndexLogBL.GetIndexedTransactions(t => t.TransId == transactionId).FirstOrDefault();

        //        if (transactionIndexLog != null)
        //        {
        //            TransactionInfo transactionInfo = SolrTransactionInfoMapper.Map(transactionIndexLog);

        //            //index the transaction into solr async
        //            SolrIndexer.AddOrUpdateAsync(transactionInfo);
        //        }
        //    }
        //}

        public void SaveTransactionDeliveryNumber(Transaction transaction)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.SaveTransactionDeliveryNumber(transaction);
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

        public void UpdateTransactionStatus(int transId, int statusId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionStatus(transId, statusId);
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
        public void UpdateTransactionSubject(EditSubjectTransactionDTO editSubjectTransactionDTO)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionSubject(editSubjectTransactionDTO.Id, editSubjectTransactionDTO.Subject);
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
        public void UpdateTransactionDelivary(int transId, int DelivaryId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionDelivary(transId, DelivaryId);
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





        public bool IsMatchNumberOrBarcode(int transId, string number, string barcode, int userId, int entityId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                var result = transactionRepository.IsMatchNumberOrBarcode(transId, number, barcode);
                transactionRepository.UpdatePhysicalTransactionAssignment(transId, userId, entityId);
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

        public static Transaction GetByTransactionNumber(int transactionNumber, int year, int transactionCategoryId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransaction(t => t.Number == transactionNumber & t.TransactionCategoryId == transactionCategoryId & t.YearH == year);
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
        public static Transaction GetTransactionBasicInfoById(int transactionId, int year, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                return transactionRepository.GetTransactionBasicInfo(transactionId, year, cultureName);
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

        public void UpdateTransactionExternalCopyStatus(int transactionId, int value, int status)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionExternalCopyStatus(transactionId, value, status);
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

        public void UpdateTransactionExternalCopyStatusById(long transactionNumber, int transactionsCopyId, int unableToDeliver)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.UpdateTransactionExternalCopyStatusById(transactionNumber, transactionsCopyId, unableToDeliver);
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

        #region FollowUp
        public static void FollowUpDetailsAdd(int transactionId, int orgUnitId, int userId, string note)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpDetailsAdd(transactionId, orgUnitId, userId, note);
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

        public static void AddFollowupUditTrial(FollowUpAuditTrail followUpAuditTrail)
        {
            try
            {
                followUpAuditTrail.CreatedOn = DateTime.Now;
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.AddFollowupUditTrial(followUpAuditTrail);
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

        public void FollowUpUpdateIsDeleted(int transactionId, int userId, string culture)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpUpdateIsDeleted(transactionId, userId);

                var transaction = GetTransactionById(transactionId, culture);

                var notificationUsers = new List<NotificationUser>
                {
                    new NotificationUser { UserId = transaction.UserId }
                };

                IUserManagementBL userManagementBL = new UserManagementBL();
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(userId);

                if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
                {
                    SendFollowUpNotification(transactionId, NotificationSource.CancelFollowup, NotificationTemplateType.CancelFollowupWeb,
                        NotificationTemplateType.CancelFollowupEmail, NotificationEmailSubject.CancelFollowupEmail, NotificationWebSubject.CancelFollowup,
                        notificationUsers, culture);
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

        public void FollowUpUpdateIsDeleted(int id, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                var followUp = transactionRepository.GetFollowUpById(id);

                transactionRepository.FollowUpUpdateIsDeleted(id);

                if (!followUp.FollowUpUserId.HasValue)
                {
                    return;
                }

                var notificationUsers = new List<NotificationUser>
                {
                    new NotificationUser { UserId = followUp.FollowUpUserId.Value}
                };
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(followUp.FollowUpUserId.Value);

                if (notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
                {
                    SendFollowUpNotification(followUp.TransactionId, NotificationSource.CancelFollowup, NotificationTemplateType.CancelFollowupWeb,
                        NotificationTemplateType.CancelFollowupEmail, NotificationEmailSubject.CancelFollowupEmail, NotificationWebSubject.CancelFollowup,
                        notificationUsers, cultureName);
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
        public void FollowUpChangeStatus(int Id, int FollowupStatus, bool IsActive)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpChangeStatus(Id, FollowupStatus, IsActive);
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
        public void FollowUpUpdateReceive(int Id, int userid)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpUpdateReceive(Id, userid);
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
        public void FollowUpUpdateReminderStatus(int Id, bool IsReminder)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpUpdateReminderStatus(Id, IsReminder);
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

        public void FollowUpUpdateEscalatedStatus(int Id, bool IsEscalated)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.FollowUpUpdateEscalatedStatus(Id, IsEscalated);
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

        public int TransactionFollowUpAdd(TransactionFollowUp oTransactionFollowUp, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                IUserManagementBL userManagementBL = new UserManagementBL();

                int followupId = transactionRepository.TransactionFollowUpAdd(oTransactionFollowUp);

                if (!oTransactionFollowUp.FollowUpUserId.HasValue)
                {
                    return followupId;
                }

                var notificationUsers = new List<NotificationUser>
                {
                    new NotificationUser { UserId = oTransactionFollowUp.FollowUpUserId.Value }
                };
                NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(oTransactionFollowUp.FollowUpUserId.Value);


                if (oTransactionFollowUp != null && notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
                {
                    SendFollowUpNotification(oTransactionFollowUp.TransactionId, NotificationSource.Followup, NotificationTemplateType.FollowupWeb,
                        NotificationTemplateType.FollowupEmail, NotificationEmailSubject.FollowupEmail, NotificationWebSubject.Followup,
                        notificationUsers, cultureName);
                }
                return followupId;
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
        public void TransactionFollowUpUpdate(TransactionFollowUp oTransactionFollowUp, string cultureName)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                IUserManagementBL userManagementBL = new UserManagementBL();
                transactionRepository.TransactionFollowUpUpdate(oTransactionFollowUp);

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

        public void SendFollowUpReminder(int FollowUpId, int TransactionId, int FollowUpUserID, string cultureName)
        {

            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            IUserManagementBL userManagementBL = new UserManagementBL();
            transactionRepository.ReminderTransactionFollowUp(FollowUpId);

            var notificationUsers = new List<NotificationUser>
                {
                    new NotificationUser { UserId = FollowUpUserID }
                };
            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(FollowUpUserID);

            if (TransactionId > 0 && notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
            {
                SendFollowUpNotification(TransactionId, NotificationSource.Followup, NotificationTemplateType.FollowupWeb,
                    NotificationTemplateType.FollowupEmail, NotificationEmailSubject.FollowupEmail, NotificationWebSubject.Followup,
                    notificationUsers, cultureName);
            }


        }
        public void EscalateFollowUp(int FollowUpId, int TransactionId, int FollowUpUserID, string cultureName)
        {

            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            IUserManagementBL userManagementBL = new UserManagementBL();
            transactionRepository.EscalateTransactionFollowUp(FollowUpId);

            var notificationUsers = new List<NotificationUser>
                {
                    new NotificationUser { UserId = FollowUpUserID }
                };
            NotificationSubscriptions notificationSubscriptions = userManagementBL.GetUserNotificationSubscriptions(FollowUpUserID);

            if (TransactionId > 0 && notificationSubscriptions.HasFlag(NotificationSubscriptions.Followup))
            {
                SendFollowUpNotification(TransactionId, NotificationSource.Followup, NotificationTemplateType.FollowupWeb,
                    NotificationTemplateType.FollowupEmail, NotificationEmailSubject.FollowupEmail, NotificationWebSubject.Followup,
                    notificationUsers, cultureName);
            }


        }
        public int? GetChildFollowUpUserId(int FollowUpId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                return transactionRepository.GetChildFollowUpUserId(FollowUpId);
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
        public bool CheckIfFollowUpAdd(int TransactionId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                return transactionRepository.CheckIfFollowUpAdd(TransactionId);
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

        public static TransactionFollowUp FollowUpDetailsByTransId(int transId, int FollowUpStatusId, int UserId, int OrgUnitId, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.FollowUpDetailsByTransId(transId, FollowUpStatusId, UserId, OrgUnitId, cultureName);
        }
        public static TransactionFollowUp FollowUpDetailsByFollowUpId(int FollowUpId, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.FollowUpDetailsByFollowUpId(FollowUpId, cultureName);
        }
        public static IList<FollowUpDetails> FollowUpDetailsById(int id, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.FollowUpDetailsById(id, cultureName);
        }
        public static IList<FollowUpAuditTrail> GetListFollowupUditTrial(int id, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.GetListFollowupUditTrial(id, cultureName);
        }
        public static IList<TransactionFollowUp> TransactionFollowUpSelectByTransId(int transId, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.TransactionFollowUpSelectByTransId(transId, cultureName);
        }
        public static IList<TransactionFollowUp> TransactionFollowUpSelectByFollowUpId(int transId, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.TransactionFollowUpSelectByFollowUpId(transId, cultureName);
        }
        public static IList<FollowUpAuditTrail> GetFollowUpAuditTrail(int followUpId, string cultureName)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
            return transactionRepository.GetFollowUpAuditTrail(followUpId, cultureName);
        }
        #endregion

        public void SaveTransactionReservation(TransactionReservation transactionReservation)
        {
            try
            {
                string[] transDefaults = SystemConfigurations.ReservationInboundDefault.Split(',');

                ISystemDefaultValuesBL systemDefaultValuesBL = IoC.Resolve<ISystemDefaultValuesBL>();
                IList<SystemDefaultValues> systemDefaultValues = systemDefaultValuesBL.GetSystemDefaultValue().ToList();

                var defaultSourceType = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Inbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.TransactionSourceType);
                var defaultDeliveryMethodId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Inbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.BasicDeliveryMethod);
                var defaultPriorityId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Inbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.PriorityLevel);
                var defaultLetterTypeId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Inbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.InboundDocumentType);
                var defaultConfidentialityId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Inbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.Confedentiality);

                int sourceTypeId = defaultSourceType != null && defaultSourceType.DefaultValueId.HasValue ? defaultSourceType.DefaultValueId.Value : Convert.ToInt32(transDefaults[0]);
                int deliveryMethodId = defaultDeliveryMethodId != null && defaultDeliveryMethodId.DefaultValueId.HasValue ? defaultDeliveryMethodId.DefaultValueId.Value : Convert.ToInt32(transDefaults[1]);
                int priorityId = defaultPriorityId != null && defaultPriorityId.DefaultValueId.HasValue ? defaultPriorityId.DefaultValueId.Value : Convert.ToInt32(transDefaults[2]);
                int letterTypeId = defaultLetterTypeId != null && defaultLetterTypeId.DefaultValueId.HasValue ? defaultLetterTypeId.DefaultValueId.Value : Convert.ToInt32(transDefaults[3]);
                int confidentialityId = defaultConfidentialityId != null && defaultConfidentialityId.DefaultValueId.HasValue ? defaultConfidentialityId.DefaultValueId.Value : Convert.ToInt32(transDefaults[4]);

                if (transactionReservation.TransactionCategoryId == Common.TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                {
                    var outboundDefaultSourceType = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Outbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.TransactionSourceType);
                    var outboundDefaultDeliveryMethodId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Outbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.BasicDeliveryMethod);
                    var outboundDefaultPriorityId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Outbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.PriorityLevel);
                    var outboundDefaultLetterTypeId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Outbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.InboundDocumentType);
                    var outboundDefaultConfidentialityId = systemDefaultValues.FirstOrDefault(p => (p.CategoryId & (int)TransactionCategories.Outbound) != 0 && p.TypeId == (int)DefaultCategoryTypes.Confedentiality);

                    transDefaults = SystemConfigurations.ReservationOutboundDefault.Split(',');

                    deliveryMethodId = outboundDefaultDeliveryMethodId != null && outboundDefaultDeliveryMethodId.DefaultValueId.HasValue ? outboundDefaultDeliveryMethodId.DefaultValueId.Value : Convert.ToInt32(transDefaults[1]);
                    sourceTypeId = outboundDefaultSourceType != null && outboundDefaultSourceType.DefaultValueId.HasValue ? outboundDefaultSourceType.DefaultValueId.Value : Convert.ToInt32(transDefaults[0]);
                    priorityId = defaultPriorityId != null && defaultPriorityId.DefaultValueId.HasValue ? defaultPriorityId.DefaultValueId.Value : Convert.ToInt32(transDefaults[2]);
                    letterTypeId = defaultLetterTypeId != null && defaultLetterTypeId.DefaultValueId.HasValue ? defaultLetterTypeId.DefaultValueId.Value : Convert.ToInt32(transDefaults[3]);
                    confidentialityId = defaultConfidentialityId != null && defaultConfidentialityId.DefaultValueId.HasValue ? defaultConfidentialityId.DefaultValueId.Value : Convert.ToInt32(transDefaults[4]);
                }

                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                int reservationId = transactionRepository.AddTransactionReservation(transactionReservation);

                for (int i = 0; i < transactionReservation.Count; i++)
                {
                    Transaction tempTransaction = new Transaction()
                    {
                        TransactionCategoryId = transactionReservation.TransactionCategoryId,
                        Subject = transactionReservation.Reason,
                        PriorityId = priorityId,
                        LetterTypeId = letterTypeId,
                        ConfidentialityId = confidentialityId,
                        DeliveryMethodId = deliveryMethodId,
                        ToUserId = transactionReservation.UserId,
                        EntityId = transactionReservation.EntityId,
                        UserId = transactionReservation.UserId,
                        TransactionTypeId = sourceTypeId,
                        OrgUnitId = transactionReservation.EntityId,
                        ReservationId = reservationId,
                        Links = new List<TransactionLink>(),
                        Attachments = new List<Attachment>(),
                        Copies = new List<TransactionCopy>(),
                        LetterNumber = transactionReservation.LetterNumber
                    };
                    PreSave(tempTransaction);
                    var transactionDetails = OnSave(tempTransaction, true);
                    PostSave(tempTransaction);
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

        public static List<TransactionReservation> GetTransactionReservations(int? orgUnitId, int? userId, SearchCriteria searchCriteria, out int rowsCount)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionReservations(orgUnitId, userId, searchCriteria, out rowsCount);
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

        public static List<Transaction> GetReservedTransaction(int reservationId)
        {
            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetReservedTransaction(reservationId);
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
        public void SendFollowUpNotification(int transactionId, NotificationSource notificationSource, NotificationTemplateType notificationTemplateType,
            NotificationTemplateType notificationEmailTemplateType, NotificationEmailSubject notificationEmailSubject, NotificationWebSubject notificationWebSubject,
            IList<NotificationUser> notificationUsers, string cultureName)
        {
            if (SystemConfigurations.IsNotificationEnabled)
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                var transaction = GetTransactionById(transactionId, cultureName);

                keyValues["{Number}"] = transaction.Number.ToString();
                keyValues["{TransactionNumber}"] = transaction.Number.ToString();
                keyValues["{TransactionTypeId}"] = transaction.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text;
                keyValues["{PriorityId}"] = transaction.Priority.Text;
                keyValues["{ConfidentialityId}"] = transaction.Confidentiality.LocalName;
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

        #region MobileApi

        public static List<Transaction> UserMobileGetUserTransactionsTray(int userId, int OrgUnitId, TrayType trayType, TransactionDateType transactionDate, YESSERMobileDomain.FilterCriteria filterCriteria, string cultureName, bool isAscending = false)
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

                Expression<Func<TransactionAssignment, bool>> where = null;
                int InProcess = TransactionStatus.InProcess.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int ExternalOutbound = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Sent = TransactionStatus.Sent.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Outbound = TransactionStatus.Outbound.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Rejected = TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int MultiOwnership = TransactionStatus.MultiOwnership.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int DraftOutbound = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Reserved = TransactionStatus.Reserved.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int TempSave = TransactionStatus.TempSave.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int SendCopyToView = ActionType.SendCopyToView.LookupIdentity(LookupCategory.ActionType, string.Empty);
                int Inbound = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int InternalOutbound = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty);
                int Completed = TransactionStatus.Completed.LookupIdentity(LookupCategory.TransactionStatus, string.Empty);
                int Delete = TransCopyStatus.Delete.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);
                int Viewed = TransCopyStatus.Viewed.LookupIdentity(LookupCategory.TransCopyStatus, string.Empty);

                switch (trayType)
                {
                    case TrayType.MyTransactions:
                        where = (s =>
                     s.ToUserId == userId &
                     s.TrayId == (int)trayType &
                     s.ToEntityId == OrgUnitId &
                     (s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected | s.Transaction.Status.Id == MultiOwnership) &
                     s.Transaction.TransactionCategoryId != ExternalOutbound
                     );
                        break;
                    case TrayType.DraftOutbound:
                        where = (s =>
                     s.ToUserId == userId &
                     s.ToEntityId == OrgUnitId &&
                     s.Transaction.StatusId != Reserved &&
                     (((s.Transaction.Status.Id == InProcess | s.Transaction.StatusId == Rejected) & s.Transaction.TransactionCategoryId == DraftOutbound & s.TrayId == (int)trayType)
                     | (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & (s.Transaction.IsDraft | s.Transaction.IsPresentationDraft))
                        /*| (s.Transaction.TransactionCategoryId == ExternalOutbound & s.TrayId == (int)TrayType.MyTransactions & s.Transaction.DeliveryMethodId == electronicPaperDelivery
                           & !s.Transaction.PrintedDeliveryReport & s.Transaction.StatusId != Outbound & s.Transaction.StatusId != Sent & s.Transaction.StatusId != Reserved)*/
                        )
                     );
                        break;
                    case TrayType.SentTransactions:
                        where = a => (((a.FromUserId != a.ToUserId && a.FromEntityId != a.ToEntityId)) && a.FromEntityId == OrgUnitId && a.FromUserId == userId && (a.TrayId == (int)TrayType.MyTransactions | a.TrayId == (int)TrayType.OrgUnit | a.TrayId == (int)TrayType.DraftOutbound)
                    && a.Transaction.StatusId == InProcess
                    && a.Transaction.TransactionCategoryId != ExternalOutbound
                                 );
                        break;
                    case TrayType.Saved:
                        where = (s =>
                                 s.TrayId == (int)trayType &
                                 s.ToUserId == userId &
                                 s.ToEntityId == OrgUnitId &
                                (s.Transaction.StatusId == TempSave | s.Transaction.StatusId == Completed) &
                                 s.Transaction.TransactionCategoryId != ExternalOutbound
                                 );
                        break;
                    case TrayType.OrgUnit:
                        where = (s =>
                                   s.ToUser == null &
                                   s.TrayId == (int)trayType &
                                   s.ToEntityId == OrgUnitId &
                                   (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership) &
                                   s.Transaction.TransactionCategoryId != ExternalOutbound
                                   & !s.Transaction.NeedAcknowled);
                        break;
                    case TrayType.Manager:
                        where = (s =>
                                  (s.TrayId == (int)TrayType.MyTransactions) &
                                   s.ToEntityId == OrgUnitId &
                                   s.ToUserId != userId &
                                  (s.Transaction.Status.Id == InProcess | s.Transaction.Status.Id == MultiOwnership)
                                   );
                        break;
                    case TrayType.ElcOutBound:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetELcOutBoundIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id) &
                                    !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft &
                                    s.Transaction.TransactionCategoryId == InternalOutbound);
                        }
                        break;
                    case TrayType.Copies:
                        where = (s =>
                        s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                            tc.UserId == userId &
                            tc.IsSent == 1 & tc.Status != Delete &
                            !tc.Transaction.IsDeleted & tc.Status != Viewed).Any());
                        break;
                    case TrayType.InternalInboundCopies:
                        where = (s =>
                     s.Transaction.Copies.Where(tc => tc.EntityId == OrgUnitId &
                         (tc.UserId == userId | tc.UserId == null) &
                         tc.IsSent == 1 & tc.Status != Delete &
                         !tc.Transaction.IsDeleted & tc.Status != Viewed).Any());
                        break;
                    case TrayType.CopiesOutbound:
                        where = (s =>
                                 (s.ToUserId == userId | s.ToUserId == null) &
                                  s.Action.Type.Id == SendCopyToView &
                                  s.Viewed != true &
                                  s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == DraftOutbound)
                        );
                        break;

                    case TrayType.YESSER:
                        where = (s =>
                                s.ToUserId == userId
                        );
                        break;
                    case TrayType.OutboundExternal:
                        where = (s =>
                                 s.FromEntityId == OrgUnitId &&
                                 s.FromUserId == userId &
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound & (s.Transaction.StatusId == Sent ||
                                 s.Transaction.StatusId == Outbound)) &
                                 !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft
                       );
                        break;
                    //   case TrayType.OutboundExternal:
                    //    where = (s =>
                    //             s.FromEntityId == OrgUnitId &&
                    //             s.FromUserId == userId &
                    //             (s.Transaction.TransactionCategoryId == ExternalOutbound & (s.Transaction.StatusId == Sent ||
                    //             s.Transaction.StatusId == Outbound)) &
                    //             !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft
                    //   );
                    //    break;


                    case TrayType.FollowUp:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowUpTransactionsIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpUnderProcess:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowProcessIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpComplete:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowCompleteIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpLate:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowLateIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpCanceld:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowDeleteIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpReminder:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowReminderIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.FollowUpEscalation:
                        {
                            ITransactionRepository transactionRepository = IoC.Resolve<ITransactionRepository>();
                            IList<int> transactionsIds = transactionRepository.GetUserFollowUpEscalationIds(userId, OrgUnitId);
                            where = (s => transactionsIds.Contains(s.Transaction.Id));
                        }
                        break;
                    case TrayType.Reservation:
                        where = (s =>
                                 s.ToUserId == userId &
                                 //s.TrayId == (int)trayType &
                                 s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == Inbound) &
                                 (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership));
                        break;
                    case TrayType.ReservedExternalOutbound:
                        where = (s =>
                                 s.ToUserId == userId &
                                 //s.TrayId == (int)trayType &
                                 s.ToEntityId == OrgUnitId &
                                 (s.Transaction.TransactionCategoryId == ExternalOutbound) &
                                 (s.Transaction.StatusId == Reserved | s.Transaction.StatusId == MultiOwnership) &
                                 !s.Transaction.IsDraft & !s.Transaction.IsPresentationDraft);
                        break;
                }

                switch (transactionDate)
                {
                    case TransactionDateType.Today:
                        {
                            if (trayType == TrayType.SentTransactions)
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                                     ts.ModefiedOn.Value.Year == DateTime.Now.Year &
                                     ts.ModefiedOn.Value.Day == DateTime.Now.Day &
                                     ts.ModefiedOn.Value.Month == DateTime.Now.Month);
                            }
                            else
                            {
                                where = ExpressionUtility.AndAlso(where, ts =>
                               ts.Date.Year == DateTime.Now.Year &
                               ts.Date.Day == DateTime.Now.Day &
                               ts.Date.Month == DateTime.Now.Month);
                            }

                        }
                        break;
                    case TransactionDateType.Late:
                        {
                            IUserManagementBL userManagementBL = new UserManagementBL();

                            UserProfile userProfile = userManagementBL.GetUserById(userId);

                            DateTime date = DateTime.Now.Date;

                            if (userProfile.TransactionProcessingPeriod > 0)
                            {
                                date = DateTime.Now.AddDays(-userProfile.TransactionProcessingPeriod);
                            }

                            where = ExpressionUtility.AndAlso(where, ts =>
                            ts.Transaction.RemindDate < DateTime.Now |
                            ts.Date < date
                           );
                        }
                        break;
                    case TransactionDateType.HasDate:
                        {
                            where = ExpressionUtility.AndAlso(where, ts =>
                         ts.Transaction.RemindDate != null
                         );
                        }
                        break;
                }

                ITransactionAssignmentRepository transactionAssignmentRepository = IoC.Resolve<TransactionAssignmentRepository>();
                return transactionAssignmentRepository.UserMobileGetUserTransactionsTray(where, userWeight, filterCriteria, cultureName, userId, isAscending);
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

        #endregion

        public void SendExternalPartiyEmail(Transaction transaction, byte[] content)
        {
            string email;
            IExternalPartyBL outboundExternalBL = IoC.Container.Resolve<IExternalPartyBL>();

            if (transaction.ExternalPartyManagerId.HasValue)
            {
                ExternalPartyManager externalPartyManager = outboundExternalBL.GetExternalPartyManagerById(transaction.ExternalPartyManagerId.Value);
                email = externalPartyManager.EmailAddress;
            }
            else if (transaction.ExternalPartyId.HasValue)
            {
                ExternalParty externalParty = outboundExternalBL.GetExternalPartyById(transaction.ExternalPartyId.Value);
                email = externalParty.Email;
            }
            else
            {
                return;
            }

            Transaction transactionForNotification = GetTransactionByIdForNotification(transaction.Id);

            List<NotificationAttachment> attachments = new List<NotificationAttachment> { new NotificationAttachment { Binary = content } };

            IList<NotificationUser> notificationUsers = new List<NotificationUser> { NotificationsManager.BuildNotificationUser(transaction.Assignments.FirstOrDefault().FromUserId) };
            notificationUsers.FirstOrDefault().User.Email = email;

            //IList<NotificationUser> notificationUsers = new List<NotificationUser> { new NotificationUser { User = new UserProfile { Email = email } } };
            SendTransactionNotification(transactionForNotification, NotificationSource.ElectronicCopies, NotificationTemplateType.ElectronicCopiesWeb,
                NotificationTemplateType.ElectronicCopiesEmail, NotificationEmailSubject.ElectronicCopiesEmail, NotificationWebSubject.ElectronicCopies,
                notificationUsers, "ar", attachments, true);
        }



        public static IList<Transaction> GetTransactionsByNumber(string transactionNumber, int Type, out int? userWeigth, int YearH, int? DestinationId, string subject, int userId, int entityId)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();
                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);
                userWeigth = permissions.Max(s => s.Weight);
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                return transactionRepository.GetTransactionsByNumber(transactionNumber, Type, YearH, DestinationId, subject, userId, entityId);
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

        public void UpdateTransactionDeleteByTransId(long transactionId, bool isDeleted)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            transactionRepository.DeleteDraftTransaction(transactionId, isDeleted);
        }

        public void DeleteDraftTransaction(long transactionId, bool isDeleted)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            transactionRepository.DeleteDraftTransaction(transactionId, isDeleted);
        }

        public static List<ReleaseNote> ReleaaseNotesUsersSelect(int userId)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            return transactionRepository.ReleaaseNotesUsersSelect(userId);
        }

        public static void ReleaaseNotesUsersAdd(int userId)
        {
            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            transactionRepository.ReleaaseNotesUsersAdd(userId);
        }

        public void SetViewedTransactionCopy(int transactionCopyId)
        {

            ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
            transactionRepository.SetViewedTransactionCopy(transactionCopyId, User.Id);
        }
        public void DeleteDocument(int documentId)
        {

            try
            {

                IDocumentBL documentBL = new DocumentBL();

                documentBL.DeleteDocument(documentId);
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
        public static void AddTransactionEncryptionCode(TransactionEncryptionCode transactionEncryptionCode)
        {

            try
            {
                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();
                transactionRepository.AddTransactionEncryptionCode(transactionEncryptionCode);
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

        public void UpdateAssignmentSelectedoption(int transactionId, string assignmentList)
        {

            try
            {

                ITransactionRepository transactionRepository = IoC.Resolve<TransactionRepository>();

                transactionRepository.UpdateAssignmentSelectedoption(transactionId, assignmentList);
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
