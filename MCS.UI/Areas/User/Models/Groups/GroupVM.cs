using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Groups
{
    public class GroupVM
    {
        public int Id { get; set; }
        public LookupVM Name { get; set; }
        public string LocalName { get; set; }
    }
}