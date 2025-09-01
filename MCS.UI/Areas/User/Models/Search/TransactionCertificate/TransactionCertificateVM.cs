using System;
using System.Collections.Generic;
using MCS.Common;

namespace MCS.UI.Areas.User.Models.Search.TransactionCertificate
{
    public class TransactionCertificateVM
    {
        public int Id { get; set; }

        public TransactionCategory TransactionCategory { get; set; }

        public string Source { get; set; }

        public long Number { get; set; }

        public string HijriDate { get; set; }

        public DateTime Date { get; set; }

        public List<TransactionCertificateLinkVM> Links { get; set; }
    }
}