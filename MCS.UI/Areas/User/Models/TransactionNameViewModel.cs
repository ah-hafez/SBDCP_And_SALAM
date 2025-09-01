using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models
{
    public class TransactionNameViewModel
    {
        public TransactionNameViewModel()
        {
            TransactionNameAddVM = new TransactionNameVM();
            TransactionNameEditVM = new TransactionNameVM();
        }
        public TransactionNameVM TransactionNameAddVM { get; set; }
        public TransactionNameVM TransactionNameEditVM { get; set; }
    }
}