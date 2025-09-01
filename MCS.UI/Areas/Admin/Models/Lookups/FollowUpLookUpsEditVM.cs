using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class FollowUpLookUpsEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.TransactionLink.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public LookupVM Status { get; set; }
    }
}