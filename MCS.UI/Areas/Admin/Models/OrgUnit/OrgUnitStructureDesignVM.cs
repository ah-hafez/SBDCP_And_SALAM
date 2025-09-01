using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class OrgUnitStructureDesignVM
    {
        public List<OrgStructureInfoVM> OrgUnits { get; set; }
        public string Settings { get; set; }
    }
}