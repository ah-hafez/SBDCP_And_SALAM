using System;

namespace MCS.DTO
{
    public class AuditDetailDTO
    {
        public int Id { get; set; }
        public AuditDTO Audit { get; set; }
        public string PropertyName { get; set; }
        public string PropertyOldValue { get; set; }
        public string PropertyNewValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
