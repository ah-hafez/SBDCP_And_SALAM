using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class ReceivedTaskDTO
    {
        public int Id { get; set; }
        public int? LevelLimitation { get; set; }
        public string FromOrgUnit { get; set; }
        public string FromUser { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateH { get; set; }
        public string TaskDescription { get; set; }
        public string Subject { get; set; }
        public List<DocumentDTO> Attachment { get; set; }
        public List<TaskReminderDTO>  Reminders { get; set; }
        public string Notes { get; set; }
        public int TransactionId { get; set; }
        public int TransactionCategoryId { get; set; }
        public long TransactionNumber { get; set; }
        public bool IsExclusive { get; set; }
       
    }
}
