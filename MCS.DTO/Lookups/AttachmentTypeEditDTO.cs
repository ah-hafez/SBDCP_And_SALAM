using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AttachmentTypeEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }

        public bool PrintBarcode { get; set; }

        public bool Archivable { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}
