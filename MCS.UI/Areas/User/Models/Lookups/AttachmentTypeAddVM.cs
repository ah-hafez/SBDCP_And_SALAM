using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class AttachmentTypeAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.AttachmentType.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.AttachmentType.PrintBarcode")]
        public bool PrintBarcode { get; set; }

        [CustomDisplayName("Admin.AttachmentType.Archivable")]
        public bool Archivable { get; set; }
    }
}