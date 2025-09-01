using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class ConfidentialityAcknowledgmentsAddVM
    {
        public List<LocalizationVM> Description { get; set; }

       [CustomDisplayNameAttribute("Admin.AttachmentType.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }



        [CustomDisplayName("Admin.ConfidentialityAcknowledgments.IsMandatary")]
        public bool IsMandatary { get; set; }
    }
}