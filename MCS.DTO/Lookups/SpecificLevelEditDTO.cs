using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class SpecificLevelEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }

        
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }

        
        public List<SpecificLevelListDTO> List { get; set; }
    }
}
