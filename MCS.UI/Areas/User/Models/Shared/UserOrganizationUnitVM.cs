using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Shared
{
    public class UserOrgUnitVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocalizationVM> LoclizationName { get; set; }
        public bool IsSelected { get; set; }

        public int ManagerId { get; set; }
    }
}