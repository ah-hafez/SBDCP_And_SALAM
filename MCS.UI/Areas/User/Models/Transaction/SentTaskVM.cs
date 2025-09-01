using System;
using System.ComponentModel;
using MCS.Common;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class SentTaskVM
    {
        public int? Id { get; set; } = null;
        public string ToOrgUnit { get; set; }
        [CustomDisplayName("User.Task.TaskAdd.ToOrgUnit")]
        [CustomRequired("User.Task.TaskAdd.ToOrgUnitRequired")]
        public int? ToOrgUnitId { get; set; }
        public string ToUser { get; set; }

        public int ToUserId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateH { get; set; }
        public string TaskDescription { get; set; }
        public string Notes { get; set; }
        public string StatusName { get; set; }
        public TaskStatus Status { get; set; }
        public long TransactionNumber { get; set; }
    }
}