using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class PrivateFollowupVM
    {
        public bool IsValid()
        {
            return ProccessId.HasValue && PeriodId.HasValue && EntityId.HasValue;
        }
        public int? ProccessId { get; set; }
        public int? PeriodId { get; set; }
        [CustomDisplayName("User.Transaction.FollowUp.Entity")]
        public int? EntityId { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateTo { get; set; }
        public string DateToH { get; set; }
        public bool IsImportant { get; set; }

    }
}