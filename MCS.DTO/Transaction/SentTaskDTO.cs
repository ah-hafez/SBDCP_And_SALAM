using System;
using MCS.Common;

namespace MCS.DTO
{
    public class SentTaskDTO
    {
        public int Id { get; set; }
        public string ToOrgUnit { get; set; }
        public string ToUser { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string DeliveryDateH { get; set; }
        public string TaskDescription { get; set; }
        public string Notes { get; set; }
        public string StatusName { get; set; }
        public TaskStatus Status { get; set; }
        public long TransactionNumber { get; set; }
    }
}
