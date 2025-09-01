using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Report
{
    public class TransactionBasicResultVM
    {
        public int? Number { get; set; }
        public DateTime CreateOn { get; set; }
        public string DateFrom { get; set; }
        public DateTime DateFromG { get; set; }
        public string DateTo { get; set; }
        public DateTime DateToG { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedEntity { get; set; }
        public string ExternalParty { get; set; }
        public string TransactionType { get; set; }
        public int TotalCount { get; set; }
        public string TenantName { get; set; }

        public int? FromTo { get; set; }
    }
}