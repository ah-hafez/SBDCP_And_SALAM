using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class TransactionTypeEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public List<LocalizationVM> Abbreviation { get; set; }

        [CustomDisplayName("Admin.TransactionType.PermissionId")]
        [CustomRequired("Admin.TransactionType.PermissionIdRequired")]
        public int PermissionId { get; set; }

        [CustomDisplayName("Admin.TransactionType.ColorId")]
        [CustomRequired("Admin.TransactionType.ColorIdRequired")]
        public int ColorId { get; set; }

        [CustomDisplayName("Admin.TransactionType.TransactionSources")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}