using System.Collections.Generic;

namespace MCS.DTO
{
    public class ActionDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public int TypeId { get; set; }
        public string LocalName { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public bool IsAsCopy { get; set; }
        public int? SortNo { get; set; }
    }
}
