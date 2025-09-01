using MCS.Common.TransactionContext;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.DataAccess
{
    class HubRelatedPersonRepository : BaseRepository<HubRelatedPerson>, IHubRelatedPersonRepository
    {
        public HubRelatedPersonRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }
    }
}
