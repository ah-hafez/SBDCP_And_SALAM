using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.UserCategories
{
    public class UserCategoryVM
    {
        public int Id { get; set; }

        public string CategoryText { get; set; }

        public List<LocalizationVM> Categories { get; set; }

        public string PermissionText { get; set; }

        public bool IsSelected { get; set; }
    }
}