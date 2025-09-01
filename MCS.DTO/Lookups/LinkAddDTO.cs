using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class LinkAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
    }
}
