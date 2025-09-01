using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.UserCategories
{
    public class UserCategoryTraysViewModel
    {
        public List<UserCategoryTrayVM> UserCategoryTrays { get; set; }

        public UserCategoryTraysViewModel()
        {
            UserCategoryTrays = new List<UserCategoryTrayVM>();
        }
    }
}