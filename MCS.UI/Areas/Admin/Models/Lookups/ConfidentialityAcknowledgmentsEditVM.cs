using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class ConfidentialityAcknowledgmentsEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.AttachmentType.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
         

        [CustomDisplayName("Admin.ConfidentialityAcknowledgments.IsMandatary")]
        public bool IsMandatary { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}