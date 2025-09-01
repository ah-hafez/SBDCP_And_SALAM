using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Models.Groups
{
    public class GroupVM
    {
        public int Id { get; set; }
        public LookupVM Name { get; set; }
        public string LocalName { get; set; }
        public bool IsActive { get; set; }
        public bool IsSelected { get; set; }
    }
}