using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class SpecificLevelVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public string LocalName { get; set; }
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}