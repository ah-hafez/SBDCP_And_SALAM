using MCS.Common.TransactionContext;
using MCS.Domain;
using System;
using System.Linq;

namespace MCS.DataAccess
{
    public class AuditingRepository : BaseRepository<ApiAuditLog>, IAuditingRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public AuditingRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public void AddApiLog(ApiAuditLog apiAuditLog)
        {
            try
            {
                _oMCSDbContext.ApiAuditLogs.Add(apiAuditLog);

                _oMCSDbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public int GetLogBySignature(string signature)
        {
            try
            {
                var logsCount = _oMCSDbContext.ApiAuditLogs.Where(x => x.Signature == signature).Count();

                return logsCount;


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
