using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models.UserCategories
{
    public class EditUserCategoryTrayVM
    {
        public int UserCategoryId { get; set; }

        public List<int> TraysIds { get; set; }
    }
}