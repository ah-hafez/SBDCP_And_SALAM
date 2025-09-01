namespace MCS.UI.Areas.User.Models.Report
{
    public class DeliveryReportTransactionInfoVM
    {
        public long TransactionNumber { get; set; }
        public int AttachmentCount { get; set; }
        public string FromEntity { get; set; }
        public string ToEntity { get; set; }
    }
}