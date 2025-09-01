using System.Collections.Generic;

namespace MCS.DTO
{
    public class LinkEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }


        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}
