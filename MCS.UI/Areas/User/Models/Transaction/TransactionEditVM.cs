using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionEditVM
    {
        public int Id { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        [CustomDisplayName("User.Transaction.DeliveryNumber")]
        [CustomRequired("User.Transaction.DeliveryNumberRequired")]
        [CustomStringLength("Global.Localization.Text", 30, 0)]
        public string DeliveryNumber { get; set; }
    }
}