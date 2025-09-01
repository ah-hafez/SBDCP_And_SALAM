using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Action : EntityBase
    {
        public bool IsAsCopy { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public virtual LocalizationIdentifier LocalizationIdentifier { get; set; }
        public string LocalName { get; set; }
        public virtual Lookup Type { get; set; }
        public int? SortNo { get; set; }
    }
}
