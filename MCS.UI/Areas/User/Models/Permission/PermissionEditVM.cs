using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Permission
{
    public class PermissionEditVM
    {
        public int Id { get; set; }
        public List<LookupLocalizationVM> Names { get; set; }
    }
}