using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class PriorityEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.Priority.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.Priority.HasDate")]
        public bool HasDate { get; set; }
        public int LateForEntity { get; set; }
        public int LateForUser { get; set; }
        public bool HasPriorityExceptions { get; set; }
        public int Sort { get; set; }
        public int ProcessPeriod { get; set; }
        public List<PriorityExceptionVM> PriorityExceptions { get; set; } = (AjaxGrid<PriorityExceptionVM>)new AjaxGridFactory().CreateAjaxGrid(new List<PriorityExceptionVM>(), 1, 0, false);
    }
}