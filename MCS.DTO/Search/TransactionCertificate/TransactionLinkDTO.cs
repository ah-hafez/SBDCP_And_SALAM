namespace MCS.DTO
{
    public class TransactionCertificateLinkDTO
    {
        public int LinkTypeId { get; set; } 

        public string LinkTypeName { get; set; }

        public TransactionCertificateDTO Transaction { get; set; }

        public long TransactionNumber { get; set; }   

        public int Year { get; set; } 

        public int OrgUnitId { get; set; }  
    }
}