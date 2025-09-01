using System.Collections.Generic;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.Permission
{
    public class PermissionEditVM
    {
        public int Id { get; set; }
        public List<LookupLocalizationVM> Names { get; set; }
    }
}