using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.UserPreferences
{
    public class DistributionListDetailsVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        public int DistributionListId { get; set; }
        public int UserId { get; set; }
        public int OrgUnitId { get; set; }
        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
    }
}