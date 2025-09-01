using System.Collections.Generic;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.Assignment
{
    public class AssignmentGroupVM
    {
        public int Id { get; set; }
        public string LocalName { get; set; }
        public List<LocalizationVM> GroupName { get; set; }
        public List<AssignmentGroupDetailVM> GroupDetails { get; set; }
    }
}