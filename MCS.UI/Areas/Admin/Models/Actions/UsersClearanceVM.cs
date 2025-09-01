namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class UsersClearanceVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int InboundTransactionsCount { get; set; }
        public int OutboundTransactionsCount { get; set; }
        public int SavedTransactionsCount { get; set; }
    }
}