using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class ConfidentialityAcknowledgmentsEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
         

        public bool IsMandatary { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}
