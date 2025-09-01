using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Permission
{
    public class PermissionVM
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsSelected { get; set; }
        public string Text { get; set; }
        public List<LookupLocalizationVM> Names { get; set; }
        public int groupId { get; set; }
        public bool IsUserDefined { get; set; }
    }
}