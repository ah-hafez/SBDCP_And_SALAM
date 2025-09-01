using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models.User
{
    public class UserViewModel
    {
        public AddUserProfileVM UserProfileAddVM { get; set; }
        public EditUserProfileVM UserProfileEditVM { get; set; }
        public UserProfileVM UserProfileVM { get; set; }

        public UserViewModel()
        {
            UserProfileAddVM = new AddUserProfileVM();
            UserProfileEditVM = new EditUserProfileVM();
            UserProfileVM = new UserProfileVM();
        }
    }
}