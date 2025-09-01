using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class PublicFollowupVM
    {
        public bool IsValid()
        {
            return ProccessId.HasValue && PeriodId.HasValue;
        }
        public int? ProccessId { get; set; }
        public int? PeriodId { get; set; }
        public DateTime? DateTo { get; set; }
        public string DateToH { get; set; }
        public bool IsImportant { get; set; }

    }
}