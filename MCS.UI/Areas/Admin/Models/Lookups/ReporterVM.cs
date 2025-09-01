using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models
{
    public class ReporterVM
    {
        public int Id { get; set; }
        [CustomRequired("Required")]
        public List<LocalizationVM> Names { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        [CustomRequired("Admin.Lookups.Correspondent.OrgUnitRequired")]
        [CustomDisplayName("Admin.Lookups.Correspondent.OrgUnit")]
        public int OrgUnitId { get; set; }
        public string OrgUnitName { get; set; }
    }
}