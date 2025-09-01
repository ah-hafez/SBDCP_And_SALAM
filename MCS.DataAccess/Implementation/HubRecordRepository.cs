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
    public class HubRecordRepository : BaseRepository<HubRecord>, IHubRecordRepository
    {
        public HubRecordRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }
    }
}
