using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class FollowUpLookUpAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }


        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
    }
}
