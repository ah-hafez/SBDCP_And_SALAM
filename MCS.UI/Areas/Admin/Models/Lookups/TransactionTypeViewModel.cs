namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class TransactionTypeViewModel
    {
        public TransactionTypeVM TransactionType { get; set; }
        public TransactionTypeAddVM AddTransactionType { get; set; }
        public TransactionTypeEditVM EditTransactionType { get; set; }

        public TransactionTypeViewModel()
        {
            TransactionType = new TransactionTypeVM();
            AddTransactionType = new TransactionTypeAddVM();
            EditTransactionType = new TransactionTypeEditVM();
        }
    }
}