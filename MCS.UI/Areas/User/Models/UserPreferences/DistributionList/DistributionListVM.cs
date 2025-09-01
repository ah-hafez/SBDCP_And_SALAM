using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.UserPreferences
{
    public class DistributionListVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        [CustomDisplayName("User.UserPreferences.DistributionList.User")]
        [CustomRequired("User.UserDelegation.DirectToRequired")]
        public int? UserId { get; set; }
        [CustomDisplayName("User.UserPreferences.DistributionList.OrgUnit")]
        [CustomRequired("User.UserDelegation.OrgUnitRequired")]
        public int? OrgUnitId { get; set; }
        public int LocalizationIdentifierId { get; set; }
        public string UserName { get; set; }
        public string OrgUnitName { get; set; }
        public List<LocalizationVM> Name { get; set; }
        public IList<DistributionListDetailsVM> DistributionListDetails { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModefiedOn { get; set; }
        public int? ModefiedBy { get; set; }
        public AjaxGrid<DistributionListVM> DistributionListGrid { get; set; } = (AjaxGrid<DistributionListVM>)new AjaxGridFactory().CreateAjaxGrid(new List<DistributionListVM>(), 1, 0, false);
        public AjaxGrid<DistributionListDetailsVM> DistributionListDetailsGrid { get; set; } = (AjaxGrid<DistributionListDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(new List<DistributionListDetailsVM>(), 1, 0, false);

    }
}