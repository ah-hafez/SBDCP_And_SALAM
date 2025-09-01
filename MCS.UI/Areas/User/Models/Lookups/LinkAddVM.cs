using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class LinkAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.TransactionLink.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}