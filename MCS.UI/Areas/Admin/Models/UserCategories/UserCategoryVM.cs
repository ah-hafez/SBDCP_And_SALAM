using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.UserCategories
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