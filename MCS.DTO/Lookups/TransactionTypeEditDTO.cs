using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionTypeEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public List<LocalizationDTO> Abbreviation { get; set; }

        //[CustomDisplayName("Admin.TransactionType.PermissionId")]
        [CustomRequired("Admin.TransactionType.PermissionIdRequired")]
        public int PermissionId { get; set; }

        //[CustomDisplayName("Admin.TransactionType.ColorId")]
        [CustomRequired("Admin.TransactionType.ColorIdRequired")]
        public int ColorId { get; set; }

        //[CustomDisplayName("Admin.TransactionType.TransactionSources")]
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
    }
}
