using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class SpecificLevelAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }

        
        public List<SpecificLevelListDTO> List { get; set; }
    }
}
