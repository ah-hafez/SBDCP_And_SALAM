using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class PriorityAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.Priority.TransactionCategories")]
        [EnsureOneElement("Admin.Priority.TransactionCategories", "IsSelected")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.Priority.HasDate")]
        public bool HasDate { get; set; }
    }
}