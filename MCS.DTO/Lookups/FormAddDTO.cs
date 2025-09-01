using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class FormAddDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        public IList<int> DepartmentIds { get; set; }

        public DocumentDTO FormContentDTO { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
    }
}
