using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class LetterTypeAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.LetterType.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.LetterType.List")]
        public List<LetterListTypeVM> List { get; set; }

        public bool IsPopularization { get; set; }
    }
}