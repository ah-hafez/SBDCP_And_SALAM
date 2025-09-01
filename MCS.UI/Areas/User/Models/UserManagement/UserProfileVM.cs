using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.UserManagement
{
    public class UserProfileVM
    {
        public int Id { get; set; }

        public string UserName { get; set; }
        public string RoleName { get; set; }

        public string LocalName { get; set; }

        public string Category { get; set; }

        public string Email { get; set; }

        public List<LocalizationVM> Names { get; set; }

        public bool IsSelected { get; set; }

        public bool IsActive { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public int? UserImageId { get; set; }
    }
}