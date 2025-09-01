using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class AttachmentTypeAddVM
    {
        public List<LocalizationVM> Description { get; set; }

       [CustomDisplayNameAttribute("Admin.AttachmentType.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        
        public bool PrintBarcode { get; set; }

        [CustomDisplayNameAttribute("Admin.AttachmentType.Archivable")]
        public bool Archivable { get; set; }
    }
}