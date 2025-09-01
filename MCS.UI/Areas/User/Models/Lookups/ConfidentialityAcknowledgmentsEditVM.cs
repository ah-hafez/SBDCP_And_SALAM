using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class ConfidentialityAcknowledgmentsEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.AttachmentType.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }



        [CustomDisplayName("Admin.ConfidentialityAcknowledgments.IsMandatary")]
        public bool IsMandatary { get; set; }
    }
}