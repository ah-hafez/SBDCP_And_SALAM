namespace MCS.UI.Areas.User.Models.Search.TransactionCertificate
{
    public class TransactionCertificateLinkVM
    {
        public int LinkTypeId { get; set; }

        public string LinkTypeName { get; set; }

        public TransactionCertificateVM Transaction { get; set; }

        public long TransactionNumber { get; set; }

        public int Year { get; set; }

        public int OrgUnitId { get; set; }
    }
}