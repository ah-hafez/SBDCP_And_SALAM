using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Controllers;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class CounterVM
    {
        public int CounterId { get; set; }
        public int OwnerEntityId { get; set; }
        [CustomDisplayName("Admin.Counter.Year")]
        public int Year { get; set; }
        public int CounterDetailId { get; set; }
        [CustomDisplayName("Admin.Counter.InitialValue")]
        [CustomRequired("Admin.Counter.InitialValue")]
        public int InitialValue { get; set; }
        public int Count { get; set; }
        public bool IsRoot { get; set; }
        public bool IsGeneral { get; set; }
        public bool JoinToGeneralCounter { get; set; }
        public ViewMode ViewMode { get; set; }
        public List<LocalizationVM> Description { get; set; }
        [CustomDisplayName("Admin.AttachmentType.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
        public List<CounterDetailVM> CounterDetails { get; set; }
    }
}