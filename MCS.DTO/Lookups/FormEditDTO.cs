using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class FormEditDTO
    {
        public int Id { get; set; }
        public List<LocalizationDTO> Description { get; set; }

        public IList<int> DepartmentIds { get; set; }

        public DocumentDTO FormContentDTO { get; set; }

        public List<TransactionCategoryDTO> TransactionCategories { get; set; }
        public IList<int> OrgUnitIds { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public int Status { get; set; }
    }
}
