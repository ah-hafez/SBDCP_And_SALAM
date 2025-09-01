using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ITransactionEntityDetailsRepository : IRepository<TransactionEntityDetails>
    {
        void AddTransactionEntityDetails(TransactionEntityDetails transactionEntityDetails);
    }
}
