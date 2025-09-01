using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class LinkAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.TransactionLink.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}