using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models
{
    public class StatisticallyDashboardViewModel
    {
        [CustomDisplayName("User.Report.Statistically.OrgUnit")]
        [CustomRequired("User.Report.Statistically.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
    }
}
