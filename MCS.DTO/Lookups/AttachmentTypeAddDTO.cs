using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class AttachmentTypeAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }

        public bool PrintBarcode { get; set; }

        public bool Archivable { get; set; }
    }
}
