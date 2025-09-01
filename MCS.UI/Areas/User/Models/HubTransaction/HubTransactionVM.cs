using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.HubTransaction
{
    public class HubTransactionVM
    {
        public int Id { get; set; }
        public int TransactionNumber { get; set; }
        public int OrgUnitId { get; set; }
        public int PriorityLevelId { get; set; }
        public int ConfidentialityLevelId { get; set; }
        public int DestinationId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime HijriRecordDate { get; set; }
        public string Remarks { get; set; }
        public string Subject { get; set; }
        //public virtual DocumentInfo MainDocument { get; set; }
        public Guid RQUID { get; set; }
    }
}