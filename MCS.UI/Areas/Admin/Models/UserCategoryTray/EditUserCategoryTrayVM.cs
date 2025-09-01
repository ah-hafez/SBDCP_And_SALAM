using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.UserCategories
{
    public class EditUserCategoryTrayVM
    {
        public int UserCategoryId { get; set; }

        public List<int> TraysIds { get; set; }
    }
}