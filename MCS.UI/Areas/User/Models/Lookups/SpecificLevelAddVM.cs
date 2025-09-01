using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class SpecificLevelAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.SpecificLevel.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.SpecificLevel.List")]
        public List<LetterListTypeVM> List { get; set; }
        
    }
}