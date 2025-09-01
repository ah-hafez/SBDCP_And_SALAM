using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class TransactionTypeAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        public List<LocalizationDTO> Abbreviation { get; set; }
        
        [Required]
        public int PermissionId { get; set; }

        [Required]
        public int ColorId { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
    }
}
