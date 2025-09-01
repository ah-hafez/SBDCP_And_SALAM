using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class PriorityAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        [Required]
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }

        public bool HasDate { get; set; }
    }
}
