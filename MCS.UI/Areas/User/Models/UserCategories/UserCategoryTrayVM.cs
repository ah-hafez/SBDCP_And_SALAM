using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Tray;

namespace MCS.UI.Areas.User.Models.UserCategories
{
    public class UserCategoryTrayVM
    {
        public int Id { get; set; }

        public List<LocalizationVM> Categories { get; set; }

        public string CategoryText { get; set; }

        public List<TrayVM> Trays { get; set; }
    }
}