using System.Collections.Generic;
using MCS.UI.Areas.Admin.Models.BarcodeDesigner;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class OrgStructureInfoVM
    {
        public int Key { get; set; }
        public int ManagerId { get; set; }
        public List<OrgUnitUserVM> Users { get; set; }
        public List<UserProfileVM> userProfiles { get; set; }
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

        public string Lineage { get; set; }
        public int? ExternalId { get; set; }
        public int? IoDepartment { get; set; }
        public int? FollowUpDepartment { get; set; }
        public bool IsExecutive { get; set; }
        public bool IsGeneralIoDepartment { get; set; }
        public bool ReceiveElcOutBoundWithAcknowled { get; set; }
        public bool SendSpecialCopy { get; set; }
    }
}