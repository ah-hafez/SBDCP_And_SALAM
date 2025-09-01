using System;

namespace MCS.UI
{
    public class AuditDetailVM
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public string PropertyOldValue { get; set; }
        public string PropertyNewValue { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
