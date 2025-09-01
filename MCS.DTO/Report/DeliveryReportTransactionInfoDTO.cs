namespace MCS.DTO
{
    public class DeliveryReportTransactionInfoDTO
    {
        public long TransactionNumber { get; set; }
        public int AttachmentCount { get; set; }
        public string FromEntity { get; set; }
        public string ExternalParty { get; set; }
        public string ToEntity { get; set; }
        public string DateH { get; set; }
        public string AttachmentTotal { get; set; }
        public string TransactionCategory { get; set; }
        public int TransactionCategoryId { get; set; }
        public string Receiver { get; set; }
        public bool IsCopy { get; set; }
        public string Subject { get; set; }
    }
}
