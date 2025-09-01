namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionHistoryVM
    {
        public int Id { get; set; }

        public string FromOrgUnitName { get; set; }

        public string FromUserName { get; set; }

        public string ToOrgUnitName { get; set; }

        public string ToUserName { get; set; }

        public string DateH { get; set; }

        public string Time { get; set; }

        public string ActionName { get; set; }

        public string Remarks { get; set; }
    }
}