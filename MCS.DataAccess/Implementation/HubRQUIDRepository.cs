using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Common.TransactionContext;

namespace MCS.DataAccess
{
    public class HubRQUIDRepository : BaseRepository<HubRQUID>, IHubRQUIDRepository
    {
        public HubRQUIDRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        public HubRQUID GetByRQUID(string rQUID)
        {
            try
            {
                HubRQUID hubRQUID = _oMCSDbContext.HubRQUIDs.Where(r => r.RQUID == rQUID).FirstOrDefault();
                return hubRQUID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long GetByTransactionNumberByRQUID(string rQUID)
        {
            try
            {
                long transactionNumber = _oMCSDbContext.HubRQUIDs.Where(r => r.RQUID == rQUID).FirstOrDefault().TransactionNumber;
                return transactionNumber;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
