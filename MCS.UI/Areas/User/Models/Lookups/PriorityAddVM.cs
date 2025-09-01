using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class PriorityAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.Priority.TransactionSources")]
        [EnsureOneElement("Admin.Priority.TransactionSources", "IsSelected")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.Priority.HasDate")]
        public bool HasDate { get; set; }
    }
}