using System.Collections.Generic;
using System.Linq;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.Admin.Controllers;
using MCS.UI.Areas.Admin.Models.BarcodeDesigner;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class OrgStructureInfoEditVM
    {
        [CustomDisplayName("Admin.OrgUnitInfo.IsRoot")]
        public bool IsRoot { get; set; }
        public int Key { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.Manager")]
        public int? ManagerId { get; set; }
        public IList<OrgUnitUserVM> Users { get; set; }
        public CounterVM Counter { get; set; } = new CounterVM();

        [CustomDisplayName("Admin.OrgUnitInfo.Parent")]
        [CustomRequired("Admin.OrgUnitInfo.ParentRequired")]
        public int ParentId { get; set; }
        public bool IsExternal { get; set; }
        public bool IsActive { get; set; }
        public List<LocalizationVM> Names { get; set; }
        public string Name
        {
            get
            {
                if (Names != null)
                {
                    LocalizationVM localizationVM =
                        Names.Where(l => l.CultureId == 1).SingleOrDefault();

                    if (localizationVM != null)
                    {
                        return localizationVM.Text;
                    }
                }

                return string.Empty;
            }
        }

        [CustomDisplayName("Admin.OrgUnitInfo.Number")]
        [CustomRequired("Admin.OrgUnitInfo.NumberRequired")]
        public string Number { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.BarCode")]
        public string BarCode { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.IsVirtualUnit")]
        public bool IsVirtualUnit { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.TransactionsProcessingPeriod")]
        [CustomRequired("Admin.OrgUnitInfo.TransactionsProcessingPeriodRequired")]
        public int TransactionsProcessingPeriod { get; set; }

        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }

        public BarcodeDesignerVM objBarcodeDesignerVM { get; set; } = new BarcodeDesignerVM();

        public List<OrgStructureInfoVM> objOrgStructureInfoVMList { get; set; }

        public string viewMode { get; set; }

        public int? ExternalId { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.IoDepartment")]
        public int? IoDepartment { get; set; }
        [CustomDisplayName("Admin.OrgUnitInfo.FollowUpDepartment")]
        public int? FollowUpDepartment { get; set; }
        [CustomDisplayName("Admin.OrgUnitInfo.IsExecutive")]
        public bool IsExecutive { get; set; }
        [CustomDisplayName("Admin.OrgUnitInfo.GeneralIo")]
        public bool IsGeneralIoDepartment { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.ReceiveElcOutBoundWithAcknowled")]
        public bool ReceiveElcOutBoundWithAcknowled { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.SendSpecialCopy")]
        public bool SendSpecialCopy { get; set; }
        public string Lineage { get; set; }

    }
}