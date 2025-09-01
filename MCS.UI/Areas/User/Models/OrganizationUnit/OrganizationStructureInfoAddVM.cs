using System.Collections.Generic;
using System.Linq;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Lookups;

namespace MCS.UI.Areas.User.Models.OrgUnit
{
    public class OrgStructureInfoAddVM
    {
        public int Key { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.Manager")]
        [CustomRequired("Admin.OrgUnitInfo.ManagerRequired")]
        public int ManagerId { get; set; }
        public IList<OrgUnitUserVM> Users { get; set; }
        public CounterVM Counter { get; set; }

        [CustomDisplayName("Admin.OrgUnitInfo.IsRoot")]
        public bool IsRoot { get; set; }

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
    }
}