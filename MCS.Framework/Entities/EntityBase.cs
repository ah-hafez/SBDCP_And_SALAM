using System;

namespace MCS.Framework.Entities
{
    public enum DataRowStatus
    {
        UnChanged = 0,
        Modified = 1,
        Added = 2,
        Deleted = 3,
        ChildModified = 4,
        Current = 5
    }
    public class EntityBase
    {
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}
