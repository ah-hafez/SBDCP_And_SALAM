using System;
using System.Collections.Generic;
using MCS.Common;

namespace MCS.DTO
{
    public class TransactionCertificateDTO
    {
        public int Id { get; set; }

        public TransactionCategory TransactionCategory { get; set; }

        public string Source { get; set; }

        public long Number { get; set; }

        public string HijriDate { get; set; }

        public DateTime Date { get; set; }

        public List<TransactionCertificateLinkDTO> Links { get; set; }
    }
}
