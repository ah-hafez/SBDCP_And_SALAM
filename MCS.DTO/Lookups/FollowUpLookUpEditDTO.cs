using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class FollowUpLookUpEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }


        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}
