using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class ReceivedTaskVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int? LevelLimitation { get; set; }
        public string FromOrgUnit { get; set; }
        public string FromUser { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateH { get; set; }
        public string TaskDescription { get; set; }
        public string Subject { get; set; }
        public List<DocumentVM> Attachment { get; set; }
        public List<TaskReminderVM> Reminders { get; set; }
        public string Notes { get; set; }
        public int TransactionId { get; set; }
        public long TransactionNumber { get; set; }
        public int TransactionCategoryId { get; set; }
        public bool IsExclusive { get; set; }
        public ReceivedTasksType ReceivedTaskType { get; set; }
        public string DelayedDaysCount { get; set; }

    }
}