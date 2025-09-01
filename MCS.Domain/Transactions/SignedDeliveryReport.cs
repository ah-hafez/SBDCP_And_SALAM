using System;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class SignedDeliveryReport : EntityBase
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int? DocumentId { get; set; }
        public virtual DocumentInfo Document { get; set; }
        public int? TransactionDeliveryReportId { get; set; }
        public virtual TransactionDeliveryReport TransactionDeliveryReport { get; set; }
    }
}
