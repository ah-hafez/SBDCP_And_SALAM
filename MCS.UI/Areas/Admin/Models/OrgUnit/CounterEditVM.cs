using System.Collections.Generic;

namespace MCS.UI.Areas.Admin.Models.OrgUnit
{
    public class CounterEditVM
    {
        public int Id { get; set; }
        public bool IsGeneral { get; set; }
        public int Year { get; set; }
        public int OwnerEntityId { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public List<CounterDetailVM> CounterDetails { get; set; }
    }
}