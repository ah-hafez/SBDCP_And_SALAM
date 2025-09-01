namespace MCS.DTO
{
    public class DeliveryReportTransactionDTO
    {
        public long TransactionNumber { get; set; }
        public int AttachmentCount { get; set; }
        public string AttachmentTotal { get; set; }
        public string DateH { get; set; }
        public string FromEntity { get; set; }
        public string ToEntity { get; set; }
        public string Receiver { get; set; }
        public string DateAndSignature = string.Empty;
        public string TransactionType { get; set; }
        public int TransactionTypeId { get; set; }
        public bool IsCopy { get; set; }
        public string ExternalParty { get; set; }
        public string Subject { get; set; }
        public string DateTime { get; set; }
    }
}
