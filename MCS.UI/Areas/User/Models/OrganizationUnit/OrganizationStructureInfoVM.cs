using System.Collections.Generic;
using MCS.UI.Areas.User.Models.BarcodeDesigner;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class OrgStructureInfoVM
    {
        public int Key { get; set; }
        public int ManagerId { get; set; }
        public List<OrgUnitUserVM> Users { get; set; }
        public AssignmentPaperVM AssignmentPaper { get; set; }
        public List<BarcodeDesignerVM> BarcodeDesigners { get; set; }
        public CounterVM Counter { get; set; }
        public int ParentId { get; set; }
        public bool IsExternal { get; set; }
        public bool IsActive { get; set; }
        public List<LocalizationVM> Names { get; set; }
        public string Name { get; set; }
        public List<int> LinkUnitsKeys { get; set; }
        public string Number { get; set; }
        public string BarCode { get; set; }
        public bool IsVirtualUnit { get; set; }
        public int TransactionsProcessingPeriod { get; set; }
        public int IdentifierId { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public string StructureAsJson { get; set; }
        public bool HasChilds { get; set; }
        public int? FollowupDepartmentId { get; set; }
    }
}