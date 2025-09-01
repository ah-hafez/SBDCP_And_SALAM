using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class FormEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.Form.DepartmentId")]
        public IList<int> DepartmentIds { get; set; }

        [CustomDisplayName("Admin.Form.FormId")]
        public DocumentVM FormContentVM { get; set; }

        [CustomDisplayName("Admin.Form.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}