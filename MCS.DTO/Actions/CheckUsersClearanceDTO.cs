namespace MCS.DTO
{
    public class UsersClearanceDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int InboundTransactionsCount { get; set; }
        public int OutboundTransactionsCount { get; set; }
        public int SavedTransactionsCount { get; set; }
    }
}
