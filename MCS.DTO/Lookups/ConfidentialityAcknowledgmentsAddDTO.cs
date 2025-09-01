using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class ConfidentialityAcknowledgmentsAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
         

        public bool IsMandatary { get; set; }
    }
}
