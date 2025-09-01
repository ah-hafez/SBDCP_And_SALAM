using MCS.Common.TransactionContext;

namespace MCS.DataAccess
{
    public class BaseWrappers
    {
        private readonly IAmbienTTransactionContextLocator _ambienTTransactionContextLocator;
        public readonly MCSDbContext _oMCSDbContext;

        public BaseWrappers(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
        {
            _ambienTTransactionContextLocator = ambienTTransactionContextLocator;
            _oMCSDbContext = _ambienTTransactionContextLocator.Get<MCSDbContext>();
        }
    }
}
