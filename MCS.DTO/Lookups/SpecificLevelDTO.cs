using System.Collections.Generic;

namespace MCS.DTO
{
    public class SpecificLevelDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public string LocalName { get; set; }
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
    }
}
