using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class TransactionEntityDetailsRepository : BaseRepository<TransactionEntityDetails>, ITransactionEntityDetailsRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public TransactionEntityDetailsRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Method

        public void AddTransactionEntityDetails(TransactionEntityDetails transactionEntityDetails)
        {
            TransactionEntityDetails oldTransactionEntityDetails = FindBy(t => t.TransactionId == transactionEntityDetails.TransactionId && t.EntityId == transactionEntityDetails.EntityId);
            if (oldTransactionEntityDetails == null)
            {
               Add(transactionEntityDetails);
            }
        }
        #endregion
    }
}
