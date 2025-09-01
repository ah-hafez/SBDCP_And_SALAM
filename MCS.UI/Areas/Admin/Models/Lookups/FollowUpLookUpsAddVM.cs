using MCS.Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class FollowUpLookUpsAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.TransactionLink.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
    }
}