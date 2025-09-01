using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Localization.SupportClasses;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionHistoryRepository : BaseRepository<TransactionHistory>, ITransactionHistoryRepository
    {
        #region Attributes

        

        #endregion Attributes

        #region Constructors

        public TransactionHistoryRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {
            
        }

        #endregion Constructors

        #region Methods

        public int AddTransactionHistory(TransactionHistory transactionHistory)
        {
            try
            {
                _oMCSDbContext.TransactionHistory.Add(transactionHistory);

                _oMCSDbContext.SaveChanges();

                return transactionHistory.Id;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public IList<TransactionHistory> GetTransactionHistory(int transactionId, string cultureName)
        {
            try
            {
                IList<TransactionHistory> transactionHistories =
                    _oMCSDbContext.TransactionHistory.Where(a => a.Transaction.Id == transactionId).ToList();

                IList<TransactionHistory> histories = transactionHistories.Select(t => new TransactionHistory
                {
                    Subject = t.Subject,
                    RemindDate = t.RemindDate,
                    RemindDateH = t.RemindDateH,

                    ToUser = (t.ToUser != null) ? new UserProfile
                    {
                        Id = t.ToUser.Id,
                        LocalName = t.ToUser.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    User = (t.User != null) ? new UserProfile
                    {
                        Id = t.User.Id,
                        LocalName = t.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null,

                    ToEntity = (t.ToEntity != null) ? new OrgUnit
                    {
                        Id = t.ToEntity.Id,
                        LocalName = t.ToEntity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).LocalText()
                    } : null
                }).ToList();

                return histories;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionHistory GetLastTransactionHistory(int transactionId)
        {
            try
            {
                return _oMCSDbContext.TransactionHistory.Where(a => a.Transaction.Id == transactionId).OrderByDescending(h => h.Id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public TransactionHistory GetTransactionHistoryById(int transactionHistoryId)
        {
            try
            {
                return _oMCSDbContext.TransactionHistory.Where(a => a.Id == transactionHistoryId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion
    }
}
