using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionOpenVM
    {
        public TransactionCategory TransactionCategory { set; get; }

        [CustomDisplayName("User.Transaction.Open.TransactionNumber")]
        [CustomRequired("User.Transaction.Open.TransactionNumberRequired")]
        public int TransactionNumber { set; get; }

        [CustomDisplayName("User.Transaction.Open.Year")]
        [CustomRequired("User.Transaction.Open.YearRequired")]
        public int Year { set; get; }

        [CustomDisplayName("User.Transaction.Open.Source")]
        [CustomRequired("User.Transaction.Open.SourceRequired")]
        public int TransactionTypeId { set; get; }

        [CustomDisplayName("User.Transaction.Open.OrgUnit")]
        [CustomRequired("User.Transaction.Open.OrgUnitRequired")]
        public int OrgUnitId { set; get; }
    }
}