using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class SubjectClassificationAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.SubjectClassification.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}