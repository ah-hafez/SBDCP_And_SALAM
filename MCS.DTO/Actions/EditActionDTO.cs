using System.Collections.Generic;

namespace MCS.DTO
{
    public class EditActionDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public int TypeId { get; set; }
        public bool IsAsCopy { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public int AssignmentTypeId { get; set; }
        public LookupDTO AssignmentType { get; set; }

    }
}
