using MCS.Common;

namespace MCS.DTO
{
    public class TransactionEditDTO
    {
        public int Id { get; set; }
        public TransactionCategory TransactionCategory { get; set; }
        public string DeliveryNumber { get; set; }
    }
}
