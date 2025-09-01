using System.Collections.Generic;
using MCS.UI.Areas.Admin.Models.Tray;

namespace MCS.UI.Areas.Admin.Models.UserCategories
{
    public class UserCategoryTrayVM
    {
        public int Id { get; set; }

        public List<LocalizationVM> Categories { get; set; }

        public string CategoryText { get; set; }

        public List<TrayVM> Trays { get; set; }
    }
}