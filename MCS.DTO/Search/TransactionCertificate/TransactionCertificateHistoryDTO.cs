using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionCertificateHistoryDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<TransactionCertificateHistoryDetailDTO> CertificateHistoryDetails { get; set; }
    }
}
