using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.Search.TransactionCertificate
{
    public class TransactionCertificateHistoryVM
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public List<TransactionCertificateHistoryDetailVM> CertificateHistoryDetails { get; set; }
    }
}