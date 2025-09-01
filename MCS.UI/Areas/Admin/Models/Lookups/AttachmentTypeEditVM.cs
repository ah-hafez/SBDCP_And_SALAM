using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class AttachmentTypeEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.AttachmentType.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.AttachmentType.PrintBarcode")]
        public bool PrintBarcode { get; set; }

        [CustomDisplayName("Admin.AttachmentType.Archivable")]
        public bool Archivable { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
    }
}