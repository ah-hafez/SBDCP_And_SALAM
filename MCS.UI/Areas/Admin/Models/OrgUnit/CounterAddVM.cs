using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class CounterAddVM
    {
        public int Id { get; set; }
        public bool IsGeneral { get; set; }
        public List<CounterDetailVM> CounterDetails { get; set; }
        public int OwnerEntityId { get; set; }
    }
}