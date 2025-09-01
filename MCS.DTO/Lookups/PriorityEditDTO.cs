using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class PriorityEditDTO
    {
        public int Id { get; set;}
        public List<LocalizationDTO> Description { get; set; }
        
        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
        
        public bool HasDate { get; set; }
        public int LateForEntity { get; set; }
        public int LateForUser { get; set; }
        public bool HasPriorityExceptions { get; set; }
        public int Sort { get; set; }
        public int ProcessPeriod { get; set; }
        public List<PriorityExceptionDTO> PriorityExceptions { get; set; }

    }
}
