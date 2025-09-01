using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class LetterTypeAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }

        
        public List<LetterListTypeDTO> List { get; set; }

        public bool IsPopularization { get; set; }
    }
}
